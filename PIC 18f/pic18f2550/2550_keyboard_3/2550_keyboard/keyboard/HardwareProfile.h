#ifndef HARDWARE_PROFILE_H
#define HARDWARE_PROFILE_H

//#define DEMO_BOARD USER_DEFINED_BOARD

#if !defined(DEMO_BOARD)


    #if defined(__18CXX)
        #if defined(__18F4550)
            #include "HardwareProfile - PICDEM FSUSB.h"
        #elif defined(__18F87J50)
            #include "HardwareProfile - PIC18F87J50 PIM.h"
        #elif defined(__18F14K50)
            #include "HardwareProfile - Low Pin Count USB Development Kit.h"
        #elif defined(__18F46J50)
            #if defined(PIC18F_STARTER_KIT_1)
                #include "HardwareProfile - PIC18F Starter Kit 1.h"
            #else
                #include "HardwareProfile - PIC18F46J50 PIM.h"
            #endif
        #elif defined(__18F47J53)
            #include "HardwareProfile - PIC18F47J53 PIM.h"
        #endif
    #endif
#endif

/*

#if !defined(DEMO_BOARD)
    #error "Demo board not defined.  Either define DEMO_BOARD for a custom board or select the correct processor for the demo board."
#endif

*/

#endif  //HARDWARE_PROFILE_H
