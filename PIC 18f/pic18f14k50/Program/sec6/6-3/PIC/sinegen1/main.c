/*********************************************************************
*
*　USB接続 正弦波発振器プログラム
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
char input_buffer[64];					//USB入力バッファ
char output_buffer[64];					//USB出力バッファ
unsigned long Freq = 10;					//周波数データ 32bits
unsigned long Delta = 10;					//周波数可変幅 32bits
char flag = 0;							//正弦波/三角波

byte MsgFreq[] =  "Freq =        Hz";		//液晶表示用バッファ
char MsgStart[] ="Start!";					//初期メッセージ
char MsgSine[] = "Sine Wave       ";		//液晶表示メッセージ
char MsgTri[]  = "Triangle        ";		//液晶表示メッセージ

/** P R I V A T E  P R O T O T Y P E S *******************************/
void SetupDDS(unsigned long freqdata, char flag);
void SerialOut(unsigned long data);
void Display(void);
void ltostring(char digit, unsigned long data, byte *buffer);

#pragma code
/*********************************************************************
 * Function:        void main(void)
 ********************************************************************/
void main(void)
{
	int i;
	
	ADCON1 |= 0x0F;                 	// 全ディジタルに設定
	CMCON = 0x07;						// コンパレータオフ
 	LATA = 0x06;						// エンコーダ、DDS初期出力
   	LATB = 0;						// 液晶表示ポートリセット
  	LATC  = 0x03;						// LED off
 	TRISA = 0x38;						// スイッチ
	TRISB = 0x01;						// 液晶表示
 	TRISC = 0x38;						// LED Output Mode
	/// USB初期化
	mInitializeUSBDriver();         	// See usbdrv.h
	/// 液晶表示器初期化
	lcd_init();						// LCD初期化
	lcd_str(MsgStart);				// 初期メッセージ表示
	Delay10KTCYx(500);				// 0.5秒
	Freq = 1000;						// 初期値1kHz
	SetupDDS(Freq, flag);				// DDS出力
	Display();						// 液晶表示
	Delay10KTCYx(100);
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
    			if(getsUSBUSART(input_buffer,64))		// データ受信ポール
    			{
            		LATCbits.LATC0 = !LATCbits.LATC0;	// LED反転
	    			switch(input_buffer[0])			// 最初の１文字チェック
	    			{
		    			case '0':					// OK応答
         				if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("OK");
           			 	break;		    			
		    			case '1':					// 現在値入力
         				if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam(MsgFreq+7, 6);
           			 	break;		    				
					case '2':					// 周波数設定　正弦波
						Freq = 0;
						for(i=0; i<6; i++)
						{
							Freq = Freq * 10 + (input_buffer[i+1] - 0x30);
						}
						flag = 0;
						break;
					case '3':					// 周波数設定 三角波
						Freq = 0;
						for(i=0; i<6; i++)
						{
								Freq = Freq *10 + (input_buffer[i+1]-0x30);
						}
						flag = 1;
						break;  				
		    			default: break;
		    		}
		    		SetupDDS(Freq, flag);				// DDS設定出力
	    			Display();
    			}
    		}
    		//// 周波数刻み設定スイッチチェック
    		if(stepSW)
    			Delta = 1000;
    		else
    			Delta = 10;    		
    		//// エンコーダの入力チェック
    		if(encoder_a == 0)
    		{

	    		if(encoder_b == 0)
	    			Freq += Delta;				// 右周りカウントアップ
	    		else
	    			Freq -= Delta;				// 左周りカウントダウン

	    		SetupDDS(Freq, flag);				// DDS設定出力
	    		Display();						// 液晶表示
			while(encoder_a == 0);			// エンコーダ入力完了待ち
    		}
     }
}

////　DDSシリアル出力関数
void SetupDDS(unsigned long freqdata, char flag) {
	float SetFreq;
	unsigned long temp;
	unsigned int uptemp, lowtemp;
	
	///設定周波数から設定パラメータ計算
	///クロック50MHzで計算
	SetFreq = 5.36871 * (unsigned long)freqdata;	// 設定値
	temp = (unsigned long)SetFreq;				// 整数に変換
	lowtemp =(unsigned int)(temp & 0x3FFF);			// 下位16ビット
	uptemp = (unsigned int)((temp >> 14) & 0x3FFF);	// 上位16ビット
	/// DDSへコマンド送信
	/// Sine 0x2028   Triangle 0x200A
	if(flag == 0)
		SerialOut(0x2028);				// コマンド出力
	else
		SerialOut(0x200A);				// 三角波コマンド出力
	SerialOut(lowtemp + 0x4000);			// 周波数データ出力
	SerialOut(uptemp + 0x4000);
}

//// DDS送信制御関数
void SerialOut(unsigned long data) {
	unsigned char i;
	unsigned int mask;
	mask = 0x8000;
	
	dds_fsync = 0;						// FSYNC low
	for(i=0; i<16; i++) {
		if(data & mask)					// 上位から送信
			dds_sdat = 1;	
		else
			dds_sdat = 0;
		dds_sclk = 0;						// SCK
		dds_sclk = 1;
		mask = mask >> 1;					// next bit
	}
	dds_fsync = 1;						// FSYNC High
}

/// データ表示関数
void Display(void) 
{
	//// 周波数可変幅表示
	lcd_cmd(0x80);						//１行目へ
	ltostring(6, Freq, &MsgFreq[0]+7);		// 周波数表示
	lcd_str((char *)MsgFreq);				// 見出し表示
	lcd_cmd(0xC0);						// ２行目へ
	if(flag == 0)
		lcd_str(MsgSine);					// 正弦波表示
	else
		lcd_str(MsgTri);					// 三角波表示
}

///// 数値から文字列に変換
void ltostring(char digit, unsigned long data, byte *buffer)
{
	char i;
	buffer += digit;					// 文字列の最後
	for(i=digit; i>0; i--) {			// 最下位桁から上位へ
		buffer--;					// ポインター１
		*buffer = (data % 10) + '0';	// その桁数値を文字にして格納
		data = data / 10;				// 桁-1
	}
}

    
