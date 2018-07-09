/*********************************************************************
 *
 *	Shoutcast MP3/WAV Client
 *  Module for Microchip TCP/IP Stack
 *	 -Downloads and plays MP3 audio streams from the Internet
 *
 *********************************************************************
 * FileName:        MP3Client.c
 * Dependencies:    Microchip TCP/IP Stack, SPISRAM2.c
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
#include "MP3Client.h"
#include "SPIRAM2.h"
//#include "OLED.h"


#define MP3_INTERRUPT_FREQ	1800ul	// Hz

//#define MP3_DEBUG_PRINT_BUFFER_FILLED

// FIFO for MP3 data (in external SPI RAM chip)
// Note that you can shrink this length of move the start address 
// if you want to use part of the RAM chip for something else
#define AUDIO_BUFFER_START	(0ul)
#define AUDIO_BUFFER_LEN	(32768ul)


static volatile WORD wHeadPtr = AUDIO_BUFFER_START; 
static volatile WORD wTailPtr = AUDIO_BUFFER_START;
static STATION_INFO MyStation;
static BOOL Openned = FALSE;
static TCP_SOCKET MySocket = INVALID_SOCKET;
static BOOL bUnderrunOccured = TRUE;

static enum 
{
	SM_IDLE = 0,
	SM_DISCONNECTION_WAIT,
	SM_CONNECT,
	SM_CONNECT_WAIT,
	SM_GET_HEADERS,
	SM_PLAYING,
	SM_FOUND_METADATA,
	SM_GET_METADATA
} smMP3State = SM_IDLE;

// Pseudo Functions
#define LOW(a) 		((a) & 0xFF)
#define HIGH(a) 	(((a)>>8) & 0xFF)


/*********************************************************************
* Function:		void MP3ClientInit(void)
*
* PreCondition:	None
*
* Input:		None
*
* Output:		None
*
* Side Effects:	None
*
* Overview:		Sets up needed interrupts for the MP3 decoder 
*				(TMR3 for SPI idle detection and feeding MP3 decoder)
*
* Note:			None
********************************************************************/
void MP3ClientInit(void)
{
	// Set up Timer 3 to automatically feed the MP3 decoder periodically
	T3CON = 0xB1;	// 16-bit mode, 1:8 prescale, internal clock, timer enabled
	TMR3H = HIGH(-((GetPeripheralClock()/8 + MP3_INTERRUPT_FREQ/2)/MP3_INTERRUPT_FREQ));
	TMR3L = LOW(-((GetPeripheralClock()/8 + MP3_INTERRUPT_FREQ/2)/MP3_INTERRUPT_FREQ));
	IPR2bits.TMR3IP = 1;	// High priority interrupt
	PIR2bits.TMR3IF = 0;	// Clear interrupt flag
	PIE2bits.TMR3IE = 1;	// Enable the interrupt
}


