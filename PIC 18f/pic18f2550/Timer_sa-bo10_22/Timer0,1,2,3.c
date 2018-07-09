#include <p18f2550.h>
#include <delays.h>
#include <usart.h>
#include <timers.h>
#include <adc.h>
#include <stdlib.h> 
#include <stdio.h>

//***** コンフィギュレーションの設定config

 
		#pragma config PLLDIV	= 5			// (20 MHz crystal on PICDEM FS USB board)
		#pragma config CPUDIV	= OSC1_PLL2	  
		#pragma config USBDIV	= 2			// Clock source from 96MHz PLL/2
		#pragma config FOSC		= HSPLL_HS
		#pragma config FCMEN	= OFF
		#pragma config IESO		= OFF
		#pragma config PWRT		= OFF
		#pragma config BOR		= OFF		//ADD
		#pragma config BORV		= 3
		//	#pragma config VREGEN	= ON	  //USB Voltage Regulator
		#pragma config VREGEN	= OFF	   //USB Voltage Regulator
		#pragma config WDT		= OFF
		#pragma config WDTPS	= 32768
		#pragma config MCLRE	= ON
		#pragma config LPT1OSC	= OFF
		#pragma config PBADEN	= OFF
		//		#pragma config CCP2MX	= ON
		#pragma config STVREN	= ON
		#pragma config LVP		= OFF
		//		#pragma config ICPRT	= OFF		// Dedicated In-Circuit Debug/Programming
		#pragma config XINST	= OFF		// Extended Instruction Set
		#pragma config CP0		= OFF
		#pragma config CP1		= OFF
		//		#pragma config CP2		= OFF
		//		#pragma config CP3		= OFF
		#pragma config CPB		= OFF
		//		#pragma config CPD		= OFF
		#pragma config WRT0		= OFF
		#pragma config WRT1		= OFF
		//		#pragma config WRT2		= OFF
		//		#pragma config WRT3		= OFF
		#pragma config WRTB		= OFF		// Boot Block Write Protection
		#pragma config WRTC		= OFF
		//		#pragma config WRTD		= OFF
		#pragma config EBTR0	= OFF
		#pragma config EBTR1	= OFF
		//		#pragma config EBTR2	= OFF
		//		#pragma config EBTR3	= OFF
		#pragma config EBTRB	= OFF

		
/********関数プロトタイプ************/ 
void Timer0_isr(void); 
void Timer1_isr(void); 
void Timer2_isr(void); 
void Timer3_isr(void); 
void RB_PORT_isr(void); 
void RB0_isr(void); 
void RB1_isr(void); 
void RB2_isr(void); 
void AD_isr(void); 
void UART_TX_isr(void); 
void UART_RC_isr(void); 
//********************************************************
/********ピンマクロ***********/
  
#define		PIN_LED_0	PORTCbits.RC0 
#define		PIN_LED_1	PORTCbits.RC1 
#define		PIN_LED_2	PORTCbits.RC2 
#define		PIN_LED_3	PORTCbits.RC4 
#define		PIN_LED_4	PORTCbits.RC5 
#define		PIN_LED_5	PORTBbits.RB4 
#define		PIN_LED_6	PORTBbits.RB5 
#define		PIN_LED_7	PORTBbits.RB6 
#define		PIN_LED_8	PORTBbits.RB7 
//***** 変数、定数の定義 
unsigned char cnt=4;            // cnt,cnt1はLED更新周期用カウンタ 
unsigned char cnt1=5; 
unsigned char cnt2=6; 
unsigned char cnt3=7; 
unsigned char cnt4=20; 
unsigned char cnt5=111; 
unsigned char cnt6=90; 
char temp_Buf[20] = {0}; 
char Send_Buf[20] = {0}; 

//***********自作PWM設定********
#define	ClockOfTime		48				//単位はMHz
//PWMピンマクロ
#define	PIN_PWM01	PORTBbits.RB0
#define	PIN_PWM02	PORTBbits.RB1
#define	PIN_PWM03	PORTBbits.RB2
#define	PIN_PWM04	PORTBbits.RB3
#define	PIN_PWM05	PORTBbits.RB4
#define	PIN_PWM06	PORTBbits.RB5
#define	PIN_PWM07	PORTBbits.RB6
#define	PIN_PWM08	PORTBbits.RB7

