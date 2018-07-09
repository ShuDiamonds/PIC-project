#include<16F877a.h>
//#include<stdio.h>
//#include<string.h>

#use delay(clock = 20000000)
#fuses HS,NOWDT,NOPROTECT


#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9
main()
{
	set_tris_a(0x00);
	set_tris_b(0x00);
	set_tris_c(0x00);
	set_tris_d(0x00);
	set_tris_e(0x00);
	
	port_a = 0;
	port_b = 0;
	port_c = 0;
	port_d = 0;
	port_e = 0;
	
	while(1)
	{
		delay_ms(1000);
		output_high(PIN_C3);
		delay_ms(1000);
		output_low(PIN_C3);
	}
}
