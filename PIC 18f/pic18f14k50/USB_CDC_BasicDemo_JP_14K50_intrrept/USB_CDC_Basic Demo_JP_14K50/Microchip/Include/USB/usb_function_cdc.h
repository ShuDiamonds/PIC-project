/************************************************************************
  File Information:
    FileName:       usb_function_cdc.h
    Dependencies:   See INCLUDES section
    Processor:      PIC18,PIC24, PIC32 and dsPIC33 USB Microcontrollers

    Complier:      Microchip C18 (for PIC18),C30 (for PIC24 and dsPIC33)
                    and C32 (for PIC32)
    Company:        Microchip Technology, Inc.

 ���쌠�F�\�t�g�E�G�A���̂́AMicrochip�Ђ�Software License Agreement
         �ɏ����܂����A�R�����g�Ȃǂ̒ǉ������̒��쌠�́A��҂�
         �ۗL���A��҂̋��𓾂Ȃ��A�S�̂���шꕔ���̓]�ځE���p��
         �֎~���܂��B

  �T�v:
	���̃t�@�C���ɂ́ACDC�@�\�Ŏg�p����S�ẮA�@�\�A�}�N���A��`�A�ϐ���
	��`���Ă��܂��B�㔼�ɁACDC�@�\�Ŏg�p����}�N������������Ă��܂��B
	���̃t�@�C���́ACDC���g���v���W�F�N�g�Ɋ܂܂�Ȃ���΂Ȃ�܂���
	����ɁAusb_descriptors.c�t�@�C���ACDC�@�\���g�p���郆�[�U�[�E�t�@�C��
	�� include ����K�v������܂��B
    
    �ʏ킱�̃t�@�C���� "\<Install Directory\>\Microchip\Include\USB"
    directory�ɕۑ�����Ă��܂��B

  Description:
    USB CDC Function Driver File
    
���̃t�@�C����V�����v���W�F�N�g�Ŏg�p����Ƃ��́A���̃t�@�C����
�V�����v���W�F�N�g�f�B���N�g������Q�Ƃ��邩�A���̃t�@�C�����̂�
���ڐV�����v���W�F�N�g�t�H���_�ɃR�s�[����K�v������܂��B
�͂��邱�Ƃ��ł��܂�
 �ŏ��̕��@�����ꍇ�A
MPLAB�́uProjegt > Build Options... > Project�v��Directories�^�u����
Include Serch Path��I�����A���̃t�@�C���p�X��o�^����K�v������܂��B

���݂̃f���E�t�H���_�ł́A�v���W�F�N�g�t�H���_����Microchip\Include
�Ƃ����t�H���_�����R�s�[���Ă��邽�߁AInclude Serch Path�ɂ�

   Microchip\Include 

�Ɠo�^����Ă��܂��B
    
�Q�Ƃ�����@�ł́A�t�@�C���̈ʒu�ɂ��A�ȉ��̂悤�ȗ���Q�l�ɓo�^
�����Ă��������B

    C:\\Microchip Solutions\\Microchip\\Include
    
    C:\\Microchip Solutions\\My Demo Application                       

*******************************************************************
 Change History:
  Rev    Description
  ----   -----------
  2.3    Decricated the mUSBUSARTIsTxTrfReady() macro.  It is 
         replaced by the USBUSARTIsTxTrfReady() function.

  2.6    Minor definition changes

  2.6a   No Changes
  
  2.9b   Added new CDCNotificationHandler() prototype and related 
         structure definitions useful when sending serial state
         notifications over the CDC interrupt endpoint.

  i_1.0  ���{��ɖ|�� 2012/05/01
********************************************************************/

#ifndef CDC_H
#define CDC_H

/** I N C L U D E S **********************************************************/
#include "GenericTypeDefs.h"
#include "USB/usb.h"
#include "usb_config.h"

/** D E F I N I T I O N S ****************************************************/

/* Class-Specific Requests */
#define SEND_ENCAPSULATED_COMMAND   0x00
#define GET_ENCAPSULATED_RESPONSE   0x01
#define SET_COMM_FEATURE            0x02
#define GET_COMM_FEATURE            0x03
#define CLEAR_COMM_FEATURE          0x04
#define SET_LINE_CODING             0x20
#define GET_LINE_CODING             0x21
#define SET_CONTROL_LINE_STATE      0x22
#define SEND_BREAK                  0x23

/* Notifications *
 * Note: Notifications are polled over
 * Communication Interface (Interrupt Endpoint)
 */
#define NETWORK_CONNECTION          0x00
#define RESPONSE_AVAILABLE          0x01
#define SERIAL_STATE                0x20


/* Device Class Code */
#define CDC_DEVICE                  0x02

/* Communication Interface Class Code */
#define COMM_INTF                   0x02

