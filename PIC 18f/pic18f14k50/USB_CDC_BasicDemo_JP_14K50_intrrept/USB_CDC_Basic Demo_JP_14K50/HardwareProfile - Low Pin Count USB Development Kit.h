/********************************************************************
 FileName:      HardwareProfile - Low Pin Count USB Development Kit.h
 Dependencies:  See INCLUDES section
 Processor:     PIC18 or PIC24 USB Microcontrollers
 Hardware:      Low Pin Count USB Development Kit
 Compiler:      Microchip C18
 Company:       Microchip Technology, Inc.

 ���쌠�F�\�t�g�E�G�A���̂́AMicrochip�Ђ�Software License Agreement
         �ɏ����܂��B�K���AMicrochip�Ђ�Agreement���e���m�F���Ă��������B
         ���{��R�����g�Ȃǂ̒ǉ������̒��쌠�́A��҂��ۗL���A
         ��҂̋��𓾂Ȃ��A�S�̂���шꕔ���̓]�ځE���p���֎~���܂��B

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
    //MCHPFSUSB framework�𓮍삳���邽�߂ɕK�v�Ȓ�`�������ɏW�߂���
    //  ���܂��B�����Œ�߂���`�́Afirmware �ɂǂ̂悤�ȃ��[�h�œ���
    //  ����̂��������A����ɕK�v�ȏ���񋟂��܂��B
    //�ǂ̍��ڂ��A���Ȃ��̖ړI�Ƃ���A�v���P�[�V�����ɕK�v���A�T�d�Ɍ���
    //  ���A�K�؂ȕύX�������Ă��������B

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

    //HID Bootloader�𗘗p���� HEX �t�@�C�����o�͂���ɂ́A�ȉ��̍s�� 
    //  �R�����g���O���Ă��������B 
    #define PROGRAMMABLE_WITH_USB_HID_BOOTLOADER	

    /** Board definition ***********************************************/
    //�����ɏ�����Ă���̒�`�́Amain()���[�`���ɁA�ǂ̃{�[�h���g�p����
    //  ���邩���������̂ł��B  ���̒�`�ɂ��A�g�p����{�[�h�̓K�؂�
    //  configuration bits�̐ݒ��A���������\�ɂ��܂��B
    //�������A�����̒�`�́A�f���v���O�����p�ɏ������ꂽ���̂ł�����A
    //  ���Ȃ��̍쐬����ŏI�I�ȃA�v���P�[�V�����ɓK���������̂Ƃ͌���
    //  �܂���B
    
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
        //SW�͈������������Ă��Ȃ����߁A����SW�����L���܂��B
    #define mInitSwitch3()      //TRISAbits.TRISA3=1
    #define sw2                 PORTAbits.RA3
    #define sw3                 PORTAbits.RA3
    #define mInitAllSwitches()  mInitSwitch2();

    /** I/O pin definitions ********************************************/
    #define INPUT_PIN 1
    #define OUTPUT_PIN 0

#endif  //HARDWARE_PROFILE_LOW_PIN_COUNT_USB_DEVELOPMENT_KIT_H
