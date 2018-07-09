#include <p18f2320.h>
#include <pwm.h>
#include <delays.h>

//config設定
#pragma config OSC = INTIO2 //内部発振器を利用
#pragma config WDT = OFF //ウォッチドッグタイマOFF
#pragma config MCLRE = OFF //MCLRを内部プルアップ
#pragma config LVP = OFF //低電圧ICSP制御OFF
//その他のconfigはデフォルト設定のまま

void main(void)
{
	unsigned int i; //使用変数の定義
	
	//初期化
	PORTC = 0b00000000; //PortCの中身をきれいにする
	OSCCON = 0b01110000; //内蔵発振器を8MHz使用に設定
	TRISC = 0b00000000; //PortC 8個全て0:出力設定

	PORTCbits.RC3 = 1; //照度比較用LED ON
	OpenPWM1(255); //PWM1をオープン
	OpenPWM2(255); //PWM2をオープン
	
	while(1)
	{
		//LEDの明るさを徐々に変える(1024分解能)
		for (i = 0; i < 1024; i++)
		{
			SetDCPWM1(i); //PWM1のDuty変更
			SetDCPWM2(1023 - i); //PWM2のDuty変更
			Delay10KTCYx(1); //5msec待つ
		}
	}
}