/* Communication Interface Class SubClass Codes */
#define ABSTRACT_CONTROL_MODEL      0x02

/* Communication Interface Class Control Protocol Codes */
#define V25TER                      0x01    // Common AT commands ("Hayes(TM)")


/* Data Interface Class Codes */
#define DATA_INTF                   0x0A

/* Data Interface Class Protocol Codes */
#define NO_PROTOCOL                 0x00    // No class specific protocol required


/* Communication Feature Selector Codes */
#define ABSTRACT_STATE              0x01
#define COUNTRY_SETTING             0x02

/* Functional Descriptors */
/* Type Values for the bDscType Field */
#define CS_INTERFACE                0x24
#define CS_ENDPOINT                 0x25

/* bDscSubType in Functional Descriptors */
#define DSC_FN_HEADER               0x00
#define DSC_FN_CALL_MGT             0x01
#define DSC_FN_ACM                  0x02    // ACM - Abstract Control Management
#define DSC_FN_DLM                  0x03    // DLM - Direct Line Managment
#define DSC_FN_TELEPHONE_RINGER     0x04
#define DSC_FN_RPT_CAPABILITIES     0x05
#define DSC_FN_UNION                0x06
#define DSC_FN_COUNTRY_SELECTION    0x07
#define DSC_FN_TEL_OP_MODES         0x08
#define DSC_FN_USB_TERMINAL         0x09
/* more.... see Table 25 in USB CDC Specification 1.1 */

/* CDC Bulk IN transfer states */
#define CDC_TX_READY                0
#define CDC_TX_BUSY                 1
#define CDC_TX_BUSY_ZLP             2       // ZLP: Zero Length Packet
#define CDC_TX_COMPLETING           3

#if defined(USB_CDC_SET_LINE_CODING_HANDLER) 
    #define LINE_CODING_TARGET &cdc_notice.SetLineCoding._byte[0]
    #define LINE_CODING_PFUNC &USB_CDC_SET_LINE_CODING_HANDLER
#else
    #define LINE_CODING_TARGET &line_coding._byte[0]
    #define LINE_CODING_PFUNC NULL
#endif

#if defined(USB_CDC_SUPPORT_HARDWARE_FLOW_CONTROL)
    #define CONFIGURE_RTS(a) UART_RTS = a;
#else
    #define CONFIGURE_RTS(a)
#endif

#if defined(USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D3)
    #error This option is not currently supported.
#else
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D3_VAL 0x00
#endif

#if defined(USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D2)
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D2_VAL 0x04
#else
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D2_VAL 0x00
#endif

#if defined(USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D1)
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D1_VAL 0x02
#else
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D1_VAL 0x00
#endif

#if defined(USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D0)
    #error This option is not currently supported.
#else
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D0_VAL 0x00
#endif

#define USB_CDC_ACM_FN_DSC_VAL  \
    USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D3_VAL |\
    USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D2_VAL |\
    USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D1_VAL |\
    USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D0_VAL

