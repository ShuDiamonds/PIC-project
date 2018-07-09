/** INCLUDES *******************************************************/
#include "usb/usb.h"
#include "usb/usb_function_cdc.h"
#include "GenericTypeDefs.h"
#include "Compiler.h"
#include "usb_config.h"
#include "usb/usb_device.h"

#include "HardwareProfile.h"

/** CONFIGURATION **************************************************/
#if defined(LOW_PIN_COUNT_USB_DEVELOPMENT_KIT)
        //14K50
        #pragma config CPUDIV = NOCLKDIV
        #pragma config USBDIV = OFF
        #pragma config FOSC   = HS
        #pragma config PLLEN  = ON
        #pragma config FCMEN  = OFF
        #pragma config IESO   = OFF
        #pragma config PWRTEN = OFF
        #pragma config BOREN  = OFF
        #pragma config BORV   = 30
//        #pragma config VREGEN = ON
        #pragma config WDTEN  = OFF
        #pragma config WDTPS  = 32768
        #pragma config MCLRE  = OFF
        #pragma config HFOFST = OFF
        #pragma config STVREN = ON
        #pragma config LVP    = OFF
        #pragma config XINST  = OFF
        #pragma config BBSIZ  = OFF
        #pragma config CP0    = OFF
        #pragma config CP1    = OFF
        #pragma config CPB    = OFF
        #pragma config WRT0   = OFF
        #pragma config WRT1   = OFF
        #pragma config WRTB   = OFF
        #pragma config WRTC   = OFF
        #pragma config EBTR0  = OFF
        #pragma config EBTR1  = OFF
        #pragma config EBTRB  = OFF       

#else
    #error No hardware board defined, see "HardwareProfile.h" and __FILE__
#endif

/** V A R I A B L E S *****************************************************/
#pragma udata
char USB_In_Buffer[64];
char USB_Out_Buffer[64];

BOOL stringPrinted;
volatile BOOL buttonPressed;
volatile BYTE buttonCount;

/** P R I V A T E  P R O T O T Y P E S ************************************/
static void InitializeSystem(void);
void ProcessIO(void);
void USBDeviceTasks(void);
void YourHighPriorityISRCode();
void YourLowPriorityISRCode();
void USBCBSendResume(void);
void BlinkUSBStatus(void);
void UserInit(void);

/** VECTOR REMAPPING ***********************************************/

    //PIC18���u�ł́A0x00�A0x08��0x18���A���Z�b�g�A���D��x�����ƒ�D��x
    //�����̃x�N�g���Ɏg���܂��B
    //�������A����Microchip USB�u�[�g���[�_�[�́A0x00-0xFFF�܂���
    //0x00-0x7FF �Ԓn���g�p���Ă��邽�߃u�[�g���[�_�[�E�R�[�h�́A������
    //�x�N�g�����ȉ��̐V�����ꏊ�ɍĔz�u�i���}�b�v�j���܂��B
    //�Ĕz�u�́A���̃v���W�F�N�g��USB�u�[�g���[�_�[�𗘗p����HEX�t�@�C����
    //�v���O��������ꍇ�ɕK�v�ł��B
    //�����A�u�[�g���[�_�[�𗘗p���Ȃ��Ȃ��usb_config.h�t�@�C����ҏW��
    //�ȉ��̒�`���R�����g�A�E�g���Ă��������B
    //#define PROGRAMMABLE_WITH_USB_HID_BOOTLOADER
    //#define PROGRAMMABLE_WITH_USB_LEGACY_CUSTOM_CLASS_BOOTLOADER
    
        #define REMAPPED_RESET_VECTOR_ADDRESS            0x00
        #define REMAPPED_HIGH_INTERRUPT_VECTOR_ADDRESS   0x08
        #define REMAPPED_LOW_INTERRUPT_VECTOR_ADDRESS    0x18
        
    #pragma code REMAPPED_HIGH_INTERRUPT_VECTOR = REMAPPED_HIGH_INTERRUPT_VECTOR_ADDRESS
    void Remapped_High_ISR (void)
    {
         _asm goto YourHighPriorityISRCode _endasm
    }
    #pragma code REMAPPED_LOW_INTERRUPT_VECTOR = REMAPPED_LOW_INTERRUPT_VECTOR_ADDRESS
    void Remapped_Low_ISR (void)
    {
         _asm goto YourLowPriorityISRCode _endasm
    }
    
    #pragma code
        
    // ��������@���[�U�[�̎��ۂɎ��s�����@�C���^���v�g�@�v���O����
    #pragma interrupt YourHighPriorityISRCode
    void YourHighPriorityISRCode()
    {
        // ������ �C���^���v�g�������������C���^���v�g �t���O���`�F�b�N�� 
        // �C���^���v�g�������[�e�B�����L�q��
        // �C���^���v�g �t���O���N���A����
        // �Ȃ�
        #if defined(USB_INTERRUPT)
            USBDeviceTasks();
        #endif
    
    }    //This return will be a "retfie fast", since this is in a #pragma interrupt section 
    #pragma interruptlow YourLowPriorityISRCode
    void YourLowPriorityISRCode()
    {
        // ������ �C���^���v�g�������������C���^���v�g �t���O���`�F�b�N�� 
        // �C���^���v�g�������[�e�B�����L�q��
        // �C���^���v�g �t���O���N���A����
        // �Ȃ�
    
    }    //���̕��A��"retfie"����, #pragma interruptlow section �Ȃ̂�

