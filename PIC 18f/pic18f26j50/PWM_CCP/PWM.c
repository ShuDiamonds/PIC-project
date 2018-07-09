#include <p18f26j50.h>	  // PIC18F26j50のヘッダ・ファイル
#include <delays.h>
#include <usart.h>

//***** コンフィギュレーションの設定config

	#pragma config WDTEN = OFF			//WDT disabled (enabled by SWDTEN bit)
	#pragma config PLLDIV = 2			//Divide by 2 (8 MHz intl osc input)
	#pragma config STVREN = ON			//stack overflow/underflow reset enabled
	#pragma config XINST = OFF			//Extended instruction set disabled
	#pragma config CPUDIV = OSC1		//No CPU system clock divide
	#pragma config CP0 = OFF			//Program memory is not code-protected
	#pragma config OSC = INTOSCPLL		//Internal oscillator, PLL enenabled
	#pragma config T1DIG = OFF			//S-Osc may not be selected, unless T1OSCEN = 1
	#pragma config LPT1OSC = OFF		//high power Timer1 mode
	#pragma config FCMEN = OFF			//Fail-Safe Clock Monitor disabled
	#pragma config IESO = OFF			//Two-Speed Start-up disabled
	#pragma config WDTPS = 32768		//1:32768
	#pragma config DSWDTOSC = INTOSCREF //DSWDT uses INTOSC/INTRC as clock
	#pragma config RTCOSC = T1OSCREF	//RTCC uses T1OSC/T1CKI as clock
	#pragma config DSBOREN = OFF		//Zero-Power BOR disabled in Deep Sleep
	#pragma config DSWDTEN = OFF		//Disabled
	#pragma config DSWDTPS = 8192		//1:8,192 (8.5 seconds)
	#pragma config IOL1WAY = OFF		//IOLOCK bit can be set and cleared
	#pragma config MSSP7B_EN = MSK7		//7 Bit address masking
	#pragma config WPFP = PAGE_1		//Write Protect Program Flash Page 0
	#pragma config WPEND = PAGE_0		//Start protection at page 0
	#pragma config WPCFG = OFF			//Write/Erase last page protect Disabled
	#pragma config WPDIS = OFF			//WPFP[5:0], WPEND, and WPCFG bits ignored 


		
//---Wait---//
//48Mhz駆動の時
#define WAIT_MS Delay1KTCYx(12)// Wait 1ms
#define WAIT_US Delay10TCYx(12)	// Wait 1us

//=======wait[ms]======//
void wait_ms(unsigned long int t) {
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
	int i;
	//クロック初期化
		//48Mhzの設定
	OSCTUNEbits.PLLEN = 1;		  // PLLを起動
	wait_ms(2);
	
	//初期化
		TRISA=0x00;						   // ポートAをすべて出力ピンにする
		TRISB=0;						// ポートBをすべて出力ピンにする
		TRISC=0x00;						   // ポートCをすべて出力ピンにする

	//PWM設定
	RPOR3 = 14;					   // CCP1/P1AをRP3(RB0)に指定
	RPOR4 = 18;					   // CCP2/P2AをRP4(RB1)に指定
	OpenPWM1(0xff); //PWM1の周期を11.7KHzでをオープン
	OpenPWM2(0xff); //PWM2の周期を11.7KHzでオープン

	while(1)
	{
		for (i = 0; i < 1024; i++)
		{
			SetDCPWM1(i); //PWM1のDuty変更
			SetDCPWM2(1023 - i); //PWM2のDuty変更
			wait_ms(1); //5msec待つ
		}

		
	}
}

