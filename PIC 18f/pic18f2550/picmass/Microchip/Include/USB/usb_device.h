/********************************************************************************/

#ifndef USBDEVICE_H
#define USBDEVICE_H
//DOM-IGNORE-END

/** DEFINITIONS ****************************************************/

#define USB_HANDLE void*

#define USB_EP0_ROM            0x00     //Data comes from RAM
#define USB_EP0_RAM            0x01     //Data comes from ROM
#define USB_EP0_BUSY           0x80     //The PIPE is busy
#define USB_EP0_INCLUDE_ZERO   0x40     //include a trailing zero packet
#define USB_EP0_NO_DATA        0x00     //no data to send
#define USB_EP0_NO_OPTIONS     0x00     //no options set

/********************************************************************/

typedef enum
{
   
    DETACHED_STATE 
    /*DOM-IGNORE-BEGIN*/    = 0x00                         /*DOM-IGNORE-END*/,
    /* Attached is the state in which the device is attached ot the bus but the
    hub/port that it is attached to is not yet configured. */
    ATTACHED_STATE
    /*DOM-IGNORE-BEGIN*/    = 0x01                         /*DOM-IGNORE-END*/,
    /* Powered is the state in which the device is attached to the bus and the 
    hub/port that it is attached to is configured. */
    POWERED_STATE
    /*DOM-IGNORE-BEGIN*/    = 0x02                         /*DOM-IGNORE-END*/,
    /* Default state is the state after the device receives a RESET command from
    the host. */
    DEFAULT_STATE
    /*DOM-IGNORE-BEGIN*/    = 0x04                         /*DOM-IGNORE-END*/,
    /* Address pending state is not an official state of the USB defined states.
    This state is internally used to indicate that the device has received a 
    SET_ADDRESS command but has not received the STATUS stage of the transfer yet.
    The device is should not switch addresses until after the STATUS stage is
    complete.  */
    ADR_PENDING_STATE
    /*DOM-IGNORE-BEGIN*/    = 0x08                         /*DOM-IGNORE-END*/,
    /* Address is the state in which the device has its own specific address on the
    bus. */
    ADDRESS_STATE
    /*DOM-IGNORE-BEGIN*/    = 0x10                         /*DOM-IGNORE-END*/,
    /* Configured is the state where the device has been fully enumerated and is
    operating on the bus.  The device is now allowed to excute its application
    specific tasks.  It is also allowed to increase its current consumption to the
    value specified in the configuration descriptor of the current configuration.
    */
    CONFIGURED_STATE
    /*DOM-IGNORE-BEGIN*/    = 0x20                        /*DOM-IGNORE-END*/
} USB_DEVICE_STATE;


/* USB device stack events description here - DWF */
typedef enum
{
    // Notification that a SET_CONFIGURATION() command was received (device)
    EVENT_CONFIGURED
    /*DOM-IGNORE-BEGIN*/    = EVENT_DEVICE_STACK_BASE        /*DOM-IGNORE-END*/,

    // A SET_DESCRIPTOR request was received (device)
    EVENT_SET_DESCRIPTOR,

    // An endpoint 0 request was received that the stack did not know how to
    // handle.  This is most often a request for one of the class drivers.  
    // Please refer to the class driver documenation for information related
    // to what to do if this request is received. (device)
    EVENT_EP0_REQUEST,

    EVENT_ATTACH                 

} USB_DEVICE_STACK_EVENTS;

/** Function Prototypes **********************************************/

/**************************************************************************


    Typical usage:
    <code>
    void main(void)
    {
        USBDeviceInit()
        while(1)
        {
            USBDeviceTasks();
            if((USBGetDeviceState() \< CONFIGURED_STATE) ||
               (USBIsDeviceSuspended() == TRUE))
            {
                //Either the device is not configured or we are suspended
                //  so we don't want to do execute any application code
                continue;   //go back to the top of the while loop
            }
            else
            {
                //Otherwise we are free to run user application code.
                UserApplication();
            }
        }
    }
    </code>
                 
  **************************************************************************/
void USBDeviceTasks(void);

/**************************************************************************/
void USBDeviceInit(void);

/********************************************************************/
BOOL USBGetRemoteWakeupStatus(void);
/*DOM-IGNORE-BEGIN*/
#define USBGetRemoteWakeupStatus() RemoteWakeup
/*DOM-IGNORE-END*/

