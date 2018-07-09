#include <p18f2550.h>
#include <delays.h>
#include <usart.h>
#include <timers.h>
#include <adc.h>
#include <stdlib.h> 
#include <stdio.h>

//***** �R���t�B�M�����[�V�����̐ݒ�config

 
		#pragma config PLLDIV	= 5			// (20 MHz crystal on PICDEM FS USB board)
		#pragma config CPUDIV	= OSC1_PLL2	  
		#pragma config USBDIV	= 2			// Clock source from 96MHz PLL/2
		#pragma config FOSC		= HSPLL_HS
		#pragma config FCMEN	= OFF
		#pragma config IESO		= OFF
		#pragma config PWRT		= OFF
		#pragma config BOR		= OFF		//ADD
		#pragma config BORV		= 3
		//	#pragma config VREGEN	= ON	  //USB Voltage Regulator
		#pragma config VREGEN	= OFF	   //USB Voltage Regulator
		#pragma config WDT		= OFF
		#pragma config WDTPS	= 32768
		#pragma config MCLRE	= ON
		#pragma config LPT1OSC	= OFF
		#pragma config PBADEN	= OFF
		//		#pragma config CCP2MX	= ON
		#pragma config STVREN	= ON
		#pragma config LVP		= OFF
		//		#pragma config ICPRT	= OFF		// Dedicated In-Circuit Debug/Programming
		#pragma config XINST	= OFF		// Extended Instruction Set
		#pragma config CP0		= OFF
		#pragma config CP1		= OFF
		//		#pragma config CP2		= OFF
		//		#pragma config CP3		= OFF
		#pragma config CPB		= OFF
		//		#pragma config CPD		= OFF
		#pragma config WRT0		= OFF
		#pragma config WRT1		= OFF
		//		#pragma config WRT2		= OFF
		//		#pragma config WRT3		= OFF
		#pragma config WRTB		= OFF		// Boot Block Write Protection
		#pragma config WRTC		= OFF
		//		#pragma config WRTD		= OFF
		#pragma config EBTR0	= OFF
		#pragma config EBTR1	= OFF
		//		#pragma config EBTR2	= OFF
		//		#pragma config EBTR3	= OFF
		#pragma config EBTRB	= OFF

		
/********�֐��v���g�^�C�v************/ 
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
/********�s���}�N��***********/
  
#define		PIN_LED_0	PORTCbits.RC0 
#define		PIN_LED_1	PORTCbits.RC1 
#define		PIN_LED_2	PORTCbits.RC2 
#define		PIN_LED_3	PORTCbits.RC4 
#define		PIN_LED_4	PORTCbits.RC5 
#define		PIN_LED_5	PORTBbits.RB4 
#define		PIN_LED_6	PORTBbits.RB5 
#define		PIN_LED_7	PORTBbits.RB6 
#define		PIN_LED_8	PORTBbits.RB7 
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

//***********����PWM�ݒ�********
#define	ClockOfTime		48				//�P�ʂ�MHz
//PWM�s���}�N��
#define	PIN_PWM01	PORTBbits.RB0
#define	PIN_PWM02	PORTBbits.RB1
#define	PIN_PWM03	PORTBbits.RB2
#define	PIN_PWM04	PORTBbits.RB3
#define	PIN_PWM05	PORTBbits.RB4
#define	PIN_PWM06	PORTBbits.RB5
#define	PIN_PWM07	PORTBbits.RB6
#define	PIN_PWM08	PORTBbits.RB7

//�ϐ���`
unsigned int PWM_duty01=0, PWM_duty02=0, PWM_duty03=0, PWM_duty04=0, PWM_duty05=0, PWM_duty06=0, PWM_duty07=0, PWM_duty08=0;
unsigned int PWM_duty_time = 1;
	
	#if( ClockOfTime == 20  )
		#define time_wait_count	5				//wait�J�E���g�ϐ�
		
		#define	TimerPWMcount	29				//200.200usec�����Ɋ��荞��
		#define	PURISUKELA	T0_PS_1_4			//�v���X�P�[���ݒ�
		#warning	�N���b�N���x20MHz
		
	#elif( ClockOfTime == 40 )
		#define time_wait_count	10				//wait�J�E���g�ϐ�
		#define	PURISUKELA	T0_PS_1_8			//�v���X�P�[���ݒ�
		#define	TimerPWMcount	17				//200.600usec�����Ɋ��荞��
		#warning	�N���b�N���x40MHz
		
	#elif( ClockOfTime == 48 )
		#define time_wait_count	12				//wait�J�E���g�ϐ�
		#define	PURISUKELA	T0_PS_1_16			//�v���X�P�[���ݒ�
		
		#define	TimerPWMcount	111				//201.083usec�����Ɋ��荞��
		#warning	�N���b�N���x48MHz
		
	#elif( ClockOfTime == 64 )
		#define time_wait_count	16				//wait�J�E���g�ϐ�
		#define	PURISUKELA	T0_PS_1_128			//�v���X�P�[���ݒ�
		
		#define	TimerPWMcount	241				//201.400usec�����Ɋ��荞��
		#warning	�N���b�N���x64MHz
		
	#else
		#define time_wait_count	16
		#define	PURISUKELA	T0_PS_1_256
		
		#define	TimerPWMcount	50
		
	#endif

