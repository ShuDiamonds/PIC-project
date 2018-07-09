/********************************************************************
 FileName:     	HardwareProfile - PICDEM FSUSB.h
 Dependencies:  See INCLUDES section
 Processor:     PIC18 USB Microcontrollers
 Hardware:      PICDEM FSUSB
 Compiler:      Microchip C18
 Company:       Microchip Technology, Inc.

 Software License Agreement:

 The software supplied herewith by Microchip Technology Incorporated
 (the ìCompanyÅE for its PICÆ Microcontroller is intended and
 supplied to you, the Companyís customer, for use solely and
 exclusively on Microchip PIC Microcontroller products. The
 software is owned by the Company and/or its supplier, and is
 protected under applicable copyright laws. All rights are reserved.
 Any use in violation of the foregoing restrictions may subject the
 user to criminal sanctions under applicable laws, as well as to
 civil liability for the breach of the terms and conditions of this
 license.

 THIS SOFTWARE IS PROVIDED IN AN ìAS ISÅECONDITION. NO WARRANTIES,
 WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED
 TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT,
 IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR
 CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.

********************************************************************
 File Description:

 Change History:
  Rev   Date         Description
  1.0   11/19/2004   Initial release
  2.1   02/26/2007   Updated for simplicity and to use common
                     coding style
  2.3   09/15/2008   Broke out each hardware platform into its own
                     "HardwareProfile - xxx.h" file
********************************************************************/

#ifndef HARDWARE_PROFILE_PICDEM_FSUSB_H
#define HARDWARE_PROFILE_PICDEM_FSUSB_H
#include "Compiler.h"		//èCê≥Å@í«â¡
#include "usb_config.h"		//èCê≥Å@í«â¡
#include "FSconfig.h"		//èCê≥Å@í«â¡

    /*******************************************************************/
    /******** USB stack hardware selection options *********************/
    /*******************************************************************/
    //This section is the set of definitions required by the MCHPFSUSB
    //  framework.  These definitions tell the firmware what mode it is
    //  running in, and where it can find the results to some information
    //  that the stack needs.
    //These definitions are required by every application developed with
    //  this revision of the MCHPFSUSB framework.  Please review each
    //  option carefully and determine which options are desired/required
    //  for your application.

    //The PICDEM FS USB Demo Board platform supports the USE_SELF_POWER_SENSE_IO
    //and USE_USB_BUS_SENSE_IO features.  Uncomment the below line(s) if
    //it is desireable to use one or both of the features.
    //#define USE_SELF_POWER_SENSE_IO
    #define tris_self_power     TRISAbits.TRISA2    // Input
    #if defined(USE_SELF_POWER_SENSE_IO)
    #define self_power          PORTAbits.RA2
    #else
    #define self_power          1
    #endif

    //#define USE_USB_BUS_SENSE_IO
    #define tris_usb_bus_sense  TRISAbits.TRISA1    // Input
    #if defined(USE_USB_BUS_SENSE_IO)
    #define USB_BUS_SENSE       PORTAbits.RA1
    #else
    #define USB_BUS_SENSE       1
    #endif

    //Uncomment the following line to make the output HEX of this  
    //  project work with the MCHPUSB Bootloader    
    //#define PROGRAMMABLE_WITH_USB_MCHPUSB_BOOTLOADER
	
    //Uncomment the following line to make the output HEX of this 
    //  project work with the HID Bootloader
//    #define PROGRAMMABLE_WITH_USB_HID_BOOTLOADER			//èCê≥Å@ÉRÉÅÉìÉgÉAÉEÉg

    /*******************************************************************/
    /******** MDD File System selection options //èCê≥Å@ÉRÉÅÉìÉgÉAÉEÉg
    /*******************************************************************/
