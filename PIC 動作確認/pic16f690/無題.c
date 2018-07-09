//#include <16f690.h>
#device PIC16F690
#fuses HS,NOWDT,NOPROTECT
#use delay(clock=8000000)
//#byte port_a = 5
#byte port_b=6
//#byte port_c = 7
//#use fast_io(a)
//#use fast_io(b)
//#use fast_io(c)

#define PIN_A0  40
#define PIN_A1  41
#define PIN_A2  42
#define PIN_A3  43
#define PIN_A4  44
#define PIN_A5  45

#define PIN_B4  52
#define PIN_B5  53
#define PIN_B6  54
#define PIN_B7  55

#define PIN_C0  56
#define PIN_C1  57
#define PIN_C2  58
#define PIN_C3  59
#define PIN_C4  60
#define PIN_C5  61
#define PIN_C6  62
#define PIN_C7  63

main()
{
	while(1)
	{
	//set_tris_a(0);
	set_tris_b(0);
	//set_tris_c(0);
	output_high(PIN_A0);
	output_high(PIN_C0);
	}
}
