#include<16F88.h>
#include<stdio.h>
#include<string.h>

#use delay(clock = 20000000)
#fuses HS,NOWDT,NOPROTECT


#byte port_a = 5
#byte port_b = 6


main()
{

	while(1)
	{
	set_tris_a(0);
	set_tris_b(0xff);
	output_high(PIN_A3);
	output_high(PIN_A2);
	output_high(PIN_A1);
	}
	/*
	while(1)
	{
		if(input(PIN_B0)==1)
		{
			output_high(PIN_A1);
		}
			if(input(PIN_B1)==1)
		{
			output_high(PIN_A2);
		}
			if(input(PIN_B2)==1)
		{
			output_high(PIN_A3);
		}
	}
	*/
}
