/*********************************************************
拍手でスイッチングプログラム
pic18f2550 のCPUクロックは48Mhz
timer0 は10ms で割り込みがかかる
RB0 割り込みでマイクも音声信号を読み取る
********************************************************/　
#include <p18f2550.h>            // PIC18f2550のヘッダ・ファイル
#include <timers.h>             // タイマ関数のヘッダ・ファイル
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <adc.h>
//#include "HardwareProfile.h"
//#include "Interruptlib.h"

//***** コンフィギュレーションの設定config

		#pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
		#pragma config CPUDIV   = OSC1_PLL2   
		#pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
		#pragma config FOSC     = HSPLL_HS
		#pragma config FCMEN    = OFF
		#pragma config IESO     = OFF
		#pragma config PWRT     = OFF
		#pragma config BOR      = ON
		#pragma config BORV     = 3
		#pragma config VREGEN   = ON      //USB Voltage Regulator
		#pragma config WDT      = OFF
		#pragma config WDTPS    = 32768
		#pragma config MCLRE    = ON
		#pragma config LPT1OSC  = OFF
		#pragma config PBADEN   = OFF
//		#pragma config CCP2MX   = ON
		#pragma config STVREN   = ON
		#pragma config LVP      = OFF
//		#pragma config ICPRT    = OFF       // Dedicated In-Circuit Debug/Programming
		#pragma config XINST    = OFF       // Extended Instruction Set
		#pragma config CP0      = OFF
		#pragma config CP1      = OFF
//		#pragma config CP2      = OFF
//		#pragma config CP3      = OFF
		#pragma config CPB      = OFF
//		#pragma config CPD      = OFF
		#pragma config WRT0     = OFF
		#pragma config WRT1     = OFF
//		#pragma config WRT2     = OFF
//		#pragma config WRT3     = OFF
		#pragma config WRTB     = OFF       // Boot Block Write Protection
		#pragma config WRTC     = OFF
//		#pragma config WRTD     = OFF
		#pragma config EBTR0    = OFF
		#pragma config EBTR1    = OFF
//		#pragma config EBTR2    = OFF
//		#pragma config EBTR3    = OFF
		#pragma config EBTRB    = OFF
/********ピンマクロ******************/
#define		PIN_LED_0	PORTCbits.RC0
#define		PIN_LED_1	PORTCbits.RC1
#define		PIN_LED_2	PORTCbits.RC2
#define		PIN_LED_3	PORTCbits.RC4
#define		PIN_LED_4	PORTCbits.RC5
#define		PIN_LED_5	PORTBbits.RB4
#define		PIN_LED_6	PORTBbits.RB5
#define		PIN_LED_7	PORTBbits.RB6
#define		PIN_LED_8	PORTBbits.RB7
/********関数プロトタイプ************/
void Timer0_isr(void);
void Timer1_isr(void);
void RB_PORT_isr(void);
void RB0_isr(void);
void RB1_isr(void);
void RB2_isr(void);
void wait_us(unsigned int t);
void wait_ms(unsigned int t);
void wait_s(unsigned long int t);

