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

int main()
{
	
	 long int a=0;
	
	set_tris_a(0);
	set_tris_b(0xff);
	set_tris_c(0);
	set_tris_d(0);
	set_tris_e(0);
	
	
	port_a = 0;
	port_b = 0;
	port_c = 0;
	port_d = 0;
	port_e = 0;
	
	setup_ccp1(CCP_PWM);
	setup_ccp2(CCP_PWM);
	SETUP_TIMER_2(T2_DIV_BY_16,0xFF,1);
	
	while(1)
	{
		a=1000;
		set_pwm1_duty(a);
		set_pwm2_duty(a);
		delay_ms(1000);
		a=0;
		set_pwm1_duty(a);
		set_pwm2_duty(a);
		delay_ms(1000);
		/*
		
		
		if(input(PIN_B0) == 1){
			set_pwm1_duty(50);
			//output_high(PIN_D0);
		}
		
		if(input(PIN_B1) == 1){
			set_pwm1_duty(1000);
			//output_low(PIN_D0);
		}
		
		*/
		
		/*
		output_high(PIN_D0);
		delay_ms(500);
		output_low(PIN_D0);
		delay_ms(500);
		*/
	}
	
}

		