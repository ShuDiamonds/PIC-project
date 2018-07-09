//ヘッダファイルの読み込み
#include <16f88.h>		
#include <stdio.h>			//標準入出力のヘッダファイル

// 入出力ピンの設定
#define TRIS_A	0x00		//ALL,OUT
#define TRIS_B	0x04		//ALL,OUT

//ピンのdefine
#define RUN_LED PIN_A1

//RS232Cの設定コマンド
#define RS_BAUD		9600	//Baud-Reatは9600bps
#define RS_TX		PIN_B5	//TXピンはPIN_C6
#define RS_RX		PIN_B2	//RXピンはPIN_C7


//コンフィギュレーションビットの設定
#fuses HS,NOWDT,NOPUT,NOPROTECT,NOBROWNOUT,NOLVP
//詳細は一番下のメモ書きにて


//クロック速度の指定（20MHz)
#use delay(clock = 20000000)

//RS232Cの設定
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)

//初期設定




//ここからmain関数
main()
{
	long int i = 0;
	char j = 0;

		
	
		//初期化
		set_tris_a(TRIS_A);
		set_tris_b(TRIS_B);
		

	printf("start");
	output_low(RUN_LED);						//動作確認用LEDを光らす
	delay_ms(50);
	while(1)
	{
			delay_ms(1000);
		output_high(RUN_LED);
			printf("yahoo");
		delay_ms(1000);
		output_low(RUN_LED);
		//j = getc();
		printf("yahoo");
		//printf("Hell, sofmap would!!No.%ld\n\r",i);
		//i++;
	}
}




/*ｃ⌒っﾟдﾟ)っφ ﾒﾓﾒﾓ...
コンフィグレーションビットについて
HS(High Speed)
NOWDT(no-Watch Dog Time)
NOPROTECT
NOLVP(no Low-Voltage-Programming)
PUT(Power-Up-Timer)
BROWNOUT*/