/** DECLARATIONS ***************************************************/
#pragma code

/************************************************************************
 * Function:        void main(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        Main program entry point.
 * Note:            None
 ************************************************************************/

void main(void){   
    InitializeSystem();

    while(1){
        #if defined(USB_INTERRUPT)
            if(USB_BUS_SENSE && (USBGetDeviceState() == DETACHED_STATE)){
                USBDeviceAttach();
            }
        #endif

        #if defined(USB_POLLING)
        // Check bus status and service USB interrupts.
        USBDeviceTasks(); // Interrupt �� polling���@�B 
              // polling���@���̗p����Ƃ��͕K������I��
              // ���̊֐����Ăяo�����ƁB���̊֐���SETUP transactions
              // �i�Ⴆ�΁A�ŏ��Ƀv���O�C����������enumeration process�j
              // �ł̏����ƕԓ����s���܂��B
              // �t�r�a�z�X�g�́A�t�r�a�f�o�C�X���v���Z�XSETUP�p�P�b�g��
              // �x�؂Ȃ���M���邱�Ƃ�v�����Ă��܂��B���������āA
              // polling���g���ꍇ�A�z�X�g����t�r�a�f�o�C�X�Ɍ���SETUP
              // �p�P�b�g�̑��M���\�z����鎞�ԑт͂��̊֐���p�Ɂi100��
              // �b���j�ɌĂяo���K�v������܂��B �قƂ�ǂ̏ꍇ�A
              // USBDeviceTasks�i�j�֐��͏����Ɏ��Ԃ�K�v�Ƃ���킯�ł�
              // ����܂���i50���߃T�C�N���j�B
              // Interrupt������polling���@����usb_config.h�Œ�`����܂��B
        #endif
                      
        // �{���̖ړI�Ƃ���A�v���P�[�V�����^�X�N �A�v���P�[�V�����Ɋ֘A
        // ����^�X�N�͂������AProcessIO() �֐����ɋL�ڂ��܂��B

        ProcessIO();        

    }//end while
}//end main

/********************************************************************
 * Function:        static void InitializeSystem(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        �������͂���InitializeSystem �ōs����
 *                  �K�v�Ƃ��邷�ׂĂ� USB initialization routines
 *                  �͂�������Ăяo�����B
 *
 *                  ���[�U�[�̎g�p���鏉�������[�e�B����
 *                  ��������Ăяo�����ƁB      
 *
 * Note:            None
 *******************************************************************/
