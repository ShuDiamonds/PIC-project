/*********************************************************************
*
*　USB接続 データロガーシステムプログラム
*	USBフレームワーク修正情報
*　　コンフィギュレーションの設定変更
*　　main.c　の変更　初期化削除　IO設定変更
*　　io_cfg.hの変更　LED削除　IOモニタ削除
*　　user.cの変更　　実行内容削除
*　　usbcfg.hの変更　IOモニタ削除
*
*********************************************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                      // Required
#include "system\usb\usb.h"                       // Required
#include "io_cfg.h"                               // Required
#include "system\usb\usb_compile_time_validation.h"
#include	"delays.h"
#include "lcd_lib.h"
#include	"i2c.h"
#include "string.h"

//****変数定義　********************************************* */
#pragma udata
char input_buffer[64];						// USB入力バッファ
char output_buffer[64];						// USB出力バッファ

char LogFlag, MsrFlag;						// ログ,計測開始フラグ
char Period;									// ログ周期定数エリア
char PeriCounter;								// 周期用カウンタ
unsigned int Result;							// アナログ計測データ
float Battery;								// バッテリ電圧変数
char ErrFlag, FullFlag;						// エラー、バッファ一杯フラグ
unsigned int	 WRAdrs, RDAdrs;					// EEPROMアドレス


char MsgStart[] ="Start!";						// 初期メッセージ
char MsgLCD1[] = "C0=xxxx  C1=xxxx";			// LCDメッセージ
char MsgLCD2[] = "BT=x.xxV xxxxxxx";			// バッテリ電圧表示
char MsgFull[] = "Full!! ";					// メモリオーバーフロー
char MsgErase[] ="EraseEEPROM ";				// EEPROｍ消去コマンド
char MsgEnd[] = "End!";
char MsgLow[]  = "Low Bat";					// 電池電圧低下
char MsgPer[]  = "Pxx00ms";					// ログ周期表示
char MsgMsr[]  = "Mesure ";					// 計測開始
char MsgDmp[]  = "DumpAll";					// メモリ送信
char MsgErr[]  = "Cmd Err";					// コマンドエラー表示
char MsgTest[] = "EEPROMTest xxxx";				// テスト
char MsgTX[] = "CH0=xxxx  CH1=xxxx  BAT=xxxx\r\n";
unsigned char TXBuffer[64];					// USB送信バッファ

//** プロトタイピング *******************************/
unsigned int ADconv(char chan);
void SaveMem(unsigned int data);
void itostring(char digit, unsigned int data, char *buffer);
void ISRProcess(void);
void WriteMem(unsigned char data);
unsigned char ReadMem(unsigned int adrs);
void ftostring(int seisu, int shousu, float data, char *buffer);

////// 割り込みベクタ定義　/////
#pragma code high_vector=0x08
void interrupt_at_high_vector(void)
{
    _asm goto ISRProcess _endasm
}
#pragma code

////// タイマ0割り込み処理 100msec
#pragma interrupt ISRProcess
void ISRProcess(void)
{
	INTCONbits.T0IF = 0;				// Interrupt Flag Clear
	TMR0H = 0xB6;						// 12MHz/64/18750=10Hz
	TMR0L = 0xC1;	
	PeriCounter--;					// 周期カウンター１
	if(PeriCounter == 0)				// 周期一致
	{
		LogFlag = 1;					// ログ開始フラグオン
		PeriCounter = Period;			// 周期カウンタ再セット
	}
	if(PORTCbits.RC0 == 0)				// 開始、停止ボタンチェック
	{
		if(MsrFlag == 2)				// 開始済みなら停止
			MsrFlag = 0;
		else
			MsrFlag = 1;				// 開始オン
	}
}

#pragma code
/*********************************************************************
 * メイン処理
 ********************************************************************/
