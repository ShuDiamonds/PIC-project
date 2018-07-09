/********************************************************************/

#ifndef __USB_DESCRIPTORS_C
#define __USB_DESCRIPTORS_C

/** INCLUDES *******************************************************/
#include "./USB/usb.h"
#include "./USB/usb_function_hid.h"

/** CONSTANTS ******************************************************/
#if defined(__18CXX)
#pragma romdata
#endif

/* Device Descriptor */
ROM USB_DEVICE_DESCRIPTOR device_dsc=
{
    0x12,    // Size of this descriptor in bytes
    USB_DESCRIPTOR_DEVICE,                // DEVICE descriptor type
    0x0200,                 // USB Spec Release Number in BCD format
    0x00,                   // Class Code
    0x00,                   // Subclass code
    0x00,                   // Protocol code
    USB_EP0_BUFF_SIZE,          // Max packet size for EP0, see usb_config.h
    MY_VID,                 // Vendor ID
    MY_PID,                 // Product ID: Mouse in a circle fw demo
    0x0003,                 // Device release number in BCD format
    0x01,                   // Manufacturer string index
    0x02,                   // Product string index
    0x00,                   // Device serial number string index
    0x01                    // Number of possible configurations
};

/* Configuration 1 Descriptor */
ROM BYTE configDescriptor1[]={
    /* Configuration Descriptor */
    0x09,//sizeof(USB_CFG_DSC),    // Size of this descriptor in bytes
    USB_DESCRIPTOR_CONFIGURATION,                // CONFIGURATION descriptor type
    DESC_CONFIG_WORD(0x0042),  //YTS  // Total length of data for this cfg
    2,   //YTS                   // Number of interfaces in this cfg
    1,                      // Index value of this configuration
    0,                      // Configuration string index
    _DEFAULT | _SELF,               // Attributes, see usb_device.h
    50,                     // Max power consumption (2X mA)

    /* Interface Descriptor1 */
    0x09,//sizeof(USB_INTF_DSC),   // Size of this descriptor in bytes
    USB_DESCRIPTOR_INTERFACE,               // INTERFACE descriptor type
    0,                      // Interface Number
    0,                      // Alternate Setting Number
    1,                      // Number of endpoints in this intf
    HID_INTF,               // Class code
    BOOT_INTF_SUBCLASS,     // Subclass code
    HID_PROTOCOL_MOUSE,     // Protocol code
    0,                      // Interface string index

    /* HID Class-Specific Descriptor */
    0x09,//sizeof(USB_HID_DSC)+3,    // Size of this descriptor in bytes RRoj hack
    DSC_HID,                // HID descriptor type
    DESC_CONFIG_WORD(0x0111),                 // HID Spec Release Number in BCD format (1.11)
    0x00,                   // Country Code (0x00 for Not supported)
    HID_NUM_OF_DSC,         // Number of class descriptors, see usbcfg.h
    DSC_RPT,                // Report descriptor type
    DESC_CONFIG_WORD(50),   //sizeof(hid_rpt01),      // Size of the report descriptor
    
    /* Endpoint Descriptor */
    0x07,/*sizeof(USB_EP_DSC)*/
    USB_DESCRIPTOR_ENDPOINT,    //Endpoint Descriptor
    HID_EP1 | _EP_IN,  //YTS          //EndpointAddress
    _INTERRUPT,                       //Attributes
    DESC_CONFIG_WORD(3),                  //size
    0x01,                        //Interval


//YTS ADD
 /* Interface Descriptor2 */
    0x09,//sizeof(USB_INTF_DSC),   // Size of this descriptor in bytes
    USB_DESCRIPTOR_INTERFACE,               // INTERFACE descriptor type
    1, //YTS                     // Interface Number
    0,                      // Alternate Setting Number
    1,                      // Number of endpoints in this intf
    HID_INTF,               // Class code
    BOOT_INTF_SUBCLASS,     // Subclass code
    HID_PROTOCOL_KEYBOARD,     // Protocol code
    0,                      // Interface string index

    /* HID Class-Specific Descriptor */
    0x09,//sizeof(USB_HID_DSC)+3,    // Size of this descriptor in bytes RRoj hack
    DSC_HID,                // HID descriptor type
    DESC_CONFIG_WORD(0x0111),                 // HID Spec Release Number in BCD format (1.11)
    0x00,                   // Country Code (0x00 for Not supported)
    HID_NUM_OF_DSC,         // Number of class descriptors, see usbcfg.h
    DSC_RPT,                // Report descriptor type
    DESC_CONFIG_WORD(63),   //sizeof(hid_rpt02),      // Size of the report descriptor

    /* Endpoint Descriptor */
    0x07,/*sizeof(USB_EP_DSC)*/
    USB_DESCRIPTOR_ENDPOINT,    //Endpoint Descriptor
    HID_EP2 | _EP_IN,            //EndpointAddress
    _INTERRUPT,                       //Attributes
    DESC_CONFIG_WORD(8),        //size
    0x01,                        //Interval

    /* Endpoint Descriptor */
    0x07,/*sizeof(USB_EP_DSC)*/
    USB_DESCRIPTOR_ENDPOINT,    //Endpoint Descriptor
    HID_EP2 | _EP_OUT,            //EndpointAddress
    _INTERRUPT,                       //Attributes
    DESC_CONFIG_WORD(8),        //size
    0x01                        //Interval

};


//Language code string descriptor
ROM struct{BYTE bLength;BYTE bDscType;WORD string[1];}sd000={
sizeof(sd000),USB_DESCRIPTOR_STRING,{0x0409
}};