static void InitializeSystem(void){

        ADCON1 |= 0x0F;                 // Default all pins to digital

// USB�d�l�ɂ��t�r�a���ӑ��u��Vbus�s���ɓd�����������Ă͂Ȃ�Ȃ����Ƃ�
// �`���Â����Ă��܂��B ����ɁA���Ӌ@��̓z�X�g/�n�u��Vbus���ɓd����
// �������Ă��Ȃ��Ƃ���D+�܂���D-�s���ɓd�����������Ă͂Ȃ�܂���B
// �i�o�X�d���𗘗p���Ȃ��j���ȓd����L����t�r�a���Ӌ@���݌v����ꍇ�A
// ���u�̃t�@�[���E�F�A�ŁAVbus�ɓd�͂���������Ȃ�����A�t�r�a���W���[��
// ��D+�����D-�s���̃v���A�b�v��R��ON�ɂ��Ȃ��悤�v���O�������܂��B 
// ����������Vbus���z�X�g����쓮����Ă��邩�����o������@���A�t�@�[��
// �E�F�A����������K�v������܂��B
// Vbus���i��R�ŕ�������Ȃǂ��j5V���͂ɑΉ��ł���I/O�s���ɐڑ����邱��
// �ɂ��AVbus�ɓd�����������Ă���i�z�X�g���d�͂��������Ă���j���A
// �������Ă��Ȃ��i�z�X�g�̓V���b�g�_�E�����A�d�͂��������Ă��Ȃ��j��
// �����o���܂��B
// USB�t�@�[���E�F�A�Œ���I�ɂ���I/O�s�����m�F�iPolling)���邱�ƂŁAUSB 
// module/D+/D-��L���ɂ��Ă��ǂ����m�邱�Ƃ��ł��܂��B�o�X�d�������œ���
// ����݌v�̎��ӑ��u�̏ꍇ�z�X�g���AVbus�ɓd�����������Ă��Ȃ��Ƃ���D+
// �܂���D-�ɓd�����������邱�Ƃ͕s�\�Ȃ��߁A���̃o�X�Ď��@�\�͕K�v�ł�
// ����܂���B
// ���̃t�@�[���E�F�A�́A    HardwareProfile.h�t�@�C���̒��ŁuUSE_USB_BUS_
// SENSE_IO�v���`���邱�ƂŃo�X�Ď��@�\���g�ݍ��܂�܂��B
    #if defined(USE_USB_BUS_SENSE_IO)
    tris_usb_bus_sense = INPUT_PIN; // See HardwareProfile.h
    #endif
    
// �z�X�gPC��GetStatus(device)�v���𑗐M�����ꍇ�A�t�@�[���E�F�A�͉������A
// �z�X�g�ɂt�r�a���ӑ��u�����݁A�o�X�d���œ��삵�Ă��邩�A���ȓd����
// ���삵�Ă��邩��m�点�Ȃ���΂Ȃ�܂���B�ڍׂ͌���USB�d�l���ő�9��
// ���m�F���Ă��������B
// ���ӑ��u���ǂ���̕��@�ł����삷��ꍇ�A���݂́A�ǂ̓d���œ��삵�Ă�
// �邩�𒲂ׂ��̌��ʂɂ��ʒm����K�v������܂��B 
// PICDEM FS USB Demo Board�Ɠ��l�ȃn�[�h�E�F�A�\���Ȃ�΁A�uRA2�v�s����
// ���݂̓d���������������Ă��܂��B���̋@�\���g�p���邽�߂ɂ́A
// HardwareProfile.h�ɁuUSE_SELF_POWER_SENSE_IO�v����`����AI/O�s�����K��
// �ɋL�q����Ă��邱�Ƃ��K�v�ł��B

    #if defined(USE_SELF_POWER_SENSE_IO)
    tris_self_power = INPUT_PIN;    // HardwareProfile.h �Q��
    #endif
    
    UserInit();

    USBDeviceInit();    //usb_device.c. ��  USB module SFRs ��@�v���O����
                        //�ϐ�������������
}//end InitializeSystem

/*************************************************************************
 * Function:        void UserInit(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        �����ɁAdemo�v���O�����Ŏg�p����f�o�C�X�̏�����
 *                  �R�[�h���L�ڂ��܂�
 * Note:            
 ************************************************************************/
void UserInit(void){
    //�`���^�����O�ϐ��̏�����
    buttonCount = 0;
    buttonPressed = FALSE;
    stringPrinted = TRUE;

    //LED pin�̏�����
    mInitAllLEDs();

    //pushbuttons�̏�����
    mInitAllSwitches();
}//end UserInit

/********************************************************************
 * Function:        void ProcessIO(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        ���̊֐��ɁA���[�U�[�E���[�`�����L�q���܂��B 
 *                  �����ɂ�USB�֘A����сAUSB�Ɋ֘A���Ȃ������̃^�X�N��
 *                  �L�ڂ���Ă��܂��B
 * Note:            None
 *******************************************************************/
void ProcessIO(void){   
    BYTE numBytesRead;

    // USB device status�𔽉f����LED��_�ł����܂�
    BlinkUSBStatus();
    // User Application USB tasks
    if((USBDeviceState < CONFIGURED_STATE)||(USBSuspendControl==1)) return;
    // �������烆�[�U�[�E�A�v�����L�ڂ��܂�
    // PushSw�������ꂽ���̏���

    if(buttonPressed){
        if(stringPrinted == FALSE){
            if(mUSBUSARTIsTxTrfReady()){
                putrsUSBUSART("Button Pressed -- \r\n");
                stringPrinted = TRUE;
            }
        }
    }else{
        stringPrinted = FALSE;
    }

    // �����R�[�h����M�����Ƃ��̏���
    if(USBUSARTIsTxTrfReady()){
        numBytesRead = getsUSBUSART(USB_Out_Buffer,64);
        if(numBytesRead != 0){
            BYTE i;
            
            for(i=0;i<numBytesRead;i++){
                switch(USB_Out_Buffer[i]){
                    case 0x0A:
                    case 0x0D:
                        USB_In_Buffer[i] = USB_Out_Buffer[i];
                        break;
                    default:
                        USB_In_Buffer[i] = USB_Out_Buffer[i] + 1;
                        break;
                }
            }
            putUSBUSART(USB_In_Buffer,numBytesRead);
        }
    }

    CDCTxService();
}        //end ProcessIO

/********************************************************************
 * Function:        void BlinkUSBStatus(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        USB device status�𔽉f����LED��_�ł����܂� 
 *
 * Note:            mLED macros �� HardwareProfile.h�ɋL�ڂ���Ă��܂�
 *                  USBDeviceState �́Ausb_device.h�Œ�`����
 *                  ���̏�Ԃ�UpDate���s���܂�
 *******************************************************************/
