/*********************************************************************
 *
 * Data SPI RAM Access Routines
 *  -Tested with AMI Semiconductor N256S0830HDA
 *
 *********************************************************************
 * FileName:        SPIRAM2.c
 * Dependencies:    Compiler.h
 * Processor:       PIC18, PIC24F, PIC24H, dsPIC30F, dsPIC33F
 * Complier:        Microchip C18 v3.11 or higher
 *					Microchip C30 v3.01 or higher
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
 * \file SPIRAM2.c
 * \author Howard Henry Schlunder
 * \date 25 July 2007
********************************************************************/
#define __SPIRAM2_C

#include "TCPIP Stack/TCPIP.h"

#if defined(SPIRAM2_CS_TRIS)

// SPI SRAM opcodes
#define READ	0x03	// Read data from memory array beginning at selected address
#define WRITE	0x02	// Write data to memory array beginning at selected address
#define RDSR	0x05	// Read Status register
#define WRSR	0x01	// Write Status register


#if defined(__PIC24F__)
    #define PROPER_SPICON1 	(0x001B | 0x0120)	// 1:1 primary prescale, 2:1 secondary prescale, CKE=1, MASTER mode
#elif defined(__dsPIC33F__) || defined(__PIC24H__)
    #define PROPER_SPICON1	(0x000F | 0x0120)	// 1:1 primary prescale, 5:1 secondary prescale, CKE=1, MASTER mode
#elif defined(__dsPIC30F__)
    #define PROPER_SPICON1 	(0x0017 | 0x0120)	// 1:1 primary prescale, 3:1 secondary prescale, CKE=1, MASTER mode
#else
	#define PROPER_SPICON1	(0x20)				// SSPEN bit is set, SPI in master mode, FOSC/4, IDLE state is low level
#endif


/*********************************************************************
 * Function:        void SPIRAM2Init(void)
 *
 * PreCondition:    None
 *
 * Input:           None
 *
 * Output:          None
 *
 * Side Effects:    None
 *
 * Overview:        Initialize SPI module to communicate to serial
 *                  RAM.
 *
 * Note:            Code sets SPI clock to Fosc/4.  
 ********************************************************************/
void SPIRAM2Init(void)
{
	BYTE Dummy;
	#if defined(__18CXX)
	BYTE SPICON1Save;
	#else
	WORD SPICON1Save;
	#endif	

	SPIRAM2_CS_IO = 1;
	SPIRAM2_CS_TRIS = 0;		// Drive SPI RAM chip select pin

	SPIRAM2_SCK_TRIS = 0;	// Set SCK pin as an output
	SPIRAM2_SDI_TRIS = 1;	// Make sure SDI pin is an input
	SPIRAM2_SDO_TRIS = 0;	// Set SDO pin as an output

	// Save SPI state (clock speed)
	SPICON1Save = SPIRAM2_SPICON1;
	SPIRAM2_SPICON1 = PROPER_SPICON1;

	#if defined(__C30__)
	    SPIRAM2_SPICON2 = 0;
	    SPIRAM2_SPISTAT = 0;    // clear SPI
	    SPIRAM2_SPISTATbits.SPIEN = 1;
	#elif defined(__18CXX)
		SPIRAM2_SPI_IF = 0;
		SPIRAM2_SPISTATbits.CKE = 1; 	// Transmit data on rising edge of clock
		SPIRAM2_SPISTATbits.SMP = 0;		// Input sampled at middle of data output time
	#endif

	// Set Burst mode
	// Activate chip select
	SPIRAM2_CS_IO = 0;
	SPIRAM2_SPI_IF = 0;

	// Send Write Status Register opcode
	SPIRAM2_SSPBUF = WRSR;
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	
	// Set status register to 0b01000000 to enable burst mode
	SPIRAM2_SSPBUF = 0x40;
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;

	// Deactivate chip select
	SPIRAM2_CS_IO = 1;

	// Restore SPI state
	SPIRAM2_SPICON1 = SPICON1Save;
}


/*********************************************************************
 * Function:        void SPIRAM2GetArray(WORD wAddress, BYTE *vData, WORD wLength)
 *
 * PreCondition:    
 *
 * Input:           
 *
 * Output:          
 *
 * Side Effects:    None
 *
 * Overview:        
 *
 * Note:            None
 ********************************************************************/