/***************************************************************************

    
    Typical usage:
    <code>
    void main(void)
    {
        USBDeviceInit()
        while(1)
        {
            USBDeviceTasks();
            if((USBGetDeviceState() \< CONFIGURED_STATE) ||
               (USBIsDeviceSuspended() == TRUE))
            {
                //Either the device is not configured or we are suspended
                //  so we don't want to do execute any application code
                continue;   //go back to the top of the while loop
            }
            else
            {
                //Otherwise we are free to run user application code.
                UserApplication();
            }
        }
    }
                                                           
  ***************************************************************************/
USB_DEVICE_STATE USBGetDeviceState(void);
/*DOM-IGNORE-BEGIN*/
#define USBGetDeviceState() USBDeviceState
/*DOM-IGNORE-END*/

/***************************************************************************

    <code>
       void main(void)
       {
           USBDeviceInit()
           while(1)
           {
               USBDeviceTasks();
               if((USBGetDeviceState() \< CONFIGURED_STATE) ||
                  (USBIsDeviceSuspended() == TRUE))
               {
                   //Either the device is not configured or we are suspended
                   //  so we don't want to do execute any application code
                   continue;   //go back to the top of the while loop
               }
               else
               {
                   //Otherwise we are free to run user application code.
                   UserApplication();
               }
           }
       }
                                                 
  ***************************************************************************/
BOOL USBGetSuspendState(void);

/*******************************************************************************/
void USBEnableEndpoint(BYTE ep, BYTE options);

/********************************************************************************/
BOOL USBIsDeviceSuspended(void);
/*DOM-IGNORE-BEGIN*/
#define USBIsDeviceSuspended() USBSuspendControl 
/*DOM-IGNORE-END*/

/********************************************************************************/
void USBSoftDetach(void);
/*DOM-IGNORE-BEGIN*/
#define USBSoftDetach()  U1CON = 0; U1IE = 0; USBDeviceState = DETACHED_STATE;
/*DOM-IGNORE-END*/

/*************************************************************************/
USB_HANDLE USBTransferOnePacket(BYTE ep,BYTE dir,BYTE* data,BYTE len);

/*************************************************************************/
BOOL USBHandleBusy(USB_HANDLE handle);
/*DOM-IGNORE-BEGIN*/
#define USBHandleBusy(handle) (handle==0?0:((volatile BDT_ENTRY*)handle)->STAT.UOWN)
/*DOM-IGNORE-END*/

/*********************************************************************/
WORD USBHandleGetLength(USB_HANDLE handle);
/*DOM-IGNORE-BEGIN*/
#define USBHandleGetLength(handle) (((volatile BDT_ENTRY*)handle)->CNT)
/*DOM-IGNORE-END*/

/*********************************************************************/
WORD USBHandleGetAddr(USB_HANDLE);
/*DOM-IGNORE-BEGIN*/
#define USBHandleGetAddr(handle) (((volatile BDT_ENTRY*)handle)->ADR)
/*DOM-IGNORE-END*/

/********************************************************************/
void USBEP0Transmit(BYTE options);
/*DOM-IGNORE-BEGIN*/
#define USBEP0Transmit(options) inPipes[0].info.Val = options | USB_EP0_BUSY
/*DOM-IGNORE-END*/

/***************************************************************************/
void USBEP0SendRAMPtr(BYTE* src, WORD size, BYTE Options);
/*DOM-IGNORE-BEGIN*/
#define USBEP0SendRAMPtr(src,size,options)  {\
            inPipes[0].pSrc.bRam = src;\
            inPipes[0].wCount.Val = size;\
            inPipes[0].info.Val = options | USB_EP0_BUSY | USB_EP0_RAM;\
            }
/*DOM-IGNORE-END*/

/****************************************************************************/
void USBEP0SendROMPtr(BYTE* src, WORD size, BYTE Options);
/*DOM-IGNORE-BEGIN*/
#define USBEP0SendROMPtr(src,size,options)  {\
            inPipes[0].pSrc.bRom = src;\
            inPipes[0].wCount.Val = size;\
            inPipes[0].info.Val = options | USB_EP0_BUSY | USB_EP0_ROM;\
            }
/*DOM-IGNORE-END*/

/***************************************************************************/
void USBEP0Receive(BYTE* dest, WORD size, void (*function));
/*DOM-IGNORE-BEGIN*/
#define USBEP0Receive(dest,size,function)  {outPipes[0].pDst.bRam = dest;outPipes[0].wCount.Val = size;outPipes[0].pFunc = function;outPipes[0].info.bits.busy = 1; }
/*DOM-IGNORE-END*/

