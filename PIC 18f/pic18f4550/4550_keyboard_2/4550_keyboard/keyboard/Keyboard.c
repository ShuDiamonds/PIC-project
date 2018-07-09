#ifndef KEYBOARD_C
#define KEYBOARD_C

/** INCLUDES *******************************************************/
#include "./USB/usb.h"
#include "HardwareProfile.h"
#include "./USB/usb_function_hid.h"

/** CONFIGURATION **************************************************/
//YTS #if defined(PICDEM_FS_USB)      // Configuration bits for PICDEM FS USB Demo Board (based on PIC18F4550)
        #pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
        #pragma config CPUDIV   = OSC1_PLL2   
        #pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
        #pragma config FOSC     = HSPLL_HS
        #pragma config FCMEN    = OFF
        #pragma config IESO     = OFF
        #pragma config PWRT     = OFF
        #pragma config BOR      = ON
        #pragma config BORV     = 3
        #pragma config VREGEN   = ON      //USB Voltage Regulator
        #pragma config WDT      = OFF
        #pragma config WDTPS    = 32768
        #pragma config MCLRE    = ON
        #pragma config LPT1OSC  = OFF
        #pragma config PBADEN   = OFF
//      #pragma config CCP2MX   = ON
        #pragma config STVREN   = ON
        #pragma config LVP      = OFF
//      #pragma config ICPRT    = OFF       // Dedicated In-Circuit Debug/Programming
        #pragma config XINST    = OFF       // Extended Instruction Set
        #pragma config CP0      = OFF
        #pragma config CP1      = OFF
//      #pragma config CP2      = OFF
//      #pragma config CP3      = OFF
        #pragma config CPB      = OFF
//      #pragma config CPD      = OFF
        #pragma config WRT0     = OFF
        #pragma config WRT1     = OFF
//      #pragma config WRT2     = OFF
//      #pragma config WRT3     = OFF
        #pragma config WRTB     = OFF       // Boot Block Write Protection
        #pragma config WRTC     = OFF
//      #pragma config WRTD     = OFF
        #pragma config EBTR0    = OFF
        #pragma config EBTR1    = OFF
//      #pragma config EBTR2    = OFF
//      #pragma config EBTR3    = OFF
        #pragma config EBTRB    = OFF

//YTS REMOVE

/** VARIABLES ******************************************************/
#pragma udata
BYTE old_sw2,old_sw3;
char buffer[8];
unsigned char OutBuffer[8];
USB_HANDLE lastINTransmission;
USB_HANDLE lastOUTTransmission;
BOOL Keyboard_out;
BOOL BlinkStatusValid;
DWORD CountdownTimerToShowUSBStatusOnLEDs;
/** PRIVATE PROTOTYPES *********************************************/
//void BlinkUSBStatus(void);
//BOOL Switch2IsPressed(void);
BOOL Switch3IsPressed(void);
static void InitializeSystem(void);
void ProcessIO(void);
void UserInit(void);
void YourHighPriorityISRCode();
void YourLowPriorityISRCode();
void USBCBSendResume(void);
void Keyboard(void);

void USBHIDCBSetReportComplete(void);

/** VECTOR REMAPPING ***********************************************/

		#define REMAPPED_RESET_VECTOR_ADDRESS			0x00
		#define REMAPPED_HIGH_INTERRUPT_VECTOR_ADDRESS	0x08
		#define REMAPPED_LOW_INTERRUPT_VECTOR_ADDRESS	0x18
	
	#pragma code HIGH_INTERRUPT_VECTOR = 0x08
	void High_ISR (void)
	{
	      #if defined(USB_INTERRUPT)
	        USBDeviceTasks();
        #endif
	}
	#pragma code LOW_INTERRUPT_VECTOR = 0x18
	void Low_ISR (void)
	{
		
	}
	#pragma code
/** DECLARATIONS ***************************************************/
#pragma code

