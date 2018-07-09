/********************************************************************/

#ifndef USB_FUNCTION_HID_C
#define USB_FUNCTION_HID_C

/** INCLUDES *******************************************************/
#include "GenericTypeDefs.h"
#include "Compiler.h"
#include "./USB/usb.h"
#include "./USB/usb_function_hid.h"


/** VARIABLES ******************************************************/
#pragma udata
BYTE idle_rate;
BYTE active_protocol;   // [0] Boot Protocol [1] Report Protocol

/** EXTERNAL PROTOTYPES ********************************************/
#if defined USER_GET_REPORT_HANDLER
    void USER_GET_REPORT_HANDLER(void);
#endif

#if defined USER_SET_REPORT_HANDLER
    void USER_SET_REPORT_HANDLER(void);
#endif     

/** Section: DECLARATIONS ***************************************************/
#pragma code

/** Section: CLASS SPECIFIC REQUESTS ****************************************/

/********************************************************************/
void USBCheckHIDRequest(void)
{
    if(SetupPkt.Recipient != USB_SETUP_RECIPIENT_INTERFACE_BITFIELD) return;
    if(SetupPkt.bIntfID != HID_INTF_ID) return;
    
    if(SetupPkt.bRequest == USB_REQUEST_GET_DESCRIPTOR)
    {
        switch(SetupPkt.bDescriptorType)
        {
            case DSC_HID:           
                if(USBActiveConfiguration == 1)
                {
                    USBEP0SendROMPtr(
                        (ROM BYTE*)&configDescriptor1 + 18,
                        sizeof(USB_HID_DSC)+3,
                        USB_EP0_INCLUDE_ZERO);
                }
                break;
            case DSC_RPT:             
                if(USBActiveConfiguration == 1)
                {
                    USBEP0SendROMPtr(
                        (ROM BYTE*)&hid_rpt01,
                        sizeof(hid_rpt01),     //See usbcfg.h
                        USB_EP0_INCLUDE_ZERO);
                }
                break;
            case DSC_PHY:
                USBEP0Transmit(USB_EP0_NO_DATA);
                break;
        }//end switch(SetupPkt.bDescriptorType)
    }//end if(SetupPkt.bRequest == GET_DSC)
    
    if(SetupPkt.RequestType != USB_SETUP_TYPE_CLASS_BITFIELD)
    {
        return;
    }

    switch(SetupPkt.bRequest)
    {
        case GET_REPORT:
            #if defined USER_GET_REPORT_HANDLER
                USER_GET_REPORT_HANDLER();
            #endif
            break;
        case SET_REPORT:
            #if defined USER_SET_REPORT_HANDLER
                USER_SET_REPORT_HANDLER();
            #endif       
            break;
        case GET_IDLE:
            USBEP0SendRAMPtr(
                (BYTE*)&idle_rate,
                1,
                USB_EP0_INCLUDE_ZERO);
            break;
        case SET_IDLE:
            USBEP0Transmit(USB_EP0_NO_DATA);
            idle_rate = SetupPkt.W_Value.byte.HB;
            break;
        case GET_PROTOCOL:
            USBEP0SendRAMPtr(
                (BYTE*)&active_protocol,
                1,
                USB_EP0_NO_OPTIONS);
            break;
        case SET_PROTOCOL:
            USBEP0Transmit(USB_EP0_NO_DATA);
            active_protocol = SetupPkt.W_Value.byte.LB;
            break;
    }//end switch(SetupPkt.bRequest)

}//end USBCheckHIDRequest

/** USER API *******************************************************/

#endif
/** EOF usb_function_hid.c ******************************************************/