void BlinkUSBStatus(void){
    static WORD led_count=0;
    
    if(led_count == 0)led_count = 10000U;
    led_count--;

    #define mLED_Both_Off()     {mLED_1_Off();mLED_2_Off();}
    #define mLED_Both_On()      {mLED_1_On();mLED_2_On();}
    #define mLED_Only_1_On()    {mLED_1_On();mLED_2_Off();}
    #define mLED_Only_2_On()    {mLED_1_Off();mLED_2_On();}

    if(USBSuspendControl == 1){
        if(led_count==0){
            mLED_1_Toggle();
            if(mGetLED_1()){
                mLED_2_On();
            }else{
                mLED_2_Off();
            }
        }//end if
    }else{
        if(USBDeviceState == DETACHED_STATE){
            mLED_Both_Off();
        }else if(USBDeviceState == ATTACHED_STATE){
            mLED_Both_On();
        }else if(USBDeviceState == POWERED_STATE){
            mLED_Only_1_On();
        }else if(USBDeviceState == DEFAULT_STATE){
            mLED_Only_2_On();
        }else if(USBDeviceState == ADDRESS_STATE){
            if(led_count == 0){
                mLED_1_Toggle();
                mLED_2_Off();
            }//end if
        }else if(USBDeviceState == CONFIGURED_STATE){
            if(led_count==0){
                mLED_1_Toggle();
                if(mGetLED_1()){
                    mLED_2_Off();
                }else{
                    mLED_2_On();
                }
            }//end if
        }//end if(...)
    }//end if(UCONbits.SUSPND...)
}//end BlinkUSBStatus

// ******************************************************************
// ************** USB Callback Functions ****************************
// ******************************************************************
// USB�t�@�[���E�F�A�E�X�^�b�N�́A�����USB�C�x���g�ɉ����ăR�[���o�b�N
// �֐�USBCBxxx�i�j���Ăяo���܂��B
// ���Ƃ��΁A�z�X�gPC���d����OFF�ɂ���Ƃ��́AUSB���u�ւ�Start of Frame
//  �iSOF�j�̑��M���~�߂܂��B ����ɉ����āA���ׂĂ̂t�r�a���u�́AUSB 
// Vbus����̏���d�������ꂼ��2.5mA �����Ɍ��������܂��B
// �t�r�a���W���[���́A���̏�ԁiUSB�d�l�ɂ��΁A3ms�ȏ�̉��̒ʐM/SOF
// �p�P�b�g���s���Ă��Ȃ���ԁj�����o����ƁAUSBCBSuspend�i�j���Ăт�
// ���܂��B���[�U�[�́A�����̏󋵂ɓK�؂ȑ[�u���Ƃ���悤�R�[���o�b�N
// �֐����C�����Ȃ���΂Ȃ�܂���B
// ���Ƃ��΁AUSBCBSuspend�i�j�ł́A�i�N���b�N��ύX������ALED���I�t�ɂ�
// ����ACPU���X���[�v�������肵�āj����d��������������R�[�h�����s��Vbus
// ����̏���d����2.5mA�����ɂ��܂��B
// ���̌�AUSBCBWakeFromSuspend�i�j�֐��ł́AUSBCBSuspend�i�j�֐��Ŏ��{
// �����ȓd�͂����ɖ߂��R�[�h�����s���邱�ƂɂȂ�܂��B
// USB�X�^�b�N������������I�ɌĂ΂Ȃ��Ƃ����_�ŁAUSBCBSendResume�i�j�֐�
// �͓��ʂł��B ���̊֐��́A�A�v���P�[�V�����E�t�@�[���E�F�A����Ă΂��
// �֐��ł��B�ڂ����͕ʂ̃R�����g���Q�Ƃ��Ă��������B
/**************************************************************************
 * Function:        void USBCBSuspend(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        Call back that is invoked when a USB suspend is detected
 *
 * Note:            None
 **************************************************************************/
void USBCBSuspend(void){
// Example power saving code 
// �A�v���P�[�V�����ɖ]�܂����K�؂ȏȓd�̓R�[�h�������ɋL�ڂ���B
// ���� �}�C�N���R���g���[�����X���[�v������Ȃ�A���L�Ɏ����菇��
// �ގ������v���Z�X���g�p�����F
/*
	ConfigureIOPinsForLowPower();
	SaveStateOfAllInterruptEnableBits();
	DisableAllInterruptEnableBits();
	EnableOnlyTheInterruptsWhichWillBeUsedToWakeTheMicro(); 
//    �ċN���iwake�j�̂��ߏ��Ȃ��Ƃ�USBActivityIF�͉ғ�������
 Sleep();
	RestoreStateOfAllPreviouslySavedInterruptEnableBits(); 
//    �o����΁AUSBCBWakeFromSuspend�i�j�֐����ɂ��̑�֋@�\�𐷍��ށB
	RestoreIOPinsToNormal(); 
//    �o����΁AUSBCBWakeFromSuspend�i�j�֐����ɂ��̑�֋@�\�𐷍��ށB
// 
// �d�v�����F ������USBActivityIF�iACTVIF�j�r�b�g���N���A���Ă͂Ȃ�Ȃ��B 
// ���̃r�b�g��usb_device.c�t�@�C���ŃN���A�����B������USBActivityIF��
// �N���A����ƈӐ}���铮����s���Ȃ��Ȃ�B    
// �� �ȓd�͉��R�[�h�B �K�v�ɂ��K�؂ȃR�[�h�������ɑ}��
    */
}

