#include <16F877A.h>
#fuses HS,NOWDT,NOPUT,NOPROTECT,BROWNOUT,NOLVP
#use delay(CLOCK=20000000)

#byte port_a=5
#byte port_b=6
#byte port_c=7
#byte port_d=8
#byte port_e=9
#use fast_io(a)
#use fast_io(b)
#use fast_io(c)
#use fast_io(d)
#use fast_io(e)
//Ç“ÇÒÉ}ÉNÉç
#define RUN_LED		PIN_C3
#define MOTA1		PIN_B3
#define MOTA2		PIN_B4
#define MOTAPWM		PIN_B5

int main()
{
	long int a=0,b=0;
	int i;
	
	
	set_tris_a(0);
	set_tris_b(0b11000000);
	set_tris_c(0);
	set_tris_d(0);
	set_tris_e(0);
	
	
	port_a = 0;
	port_b = 0;
	port_c = 0;
	port_d = 0;
	port_e = 0;
	
	output_high(RUN_LED);
	delay_ms(500);
	output_low(RUN_LED);
	output_high(RUN_LED);
	delay_ms(500);
	output_low(RUN_LED);
	output_high(RUN_LED);
	delay_ms(500);
	output_low(RUN_LED);
	
	output_high(MOTA1);
	delay_ms(500);
	output_low(MOTA2);
	a=2000;
	b=1700;
	while(1)
	{
		for(i=0;i<100;i++)
		{
			output_high(MOTAPWM);
			delay_us(b);
			output_low(MOTAPWM);
			delay_us(a-b);
		}
		//b=b+5;
		/*
		if(b>=20)
		{
			b=0;
		}
		*/
	}
	
}

		