/**************************************************
*  リモートスピーカシステム
*　ICMP + ARP + DHCP + NBNS + UDPアプリ
*  プロジェクト名：LANSpeak
**************************************************/
// 宣言とヘッダファイルのインクルード
#include "TCPIP Stack/TCPIP.h"

// スタック内で使う変数でメインで定義しておく
APP_CONFIG AppConfig;
BYTE myDHCPBindCount = 0xFF;
#if !defined(STACK_USE_DHCP_CLIENT)
	#define DHCPBindCount	(1)
#endif

static  char AppMode __attribute__((far));
static char SW_Flag, Full_Flag, First __attribute__((far));
static int BufIndex;
const BYTE SinData[20] = {				// テスト用正弦波データ
	127,202,248,247,200,125,50,5,
	7,56,132,206,249,246,196,120,
	46,4,9,60
};
#define MaxBuf 800
#pragma udata PINPON = 0x300
static BYTE BufferA[MaxBuf] __attribute__((far));
static BYTE BufferB[MaxBuf] __attribute__((far));
static BYTE TempBuffer[MaxBuf+10] __attribute__((far));

// 関数プロトタイピング
static void InitAppConfig(void);
static void InitializeBoard(void);
static void DisplayIPValue(IP_ADDR IPVal);
void UDPControlTask(void);
void SpeakerControl(void);

/**********************************************
* 割り込み処理
***********************************************/
// 低位割り込み（インターバルタイマ）
#pragma interruptlow LowISR
void LowISR(void){
    TickUpdate();
}
#pragma code lowVector=0x18
void LowVector(void){_asm goto LowISR _endasm}

// 高位割り込み（タイマ1）
#pragma code 		// Return to default code section
#pragma interrupt HighISR
void HighISR(void){
	SpeakerControl();
}
#pragma code highVector=0x8
void HighVector(void){_asm goto HighISR _endasm}

#pragma code 		// Return to default code section
/************************************************
*  タイマ１割り込み処理
*  0.再生終了、タイマ1停止
*  1.テストモード　正弦波出力
*  2.A/D入力し録音
*  3.受信データ再生
*  4.折り返しテスト
************************************************/
void SpeakerControl(void){
	LED1_IO = 1;								// 目印　125usec
   	TMR1H = 0xFB;								// タイマ1再セット
   	TMR1L = 0xEA;								// 8kHzになるよう微調整
	/***** モードにより処理分岐　*****/
	switch(AppMode) {
		case 0: 	break;						// 再生終了
		case 1:								// テストモード
			CCPR1L = SinData[BufIndex++];		// 正弦波出力
			if(BufIndex >= 20)
				BufIndex = 0;					// 繰り返し
			break;
		case 2:								// 再生モード
			if(SW_Flag) {						// バッファ交互切り替え
				CCPR1L = BufferA[BufIndex++];	// 
				LED2_IO = 1;					// 目印バッファ切り替え
				if(BufIndex >= MaxBuf){		// 終わりまで進んだら切り替え
					BufIndex = 0;				// バッファインデックスクリア
					Full_Flag = 1;			// バッファ空、次のデータ要求
					SW_Flag = 0;				// バッファ切り替え
				}
			}
			else {
				LED2_IO = 0;					// 目印バッファ切り替え
				CCPR1L = BufferB[BufIndex++];	
				if(BufIndex >= MaxBuf){		// 終わりまで進んだら切り替え
					BufIndex = 0;				// バッファインでクスクリア
					Full_Flag = 1;			// バッファ空、次のデータ要求
					SW_Flag = 1;				// バッファ切り替え
				}
			}
			break;
		case 3:								// 録音モードのとき
			if(SW_Flag)						// バッファ交互
				BufferA[BufIndex++] = ADRESH;
			else
				BufferB[BufIndex++] = ADRESH;
			if(BufIndex >= MaxBuf){			// バッファ一杯か
				BufIndex = 0;
				SW_Flag = !SW_Flag;			// バッファ切り替え
				Full_Flag = 1;				// バッファ一杯、送信実行
			}
			ADCON0 = 0x0B;					// 次のA/D変換スタート	
			break;
		case 4:								// ローカル折り返しモード
			CCPR1L = ADRESH;					// 入力データをPWMへ出力
			ADCON0 = 0x0B;					// 次のA/D変換スタート
			break;
		default: break;
	}
	PIR1bits.TMR1IF = 0;						// タイマ１割り込みフラグクリア
	LED1_IO = 0;								// 目印　タイマ１処理時間チェック
}


