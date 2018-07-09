/********************************************************************/

#ifndef HARDWARE_PROFILE_PICDEM_FSUSB_H
#define HARDWARE_PROFILE_PICDEM_FSUSB_H

    //#define USE_SELF_POWER_SENSE_IO			//�Z���t�d�����o�F�g�����ǂ���
    #define tris_self_power     TRISAbits.TRISA2    // Input
    #if defined(USE_SELF_POWER_SENSE_IO)
    #define self_power          PORTAbits.RA2
    #else
    #define self_power          1
    #endif

    //#define USE_USB_BUS_SENSE_IO			//USB�o�X�d�����o�F�g�����ǂ���
    #define tris_usb_bus_sense  TRISAbits.TRISA1    // Input
    #if defined(USE_USB_BUS_SENSE_IO)
    #define USB_BUS_SENSE       PORTAbits.RA1
    #else
    #define USB_BUS_SENSE       1
    #endif


  //  #define PROGRAMMABLE_WITH_USB_HID_BOOTLOADER		

//YTS �ȉ����ׂč폜
#endif  //HARDWARE_PROFILE_PICDEM_FSUSB_H
