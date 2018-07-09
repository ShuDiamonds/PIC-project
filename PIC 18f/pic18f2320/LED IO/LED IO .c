/************************************************************
　　MPLAB-C18による入出力ポートの制御テストプログラム
　　スイッチの入力をポートDから行い、スイッチの状態に
　　従って、指定された発光ダイオードを点滅させる。
　　　　RD0のスイッチがOFFの時LED１
　　　　RD1のスイッチがOFFの時LED2
　　　　RD2のスイッチがOFFの時LED3
　　　　全スイッチがONの時全LED
*************************************************************/
#include <p18f2320.h>    // PIC18C452のヘッダ・ファイル
#include <delays.h>
#include <usart.h>

//***** コンフィギュレーションの設定config


#pragma	config	OSC = HSPLL
#pragma	config	FSCM = OFF
#pragma	config	IESO = OFF
#pragma	config	PWRT = OFF
#pragma	config	BOR = OFF
#pragma	config	BORV = 20
#pragma	config	WDT = OFF
#pragma	config	WDTPS = 1
#pragma	config	MCLRE = ON
#pragma	config	PBAD = DIG
//	#pragma	config	PBAD = ANA
//	#pragma	config	CCP2MX = C1
#pragma	config	STVR = ON
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
		

//***** メイン関数
void main(void)                     // メイン関数
{
	//ローカル変数定義
	
	
	//UART初期化
	
	
	//初期化
		TRISA=0x00;                        // ポートAをすべて出力ピンにする
		TRISB=0;                        // ポートBをすべて出力ピンにする
		TRISC=0xFF;                        // ポートCをすべて出力ピンにする
	
	while(1)
	{
		if(PORTCbits.RC3)				//スイッチがOFFのとき
		{
		PORTA = 0xFF;					//LED OFF
		}
		else{							//スイッチがONのとき
		PORTA = 0x00;					//LED ON
		}
		
	}
}

