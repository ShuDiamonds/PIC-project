/**********************************************************/
/*delay関数ライブラリ                                     */
/*2011.12.30                                              */
/**********************************************************/
#include "p33Fxxxx.h"
#include "delaylib.h"

//====なんちゃってdelay_us関数==================================================
void delay_us(unsigned long time)
{
	volatile unsigned long i;
	for(i = 0; i <= time * 8; i++)
	{
		asm volatile ("nop" :: ); 
	}
}

//====なんちゃってdelay_ms関数==================================================
void delay_ms(unsigned long time)
{
	volatile unsigned long i;
	for(i = 0; i <= time * 80; i++)
	{
		delay_us(1);
	}
}


