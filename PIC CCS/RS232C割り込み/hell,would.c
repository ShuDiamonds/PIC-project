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

//グローバル変数定義
char data = 0;

//RS232C割り込み
#int_rda
void isr_rcv()
{
	data = getc();
	return;
}
//ここからmain関数
void main(void)
{
	long int i = 0;
	char j = 0;

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
	//割り込み許可
	enable_interrupts(int_rda);
	enable_interrupts(global);
	printf("start\n\r");
	output_low(RUN_LED);						//動作確認用LEDを光らす
	delay_ms(50);
	while(1)
	{
			delay_ms(1000);
		output_high(RUN_LED);
			printf("\n\rdata = ");
		putc(data);
		delay_ms(1000);
		output_low(RUN_LED);
	}
}

//参考URL	http://amahime.main.jp/sirial/main.php?name=siri


/*ｃ⌒っﾟдﾟ)っφ ﾒﾓﾒﾓ...
コンフィグレーションビットについて
HS(High Speed)
NOWDT(no-Watch Dog Time)
NOPROTECT
NOLVP(no Low-Voltage-Programming)
PUT(Power-Up-Timer)
BROWNOUT*/