//********************************************************
//***** グローバル変数、定数の定義
unsigned long int cnt=0;            // cnt,cnt1はLED更新周期用カウンタ
unsigned char cnt1=0;
unsigned int signaldata = 0;
unsigned int data_i= 1;
unsigned int data_z= 0;
unsigned int i= 0;
unsigned int VOICE_data[121] = {0};
//****** メイン関数
void main(void)					// メイン関数
{
	TRISA=0xFF;
	TRISB=0x0F;
	TRISC=0;					// ポートCをすべて出力ピンにする
	PORTC = 0xFF;
	
	PIN_LED_5 = 0;
	PIN_LED_6 = 0;
	PIN_LED_7 = 0;
	PIN_LED_8 = 0;
	wait_s(1);
	PIN_LED_5 = 1;
	PIN_LED_6 = 1;
	PIN_LED_7 = 1;
	PIN_LED_8 = 1;
	wait_s(1);
	PIN_LED_5 = 0;
	PIN_LED_6 = 0;
	PIN_LED_7 = 0;
	PIN_LED_8 = 0;
	wait_s(1);
	PIN_LED_5 = 1;
	PIN_LED_6 = 1;
	PIN_LED_7 = 1;
	PIN_LED_8 = 1;
	wait_s(1);
	PIN_LED_5 = 0;
	PIN_LED_6 = 0;
	PIN_LED_7 = 0;
	PIN_LED_8 = 0;
	/*
	//**********Timer0初期化****************
    OpenTimer0(TIMER_INT_ON & T0_8BIT & T0_SOURCE_INT & T0_PS_1_256);
								// タイマ0の設定, 8ビットモード, 割込み使用 
								//内部クロック、1:256プリスケーラ
	
	INTCONbits.T0IF=0;			// タイマ0割り込みフラグを0にする
	//カウント値のロード
		WriteTimer0(65067);			//10msに設定
		WriteTimer0();			//10msに設定
	*/
	
	
	/*
	
	//**********Timer1初期化****************
	OpenTimer1(TIMER_INT_ON & T1_16BIT_RW & T1_SOURCE_INT & T1_PS_1_8 & 
			T1_OSC1EN_OFF);     //タイマ１の設定,16ビットモード、割込み使用
								//内部クロック、1:256プリスケーラ
	PIR1bits.TMR1IF=0;			// タイマ１割り込みフラグを0にする
	
	*/
	
	
//***********UART初期化***********************
	
	//UART初期化
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 77); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600の時で64にする。また48Mhzで9600の時で77にする
	
	/*
	
	
//***********AD変換初期化**********************
	OpenADC(ADC_FOSC_64 &           //AD変換用クロック　　システムクロックの1/64　0.05μsec×64＝3.2μsec　>=　1.6μsec　→　OK
				ADC_RIGHT_JUST &        //変換結果の保存方法　左詰め　
				ADC_8_TAD,              //AD変換のアクイジションタイム選択　3.2μsec（=1Tad）×8Tad=25.6μsec　＞＝　12.8μsec　→　OK
				ADC_CH0 &                       //AD変換するのチャンネル選択（PIC18Fは同時に複数のAD変換はできない）
				ADC_INT_OFF &           //AD変換での割込み使用の有無
				ADC_VREFPLUS_VDD &      //Vref+の設定　　　ＰＩＣの電源電圧と同じ：ADC_VREFPLUS_VDD 　or　外部（ＡＮ３）の電圧：ADC_VREFPLUS_EXT
				ADC_VREFMINUS_VSS,      //Vref-の設定　　　ＰＩＣの0Ｖ：ADC_VREFMINUS_VSS    or　外部（AN2)の電圧：ADC_VREFMINUS_EXT
				0b1110  //ポートのアナログ・デジタル選択　（ADCON1の下位４ビットを記載）　　AN0のみアナログポートを選択、他はデジタルポートを選択
				//例 　アナログポートが　AN0のみ → 0b1110　　、AN0 & AN1　→　0b1011、 AN0 & AN1 & AN2 →1100　他　詳細データシート参照
		);
	
	
	*/
	
//***** 優先順位割込み使用宣言
    RCONbits.IPEN=1;
//***********割り込み設定***********************
	
//************Timer0割り込み設定********
	INTCONbits.TMR0IE=0;		//Timer0割り込み許可・禁止ビット
	INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
//************Timer1割り込み設定********
	PIE1bits.TMR1IE=0;		//Timer0割り込み許可・禁止ビット
	PIR1bits.TMR1IF=0;          // タイマ１割り込みフラグを0にする
//************RB0割り込み設定********
	//RB0/INT は割り込みに優先順位はなく高位のに割り込みがかかる
	INTCONbits.INT0IE=1;		//RB0/INT 外部割込みイネーブルビット(4bit目)を許可
	INTCON2bits.INTEDG0=0;			//RB0/INT ピンの立ち下がりエッジにより割り込み(6bit目)	
	
//************RB1割り込み設定*********
	INTCON3bits.INT1IP = 0;		//RB1/INT 高位割り込みにする
	INTCON3bits.INT1IE = 1;		//RB1/INT 外部割込みイネーブルビット(4bit目)を許可
	INTCON2bits.INTEDG1 = 0;			//RB1/INT ピンの立ち下がりエッジにより割り込み(6bit目)

