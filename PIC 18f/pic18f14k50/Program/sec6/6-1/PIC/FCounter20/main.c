/*********************************************************************
*
*　USB接続 周波数カウンタプログラム
*	USBフレームワーク修正情報
*　　コンフィギュレーションの設定変更
*　　main.c　の変更　初期化削除　IO設定変更
*　　io_cfg.hの変更　LED削除　IOモニタ削除
*　　user.cの変更　　実行内容削除
*　　usbcfg.hの変更　IOモニタ削除
*
*********************************************************************
/** I N C L U D E S *************************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                        // Required
#include "system\usb\usb.h"                         // Required
#include "io_cfg.h"                                 // Required
#include "system\usb\usb_compile_time_validation.h" // Optional
#include	"delays.h"
#include "lcd_lib.h"

/*** Configuration *******/
#pragma config FOSC = HSPLL_HS
#pragma config WDT = OFF
#pragma config PLLDIV = 5
#pragma config CPUDIV = OSC1_PLL2
#pragma config USBDIV = 2
#pragma config PWRT = ON
#pragma config BOR = ON
#pragma config BORV = 43
#pragma config LVP = OFF
#pragma config VREGEN = ON
#pragma config MCLRE = ON
#pragma config PBADEN = OFF

/** V A R I A B L E S ***********************************************/
#pragma udata
char input_buffer[64];					// USB入力バッファ
char output_buffer[64];					// USB出力バッファ

char Prescale;							// プリセーラフラグ
char EndFlag = 0;							// カウント終了フラグ
char SecCounter;							// 1秒カウント
unsigned long FreqHigh;					// 周波数カウンタ上位
unsigned long dumy;						// 周波数計算用ワーク


char MsgFreq[] = "Freq=         Hz";		// 液晶表示用メッセージ
char MsgSend[] = "        \r\n";			// USB送信用メッセージ
char MsgStart[] ="Start!";					// 初期メッセージ
char MsgPre8[] = "Scale1/8Max50MHz";		// プリスケーラ設定表示
char MsgPre1[] = "Scale1/1Max10MHz";
char MsgAns[] = " ";


/** P R I V A T E  P R O T O T Y P E S *******************************/
void Display(unsigned long Freq);
void ltostring(char digit, unsigned long data, char *buffer);
void ISRProcess(void);

/** V E C T O R  M A P P I N G ***********************************/
#pragma code high_vector=0x08
void interrupt_at_high_vector(void)
{
    _asm goto ISRProcess _endasm
}
#pragma code

////// CCP1 Interrupt Process
#pragma interrupt ISRProcess
void ISRProcess(void)
{
	PIR1bits.CCP1IF = 0;				// Interrupt Flag Clear		
	SecCounter--;
	if(SecCounter == 0)
	{
 		GreenLED = !GreenLED;
		T0CONbits.TMR0ON = 0;			// Stop Count
		T1CONbits.TMR1ON = 0;
		EndFlag = 1;					// Count End Flag
	}
}



#pragma code
/*********************************************************************
 * Function:        void main(void)
 ********************************************************************/
