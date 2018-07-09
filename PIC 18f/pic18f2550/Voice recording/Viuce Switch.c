/*********************************************************
����ŃX�C�b�`���O�v���O����
pic18f2550 ��CPU�N���b�N��48Mhz
timer0 ��10ms �Ŋ��荞�݂�������
RB0 ���荞�݂Ń}�C�N�������M����ǂݎ��
********************************************************/�@
#include <p18f2550.h>            // PIC18f2550�̃w�b�_�E�t�@�C��
#include <timers.h>             // �^�C�}�֐��̃w�b�_�E�t�@�C��
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <adc.h>
//#include "HardwareProfile.h"
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
//		#pragma config CCP2MX   = ON
		#pragma config STVREN   = ON
		#pragma config LVP      = OFF
//		#pragma config ICPRT    = OFF       // Dedicated In-Circuit Debug/Programming
		#pragma config XINST    = OFF       // Extended Instruction Set
		#pragma config CP0      = OFF
		#pragma config CP1      = OFF
//		#pragma config CP2      = OFF
//		#pragma config CP3      = OFF
		#pragma config CPB      = OFF
//		#pragma config CPD      = OFF
		#pragma config WRT0     = OFF
		#pragma config WRT1     = OFF
//		#pragma config WRT2     = OFF
//		#pragma config WRT3     = OFF
		#pragma config WRTB     = OFF       // Boot Block Write Protection
		#pragma config WRTC     = OFF
//		#pragma config WRTD     = OFF
		#pragma config EBTR0    = OFF
		#pragma config EBTR1    = OFF
//		#pragma config EBTR2    = OFF
//		#pragma config EBTR3    = OFF
		#pragma config EBTRB    = OFF
/********�s���}�N��******************/
#define		PIN_LED_0	PORTCbits.RC0
#define		PIN_LED_1	PORTCbits.RC1
#define		PIN_LED_2	PORTCbits.RC2
#define		PIN_LED_3	PORTCbits.RC4
#define		PIN_LED_4	PORTCbits.RC5
#define		PIN_LED_5	PORTBbits.RB4
#define		PIN_LED_6	PORTBbits.RB5
#define		PIN_LED_7	PORTBbits.RB6
#define		PIN_LED_8	PORTBbits.RB7
/********�֐��v���g�^�C�v************/
void Timer0_isr(void);
void Timer1_isr(void);
void RB_PORT_isr(void);
void RB0_isr(void);
void RB1_isr(void);
void RB2_isr(void);
void wait_us(unsigned int t);
void wait_ms(unsigned int t);
void wait_s(unsigned long int t);