/*********************************************************************
* Function:		void MP3ClientTask(void)
*
* PreCondition:	None
*
* Input:		None
*
* Output:		None
*
* Side Effects:	None
*
* Overview:		Downloads an MP3 or WAV from a Shoutcast server or 
*				similar and plays it.
*
* Note:			None
********************************************************************/
void MP3ClientTask(void)
{
	static DWORD dwMetaDataInterval;
	static DWORD dwNextMetaData;
	static ROM BYTE *rstrMessage;
	static DWORD dwTimer;
	static enum
	{
		SM_FIND_HEADERS = 0,
		SM_FOUND_NAME,
		SM_FOUND_METAINT
	} smHeaderParser;
	BYTE vBuffer[32];
	BYTE strHeader[65];
	BYTE *strTitle;
	WORD w, wMin, wSpace;
	BYTE i, j;
	WORD wHeadPtrShadow;
	WORD wTailPtrShadow;

	// Reconnect if we lost our connection to the remote server -- a 
	// common task that needs to be done in almost every state
	if((BYTE)smMP3State >= (BYTE)SM_GET_HEADERS)
	{
		if(!TCPIsConnected(MySocket))
		{
			TCPDisconnect(MySocket);
			TCPDisconnect(MySocket);
			MySocket = INVALID_SOCKET;
			smMP3State = SM_DISCONNECTION_WAIT;
			dwTimer = TickGet() + 3*TICK_SECOND;
			return;
		}
	}

	// Get buffer head and tail shadows (interrupt safe)
	PIE2bits.TMR3IE = 0;
	wHeadPtrShadow = wHeadPtr;
	wTailPtrShadow = wTailPtr;
	PIE2bits.TMR3IE = 1;	



	// Show buffered data count on OLED display
	#if defined(MP3_DEBUG_PRINT_BUFFER_FILLED)
	{
		static BYTE strSize[6];
		BYTE i;
		WORD wSpace;

		// Only do an update if we have changed by 128 bytes or more
		if(wHeadPtrShadow >= wTailPtrShadow)
			wSpace = wHeadPtrShadow - wTailPtrShadow;
		else
			wSpace = AUDIO_BUFFER_LEN - (wTailPtrShadow - wHeadPtrShadow);
	
		uitoa(wSpace, strSize);
	
		for(i = strlen(strSize); i < sizeof(strSize)-1; i++)
			strSize[i] = ' ';
		strSize[sizeof(strSize)-1] = 0;
		oledPutString(strSize, 0xB4, 92, 1);
	}
	#endif


	switch(smMP3State)
	{
		case SM_IDLE:
			break;
		
		case SM_DISCONNECTION_WAIT:
			if((LONG)(TickGet() - dwTimer) >= (LONG)0)
				smMP3State = SM_CONNECT;
			break;

		// Initiate TCP connection 
		case SM_CONNECT:
			MySocket = TCPOpen((DWORD)&MyStation.HostName[0], TCP_OPEN_ROM_HOST, MyStation.port, TCP_PURPOSE_MP3_CLIENT);
			if(MySocket == INVALID_SOCKET)
				return;

			rstrMessage = MyStation.Message;
			smMP3State = SM_CONNECT_WAIT;
			break;
	
		// Wait for connection to be established
		case SM_CONNECT_WAIT:
			if(TCPIsConnected(MySocket))
			{
				if(TCPIsPutReady(MySocket))
				{
					rstrMessage = TCPPutROMString(MySocket, rstrMessage);
					TCPFlush(MySocket);
					if(*rstrMessage == 0)
					{
						wHeadPtrShadow = AUDIO_BUFFER_START;
						wTailPtrShadow = AUDIO_BUFFER_START;
						PIE2bits.TMR3IE = 0;
						wHeadPtr = wHeadPtrShadow;
						wTailPtr = wTailPtrShadow;
						PIE2bits.TMR3IE = 1;
						dwMetaDataInterval = 0;
						smHeaderParser = SM_FIND_HEADERS;
						smMP3State = SM_GET_HEADERS;
					}
				}
			}
			break;

		case SM_GET_HEADERS:
			switch(smHeaderParser)
			{
				case SM_FIND_HEADERS:
					wMin = 0xFFFF;

					// Look for the "icy-name" Server Name header
					w = TCPFindROMArray(MySocket, (ROM BYTE*)"\nicy-name:", 10, 0, TRUE);
					if(w < wMin)
					{
						wMin = w + 10;
						smHeaderParser = SM_FOUND_NAME;
					}

					// Look for the "icy-metaint" meta data interval field
					w = TCPFindROMArray(MySocket, (ROM BYTE*)"\nicy-metaint:", 13, 0, TRUE);
					if(w < wMin)
					{
						wMin = w + 13;
						smHeaderParser = SM_FOUND_METAINT;
					}

					// Look for the end of headers marker (two carraige return line feeds)
					w = TCPFindROMArray(MySocket, (ROM BYTE*)"\r\n\r\n", 4, 0, FALSE);
					if(w < wMin)
					{
						wMin = w + 4;
						dwNextMetaData = dwMetaDataInterval;					
						smMP3State = SM_PLAYING;
					}

					// See if we found at least one header
					if(wMin != 0xFFFF)
					{
						// Throw away all data up to and including the first header string (but not data)
						TCPGetArray(MySocket, NULL, wMin);
					}
					else
					{
						// More headers to parse: throw away unneeded headers
						// 13 is maximum header name field length.  Increase this if you have a bigger header name to find.
						w = TCPIsGetReady(MySocket);
						if(w > 13)	
							TCPGetArray(MySocket, NULL, w - 13);
					}

					break;
	
				case SM_FOUND_NAME:
					// Look for the terminator characters
					w = TCPFindROMArray(MySocket, (ROM BYTE*)"\r\n", 2, 0, FALSE);
					if(w == 0xFFFF)
						break;

					// Obtain the icy-name header data
					if(sizeof(strHeader)-1 < w)
						w = sizeof(strHeader);
					TCPGetArray(MySocket, strHeader, w);
					strHeader[w] = 0;
					
					// Remove any spaces in front
					strTitle = strHeader;
					while(*strTitle == ' ')
					{
						strTitle++;
					}

					// Call user callback to let us know that we have this data
					NewServerTitleProc(strTitle);

					smHeaderParser = SM_FIND_HEADERS;
					break;

				case SM_FOUND_METAINT:
					// Look for the terminator characters
					w = TCPFindROMArray(MySocket, (ROM BYTE*)"\r\n", 2, 0, FALSE);
					if(w == 0xFFFF)
						break;

					// Obtain and convert the meta interval to a DWORD (from ASCII string)
					if(sizeof(strHeader)-1 < w)
						w = sizeof(strHeader);
					TCPGetArray(MySocket, strHeader, w);
					strHeader[w] = 0;
					dwMetaDataInterval = atol(strHeader);

					smHeaderParser = SM_FIND_HEADERS;
					break;
			}
			break;

		case SM_PLAYING:
			// Write incoming TCP data to vBuffer
			while(1)
			{	
				// Calculate the free space in our ring buffer
				if(wHeadPtrShadow >= wTailPtrShadow)
					wSpace = (AUDIO_BUFFER_LEN - 1) - (wHeadPtrShadow - wTailPtrShadow);
				else
					wSpace = wTailPtrShadow - wHeadPtrShadow - 1;

				if(wSpace == 0)
					break;

				// Find the number of bytes waiting in the TCP FIFO
				w = TCPIsGetReady(MySocket);
				if(w == 0)
					break;

				// Don't fetch more bytes that we can store
				if(w > wSpace)
					w = wSpace;
				if(w > sizeof(vBuffer))
					w = sizeof(vBuffer);

				// Don't fetch meta data bytes
				if(dwMetaDataInterval)
				{
					if(w > dwNextMetaData)
						w = dwNextMetaData;
				}

				// Don't fetch more bytes than can fit in the FIFO 
				// without causing a wrapparound
				if(w >= AUDIO_BUFFER_START + AUDIO_BUFFER_LEN - wHeadPtrShadow)
				{
					// Fetch the bytes
					w = AUDIO_BUFFER_START + AUDIO_BUFFER_LEN - wHeadPtrShadow;
					TCPGetArray(MySocket, vBuffer, w);
					SPIRAM2PutArray(wHeadPtrShadow, vBuffer, w);
					wHeadPtrShadow = AUDIO_BUFFER_START;
				}
				else
				{
					// Fetch the bytes
					TCPGetArray(MySocket, vBuffer, w);
					SPIRAM2PutArray(wHeadPtrShadow, vBuffer, w);
					wHeadPtrShadow += w;
				}

				// Retrieve meta data if it is coming up
				if(dwMetaDataInterval)
				{
					dwNextMetaData -= w;
					if(dwNextMetaData == 0)
					{
						smMP3State = SM_FOUND_METADATA;
						break;
					}
				}
			}
			break;

		case SM_FOUND_METADATA:
			if(!TCPIsGetReady(MySocket))
				break;

			// Get the meta data length (/16) and calculate actual value
			*((WORD*)&dwNextMetaData) = 0;
			TCPGet(MySocket, (BYTE*)&dwNextMetaData);
			*((WORD*)&dwNextMetaData) <<= (WORD)4;	// Multiply by 16
	
			smMP3State = SM_GET_METADATA;
			// No break		

		case SM_GET_METADATA:
			if(TCPIsGetReady(MySocket) < (WORD)dwNextMetaData)
				break;

			w = (WORD)dwNextMetaData;
			if(w > sizeof(strHeader) - 1)
				w = sizeof(strHeader) - 1;

			// Retrieve the meta data and null terminate it
			TCPGetArray(MySocket, strHeader, w);
			strHeader[w] = 0;

			// Throw away any other meta data bytes that we don't 
			// have space to store
			if((WORD)dwNextMetaData > w)
				TCPGetArray(MySocket, NULL, (WORD)dwNextMetaData - w);
	
			if(w)
			{
				strTitle = strstrrampgm(strHeader, (far ROM BYTE*)"StreamTitle='");
				if(strTitle)
				{
					// Stringify the stream title and call 
					// NewStreamTitleProc application callback
					strTitle += 13;
					for(i = 0; i < strlen(strTitle); i++)
					{
						if(strTitle[i] == '\'')
						{
							strTitle[i] = 0;
							NewStreamTitleProc(strTitle);
							break;
						}
					}
				}
			}

			dwNextMetaData = dwMetaDataInterval;
			smMP3State = SM_PLAYING;
			break;
	}

	PIE2bits.TMR3IE = 0;
	wHeadPtr = wHeadPtrShadow;
	PIE2bits.TMR3IE = 1;	
}