/********************************************************************/
USB_HANDLE USBTxOnePacket(BYTE ep, BYTE* data, WORD len);
/*DOM-IGNORE-BEGIN*/
#define USBTxOnePacket(ep,data,len)     USBTransferOnePacket(ep,IN_TO_HOST,data,len)
/*DOM-IGNORE-END*/

/*********************************************************************/
USB_HANDLE USBRxOnePacket(BYTE ep, BYTE* data, WORD len);
/*DOM-IGNORE-BEGIN*/
#define USBRxOnePacket(ep,data,len)      USBTransferOnePacket(ep,OUT_FROM_HOST,data,len)
/*DOM-IGNORE-END*/

/*********************************************************************/
void USBStallEndpoint(BYTE ep, BYTE dir);

/******************************************************************************/
void USBDeviceDetach(void);

/**************************************************************************
****************************************************************************/
void USBDeviceAttach(void);

/*************************************************************************************/
BOOL USB_APPLICATION_EVENT_HANDLER(BYTE address, USB_EVENT event, void *pdata, WORD size);
/*******************************************************************************/
ROM void *USBDeviceCBGetDescriptor (UINT16 *length, DESCRIPTOR_ID *id);
/**************************************************************************/
void USBCancelIO(BYTE endpoint);


/** Section: MACROS ******************************************************/

#define DESC_CONFIG_WORD(a) (a&0xFF),((a>>8)&0xFF)


#define DESC_CONFIG_DWORD(a) (a&0xFF),((a>>8)&0xFF),((a>>16)&0xFF),((a>>24)&0xFF)


#define DESC_CONFIG_BYTE(a) (a)

















/* DOM-IGNORE-BEGIN */
/*******************************************************************************
********************************************************************************
********************************************************************************
    This section contains implementation specific information that may vary
    between releases as the implementation needs to change.  This section is
    included for compilation reasons only.
********************************************************************************
********************************************************************************
*******************************************************************************/

#if defined(USB_POLLING)
    #define USB_VOLATILE
#else
    #define USB_VOLATILE volatile
#endif

#define CTRL_TRF_RETURN void
#define CTRL_TRF_PARAMS void

// Defintion of the PIPE structure
//  This structure is used to keep track of data that is sent out
//  of the stack automatically.
typedef struct __attribute__ ((packed))
{
    union __attribute__ ((packed))
    {
        //Various options of pointers that are available to
        // get the data from
        BYTE *bRam;
        ROM BYTE *bRom;
        WORD *wRam;
        ROM WORD *wRom;
    }pSrc;
    union __attribute__ ((packed))
    {
        struct __attribute__ ((packed))
        {
            //is this transfer from RAM or ROM?
            BYTE ctrl_trf_mem          :1;
            BYTE reserved              :5;
            //include a zero length packet after
            //data is done if data_size%ep_size = 0?
            BYTE includeZero           :1;
            //is this PIPE currently in use
            BYTE busy                  :1;
        }bits;
        BYTE Val;
    }info;
    WORD_VAL wCount;
}IN_PIPE;

extern USB_VOLATILE IN_PIPE inPipes[];

typedef struct __attribute__ ((packed))
{
    union __attribute__ ((packed))
    {
        //Various options of pointers that are available to
        // get the data from
        BYTE *bRam;
        WORD *wRam;
    }pDst;
    union __attribute__ ((packed))
    {
        struct __attribute__ ((packed))
        {
            BYTE reserved              :7;
            //is this PIPE currently in use
            BYTE busy                  :1;
        }bits;
        BYTE Val;
    }info;
    WORD_VAL wCount;
    CTRL_TRF_RETURN (*pFunc)(CTRL_TRF_PARAMS);
}OUT_PIPE;

/************* DWF - SHOULD BE REIMPLEMENTED AS AN EVENT *******************/
//#if defined(ENABLE_EP0_DATA_RECEIVED_CALLBACK)
//    void USBCBEP0DataReceived(void);
//    #define USBCB_EP0_DATA_RECEIVED() USBCBEP0DataReceived()
//#else
//    #define USBCB_EP0_DATA_RECEIVED()
//#endif

extern USB_VOLATILE BOOL RemoteWakeup;
extern USB_VOLATILE USB_DEVICE_STATE USBDeviceState;
extern USB_VOLATILE BYTE USBActiveConfiguration;
/******************************************************************************/
/* DOM-IGNORE-END */

#endif //USBD_H