/******************************************************************************
    Function:
        void CDCSetBaudRate(DWORD baudRate)
        
    Summary:
        ���� macro �́Aget line coding request �̂Ƃ��ɁAPC��
        baud rate ���񓚂���B (optional)

    Description:

        �g�p��:
        <code>
            CDCSetBaudRate(19200);
        </code>

        ���̋@�\�́Aoptional �ł��B�Ȃ��Ȃ� CDC devices �́AUSB�ʐM��
        �ۂɎ��ۂɂ�UART�̃n�[�h���R���g���[�����Ȃ�����  

    PreCondition:
        None
        
    Parameters:
        DWORD baudRate - ���]���� baudrate
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetBaudRate(baudRate) {line_coding.dwDTERate.Val=baudRate;}

/******************************************************************************
    Function:
        void CDCSetCharacterFormat(BYTE charFormat)
        
    Summary:
        ���� macro �́Aget line coding request �̂Ƃ��ɁAPC��
        character format ���񓚂���B (optional)

    Description:
        �g�p��:
        <code>
            CDCSetCharacterFormat(NUM_STOP_BITS_1);
        </code>
        
        ���̋@�\�́Aoptional �ł��B�Ȃ��Ȃ� CDC devices �́AUSB�ʐM��
        �ۂɎ��ۂɂ�UART�̃n�[�h���R���g���[�����Ȃ�����  

    PreCondition:
        None
        
    Parameters:
        BYTE charFormat - stop bit�̒��� �g�p�\�� option ��:
         * NUM_STOP_BITS_1   - 1 Stop bit
         * NUM_STOP_BITS_1_5 - 1.5 Stop bits
         * NUM_STOP_BITS_2   - 2 Stop bits
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetCharacterFormat(charFormat) {line_coding.bCharFormat=charFormat;}
#define NUM_STOP_BITS_1     0   //1 stop bit - used by CDCSetLineCoding() and CDCSetCharacterFormat()
#define NUM_STOP_BITS_1_5   1   //1.5 stop bit - used by CDCSetLineCoding() and CDCSetCharacterFormat()
#define NUM_STOP_BITS_2     2   //2 stop bit - used by CDCSetLineCoding() and CDCSetCharacterFormat()

/******************************************************************************
    Function:
        void CDCSetParity(BYTE parityType)
        
    Summary:
        ���� macro �́Aget line coding request �̂Ƃ��ɁAPC��
        parity format ���񓚂���B (optional)

    Description:
        �g�p��:
        <code>
            CDCSetParity(PARITY_NONE);
        </code>
        
        ���̋@�\�́Aoptional �ł��B�Ȃ��Ȃ� CDC devices �́AUSB�ʐM��
        �ۂɎ��ۂɂ�UART�̃n�[�h���R���g���[�����Ȃ�����  

    PreCondition:
        None
        
    Parameters:
        BYTE parityType - Type of parity.  �g�p�\�� option ��:
            * PARITY_NONE
            * PARITY_ODD
            * PARITY_EVEN
            * PARITY_MARK
            * PARITY_SPACE
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetParity(parityType) {line_coding.bParityType=parityType;}
#define PARITY_NONE     0 //no parity - used by CDCSetLineCoding() and CDCSetParity()
#define PARITY_ODD      1 //odd parity - used by CDCSetLineCoding() and CDCSetParity()
#define PARITY_EVEN     2 //even parity - used by CDCSetLineCoding() and CDCSetParity()
#define PARITY_MARK     3 //mark parity - used by CDCSetLineCoding() and CDCSetParity()
#define PARITY_SPACE    4 //space parity - used by CDCSetLineCoding() and CDCSetParity()

/******************************************************************************
    Function:
        void CDCSetDataSize(BYTE dataBits)
        
    Summary:
        ���� macro �́Aget line coding request �̂Ƃ��ɁAPC��
        �f�[�^�r�b�g�����񓚂���B (optional)

    Description:
        �g�p��:

        <code>
            CDCSetDataSize(8);
        </code>
        
        ���̋@�\�́Aoptional �ł��B�Ȃ��Ȃ� CDC devices �́AUSB�ʐM��
        �ۂɎ��ۂɂ�UART�̃n�[�h���R���g���[�����Ȃ�����  

    PreCondition:
        None
        
    Parameters:
        BYTE dataBits - number of data bits.  The options are 5, 6, 7, 8, or 16.
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetDataSize(dataBits) {line_coding.bDataBits=dataBits;}

/******************************************************************************
    Function:
        void CDCSetLineCoding(DWORD baud, BYTE format, BYTE parity, BYTE dataSize)
        
    Summary:
        ���� macro �́Aget line coding request �̂Ƃ��ɁAPC��
        �֘A�ݒ�l���񓚂���B (optional)

    Description:
        �g�p��:
        <code>
            CDCSetLineCoding(19200, NUM_STOP_BITS_1, PARITY_NONE, 8);
        </code>
        
        ���̋@�\�́Aoptional �ł��B�Ȃ��Ȃ� CDC devices �́AUSB�ʐM��
        �ۂɎ��ۂɂ�UART�̃n�[�h���R���g���[�����Ȃ�����  

    PreCondition:
        None
        
    Parameters:
        DWORD baud - The desired baudrate
        BYTE format - number of stop bits.  Available options are:
         * NUM_STOP_BITS_1 - 1 Stop bit
         * NUM_STOP_BITS_1_5 - 1.5 Stop bits
         * NUM_STOP_BITS_2 - 2 Stop bits
        BYTE parity - Type of parity.  The options are the following:
            * PARITY_NONE
            * PARITY_ODD
            * PARITY_EVEN
            * PARITY_MARK
            * PARITY_SPACE
        BYTE dataSize - number of data bits.  The options are 5, 6, 7, 8, or 16.
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetLineCoding(baud,format,parity,dataSize) {\
            CDCSetBaudRate(baud);\
            CDCSetCharacterFormat(format);\
            CDCSetParity(parity);\
            CDCSetDataSize(dataSize);\
        }

/******************************************************************************
    Function:
        BOOL USBUSARTIsTxTrfReady(void)
        
    Summary:
        ���� macro �� CDC bulk �ŁA�f�[�^�𑗂邱�Ƃ��ł��邩�m�F����

        �g�p��:
        <code>
            if(USBUSARTIsTxTrfReady())
            {
                putrsUSBUSART("Hello World");
            }
        </code>
        
    PreCondition:
        ���̖߂�l�́A�f�o�C�X��configured state �̂Ƃ��ɂ̂ݗL���ł���B
        (i.e. - USBDeviceGetState() returns CONFIGURED_STATE)
        
    Parameters:
        None
        
    Return Values:
        �߂�l�́A�u�P�v�܂��́u�O�v�ŁACDC�v���O�������V�����f�[�^���󂯎��A
        bulk��IN endpoint����host �ɑ��o�ł��邩�������܂��B �߂�l�́u�P�v
        �itrue�j�́ACDC handler ���󂯎��\�ł���AputrsUSBUSART() �� 
        putsUSBUSART()�Ȃǂ�API���R�[�����Ă����Ȃ����Ƃ������܂��B
        �߂�l�́u�O�v�ifalse�j�́Afirmware ���O�̃f�[�^���������Ă���A
        ���̎��_�ł́A�V�����f�[�^�̏��������鏀�����o���܂���B
        
    Remarks:
        ���[�U�[�A�v���P�[�V������CDCTxService() �����I�ɌĂяo���A
        �Ă��������A�����Ȃ��ƁAUSB IN transfers �́A�󋵂�i�߁A
        ���������������邱�Ƃ��ł��܂���B
  
 *****************************************************************************/
