#include<16f877a.h>

#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,BROWNOUT
#use delay(clock = 20000000)

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
//�O���[�o���ϐ���`
	int l = 0;	//�^�C�}�[�J�E���g�l


#int_timer1
void intval() {
	set_timer1(0x0BDB);			//�J�E���g�l���
	l++;
	
}


main()
{ 
	int a=0;
	set_tris_a(0xff);
	set_tris_b(0xff);
	set_tris_c(0xf0);
	set_tris_d(0x00);
	set_tris_e(0xff);
	
	//PWM�ݒ�
	setup_ccp1(CCP_PWM);
	setup_ccp2(CCP_PWM);
	setup_timer_2(T2_DIV_BY_16,0xFF,1);
	//set_pwm1_duty(0);
	output_low(PIN_C0);
	//Timer1�ݒ�
	SETUP_TIMER_1(T1_INTERNAL|T1_DIV_BY_8);	//�����N���b�N�g�p�@�v���X�P�[���W
	//���荞�݋���
	enable_interrupts(int_timer1);
	enable_interrupts(GLOBAL);

	while(1)
		{
			 
			if(l==10)			//1�b��������
			{
				l=0;
				
				port_c = port_c^0b00000001;
			}
		}
}

