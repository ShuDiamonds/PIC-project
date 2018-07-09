#include <p18f2320.h>    // PIC18C452のヘッダ・ファイル
#include <delays.h>
#include <usart.h>
#include <timers.h>
#include <adc.h>

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
	char Message1[8]="Start!!";
	char Message2[7]="FUKUDA";
	char data;
	
	//UART初期化
	/*OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 64); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600
	*/
	//Timer0初期化
	OpenTimer0(TIMER_INT_ON & T0_16BIT & T0_SOURCE_INT & T0_PS_1_256);
		//カウント値のロード
		WriteTimer0(0x676A);
	//初期化
		TRISA=0x00;                        // ポートAをすべて出力ピンにする
		TRISB=0;                        // ポートBをすべて出力ピンにする
		TRISC=0b10111111;                        // ポートCをすべて出力ピンにする
		ADCON1 = 0b00001111;				//すべてデジタル
	//**** 割込みの許可
    INTCONbits.GIE=1;           // 割り込みをイネーブルにする
	
	putsUSART(Message1);	
	PORTA = 0b00111100;
	while(1)
	{
		
 		
		
		
		
	}
	//UART終了
	CloseUSART( );
}
//******************************************************
//****** 割込み宣言（優先順位を使わない）
#pragma interrupt isr save = PROD
//***** 割込みベクタへジャンプ命令セット
#pragma code isrcode = 0x0000008
void isr_direct(void)
{ _asm  goto isr  _endasm }

//***** 割込み処理関数
#pragma code
void isr(void)                      // 割り込み関数
{
		if(INTCONbits.T0IF)				// タイマ0割り込み？
		{
			//カウント値のロード
			WriteTimer0(0x676A);
			INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
			PORTA = PORTA^0b00111100;
		
		
		}                                   
}

