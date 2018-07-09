/*******************************************************************************/
//DOM-IGNORE-END

#ifndef USB_HAL_PIC18_H
#define USB_HAL_PIC18_H

/*****************************************************************/
#define USBPowerModule()

/****************************************************************/
#define USBModuleDisable() {\
    UCON = 0;\
    UIE = 0;\
    USBDeviceState = DETACHED_STATE;\
}    

/****************************************************************/
#define USBSetBDTAddress(addr)

/********************************************************************/
#define USBClearInterruptFlag(reg_name, if_and_flag_mask)	(reg_name &= if_and_flag_mask)	

/********************************************************************/
#define USBClearInterruptRegister(reg) reg = 0;

#if defined(USB_DISABLE_SOF_HANDLER)
    #define USB_SOF_INTERRUPT 0x00
#else
    #define USB_SOF_INTERRUPT 0x40
#endif

#if defined(USB_DISABLE_ERROR_HANDLER)
    #define USB_ERROR_INTERRUPT 0x02
#else
    #define USB_ERROR_INTERRUPT 0x02
#endif

//STALLIE, IDLEIE, TRNIE, and URSTIE are all enabled by default and are required
#define USBEnableInterrupts() {RCONbits.IPEN = 1;IPR2bits.USBIP = 1;PIE2bits.USBIE = 1;INTCONbits.GIEH = 1;}
#if defined(USB_INTERRUPT)
    #define USBMaskInterrupts() {PIE2bits.USBIE = 0;}
    #define USBUnmaskInterrupts() {PIE2bits.USBIE = 1;}
#else
    #define USBMaskInterrupts() 
    #define USBUnmaskInterrupts() 
#endif

#define ENDPOINT_MASK 0b01111000


#define USBPingPongBufferReset UCONbits.PPBRST


#define USBTransactionCompleteIE UIEbits.TRNIE
#define USBTransactionCompleteIF UIRbits.TRNIF
#define USBTransactionCompleteIFReg UIR
#define USBTransactionCompleteIFBitNum 0xF7		//AND mask for clearing TRNIF bit position 4

#define USBResetIE  UIEbits.URSTIE
#define USBResetIF  UIRbits.URSTIF
#define USBResetIFReg UIR
#define USBResetIFBitNum 0xFE					//AND mask for clearing URSTIF bit position 0

#define USBIdleIE UIEbits.IDLEIE
#define USBIdleIF UIRbits.IDLEIF
#define USBIdleIFReg UIR
#define USBIdleIFBitNum 0xEF					//AND mask for clearing IDLEIF bit position 5

#define USBActivityIE UIEbits.ACTVIE
#define USBActivityIF UIRbits.ACTVIF
#define USBActivityIFReg UIR
#define USBActivityIFBitNum 0xFB				//AND mask for clearing ACTVIF bit position 2

#define USBSOFIE UIEbits.SOFIE
#define USBSOFIF UIRbits.SOFIF
#define USBSOFIFReg UIR
#define USBSOFIFBitNum 0xBF						//AND mask for clearing SOFIF bit position 6

#define USBStallIE UIEbits.STALLIE
#define USBStallIF UIRbits.STALLIF
#define USBStallIFReg UIR
#define USBStallIFBitNum 0xDF					//AND mask for clearing STALLIF bit position 5

#define USBErrorIE UIEbits.UERRIE
#define USBErrorIF UIRbits.UERRIF
#define USBErrorIFReg UIR
#define USBErrorIFBitNum 0xFD					//UERRIF bit position 1.  Note: This bit is read only and is cleared by clearing the enabled UEIR flags

#define USBSE0Event UCONbits.SE0
#define USBSuspendControl UCONbits.SUSPND
#define USBPacketDisable UCONbits.PKTDIS
#define USBResumeControl UCONbits.RESUME

