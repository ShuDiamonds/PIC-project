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
#include <p18f2550.h>            // PIC18f2550のヘッダ・ファイル
#include <timers.h>             // タイマ関数のヘッダ・ファイル
#include <delays.h>
#include "HardwareProfile.h"
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
//      #pragma config CCP2MX   = ON
        #pragma config STVREN   = ON
        #pragma config LVP      = OFF
//      #pragma config ICPRT    = OFF       // Dedicated In-Circuit Debug/Programming
        #pragma config XINST    = OFF       // Extended Instruction Set
        #pragma config CP0      = OFF
        #pragma config CP1      = OFF
//      #pragma config CP2      = OFF
//      #pragma config CP3      = OFF
        #pragma config CPB      = OFF
//      #pragma config CPD      = OFF
        #pragma config WRT0     = OFF
        #pragma config WRT1     = OFF
//      #pragma config WRT2     = OFF
//      #pragma config WRT3     = OFF
        #pragma config WRTB     = OFF       // Boot Block Write Protection
        #pragma config WRTC     = OFF
//      #pragma config WRTD     = OFF
        #pragma config EBTR0    = OFF
        #pragma config EBTR1    = OFF
//      #pragma config EBTR2    = OFF
//      #pragma config EBTR3    = OFF
        #pragma config EBTRB    = OFF
        
/********関数プロトタイプ************/
void Timer0_isr(void);
void Timer1_isr(void);
void RB_PORT_isr(void);
void RB0_isr(void);
void RB1_isr(void);
void RB2_isr(void);
//********************************************************
//***** 変数、定数の定義
unsigned char cnt=4;            // cnt,cnt1はLED更新周期用カウンタ
unsigned char cnt1=5;
//****** メイン関数
void main(void)                 // メイン関数
{
	TRISB=0xFF;
    TRISC=0;                    // ポートCをすべて出力ピンにする
	PORTC = 0xFF;
	
    OpenTimer0(TIMER_INT_ON & T0_8BIT & T0_SOURCE_INT & T0_PS_1_256);
                                // タイマ0の設定, 8ビットモード, 割込み使用 
                                //内部クロック、1:256プリスケーラ
    OpenTimer1(TIMER_INT_ON & T1_8BIT_RW & T1_SOURCE_INT & T1_PS_1_8 & 
            T1_OSC1EN_OFF);     //タイマ１の設定,8ビットモード、割込み使用
                                //内部クロック、1:8プリスケーラ
	
	
	
//***** 優先順位割込み使用宣言
    RCONbits.IPEN=1;
//***** 低レベル使用周辺の定義
    IPR1bits.TMR1IP=0;
//***********割り込み設定***********************
	
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
{   _asm
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
//**********RB0割り込み*************
void RB0_isr(void)
{
	INTCONbits.INT0IF = 0;		//割り込みフラグクリア
	PIN_LED_0=0;    //LED3を１秒間隔で点滅
	//wait_ms(1000);
	//PIN_LED_1=1;
}
//**********RB1割り込み*************
void RB1_isr(void)
{
	INTCON3bits.INT1IF = 0;		//割り込みフラグクリア
	PIN_LED_1=0;    //LED3を１秒間隔で点滅
	//wait_ms(1000);
	//PIN_LED_2=1;
}
//**********RB2割り込み*************
void RB2_isr(void)
{
	INTCON3bits.INT2IF = 0;		//割り込みフラグクリア
	PIN_LED_2=0;    //LED3を１秒間隔で点滅
	//wait_ms(1000);
	//PIN_LED_2=1;
}
//**********RB PORT変化割り込み*************
void RB_PORT_isr(void)
{
	int PORT_data = 0;
	PORT_data = PORTB;
	INTCONbits.RBIF = 0;		//割り込みフラグクリア
	
}


