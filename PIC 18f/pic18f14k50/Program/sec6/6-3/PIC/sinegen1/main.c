/*********************************************************************
*
*�@USB�ڑ� �����g���U��v���O����
*	USB�t���[�����[�N�C�����
*�@�@�R���t�B�M�����[�V�����̐ݒ�ύX
*�@�@main.c�@�̕ύX�@�������폜�@IO�ݒ�ύX
*�@�@io_cfg.h�̕ύX�@LED�폜�@IO���j�^�폜
*�@�@user.c�̕ύX�@�@���s���e�폜
*�@�@usbcfg.h�̕ύX�@IO���j�^�폜
*
*********************************************************************
/** I N C L U D E S *************************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                        // Required
#include "system\usb\usb.h"                         // Required
#include "io_cfg.h"                                 // Required

#include "system\usb\usb_compile_time_validation.h" // Optional
#include	"delays.h"
#include "lcd_lib.h"

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

/** V A R I A B L E S ***********************************************/
#pragma udata
char input_buffer[64];					//USB���̓o�b�t�@
char output_buffer[64];					//USB�o�̓o�b�t�@
unsigned long Freq = 10;					//���g���f�[�^ 32bits
unsigned long Delta = 10;					//���g���ϕ� 32bits
char flag = 0;							//�����g/�O�p�g

byte MsgFreq[] =  "Freq =        Hz";		//�t���\���p�o�b�t�@
char MsgStart[] ="Start!";					//�������b�Z�[�W
char MsgSine[] = "Sine Wave       ";		//�t���\�����b�Z�[�W
char MsgTri[]  = "Triangle        ";		//�t���\�����b�Z�[�W

/** P R I V A T E  P R O T O T Y P E S *******************************/
void SetupDDS(unsigned long freqdata, char flag);
void SerialOut(unsigned long data);
void Display(void);
void ltostring(char digit, unsigned long data, byte *buffer);

#pragma code
/*********************************************************************
 * Function:        void main(void)
 ********************************************************************/
void main(void)
{
	int i;
	
	ADCON1 |= 0x0F;                 	// �S�f�B�W�^���ɐݒ�
	CMCON = 0x07;						// �R���p���[�^�I�t
 	LATA = 0x06;						// �G���R�[�_�ADDS�����o��
   	LATB = 0;						// �t���\���|�[�g���Z�b�g
  	LATC  = 0x03;						// LED off
 	TRISA = 0x38;						// �X�C�b�`
	TRISB = 0x01;						// �t���\��
 	TRISC = 0x38;						// LED Output Mode
	/// USB������
	mInitializeUSBDriver();         	// See usbdrv.h
	/// �t���\���평����
	lcd_init();						// LCD������
	lcd_str(MsgStart);				// �������b�Z�[�W�\��
	Delay10KTCYx(500);				// 0.5�b
	Freq = 1000;						// �����l1kHz
	SetupDDS(Freq, flag);				// DDS�o��
	Display();						// �t���\��
	Delay10KTCYx(100);
    /// ���C�����[�v
    while(1)
    {
    		USBCheckBusStatus();        	// USB�ڑ��`�F�b�N
    		if(UCFGbits.UTEYE!=1)			// �A�C�p�^�[�����[�h�I�t��
        		USBDriverService();		// USB�C�x���g�|�[�����O
		CDCTxService();				// �N���X����M���s
		/// ����M�f�[�^�̏������s
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
    			if(getsUSBUSART(input_buffer,64))		// �f�[�^��M�|�[��
    			{
            		LATCbits.LATC0 = !LATCbits.LATC0;	// LED���]
	    			switch(input_buffer[0])			// �ŏ��̂P�����`�F�b�N
	    			{
		    			case '0':					// OK����
         				if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("OK");
           			 	break;		    			
		    			case '1':					// ���ݒl����
         				if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam(MsgFreq+7, 6);
           			 	break;		    				
					case '2':					// ���g���ݒ�@�����g
						Freq = 0;
						for(i=0; i<6; i++)
						{
							Freq = Freq * 10 + (input_buffer[i+1] - 0x30);
						}
						flag = 0;
						break;
					case '3':					// ���g���ݒ� �O�p�g
						Freq = 0;
						for(i=0; i<6; i++)
						{
								Freq = Freq *10 + (input_buffer[i+1]-0x30);
						}
						flag = 1;
						break;  				
		    			default: break;
		    		}
		    		SetupDDS(Freq, flag);				// DDS�ݒ�o��
	    			Display();
    			}
    		}
    		//// ���g�����ݐݒ�X�C�b�`�`�F�b�N
    		if(stepSW)
    			Delta = 1000;
    		else
    			Delta = 10;    		
    		//// �G���R�[�_�̓��̓`�F�b�N
    		if(encoder_a == 0)
    		{

	    		if(encoder_b == 0)
	    			Freq += Delta;				// �E����J�E���g�A�b�v
	    		else
	    			Freq -= Delta;				// ������J�E���g�_�E��

	    		SetupDDS(Freq, flag);				// DDS�ݒ�o��
	    		Display();						// �t���\��
			while(encoder_a == 0);			// �G���R�[�_���͊����҂�
    		}
     }
}