//********************************************************
//***** �O���[�o���ϐ��A�萔�̒�`
unsigned long int cnt=0;            // cnt,cnt1��LED�X�V�����p�J�E���^
unsigned char cnt1=0;
unsigned int signaldata = 0;
unsigned int data_i= 1;
unsigned int data_z= 0;
unsigned int i= 0;
unsigned int VOICE_data[121] = {0};
//****** ���C���֐�
void main(void)					// ���C���֐�
{
	TRISA=0xFF;
	TRISB=0x0F;
	TRISC=0;					// �|�[�gC�����ׂďo�̓s���ɂ���
	PORTC = 0xFF;
	
	PIN_LED_5 = 0;
	PIN_LED_6 = 0;
	PIN_LED_7 = 0;
	PIN_LED_8 = 0;
	wait_s(1);
	PIN_LED_5 = 1;
	PIN_LED_6 = 1;
	PIN_LED_7 = 1;
	PIN_LED_8 = 1;
	wait_s(1);
	PIN_LED_5 = 0;
	PIN_LED_6 = 0;
	PIN_LED_7 = 0;
	PIN_LED_8 = 0;
	wait_s(1);
	PIN_LED_5 = 1;
	PIN_LED_6 = 1;
	PIN_LED_7 = 1;
	PIN_LED_8 = 1;
	wait_s(1);
	PIN_LED_5 = 0;
	PIN_LED_6 = 0;
	PIN_LED_7 = 0;
	PIN_LED_8 = 0;
	/*
	//**********Timer0������****************
    OpenTimer0(TIMER_INT_ON & T0_8BIT & T0_SOURCE_INT & T0_PS_1_256);
								// �^�C�}0�̐ݒ�, 8�r�b�g���[�h, �����ݎg�p 
								//�����N���b�N�A1:256�v���X�P�[��
	
	INTCONbits.T0IF=0;			// �^�C�}0���荞�݃t���O��0�ɂ���
	//�J�E���g�l�̃��[�h
		WriteTimer0(65067);			//10ms�ɐݒ�
		WriteTimer0();			//10ms�ɐݒ�
	*/
	
	
	/*
	
	//**********Timer1������****************
	OpenTimer1(TIMER_INT_ON & T1_16BIT_RW & T1_SOURCE_INT & T1_PS_1_8 & 
			T1_OSC1EN_OFF);     //�^�C�}�P�̐ݒ�,16�r�b�g���[�h�A�����ݎg�p
								//�����N���b�N�A1:256�v���X�P�[��
	PIR1bits.TMR1IF=0;			// �^�C�}�P���荞�݃t���O��0�ɂ���
	
	*/
	
	
//***********UART������***********************
	
	//UART������
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 77); 	//���Ӂ@OpenUSART�@�֐��̍Ō�̕����̓{�[���[�g�ݒ�ō���40Mhz��9600�̎���64�ɂ���B�܂�48Mhz��9600�̎���77�ɂ���
	
	/*
	
	
//***********AD�ϊ�������**********************
	OpenADC(ADC_FOSC_64 &           //AD�ϊ��p�N���b�N�@�@�V�X�e���N���b�N��1/64�@0.05��sec�~64��3.2��sec�@>=�@1.6��sec�@���@OK
				ADC_RIGHT_JUST &        //�ϊ����ʂ̕ۑ����@�@���l�߁@
				ADC_8_TAD,              //AD�ϊ��̃A�N�C�W�V�����^�C���I���@3.2��sec�i=1Tad�j�~8Tad=25.6��sec�@�����@12.8��sec�@���@OK
				ADC_CH0 &                       //AD�ϊ�����̃`�����l���I���iPIC18F�͓����ɕ�����AD�ϊ��͂ł��Ȃ��j
				ADC_INT_OFF &           //AD�ϊ��ł̊����ݎg�p�̗L��
				ADC_VREFPLUS_VDD &      //Vref+�̐ݒ�@�@�@�o�h�b�̓d���d���Ɠ����FADC_VREFPLUS_VDD �@or�@�O���i�`�m�R�j�̓d���FADC_VREFPLUS_EXT
				ADC_VREFMINUS_VSS,      //Vref-�̐ݒ�@�@�@�o�h�b��0�u�FADC_VREFMINUS_VSS    or�@�O���iAN2)�̓d���FADC_VREFMINUS_EXT
				0b1110  //�|�[�g�̃A�i���O�E�f�W�^���I���@�iADCON1�̉��ʂS�r�b�g���L�ځj�@�@AN0�̂݃A�i���O�|�[�g��I���A���̓f�W�^���|�[�g��I��
				//�� �@�A�i���O�|�[�g���@AN0�̂� �� 0b1110�@�@�AAN0 & AN1�@���@0b1011�A AN0 & AN1 & AN2 ��1100�@���@�ڍ׃f�[�^�V�[�g�Q��
		);
	
	
	*/
	
//***** �D�揇�ʊ����ݎg�p�錾
    RCONbits.IPEN=1;
//***********���荞�ݐݒ�***********************
	
//************Timer0���荞�ݐݒ�********
	INTCONbits.TMR0IE=0;		//Timer0���荞�݋��E�֎~�r�b�g
	INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
