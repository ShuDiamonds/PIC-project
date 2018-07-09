/*********************************************************************
 *
 *  Application to Demo HTTP2 Server
 *  Support for HTTP2 module in Microchip TCP/IP Stack
 *	 -Implements the application 
 *	 -Reference: RFC 1002
 *
 *********************************************************************
 * FileName:        CustomHTTPApp.c
 * Dependencies:    TCP/IP stack
 * Processor:       PIC18, PIC24F, PIC24H, dsPIC30F, dsPIC33F, PIC32MX
 * Compiler:        Microchip C32 v1.00 or higher
 *					Microchip C30 v3.01 or higher
 *					Microchip C18 v3.13 or higher
 *					HI-TECH PICC-18 STD 9.50PL3 or higher
 * Company:         Microchip Technology, Inc.
 *
 * Software License Agreement
 *
 * Copyright © 2002-2007 Microchip Technology Inc.  All rights 
 * reserved.
 *
 * Microchip licenses to you the right to use, modify, copy, and 
 * distribute: 
 * (i)  the Software when embedded on a Microchip microcontroller or 
 *      digital signal controller product (“DeviceE which is 
 *      integrated into Licensee’s product; or
 * (ii) ONLY the Software driver source files ENC28J60.c and 
 *      ENC28J60.h ported to a non-Microchip device used in 
 *      conjunction with a Microchip ethernet controller for the 
 *      sole purpose of interfacing with the ethernet controller. 
 *
 * You should refer to the license agreement accompanying this 
 * Software for additional information regarding your rights and 
 * obligations.
 *
 * THE SOFTWARE AND DOCUMENTATION ARE PROVIDED “AS ISEWITHOUT 
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
 * Author               Date    Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Elliott Wood     	6/18/07	Original
 ********************************************************************/
#define __CUSTOMHTTPAPP_C

#include "TCPIP Stack/TCPIP.h"

#if defined(STACK_USE_HTTP2_SERVER)

#if defined(HTTP_USE_POST)
	#if defined(USE_LCD)
	static HTTP_IO_RESULT HTTPPostLCD(void);
	#endif
	#if defined(STACK_USE_MD5)
	static HTTP_IO_RESULT HTTPPostMD5(void);
	#endif
	#if defined(STACK_USE_APP_RECONFIG)
	static HTTP_IO_RESULT HTTPPostConfig(void);
	#endif
	#if defined(STACK_USE_SMTP_CLIENT)
	static HTTP_IO_RESULT HTTPPostEmail(void);
	#endif
#endif

extern HTTP_CONN curHTTP;
extern HTTP_STUB httpStubs[MAX_HTTP_CONNECTIONS];
extern BYTE curHTTPID;
extern int Width1, Width2;

/*********************************************************************
 * Function:        HTTP_IO_RESULT HTTPExecuteGet(void)
 *
 * PreCondition:    curHTTP is loaded
 *
 * Input:           None
 *
 * Output:          HTTP_IO_DONE on success
 *					HTTP_IO_WAITING if waiting for asynchronous process
 *
 * Side Effects:    None
 *
 * Overview:        This function is called if data was read from the
 *					HTTP request from either the GET arguments, or
 *					any cookies sent.  curHTTP.data contains
 *					sequential pairs of strings representing the
 *					data received.  Any required authentication has 
 *					already been validated.
 *
 * Note:            In this simple example, HTTPGetROMArg is used to 
 *					search for values associated with argument names.
 *					At this point, the application may overwrite/modify
 *					curHTTP.data if additional storage associated with
 *					a connection is needed.  Cookies may be set; see
 *					HTTPExecutePostCookies for an example.  For 
 *					redirect functionality, set curHTTP.data to the 
 *					destination and change curHTTP.httpStatus to
 *					HTTP_REDIRECT.
 ********************************************************************/