#pragma tmpdata SPIRAM2
void SPIRAM2GetArray(WORD wAddress, BYTE *vData, WORD wLength)
{
	BYTE Dummy;
	#if defined(__18CXX)
	BYTE SPICON1Save;
	#else
	WORD SPICON1Save;
	#endif

	// Ignore operations when the destination is NULL or nothing to read
	if(vData == NULL)
		return;
	if(wLength == 0)
		return;

	// Save SPI state (clock speed)
	SPICON1Save = SPIRAM2_SPICON1;
	SPIRAM2_SPICON1 = PROPER_SPICON1;
	
	// Activate chip select
	SPIRAM2_CS_IO = 0;
	SPIRAM2_SPI_IF = 0;

	// Send READ opcode
	SPIRAM2_SSPBUF = READ;
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	
	// Send address
	SPIRAM2_SSPBUF = ((BYTE*)&wAddress)[1];
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	SPIRAM2_SSPBUF = ((BYTE*)&wAddress)[0];
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	
	// Read data
	while(wLength--)
	{
		SPIRAM2_SSPBUF = 0;
		while(!SPIRAM2_SPI_IF);
		*vData++ = SPIRAM2_SSPBUF;
		SPIRAM2_SPI_IF = 0;
	};
	
	// Deactivate chip select
	SPIRAM2_CS_IO = 1;

	// Restore SPI state
	SPIRAM2_SPICON1 = SPICON1Save;
}


/*********************************************************************
 * Function:        void SPIRAM2PutArray(WORD wAddress, BYTE *vData, WORD wLength)
 *
 * PreCondition:    
 *
 * Input:           
 *
 * Output:          
 *
 * Side Effects:    None
 *
 * Overview:        
 *
 * Note:            None
 ********************************************************************/
void SPIRAM2PutArray(WORD wAddress, BYTE *vData, WORD wLength)
{
	BYTE Dummy;
	#if defined(__18CXX)
	BYTE SPICON1Save;
	#else
	WORD SPICON1Save;
	#endif	

	// Ignore operations when the source data is NULL
	if(vData == NULL)
		return;
	if(wLength == 0)
		return;

	// Save SPI state (clock speed)
	SPICON1Save = SPIRAM2_SPICON1;
	SPIRAM2_SPICON1 = PROPER_SPICON1;

	// Activate chip select
	SPIRAM2_CS_IO = 0;
	SPIRAM2_SPI_IF = 0;

	// Send WRITE opcode
	SPIRAM2_SSPBUF = WRITE;
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	
	// Send address
	SPIRAM2_SSPBUF = ((BYTE*)&wAddress)[1];
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	SPIRAM2_SSPBUF = ((BYTE*)&wAddress)[0];
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	
	// Write data
	while(wLength--)
	{
		SPIRAM2_SSPBUF = *vData++;
		while(!SPIRAM2_SPI_IF);
		Dummy = SPIRAM2_SSPBUF;
		SPIRAM2_SPI_IF = 0;
	};
	
	// Deactivate chip select
	SPIRAM2_CS_IO = 1;

	// Restore SPI state
	SPIRAM2_SPICON1 = SPICON1Save;
}

#if defined(__18CXX)
void SPIRAM2PutROMArray(WORD wAddress, ROM BYTE *vData, WORD wLength)
{
	BYTE Dummy;
	#if defined(__18CXX)
	BYTE SPICON1Save;
	#else
	WORD SPICON1Save;
	#endif	

	// Ignore operations when the source data is NULL
	if(vData == NULL)
		return;
	if(wLength == 0)
		return;

	// Save SPI state (clock speed)
	SPICON1Save = SPIRAM2_SPICON1;
	SPIRAM2_SPICON1 = PROPER_SPICON1;

	// Activate chip select
	SPIRAM2_CS_IO = 0;
	SPIRAM2_SPI_IF = 0;

	// Send WRITE opcode
	SPIRAM2_SSPBUF = WRITE;
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	
	// Send address
	SPIRAM2_SSPBUF = ((BYTE*)&wAddress)[1];
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	SPIRAM2_SSPBUF = ((BYTE*)&wAddress)[0];
	while(!SPIRAM2_SPI_IF);
	Dummy = SPIRAM2_SSPBUF;
	SPIRAM2_SPI_IF = 0;
	
	// Write data
	while(wLength--)
	{
		SPIRAM2_SSPBUF = *vData++;
		while(!SPIRAM2_SPI_IF);
		Dummy = SPIRAM2_SSPBUF;
		SPIRAM2_SPI_IF = 0;
	};
	
	// Deactivate chip select
	SPIRAM2_CS_IO = 1;

	// Restore SPI state
	SPIRAM2_SPICON1 = SPICON1Save;
}
#endif

#pragma tmpdata

#endif //#if defined(SPIRAM2_CS_TRIS)
