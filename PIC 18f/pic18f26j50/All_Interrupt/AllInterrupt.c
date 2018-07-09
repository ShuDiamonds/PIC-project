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
#include <p18f26j50.h>            // PIC18f2550�̃w�b�_�E�t�@�C��
#include <timers.h>             // �^�C�}�֐��̃w�b�_�E�t�@�C��
#include <delays.h>
#include <adc.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include "HardwareProfile.h"

//***** �R���t�B�M�����[�V�����̐ݒ�config

	#pragma config WDTEN = OFF          //WDT disabled (enabled by SWDTEN bit)
	#pragma config PLLDIV = 2           //Divide by 2 (8 MHz intl osc input)
	#pragma config STVREN = ON          //stack overflow/underflow reset enabled
	#pragma config XINST = OFF          //Extended instruction set disabled
	#pragma config CPUDIV = OSC1        //No CPU system clock divide
	#pragma config CP0 = OFF            //Program memory is not code-protected
	#pragma config OSC = INTOSCPLL      //Internal oscillator, PLL enenabled
	#pragma config T1DIG = OFF          //S-Osc may not be selected, unless T1OSCEN = 1
	#pragma config LPT1OSC = OFF        //high power Timer1 mode
	#pragma config FCMEN = OFF          //Fail-Safe Clock Monitor disabled
	#pragma config IESO = OFF           //Two-Speed Start-up disabled
	#pragma config WDTPS = 32768        //1:32768
	#pragma config DSWDTOSC = INTOSCREF //DSWDT uses INTOSC/INTRC as clock
	#pragma config RTCOSC = T1OSCREF    //RTCC uses T1OSC/T1CKI as clock
	#pragma config DSBOREN = OFF        //Zero-Power BOR disabled in Deep Sleep
	#pragma config DSWDTEN = OFF        //Disabled
	#pragma config DSWDTPS = 8192       //1:8,192 (8.5 seconds)
	#pragma config IOL1WAY = OFF        //IOLOCK bit can be set and cleared
	#pragma config MSSP7B_EN = MSK7     //7 Bit address masking
	#pragma config WPFP = PAGE_1        //Write Protect Program Flash Page 0
	#pragma config WPEND = PAGE_0       //Start protection at page 0
	#pragma config WPCFG = OFF          //Write/Erase last page protect Disabled
	#pragma config WPDIS = OFF          //WPFP[5:0], WPEND, and WPCFG bits ignored 