HTTP_IO_RESULT HTTPExecuteGet(void)
{
	BYTE *ptr;
	BYTE filename[20];
	
	// Load the file name
	// Make sure BYTE filename[] above is large enough for your longest name
	MPFSGetFilename(curHTTP.file, filename, 20);
	
	// If its the forms.htm page
	if(!memcmppgm2ram(filename, "forms.htm", 9))
	{
		// Seek out each of the four LED strings, and if it exists set the LED states
		ptr = HTTPGetROMArg(curHTTP.data, (ROM BYTE *)"led4");
		if(ptr)
			LED4_IO = (*ptr == '1');

		ptr = HTTPGetROMArg(curHTTP.data, (ROM BYTE *)"led3");
		if(ptr)
			LED3_IO = (*ptr == '1');

		ptr = HTTPGetROMArg(curHTTP.data, (ROM BYTE *)"led2");
		if(ptr)
			LED2_IO = (*ptr == '1');

		ptr = HTTPGetROMArg(curHTTP.data, (ROM BYTE *)"led1");
		if(ptr)
			LED1_IO = (*ptr == '1');
	}
	
	// If it's the LED updater file
	else if(!memcmppgm2ram(filename, "cookies.htm", 11))
	{
		// This is very simple.  The names and values we want are already in
		// the data array.  We just set the hasArgs value to indicate how many
		// name/value pairs we want stored as cookies.
		// To add the second cookie, just increment this value.
		// remember to also add a dynamic variable callback to control the printout.
		curHTTP.hasArgs = 0x01;
	}
		
	
	// If it's the LED updater file
	else if(!memcmppgm2ram(filename, "leds.cgi", 8))
	{
		// Determine which LED to toggle
		ptr = HTTPGetROMArg(curHTTP.data, (ROM BYTE *)"led");
		
		// Toggle the specified LED
		switch(*ptr) {
		case '1':
			UIO1_IO ^= 1;
			break;
		case '2':
			UIO2_IO ^= 1;
			break;
		case '3':
			LED3_IO ^= 1;
			Width1 +=10;	
			break;
		case '4':
			LED4_IO ^= 1;
			Width1 -=10;
			break;
		case '5':
			LED5_IO ^= 1;
			Width2 -= 10;
			break;
		case '6':
			LED6_IO ^= 1;
			Width2 += 10;
			break;
		case '7':
			LED7_IO ^= 1;
			break;
		}
		
	}
	
	return HTTP_IO_DONE;
}

#if defined(HTTP_USE_POST)

/*********************************************************************
 * Function:        HTTP_IO_RESULT HTTPExecutePost(void)
 *
 * PreCondition:    curHTTP is loaded
 *
 * Input:           None
 *
 * Output:          HTTP_IO_DONE on success
 *					HTTP_IO_NEED_DATA if more data is requested
 *					HTTP_IO_WAITING if waiting for asynchronous process
 *
 * Side Effects:    None
 *
 * Overview:        This function is called if the request method was
 *					POST.  It is called after HTTPExecuteGet and 
 *					after any required authentication has been validated.
 *
 * Note:            In this example, this function calls additional
 *					helpers depending on which file was requested.
 ********************************************************************/
HTTP_IO_RESULT HTTPExecutePost(void)
{
	// Resolve which function to use and pass along
	BYTE filename[20];
	
	// Load the file name
	// Make sure BYTE filename[] above is large enough for your longest name
	MPFSGetFilename(curHTTP.file, filename, 20);
	
#if defined(USE_LCD)
	if(!memcmppgm2ram(filename, "forms.htm", 9))
		return HTTPPostLCD();
#endif

#if defined(STACK_USE_MD5)
	if(!memcmppgm2ram(filename, "upload.htm", 10))
		return HTTPPostMD5();
#endif

#if defined(STACK_USE_APP_RECONFIG)
	if(!memcmppgm2ram(filename, "protect/config.htm", 18))
		return HTTPPostConfig();
#endif

#if defined(STACK_USE_SMTP_CLIENT)
	if(!strcmppgm2ram((char*)filename, "email/index.htm"))
		return HTTPPostEmail();
#endif

	return HTTP_IO_DONE;
}

/*********************************************************************
 * Function:        HTTP_IO_RESULT HTTPPostLCD(void)
 *
 * PreCondition:    curHTTP is loaded
 *
 * Input:           None
 *
 * Output:          HTTP_IO_DONE on success
 *					HTTP_IO_NEED_DATA if more data is requested
 *					HTTP_IO_WAITING if waiting for asynchronous process
 *
 * Side Effects:    None
 *
 * Overview:        This function reads an input parameter "lcd" from
 *					the POSTed data, and writes that string to the
 *					board's LCD display.
 *
 * Note:            None
 ********************************************************************/
