
/*********************************************************************
*
*　USB接続 オシロスコープ
*	USBフレ-ムワーク修正情報
*　　コンフィギュレーションの設定変更
*    CDCクラスベース
*　　main.c　の変更　初期化削除　IO設定変更
*　　io_cfg.hの変更　LED削除　IOモニタ削除
*　　user.cの変更　　削除
*　　usbcfg.hの変更　IOモニタ削除
*  ★★Linkerを修正しているので注意★★
*
*********************************************************************

/** I N C L U D E S ************************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                 // Required
#include "system\usb\usb.h"                  // Required
#include "io_cfg.h"                          // Required

#include "system\usb\usb_compile_time_validation.h"

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

/** V A R I A B L E S **********************************************/
#pragma udata
static char input_buffer[64];			// USB入力バッファ
static char output_buffer[64];			// USB出力バッファ

unsigned int RcvSize;					// 受信データサイズ
char State = 0;						// ステートフラグ
unsigned int Period;					// サンプル周期
unsigned int Index = 0;				// バッファインデックス
char FreeFlag;						// フリーランフラグ
byte New_Data;						// 今回データ
byte Old_Data;						// 前回データ
byte Threshold;						// トリガ用スレッショルド
byte Sample;							// サンプリング周期
byte Channel;							// チャネル番号

rom char MsgOK[] ="OK";				// OK応答メッセージ
// アナログ計測バッファ　合計960バイト
#pragma udata gpr1 
static byte BufferA[512];				// 
#pragma udata usb6
static byte BufferB[448];				// CDCで一部使用、LinkerのProtectをはずす

/** P R I V A T E  P R O T O T Y P E S ******************************/
void ltostring(char digit, unsigned long data, byte *buffer);
byte AD_input(char chanl);
void ISRProcess(void);

/** V E C T O R  R E M A P P I N G **********************************/
#pragma code high_vector=0x08
void interrupt_at_high_vector(void)
{
    _asm goto ISRProcess _endasm
}
#pragma code

/*****************************************/
/* CCP1の割り込み処理
/* 一定周期でA/D変換を行い信号サンプリング
/* 最短10μsec周期だが性能は50μsec
/****************************************/
#pragma interrupt ISRProcess
void ISRProcess(void)
{
	PIR1bits.CCP1IF = 0;					// 割り込みフラグクリア
	Old_Data = New_Data;					// 同期用に２世代
	New_Data = AD_input(Channel);			// 新規データサンプリング
   	//// トリガ検出と保存
   	switch (State)
   	{
   		case 0:		/// トリガ検出
   			if(FreeFlag == 0) {
	   			if(Threshold > 127) {			// プラスの時は立ち上がりトリガ
	  	 			if((Old_Data < Threshold) && (New_Data > Threshold)) {
		  	 			State = 1;
		  	 			Index = 0;
		  	 		}
		  	 	}
		  	 	else {						// マイナスの時は立ち下がりトリガ
			  	 	if((Old_Data > Threshold) && (New_Data < Threshold)) {
				  	 	State = 1;
				  	 	Index = 0;
				  	 }
				}
			}
			else {
				State = 1;
				Index = 0;
			}			
	  	 	break;
	 	case 1:		/// データ格納
	         if(Index < 512) {				// まずAバッファに格納
	   	      	GreenLED = !GreenLED;		         
				BufferA[Index++] = New_Data;
			}
			else {						// 続きをBバッファに格納
				if(Index < 960) {
	   	      		YellowLED = !YellowLED;	
					BufferB[Index-512] = New_Data;
					Index++;
				}
				else {					// バッファ一杯でステート更新
					State = 2;
					Index = 0;
					T1CONbits.TMR1ON = 0;	// タイマ１停止
				}
			}
			break;
		default: break;
	}
}
//////// A/D変換読み込み関数
byte AD_input(char chanl)
{
    	byte adData;        				// 8ビットモード
    	
	ADCON0 = 0x01 + (chanl * 4);
    	ADCON0bits.GO = 1;              	// Start AD conversion
    	while(ADCON0bits.NOT_DONE);     	// Wait for conversion
 	adData = (byte)ADRESH;				// 上位８ビットのみ取り出し
	return(adData);                    	// データ返し
}


/**********************************************************
 * メイン関数
 * 初期化後USB受信待ち、受信したコマンドに応じた処理を実行
 *********************************************************/
