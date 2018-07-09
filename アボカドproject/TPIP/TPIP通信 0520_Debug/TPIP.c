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


//関数宣言
void LED_tikatika( unsigned int16 a)
{
	output_high(RUN_LED);   //動作確認
	delay_ms(a);
	output_low(RUN_LED);   //動作確認
	delay_ms(a);
	output_high(RUN_LED);   //動作確認
	delay_ms(a);
	output_low(RUN_LED);   //動作確認
	delay_ms(a);
	output_high(RUN_LED);   //動作確認
	
	return;	
}
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
	char data_H[10];
	char data_L[10];		//受信データ格納スペース
	int16 E=0,F=0;				
	int ID=0,hugou=0,i=0;
	char motasuu1=0;
	int motasuu = 0;
	//初期化
	set_tris_a(0x01);
	set_tris_b(0x00);
	set_tris_c(0x80);
	set_tris_d(0x00);
	set_tris_e(0x00);
	ADCON1 = 0b00000111;		//デジタルピン設定	
	//PWM初期化
	/*
	setup_ccp1(CCP_PWM);
	setup_ccp2(CCP_PWM);
	SETUP_TIMER_2(T2_DIV_BY_16,0xFF,1);
	*/
	//モーター初期化
	//鉄火巻き初期化
	/*
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
	
	//LEDちかちか動作確認
	LED_tikatika(500);
	
	//output_high(TEKKA_LED);   //動作確認
	output_high(KAPPA_LED1);   //動作確認
	output_high(KAPPA_LED2);   //動作確認
	output_high(KAPPA2_LED1);   //動作確認
	output_high(KAPPA2_LED2);   //動作確認
	
	//delay_ms(30);
	while(1)		//アイドルループ
	{
		//printf("1\n\r");
		while(1)			//受信ループ
		{	
		//スタートデータ待ち
		//	printf("2\n\r");
			while(cheaker != '@')
			{
				//printf("3\n\r");
				if(kbhit())
				{
				cheaker = getc();
				}
			}
			//LED_tikatika(500);
			//printf("\n\r receive StartBit \n\r");
		//モータの数を確認
			motasuu1 = getc();
			motasuu = motasuu1;
		//データ受信
			for(i=1;motasuu >= i;i++)
			{
				//printf("4\n\r");
				data_H[i] =getc();
				data_L[i] =getc();
			}
		//ストップデータ確認
			cheaker = getc();
			
			if(cheaker != '*')
			{
				putc('N');
				//printf("\n\r No stop bit so breaked \n\r");
				break;
			}
			
		//データ復号
			for(i=1;motasuu >= i;i++)
				{
					//printf("\n\r ID convert \n\r");
					//IDの取り出し
					ID = data_H[i] & 0b11111000;
					ID = ID>>3;
					//符号取り出し
					hugou = data_H[i] & 0b0000100;
					hugou = hugou>>2;
					//PWMデータ取り出し
					F = data_H[i] & 0b0000011;		//PWM上位2bit取り出し
					F = F<<8;						//PWMデータを8ビット左にシフト
					E = data_L[i];					
					F = F | E;						//PWMのデータの上位と下位をORする(FにPWM用のデータがはいってる)
					
					//受信データ初期化
					data_H[i]=0;
					data_L[i]=0;
					
					
					/*
					//PWMデータ判別
					if(F <=370 && F != 0)			//ストップではなく、PWMが370以下のとき
					{
						printf("6\n\r");
						//printf("\n\r Less PWM data \n\r");
						F = 420;
					}
					*/
					
					
					printf("\n\r switching \n\r");
				//データ実行
					switch (ID)						//モーター判別
					{
						case 0x01:					//かっぱモーター１
								if(F)						//モーターが動かす時
								{
									
									if(hugou)
									{
										printf("\n\r kappa moter1 hugou=1 \n\r");
										output_high(KAPPA_Mortor1_IO_R);
										output_low(KAPPA_Mortor1_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter1 hugou=0 \n\r");
										output_high(KAPPA_Mortor1_IO_L);
										output_low(KAPPA_Mortor1_IO_R);
										break;
									}
								}else						//モーターを動かさないとき
								{
										printf("\n\r kappa moter1 No action \n\r");
										output_low(KAPPA_Mortor1_IO_L);
										output_low(KAPPA_Mortor1_IO_R);
									break;
								}
						case 0x02:					//かっぱモーター２
								if(F)						//モーターが動かす時
								{
									if(hugou)
									{
										printf("\n\r kappa moter2 hugou=1 \n\r");
										output_high(KAPPA_Mortor2_IO_R);
										output_low(KAPPA_Mortor2_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter2 hugou=0 \n\r");
										output_high(KAPPA_Mortor2_IO_L);
										output_low(KAPPA_Mortor2_IO_R);
										break;
									}
								}else						//モーターを動かさないとき
								{
										printf("\n\r kappa moter2 No action \n\r");
										output_low(KAPPA_Mortor2_IO_L);
										output_low(KAPPA_Mortor2_IO_R);
									break;
								}
						case 0x03:					//かっぱモーター３
								if(F)						//モーターが動かす時
								{
									if(hugou)
									{
										printf("\n\r kappa moter3 hugou=1 \n\r");
										output_high(KAPPA_Mortor3_IO_R);
										output_low(KAPPA_Mortor3_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter3 hugou=0 \n\r");
										output_high(KAPPA_Mortor3_IO_L);
										output_low(KAPPA_Mortor3_IO_R);
										break;
									}
								}else						//モーターを動かさないとき
								{
										printf("\n\r kappa moter3 No action \n\r");
										output_low(KAPPA_Mortor3_IO_L);
										output_low(KAPPA_Mortor3_IO_R);
									break;
								}
						case 0x04:					//かっぱモーター４
								if(F)						//モーターが動かす時
								{
									if(hugou)
									{
										printf("\n\r kappa moter4 hugou=1 \n\r");
										output_high(KAPPA_Mortor4_IO_R);
										output_low(KAPPA_Mortor4_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter4 hugou=0 \n\r");
										output_high(KAPPA_Mortor4_IO_L);
										output_low(KAPPA_Mortor4_IO_R);
										break;
									}
								}else						//モーターを動かさないとき
								{
										printf("\n\r kappa moter4 No action \n\r");
										output_low(KAPPA_Mortor4_IO_L);
										output_low(KAPPA_Mortor4_IO_R);
									break;
								}
						
						//ここからかっぱポート2番目
						case 0x05:					//かっぱモーター5
								if(F)						//モーターが動かす時
								{
									if(hugou)
									{
										printf("\n\r kappa moter5 hugou=1 \n\r");
										output_high(KAPPA2_Mortor1_IO_R);
										output_low(KAPPA2_Mortor1_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter5 hugou=0 \n\r");
										output_high(KAPPA2_Mortor1_IO_L);
										output_low(KAPPA2_Mortor1_IO_R);
										break;
									}
								}else						//モーターを動かさないとき
								{
									printf("\n\r kappa moter5 No action \n\r");
										output_low(KAPPA2_Mortor1_IO_L);
										output_low(KAPPA2_Mortor1_IO_R);
									break;
								}
						
						case 0x06:					//かっぱモーター6
								if(F)						//モーターが動かす時
								{
									if(hugou)
									{
										printf("\n\r kappa moter6 hugou=1 \n\r");
										output_high(KAPPA2_Mortor2_IO_R);
										output_low(KAPPA2_Mortor2_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter6 hugou=0 \n\r");
										output_high(KAPPA2_Mortor2_IO_L);
										output_low(KAPPA2_Mortor2_IO_R);
										break;
									}
								}else						//モーターを動かさないとき
								{
									printf("\n\r kappa moter6 No action \n\r");
										output_low(KAPPA2_Mortor2_IO_L);
										output_low(KAPPA2_Mortor2_IO_R);
									break;
								}
						
						case 0x07:					//かっぱモーター7
								if(F)						//モーターが動かす時
								{
									if(hugou)
									{
										printf("\n\r kappa moter7 hugou=1 \n\r");
										output_high(KAPPA2_Mortor3_IO_R);
										output_low(KAPPA2_Mortor3_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter7 hugou=0 \n\r");
										output_high(KAPPA2_Mortor3_IO_L);
										output_low(KAPPA2_Mortor3_IO_R);
										break;
									}
								}else						//モーターを動かさないとき
								{
									printf("\n\r kappa moter7 No action \n\r");
										output_low(KAPPA2_Mortor3_IO_L);
										output_low(KAPPA2_Mortor3_IO_R);
									break;
								}
						
						case 0x08:					//かっぱモーター8
								if(F)						//モーターが動かす時
								{
									if(hugou)
									{
										printf("\n\r kappa moter8 hugou=1 \n\r");
										output_high(KAPPA2_Mortor4_IO_R);
										output_low(KAPPA2_Mortor4_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter8 hugou=0 \n\r");
										output_high(KAPPA2_Mortor4_IO_L);
										output_low(KAPPA2_Mortor4_IO_R);
										break;
									}
								}else						//モーターを動かさないとき
								{
									printf("\n\r kappa moter8 No action \n\r");
										output_low(KAPPA2_Mortor4_IO_L);
										output_low(KAPPA2_Mortor4_IO_R);
									break;
								}						
						
						
						
						default:	printf("\n\r default ID \n\r");
							break;
					
					}
					
				}	//for文抜け出し
			
			
		//変数初期化
			printf("\n\r format of data \n\r");
			cheaker=0;
			motasuu=0;
			motasuu1=0;
			 E=0;
			F=0;
			ID=0;
			hugou=0;
			i=0;
		}
		printf("\n\r End of one communication \n\r");		//正常終了した場合実行されない
	}		//アイドルループ終了
	
	printf("\n\r End main \n\r");
	return(0);
}

/*参考	getcについて
機能: RS-232C RCV ピンから文字を読み込み、文字を返します。
この関数は文字入力が有るまで待ち続けます。この状態を避けたい場合は、この関数の使用直前で
関数kbhit()を使って文字入力可能か否かのテストをして下さい。
もし、内蔵USART 機能があれば、ハードウエアは３文字をバッファすることが出来ます。
無ければ、PIC によって文字が受信されている間、文字の取りこぼしを無くす為、GETC で取り込
みを継続する必要があります。fgetc()使用時は、指定されたストリームからの入力をします。
getc()のデフォルト・ストリームはSTDIN です。若しくは、プログラム的にこれ以前で使用された、
ストリームが指定されたと見なして処理します。
*/