/****************************************************************************
 * Function:        void USBCBWakeFromSuspend(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        �z�X�g�́A�i3+ms�̃A�C�h�����u����v���ƂŁj�t�r�a���ӑ��u
 *                    ��d��Suspend���[�h�ɂ��邱�Ƃ�����B Suspend���[�h�̎��ɁA
 *                    ���z�X�g�́A�A�C�h���ȊO�̐M���𑗂邱�ŁA���u���ĂыN��
 *                    �iWake)������B
 *                    ���̃R�[���o�b�N�́AUSB suspend ����̋N���iWakeup)�����o
 *                    ���ꂽ�Ƃ��Ɏ��{�����B
 * Note:            None
 ************************************************************************/
void USBCBWakeFromSuspend(void){
 // �N���b�N�̕ύX�⑼�̏ȓd�͏��u���Ƃ�AUSBCBSuspend�i�j�����s���āA
 // ���̌�A�ʏ�̃t���p���[���샂�[�h�ւ̈ڍs���鎞�@�z�X�g��
 // 2 - 3�~���b�̃X���[�v�������Ԃ�݂��Ă��܂��B���̊Ԃɑ��u�͒ʏ�
 // ����ɖ߂�A�����USB�p�P�b�g���󂯂āA�����ł���悤�ɂȂ�K�v
 // ������܂��B�������邽�߂ɁA�t�r�a���W���[���́A�K�؂ȃN���b�N
 // �ɖ߂�K�v������܂��iIE�F�t���X�s�[�hUSB��SIE�𗘗p����ɂ�
 // 48MHz�̃N���b�N���K�v�ł��j�B
}

/********************************************************************
 * Function:        void USBCB_SOF_Handler(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        �t�r�a�z�X�g�́Afull-speed���u��1ms������SOFpacket��
 *                  ���o���܂��B ���̊��荞�݂͒���I�ȃA�C�\�N���i�X�]��
 *                  ����� ���p����ꍇ������܂��B
 *                  �K�v�ɉ����ăR�[���o�b�N�E���[�`���Ɏ������Ă��������B
 *                  ���̗�ł́A����I�Ɏ��s����邱�Ƃ𗘗p���APushSW��
 *                  �`���^�����O�h�~�@�\���������Ă��܂��B
 * Note:            None
 *******************************************************************/
void USBCB_SOF_Handler(void){
    // UIRbits.SOFIF �� 0 �ɃN���A����K�v�͂���܂���
    // Callback caller �����ɂ��̏��������{���Ă��܂��B

    //------------------- SW�̃`���^�����O���� ---------------------------
    //SW�̃`���^�����O���������ASW�̏�Ԃ�buttonPressed(bP�Ə����܂�)��
    //�\���܂��BSW���������Ƃ�buttonPressed(bP)�́u�P�v�ɂȂ�܂��B 
    //SW�̏�Ԃ��ψʂ�����A100�J�E���g�̓ǎ��֎~���Ԃ��݂����Ă��܂��B 
    if(buttonPressed == sw2){  // (bP)�Ǝ��ۂ�SW�̏�Ԃ��قȂ��Ă���
        if(buttonCount != 0){  //    �`���^�����O�̉\����������Ԃ��H
            buttonCount--;     //       �ǎ��֎~���Ԃ�i�߂�
        }else{                 //    �ǎ��֎~���Ԃ͏I��
            // SW��Ԃ��i���_���̂��߁j���]���ēǎ��
            buttonPressed = !sw2;    
            buttonCount = 100; //       �ǎ��֎~���Ԃ�������
        }
    }else{                     // (bP)�Ǝ��ۂ�SW�̏�Ԃ���v���Ă���B
        if(buttonCount != 0){  //    �`���^�����O�̉\����������ԂȂ�
            buttonCount--;     //       �ǎ��֎~���Ԃ�i�߂�B
        }
    }  // ----- SW�̃`���^�����O���� �I�� -----
}

/*******************************************************************
 * Function:        void USBCBErrorHandler(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:    ���̃R�[���o�b�N�̖ړI�́A��ɊJ�����̃f�o�b�O�ł��B
 *              UEIR���`�F�b�N���ǂ̃G���[���A���荞�݂������N��������
 *              �������邱�Ƃ��ł��܂� 
 *
 * Note:            None
 *******************************************************************/
