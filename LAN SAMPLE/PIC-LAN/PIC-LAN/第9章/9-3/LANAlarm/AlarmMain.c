/*************************************************************
*  アラームメールシステム
*　ICMP + ARP + DHCP Client + NBNS +UDP + TCP + SMTP + DNS
*  プロジェクト名：LANAlarm
*************************************************************/
// 宣言とヘッダファイルのインクルード
#include "TCPIP Stack/TCPIP.h"

// スタック内で使う変数でメインで定義しておく
APP_CONFIG AppConfig;
BYTE myDHCPBindCount = 0xFF;
#if !defined(STACK_USE_DHCP_CLIENT)
	#define DHCPBindCount	(1)
#endif
BYTE OldValue;
int	Result, Flag;
int Limit = 0x0200;			// Analog limit

// 関数プロトタイピング
static void InitAppConfig(void);
static void InitializeBoard(void);
static void DisplayIPValue(IP_ADDR IPVal);
void TCPControlTask(void);
static void AlarmTask(void);

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
	char Dumy[10];

    // ハードウェア初期化
    InitializeBoard();
	OldValue = ~PORTC;						// 状態変化検出用
	Flag = 0;								// 計測上限オーバフラグ
	Limit = 1000;								// 計測上限値初期値
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
		// LED0の点滅と計測値のLCD表示（0.5秒間隔）
		if(TickGet() - t >= TICK_SECOND/2ul)
        	{
            	t = TickGet();
            	LED0_IO ^= 1;						// LED反転	
			// 計測値のLCDへの表示
			memset(LCDText, ' ', 16);			// 1行目クリア
			LCDText[0] = 'M';					// 見出し
			LCDText[8] = 'L';
			itoa(Result, Dumy);				// 現在値
			for(i=0; i< strlen(Dumy); i++)		
				LCDText[i+2] = Dumy[i];		// 文字変換しセット
			itoa(Limit, Dumy);				// 上限値
			for(i=0; i< strlen(Dumy); i++)
				LCDText[i+10] = Dumy[i];		// 文字変換しセット
			LCDUpdate();						// LCD表示
      	}
		// スタックの送受信実行タスク（一定時間内に実行必須）
        	StackTask();
		// NBNSによる名前解決	
		NBNSTask();
		// SMTPによるアラームメールシステム
		SMTPTask();
        	// TCP送受信処理 ユーティリティ			// メッセージ設定
		TCPControlTask();
		AlarmTask();							// アラーム監視
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


/***************************************************
*  アラームメール送信アプリケーション
**************************************************/
// メッセージ（最大半角60文字まで)
#pragma idata MESSAGE = 0x700
BYTE AlmMsg1[64] = "Switch1 Alarm generated!";
BYTE AlmMsg2[64] = "Switch2 Alarm generated!";
BYTE AlmMsg3[64] = "Switch 3 Major Alarm generated.";
BYTE AlmMsg4[64] = "Switch 4 Major Alarm generated.";
BYTE AlmMsg5[64] = "Analog Upper Limit Over";
BYTE RAMStringTo[64] = "who@picfun.com";
BYTE RAMStringServer[64] = "smtp.xxxx.xxxx.ne.jp";
BYTE RAMStringSubject[64] = "Alarm Notification!";

