#include <p18f67j60.h>
/*
#pragma config OSC = HS
#pragma config PWRT = OFF
#pragma config BOR = OFF
#pragma config WDT = OFF
#pragma config LVP = OFF
*/
#include <config.h>
 
#pragma romdata config=0x300000
 unsigned char rom _CONFIG0 = _CP_ON_0;
 unsigned char rom _CONFIG1 = _OSCS_OFF_1 & _HSPLL_OSC_1;
 unsigned char rom _CONFIG2 = _BOR_ON_2 & _BORV_25_2 & _PWRT_ON_2;
 unsigned char rom _CONFIG3 = _WDT_ON_3 & _WDTPS_64_3;
 unsigned char rom _CONFIG5 = _CCP2MX_OFF_5;
 unsigned char rom _CONFIG6 = _STVR_ON_6;
 
#pragma romdata idlocs=0x200000
 unsigned char rom idlocData[8] = "01234567";
 
#pragma romdata devid=0x3ffffe
 unsigned char rom devidID[2] = {0x02,0x02};
 


void main (void) {
	while(1)
	{
		
	}

}
 
