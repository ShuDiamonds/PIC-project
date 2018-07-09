/*********************************************************
  MPLAB-C18テストプログラム　No8
  割込みのテストNo1
  　優先順位を使った複数割込みのテスト
  　　高レベル：タイマ０　　低レベル：タイマ１
  機能
  　　アイドルループ：０．５秒間隔でLED1を点滅
  　　タイマ０　　　：０．１秒間隔でLED2を点滅
  　　タイマ１　　　：１秒間隔でLED3を点滅
********************************************************/　
#include <p18f26j50.h>            // PIC18f2550のヘッダ・ファイル
#include <timers.h>             // タイマ関数のヘッダ・ファイル
#include <delays.h>
#include <adc.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include "HardwareProfile.h"

//***** コンフィギュレーションの設定config

	#pragma config WDTEN = OFF          //WDT disabled (enabled by SWDTEN bit)
	#pragma config PLLDIV = 2           //Divide by 2 (8 MHz intl osc input)
	#pragma config STVREN = ON          //stack overflow/underflow reset enabled
	#pragma config XINST = OFF          //Extended instruction set disabled
	#pragma config CPUDIV = OSC1        //No CPU system clock divide
	#pragma config CP0 = OFF            //Program memory is not code-protected
	#pragma config OSC = INTOSCPLL      //Internal oscillator, PLL enenabled
	#pragma config T1DIG = OFF          //S-Osc may not be selected, unless T1OSCEN = 1
	#pragma config LPT1OSC = OFF        //high power Timer1 mode
	#pragma config FCMEN = OFF          //Fail-Safe Clock Monitor disabled
	#pragma config IESO = OFF           //Two-Speed Start-up disabled
	#pragma config WDTPS = 32768        //1:32768
	#pragma config DSWDTOSC = INTOSCREF //DSWDT uses INTOSC/INTRC as clock
	#pragma config RTCOSC = T1OSCREF    //RTCC uses T1OSC/T1CKI as clock
	#pragma config DSBOREN = OFF        //Zero-Power BOR disabled in Deep Sleep
	#pragma config DSWDTEN = OFF        //Disabled
	#pragma config DSWDTPS = 8192       //1:8,192 (8.5 seconds)
	#pragma config IOL1WAY = OFF        //IOLOCK bit can be set and cleared
	#pragma config MSSP7B_EN = MSK7     //7 Bit address masking
	#pragma config WPFP = PAGE_1        //Write Protect Program Flash Page 0
	#pragma config WPEND = PAGE_0       //Start protection at page 0
	#pragma config WPCFG = OFF          //Write/Erase last page protect Disabled
	#pragma config WPDIS = OFF          //WPFP[5:0], WPEND, and WPCFG bits ignored 