#define USBUSARTIsTxTrfReady()      (cdc_trf_state == CDC_TX_READY)

/******************************************************************************
    Function:
        void mUSBUSARTTxRam(BYTE *pData, BYTE len)
    
    Description:
        Depricated in MCHPFSUSB v2.3.
        ���� macro �́AUSBUSARTIsTxTrfReady()�ɒu����������B
 *****************************************************************************/
#define mUSBUSARTIsTxTrfReady()     USBUSARTIsTxTrfReady()

/******************************************************************************
    Function:
        void mUSBUSARTTxRam(BYTE *pData, BYTE len)
        
    Description:
        �f�[�^�������ɂ���f�[�^����������Ƃ��ɂ��� macro ���g�p�����
        �ȉ��̏����ł��� macro ���g�p����:
            1. �f�[�^�� null �ŏI�����Ȃ�
            2. �]������o�C�g�������m�ł���
        ����: cdc_trf_state == CDC_TX_READY �ł��邱��
        putsUSBUSART �Ƃ͈قȂ�A�����̍ۂɑ��M�󋵂��_�u���`�F�b�N����
        ���Ƃ͂Ȃ��̂ŁAcdc_trf_state != CDC_TX_READY �Ɏ��s�����
        �\�����Ȃ����������B
 
         �g�p��:
        <code>
            if(USBUSARTIsTxTrfReady())
            {
                mUSBUSARTTxRam(&UserDataBuffer[0], 200);
            }
        </code>
        
    PreCondition:
        cdc_trf_state �́ACDC_TX_READY �ł��邱��
        'len' �̒l�́A255 byte �ȉ��ł��邱��
        ����API���ŏ��ɁA���s����Ƃ��́AUSB �ڑ��󋵂��ACONFIGURED_STATE
        �ɂȂ��Ă��邱��
        
    Paramters:
        pDdata  : �f�[�^�̊J�n�A�h���X��Pointer
        len     : �]�������o�C�g��
        
    Return Values:
        None
        
    Remarks:
        ���� macro �́A�f�[�^�]���̏������s�������ŁA���ۂ̓]����
        CDCTxService() �ɂ����s�����B���� macro �́A�f�[�^��
        "double buffer" ���Ă��Ȃ��B  ���ׂẴf�[�^�����o���������
        �܂Ń��[�U�[�\�t�g�ł́ApData buffer �̓��e��ύX���Ă�
        �����Ȃ��B�����̏I���́A USBUSARTIsTxTrfReady() �Ŋm�F����
        ���Ƃ��ł���
        
  
 *****************************************************************************/
#define mUSBUSARTTxRam(pData,len)   \
{                                   \
    pCDCSrc.bRam = pData;           \
    cdc_tx_len = len;               \
    cdc_mem_type = USB_EP0_RAM;     \
    cdc_trf_state = CDC_TX_BUSY;    \
}