void USBCBErrorHandler(void){
  // ������UEIR��0�ɃN���A����K�v�͂���܂���B���łɃR�[���o�b�N����
  // �N���A���Ă��܂��B
  // ��ʓI�ɁA���[�U�[�E�t�@�[���E�F�A�ł́A���ɉ�����������K�v�͂���
  // �܂���BUSB�G���[�������������B���Ƃ��΁A �z�X�g��OUT�p�P�b�g�𑗐M
  // �������A���u�ւ��̃p�P�b�g���͂��Ȃ������Ƃ��i��F�ڐG�s�ǂ�A�����
  // �t�r�a�P�[�u���̃v���O�𔲂����Ȃǁj�A����ɂ��A�ʏ��ȏ��USB
  // �G���[���荞�݂��������܂��B�������ASIE�������I�ɁuNAK�v�p�P�b�g��
  // �z�X�g�ɑ��M���Ă������A����ɉ�������K�v�͂���܂���B
  // �ʏ�z�X�g�̓p�P�b�g���đ����邽�߂Ƀf�[�^�����͋N����܂���B
  // �V�X�e���́A��ʓI�ɃA�v���P�[�V�����E�t�@�[���E�F�A�̏�����K�v��
  // �����A�����I�ɕ������܂��B�܂�A���̃R�[���o�b�N�@�\�̓f�o�b�O��
  // �ړI�̂��߂ɒ񋟂���Ă��܂��B 
}

/*******************************************************************
 * Function:        void USBCBCheckOtherReq(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        SETUP�p�P�b�g���z�X�g���瓞�������Ƃ��A
 *                  �t�@�[���E�F�A�ɂ���ẮA�v���𖞂������߂�
 *                  �K�؂ɁA�f�[�^�𑗐M������A�������Ȃ���΂Ȃ�܂���B
 *                  SETUP�p�P�b�g�̈ꕔ�́AstandardUSB "chapter 9"
 *                  �i����USB�d�l���̑�9�͂𖞂������߁j�ɏ]���܂����A
 *                  USB�f�o�C�X�N���X���L�̂��̂�����܂��B ���Ƃ��΁AHID
 *                  ���u�́uGET REPORT�v�ɉ�������K�v������܂��B
 *                   ����͎d�l���̑�9�̗͂v���łȂ����߁Ausb_device.c�ł�
 *                  ��舵���܂���B���̑���ɁA����̃N���X����舵��
 *                  �t�@�[���E�F�A���Ƃ��� usb_function_hid.c.�Ɋ܂܂�܂�
 *
 * Note:            None
 *******************************************************************/
void USBCBCheckOtherReq(void){
    USBCheckCDCRequest();
}//end

/*******************************************************************
 * Function:        void USBCBStdSetDscHandler(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        USBCBStdSetDscHandler�i�j�R�[���o�b�N�@�\�́ASETUP,
 *                    bRequest�FSET_DESCRIPTOR�v���̎�M���ɌĂ΂�܂��B
 *                    ��ʓI�ɁASET_DESCRIPTOR�v���͑命���̃A�v���P�[
 *                    �V�����Ŏg���܂���B
 *                    ���̎�̗v�����T�|�[�g���邱�Ƃ̓I�v�V�����ł��B
 *
 * Note:            None
 *******************************************************************/
void USBCBStdSetDscHandler(void){
    // Must claim session ownership if supporting this request
}//end

/*******************************************************************
 * Function:        void USBCBInitEP(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        ���u�������������Ƃ��A���̋@�\���Ă΂�܂�
 *                  �z�X�g��SET_CONFIGURATION(wValue not = 0)�𑗂������Ƃ�
 *                  ��������v���ł��B���̃R�[���o�b�N�@�\�́A���u�̎��{
 *                  ���鏈���ɕK�v�ƂȂ�G���h�|�C���g�̏��������s���܂�
 *
 * Note:            None
 *******************************************************************/
void USBCBInitEP(void){
    CDCInitEP();
}

