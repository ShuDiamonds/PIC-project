/*********************************************************************
*
*�@USB�ڑ� �ėp�h�^�n�v���O����
*	USB�t��-�����[�N�C�����
*�@�@�R���t�B�M�����[�V�����̐ݒ�ύX
*    CDC�N���X�x�[�X
*�@�@main.c�@�̕ύX�@�������폜�@IO�ݒ�ύX
*�@�@io_cfg.h�̕ύX�@LED�폜�@IO���j�^�폜
*�@�@user.c�̕ύX�@�@���s���e�폜
*�@�@usbcfg.h�̕ύX�@IO���j�^�폜
*
*********************************************************************

/** I N C L U D E S ************************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                 // Required
#include "system\usb\usb.h"                  // Required
#include "io_cfg.h"                          // Required

#include "system\usb\usb_compile_time_validation.h"
#include	"delays.h"
#include "lcd_lib.h"
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

/*** Configuration *******/
#pragma config FOSC = HSPLL_HS
#pragma config WDT = OFF
#pragma config PLLDIV = 5
#pragma config CPUDIV = OSC1_PLL2
#pragma config USBDIV = 2
#pragma config PWRT = ON
#pragma config BOR = OFF
//#pragma config BORV = 43
#pragma config LVP = OFF
#pragma config VREGEN = ON
#pragma config MCLRE = ON
#pragma config PBADEN = OFF

/** V A R I A B L E S **********************************************/
#pragma udata
char input_buffer[64];						// USB���̓o�b�t�@
char output_buffer[64];						// USB�o�̓o�b�t�@
unsigned int  Pot0;							// �v���f�[�^
unsigned int Counter;
unsigned int cmnd;
char MsgPot[] =  "Pot =         ";			// �t���\���p�o�b�t�@
char MsgStart[] = "Start!";					// �������b�Z�[�W
char MsgHello[] = "Welcome         ";
char MsgState[] = "  ";						// LED���
// �A�i���O�f�[�^���M�o�b�t�@�i�W�`���l�����~�S�����j
char MsgAnalog[] = "                                    ";

/** P R I V A T E  P R O T O T Y P E S ******************************/
void ltostring(char digit, unsigned long data, byte *buffer);
unsigned int AD_input(char chanl);
unsigned int toHEX(char *str);

#pragma code
/*********************************************************************
 * Function:        void main(void)
 ********************************************************************/
