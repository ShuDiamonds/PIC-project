#include <16f84a.h>
#fuses HS,NOWDT,NOPROTECT
#use delay(clock=20000000)
#define SECMAX 38
#byte port_b=6
int sec,flag;
#INT_RTCC
rtcc_isr()
{
	sec=sec-1;
	if(sec==0)
	{
		sec=SECMAX;
		if(flag==0)
		{
			port_b=0xc0;//port_b_ÇÃÇVÇ∆ÇUÇ…èoóÕ
			flag=1;
		}
		else
		{
			port_b=0x30;//port_bÇÃÇTÇ∆ÇSÇ…èoóÕ
			flag=0;
		}
	}
}
main()
{
	while(1)
	{
		if(input(PIN_A2)==0)
		break;
	}
	set_tris_a(0x04);
	set_tris_b(0);
	setup_counters(RTCC_INTERNAL,RTCC_DIV_256);
	sec=SECMAX;
	enable_interrupts(INT_RTCC);
	enable_interrupts(GLOBAL);
	while(1)
	{
	}
}
