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


#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,NOBROWNOUT
//詳細は一番下のメモ書きにて


//クロック速度の指定（20MHz)
#use delay(clock = 20000000)

//RS232Cの設定
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)

//初期設定
#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9

//ここからmain関数
main()
{
	//変数定義
	int cmnd =0; 
	char data[7];
	
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
	
	//２バイトデータのとき
	data[0] = '@';
	data[1] = 0x02;
	data[2] = 0x09;
	data[3] = 0x04;
	data[4] = 0x11;
	data[5] = 0x04;
	data[6] = '*';
	
	
	
		while(1) {					//永久繰り返し
			
			
			/*		２バイトデータ
			delay_ms(1000);
			putc(data[0]);
			putc(data[1]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[6]);
			*/
		
			
			/*
			//１バイトデータのとき
			delay_ms(1000);
			putc(data[0]);
			putc(data[1]);
			putc(data[2]);
			putc(data[3]);
			putc(data[6]);
			
			*/
			
			data[1] = 0x08;
			
			//		8バイトデータ
			delay_ms(500);
			putc(data[0]);
			putc(data[1]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[6]);
			
			
 }
} 
