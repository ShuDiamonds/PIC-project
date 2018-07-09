/***********************************************
*  グラフィックLCD(128ｘ64ドット）用ライブラリ
*	lcd_Write();
*	lcd_Read();
*	lcd_Init();
*	lcd_Clear();
*	lcd_Pixel();
*	lcd_Char();
*	lcd_Char1();
*	lcd_Str();
*	lcd_Line();
*	lcd_Image();
************************************************/
#include <p18F67J60.h>
#include "glcd_lib2.h"
#include "font.h"

/***************************
 データ出力関数 
***************************/
void lcd_Write(char cs, char code, char DIflag){
	char data;

	LCD_RW = 0;						// write mode
	if(cs==1)
		LCD_CS1 = 0;
	else
		LCD_CS2 = 0;
	data = code;
	LCD_DBL = (LCD_DBL & 0xF0) | ((data) & 0x0F);
	LCD_DBH = (LCD_DBH & 0xF0) | ((data >> 4) & 0x0F);
	if (DIflag == 0)
		LCD_DI = 1;					// 表示データの場合 
	else
		LCD_DI = 0;					// コマンドデータの場合
	Delay1u(1);						// 1usec
	LCD_E = 1;						// strobe out
	Delay1u(1);						// pulse width 1usec
	LCD_E = 0;						// reset strobe
	LCD_CS1 = 1;
	LCD_CS2 = 1;
	LCD_RW = 1;
	Delay1u(5);
}
/*****************************
* データ読み出し関数　
*****************************/
char lcd_Read(char cs){
	char data;

	LCD_TRISH = 0xFF;						// chane to input mode
	LCD_TRISL = 0x0F;
	LCD_RW = 1;							// set read mode
	if(cs==1)							// CS1 or CS2?
		LCD_CS1 = 0;
	else
		LCD_CS2 = 0;
	LCD_DI = 1;							// set data mode
	Delay1u(1);							// 1usec delay
	LCD_E = 1;							// strobe out
	Delay1u(1);							// pulse width 1usec
	LCD_E = 0;							// reset strobe
	Delay1u(3);							// wait 3usec
	data = (LCD_DBH << 4)& 0xF0;		// input data
	data = data | ((LCD_DBL) & 0x0F);	
	Delay1u(1);							// wait 1usec
	LCD_CS1 = 1;						// reset CS
	LCD_CS2 = 1;
	LCD_TRISH = 0xF0;						// back to output mode
	LCD_TRISL = 0x00;
	return(data);					// return cuurrent status
}
/***************************
* 画面消去関数
****************************/
void lcd_Clear(char data){
	char page, colum;	
	
	for(page=0; page<8; page++){			// repeat 8 page
		lcd_Write(1, 0xB8+page, 1);			// page set
		lcd_Write(1, 0x40, 1);				// colum reset
		lcd_Write(2, 0xB8+page, 1);			// page set 
		lcd_Write(2, 0x40, 1);				// colum reset
		for(colum=0; colum<64; colum++){	// repeat 64 colum	
			lcd_Write(1, data, 0);			// fill data
			lcd_Write(2, data, 0);			// fill data
		}
	}
	lcd_Write(1, 0xC0, 1);					// reset start line
	lcd_Write(2, 0xC0, 1);
}	
/****************************
* 初期化関数  
*****************************/
void lcd_Init(void){
	LCD_TRISL = 0x00;
	LCD_TRISH = 0xF0;
	LCD_TRISD = 0x00; 
	LCD_TRISF = 0x0F;
	Delay1m(10);
	lcd_Write(1, 0x3F, 1);					// Display on
	lcd_Write(2, 0x3F, 1);					// Display on		
	lcd_Clear(0);
}
/****************************
*  Draw Piccel Function
*  座標は(0,0)-(127,63)  
*****************************/
void lcd_Pixel(int Xpos, int Ypos, char On){
	char cs, data, page, pos, count, i;

	/* if colum >127 then do nothing  */
	if(Xpos<128){
		if(Xpos>63){						// 64=<colum<=127?
			Xpos = Xpos-64;					// shift 64 dot
			cs = 1;
		}
		else
			cs = 2;
		page = (char)(7-Ypos/8);			// set page
		lcd_Write(cs, 0xB8+page, 1);			
		lcd_Write(cs, 0x40+Xpos, 1);		// set colum 
		data = lcd_Read(cs);				// get current data
		lcd_Write(cs, 0x40+Xpos, 1);		// set colum ???? 
		data = lcd_Read(cs);				// get current data	????	
		pos =1;								// set bit position
		count = (char)(7-Ypos%8);			// set bit
		for(i=0; i<count; i++)				// caluculate 2^n
			pos *= 2;
		lcd_Write(cs, 0x40+Xpos, 1);		// back address
		if(On==1)							// set or reset bit
			lcd_Write(cs, data | pos, 0);	// set 1
		else
			lcd_Write(cs, data & ~pos, 0);	// set 0
	}
}
/***************************
*  直線描画関数
***************************/
#define	abs(a)	(((a)>0) ? (a) : -(a))
void lcd_Line(int x0, int y0, int x1, int y1)
{
	int steep, t;
	int	deltax, deltay, error;
	int x, y;
	int ystep;
	
	/// 差分の大きいほうを求める
	steep = (abs(y1 - y0) > abs(x1 - x0));
	/// ｘ、ｙの入れ替え
	if(steep){
		t = x0; x0 = y0; y0 = t;
		t = x1; x1 = y1; y1 = t;
	}
	if(x0 > x1) {
		t = x0; x0 = x1; x1 = t;
		t = y0; y0 = y1; y1 = t;
	}
	deltax = x1 - x0;						// 傾き計算	
	deltay = abs(y1 - y0);
	error = 0;
	y = y0;
	/// 傾きでステップの正負を切り替え
	if(y0 < y1) ystep = 1; else ystep = -1;
	/// 直線を点で描画
	for(x=x0; x<x1; x++) {
		if(steep) lcd_Pixel(y,x,1); else lcd_Pixel(x,y,1);
		error += deltay;
		if((error << 1) >= deltax) {
			y += ystep;
			error -= deltax;
		}
	}
}

