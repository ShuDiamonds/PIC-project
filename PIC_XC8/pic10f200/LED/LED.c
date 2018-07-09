#include <stdio.h>
#include <stdlib.h>
#include <xc.h>

//#pragma config CP = 0x03ff, PWRTE = 0, WDTE = 0, FOSC = 0x3


main()
{

	int i = 0;

	//TRISAbits.TRISA0 = 0;
	
	//TRISbits.TRIS0 = 0;
	//TRISbits.GP0 = 0;
	//TRISbits.GP0 = 0;
	
	GPIO = 0;
	
	while(1)
	{
		 //PORTAbits.RA0 ^= 1;
		GPIObits.GP0 ^= 1;
		
		 for(i=0;i<1000;i++);
	}

 return (EXIT_SUCCESS);
 
}

