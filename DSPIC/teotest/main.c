//****ヘッダファイルインクルード********************************************************************
#include "p33FJ128MC802.h"	//デバイスコンフィグ
#include "stdio.h"			//標準入出力
#include "stdlib.h"			//標準ライブラリ
#include "string.h"			//文字列操作
#include "math.h"			//数学

#include "interruptlib.h"	//割り込み処理					○
#include "ppslib.h"			//ペリフェラルピンセレクト		○
#include "timerlib.h"		//タイマ						○
#include "adclib.h"			//ADC							○
#include "uartlib.h"		//UART							○
#include "pwmlib.h"			//pwm							○
//#include "motorpwm.h"		//モーターコントロール用pwm		動作未確認
#include "delaylib.h"		//delay							○
//#include "dmalib.h"			//Direct Memory Access			動作未確認


//****マクロ定義************************************************************************************
//入出力設定                                           FEDC BA98 7654 3210
#define		TRIS_APORT			0x03;				//           0000 0011
#define		TRIS_BPORT			0x8FA8;				// 1000 1111 1010 1000

//ピンマクロ
#define		LEDPIN0				LATAbits.LATA4

//アナログ入力設定
#define		ADC_PCFGBIT			ADC_AN0 & ADC_AN1 & ADC_AN4 & ADC_AN5	//アナログ入力ピン0,1,4,5をアナログ入力に設定

//****ヒューズビット設定****************************************************************************
_FBS(BSS_NO_FLASH);			//ブートモード設定、なんか分からんからoff
_FSS(SWRP_WRPROTECT_OFF)	//セキュアセグメントプログラムのライトプロテクト
_FGS(GCP_OFF);				//コードプロテクトoff
_FOSCSEL(FNOSC_FRCPLL & IESO_ON);	//内臓クロックをPLLで使用、Two-speed Oscillator Startupはなんか知らんけどon
_FOSC(FCKSM_CSDCMD &  IOL1WAY_OFF & OSCIOFNC_ON & POSCMD_NONE);	//クロック切り替えとクロックモニタ無効、OSCCONビットは書き込み１回のみ、OSCピンはデジタルIOとして使用、クロックモード指定なし
_FWDT(FWDTEN_OFF);			//ウォッチドッグタイマを使用しない
_FPOR(ALTI2C_OFF);			//なんか知らんけどoff
_FICD(JTAGEN_OFF);			//よう分からんけどoff


//****関数プロトタイプ宣言**************************************************************************
//初期化関数
void Setup(void);							//ペリフェラル初期化関数
//割り込み関数
void U1RXInterrupt(void);					//UART1受信割り込み関数
void U2RXInterrupt(void);					//UART2受信割り込み関数
void ADC1Interrupt(void);					//ADC1変換終了割り込み関数
void INT1Interrupt(void);					//外部割り込み1割り込み関数
void INT2Interrupt(void);					//外部割り込み2割り込み関数
void T1Interrupt(void);						//タイマ1割り込み関数
void T2Interrupt(void);						//タイマ2割り込み関数
void T3Interrupt(void);						//タイマ3割り込み関数
void T4Interrupt(void);						//タイマ4割り込み関数
void T5Interrupt(void);						//タイマ5割り込み関数


//****グローバル変数宣言****************************************************************************
int UART_RX_Flag = 1;
char UART_RX_Buffer[64] = {0};

//****main関数**************************************************************************************
int main(void)
{
	//----ローカル変数定義--------------------------------------------
	int a=0;
	char charbuffer = 0;
	Setup();
	
	
	printf("awaawaaaaa\r\n");
	
	
	
	while(1)
	{
		a = (a + 1) % 65535;
		PWM1_SetValue(a);
		
		
		
	
	charbuffer = U1RXREG;
	
	printf("%c", charbuffer );
		//printf("helloworld a= %d\r\n",a);
		delay_ms(10);
	}
	
	return(0);
}



//****UART1受信割り込み関数*************************************************************************
void  __attribute__((__interrupt__, __auto_psv__)) _U1RXInterrupt(void)
{
	char charbuffer = 0;
	
	charbuffer = U1RXREG;
	
	printf("%c", charbuffer + 1);
	
	//UART1受信割り込みステータスビット
	IFS0bits.U1RXIF = 0;	//割り込み要求クリア
}

//****UART2受信割り込み関数*************************************************************************
void  __attribute__((__interrupt__, __auto_psv__)) _U2RXInterrupt(void)
{
	//UART2受信割り込みステータスビット
	IFS1bits.U2RXIF = 0;	//割り込み要求クリア
}


//****ADC1割り込み関数******************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _ADC1Interrupt(void)
{
	IFS0bits.AD1IF = 0;		//ADC1割り込みフラグクリア
}

//****外部割り込み1関数*****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _INT1Interrupt(void)
{
	IFS1bits.INT1IF = 0;	//Int1割り込みフラグクリア
}

//****外部割り込み2関数*****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _INT2Interrupt(void)
{
	IFS1bits.INT2IF = 0;	//Int1割り込みフラグクリア
}

//****Timer1割り込み関数****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T1Interrupt(void)
{
	IFS0bits.T1IF = 0;		//Timer1割り込みフラグクリア
}

//****Timer2割り込み関数****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T2Interrupt(void)
{
	IFS0bits.T2IF = 0;		//Timer2割り込みフラグクリア
}

