/*********************************************************************
 *
 * Data SPI RAM Access Routines
 *  -Tested with AMI Semiconductor N256S0830HDA
 *
 *********************************************************************
 * FileName:        SPIRAM2.h
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
 * \file SPIRAM2.h
 * \author Howard Henry Schlunder
 * \date 25 July 2007
********************************************************************/
#ifndef __SPIRAM2_H
#define __SPIRAM2_H

void SPIRAM2Init(void);
void SPIRAM2GetArray(WORD wAddress, BYTE *vData, WORD wLength);
void SPIRAM2PutArray(WORD wAddress, BYTE *vData, WORD wLength);

#define SPIRAM2PutString(a,b)			SPIRAM2PutArray(a, strlen((char*)b))

#if defined(__18CXX)
	void SPIRAM2PutROMArray(WORD wAddress, ROM BYTE *vData, WORD wLength);
	#define SPIRAM2PutROMString(a,b)	SPIRAM2PutROMArray(a, strlenpgm((ROM char*)b))
#else
	#define SPIRAM2PutROMString(a,b)	SPIRAM2PutArray(a, strlen((char*)b))
	#define SPIRAM2PutROMArray(a,b,c)	SPIRAM2PutROMArray(a, b, c)
#endif


#endif
