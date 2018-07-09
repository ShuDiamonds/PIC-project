
/*********************************************************************
*
*�@USB�ڑ� �I�V���X�R�[�v
*	USB�t��-�����[�N�C�����
*�@�@�R���t�B�M�����[�V�����̐ݒ�ύX
*    CDC�N���X�x�[�X
*�@�@main.c�@�̕ύX�@�������폜�@IO�ݒ�ύX
*�@�@io_cfg.h�̕ύX�@LED�폜�@IO���j�^�폜
*�@�@user.c�̕ύX�@�@�폜
*�@�@usbcfg.h�̕ύX�@IO���j�^�폜
*  ����Linker���C�����Ă���̂Œ��Ӂ���
*
*********************************************************************

/** I N C L U D E S ************************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                 // Required
#include "system\usb\usb.h"                  // Required
#include "io_cfg.h"                          // Required

#include "system\usb\usb_compile_time_validation.h"

/*** Configuration *******/
#pragma config FOSC = HSPLL_HS
#pragma config WDT = OFF
#pragma config PLLDIV = 5
#pragma config CPUDIV = OSC1_PLL2
#pragma config USBDIV = 2
#pragma config PWRT = ON
#pragma config BOR = ON
#pragma config BORV = 43
#pragma config LVP = OFF
#pragma config VREGEN = ON
#pragma config MCLRE = ON
#pragma config PBADEN = OFF

/** V A R I A B L E S **********************************************/
#pragma udata
static char input_buffer[64];			// USB���̓o�b�t�@
static char output_buffer[64];			// USB�o�̓o�b�t�@

unsigned int RcvSize;					// ��M�f�[�^�T�C�Y
char State = 0;						// �X�e�[�g�t���O
unsigned int Period;					// �T���v������
unsigned int Index = 0;				// �o�b�t�@�C���f�b�N�X
char FreeFlag;						// �t���[�����t���O
byte New_Data;						// ����f�[�^
byte Old_Data;						// �O��f�[�^
byte Threshold;						// �g���K�p�X���b�V�����h
byte Sample;							// �T���v�����O����
byte Channel;							// �`���l���ԍ�

rom char MsgOK[] ="OK";				// OK�������b�Z�[�W
// �A�i���O�v���o�b�t�@�@���v960�o�C�g
#pragma udata gpr1 
static byte BufferA[512];				// 
#pragma udata usb6
static byte BufferB[448];				// CDC�ňꕔ�g�p�ALinker��Protect���͂���

/** P R I V A T E  P R O T O T Y P E S ******************************/
void ltostring(char digit, unsigned long data, byte *buffer);
byte AD_input(char chanl);
void ISRProcess(void);

/** V E C T O R  R E M A P P I N G **********************************/
#pragma code high_vector=0x08
void interrupt_at_high_vector(void)
{
    _asm goto ISRProcess _endasm
}
#pragma code

/*****************************************/
/* CCP1�̊��荞�ݏ���
/* ��������A/D�ϊ����s���M���T���v�����O
/* �ŒZ10��sec�����������\��50��sec
/****************************************/
#pragma interrupt ISRProcess
void ISRProcess(void)
{
	PIR1bits.CCP1IF = 0;					// ���荞�݃t���O�N���A
	Old_Data = New_Data;					// �����p�ɂQ����
	New_Data = AD_input(Channel);			// �V�K�f�[�^�T���v�����O
   	//// �g���K���o�ƕۑ�
   	switch (State)
   	{
   		case 0:		/// �g���K���o
   			if(FreeFlag == 0) {
	   			if(Threshold > 127) {			// �v���X�̎��͗����オ��g���K
	  	 			if((Old_Data < Threshold) && (New_Data > Threshold)) {
		  	 			State = 1;
		  	 			Index = 0;
		  	 		}
		  	 	}
		  	 	else {						// �}�C�i�X�̎��͗���������g���K
			  	 	if((Old_Data > Threshold) && (New_Data < Threshold)) {
				  	 	State = 1;
				  	 	Index = 0;
				  	 }
				}
			}
			else {
				State = 1;
				Index = 0;
			}			
	  	 	break;
	 	case 1:		/// �f�[�^�i�[
	         if(Index < 512) {				// �܂�A�o�b�t�@�Ɋi�[
	   	      	GreenLED = !GreenLED;		         
				BufferA[Index++] = New_Data;
			}
			else {						// ������B�o�b�t�@�Ɋi�[
				if(Index < 960) {
	   	      		YellowLED = !YellowLED;	
					BufferB[Index-512] = New_Data;
					Index++;
				}
				else {					// �o�b�t�@��t�ŃX�e�[�g�X�V
					State = 2;
					Index = 0;
					T1CONbits.TMR1ON = 0;	// �^�C�}�P��~
				}
			}
			break;
		default: break;
	}
}
//////// A/D�ϊ��ǂݍ��݊֐�
byte AD_input(char chanl)
{
    	byte adData;        				// 8�r�b�g���[�h
    	
	ADCON0 = 0x01 + (chanl * 4);
    	ADCON0bits.GO = 1;              	// Start AD conversion
    	while(ADCON0bits.NOT_DONE);     	// Wait for conversion
 	adData = (byte)ADRESH;				// ��ʂW�r�b�g�̂ݎ��o��
	return(adData);                    	// �f�[�^�Ԃ�
}


