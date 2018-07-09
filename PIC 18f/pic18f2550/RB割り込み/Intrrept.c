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
//#include "Interruptlib.h"

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
        
/********�֐��v���g�^�C�v************/
void Timer0_isr(void);
void Timer1_isr(void);
void RB_PORT_isr(void);
void RB0_isr(void);
void RB1_isr(void);
void RB2_isr(void);
//********************************************************
//***** �ϐ��A�萔�̒�`
unsigned char cnt=4;            // cnt,cnt1��LED�X�V�����p�J�E���^
unsigned char cnt1=5;
//****** ���C���֐�
void main(void)                 // ���C���֐�
{
	TRISB=0xFF;
    TRISC=0;                    // �|�[�gC�����ׂďo�̓s���ɂ���
	PORTC = 0xFF;
	
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
//***********���荞�ݐݒ�***********************
	
//************RB0���荞�ݐݒ�********
	//RB0/INT �͊��荞�݂ɗD�揇�ʂ͂Ȃ����ʂ̂Ɋ��荞�݂�������
	INTCONbits.INT0IE=1;		//RB0/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
	INTCON2bits.INTEDG0=0;			//RB0/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)	
	
//************RB1���荞�ݐݒ�*********
	INTCON3bits.INT1IP = 0;		//RB1/INT ���ʊ��荞�݂ɂ���
	INTCON3bits.INT1IE = 1;		//RB1/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
	INTCON2bits.INTEDG1 = 0;			//RB1/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)

//************RB2���荞�ݐݒ�*********
	INTCON3bits.INT2IP = 0;			//RB2/INT ���ʊ��荞�݂ɂ���
	INTCON3bits.INT2IE = 1;		//R2/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
	INTCON2bits.INTEDG2 = 0;	//RB2/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)

//************RBPORT���荞�ݐݒ�*********
	INTCON2bits.RBIP = 0;		//RBPORT/INT ���ʊ��荞�݂ɂ���
	INTCONbits.RBIE = 1;		//RB2/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
	
	
	
	
	//***** �����݋���
    INTCONbits.GIEH=1;          // �����x������
    INTCONbits.GIEL=1;          // �჌�x������
	
	

	
	//***** ���C�����[�v�i�A�C�h�����[�v�j
    while(1)    
    {
    	/*
        PIN_LED_0 = 1;
        Delay10KTCYx(100);
       PIN_LED_0 = 0;
        Delay10KTCYx(100);
    */
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
	INTCONbits.GIEH=0;          // �����x���s����
	if(INTCONbits.T0IF)	Timer0_isr();       // �^�C�}0���荞�݁H
	if(PIR1bits.TMR1IF)	Timer1_isr();		 // �^�C�}�P���荞�݁H
	if(INTCONbits.INT0IF)	RB0_isr();		 // �O�������݂O���荞�݁H
	if(INTCON3bits.INT1IF)	RB1_isr();		 // �O�������݂P���荞�݁H
	if(INTCON3bits.INT2IF)	RB2_isr();		 // �O�������݂Q���荞�݁H
	if(INTCONbits.RBIF)	RB_PORT_isr();		 // �O��������PORT�ω����荞�݁H
	INTCONbits.GIEH=1;          // �����x������
	
}                                   
//***** �჌�x�������ݏ����֐�
void Low_isr(void)                     // ���荞�݊֐�
{
	INTCONbits.GIEL=0;          // �჌�x���s����
	if(INTCONbits.T0IF)	Timer0_isr();       // �^�C�}0���荞�݁H
	if(PIR1bits.TMR1IF)	Timer1_isr();		 // �^�C�}�P���荞�݁H
	if(INTCONbits.INT0IF)	RB0_isr();		 // �O�������݂O���荞�݁H
	if(INTCON3bits.INT1IF)	RB1_isr();		 // �O�������݂P���荞�݁H
	if(INTCON3bits.INT2IF)	RB2_isr();		 // �O�������݂Q���荞�݁H
	if(INTCONbits.RBIF)	RB_PORT_isr();		 // �O��������PORT�ω����荞�݁H
   // INTCONbits.GIEL=1;          // �჌�x������
	
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
/***********���荞�݊֐����s��*********/

//**********Timer0���荞��*************
void Timer0_isr(void)
{
	INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
	if(--cnt==0)
	{               // cnt��-1���Č��ʂ�0�H
		cnt=4;                  // cnt��LED�̍X�V�����������߂�
		if(PIN_LED_1)
			PIN_LED_1=0;    //LED2��0.1�b�Ԋu�œ_��
		else
			PIN_LED_1=1;
	}
}
//**********Timer1���荞��*************
void Timer1_isr(void)
{
		PIR1bits.TMR1IF=0;          // �^�C�}�P���荞�݃t���O��0�ɂ���
		if(--cnt1==0)
		{              // cnt1��-1���Č��ʂ�0�H
			cnt1=5;                 // cnt1��LED�̍X�V�����������߂�
			if(PIN_LED_2)
			PIN_LED_2=0;    //LED3���P�b�Ԋu�œ_��
			else
			PIN_LED_2=1;
		}
}
//**********RB0���荞��*************
void RB0_isr(void)
{
	INTCONbits.INT0IF = 0;		//���荞�݃t���O�N���A
	PIN_LED_0=0;    //LED3���P�b�Ԋu�œ_��
	//wait_ms(1000);
	//PIN_LED_1=1;
}
//**********RB1���荞��*************
void RB1_isr(void)
{
	INTCON3bits.INT1IF = 0;		//���荞�݃t���O�N���A
	PIN_LED_1=0;    //LED3���P�b�Ԋu�œ_��
	//wait_ms(1000);
	//PIN_LED_2=1;
}
//**********RB2���荞��*************
void RB2_isr(void)
{
	INTCON3bits.INT2IF = 0;		//���荞�݃t���O�N���A
	PIN_LED_2=0;    //LED3���P�b�Ԋu�œ_��
	//wait_ms(1000);
	//PIN_LED_2=1;
}
//**********RB PORT�ω����荞��*************
void RB_PORT_isr(void)
{
	int PORT_data = 0;
	PORT_data = PORTB;
	INTCONbits.RBIF = 0;		//���荞�݃t���O�N���A
	
}