/*********************************************************************/
#if defined(__18CXX)
void main(void)
#else
int main(void)
#endif
{
    InitializeSystem();

	#if defined(USB_INTERRUPT)
	    USBDeviceAttach();
	#endif

	while(1)
    {
		#if defined(USB_POLLING)
		// Check bus status and service USB interrupts.
		USBDeviceTasks(); // Interrupt or polling method.  If using polling, must call
						  
		#endif
		ProcessIO();        
	}//end while
}//end main
/********************************************************************/
static void InitializeSystem(void)
{
    #if (defined(__18CXX) & !defined(PIC18F87J50_PIM))
        ADCON1 |= 0x0F;                 // Default all pins to digital
 
    #endif

    #if defined(USE_USB_BUS_SENSE_IO)
    tris_usb_bus_sense = INPUT_PIN; // See HardwareProfile.h
    #endif
    
    #if defined(USE_SELF_POWER_SENSE_IO)
    tris_self_power = INPUT_PIN;	// See HardwareProfile.h
    #endif
    
    UserInit();

    USBDeviceInit();	//usb_device.c.  Initializes USB module SFRs and firmware
    					//variables to known states.
}//end InitializeSystem
/******************************************************************************/
void UserInit(void)
{
    //Initialize all of the LED pins
//YTS    mInitAllLEDs();
//YTS    BlinkStatusValid = TRUE;
    
    //Initialize all of the push buttons
    mInitAllSwitches();
    old_sw2 = sw2;
    old_sw3 = sw3;

    //initialize the variable holding the handle for the last
    // transmission

    lastINTransmission = 0;
    lastOUTTransmission = 0;

}//end UserInit
/********************************************************************/
void ProcessIO(void)
{   
    if((USBDeviceState < CONFIGURED_STATE)||(USBSuspendControl==1)) return;
    Keyboard();        
     
}//end ProcessIO


void Keyboard(void)
{
	static unsigned char key = 4;	

	//Check if the IN endpoint is not busy, and if it isn't check if we want to send 
	//keystroke data to the host.
    if(!HIDTxHandleBusy(lastINTransmission))		//前回のデータ送信が終了しているか判断する
    {
        if(Switch3IsPressed())
        {
        	//Load the HID buffer
        	hid_report_in[0] = 0;
        	hid_report_in[1] = 0;
        	hid_report_in[2] = key++;
        	hid_report_in[3] = 0;
        	hid_report_in[4] = 0;
        	hid_report_in[5] = 0;
        	hid_report_in[6] = 0;
        	hid_report_in[7] = 0;
           	//Send the 8 byte packet over USB to the host.
           	lastINTransmission = HIDTxPacket(HID_EP, (BYTE*)hid_report_in, 0x08);
    
            if(key == 40)
            {
                key = 4;
            }
        }
        else
        {
        	//Load the HID buffer
        	hid_report_in[0] = 0;
        	hid_report_in[1] = 0;
        	hid_report_in[2] = 0;   //Indicate no character pressed
        	hid_report_in[3] = 0;
        	hid_report_in[4] = 0;
        	hid_report_in[5] = 0;
        	hid_report_in[6] = 0;
        	hid_report_in[7] = 0;
           	//Send the 8 byte packet over USB to the host.
           	lastINTransmission = HIDTxPacket(HID_EP, (BYTE*)hid_report_in, 0x08);
        }
    }
    
    
//YTS REMOVE
    
    return;		
}//end keyboard()
/******************************************************************************/
BOOL Switch2IsPressed(void)
{
    if(sw2 != old_sw2)
    {
        old_sw2 = sw2;                  // Save new value
        if(sw2 == 0)                    // If pressed
            return TRUE;                // Was pressed
    }//end if
    return FALSE;                       // Was not pressed
}//end Switch2IsPressed
/******************************************************************************/
BOOL Switch3IsPressed(void)
{
    if(sw3 != old_sw3)
    {
        old_sw3 = sw3;                  // Save new value
        if(sw3 == 0)                    // If pressed
            return TRUE;                // Was pressed
    }//end if
    return FALSE;                       // Was not pressed
}//end Switch3IsPressed