/********************************************************************
 * Function:        void USBCBSendResume(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        TUSB�d�l�́A������USB���ӑ��u�Ƀz�X�gPC��wake up��
 *        ���邱�Ƃ������܂��i���Ƃ���low power suspend����RAM�Ȃǁj�B
 *        ����́A�������̂t�r�a�A�v���P�[�V�����Ŕ��ɖ��ɗ��@�\�ł�
 *        �i�Ⴆ�ΐԊO�������R���j�B ���[�U�[���ԊO�������R���́u�d���v�{�^
 *        �������������A�ԊO�����V�[�o�[�́A���̐M������M���APC��USB
 *         "command" �𑗐M����PC�� wake up�o����΂���͑f���炵�����Ƃł��B
 *                    
 *        USBCBSendResume�i�j�R�[���o�b�N�@�\���A���̓��ʂ�USB�M���𑗐M��
 *        PC��wakes up����̂Ɏg���܂��B���̋@�\�́A�A�v���P�[�V�����E
 *        �t�@�[���E�F�A����PC��wakes up����̂ɌĂяo�����Ƃ��ł��܂��B 
 *        �������A���̋@�\�́A���L�̑S�Ă𖞂����K�v������܂��F
 *                    
 *                    1.  PC�g���Ă���USB�h���C�o�[���Aremote wakeup�@�\
 *                        ���T�|�[�g����
 *                    2.  USB configuration descriptor��bmAttributes
 *                        �t�B�[���h��remote wakeup capable�ƋL�q���Ă���B
 *                    3.  �z�X�gPC�͌��݃X���[�v���Ă��āA�ȑO����USB���u
 *                        ����remote wakeup�@�\��"armed"�ɂ���SET FEATURE 
 *                        �̃Z�b�g �A�b�v�p�P�b�g�𑗐M���Ă���
 *
 *                  �z�X�g��remote wakeup�� armed ��Ԃɂ��Ă��Ȃ��ꍇ�A
 *                  ���̋@�\��remote wakeup���N���������^�[�����܂��B
 *                  ����́A��߂�ꂽ����ł��A remote wakeup�� armed 
 *                  ��Ԃɂ��Ȃ������t�r�a���u��remote wakeup�M�����o�X��
 *                  ��ɑ��M���Ă͂����܂���;�������M����ƁA
 *                  USB compliance testing failure�𔭐������܂��B
 *
 *                  ���̃R�[���o�b�N�́A1-15ms�̊ԁARESUME�M���𑗂�Ȃ�
 *                  ��΂Ȃ�܂���B
 *
 * Note:            USB�o�X�ƃz�X�g��suspended��ԁA����������΁Aremote 
 *                  wakeup ready��ԂɂȂ��Ȃ�΁A���̋@�\�͉������Ȃ��ŁA
 *                  �����ɖ߂�܂��B
*                   ���������āA����I�ɂ��̋@�\���Ă�ł����S�ł���A
 *                  �A�v���P�[�V�������C�ӂ̃^�C�~���O�ŌĂяo���Ă��A
 *                  �o�X���{����remote wakeup���󂯓�������ԂłȂ�
 *                  ����e����^���܂���B
 *
  *                  ���̋@�\�����s�ɍ��킹�AUSBCBWakeFromSuspend�i�j�̒�
 *                  �ŃN���b�N�ύX�����Ă����K�v�����邩������܂���B
 *                  �Ȃ��Ȃ�USB�o�X�͂��̋@�\����A���^�[������Ƃ����ɁA
 *                  suspended�ł͂Ȃ����߁AUSB���W���[���̓z�X�g�����
 *                  �M������M���鏀�����ł��Ă���ł��邱�Ƃ��K�v�ł��B
 *
 *                  ���̃��[�`���̏C���\�Z�N�V�����́A�A�v���P�[�V�����E
 *                  �j�[�Y�𖞂������߂ɕύX����K�v������܂��B 
 *                  ���݂̎b��ݒ�́A��\�I�ȃN���b�N���g���ő��̋@�\��
 *                  ���s��3-15 ms�̊��ԃu���b�N���܂��B
 *
 *                  USB 2.0�d�l����section7.1.7.7�ɂ��ƁA�uremote wakeup 
 *                  ���u�́A���Ȃ��Ƃ�1 ms�ȏ�A15 ms�ȉ��̊�resume�M����
 *                  �ێ����Ȃ���΂Ȃ�܂���v ���̓���́Adelay counter 
 *                  loop���g�p���邱�ƂŎ������Ă��܂��B�ȉ��̈�ʂȒl��
 *                  �g�p���邱�ƂōL�͈͂ɂ킽��R�A���g���œ���\�ł��B

 *                  That value selected is 1800. See table below:
 *                  =======================================================
 *                  Core Freq(MHz)      MIP      RESUME Signal Period (ms)
 *                  =======================================================
 *                      48              12          1.05
 *                       4              1           12.6
 *                  =======================================================
 *                  * �����̃^�C�~���O�̓R�[�h�œK���܂��͊g�����߃��[�h
 *                    �̎g�p�⑼�̊��荞�݂��\�ɂ��Ă����Ƃ���ƁA�قȂ�
 *                    �ꍇ������܂��B �K��MPLAB SIM��Stopwatch�@�\��I�V
 *                    ���X�R�[�v�Ŏ��ۂ̐M�����m���߂�悤�ɂ��Ă��������B 
*******************************************************************/
void USBCBSendResume(void){
    static WORD delay_count;
    
    // ��1�ɂ́A�z�X�g��remote wakeup�����s���邽�߂�armed����Ă��邱�Ƃ�
    // �m���߂Ă��������Bremote wakeup���\�ɂ���ɂ́A�ʏ�z�X�g��
    // standby mode�ɂȂ钼�O��SET_FEATURE�𑗐M���邱�Ƃɂ���čs���܂��A
    // �����F configuration descriptor��remote wakeup capable�ƋL�q����
    // ����A�z�X�g�̋@�\���C�l�[�u������Ă���΁ASET_FEATURE�v���𑗐M
    // ���邾���ł��B
    // Windows�x�[�X�̃z�X�g�̋@�\�C�l�[�u���́A�f�o�C�X�}�l�[�W����
    // �t�r�a���u�v���p�e�B�\����p���[�}�l�W�����g�E�^�u��I��
    // "Allow this device to bring the computer out of standby."  
    // �`�F�b�N�{�b�N�X���`�F�b�N����Ă��邱��

    if(USBGetRemoteWakeupStatus() == TRUE) {
        //Verify that the USB bus is in fact suspended, before we send
        //remote wakeup signalling.
        if(USBIsBusSuspended() == TRUE){
            USBMaskInterrupts();
            
            //Clock switch to settings consistent with normal USB operation.
            USBCBWakeFromSuspend();
            USBSuspendControl = 0; 
            USBBusIsSuspended = FALSE;  //So we don't execute this code again, 
                                        //until a new suspend condition is detected.

            // USB 2.0�d�l�̑�7.1.7.7�߂́AUSB���u�́Aremote wakeup��
            // ���M����O�ɁA�o�X�ŃA�C�h����A���I��5ms+�Ď����Ȃ���
            // �΂Ȃ�܂���B���̗v�������������̕��@�Ƃ��āA2ms+��
            // blocking delay��ǉ����Ă��܂��B
            //  USBIsBusSuspended() == TRUE�Ŋm�F����Ă��鏭�Ȃ��Ƃ�3ms
            // �̃o�X�A�C�h����2ms��ǉ����邱�ƂŃA�C�h���̃X�^�[�g����
            // ���v5ms+�ƂȂ�܂��B

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
            USBResumeControl = 0;     //Finished driving resume signalling

            USBUnmaskInterrupts();
        }
    }
}