//************RB2割り込み設定*********
	INTCON3bits.INT2IP = 0;			//RB2/INT 高位割り込みにする
	INTCON3bits.INT2IE = 1;		//R2/INT 外部割込みイネーブルビット(4bit目)を許可
	INTCON2bits.INTEDG2 = 0;	//RB2/INT ピンの立ち下がりエッジにより割り込み(6bit目)

//************RBPORT割り込み設定*********
	INTCON2bits.RBIP = 0;		//RBPORT/INT 高位割り込みにする
	INTCONbits.RBIE = 1;		//RB2/INT 外部割込みイネーブルビット(4bit目)を許可
	
	
	
	
	//***** 割込み許可
    INTCONbits.GIEH=1;          // 高レベル許可
    INTCONbits.GIEL=1;          // 低レベル許可
	
	
	
	//***** メインループ（アイドルループ）
	while(1)    
	{
		/*
		PIN_LED_0 = 1;
		Delay10KTCYx(100);
		PIN_LED_0 = 0;
		Delay10KTCYx(100);
	*/
	}
}

//****************************************************
//****** 割込みの宣言　優先順位使用
#pragma interrupt High_isr save = PROD
#pragma interruptlow Low_isr save = WREG,BSR,STATUS,PROD

//***** 割込みベクタジャンプ命令セット
#pragma code isrcode = 0x8
void isr_direct(void)
{
	_asm
	goto High_isr
	_endasm
}
#pragma code lowcode = 0x18
void low_direct(void)
{
	_asm
	goto Low_isr
	_endasm
}
//**** 高レベル　割込み処理関数
#pragma code
void High_isr(void)                      // 割り込み関数
{
	INTCONbits.GIEH=0;          // 高レベル不許可
	if(INTCONbits.T0IF)	Timer0_isr();       // タイマ0割り込み？
	if(PIR1bits.TMR1IF)	Timer1_isr();		 // タイマ１割り込み？
	if(INTCONbits.INT0IF)	RB0_isr();		 // 外部割込み０割り込み？
	if(INTCON3bits.INT1IF)	RB1_isr();		 // 外部割こみ１割り込み？
	if(INTCON3bits.INT2IF)	RB2_isr();		 // 外部割込み２割り込み？
	if(INTCONbits.RBIF)	RB_PORT_isr();		 // 外部割込みPORT変化割り込み？
	INTCONbits.GIEH=1;          // 高レベル許可
	
}                                   
//***** 低レベル割込み処理関数
void Low_isr(void)                     // 割り込み関数
{
	INTCONbits.GIEL=0;          // 低レベル不許可
	if(INTCONbits.T0IF)	Timer0_isr();       // タイマ0割り込み？
	if(PIR1bits.TMR1IF)	Timer1_isr();		 // タイマ１割り込み？
	if(INTCONbits.INT0IF)	RB0_isr();		 // 外部割込み０割り込み？
	if(INTCON3bits.INT1IF)	RB1_isr();		 // 外部割こみ１割り込み？
	if(INTCON3bits.INT2IF)	RB2_isr();		 // 外部割込み２割り込み？
	if(INTCONbits.RBIF)	RB_PORT_isr();		 // 外部割込みPORT変化割り込み？
   // INTCONbits.GIEL=1;          // 低レベル許可
	
}




/************************************************************/

//---Wait---//
//48Mhz駆動の時
#define WAIT_MS Delay1KTCYx(12)// Wait 1ms
#define WAIT_US Delay10TCYx(12)	// Wait 1us