/************************************************
*  メイン関数
************************************************/
void main(void)
{
    static TICK t = 0;
	BYTE i;

    // ハードウェア初期化
    InitializeBoard();
	// アプリケーション変数初期化
	AppMode = 1;								// 初期値をテストとする
	First = 0;								// 最初フラグオン
	BufIndex = 0;								// バッファインデックスクリア
	// LCDの初期化と初期表示
	LCDInit();
	strcpypgm2ram((char*)LCDText, "TCPStack " VERSION "                  ");
	LCDUpdate();								// LCD表示出力
    // インターバルタイマ初期化
    TickInit();
    // アプリケーションの初期化（変数の初期化）
    InitAppConfig();
	// スタック初期化(MAC, ARP, TCP, UDP)
    StackInit();
	/********** メインループ  ********************/
    while(1)
    {
		// LED0の点滅（1秒間隔）
		if(TickGet() - t >= TICK_SECOND/2ul)
        	{
            	t = TickGet();
            	LED0_IO ^= 1;						// LED反転
        	}
		// スタックの送受信実行タスク（一定時間内に実行必須）
        	StackTask();
		// NBNSによる名前解決
		NBNSTask();
        // UDP送受信デモ
		UDPControlTask();						// (新規追加)
        // デフォルトIPアドレスのLCDへの表示
        if(DHCPBindCount != myDHCPBindCount)		// 変わったときだけ表示
        {
            myDHCPBindCount = DHCPBindCount;
            DisplayIPValue(AppConfig.MyIPAddr);	// IPアドレスLCD表示
        }
    }
}
/***************************************************
*  IPアドレスのLCDへの表示関数
*   ***.***.***.***形式　（ゼロサプレス）
**************************************************/
static void DisplayIPValue(IP_ADDR IPVal)
{
    BYTE IPDigit[4];							// ローカル変数
	BYTE i;
	BYTE j;
	BYTE LCDPos=16;
	// 表示実行
	for(i = 0; i < sizeof(IP_ADDR); i++)
	{
	    uitoa((WORD)IPVal.v[i], IPDigit);		// IP1桁の文字変換
		for(j = 0; j < strlen((char*)IPDigit); j++)
		{
			LCDText[LCDPos++] = IPDigit[j];		// バッファに格納
		}
		if(i == sizeof(IP_ADDR)-1)
			break;
		LCDText[LCDPos++] = '.';				// ドットの格納
	}
	if(LCDPos < 32)							// 32文字以下で表示
		LCDText[LCDPos] = 0;
	LCDUpdate();								// 表示実行
}

/**********************************************
*  リモートスピーカ アプリケーション
*  コマンド受信でデータ送受信開始、終了
**********************************************/
#define REMOTE_PORT	10001						// UDPのポート番号の指定
#define LOCAL_PORT	10002
const BYTE MD[] = "MDOK";							// 返送メッセージ
const BYTE END[] = "END";

