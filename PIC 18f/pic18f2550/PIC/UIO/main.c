/*********************************************************************
*
*　USB接続 汎用Ｉ／Ｏプログラム
*	USBフレ-ムワーク修正情報
*　　コンフィギュレーションの設定変更
*    CDCクラスベース
*　　main.c　の変更　初期化削除　IO設定変更
*　　io_cfg.hの変更　LED削除　IOモニタ削除
*　　user.cの変更　　実行内容削除
*　　usbcfg.hの変更　IOモニタ削除
*
*********************************************************************

/** I N C L U D E S ************************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                 // Required
#include "system\usb\usb.h"                  // Required
#include "io_cfg.h"  
                        // Required
#include	<p18f2550.h>

#include "system\usb\usb_compile_time_validation.h"
#include	"delays.h"
#include "lcd_lib.h"
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

/*** Configuration *******/
        #pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
        #pragma config CPUDIV   = OSC1_PLL2   
        #pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
        #pragma config FOSC     = HSPLL_HS
        #pragma config FCMEN    = OFF
        #pragma config IESO     = OFF
        #pragma config PWRT     = OFF
        #pragma config BOR      = ON
        #pragma config BORV     = 3
        #pragma config VREGEN   = ON      //USB Voltage Regulator
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
/** V A R I A B L E S **********************************************/
#pragma udata
char input_buffer[64];						// USB入力バッファ
char output_buffer[64];						// USB出力バッファ
unsigned int  Pot0;							// 計測データ
unsigned int Counter;
unsigned int cmnd;
char MsgPot[] =  "Pot =         ";			// 液晶表示用バッファ
char MsgStart[] = "Start!";					// 初期メッセージ
char MsgHello[] = "Welcome         ";
char MsgState[] = "  ";						// LED状態
// アナログデータ送信バッファ（８チャネル分×４文字）
char MsgAnalog[] = "                                    ";

/** P R I V A T E  P R O T O T Y P E S ******************************/
void ltostring(char digit, unsigned long data, byte *buffer);
unsigned int AD_input(char chanl);
unsigned int toHEX(char *str);

#pragma code
/*********************************************************************
 * Function:        void main(void)
 ********************************************************************/
void main(void)
{
	int i;
	byte MsgSample[] = {" "};
	
	ADCON1 = 0x07;                 		// RA,RE全アナログに設定
	ADCON2 = 0xBB;					// Right,Tad＝Tcy/16 
	ADCON0 = 0x1D;					// AD On	
	CMCON = 0x07;						// コンパレータオフ
 	LATA = 0xFF;						// 初期出力
   	LATB = 0;							// 液晶表示ポートリセット
  	LATC = 0x07;						// LED Off
 	TRISA = 0xFF;						// アナログ入力
	TRISB = 0x00;						// 液晶表示
 	TRISC = 0xB8;						// LED, USART
	/// USB初期化
	mInitializeUSBDriver();         	// See usbdrv.h
	/// 液晶表示器初期化
	Counter = 0;
	//lcd_init();						// LCD初期化
	//lcd_str(MsgStart);				// 初期メッセージ表示

    /// メインループ
    while(1)
    {
    		USBCheckBusStatus();        							// USB接続チェック
    		if(UCFGbits.UTEYE!=1)									// アイパターンモードオフか
        		USBDriverService();								// USBイベントポーリング
		CDCTxService();										// クラス送受信実行
 		/// 送受信データの処理実行
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
    			if(getsUSBUSART(input_buffer,64))					// データ受信ポール
    			{
            		LATCbits.LATC0 = !LATCbits.LATC0;				// LED0反転
	    			switch(input_buffer[0])						// 最初の１文字チェック
	    			{
		    			case '0':								// OKメッセージ返送
        					if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("OK");
           			 	break;		    				
		    			
	    				case '1':								// OKメッセージ返送
        					if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("Farst");
           			 	break;		
	    				
	    				case 'F':								// OKメッセージ返送
        					if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("fukuda");
           			 	break;
	    				
		    			default:
	    				if(mUSBUSARTIsTxTrfReady())
	    				MsgSample[0] = input_buffer[0];
           			 	mUSBUSARTTxRam(MsgSample,1);
	    				
	    				break;
		    		}
    			}
    		}
     }
}

/////// ２文字を16進数１桁に変換
unsigned int toHEX(char* str)
{
	int data;

	data = 0;
	if((*str >= '0') && (*str <= '9'))
		data = (int)(16 * (*str - '0'));
	else
	{
		if((*str >= 'A') && (*str <= 'F'))
			data = (int)(16 * (*str - 'A' + 10));
		else
		{
			if((*str >= 'a') && (*str <= 'f'))
				data = (int)(16 * (*str - 'a' + 10));
		}
	}
	str++;
	if((*str >= '0') && (*str <= '9'))
		data += (int) (*str - '0');
	else
	{
		if((*str >= 'A') && (*str <= 'F'))
			data += (int)(*str - 'A' + 10);
		else
		{
			if((*str >= 'a') && (*str <= 'f'))
				data += (int)(*str - 'a' + 10);
		}
	}	
	return(data);
}

//////// A/D変換読み込み関数
unsigned int AD_input(char chanl)
{
    	unsigned int adData;        		// 10ビットモード
    	
	ADCON0 = 0x01 + (chanl * 4);
    	ADCON0bits.GO = 1;              	// Start AD conversion
    	while(ADCON0bits.NOT_DONE);     	// Wait for conversion
 	adData = ADRESH * 256 + ADRESL;
	return(adData);                    	// データ返し
}

///// 数値から文字列に変換
void ltostring(char digit, unsigned long data, byte *buffer)
{
	char i;
	buffer += digit;					// 文字列の最後
	for(i=digit; i>0; i--) {			// 最下位桁から上位へ
		buffer--;						// ポインター１
		*buffer = (data % 10) + '0';	// その桁数値を文字にして格納
		data = data / 10;				// 桁-1
	}
}

