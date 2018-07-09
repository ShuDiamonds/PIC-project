#include <p18f2550.h>
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <adc.h>


//***** コンフィギュレーションの設定config


		#pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
		#pragma config CPUDIV   = OSC1_PLL2   
		#pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
		#pragma config FOSC     = HSPLL_HS
		#pragma config FCMEN    = OFF
		#pragma config IESO     = OFF
		#pragma config PWRT     = OFF
		#pragma config BOR      = OFF
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

//=======wait[us]======//
void wait_us(unsigned int t) {
	while(t--) {
		WAIT_US;
	}
}
//***** メイン関数
void main(void)                     // メイン関数
{
	//ローカル変数定義
	char Message1[10]="\rStart!!\n";
	char Message2[7]="FUKUDA";
	char  data=0;
	float DATA=0;
	
	//UART初期化
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 77); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600の時で64にする。また48Mhzで9600の時で77にする
	
		

	
	//初期化
		TRISA=0x0F;                        // ポートAを半分出力ピンにする
		TRISB=0;                        // ポートBをすべて出力ピンにする
		TRISC=0b10111111;                        // ポートCをすべて出力ピンにする
	putsUSART(Message1);
	printf("\rHello world\n");		//「Hello」と出力
	
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
		
		
		PORTB = 0x00;
		Delay100TCYx(5);		//20usec delay
		//if(DataRdyUSART());
		//{
			
			while(!DataRdyUSART());
			data = ReadUSART();
			WriteUSART(data);
			PORTB = 0xFF;
		//}
		
		
		
	}
	//UART終了
	//CloseUSART( );
	

}
