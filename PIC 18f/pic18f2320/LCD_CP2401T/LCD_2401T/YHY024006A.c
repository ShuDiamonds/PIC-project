#include <stdio.h>
#include "YHY024006A.h"

rom PATT8* font_patt;
COLOR16 font_color;
COLOR16 font_sub_color;
int str_base_x;
int str_base_y;
int str_cur_x;
int str_cur_y;

char buf[22];

//=======wait[ms]======//
void wait_ms(int t) {
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[us]======//
void wait_us(int t) {
	while(t--) {
		WAIT_US;
	}
}

//======Write reg. command======//
/*********関数説明************
16ビットのデータをシフトして
2回に分けて送っている

******************************/
void Trans_Com_16(uint data) {
    Dc=0;
    LCD_DATA = data>>8;
    Wr=0;
    Wr=1;
    LCD_DATA = data;
    Wr=0;
    Wr=1;
    Dc=1;
}


//======Write reg. data======//
/*********関数説明************





******************************/
void Trans_Dat_16(uint data) {
    LCD_DATA = data>>8;
    Wr=0;
    Wr=1;
    LCD_DATA = data;
    Wr=0;
    Wr=1;
}

//======Get Touch Possition======//
/*********関数説明************





******************************/




int Lcd_Get_Touch(int *x, int *y) {
    int x1, x2, y1, y2;
    int xmax, xmin, xlen;
    int ymax, ymin, ylen;

    //---Get Position---//
    do{
	    SETUP_XR;
	    wait_us(200);
        GET_XR(y1);
        SETUP_YU;
    	wait_us(200);
        GET_YU(x1);
        
        SETUP_XR;
        wait_us(200);
        GET_XR(y2);
        SETUP_YU;
        wait_us(200);
        GET_YU(x2);
        
    }while(ABS(x2-x1) > 3 || ABS(y2-y1) > 3);
    
    *y = (y1+y2) / 2;
    *x = (x1+x2) / 2;
    
    //---Fit Display Size---//
    //Initialize XY Data
    xmin=140;
    ymin=135;
    xmax=680;
    ymax=810;

    //No Touch
    if (*x < xmin && *y < ymin) {
        *x = 0;
        *y = 0;
        return 0;
    }

    //Position Calc
    xlen = xmax-xmin;
    ylen = ymax-ymin;

    *x = 240 - ((long)(*x-xmin)*240) / xlen;
    *y = 320 - ((long)(*y-ymin)*320) / ylen;
    
    //Minus to Zero
    if(*x < 0)
    	*x = 0;
    if(*y < 0)
    	*y = 0;
    
    return 1;
}

//====== RGB to Color16 ======//
COLOR16 Get_Color(uchar red, uchar green, uchar blue) {
	COLOR16 color = 0x0000;
	
	color += ((COLOR16)red * 0x20 / 0x100) << 11;
	color += ((COLOR16)green * 0x40 / 0x100) << 5;
	color += ((COLOR16)blue * 0x20 / 0x100);
	
	return color;
}

//======Gradation Edit======
COLOR16 Gradation(COLOR16 start, COLOR16 end, int max, int t) {
	COLOR16 ret = 0;
	
	//red
	ret += ((start&0xf800) +
		((long)(end&0xf800) - (start&0xf800)) * t / max)&0xf800;
	//green
	ret += ((start&0x07E0) +
		((long)(end&0x07E0) - (start&0x07E0)) * t / max)&0x07E0;
	//red
	ret += ((start&0x001f) +
		((long)(end&0x001f) - (start&0x001f)) * t / max)&0x001f;
	
	return ret;
}

//======LCD Initialize======//
void Lcd_Init() {
    SETUP_AD;
    LCD_DATA_OUTPUT;		//ポート初期化
    LCD_PORT_OUTPUT;		//ポート初期化
    
    Wr = 1;
    Dc = 1;

    str_base_x = str_base_y = 0;
    str_cur_x = str_cur_y = 0;

    //---Lcd Reset---//
	Res=1;
    wait_ms(1);
    Res=0;
    wait_ms(5);
    Res=1;
    wait_ms(10);
	
	//-----------------------------------------//
	// aitendo Demo Code
	// www.aitendo.co.jp/product/1975
	//-----------------------------------------//
	//OTM3225A-C + HYDIS 2.4"QVGA
	//VCC 2.8V
	Trans_Com_16(0x00E3); Trans_Dat_16(0x3008); // Set internal timing
	Trans_Com_16(0x00E7); Trans_Dat_16(0x0012); // Set internal timing
	Trans_Com_16(0x00EF); Trans_Dat_16(0x1231); // Set internal timing
	Trans_Com_16(0x0001); Trans_Dat_16(0x0100); // set SS and SM bit
	Trans_Com_16(0x0002); Trans_Dat_16(0x0700); // set 1 line inversion
	Trans_Com_16(0x0003); Trans_Dat_16(0x1030); // set GRAM write direction and BGR=1.
	Trans_Com_16(0x0004); Trans_Dat_16(0x0000); // Resize register
	Trans_Com_16(0x0008); Trans_Dat_16(0x0207); // set the back porch and front porch
	Trans_Com_16(0x0009); Trans_Dat_16(0x0000); // set non-display area refresh cycle ISC[3:0]
	Trans_Com_16(0x000A); Trans_Dat_16(0x0000); // FMARK function
	Trans_Com_16(0x000C); Trans_Dat_16(0x0000); // RGB interface setting
	Trans_Com_16(0x000D); Trans_Dat_16(0x0000); // Frame marker Position
	Trans_Com_16(0x000F); Trans_Dat_16(0x0000); // RGB interface polarity
	//--------------Power On sequence --------------//
	Trans_Com_16(0x0010); Trans_Dat_16(0x0000); // SAP); Trans_Dat_16(BT[3:0]); Trans_Dat_16(AP); Trans_Dat_16(DSTB); Trans_Dat_16(SLP); Trans_Dat_16(STB
	Trans_Com_16(0x0011); Trans_Dat_16(0x0007); // DC1[2:0]); Trans_Dat_16(DC0[2:0]); Trans_Dat_16(VC[2:0]
	Trans_Com_16(0x0012); Trans_Dat_16(0x0000); // VREG1OUT voltage
	Trans_Com_16(0x0013); Trans_Dat_16(0x0000); // VDV[4:0] for VCOM amplitude
	wait_ms(200); // Dis-charge capacitor power voltage
	Trans_Com_16(0x0010); Trans_Dat_16(0x1490); // SAP); Trans_Dat_16(BT[3:0]); Trans_Dat_16(AP); Trans_Dat_16(DSTB); Trans_Dat_16(SLP); Trans_Dat_16(STB
	Trans_Com_16(0x0011); Trans_Dat_16(0x0227); // R11h=0x0221 at VCI=3.3V); Trans_Dat_16(DC1[2:0]); Trans_Dat_16(DC0[2:0]); Trans_Dat_16(VC[2:0]
	wait_ms(50); // Delayms 50ms
	Trans_Com_16(0x0012); Trans_Dat_16(0x001c); // External reference voltage= Vci;
	wait_ms(50); // Delayms 50ms
	Trans_Com_16(0x0013); Trans_Dat_16(0x0A00); // R13=0F00 when R12=009E;VDV[4:0] for VCOM amplitude
	Trans_Com_16(0x0029); Trans_Dat_16(0x000F); // R29=0019 when R12=009E;VCM[5:0] for VCOMH//0012//
	Trans_Com_16(0x002B); Trans_Dat_16(0x000D); // Frame Rate = 91Hz
	wait_ms(50); // Delayms 50ms
	Trans_Com_16(0x0020); Trans_Dat_16(0x0000); // GRAM horizontal Address
	Trans_Com_16(0x0021); Trans_Dat_16(0x0000); // GRAM Vertical Address
	// ----------- Adjust the Gamma Curve ----------//
	Trans_Com_16(0x0030); Trans_Dat_16(0x0000);
	Trans_Com_16(0x0031); Trans_Dat_16(0x0203);
	Trans_Com_16(0x0032); Trans_Dat_16(0x0001);
	Trans_Com_16(0x0035); Trans_Dat_16(0x0205);
	Trans_Com_16(0x0036); Trans_Dat_16(0x030C);
	Trans_Com_16(0x0037); Trans_Dat_16(0x0607);
	Trans_Com_16(0x0038); Trans_Dat_16(0x0405);
	Trans_Com_16(0x0039); Trans_Dat_16(0x0707);
	Trans_Com_16(0x003C); Trans_Dat_16(0x0502);
	Trans_Com_16(0x003D); Trans_Dat_16(0x1008);
	//------------------ Set GRAM area ---------------//
	Trans_Com_16(0x0050); Trans_Dat_16(0x0000); // Horizontal GRAM Start Address
	Trans_Com_16(0x0051); Trans_Dat_16(0x00EF); // Horizontal GRAM End Address
	Trans_Com_16(0x0052); Trans_Dat_16(0x0000); // Vertical GRAM Start Address
	Trans_Com_16(0x0053); Trans_Dat_16(0x013F); // Vertical GRAM Start Address
	Trans_Com_16(0x0060); Trans_Dat_16(0xA700); // Gate Scan Line
	Trans_Com_16(0x0061); Trans_Dat_16(0x0001); // NDL,VLE); Trans_Dat_16(REV
	Trans_Com_16(0x006A); Trans_Dat_16(0x0000); // set scrolling line
	//-------------- Partial Display Control ---------//
	Trans_Com_16(0x0080); Trans_Dat_16(0x0000);
	Trans_Com_16(0x0081); Trans_Dat_16(0x0000);
	Trans_Com_16(0x0082); Trans_Dat_16(0x0000);
	Trans_Com_16(0x0083); Trans_Dat_16(0x0000);
	Trans_Com_16(0x0084); Trans_Dat_16(0x0000);
	Trans_Com_16(0x0085); Trans_Dat_16(0x0000);
	//-------------- Panel Control -------------------//
	Trans_Com_16(0x0090); Trans_Dat_16(0x0010);
	Trans_Com_16(0x0092); Trans_Dat_16(0x0600);//0x0000
	Trans_Com_16(0x0093); Trans_Dat_16(0x0003);
	Trans_Com_16(0x0095); Trans_Dat_16(0x0110);
	Trans_Com_16(0x0097); Trans_Dat_16(0x0000);
	Trans_Com_16(0x0098); Trans_Dat_16(0x0000);
	Trans_Com_16(0x0007); Trans_Dat_16(0x0133); // 262K color and display ON

	Trans_Com_16(0x0022);
}

//======Set GRAM Write Area======//
void Lcd_Set_Area(int x, int y, int w, int h) {
    Trans_Com_16(0x0050);
    Trans_Dat_16(x);		//Horizontal GRAM Start Address
    Trans_Com_16(0x0051);
    Trans_Dat_16(x+w-1);	// Horizontal GRAM End Address
    Trans_Com_16(0x0052);
    Trans_Dat_16(y);		// Vertical GRAM Start Address
    Trans_Com_16(0x0053);
    Trans_Dat_16(y+h-1);	// Vertical GRAM Start Address

    Trans_Com_16(0x0020);
    Trans_Dat_16(x);		// GRAM horizontal Address
    Trans_Com_16(0x0021);
    Trans_Dat_16(y);		// GRAM Vertical Address
    Trans_Com_16(0x0022);
}

//======Set GRAM Write Possition(Address)======//
void Lcd_Set_Pos(int x, int y) {
    Trans_Com_16(0x0020);
    Trans_Dat_16(x);		//GRAM horizontal Address
    Trans_Com_16(0x0021);
    Trans_Dat_16(y);		//GRAM Vertical Address
    Trans_Com_16(0x0022);
}

//======Clear GRAM======//
void Lcd_Clear(COLOR16 color) {
    int i, j;
    Lcd_Set_Area(0, 0, LCD_W, LCD_H);

    for (i=0; i<LCD_H; i++) {
	    for (j=0; j<LCD_W; j++) {
	        Trans_Dat_16(color);
	    }
    }
}

//======Draw Pattern Data======//
void Lcd_Draw_Patt(rom PATT16* patt, int x, int y) {
    int i, j;
    int w = PATT_W(patt);
    int h = PATT_H(patt);

    Lcd_Set_Area(x, y, w, h);

    patt += PATT_HEADER_SIZE;

    for (j=0; j<h; j++) {
	    for (i=0; i<w; i++) {
	        Trans_Dat_16(*patt);
	        patt++;
	    }
	}
}

//======Set Font Pattern======//
// !!! patt data is PATT8*
void Lcd_Set_Font(rom PATT8* patt) {
    font_patt = patt;
}

//======Set Font Sub Color======//
void Lcd_Set_Font_Sub_Color(COLOR16 color) {
    font_sub_color = color;
}





//======Draw Char======//
void Lcd_Draw_Char(const char c) {
    int i, j;
    int w = PATT_W(font_patt);
    int h = PATT_H(font_patt);
    int x = str_base_x + str_cur_x * w;
    int y = str_base_y + str_cur_y * h;
    int dot;
    int pos_f;		// Position Set Flag
	
    rom PATT8* patt = font_patt;

    Lcd_Set_Area(x, y, w, h);

    patt += PATT_HEADER_SIZE;
    patt += (c & 0xf0) >> 4;
    patt += (c & 0x0f) * h * 16;

    pos_f = 0;
    for (j=0; j<h; j++) {
        for (i=0; i<w; i++) {
            if ((*patt >> (7-i)) & 0x01) {
                //if (pos_f)Lcd_Set_Pos(x+i, y+j);
                //pos_f = 0;
                Trans_Dat_16(font_color);
            } else {
                //pos_f = 1;
                Trans_Dat_16(font_sub_color);
            }
        }
        patt += 16;
    }
    str_cur_x ++;
}
//======Draw String======//
void Lcd_Draw_String(char* str) {
    while (*str) {
        Lcd_Draw_Char(*str++);
    }
}
//======Draw String (ROM)======//
void Lcd_Draw_String_Rom(rom char* str) {
    while (*str) {
        Lcd_Draw_Char(*str++);
    }
}







/*
//======Printf======//
void Lcd_printf(int x, int y, COLOR16 color, char *str, ...) {
    va_list arg;
	
	// Minus Position
    if(x >= 0 && y >= 0) {
    	str_base_x = x;
    	str_base_y = y;
    	str_cur_x = str_cur_y = 0;
    }
    font_color = color;

    va_start(arg, str);
    
    vsprintf(buf, str, arg);

    va_end(arg);
    
    Lcd_Draw_String(buf);
}


*/



/*


//======Printf (Rom)======//
void Lcd_printf_Rom(int x, int y, COLOR16 color, rom char *str, ...) {
    va_list arg;

    // Minus Position
    if(x >= 0 && y >= 0) {
    	str_base_x = x;
    	str_base_y = y;
    	str_cur_x = str_cur_y = 0;
    }
    font_color = color;
	
    va_start(arg, str);
    
    vsprintf(buf, str, arg);

    va_end(arg);
    
    Lcd_Draw_String(buf);
}

//======Draw XLine======//
void Lcd_Draw_XLine(int x1, int x2, int y, COLOR16 color) {
    int i;
    int x;
    int w = 1;
    int h = 1;

    //---Decide x, w---//
    if (x2>=x1) {
        x = x1;
        w += x2-x1;
    } else {
        //Reverse Up Down
        x = x2;
        w += x1-x2;
    }

    Lcd_Set_Area(x, y, w, h);

    //---Draw Line---//
    for (i=0; i<w; i++) {
        Trans_Dat_16(color);
    }
}

//======Draw YLine======//
void Lcd_Draw_YLine(int y1, int y2, int x, COLOR16 color) {
    int i;
    int y;
    int h = 1;
    int w = 1;

    //---Decide x, w---//
    if (y2>=y1) {
        y = y1;
        h += y2-y1;
    } else {
        //Reverse Left Right
        y = y2;
        h += y1-y2;
    }
    Lcd_Set_Area(x, y, w, h);

    //---Draw Line---//
    for (i=0; i<h; i++) {
        Trans_Dat_16(color);
    }
}


*/


//======Draw Dot======//
void Lcd_Draw_Dot(int x, int y, COLOR16 color) {
    Lcd_Set_Pos(x, y);
    Trans_Dat_16(color);
}

/*
//====== Draw Line ======//
void Lcd_Draw_Line(int x, int y, int xx, int yy, COLOR16 color) {
    int i;
    int w, h;
    int temp;
    int w2, h2;

    if (x > xx) {
        //Reverse Left<->Right
        temp = x;
        x    = xx;
        xx   = temp;

        temp = y;
        y    = yy;
        yy   = temp;
    }

    //Calc width, Height
    w = xx - x;
    h = ABS(y - yy);

    w++;
    h++;

    //Max Width, Height
    if ((w > 512) || (h > 512))
        return;

    //X Line
    if (h == 1) {
        Lcd_Draw_XLine(x, xx, y, color);
        return;
    }
    //Y Line
    if (w == 1) {
        Lcd_Draw_YLine(y, yy, x, color);
        return;
    }

    //Calc Width, Height x2
    w2 = w << 1;
    h2 = h << 1;

    i = 1;
    //---Width Long Line---//
    if (w >= h) {
        //Draw Start, End Dot
        Lcd_Draw_Dot(x++, y, color);
        Lcd_Draw_Dot(xx--, yy, color);
        //Right Down Line
        if (y < yy) {
            while (x <= xx) {
                temp = ((long)h2 * (i++) + h) / w2;
                Lcd_Draw_Dot(x++, y + temp, color);
            }
        }
        //Right Up Line
        else {
            while (x <= xx) {
                temp = ((long)h2 * (i++) + h) / w2;
                Lcd_Draw_Dot(x++, y - temp, color);
            }
        }
    }

    //---Height Long Line---//
    else {
        //Right Down Line
        if (y < yy) {
            //Draw Start, End Dot
            Lcd_Draw_Dot(x, y++, color);
            Lcd_Draw_Dot(xx, yy--, color);

            while (y <= yy) {
                temp = ((long)w2 * (i++) + w) / h2;
                Lcd_Draw_Dot(x + temp, y++, color);
            }
        }
        //Right Up Line
        else {
            //Draw Start, End Dot
            Lcd_Draw_Dot(x, y--, color);
            Lcd_Draw_Dot(xx, yy++, color);

            while (y >= yy) {
                temp = ((long)w2 * (i++) + w) / h2;
                Lcd_Draw_Dot(x + temp, y--, color);
            }
        }
    }

    return;
}

//======sqrt(int)======//
//Future's Laboratory
//www001.upp.so-net.ne.jp/y_yutaka/labo/math_algo/math_algo.html
//Yutaka Yoshisaka
int my_sqrt(int x) {
	int s, t;
	
	if (x <= 0) return 0;
	
	s = 1;  t = x;
	while (s < t) {  s <<= 1;  t >>= 1;  }
	do {
	t = s;
	s = (x / s + s) >> 1;
	} while (s < t);

	return t;
}


*/


//======Draw Circle======//


/*
void Lcd_Draw_Circle(int xx, int yy, int r, COLOR16 color) {
	int x, y;
	int r2 = r/1.414;
	
	for(x=0; x<=r2; x++) {
		y = my_sqrt(r*r-x*x);
		
		Lcd_Draw_Dot(xx+x, yy+y, color);
		Lcd_Draw_Dot(xx-x, yy+y, color);
		Lcd_Draw_Dot(xx+x, yy-y, color);
		Lcd_Draw_Dot(xx-x, yy-y, color);
	}
	
	for(y=0; y<=r2; y++) {
		x = my_sqrt(r*r-y*y);
		
		Lcd_Draw_Dot(xx+x, yy+y, color);
		Lcd_Draw_Dot(xx-x, yy+y, color);
		Lcd_Draw_Dot(xx+x, yy-y, color);
		Lcd_Draw_Dot(xx-x, yy-y, color);
	}
}


*/


/*


//======Draw Circle2======//
void Lcd_Draw_Circle2(int xx, int yy, int r, COLOR16 color) {
	int x, y;
	int r2 = r/1.414;
	
	for(x=0; x<=r2; x++) {
		y = my_sqrt(r*r-x*x);
		
		Lcd_Draw_XLine(xx-x, xx+x, yy-y, color);
		Lcd_Draw_XLine(xx-x, xx+x, yy+y, color);
	}
	
	for(y=0; y<=r2; y++) {
		x = my_sqrt(r*r-y*y);
		
		Lcd_Draw_XLine(xx-x, xx+x, yy-y, color);
		Lcd_Draw_XLine(xx-x, xx+x, yy+y, color);
	}
}

//======Draw Box======//
void Lcd_Draw_Box(int x, int y, int w, int h, COLOR16 color) {
	w--; h--;
	Lcd_Draw_XLine(x, x+w, y, color);
	Lcd_Draw_XLine(x, x+w, y+h, color);
	Lcd_Draw_YLine(y, y+h, x, color);
	Lcd_Draw_YLine(y, y+h, x+w, color);
}
//======Draw Box2======//
void Lcd_Draw_Box2(int x, int y, int w, int h, COLOR16 color) {
	int i, j;
	Lcd_Set_Area(x, y, w, h);
	for(j=0; j<h; j++) {
		for(i=0; i<w; i++) {
			Trans_Dat_16(color);
		}
	}
}






*/