/**********************************************************
 * ���C���֐�
 * ��������USB��M�҂��A��M�����R�}���h�ɉ��������������s
 *********************************************************/
void main(void)
{
	int	i;
	
	ADCON1 = 0x07;             	// �|�[�gA�S�A�i���O�ɐݒ�
	ADCON2 = 0x16;				// Left,Tad��Tcy/64 Taq=4Tad 
	ADCON0 = 0x1D;				// AD On	
	CMCON = 0x07;					// �R���p���[�^�I�t
	/// �|�[�g������	
   	LATB = 0xFF;					// LED�I�t
 	TRISA = 0xFF;					// �A�i���O����
	TRISB = 0x00;					// LED�o��
 	TRISC = 0xB0;					// USB
 	// �^�C�}�P������
 	T1CON = 0xB0;					// �����N���b�N �v���X�P�[��1/8
 	TMR1H = 0;					// � = 48MHz/4/8=1.5MHz
 	TMR1L = 0;
	T3CON = 0;					// CCP1��v�Ń^�C�}�P���Z�b�g
 	// CCP1������
 	Period = 15;					// 10usec �P�ʂƂ���
 	CCP1CON = 0x0B;				// ��v�Ń^�C�}�P�N���A
	CCPR1 = Period * 10;			// 10usec�P��(10usec to 40.390msec)	
	/// USB������
	mInitializeUSBDriver();        	// See usbdrv.h
	/// �ϐ�������
	State = 0;					// �X�e�[�g�t���O���Z�b�g
	Threshold = 190;				// �g���K�v�X���b�V�����h�����l
	Sample =10;					// �T���v�����������l100usec
	FreeFlag = 1;					// �t���[�����t���O�I��
	Channel = 0;					// �`���l���ԍ�0
	T1CONbits.TMR1ON = 0;			// �^�C�}������~
	// ���荞�݋���
	RCONbits.IPEN = 0;			// �D�揇�ʂȂ�
 	PIE1bits.CCP1IE = 1;			// CCP1���荞�݋���		
	INTCON = 0xC0;				// �O���[�o�����荞�݋���

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
				RedLED = !RedLED;								// �ڈ�LED
	    			switch(input_buffer[0])						// �ŏ��̂P�����`�F�b�N
	    			{
		    			case '0':								// OK���b�Z�[�W�ԑ�
						State = 0;
        					if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("OK");
           			 	break;
           			case '1':								// �f�[�^���W�J�n�R�}���h
           				State = 0;							// �X�e�[�g������ԃZ�b�g
           				T1CONbits.TMR1ON = 1;					// �^�C�}�P�X�^�[�g
           				Threshold = (byte)input_buffer[1];		// �X���b�V�����h�擾
           				Sample = (byte)input_buffer[2];			// �T���v�����O�����擾
           				CCPR1 = Period * Sample;				// �����ݒ�10usec�P��
           				Channel = (byte)input_buffer[3];		// �`���l���ԍ��ݒ�
	    					FreeFlag = 0;							// �t���[�����I�t           				
           				break;	
           			case '2':								// ��Ԗ₢���킹
           				if(State == 2) {						// �f�[�^�擾�������H
        						if(mUSBUSARTIsTxTrfReady())	         					
           			 			putrsUSBUSART("1");			// �����Ȃ�P��Ԃ�
           			 	}
           			 	else {
       						if(mUSBUSARTIsTxTrfReady())
           			 			putrsUSBUSART("0");			// �������Ȃ�O��Ԃ�
           			 	}
           			 	break;
		    			case '3':								// �v���f�[�^���M
				  		if((Index < 512) && (State == 2)) {		// �o�b�t�@A�̑��M
							/// �v���f�[�^��512�o�C�g��]������
						 	if(mUSBUSARTIsTxTrfReady())
           			 			mUSBUSARTTxRam(&BufferA[Index], 64);	// ���M���s
							Index += 64;
						}									
						else {								// �o�b�t�@B�̑��M
							if((State == 2) && (Index < 960)) {
							 	if(mUSBUSARTIsTxTrfReady())
							 		mUSBUSARTTxRam(&BufferB[Index-512], 64);
								Index += 64;
							}
							else							// ���M�����ŃX�e�[�g���Z�b�g
								State = 0;
						}
		    				break;
		    			case '4':								// �t���[�����w��
           				State = 0;							// �X�e�[�g������ԃZ�b�g
           				T1CONbits.TMR1ON = 1;					// �^�C�}�P�X�^�[�g
           				Threshold = (byte)input_buffer[1];		// �X���b�V�����h�擾
           				Sample = (byte)input_buffer[2];			// �T���v�����O�����擾
           				CCPR1 = Period * Sample;				// �����ݒ�10usec�P��
            				Channel = (byte)input_buffer[3];		// �`���l���ԍ��ݒ�          				
	    					FreeFlag = 1;							// �t���[�����I��          				
           				break;			    			
		    			default: break;
		    		}
    			}
    		}
     }
}

