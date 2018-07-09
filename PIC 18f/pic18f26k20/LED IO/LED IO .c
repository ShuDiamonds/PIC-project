/************************************************************
	MPLAB-C18による入出力ポートの制御テストプログラム
	pic18f26k20を64MHzで動かし、LEDを1秒間隔でちかちかさせる
*************************************************************/
#include <p18f26k20.h>	 // PIC18F26k20のヘッダ・ファイル
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>

//***** コンフィギュレーションの設定config

#pragma	config	FOSC = INTIO67
#pragma	config	FCMEN = OFF
#pragma	config	IESO = OFF
#pragma	config	PWRT = OFF
#pragma	config	BOREN = OFF
#pragma	config	BORV = 18
#pragma	config	WDTEN = OFF
#pragma	config	WDTPS = 1
#pragma	config	MCLRE = ON
#pragma	config	PBADEN = OFF		//デジタルに設定
//	#pragma	config	PBAD = ANA
//	#pragma	config	CCP2MX = C1
#pragma	config	STVREN = ON
#pragma	config	LVP = ON
#pragma	config	DEBUG = OFF
#pragma	config	CP0 = OFF
#pragma	config	CP1 = OFF
#pragma	config	CP2 = OFF
#pragma	config	CP3 = OFF
#pragma	config	CPB = OFF
#pragma	config	CPD = OFF
#pragma	config	WRT0 = OFF
#pragma	config	WRT1 = OFF
#pragma	config	WRT2 = OFF
#pragma	config	WRT3 = OFF
#pragma	config	WRTB = OFF
#pragma	config	WRTC = OFF
#pragma	config	WRTD = OFF
#pragma	config	EBTR0 = OFF
#pragma	config	EBTR1 = OFF
#pragma	config	EBTR2 = OFF
#pragma	config	EBTR3 = OFF
#pragma	config	EBTRB = OFF	
		
//---Wait---//
//64Mhz駆動の時
#define WAIT_MS Delay1KTCYx(16)// Wait 1ms
#define WAIT_US Delay10TCYx(16)	// Wait 1us

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
	
	//内部クロック初期化
	OSCCONbits.IRCF0 = 1;		//16MHzに設定
	OSCCONbits.IRCF1 = 1;
	OSCCONbits.IRCF2 = 1;

	OSCTUNEbits.PLLEN = 1;        // PLLを起動する(16*4=64Mhzに設定)
	
	//UART初期化
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 103); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600の時で64にする。また48Mhzで9600の時で77にする 64で103にする
	
	
	//wait_ms(1000);
	
	//初期化
		TRISA=0x00;                        // ポートAをすべて出力ピンにする
		TRISB=0;                        // ポートBをすべて出力ピンにする
		TRISC=0xFF;                        // ポートCをすべて出力ピンにする
	
	while(1)
	{
		
		PORTA = 0xFF;					//LED OFF
		PORTB = 0xFF;
		wait_ms(1000);
		PORTA = 0x00;					//LED ON
		PORTB = 0x00;
		wait_ms(1000);
		
	}
}

