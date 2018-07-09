/**************************************************
*  UDPデモアプリケーション
*　ICMP + ARP + DHCP Client + NBNS + UDPアプリ
*  プロジェクト名：LANUDP
**************************************************/
// 宣言とヘッダファイルのインクルード
#include "TCPIP Stack/TCPIP.h"

// スタック内で使う変数でメインで定義しておく
APP_CONFIG AppConfig;
BYTE myDHCPBindCount = 0xFF;
#if !defined(STACK_USE_DHCP_CLIENT)
	#define DHCPBindCount	(1)
#endif

// 関数プロトタイピング
static void InitAppConfig(void);
static void InitializeBoard(void);
static void DisplayIPValue(IP_ADDR IPVal);
void UDPControlTask(void);

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
		UDPControlTask();	
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
*  UDP汎用I/O処理部
*　・LEDのオンオフ制御と状態返送
*  ・スイッチの状態返送
*  ・計測値の返送
*  ・LCDへの文字表示 ・LCDの消去
**********************************************/
#define REMOTE_PORT	10001					// UDPのポート番号の指定
#define LOCAL_PORT	10002
const BYTE MD[] = "MDOK";						// 返送メッセージ
const BYTE ME[] = "MEOK";

void UDPControlTask(void)
{
	/// ローカル変数の定義
	static enum {							// ステートマシンの定義	
		CONT_IDLE = 0,
		CONT_LISTEN,
		CONT_EXEC
	} ContSM = CONT_IDLE;	
	static UDP_SOCKET		MySocket;			// 受信ソケットを維持する	
	static char ANString[8];					// アナログ入力変換バッファ
	unsigned int RcvLen;	
	static BYTE	buffer[40];					// 受信データ格納バッファ

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
			if(!UDPIsGetReady(MySocket))					// 受信有無チェック
				return;									// 受信なし即リターン
			RcvLen = UDPGetArray(buffer, sizeof(buffer));	// 受信データ取得
			UDPDiscard();									// UDPソケット切り離し
    			ContSM++;									// ステータス更新返送へ
			/// 受信データ キーワードチェック
			if(!(buffer[0] == 'S')) {						// 開始キー
				ContSM = CONT_LISTEN;						// キーワード不一致
				return;									// Listenに戻る
			}			
			else{		// キーワード一致　データ処理開始
				switch(buffer[1])	{						// コマンド取り出し
					case 'C':							// LED制御コマンド
						switch(buffer[2])	{
							case '1':					// LED2の場合
								LED2_IO = buffer[3] - '0';	// オンオフ判定
								break;
							case '2':					// LED3の場合
								LED3_IO = buffer[3] - '0';	// オンオフ判定
								break;
							default : break;
						}
					case 'A':							// 計測コマンド
						ADCON1 = 0b00001011;				// A/D設定
						ADCON2 = 0b10110110;
						TRISA = 0x2C;
						if(buffer[2] == '1')				// チャネル判定
							ADCON0 = 0b00001001;			// Channel 2
						else
							ADCON0 = 0b00001101;			// Channel 3
    						ADCON0bits.GO = 1;				// A/D変換開始
    						while(ADCON0bits.GO);				// A/D変換終了待ち
    						ltoa(*((WORD*)(&ADRESL)), ANString);// ASCIIに変換
						break;
					case 'D':							// LCD表示コマンド
						if(RcvLen < 36){					// 32文字で制限
							memset(LCDText, ' ', 32);
							memcpy((void *)LCDText, (void *)&buffer[2], RcvLen-2);
						}
						else{
							memcpy((void *)LCDText, (void *)&buffer[2], 32);
						}
						LCDUpdate();						// LCDに表示
						break;
					case 'E':							// LCD消去コマンド
						LCDErase();						// LCD全消去実行
						break;
					default: break;			
 				}
			}
			break;
		case CONT_EXEC:		/****** 送信  *****/
			/// 制御実行
			if(!UDPIsPutReady(MySocket)){					// 送信レディーか？ 
				return;									// まだリターン
			}
			else {				// 折り返し返送
				switch(buffer[1])	{						// コマンド区別
					case 'C':							// LED制御のときL
						UDPPut('M');						// Key
						UDPPut('L');						// LED
						UDPPut(LED2_IO ? '1':'0');			// LEDの状態返送
						UDPPut(LED3_IO ? '1':'0');	
						UDPPut('E');						
						break;
					case 'B':							// スイッチ状態返送
						UDPPut('M');						// Key
						UDPPut('K');						// Button
						UDPPut(BUTTON0_IO ? '1':'0');
						UDPPut(BUTTON1_IO ? '1':'0');						
						UDPPut(BUTTON2_IO ? '1':'0');
						UDPPut(BUTTON3_IO ? '1':'0');
						UDPPut('E');						
						break;
					case 'A':							// 計測データ返送
						UDPPut('M');						// Key
						UDPPut('A');						// Analog
						UDPPutString(ANString);
						UDPPut(0);
						UDPPut('E');
						break;
					case 'D':							// LCD表示OK返送
						UDPPutString((BYTE*)MD);
						UDPPut(0);
						break;
					case 'E':
						UDPPutString((BYTE*)ME);			// LCD消去OK返送
						UDPPut(0);
						break;
					default:	break;						// コマンドエラーのとき
				}
				UDPFlush();								// UDP送信実行
				ContSM = CONT_LISTEN;						// ステートを戻す	
			}											// caseごとに実行
			break;
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
