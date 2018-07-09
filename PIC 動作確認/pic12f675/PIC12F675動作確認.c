#include<12F683.h>
#include<stdio.h>

#use delay(clock = 10000000)
#fuses HS,NOWDT,NOPROTECT


#byte port_a = 5
main()
{

	while(1)
	{
	set_tris_a(0);
	output_high(PIN_A1);
	}
}