#define U1ADDR UADDR
#define U1IE UIE
#define U1IR UIR
#define U1EIR UEIR
#define U1EIE UEIE
#define U1CON UCON
#define U1EP0 UEP0
#define U1CONbits UCONbits
#define U1EP1 UEP1
#define U1CNFG1 UCFG
#define U1STAT USTAT
#define U1EP0bits UEP0bits

/* Buffer Descriptor Status Register Initialization Parameters */
#define _BSTALL     0x04        //Buffer Stall enable
#define _DTSEN      0x08        //Data Toggle Synch enable
#define _INCDIS     0x10        //Address increment disable
#define _KEN        0x20        //SIE keeps buff descriptors enable
#define _DAT0       0x00        //DATA0 packet expected next
#define _DAT1       0x40        //DATA1 packet expected next
#define _DTSMASK    0x40        //DTS Mask
#define _USIE       0x80        //SIE owns buffer
#define _UCPU       0x00        //CPU owns buffer

#define _STAT_MASK  0xFF

/* BDT entry structure definition */
typedef union _BD_STAT
{
    BYTE Val;
    struct{
        //If the CPU owns the buffer then these are the values
        unsigned BC8:1;         //bit 8 of the byte count
        unsigned BC9:1;         //bit 9 of the byte count
        unsigned BSTALL:1;      //Buffer Stall Enable
        unsigned DTSEN:1;       //Data Toggle Synch Enable
        unsigned INCDIS:1;      //Address Increment Disable
        unsigned KEN:1;         //BD Keep Enable
        unsigned DTS:1;         //Data Toggle Synch Value
        unsigned UOWN:1;        //USB Ownership
    };
    struct{
        //if the USB module owns the buffer then these are
        // the values
        unsigned BC8:1;         //bit 8 of the byte count
        unsigned BC9:1;         //bit 9 of the byte count
        unsigned PID0:1;        //Packet Identifier
        unsigned PID1:1;
        unsigned PID2:1;
        unsigned PID3:1;
        unsigned :1;
        unsigned UOWN:1;        //USB Ownership
    };
    struct{
        unsigned :2;
        unsigned PID:4;         //Packet Identifier
        unsigned :2;
    };
} BD_STAT;                      //Buffer Descriptor Status Register

// BDT Entry Layout
typedef union __BDT
{
    struct
    {
        BD_STAT STAT;
        BYTE CNT;
        BYTE ADRL;                      //Buffer Address Low
        BYTE ADRH;                      //Buffer Address High
    };
    struct
    {
        unsigned :8;
        unsigned :8;
        BYTE* ADR;                      //Buffer Address
    };
    DWORD Val;
    BYTE v[4];
} BDT_ENTRY;

//Definitions for the BDT
#ifndef USB_PING_PONG_MODE
    #error "No ping pong mode defined."
#endif

#if (USB_PING_PONG_MODE == USB_PING_PONG__NO_PING_PONG)
    extern volatile BDT_ENTRY BDT[(USB_MAX_EP_NUMBER + 1) * 2];
#elif (USB_PING_PONG_MODE == USB_PING_PONG__EP0_OUT_ONLY)
    extern volatile BDT_ENTRY BDT[((USB_MAX_EP_NUMBER+1) * 2)+1];
#elif (USB_PING_PONG_MODE == USB_PING_PONG__FULL_PING_PONG)
    extern volatile BDT_ENTRY BDT[(USB_MAX_EP_NUMBER + 1) * 4];
#elif (USB_PING_PONG_MODE == USB_PING_PONG__ALL_BUT_EP0)
    extern volatile BDT_ENTRY BDT[((USB_MAX_EP_NUMBER + 1) * 4)-2];
#else
    #error "No ping pong mode defined."
#endif

#define USTAT_EP0_PP_MASK   ~0x02
#define USTAT_EP_MASK       0x7E
#define USTAT_EP0_OUT       0x00
#define USTAT_EP0_OUT_EVEN  0x00
#define USTAT_EP0_OUT_ODD   0x02

