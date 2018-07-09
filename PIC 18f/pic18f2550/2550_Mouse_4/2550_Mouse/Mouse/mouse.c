/********************************************************************/

#ifndef USBMOUSE_C
#define USBMOUSE_C

/** INCLUDES *******************************************************/
#include "./USB/usb.h"
#include "HardwareProfile.h"
#include "./USB/usb_function_hid.h"
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <adc.h>

/** CONFIGURATION **************************************************/
        #pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
        #pragma config CPUDIV   = OSC1_PLL2   
        #pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
        #pragma config FOSC     = HSPLL_HS
        #pragma config FCMEN    = OFF
        #pragma config IESO     = OFF
        #pragma config PWRT     = OFF
        #pragma config BOR      = OFF
        #pragma config BORV     = 3
  //      #pragma config VREGEN   = ON      //USB Voltage Regulator
        #pragma config VREGEN   = OFF      //USB Voltage Regulator
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

/***************PINマクロ*******************************************/
	#define		MOUSE_Pointing_Top			PORTBbits.RB0
	#define		MOUSE_Pointing_Bottom		PORTBbits.RB1
	#define		MOUSE_Pointing_Right		PORTBbits.RB2
	#define		MOUSE_Pointing_Left			PORTBbits.RB3
	#define		MOUSE_Click_Right			PORTBbits.RB4
	#define		MOUSE_Click_Left			PORTBbits.RB5
	
/** VARIABLES ******************************************************/
#pragma udata
BOOL emulate_mode;
BYTE INPUT_VALUE = 0;
char buffer[3];
USB_HANDLE lastTransmission;

long int X1=0;
long int Y1=0;
long int Z1=0;
signed long int X2=0;
signed long int Y2=0;
/** PRIVATE PROTOTYPES *********************************************/
void Emulate_Mouse(void);
static void InitializeSystem(void);
void ProcessIO(void);
void UserInit(void);
void YourHighPriorityISRCode();
void YourLowPriorityISRCode();

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
	//PIN初期化
		TRISA=0xFF;                        // ポートAをすべて出力ピンにする
		TRISB=0xFF;                        // ポートBをすべて出力ピンにする
		TRISC=0x00;                        // ポートCをすべて入力ピンにする
	
		
	
	
	//UART初期化
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 77); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600の時で64にする。また48Mhzで9600の時で77にする
	
	//fprintf(_H_USART,"\rHello world\n");		//「Hello」と出力
	
		//AD変換初期化
	//define mode of A/D
	//OpenADC(ADC_FOSC_16 & ADC_RIGHT_JUST & ADC_8ANA_0REF,ADC_CH0 & ADC_INT_OFF);
	
		OpenADC(ADC_FOSC_64 &           //AD変換用クロック　　システムクロックの1/64　0.05μsec×64＝3.2μsec　>=　1.6μsec　→　OK
				ADC_RIGHT_JUST &        //変換結果の保存方法　左詰め　
				ADC_8_TAD,              //AD変換のアクイジションタイム選択　3.2μsec（=1Tad）×8Tad=25.6μsec　＞＝　12.8μsec　→　OK
				ADC_CH0 &                       //AD変換するのチャンネル選択（PIC18Fは同時に複数のAD変換はできない）
				ADC_INT_OFF &           //AD変換での割込み使用の有無
				ADC_VREFPLUS_VDD &      //Vref+の設定　　　ＰＩＣの電源電圧と同じ：ADC_VREFPLUS_VDD 　or　外部（ＡＮ３）の電圧：ADC_VREFPLUS_EXT
				ADC_VREFMINUS_VSS,      //Vref-の設定　　　ＰＩＣの0Ｖ：ADC_VREFMINUS_VSS    or　外部（AN2)の電圧：ADC_VREFMINUS_EXT
				0b1110  //ポートのアナログ・デジタル選択　（ADCON1の下位４ビットを記載）　　AN0のみアナログポートを選択、他はデジタルポートを選択
				//例 　アナログポートが　AN0のみ → 0b1110　　、AN0 & AN1　→　0b1011、 AN0 & AN1 & AN2 →1100　他　詳細データシート参照
			);
			
		SetChanADC(ADC_CH0);	//Select Channel 0
	
	
	
		#if defined(USE_USB_BUS_SENSE_IO)
		tris_usb_bus_sense = INPUT_PIN; // See HardwareProfile.h	　USBバス電圧検出用ピン
		#endif


		#if defined(USE_SELF_POWER_SENSE_IO)	//セルフ電圧検出　：　オプション
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
    lastTransmission = 0;
}//end UserInit


/********************************************************************/
void ProcessIO(void)
{   
    if((USBDeviceState < CONFIGURED_STATE)||(USBSuspendControl==1)) return;

    Emulate_Mouse();
    
}//end ProcessIO