//---Wait�֐���`---//
#define WAIT_MS Delay1KTCYx(time_wait_count)// Wait 1ms
#define WAIT_US Delay10TCYx(time_wait_count) // Wait 1us
//=======wait[ms]======// 
void wait_ms(unsigned int t) { 
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
 
//***** ���C���֐�
void main(void)						// ���C���֐�
{
	
	
	
	//���[�J���ϐ���`
	char Message1[8]="Start!!";
	char Message2[7]="FUKUDA";
	char data;
	unsigned int i=0;
	
	//UART������
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 64);	//���Ӂ@OpenUSART�@�֐��̍Ō�̕����̓{�[���[�g�ݒ�ō���40Mhz��9600
	
	//Timer0������
	OpenTimer0(TIMER_INT_ON & T0_8BIT & T0_SOURCE_INT & PURISUKELA);
	
/*	OpenTimer1(TIMER_INT_ON & T1_8BIT_RW & T1_SOURCE_INT & T1_PS_1_8 &	
		T1_OSC1EN_OFF);		//�^�C�}�P�̐ݒ�,8�r�b�g���[�h�A�����ݎg�p 
							//�����N���b�N�A1:8�v���X�P�[�� 
	OpenTimer2(	 
		TIMER_INT_ON & 
		T2_PS_1_4 & 
		T2_POST_1_8 ); 
		
	OpenTimer3(TIMER_INT_ON & T3_SOURCE_INT &  T3_SYNC_EXT_ON &	 
		T3_SOURCE_CCP); 
*/		//�J�E���g�l�̃��[�h
		WriteTimer0(TimerPWMcount);
	
	//������
		TRISA=0x00;						   // �|�[�gA�����ׂďo�̓s���ɂ���
		TRISB=0;						// �|�[�gB�����ׂďo�̓s���ɂ���
		TRISC=0b10111111;						 // �|�[�gC�����ׂďo�̓s���ɂ���
		ADCON1 = 0b00001111;				//���ׂăf�W�^��
	//-----AD������--------// 
	OpenADC(ADC_FOSC_64 &			//AD�ϊ��p�N���b�N�@�@�V�X�e���N���b�N��1/64�@0.05��sec�~64��3.2��sec�@>=�@1.6��sec�@���@OK 
			ADC_RIGHT_JUST &		//�ϊ����ʂ̕ۑ����@�@���l�߁@ 
			ADC_8_TAD,				//AD�ϊ��̃A�N�C�W�V�����^�C���I���@3.2��sec�i=1Tad�j�~8Tad=25.6��sec�@�����@12.8��sec�@���@OK 
			ADC_CH0 &						//AD�ϊ�����̃`�����l���I���iPIC18F�͓����ɕ�����AD�ϊ��͂ł��Ȃ��j 
			ADC_INT_ON &		   //AD�ϊ��ł̊����ݎg�p�̗L�� 
			ADC_VREFPLUS_VDD &		//Vref+�̐ݒ�@�@�@�o�h�b�̓d���d���Ɠ����FADC_VREFPLUS_VDD �@or�@�O���i�`�m�R�j�̓d���FADC_VREFPLUS_EXT 
			ADC_VREFMINUS_VSS,		//Vref-�̐ݒ�@�@�@�o�h�b��0�u�FADC_VREFMINUS_VSS	 or�@�O���iAN2)�̓d���FADC_VREFMINUS_EXT 
			0b1110	//�|�[�g�̃A�i���O�E�f�W�^���I���@�iADCON1�̉��ʂS�r�b�g���L�ځj�@�@AN0�̂݃A�i���O�|�[�g��I���A���̓f�W�^���|�[�g��I�� 
			//�� �@�A�i���O�|�[�g���@AN0�̂� �� 0b1110�@�@�AAN0 & AN1�@���@0b1011�A AN0 & AN1 & AN2 ��1100�@���@�ڍ׃f�[�^�V�[�g�Q�� 
		); 
	 
	//***********���荞�ݐݒ�*********************** 
//***** �D�揇�ʊ����ݎg�p�錾 
	RCONbits.IPEN=1; 
//***** �჌�x���g�p���ӂ̒�` 
	IPR1bits.TMR1IP=0; 
//************Timer0�D�揇�ʐݒ�***** 
	INTCON2bits.TMR0IP = 1; //���ʊ��荞�݂ɐݒ� 
//************Timer1�D�揇�ʐݒ�***** 
	IPR1bits.TMR1IP = 0;	//���ʊ��荞�݂ɐݒ� 
//************Timer2�D�揇�ʐݒ�***** 
	IPR1bits.TMR2IP = 0;	//���ʊ��荞�݂ɐݒ� 
//************Timer3�D�揇�ʐݒ�***** 
	IPR2bits.TMR3IP = 0;	//���ʊ��荞�݂ɐݒ� 
  
//************RB0���荞�ݐݒ�******** 
	//RB0/INT �͊��荞�݂ɗD�揇�ʂ͂Ȃ����ʂ̂Ɋ��荞�݂������� 
	INTCONbits.INT0IE=0;		//RB0/INT �O�������݃C�l�[�u���r�b�g(4bit��)�� 1:���� , 0:�s���� 
	INTCON2bits.INTEDG0=0;			//RB0/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)	
	  
//************RB1���荞�ݐݒ�********* 
	INTCON3bits.INT1IP = 0;		//RB1/INT ���ʊ��荞�݂ɂ��� 
	INTCON3bits.INT1IE = 0;		//RB1/INT �O�������݃C�l�[�u���r�b�g(4bit��)�� 1:���� , 0:�s���� 
	INTCON2bits.INTEDG1 = 0;			//RB1/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��) 
  
//************RB2���荞�ݐݒ�********* 
	INTCON3bits.INT2IP = 0;			//RB2/INT ���ʊ��荞�݂ɂ��� 
	INTCON3bits.INT2IE = 0;		//R2/INT �O�������݃C�l�[�u���r�b�g(4bit��)�� 1:���� , 0:�s���� 
	INTCON2bits.INTEDG2 = 0;	//RB2/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��) 
  
//************RBPORT���荞�ݐݒ�********* 
	INTCON2bits.RBIP = 0;		//RBPORT/INT ��ʊ��荞�݂ɂ��� 
	INTCONbits.RBIE = 0;		//RB2/INT �O�������݃C�l�[�u���r�b�g(4bit��)�� 1:���� , 0:�s����
	  
//************AD�ϊ����荞��************* 
	IPR1bits.ADIP = 0;		//AD�ϊ����荞�݂��ʊ��荞�݂ɐݒ� 
	PIE1bits.ADIE = 0;		//AD�ϊ����荞�݂� 1:���� , 0:�s����
//************UART���M���荞��************* 
	IPR1bits.TXIP = 0;		//uart�ϊ����荞�݂��ʊ��荞�݂ɐݒ� 
	PIE1bits.TXIE= 0;		//uart�ϊ����荞�݂� 1:���� , 0:�s����
//************UART��M���荞��************* 
	IPR1bits.RCIP = 0;		//AD�ϊ����荞�݂��ʊ��荞�݂ɐݒ� 
	PIE1bits.RCIE= 0;		//AD�ϊ����荞�݂� 1:���� , 0:�s���� 
	  
	//***** �����݋��� 
	INTCONbits.GIEH=1;			// �����x������ 
	INTCONbits.GIEL=1;			// �჌�x������ 
	  
	  
	  //����PWM������
	  PIN_PWM01 = 1;
		PIN_PWM02 = 1;
		PIN_PWM03 = 1;
		PIN_PWM04 = 1;
		PIN_PWM05 = 1;
		PIN_PWM06 = 1;
		PIN_PWM07 = 1;
		PIN_PWM08 = 1;
//	putsUSART(Message1); 
	//***** ���C�����[�v�i�A�C�h�����[�v�j 
	while(1)	 
	{ 
		/*
		
		SetChanADC(ADC_CH0);	//Select Channel 0 
		Delay100TCYx(50);		//20usec delay 
		ConvertADC();		//Start A/D 
		while(BusyADC());	//Wait end of conversion 
		data = ReadADC();	//Get A/D data 
		fprintf(_H_USART,"\rData=%ld\n",data);	 
		  
		putsUSART(Message1); 
		  
		
		
		PIN_LED_0 = 1; 
		Delay10KTCYx(100); 
		PIN_LED_0 = 0; 
		Delay10KTCYx(100); 
		
		*/
		PWM_duty01=i;
		PWM_duty02=100-i;
		i++;
		if(i==100)
		{
			i=0;
		}
		wait_ms(10);
		
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
void High_isr(void)						 // ���荞�݊֐� 
{ 
	INTCONbits.GIEH=0;			// �����x���s���� 
	if(INTCONbits.T0IF) Timer0_isr();		// �^�C�}0���荞�݁H 
	if(PIR1bits.TMR1IF) Timer1_isr();		 // �^�C�}�P���荞�݁H 
	if(PIR1bits.TMR2IF) Timer2_isr();		//�^�C�}2���荞�݁H 
	if(PIR2bits.TMR3IF) Timer3_isr();		// �^�C�}3���荞�݁H 
	if(INTCONbits.INT0IF)	RB0_isr();		 // �O�������݂O���荞�݁H 
	if(INTCON3bits.INT1IF)	RB1_isr();		 // �O�������݂P���荞�݁H 
	if(INTCON3bits.INT2IF)	RB2_isr();		 // �O�������݂Q���荞�݁H 
	if(INTCONbits.RBIF) RB_PORT_isr();		 // �O��������PORT�ω����荞�݁H 
	if(PIR1bits.ADIF)	AD_isr();			 //AD���荞�݁H 
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART���M���荞�݁H 
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART��M���荞�݁H 
	//INTCONbits.GIEH=1;		  // �����x������ 
	  
}									 
//***** �჌�x�������ݏ����֐� 
void Low_isr(void)					   // ���荞�݊֐� 
{ 
	INTCONbits.GIEL=0;			// �჌�x���s���� 
	if(INTCONbits.T0IF) Timer0_isr();		//�^�C�}0���荞�݁H 
	if(PIR1bits.TMR1IF) Timer1_isr();		//�^�C�}�P���荞�݁H 
	if(PIR1bits.TMR2IF) Timer2_isr();		//�^�C�}2���荞�݁H 
	if(PIR2bits.TMR3IF) Timer3_isr();		// �^�C�}3���荞�݁H 
	if(INTCONbits.INT0IF)	RB0_isr();		 // �O�������݂O���荞�݁H 
	if(INTCON3bits.INT1IF)	RB1_isr();		 // �O�������݂P���荞�݁H 
	if(INTCON3bits.INT2IF)	RB2_isr();		 // �O�������݂Q���荞�݁H 
	if(INTCONbits.RBIF) RB_PORT_isr();		 // �O��������PORT�ω����荞�݁H 
	if(PIR1bits.ADIF)	AD_isr();			 //AD���荞�݁H 
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART���M���荞�݁H 
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART��M���荞�݁H 
	  
}						 
  
  
  
  
/************************************************************/ 

/***********���荞�݊֐����s��*********/
  
//**********Timer0���荞��************* 
void Timer0_isr(void) 
{ 
	INTCONbits.T0IF=0;			// �^�C�}0���荞�݃t���O��0�ɂ��� 
	WriteTimer0(TimerPWMcount);	//�J�E���g�l�Z�b�g
	PWM_duty_time++;
	
	if(PWM_duty_time>=PWM_duty01) 
	{	PIN_PWM01 = 0; } 
	if(PWM_duty_time>=PWM_duty02) 
	{	PIN_PWM02 = 0; } 
	if(PWM_duty_time>=PWM_duty03) 
	{	PIN_PWM03 = 0; } 
	if(PWM_duty_time>=PWM_duty04) 
	{	PIN_PWM04 = 0; } 
	if(PWM_duty_time>=PWM_duty05) 
	{	PIN_PWM05 = 0; } 
	if(PWM_duty_time>=PWM_duty06) 
	{	PIN_PWM06 = 0; } 
	if(PWM_duty_time>=PWM_duty07) 
	{	PIN_PWM07 = 0; } 
	if(PWM_duty_time>=PWM_duty08) 
	{	PIN_PWM08 = 0; } 
	
	if(PWM_duty_time >= 100)
	{	
		PWM_duty_time=1;	//�^�C�}�[������
		PIN_PWM01 = 1;
		PIN_PWM02 = 1;
		PIN_PWM03 = 1;
		PIN_PWM04 = 1;
		PIN_PWM05 = 1;
		PIN_PWM06 = 1;
		PIN_PWM07 = 1;
		PIN_PWM08 = 1;
	
	}
} 
//**********Timer1���荞��************* 
void Timer1_isr(void) 
{ 
		PIR1bits.TMR1IF=0;			// �^�C�}�P���荞�݃t���O��0�ɂ��� 

		if(--cnt1==0) 
		{			   // cnt1��-1���Č��ʂ�0�H 
			cnt1=5;					// cnt1��LED�̍X�V�����������߂� 
			if(PIN_LED_2) 
			PIN_LED_2=0;	//LED3���P�b�Ԋu�œ_�� 
			else
			PIN_LED_2=1; 
		} 
} 
//**********Timer2���荞��************* 
void Timer2_isr(void) 
{ 
		PIR1bits.TMR2IF=0;			// �^�C�}2���荞�݃t���O��0�ɂ��� 
		if(--cnt2==0) 
		{			   // cnt1��-1���Č��ʂ�0�H 
			cnt2 = 113; 
			if(PIN_LED_3) 
			PIN_LED_3=0;	//LED3���P�b�Ԋu�œ_�� 
			else
			PIN_LED_3=1; 
		} 
} 
//**********Timer3���荞��************* 
void Timer3_isr(void) 
{ 
		PIR2bits.TMR3IF=0;			// �^�C�}3���荞�݃t���O��0�ɂ��� 
		if(--cnt3==0) 
		{			   // cnt1��-1���Č��ʂ�0�H 
			cnt3=7; 
			if(PIN_LED_4) 
			PIN_LED_4=0;	//LED3���P�b�Ԋu�œ_�� 
			else
			PIN_LED_4=1; 
		} 
} 
//**********RB0���荞��************* 
void RB0_isr(void) 
{ 
	INTCONbits.INT0IF = 0;		//���荞�݃t���O�N���A 
	PIN_LED_0=1;	//LED3���P�b�Ԋu�œ_�� 
	wait_ms(1000); 
	//PIN_LED_1=1; 
} 
//**********RB1���荞��************* 
void RB1_isr(void) 
{ 
	INTCON3bits.INT1IF = 0;		//���荞�݃t���O�N���A 
	PIN_LED_1=1;	//LED3���P�b�Ԋu�œ_�� 
	wait_ms(1000); 
	//PIN_LED_2=1; 
} 
//**********RB2���荞��************* 
void RB2_isr(void) 
{ 
	INTCON3bits.INT2IF = 0;		//���荞�݃t���O�N���A 
	PIN_LED_2=1;	//LED3���P�b�Ԋu�œ_�� 
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
		{			   // cnt1��-1���Č��ʂ�0�H 
			cnt4=255; 
			if(PIN_LED_5) 
			PIN_LED_5=0;	//LED3���P�b�Ԋu�œ_�� 
			else
			PIN_LED_5=1; 
		} 
} 
//**********UART���M���荞��************* 
void UART_TX_isr(void) 
{ 
	int k =0; 
	putcUSART(Send_Buf); 
		for(k=0;k>=20;k++) 
		{ 
			Send_Buf[k] = 0; 
		} 
		PIR1bits.TXIF = 0;		//���荞�݃t���O�N���A 
	if(--cnt5==0) 
		{			   // cnt1��-1���Č��ʂ�0�H 
			cnt5=255; 
			if(PIN_LED_6) 
			PIN_LED_6=0;	//LED3���P�b�Ԋu�œ_�� 
			else
			PIN_LED_6=1; 
		} 
} 
//**********UART��M���荞��************* 
void UART_RC_isr(void) 
{ 
	int i =0; 
	i++; 
	temp_Buf[i] = getcUSART(); 
	PIR1bits.RCIF = 0;		//���荞�݃t���O�N���A 
	if(--cnt6==0) 
		{			   // cnt1��-1���Č��ʂ�0�H 
			cnt6=255; 
			if(PIN_LED_7) 
			PIN_LED_7=0;	//LED3���P�b�Ԋu�œ_�� 
			else
			PIN_LED_7=1; 
		} 
}