void main(void)
{
	unsigned char i;
	unsigned int L;
	
	// I/O設定
	CMCON = 0x07;						// コンパレータオフ
   	LATB = 0;							// 液晶表示ポートリセット
  	LATC  = 0xC0;						// LED off
 	TRISA = 0xFF;						// 全入力
	TRISB = 0x0;						// 液晶表示
 	TRISC = 0x31;						// LED Output Mode, SW input
 	// A/D初期設定
 	ADCON1 = 0x1A;					// AN0-AN2,AN4 Analog Input Vref+
 	ADCON2 = 0x95;					// 4Tad, Fosc/16
 	ADCON0 = 0x01;					// AD on
 	// タイマ初期化
	T0CON = 0x85;						// Int 1/64
	TMR0H = 0xB6;						// 12MHz/64/18750=10Hz
	TMR0L = 0xC1;
	/// I2C初期化
	OpenI2C(MASTER, SLEW_ON);
	SSPADD = 27;						//400kHz @48MHz
	/// USB初期化
	mInitializeUSBDriver();         	// See usbdrv.h
	/// 液晶表示器初期化
	lcd_init();						// LCD初期化
	lcd_str(MsgStart);				// 初期メッセージ表示
	Delay10KTCYx(500);				// 0.5秒
	/// 変数初期化
	Period = 10;
	PeriCounter = 10;
	WRAdrs = 0;
	RDAdrs = 0;
	LogFlag = 0;
	MsrFlag = 0;
	FullFlag = 0;
	// Enable Interrupt
	RCONbits.IPEN = 0;				// No Priority
	INTCON = 0xA0;					// Enable Interrupt
	
    /// メインループ
    while(1)
    {
    		USBCheckBusStatus();        						// USB接続チェック
    		if(UCFGbits.UTEYE!=1)								// アイパターンモードオフか
    			USBDriverService();							// USBイベントポーリング
		CDCTxService();									// クラス送受信実行
		/// 送受信データの処理実行
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
    			if(getsUSBUSART(input_buffer,64))				// データ受信ポール
    			{
            		LATCbits.LATC7 = !LATCbits.LATC7;			// LED反転
	    			switch(input_buffer[0])					// 最初の１文字チェック
	    			{
		    			case '0':	// 初期設定とオープン
						RDAdrs = 0;						// アドレス初期値セット
						WRAdrs = 0;
						MsrFlag = 0;						// 計測停止		    				
           			    	if(mUSBUSARTIsTxTrfReady())		// 送信可能か？
        		   			 	putrsUSBUSART("OK");			// OKデータ送信
        		   			 break;         			 	
		    			case '1':	// 周期設定
						Period = input_buffer[1];			// 周期データ取得
						strcpy(MsgLCD2+9, MsgPer);			// 周期表示
						itostring(2, Period, MsgLCD2+10);
						lcd_cmd(0xC0);
						lcd_str(MsgLCD2);
           			 	break;
           			case '2':	// データ直接送信
 						lcd_cmd(0xC0);
						strcpy(MsgLCD2+9, MsgMsr);			// 計測開始表示
						lcd_str(MsgLCD2);	          			
           			    	if(mUSBUSARTIsTxTrfReady())		// 送信可能か？
        					{
		    					Result = ADconv(0);			// CH0測定
		    					itostring(4, Result, MsgTX+4);
	           			 	Result = ADconv(1);			// CH1測定
    		       			 	itostring(4, Result, MsgTX+14);
	           			 	Result = ADconv(4);			// CH4測定
     	      			 	itostring(4, Result, MsgTX+24);
     	      			 	
        		   			 	mUSBUSARTTxRam((byte*)(MsgTX), 30);	// データ送信
         		   		}
           				break;
           			case '3':	// メモリダンプ
  						lcd_cmd(0xC0);
						strcpy(MsgLCD2+9, MsgDmp);			// メモリダンプ表示
						lcd_str(MsgLCD2);	          			
       					if(mUSBUSARTIsTxTrfReady())		// 送信可能か 
	       				{
							for(i=0; i<64; i++)			// 64バイト分読み出し
							{
         						TXBuffer[i] = ReadMem(RDAdrs);	// 現在アドレスのデータ
        							RDAdrs++;				// アドレス＋１
        						}
							mUSBUSARTTxRam(TXBuffer, 64);	// データ送信
						}
						break;
					case '4':	// メモリReadテスト
       					if(mUSBUSARTIsTxTrfReady())		// 送信可能か
						{
							lcd_clear();
							itostring(4, RDAdrs, MsgTest+11);
							lcd_str(MsgTest);				// 現在アドレス表示
							for(i=0; i<64; i++)			// 64バイト読み出し
							{
								TXBuffer[i] = ReadMem(RDAdrs);	// ASCIIコードとして表示
								RDAdrs ++;
							}
							mUSBUSARTTxRam(TXBuffer, 64);	// データ送信	
						}
						break;
					case '5':	// メモリWriteテスト実行
						lcd_clear();
						itostring(4, WRAdrs, MsgTest+11);	// 書き込みテスト表示
						lcd_str(MsgTest);
						for(i=0; i<64; i++)				// 64バイト書き込み
						{
							WriteMem(i+0x20);				// ASCIIコード書き込み
							WRAdrs++;
						}
						break;
					case '6':	// メモリ全消去　長時間
						lcd_clear();
						lcd_str(MsgErase);				// EERPOM消去メッセージ
						for(L=0; L<0x7FFF; L++)			// １チップ目32kバイト
						{
							WriteMem(0xFF);				// FFで消去
							WRAdrs++;
						}
						lcd_str(MsgEnd);					// ２チップ目32kバイト
						for(L=0; L<0x7FFF; L++)
						{
							WriteMem(0xFF);				// FFで消去
							WRAdrs++;
						}
						lcd_str(MsgEnd);					// 終了メッセージ
						WRAdrs = 0;
						break;
					case '7':	// 計測開始/停止
					 	if(MsrFlag == 2)
					 		MsrFlag = 0;					// 開始済みなら停止
					 	else
						 	MsrFlag = 1;					// 開始
					 	break;
		    			default:
  						lcd_cmd(0xC0);					// エラーコマンド表示
						strcpy(MsgLCD2+9, MsgErr);
						lcd_str(MsgLCD2);
						RDAdrs = 0;						// アドレス初期値セット
						WRAdrs = 0;
		    			 	break;
		    		}
    			}
    		}
		//// ログ実行処理　一定周期
  		if((LogFlag == 1) && (MsrFlag != 0))				// ログフラグオンか？
    		{	    		
	    		LATCbits.LATC6 = !LATCbits.LATC6;				// LED反転
	    		LogFlag = 0;									// １回のみでクリア
	    		// Mesure CH0
			Result = ADconv(0);							// CH0計測
			SaveMem(Result);								// メモリに保存
			itostring(4, Result, MsgLCD1+3);				// LCD表示
			// Mesure CH1
			Result = ADconv(1);							// CH1計測
			SaveMem(Result);								// メモリに保存
			itostring(4, Result, MsgLCD1+12);				// LCD表示
			lcd_clear();
			lcd_str(MsgLCD1);
		    	Result = ADconv(4);							// CH4計測
		    	SaveMem(Result);								// メモリに保存
			Battery = (Result*5.00)/1024.0;					// 電圧値に変換
		    	ftostring(1, 2, Battery, MsgLCD2+3);		  		// LCD表示
		    	lcd_cmd(0xC0);
			lcd_str(MsgLCD2);
			// バッテリ低電圧検出(3V以下で停止) USBバスパワーでないとき		

/*			if((PORTCbits.RC1 == 0) && (Result < 614))		// 低電圧チェック
			{
				lcd_cmd(0xC0);
				strcpy(MsgLCD2+9, MsgLow);
				lcd_str(MsgLCD2);
	    			_asm sleep _endasm						// Then sleep
	    		}
*/
	    		SaveMem(0xFFFF);								// 終了フラグ保存	
	    		WRAdrs = WRAdrs - 2;							// アドレスを戻す
			if(WRAdrs > 0xFFFA)							// 最終アドレスか？
			{
				WRAdrs = 0;								// 終了、アドレス0に
				lcd_cmd(0xC0);
				strcpy(MsgLCD2+9, MsgFull);				// LCD表示
				lcd_str(MsgLCD2);
			}
			MsrFlag = 2;									// 計測開始すみ 
	    }
     }
}