//変数定義
unsigned int PWM_duty01=0, PWM_duty02=0, PWM_duty03=0, PWM_duty04=0, PWM_duty05=0, PWM_duty06=0, PWM_duty07=0, PWM_duty08=0;
unsigned int PWM_duty_time = 1;
	
	#if( ClockOfTime == 20  )
		#define time_wait_count	5				//waitカウント変数
		
		#define	TimerPWMcount	29				//200.200usecおきに割り込み
		#define	PURISUKELA	T0_PS_1_4			//プリスケーラ設定
		#warning	クロック速度20MHz
		
	#elif( ClockOfTime == 40 )
		#define time_wait_count	10				//waitカウント変数
		#define	PURISUKELA	T0_PS_1_8			//プリスケーラ設定
		#define	TimerPWMcount	17				//200.600usecおきに割り込み
		#warning	クロック速度40MHz
		
	#elif( ClockOfTime == 48 )
		#define time_wait_count	12				//waitカウント変数
		#define	PURISUKELA	T0_PS_1_16			//プリスケーラ設定
		
		#define	TimerPWMcount	111				//201.083usecおきに割り込み
		#warning	クロック速度48MHz
		
	#elif( ClockOfTime == 64 )
		#define time_wait_count	16				//waitカウント変数
		#define	PURISUKELA	T0_PS_1_128			//プリスケーラ設定
		
		#define	TimerPWMcount	241				//201.400usecおきに割り込み
		#warning	クロック速度64MHz
		
	#else
		#define time_wait_count	16
		#define	PURISUKELA	T0_PS_1_256
		
		#define	TimerPWMcount	50
		
	#endif

