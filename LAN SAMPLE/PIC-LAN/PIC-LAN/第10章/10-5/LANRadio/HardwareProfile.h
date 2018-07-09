/*********************************************************************
 *
 *	Hardware specific definitions
 *
 ********************************************************************/
// Clock frequency value.
// This value is used to calculate Tick Counter value
#define CLOCK_FREQ			(41666667ul)      // Hz
#define GetSystemClock()		(41666667ul)      // Hz
#define GetInstructionClock()	(GetSystemClock()/4)
#define GetPeripheralClock()	GetInstructionClock()
// I/O pins
#define LED0_TRIS				(TRISDbits.TRISD1)
#define LED0_IO				(LATDbits.LATD1)

#define BUTTON0_TRIS			(TRISFbits.TRISF3)
#define	BUTTON0_IO			(PORTFbits.RF3)
#define BUTTON1_TRIS			(TRISFbits.TRISF2)
#define	BUTTON1_IO			(PORTFbits.RF2)
#define BUTTON2_TRIS			(TRISFbits.TRISF1)
#define	BUTTON2_IO			(PORTFbits.RF1)

// Serial SRAM
#define SPIRAM_CS_TRIS			(TRISAbits.TRISA4)
#define SPIRAM_CS_IO			(LATAbits.LATA4)
#define SPIRAM_SCK_TRIS		(TRISCbits.TRISC3)
#define SPIRAM_SDI_TRIS		(TRISCbits.TRISC4)
#define SPIRAM_SDO_TRIS		(TRISCbits.TRISC5)
#define SPIRAM_SPI_IF			(PIR1bits.SSPIF)
#define SPIRAM_SSPBUF			(SSPBUF)
#define SPIRAM_SPICON1			(SSP1CON1)
#define SPIRAM_SPICON1bits		(SSP1CON1bits)
#define SPIRAM_SPICON2			(SSP1CON2)
#define SPIRAM_SPISTAT			(SSP1STAT)
#define SPIRAM_SPISTATbits		(SSP1STATbits)
#define SPIRAM2_CS_TRIS		(TRISAbits.TRISA5)
#define SPIRAM2_CS_IO			(LATAbits.LATA5)
#define SPIRAM2_SCK_TRIS		(TRISCbits.TRISC3)
#define SPIRAM2_SDI_TRIS		(TRISCbits.TRISC4)
#define SPIRAM2_SDO_TRIS		(TRISCbits.TRISC5)
#define SPIRAM2_SPI_IF			(PIR1bits.SSPIF)
#define SPIRAM2_SSPBUF			(SSPBUF)
#define SPIRAM2_SPICON1		(SSP1CON1)
#define SPIRAM2_SPICON1bits	(SSP1CON1bits)
#define SPIRAM2_SPICON2		(SSP1CON2)
#define SPIRAM2_SPISTAT		(SSP1STAT)
#define SPIRAM2_SPISTATbits	(SSP1STATbits)

// VLSI VS1011 MP3 decoder
#define MP3_DREQ_TRIS			(TRISCbits.TRISC0)	// Data Request
#define MP3_DREQ_IO 			(PORTCbits.RC0)		
#define MP3_XRESET_TRIS		(TRISCbits.TRISC7)	// Reset, active low
#define MP3_XRESET_IO			(LATCbits.LATC7)
#define MP3_XDCS_TRIS			(TRISCbits.TRISC6)	// Data Chip Select
#define MP3_XDCS_IO			(LATCbits.LATC6)
#define MP3_XCS_TRIS			(TRISCbits.TRISC1)	// Control Chip Select
#define MP3_XCS_IO			(LATCbits.LATC1)
#define MP3_SCK_TRIS			(TRISCbits.TRISC3)
#define MP3_SDI_TRIS			(TRISCbits.TRISC4)
#define MP3_SDO_TRIS			(TRISCbits.TRISC5)
#define MP3_SPI_IF			(PIR1bits.SSPIF)
#define MP3_SSPBUF			(SSPBUF)
#define MP3_SPICON1			(SSP1CON1)
#define MP3_SPICON1bits		(SSP1CON1bits)
#define MP3_SPICON2			(SSP1CON2)
#define MP3_SPISTAT			(SSP1STAT)
#define MP3_SPISTATbits		(SSP1STATbits)

