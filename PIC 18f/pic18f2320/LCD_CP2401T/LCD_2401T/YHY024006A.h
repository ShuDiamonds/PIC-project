/*=======================================================================================
Programmer:     kidonaru
Date:           12-19-2011
Hardware:       PIC18F4620[clock:40MHz(10x4)]
Description:    YHY024006A Functions
=======================================================================================*/
#ifndef YHY024006A_H
#define YHY024006A_H

//#include <p18f4620.h>
#include <p18f2320.h>
#include <adc.h>

#ifndef uint
#define uint unsigned int
#endif

#ifndef uchar
#define uchar unsigned char
#endif

#ifndef PATT16
#define PATT16 unsigned int
#endif

#ifndef PATT8
#define PATT8 unsigned char
#endif

#ifndef COLOR16
#define COLOR16 unsigned int
#endif

#ifndef TRUE
#define TRUE 1
#endif

#ifndef FALSE
#define FALSE 0
#endif

//======Define======//

//---Color Sumple---//
#define RED			0xf800
#define GREEN		0x07e0
#define BLUE		0x001f
#define BLACK		0x0000
#define WHITE		0xffff
#define GRAY		0x8c51
#define YELLOW		0xFFE0
#define CYAN		0x07FF
#define PURPLE		0xF81F

//---PATT (image data pattern)---//
#define PATT_HEADER_SIZE	3            // 
#define PATT_BIT(patt)		patt[0]        // 
#define PATT_W(patt)		patt[1]        // 
#define PATT_H(patt)		patt[2]        // 
#define PATT_DATA(patt,x,y)	patt[PATT_HEADER_SIZE + (y) * PATT_W(patt) + (x)]

//---Port Setting---//
//#define LCD_DATA_OUTPUT TRISE = 0x00
#define LCD_DATA_OUTPUT TRISC = 0x00
#define LCD_PORT_OUTPUT TRISB = 0x00


#define Res PORTCbits.RC4
#define Dc PORTCbits.RC3
#define Wr PORTCbits.RC2

 // •ÏX
 /*
#define Res PORTEbits.RE0
#define Dc PORTEbits.RE2
#define Wr PORTEbits.RE1
*/
#define LCD_DATA PORTB

#define SETUP_AD OpenADC(ADC_FOSC_16 & ADC_RIGHT_JUST & ADC_8_TAD,\
			ADC_CH0 & ADC_INT_OFF & ADC_REF_VDD_VSS, 0x0b)
#define XL PORTAbits.RA5
#define YD PORTAbits.RA4
#define XL_IO TRISAbits.RA5
#define YD_IO TRISAbits.RA4
#define XR PORTCbits.RC1
#define YU PORTCbits.RC0
#define XR_IO TRISCbits.RC1
#define YU_IO TRISCbits.RC0
#define SETUP_XR XL_IO=1; XR_IO=1; YD_IO=0; YD=1; YU_IO=0; YU=0
#define SETUP_YU YD_IO=1; YU_IO=1; XL_IO=0; XL=1; XR_IO=0; XR=0
#define GET_XR(out) SetChanADC(ADC_CH1);ConvertADC();wait_us(400);while(BusyADC());out=ReadADC()
#define GET_YU(out) SetChanADC(ADC_CH0);ConvertADC();wait_us(400);while(BusyADC());out=ReadADC()

//---Lcd Display Data---//
#define LCD_W 240
#define LCD_H 320

//---Wait---//
#define WAIT_MS Delay10KTCYx(1)	// Wait 1ms
#define WAIT_US Delay10TCYx(1)	// Wait 1us

//---Function---//
#define ABS(num) (((num) > 0)?(num):-(num))	// My abs Function


//======Function Prototype======//
void wait_ms(int t);
COLOR16 Get_Color(uchar red, uchar green, uchar blue);
COLOR16 Gradation(COLOR16 start, COLOR16 end, int max, int t);
void Lcd_Init();
void Trans_Dat_16(uint data);
void Lcd_Set_Area(int x, int y, int w, int h);
void Lcd_Set_Pos(int x, int y);
void Lcd_Draw_Patt(rom PATT16* patt, int x, int y);
void Lcd_Clear(COLOR16 color);
void Lcd_Set_Font(rom PATT8* patt);
void Lcd_Set_Font_Sub_Color(COLOR16 color);
void Lcd_Draw_Char(const char c);
void Lcd_Draw_String(char* str);
void Lcd_Draw_String_Rom(rom char* str);
void Lcd_printf(int x, int y, COLOR16 color, char *str, ...);
void Lcd_printf_Rom(int x, int y, COLOR16 color, rom char *str, ...);
int Lcd_Get_Touch(int *x, int *y);
void Lcd_Draw_XLine(int x1, int x2, int y, COLOR16 color);
void Lcd_Draw_YLine(int y1, int y2, int x, COLOR16 color);
void Lcd_Draw_Dot(int x, int y, COLOR16 color);
void Lcd_Draw_Line(int x, int y, int xx, int yy, COLOR16 color);
void Lcd_Draw_Circle(int xx, int yy, int r, COLOR16 color);
void Lcd_Draw_Circle2(int xx, int yy, int r, COLOR16 color);
void Lcd_Draw_Box(int x, int y, int w, int h, COLOR16 color);
void Lcd_Draw_Box2(int x, int y, int w, int h, COLOR16 color);

#endif