/******************************************************************************/
void Emulate_Mouse(void)
{   
    if(emulate_mode == TRUE)
    {
        buffer[0] = buffer[1] = buffer[2] = 0;	//送信データ初期化
    	
    	
    	/*
    	
    	//マウス左右情報取得
    	if(MOUSE_Pointing_Right == 0)				//右ボタンが選択されているとき
    	{
    		buffer[1] = 1;
    	}else if(MOUSE_Pointing_Left == 0)			//左ボタンが選択されているとき
    	{
    		buffer[1] = -1;
    	}else
    	{
    		buffer[1] = 0;
    	}
    	//マウス上下情報取得
    	if(MOUSE_Pointing_Top == 0)			//上ボタンが選択されているとき
    	{
    		buffer[2] = -1;
    	}else if(MOUSE_Pointing_Bottom == 0)	//下ボタンが選択されているとき
    	{
    		buffer[2] = 1;
    	}else								//上下ボタンが押されてないとき
    	{
    		buffer[2] = 0;
    	}
    	
    	*/
    	
    	
    	
    	
    	SetChanADC(ADC_CH0);	//Select Channel 0
		Delay100TCYx(5);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		X1 = ReadADC();	//Get A/D data
    	fprintf(_H_USART,"\r read of buffer[1]=%ld\n",X1);	
    	X2 = X1-514;
    	buffer[1] = X2 / 2;
    	fprintf(_H_USART,"\r change of buffer[1]=%d\n",buffer[1]);
		
    	SetChanADC(ADC_CH1);	//Select Channel 0
		Delay100TCYx(5);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		Y1 = ReadADC();	//Get A/D data
    	fprintf(_H_USART,"\r read of buffer[2]=%ld\n",Y1);	
    	Y2 = Y1-503;
    	buffer[2] = Y2 / 2;
    	fprintf(_H_USART,"\r change of buffer[2]=%d\n",buffer[2]);
    	
    	
    	
    	

    	
    	
    	//********角度確認プログラム*************//
    	/*
    	SetChanADC(ADC_CH0);	//Select Channel 0
		Delay100TCYx(5);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		X1 = ReadADC();	//Get A/D data
    	
    	SetChanADC(ADC_CH1);	//Select Channel 0
		Delay100TCYx(5);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		Y1 = ReadADC();	//Get A/D data
    	
    	SetChanADC(ADC_CH2);	//Select Channel 0
		Delay100TCYx(5);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		Z1 = ReadADC();	//Get A/D data
    	
    	fprintf(_H_USART,"\r (X,Y,Z)=(%ld,%ld,%ld) \n" ,X1,Y1,Z1);	
    	
    	X1=0;
    	Y1=0;
    	Z1=0;
    	*/
    	//********角度確認プログラム終了*************//
    	
    	/*
    	
    	//マウスクリック情報取得
    	if (MOUSE_Click_Right == 0) {			//右クリック
			buffer[0] |= 0x01;
		}
		if (MOUSE_Click_Left == 0) {			//左クリック
			buffer[0] |= 0x02;
		}
    
    	
    	*/
    	
    }else
    {
        //don't move the mouse
        buffer[0] = buffer[1] = buffer[2] = 0;
    }

    if(HIDTxHandleBusy(lastTransmission) == 0)
    {
        //copy over the data to the HID buffer
        hid_report_in[0] = buffer[0];
        hid_report_in[1] = buffer[1];
        hid_report_in[2] = buffer[2];
       
    	/*
    	参考
    	・　キーボードが出力するデータ列（８バイト）：　Modifier（ビット０～７）がそれぞれ 左Ｃｔｒｌ、左Ｓｈｉｆｔ、・・など）、Reserved(0)、 残り６バイトは Keycode（0x04-0x1d: a-z、0x1e-0x27: 1-0、0x27-0x2c: Enter,Esc,Back,Space,Tab,Space、0x2d-0x39: _, =, [, ], \, #, ;, ", ~, ,, ., /, CapsLock、0x3a-0x45: F1-F12、0x46-0x48: PrtScr,Scroll Lock,Pause、0x49-0x4b: Insert,Home,PageUp、0x4c-0x4e: Del,End,PageDown）

    	*/
    	
    	/*
    	 hid_report_in[1]=1,hid_report_in[2] = 1の時画面右したに行く
    	 hid_report_in[1] = -1,hid_report_in[2] = 1の時画面左下に行く
    	hid_report_in[1] = 1,hid_report_in[2] = -1の時画面右上に行く
    	hid_report_in[1] = -1,hid_report_in[2] = -1の時画面左上に行く
    	
    	*/
     
        //Send the 3 byte packet over USB to the host.
        lastTransmission = HIDTxPacket(HID_EP, (BYTE*)hid_report_in, 0x03);
    }
}//end Emulate_Mouse

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
    // Must claim session ownership if supporting this request
}//end


/*******************************************************************/
void USBCBInitEP(void)
{
    //enable the HID endpoint
    USBEnableEndpoint(HID_EP,USB_IN_ENABLED|USB_HANDSHAKE_ENABLED|USB_DISALLOW_SETUP);
}

/********************************************************************/
void USBCBSendResume(void)
{
    static WORD delay_count;
    
    USBResumeControl = 1;                // Start RESUME signaling
    
    delay_count = 1800U;                // Set RESUME line for 1-13 ms
    do
    {
        delay_count--;
    }while(delay_count);
    USBResumeControl = 0;
}


/*******************************************************************/
BOOL USER_USB_CALLBACK_EVENT_HANDLER(USB_EVENT event, void *pdata, WORD size)
{
    switch(event)
    {
        case EVENT_CONFIGURED: 
            USBCBInitEP();
            break;
        case EVENT_SET_DESCRIPTOR:
            USBCBStdSetDscHandler();
            break;
        case EVENT_EP0_REQUEST:
            USBCBCheckOtherReq();
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
        case EVENT_BUS_ERROR:
            USBCBErrorHandler();
            break;
        case EVENT_TRANSFER:
            Nop();
            break;
        default:
            break;
    }      
    return TRUE; 
}

/** EOF mouse.c *************************************************/
#endif