//---Wait関数定義---//
#define WAIT_MS Delay1KTCYx(time_wait_count)// Wait 1ms
#define WAIT_US Delay10TCYx(time_wait_count) // Wait 1us
//=======wait[ms]======// 
void wait_ms(unsigned int t) { 
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
 
//***** メイン関数
void main(void)						// メイン関数
{
	
	
	
	//ローカル変数定義
	char Message1[8]="Start!!";
	char Message2[7]="FUKUDA";
	char data;
	unsigned int i=0;
	
	//UART初期化
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 64);	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600
	
	//Timer0初期化
	OpenTimer0(TIMER_INT_ON & T0_8BIT & T0_SOURCE_INT & PURISUKELA);
	
/*	OpenTimer1(TIMER_INT_ON & T1_8BIT_RW & T1_SOURCE_INT & T1_PS_1_8 &	
		T1_OSC1EN_OFF);		//タイマ１の設定,8ビットモード、割込み使用 
							//内部クロック、1:8プリスケーラ 
	OpenTimer2(	 
		TIMER_INT_ON & 
		T2_PS_1_4 & 
		T2_POST_1_8 ); 
		
	OpenTimer3(TIMER_INT_ON & T3_SOURCE_INT &  T3_SYNC_EXT_ON &	 
		T3_SOURCE_CCP); 
*/		//カウント値のロード
		WriteTimer0(TimerPWMcount);
	
	//初期化
		TRISA=0x00;						   // ポートAをすべて出力ピンにする
		TRISB=0;						// ポートBをすべて出力ピンにする
		TRISC=0b10111111;						 // ポートCをすべて出力ピンにする
		ADCON1 = 0b00001111;				//すべてデジタル
	//-----AD初期化--------// 
	OpenADC(ADC_FOSC_64 &			//AD変換用クロック　　システムクロックの1/64　0.05μsec×64＝3.2μsec　>=　1.6μsec　→　OK 
			ADC_RIGHT_JUST &		//変換結果の保存方法　左詰め　 
			ADC_8_TAD,				//AD変換のアクイジションタイム選択　3.2μsec（=1Tad）×8Tad=25.6μsec　＞＝　12.8μsec　→　OK 
			ADC_CH0 &						//AD変換するのチャンネル選択（PIC18Fは同時に複数のAD変換はできない） 
			ADC_INT_ON &		   //AD変換での割込み使用の有無 
			ADC_VREFPLUS_VDD &		//Vref+の設定　　　ＰＩＣの電源電圧と同じ：ADC_VREFPLUS_VDD 　or　外部（ＡＮ３）の電圧：ADC_VREFPLUS_EXT 
			ADC_VREFMINUS_VSS,		//Vref-の設定　　　ＰＩＣの0Ｖ：ADC_VREFMINUS_VSS	 or　外部（AN2)の電圧：ADC_VREFMINUS_EXT 
			0b1110	//ポートのアナログ・デジタル選択　（ADCON1の下位４ビットを記載）　　AN0のみアナログポートを選択、他はデジタルポートを選択 
			//例 　アナログポートが　AN0のみ → 0b1110　　、AN0 & AN1　→　0b1011、 AN0 & AN1 & AN2 →1100　他　詳細データシート参照 
		); 
	 
	//***********割り込み設定*********************** 
//***** 優先順位割込み使用宣言 
	RCONbits.IPEN=1; 
//***** 低レベル使用周辺の定義 
	IPR1bits.TMR1IP=0; 
//************Timer0優先順位設定***** 
	INTCON2bits.TMR0IP = 1; //高位割り込みに設定 
//************Timer1優先順位設定***** 
	IPR1bits.TMR1IP = 0;	//高位割り込みに設定 
//************Timer2優先順位設定***** 
	IPR1bits.TMR2IP = 0;	//高位割り込みに設定 
//************Timer3優先順位設定***** 
	IPR2bits.TMR3IP = 0;	//高位割り込みに設定 
  
//************RB0割り込み設定******** 
	//RB0/INT は割り込みに優先順位はなく高位のに割り込みがかかる 
	INTCONbits.INT0IE=0;		//RB0/INT 外部割込みイネーブルビット(4bit目)を 1:許可 , 0:不許可 
	INTCON2bits.INTEDG0=0;			//RB0/INT ピンの立ち下がりエッジにより割り込み(6bit目)	
	  
//************RB1割り込み設定********* 
	INTCON3bits.INT1IP = 0;		//RB1/INT 高位割り込みにする 
	INTCON3bits.INT1IE = 0;		//RB1/INT 外部割込みイネーブルビット(4bit目)を 1:許可 , 0:不許可 
	INTCON2bits.INTEDG1 = 0;			//RB1/INT ピンの立ち下がりエッジにより割り込み(6bit目) 
  
//************RB2割り込み設定********* 
	INTCON3bits.INT2IP = 0;			//RB2/INT 高位割り込みにする 
	INTCON3bits.INT2IE = 0;		//R2/INT 外部割込みイネーブルビット(4bit目)を 1:許可 , 0:不許可 
	INTCON2bits.INTEDG2 = 0;	//RB2/INT ピンの立ち下がりエッジにより割り込み(6bit目) 
  
//************RBPORT割り込み設定********* 
	INTCON2bits.RBIP = 0;		//RBPORT/INT 低位割り込みにする 
	INTCONbits.RBIE = 0;		//RB2/INT 外部割込みイネーブルビット(4bit目)を 1:許可 , 0:不許可
	  
//************AD変換割り込み************* 
	IPR1bits.ADIP = 0;		//AD変換割り込みを低位割り込みに設定 
	PIE1bits.ADIE = 0;		//AD変換割り込みを 1:許可 , 0:不許可
//************UART送信割り込み************* 
	IPR1bits.TXIP = 0;		//uart変換割り込みを低位割り込みに設定 
	PIE1bits.TXIE= 0;		//uart変換割り込みを 1:許可 , 0:不許可
//************UART受信割り込み************* 
	IPR1bits.RCIP = 0;		//AD変換割り込みを低位割り込みに設定 
	PIE1bits.RCIE= 0;		//AD変換割り込みを 1:許可 , 0:不許可 
	  
	//***** 割込み許可 
	INTCONbits.GIEH=1;			// 高レベル許可 
	INTCONbits.GIEL=1;			// 低レベル許可 
	  
	  
	  //自作PWM初期化
	  PIN_PWM01 = 1;
		PIN_PWM02 = 1;
		PIN_PWM03 = 1;
		PIN_PWM04 = 1;
		PIN_PWM05 = 1;
		PIN_PWM06 = 1;
		PIN_PWM07 = 1;
		PIN_PWM08 = 1;
//	putsUSART(Message1); 
	//***** メインループ（アイドルループ） 
	while(1)	 
	{ 
		/*
		
		SetChanADC(ADC_CH0);	//Select Channel 0 
		Delay100TCYx(50);		//20usec delay 
		ConvertADC();		//Start A/D 
		while(BusyADC());	//Wait end of conversion 
		data = ReadADC();	//Get A/D data 
		fprintf(_H_USART,"\rData=%ld\n",data);	 
		  
		putsUSART(Message1); 
		  
		
		
		PIN_LED_0 = 1; 
		Delay10KTCYx(100); 
		PIN_LED_0 = 0; 
		Delay10KTCYx(100); 
		
		*/
		PWM_duty01=i;
		PWM_duty02=100-i;
		i++;
		if(i==100)
		{
			i=0;
		}
		wait_ms(10);
		
	} 
} 
  