#if defined(USE_LCD)
static HTTP_IO_RESULT HTTPPostLCD(void)
{
	BYTE *ptr;
	WORD len;

	// Look for the lcd string
	len = TCPFindROMArray(sktHTTP, (ROM BYTE *)"lcd=", 4, 0, FALSE);
	
	// If not found, then throw away almost all the data we have and ask for more
	if(len == 0xffff)
	{
		curHTTP.byteCount -= TCPGetArray(sktHTTP, NULL, TCPIsGetReady(sktHTTP) - 4);
		return HTTP_IO_NEED_DATA;
	}
	
	// Throw away all data preceeding the lcd string
	curHTTP.byteCount -= TCPGetArray(sktHTTP, NULL, len);
	
	// Look for end of LCD string
	len = TCPFind(sktHTTP, '&', 0, FALSE);
	if(len == 0xffff)
		len = curHTTP.byteCount;
	
	// If not found, ask for more data
	if(curHTTP.byteCount > TCPIsGetReady(sktHTTP))
		return HTTP_IO_NEED_DATA;
		
	// Prevent buffer overflows
	if(len > HTTP_MAX_DATA_LEN - 2)
		len = HTTP_MAX_DATA_LEN - 2;
		
	// Read entire LCD update string into buffer and parse it
	len = TCPGetArray(sktHTTP, curHTTP.data, len);
	curHTTP.byteCount -= len;
	curHTTP.data[len] = '\0';
	ptr = HTTPURLDecode(curHTTP.data);
	ptr = HTTPGetROMArg(curHTTP.data, (ROM BYTE *)"lcd");
	
	// Copy up to 32 characters to the LCD
	if(strlen((char*)curHTTP.data) < 32u)
	{
		memset(LCDText, ' ', 32);
		strcpy((char*)LCDText, (char*)ptr);
	}
	else
	{
		memcpy(LCDText, (void *)ptr, 32);
	}
	
	LCDUpdate();
	
	strcpypgm2ram((char*)curHTTP.data, (ROM void*)"forms.htm");
	curHTTP.httpStatus = HTTP_REDIRECT;
	
	return HTTP_IO_DONE;
}
#endif




/*********************************************************************
 * Function:        void HTTPPrint_varname(TCP_SOCKET sktHTTP, 
 *							DWORD callbackPos, BYTE *data)
 *
 * PreCondition:    None
 *
 * Input:           sktHTTP: the TCP socket to which to write
 *					callbackPos: 0 initially
 *						return value of last call for subsequent callbacks
 *					data: this connection's data buffer
 *
 * Output:          0 if output is complete
 *					application-defined otherwise
 *
 * Side Effects:    None
 *
 * Overview:        Outputs a variable to the HTTP client.
 *
 * Note:            Return zero to indicate that this callback function 
 *					has finished writing data to the TCP socket.  A 
 *					non-zero return value indicates that more data 
 *					remains to be written, and this callback should 
 *					be called again when more space is available in 
 *					the TCP TX FIFO.  This non-zero return value will 
 *					be the value of the parameter callbackPos for the 
 *					next call.
 ********************************************************************/

ROM BYTE HTML_UP_ARROW[] = "up";
ROM BYTE HTML_DOWN_ARROW[] = "dn";

void HTTPPrint_btn(WORD num)
{
	// Determine which button
	switch(num)
	{
		case 0:
			num = BUTTON0_IO;
			break;
		case 1:
			num = BUTTON1_IO;
			break;
		case 2:
			num = BUTTON2_IO;
			break;
		case 3:
			num = BUTTON3_IO;
			break;
		default:
			num = 0;
	}

	// Print the output
	TCPPutROMString(sktHTTP, (num?HTML_UP_ARROW:HTML_DOWN_ARROW));
	return;
}
	
