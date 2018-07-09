//////////////////////////////////////////////////
// ＰＷＭテストプログラム１											//
// PIC16F873								RIKIYA 2002.12.15  //
//															picpwm1.C  //
// TMR1をつかってサーボーモータ用のPWM波形を出力  //
// CCP2コンペアマッチ割込で周期20mSを生成			  //
// CCP1コンペアマッチ割込でパルス幅を生成				//
//////////////////////////////////////////////////
#define	SorvoPIN1	PIN_C2
#define	SorvoPIN2	PIN_B2

#include <16f877a.h>
#fuses HS,NOWDT,NOPROTECT,PUT,BROWNOUT,NOLVP
#use delay(clock = 20000000)					   // clock 20MHz
#use fast_io(C)											 // 固定入力モード

void ccp1_int(void);									 // プロトタイプ
void ccp2_int(void);									 // プロトタイプ
void intval();


//メイン関数////////////////////////////////////
main()
{
	set_tris_a(0x00);								  
	set_tris_b(0x00);
	set_tris_c(0x00);								  //RC 7-0:OUTs
	set_tris_d(0x00);
	
	setup_timer_1(T1_INTERNAL | T1_DIV_BY_2);
	setup_ccp1(CCP_COMPARE_INT);		  // CCP1コンペアマッチ割込み設定
	setup_ccp2(CCP_COMPARE_INT);		  // CCP2コンペアマッチ割込み設定
	set_timer1(15535);						
	
	CCP_1 = 50000;									  // パルス幅 0
	CCP_2 = 50000;							   // 周期 0.2u * 2 * 50000 = 20mS
	
	enable_interrupts(INT_CCP1);			   //CCP1コンペアマッチチ割込み許可
	enable_interrupts(INT_CCP2);			   //CCP2コンペアマッ	// TMR1クリア
	enable_interrupts(INT_TIMER1);
	enable_interrupts(GLOBAL);			  //全設定割込み許可

	while(1)
	{
			CCP_1 = 18035;						  //パルス幅 1.5mS
			delay_ms(1000);
			CCP_1 = 19285;						  //パルス幅 1.0mS
			delay_ms(1000);
			CCP_1 = 20535;						  //パルス幅 2.0mS
			delay_ms(1000);
	}
}

//パルスクリア///////////////////////////////////
#INT_CCP1
void ccp1_int()
{
	  output_bit(SorvoPIN1,0);
}

//パルス出力/////////////////////////////////////
#INT_CCP2
void ccp2_int()
{
	output_bit(SorvoPIN2,0);
}
//	timer1割り込み関数
#int_timer1
void intval() 
{
	
	set_timer1(15535);
	output_bit(SorvoPIN1,1);
	output_bit(SorvoPIN2,1);
	
}

/*	備考
周期を20msにするために、timer1の値に15535を入れているので、
そこからカウントアップされる。
つまりサーボの角度を0度にしようとすれば、1msにすればよいので、
1000us=0.2us * 2 * x より、x=2500 カウント必要となる。
よって、15535 + 2500 = 18035 の値をCCP_1に入れればよい

同様に90度するには
1500 = 0.4 * x
x=3750
よって、3750 + 15535 = 19285

また180度の場合は
2000 = 0.4 * x
5000 + 15535 = 20535

*/
