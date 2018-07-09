/********************************************************************/

#ifndef USBMOUSE_C
#define USBMOUSE_C

/** INCLUDES *******************************************************/
#include "./USB/usb.h"
#include "HardwareProfile.h"
#include "./USB/usb_function_hid.h"

/** CONFIGURATION **************************************************/
//YTS #if defined(PICDEM_FS_USB)      // Configuration bits for PICDEM FS USB Demo Board (based on PIC18F4550)
        #pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
//YTS        #if (USB_SPEED_OPTION == USB_FULL_SPEED)
            #pragma config CPUDIV   = OSC1_PLL2  
//YTS        #else
//YTS            #pragma config CPUDIV   = OSC3_PLL4   
//YTS        #endif
        #pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
        #pragma config FOSC     = HS //HSPLL_HS  //YTS
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

//YTS REMOVED
/***************PINÉ}ÉNÉç*******************************************/
	#define		MOUSE_Pointing_Top			PORTBbits.RB0
	#define		MOUSE_Pointing_Bottom		PORTBbits.RB1
	#define		MOUSE_Pointing_Right		PORTBbits.RB2
	#define		MOUSE_Pointing_Left			PORTBbits.RB3
	#define		MOUSE_Click_Right			PORTBbits.RB4
	#define		MOUSE_Click_Left			PORTBbits.RB5

/** VARIABLES ******************************************************/
#pragma udata
BYTE old_sw2,old_sw3;
BOOL emulate_mode;
BYTE movement_length;
BYTE vector = 0;
char buffer[3];
USB_HANDLE lastTransmission1;//YTS
USB_HANDLE lastTransmission2;//YTS
int key_flag=0;//YTS

//The direction that the mouse will move in
ROM signed char dir_table[]={-4,-4,-4, 0, 4, 4, 4, 0};

/** PRIVATE PROTOTYPES *********************************************/
//YTS void BlinkUSBStatus(void);
//YTS BOOL Switch2IsPressed(void);
BOOL Switch3IsPressed(void);
void Emulate_Mouse(void);
void Keyboard(void);
static void InitializeSystem(void);
void ProcessIO(void);
void UserInit(void);
void YourHighPriorityISRCode();
void YourLowPriorityISRCode();
void USBCBSendResume(void);
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

/********************************************************************/
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
        USBDeviceTasks(); // Interrupt or polling method.  If using polling, must call
        #endif
    				  

        ProcessIO();        
    }//end while
}//end main


/*********************************************************************/
static void InitializeSystem(void)
{
    #if (defined(__18CXX) & !defined(PIC18F87J50_PIM))
        ADCON1 |= 0x0F;                 // Default all pins to digital
		INTCON2bits.RBPU=0;				//YTS ADD for pullup for port B
		TRISB = 0x0F;	//YTS ADD set PORTB(RB4 to RB7 pins) to output and PORTB(RB0 to RB3 pins) to input
						//YTS     "0" corresponds to "o"utput and "1" to "i"nput */
	
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
    buffer[0]=buffer[1]=buffer[2]=0;
    emulate_mode = TRUE;
    
    //initialize the variable holding the handle for the last
    // transmission
    lastTransmission1 = 0;    lastTransmission2 = 0;//YTS
}//end UserInit


/********************************************************************/
void ProcessIO(void)
{   
    //Blink the LEDs according to the USB device status
//YTS    BlinkUSBStatus();

    // User Application USB tasks
    if((USBDeviceState < CONFIGURED_STATE)||(USBSuspendControl==1)) return;

//YTS REMOVED
    
    //Call the function that emulates the mouse
    Emulate_Mouse();
	Keyboard();
}//end ProcessIO


