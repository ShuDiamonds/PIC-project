#include <p18f2320.h>    // PIC18C452のヘッダ・ファイル
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <adc.h>

//***** コンフィギュレーションの設定config


#pragma	config	OSC = HSPLL
#pragma	config	FSCM = OFF
#pragma	config	IESO = OFF
#pragma	config	PWRT = OFF
#pragma	config	BOR = OFF
#pragma	config	BORV = 20
#pragma	config	WDT = OFF
#pragma	config	WDTPS = 1
#pragma	config	MCLRE = ON
#pragma	config	PBAD = DIG
//	#pragma	config	PBAD = ANA
//	#pragma	config	CCP2MX = C1
#pragma	config	STVR = ON
#pragma	config	LVP = ON
#pragma	config	DEBUG = OFF
#pragma	config	CP0 = OFF
#pragma	config	CP1 = OFF
#pragma	config	CP2 = OFF
#pragma	config	CP3 = OFF
#pragma	config	CPB = OFF
#pragma	config	CPD = OFF
#pragma	config	WRT0 = OFF
#pragma	config	WRT1 = OFF
#pragma	config	WRT2 = OFF
#pragma	config	WRT3 = OFF
#pragma	config	WRTB = OFF
#pragma	config	WRTC = OFF
#pragma	config	WRTD = OFF
#pragma	config	EBTR0 = OFF
#pragma	config	EBTR1 = OFF
#pragma	config	EBTR2 = OFF
#pragma	config	EBTR3 = OFF
#pragma	config	EBTRB = OFF
		

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
			USART_CONT_RX & USART_BRGH_LOW, 64); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600
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
		TRISA=0x0F;                        // ポートAをすべて出力ピンにする
		TRISB=0;                        // ポートBをすべて出力ピンにする
		TRISC=0b10111111;                        // ポートCをすべて出力ピンにする
	putsUSART(Message1);
	//fprintf(_H_USART,"\rHello world\n");		//「Hello」と出力
	while(1)
	{
		SetChanADC(ADC_CH0);	//Select Channel 0
		Delay100TCYx(5);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		data = ReadADC();	//Get A/D data
		
		fprintf(_H_USART,"\rData=%ld\n",data);		
		
		/*
		float型のUARTでの出力は無理らしい
		DATA = data * 0.0488;
		fprintf(_H_USART,"\rDATA=%f\n",DATA);
		*/
		Delay10KTCYx(200);		//1秒待つ
		Delay10KTCYx(200);
		Delay10KTCYx(200);
		Delay10KTCYx(200);
		Delay10KTCYx(200);
		
	}
	//UART終了
	//CloseUSART( );
	//AD変換終了
	//CloseADC();

}
