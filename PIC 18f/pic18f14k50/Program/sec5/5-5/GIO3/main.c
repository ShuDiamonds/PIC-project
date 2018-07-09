/**************************************************************
*
*�@USB�ڑ� �ėp�h�^�n�v���O����
*   �����̔ėpUSB�N���X���g�p
*   �e�X�Q�̃p�C�v�����C���^�[�t�F�[�X���Q�ɑ��₵���o�[�W����
*	USB�t��-�����[�N�C�����
*�@�@�R���t�B�M�����[�V�����̐ݒ�ύX
*�@�@main.c�@�̕ύX�@�������폜�@IO�ݒ�ύX
*�@�@io_cfg.h�̕ύX�@LED�폜�@IO���j�^�폜
*�@�@user.c�̕ύX�@�@���s���e�폜
*�@�@usbcfg.h�̕ύX�@IO���j�^�폜
*�@�@usbdsc.c�Ausbdsc.h�@�f�X�N���v�^�ύX
*    usbgen.c�Ausbgen.h�@�G���h�|�C���g�Q�p�ǉ�
*    uabmmap.c�Ausbmmap.h�@USB�p�o�b�t�@�̒ǉ��@�o���N�z�u�ύX
*
***************************************************************

/** I N C L U D E S *******************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                 // Required
#include "system\usb\usb.h"                  // Required
#include "io_cfg.h"                          // Required

#include "system\usb\usb_compile_time_validation.h"
#include	"delays.h"
#include "lcd_lib.h"

/** V A R I A B L E S *****************************************/
#pragma udata
char input_buffer[64];					// USB���̓o�b�t�@
char output_buffer[64];					// USB�o�̓o�b�t�@
unsigned int  Pot0;						// �v���f�[�^
unsigned int Counter;						// �܂�Ԃ��p�J�E���^
unsigned int RcvSize;						// ��M�f�[�^�T�C�Y

char MsgOK[] ="OK";						// OK���b�Z�[�W
char MsgPot[] =  "Pot =        \r\n";		// �t���\���p�o�b�t�@
char MsgStart[] = "Start!";				// �������b�Z�[�W
char MsgHello[] = "Welcome        \r\n";	// �܂�Ԃ��p���b�Z�[�W
char MsgState[] = " ";					// LED���
// �A�i���O�f�[�^���M�o�b�t�@�i�W�`���l�����~�S�����j
char MsgAnalog[] = "                                      ";
/** P R I V A T E  P R O T O T Y P E S ************************/
void ltostring(char digit, unsigned long data, char *buffer);
unsigned int AD_input(char chanl);

/** V E C T O R  R E M A P P I N G ****************************/
/*
extern void _startup (void);        		// 
#pragma code _RESET_INTERRUPT_VECTOR = 0x000800
void _reset (void)
{
    _asm goto _startup _endasm
}
*/
#pragma code
/****************************************************
 * Function:        void main(void)
 ****************************************************/
