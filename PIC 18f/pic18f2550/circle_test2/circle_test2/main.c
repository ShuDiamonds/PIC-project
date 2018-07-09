//#include <p18f4620.h>
#include <p18f2550.h>
#include <delays.h>
#include <stdlib.h>

#include "YHY024006A.h"
#include "font_patt.h"

/*

#pragma config OSC=HSPLL, FCMEN=OFF, IESO=OFF, PWRT=ON
#pragma config BOREN=ON, BORV=2, WDT=OFF, WDTPS=1024
#pragma config MCLRE=ON, PBADEN=OFF, CCP2MX=PORTC
#pragma config STVREN=OFF, LVP=OFF, DEBUG=OFF
#pragma config CP0=OFF, CP1=OFF, CP2=OFF, CP3=OFF, CPB=OFF
#pragma config CPD=OFF, WRT0=OFF, WRT1=OFF, WRT2=OFF, WRT3=OFF
#pragma config WRTB=OFF, WRTC=OFF, WRTD=OFF, EBTR0=OFF
#pragma config EBTR1=OFF, EBTR2=OFF, EBTR3=OFF, EBTRB=OFF


*/

/** CONFIGURATION **************************************************/
        #pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
        #pragma config CPUDIV   = OSC1_PLL2   
        #pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
        #pragma config FOSC     = HSPLL_HS
        #pragma config FCMEN    = OFF
        #pragma config IESO     = OFF
        #pragma config PWRT     = OFF
        #pragma config BOR      = ON
        #pragma config BORV     = 3
        #pragma config VREGEN   = ON      //USB Voltage Regulator
        #pragma config WDT      = OFF
        #pragma config WDTPS    = 32768
        #pragma config MCLRE    = ON
        #pragma config LPT1OSC  = OFF
        #pragma config PBADEN   = OFF
//      #pragma config CCP2MX   = ON
        #pragma config STVREN   = ON
        #pragma config LVP      = OFF
//      #pragma config ICPRT    = OFF       // Dedicated In-Circuit Debug/Programming
        #pragma config XINST    = OFF       // Extended Instruction Set
        #pragma config CP0      = OFF
        #pragma config CP1      = OFF
//      #pragma config CP2      = OFF
//      #pragma config CP3      = OFF
        #pragma config CPB      = OFF
//      #pragma config CPD      = OFF
        #pragma config WRT0     = OFF
        #pragma config WRT1     = OFF
//      #pragma config WRT2     = OFF
//      #pragma config WRT3     = OFF
        #pragma config WRTB     = OFF       // Boot Block Write Protection
        #pragma config WRTC     = OFF
//      #pragma config WRTD     = OFF
        #pragma config EBTR0    = OFF
        #pragma config EBTR1    = OFF
//      #pragma config EBTR2    = OFF
//      #pragma config EBTR3    = OFF
        #pragma config EBTRB    = OFF

//======main program======//
void main(void) {
	int i;
	int size;
	COLOR16 color, color2;
	int x, y;
	char FFF=0;
	int a=0;
	int B=0;
		
	//---Port I/O Setting---//
	ADCON1 = 0x0F;
	TRISA = 0x0F;
	TRISB = 0x00;
	
	PORTA = 0x00;
	PORTB = 0x00;
	
	Delay1KTCYx(100);
	Lcd_Init();
	Lcd_Set_Font(font_patt_816);
	Lcd_Set_Font_Sub_Color(WHITE);

	Lcd_Clear(WHITE);
	
	while(1)
	{
					Lcd_Draw_Circle(x, y, i, Gradation(color, color2, 11, i));
		while(Lcd_Get_Touch(&x, &y) == 0);
		Lcd_printf(0, 0, color, "Hello World\r\n");
		
	}
		
	while(1) {
		
		size = rand()%30+10;
		color = (COLOR16)rand()*2;
		color2 = (COLOR16)rand()*2;
		
		Lcd_Set_Font_Sub_Color(color2);
		
		Lcd_printf(0, 0, color, "Let's Touch Display!!!");
		
		while(Lcd_Get_Touch(&x, &y) == 0);
		
		Lcd_printf(-1, -1, color, " %3d %3d", x, y);
		
		for(i=0; i<size; i++) {
			
			Lcd_Draw_Circle(x, y, i, Gradation(color, color2, size, i));
		}
	}
	
	
	

}

