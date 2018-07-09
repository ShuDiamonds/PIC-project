/*********************************************************************
*
*�@USB�ڑ� �t���J���[�Ɩ�
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
#include "stdlib.h"

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
char input_buffer[64];					// USB���̓o�b�t�@
char output_buffer[64];					// USB�o�̓o�b�t�@

struct PWMCont
{
	char	UpDown;						// �㏸���~�t���O
	unsigned int Value;					// PWM�l
};
struct PWMCont PWM[6];					// 6�F�� 

unsigned int		Period;					// PWM����
unsigned int		Count;
unsigned int		Interval;				// �F�X�V����
char 			Flag;					// �F�X�V�t���O
char			Color;					// �F���l
char			ColorIndex = 0;			// �F���l�擾�C���f�b�N�X
unsigned int		Max = 1023;				// �F�X���E�l�ݒ�
unsigned int		Min = 1;
unsigned int		ChangeInterval = 10;		// �F���X�V����
char			ChangeFlag = 0;			// �F���ύX�w���t���O 
unsigned int		Random;

unsigned char MsgOK[3] = "OK";

/** P R I V A T E  P R O T O T Y P E S *******************************/
void ISRProcess(void);
void Illuminate(void);
unsigned char EERead(unsigned char adrs);
void EEWrite(unsigned char adrs, unsigned char data);
void InitEEPROM(void);
void Setup(void);

/** V E C T O R  M A P P I N G ***********************************/
#pragma code high_vector=0x08
void interrupt_at_high_vector(void)
{
    _asm goto ISRProcess _endasm
}
#pragma code

////// Timer0 Interrupt Process
////// 20msec Interval
#pragma interrupt ISRProcess
void ISRProcess(void)
{
	INTCONbits.TMR0IF = 0;							// Interrupt Flag Clear
	TMR0H = 0xF1;									// 
	TMR0L = 0x59;	
	if(Interval > 0)								// PWM�X�V�������H
		Interval--;								// PWM�X�V����-1	
	else	
	{
		Interval = (unsigned int)EERead(2)*256 + (unsigned int)EERead(3);
		Flag = 1;
		// �F���X�V
		if(ChangeInterval > 0)						// �F���X�V�������H
	  		ChangeInterval--;						// 			
		else
		{
			ChangeInterval = (unsigned int)EERead(0xA)*256 + (unsigned int)EERead(0xB);
			ChangeFlag = 1;
		}
	}
}
	
#pragma code
/*********************************************************************
 * Function:        void main(void)
 ********************************************************************/
