/*********************************************************************
 *
 *	VLS VS1011 MP3 Decoder Driver
 *  Module for Microchip TCP/IP Stack
 *	 -Downloads and plays MP3 audio streams from the Internet
 *
 *********************************************************************
 * FileName:        VS1011.c
 * Dependencies:    Microchip TCP/IP Stack
 * Processor:       PIC18, PIC24F, PIC24H, dsPIC30F, dsPIC33F
 * Complier:        Microchip C18 v3.10 or higher
 *					Microchip C30 v2.05 or higher
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
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Francesco Volpe		12/??/2006	Original
 * Howard Schlunder		03/12/2007	Completely revamped
 ********************************************************************/
#include "TCPIP Stack/TCPIP.h"
#include "VS1011.h"


static BYTE WriteSPI(BYTE output)
{
	MP3_SPI_IF = 0;
	MP3_SSPBUF = output;
	while(!MP3_SPI_IF);
	return MP3_SSPBUF;
}



// **** Set volume for analog outputs on VS1011
// vRight = right channel attenuation from maximum volume, 0.5dB steps (0x00 = full volume, 0xFF = muted)
// vLeft = left channel attenuation from maximum volume, 0.5dB steps (0x00 = full volume, 0xFF = muted)
void SetVolume(BYTE vRight, BYTE vLeft)
{
	// Send command to VS1011
	MP3_XDCS_IO = 1;
	while(!MP3_DREQ_IO);
	MP3_XCS_IO = 0;
	WriteSPI(0x02);		// Write
	WriteSPI(0xB);		// Register address
	WriteSPI(vLeft);	// Volume value
	WriteSPI(vRight);	
	MP3_XCS_IO = 1;
}

// **** Set Bass Boost level
// bass = Bass gain in dB, range from 0 to 15
// gfreq = Limit frequency for bass boost, 10 Hz steps (range from 0 to 15)
void SetBassBoost(BYTE bass, BYTE gfreq)
{
	BYTE templ = 0;
	
	// Make sure values are in the allowed range
	if(bass > 15)
		bass = 15;
	if(gfreq > 15)
		gfreq = 15;
		
	// put gfreq into the lower 4 bit
	templ = gfreq;

	// put bass boost value into the upper 4 bit
	templ |= (bass << 4);
	
	// Send command to VS1011
	MP3_XDCS_IO = 1;
	while(!MP3_DREQ_IO);
	MP3_XCS_IO = 0;
	WriteSPI(0x02);		// Write
	WriteSPI(0x02);		// Register address
	WriteSPI(0xFF);		// Dummy
	WriteSPI(templ);		// Bass boost and limit frequency
	MP3_XCS_IO = 1;
	
	return;

}


// **** Get bitrate of currently playing stream
WORD GetBitrate(void)
{
	BYTE temph,templ,bitrate,streamid;

	// Get values from VS1011
	MP3_XDCS_IO = 1;
	while(!MP3_DREQ_IO);
	MP3_XCS_IO = 0;
	WriteSPI(0x03);			// Read
	WriteSPI(0x08);			// Register adress
	temph = WriteSPI(0xFF);	// Data
	templ = WriteSPI(0xFF);	
	MP3_XCS_IO = 1;

	// Extract bitrate
	bitrate = (temph & 0xF0) >> 4;
	
	// Get VS1011 stream id
	MP3_XDCS_IO = 1;
	while(!MP3_DREQ_IO);
	MP3_XCS_IO = 0;
	WriteSPI(0x03);			// Read
	WriteSPI(0x09);			// Register adress
	temph = WriteSPI(0xFF);	// Data
	templ = WriteSPI(0xFF);	
	MP3_XCS_IO = 1;

	// Extract stream id
	streamid = (templ & 0x18) >> 3;

	// Use matching table, according to stream id 
	if(streamid == 3)
	{	switch(bitrate)
		{
			case 14: 
				return 320;
				break;
			case 13: 
				return 256;
				break;
			case 12: 
				return 224;
				break;
			case 11: 
				return 192;
				break;
			case 10: 
				return 160;
				break;
			case 9: 
				return 128;
				break;
			case 8: 
				return 112;
				break;
			case 7: 
				return 96;
				break;
			case 6: 
				return 80;
				break;
			case 5: 
				return 64;
				break;
			case 4: 
				return 56;
				break;
			case 3: 
				return 48;
				break;
			case 2: 
				return 40;
				break;
			case 1: 
				return 32;
				break;
		 	default: 
				return 0;
				break;
		}
	}
	else
	{
		switch(bitrate)
		{
			case 14: 
				return 160;
				break;
			case 13: 
				return 144;
				break;
			case 12: 
				return 128;
				break;
			case 11: 
				return 112;
				break;
			case 10: 
				return 96;
				break;
			case 9: 
				return 80;
				break;
			case 8: 
				return 64;
				break;
			case 7: 
				return 56;
				break;
			case 6: 
				return 48;
				break;
			case 5: 
				return 40;
				break;
			case 4: 
				return 32;
				break;
			case 3: 
				return 24;
				break;
			case 2: 
				return 16;
				break;
			case 1: 
				return 8;
				break;
		 	default: 
				return 0;
				break;
		}
	}
}