void HTTPPrint_led(WORD num)
{
	// Determine which LED
	switch(num)
	{
		case 0:
			num = LED0_IO;
			break;
		case 1:
			num = UIO1_IO;
			break;
		case 2:
			num = UIO2_IO;
			break;
		case 3:
			num = LED3_IO;
			break;
		case 4:
			num = LED4_IO;
			break;
		case 5:
			num = LED5_IO;
			break;
		case 6:
			num = LED6_IO;
			break;
		case 7:
			num = LED7_IO;
			break;

		default:
			num = 0;
	}

	// Print the output
	TCPPut(sktHTTP, (num?'1':'0'));
	return;
}

void HTTPPrint_ledSelected(WORD num, WORD state)
{
	// Determine which LED to check
	switch(num)
	{
		case 0:
			num = LED0_IO;
			break;
		case 1:
			num = LED1_IO;
			break;
		case 2:
			num = LED2_IO;
			break;
		case 3:
			num = LED3_IO;
			break;
		case 4:
			num = LED4_IO;
			break;
		case 5:
			num = LED5_IO;
			break;
		case 6:
			num = LED6_IO;
			break;
		case 7:
			num = LED7_IO;
			break;

		default:
			num = 0;
	}
	
	// Print output if TRUE and ON or if FALSE and OFF
	if((state && num) || (!state && !num))
		TCPPutROMString(sktHTTP, (ROM BYTE*)"SELECTED");
	return;
}

void HTTPPrint_pot(void)
{    
	char AN0String[8];
	WORD ADval;

#if defined(__18CXX)
    // Wait until A/D conversion is done
	ADCON0 = 0;
	ADCON1 = 0b00001011;				// A/DÝ’è
	ADCON2 = 0b10111110;
	ADCON0 = 0b00001101;			// Channel 3
    ADCON0bits.GO = 1;
    while(ADCON0bits.GO);

    // Convert 10-bit value into ASCII string
    ADval = (WORD)ADRES;
    ADval = ADval*50/1024;
    //ADval /= (WORD)102;
    uitoa(ADval, AN0String);
#else
	ADval = (WORD)ADC1BUF0;
	//ADval *= (WORD)10;
	//ADval /= (WORD)102;
    uitoa(ADval, (BYTE*)AN0String);
#endif

   	TCPPutArray(sktHTTP,(void *)AN0String, strlen((char*)AN0String));
}

void HTTPPrint_version(void)
{
	TCPPutROMArray(sktHTTP,(ROM void*)VERSION, strlenpgm((ROM char*)VERSION));
}

void HTTPPrint_builddate(void)
{
	TCPPutROMArray(sktHTTP,(ROM void*)__DATE__" "__TIME__, strlenpgm((ROM char*)__DATE__" "__TIME__));
}

void HTTPPrint_lcdtext(void)
{
	WORD len;

	// Determine how many bytes we can write
	len = TCPIsPutReady(sktHTTP);
	
	#if defined(USE_LCD)
	// If just starting, set callbackPos
	if(curHTTP.callbackPos == 0)
		curHTTP.callbackPos = 32;
	
	// Write a byte at a time while we still can
	// It may take up to 12 bytes to write a character
	// (spaces and newlines are longer)
	while(len > 12 && curHTTP.callbackPos)
	{
		// After 16 bytes write a newline
		if(curHTTP.callbackPos == 16)
			len -= TCPPutROMArray(sktHTTP, (ROM BYTE*)"<br />", 6);

		if(LCDText[32-curHTTP.callbackPos] == ' ' || LCDText[32-curHTTP.callbackPos] == '\0')
			len -= TCPPutROMArray(sktHTTP, (ROM BYTE*)"&nbsp;", 6);
		else
			len -= TCPPut(sktHTTP, LCDText[32-curHTTP.callbackPos]);

		curHTTP.callbackPos--;
	}
	#else
	TCPPutROMArray(sktHTTP, (ROM BYTE*)"No LCD Present", 14);
	#endif

	return;
}

void HTTPPrint_hellomsg(void)
{
	BYTE *ptr;
	
	ptr = HTTPGetROMArg(curHTTP.data, (ROM BYTE*)"name");
	
	// We omit checking for space because this is the only data being written
	if(ptr != NULL)
	{
		TCPPutROMArray(sktHTTP, (ROM BYTE*)"Hello, ", 7);
		TCPPutArray(sktHTTP, ptr, strlen((char*)ptr));
	}

	return;
}

