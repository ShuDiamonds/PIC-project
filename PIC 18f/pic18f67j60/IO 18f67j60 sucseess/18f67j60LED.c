/**********************************************
* ヘッダファイルのインクルード
***********************************************/
#include <p18f67j60.h>
#include <delays.h>
/**********************************************
* config設定
***********************************************/
/*
#pragma config DEBUG = OFF		//バックグラウンド・デバッグ・モード（BDMを使わない
//#pragma config ETHLED = ON		//Ethernet LED を使う		エラーになるのでコメントアウト
#pragma config XINST = OFF		//拡張命令セットとインデックスアドレスは 使 わない（ 従来 モード）選択ビット
#pragma config STVR = OFF		//Stack Overflow Resetを使わない
#pragma config WDT = OFF		//ウォッチドッグタイマーを使わない
#pragma config WDTPS = 32768	//ウォッチドッグタイマーを1:32768にする
#pragma config CP0 = OFF		//ブロック０（000800－001ＦＦＦｈ）のコードを保護しない
#pragma config FCMEN = OFF		//Fail-Safe Clock Monitorを使わない
#pragma config IESO = OFF		//Internal External Osc. Switchを使わない
#pragma config FOSC = EC		//内部クロックを使う
//#pragma config FOSC = HS		//外付け振動子 利用 の 高速 クロック（8ＭＨｚ 以上 ） 発振
#pragma config FOSC2 = OFF		//Default/Reset System Clock Select Bit　のことらしい

//#pragma code
*/

#pragma config XINST=ON
#pragma config WDT=OFF, FOSC2=ON, FOSC=HSPLL, ETHLED=ON
#pragma config CP0=OFF //2011.01.04 コードプロテクトをＯＦＦ追加


/**************************************************
*configについての解説
**************************************************/
/*
		//configの設定はPICによって記述の仕方が違うので注意
			上のxconfigはpic16f67j60のものでほかのPICではコンパイルできない
			
		
	*Brown-out Resetとは、電圧がある一定いかになるとリセットをかける動作
	*Stack Overflow Resetとは、スタックオーバーフローが起きるとリセットをかける動作
	*Fail-Safe Clock Monitorとは、外部クロックが入力されなくなった場合に、それを検知して内部クロックで動くようにするオプションです
	*Internal External Osc. Switch、とは外部クロックが安定するまで内部クロックで動作。これを自動で行うオプションで
	*Low Voltage Programmingとは、低電圧書き込みモードのことで用は５V（PICによって変わる）で書き込む時に必要PICは買ったばかりの時はON（１）である
	*プリすケーラとは、クロックのスピードを遅くすること例えばPLLDIV = 2		２で 割 り 算 するつまり８MHzを４MHzにするということ
	*HCS08マイコンは，バックグラウンド・デバッグ・モード（BDM）と呼ばれるデバッグ・モードを装備しており，原則マイコンの動作を妨げることなくマイコンの内部状態を調べること
	*picのpllについて、#pragma config	FOSC = HS	で外付け振動子 利用 の 高速 クロック（8ＭＨｚ 以上 ） 発振
	でそれを#pragma config	FOSC = HSPLL_HSに変えると外付け振動子利用の高速クロック発振で、ＰＬＬを 使 う(HSPLL)　→ラトルズではこのモード
	になるそのうちPLLは、
#pragma config	PLLDIV = 1	プリスケーラ 使用 しない（4ＭＨｚの 発振器 入力 を 直接 利用 ）
#pragma config	PLLDIV = 2	２で 割 り 算 する（8ＭＨｚの 発振 器 入力 を 利用 ）
#pragma config	PLLDIV = 3	３で割り算する（１２ＭＨｚの発振器入力を利用）
#pragma config	PLLDIV = 4	４で割り算する（１６ＭＨｚの発振器入力を利用）
#pragma config	PLLDIV = 5	５で割り算する（２０ＭＨｚの発振器入力を利用） ->ラトルズ 基板 20ＭＨｚ／5＝4ＭＨｚ
#pragma config	PLLDIV = 6	６で割り算する（２４ＭＨｚの発振器入力を利用）
#pragma config	PLLDIV = 10	１０で割り算する（４０ＭＨｚの発振器入力を利用）
#pragma config	PLLDIV = 12	１２で割り算する（４８ＭＨｚの発振器入力を利用）
		になり例えば１０MHzで	#pragma config	PLLDIV = 2
								#pragma config	FOSC = HSPLL_HS
								とするとPICは20MHzで起動する		http://homepage3.nifty.com/mitt/pic/pic1320_04.html

使用例を載せておく
ここから

#pragma config MCLRE  = OFF		//マスタークリアを使わない
#pragma config PWRTEN = OFF		//Power-up Timerを使わない
#pragma config BOREN  = OFF		//Brown-out Resetを使わない		
#pragma config BORV   = 30		//Brown-out Voltageを3Vにする
#pragma config WDTEN  = OFF		//ウォッチドッグタイマーを使わない
#pragma config WDTPS  = 32768		//ウォッチドッグタイマーを1:32768にする
#pragma config STVREN = ON		//Stack Overflow Resetを使う
#pragma config FOSC   = IRC		//  内部クロック
//#pragma config	FOSC = HS	//外付け振動子 利用 の 高速 クロック（8ＭＨｚ 以上 ） 発振
#pragma config PLLEN  = ON		//わからない
#pragma config CPUDIV = NOCLKDIV		//CPU System Clock Postscaler
#pragma config USBDIV = OFF		//USBのclockをOSC1/OSC2から取る
#pragma config FCMEN  = OFF		//Fail-Safe Clock Monitorを使わない
#pragma config IESO   = OFF		//Internal External Osc. Switchを使わない
#pragma config HFOFST = OFF		//
#pragma config LVP    = ON		//Low Voltage Programmingを使う
#pragma config XINST  = OFF		//拡張命令セットとインデックスアドレスは 使 わない（ 従来 モード）選択ビット
#pragma config BBSIZ  = OFF		//Boot Block Sizeのサイズ指定っぽい
#pragma config CP0    = OFF		//ブロック０（000800－001ＦＦＦｈ）のコードを保護しない
#pragma config CP1    = OFF		//ブロック１（00２０00－00３ＦＦＦｈ）のコードを保護しない
#pragma config CPB    = OFF		//ブートブロック（0000０00－0007ＦＦｈ）のコードを保護しない
#pragma config WRT0   = OFF		//ブロック0 (000800-001FFFh) の書込み保護しない
#pragma config WRT1   = OFF		//ブロック1 (002000-003FFFh) の書込み保護しない
#pragma config WRTB   = OFF		//Boot block (000000-0007FFh)の書込み保護しない
#pragma config WRTC   = OFF		//Configuration registers (300000-3000FFh) の書込み保護しない
#pragma config EBTR0  = OFF		//Block 0 (000800-001FFFh) を他のブロック実行時のテーブル読み取りから保護しない
#pragma config EBTR1  = OFF		//Block 1 (002000-003FFFh) を他のブロック実行時のテーブル読み取りから保護しない
#pragma config EBTRB  = OFF		//Boot block (000000-0007FFh)を他のブロック実行時のテーブル読み取りから保護しない

#pragma code

ここまで



*/
/**********************************************
* ピンマクロ
***********************************************/

	#define 	LED0_TRIS		(TRISEbits.TRISE2)
	#define 	LED0_IO			(PORTEbits.RE2)
	#define 	LED1_TRIS		(TRISEbits.TRISE3)
	#define 	LED1_IO			(PORTEbits.RE3)
	#define 	LED2_TRIS		(TRISEbits.TRISE5)
	#define 	LED2_IO			(PORTEbits.RE5)
	//#define 	LED3_TRIS		(TRISEbits.TRISD1)
	//#define 	LED3_IO			(PORTEbits.RD1)

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
	#define		RCS1_TRIS		(TRISCbits.TRISC2)
	#define 	RCS1_IO			(LATCbits.LATC2)
	#define		RCS2_TRIS		(TRISCbits.TRISC0)
	#define 	RCS2_IO			(LATCbits.LATC0)
	#define		UIO1_TRIS		(TRISCbits.TRISA4)
	#define		UIO1_IO			(LATCbits.LATA4)
	#define		UIO2_TRIS		(TRISCbits.TRISC7)
	#define		UIO2_IO			(LATCbits.LATC7)

