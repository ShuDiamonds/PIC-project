/**************************************************
*  ビデオカメラリモコンプログラム
*　ICMP + ARP + NBNS + DHCP + UDP + TCP
*  プロジェクト名：LANServo
**************************************************/
// 宣言とヘッダファイルのインクルード
#include "TCPIP Stack/TCPIP.h"

// スタック内で使う変数でメインで定義しておく
APP_CONFIG AppConfig;
BYTE myDHCPBindCount = 0xFF;
#if !defined(STACK_USE_DHCP_CLIENT)
	#define DHCPBindCount	(1)
#endif

static int Width1, Width2;			// ECCPのパルス幅

// 関数プロトタイピング
static void InitAppConfig(void);
static void InitializeBoard(void);
static void DisplayIPValue(IP_ADDR IPVal);
void TCPControlTask(void);
void RCServo(void);

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
#pragma code 		// Return to default code section
// 高位割り込み(タイマ３） 20mmsec周期で割り込む
#pragma interrupt HighISR
void HighISR(void){
	RCServo();
}
#pragma code highVector=0x8
void HighVector(void){_asm goto HighISR _endasm}
#pragma code 		// Return to default code section

/************************************************
*  タイマ3割り込み処理、
*  RCサーボ制御出力　20msec周期のパルス生成
************************************************/
void RCServo(void){
	if(PIR2bits.TMR3IF) {				// タイマ3の割り込み確認
    		TMR3H = 0x9A;					// 20msecに再設定
    		TMR3L = 0x46;		
  		T1CONbits.TMR1ON = 0;			// タイマ１は停止（PWM生成用）
	  	TMR1H = 0;					// タイマ１クリア
    		TMR1L = 0;
		// チャネル１　ECCP1パルス幅設定 1bit = 0.77usec
		// 0.9msec～2.1msecの範囲
		if(Width1 > 2730)				// Max制限
			Width1 = 2730;
		if(Width1 < 1170)				// Min制限
			Width1 = 1170;
		CCPR1H = (char)(Width1 >> 8);	// パルス幅として設定
		CCPR1L = (char)Width1;
		// チャネル２　ECCP2パルス幅設定
		if(Width2 > 2730)				// Max制限
			Width2 = 2730;
		if(Width2 < 1170)				// Min制限
			Width2 = 1170;
		CCPR2H = (char)(Width2 >> 8);	// パルス幅として設定
		CCPR2L = (char)Width2;
		// ワンショットパルスモードで動作開始
		CCP1CON = 0x08;
		CCP2CON = 0x08;
		T1CONbits.TMR1ON = 1;			// タイマ１再スタート		
		PIR2bits.TMR3IF = 0;			// タイマ3割り込みフラグクリア
	}
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
    while(1)   {
		// LED0の点滅（1秒間隔）
		if(TickGet() - t >= TICK_SECOND/2ul){
            	t = TickGet();
            	LED0_IO ^= 1;						// LED反転
        	}
		// スタックの送受信実行タスク（一定時間内に実行必須）
        	StackTask();
		// NBNSによる名前解決	
		NBNSTask();
        	// TCP送受信デモ
		TCPControlTask();
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
	for(i = 0; i < sizeof(IP_ADDR); i++){
	    uitoa((WORD)IPVal.v[i], IPDigit);		// IP1桁の文字変換
		for(j = 0; j < strlen((char*)IPDigit); j++)	{
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
*  TCP汎用I/O処理部
*　・外部出力制御と状態返送
*  ・温度の計測
*  ・LCDへの文字表示
*  ・LCDの消去
**********************************************/
/// TCPポート番号の指定
#define LOCAL_PORT	50001
const BYTE MD[] = "MDOK";						// 返送メッセージ
const BYTE ME[] = "MEOK";

void TCPControlTask(void)
{
	//// ステートマシンの定義	
	static enum {
		SM_IDLE = 0,				// 初期状態
		SM_LISTEN_WAIT,			// 接続待ち
		SM_CONNECTED,				// 接続中
		SM_EXEC,					// 送受信実行中
		SM_SEND_WAIT,				// 送信待ち中
		SM_DISCONNECT				// 接続切り離し要求
	} ContSM = SM_IDLE;
	/***** ローカル変数の定義  ****/
	static TCP_SOCKET	 MySocket = INVALID_SOCKET;			// 受信ソケットを維持する	
	static char ANString[8];								// アナログ入力変換バッファ
	static unsigned int  RcvLen;
	static BYTE	buffer[128];								// 受信データ格納バッファ

	/**** 関数実行部　ステートマシンにより進行 *****/
	switch(ContSM)
	{
		case SM_IDLE:		/****  アイドルからListenへ ****/
			MySocket = TCPListen(LOCAL_PORT);				// TCPサーバとしてListen状態にする
			if(MySocket == INVALID_SOCKET)
				return;									// オープン失敗なら繰り返し
			else
				ContSM = SM_LISTEN_WAIT;					// LISTENへ移行
			break;
		case SM_LISTEN_WAIT:	/***** LISTEN 接続待ち *******/
			LED1_IO = 1;									// LED1オン
			if(TCPIsConnected(MySocket))					// 
				ContSM = SM_CONNECTED;						// 受信待ちへ移行
			break;;
		case SM_CONNECTED:	/***** Establish 受信 ****/
			LED1_IO = 0;									// LED1オフ
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
			else{		/***** キーワード一致　データ処理開始 *****/
				switch(buffer[1])	{						// コマンド取り出し
					case 'C':							// LED制御コマンド
						switch(buffer[2])	{
							case '1':					// 汎用出力1の場合
								UIO1_IO = buffer[3] - '0';	// オンオフ判定
								break;
							case '2':					// 汎用出力2の場合
								UIO2_IO = buffer[3] - '0'; // オンオフ判定
								break;
							default :
								break;
						}
					case 'A':							// 計測コマンド
						ADCON0 = 0;
						ADCON1 = 0b00001011;				// A/D設定
						ADCON2 = 0b10111110;
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
							memset(LCDText, ' ', 32);
							memcpy((void *)LCDText, (void *)&buffer[2], 32);
						}
						LCDUpdate();						// LCDに表示
						break;
					case 'E':							// LCD消去コマンド
						LCDErase();						// LCD全消去実行
						break;
					case 'R':							// RCサーボの制御
						switch(buffer[2]) {				
							case '1':					// チャネル1側
								Width1 = atol(&buffer[3]);	// パルス幅として保存
								break;
							case '2':					// チャネル2側
								Width2 = atol(&buffer[3]);	// パルス幅として保存
								break;
							default : break;
						}
					default: break;			
 				}
			}
			break;
		case SM_EXEC:		/***** Establish 送信 ****/
			if(!TCPIsPutReady(MySocket))					// 接続正常か？ 
				return;
			else {
				// 折り返し返送
				switch(buffer[1])	{						// コマンド区別
					case 'C':							// LED制御のとき
						TCPPut(MySocket,'M');				// Key
						TCPPut(MySocket,'L');				// 汎用出力
						TCPPut(MySocket,UIO1_IO ? '1':'0');	// 汎用出力の状態返送							
						TCPPut(MySocket,UIO2_IO ? '1':'0');	
						TCPPut(MySocket,'E');						
						break;
					case 'B':							// スイッチ状態返送
						TCPPut(MySocket,'M');				// Key
						TCPPut(MySocket,'K');				// Button
						TCPPut(MySocket,BUTTON0_IO ? '1':'0');
						TCPPut(MySocket,BUTTON1_IO ? '1':'0');						
						TCPPut(MySocket,BUTTON2_IO ? '1':'0');
						TCPPut(MySocket,BUTTON3_IO ? '1':'0');
						TCPPut(MySocket,'E');						
						break;
					case 'A':							// 計測データ返送
						TCPPut(MySocket,'M');				// Key
						TCPPut(MySocket,'A');				// Analog
						TCPPutString(MySocket, ANString);
						TCPPut(MySocket, 0);
						TCPPut(MySocket,'E');
						break;
					case 'D':							// LCD表示OK返送
						TCPPutString(MySocket, (BYTE*)MD);
						TCPPut(MySocket, 0);
						break;
					case 'E':
						TCPPutString(MySocket,(BYTE*)ME);	// LCD消去OK返送
						TCPPut(MySocket, 0);
						break;
					default:		break;					// コマンドエラーのとき
				}
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
	UIO1_TRIS = 0;
	UIO2_TRIS = 0;
	UIO1_IO = 0;
	UIO2_IO = 0;

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

	// タイマ１の初期化関数
    // Initialize the time for Capture
    TMR1H = 0;
    TMR1L = 0;
	// Disable timer interrupt
    PIE1bits.TMR1IE = 0;		// Dsiable interrupt
    // Timer1 off, 16-bit, internal timer, 1:8 prescalar
	// 1bit = 0.77usec
    T1CON = 0xB0;

	// タイマ3の初期化関数 20msec
    TMR3H = 0x9A;
    TMR3L = 0x46;
	// Set up the timer interrupt
	IPR2bits.TMR3IP = 1;		// Low priority
    	PIR2bits.TMR3IF = 0;
    	PIE2bits.TMR3IE = 1;		// Enable interrupt
    	// Timer3 on, 16-bit, internal timer, 1:8 prescalar
	// all ECCP timebase is Timer1 and Timer2
    	T3CON = 0xB1;

	//ECCPモジュールの初期化関数
	// IO
	RCS1_TRIS = 0;			// Output Mode
	RCS2_TRIS = 0;
	// ECCP1 initialize Duty low 2bits always 0
	T3CONbits.T3CCP2 = 0;	// select timer1 for timebase
	T3CONbits.T3CCP1 = 0;
	// Stop Timer1
	T1CONbits.TMR1ON = 0;
	// Setup Compare Mode
	CCPR1H = 0;
	CCPR1L = 0;
	CCP1CON = 0x08;			// Compare mode Initial Low
	CCPR2H = 0;
	CCPR2L = 0;
	CCP2CON = 0x08; 
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
	Width1 = 1950;
	Width2 = 1950;
}
