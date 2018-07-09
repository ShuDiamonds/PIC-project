#include <stdio.h>
#include <stdlib.h>
#include <xc.h>

//#pragma config CP = 0x03ff, PWRTE = 0, WDTE = 0, FOSC = 0x3

static void InitPortIOReg(void);

main()
{

	int i = 0;

	InitPortIOReg();

	while(1)
	{
		 PORTAbits.RA0 ^= 1;
		 for(i=0;i<1000;i++);
	}

 return (EXIT_SUCCESS);
 
}

static void InitPortIOReg()
{

	TRISAbits.TRISA0 = 0;
	
}
