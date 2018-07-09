/*********************************************************************
*
*�@USB�ڑ� ���g���J�E���^�v���O����
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
char input_buffer[64];					// USB���̓o�b�t�@
char output_buffer[64];					// USB�o�̓o�b�t�@

char Prescale;							// �v���Z�[���t���O
char EndFlag = 0;							// �J�E���g�I���t���O
char SecCounter;							// 1�b�J�E���g
unsigned long FreqHigh;					// ���g���J�E���^���
unsigned long dumy;						// ���g���v�Z�p���[�N


char MsgFreq[] = "Freq=         Hz";		// �t���\���p���b�Z�[�W
char MsgSend[] = "        \r\n";			// USB���M�p���b�Z�[�W
char MsgStart[] ="Start!";					// �������b�Z�[�W
char MsgPre8[] = "Scale1/8Max50MHz";		// �v���X�P�[���ݒ�\��
char MsgPre1[] = "Scale1/1Max10MHz";
char MsgAns[] = " ";


/** P R I V A T E  P R O T O T Y P E S *******************************/
void Display(unsigned long Freq);
void ltostring(char digit, unsigned long data, char *buffer);
void ISRProcess(void);

/** V E C T O R  M A P P I N G ***********************************/
#pragma code high_vector=0x08
void interrupt_at_high_vector(void)
{
    _asm goto ISRProcess _endasm
}
#pragma code

////// CCP1 Interrupt Process
#pragma interrupt ISRProcess
void ISRProcess(void)
{
	PIR1bits.CCP1IF = 0;				// Interrupt Flag Clear		
	SecCounter--;
	if(SecCounter == 0)
	{
 		GreenLED = !GreenLED;
		T0CONbits.TMR0ON = 0;			// Stop Count
		T1CONbits.TMR1ON = 0;
		EndFlag = 1;					// Count End Flag
	}
}



#pragma code
/*********************************************************************
 * Function:        void main(void)
 ********************************************************************/
void main(void)
{
	int i;
	
	ADCON1 |= 0x0F;    	             	// �S�f�B�W�^���ɐݒ�
	CMCON = 0x07;						// �R���p���[�^�I�t
   	LATB = 0;						// �t���\���|�[�g���Z�b�g
  	LATC  = 0xC0;						// LED off
 	TRISA = 0x10;						// T0CKI
	TRISB = 0x0;						// �t���\��
 	TRISC = 0x31;						// LED Output Mode
 	// �^�C�}������
 	T1CON = 0xB2;						// ExtClock 1/8
 	TMR1H = 0;						// 64000 Count
 	TMR1L = 0;
	T0CON = 0x22;						// Ext Counter 1/8
	TMR0H = 0;
	TMR0L = 0;
	T3CON = 0;						// Timer1 for CCP1
 	// CCP1������
 	CCP1CON = 0x0B;					// Timer Clear, Interrupt
	CCPR1 = 64000;					// 12.8MHz��8��64000=25Hz					
	/// USB������
	mInitializeUSBDriver();         	// See usbdrv.h
	/// �t���\���평����
	lcd_init();						// LCD������
	lcd_str(MsgStart);				// �������b�Z�[�W�\��
	Delay10KTCYx(500);				// 0.5�b
	/// �ϐ�������
	Prescale = 0;						// default 1/1
	FreqHigh = 0;
	EndFlag = 1;
	// Enable Interrupt
	RCONbits.IPEN = 0;				// No Priority
 	PIE1bits.CCP1IE = 1;				// Enable CCP1		
	INTCON = 0xC0;					// Enable Interrupt
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
    			if(getsUSBUSART(input_buffer,64))			// �f�[�^��M�|�[��
    			{
	    			switch(input_buffer[0])				// �ŏ��̂P�����`�F�b�N
	    			{
		    			case '0':						// OK����
         				if(mUSBUSARTIsTxTrfReady())
           			 		putrsUSBUSART("OK");
           			 	break;
           			case '1':
           				Prescale = 0;
           				MsgAns[0] = '0';
         				if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam((byte*)MsgAns, 1);           				
           				break;
           			case '2':
           				Prescale = 1;
           				MsgAns[0] = '1';
         				if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam((byte*)MsgAns, 1);            				
           				break;		    				
					case '3':
         				if(mUSBUSARTIsTxTrfReady())
           			 		mUSBUSARTTxRam((byte*)MsgSend, 10);
           			 	break;					

		    			default: break;
		    		}
    			}
    		}
		//// ���g���J�E���^�@�\���s
		if(INTCONbits.TMR0IF)				// �^�C�}0�I�[�o�[�t���[
 		{
	 		INTCONbits.TMR0IF = 0;		
 			FreqHigh++;					// �J�E���^�ϐ��{�P
 		}		
    		if(EndFlag == 1)					// �P�b�Q�[�g�I�����H
    		{
	    		/// ���g���v�Z
	    		dumy = (unsigned long)TMR0L;	
			dumy = dumy + (unsigned long)TMR0H*256;
			if(Prescale)
				dumy = 8*(FreqHigh*65536 + dumy);			
			else
				dumy = FreqHigh*65536 + dumy;
			// ���g���\��			
	    		Display(dumy);
			// ���̌v������
			if(Prescale)
			{
				T0CON = 0x22;				// 1/8
				RedLED = 1;				// LED Off
			}
			else
			{
				T0CON = 0x28;				// 1/1
				RedLED = 0;				// LED On
			}
	    		EndFlag = 0;
	    		TMR0H = 0;					// reset counter
	    		TMR0L = 0;
			FreqHigh = 0;
	    		SecCounter = 25;				// Reset 1 sec counter
	    		TMR1H = 0;					// Reset Timer1
			TMR1L = 0;
			// ���̌v���J�n
			T1CONbits.TMR1ON = 1;
	    		T0CONbits.TMR0ON = 1;			// Start Count
	    }		
     }
}

/// �f�[�^�\���֐�
void Display(unsigned long Freq) 
{
	//// ���g���ϕ��\��
	lcd_cmd(0x80);						//�P�s�ڂ�
	ltostring(8, Freq, &MsgFreq[0]+6);		// ���g���\��
	ltostring(8, Freq, &MsgSend[0]);
	lcd_str((char *)MsgFreq);				// ���o���\��
	lcd_cmd(0xC0);						// 2�s�ڂ�
	if(Prescale)
		lcd_str(MsgPre8);
	else
		lcd_str(MsgPre1);
}

///// ���l���當����ɕϊ�
void ltostring(char digit, unsigned long data, char *buffer)
{
	char i;
	buffer += digit;					// ������̍Ō�
	for(i=digit; i>0; i--) {			// �ŉ��ʌ������ʂ�
		buffer--;					// �|�C���^�[�P
		*buffer = (data % 10) + '0';	// ���̌����l�𕶎��ɂ��Ċi�[
		data = data / 10;				// ��-1
	}
}

    
