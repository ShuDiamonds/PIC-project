//ヘッダファイルの読み込み
#include <16f877a.h>		//16F873のヘッダファイル
#include <stdio.h>			//標準入出力のヘッダファイル

//ピンのdefine
#define RUN_LED PIN_C3

//RS232Cの設定コマンド
#define RS_BAUD		9600	//Baud-Reatは9600bps
#define RS_TX		PIN_C6	//TXピンはPIN_C6
#define RS_RX		PIN_C7	//RXピンはPIN_C7


//コンフィギュレーションビットの設定
#fuses HS,NOWDT,NOPROTECT
//詳細は一番下のメモ書きにて


//クロック速度の指定（20MHz)
#use delay(clock = 20000000)

//RS232Cの設定
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)

//初期設定

#use fast_io(a)
#use fast_io(b)
#use fast_io(c)
#use fast_io(d)
#use fast_io(e)

#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9
//自作PWM関数設定
		#define	TimerPWMcount	15				//200.200usecおきに割り込み
	//	#warning	クロック速度20MHz
//グローバル変数定義
unsigned int PWM_duty01=0, PWM_duty02=0, PWM_duty03=0, PWM_duty04=0, PWM_duty05=0, PWM_duty06=0, PWM_duty07=0, PWM_duty08=0;
unsigned int PWM_duty_time = 1;
//PWMピンマクロ
#define	PIN_PWM01	PIN_B0
#define	PIN_PWM02	PIN_B1
#define	PIN_PWM03	PIN_B2
#define	PIN_PWM04	PIN_B3
#define	PIN_PWM05	PIN_B4
#define	PIN_PWM06	PIN_B5
#define	PIN_PWM07	PIN_B6
#define	PIN_PWM08	PIN_B7


#INT_RTCC
rtcc_isr()
{
	
	set_timer0(TimerPWMcount);				//200.000usecおきに割り込み
	PWM_duty_time++;
	if(PWM_duty_time>=PWM_duty01) 
	{	output_low(PIN_PWM01); } 
	if(PWM_duty_time>=PWM_duty02) 
	{	output_low(PIN_PWM02); } 
	if(PWM_duty_time>=PWM_duty03) 
	{	output_low(PIN_PWM03); } 
	if(PWM_duty_time>=PWM_duty04) 
	{	output_low(PIN_PWM04); } 
	if(PWM_duty_time>=PWM_duty05) 
	{	output_low(PIN_PWM05); } 
	if(PWM_duty_time>=PWM_duty06) 
	{	output_low(PIN_PWM06); } 
	if(PWM_duty_time>=PWM_duty07) 
	{	output_low(PIN_PWM07); } 
	if(PWM_duty_time>=PWM_duty08) 
	{	output_low(PIN_PWM08); } 
	
	if(PWM_duty_time >= 100)
	{	
		PWM_duty_time=1;	//タイマー初期化
		output_high(PIN_PWM01);
		output_high(PIN_PWM02);
		output_high(PIN_PWM03);
		output_high(PIN_PWM04);
		output_high(PIN_PWM05);
		output_high(PIN_PWM06);
		output_high(PIN_PWM07);
		output_high(PIN_PWM08);
	}
}
main()
{
	//初期化
	set_tris_a(0);
	set_tris_b(0);
	set_tris_c(0b0100000);
	set_tris_d(0);
	set_tris_e(0);
	port_a = 0;
	port_b = 0;
	//port_c = 0;
	port_d = 0;
	port_e = 0;
	output_low(RUN_LED);						//動作確認用LEDを光らす
	//自作PWM初期化
	output_high(PIN_PWM01);
	output_high(PIN_PWM02);
	output_high(PIN_PWM03);
	output_high(PIN_PWM04);
	output_high(PIN_PWM05);
	output_high(PIN_PWM06);
	output_high(PIN_PWM07);
	output_high(PIN_PWM08);
	//タイマー初期化
	setup_counters(RTCC_8_BIT,RTCC_DIV_4);
	
	set_timer0(TimerPWMcount);						//カウント値のロード
	enable_interrupts(INT_RTCC);				//timer0割り込み許可
	enable_interrupts(GLOBAL);					//グローバル割り込み許可
	while(1)
	{
		PWM_duty01=100;
		PWM_duty02=50;
		PWM_duty03=10;
		PWM_duty04=1;
		PWM_duty05=0;
		PWM_duty06=0;
		PWM_duty07=0;
		PWM_duty08=0;
	}
}

