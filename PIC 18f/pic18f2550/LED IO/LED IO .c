/************************************************************
�@�@MPLAB-C18�ɂ����o�̓|�[�g�̐���e�X�g�v���O����
�@�@�X�C�b�`�̓��͂��|�[�gD����s���A�X�C�b�`�̏�Ԃ�
�@�@�]���āA�w�肳�ꂽ�����_�C�I�[�h��_�ł�����B
�@�@�@�@RD0�̃X�C�b�`��OFF�̎�LED�P
�@�@�@�@RD1�̃X�C�b�`��OFF�̎�LED2
�@�@�@�@RD2�̃X�C�b�`��OFF�̎�LED3
�@�@�@�@�S�X�C�b�`��ON�̎��SLED
*************************************************************/
#include <p18f2550.h>    // PIC18C452�̃w�b�_�E�t�@�C��
#include <delays.h>
#include <usart.h>

//***** �R���t�B�M�����[�V�����̐ݒ�config


		#pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
		#pragma config CPUDIV   = OSC1_PLL2   
		#pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
		#pragma config FOSC     = HSPLL_HS
		#pragma config FCMEN    = OFF
		#pragma config IESO     = OFF
		#pragma config PWRT     = OFF
		#pragma config BOR      = OFF		//ADD
		#pragma config BORV     = 3
	//	#pragma config VREGEN   = ON      //USB Voltage Regulator
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
		
//---Wait---//
//48Mhz�쓮�̎�
#define WAIT_MS Delay1KTCYx(12)// Wait 1ms
#define WAIT_US Delay10TCYx(12)	// Wait 1us

//=======wait[ms]======//
void wait_ms(unsigned long int t) {
	while(t--) {
		WAIT_MS;
	}
}
//***** ���C���֐�
void main(void)                     // ���C���֐�
{
	
	//������
		TRISA=0x00;                        // �|�[�gA�����ׂďo�̓s���ɂ���
		TRISB=0x0F;                        // �|�[�gB�����ׂďo�̓s���ɂ���
		TRISC=0xFF;                        // �|�[�gC�����ׂďo�̓s���ɂ���
	
	
		PORTB = 0x00;
	wait_ms(1000);
	PORTB = 0xFF;
	wait_ms(1000);
	PORTB = 0x00;
	wait_ms(1000);
	PORTB = 0xFF;
	wait_ms(1000);
	PORTB = 0x00;
	
	
	while(1)
	{
		if(PORTBbits.RB0)				//�X�C�b�`��OFF�̂Ƃ�
		{
		PORTB = 0xFF;					//LED OFF
		}
		else{							//�X�C�b�`��ON�̂Ƃ�
		PORTB = 0x00;					//LED ON
		}
		
	}
}

