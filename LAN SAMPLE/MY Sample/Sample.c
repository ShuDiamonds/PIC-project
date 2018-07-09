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
	
    	
	   // TickUpdate();
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


/********************************************
*  メイン関数
************************************************/
void main(void)
{
    static TICK t = 0;
	int p;
	BYTE i;

	
	
    // ハードウェア初期化
    InitializeBoard();
    // インターバルタイマ初期化
    TickInit();
	
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
	
	/********** メインループ  ********************/
	while(1)
	{
		
		if(BUTTON0_IO == 0)           // スイッチ１OFFか？
			{
			    LED0_IO = 1;
for(p=0;1000000000000000000000000<=p;p++)
		{
			Nop();
		}			  LED0_IO = 0;
			    for(p=0;1000000000000000000000000<=p;p++)
		{
			Nop();
		}(100);
			}
			else if(BUTTON1_IO == 0)      // スイッチ２OFFか？
			{
			   LED1_IO = 1;
for(p=0;1000000000000000000000000<=p;p++)
		{
			Nop();
		}			    LED1_IO = 0;
for(p=0;1000000000000000000000000<=p;p++)
		{
			Nop();
		}			}
			else if(BUTTON2_IO == 0)      // スイッチ３OFFか？
			{
			   LED2_IO = 1;
for(p=0;1000000000000000000000000<=p;p++)
		{
			Nop();
		}			   LED2_IO = 0;
for(p=0;1000000000000000000000000<=p;p++)
		{
			Nop();
		}			}
			else                        // 全部ONか？
			{
			LED0_IO = 0;
			LED1_IO = 0;
			LED2_IO = 0;
			}
		/*
		if(BUTTON0_IO == 0)           // スイッチ１OFFか？
			{
			    LED0_IO = 1;
			    Delay10KTCYx(100);      // LED１を点滅
			  LED0_IO = 0;
			    Delay10KTCYx(100);
			}
			else if(BUTTON1_IO == 0)      // スイッチ２OFFか？
			{
			   LED1_IO = 1;
			    Delay10KTCYx(200);      // LED2を点滅
			    LED1_IO = 0;
			    Delay10KTCYx(200);
			}
			else if(BUTTON2_IO == 0)      // スイッチ３OFFか？
			{
			   LED2_IO = 1;
			    Delay10KTCYx(100);      // LED３を点滅
			   LED2_IO = 0;
			    Delay10KTCYx(100);
			}
			else                        // 全部ONか？
			{
			LED0_IO = 0;
			LED1_IO = 0;
			LED2_IO = 0;
			}
*/
	    	
	    	
	    /*
	    	
			// LED0の点滅（1秒間隔）
			if(TickGet() - t >= TICK_SECOND/2ul)
	        	{
	            	t = TickGet();
	            	LED0_IO ^= 1;
	        		
	        	}
	    */
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
	//LED3_IO = 0;
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
