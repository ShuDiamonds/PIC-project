#include<16f877a.h>
#include<stdio.h>

//#define RS_BAUD		115200
#define RS_BAUD		9600
#define RS_TX		PIN_C6
#define RS_RX		PIN_C7
#define RUN_LED		PIN_C0
//ぴんマクロ
//鉄火巻きモーターI/O
	#define	TEKKA_Mortor1_IO_R	PIN_A2
	#define	TEKKA_Mortor1_IO_L	PIN_A4
	#define	TEKKA_LED			PIN_E1
	#define	TEKKA_NOPIN			PIN_E0
	#define	TEKKA_Mortor2_IO_R	PIN_A3
	#define	TEKKA_Mortor2_IO_L	PIN_A5
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

#use delay(clock = 20000000)
#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,BROWNOUT
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
	//鉄火巻き初期化
	set_pwm1_duty(0);
	set_pwm2_duty(0);
	output_low(TEKKA_Mortor1_IO_L);
	output_low(TEKKA_Mortor1_IO_R);
	output_low(TEKKA_Mortor2_IO_L);
	output_low(TEKKA_Mortor2_IO_R);
	//かっぱ巻き初期化
	output_low(KAPPA_Mortor1_IO_L);
	output_low(KAPPA_Mortor1_IO_R);
	output_low(KAPPA_Mortor2_IO_L);
	output_low(KAPPA_Mortor2_IO_R);
	output_low(KAPPA_Mortor3_IO_L);
	output_low(KAPPA_Mortor3_IO_R);
	output_low(KAPPA_Mortor4_IO_L);
	output_low(KAPPA_Mortor4_IO_R);
	
	
	
	output_low(RUN_LED);   //動作確認
	output_high(TEKKA_LED);   //動作確認
	output_high(KAPPA_LED1);   //動作確認
	output_high(KAPPA_LED2);   //動作確認
	delay_ms(30);
	while(1)
	{
		while(1)
		{	
		//スタートデータ待ち
			while(!cheaker == '@')
			{
				cheaker = getc();
			}
		//モータの数を確認
			motasuu = getc();
		//データ受信
			for(i=0;motasuu > i;i++)
			{
				data_H[i] =getc();
				data_L[i] =getc();
			}
		//ストップデータ確認
			cheaker = getc();
			if(cheaker == '*')
			{
				break;
			}
		//データ復号
			for(i=0;motasuu == i;i++)
				{
					//IDの取り出し
					ID = data_H[i] & 0b11111000;
					ID = ID>>3;
					//符号取り出し
					hugou = data_H[i] & 0b0000100;
					//PWMデータ取り出し
					F = data_H[i] & 0b0000011;		//PWM上位2bit取り出し
					F = F<<8;						//PWMデータを8ビット左にシフト
					E = data_L[i];					
					F = F | E;						//PWMのデータの上位と下位をORする(FにPWM用のデータがはいってる)
					
					//PWMデータ判別
					if(F <=370 && F != 0)			//ストップではなく、PWMが370以下のとき
					{
						F = 420;
					}
					
				//データ実行
					switch (ID)						//モーター判別
					{
				case 0b00000001:					//かっぱモーター１
						if(F)						//モーターが動かす時
						{
							if(hugou)
							{
								printf("かっぱモーター１_R\n\r");
								output_high(KAPPA_Mortor1_IO_R);
								output_low(KAPPA_Mortor1_IO_L);
							}else if(!hugou)
							{
								printf("かっぱモーター１_L\n\r");
								output_high(KAPPA_Mortor1_IO_L);
								output_low(KAPPA_Mortor1_IO_R);
							}
						}else						//モーターを動かさないとき
						{
								printf("かっぱモーター１_else\n\r");
								output_low(KAPPA_Mortor1_IO_L);
								output_low(KAPPA_Mortor1_IO_R);
						}
					break;
				case 0b00000010:					//かっぱモーター２
						if(F)						//モーターが動かす時
						{
							if(hugou)
							{
								printf("かっぱモーター2_R\n\r");
								output_high(KAPPA_Mortor2_IO_R);
								output_low(KAPPA_Mortor2_IO_L);
							}else if(!hugou)
							{
								printf("かっぱモーター2_L\n\r");
								output_high(KAPPA_Mortor2_IO_L);
								output_low(KAPPA_Mortor2_IO_R);
							}
						}else						//モーターを動かさないとき
						{
								printf("かっぱモーター2_else\n\r");
								output_low(KAPPA_Mortor2_IO_L);
								output_low(KAPPA_Mortor2_IO_R);
						}
					break;
				case 0b00000011:					//かっぱモーター３
						if(F)						//モーターが動かす時
						{
							printf("かっぱモーター3_R\n\r");
							if(hugou)
							{
								output_high(KAPPA_Mortor3_IO_R);
								output_low(KAPPA_Mortor3_IO_L);
							}else if(!hugou)
							{
								printf("かっぱモーター3_L\n\r");
								output_high(KAPPA_Mortor3_IO_L);
								output_low(KAPPA_Mortor3_IO_R);
							}
						}else						//モーターを動かさないとき
						{
								printf("かっぱモーター3_else\n\r");
								output_low(KAPPA_Mortor3_IO_L);
								output_low(KAPPA_Mortor3_IO_R);
						}
					break;
				case 0b00000100:					//かっぱモーター４
						if(F)						//モーターが動かす時
						{
							if(hugou)
							{
								printf("かっぱモーター4_R\n\r");
								output_high(KAPPA_Mortor4_IO_R);
								output_low(KAPPA_Mortor4_IO_L);
							}else if(!hugou)
							{
								printf("かっぱモーター4_L\n\r");
								output_high(KAPPA_Mortor4_IO_L);
								output_low(KAPPA_Mortor4_IO_R);
							}
						}else						//モーターを動かさないとき
						{
								printf("かっぱモーター4_else\n\r");
								output_low(KAPPA_Mortor4_IO_L);
								output_low(KAPPA_Mortor4_IO_R);
						}
					break;
				case 0b00000101:					//鉄火巻きモーター１
						if(F)						//モーターが動かす時
						{
							if(hugou)
							{
								printf("tekka1_R\n\r");
								output_high(TEKKA_Mortor1_IO_R);
								output_low(TEKKA_Mortor1_IO_L);
								set_pwm1_duty(F);
							}else if(!hugou)
							{
								printf("tekka１_L\n\r");
								output_high(TEKKA_Mortor1_IO_L);
								output_low(TEKKA_Mortor1_IO_R);
								set_pwm1_duty(F);
							}
						}else						//モーターを動かさないとき
						{
							printf("tekka１_else\n\r");
							set_pwm1_duty(0);
							output_low(TEKKA_Mortor1_IO_L);
							output_low(TEKKA_Mortor1_IO_R);
						}
					break;
				case 0b00000110:					//鉄火巻きモーター２
						if(F)						//モーターが動かす時
						{
							if(hugou)
							{
								printf("tekka2_R\n\r");
								output_high(TEKKA_Mortor2_IO_R);
								output_low(TEKKA_Mortor2_IO_L);
								set_pwm2_duty(F);
							}else if(!hugou)
							{
								printf("tekka2_L\n\r");
								output_high(TEKKA_Mortor2_IO_L);
								output_low(TEKKA_Mortor2_IO_R);
								set_pwm2_duty(F);
							}
						}else						//モーターを動かさないとき
						{
							printf("tekka2_else\n\r");
							set_pwm2_duty(0);
							output_low(TEKKA_Mortor2_IO_L);
							output_low(TEKKA_Mortor2_IO_R);
						}
					break;
				default:
					break;
					
					}
					
				}
		//変数初期化
			cheaker=0;
			motasuu=0;
		}
	}
	return(0);
}