/******************************************************************************
    Function:
        void mUSBUSARTTxRom(rom BYTE *pData, BYTE len)
        
    Description:
        �v���O�����������ɂ���f�[�^����������Ƃ��ɂ��� macro ���g�p�����
        �ȉ��̏����ł��� macro ���g�p����:
            1. �f�[�^�� null �ŏI�����Ȃ�
            2. �]������o�C�g�������m�ł���
        ����: cdc_trf_state == CDC_TX_READY �ł��邱��
        putsUSBUSART �Ƃ͈قȂ�A�����̍ۂɑ��M�󋵂��_�u���`�F�b�N����
        ���Ƃ͂Ȃ��̂ŁAcdc_trf_state != CDC_TX_READY �Ɏ��s�����
        �\�����Ȃ����������B

          �g�p��:
        <code>
            if(USBUSARTIsTxTrfReady())
            {
                mUSBUSARTTxRom(&SomeRomString[0], 200);
            }
        </code>
       
    PreCondition:
        cdc_trf_state �́ACDC_TX_READY �ł��邱��
        'len' �̒l�́A255 byte �ȉ��ł��邱��
        ����API���ŏ��ɁA���s����Ƃ��́AUSB �ڑ��󋵂��ACONFIGURED_STATE
        �ɂȂ��Ă��邱��
        
    Paramters:
        pDdata  : �f�[�^�̊J�n�A�h���X��Pointer
        len     : �]�������o�C�g��
        
    Return Values:
        None
        
    Remarks:
        ���� macro �́A�f�[�^�]���̏������s�������ŁA���ۂ̓]����
        CDCTxService() �ɂ����s�����B
                    
 *****************************************************************************/
#define mUSBUSARTTxRom(pData,len)   \
{                                   \
    pCDCSrc.bRom = pData;           \
    cdc_tx_len = len;               \
    cdc_mem_type = USB_EP0_ROM;     \
    cdc_trf_state = CDC_TX_BUSY;    \
}

/**************************************************************************
  Function:
        void CDCInitEP(void)
    
  Summary:
    ���̊֐��� CDC function driver ������������B���̊֐��́A�K��
    SET_CONFIGURATION �R�}���h�̌�Ɏ��s���邱�ƁB
       (ex: within the context of the USBCBInitEP() function).
  Description:
    ���̊֐��� CDC function driver ������������B���̊֐��́Aline coding 
    �̊��菉���l (baud rate, bit parity, �r�b�g��,and format)�ݒ���s���B
    ������ endpoint ��L������ �z�X�g����̍ŏ��̓]������������B

    ���̊֐��́A�K�� SET_CONFIGURATION �R�}���h�̌�ɌĂяo�����ƁB
    �ȒP�ɂ� USBCBInitEP() ���Ăяo�����ƂŖ��������B
    
    �g�p��:
    <code>
        void USBCBInitEP(void)
        {
            CDCInitEP();
        }
    </code>
  Conditions:
    None
  Remarks:
    None                                                                   
  **************************************************************************/
void CDCInitEP(void);

/******************************************************************************
 	Function:
 		void USBCheckCDCRequest(void)
 
 	Description:
 		���̃��[�e�B���́A��M�����ŐV�� SETUP data packet ���m�F���A
 		CDC class �Ɋ֌W����v�����������`�F�b�N����B
 		�����ACDC class �Ɋ֌W����v��������΁A���̗v���ɑ΂��āA
 		�K�؂ȏ��������s����B

 	PreCondition:
 		���̊֐��́A�K���z�X�g���� control transfer �� SETUP packet 
 		���������Ă���A�Ăяo�����ƁB

	Parameters:
		None
		
	Return Values:
		None
		
	Remarks:
 		SETUP packet �� CDC class �Ɋ֌W����v���������ꍇ�A���̊֐��́A
 		�X�e�C�^�X�̍X�V�⏈���͈�؍s��Ȃ��B
  *****************************************************************************/
void USBCheckCDCRequest(void);


/**************************************************************************
  Function: void CDCNotificationHandler(void)
  Description: 
      DSR status ���`�F�b�N�� USB host�Ƀ��|�[�g����B
  Conditions: 
      �ŏ��� CDCNotificationHandler() ���Ăяo���O�ɁACDCInitEP() ��
      ��x�͌Ăяo������s����Ă��邱�ƁB
  Remarks:
    ���̊֐��́AUSB_CDC_SUPPORT_DSR_REPORTING �I�v�V�������L���ƂȂ��Ă���
    �ꍇ�ɂ̂݁A�K�v�Ƃ�����s�����B�L���̏ꍇ�A���̊֐��͒���I�ɌĂяo
    ���� DSR�s���̏�Ԃ��T���v������A��� USB host �ɑ��M�����B
    ���̂��߂ɁACDCNotificationHandler() ���̂��Ăяo�����A���߂� 
    CDCNotificationHandler() ���Ăяo������́A CDCTxService() ���Ăяo���B
  **************************************************************************/
void CDCNotificationHandler(void);


