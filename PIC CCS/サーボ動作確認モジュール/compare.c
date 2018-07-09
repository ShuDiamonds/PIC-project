#include <16f88.h>

#fuses INTRC_IO,NOWDT,NOCPD,NOPROTECT,PUT,NOMCLR,NOBROWNOUT
#use delay(clock = 8000000)					 // clock 8MHz
void ccp1_int(void);						 // �v���g�^�C�v
void intval();
//********PIN�}�N��
#define SorvoPIN	PIN_A6
#define AD_PIN		PIN_A0
#define M1			PIN_B1
#define M2			PIN_B2
#define SW1			PIN_B3
#define SW2			PIN_B4


/*
#byte GP=5			//GP��0�`5��6�|�[�g
#bit SorvoPIN=GP.5		//�T�[�{�pPIN 
#bit AD_PIN=GP.0		//�ϒ�R�ڑ�
#bit M1=GP.1		//���[�^�[�M�����P
#bit M2=GP.2		//���[�^�[�M�����Q
#bit SW1=GP.4		//�^�N�g�X�C�b�`�P
#bit SW2=GP.3		//�^�N�g�X�C�b�`�Q
*/
//****************//
#define HIGH 1
#define LOW 0
#define TimerTIME 45536

#use fast_io(a)
#use fast_io(b)
#byte port_a = 5
#byte port_b = 6
//���C���֐�////////////////////////////////////
main()
{
	long int adc = 0; 
	//�|�[�g�ݒ�
	set_tris_a(0b00011001);
	set_tris_b(0b00011001);	
	//�����N���b�N�w��
	setup_oscillator(OSC_8MHZ);
	//AD�ϊ��평����
	setup_adc_ports(0b11111110);
	setup_adc(ADC_CLOCK_DIV_32);
	//�T�[�{�pPWM������
	setup_timer_1( T1_INTERNAL| T1_DIV_BY_2);	//1�J�E���g��1us������
	setup_ccp1(CCP_COMPARE_INT);		  // CCP1�R���y�A�}�b�`�����ݐݒ�
	set_timer1(TimerTIME);						//������20ms�ɐݒ�
	
	CCP_1 = 1500+TimerTIME;									  // �p���X�� 0	
	enable_interrupts(INT_CCP1);			   //CCP1�R���y�A�}�b�`�`�����݋���
	enable_interrupts(INT_TIMER1);
	enable_interrupts(GLOBAL);			  //�S�ݒ芄���݋���

	while(1)
	{
		set_adc_channel(0);  //0�`�����l�����g�p
		delay_ms(50);
		adc = read_adc();  //A/D�ϊ��l�̓ǂݍ���
		adc = adc*4;
		if(adc >=1000)
		{
			adc=1000;
		}
		CCP_1 = adc+1000+TimerTIME;	
		
		/*
			CCP_1 = 1500+TimerTIME;						  //�p���X�� 1.5mS
			delay_ms(1000);
			CCP_1 = 1000+TimerTIME;						  //�p���X�� 1.0mS
			delay_ms(1000);
			CCP_1 = 2000+TimerTIME;						  //�p���X�� 2.0mS
			delay_ms(1000);
		*/
	}
}

//�p���X�N���A///////////////////////////////////
#INT_CCP1
void ccp1_int()
{
	//SorvoPIN=LOW;
	output_low(SorvoPIN);
}

//	timer1���荞�݊֐�
#int_timer1
void intval() 
{
	
	set_timer1(TimerTIME);
	//SorvoPIN=HIGH;
	output_high(SorvoPIN);
	
}

/*	���l
������20ms�ɂ��邽�߂ɁAtimer1�̒l��45536�����Ă���̂ŁA
��������J�E���g�A�b�v�����B
�܂�T�[�{�̊p�x��0�x�ɂ��悤�Ƃ���΁A1ms�ɂ���΂悢�̂ŁA
1�J�E���g 1us�Ȃ̂Ł@1000�J�E���g�K�v
����āA45536 + 1000 �l��CCP_1�ɓ����΂悢


*/