#pragma code
static void AlarmTask(void)
{
	BYTE Mask, Dumy;
	static TICK WaitTime;
	static 	BYTE RAMStringBody[80];
	static enum	{							// ステート変数の定義
		MAIL_HOME = 0,
		MAIL_BEGIN,
		MAIL_SMTP_FINISHING,
		MAIL_DONE
	} MailState = MAIL_HOME;
	//  アラーム送信					    
	switch(MailState)							// ステートにより進行
	{
		case MAIL_HOME:						// 入力変化があったら送信
			Dumy = ~PORTC;					// 状態変化検出
			Mask = Dumy ^ OldValue;			// 前回と同じかチェック
			OldValue = Dumy;					// 状態更新
			Mask = Mask & OldValue;			// Low入力の場合だけアラーム
			if(Mask != 0){					// 1個でもLowありか//
				LED1_IO = 1;					// 目印LED
				MailState++;
				LED2_IO = 0;	
				// ビットに合わせてメッセージ選択			
				if(Mask & 0x02)		
					strcpy(RAMStringBody, AlmMsg1);
				if(Mask & 0x04)
					strcpy(RAMStringBody, AlmMsg2);	
				if(Mask & 0x040)
					strcpy(RAMStringBody, AlmMsg3);								
				if(Mask & 0x080)
					strcpy(RAMStringBody, AlmMsg4);	
			}
			if(Mask == 0){
				ADCON0 = 0;
				ADCON1 = 0b00001011;					// A/D設定
				ADCON2 = 0b10111110;
				TRISA = 0x2C;
				ADCON0 = 0b00001101;					// Channel 3
	  			ADCON0bits.GO = 1;					// A/D変換開始	
	  			while(ADCON0bits.GO);					// A/D変換終了待ち
				Result = ADRESH * 256 + ADRESL;			// 10ビットに変換
				if(Result < Limit - 0x20)				// アラーム復旧
					Flag = 0;						// フラグオフ
	  			else {
					if((Flag == 0) &&(Result > Limit)){	// 上限オーバチェック
						Flag = 1;					// 1回だけ送信
						LED1_IO = 1;
						MailState = MAIL_BEGIN;
						LED2_IO = 0;
						strcpy(RAMStringBody, AlmMsg5);	// 警報メッセージ
					}
				}
			}
			break;
		case MAIL_BEGIN:				// メール送信メッセージ準備
			if(SMTPBeginUsage()) {
				SMTPClient.Server.szRAM = RAMStringServer;
				SMTPClient.To.szRAM = RAMStringTo;
				SMTPClient.From.szROM = (ROM BYTE*)"\"PIC-UIO Alarm System\"";
				SMTPClient.ROMPointers.From = 1;				
				SMTPClient.Subject.szRAM = RAMStringSubject;
				SMTPClient.Body.szRAM = RAMStringBody;
				SMTPSendMail();
				MailState++;
			}
			break;
		case MAIL_SMTP_FINISHING:
			if(!SMTPIsBusy())	{
				LED1_IO = 0;
				MailState++;
				WaitTime = TickGet();
				SMTPEndUsage();
				LED2_IO = (SMTPEndUsage() == SMTP_SUCCESS);
			}
			break;
		case MAIL_DONE:
			if(TickGet() - WaitTime > TICK_SECOND)
				MailState = MAIL_HOME;
			break;
		default: break;
	}
}

/**********************************************
*  アラームユーティリティ処理部（TCP)
**********************************************/
#define LOCAL_PORT	50001
const BYTE MD[] = "MDOK";									// 返送メッセージ