/**********************************************************************************
  Function:
    BOOL USBCDCEventHandler(USB_EVENT event, void *pdata, WORD size)
    
  Summary:
    USB stack ����� CDC endpoint �ɌW�� event ����������B 

  Description:
    USB stack ����� event ����������B���̊֐��́ACDC driver�ɂ��
    ��������Ȃ���΂Ȃ�Ȃ� USB event �������������ɁA���̊֐���
    �Ăяo�����K�v������B
    
  Conditions:
    ������'len'�� CDC class �Ƃ��āA USB host �����M����� bulk data 
    �p�� endpoint size �ȉ��łȂ���΂Ȃ�Ȃ��B
    ������'buffer'�� 'len'�ɂ��w�肳��钷���ȏ�̑傫�������o�b�t�@
    �G���A�̃A�h���X�� point ���Ȃ���΂Ȃ�Ȃ��B

  Input:
    event - �������� event �̃^�C�v
    pdata - pointer to the data that caused the event
    size - the size of the data that is pointed to by pdata
                                                                                   
  **********************************************************************************/
BOOL USBCDCEventHandler(USB_EVENT event, void *pdata, WORD size);


/**********************************************************************************
  Function:
        BYTE getsUSBUSART(char *buffer, BYTE len)
    
  Summary:
    getsUSBUSART �́AUSB CDC Bulk OUT��endpoint�����M���ꂽ�������
    ���[�U�[�̎w������ꏊ�ɃR�s�[������B
    ���̋@�\�̓f�[�^����M����܂őҋ@�͂����ɁA��M�f�[�^���������ɂ́A
    �߂�l��'0'�Ƃ��āA��M�f�[�^���������Ƃ��t�B�[�h�o�b�N����B
   
    �g�p��:
    <code>
        BYTE numBytes;
        BYTE buffer[64]
    
        numBytes = getsUSBUSART(buffer,sizeof(buffer));
        if(numBytes \> 0)
        {
            // numBytes �̎�M�f�[�^��"buffer"�ɃR�s�[���ꂽ�̂�
            //  �����Ń��[�U�[�̕K�v�ȏ������s��
        }
    </code>
  Conditions:
    ������'len'�� CDC class �Ƃ��āA USB host �����M����� bulk data 
    �p�� endpoint size �ȉ��łȂ���΂Ȃ�Ȃ��B
    ������'buffer'�� 'len'�ɂ��w�肳��钷���ȏ�̑傫�������o�b�t�@
    �G���A�̃A�h���X�� point ���Ȃ���΂Ȃ�Ȃ��B

  Input:
    buffer -  ��M���ꂽ�f�[�^�̕ۑ��ꏊ
    len -     �\�肳����M�o�C�g��
  Output:
    BYTE -    �߂�l�́A���ۂɎ�M����w��̏ꏊ�ɃR�s�[���ꂽ�o�C�g���� 
              ����A���̒l�́A0 ���������'len'�܂ł͈̔͂ɂ���B
              0 �́ACDC bulk OUT endpoint �ɐV���Ƀf�[�^���������Ƃ��Ӗ�����
              indicates that no new CDC bulk OUT endpoint data was available.
  **********************************************************************************/
BYTE getsUSBUSART(char *buffer, BYTE len);

/******************************************************************************
  Function:
	void putUSBUSART(char *data, BYTE length)
		
  Summary:
    putUSBUSART �́A��Q�̃f�[�^��USB�ɏ������ށB���̋@�\���g�p���邱�Ƃ�
    0x00 �𑗏o���邱�Ƃ��ł���B(����͈�ʓI�� NULL �����ƌ�����)
    
    �g�p��:
    <code>
        if(USBUSARTIsTxTrfReady())
        {
            char data[] = {0x00, 0x01, 0x02, 0x03, 0x04};
            putUSBUSART(data,5);
        }
    </code>
    device-to-host(put)�̎d�g�݂́Ahost-to-device(get)���A�Z�ʐ���
    �x��ł���Abulk IN endpoint �̃T�C�Y��蒷�������f�[�^����������
    ���Ƃ��ł���B�f�[�^�������ꍇ�ɂ́A���񂩂ɕ����ē]�����s������
    ������B�����̓]�����p���I�Ɏ��{���邽�߂� CDCTxService() �͒���I
    �ɌĂяo�����K�v������B

  Conditions:
    USBUSARTIsTxTrfReady() �̌��ʂ� TRUE �̕K�v������B����́A�Ō��
    �]�����������A�V���Ƀf�[�^�̎󂯓��ꏀ�������������Ƃ������Ă���B
     'data' �ɂ��|�C���g����镶����́A255�o�C�g�ȉ��ł��邱��

  Input:
    char *data - host �ɑ��M����� RAM �G���A�ɂ���f�[�^�̃|�C���^
    BYTE length - ���M�����o�C�g�� (255�ȉ�).
		
 *****************************************************************************/
void putUSBUSART(char *data, BYTE Length);

