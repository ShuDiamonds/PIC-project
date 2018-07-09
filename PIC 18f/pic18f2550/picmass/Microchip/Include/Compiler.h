/*********************************************************************/
#ifndef __COMPILER_H
#define __COMPILER_H

// Include proper device header file
#if defined(__18CXX) || defined(HI_TECH_C)	
	// All PIC18 processors
	#if defined(HI_TECH_C) && defined(__PICC18__)	// HI TECH PICC-18 compiler
		#define __18CXX
		#include <htc.h>
	#else					// Microchip C18 compiler
	    #include <p18cxxx.h>
	#endif

#else
	#error Unknown processor or compiler.  See Compiler.h
#endif

#include <stdio.h>
#include <stdlib.h>
#include <string.h>


// Base RAM and ROM pointer types for given architecture

#if defined(__18CXX)
	#define PTR_BASE		WORD
	#define ROM_PTR_BASE	unsigned short long
	#if defined(HI_TECH_C)
		#undef ROM_PTR_BASE
		#define ROM_PTR_BASE	DWORD
	#endif
#endif


// Definitions that apply to all except Microchip MPLAB C Compiler for PIC18 MCUs (formerly C18)
#if !defined(__18CXX) || (defined(HI_TECH_C) && defined(__PICC18__))
	#define memcmppgm2ram(a,b,c)	memcmp(a,b,c)
	#define strcmppgm2ram(a,b)		strcmp(a,b)
	#define memcpypgm2ram(a,b,c)	memcpy(a,b,c)
	#define strcpypgm2ram(a,b)		strcpy(a,b)
	#define strncpypgm2ram(a,b,c)	strncpy(a,b,c)
	#define strstrrampgm(a,b)		strstr(a,b)
	#define	strlenpgm(a)			strlen(a)
	#define strchrpgm(a,b)			strchr(a,b)
	#define strcatpgm2ram(a,b)		strcat(a,b)
#endif


// Definitions that apply to all 8-bit products
// (PIC18)
#if defined(__18CXX)
	#define	__attribute__(a)

    #define FAR                         far

	// Microchip C18 specific defines
	#if !defined(HI_TECH_C)
	    #define ROM                 	rom
		#define strcpypgm2ram(a, b)		strcpypgm2ram(a,(far rom char*)b)
	#endif
	
	// HI TECH PICC-18 STD specific defines
	#if defined(HI_TECH_C)
	    #define ROM                 	const
		#define rom
	    #define Nop()               	asm("NOP");
		#define ClrWdt()				asm("CLRWDT");
	    #define Reset()					asm("RESET");
	#endif
    
// Definitions that apply to all 16-bit and 32-bit products
// (PIC24F, PIC24H, dsPIC30F, dsPIC33F, and PIC32)
#else
	#define	ROM						const

	
#endif



#endif