void main(void)
{
	int	i;
	
	ADCON1 = 0x07;             	// RA,RE�S�A�i���O�ɐݒ�
	ADCON2 = 0xBB;				// Right,Tad��Tcy/16 
	ADCON0 = 0x1D;				// AD On	
	CMCON = 0x07;					// �R���p���[�^�I�t
 	LATA = 0xFF;					// �����o��
   	LATB = 0;					// �t���\���|�[�g���Z�b�g
  	LATC = 0x07;					// LED Off
  	LATD = 0;
 	TRISA = 0xFF;					// �A�i���O����
	TRISB = 0x00;					// �t���\���o��
 	TRISC = 0xB8;					// LED, USART
 	TRISD = 0xF0;					// Universal I/O
 	TRISE = 0xFF;					// �A�i���O����
	/// USB������
	mInitializeUSBDriver();        	// See usbdrv.h
	/// �t���\���평����
	Counter = 0;
	lcd_init();					// LCD������
	lcd_str(MsgStart);			// �������b�Z�[�W�\��

    /// ���C�����[�v
    while(1)
    {
	    /// USB�C�x���g�|�[�����O
    		USBCheckBusStatus();        							// USB�ڑ��`�F�b�N
    		if(UCFGbits.UTEYE!=1)									// �A�C�p�^�[�����[�h�I�t��
        		USBDriverService();
 		/// ��M�f�[�^����
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
			/// ��M�`�F�b�N
			RcvSize = USBGenRead((byte*)&input_buffer,sizeof(input_buffer));
			if(RcvSize)										// �f�[�^��M���肩�H
    			{
            		LATCbits.LATC0 = !LATCbits.LATC0;				// LED0���]�ڈ�p
	    			switch(input_buffer[0])						// �ŏ��̂P�����`�F�b�N
	    			{
		    			case '0':								// ���b�Z�[�W�ԑ�
         				if(!mUSBGenTxIsBusy())						// ���M���f�B�[���H         			
           			 		USBGenWrite((byte*)MsgOK, 3);		// ���M���s
           			 	break;		    				
		    			case '1':								// 
            				LATCbits.LATC1 = !LATCbits.LATC1;		// LED1���]
            				if(LATCbits.LATC1 == 1)
            					MsgState[0] = '1';				// ���ݏ�ԕԑ�
            				else
            					MsgState[0] = '0';
        					if(!mUSBGenTxIsBusy())					// ���M���f�B�[���H
        						USBGenWrite((byte*)MsgState, 1);
		    				break;
		    			case '2':
            				LATCbits.LATC2 = !LATCbits.LATC2;		// LED2���]
             				if(LATCbits.LATC2 == 1)
            					MsgState[0] = '1';					// ���ݏ�ԕԑ�
            				else
            					MsgState[0] = '0';
        					if(!mUSBGenTxIsBusy())					// ���M���f�B�[���H	
        						USBGenWrite((byte*)MsgState, 1);
		    				break;
		    			case '3':
		    				lcd_cmd(input_buffer[1]);				// �t�����b�Z�[�W�\��
		    				lcd_str((char *)(&input_buffer[3]));
		    				break;
		    			case '4':
		    				lcd_clear();							// �t������
		    				Counter = 0;
		    				break;
		    			case '5':
	    					Pot0 = AD_input(7);
	    					ltostring(4, Pot0, MsgPot+6);			// �ϒ�R�̒l���M
         				if(!mUSBGenTxIsBusy())					// ���M���f�B�[���H		    					
           			 		USBGenWrite((byte*)MsgPot, 15);
		    				break;
		    			case '6':								// PORTD�ւ̏o�͂Ɠ���
		    				LATD = input_buffer[1];
						MsgState[0] = PORTD;
           				if(!mUSBGenTxIsBusy())					// ���M���f�B�[���H
            					USBGenWrite((byte*)MsgState, 1);
		    				break;
		    			case '7':
         				for(i=0; i<8; i++)					// 8�`���l�����v��
	         			{
			    				Pot0 = AD_input(i);
		    					ltostring(4, Pot0, MsgAnalog+4*i);	// �ϒ�R�̒l�ҏW
           			 	}
		    				if(!mUSBGenTxIsBusy())					// ���M���f�B�[���H
        			 			USBGenWrite((byte*)MsgAnalog, 32);	// ���M���s
           			 	break;
           			 case '8':
         				Counter++;								// ���b�Z�[�W�J�E���^�X�V
         				ltostring(6, Counter, MsgHello+8);	
         				if(!mUSBGenTxIsBusy())						// ���M���f�B�[���H         			
           			 		USBGenWrite((byte*)MsgHello, 17);	// ���M���s           			 
           			 	break;
           			 	
		    			default: break;
		    		}
    			}
			/// ��M�`�F�b�N
			RcvSize = USBIORead((byte*)&input_buffer,sizeof(input_buffer));		if(RcvSize)										// �f�[�^��M���肩�H
    			{
            		LATCbits.LATC0 = !LATCbits.LATC0;				// LED0���]�ڈ�p
	    			switch(input_buffer[0])						// �ŏ��̂P�����`�F�b�N
	    			{
		    			case '0':								// ���b�Z�[�W�ԑ�
         				if(!mUSBIOTxIsBusy())						// ���M���f�B�[���H         			
           			 		USBIOWrite((byte*)MsgOK, 3);		// ���M���s
           			 	break;		    				
		    			case '1':								// 
            				LATCbits.LATC1 = !LATCbits.LATC1;		// LED1���]
            				if(LATCbits.LATC1 == 1)
            					MsgState[0] = '1';					// ���ݏ�ԕԑ�
            				else
            					MsgState[0] = '0';
        					if(!mUSBIOTxIsBusy())					// ���M���f�B�[���H
        						USBIOWrite((byte*)MsgState, 1);
		    				break;
		    			case '5':
	    					Pot0 = AD_input(0);
	    					ltostring(4, Pot0, MsgPot+6);			// �l���M
         				if(!mUSBIOTxIsBusy())						// ���M���f�B�[���H		    					
           			 		USBIOWrite((byte*)MsgPot, 15);
		    				break;
					default: break;
				}
			}
    		}
     }
}

//////// A/D�ϊ��ǂݍ��݊֐�
unsigned int AD_input(char chanl)
{
    	unsigned int adData;        			// 10�r�b�g���[�h
    	
	ADCON0 = 0x01 + (chanl * 4);
    	ADCON0bits.GO = 1;              		// Start AD conversion
    	while(ADCON0bits.NOT_DONE);     		// Wait for conversion
 	adData = ADRESH * 256 + ADRESL;
	return(adData);                   	 	// �f�[�^�Ԃ�
}

///// ���l���當����ɕϊ�
void ltostring(char digit, unsigned long data, char *buffer)
{
	char i;
	buffer += digit;						// ������̍Ō�
	for(i=digit; i>0; i--) {				// �ŉ��ʌ������ʂ�
		buffer--;							// �|�C���^�[�P
		*buffer = (data % 10) + '0';		// ���̌����l�𕶎��ɂ��Ċi�[
		data = data / 10;					// ��-1
	}
}