//Manufacturer string descriptor
ROM struct{BYTE bLength;BYTE bDscType;WORD string[25];}sd001={
sizeof(sd001),USB_DESCRIPTOR_STRING,
{'M','i','c','r','o','c','h','i','p',' ',
'T','e','c','h','n','o','l','o','g','y',' ','I','n','c','.'
}};

//Product string descriptor
ROM struct{BYTE bLength;BYTE bDscType;WORD string[22];}sd002={
sizeof(sd002),USB_DESCRIPTOR_STRING,
{'M','o','u','s','e',' ','I','n',' ','a',' ',
'C','i','r','c','l','e',' ','D','e','m','o'
}};

//Class specific descriptor - HID mouse
ROM struct{BYTE report[HID_RPT01_SIZE];}hid_rpt01={
    {0x05, 0x01, /* Usage Page (Generic Desktop)             */
    0x09, 0x02, /* Usage (Mouse)                            */
    0xA1, 0x01, /* Collection (Application)                 */
    0x09, 0x01, /*  Usage (Pointer)                         */
    0xA1, 0x00, /*  Collection (Physical)                   */
    0x05, 0x09, /*      Usage Page (Buttons)                */
    0x19, 0x01, /*      Usage Minimum (01)                  */
    0x29, 0x03, /*      Usage Maximum (03)                  */
    0x15, 0x00, /*      Logical Minimum (0)                 */
    0x25, 0x01, /*      Logical Maximum (0)                 */
    0x95, 0x03, /*      Report Count (3)                    */
    0x75, 0x01, /*      Report Size (1)                     */
    0x81, 0x02, /*      Input (Data, Variable, Absolute)    */
    0x95, 0x01, /*      Report Count (1)                    */
    0x75, 0x05, /*      Report Size (5)                     */
    0x81, 0x01, /*      Input (Constant)    ;5 bit padding  */
    0x05, 0x01, /*      Usage Page (Generic Desktop)        */
    0x09, 0x30, /*      Usage (X)                           */
    0x09, 0x31, /*      Usage (Y)                           */
    0x15, 0x81, /*      Logical Minimum (-127)              */
    0x25, 0x7F, /*      Logical Maximum (127)               */
    0x75, 0x08, /*      Report Size (8)                     */
    0x95, 0x02, /*      Report Count (2)                    */
    0x81, 0x06, /*      Input (Data, Variable, Relative)    */
    0xC0, 0xC0}
};/* End Collection,End Collection            */

ROM struct{BYTE report[HID_RPT02_SIZE];}hid_rpt02={
{   0x05, 0x01,                    // USAGE_PAGE (Generic Desktop)
    0x09, 0x06,                    // USAGE (Keyboard)
    0xa1, 0x01,                    // COLLECTION (Application)
    0x05, 0x07,                    //   USAGE_PAGE (Keyboard)
    0x19, 0xe0,                    //   USAGE_MINIMUM (Keyboard LeftControl)
    0x29, 0xe7,                    //   USAGE_MAXIMUM (Keyboard Right GUI)
    0x15, 0x00,                    //   LOGICAL_MINIMUM (0)
    0x25, 0x01,                    //   LOGICAL_MAXIMUM (1)
    0x75, 0x01,                    //   REPORT_SIZE (1)
    0x95, 0x08,                    //   REPORT_COUNT (8)
    0x81, 0x02,                    //   INPUT (Data,Var,Abs)
    0x95, 0x01,                    //   REPORT_COUNT (1)
    0x75, 0x08,                    //   REPORT_SIZE (8)
    0x81, 0x03,                    //   INPUT (Cnst,Var,Abs)
    0x95, 0x05,                    //   REPORT_COUNT (5)
    0x75, 0x01,                    //   REPORT_SIZE (1)
    0x05, 0x08,                    //   USAGE_PAGE (LEDs)
    0x19, 0x01,                    //   USAGE_MINIMUM (Num Lock)
    0x29, 0x05,                    //   USAGE_MAXIMUM (Kana)
    0x91, 0x02,                    //   OUTPUT (Data,Var,Abs)
    0x95, 0x01,                    //   REPORT_COUNT (1)
    0x75, 0x03,                    //   REPORT_SIZE (3)
    0x91, 0x03,                    //   OUTPUT (Cnst,Var,Abs)
    0x95, 0x06,                    //   REPORT_COUNT (6)
    0x75, 0x08,                    //   REPORT_SIZE (8)
    0x15, 0x00,                    //   LOGICAL_MINIMUM (0)
    0x25, 0x65,                    //   LOGICAL_MAXIMUM (101)
    0x05, 0x07,                    //   USAGE_PAGE (Keyboard)
    0x19, 0x00,                    //   USAGE_MINIMUM (Reserved (no event indicated))
    0x29, 0x65,                    //   USAGE_MAXIMUM (Keyboard Application)
    0x81, 0x00,                    //   INPUT (Data,Ary,Abs)
    0xc0}                          // End Collection
};

//Array of configuration descriptors
ROM BYTE *ROM USB_CD_Ptr[]=
{
    (ROM BYTE *ROM)&configDescriptor1
};

//Array of string descriptors
ROM BYTE *ROM USB_SD_Ptr[]=
{
    (ROM BYTE *ROM)&sd000,
    (ROM BYTE *ROM)&sd001,
    (ROM BYTE *ROM)&sd002
};

/** EOF usb_descriptors.c ***************************************************/

#endif