/**********************************************
* 関数プロトタイプ
***********************************************/
static void InitializeBoard(void);
/**********************************************
* 割り込み処理
***********************************************/
/*
// 低位割り込み（インターバルタイマ）
	#pragma interruptlow LowISR
	void LowISR(void)
	{
		
	}
	#pragma code lowVector=0x18
	void LowVector(void){_asm goto LowISR _endasm}
// 高位割り込み（未使用）
//	#pragma code highVector=0x8
//	void HighVector(void){_asm goto HighISR _endasm}

#pragma code 		// Return to default code section
*/
/********************************************
*  メイン関数
************************************************/

void main(void)
{
	//ローカル変数定義
	
	int p;
	
	LED_RUN_IO = 0;		//動作確認
	LED0_IO	= 0;
	LED1_IO	= 0;
	while(1)
	{
	
		LED_RUN_IO = 0;		//動作確認
		for(p=0;1000000000000000000000000<=p;p++)
			{
				Nop();
			}
		LED_RUN_IO = 1;
		 for(p=0;100000000000000000000000<=p;p++)
		{
			Nop();
		};
		LED_RUN_IO = 0;
		for(p=0;1000000000000000000000000<=p;p++)
		{
			Nop();
		}
		LED_RUN_IO = 1;
		for(p=0;10000000000000000000000<=p;p++)
		{
			Nop();
		}
		LED_RUN_IO = 0;
	}
}

/*******************************************************************
*  ハードウェア初期化関数
*
********************************************************************/

static void InitializeBoard(void)
{	
	/*
	// LEDs
	LED0_TRIS = 0;
	LED1_TRIS = 0;
	LED2_TRIS = 0;
	//LED3_TRIS = 0;
	LED_RUN_TRIS = 0;
	LED0_IO = 0;
	LED1_IO = 0;
	LED2_IO = 0;
	//LED3_IO = 0;
	LED_RUN_IO = 0;
	
	//button
	BUTTON0_TRIS = 1;
	BUTTON1_TRIS = 1;
	BUTTON2_TRIS = 1;
	BUTTON3_TRIS = 1;
	
	// Servo 
	RCS1_TRIS = 0;
	RCS1_IO = 0;
	RCS2_TRIS = 0;
	RCS2_IO = 0;
	//UIO1_TRIS = 0;
	//UIO1_IO = 0;
	*/
	
	
	TRISA=0;                        // ポートAをすべて出力ピンにする
	TRISB=0;                        // ポートBをすべて出力ピンにする
	TRISC=0;                        // ポートCをすべて出力ピンにする
	TRISD=0x00;                     // Upper is Output and Lower is Input
	
	
	// Enable 4x/5x PLL on PIC18F87J10, PIC18F97J60, etc.
	OSCTUNE = 0x40;			
	// Set up analog features of PORTA
	ADCON0 = 0x0D;		// ADON Channel 3
	ADCON1 = 0x0B;		// Vdd/Vss AN0 to AN3 are analog
	ADCON2 = 0xBE;			// Right justify, 20TAD ACQ time, Fosc/64 (~21.0kHz)
    /*
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
	*/
}

