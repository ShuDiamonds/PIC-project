#include<16f877a.h>
#include<stdio.h>

//#define RS_BAUD		115200
#define RS_BAUD		9600
#define RS_TX		PIN_C6
#define RS_RX		PIN_C7
#define RUN_LED		PIN_C0

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
	set_pwm1_duty(700);
	
	output_low(RUN_LED);   //動作確認
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
			for(i=0;motasuu == i;i++)
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
					
				//データ実行
					switch (ID)						//モーター判別
					{
				case 0x00000001:
					break;
				case 0x00000010:
					break;
				case 0x00000011:
					break;
				case 0x00000100:
					break;
				case 0x00000101:
					break;
				case 0x00000110:
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