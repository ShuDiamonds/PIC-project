#include <16F877A.h>
#fuses HS,NOWDT,NOPUT,NOPROTECT,NOBROWNOUT,NOLVP
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

int main()
{
	
	unsigned long int a=0;
	int  i=0;
	unsigned int k=0;
	set_tris_a(0);
	set_tris_b(0x00);
	set_tris_c(0);
	set_tris_d(0);
	set_tris_e(0);
	
	
	port_a = 0;
	port_b = 0;
	port_c = 0;
	port_d = 0;
	port_e = 0;
	
	
#define	hennsuu		2000
	a=5;
	while(1)
	{
		
		
		
		output_high(PIN_C0);
		
		for( i=0;i<=50;i++)
		{
		output_high(PIN_B5);
		delay_us(1500);
		output_low(PIN_B5);
		delay_ms(18);
		}                                                                                 
		
		
		for( i=0;i<=50;i++)
		{
		output_high(PIN_B5);
		delay_us(900);
		output_low(PIN_B5);
		delay_ms(19);
		}      
		
		
		for( i=0;i<=50;i++)
		{
		output_high(PIN_B5);
		delay_us(1800);
		output_low(PIN_B5);
		delay_ms(18);
		}      
		/*
		a=4;
		
		output_low(PIN_C0);
		
		for( i=0;i<=50;i++)
		{
		output_high(PIN_B5);
		delay_ms(a);
		output_low(PIN_B5);
		delay_ms(hennsuu-a);
		}
		
		output_high(PIN_C0);
		a=7;
		
		for( i=0;i<=50;i++)
		{
		output_high(PIN_B5);
		delay_ms(a);
		output_low(PIN_B5);
		delay_ms(hennsuu-a);
		}
		
		
		output_low(PIN_C0);
		a=12;
		
		for( i=0;i<=50;i++)
		{
		output_high(PIN_B5);
		delay_ms(a);
		output_low(PIN_B5);
		delay_ms(hennsuu-a);
		}
		
		if(a==12)
		{
			a=0;
		}
		*/
		
	}
	
}

		