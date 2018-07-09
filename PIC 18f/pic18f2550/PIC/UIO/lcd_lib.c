///////////////////////////////////////////////
//  液晶表示器制御ライブラリ　for PIC18Fxxxx
//  内蔵関数は以下
//    lcd_init()    ----- 初期化
//    lcd_cmd(cmd)  ----- コマンド出力
//    lcd_data(chr) ----- １文字表示出力
//    lcd_clear()   ----- 全消去
//////////////////////////////////////////////
#include <p18cxxx.h>
#include "io_cfg.h"
#include	"delays.h"
#include "lcd_lib.h"

//////// データ出力サブ関数
void lcd_out(char code, char flag)
{
	lcd_port = code & 0xF0;
	if (flag == 0)
		lcd_rs = 1;			//表示データの場合
	else
		lcd_rs = 0;			//コマンドデータの場合
	Delay1TCY();				//NOP		
	lcd_stb = 1;				//strobe out
	Delay10TCYx(1);			//10NOP
	lcd_stb = 0;				//reset strobe
}
//////// １文字表示関数
void lcd_data(char asci)
{
	lcd_out(asci, 0);			//上位４ビット出力
	lcd_out(asci<<4, 0);		//下位４ビット出力
	Delay10TCYx(50);			//50μsec待ち
}
/////// コマンド出力関数
void lcd_cmd(char cmd)
{
	lcd_out(cmd, 1);			//上位４ビット出力
	lcd_out(cmd<<4, 1);		//下位４ビット出力
	if((cmd & 0x03) != 0)
		Delay10KTCYx(2);		//2msec待ち
	else
		Delay10TCYx(50);		//50usec待ち
}
/////// 全消去関数
void lcd_clear(void)
{
	lcd_cmd(0x01);			//初期化コマンド出力
}

/////// 文字列出力関数
void lcd_str(char *str)
{
	while(*str != 0x00)			//文字列の終わり判定
	{
		lcd_data(*str);			//文字列１文字出力
		str++;					//ポインタ＋１
	}
}

/////// 初期化関数
void lcd_init(void)
{
	Delay10KTCYx(20);
	lcd_out(0x30, 1);			//8bit mode set
	Delay10KTCYx(5);
	lcd_out(0x30, 1);			//8bit mode set
	Delay10KTCYx(1);
	lcd_out(0x30, 1);			//8bit mode set
	Delay10KTCYx(1);
	lcd_out(0x20, 1);			//4bit mode set
	Delay10KTCYx(1);
	lcd_cmd(0x2E);			//DL=0 4bit mode
	lcd_cmd(0x08);			//display off C=D=B=0
	lcd_cmd(0x0D);			//display on C=D=1 B=0
	lcd_cmd(0x06);			//entry I/D=1 S=0
	lcd_cmd(0x01);			//all clear
}
