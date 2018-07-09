/********************************************************************
 FileName:      HardwareProfile - Low Pin Count USB Development Kit.h
 Dependencies:  See INCLUDES section
 Processor:     PIC18 or PIC24 USB Microcontrollers
 Hardware:      Low Pin Count USB Development Kit
 Compiler:      Microchip C18
 Company:       Microchip Technology, Inc.

 著作権：ソフトウエア自体は、Microchip社のSoftware License Agreement
         に準じます。必ず、Microchip社のAgreement内容を確認してください。
         日本語コメントなどの追加部分の著作権は、作者が保有し、
         作者の許可を得ない、全体および一部分の転載・引用を禁止します。

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

#ifndef HARDWARE_PROFILE_LOW_PIN_COUNT_USB_DEVELOPMENT_KIT_H
#define HARDWARE_PROFILE_LOW_PIN_COUNT_USB_DEVELOPMENT_KIT_H

    /*******************************************************************/
    /******** USB stack hardware selection options *********************/
    /*******************************************************************/
    //MCHPFSUSB frameworkを動作させるために必要な定義がここに集められて
    //  います。ここで定めれる定義は、firmware にどのようなモードで動作
    //  するのかを示し、動作に必要な情報を提供します。
    //どの項目が、あなたの目的とするアプリケーションに必要か、慎重に検討
    //  し、適切な変更を加えてください。

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

    /*******************************************************************/
    /*******************************************************************/
    /*******************************************************************/
    /******** Application specific definitions *************************/
    /*******************************************************************/
    /*******************************************************************/
    /*******************************************************************/

    //HID Bootloaderを利用する HEX ファイルを出力するには、以下の行の 
    //  コメントを外してください。 
    #define PROGRAMMABLE_WITH_USB_HID_BOOTLOADER	

    /** Board definition ***********************************************/
    //ここに書かれているの定義は、main()ルーチンに、どのボードが使用さて
    //  いるかを示すものです。  この定義により、使用するボードの適切な
    //  configuration bitsの設定や、初期化を可能にします。
    //しかし、これらの定義は、デモプログラム用に準備されたものですから、
    //  あなたの作成する最終的なアプリケーションに適合したものとは限り
    //  ません。
    
    #define DEMO_BOARD LOW_PIN_COUNT_USB_DEVELOPMENT_KIT
    #define LOW_PIN_COUNT_USB_DEVELOPMENT_KIT
    #define CLOCK_FREQ 48000000
    #define GetSystemClock() CLOCK_FREQ
    
    /** LED ************************************************************/
    #define mInitAllLEDs()      LATC &= 0xF0; TRISC &= 0xF0;
    
    #define mLED_1              LATCbits.LATC0
    #define mLED_2              LATCbits.LATC1
    #define mLED_3              LATCbits.LATC2
    #define mLED_4              LATCbits.LATC3
    
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
    
    /** SWITCH *********************************************************/
    #define mInitSwitch2()      //TRISAbits.TRISA3=1
        //SWは一つしか装備されていないため、そのSWを共有します。
    #define mInitSwitch3()      //TRISAbits.TRISA3=1
    #define sw2                 PORTAbits.RA3
    #define sw3                 PORTAbits.RA3
    #define mInitAllSwitches()  mInitSwitch2();

    /** I/O pin definitions ********************************************/
    #define INPUT_PIN 1
    #define OUTPUT_PIN 0

#endif  //HARDWARE_PROFILE_LOW_PIN_COUNT_USB_DEVELOPMENT_KIT_H
