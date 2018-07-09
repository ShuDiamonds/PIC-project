/****************************************
*  TCP/IPスタック　サンプルプログラム
*  プロジェクト名：Sample
*　LEDの点滅のみ
****************************************/
// 宣言とヘッダファイルのインクルード
#define THIS_IS_STACK_APPLICATION
#include "TCPIP Stack/TCPIP.h"			// 必要なヘッダをまとめてインクルード

// 関数プロトタイピング
static void InitializeBoard(void);

/**********************************************
* 割り込み処理
***********************************************/
// 低位割り込み（インターバルタイマ）
	#pragma interruptlow LowISR
	void LowISR(void)
	{
	    TickUpdate();
	}
	#pragma code lowVector=0x18
	void LowVector(void){_asm goto LowISR _endasm}
// 高位割り込み（未使用）
//	#pragma code highVector=0x8
//	void HighVector(void){_asm goto HighISR _endasm}

	#pragma code 		// Return to default code section

/************************************************
*  メイン関数
************************************************/
void main(void)
{
    static TICK t = 0;
	BYTE i;

    // ハードウェア初期化
    InitializeBoard();
    // インターバルタイマ初期化
    TickInit();
	/********** メインループ  ********************/
    while(1)
    {
		// LED0の点滅（1秒間隔）
		if(TickGet() - t >= TICK_SECOND/2ul)
        	{
            	t = TickGet();
            	LED0_IO ^= 1;
        	}
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
	LED3_TRIS = 0;
	LED0_IO = 0;
	LED1_IO = 0;
	LED2_IO = 0;
	LED3_IO = 0;

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
