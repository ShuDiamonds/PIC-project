/*************************************************
*  グラフィック液晶表示器用ライブラリ　ヘッダ
*		ポートの定義
*		関数プロトタイプ
**************************************************/

/* LCD Ports define */
#define LCD_DBL		PORTE
#define LCD_DBH 	PORTB
#define LCD_TRISL 	TRISE
#define LCD_TRISH 	TRISB
#define LCD_TRISF 	TRISF
#define LCD_TRISD 	TRISD
#define LCD_CS1		LATFbits.LATF6
#define	LCD_CS2		LATFbits.LATF5
#define	LCD_E		LATEbits.LATE4
#define	LCD_RW		LATEbits.LATE5
#define	LCD_DI		LATDbits.LATD0


/***  関数プロトタイプ ****/
void lcd_Write(char cs, char code, char DIflag);
char lcd_Read(char cs);
void lcd_Init(void);
void lcd_Clear(char data);
void lcd_Pixel(int Xpos, int Ypos, char On);
void lcd_Char(char line, char colum, int letter);
void lcd_Char1(char line, char colum, int letter);
void lcd_Str(char line, char colum, rom const char *s);
void lcd_StrRam(char line, char colum, char *s);
void lcd_Line(int x0, int y0, int x1, int y1);
void lcd_Scroll(int delay);
void lcd_Image(char *ptr);
void Delay200n(void);
void Delay1u(int time);
void Delay1m(int time);