//************Timer1���荞�ݐݒ�********
	PIE1bits.TMR1IE=0;		//Timer0���荞�݋��E�֎~�r�b�g
	PIR1bits.TMR1IF=0;          // �^�C�}�P���荞�݃t���O��0�ɂ���
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
{
	_asm
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
void wait_ms(unsigned int t) {
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[s]======//
void wait_s(unsigned long int t) {
	t = t*1000;
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[us]======//
void wait_us(unsigned int t) {
	while(t--) {
		WAIT_US;
	}
}
/***********���荞�݊֐����s��*********/

//**********Timer0���荞��*************
void Timer0_isr(void)
{
	INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
	//�J�E���g�l�̃��[�h
	WriteTimer0(65067);			//10ms�ɐݒ�
	cnt++;
	
}
//**********Timer1���荞��*************
void Timer1_isr(void)
{
		PIR1bits.TMR1IF=0;          // �^�C�}�P���荞�݃t���O��0�ɂ���
}
//**********RB0���荞��*************
void RB0_isr(void)
{
	
	INTCONbits.INT0IF = 0;		//���荞�݃t���O�N���A
	
	//90ms��dekay
	wait_ms(90);
	
	if(INTCONbits.TMR0IE == 0)				//�^�C�}���荞�݂��J�n���Ă��Ȃ����H 
	{
		
		//**********Timer0������****************
	OpenTimer0(TIMER_INT_ON & T0_16BIT & T0_SOURCE_INT & T0_PS_1_256);
								// �^�C�}0�̐ݒ�, 16�r�b�g���[�h, �����ݎg�p 
								//�����N���b�N�A1:256�v���X�P�[��
		
		//timer���g�����Ԃ̌v���J�n
		INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
		INTCONbits.TMR0IE=1;		//�^�C�}0���荞�݋���
		//�J�E���g�l�̃��[�h
		WriteTimer0(65067);			//10ms�ɐݒ�
	}
	else					//�J�E���g���J�n���Ă����ꍇ
	{
		if(20<=cnt && cnt <=200)
		{
			INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
			//�J�E���g�l�̃��[�h
			WriteTimer0(65067);			//10ms�ɐݒ�
			//�f�[�^��1�Ƃ��Ď�M
			data_z = 1;
			data_z =  data_z<<data_i;
			signaldata = signaldata |data_z;
			data_i++;
		}else if(300<=cnt && cnt <=500)
		{
			INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
			//�J�E���g�l�̃��[�h
			WriteTimer0(65067);			//10ms�ɐݒ�
			//�f�[�^��0�Ƃ��Ď�M
			data_z = 0;
			data_z =  data_z<<data_i;
			signaldata = signaldata |data_z;
			data_i++;
		}else if(600<=cnt)				//����I��
		{
			CloseTimer0();				//�^�C�}�[�I��
			//�M���I��
			INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
			INTCONbits.TMR0IE=0;		//�^�C�}0���荞�ݕs����
			//�f�[�^���s
			switch(signaldata)
				{
				case	0b00001010:
					PIN_LED_5 = 1;
							break;
				case	0b00011000:
				PIN_LED_6 = 1;
						break;
				case	0b00101110:
				PIN_LED_7 = 1;
						break;
				case	0b01011110:
				PIN_LED_8 = 1;
						break;
				}
			
			//�f�[�^������
			signaldata = 0;
			data_i = 0;
		}else		//��O
		{
			
			CloseTimer0();				//�^�C�}�[�I��
			INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
			INTCONbits.TMR0IE=0;		//�^�C�}0���荞�ݕs����
			//�f�[�^������
			signaldata = 0;
			data_i = 0;
		}
		
		//�J�E���g��������
		cnt = 0;
	}
	
}
//**********RB1���荞��*************
void RB1_isr(void)
{
	INTCON3bits.INT1IF = 0;		//���荞�݃t���O�N���A
}
//**********RB2���荞��*************
void RB2_isr(void)
{
	INTCON3bits.INT2IF = 0;		//���荞�݃t���O�N���A
}
//**********RB PORT�ω����荞��*************
void RB_PORT_isr(void)
{
	//���[�J���ϐ���`
	int PORT_data = 0;
	
	PORT_data = PORTB;			//�f�[�^�ǂݍ���
	INTCONbits.RBIF = 0;		//���荞�݃t���O�N���A
	
}
/**********timer0�̃J�E���g�l�̐ݒ�**************


PIC18F452���g�p���ă^�C�}�[0��1�b���Ɋ��荞�݂��s�킹�鏈���������Ă݂܂����B �����̓v���O�������ɋL�ڂ��Ă��܂����A���荞�݂Ɋւ��镔���͌���L�ڂ��悤�Ǝv���܂��B ����͊��荞�݂̗D�惌�x���͍l���Ă��܂���B
�@
�Z�����b�N10MHz���g�p���ē�����PLL4�{�A�����40MHz����ł��B
�@
1�b���i40MHz/4�j=1��10MHz=1��10,000,000=0.0000001=100ns
�@
��̌v�Z��1���߃T�C�N���̎��Ԃł��B�Ȃ̂�1�b�Ԃɉ��񂱂���s���΂��������l���܂��B
�@
1�b��100ns=1��0.0000001=10,000,000��
�@
����͓���N���b�N���v�Z���₷���l�Ŋy�ł����B�������A1000����Ƃ����l�̓^�C�}�[�ɐݒ�ł��܂���B �����Ńv���X�P�[�����g�p���܂��B�^�C�}�[0�ɂ�256���ݒ�ł���̂Ōv�Z���Ă݂܂��B
�@
10,000,000��256=39,062��@�i���m�ɂ�39,062.5�j
�@
16�r�b�g�Ȃ̂�65,535�܂ŉ\�Ȃ̂ō���͑��v�Ȃ悤�ł��B�Ȃ̂Ńv���X�P�[����256�ɐݒ肵�ăJ�E���g��39,062�ł��B �������A�^�C�}�[�̃J�E���^�̓A�b�v�J�E���^�ł��B�Ȃ̂�MAX�l����K�v�ȃJ�E���g���������Ă��K�v������܂��B
�@
65,536-39,062=0x10000-0x9896=0x676A
�@
��L�̂Ƃ���A�^�C�}�[�̐ݒ��0x676A�Ɍ��܂�܂����B



�Q�lURL�@http://amahime.main.jp/c18prog/main.php?name=c18prog
*/