/******************************************************************************
	Function:
		void putsUSBUSART(char *data)
		
  Summary:
    putsUSBUSART �́Anull�������܂߂�������� USB �ɑ��M����
    'puts'�@�\�́ARAM �G���A�̕�������Ɏg�p����

    �g�p��:
    <code>
        if(USBUSARTIsTxTrfReady())
        {
            char data[] = "Hello World";
            putsUSBUSART(data);
        }
    </code>
    
    device-to-host(put)�̎d�g�݂́Ahost-to-device(get)���A�Z�ʐ���
    �x��ł���Abulk IN endpoint �̃T�C�Y��蒷�������f�[�^����������
    ���Ƃ��ł���B�f�[�^�������ꍇ�ɂ́A���񂩂ɕ����ē]�����s������
    ������B�����̓]�����p���I�Ɏ��{���邽�߂� CDCTxService() �͒���I
    �ɌĂяo�����K�v������B

  Conditions:
    USBUSARTIsTxTrfReady() �̌��ʂ� TRUE �̕K�v������B����́A�Ō��
    �]�����������A�V���Ƀf�[�^�̎󂯓��ꏀ�������������Ƃ������Ă���B
     'data' �ɂ��|�C���g����镶����́A255�o�C�g�ȉ��ł��邱��

  Input:
    char *data -  null�����ɂ��I��镶����B�����Anull������������Ȃ�
                  �ꍇ�ɂ́A255 �o�C�g�̃f�[�^�� host �ɑ��M�����
		
 *****************************************************************************/
void putsUSBUSART(char *data);


/**************************************************************************
  Function:
        void putrsUSBUSART(const ROM char *data)
    
    putsUSBUSART �́Anull�������܂߂�������� USB �ɑ��M����
    'putrs'�@�\�́A�v���O���������� �G���A�̕�������Ɏg�p����

    �g�p��:
    <code>
        if(USBUSARTIsTxTrfReady())
        {
            putrsUSBUSART("Hello World");
        }
    </code>
    
    device-to-host(put)�̎d�g�݂́Ahost-to-device(get)���A�Z�ʐ���
    �x��ł���Abulk IN endpoint �̃T�C�Y��蒷�������f�[�^����������
    ���Ƃ��ł���B�f�[�^�������ꍇ�ɂ́A���񂩂ɕ����ē]�����s������
    ������B�����̓]�����p���I�Ɏ��{���邽�߂� CDCTxService() �͒���I
    �ɌĂяo�����K�v������B

  Conditions:
    USBUSARTIsTxTrfReady() �̌��ʂ� TRUE �̕K�v������B����́A�Ō��
    �]�����������A�V���Ƀf�[�^�̎󂯓��ꏀ�������������Ƃ������Ă���B
     'data' �ɂ��|�C���g����镶����́A255�o�C�g�ȉ��ł��邱��

  Input:
    const ROM char *data -  null�����ɂ��I��镶����B�����Anull������������Ȃ�
                            �ꍇ�ɂ́A255 �o�C�g�̃f�[�^�� host �ɑ��M�����
      
  **************************************************************************/
void putrsUSBUSART(const ROM char *data);

/************************************************************************
  Function:
        void CDCTxService(void)
    
  Summary:
    CDCTxService �́Adevice-to-host �̑��M����������B���̋@�\��
    configured state �ȍ~�́A���C���v���O�����E���[�v�Ŗ���Ăяo������

  Description:
    CDCTxService �́Adevice-to-host �̑��M����������B���̋@�\��
    configured state (CDCIniEP() �@�\�����s���ꂽ)�ȍ~�́A���C��
    �v���O�������[�v�Ŗ���Ăяo������
    host �ɑ΂�CDC�V���A���f�[�^�𕡐��񑗐M���s�������ŁA�����ł�
    �X�e�[�g�Ǘ��ɕK�v�Ȋ֐��ł���B CDCTxService() �����I�Ɏ��s�ł��Ȃ�
    �ꍇ�AUSB host �ւ̑��M���s���Ȃ��B
    
    Typical Usage:
    <code>
    void main(void)
    {
        USBDeviceInit();
        while(1)
        {
            USBDeviceTasks();
            if((USBGetDeviceState() \< CONFIGURED_STATE) ||
               (USBIsDeviceSuspended() == TRUE))
            {
                //device configured �łȂ����Asuspended ���Ă���̂�
                //  �����Ȃ郆�[�U�[�A�v�������s���Ȃ�
                continue;   // ���[�v�̏��߂ɖ߂�
            }
            else
            {
                //�p���I�Ƀf�[�^�� PC �ɑ��M���邽�߂ɕK�v
                CDCTxService();
    
                //�����Ƀ��[�U�[�A�v��
                UserApplication();
            }
        }
    }
    </code>

  Conditions:
    CDCIniEP() �֐������Ɏ��s����Ă���Adevice ���ACONFIGURED_STATE
    �ɂ��邱��
  Remarks:
    None                                                                 
  ************************************************************************/
void CDCTxService(void);


/** S T R U C T U R E S ******************************************************/

