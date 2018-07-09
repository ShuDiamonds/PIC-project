/**********************************************
* ヘッダファイルのインクルード
***********************************************/

#include <p18f67j60.h>    // PIC18f67j60のヘッダ・ファイル
#include <delays.h>

/**********************************************
* ピンマクロ
***********************************************/

	#define 	LED0_TRIS		(TRISEbits.TRISE2)
	#define 	LED0_IO			(PORTEbits.RE2)
	#define 	LED1_TRIS		(TRISEbits.TRISE3)
	#define 	LED1_IO			(PORTEbits.RE3)
	#define 	LED2_TRIS		(TRISEbits.TRISE5)
	#define 	LED2_IO			(PORTEbits.RE5)
	#define 	LED_RUN_TRIS		(TRISDbits.TRISD2)
	#define 	LED_RUN_IO			(PORTDbits.RD2)
//	#define 	LED_IO			(*((volatile unsigned char*)(&PORTE)))

	#define 	BUTTON0_TRIS		(TRISBbits.TRISB3)
	#define		BUTTON0_IO		(PORTBbits.RB3)
	#define 	BUTTON1_TRIS		(TRISBbits.TRISB2)
	#define		BUTTON1_IO		(PORTBbits.RB2)
	#define 	BUTTON2_TRIS		(TRISBbits.TRISB1)
	#define		BUTTON2_IO		(PORTBbits.RB1)
	#define 	BUTTON3_TRIS		(TRISBbits.TRISB0)
	#define		BUTTON3_IO		(PORTBbits.RB0)

	// LCD I/O pins
	#define 	LCD_DATA_TRIS	(TRISF)
	#define 	LCD_DATA_IO		(LATF)
	#define 	LCD_RD_WR_TRIS	(TRISFbits.TRISF1)
	#define 	LCD_RD_WR_IO	(LATFbits.LATF1)
	#define 	LCD_RS_TRIS		(TRISFbits.TRISF2)
	#define 	LCD_RS_IO		(LATFbits.LATF2)
	#define 	LCD_E_TRIS		(TRISFbits.TRISF3)
	#define 	LCD_E_IO		(LATFbits.LATF3)

	// Servo Output
	#define	RCS1_TRIS		(TRISCbits.TRISC2)
	#define RCS1_IO			(LATCbits.LATC2)
	#define	RCS2_TRIS		(TRISCbits.TRISC0)
	#define RCS2_IO			(LATCbits.LATC0)
	#define	UIO1_TRIS		(TRISCbits.TRISA4)
	#define	UIO1_IO			(LATCbits.LATA4)
	#define	UIO2_TRIS		(TRISCbits.TRISC7)
	#define	UIO2_IO			(LATCbits.LATC7)
	
/**********************************************
* マクロ
***********************************************/	

		#define CLOCK_FREQ		(41666667ul)      // Hz
		#define GetSystemClock()		(41666667ul)      // Hz
		#define GetInstructionClock()	(GetSystemClock()/4)
		#define GetPeripheralClock()	GetInstructionClock()
	
	#define BusyUART()				BusyUSART()
	#define CloseUART()				CloseUSART()
	#define ConfigIntUART(a)		ConfigIntUSART(a)
	#define DataRdyUART()			DataRdyUSART()
	#define OpenUART(a,b,c)			OpenUSART(a,b,c)
	#define ReadUART()				ReadUSART()
	#define WriteUART(a)			WriteUSART(a)
	#define getsUART(a,b,c)			getsUSART(b,a)
	#define putsUART(a)				putsUSART(a)
	#define getcUART()				ReadUSART()
	#define putcUART(a)				WriteUSART(a)
	#define putrsUART(a)			putrsUSART((far rom char*)a)
	

/**********************************************
* 関数プロトタイプ
***********************************************/
static void InitializeBoard(void);

/**********************************************
* 割り込み処理
***********************************************/

// 低位割り込み（インターバルタイマ）
	#pragma interruptlow LowISR
	void LowISR(void)
	{
		//LED_RUN_IO = 1;
	}
	#pragma code lowVector=0x18
	void LowVector(void){_asm goto LowISR _endasm}
// 高位割り込み（未使用）
//	#pragma code highVector=0x8
//	void HighVector(void){_asm goto HighISR _endasm}

	#pragma code 		// Return to default code section
	
/********************************************
*config
********************************************/

#pragma config XINST=ON
#pragma config WDT=OFF, FOSC2=ON, FOSC=HSPLL, ETHLED=ON
#pragma config CP0=OFF //2011.01.04 コードプロテクトをＯＦＦ追加


/********************************************
*  メイン関数
************************************************/
void main(void)                     // メイン関数
{

	
	//ローカル変数定義
	int p;
	
	 // ハードウェア初期化
	InitializeBoard();
	/*
    TRISA=0;                        // ポートAをすべて出力ピンにする
    TRISB=0;                        // ポートBをすべて出力ピンにする
    TRISC=0;                        // ポートCをすべて出力ピンにする
    TRISD=0x0F;                     // Upper is Output and Lower is Input
*/

		LED_RUN_IO = 0;		//動作確認

	/********** メインループ  ********************/
															
	while(1)
	{/*
	LED_RUN_IO = 0;		//動作確認
	for(p=0;100000000000000000000000<=p;p++)
		{
			Nop();
		}
	LED_RUN_IO = 1;
	 for(p=0;10000000000000000000000<=p;p++)
		{
			Nop();
		};
		*/
		LED_RUN_IO = 1;
	}
}

/*******************************************************************
*  ハードウェア初期化関数
*
********************************************************************/

static void InitializeBoard(void)
{	
	// LEDs
	LED0_TRIS = 0;
	LED1_TRIS = 0;
	LED2_TRIS = 0;
	//LED3_TRIS = 0;
	LED_RUN_TRIS = 0;
	LED0_IO = 0;
	LED1_IO = 0;
	LED2_IO = 0;
//	LED3_IO = 0;
	LED_RUN_IO = 0;
	
	//button
	BUTTON0_TRIS = 1;
	BUTTON1_TRIS = 1;
	BUTTON2_TRIS = 1;
	//BUTTON3_TRIS = 1;
	
	// Servo 
	RCS1_TRIS = 0;
	RCS1_IO = 0;
	RCS2_TRIS = 0;
	RCS2_IO = 0;
	//UIO1_TRIS = 0;
	//UIO1_IO = 0;
	
	
	// Enable 4x/5x PLL on PIC18F87J10, PIC18F97J60, etc.
	OSCTUNE = 0x40;
	// Set up analog features of PORTA
	ADCON0 = 0x0D;		// ADON Channel 3
	ADCON1 = 0x0B;		// Vdd/Vss AN0 to AN3 are analog
	ADCON2 = 0xBE;			// Right justify, 20TAD ACQ time, Fosc/64 (~21.0kHz)
    
	// Enable internal PORTB pull-ups
   INTCON2bits.RBPU = 0;
	
	// Enable Interrupts
	RCONbits.IPEN = 1;		// Enable interrupt priorities
    INTCONbits.GIEH = 1;
    INTCONbits.GIEL = 1;
	ADCON0bits.ADCAL = 1;
    	ADCON0bits.GO = 1;
	while(ADCON0bits.GO);
	ADCON0bits.ADCAL = 0;
	
}








