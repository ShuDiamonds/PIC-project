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
#define MOTA1_R		PIN_B0
#define MOTA1_L		PIN_B1

int main()
{
	
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
	
	setup_ccp1(CCP_PWM);
	setup_ccp2(CCP_PWM);
	SETUP_TIMER_2(T2_DIV_BY_16,0xFF,1);
	
	while(1)
	{
		
		if(input(PIN_B7) == 1 && input(PIN_B6) == 0){
			set_pwm1_duty(800);
			set_pwm2_duty(800);
			output_high(MOTA1_L);
			output_low(MOTA1_R);
			output_low(RUN_LED);
		}else if(input(PIN_B7) == 0 && input(PIN_B6) == 0){
			set_pwm1_duty(500);
			set_pwm2_duty(500);
			output_high(MOTA1_R);
			output_low(MOTA1_L);
			output_high(RUN_LED);
		}else if(input(PIN_B6) == 1){
			set_pwm1_duty(1024);
			set_pwm2_duty(1024);
			output_high(MOTA1_R);
			output_high(MOTA1_L);
			output_high(RUN_LED);
		}
		
		
	}
	
}

		