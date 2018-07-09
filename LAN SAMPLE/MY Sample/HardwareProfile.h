/*********************************************************************
 *
 *	Hardware specific definitions
 *
 *********************************************************************
 * FileName:        HardwareProfile.h
 * Dependencies:    None
 * Processor:       PIC18, PIC24F, PIC24H, dsPIC30F, dsPIC33F, PIC32
 * Compiler:        Microchip C32 v1.00 or higher
 *					Microchip C30 v3.01 or higher
 *					Microchip C18 v3.13 or higher
 *					HI-TECH PICC-18 STD 9.50PL3 or higher
 * Company:         Microchip Technology, Inc.
 *
 * Software License Agreement
 *
 * Copyright (C) 2002-2008 Microchip Technology Inc.  All rights 
 * reserved.
 *
 * Microchip licenses to you the right to use, modify, copy, and 
 * distribute: 
 * (i)  the Software when embedded on a Microchip microcontroller or 
 *      digital signal controller product ("Device") which is 
 *      integrated into Licensee's product; or
 * (ii) ONLY the Software driver source files ENC28J60.c and 
 *      ENC28J60.h ported to a non-Microchip device used in 
 *      conjunction with a Microchip ethernet controller for the 
 *      sole purpose of interfacing with the ethernet controller. 
 *
 * You should refer to the license agreement accompanying this 
 * Software for additional information regarding your rights and 
 * obligations.
 *
 * THE SOFTWARE AND DOCUMENTATION ARE PROVIDED "AS IS" WITHOUT 
 * WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT 
 * LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT SHALL 
 * MICROCHIP BE LIABLE FOR ANY INCIDENTAL, SPECIAL, INDIRECT OR 
 * CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA, COST OF 
 * PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR SERVICES, ANY CLAIMS 
 * BY THIRD PARTIES (INCLUDING BUT NOT LIMITED TO ANY DEFENSE 
 * THEREOF), ANY CLAIMS FOR INDEMNITY OR CONTRIBUTION, OR OTHER 
 * SIMILAR COSTS, WHETHER ASSERTED ON THE BASIS OF CONTRACT, TORT 
 * (INCLUDING NEGLIGENCE), BREACH OF WARRANTY, OR OTHERWISE.
 *
 *
 * Author               Date		Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Howard Schlunder		10/03/06	Original, copied from Compiler.h
 ********************************************************************/
#ifndef __HARDWARE_PROFILE_H
#define __HARDWARE_PROFILE_H

// Choose which hardware profile to compile for here.  See 
// the hardware profiles below for meaning of various options.  
//#define PICDEMNET2
//#define HPC_EXPLORER
//#define PICDEMZ
//#define PIC24FJ64GA004_PIM	// Explorer 16, but with the PIC24FJ64GA004 PIM module, which has significantly differnt pin mappings
//#define EXPLORER_16		// PIC24FJ128GA010, PIC24HJ256GP610, dsPIC33FJ256GP710 PIMs
//#define DSPICDEM11
#define YOUR_BOARD

// If no hardware profiles are defined, assume that we are using 
// a Microchip demo board and try to auto-select the correct profile
// based on processor selected in MPLAB
#if !defined(PICDEMNET2) && !defined(HPC_EXPLORER) && !defined(PICDEMZ) && !defined(EXPLORER_16) && !defined(PIC24FJ64GA004_PIM) && !defined(DSPICDEM11) && !defined(PICDEMNET2) && !defined(INTERNET_RADIO) && !defined(YOUR_BOARD)
	#if defined(__18F67J60) || defined(_18F67J60)
		#define INTERNET_RADIO
	#endif
#endif