#define USTAT_EP0_IN        0x04
#define USTAT_EP0_IN_EVEN   0x04
#define USTAT_EP0_IN_ODD    0x06

typedef union
{
    WORD UEP[16];
} _UEP;

#define UEP_STALL 0x0001

#define USB_BDT_ADDRESS 0x400

typedef union _POINTER
{
    struct
    {
        BYTE bLow;
        BYTE bHigh;
        //byte bUpper;
    };
    WORD _word;                         // bLow & bHigh
    
    //pFunc _pFunc;                       // Usage: ptr.pFunc(); Init: ptr.pFunc = &<Function>;

    BYTE* bRam;                         // Ram byte pointer: 2 bytes pointer pointing
                                        // to 1 byte of data
    WORD* wRam;                         // Ram word poitner: 2 bytes poitner pointing
                                        // to 2 bytes of data

    ROM BYTE* bRom;                     // Size depends on compiler setting
    ROM WORD* wRom;
    //rom near byte* nbRom;               // Near = 2 bytes pointer
    //rom near word* nwRom;
    //rom far byte* fbRom;                // Far = 3 bytes pointer
    //rom far word* fwRom;
} POINTER;

    //******** Depricated: v2.2 - will be removed at some point of time ***
    #define _LS         0x00            // Use Low-Speed USB Mode
    #define _FS         0x04            // Use Full-Speed USB Mode
    #define _TRINT      0x00            // Use internal transceiver
    #define _TREXT      0x08            // Use external transceiver
    #define _PUEN       0x10            // Use internal pull-up resistor
    #define _OEMON      0x40            // Use SIE output indicator
    //**********************************************************************

    #define USB_PULLUP_ENABLE 0x10
    #define USB_PULLUP_DISABLED 0x00
    
    #define USB_INTERNAL_TRANSCEIVER 0x00
    #define USB_EXTERNAL_TRANSCEIVER 0x08
    
    #define USB_FULL_SPEED 0x04
    #define USB_LOW_SPEED  0x00

#define ConvertToPhysicalAddress(a) a
#define USBClearUSBInterrupt() PIR2bits.USBIF = 0;

#if !defined(USBDEVICE_C)
    //extern USB_VOLATILE USB_DEVICE_STATE USBDeviceState;
    extern USB_VOLATILE BYTE USBActiveConfiguration;
    extern USB_VOLATILE IN_PIPE inPipes[1];
    extern USB_VOLATILE OUT_PIPE outPipes[1];
    extern volatile BDT_ENTRY *pBDTEntryIn[USB_MAX_EP_NUMBER+1];
#endif

/* Endpoint configuration options for USBEnableEndpoint() function */
#define EP_CTRL     0x06            // Cfg Control pipe for this ep
#define EP_OUT      0x0C            // Cfg OUT only pipe for this ep
#define EP_IN       0x0A            // Cfg IN only pipe for this ep
#define EP_OUT_IN   0x0E            // Cfg both OUT & IN pipes for this ep

// Handshake should be disable for isoch
#define USB_HANDSHAKE_ENABLED   0x10
#define USB_HANDSHAKE_DISABLED  0x00

#define USB_OUT_ENABLED         0x04
#define USB_OUT_DISABLED        0x00

#define USB_IN_ENABLED          0x02
#define USB_IN_DISABLED         0x00

#define USB_ALLOW_SETUP         0x00
#define USB_DISALLOW_SETUP      0x08

#define USB_STALL_ENDPOINT      0x01

#define SetConfigurationOptions()   {\
                                        U1CNFG1 = USB_PULLUP_OPTION | USB_TRANSCEIVER_OPTION | USB_SPEED_OPTION | USB_PING_PONG_MODE;\
                                        U1EIE = 0x9F;\
                                        UIE = 0x39 | USB_SOF_INTERRUPT | USB_ERROR_INTERRUPT;\
                                    }  

#endif //#ifndef USB_HAL_PIC18_H