void HTTPPrint_cookiename(void)
{
	BYTE *ptr;
	
	ptr = HTTPGetROMArg(curHTTP.data, (ROM BYTE*)"name");
	
	if(ptr)
		TCPPutArray(sktHTTP, ptr, strlen((char*)ptr));
	else
		TCPPutROMArray(sktHTTP, (ROM BYTE*)"not set", 7);
	
	return;
}

void HTTPPrint_uploadedmd5(void)
{
	BYTE i;

	// Set a flag to indicate not finished
	curHTTP.callbackPos = 1;
	
	// Make sure there's enough output space
	if(TCPIsPutReady(sktHTTP) < 32 + 37 + 5)
		return;

	// Check for flag set in HTTPPostMD5
	if(curHTTP.data[0] != 0x05)
	{// No file uploaded, so just return
		TCPPutROMArray(sktHTTP, (ROM BYTE*)"<b>Upload a File</b>", 20);
		curHTTP.callbackPos = 0;
		return;
	}
	
	TCPPutROMArray(sktHTTP, (ROM BYTE*)"<b>Uploaded File's MD5 was:</b><br />", 37);
	
	// Write a byte of the md5 sum at a time
	for(i = 1; i <= 16; i++)
	{
		TCPPut(sktHTTP, btohexa_high(curHTTP.data[i]));
		TCPPut(sktHTTP, btohexa_low(curHTTP.data[i]));
		if((i & 0x03) == 0)
			TCPPut(sktHTTP, ' ');
	}
	
	curHTTP.callbackPos = 0x00;
	return;
}

extern APP_CONFIG AppConfig;

void HTTPPrintIP(IP_ADDR ip)
{
	BYTE digits[4];
	BYTE i;
	
	for(i = 0; i < 4; i++)
	{
		if(i != 0)
			TCPPut(sktHTTP, '.');
		uitoa(ip.v[i], digits);
		TCPPutArray(sktHTTP, digits, strlen((char*)digits));
	}
}

void HTTPPrint_config_hostname(void)
{
	TCPPutArray(sktHTTP, AppConfig.NetBIOSName, strlen((char*)AppConfig.NetBIOSName));
	return;
}

void HTTPPrint_config_dhcpchecked(void)
{
	if(AppConfig.Flags.bIsDHCPEnabled)
		TCPPutROMArray(sktHTTP, (ROM BYTE*)"checked", 7);
	return;
}

void HTTPPrint_config_ip(void)
{
	HTTPPrintIP(AppConfig.MyIPAddr);
	return;
}

void HTTPPrint_config_gw(void)
{
	HTTPPrintIP(AppConfig.MyGateway);
	return;
}

void HTTPPrint_config_subnet(void)
{
	HTTPPrintIP(AppConfig.MyMask);
	return;
}

void HTTPPrint_config_dns1(void)
{
	HTTPPrintIP(AppConfig.PrimaryDNSServer);
	return;
}

void HTTPPrint_config_dns2(void)
{
	HTTPPrintIP(AppConfig.SecondaryDNSServer);
	return;
}

void HTTPPrint_config_mac(void)
{
	BYTE i;
	
	if(TCPIsPutReady(sktHTTP) < 18)
	{//need 17 bytes to write a MAC
		curHTTP.callbackPos = 0x01;
		return;
	}	
	
	// Write each byte
	for(i = 0; i < 6; i++)
	{
		if(i != 0)
			TCPPut(sktHTTP, ':');
		TCPPut(sktHTTP, btohexa_high(AppConfig.MyMACAddr.v[i]));
		TCPPut(sktHTTP, btohexa_low(AppConfig.MyMACAddr.v[i]));
	}
	
	// Indicate that we're done
	curHTTP.callbackPos = 0x00;
	return;
}

void HTTPPrint_reboot(void)
{
	// This is not so much a print function, but causes the board to reboot
	// when the configuration is changed.  If called via an AJAX call, this
	// will gracefully reset the board and bring it back online immediately
	Reset();
}

void HTTPPrint_rebootaddr(void)
{// This is the expected address of the board upon rebooting
	TCPPutArray(sktHTTP, curHTTP.data, strlen((char*)curHTTP.data));	
}

#endif