// Set configuration fuses (but only once)
#if defined(THIS_IS_STACK_APPLICATION)
	#if defined(__18CXX)
		#if defined(__EXTENDED18__)
			#pragma config XINST=ON
		#elif !defined(HI_TECH_C)
			#pragma config XINST=OFF
		#endif
	
		#if defined(__18F8722)
			// PICDEM HPC Explorer board
			#pragma config OSC=HSPLL, FCMEN=OFF, IESO=OFF, PWRT=OFF, WDT=OFF, LVP=OFF
	
		
		#elif defined(__18F97J60) || defined(__18F96J65) || defined(__18F96J60) || defined(__18F87J60) || defined(__18F86J65) || defined(__18F86J60) || defined(__18F67J60) || defined(__18F66J65) || defined(__18F66J60) 
			// PICDEM.net 2 or any other PIC18F97J60 family device
			#pragma config WDT=OFF, FOSC2=ON, FOSC=HSPLL, ETHLED=ON
		#elif defined(_18F97J60) || defined(_18F96J65) || defined(_18F96J60) || defined(_18F87J60) || defined(_18F86J65) || defined(_18F86J60) || defined(_18F67J60) || defined(_18F66J65) || defined(_18F66J60) 
			// PICDEM.net 2 board with HI-TECH PICC-18 compiler
			__CONFIG(1, WDTDIS & XINSTDIS);
			__CONFIG(2, HSPLL);
			__CONFIG(3, ETHLEDEN);
		
		#endif
	
	
	#endif
#endif // Prevent more than one set of config fuse definitions

// Clock frequency value.
// This value is used to calculate Tick Counter value
#if defined(__18CXX)
	// All PIC18 processors
	#if defined(PICDEMNET2) || defined(INTERNET_RADIO) || defined(YOUR_BOARD)
		#define CLOCK_FREQ		(41666667ul)      // Hz
		#define GetSystemClock()		(41666667ul)      // Hz
		#define GetInstructionClock()	(GetSystemClock()/4)
		#define GetPeripheralClock()	GetInstructionClock()
	#elif defined(PICDEMZ)
		#define GetSystemClock()		(16000000ul)      // Hz
		#define GetInstructionClock()	(GetSystemClock()/4)
		#define GetPeripheralClock()	GetInstructionClock()
	#else
		#define GetSystemClock()		(40000000ul)      // Hz
		#define GetInstructionClock()	(GetSystemClock()/4)
		#define GetPeripheralClock()	GetInstructionClock()
	#endif

#endif

// Hardware mappings
#if defined(YOUR_BOARD)
// Define your own board hardware profile here
	// I/O pins
	#define 	LED0_TRIS		(TRISEbits.TRISE2)
	#define 	LED0_IO			(PORTEbits.RE2)
	#define 	LED1_TRIS		(TRISEbits.TRISE3)
	#define 	LED1_IO			(PORTEbits.RE3)
	#define 	LED2_TRIS		(TRISEbits.TRISE5)
	#define 	LED2_IO			(PORTEbits.RE5)
	//#define 	LED3_TRIS		(TRISEbits.TRISD1)
	//#define 	LED3_IO			(PORTEbits.RD1)
	#define 	LED_RUN_TRIS		(TRISDbits.TRISD2)
	#define 	LED_RUN_IO			(PORTDbits.RD2)
	
//	#define 	LED4_TRIS		(TRISDbits.TRISD0)
//	#define 	LED4_IO			(PORTDbits.RD0)
//	#define 	LED5_TRIS		(TRISDbits.TRISD1)
//	#define 	LED5_IO			(PORTDbits.RD1)
//	#define 	LED6_TRIS		(TRISDbits.TRISD2)
//	#define 	LED6_IO			(PORTDbits.RD2)
//	#define 	LED7_TRIS		(TRISEbits.TRISE0)
//	#define 	LED7_IO			(PORTEbits.RE0)
	
	#define 	LED_IO			(*((volatile unsigned char*)(&PORTE)))

	#define 	BUTTON0_TRIS		(TRISBbits.TRISB3)
	#define		BUTTON0_IO		(PORTBbits.RB3)
	#define 	BUTTON1_TRIS		(TRISBbits.TRISB2)
	#define		BUTTON1_IO		(PORTBbits.RB2)
	#define 	BUTTON2_TRIS		(TRISBbits.TRISB1)
	#define		BUTTON2_IO		(PORTBbits.RB1)
	#define 	BUTTON3_TRIS		(TRISBbits.TRISB0)
	#define		BUTTON3_IO		(PORTBbits.RB0)

	// LCD I/O pins
	#define 	LCD_DATA_TRIS	(TRISF)
	#define 	LCD_DATA_IO		(LATF)
	#define 	LCD_RD_WR_TRIS	(TRISFbits.TRISF1)
	#define 	LCD_RD_WR_IO	(LATFbits.LATF1)
	#define 	LCD_RS_TRIS		(TRISFbits.TRISF2)
	#define 	LCD_RS_IO		(LATFbits.LATF2)
	#define 	LCD_E_TRIS		(TRISFbits.TRISF3)
	#define 	LCD_E_IO		(LATFbits.LATF3)

	// Servo Output
	#define	RCS1_TRIS		(TRISCbits.TRISC2)
	#define RCS1_IO			(LATCbits.LATC2)
	#define	RCS2_TRIS		(TRISCbits.TRISC0)
	#define RCS2_IO			(LATCbits.LATC0)
	#define	UIO1_TRIS		(TRISCbits.TRISA4)
	#define	UIO1_IO			(LATCbits.LATA4)
	#define	UIO2_TRIS		(TRISCbits.TRISC7)
	#define	UIO2_IO			(LATCbits.LATC7)

