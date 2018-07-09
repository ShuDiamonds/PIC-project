/************************************************************
　　MPLAB-C18による入出力ポートの制御テストプログラム
　　スイッチの入力をポートDから行い、スイッチの状態に
　　従って、指定された発光ダイオードを点滅させる。
　　　　RD0のスイッチがOFFの時LED１
　　　　RD1のスイッチがOFFの時LED2
　　　　RD2のスイッチがOFFの時LED3
　　　　全スイッチがONの時全LED
*************************************************************/
#include <p18f26j50.h>    // PIC18F26j50のヘッダ・ファイル
#include <delays.h>
#include <usart.h>
#include <timers.h>
#include <stdio.h>


//***** コンフィギュレーションの設定config

	#pragma config WDTEN = OFF          //WDT disabled (enabled by SWDTEN bit)
	#pragma config PLLDIV = 2           //Divide by 2 (8 MHz intl osc input)
	#pragma config STVREN = ON          //stack overflow/underflow reset enabled
	#pragma config XINST = OFF          //Extended instruction set disabled
	#pragma config CPUDIV = OSC1        //No CPU system clock divide
	#pragma config CP0 = OFF            //Program memory is not code-protected
	#pragma config OSC = INTOSCPLL      //Internal oscillator, PLL enenabled
	#pragma config T1DIG = OFF          //S-Osc may not be selected, unless T1OSCEN = 1
	#pragma config LPT1OSC = OFF        //high power Timer1 mode
	#pragma config FCMEN = OFF          //Fail-Safe Clock Monitor disabled
	#pragma config IESO = OFF           //Two-Speed Start-up disabled
	#pragma config WDTPS = 32768        //1:32768
	#pragma config DSWDTOSC = INTOSCREF //DSWDT uses INTOSC/INTRC as clock
	#pragma config RTCOSC = T1OSCREF    //RTCC uses T1OSC/T1CKI as clock
	#pragma config DSBOREN = OFF        //Zero-Power BOR disabled in Deep Sleep
	#pragma config DSWDTEN = OFF        //Disabled
	#pragma config DSWDTPS = 8192       //1:8,192 (8.5 seconds)
	#pragma config IOL1WAY = OFF        //IOLOCK bit can be set and cleared
	#pragma config MSSP7B_EN = MSK7     //7 Bit address masking
	#pragma config WPFP = PAGE_1        //Write Protect Program Flash Page 0
	#pragma config WPEND = PAGE_0       //Start protection at page 0
	#pragma config WPCFG = OFF          //Write/Erase last page protect Disabled
	#pragma config WPDIS = OFF          //WPFP[5:0], WPEND, and WPCFG bits ignored 


		
//---Wait---//
//48Mhz駆動の時
#define WAIT_MS Delay1KTCYx(12)// Wait 1ms
#define WAIT_US Delay10TCYx(12)	// Wait 1us