/******************************************************************************/
void Emulate_Mouse(void)
{   
    static BOOL sent_dont_move = FALSE;

    if(emulate_mode == TRUE)
    {
        sent_dont_move = FALSE;

        //go 14 times in the same direction before changing direction
        if(movement_length > 14)
        {
            buffer[0] = 0;
            buffer[1] = dir_table[vector & 0x07];           // X-Vector
            buffer[2] = dir_table[(vector+2) & 0x07];       // Y-Vector
            //go to the next direction in the table
            vector++;
            //reset the counter for when to change again
            movement_length = 0;
        }//end if(movement_length > 14)
    }
    else
    {
        //don't move the mouse
        buffer[0] = buffer[1] = buffer[2] = 0;
    }

    if(HIDTxHandleBusy(lastTransmission1) == 0)
    {
        //copy over the data to the HID buffer
        hid_report_in1[0] = buffer[0];
        hid_report_in1[1] = buffer[1];
        hid_report_in1[2] = buffer[2];

     
        if(((sent_dont_move == FALSE) && (emulate_mode == FALSE)) || (emulate_mode == TRUE))
        {
            //Send the 3 byte packet over USB to the host.
            lastTransmission1 = HIDTxPacket(HID_EP1, (BYTE*)hid_report_in1, 0x03);
    
            //increment the counter of when to change the data sent
            movement_length++;

            sent_dont_move = TRUE;
        }
    }
}//end Emulate_Mouse

void Keyboard(void)
{
	static unsigned char key = 4;	

    if(!HIDTxHandleBusy(lastTransmission2))
    {
        if(MOUSE_Click_Right == 0)
        {
        	//Load the HID buffer
        	hid_report_in2[0] = 0;
        	hid_report_in2[1] = 0;
        	hid_report_in2[2] = key++;
        	hid_report_in2[3] = 0;
        	hid_report_in2[4] = 0;
        	hid_report_in2[5] = 0;
        	hid_report_in2[6] = 0;
        	hid_report_in2[7] = 0;
           	//Send the 8 byte packet over USB to the host.
           	lastTransmission2 = HIDTxPacket(HID_EP2, (BYTE*)hid_report_in2, 0x08);
    
            if(key == 40)
            {
                key = 4;
            }
			key_flag=1;
        }
        else
        {
        	//Load the HID buffer
        	hid_report_in2[0] = 0;
        	hid_report_in2[1] = 0;
        	hid_report_in2[2] = 0;   //Indicate no character pressed
        	hid_report_in2[3] = 0;
        	hid_report_in2[4] = 0;
        	hid_report_in2[5] = 0;
        	hid_report_in2[6] = 0;
        	hid_report_in2[7] = 0;
           	//Send the 8 byte packet over USB to the host.
           	lastTransmission2 = HIDTxPacket(HID_EP2, (BYTE*)hid_report_in2, 0x08);
			key_flag=0;
        }
    }
    return;		
}//end keyboard()

// ************** USB Callback Functions ****************************************************************

/******************************************************************************/
void USBCBSuspend(void)
{
	//ConfigureIOPinsForLowPower();
	//SaveStateOfAllInterruptEnableBits();
	//DisableAllInterruptEnableBits();
	//EnableOnlyTheInterruptsWhichWillBeUsedToWakeTheMicro();	//should enable at least USBActivityIF as a wake source
	//Sleep();
	//RestoreStateOfAllPreviouslySavedInterruptEnableBits();	//Preferrably, this should be done in the USBCBWakeFromSuspend() function instead.
	//RestoreIOPinsToNormal();									//Preferrably, this should be done in the USBCBWakeFromSuspend() function instead.
}
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
/*******************************************************************/
void USBCBInitEP(void)
{
    //enable the HID endpoint
    USBEnableEndpoint(HID_EP1,USB_IN_ENABLED|USB_HANDSHAKE_ENABLED|USB_DISALLOW_SETUP);//YTS
    USBEnableEndpoint(HID_EP2,USB_IN_ENABLED|USB_HANDSHAKE_ENABLED|USB_DISALLOW_SETUP);//YTS
}
/********************************************************************/
void USBCBSendResume(void)
{
    static WORD delay_count;
    
    if(USBGetRemoteWakeupStatus() == TRUE) 
    {
        //Verify that the USB bus is in fact suspended, before we send
        //remote wakeup signalling.
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
            
            //Now drive the resume K-state signalling onto the USB bus.
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
/** EOF mouse.c *************************************************/
#endif

