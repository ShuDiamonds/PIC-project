/************************************************************
　　MPLAB-C18による入出力ポートの制御テストプログラム
　　スイッチの入力をポートDから行い、スイッチの状態に
　　従って、指定された発光ダイオードを点滅させる。
　　　　RD0のスイッチがOFFの時LED１
　　　　RD1のスイッチがOFFの時LED2
　　　　RD2のスイッチがOFFの時LED3
　　　　全スイッチがONの時全LED
*************************************************************/
#include <p18f2550.h>    // PIC18C452のヘッダ・ファイル
#include <delays.h>
#include <usart.h>

//***** コンフィギュレーションの設定config


		#pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
		#pragma config CPUDIV   = OSC1_PLL2   
		#pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
		#pragma config FOSC     = HSPLL_HS
		#pragma config FCMEN    = OFF
		#pragma config IESO     = OFF
		#pragma config PWRT     = OFF
		#pragma config BOR      = OFF		//ADD
		#pragma config BORV     = 3
	//	#pragma config VREGEN   = ON      //USB Voltage Regulator
		#pragma config VREGEN   = OFF      //USB Voltage Regulator
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
		
//---Wait---//
//48Mhz駆動の時
#define WAIT_MS Delay1KTCYx(12)// Wait 1ms
#define WAIT_US Delay10TCYx(12)	// Wait 1us

//=======wait[ms]======//
void wait_ms(unsigned long int t) {
	while(t--) {
		WAIT_MS;
	}
}
//***** メイン関数
void main(void)                     // メイン関数
{
	
	//初期化
		TRISA=0x00;                        // ポートAをすべて出力ピンにする
		TRISB=0x0F;                        // ポートBをすべて出力ピンにする
		TRISC=0xFF;                        // ポートCをすべて出力ピンにする
	
	
		PORTB = 0x00;
	wait_ms(1000);
	PORTB = 0xFF;
	wait_ms(1000);
	PORTB = 0x00;
	wait_ms(1000);
	PORTB = 0xFF;
	wait_ms(1000);
	PORTB = 0x00;
	
	
	while(1)
	{
		if(PORTBbits.RB0)				//スイッチがOFFのとき
		{
		PORTB = 0xFF;					//LED OFF
		}
		else{							//スイッチがONのとき
		PORTB = 0x00;					//LED ON
		}
		
	}
}