/*********************************************************************
* Function:		void MP3OpenStation(STATION_INFO *lpStation)
*
* PreCondition:	None
*
* Input:		*lpStation: STATION_INFO filled will ip address, 
*				port, and message
*
* Output:		None
*
* Side Effects:	None
*
* Overview:		Begins connected to the remote station
*
* Note:			None
********************************************************************/
void MP3OpenStation(STATION_INFO *lpStation)
{
	if(Openned)
		MP3CloseStation();

	Openned = TRUE;

	memcpy((void*)&MyStation, (void*)lpStation, sizeof(MyStation));
	smMP3State = SM_CONNECT;
}


/*********************************************************************
* Function:		void MP3CloseStation(void)
*
* PreCondition:	None
*
* Input:		None
*
* Output:		None
*
* Side Effects:	None
*
* Overview:		Stops playing the current station, and disconnects 
*				if any.
*
* Note:			None
********************************************************************/
void MP3CloseStation(void)
{
	Openned = FALSE;

	if(MySocket != INVALID_SOCKET)
	{
		TCPDisconnect(MySocket);	// Send out a FIN disconnection request		
		TCPDisconnect(MySocket);	// Do this a second time to force an immediate disconnect via a RST packet.  This keeps the user interface snappy by not waiting for the remote node to send us a FIN as well.
		MySocket = INVALID_SOCKET;
		smMP3State = SM_IDLE;
	}
}


