
#ifndef _USB_H_
#define _USB_H_
//DOM-IGNORE-END
// *****************************************************************************


#include "GenericTypeDefs.h"
#include "Compiler.h"

#include "usb_config.h"             // Must be defined by the application

#include "usb/usb_common.h"         // Common USB library definitions
#include "usb/usb_ch9.h"            // USB device framework definitions

#if defined( USB_SUPPORT_DEVICE )
    #include "usb/usb_device.h"     // USB Device abstraction layer interface
#endif

#if defined( USB_SUPPORT_HOST )
    #include "usb/usb_host.h"       // USB Host abstraction layer interface
#endif

#if defined ( USB_SUPPORT_OTG )
    #include "usb/usb_otg.h" 
#endif

#include "usb/usb_hal.h"            // Hardware Abstraction Layer interface

// *****************************************************************************


#define USB_MAJOR_VER   2       // Firmware version, major release number.
#define USB_MINOR_VER   6       // Firmware version, minor release number.
#define USB_DOT_VER     0       // Firmware version, dot release number.

#endif // _USB_H_