//**************************************************** 
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
void High_isr(void)						 // 割り込み関数 
{ 
	INTCONbits.GIEH=0;			// 高レベル不許可 
	if(INTCONbits.T0IF) Timer0_isr();		// タイマ0割り込み？ 
	if(PIR1bits.TMR1IF) Timer1_isr();		 // タイマ１割り込み？ 
	if(PIR1bits.TMR2IF) Timer2_isr();		//タイマ2割り込み？ 
	if(PIR2bits.TMR3IF) Timer3_isr();		// タイマ3割り込み？ 
	if(INTCONbits.INT0IF)	RB0_isr();		 // 外部割込み０割り込み？ 
	if(INTCON3bits.INT1IF)	RB1_isr();		 // 外部割こみ１割り込み？ 
	if(INTCON3bits.INT2IF)	RB2_isr();		 // 外部割込み２割り込み？ 
	if(INTCONbits.RBIF) RB_PORT_isr();		 // 外部割込みPORT変化割り込み？ 
	if(PIR1bits.ADIF)	AD_isr();			 //AD割り込み？ 
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART送信割り込み？ 
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART受信割り込み？ 
	//INTCONbits.GIEH=1;		  // 高レベル許可 
	  
}									 
//***** 低レベル割込み処理関数 
void Low_isr(void)					   // 割り込み関数 
{ 
	INTCONbits.GIEL=0;			// 低レベル不許可 
	if(INTCONbits.T0IF) Timer0_isr();		//タイマ0割り込み？ 
	if(PIR1bits.TMR1IF) Timer1_isr();		//タイマ１割り込み？ 
	if(PIR1bits.TMR2IF) Timer2_isr();		//タイマ2割り込み？ 
	if(PIR2bits.TMR3IF) Timer3_isr();		// タイマ3割り込み？ 
	if(INTCONbits.INT0IF)	RB0_isr();		 // 外部割込み０割り込み？ 
	if(INTCON3bits.INT1IF)	RB1_isr();		 // 外部割こみ１割り込み？ 
	if(INTCON3bits.INT2IF)	RB2_isr();		 // 外部割込み２割り込み？ 
	if(INTCONbits.RBIF) RB_PORT_isr();		 // 外部割込みPORT変化割り込み？ 
	if(PIR1bits.ADIF)	AD_isr();			 //AD割り込み？ 
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART送信割り込み？ 
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART受信割り込み？ 
	  
}						 
  
  
  
  
/************************************************************/ 

/***********割り込み関数実行部*********/
  