void main(void)
{
	char i;
	
	CMCON = 0x07;						// �R���p���[�^�I�t
   	LATB = 0;						// LED�S����
  	LATC  = 0x03;						// LED off
 	TRISA = 0x03;						// 
	TRISB = 0x0;						// �t���\��
 	TRISC = 0x30;						// LED Output Mode
	// Timer0 ������
	T0CON = 0x85;						// Internal 1/64
	TMR0H = 0xF1;						// 12MHz/64/3750=50Hz
	TMR0L = 0x59;
 	// A/D�����ݒ�
 	ADCON1 = 0x0B;					// AN0-AN1 Analog Input
 	ADCON2 = 0x95;					// 4Tad, Fosc/16
 	ADCON0 = 0x01;					// AD on
	/// USB������
	mInitializeUSBDriver();         	// See usbdrv.h
	/// EEPROM�f�t�H���g�ݒ�i�f�o�b�O�p�j
//	InitEEPROM();
	/// �ϐ������� EEPROM����ǂݏo��
	Setup();
	/// ���荞�݋���
	INTCONbits.TMR0IE = 1;
	INTCONbits.GIE = 1;
    /// ���C�����[�v
    while(1)
    {
    		USBCheckBusStatus();        					// USB�ڑ��`�F�b�N
    		if(UCFGbits.UTEYE!=1)							// �A�C�p�^�[�����[�h�I�t��
        		USBDriverService();						// USB�C�x���g�|�[�����O
		CDCTxService();								// �N���X����M���s
		/// ��M�f�[�^�̏������s
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
    			if(getsUSBUSART(input_buffer,64))			// �f�[�^��M�|�[��
    			{
            		LATCbits.LATC0 = !LATCbits.LATC0;		// LED���]
	    			switch(input_buffer[0])				// �ŏ��̂P�����`�F�b�N
	    			{
		    			case '0':						// OK����
						if(mUSBUSARTIsTxTrfReady())
							putrsUSBUSART("OK");
          			 	break;
					case '1':
						for(i=0; i<0x1F; i++)			// Write except Random
						{
							EEWrite(i, input_buffer[i+1]);
							Setup();
						}
						break;
					case '9':
		    				InitEEPROM();
		    				Setup();				
						break;						
		    			default: break;
		    		}
    			}
    		}
   		/// PWM����iCount���{�P��Max�ōŏ��ɖ߂�j 		
    		Count++;										// PWM�X�e�b�v�A�b�v
    		if(Count == Period)							// �����I��肩�H
    		{ 
	    		Count = 0;								// �����ŏ��ɖ߂�
	    		Period = (unsigned int)EERead(0)*256 + (unsigned int)EERead(1);
  			LATB = 0x3F;								// �SLED�I��
  			// PWM���X�V�����Ȃ�PWM�X�V�iInterval)
			if(Flag)									// �X�V�������H
			{
				Flag = 0;							// �F�X�V�t���O�I�t
				Illuminate();							// �C���~�l�[�V�������s
			}
 		}
		//// PWM�I�t����@�iCount��PWM�l����v�j
		if(Count == PWM[0].Value)
			LEDR1 = 0;
		if(Count == PWM[1].Value)
			LEDG1 = 0;
		if(Count == PWM[2].Value)
			LEDB1 = 0;
		if(Count == PWM[3].Value)
			LEDR2 = 0;
		if(Count == PWM[4].Value)
			LEDG2 = 0;
		if(Count == PWM[5].Value)
			LEDB2 = 0;
     }
}

//// �C���~�l�[�g���s�֐�
void Illuminate(void)
{
	char i, Mask;
	
	Mask = 1;
	for(i=0; i<6; i++) {
		if(Color & Mask) {							// �F�w�肠�肩�H
			if(PWM[i].UpDown) {						// �㏸�����H
				PWM[i].Value++;						// PWM���A�b�v
				if(PWM[i].Value >= Max) {				// �ő�l�܂œ��B���H
					PWM[i].Value = Max;				// �ő�l�Ő���
					PWM[i].UpDown = 0;				// ���~�ɕύX
				}
			}
			else {									// PWM���~��
				PWM[i].Value--;						// PWM�l�_�E��
				if(PWM[i].Value <= Min) {				// �ŏ��l�܂ŒB�������H
					PWM[i].Value = Min;				// �ŏ��l�Ő���
					PWM[i].UpDown = 1;				// �㏸�ɕύX
					if(ChangeFlag == 1) {				// �F���ύX�w�����肩�H
						ChangeFlag = 0;				// �w���N���A
						ColorIndex++;					// �F���C���f�b�N�X�X�V
						Color = EERead(ColorIndex);	// �F���擾
						if((Color == 0) ||(ColorIndex > 0x20))
							Setup();					// �F��������				
					}								
				}
			}
		}
		else										// �F�w�薳���̏ꍇ
			PWM[i].Value = Min;						// MIN�l�ɐݒ�
		Mask = Mask * 2;								// �F�w��r�b�g�}�X�N�X�V
	}
}	

