/////////////////////////////////////////////////
//　This program is test program of standard io.
//　PIC16F84 is target PIC and drive a LCD.
//　The PORT for RS232 is below.
//　　送信=RA3　受信=RA4
/////////////////////////////////////////////////
//ヘッダファイルの読み込み
#include <16f877a.h>		//16F873のヘッダファイル
#include <stdio.h>			//標準入出力のヘッダファイル

// 入出力ピンの設定
#define TRIS_A	0x00		//ALL,OUT
#define TRIS_B	0x00		//ALL,OUT
#define TRIS_C	0x80		//PIN_C7:IN
#define TRIS_D	0x00		//ALL,OUT
#define TRIS_E	0x00		//ALL,OUT

//ピンのdefine
#define RUN_LED PIN_C3

//RS232Cの設定コマンド
#define RS_BAUD		9600	//Baud-Reatは9600bps
#define RS_TX		PIN_C6	//TXピンはPIN_C6
#define RS_RX		PIN_C7	//RXピンはPIN_C7


//コンフィギュレーションビットの設定
#fuses HS,NOWDT,NOPROTECT
//詳細は一番下のメモ書きにて


//クロック速度の指定（20MHz)
#use delay(clock = 20000000)

//RS232Cの設定
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)

//初期設定

#use fast_io(a)
#use fast_io(b)
#use fast_io(c)
#use fast_io(d)
#use fast_io(e)

#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9

//ここからmain関数
main() {
		//変数定義
		char cmnd =0; 
		
	
		//初期化
		set_tris_a(TRIS_A);
		set_tris_b(TRIS_B);
		set_tris_c(TRIS_C);
		set_tris_d(TRIS_D);
		set_tris_e(TRIS_E);
		
		port_a = 0;
		port_b = 0;
		//port_c = 0;
		port_d = 0;
		port_e = 0;
	
		while(1) 
	{					//永久繰り返し
		delay_ms(30);
		printf("\nCommand= ");		//Command=表示	
		cmnd=getc();				//１文字入力
		printf(" Input= ");			//Input=表示
		putc(cmnd);					//入力文字表示
 	}
} 
