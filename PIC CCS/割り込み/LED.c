#include<16f88.h>
#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,BROWNOUT
#use delay(clock=20000000)
#byte port_a=5
#byte port_b=6
#INT_EXT
ext_isr()
{
	int c;
	while(1)
	{
		c=5;
		while(1)
		{
			port_a=0x06;
			delay_ms(500);
			port_a=0;
			port_b=0;
			port_a=0x01;
			port_b=0x80;
			delay_ms(500);
			c=c-1;
			if(c==0)
			break;
		}
		port_b=0;
		break;
	}
}
main()
{
	int k,s;
	set_tris_a(0x18);
	set_tris_b(0x01);
	port_a=0;
	port_b=0;
	enable_interrupts(INT_EXT);
	enable_interrupts(GLOBAL);
	ext_int_edge(H_TO_L);
	while(1)
	{
		while(1)
		{
		if(input(PIN_A3)==0)
		break;
	}
	while(1)
	{
	
		for(k=0;k<=2;K++)
		{
			s=0x04>>k;
			port_a=s;
			delay_ms(200);
			if(input(PIN_A4)==0)
			break;
			else if(s==0x01)
			break;
		}
		port_a=0;
		if(input(PIN_A4)==0)
		break;
		port_b=0x80;
		delay_ms(200);
		port_b=0;
		if(input(PIN_A4)==0)
		break;
	}
}
}