//****Timer3割り込み関数****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T3Interrupt(void)
{
	
	IFS0bits.T3IF = 0;		//Timer3割り込みフラグクリア
}

//****Timer4割り込み関数****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T4Interrupt(void)
{
	IFS1bits.T4IF = 0;		//Timer4割り込みフラグクリア
}

//****Timer5割り込み関数****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T5Interrupt(void)
{
	IFS1bits.T5IF = 0;		//Timer5割り込みフラグクリア
}

//****ペリフェラル初期化関数************************************************************************
void Setup(void)
{
	//----割り込み禁止処理--------------------------------------------
	DisableInterrupt();
	
	//----クロック設定------------------------------------------------
	OSCCON = 0x00300;				// PRIPLL指定(いまいちわかんね)
	CLKDIV = 0x0100;				// クロックの分周1:2に設定
	PLLFBD = 0x0026;				// 40倍　8MHz÷2×40÷2 = 80MHz(らしい)
	
	//----ポート入出力設定--------------------------------------------
	TRISA = TRIS_APORT;
	TRISB = TRIS_BPORT;
	
	//----UART初期化--------------------------------------------------
	
	
	//XBeeとFT232経由のとき
	//UART1_Init(UART_115200BPS, UART_FLOW);	//115200bps、フロー制御あり
	UART1_Init(UART_115200BPS, UART_NOFLOW);	//115200bps、フロー制御なし
	UART1_RxIntEnable(5);		//受信割り込み有効化
	PPSOut(RP2, RP_U1TX);		//TXを割り当てる
	PPSIn(RP3, RP_U1RX);		//RXを割り当てる
	//PPSIn(RP5, RP_U1CTS);		//CTSを割り当てる
	//PPSOut(RP4, RP_U1RTS);		//RTSを割り当てる
	
	/*
	//PICKIT2経由のとき
	TRISBbits.TRISB0 = 0;		//入出力設定変更
	TRISBbits.TRISB1 = 1;		//入出力設定変更
	UART1_Init(UART_9600BPS, UART_NOFLOW);	//9600bps、フロー制御なし
	UART1_RxIntEnable(5);		//受信割り込み有効化
	PPSOut(RP0, RP_U1TX);		//Txを割り当てる
	PPSIn(RP1, RP_U1RX);		//Rxを割り当てる
	*/
	
	
	//----ADC初期化---------------------------------------------------
	//ADC1_10bit_Init(ADC_PCFGBIT);
	
	
	
	//----Timer1初期化------------------------------------------------
	//Timer1はADC制御タイマ
	Timer1_Init(PRESCALER_8);		//プリスケーラ1:8でタイマ1初期化
	//Timer1_IntEnable(6);			//割り込み許可
	Timer1_IntDisable();
	//Timer1_Start(0x1000);			//0xFFFFで割り込み発生
	
	//----Timer2初期化------------------------------------------------
	//Timer2はPWMタイムベースタイマ
	Timer2_Init(PRESCALER_1);		//プリスケーラ1:1でタイマ2初期化
	//Timer2_IntEnable(3);			//割り込み許可
	Timer2_IntDisable();			//割り込み禁止
	Timer2_Start(0xFFFF);			//0xFFFF回カウント
	
	//----Timer3初期化------------------------------------------------
	//Timer3はモーター制御用タイマ
	Timer3_Init(PRESCALER_8);		//プリスケーラ1:8でタイマ3初期化
	//Timer3_IntEnable(6);			//割り込み許可
	Timer3_IntDisable();			//割り込み禁止
	//Timer3_Start(0xFFFF);			//0xFFFF回カウント
	
	//----Timer45初期化-----------------------------------------------
	//Timer45は起動時間計測タイマ
	Timer45_Init(PRESCALER_255);	//プリスケーラ1:255でタイマ45初期化
	Timer4_IntDisable();			//割り込み禁止
	Timer5_IntDisable();
	Timer45_Start(0xFFFFFFFF);		//0xFFFFFFFF回カウント
	
	
	//----PWM初期化---------------------------------------------------
	PWM1_Init(TIMER2);				//PWMをタイマ2で初期化
	PWM2_Init(TIMER2);
	PWM3_Init(TIMER2);
	PWM4_Init(TIMER2);
	PPSOut(RP0, RP_OC1);			//RPピン設定
	PPSOut(RP13, RP_OC2);
	PPSOut(RP14, RP_OC3);
	PPSOut(RP15, RP_OC4);
	PWM1_Start(0);					//PWM開始、初期値0
	PWM2_Start(0);
	PWM3_Start(0);
	PWM4_Start(0);
	
	
	//----外部割り込み許可--------------------------------------------
	//ロリコンパルスA入力割り込み
	ExternalInt1_Enable(LOW_TO_HI, 1);		//割り込みレベル1
	PPSIn(RP9, RP_INT1);

	//スイッチ1入力割り込み
	ExternalInt2_Enable(HI_TO_LOW, 1);
	PPSIn(RP10, RP_INT2);
	
	
	//----コマンドライン開始------------------------------------------
	//printf("Please Enter Comand\n> ");
	
	//----割り込み許可処理--------------------------------------------
	EnableInterrupt();
	
	//----LED表示off--------------------------------------------------
	LEDPIN0 = 1;
}

/*
0	0000
1	0001
2	0010
3	0011
4	0100
5	0101
6	0110
7	0111
8	1000
9	1001
A	1010
B	1011
C	1100
D	1101
E	1110
F	1111
*/