/*************************
*  文字表示関数
*  (0, 0) - (7, 15)
**************************/
void lcd_Char(char line, char colum, int letter){
	char cs, i;
	int pos;
	  
	if(colum < 16){
		if(colum > 7){
			pos = (colum- 8) * 8;
		  	cs = 1;
		}
		else{
		  	pos = colum * 8;
		  	cs = 2;
		}
		lcd_Write(cs, 0xB8+line, 1);		// set page
		lcd_Write(cs, 0x40+pos, 1);			// set colum
		for(i=0; i<5; i++)
			lcd_Write(cs, Font[letter-0x20][i], 0);
		lcd_Write(cs, 0, 0);
		lcd_Write(cs, 0, 0);
		lcd_Write(cs, 0, 0);
	}
}
/*************************
*  文字表示関数2 7x8
*  (0, 0) - (7, 17)
**************************/
void lcd_Char1(char line, char colum, int letter){
	char cs, i;
	int pos;
	  
	if(colum < 18){
		if(colum > 8){
			pos = (colum- 9) * 7;
		  	cs = 1;
		}
		else{
		  	pos = colum * 7;
		  	cs = 2;
		}
		lcd_Write(cs, 0xB8+line, 1);		// set page
		lcd_Write(cs, 0x40+pos, 1);			// set colum
		for(i=0; i<5; i++)
			lcd_Write(cs, Font[letter-0x20][i], 0);
		lcd_Write(cs, 0, 0);
		lcd_Write(cs, 0, 0);
	}
}
/******************************
*   文字列描画関数
******************************/
void lcd_Str(char line, char colum, rom const char *s)
{
    while (*s)
        lcd_Char1(line, colum++, *s++);
}
void lcd_StrRam(char line, char colum, char *s)
{
    while (*s)
        lcd_Char1(line, colum++, *s++);
}

/*****************************
* イメージ表示関数
*****************************/
void lcd_Image(char *ptr)
{
	char cs, Xpos;
	int page, colum;

	for(page=0; page<8; page++){
		for(colum=0; colum<128; colum++){
			if(colum > 63){
				Xpos=colum-64;
				cs = 1;
			}
			else{
				Xpos = colum;
				cs = 2;
			}
			lcd_Write(cs, 0xB8+page, 1);
			lcd_Write(cs, 0x40+Xpos, 1);
			lcd_Write(cs, *ptr++, 0);
		}
	}
}
/*********************************
* スクロール関数
***********************************/	  
void lcd_Scroll(int delay){
	int i;
	
	for(i=0; i<64; i++){
		lcd_Write(1, 0xC0+i,1);
		lcd_Write(2, 0xC0+i,1);
		Delay1m(delay);
	}
}
/******************************************
* 遅延関数群
******************************************/	
/** 1msec Delay SubFunction at 10MIPS ***/
void Delay1m(int time){
	while(time > 0){
		Delay1u(1000);
		time--;
	}
}
/** 1usec Delay SubFunction at 10MIPS ****/
void Delay1u(int time){
	do{
		time--;
	}while(time > 0);
}

