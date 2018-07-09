/*********************************************************************
 *
 *   液晶表示制御サブ関数
 *
 *********************************************************************/

#define __LCDBLOCKING_C

#include "TCPIP Stack/TCPIP.h"

#if defined(USE_LCD)

// LCDText is a 32 byte shadow of the LCD text.  Write to it and 
// then call LCDUpdate() to copy the string into the LCD module.
BYTE LCDText[16*2+1];

/******************************************************************************
 * Function:        static void LCDWrite(BYTE RS, BYTE Data)
 *
 * PreCondition:    None
 *
 * Input:           RS - Register Select - 1:RAM, 0:Config registers
 *					Data - 8 bits of data to write
 *
 * Output:          None
 *
 * Side Effects:    None
 *
 * Overview:        Controls the Port I/O pins to cause an LCD write
 *
 * Note:            None
 *****************************************************************************/
static void LCDWrite(BYTE RS, BYTE Data)
{
	LCD_DATA_TRIS = 0x00;
	LCD_DATA_IO = Data;		// 上位4ビット出力
	LCD_RS_IO = RS;
	Nop();					// Wait Data setup time (min 40ns)
	LCD_E_IO = 1;
	Nop();					// Wait E Pulse width time (min 230ns)
	Nop();
	Nop();
	LCD_E_IO = 0;	
	/// Lower
	LCD_DATA_IO = Data << 4;	// 下位4ビット出力
	LCD_RS_IO = RS;
	Nop();					// Wait Data setup time (min 40ns)
	Nop();
	LCD_E_IO = 1;
	Nop();					// Wait E Pulse width time (min 230ns)
	Nop();
	Nop();
	LCD_E_IO = 0;	
}


/******************************************************************************
 * Function:        void LCDInit(void)
 *
 * PreCondition:    None
 *
 * Input:           None
 *
 * Output:          None
 *
 * Side Effects:    None
 *
 * Overview:        LCDText[] is blanked, port I/O pin TRIS registers are 
 *					configured, and the LCD is placed in the default state
 *
 * Note:            None
 *****************************************************************************/
void LCDInit(void)
{
	BYTE i;

	memset(LCDText, ' ', sizeof(LCDText)-1);
	LCDText[sizeof(LCDText)-1] = 0;
	LCD_DATA_TRIS = 0x00;
	// Wait the required time for the LCD to reset
	DelayMs(40);
	LCD_RS_IO = 0;
	LCD_DATA_IO = 0x30;		// 8ビットモードの出力
	Nop();					// Wait Data setup time (min 40ns)
	for(i = 0; i < 3; i++)		// 3回出力
	{
		LCD_E_IO = 1;
		Delay10us(1);			// Wait E Pulse width time (min 230ns)
		LCD_E_IO = 0;
		Delay10us(5);			// 処理時間待ち
	}
	LCD_DATA_IO = 0x20;		// 4ビットモード出力
	Nop();					// Wait Data setup time (min 40ns)
	LCD_E_IO = 1;
	Delay10us(1);				// Wait E Pulse width time (min 230ns)
	LCD_E_IO = 0;
	Delay10us(5);				// 処理時間待ち
	// Set the entry mode
	LCDWrite(0, 0x06);		// Increment after each write, do not shift
	Delay10us(5);
	// Set the display control
	LCDWrite(0, 0x0C);		// Turn display on, no cusor, no cursor blink
	Delay10us(5);
	// Clear the display
	LCDWrite(0, 0x01);	
	DelayMs(2);
}


/******************************************************************************
 * Function:        void LCDUpdate(void)
 *
 * PreCondition:    LCDInit() must have been called once
 *
 * Input:           LCDText[]
 *
 * Output:          None
 *
 * Side Effects:    None
 *
 * Overview:        Copies the contents of the local LCDText[] array into the 
 *					LCD's internal display buffer.  Null terminators in 
 *					LCDText[] terminate the current line, so strings may be 
 *					printed directly to LCDText[].
 *
 * Note:            None
 *****************************************************************************/
void LCDUpdate(void)
{
	BYTE i, j;

	// Go home
	LCDWrite(0, 0x02);			// ホームコマンド出力
	DelayMs(2);					// 処理時間待ち
	// Output first line
	for(i = 0; i < 16u; i++)
	{
		// Erase the rest of the line if a null char is 
		// encountered (good for printing strings directly)
		if(LCDText[i] == 0u)
		{
			for(j=i; j < 16u; j++)
			{
				LCDText[j] = ' ';
			}
		}
		LCDWrite(1, LCDText[i]);
		Delay10us(5);
	}
	
	// Set the address to the second line
	LCDWrite(0, 0xC0);
	Delay10us(5);

	// Output second line
	for(i = 16; i < 32u; i++)
	{
		// Erase the rest of the line if a null char is 
		// encountered (good for printing strings directly)
		if(LCDText[i] == 0u)
		{
			for(j=i; j < 32u; j++)
			{
				LCDText[j] = ' ';
			}
		}
		LCDWrite(1, LCDText[i]);
		Delay10us(5);
	}
}

/******************************************************************************
 * Function:        void LCDErase(void)
 *
 * PreCondition:    LCDInit() must have been called once
 *
 * Input:           None
 *
 * Output:          None
 *
 * Side Effects:    None
 *
 * Overview:        Clears LCDText[] and the LCD's internal display buffer
 *
 * Note:            None
 *****************************************************************************/
void LCDErase(void)
{
	// Clear display
	LCDWrite(0, 0x01);
	DelayMs(2);

	// Clear local copy
	memset(LCDText, ' ', 32);
}

#endif	//#if defined(USE_LCD)