//=======wait[ms]======//
void wait_ms(unsigned long int t) {
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[us]======//
void wait_us(unsigned int t) {
	while(t--) {
		WAIT_US;
	}
}

//グローバル変数定義
// --------- 時計初期値 BCDで設定する ---------
		unsigned char year  = 0x12;   // 2012年
		unsigned char month = 0x01;     //  1月
		unsigned char day   = 0x01;     //  1日
		unsigned char hour  = 0x12;     // 12時
		unsigned char min   = 0x00;     //  0分
		unsigned char sec   = 0x00;     //  0秒

		char CntSW ;                    // SW状態・カウント
		char tens, ones ;               // SW状態・カウント
		char temp_Buf[20] = {0};
//+++++++++define*++++++++++//
// もし、RTCC クロックがずれているようなら、
// RTCC キャリブレーションの値をセットし、毎分のクロック数を増減する。
#define		RTCCALIBRATION    0
#define		LED2				LATCbits.LATC2      // 毎秒間隔で点滅
#define		SW					LATCbits.LATC4
//+++++++++７セグ宣言+++++
/*
#define		SEG7_ZERO	0b11101110
#define		SEG7_ONE	0b01001000
#define		SEG7_TWO	0b10111100
#define		SEG7_THREE	0b11011100
#define		SEG7_FOUR	0b01011010
#define		SEG7_FIVE	0b11010110
#define		SEG7_SIX	0b11110110
#define		SEG7_SEVEN	0b01001110
#define		SEG7_EIGHT	0b11111110
#define		SEG7_NINE	0b01011110
*/

#define		SEG7_ZERO	0b00010001
#define		SEG7_ONE	0b10110111
#define		SEG7_TWO	0b01000011
#define		SEG7_THREE	0b00100011
#define		SEG7_FOUR	0b10100101
#define		SEG7_FIVE	0b00101000
#define		SEG7_SIX	0b00001001
#define		SEG7_SEVEN	0b10110001
#define		SEG7_EIGHT	0b00000001
#define		SEG7_NINE	0b10100001

//セグメント選択用ピン
#define		SEG_PORT			PORTB
#define		SEG_TRIS			TRISB
#define		SEG_SELECT_ONE		LATAbits.LATA1
#define		SEG_SELECT_TWO		LATAbits.LATA2
#define		SEG_SELECT_THREE	LATAbits.LATA3
#define		SEG_SELECT_FOUR		LATAbits.LATA0
#define		SEG_SELECT_FIVE		LATAbits.LATA5
#define		SEG_SELECT_SIX		LATAbits.LATA6



// プロトタイプの宣言
void UART_RC_isr(void);
void UART_TX_isr(void);
void Timer0_isr(void);
void RB0_isr(void);
void PRINT_TIME(void);
char HEX2SEG(int i);

//***** メイン関数
void main(void)                     // メイン関数
{
	//ローカル変数定義
	
	int ii=0;
	
	
	//クロック初期化
		//48Mhzの設定
	OSCTUNEbits.PLLEN = 1;        // PLLを起動
	wait_ms(2);
	
	//初期化
		TRISA=0x00;
		//TRISB=0;		//7セグに使用 
		TRISC=0b10111011;
		ADCON1=0b00001111;
	
	//７セグ初期化
	SEG_TRIS=0x01;
	SEG_PORT=0;
	SEG_SELECT_ONE=0;
	SEG_SELECT_TWO=0;
	SEG_SELECT_THREE=0;
	SEG_SELECT_FOUR=0;
	SEG_SELECT_FIVE=0;
	SEG_SELECT_SIX=0;
	
	//----Timer初期化------//
	OpenTimer0(TIMER_INT_ON & T0_8BIT & T0_SOURCE_INT & T0_PS_1_64);
								// タイマ0の設定, 8ビットモード, 割込み使用 
								//内部クロック、1:256プリスケーラ
	
	
	//RTC設定(32kHz)
	 OpenTimer1(TIMER_INT_OFF &		 // Secondary Oscを RTCC 
		T1_SOURCE_PINOSC &			// で利用するため
		T1_PS_1_1 &					// OSC1を有効にする
		T1_OSC1EN_ON & 
		T1_SYNC_EXT_OFF, 0);
	
	// --------- RTCC 日時の設定 (起動時のみ) ---------
	
	EECON2 = 0x55;                  // RTCC レジスタを
	EECON2 = 0xAA;                  // unlock しRTCCの
	RTCCFGbits.RTCWREN = 1;         // 設定準備をする
	RTCCFGbits.RTCPTR1 = 1;         // RTCVALHにアクセスする毎に
	RTCCFGbits.RTCPTR0 = 1;         // RTCPTRは自動的に -1 する
	/*
	RTCVALL = year;                 // 年
	RTCVALH = 0xFF;                 // 未設定
	RTCVALL = day;                  // 日
	RTCVALH = month;                // 月
	RTCVALL = hour;                 // 時
	RTCVALH = 0x01;                 // 曜日（0:日、1:月、…）
	RTCVALL = sec;                  // 秒
	RTCVALH = min;                  // 分
	*/
	
	
	RTCCFGbits.RTCEN = 1;           // RTCC を有効に
	RTCCAL = RTCCALIBRATION;        // キャリブレーション値セット
	
	
	//UART初期化
	//----UART初期化-------//
	Open1USART(USART_TX_INT_OFF & USART_RX_INT_ON &
			USART_ASYNCH_MODE & USART_EIGHT_BIT &
			USART_CONT_RX & USART_BRGH_LOW, 77); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600
	
	printf("Hello world\n\r");		//「Hello」と出力

	//***********割り込み設定***********************
	//***** 優先順位割込み使用宣言
	RCONbits.IPEN=1;
	//***** 低レベル使用周辺の定義
	IPR1bits.TMR1IP=0;
	//************Timer0優先順位設定*****
	INTCON2bits.TMR0IP = 1;	//高位割り込みに設定
	//************UART受信割り込み*************
	IPR1bits.RCIP = 0;		//UART変換割り込みを低位割り込みに設定
	PIE1bits.RCIE= 1;		//UART変換割り込みを許可

	//************RB0割り込み設定********
	//RB0/INT は割り込みに優先順位はなく高位のに割り込みがかかる
	INTCONbits.INT0IE=0;		//RB0/INT 外部割込みイネーブルビット(4bit目)を許可
	INTCON2bits.INTEDG0=0;			//RB0/INT ピンの立ち下がりエッジにより割り込み(6bit目)	
	
	
	//***** 割込み許可
	INTCONbits.GIEH=0;          // 高レベル許可
	INTCONbits.GIEL=0;          // 低レベル許可
	


SEG_SELECT_ONE=0;
	SEG_PORT=HEX2SEG(0x00&0x0F);
	wait_ms(1000);

SEG_SELECT_TWO=0;
	
	SEG_PORT=HEX2SEG(0x00&0x0F);
	wait_ms(1000);
	
SEG_SELECT_THREE=0;

	SEG_PORT=HEX2SEG(0x00&0x0F);
	wait_ms(1000);
	
SEG_SELECT_FOUR=0;
	SEG_PORT=HEX2SEG(0x00&0x0F);
	wait_ms(1000);
	
SEG_SELECT_FIVE=0;
	SEG_PORT=HEX2SEG(0x00&0x0F);
	wait_ms(1000);
	
SEG_SELECT_SIX=0;
	SEG_PORT=HEX2SEG(0x00&0x0F);
	wait_ms(1000);
		

	printf("aaa world\n\r");		//「Hello」と出力

SEG_SELECT_ONE=0;
	SEG_PORT=HEX2SEG(0x00&0x0F);
	wait_ms(500);
	SEG_PORT=HEX2SEG(0x01&0x0F);
	wait_ms(500);
	SEG_PORT=HEX2SEG(0x02&0x0F);
	wait_ms(500);
	SEG_PORT=HEX2SEG(0x03&0x0F);
	wait_ms(500);
	SEG_PORT=HEX2SEG(0x04&0x0F);
	wait_ms(500);
	SEG_PORT=HEX2SEG(0x05&0x0F);
	wait_ms(500);
	SEG_PORT=HEX2SEG(0x06&0x0F);
	wait_ms(500);
	SEG_PORT=HEX2SEG(0x07&0x0F);
	wait_ms(500);
	SEG_PORT=HEX2SEG(0x08&0x0F);
	wait_ms(500);
	SEG_PORT=HEX2SEG(0x09&0x0F);
	wait_ms(500);
	printf("end world\n\r");		//「Hello」と出力
	

	//***** 割込み許可
	INTCONbits.GIEH=1;          // 高レベル許可
	INTCONbits.GIEL=1;          // 低レベル許可
	
	
	/*
	//LED動作確認用
		LED2=!LED2;
		wait_ms(1000);
		LED2=!LED2;
		wait_ms(1000);
		LED2=!LED2;
		wait_ms(1000);
		LED2=!LED2;
		wait_ms(1000);
	*/
	

	while(1)
	{
		//PORTB = !PORTB;
		//wait_ms(1000);
		//printf("%d\n\r",ii++);
		/*
		if(SW==1)
		{
			printf("hogehoge\n\r");
		}
		*/
		PRINT_TIME();
		//SEG_PORT=HEX2SEG(sec&0x0F);
	}
}



void PRINT_TIME(void)
{
	
	RTCCFGbits.RTCPTR1 = 1;		 // RTCPTR のセット 
	RTCCFGbits.RTCPTR0 = 1;
	while(RTCCFGbits.RTCSYNC != 1); // RTCの位上げ中に、読み込ぬよう
	while(RTCCFGbits.RTCSYNC == 1); // SYCが「０」になるまで待つ
	LED2=!LED2;
	year  = RTCVALL;
	day	  = RTCVALH;	 // RTCPTR を -1 するためのダミー読み込み 
	day	  = RTCVALL;
	month = RTCVALH;
	hour  = RTCVALL;
	sec	  = RTCVALH;	 // RTCPTR を -1 するためのダミー読み込み 
	sec	  = RTCVALL;
	min	  = RTCVALH;
	// --------- RTCC 日時の表示 -----------------
	printf("20%02X/%02X/%02X ", year, month, day);
	printf("%02X:%02X:%02X\n\r", hour, min, sec);
	year  = 0x12;   // 2012年
	month = 0x01;     //  1月
	day   = 0x01;     //  1日
	
	

}

char HEX2SEG(int i)
{
	
	switch (i)
	{
		case 0:
			return SEG7_ZERO;
		case 1:
			return SEG7_ONE;
		case 2:
			return SEG7_TWO;
		case 3:
			return SEG7_THREE;
		case 4:
			return SEG7_FOUR;
		case 5:
			return SEG7_FIVE;
		case 6:
			return SEG7_SIX;
		case 7:
			return SEG7_SEVEN;
		case 8:
			return SEG7_EIGHT;
		case 9:
			return SEG7_NINE;
		
		default: return 0;
		
	}
	
}


//****** 割込みの宣言　優先順位使用
#pragma interrupt High_isr save = PROD
#pragma interruptlow Low_isr save = WREG,BSR,STATUS,PROD

//***** 割込みベクタジャンプ命令セット
#pragma code isrcode = 0x8
void isr_direct(void)
{
	_asm
	goto High_isr
	_endasm
}
#pragma code lowcode = 0x18
void low_direct(void)
{	_asm
	goto Low_isr
	_endasm
}
//**** 高レベル　割込み処理関数
#pragma code
void High_isr(void)                      // 割り込み関数
{
	INTCONbits.GIEH=0;          // 高レベル不許可
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART送信割り込み？
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART受信割り込み？
	if(INTCONbits.T0IF)	Timer0_isr();       // タイマ0割り込み？
	if(INTCONbits.INT0IF)	RB0_isr();		 // 外部割込み０割り込み？
	INTCONbits.GIEH=1;          // 高レベル許可
	
}                                   
//***** 低レベル割込み処理関数
void Low_isr(void)                     // 割り込み関数
{
	INTCONbits.GIEL=0;          // 低レベル不許可
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART送信割り込み？
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART受信割り込み？
	if(INTCONbits.T0IF)	Timer0_isr();       //タイマ0割り込み？
	if(INTCONbits.INT0IF)	RB0_isr();		 // 外部割込み０割り込み？
	INTCONbits.GIEL=1;          // 低レベル不許可
	
}

//**********Timer0割り込み*************
void Timer0_isr(void)
{
	static int i=1;
	INTCONbits.T0IF=0;          // タイマ0割り込みフラグを0にする
	//とりあえず全部消す
	SEG_SELECT_ONE=1;
	SEG_SELECT_TWO=1;
	SEG_SELECT_THREE=1;
	SEG_SELECT_FOUR=1;
	SEG_SELECT_FIVE=1;
	SEG_SELECT_SIX=1;
	/*
	SEG_SELECT_ONE=0;
	SEG_SELECT_TWO=0;
	SEG_SELECT_THREE=0;
	SEG_SELECT_FOUR=0;
	SEG_SELECT_FIVE=0;
	SEG_SELECT_SIX=0;
	*/
	
	
	switch (i)
	{
		case 1:
			SEG_PORT=HEX2SEG(sec&0x0F);
			SEG_SELECT_ONE=0;
			break;
		case 2:
			SEG_PORT=HEX2SEG((sec&0xF0)>>4);
			SEG_SELECT_TWO=0;
			break;
		case 3:
			SEG_PORT=HEX2SEG(min&0x0F);
			SEG_SELECT_THREE=0;
			break;
		case 4:
			SEG_PORT=HEX2SEG((min&0xF0)>>4);
			SEG_SELECT_FOUR=0;
			break;
		case 5:
			SEG_PORT=HEX2SEG(hour&0x0F);
			SEG_SELECT_FIVE=0;
			break;
		case 6:
			SEG_PORT=HEX2SEG((hour&0xF0)>>4);
			SEG_SELECT_SIX=0;
			break;
		//default:
			/*
			SEG_PORT=0b01001000;
			SEG_SELECT_ONE=0;
			SEG_SELECT_TWO=0;
			SEG_SELECT_THREE=0;
			SEG_SELECT_FOUR=0;
			SEG_SELECT_FIVE=0;
			SEG_SELECT_SIX=0;

		*/
		
	}
	 
	i++;
	if(i>=7)
	{
		i=1;
	}
}

//**********UART受信割り込み*************
void UART_RC_isr(void)
{
	static int i =0;
	int hour2=0,sec2=0,min2=0;
	char hoge[3]="";
	

	temp_Buf[i] = getc1USART();
	PIR1bits.RCIF = 0;		//割り込みフラグクリア
	
	i++;
	printf("i=%d\n\r",i);
	printf("temp_Buf=");
	//puts1USART(temp_Buf);
	printf("%c",temp_Buf[i-1]);
	printf("\n\r");
	if(temp_Buf[i-1]==27 && i>=8)//ESCの時
	{
		if(temp_Buf[i-4]==':' &&temp_Buf[i-7]==':')
		{
			hoge[0]=temp_Buf[i-3];
			hoge[1]=temp_Buf[i-2];
			hoge[2]='\n';			//改行コード
			sec2=atoi(hoge);
			printf("sec2=");
			//puts1USART(hoge);
			printf("%d\n\r",sec2);
			
			hoge[0]=temp_Buf[i-6];
			hoge[1]=temp_Buf[i-5];
			hoge[2]='\n';			//改行コード
			min2=atoi(hoge);
			printf("min2=");
			printf("%d\n\r",min2);
			
			hoge[0]=temp_Buf[i-9];
			hoge[1]=temp_Buf[i-8];
			hoge[2]='\n';			//改行コード
			hour2=atoi(hoge);
			printf("hour2=");
			printf("%d\n\r",hour2);
			
			// --------- RTCC 日時の設定 ---------
			/*
			EECON2 = 0x55;                  // RTCC レジスタを
			EECON2 = 0xAA;                  // unlock しRTCCの
			RTCCFGbits.RTCWREN = 1;         // 設定準備をする
			RTCCFGbits.RTCPTR1 = 1;         // RTCVALHにアクセスする毎に
			RTCCFGbits.RTCPTR0 = 1;         // RTCPTRは自動的に -1 する
			*/
			RTCVALL = year;                 // 年
			RTCVALH = 0xFF;                 // 未設定
			RTCVALL = day;                  // 日
			RTCVALH = month;                // 月
			
			RTCVALL = hour2;                 // 時
			RTCVALH = 0x01;                 // 曜日（0:日、1:月、…）
			RTCVALL = sec2;                  // 秒
			RTCVALH = min2;                  // 分
			/*
			RTCCFGbits.RTCEN = 1;           // RTCC を有効に
			RTCCAL = RTCCALIBRATION;        // キャリブレーション値セット
			
			*/
		}
	//初期化
		for(;i>0;i--)
		{
			temp_Buf[i]=0;
		}
		temp_Buf[0]=0;
		i=0;
		printf("reset\n\r");
	
	}
	
}

//**********UART送信割り込み*************
void UART_TX_isr(void)
{
	
}
//**********RB0割り込み*************
void RB0_isr(void)
{
	INTCONbits.INT0IF = 0;		//割り込みフラグクリア
	
	printf("RB\n\r");
}
/*************************************
参考URL

http://sky.geocities.jp/home_iwamoto/page/P26J50/P26_B04.htm		//PIC18f RTC

**************************************/