/*    #define USE_PIC18

    #define ERASE_BLOCK_SIZE        64
    #define WRITE_BLOCK_SIZE        32
*/

    /*******************************************************************/
    /******** MDD File System selection options  //èCê≥ à»â∫í«â¡
    /*******************************************************************/
    #define USE_PIC18

    #define USE_SD_INTERFACE_WITH_SPI

    #define INPUT_PIN           1
    #define OUTPUT_PIN          0

    // Chip Select Signal
    #define SD_CS               PORTBbits.RB3
    #define SD_CS_TRIS          TRISBbits.TRISB3
    
    // Card detect signal
    #define SD_CD               PORTBbits.RB4
    #define SD_CD_TRIS          TRISBbits.TRISB4
    
    // Write protect signal
    #define SD_WE               PORTBbits.RB2
    #define SD_WE_TRIS          TRISBbits.TRISB2
 
    // Registers for the SPI module you want to use
    #define SPICON1             SSPCON1
    #define SPISTAT             SSPSTAT
    #define SPIBUF              SSPBUF
    #define SPISTAT_RBF         SSPSTATbits.BF
    #define SPICON1bits         SSPCON1bits
    #define SPISTATbits         SSPSTATbits

    #define SPI_INTERRUPT_FLAG  PIR1bits.SSPIF   

    // Defines for the HPC Explorer board
    #define SPICLOCK            TRISBbits.TRISB1
    #define SPIIN               TRISBbits.TRISB0
    #define SPIOUT              TRISCbits.TRISC7

    // Latch pins for SCK/SDI/SDO lines
    #define SPICLOCKLAT         LATBbits.LATB1
    #define SPIINLAT            LATBbits.LATB0
    #define SPIOUTLAT           LATCbits.LATC7

    // Port pins for SCK/SDI/SDO lines
    #define SPICLOCKPORT        PORTBbits.RB1
    #define SPIINPORT           PORTBbits.RB0
    #define SPIOUTPORT          PORTCbits.RC7

    #define SPIENABLE           SSPCON1bits.SSPEN
    /*******************************************************************/
    /*******************************************************************/
    /*******************************************************************/
    /******** Application specific definitions *************************/
    /*******************************************************************/
    /*******************************************************************/
    /*******************************************************************/

    /** Board definition ***********************************************/
    //These defintions will tell the main() function which board is
    //  currently selected.  This will allow the application to add
    //  the correct configuration bits as wells use the correct
    //  initialization functions for the board.  These defitions are only
    //  required in the stack provided demos.  They are not required in
    //  final application design.
    #define DEMO_BOARD PICDEM_FS_USB
    #define PICDEM_FS_USB
    #define CLOCK_FREQ 48000000
    #define GetSystemClock()  CLOCK_FREQ		//èCê≥Å@í«â¡
    #define GetInstructionClock() CLOCK_FREQ   	//èCê≥Å@í«â¡

    /** LED ************************************************************///èCê≥ à»â∫ÉRÉÅÉìÉgÉAÉEÉg
/*
    #define mInitAllLEDs()      LATD &= 0xF0; TRISD &= 0xF0;
    
    #define mLED_1              LATDbits.LATD0
    #define mLED_2              LATDbits.LATD1
    #define mLED_3              LATDbits.LATD2
    #define mLED_4              LATDbits.LATD3
    
    #define mGetLED_1()         mLED_1
    #define mGetLED_2()         mLED_2
    #define mGetLED_3()         mLED_3
    #define mGetLED_4()         mLED_4

    #define mLED_1_On()         mLED_1 = 1;
    #define mLED_2_On()         mLED_2 = 1;
    #define mLED_3_On()         mLED_3 = 1;
    #define mLED_4_On()         mLED_4 = 1;
    
    #define mLED_1_Off()        mLED_1 = 0;
    #define mLED_2_Off()        mLED_2 = 0;
    #define mLED_3_Off()        mLED_3 = 0;
    #define mLED_4_Off()        mLED_4 = 0;
    
    #define mLED_1_Toggle()     mLED_1 = !mLED_1;
    #define mLED_2_Toggle()     mLED_2 = !mLED_2;
    #define mLED_3_Toggle()     mLED_3 = !mLED_3;
    #define mLED_4_Toggle()     mLED_4 = !mLED_4;
*/
    /** SWITCH *********************************************************/
/*
    #define mInitAllSwitches()  TRISBbits.TRISB4=1;TRISBbits.TRISB5=1;
    #define mInitSwitch2()      TRISBbits.TRISB4=1;
    #define mInitSwitch3()      TRISBbits.TRISB5=1;
    #define sw2                 PORTBbits.RB4
    #define sw3                 PORTBbits.RB5
 */   
    /** USB external transceiver interface (optional) ******************/
/* 
   #define tris_usb_vpo        TRISBbits.TRISB3    // Output
    #define tris_usb_vmo        TRISBbits.TRISB2    // Output
    #define tris_usb_rcv        TRISAbits.TRISA4    // Input
    #define tris_usb_vp         TRISCbits.TRISC5    // Input
    #define tris_usb_vm         TRISCbits.TRISC4    // Input
    #define tris_usb_oe         TRISCbits.TRISC1    // Output
    
    #define tris_usb_suspnd     TRISAbits.TRISA3    // Output
*/
    /** I/O pin definitions ********************************************/
/*
    #define INPUT_PIN 1
    #define OUTPUT_PIN 0
*/  
#endif  //HARDWARE_PROFILE_PICDEM_FSUSB_H
