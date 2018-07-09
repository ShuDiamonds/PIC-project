#include <12f683.h>
#fuses INTRC_IO,NOWDT,NOCPD,NOPROTECT,PUT,NOMCLR,NOBROWNOUT
#use delay(clock = 8000000)					 // clock 8MHz
void ccp1_int(void);						 // プロトタイプ
void intval();
//********PINマクロ
#byte GP=5			//GPは0～5の6ポート
#bit SorvoPIN=GP.1		//サーボ用PIN 
#bit AD_PIN=GP.0		//可変抵抗接続
#bit M1=GP.5		//モーター信号線１
#bit M2=GP.2		//モーター信号線２
#bit SW1=GP.4		//タクトスイッチ１
#bit SW2=GP.3		//タクトスイッチ２
//****************//
#define HIGH 1
#define LOW 0
#define TimerTIME 45536
//メイン関数////////////////////////////////////
main()
{
	//ポート設定
	//set_tris_a(0b00011001);
	set_tris_a(0);	
	//内部クロック指定
	setup_oscillator(OSC_8MHZ);
/*	//AD変換器初期化
	setup_adc_ports(0b11111110);
	setup_adc(ADC_CLOCK_DIV_32);
	//サーボ用PWM初期化
	setup_timer_1( T1_INTERNAL| T1_DIV_BY_2);	//1カウントに1usかかる
	setup_ccp1(CCP_COMPARE_INT);		  // CCP1コンペアマッチ割込み設定
	set_timer1(TimerTIME);						//周期を20msに設定
	
	CCP_1 = 1500+TimerTIME;									  // パルス幅 0	
	enable_interrupts(INT_CCP1);			   //CCP1コンペアマッチチ割込み許可
	enable_interrupts(INT_TIMER1);
	enable_interrupts(GLOBAL);			  //全設定割込み許可
*/
	
	SorvoPIN = HIGH;
	delay_ms(500);
	SorvoPIN = LOW;
	delay_ms(500);
	SorvoPIN = HIGH;
	delay_ms(500);
	SorvoPIN = LOW;
	delay_ms(500);
	
	while(1)
	{
		//set_adc_channel(0);  //0チャンネルを使用
		/*
			CCP_1 = 1500+TimerTIME;						  //パルス幅 1.5mS
			delay_ms(1000);
			CCP_1 = 1000+TimerTIME;						  //パルス幅 1.0mS
			delay_ms(1000);
			CCP_1 = 2000+TimerTIME;						  //パルス幅 2.0mS
			delay_ms(1000);
		*/
		
		if(SW1 == HIGH)
		{
			SorvoPIN = HIGH;
		}
		
		if(SW2 == HIGH)
		{
			SorvoPIN = LOW;
		}
		
	}
}

//パルスクリア///////////////////////////////////
#INT_CCP1
void ccp1_int()
{
	SorvoPIN=LOW;
}

//	timer1割り込み関数
#int_timer1
void intval() 
{
	
	set_timer1(TimerTIME);
	SorvoPIN=HIGH;
	
}

/*	備考
周期を20msにするために、timer1の値に45536を入れているので、
そこからカウントアップされる。
つまりサーボの角度を0度にしようとすれば、1msにすればよいので、
1カウント 1usなので　1000カウント必要
よって、45536 + 1000 値をCCP_1に入れればよい


*/