/*********************************************************************
* Function:		void MP3Interrupt(void)
*
* PreCondition:	None
*
* Input:		None
*
* Output:		None
*
* Side Effects:	None
*
* Overview:		Periodically copies data into the VS1011 MP3 decoder 
*				chip when the DREQ signal (INT0) is asserted (active 
*				high)
*
* Note:			This is meant for the interrupt context only.  Do 
*				not call from main line code.
********************************************************************/
#pragma tmpdata MP3ISRTemp
void MP3Timer3Interrupt(void)
{
	static BYTE vBuffer[32];
	static WORD wSpace;
	static BYTE i, j;
	
	// Reset Timer 3 so that we can poll for a time when the SPI is free
	TMR3H = HIGH(-((GetPeripheralClock()/8 + MP3_INTERRUPT_FREQ/2)/MP3_INTERRUPT_FREQ));
	TMR3L = LOW(-((GetPeripheralClock()/8 + MP3_INTERRUPT_FREQ/2)/MP3_INTERRUPT_FREQ));

	// Clear Timer 3 intrrupt flag
	PIR2bits.TMR3IF = 0;

	// Return immediately if there is nothing to do
	if(!MP3_DREQ_IO)
		return;

	// Return immediately if we can't process this interrupt 
	// because something else is using the SPI right now
	if((SPIRAM_CS_IO == 0) || (SPIRAM2_CS_IO == 0) || (MP3_XCS_IO == 0))
		return;

	// Ensure we have at least 32 bytes in the ring buffer to copy
	if(wHeadPtr >= wTailPtr)
		wSpace = wHeadPtr - wTailPtr;
	else
		wSpace = AUDIO_BUFFER_LEN - (wTailPtr - wHeadPtr);
	
	if(wSpace < 32)
	{
		bUnderrunOccured = TRUE;
		return;
	}
	
	// After an underrun condition, do not start playing again 
	// until we have at least half a second of buffered data
	if(bUnderrunOccured)
	{
		if(wSpace < 8192)
			return;
		bUnderrunOccured = FALSE;
	}
	
	
	// Read 32 bytes from the audio buffer
	SPIRAM2GetArray(wTailPtr, vBuffer, sizeof(vBuffer));
	wTailPtr += sizeof(vBuffer);
	if(wTailPtr >= AUDIO_BUFFER_START + AUDIO_BUFFER_LEN)
		wTailPtr = AUDIO_BUFFER_START;

	// Write the 32 bytes to the MP3 decoder
	MP3_XDCS_IO = 0;                  // Set the data CS pin low
	for(i = 0; i < sizeof(vBuffer); i++)
	{
		PIR1bits.SSP1IF = 0;
		SSP1BUF = vBuffer[i];
		while(!PIR1bits.SSP1IF);
	}
	MP3_XDCS_IO = 1;                  // Set the data CS pin high
}
#pragma tmpdata