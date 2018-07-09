/**********************************************************/
/*delayŠÖ”ƒ‰ƒCƒuƒ‰ƒŠ                                     */
/*2011.12.30                                              */
/**********************************************************/
#include "p33Fxxxx.h"
#include "delaylib.h"

//====‚È‚ñ‚¿‚á‚Á‚Ädelay_usŠÖ”==================================================
void delay_us(unsigned long time)
{
	volatile unsigned long i;
	for(i = 0; i <= time * 8; i++)
	{
		asm volatile ("nop" :: ); 
	}
}

//====‚È‚ñ‚¿‚á‚Á‚Ädelay_msŠÖ”==================================================
void delay_ms(unsigned long time)
{
	volatile unsigned long i;
	for(i = 0; i <= time * 80; i++)
	{
		delay_us(1);
	}
}


