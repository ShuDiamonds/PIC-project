/*********************************************************
	MPLAB-C18�e�X�g�v���O�����@No8
	�����݂̃e�X�gNo1
	�@�D�揇�ʂ��g�������������݂̃e�X�g
	�@�@�����x���F�^�C�}�O�@�@�჌�x���F�^�C�}�P
	�@�\
	�@�@�A�C�h�����[�v�F�O�D�T�b�Ԋu��LED1��_��
	�@�@�^�C�}�O�@�@�@�F�O�D�P�b�Ԋu��LED2��_��
	�@�@�^�C�}�P�@�@�@�F�P�b�Ԋu��LED3��_��
********************************************************/�@
#include <p18f2550.h>            // PIC18f2550�̃w�b�_�E�t�@�C��
#include <timers.h>             // �^�C�}�֐��̃w�b�_�E�t�@�C��
#include <delays.h>
#include "HardwareProfile.h"

//***** �R���t�B�M�����[�V�����̐ݒ�config

        #pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
        #pragma config CPUDIV   = OSC1_PLL2   
        #pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
        #pragma config FOSC     = HSPLL_HS
        #pragma config FCMEN    = OFF
        #pragma config IESO     = OFF
        #pragma config PWRT     = OFF
        #pragma config BOR      = ON
        #pragma config BORV     = 3
        #pragma config VREGEN   = ON      //USB Voltage Regulator
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
        
//********************************************************
//***** �ϐ��A�萔�̒�`
unsigned char cnt=4;            // cnt,cnt1��LED�X�V�����p�J�E���^
unsigned char cnt1=5;
//****** ���C���֐�
void main(void)                 // ���C���֐�
{
    TRISC=0;                    // �|�[�gC�����ׂďo�̓s���ɂ���

    OpenTimer0(TIMER_INT_ON & T0_8BIT & T0_SOURCE_INT & T0_PS_1_256);
                                // �^�C�}0�̐ݒ�, 8�r�b�g���[�h, �����ݎg�p 
                                //�����N���b�N�A1:256�v���X�P�[��
    OpenTimer1(TIMER_INT_ON & T1_8BIT_RW & T1_SOURCE_INT & T1_PS_1_8 & 
            T1_OSC1EN_OFF);     //�^�C�}�P�̐ݒ�,8�r�b�g���[�h�A�����ݎg�p
                                //�����N���b�N�A1:8�v���X�P�[��
//***** �D�揇�ʊ����ݎg�p�錾
    RCONbits.IPEN=1;
//***** �჌�x���g�p���ӂ̒�`
    IPR1bits.TMR1IP=0;
//***** �����݋���
    INTCONbits.GIEH=1;          // �����x������
    INTCONbits.GIEL=1;          // �჌�x������

//***** ���C�����[�v�i�A�C�h�����[�v�j
    while(1)    
    {
        PIN_LED_0 = 1;
        Delay10KTCYx(100);
       PIN_LED_0 = 0;
        Delay10KTCYx(100);
    }
}

//****************************************************
//****** �����݂̐錾�@�D�揇�ʎg�p
#pragma interrupt High_isr save = PROD
#pragma interruptlow Low_isr save = WREG,BSR,STATUS,PROD

//***** �����݃x�N�^�W�����v���߃Z�b�g
#pragma code isrcode = 0x8
void isr_direct(void)
{
    _asm
    goto High_isr
    _endasm
}
#pragma code lowcode = 0x18
void low_direct(void)
{   _asm
    goto Low_isr
    _endasm
}
//**** �����x���@�����ݏ����֐�
#pragma code
void High_isr(void)                      // ���荞�݊֐�
{
    if(INTCONbits.T0IF){            // �^�C�}0���荞�݁H
        INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
        if(--cnt==0){               // cnt��-1���Č��ʂ�0�H
            cnt=4;                  // cnt��LED�̍X�V�����������߂�
            if(PIN_LED_1)
                PIN_LED_1=0;    //LED2��0.1�b�Ԋu�œ_��
            else
               PIN_LED_1=1;
        }
    }
}                                   
//***** �჌�x�������ݏ����֐�
void Low_isr(void)                     // ���荞�݊֐�
{
    if(PIR1bits.TMR1IF){            // �^�C�}�P���荞�݁H
        PIR1bits.TMR1IF=0;          // �^�C�}�P���荞�݃t���O��0�ɂ���
        if(--cnt1==0){              // cnt1��-1���Č��ʂ�0�H
            cnt1=5;                 // cnt1��LED�̍X�V�����������߂�
            if(PIN_LED_2)
                PIN_LED_2=0;    //LED3���P�b�Ԋu�œ_��
            else
                PIN_LED_2=1;
        }
    }
}                       




/************************************************************/

//---Wait---//
//48Mhz�쓮�̎�
#define WAIT_MS Delay1KTCYx(12)// Wait 1ms
#define WAIT_US Delay10TCYx(12)	// Wait 1us

//=======wait[ms]======//
void wait_ms(int t) {
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[us]======//
void wait_us(int t) {
	while(t--) {
		WAIT_US;
	}
}




