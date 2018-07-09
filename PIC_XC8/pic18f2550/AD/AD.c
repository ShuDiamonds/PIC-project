#include <p18f2550.h>
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <adc.h>

//***** コンフィギュレーションの設定config


		#pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
		#pragma config CPUDIV   = OSC1_PLL2   
		#pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
		#pragma config FOSC     = HSPLL_HS
		#pragma config FCMEN    = OFF
		#pragma config IESO     = OFF
		#pragma config PWRT     = OFF
		#pragma config BOR      = ON
		#pragma config BORV     = 3
		#pragma config VREGEN   = ON      //USB Voltage Regulator
		#pragma config WDT      = OFF
		#pragma config WDTPS    = 32768
		#pragma config MCLRE    = ON
		#pragma config LPT1OSC  = OFF
		#pragma config PBADEN   = OFF
		//      #pragma config CCP2MX   = ON
		#pragma config STVREN   = ON
		#pragma config LVP      = OFF
		//      #pragma config ICPRT    = OFF       // Dedicated In-Circuit Debug/Programming
		#pragma config XINST    = OFF       // Extended Instruction Set
		#pragma config CP0      = OFF
		#pragma config CP1      = OFF
		//      #pragma config CP2      = OFF
		//      #pragma config CP3      = OFF
		#pragma config CPB      = OFF
		//      #pragma config CPD      = OFF
		#pragma config WRT0     = OFF
		#pragma config WRT1     = OFF
		//      #pragma config WRT2     = OFF
		//      #pragma config WRT3     = OFF
		#pragma config WRTB     = OFF       // Boot Block Write Protection
		#pragma config WRTC     = OFF
		//      #pragma config WRTD     = OFF
		#pragma config EBTR0    = OFF
		#pragma config EBTR1    = OFF
		//      #pragma config EBTR2    = OFF
		//      #pragma config EBTR3    = OFF
		#pragma config EBTRB    = OFF

//***** メイン関数
void main(void)                     // メイン関数
{
	//ローカル変数定義
	char Message1[10]="\rStart!!\n";
	char Message2[7]="FUKUDA";
	long int data=0;
	float DATA=0;
	
	//UART初期化
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 77); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600の時で64にする。また48Mhzで9600の時で77にする
	//AD変換初期化
	//define mode of A/D
	//OpenADC(ADC_FOSC_16 & ADC_RIGHT_JUST & ADC_8ANA_0REF,ADC_CH0 & ADC_INT_OFF);
	
		OpenADC(ADC_FOSC_64 &           //AD変換用クロック　　システムクロックの1/64　0.05μsec×64＝3.2μsec　>=　1.6μsec　→　OK
				ADC_RIGHT_JUST &        //変換結果の保存方法　左詰め　
				ADC_8_TAD,              //AD変換のアクイジションタイム選択　3.2μsec（=1Tad）×8Tad=25.6μsec　＞＝　12.8μsec　→　OK
				ADC_CH0 &                       //AD変換するのチャンネル選択（PIC18Fは同時に複数のAD変換はできない）
				ADC_INT_OFF &           //AD変換での割込み使用の有無
				ADC_VREFPLUS_VDD &      //Vref+の設定　　　ＰＩＣの電源電圧と同じ：ADC_VREFPLUS_VDD 　or　外部（ＡＮ３）の電圧：ADC_VREFPLUS_EXT
				ADC_VREFMINUS_VSS,      //Vref-の設定　　　ＰＩＣの0Ｖ：ADC_VREFMINUS_VSS    or　外部（AN2)の電圧：ADC_VREFMINUS_EXT
				0b1110  //ポートのアナログ・デジタル選択　（ADCON1の下位４ビットを記載）　　AN0のみアナログポートを選択、他はデジタルポートを選択
				//例 　アナログポートが　AN0のみ → 0b1110　　、AN0 & AN1　→　0b1011、 AN0 & AN1 & AN2 →1100　他　詳細データシート参照
		);

	
	SetChanADC(ADC_CH0);	//Select Channel 0
	//初期化
		TRISA=0x0F;                        // ポートAを半分出力ピンにする
		TRISB=0;                        // ポートBをすべて出力ピンにする
		TRISC=0b10111111;                        // ポートCをすべて出力ピンにする
	putsUSART(Message1);
	printf("\rHello world\n");		//「Hello」と出力
	while(1)
	{
		
		
		SetChanADC(ADC_CH0);	//Select Channel 0
		Delay100TCYx(5);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		data = ReadADC();	//Get A/D data
		
		
		printf("\rdata=%ld\n",data);		
		
		/*
		//電圧表示部分
		printf("\rdata=%ld\n",data);
		data16 = data*4882;
		data32 = data16/1000000;
		data64 = data16%1000000;
		printf("\rVCC =%ld.%ld\n",data32,data64);
		
		*/
		
		/*
		
		//グラフ化部分
		data = data / 1;
		for(;data>0;data--)
		{
			printf("*");
		}
		printf("\r\n");
		//printf("\rdata=%ld\n",data);		
		
		
		*/
		
		/*
	//	float型のUARTでの出力は無理らしい
		DATA = data * 0.0488;
		printf("\rDATA=%f\n",DATA);
		*/
		
		/*
		Delay10KTCYx(200);		//1秒待つ
		Delay10KTCYx(200);
		Delay10KTCYx(200);
		Delay10KTCYx(200);
		Delay10KTCYx(200);
		
		*/
		
		
		
	}
	//UART終了
	//CloseUSART( );
	//AD変換終了
	//CloseADC();

}