/* Line Coding Structure */
#define LINE_CODING_LENGTH          0x07

typedef union _LINE_CODING
{
    struct
    {
        BYTE _byte[LINE_CODING_LENGTH];
    };
    struct
    {
        DWORD_VAL   dwDTERate;          // Complex data structure
        BYTE    bCharFormat;
        BYTE    bParityType;
        BYTE    bDataBits;
    };
} LINE_CODING;

typedef union _CONTROL_SIGNAL_BITMAP
{
    BYTE _byte;
    struct
    {
        unsigned DTE_PRESENT:1;       // [0] Not Present  [1] Present
        unsigned CARRIER_CONTROL:1;   // [0] Deactivate   [1] Activate
    };
} CONTROL_SIGNAL_BITMAP;


/* Functional Descriptor Structure - See CDC Specification 1.1 for details */

/* Header Functional Descriptor */
typedef struct __attribute__((packed)) _USB_CDC_HEADER_FN_DSC
{
    BYTE bFNLength;
    BYTE bDscType;
    BYTE bDscSubType;
    WORD bcdCDC;
} USB_CDC_HEADER_FN_DSC;

/* Abstract Control Management Functional Descriptor */
typedef struct __attribute__((packed)) _USB_CDC_ACM_FN_DSC
{
    BYTE bFNLength;
    BYTE bDscType;
    BYTE bDscSubType;
    BYTE bmCapabilities;
} USB_CDC_ACM_FN_DSC;

/* Union Functional Descriptor */
typedef struct __attribute__((packed)) _USB_CDC_UNION_FN_DSC
{
    BYTE bFNLength;
    BYTE bDscType;
    BYTE bDscSubType;
    BYTE bMasterIntf;
    BYTE bSaveIntf0;
} USB_CDC_UNION_FN_DSC;

/* Call Management Functional Descriptor */
typedef struct __attribute__((packed)) _USB_CDC_CALL_MGT_FN_DSC
{
    BYTE bFNLength;
    BYTE bDscType;
    BYTE bDscSubType;
    BYTE bmCapabilities;
    BYTE bDataInterface;
} USB_CDC_CALL_MGT_FN_DSC;

typedef union __attribute__((packed)) _CDC_NOTICE
{
    LINE_CODING GetLineCoding;
    LINE_CODING SetLineCoding;
    unsigned char packet[CDC_COMM_IN_EP_SIZE];
} CDC_NOTICE, *PCDC_NOTICE;

/* Bit structure definition for the SerialState notification byte */
typedef union
{
    BYTE byte;
    struct
    {
        BYTE    DCD             :1;
        BYTE    DSR             :1;
        BYTE    BreakState      :1;
        BYTE    RingDetect      :1;
        BYTE    FramingError    :1;
        BYTE    ParityError     :1;
        BYTE    Overrun         :1;
        BYTE    Reserved        :1;          
    }bits;    
}BM_SERIAL_STATE;  

/* Serial State Notification Packet Structure */
typedef struct
{
    BYTE    bmRequestType;  //Always 0xA1 for serial state notification packets
    BYTE    bNotification;  //Always SERIAL_STATE (0x20) for serial state notification packets
    UINT16  wValue;     //Always 0 for serial state notification packets
    UINT16  wIndex;     //Interface number
    UINT16  wLength;    //Should always be 2 for serial state notification packets
    BM_SERIAL_STATE SerialState;
    BYTE    Reserved;
}SERIAL_STATE_NOTIFICATION;   


/** E X T E R N S ************************************************************/
extern BYTE cdc_rx_len;
extern USB_HANDLE lastTransmission;

extern BYTE cdc_trf_state;
extern POINTER pCDCSrc;
extern BYTE cdc_tx_len;
extern BYTE cdc_mem_type;

extern volatile FAR CDC_NOTICE cdc_notice;
extern LINE_CODING line_coding;

extern volatile CTRL_TRF_SETUP SetupPkt;
extern ROM BYTE configDescriptor1[];

/** Public Prototypes *************************************************/
//------------------------------------------------------------------------------
//This is the list of public API functions provided by usb_function_cdc.c.
//This list is commented out, since the actual prototypes are declared above
//with associated inline documentation.
//------------------------------------------------------------------------------
//void USBCheckCDCRequest(void);
//void CDCInitEP(void);
//BOOL USBCDCEventHandler(USB_EVENT event, void *pdata, WORD size);
//BYTE getsUSBUSART(char *buffer, BYTE len);
//void putUSBUSART(char *data, BYTE Length);
//void putsUSBUSART(char *data);
//void putrsUSBUSART(const ROM char *data);
//void CDCTxService(void);
//void CDCNotificationHandler(void);
//------------------------------------------------------------------------------



#endif //CDC_H