/********関数プロトタイプ************/
void wait_ms(int t);
void wait_us(int t);
void Timer0_isr(void);
void Timer1_isr(void);
void Timer2_isr(void);
void Timer3_isr(void);
void RB_PORT_isr(void);
void RB0_isr(void);
void RB1_isr(void);
void RB2_isr(void);
void AD_isr(void);
void UART_TX_isr(void);
void UART_RC_isr(void);
//********************************************************
//***** 変数、定数の定義
unsigned char cnt=4;            // cnt,cnt1はLED更新周期用カウンタ
unsigned char cnt1=5;
unsigned char cnt2=6;
unsigned char cnt3=7;
unsigned char cnt4=20;
unsigned char cnt5=111;
unsigned char cnt6=90;
char temp_Buf[20] = {0};
char Send_Buf[20] = {0};
//****** メイン関数
void main(void)                 // メイン関数
{
	//ローカル変数定義
	long int data = 0;
	char Message1[10]="\rStart!!\n";
	char Message2[7]="FUKUDA";
	float DATA=0;
	
	//クロック初期化
		//48Mhzの設定
	OSCTUNEbits.PLLEN = 1;        // PLLを起動
	wait_ms(2);
	//ポート初期化
	TRISA=0x0F;
	TRISB=0x0F;
    TRISC=0;                    // ポートCをすべて出力ピンにする
	PORTC = 0xFF;
	//----UART初期化-------//
	Open1USART(USART_TX_INT_ON & USART_RX_INT_ON & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 64); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600
	
	//----Timer初期化------//
	OpenTimer0(TIMER_INT_ON & T0_8BIT & T0_SOURCE_INT & T0_PS_1_256);
								// タイマ0の設定, 8ビットモード, 割込み使用 
								//内部クロック、1:256プリスケーラ
/*	OpenTimer1(TIMER_INT_ON & T1_8BIT_RW & T1_SOURCE_INT & T1_PS_1_8 & 
	        T1_OSC1EN_OFF);     //タイマ１の設定,8ビットモード、割込み使用
	                            //内部クロック、1:8プリスケーラ
*/
	OpenTimer2( 
		TIMER_INT_ON &
		T2_PS_1_4 &
		T2_POST_1_8 );
	
/*	OpenTimer3(TIMER_INT_ON & T3_SOURCE_INT &  T3_SYNC_EXT_ON & 
            T3_SOURCE_CCP);	
*/
	//-----AD初期化--------//
	OpenADC(ADC_FOSC_64 &           //AD変換用クロック　　システムクロックの1/64　0.05μsec×64＝3.2μsec　>=　1.6μsec　→　OK
				ADC_RIGHT_JUST &        //変換結果の保存方法　左詰め　
				ADC_8_TAD,              //AD変換のアクイジションタイム選択　3.2μsec（=1Tad）×8Tad=25.6μsec　＞＝　12.8μsec　→　OK
				ADC_CH0 &                       //AD変換するのチャンネル選択（PIC18Fは同時に複数のAD変換はできない）
				ADC_INT_ON &           //AD変換での割込み使用の有無
				ADC_VREFPLUS_VDD &      //Vref+の設定　　　ＰＩＣの電源電圧と同じ：ADC_VREFPLUS_VDD 　or　外部（ＡＮ３）の電圧：ADC_VREFPLUS_EXT
				ADC_VREFMINUS_VSS,      //Vref-の設定　　　ＰＩＣの0Ｖ：ADC_VREFMINUS_VSS    or　外部（AN2)の電圧：ADC_VREFMINUS_EXT
				0b1110  //ポートのアナログ・デジタル選択　（ADCON1の下位４ビットを記載）　　AN0のみアナログポートを選択、他はデジタルポートを選択
				//例 　アナログポートが　AN0のみ → 0b1110　　、AN0 & AN1　→　0b1011、 AN0 & AN1 & AN2 →1100　他　詳細データシート参照
		);
	
	

//***********割り込み設定***********************
//***** 優先順位割込み使用宣言
    RCONbits.IPEN=1;
//***** 低レベル使用周辺の定義
    IPR1bits.TMR1IP=0;
//************Timer0優先順位設定*****
	INTCON2bits.TMR0IP = 1;	//高位割り込みに設定
//************Timer1優先順位設定*****
	IPR1bits.TMR1IP = 1;	//高位割り込みに設定
//************Timer2優先順位設定*****
	IPR1bits.TMR2IP = 1;	//高位割り込みに設定
//************Timer3優先順位設定*****
	IPR2bits.TMR3IP = 1;	//高位割り込みに設定

//************RB0割り込み設定********
	//RB0/INT は割り込みに優先順位はなく高位のに割り込みがかかる
	INTCONbits.INT0IE=0;		//RB0/INT 外部割込みイネーブルビット(4bit目)を許可
	INTCON2bits.INTEDG0=0;			//RB0/INT ピンの立ち下がりエッジにより割り込み(6bit目)	
	
//************RB1割り込み設定*********
	INTCON3bits.INT1IP = 0;		//RB1/INT 高位割り込みにする
	INTCON3bits.INT1IE = 0;		//RB1/INT 外部割込みイネーブルビット(4bit目)を許可
	INTCON2bits.INTEDG1 = 0;			//RB1/INT ピンの立ち下がりエッジにより割り込み(6bit目)

//************RB2割り込み設定*********
	INTCON3bits.INT2IP = 0;			//RB2/INT 高位割り込みにする
	INTCON3bits.INT2IE = 0;		//R2/INT 外部割込みイネーブルビット(4bit目)を許可
	INTCON2bits.INTEDG2 = 0;	//RB2/INT ピンの立ち下がりエッジにより割り込み(6bit目)

//************RBPORT割り込み設定*********
	INTCON2bits.RBIP = 0;		//RBPORT/INT 高位割り込みにする
	INTCONbits.RBIE = 0;		//RB2/INT 外部割込みイネーブルビット(4bit目)を許可
	
//************AD変換割り込み*************
	IPR1bits.ADIP = 0;		//AD変換割り込みを低位割り込みに設定
	PIE1bits.ADIE = 1;		//AD変換割り込みを許可
//************UART送信割り込み*************
	IPR1bits.TXIP = 0;		//AD変換割り込みを低位割り込みに設定
	PIE1bits.TXIE= 1;		//AD変換割り込みを許可
//************UART受信割り込み*************
	IPR1bits.RCIP = 0;		//AD変換割り込みを低位割り込みに設定
	PIE1bits.RCIE= 0;		//AD変換割り込みを許可
	
	//***** 割込み許可
    INTCONbits.GIEH=1;          // 高レベル許可
    INTCONbits.GIEL=1;          // 低レベル許可
	
//	putsUSART(Message1);
	//***** メインループ（アイドルループ）
    while(1)    
    {
    	SetChanADC(ADC_CH0);	//Select Channel 0
		Delay100TCYx(50);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		data = ReadADC();	//Get A/D data
    	fprintf(_H_USART,"\rData=%ld\n",data);	
    	
    	puts1USART(Message1);
    	
		PIN_LED_0 = 1;
		Delay10KTCYx(100);
		PIN_LED_0 = 0;
		Delay10KTCYx(100);
    
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
{	_asm
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
	if(PIR1bits.TMR2IF)	Timer2_isr();       //タイマ2割り込み？
	if(PIR2bits.TMR3IF)	Timer3_isr();		// タイマ3割り込み？
	if(INTCONbits.INT0IF)	RB0_isr();		 // 外部割込み０割り込み？
	if(INTCON3bits.INT1IF)	RB1_isr();		 // 外部割こみ１割り込み？
	if(INTCON3bits.INT2IF)	RB2_isr();		 // 外部割込み２割り込み？
	if(INTCONbits.RBIF)	RB_PORT_isr();		 // 外部割込みPORT変化割り込み？
	if(PIR1bits.ADIF)	AD_isr();			 //AD割り込み？
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART送信割り込み？
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART受信割り込み？
	//INTCONbits.GIEH=1;          // 高レベル許可
	
}                                   
//***** 低レベル割込み処理関数
void Low_isr(void)                     // 割り込み関数
{
	INTCONbits.GIEL=0;          // 低レベル不許可
	if(INTCONbits.T0IF)	Timer0_isr();       //タイマ0割り込み？
	if(PIR1bits.TMR1IF)	Timer1_isr();		//タイマ１割り込み？
	if(PIR1bits.TMR2IF)	Timer2_isr();       //タイマ2割り込み？
	if(PIR2bits.TMR3IF)	Timer3_isr();		// タイマ3割り込み？
	if(INTCONbits.INT0IF)	RB0_isr();		 // 外部割込み０割り込み？
	if(INTCON3bits.INT1IF)	RB1_isr();		 // 外部割こみ１割り込み？
	if(INTCON3bits.INT2IF)	RB2_isr();		 // 外部割込み２割り込み？
	if(INTCONbits.RBIF)	RB_PORT_isr();		 // 外部割込みPORT変化割り込み？
	if(PIR1bits.ADIF)	AD_isr();			 //AD割り込み？
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART送信割り込み？
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART受信割り込み？
	
}



/************************************************************/

//---Wait---//
//48Mhz駆動の時
#define WAIT_MS Delay1KTCYx(12)// Wait 1ms
#define WAIT_US Delay10TCYx(12)	// Wait 1us

//=======wait[ms]======//
void wait_ms(int t) {
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[us]======//
void wait_us(int t) {
	while(t--) {
		WAIT_US;
	}
}
/***********割り込み関数実行部*********/

//**********Timer0割り込み*************
void Timer0_isr(void)
{
	INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
	if(--cnt==0)
	{               // cntを-1して結果が0？
		cnt=4;                  // cntにLEDの更新周期を書き戻す
		if(PIN_LED_1)
			PIN_LED_1=0;    //LED2を0.1秒間隔で点滅
		else
			PIN_LED_1=1;
	}
}
//**********Timer1割り込み*************
void Timer1_isr(void)
{
		PIR1bits.TMR1IF=0;          // タイマ１割り込みフラグを0にする
		if(--cnt1==0)
		{              // cnt1を-1して結果が0？
			cnt1=5;                 // cnt1にLEDの更新周期を書き戻す
			if(PIN_LED_2)
			PIN_LED_2=0;    //LED3を１秒間隔で点滅
			else
			PIN_LED_2=1;
		}
}
//**********Timer2割り込み*************
void Timer2_isr(void)
{
		PIR1bits.TMR2IF=0;          // タイマ2割り込みフラグを0にする
		if(--cnt2==0)
		{              // cnt1を-1して結果が0？
			cnt2 = 113;
			if(PIN_LED_3)
			PIN_LED_3=0;    //LED3を１秒間隔で点滅
			else
			PIN_LED_3=1;
		}
}
//**********Timer3割り込み*************
void Timer3_isr(void)
{
		PIR2bits.TMR3IF=0;          // タイマ3割り込みフラグを0にする
		if(--cnt3==0)
		{              // cnt1を-1して結果が0？
			cnt3=7;
			if(PIN_LED_4)
			PIN_LED_4=0;    //LED3を１秒間隔で点滅
			else
			PIN_LED_4=1;
		}
}
//**********RB0割り込み*************
void RB0_isr(void)
{
	INTCONbits.INT0IF = 0;		//割り込みフラグクリア
	PIN_LED_0=1;    //LED3を１秒間隔で点滅
	wait_ms(1000);
	//PIN_LED_1=1;
}
//**********RB1割り込み*************
void RB1_isr(void)
{
	INTCON3bits.INT1IF = 0;		//割り込みフラグクリア
	PIN_LED_1=1;    //LED3を１秒間隔で点滅
	wait_ms(1000);
	//PIN_LED_2=1;
}
//**********RB2割り込み*************
void RB2_isr(void)
{
	INTCON3bits.INT2IF = 0;		//割り込みフラグクリア
	PIN_LED_2=1;    //LED3を１秒間隔で点滅
	wait_ms(1000);
	//PIN_LED_2=1;
}
//**********RB PORT変化割り込み*************
void RB_PORT_isr(void)
{
	int PORT_data = 0;
	PORT_data = PORTB;
	INTCONbits.RBIF = 0;		//割り込みフラグクリア
	
}
//**********AD割り込み*************
void AD_isr(void)
{
	PIR1bits.ADIF = 0;		//割り込みフラグクリア
	if(--cnt4==0)
		{              // cnt1を-1して結果が0？
			cnt4=255;
			if(PIN_LED_5)
			PIN_LED_5=0;    //LED3を１秒間隔で点滅
			else
			PIN_LED_5=1;
		}
}
//**********UART送信割り込み*************
void UART_TX_isr(void)
{
	int k =0;
	putc1USART(Send_Buf);
		for(k=0;k>=20;k++)
		{
			Send_Buf[k] = 0;
		}
		PIR1bits.TXIF = 0;		//割り込みフラグクリア
	if(--cnt5==0)
		{              // cnt1を-1して結果が0？
			cnt5=255;
			if(PIN_LED_6)
			PIN_LED_6=0;    //LED3を１秒間隔で点滅
			else
			PIN_LED_6=1;
		}
}
//**********UART受信割り込み*************
void UART_RC_isr(void)
{
	int i =0;
	i++;
	temp_Buf[i] = getc1USART();
	PIR1bits.RCIF = 0;		//割り込みフラグクリア
	if(--cnt6==0)
		{              // cnt1を-1して結果が0？
			cnt6=255;
			if(PIN_LED_7)
			PIN_LED_7=0;    //LED3を１秒間隔で点滅
			else
			PIN_LED_7=1;
		}
}