/*******************************************************************
 * Function:        void USBCBEP0DataReceived(void)
 * PreCondition:    ENABLE_EP0_DATA_RECEIVED_CALLBACK must be
 *                  defined already (in usb_config.h)
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        EP0�f�[�^�p�P�b�g����M���ꂽ���͂��ł��A���̋@�\��
 *                  �Ă΂�܂�����́A���낢��ȃN���X�ŗ��p�����control
 *                  endpoint���g�p�����f�[�^�擾���@��Ꭶ���܂��B
 *                  ���̋@�\�́AUSBCBCheckOtherReq�i�j�ƈꏏ�Ɏg����K�v
 *                  ������܂��BUSBCBCheckOtherReq�i�j�@�\�́A�f�[�^������
 *                  ����O�ɍŏ���control ����M����A�v���ł��B

 *
 * Note:            None
 *******************************************************************/
#if defined(ENABLE_EP0_DATA_RECEIVED_CALLBACK)
void USBCBEP0DataReceived(void){
}
#endif

/*******************************************************************
 * Function:        BOOL USER_USB_CALLBACK_EVENT_HANDLER(
 *                        USB_EVENT event, void *pdata, WORD size)
 * PreCondition:    None
 * Input:           USB_EVENT event - the type of event
 *                  void *pdata - pointer to the event data
 *                  WORD size - size of the event data
 * Output:          None
 * Side Effects:    None
 * Overview:        ���̋@�\�́AUSB�C�x���g�������������Ƃ����[�U�A�v���P
 *                  �[�V�����ɒʒm���邽�߂ɁAUSB�X�^�b�N����Ă΂�܂��B
 *                   USB_INTERRUPT�I�v�V�������I������Ă���Ƃ��A����
 *                  �R�[���o�b�N�͊����ݏ����ɂ�����܂��B
 *
 * Note:            None
 *******************************************************************/
BOOL USER_USB_CALLBACK_EVENT_HANDLER(USB_EVENT event, void *pdata, WORD size){
    switch(event){
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
            //�K�v�ɉ����ĕK�v�ȃA�v���P�[�V�����p�R�[���o�b�N�^�X�N�܂���
            //�R�[���o�b�N�@�\�������ɉ����Ă��������B
            //EVENT_TRANSFER_TERMINATED�́Aarmed �iUOWN= 1�j����Ă���A�v
            //���P�[�V�����E�G���h�|�C���g�ɁA�z�X�g��CLEARFEATURE
            //(endpoint halt)�v�����s�����Ƃ��ɔ������܂��B
            //�ȉ��̋@�\�́A�����ɒu���Ɨǂ��ł��傤�F
            //1.  *pdata�̃n���h���l���`�F�b�N���邱�Ƃɂ���āA�ǂ̃G���h
            //    �|�C���g�ŁA�ʐM���I�������̂��������B
            //2.  �K�v�ɉ����ăG���h�|�C���g��Re-arm����
            //     �i��ʓI�ɂ�OUT�G���h�|�C���g�̏ꍇ�j
            break;
        default:
            break;
    }      
    return TRUE; 
}

/** EOF main.c *************************************************/

