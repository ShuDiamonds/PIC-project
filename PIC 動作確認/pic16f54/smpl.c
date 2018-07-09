//#device PIC16F84
#include<16F88.h>
#include<stdio.h>
#include<string.h>

#use delay(clock = 20000000)
#fuses HS,NOWDT,NOPROTECT

/*
#define PIN_A0  40
#define PIN_A1  41
#define PIN_A2  42
#define PIN_A3  43
#define PIN_A4  44

#define PIN_B0  48
#define PIN_B1  49
#define PIN_B2  50
#define PIN_B3  51
#define PIN_B4  52
#define PIN_B5  53
#define PIN_B6  54
#define PIN_B7  55
*/

//#use fast_io(b)
#byte port_a = 5
#byte port_b = 6


main()
{

	set_tris_a(0);
	set_tris_b(0xff);
	output_high(PIN_A1);
	return(0);
}