void UDPControlTask(void)
{
	/// ローカル変数の定義
	static enum {								// ステートマシンの定義	
		CONT_IDLE = 0,
		CONT_LISTEN,
		CONT_EXEC
	} ContSM = CONT_IDLE;	
	static UDP_SOCKET		MySocket;				// 受信ソケットを維持する	
	unsigned int RcvLen;	

	/***** 関数実行部　ステートマシンにより進行  ******/
	switch(ContSM){
		case CONT_IDLE:		/**** アイドルから受信待ちへ *****/
			MySocket = UDPOpen(LOCAL_PORT, NULL, REMOTE_PORT);
			if(MySocket == INVALID_UDP_SOCKET)
				return;									// オープン失敗リターン
			else
				ContSM++;								// オープン成功受信待ちへ
			break;
		case CONT_LISTEN:		/****** 受信　*****/
			if(!UDPIsGetReady(MySocket)){					// 受信有無チェック
				return;									// 受信なし即リターン
			}
			RcvLen = UDPGetArray(TempBuffer, sizeof(TempBuffer));	// 受信データ取得
			UDPDiscard();									// UDPソケット切り離し
    			ContSM++;									// ステータス更新返送へ
			/// 受信データ キーワードチェック
			if(!(TempBuffer[0] == 'S')) {					// 開始キー
				ContSM = CONT_LISTEN;						// キーワード不一致
				return;									// Listenに戻る
			}			
			else{			/******* データ処理  *****/
				// タイマ停止、再開制御
				if(TempBuffer[1] == 'E')					// 受信終了コマンドチェック
					T1CONbits.TMR1ON = 0;					// タイマ1停止
				else
					T1CONbits.TMR1ON = 1;					// タイマ１再スタート
				// 受信コマンド解析
				switch(TempBuffer[1])	{					// コマンド取り出し
					case 'T':							// テストモードコマンド
						AppMode = 1;						// テストモードに切り替え
						BufIndex = 0;
						First = 0;
						break;
					case 'O':	/**** 音声データ受信(再生）  ****/
						AppMode = 2;						// 受信モードに切り替え
						if(!First){						// 最初の場合チェック
							BufIndex = 0;					// 最初のときだけクリア
							memcpy((void *)BufferA, (void *)TempBuffer+2, MaxBuf);							
							SW_Flag = 1;					// 交互フラグセット
							Full_Flag = 0;				// 直ぐ次のデータ要求
							First = 1;					// 最初フラグセット
						}
						else{							// 2回目以降
							if(SW_Flag)					// バッファに交互に格納
								memcpy((void *)BufferB, (void *)TempBuffer+2, MaxBuf);
							else
								memcpy((void *)BufferA, (void *)TempBuffer+2, MaxBuf);
						}
						break;
					case 'I':	/*** 音声データ送信(録音） ***/						
						AppMode = 3;						// 送信モードに切り替え
						BufIndex = 0;						// 送信準備クリア
						First = 0;
						SW_Flag = 1;
						Full_Flag = 0;
						break;
					case 'M':	/**** 折り返しテストモード ****/
						AppMode = 4;						// 折り返しモードに切り替え
						First = 0;
						break;
					case 'E':	/**** 再生終了コマンド *****/
						AppMode = 0;						// 受信終了モードへ
						First = 0;
					default: break;			
 				}
			}
			break;
		case CONT_EXEC:		/****** 応答送信  *****/
			if(!UDPIsPutReady(MySocket)){					// 送信レディーか？ 
				return;		
			}
			else {
				switch(AppMode){		// モードにより処理切り替え
					case 0:
						First = 0;						// 受信終了初期化
						SW_Flag =0;						// バッファ交互フラグクリア
						Full_Flag = 0;					// バッファ一杯、空フラグクリア
						BufIndex = 0;						// バッファインデックスクリア
						ContSM = CONT_LISTEN;				// ステートを受信待ちへ
						break;
					case 1:			// テストモード応答
					case 4:			// 折り返しテストモード応答
						UDPPutString((BYTE*)MD);			// 応答送信
						UDPPut(TempBuffer[1]);
						UDPPut(0);
						UDPFlush();						// UDP送信実行
						ContSM = CONT_LISTEN;				// ステート受信待ちへ	
						break;
					case 2:			// 音声データ受信（再生）
						if(Full_Flag)	{					// バッファ空か?
							Full_Flag = 0;				// 空フラグクリア
							UDPPutString((BYTE*)MD);		// 次のデータ請求
							UDPPut('O');
							UDPPut(0);
							UDPFlush();					// UDP送信実行
							ContSM = CONT_LISTEN;			// ステートを受信待ちへ
						}
						break;
					case 3:			// 音声データ送信(録音）
						if(BUTTON1_IO == 0){				// ボタン１が押されたら終了
							First = 0;					// 最初フラグセット
							UDPPutString((BYTE*)END);		// END応答
							UDPPut(0);
							UDPFlush();					// UDP送信実行
							ContSM = CONT_LISTEN;			// ステートを戻す	
						}
						else {		// 音声データ送信実行（連続送信実行)
							/// データ送信
							if(Full_Flag){				// バッファ一杯を待つ
								Full_Flag = 0;			// 一杯フラグクリア
								if(SW_Flag){				// どちらかのバッファ送信
									LED3_IO = 1;			// 目印
									UDPPutArray(BufferB, MaxBuf);
								}
								else{
									LED3_IO = 0;			// 目印
									UDPPutArray(BufferA, MaxBuf);
								}
								UDPPut(0);				// 送信実行
								UDPFlush();
							}
						}
						break;
					default: break;
				}
			}
		default: break;
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
    // Enable internal PORTB pull-ups
    INTCON2bits.RBPU = 0;

	// タイマ１の初期化関数
    TMR1H = 0xFB;  		  	// Initialize the time for 8kHz
    TMR1L = 0xFA;				// 8kHz=125usec
	IPR1bits.TMR1IP = 1;		// High priority
    	PIR1bits.TMR1IF = 0;
    	PIE1bits.TMR1IE = 1;		// Enable interrupt
    T1CON = 0x81;   			 // On, 16-bit, internal, 1:1
	
	// ECCP1モジュールの初期化関数
	RCS1_TRIS = 0;			// Output Mode
	TMR2 = 0;				// Timer2 initialize
	T2CON = 0x04;				// postscale 1:1 prescale 1:1 enable
	PR2 = 0xFF;				// 41kHz 10bit resolution
	// ECCP1 initialize Duty low 2bits always 0
	T3CONbits.T3CCP2 = 0;		// select timer2 for timebase
	T3CONbits.T3CCP1 = 0;
	CCP1CON = 0x0F;			// single out on P1A, Active High
	CCPR1L = 0x80;			// Duty always 50%

	// A/Dコンバータの初期化
	ADCON1 = 0x0B;			// AN0 to AN3 VSS-VDD
	ADCON2 = 0x22;			// Right, 8TAD, FOSC/32
	ADCON0 = 0x0B;			// Select AN2 StartA/D

	// Enable Interrupts
	RCONbits.IPEN = 1;		// Enable interrupt priorities
    INTCONbits.GIEH = 1;
    INTCONbits.GIEL = 1;
}

/*******************************************************************
* アプリケーションの初期化関数
*
********************************************************************/
// MACアドレスの初期設定
static ROM BYTE SerializedMACAddress[6] = {MY_DEFAULT_MAC_BYTE1, MY_DEFAULT_MAC_BYTE2, 
		MY_DEFAULT_MAC_BYTE3, MY_DEFAULT_MAC_BYTE4, MY_DEFAULT_MAC_BYTE5, MY_DEFAULT_MAC_BYTE6};
// IPアドレスその他の初期設定
static void InitAppConfig(void)
{
	AppConfig.Flags.bIsDHCPEnabled = TRUE;
	AppConfig.Flags.bInConfigMode = TRUE;
	memcpypgm2ram((void*)&AppConfig.MyMACAddr, (ROM void*)SerializedMACAddress, 
				sizeof(AppConfig.MyMACAddr));
	AppConfig.MyIPAddr.Val = MY_DEFAULT_IP_ADDR_BYTE1 | MY_DEFAULT_IP_ADDR_BYTE2<<8ul 
					| MY_DEFAULT_IP_ADDR_BYTE3<<16ul | MY_DEFAULT_IP_ADDR_BYTE4<<24ul;
	AppConfig.DefaultIPAddr.Val = AppConfig.MyIPAddr.Val;
	AppConfig.MyMask.Val = MY_DEFAULT_MASK_BYTE1 | MY_DEFAULT_MASK_BYTE2<<8ul 
					| MY_DEFAULT_MASK_BYTE3<<16ul | MY_DEFAULT_MASK_BYTE4<<24ul;
	AppConfig.DefaultMask.Val = AppConfig.MyMask.Val;
	AppConfig.MyGateway.Val = MY_DEFAULT_GATE_BYTE1 | MY_DEFAULT_GATE_BYTE2<<8ul 
					| MY_DEFAULT_GATE_BYTE3<<16ul | MY_DEFAULT_GATE_BYTE4<<24ul;
	AppConfig.PrimaryDNSServer.Val = MY_DEFAULT_PRIMARY_DNS_BYTE1 | MY_DEFAULT_PRIMARY_DNS_BYTE2<<8ul  
					| MY_DEFAULT_PRIMARY_DNS_BYTE3<<16ul  | MY_DEFAULT_PRIMARY_DNS_BYTE4<<24ul;
	AppConfig.SecondaryDNSServer.Val = MY_DEFAULT_SECONDARY_DNS_BYTE1 | MY_DEFAULT_SECONDARY_DNS_BYTE2<<8ul  
					| MY_DEFAULT_SECONDARY_DNS_BYTE3<<16ul  | MY_DEFAULT_SECONDARY_DNS_BYTE4<<24ul;

	// Load the default NetBIOS Host Name
	memcpypgm2ram(AppConfig.NetBIOSName, (ROM void*)MY_DEFAULT_HOST_NAME, 16);
	FormatNetBIOSName(AppConfig.NetBIOSName);
}












