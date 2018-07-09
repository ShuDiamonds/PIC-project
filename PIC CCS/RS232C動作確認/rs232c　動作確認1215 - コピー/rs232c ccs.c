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
main() {
		//変数定義
		char  cmnd =0; 
		
	
		//初期化
		set_tris_a(TRIS_A);
		set_tris_b(TRIS_B);
		

	printf("Hello world\n\r");
		while(1) 
		{					//永久繰り返し
			delay_ms(30);
			printf("\nCommand= ");		//Command=表示	
			cmnd=getc();				//１文字入力
			printf(" Input= ");			//Input=表示
			putc(cmnd);					//入力文字表示
		}
} 