void USBCBSuspend(void)
{
	
}
/******************************************************************************/
#if 0
void __attribute__ ((interrupt)) _USB1Interrupt(void)
{
    #if !defined(self_powered)
        if(U1OTGIRbits.ACTVIF)
        {       
            IEC5bits.USB1IE = 0;
            U1OTGIEbits.ACTVIE = 0;
            IFS5bits.USB1IF = 0;
        
            //USBClearInterruptFlag(USBActivityIFReg,USBActivityIFBitNum);
            USBClearInterruptFlag(USBIdleIFReg,USBIdleIFBitNum);
            //USBSuspendControl = 0;
        }
    #endif
}
#endif

/******************************************************************************/
void USBCBWakeFromSuspend(void)
{

}

/********************************************************************/
void USBCB_SOF_Handler(void)
{

}
/*******************************************************************/
void USBCBErrorHandler(void)
{

}
/*******************************************************************/
void USBCBCheckOtherReq(void)
{
    USBCheckHIDRequest();
}//end
/*******************************************************************/
void USBCBStdSetDscHandler(void)
{
	
}//end
/********************************************************************/
void USBCBInitEP(void)
{
    //enable the HID endpoint
    USBEnableEndpoint(HID_EP,USB_IN_ENABLED|USB_OUT_ENABLED|USB_HANDSHAKE_ENABLED|USB_DISALLOW_SETUP);

	lastOUTTransmission = HIDRxPacket(HID_EP,(BYTE*)&hid_report_out,1);

}
/********************************************************************/
void USBCBSendResume(void)
{
    static WORD delay_count;

    if(USBGetRemoteWakeupStatus() == TRUE) 
    {
     
        if(USBIsBusSuspended() == TRUE)
        {
            USBMaskInterrupts();
            
            //Clock switch to settings consistent with normal USB operation.
            USBCBWakeFromSuspend();
            USBSuspendControl = 0; 
            USBBusIsSuspended = FALSE;  //So we don't execute this code again, 

            delay_count = 3600U;        
            do
            {
                delay_count--;
            }while(delay_count);
            USBResumeControl = 1;       // Start RESUME signaling
            delay_count = 1800U;        // Set RESUME line for 1-13 ms
            do
            {
                delay_count--;
            }while(delay_count);
            USBResumeControl = 0;       //Finished driving resume signalling

            USBUnmaskInterrupts();
        }
    }
}


/*******************************************************************/
BOOL USER_USB_CALLBACK_EVENT_HANDLER(USB_EVENT event, void *pdata, WORD size)
{
    switch(event)
    {
        case EVENT_TRANSFER:
            //Add application specific callback task or callback function here if desired.
            break;
        case EVENT_SOF:
            USBCB_SOF_Handler();
            break;
        case EVENT_SUSPEND:
            USBCBSuspend();
            break;
        case EVENT_RESUME:
            USBCBWakeFromSuspend();
            break;
        case EVENT_CONFIGURED: 
            USBCBInitEP();
            break;
        case EVENT_SET_DESCRIPTOR:
            USBCBStdSetDscHandler();
            break;
        case EVENT_EP0_REQUEST:
            USBCBCheckOtherReq();
            break;
        case EVENT_BUS_ERROR:
            USBCBErrorHandler();
            break;
        case EVENT_TRANSFER_TERMINATED:
            break;
        default:
            break;
    }      
    return TRUE; 
}


// ************** USB Class Specific Callback Function(s) **********************
/*******************************************************************/
void USBHIDCBSetReportHandler(void)
{
	USBEP0Receive((BYTE*)&CtrlTrfData, USB_EP0_BUFF_SIZE, USBHIDCBSetReportComplete);
}


void USBHIDCBSetReportComplete(void)
{

	//Num Lock LED state is in Bit0.
	if(CtrlTrfData[0] & 0x01)	//Make LED1 and LED2 match Num Lock state.
	{
		mLED_1_On();
		mLED_2_On();
	}
	else
	{
		mLED_1_Off();
		mLED_2_Off();			
	}
	
	BlinkStatusValid = FALSE;	
	CountdownTimerToShowUSBStatusOnLEDs = 140000; 
}	
/** EOF Keyboard.c **********************************************/
#endif
