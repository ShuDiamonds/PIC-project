
#include<16f877a.h>
//#include<stdio.h>

//#define RS_BAUD		115200
#define RS_BAUD		9600
#define RS_TX		PIN_C6
#define RS_RX		PIN_C7
#define RUN_LED		PIN_C0
//ぴんマクロ
/*
//鉄火巻きモーターI/O
	#define	TEKKA_Mortor1_IO_R	PIN_A2
	#define	TEKKA_Mortor1_IO_L	PIN_A4
	#define	TEKKA_LED			PIN_E1
	#define	TEKKA_NOPIN			PIN_E0
	#define	TEKKA_Mortor2_IO_R	PIN_A3
	#define	TEKKA_Mortor2_IO_L	PIN_A5
	*/
	//モーターPWM
	#define	TEKKA_Mortor1_PWM	PIN_C1
	#define	TEKKA_Mortor2_PWM	PIN_C2
	//かっぱ巻きモーターI/O
	#define	KAPPA_LED1			PIN_D5	
	#define	KAPPA_LED2			PIN_D4	
	#define	KAPPA_Mortor1_IO_R	PIN_D0	
	#define	KAPPA_Mortor1_IO_L	PIN_C3
	#define	KAPPA_Mortor2_IO_R	PIN_D2	
	#define	KAPPA_Mortor2_IO_L	PIN_D3	
	#define	KAPPA_Mortor3_IO_R	PIN_C4	
	#define	KAPPA_Mortor3_IO_L	PIN_C5	
	#define	KAPPA_Mortor4_IO_R	PIN_D1	
	#define	KAPPA_Mortor4_IO_L	PIN_A0	
	//かっぱ巻きモーター2I/O
	#define	KAPPA2_LED1			PIN_B6	
	#define	KAPPA2_LED2			PIN_B7	
	#define	KAPPA2_Mortor1_IO_R	PIN_D6
	#define	KAPPA2_Mortor1_IO_L	PIN_D7
	#define	KAPPA2_Mortor2_IO_R	PIN_B0	
	#define	KAPPA2_Mortor2_IO_L	PIN_B1	
	#define	KAPPA2_Mortor3_IO_R	PIN_B2	
	#define	KAPPA2_Mortor3_IO_L	PIN_B3	
	#define	KAPPA2_Mortor4_IO_R	PIN_B4	
	#define	KAPPA2_Mortor4_IO_L	PIN_B5	


#use delay(clock = 20000000)
#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,NOBROWNOUT
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)  //rs232c設定
/*
#use fast_io(a)
#use fast_io(b)
#use fast_io(c)
#use fast_io(d)
#use fast_io(e)
*/
#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9
#byte ADCON1 = 0x9F			//アナログデジタルピン設定	
main()
{
	//ローカル変数定義
	char cheaker=0;	
	int data_H[20];
	int data_L[20];		//受信データ格納スペース
	int16 E=0,F=0;				
	int ID=0,hugou=0,i=0;
	int motasuu=0;
	
	//初期化
	set_tris_a(0x01);
	set_tris_b(0x00);
	set_tris_c(0x80);
	set_tris_d(0x00);
	set_tris_e(0x00);
	ADCON1 = 0b00000111;		//デジタルピン設定	
	//PWM初期化
	setup_ccp1(CCP_PWM);
	setup_ccp2(CCP_PWM);
	SETUP_TIMER_2(T2_DIV_BY_16,0xFF,1);
	
	//モーター初期化
	/*
	
	//鉄火巻き初期化
	set_pwm1_duty(0);
	set_pwm2_duty(0);
	output_low(TEKKA_Mortor1_IO_L);
	output_low(TEKKA_Mortor1_IO_R);
	output_low(TEKKA_Mortor2_IO_L);
	output_low(TEKKA_Mortor2_IO_R);
	*/
	//かっぱ巻き初期化
	output_low(KAPPA_Mortor1_IO_L);
	output_low(KAPPA_Mortor1_IO_R);
	output_low(KAPPA_Mortor2_IO_L);
	output_low(KAPPA_Mortor2_IO_R);
	output_low(KAPPA_Mortor3_IO_L);
	output_low(KAPPA_Mortor3_IO_R);
	output_low(KAPPA_Mortor4_IO_L);
	output_low(KAPPA_Mortor4_IO_R);
	//かっぱ巻き2初期化
	output_low(KAPPA2_Mortor1_IO_L);
	output_low(KAPPA2_Mortor1_IO_R);
	output_low(KAPPA2_Mortor2_IO_L);
	output_low(KAPPA2_Mortor2_IO_R);
	output_low(KAPPA2_Mortor3_IO_L);
	output_low(KAPPA2_Mortor3_IO_R);
	output_low(KAPPA2_Mortor4_IO_L);
	output_low(KAPPA2_Mortor4_IO_R);
	
	output_high(RUN_LED);   //動作確認
	delay_ms(500);
	output_low(RUN_LED);   //動作確認
	delay_ms(500);
	output_high(RUN_LED);   //動作確認
	delay_ms(500);
	output_low(RUN_LED);   //動作確認
	delay_ms(500);
	output_high(RUN_LED);   //動作確認
//	output_high(TEKKA_LED);   //動作確認
	output_high(KAPPA_LED1);   //動作確認
	output_high(KAPPA_LED2);   //動作確認
	output_high(KAPPA2_LED1);   //動作確認
	output_high(KAPPA2_LED2);   //動作確認
	//delay_ms(30);
	while(1)
	{
		
		//かっぱ１ポート１目
		output_high(KAPPA_Mortor1_IO_R);
		output_low(KAPPA_Mortor1_IO_L);
		
		output_high(KAPPA_Mortor2_IO_R);
		output_low(KAPPA_Mortor2_IO_L);
		
		
		output_high(KAPPA_Mortor3_IO_R);
		output_low(KAPPA_Mortor3_IO_L);
		
		output_high(KAPPA_Mortor4_IO_R);
		output_low(KAPPA_Mortor4_IO_L);
		
		//かっぱ２ポート２目
		output_high(KAPPA2_Mortor1_IO_R);
		output_low(KAPPA2_Mortor1_IO_L);
		
		output_high(KAPPA2_Mortor2_IO_R);
		output_low(KAPPA2_Mortor2_IO_L);
		
		
		output_high(KAPPA2_Mortor3_IO_R);
		output_low(KAPPA2_Mortor3_IO_L);
		
		output_high(KAPPA2_Mortor4_IO_R);
		output_low(KAPPA2_Mortor4_IO_L);
		
		
	}
	
}
