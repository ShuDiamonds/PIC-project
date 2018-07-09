/**************************************************************
*
*　USB接続 汎用Ｉ／Ｏプログラム
*    汎用USBクラスを使用
*	USBフレ-ムワーク修正情報
*　　コンフィギュレーションの設定変更
*　　main.c　の変更　初期化削除　IO設定変更
*　　io_cfg.hの変更　LED削除　IOモニタ削除
*　　user.cの変更　　実行内容削除
*　　usbcfg.hの変更　IOモニタ削除
*
***************************************************************

/** I N C L U D E S *******************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                 // Required
#include "system\usb\usb.h"                  // Required
#include "io_cfg.h"                          // Required

#include "system\usb\usb_compile_time_validation.h"
#include	"delays.h"
#include "lcd_lib.h"

/*** Configuration *******/
#pragma config FOSC = HSPLL_HS
#pragma config CPUDIV = OSC1_PLL2
#pragma config PLLDIV = 5
#pragma config WDT = OFF
#pragma config USBDIV = 2
#pragma config PWRT = ON
#pragma config BOR = ON
#pragma config BORV = 43
#pragma config LVP = OFF
#pragma config VREGEN = ON
#pragma config MCLRE = ON
#pragma config PBADEN = OFF

/** V A R I A B L E S *****************************************/
#pragma udata
char input_buffer[64];					// USB入力バッファ
char output_buffer[64];					// USB出力バッファ
unsigned int  Pot0;						// 計測データ
unsigned int Counter;						// 折り返し用カウンタ
unsigned int RcvSize;						// 受信データサイズ

char MsgOK[] ="OK";						// OKメッセージ
char MsgPot[] =  "Pot =        \r\n";		// 液晶表示用バッファ
char MsgStart[] = "Start!";				// 初期メッセージ
char MsgHello[] = "Welcome        \r\n";	// 折り返し用メッセージ
char MsgState[] = " ";					// LED状態
// アナログデータ送信バッファ（８チャネル分×４文字）
char MsgAnalog[] = "                                      ";
/** P R I V A T E  P R O T O T Y P E S ************************/
void ltostring(char digit, unsigned long data, char *buffer);
unsigned int AD_input(char chanl);

#pragma code
/****************************************************
 * Function:        void main(void)
 ****************************************************/
void main(void)
{
	int	i;
	
	ADCON1 = 0x07;             	// RA,RE全アナログに設定
	ADCON2 = 0xBB;				// Right,Tad＝Tcy/16 
	ADCON0 = 0x1D;				// AD On	
	CMCON = 0x07;					// コンパレータオフ
 	LATA = 0xFF;					// 初期出力
   	LATB = 0;					// 液晶表示ポートリセット
  	LATC = 0x07;					// LED Off
  	LATD = 0;
 	TRISA = 0xFF;					// アナログ入力
	TRISB = 0x00;					// 液晶表示出力
 	TRISC = 0xB8;					// LED, USART
 	TRISD = 0xF0;					// Universal I/O
 	TRISE = 0xFF;					// アナログ入力
	/// USB初期化
	mInitializeUSBDriver();        	// See usbdrv.h
	/// 液晶表示器初期化
	Counter = 0;
	lcd_init();					// LCD初期化
	lcd_str(MsgStart);			// 初期メッセージ表示

    /// メインループ
    while(1)
    {
	    /// USBイベントポーリング
    		USBCheckBusStatus();        							// USB接続チェック
    		if(UCFGbits.UTEYE!=1)									// アイパターンモードオフか
        		USBDriverService();
 		/// 受信データ処理
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
			/// 受信チェック
			RcvSize = USBGenRead((byte*)&input_buffer,sizeof(input_buffer));
			if(RcvSize)										// データ受信ありか？
    			{
            		LATCbits.LATC0 = !LATCbits.LATC0;				// LED0反転目印用
	    			switch(input_buffer[0])						// 最初の１文字チェック
	    			{
		    			case '0':								// メッセージ返送
         				if(!mUSBGenTxIsBusy())					// 送信レディーか？         			
           			 		USBGenWrite((byte*)MsgOK, 3);		// 送信実行
           			 	break;		    				
		    			case '1':								// 
            				LATCbits.LATC1 = !LATCbits.LATC1;		// LED1反転
            				if(LATCbits.LATC1 == 1)
            					MsgState[0] = '1';				// 現在状態返送
            				else
            					MsgState[0] = '0';
        					if(!mUSBGenTxIsBusy())					// 送信レディーか？
        						USBGenWrite((byte*)MsgState, 1);
		    				break;
		    			case '2':
            				LATCbits.LATC2 = !LATCbits.LATC2;		// LED2反転
             				if(LATCbits.LATC2 == 1)
            					MsgState[0] = '1';				// 現在状態返送
            				else
            					MsgState[0] = '0';
        					if(!mUSBGenTxIsBusy())					// 送信レディーか？	
        						USBGenWrite((byte*)MsgState, 1);
		    				break;
		    			case '3':
		    				lcd_cmd(input_buffer[1]);				// 液晶メッセージ表示
		    				lcd_str((char *)(&input_buffer[3]));
		    				break;
		    			case '4':
		    				lcd_clear();							// 液晶消去
		    				Counter = 0;
		    				break;
		    			case '5':
	    					Pot0 = AD_input(7);
	    					ltostring(4, Pot0, MsgPot+6);			// 可変抵抗の値送信
         				if(!mUSBGenTxIsBusy())					// 送信レディーか？		    					
           			 		USBGenWrite((byte*)MsgPot, 15);
		    				break;
		    			case '6':								// PORTDへの出力と入力
		    				LATD = input_buffer[1];
						MsgState[0] = PORTD;
           				if(!mUSBGenTxIsBusy())					// 送信レディーか？
            					USBGenWrite((byte*)MsgState, 1);
		    				break;
		    			case '7':
         				for(i=0; i<8; i++)					// 8チャネル分計測
	         			{
			    				Pot0 = AD_input(i);
		    					ltostring(4, Pot0, MsgAnalog+4*i);	// 可変抵抗の値編集
           			 	}
		    				if(!mUSBGenTxIsBusy())					// 送信レディーか？
        			 			USBGenWrite((byte*)MsgAnalog, 32);	// 送信実行
           			 	break;
           			 case '8':
         				Counter++;							// メッセージカウンタ更新
         				ltostring(6, Counter, MsgHello+8);
         				if(!mUSBGenTxIsBusy())					// 送信レディーか？         			
           			 		USBGenWrite((byte*)MsgHello, 17);	// 送信実行           			 
           			 	break;
           			 	
		    			default: break;
		    		}
    			}
    		}
     }
}

//////// A/D変換読み込み関数
unsigned int AD_input(char chanl)
{
    	unsigned int adData;        			// 10ビットモード
    	
	ADCON0 = 0x01 + (chanl * 4);
    	ADCON0bits.GO = 1;              		// Start AD conversion
    	while(ADCON0bits.NOT_DONE);     		// Wait for conversion
 	adData = ADRESH * 256 + ADRESL;
	return(adData);                   	 	// データ返し
}

///// 数値から文字列に変換
void ltostring(char digit, unsigned long data, char *buffer)
{
	char i;
	buffer += digit;						// 文字列の最後
	for(i=digit; i>0; i--) {				// 最下位桁から上位へ
		buffer--;						// ポインター１
		*buffer = (data % 10) + '0';		// その桁数値を文字にして格納
		data = data / 10;					// 桁-1
	}
}