//// EEPROM����p�����[�^�ݒ�i�����X�^�[�g���j
void Setup(void)
{
	char i;
	
	Period = (unsigned int)EERead(0)*256 + (unsigned int)EERead(1);
	Interval =(unsigned int)EERead(2)*256 + (unsigned int)EERead(3);
	Max = (unsigned int)EERead(4)*256 + (unsigned int)EERead(5);
	if(Max > Period) {
		Max = Period;
		EEWrite(4, (unsigned char)(Max / 256));		// �C��
		EEWrite(5, (unsigned char)(Max % 256));
	}
	Min = (unsigned int)EERead(8)*256 + (unsigned int)EERead(9);
	if((Min < 1) || (Min > 255)) {
		Min = 1;
		EEWrite(8, (unsigned char)(Min / 256));		// �C��
		EEWrite(9, (unsigned char)(Min % 256));
	}		
	ChangeInterval = (unsigned int)EERead(0x0A)*256 + (unsigned int)EERead(0x0B);
	ColorIndex = 0x10;
	Color = EERead(ColorIndex);
	Count = 0;
	/// PWM�����l�����_���ݒ�i�L���l����ݒ�j
	Random = (unsigned int)EERead(0x30)*256 + (unsigned int)EERead(0x31);
	Random = Random + 10;								// �����_����X�V
	if(Random > Period-10)
		Random = 10;
	EEWrite(0x30, (unsigned char)(Random / 256));		// �����_����L��
	EEWrite(0x31, (unsigned char)(Random % 256));	
	for(i=0; i<6; i++)
	{
		srand(Random++);								// PWM�l�����_���ݒ�
		PWM[i].Value = rand() >> 6;
//		PWM[i].Value = 1;							// for Debug
		PWM[i].UpDown = 0;
	}
}
	
/// EEPROM�������݊֐�
void EEWrite(unsigned char adrs, unsigned char data)
{	
 	EECON1bits.EEPGD = 0;
 	EECON1bits.CFGS = 0;
  	EEADR = adrs;
  	EEDATA = data;
  	EECON1bits.WREN = 1;
  	EECON2 = 0x55; 				// �������݃V�[�P���X  
  	EECON2 = 0xaa;  
  	EECON1bits.WR = 1; 
  	while (EECON1bits.WR);
  	PIR2bits.EEIF = 0;
}
/// EEPROM�ǂݏo���֐�
unsigned char EERead(unsigned char adrs)
{
  	EECON1bits.EEPGD = 0; 
	EECON1bits.CFGS = 0;  	
  	EEADR = adrs;
  	EECON1bits.RD = 1;  
  	return(EEDATA); 
}

///// EEPROM�f�t�H���g�l�ݒ�
void InitEEPROM(void)
{
	EEWrite(0, 0x03);				// Period
	EEWrite(1, 0xFF);
	EEWrite(2, 0x00);				// Interval
	EEWrite(3, 0x01);
	EEWrite(4, 0x01);				// Max
	EEWrite(5, 0xFF);
	EEWrite(6, 0x00);				// 
	EEWrite(7, 0xFF);
	EEWrite(8, 0x00);				// Min
	EEWrite(9, 0x01);
	EEWrite(0x0A, 0x04);			// ChangeInterval
	EEWrite(0x0B, 0xFF);
	EEWrite(0x10, 0x09);			// pattern0
	EEWrite(0x11, 0x12);			// pattern1
	EEWrite(0x12, 0x24);			// pattern2
	EEWrite(0x13, 0x1B);			// pattern3
	EEWrite(0x14, 0x1E);			// pattern4
	EEWrite(0x15, 0x36);			// pattern5
	EEWrite(0x16, 0x2D);			// pattern6
	EEWrite(0x17, 0x3F);
	EEWrite(0x18, 0x21);
	EEWrite(0x19, 0x14);
	EEWrite(0x1A, 0x0C);
	EEWrite(0x1B, 0x33);
	EEWrite(0x1C, 0);
	EEWrite(0x1D, 0);
	EEWrite(0x1E, 0);
	EEWrite(0x1F, 0);
}
			

/******** EEPROM Memory Map
	 0 : Period High
	 1 : Period Low
	 2 : Interval High
	 3 : Interval Low
	 4 : Max High
	 5 : Max Low
	 6 : 
	 7 : 
	 8 : Min High
	 9 : Min Low
	 A : ChangeInterval High
	 B : ChangeInterval Low
	 C : 
	 D : 
	 E :
	 F :	
	10 : pattern[0]
	11 : pattern[1]
	12 : pattern[2]
	13 : pattern[3]
	14 : pattern[4]
	-------
	1F : pattern[16]
	30 : Random High
	31 : Random Low
*************************************/