void main(void)
{
	int i;
	
	ADCON1 = 0x07;                 		// RA,RE�S�A�i���O�ɐݒ�
	ADCON2 = 0xBB;					// Right,Tad��Tcy/16 
	ADCON0 = 0x1D;					// AD On	
	CMCON = 0x07;						// �R���p���[�^�I�t
 	LATA = 0xFF;						// �����o��
   	LATB = 0;							// �t���\���|�[�g���Z�b�g
  	LATC = 0x07;						// LED Off
  	LATD = 0;
 	TRISA = 0xFF;						// �A�i���O����
	TRISB = 0x00;						// �t���\��
 	TRISC = 0xB8;						// LED, USART
 	TRISD = 0xF0;						// Universal I/O
 	TRISE = 0xFF;
	/// USB������
	mInitializeUSBDriver();         	// See usbdrv.h
	/// �t���\���평����
	Counter = 0;
	lcd_init();						// LCD������
	lcd_str(MsgStart);				// �������b�Z�[�W�\��

    /// ���C�����[�v
    while(1)
    {
    		USBCheckBusStatus();        							// USB�ڑ��`�F�b�N
    		if(UCFGbits.UTEYE!=1)									// �A�C�p�^�[�����[�h�I�t��
        		USBDriverService();								// USB�C�x���g�|�[�����O
		CDCTxService();										// �N���X����M���s
 		/// ����M�f�[�^�̏������s
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
    			if(getsUSBUSART(input_buffer,64))					// �f�[�^��M�|�[��
    			{
            		LATCbits.LATC0 = !LATCbits.LATC0;				// LED0���]
	    			switch(input_buffer[0])						// �ŏ��̂P�����`�F�b�N
	    			{
		    			case '0':								// OK���b�Z�[�W�ԑ�
        					if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("OK");
           			 	break;		    				
		    			case '1':								// 
            				LATCbits.LATC1 = !LATCbits.LATC1;		// LED1���]
            				if(LATCbits.LATC1 == 1)
            					MsgState[0] = '1';				// ���ݏ�ԕԑ�
            				else
            					MsgState[0] = '0';
            				mUSBUSARTTxRam((byte*)MsgState, 1);
		    				break;
		    			case '2':
            				LATCbits.LATC2 = !LATCbits.LATC2;		// LED2���]
             				if(LATCbits.LATC2 == 1)
            					MsgState[0] = '1';				// ���ݏ�ԕԑ�
            				else
            					MsgState[0] = '0';
            				mUSBUSARTTxRam((byte*)MsgState, 1);
		    				break;
		    			case '3':
		    				cmnd = toHEX(&input_buffer[1]);
		    				lcd_cmd(cmnd);						// �t�����b�Z�[�W�\��
		    				lcd_str((char *)(&input_buffer[3]));
		    				break;		    			
		    			case '4':
		    				lcd_clear();							// �t������
		    				Counter = 0;
		    				break;
		    			case '5':
	    					Pot0 = AD_input(7);
	    					//ltostring(4, Pot0, MsgPot+6);			// �ϒ�R�̒l���M
        					if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam((byte*)MsgPot, 10);
		    				break;
		    			case '6':
		    				LATD = input_buffer[1];				// PORTD�̐���
		    				MsgState[0] = PORTD;
        					if(mUSBUSARTIsTxTrfReady())	
           			 		mUSBUSARTTxRam((byte*)MsgState, 1);
		    				break;
		    			case '7':
         				for(i=0; i<8; i++)					// 8�`���l�����v��
	         			{
			    				Pot0 = AD_input(i);
		    					//ltostring(4, Pot0, MsgAnalog+4*i);	// �ϒ�R�̒l�ҏW
           			 	}
        					if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam((byte*)MsgAnalog, 32);
		    				break; 
		    			case '8':								// ���b�Z�[�W�ԑ�
        					Counter++;
        					//ltostring(6, Counter, MsgHello+8);
        					if(mUSBUSARTIsTxTrfReady())	
           			 		mUSBUSARTTxRam((byte*)MsgHello, 14);
		    				break;
		    			default: break;
		    		}
    			}
    		}
     }
}

/////// �Q������16�i���P���ɕϊ�
unsigned int toHEX(char* str)
{
	int data;

	data = 0;
	if((*str >= '0') && (*str <= '9'))
		data = (int)(16 * (*str - '0'));
	else
	{
		if((*str >= 'A') && (*str <= 'F'))
			data = (int)(16 * (*str - 'A' + 10));
		else
		{
			if((*str >= 'a') && (*str <= 'f'))
				data = (int)(16 * (*str - 'a' + 10));
		}
	}
	str++;
	if((*str >= '0') && (*str <= '9'))
		data += (int) (*str - '0');
	else
	{
		if((*str >= 'A') && (*str <= 'F'))
			data += (int)(*str - 'A' + 10);
		else
		{
			if((*str >= 'a') && (*str <= 'f'))
				data += (int)(*str - 'a' + 10);
		}
	}	
	return(data);
}

//////// A/D�ϊ��ǂݍ��݊֐�
unsigned int AD_input(char chanl)
{
    	unsigned int adData;        		// 10�r�b�g���[�h
    	
	ADCON0 = 0x01 + (chanl * 4);
    	ADCON0bits.GO = 1;              	// Start AD conversion
    	while(ADCON0bits.NOT_DONE);     	// Wait for conversion
 	adData = ADRESH * 256 + ADRESL;
	return(adData);                    	// �f�[�^�Ԃ�
}

///// ���l���當����ɕϊ�
void ltostring(char digit, unsigned long data, byte *buffer)
{
	char i;
	buffer += digit;					// ������̍Ō�
	for(i=digit; i>0; i--) {			// �ŉ��ʌ������ʂ�
		buffer--;						// �|�C���^�[�P
		*buffer = (data % 10) + '0';	// ���̌����l�𕶎��ɂ��Ċi�[
		data = data / 10;				// ��-1
	}
}