void TCPControlTask(void)
{
	//// ステートマシンの定義	
	static enum {
		SM_IDLE = 0,			// 初期状態
		SM_LISTEN_WAIT,		// 接続待ち
		SM_CONNECTED,			// 接続中
		SM_EXEC,				// 送受信実行中
		SM_SEND_WAIT,			// 送信待ち中
		SM_DISCONNECT			// 接続切り離し要求
	} ContSM = SM_IDLE;
	/***** ローカル変数の定義  ****/
	static TCP_SOCKET	 MySocket = INVALID_SOCKET;			// 受信ソケットを維持する	
	static char ANString[8];								// アナログ入力変換バッファ
	static unsigned int  RcvLen;
	static BYTE	buffer[80];								// 受信データ格納バッファ
	/**** 関数実行部　ステートマシンにより進行 *****/
	switch(ContSM)	{
		case SM_IDLE:		/****  アイドルからListenへ ****/
			MySocket = TCPListen(LOCAL_PORT);				// Listen状態にする
			if(MySocket == INVALID_SOCKET)
				return;									// オープン失敗なら繰り返し
			else
				ContSM = SM_LISTEN_WAIT;					// LISTENへ移行
			break;
		case SM_LISTEN_WAIT:	/***** LISTEN 接続待ち *******/
			LED3_IO = 1;									// LED3オン
			if(TCPIsConnected(MySocket))					// 
				ContSM = SM_CONNECTED;						// 受信待ちへ移行
			break;;
		case SM_CONNECTED:	/***** Establish 受信 ****/
			LED3_IO = 0;									// LED3オフ
			if(!TCPIsConnected(MySocket))					// 切り離された場合
				ContSM = SM_LISTEN_WAIT;					// LISTEN WAITに戻る	
			if(!TCPIsGetReady(MySocket))					// 受信データありか？
				return;									// 無ければ即リターン
			RcvLen =TCPGetArray(MySocket, buffer, sizeof(buffer));// 受信データ取得
			TCPDiscard(MySocket);							// 受信ソケット開放
    			ContSM = SM_EXEC;								// 送信へ移行
			/// 受信データ キーワードチェック
			if(!(buffer[0] == 'S')) {						// 開始キー
				ContSM = SM_CONNECTED;						// キーワード不一致
				break;;									// Listenに戻る
			}			
			else{		// キーワード一致　データ処理開始
				if(RcvLen > 66){
					buffer[66] = 0;						// 長さ制限最大63文字
					RcvLen = 66;
				}
				// メッセージ振り分け
				switch(buffer[1])	{						// コマンド取り出し
					case 'A':							// サーバアドレス
//						memcpy((void *)RAMStringServer, (void *)&buffer[2], RcvLen-2);
						break;
					case 'B':							// 宛先
//						memcpy((void *)RAMStringTo, (void *)&buffer[2], RcvLen - 2);
						break;
					case 'C':							// Alarm1
						memcpy((void *)AlmMsg1, (void *)&buffer[2], RcvLen-2);
						break;
					case 'D':							// Alarm2
						memcpy((void *)AlmMsg2, (void *)&buffer[2], RcvLen-2);
						break;
					case 'E':							// Alarm3
						memcpy((void *)AlmMsg3, (void *)&buffer[2], RcvLen-2);
						break;
					case 'F':							// Alarm4
						memcpy((void *)AlmMsg4, (void *)&buffer[2], RcvLen-2);
						break;
					case 'G':							// Alarm5
						memcpy((void *)AlmMsg5, (void *)&buffer[2], RcvLen-2);
						break;
					case 'H':
						Limit = atoi(buffer + 2);
						if(Limit > 1023)
							Limit = 1023;
						break;
					default: break;			
 				}
			}
			break;
		case SM_EXEC:		/***** Establish 送信 ****/
			if(!TCPIsPutReady(MySocket))					// 接続正常か？ 
				return;
			else {
				// 折り返し返送
				TCPPutString(MySocket, (BYTE*)MD);
				TCPPut(MySocket, buffer[1]);				// データ区別
				TCPPut(MySocket, 0);
			}											// 返送実行
			TCPFlush(MySocket);							// 送信要求
			ContSM = SM_SEND_WAIT;							// 送信完了待ちへ移行
			break;
		case SM_SEND_WAIT:								// 送信完了待ち
			TCPDiscard(MySocket);							// 送信ソケットリリース
			ContSM = SM_CONNECTED;							// 受信へ移行
			break;;
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
	// Set up analog features of PORTA
	ADCON0 = 0x0D;			// ADON Channel 3
	ADCON1 = 0x0B;			// Vdd/Vss AN0 to AN3 are analog
	ADCON2 = 0xBE;			// Right justify, 20TAD ACQ time, Fosc/64 (~21.0kHz)
    // Enable internal PORTB pull-ups
    INTCON2bits.RBPU = 0;
	// Enable Interrupts
	RCONbits.IPEN = 1;		// Enable interrupt priorities
    INTCONbits.GIEH = 1;
    INTCONbits.GIEL = 1;
	// Calibration of ADC
	ADCON0bits.ADCAL = 1;
	ADCON0bits.GO = 1;
	while(ADCON0bits.GO);
	ADCON0bits.ADCAL = 0;
	// 汎用入力部初期設定
	TRISC = 0xFF;	
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
	memcpypgm2ram(AppConfig.NetBIOSName, (ROM void*)MY_DEFAULT_HOST_NAME, 16);	//(新規追加)
	FormatNetBIOSName(AppConfig.NetBIOSName);
}
