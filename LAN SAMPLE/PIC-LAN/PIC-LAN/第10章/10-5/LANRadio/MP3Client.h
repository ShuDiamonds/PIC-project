/*********************************************************************
 *
 *	Shoutcast MP3/WAV Client
 *  Module for Microchip TCP/IP Stack
 *	 -Downloads and plays MP3 audio streams from the Internet
 *
 *********************************************************************
 * FileName:        MP3Client.h
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
 * Howard Schlunder		03/12/2007	Original
 ********************************************************************/
#ifndef MP3ClIENT_H
#define MP3CLIENT_H

#include "TCPIP Stack/TCPIP.h"
#include "VS1011.h"

// Address structure to hold information about one shoutcast server
typedef struct _STATION_INFO {
	ROM BYTE *HumanName;// For display on the LCD module
	ROM BYTE *HostName;	// Host name string of the server
	WORD port;			// TCP Port the remote server daemon is listening on
	ROM BYTE *Message;	// The message that is send to the remote server to request the MP3 stream. Hardcoded in this version.
} STATION_INFO;

void MP3ClientInit(void);
void MP3ClientTask(void);
void MP3OpenStation(STATION_INFO *lpStation);
void MP3CloseStation(void);
void MP3Timer3Interrupt(void);

// Callback function to let the App know that we received new meta data
extern void NewStreamTitleProc(BYTE *strStreamTitle);
extern void NewServerTitleProc(BYTE *strServerTitle);

#endif