void main(void)
{
	int	i;
	
	ADCON1 = 0x07;             	// ポートA全アナログに設定
	ADCON2 = 0x16;				// Left,Tad＝Tcy/64 Taq=4Tad 
	ADCON0 = 0x1D;				// AD On	
	CMCON = 0x07;					// コンパレータオフ
	/// ポート初期化	
   	LATB = 0xFF;					// LEDオフ
 	TRISA = 0xFF;					// アナログ入力
	TRISB = 0x00;					// LED出力
 	TRISC = 0xB0;					// USB
 	// タイマ１初期化
 	T1CON = 0xB0;					// 内部クロック プリスケール1/8
 	TMR1H = 0;					// 基準 = 48MHz/4/8=1.5MHz
 	TMR1L = 0;
	T3CON = 0;					// CCP1一致でタイマ１リセット
 	// CCP1初期化
 	Period = 15;					// 10usec 単位とする
 	CCP1CON = 0x0B;				// 一致でタイマ１クリア
	CCPR1 = Period * 10;			// 10usec単位(10usec to 40.390msec)	
	/// USB初期化
	mInitializeUSBDriver();        	// See usbdrv.h
	/// 変数初期化
	State = 0;					// ステートフラグリセット
	Threshold = 190;				// トリガ」スレッショルド初期値
	Sample =10;					// サンプル周期初期値100usec
	FreeFlag = 1;					// フリーランフラグオン
	Channel = 0;					// チャネル番号0
	T1CONbits.TMR1ON = 0;			// タイマ初期停止
	// 割り込み許可
	RCONbits.IPEN = 0;			// 優先順位なし
 	PIE1bits.CCP1IE = 1;			// CCP1割り込み許可		
	INTCON = 0xC0;				// グローバル割り込み許可

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
				RedLED = !RedLED;								// 目印LED
	    			switch(input_buffer[0])						// 最初の１文字チェック
	    			{
		    			case '0':								// OKメッセージ返送
						State = 0;
        					if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("OK");
           			 	break;
           			case '1':								// データ収集開始コマンド
           				State = 0;							// ステート初期状態セット
           				T1CONbits.TMR1ON = 1;					// タイマ１スタート
           				Threshold = (byte)input_buffer[1];		// スレッショルド取得
           				Sample = (byte)input_buffer[2];			// サンプリング周期取得
           				CCPR1 = Period * Sample;				// 周期設定10usec単位
           				Channel = (byte)input_buffer[3];		// チャネル番号設定
	    					FreeFlag = 0;							// フリーランオフ           				
           				break;	
           			case '2':								// 状態問い合わせ
           				if(State == 2) {						// データ取得完了か？
        						if(mUSBUSARTIsTxTrfReady())	         					
           			 			putrsUSBUSART("1");			// 完了なら１を返す
           			 	}
           			 	else {
       						if(mUSBUSARTIsTxTrfReady())
           			 			putrsUSBUSART("0");			// 未完了なら０を返す
           			 	}
           			 	break;
		    			case '3':								// 計測データ送信
				  		if((Index < 512) && (State == 2)) {		// バッファAの送信
							/// 計測データの512バイトを転送する
						 	if(mUSBUSARTIsTxTrfReady())
           			 			mUSBUSARTTxRam(&BufferA[Index], 64);	// 送信実行
							Index += 64;
						}									
						else {								// バッファBの送信
							if((State == 2) && (Index < 960)) {
							 	if(mUSBUSARTIsTxTrfReady())
							 		mUSBUSARTTxRam(&BufferB[Index-512], 64);
								Index += 64;
							}
							else							// 送信完了でステートリセット
								State = 0;
						}
		    				break;
		    			case '4':								// フルーラン指定
           				State = 0;							// ステート初期状態セット
           				T1CONbits.TMR1ON = 1;					// タイマ１スタート
           				Threshold = (byte)input_buffer[1];		// スレッショルド取得
           				Sample = (byte)input_buffer[2];			// サンプリング周期取得
           				CCPR1 = Period * Sample;				// 周期設定10usec単位
            				Channel = (byte)input_buffer[3];		// チャネル番号設定          				
	    					FreeFlag = 1;							// フリーランオン          				
           				break;			    			
		    			default: break;
		    		}
    			}
    		}
     }
}