//=======wait[ms]======//
void wait_ms(unsigned int t) {
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[s]======//
void wait_s(unsigned long int t) {
	t = t*1000;
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[us]======//
void wait_us(unsigned int t) {
	while(t--) {
		WAIT_US;
	}
}
/***********割り込み関数実行部*********/

//**********Timer0割り込み*************
void Timer0_isr(void)
{
	INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
	//カウント値のロード
	WriteTimer0(65067);			//10msに設定
	cnt++;
	
}
//**********Timer1割り込み*************
void Timer1_isr(void)
{
		PIR1bits.TMR1IF=0;          // タイマ１割り込みフラグを0にする
}
//**********RB0割り込み*************
void RB0_isr(void)
{
	
	INTCONbits.INT0IF = 0;		//割り込みフラグクリア
	
	//90msのdekay
	wait_ms(90);
	
	if(INTCONbits.TMR0IE == 0)				//タイマ割り込みが開始していないか？ 
	{
		
		//**********Timer0初期化****************
	OpenTimer0(TIMER_INT_ON & T0_16BIT & T0_SOURCE_INT & T0_PS_1_256);
								// タイマ0の設定, 16ビットモード, 割込み使用 
								//内部クロック、1:256プリスケーラ
		
		//timerを使い時間の計測開始
		INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
		INTCONbits.TMR0IE=1;		//タイマ0割り込み許可
		//カウント値のロード
		WriteTimer0(65067);			//10msに設定
	}
	else					//カウントを開始していた場合
	{
		if(20<=cnt && cnt <=200)
		{
			INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
			//カウント値のロード
			WriteTimer0(65067);			//10msに設定
			//データを1として受信
			data_z = 1;
			data_z =  data_z<<data_i;
			signaldata = signaldata |data_z;
			data_i++;
		}else if(300<=cnt && cnt <=500)
		{
			INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
			//カウント値のロード
			WriteTimer0(65067);			//10msに設定
			//データを0として受信
			data_z = 0;
			data_z =  data_z<<data_i;
			signaldata = signaldata |data_z;
			data_i++;
		}else if(600<=cnt)				//正常終了
		{
			CloseTimer0();				//タイマー終了
			//信号終了
			INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
			INTCONbits.TMR0IE=0;		//タイマ0割り込み不許可
			//データ実行
			switch(signaldata)
				{
				case	0b00001010:
					PIN_LED_5 = 1;
							break;
				case	0b00011000:
				PIN_LED_6 = 1;
						break;
				case	0b00101110:
				PIN_LED_7 = 1;
						break;
				case	0b01011110:
				PIN_LED_8 = 1;
						break;
				}
			
			//データ初期化
			signaldata = 0;
			data_i = 0;
		}else		//例外
		{
			
			CloseTimer0();				//タイマー終了
			INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
			INTCONbits.TMR0IE=0;		//タイマ0割り込み不許可
			//データ初期化
			signaldata = 0;
			data_i = 0;
		}
		
		//カウントを初期化
		cnt = 0;
	}
	
}
//**********RB1割り込み*************
void RB1_isr(void)
{
	INTCON3bits.INT1IF = 0;		//割り込みフラグクリア
}
//**********RB2割り込み*************
void RB2_isr(void)
{
	INTCON3bits.INT2IF = 0;		//割り込みフラグクリア
}
//**********RB PORT変化割り込み*************
void RB_PORT_isr(void)
{
	//ローカル変数定義
	int PORT_data = 0;
	
	PORT_data = PORTB;			//データ読み込み
	INTCONbits.RBIF = 0;		//割り込みフラグクリア
	
}
/**********timer0のカウント値の設定**************


PIC18F452を使用してタイマー0で1秒毎に割り込みを行わせる処理を書いてみました。 説明はプログラム中に記載していますが、割り込みに関する部分は後日記載しようと思います。 今回は割り込みの優先レベルは考えていません。
　
セラロック10MHzを使用して内部でPLL4倍、よって40MHz動作です。
　
1秒÷（40MHz/4）=1÷10MHz=1÷10,000,000=0.0000001=100ns
　
上の計算は1命令サイクルの時間です。なので1秒間に何回これを行えばいいかを考えます。
　
1秒÷100ns=1÷0.0000001=10,000,000回
　
今回は動作クロックが計算しやすい値で楽でした。しかし、1000万回という値はタイマーに設定できません。 そこでプリスケーラを使用します。タイマー0には256が設定できるので計算してみます。
　
10,000,000÷256=39,062回　（正確には39,062.5）
　
16ビットなので65,535まで可能なので今回は大丈夫なようです。なのでプリスケーラを256に設定してカウントは39,062です。 しかし、タイマーのカウンタはアップカウンタです。なのでMAX値から必要なカウント数を引いてやる必要があります。
　
65,536-39,062=0x10000-0x9896=0x676A
　
上記のとおり、タイマーの設定は0x676Aに決まりました。



参考URL　http://amahime.main.jp/c18prog/main.php?name=c18prog
*/