// **** Initiate VS1011 Sine Test
// SDI Test Mode must be enabled in MODE Register (0x00)
// Enters infinite loop, should only be used for VS1011 function test
#if 0
void VS1011_SineTest()
{
	// Send Test Sequence
	while(1)
	{
		while(!MP3_DREQ_IO);
		MP3_XDCS_IO = 0;
  		// Start Sine Test
		WriteSPI(0x53);
  		WriteSPI(0xEF);
  		WriteSPI(0x6E);
  		WriteSPI(0x7D);
  		WriteSPI(0x00);
  		WriteSPI(0x00);
  		WriteSPI(0x00);
  		WriteSPI(0x00);
		MP3_XDCS_IO = 1;
	}
}
#endif

// **** Configure VS1011
// Does not return if communication with VS1011 is broken
void VS1011_Init(void)
{
	BYTE b1,b2;

	// Set up SPI port pins
	MP3_XDCS_IO = 1;			// Make the Data CS pin high
	MP3_XCS_IO = 1;				// Make the Control CS pin high
	MP3_XRESET_IO = 0;
	
	MP3_XRESET_TRIS = 0;
	MP3_DREQ_TRIS = 1;
	MP3_XDCS_TRIS = 0;			// Make the Data CS pin an output
	MP3_XCS_TRIS = 0;			// Make the Control CS pin an output
	MP3_SDI_TRIS = 1;			// Make the DIN pin an input
	MP3_SDO_TRIS = 0;			// Make the DOUT pin an output
	MP3_SCK_TRIS = 0;			// Make the SCK pin an output

    // Set up SPI module
	MP3_SPISTATbits.SMP = 0;	// Sample at middle
	MP3_SPISTATbits.CKE = 1;	// Transmit data on rising edge of clock
	MP3_SPICON1 = 0x21;			// SSPEN = 1, CKP = 0, CLK = Fosc/16 (2.604MHz)
	MP3_SPI_IF = 0;


	// Deassert RESET (active low)
	Delay1KTCYx(10);
	MP3_XRESET_IO = 1;

	// Write configuration register
	do
	{		
		while(!MP3_DREQ_IO);
		MP3_XCS_IO = 0;
		WriteSPI(0x02);		// Write
		WriteSPI(0x00);		// Register address
		WriteSPI(0x08);		// 16 bit value to write
		WriteSPI(0x20);		// Normal transfer mode (using DREQ), new serial mode, SDI tests enabled
		//WriteSPI(0x60);		// Same, but with stream transfer mode instead of normal mode
		MP3_XCS_IO = 1;

		while(!MP3_DREQ_IO);	// Read back first byte to make sure the VS1011 is present
		MP3_XCS_IO = 0;
		WriteSPI(0x03);
		WriteSPI(0x00);
		b1 = WriteSPI(0xFF);
		b2 = WriteSPI(0xFF);
		MP3_XCS_IO = 1;
	}while(b1 != 0x08);

}