//**********Timer0割り込み************* 
void Timer0_isr(void) 
{ 
	INTCONbits.T0IF=0;			// タイマ0割り込みフラグを0にする 
	WriteTimer0(TimerPWMcount);	//カウント値セット
	PWM_duty_time++;
	
	if(PWM_duty_time>=PWM_duty01) 
	{	PIN_PWM01 = 0; } 
	if(PWM_duty_time>=PWM_duty02) 
	{	PIN_PWM02 = 0; } 
	if(PWM_duty_time>=PWM_duty03) 
	{	PIN_PWM03 = 0; } 
	if(PWM_duty_time>=PWM_duty04) 
	{	PIN_PWM04 = 0; } 
	if(PWM_duty_time>=PWM_duty05) 
	{	PIN_PWM05 = 0; } 
	if(PWM_duty_time>=PWM_duty06) 
	{	PIN_PWM06 = 0; } 
	if(PWM_duty_time>=PWM_duty07) 
	{	PIN_PWM07 = 0; } 
	if(PWM_duty_time>=PWM_duty08) 
	{	PIN_PWM08 = 0; } 
	
	if(PWM_duty_time >= 100)
	{	
		PWM_duty_time=1;	//タイマー初期化
		PIN_PWM01 = 1;
		PIN_PWM02 = 1;
		PIN_PWM03 = 1;
		PIN_PWM04 = 1;
		PIN_PWM05 = 1;
		PIN_PWM06 = 1;
		PIN_PWM07 = 1;
		PIN_PWM08 = 1;
	
	}
} 
//**********Timer1割り込み************* 
void Timer1_isr(void) 
{ 
		PIR1bits.TMR1IF=0;			// タイマ１割り込みフラグを0にする 

		if(--cnt1==0) 
		{			   // cnt1を-1して結果が0？ 
			cnt1=5;					// cnt1にLEDの更新周期を書き戻す 
			if(PIN_LED_2) 
			PIN_LED_2=0;	//LED3を１秒間隔で点滅 
			else
			PIN_LED_2=1; 
		} 
} 
//**********Timer2割り込み************* 
void Timer2_isr(void) 
{ 
		PIR1bits.TMR2IF=0;			// タイマ2割り込みフラグを0にする 
		if(--cnt2==0) 
		{			   // cnt1を-1して結果が0？ 
			cnt2 = 113; 
			if(PIN_LED_3) 
			PIN_LED_3=0;	//LED3を１秒間隔で点滅 
			else
			PIN_LED_3=1; 
		} 
} 
//**********Timer3割り込み************* 
void Timer3_isr(void) 
{ 
		PIR2bits.TMR3IF=0;			// タイマ3割り込みフラグを0にする 
		if(--cnt3==0) 
		{			   // cnt1を-1して結果が0？ 
			cnt3=7; 
			if(PIN_LED_4) 
			PIN_LED_4=0;	//LED3を１秒間隔で点滅 
			else
			PIN_LED_4=1; 
		} 
} 
//**********RB0割り込み************* 
void RB0_isr(void) 
{ 
	INTCONbits.INT0IF = 0;		//割り込みフラグクリア 
	PIN_LED_0=1;	//LED3を１秒間隔で点滅 
	wait_ms(1000); 
	//PIN_LED_1=1; 
} 
//**********RB1割り込み************* 
void RB1_isr(void) 
{ 
	INTCON3bits.INT1IF = 0;		//割り込みフラグクリア 
	PIN_LED_1=1;	//LED3を１秒間隔で点滅 
	wait_ms(1000); 
	//PIN_LED_2=1; 
} 
//**********RB2割り込み************* 
void RB2_isr(void) 
{ 
	INTCON3bits.INT2IF = 0;		//割り込みフラグクリア 
	PIN_LED_2=1;	//LED3を１秒間隔で点滅 
	wait_ms(1000); 
	//PIN_LED_2=1; 
} 
//**********RB PORT変化割り込み************* 
void RB_PORT_isr(void) 
{ 
	int PORT_data = 0; 
	PORT_data = PORTB; 
	INTCONbits.RBIF = 0;		//割り込みフラグクリア 
	  
} 
//**********AD割り込み************* 
void AD_isr(void) 
{ 
	PIR1bits.ADIF = 0;		//割り込みフラグクリア 
	if(--cnt4==0) 
		{			   // cnt1を-1して結果が0？ 
			cnt4=255; 
			if(PIN_LED_5) 
			PIN_LED_5=0;	//LED3を１秒間隔で点滅 
			else
			PIN_LED_5=1; 
		} 
} 
//**********UART送信割り込み************* 
void UART_TX_isr(void) 
{ 
	int k =0; 
	putcUSART(Send_Buf); 
		for(k=0;k>=20;k++) 
		{ 
			Send_Buf[k] = 0; 
		} 
		PIR1bits.TXIF = 0;		//割り込みフラグクリア 
	if(--cnt5==0) 
		{			   // cnt1を-1して結果が0？ 
			cnt5=255; 
			if(PIN_LED_6) 
			PIN_LED_6=0;	//LED3を１秒間隔で点滅 
			else
			PIN_LED_6=1; 
		} 
} 
//**********UART受信割り込み************* 
void UART_RC_isr(void) 
{ 
	int i =0; 
	i++; 
	temp_Buf[i] = getcUSART(); 
	PIR1bits.RCIF = 0;		//割り込みフラグクリア 
	if(--cnt6==0) 
		{			   // cnt1を-1して結果が0？ 
			cnt6=255; 
			if(PIN_LED_7) 
			PIN_LED_7=0;	//LED3を１秒間隔で点滅 
			else
			PIN_LED_7=1; 
		} 
}