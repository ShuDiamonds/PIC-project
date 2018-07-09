//�w�b�_�t�@�C���̓ǂݍ���
#include <16f877a.h>		//16F873�̃w�b�_�t�@�C��
#include <stdio.h>			//�W�����o�͂̃w�b�_�t�@�C��

//�s����define
#define RUN_LED PIN_C3

//RS232C�̐ݒ�R�}���h
#define RS_BAUD		9600	//Baud-Reat��9600bps
#define RS_TX		PIN_C6	//TX�s����PIN_C6
#define RS_RX		PIN_C7	//RX�s����PIN_C7


//�R���t�B�M�����[�V�����r�b�g�̐ݒ�
#fuses HS,NOWDT,NOPROTECT
//�ڍׂ͈�ԉ��̃��������ɂ�


//�N���b�N���x�̎w��i20MHz)
#use delay(clock = 20000000)

//RS232C�̐ݒ�
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)

//�����ݒ�

#use fast_io(a)
#use fast_io(b)
#use fast_io(c)
#use fast_io(d)
#use fast_io(e)

#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9
//����PWM�֐��ݒ�
		#define	TimerPWMcount	15				//200.200usec�����Ɋ��荞��
	//	#warning	�N���b�N���x20MHz
//�O���[�o���ϐ���`
unsigned int PWM_duty01=0, PWM_duty02=0, PWM_duty03=0, PWM_duty04=0, PWM_duty05=0, PWM_duty06=0, PWM_duty07=0, PWM_duty08=0;
unsigned int PWM_duty_time = 1;
//PWM�s���}�N��
#define	PIN_PWM01	PIN_B0
#define	PIN_PWM02	PIN_B1
#define	PIN_PWM03	PIN_B2
#define	PIN_PWM04	PIN_B3
#define	PIN_PWM05	PIN_B4
#define	PIN_PWM06	PIN_B5
#define	PIN_PWM07	PIN_B6
#define	PIN_PWM08	PIN_B7


#INT_RTCC
rtcc_isr()
{
	
	set_timer0(TimerPWMcount);				//200.000usec�����Ɋ��荞��
	PWM_duty_time++;
	if(PWM_duty_time>=PWM_duty01) 
	{	output_low(PIN_PWM01); } 
	if(PWM_duty_time>=PWM_duty02) 
	{	output_low(PIN_PWM02); } 
	if(PWM_duty_time>=PWM_duty03) 
	{	output_low(PIN_PWM03); } 
	if(PWM_duty_time>=PWM_duty04) 
	{	output_low(PIN_PWM04); } 
	if(PWM_duty_time>=PWM_duty05) 
	{	output_low(PIN_PWM05); } 
	if(PWM_duty_time>=PWM_duty06) 
	{	output_low(PIN_PWM06); } 
	if(PWM_duty_time>=PWM_duty07) 
	{	output_low(PIN_PWM07); } 
	if(PWM_duty_time>=PWM_duty08) 
	{	output_low(PIN_PWM08); } 
	
	if(PWM_duty_time >= 100)
	{	
		PWM_duty_time=1;	//�^�C�}�[������
		output_high(PIN_PWM01);
		output_high(PIN_PWM02);
		output_high(PIN_PWM03);
		output_high(PIN_PWM04);
		output_high(PIN_PWM05);
		output_high(PIN_PWM06);
		output_high(PIN_PWM07);
		output_high(PIN_PWM08);
	}
}
main()
{
	//������
	set_tris_a(0);
	set_tris_b(0);
	set_tris_c(0b0100000);
	set_tris_d(0);
	set_tris_e(0);
	port_a = 0;
	port_b = 0;
	//port_c = 0;
	port_d = 0;
	port_e = 0;
	output_low(RUN_LED);						//����m�F�pLED�����炷
	//����PWM������
	output_high(PIN_PWM01);
	output_high(PIN_PWM02);
	output_high(PIN_PWM03);
	output_high(PIN_PWM04);
	output_high(PIN_PWM05);
	output_high(PIN_PWM06);
	output_high(PIN_PWM07);
	output_high(PIN_PWM08);
	//�^�C�}�[������
	setup_counters(RTCC_8_BIT,RTCC_DIV_4);
	
	set_timer0(TimerPWMcount);						//�J�E���g�l�̃��[�h
	enable_interrupts(INT_RTCC);				//timer0���荞�݋���
	enable_interrupts(GLOBAL);					//�O���[�o�����荞�݋���
	while(1)
	{
		PWM_duty01=100;
		PWM_duty02=50;
		PWM_duty03=10;
		PWM_duty04=1;
		PWM_duty05=0;
		PWM_duty06=0;
		PWM_duty07=0;
		PWM_duty08=0;
	}
}