/// アナログデータ入力
unsigned int ADconv(char chan)
{
	unsigned int data;
	 
	ADCON0 = (chan << 2) + 0x01;			// チャネル選択
	Delay10TCYx(20);	    					// アクイジションタイム
	ADCON0bits.GO = 1;					// 変換開始
	while(ADCON0bits.GO);					// 変換終了待ち
	data = ADRESH * 256 + ADRESL;			// データ入力
	return(data);					
}
/// EEPROM２バイト保存
void SaveMem(unsigned int data)
{
	WriteMem(data >> 8);					// 上位保存
	WRAdrs++;
	WriteMem(data & 0xFF);					// 下位保存
	WRAdrs++;
}

/// EEPROMバイト書き込み
void WriteMem(unsigned char data)
{
	unsigned char Err, Chip;
	
	if(WRAdrs < 0x8000)					// チップ選択
		Chip = 0xA0;
	else
		Chip = 0xA2;
	///
	IdleI2C();
	StartI2C();							// スタートシーケンス
	while ( SSPCON2bits.SEN );
	WriteI2C(Chip);						// コントロール
	IdleI2C();
	WriteI2C((WRAdrs >> 8) & 0x7F);			// アドレス上位
	IdleI2C();
	WriteI2C(WRAdrs);						// アドレス下位
	IdleI2C();
	WriteI2C(data);						// データ出力
	IdleI2C();
	StopI2C();							// ストップシーケンス
	while ( SSPCON2bits.PEN );
	Delay10KTCYx(5);						// 5msec待ち
}
//// EEPROMバイト読み出し
unsigned char ReadMem(unsigned int adrs)
{
	unsigned char data, Chip;

	if(adrs < 0x8000)						// チップ選択
		Chip = 0xA0;
	else
		Chip = 0xA2;
	////
	IdleI2C();
	StartI2C();							// スタートシーケンス
	while ( SSPCON2bits.SEN );
	WriteI2C(Chip);						// コントロール
	IdleI2C();
	WriteI2C((adrs >> 8) & 0x7F);			// アドレス上位
	IdleI2C();
	WriteI2C(adrs);						// アドレス下位
	IdleI2C();
	RestartI2C();							// リスタートシーケンス
	while ( SSPCON2bits.RSEN );
	WriteI2C(Chip + 0x01);					// リードコントロール
	IdleI2C();
	SSPCON2bits.RCEN = 1;
	while ( SSPCON2bits.RCEN ); 			// データ待ち
	NotAckI2C();              				// NAK送信
	while ( SSPCON2bits.ACKEN );  
	StopI2C();              				// ストップシーケンス
	while ( SSPCON2bits.PEN );  
	return(SSPBUF);						// データ取り出し
}
///// 数値から文字列に変換
void itostring(char digit, unsigned int data, char *buffer)
{
	char i;
	buffer += digit;						// 文字列の最後
	for(i=digit; i>0; i--) {				// 最下位桁から上位へ
		buffer--;						// ポインター１
		*buffer = (data % 10) + '0';		// その桁数値を文字にして格納
		data = data / 10;					// 桁-1
	}
}
////// Floatから文字列へ変換
//////　合計有効桁は５桁以下とすること
void ftostring(int seisu, int shousu, float data, char *buffer)
{
	int i, dumy;

	if(shousu != 0)						//小数部桁ありか
		buffer += seisu+shousu+1;			//全体桁数＋小数点
	else								//小数部桁なしのとき
		buffer += seisu + shousu;			//全体桁数のみ
	buffer--;							//配列ポインタ-1
	for(i=0; i<shousu; i++)				//小数部を整数に変換
		data = data * 10;					//１０倍
	dumy = (int) (data + 0.5);				//四捨五入して整数に変換
	for(i=shousu; i>0; i--)	{			//小数桁数分繰り返し
		*buffer =(dumy % 10)+'0';			//数値を文字に変換格納
		buffer--;						//格納場所下位から上位へ
		dumy /= 10;						//次の桁へ
	}
	if(shousu != 0) {						//小数桁0なら小数点なし
		*buffer = '.';					//小数点を格納
		buffer--;						//ポインタ-1
	}
	for(i=seisu; i>0; i--) {				//整数桁分繰り返し
		*buffer = (dumy % 10)+'0';			//数値を文字に変換格納
		buffer--;						//ポインタ-1
		dumy /= 10;						//次の桁へ
	}
}    
