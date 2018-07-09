/**********************************************************/
/*delay�֐����C�u����                                     */
/*2011.12.30                                              */
/**********************************************************/
#include "p33Fxxxx.h"
#include "delaylib.h"

//====�Ȃ񂿂����delay_us�֐�==================================================
void delay_us(unsigned long time)
{
	volatile unsigned long i;
	for(i = 0; i <= time * 8; i++)
	{
		asm volatile ("nop" :: ); 
	}
}

//====�Ȃ񂿂����delay_ms�֐�==================================================
void delay_ms(unsigned long time)
{
	volatile unsigned long i;
	for(i = 0; i <= time * 80; i++)
	{
		delay_us(1);
	}
}