/********�֐��v���g�^�C�v************/
void wait_ms(int t);
void wait_us(int t);
void Timer0_isr(void);
void Timer1_isr(void);
void Timer2_isr(void);
void Timer3_isr(void);
void RB_PORT_isr(void);
void RB0_isr(void);
void RB1_isr(void);
void RB2_isr(void);
void AD_isr(void);
void UART_TX_isr(void);
void UART_RC_isr(void);
//********************************************************
//***** �ϐ��A�萔�̒�`
unsigned char cnt=4;            // cnt,cnt1��LED�X�V�����p�J�E���^
unsigned char cnt1=5;
unsigned char cnt2=6;
unsigned char cnt3=7;
unsigned char cnt4=20;
unsigned char cnt5=111;
unsigned char cnt6=90;
char temp_Buf[20] = {0};
char Send_Buf[20] = {0};
//****** ���C���֐�
void main(void)                 // ���C���֐�
{
	//���[�J���ϐ���`
	long int data = 0;
	char Message1[10]="\rStart!!\n";
	char Message2[7]="FUKUDA";
	float DATA=0;
	
	//�N���b�N������
		//48Mhz�̐ݒ�
	OSCTUNEbits.PLLEN = 1;        // PLL���N��
	wait_ms(2);
	//�|�[�g������
	TRISA=0x0F;
	TRISB=0x0F;
    TRISC=0;                    // �|�[�gC�����ׂďo�̓s���ɂ���
	PORTC = 0xFF;
	//----UART������-------//
	Open1USART(USART_TX_INT_ON & USART_RX_INT_ON & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 64); 	//���Ӂ@OpenUSART�@�֐��̍Ō�̕����̓{�[���[�g�ݒ�ō���40Mhz��9600
	
	//----Timer������------//
	OpenTimer0(TIMER_INT_ON & T0_8BIT & T0_SOURCE_INT & T0_PS_1_256);
								// �^�C�}0�̐ݒ�, 8�r�b�g���[�h, �����ݎg�p 
								//�����N���b�N�A1:256�v���X�P�[��
/*	OpenTimer1(TIMER_INT_ON & T1_8BIT_RW & T1_SOURCE_INT & T1_PS_1_8 & 
	        T1_OSC1EN_OFF);     //�^�C�}�P�̐ݒ�,8�r�b�g���[�h�A�����ݎg�p
	                            //�����N���b�N�A1:8�v���X�P�[��
*/
	OpenTimer2( 
		TIMER_INT_ON &
		T2_PS_1_4 &
		T2_POST_1_8 );
	
/*	OpenTimer3(TIMER_INT_ON & T3_SOURCE_INT &  T3_SYNC_EXT_ON & 
            T3_SOURCE_CCP);	
*/
	//-----AD������--------//
	OpenADC(ADC_FOSC_64 &           //AD�ϊ��p�N���b�N�@�@�V�X�e���N���b�N��1/64�@0.05��sec�~64��3.2��sec�@>=�@1.6��sec�@���@OK
				ADC_RIGHT_JUST &        //�ϊ����ʂ̕ۑ����@�@���l�߁@
				ADC_8_TAD,              //AD�ϊ��̃A�N�C�W�V�����^�C���I���@3.2��sec�i=1Tad�j�~8Tad=25.6��sec�@�����@12.8��sec�@���@OK
				ADC_CH0 &                       //AD�ϊ�����̃`�����l���I���iPIC18F�͓����ɕ�����AD�ϊ��͂ł��Ȃ��j
				ADC_INT_ON &           //AD�ϊ��ł̊����ݎg�p�̗L��
				ADC_VREFPLUS_VDD &      //Vref+�̐ݒ�@�@�@�o�h�b�̓d���d���Ɠ����FADC_VREFPLUS_VDD �@or�@�O���i�`�m�R�j�̓d���FADC_VREFPLUS_EXT
				ADC_VREFMINUS_VSS,      //Vref-�̐ݒ�@�@�@�o�h�b��0�u�FADC_VREFMINUS_VSS    or�@�O���iAN2)�̓d���FADC_VREFMINUS_EXT
				0b1110  //�|�[�g�̃A�i���O�E�f�W�^���I���@�iADCON1�̉��ʂS�r�b�g���L�ځj�@�@AN0�̂݃A�i���O�|�[�g��I���A���̓f�W�^���|�[�g��I��
				//�� �@�A�i���O�|�[�g���@AN0�̂� �� 0b1110�@�@�AAN0 & AN1�@���@0b1011�A AN0 & AN1 & AN2 ��1100�@���@�ڍ׃f�[�^�V�[�g�Q��
		);
	
	

//***********���荞�ݐݒ�***********************
//***** �D�揇�ʊ����ݎg�p�錾
    RCONbits.IPEN=1;
//***** �჌�x���g�p���ӂ̒�`
    IPR1bits.TMR1IP=0;
//************Timer0�D�揇�ʐݒ�*****
	INTCON2bits.TMR0IP = 1;	//���ʊ��荞�݂ɐݒ�
//************Timer1�D�揇�ʐݒ�*****
	IPR1bits.TMR1IP = 1;	//���ʊ��荞�݂ɐݒ�
//************Timer2�D�揇�ʐݒ�*****
	IPR1bits.TMR2IP = 1;	//���ʊ��荞�݂ɐݒ�
//************Timer3�D�揇�ʐݒ�*****
	IPR2bits.TMR3IP = 1;	//���ʊ��荞�݂ɐݒ�

//************RB0���荞�ݐݒ�********
	//RB0/INT �͊��荞�݂ɗD�揇�ʂ͂Ȃ����ʂ̂Ɋ��荞�݂�������
	INTCONbits.INT0IE=0;		//RB0/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
	INTCON2bits.INTEDG0=0;			//RB0/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)	
	
//************RB1���荞�ݐݒ�*********
	INTCON3bits.INT1IP = 0;		//RB1/INT ���ʊ��荞�݂ɂ���
	INTCON3bits.INT1IE = 0;		//RB1/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
	INTCON2bits.INTEDG1 = 0;			//RB1/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)

//************RB2���荞�ݐݒ�*********
	INTCON3bits.INT2IP = 0;			//RB2/INT ���ʊ��荞�݂ɂ���
	INTCON3bits.INT2IE = 0;		//R2/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
	INTCON2bits.INTEDG2 = 0;	//RB2/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)

//************RBPORT���荞�ݐݒ�*********
	INTCON2bits.RBIP = 0;		//RBPORT/INT ���ʊ��荞�݂ɂ���
	INTCONbits.RBIE = 0;		//RB2/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
	
//************AD�ϊ����荞��*************
	IPR1bits.ADIP = 0;		//AD�ϊ����荞�݂��ʊ��荞�݂ɐݒ�
	PIE1bits.ADIE = 1;		//AD�ϊ����荞�݂�����
//************UART���M���荞��*************
	IPR1bits.TXIP = 0;		//AD�ϊ����荞�݂��ʊ��荞�݂ɐݒ�
	PIE1bits.TXIE= 1;		//AD�ϊ����荞�݂�����
//************UART��M���荞��*************
	IPR1bits.RCIP = 0;		//AD�ϊ����荞�݂��ʊ��荞�݂ɐݒ�
	PIE1bits.RCIE= 0;		//AD�ϊ����荞�݂�����
	
	//***** �����݋���
    INTCONbits.GIEH=1;          // �����x������
    INTCONbits.GIEL=1;          // �჌�x������
	
//	putsUSART(Message1);
	//***** ���C�����[�v�i�A�C�h�����[�v�j
    while(1)    
    {
    	SetChanADC(ADC_CH0);	//Select Channel 0
		Delay100TCYx(50);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		data = ReadADC();	//Get A/D data
    	fprintf(_H_USART,"\rData=%ld\n",data);	
    	
    	puts1USART(Message1);
    	
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
{	_asm
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
	if(PIR1bits.TMR2IF)	Timer2_isr();       //�^�C�}2���荞�݁H
	if(PIR2bits.TMR3IF)	Timer3_isr();		// �^�C�}3���荞�݁H
	if(INTCONbits.INT0IF)	RB0_isr();		 // �O�������݂O���荞�݁H
	if(INTCON3bits.INT1IF)	RB1_isr();		 // �O�������݂P���荞�݁H
	if(INTCON3bits.INT2IF)	RB2_isr();		 // �O�������݂Q���荞�݁H
	if(INTCONbits.RBIF)	RB_PORT_isr();		 // �O��������PORT�ω����荞�݁H
	if(PIR1bits.ADIF)	AD_isr();			 //AD���荞�݁H
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART���M���荞�݁H
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART��M���荞�݁H
	//INTCONbits.GIEH=1;          // �����x������
	
}                                   
//***** �჌�x�������ݏ����֐�
void Low_isr(void)                     // ���荞�݊֐�
{
	INTCONbits.GIEL=0;          // �჌�x���s����
	if(INTCONbits.T0IF)	Timer0_isr();       //�^�C�}0���荞�݁H
	if(PIR1bits.TMR1IF)	Timer1_isr();		//�^�C�}�P���荞�݁H
	if(PIR1bits.TMR2IF)	Timer2_isr();       //�^�C�}2���荞�݁H
	if(PIR2bits.TMR3IF)	Timer3_isr();		// �^�C�}3���荞�݁H
	if(INTCONbits.INT0IF)	RB0_isr();		 // �O�������݂O���荞�݁H
	if(INTCON3bits.INT1IF)	RB1_isr();		 // �O�������݂P���荞�݁H
	if(INTCON3bits.INT2IF)	RB2_isr();		 // �O�������݂Q���荞�݁H
	if(INTCONbits.RBIF)	RB_PORT_isr();		 // �O��������PORT�ω����荞�݁H
	if(PIR1bits.ADIF)	AD_isr();			 //AD���荞�݁H
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART���M���荞�݁H
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART��M���荞�݁H
	
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
//**********Timer2���荞��*************
void Timer2_isr(void)
{
		PIR1bits.TMR2IF=0;          // �^�C�}2���荞�݃t���O��0�ɂ���
		if(--cnt2==0)
		{              // cnt1��-1���Č��ʂ�0�H
			cnt2 = 113;
			if(PIN_LED_3)
			PIN_LED_3=0;    //LED3���P�b�Ԋu�œ_��
			else
			PIN_LED_3=1;
		}
}
//**********Timer3���荞��*************
void Timer3_isr(void)
{
		PIR2bits.TMR3IF=0;          // �^�C�}3���荞�݃t���O��0�ɂ���
		if(--cnt3==0)
		{              // cnt1��-1���Č��ʂ�0�H
			cnt3=7;
			if(PIN_LED_4)
			PIN_LED_4=0;    //LED3���P�b�Ԋu�œ_��
			else
			PIN_LED_4=1;
		}
}
//**********RB0���荞��*************
void RB0_isr(void)
{
	INTCONbits.INT0IF = 0;		//���荞�݃t���O�N���A
	PIN_LED_0=1;    //LED3���P�b�Ԋu�œ_��
	wait_ms(1000);
	//PIN_LED_1=1;
}
//**********RB1���荞��*************
void RB1_isr(void)
{
	INTCON3bits.INT1IF = 0;		//���荞�݃t���O�N���A
	PIN_LED_1=1;    //LED3���P�b�Ԋu�œ_��
	wait_ms(1000);
	//PIN_LED_2=1;
}
//**********RB2���荞��*************
void RB2_isr(void)
{
	INTCON3bits.INT2IF = 0;		//���荞�݃t���O�N���A
	PIN_LED_2=1;    //LED3���P�b�Ԋu�œ_��
	wait_ms(1000);
	//PIN_LED_2=1;
}
//**********RB PORT�ω����荞��*************
void RB_PORT_isr(void)
{
	int PORT_data = 0;
	PORT_data = PORTB;
	INTCONbits.RBIF = 0;		//���荞�݃t���O�N���A
	
}
//**********AD���荞��*************
void AD_isr(void)
{
	PIR1bits.ADIF = 0;		//���荞�݃t���O�N���A
	if(--cnt4==0)
		{              // cnt1��-1���Č��ʂ�0�H
			cnt4=255;
			if(PIN_LED_5)
			PIN_LED_5=0;    //LED3���P�b�Ԋu�œ_��
			else
			PIN_LED_5=1;
		}
}
//**********UART���M���荞��*************
void UART_TX_isr(void)
{
	int k =0;
	putc1USART(Send_Buf);
		for(k=0;k>=20;k++)
		{
			Send_Buf[k] = 0;
		}
		PIR1bits.TXIF = 0;		//���荞�݃t���O�N���A
	if(--cnt5==0)
		{              // cnt1��-1���Č��ʂ�0�H
			cnt5=255;
			if(PIN_LED_6)
			PIN_LED_6=0;    //LED3���P�b�Ԋu�œ_��
			else
			PIN_LED_6=1;
		}
}
//**********UART��M���荞��*************
void UART_RC_isr(void)
{
	int i =0;
	i++;
	temp_Buf[i] = getc1USART();
	PIR1bits.RCIF = 0;		//���荞�݃t���O�N���A
	if(--cnt6==0)
		{              // cnt1��-1���Č��ʂ�0�H
			cnt6=255;
			if(PIN_LED_7)
			PIN_LED_7=0;    //LED3���P�b�Ԋu�œ_��
			else
			PIN_LED_7=1;
		}
}