////�@DDS�V���A���o�͊֐�
void SetupDDS(unsigned long freqdata, char flag) {
	float SetFreq;
	unsigned long temp;
	unsigned int uptemp, lowtemp;
	
	///�ݒ���g������ݒ�p�����[�^�v�Z
	///�N���b�N50MHz�Ōv�Z
	SetFreq = 5.36871 * (unsigned long)freqdata;	// �ݒ�l
	temp = (unsigned long)SetFreq;				// �����ɕϊ�
	lowtemp =(unsigned int)(temp & 0x3FFF);			// ����16�r�b�g
	uptemp = (unsigned int)((temp >> 14) & 0x3FFF);	// ���16�r�b�g
	/// DDS�փR�}���h���M
	/// Sine 0x2028   Triangle 0x200A
	if(flag == 0)
		SerialOut(0x2028);				// �R�}���h�o��
	else
		SerialOut(0x200A);				// �O�p�g�R�}���h�o��
	SerialOut(lowtemp + 0x4000);			// ���g���f�[�^�o��
	SerialOut(uptemp + 0x4000);
}

//// DDS���M����֐�
void SerialOut(unsigned long data) {
	unsigned char i;
	unsigned int mask;
	mask = 0x8000;
	
	dds_fsync = 0;						// FSYNC low
	for(i=0; i<16; i++) {
		if(data & mask)					// ��ʂ��瑗�M
			dds_sdat = 1;	
		else
			dds_sdat = 0;
		dds_sclk = 0;						// SCK
		dds_sclk = 1;
		mask = mask >> 1;					// next bit
	}
	dds_fsync = 1;						// FSYNC High
}

/// �f�[�^�\���֐�
void Display(void) 
{
	//// ���g���ϕ��\��
	lcd_cmd(0x80);						//�P�s�ڂ�
	ltostring(6, Freq, &MsgFreq[0]+7);		// ���g���\��
	lcd_str((char *)MsgFreq);				// ���o���\��
	lcd_cmd(0xC0);						// �Q�s�ڂ�
	if(flag == 0)
		lcd_str(MsgSine);					// �����g�\��
	else
		lcd_str(MsgTri);					// �O�p�g�\��
}

///// ���l���當����ɕϊ�
void ltostring(char digit, unsigned long data, byte *buffer)
{
	char i;
	buffer += digit;					// ������̍Ō�
	for(i=digit; i>0; i--) {			// �ŉ��ʌ������ʂ�
		buffer--;					// �|�C���^�[�P
		*buffer = (data % 10) + '0';	// ���̌����l�𕶎��ɂ��Ċi�[
		data = data / 10;				// ��-1
	}
}

    