#else
	#error "Hardware profile not defined.  See available profiles in HardwareProfile.h.  Add the appropriate macro definition to your application configuration file ('TCPIPConfig.h', etc.)"
#endif


#if defined(__18CXX)	// PIC18
	// UART mapping functions for consistent API names across 8-bit and 16 or 
	// 32 bit compilers.  For simplicity, everything will use "UART" instead 
	// of USART/EUSART/etc.
	#define BusyUART()				BusyUSART()
	#define CloseUART()				CloseUSART()
	#define ConfigIntUART(a)		ConfigIntUSART(a)
	#define DataRdyUART()			DataRdyUSART()
	#define OpenUART(a,b,c)			OpenUSART(a,b,c)
	#define ReadUART()				ReadUSART()
	#define WriteUART(a)			WriteUSART(a)
	#define getsUART(a,b,c)			getsUSART(b,a)
	#define putsUART(a)				putsUSART(a)
	#define getcUART()				ReadUSART()
	#define putcUART(a)				WriteUSART(a)
	#define putrsUART(a)			putrsUSART((far rom char*)a)

#else	 // PIC24F, PIC24H, dsPIC30, dsPIC33, PIC32
	// Some A/D converter registers on dsPIC30s are named slightly differently 
	// on other procesors, so we need to rename them.
	#if defined(__dsPIC30F__)
		#define ADC1BUF0			ADCBUF0
		#define AD1CHS				ADCHS
		#define	AD1CON1				ADCON1
		#define AD1CON2				ADCON2
		#define AD1CON3				ADCON3
		#define AD1PCFGbits			ADPCFGbits
		#define AD1CSSL				ADCSSL
		#define AD1IF				ADIF
		#define AD1IE				ADIE
		#define _ADC1Interrupt		_ADCInterrupt
	#endif

	// Select which UART the STACK_USE_UART and STACK_USE_UART2TCP_BRIDGE 
	// options will use.  You can change these to U1BRG, U1MODE, etc. if you 
	// want to use the UART1 module instead of UART2.
	#define UBRG					U2BRG
	#define UMODE					U2MODE
	#define USTA					U2STA
	#define BusyUART()				BusyUART2()
	#define CloseUART()				CloseUART2()
	#define ConfigIntUART(a)		ConfigIntUART2(a)
	#define DataRdyUART()			DataRdyUART2()
	#define OpenUART(a,b,c)			OpenUART2(a,b,c)
	#define ReadUART()				ReadUART2()
	#define WriteUART(a)			WriteUART2(a)
	#define getsUART(a,b,c)			getsUART2(a,b,c)
	#if defined(__C32__)
		#define putsUART(a)			putsUART2(a)
	#else
		#define putsUART(a)			putsUART2((unsigned int*)a)
	#endif
	#define getcUART()				getcUART2()
	#define putcUART(a)				WriteUART(a)
	#define putrsUART(a)			putsUART(a)
#endif


#endif