void main(void)
{
	int i;
	
	ADCON1 |= 0x0F;    	             	// 全ディジタルに設定
	CMCON = 0x07;						// コンパレータオフ
   	LATB = 0;						// 液晶表示ポートリセット
  	LATC  = 0xC0;						// LED off
 	TRISA = 0x10;						// T0CKI
	TRISB = 0x0;						// 液晶表示
 	TRISC = 0x31;						// LED Output Mode
 	// タイマ初期化
 	T1CON = 0xB2;						// ExtClock 1/8
 	TMR1H = 0;						// 64000 Count
 	TMR1L = 0;
	T0CON = 0x22;						// Ext Counter 1/8
	TMR0H = 0;
	TMR0L = 0;
	T3CON = 0;						// Timer1 for CCP1
 	// CCP1初期化
 	CCP1CON = 0x0B;					// Timer Clear, Interrupt
	CCPR1 = 64000;					// 12.8MHz÷8÷64000=25Hz					
	/// USB初期化
	mInitializeUSBDriver();         	// See usbdrv.h
	/// 液晶表示器初期化
	lcd_init();						// LCD初期化
	lcd_str(MsgStart);				// 初期メッセージ表示
	Delay10KTCYx(500);				// 0.5秒
	/// 変数初期化
	Prescale = 0;						// default 1/1
	FreqHigh = 0;
	EndFlag = 1;
	// Enable Interrupt
	RCONbits.IPEN = 0;				// No Priority
 	PIE1bits.CCP1IE = 1;				// Enable CCP1		
	INTCON = 0xC0;					// Enable Interrupt
    /// メインループ
    while(1)
    {
    		USBCheckBusStatus();        	// USB接続チェック
    		if(UCFGbits.UTEYE!=1)			// アイパターンモードオフか
        		USBDriverService();		// USBイベントポーリング
		CDCTxService();				// クラス送受信実行
		/// 送受信データの処理実行
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
    			if(getsUSBUSART(input_buffer,64))			// データ受信ポール
    			{
	    			switch(input_buffer[0])				// 最初の１文字チェック
	    			{
		    			case '0':						// OK応答
         				if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("OK");
           			 	break;
           			case '1':
           				Prescale = 0;
           				MsgAns[0] = '0';
         				if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam((byte*)MsgAns, 1);           				
           				break;
           			case '2':
           				Prescale = 1;
           				MsgAns[0] = '1';
         				if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam((byte*)MsgAns, 1);            				
           				break;		    				
					case '3':
         				if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam((byte*)MsgSend, 10);
           			 	break;					

		    			default: break;
		    		}
    			}
    		}
		//// 周波数カウンタ機能実行
		if(INTCONbits.TMR0IF)				// タイマ0オーバーフロー
 		{
	 		INTCONbits.TMR0IF = 0;		
 			FreqHigh++;					// カウンタ変数＋１
 		}		
    		if(EndFlag == 1)					// １秒ゲート終了か？
    		{
	    		/// 周波数計算
	    		dumy = (unsigned long)TMR0L;	
			dumy = dumy + (unsigned long)TMR0H*256;
			if(Prescale)
				dumy = 8*(FreqHigh*65536 + dumy);			
			else
				dumy = FreqHigh*65536 + dumy;
			// 周波数表示			
	    		Display(dumy);
			// 次の計測準備
			if(Prescale)
			{
				T0CON = 0x22;				// 1/8
				RedLED = 1;				// LED Off
			}
			else
			{
				T0CON = 0x28;				// 1/1
				RedLED = 0;				// LED On
			}
	    		EndFlag = 0;
	    		TMR0H = 0;					// reset counter
	    		TMR0L = 0;
			FreqHigh = 0;
	    		SecCounter = 25;				// Reset 1 sec counter
	    		TMR1H = 0;					// Reset Timer1
			TMR1L = 0;
			// 次の計測開始
			T1CONbits.TMR1ON = 1;
	    		T0CONbits.TMR0ON = 1;			// Start Count
	    }		
     }
}

/// データ表示関数
void Display(unsigned long Freq) 
{
	//// 周波数可変幅表示
	lcd_cmd(0x80);						//１行目へ
	ltostring(8, Freq, &MsgFreq[0]+6);		// 周波数表示
	ltostring(8, Freq, &MsgSend[0]);
	lcd_str((char *)MsgFreq);				// 見出し表示
	lcd_cmd(0xC0);						// 2行目へ
	if(Prescale)
		lcd_str(MsgPre8);
	else
		lcd_str(MsgPre1);
}

///// 数値から文字列に変換
void ltostring(char digit, unsigned long data, char *buffer)
{
	char i;
	buffer += digit;					// 文字列の最後
	for(i=digit; i>0; i--) {			// 最下位桁から上位へ
		buffer--;					// ポインター１
		*buffer = (data % 10) + '0';	// その桁数値を文字にして格納
		data = data / 10;				// 桁-1
	}
}

    
