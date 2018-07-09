#include<16f877a.h>
#include<stdio.h>

//#define RS_BAUD		115200
#define RS_BAUD		9600
#define RS_TX		PIN_C6
#define RS_RX		PIN_C7
#define RUN_LED		PIN_C0
//�҂�}�N��
//�S�Ί������[�^�[I/O
	#define	TEKKA_Mortor1_IO_R	PIN_A2
	#define	TEKKA_Mortor1_IO_L	PIN_A4
	#define	TEKKA_LED			PIN_E1
	#define	TEKKA_NOPIN			PIN_E0
	#define	TEKKA_Mortor2_IO_R	PIN_A3
	#define	TEKKA_Mortor2_IO_L	PIN_A5
	//���[�^�[PWM
	#define	TEKKA_Mortor1_PWM	PIN_C1
	#define	TEKKA_Mortor2_PWM	PIN_C2
	//�����ϊ������[�^�[I/O
	#define	KAPPA_LED1			PIN_D5	
	#define	KAPPA_LED2			PIN_D4	
	#define	KAPPA_Mortor1_IO_R	PIN_D0	
	#define	KAPPA_Mortor1_IO_L	PIN_C3
	#define	KAPPA_Mortor2_IO_R	PIN_D2	
	#define	KAPPA_Mortor2_IO_L	PIN_D3	
	#define	KAPPA_Mortor3_IO_R	PIN_C4	
	#define	KAPPA_Mortor3_IO_L	PIN_C5	
	#define	KAPPA_Mortor4_IO_R	PIN_D1	
	#define	KAPPA_Mortor4_IO_L	PIN_A0	

#use delay(clock = 20000000)
#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,BROWNOUT
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)  //rs232c�ݒ�
/*
#use fast_io(a)
#use fast_io(b)
#use fast_io(c)
#use fast_io(d)
#use fast_io(e)
*/
#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9
#byte ADCON1 = 0x9F			//�A�i���O�f�W�^���s���ݒ�	
main()
{
	//���[�J���ϐ���`
	char cheaker=0;	
	int data_H[20];
	int data_L[20];		//��M�f�[�^�i�[�X�y�[�X
	int16 E=0,F=0;				
	int ID=0,hugou=0,i=0;
	int motasuu=0;
	
	//������
	set_tris_a(0x01);
	set_tris_b(0x00);
	set_tris_c(0x80);
	set_tris_d(0x00);
	set_tris_e(0x00);
	ADCON1 = 0b00000111;		//�f�W�^���s���ݒ�	
	//PWM������
	setup_ccp1(CCP_PWM);
	setup_ccp2(CCP_PWM);
	SETUP_TIMER_2(T2_DIV_BY_16,0xFF,1);
	
	//���[�^�[������
	//�S�Ί���������
	set_pwm1_duty(0);
	set_pwm2_duty(0);
	output_low(TEKKA_Mortor1_IO_L);
	output_low(TEKKA_Mortor1_IO_R);
	output_low(TEKKA_Mortor2_IO_L);
	output_low(TEKKA_Mortor2_IO_R);
	//�����ϊ���������
	output_low(KAPPA_Mortor1_IO_L);
	output_low(KAPPA_Mortor1_IO_R);
	output_low(KAPPA_Mortor2_IO_L);
	output_low(KAPPA_Mortor2_IO_R);
	output_low(KAPPA_Mortor3_IO_L);
	output_low(KAPPA_Mortor3_IO_R);
	output_low(KAPPA_Mortor4_IO_L);
	output_low(KAPPA_Mortor4_IO_R);
	
	
	
	output_low(RUN_LED);   //����m�F
	output_high(TEKKA_LED);   //����m�F
	output_high(KAPPA_LED1);   //����m�F
	output_high(KAPPA_LED2);   //����m�F
	delay_ms(30);
	while(1)
	{
		while(1)
		{	
		//�X�^�[�g�f�[�^�҂�
			while(!cheaker == '@')
			{
				cheaker = getc();
			}
		//���[�^�̐����m�F
			motasuu = getc();
		//�f�[�^��M
			for(i=0;motasuu > i;i++)
			{
				data_H[i] =getc();
				data_L[i] =getc();
			}
		//�X�g�b�v�f�[�^�m�F
			cheaker = getc();
			if(cheaker == '*')
			{
				break;
			}
		//�f�[�^����
			for(i=0;motasuu == i;i++)
				{
					//ID�̎��o��
					ID = data_H[i] & 0b11111000;
					ID = ID>>3;
					//�������o��
					hugou = data_H[i] & 0b0000100;
					//PWM�f�[�^���o��
					F = data_H[i] & 0b0000011;		//PWM���2bit���o��
					F = F<<8;						//PWM�f�[�^��8�r�b�g���ɃV�t�g
					E = data_L[i];					
					F = F | E;						//PWM�̃f�[�^�̏�ʂƉ��ʂ�OR����(F��PWM�p�̃f�[�^���͂����Ă�)
					
					//PWM�f�[�^����
					if(F <=370 && F != 0)			//�X�g�b�v�ł͂Ȃ��APWM��370�ȉ��̂Ƃ�
					{
						F = 420;
					}
					
				//�f�[�^���s
					switch (ID)						//���[�^�[����
					{
				case 0b00000001:					//�����σ��[�^�[�P
						if(F)						//���[�^�[����������
						{
							if(hugou)
							{
								printf("�����σ��[�^�[�P_R\n\r");
								output_high(KAPPA_Mortor1_IO_R);
								output_low(KAPPA_Mortor1_IO_L);
							}else if(!hugou)
							{
								printf("�����σ��[�^�[�P_L\n\r");
								output_high(KAPPA_Mortor1_IO_L);
								output_low(KAPPA_Mortor1_IO_R);
							}
						}else						//���[�^�[�𓮂����Ȃ��Ƃ�
						{
								printf("�����σ��[�^�[�P_else\n\r");
								output_low(KAPPA_Mortor1_IO_L);
								output_low(KAPPA_Mortor1_IO_R);
						}
					break;
				case 0b00000010:					//�����σ��[�^�[�Q
						if(F)						//���[�^�[����������
						{
							if(hugou)
							{
								printf("�����σ��[�^�[2_R\n\r");
								output_high(KAPPA_Mortor2_IO_R);
								output_low(KAPPA_Mortor2_IO_L);
							}else if(!hugou)
							{
								printf("�����σ��[�^�[2_L\n\r");
								output_high(KAPPA_Mortor2_IO_L);
								output_low(KAPPA_Mortor2_IO_R);
							}
						}else						//���[�^�[�𓮂����Ȃ��Ƃ�
						{
								printf("�����σ��[�^�[2_else\n\r");
								output_low(KAPPA_Mortor2_IO_L);
								output_low(KAPPA_Mortor2_IO_R);
						}
					break;
				case 0b00000011:					//�����σ��[�^�[�R
						if(F)						//���[�^�[����������
						{
							printf("�����σ��[�^�[3_R\n\r");
							if(hugou)
							{
								output_high(KAPPA_Mortor3_IO_R);
								output_low(KAPPA_Mortor3_IO_L);
							}else if(!hugou)
							{
								printf("�����σ��[�^�[3_L\n\r");
								output_high(KAPPA_Mortor3_IO_L);
								output_low(KAPPA_Mortor3_IO_R);
							}
						}else						//���[�^�[�𓮂����Ȃ��Ƃ�
						{
								printf("�����σ��[�^�[3_else\n\r");
								output_low(KAPPA_Mortor3_IO_L);
								output_low(KAPPA_Mortor3_IO_R);
						}
					break;
				case 0b00000100:					//�����σ��[�^�[�S
						if(F)						//���[�^�[����������
						{
							if(hugou)
							{
								printf("�����σ��[�^�[4_R\n\r");
								output_high(KAPPA_Mortor4_IO_R);
								output_low(KAPPA_Mortor4_IO_L);
							}else if(!hugou)
							{
								printf("�����σ��[�^�[4_L\n\r");
								output_high(KAPPA_Mortor4_IO_L);
								output_low(KAPPA_Mortor4_IO_R);
							}
						}else						//���[�^�[�𓮂����Ȃ��Ƃ�
						{
								printf("�����σ��[�^�[4_else\n\r");
								output_low(KAPPA_Mortor4_IO_L);
								output_low(KAPPA_Mortor4_IO_R);
						}
					break;
				case 0b00000101:					//�S�Ί������[�^�[�P
						if(F)						//���[�^�[����������
						{
							if(hugou)
							{
								printf("tekka1_R\n\r");
								output_high(TEKKA_Mortor1_IO_R);
								output_low(TEKKA_Mortor1_IO_L);
								set_pwm1_duty(F);
							}else if(!hugou)
							{
								printf("tekka�P_L\n\r");
								output_high(TEKKA_Mortor1_IO_L);
								output_low(TEKKA_Mortor1_IO_R);
								set_pwm1_duty(F);
							}
						}else						//���[�^�[�𓮂����Ȃ��Ƃ�
						{
							printf("tekka�P_else\n\r");
							set_pwm1_duty(0);
							output_low(TEKKA_Mortor1_IO_L);
							output_low(TEKKA_Mortor1_IO_R);
						}
					break;
				case 0b00000110:					//�S�Ί������[�^�[�Q
						if(F)						//���[�^�[����������
						{
							if(hugou)
							{
								printf("tekka2_R\n\r");
								output_high(TEKKA_Mortor2_IO_R);
								output_low(TEKKA_Mortor2_IO_L);
								set_pwm2_duty(F);
							}else if(!hugou)
							{
								printf("tekka2_L\n\r");
								output_high(TEKKA_Mortor2_IO_L);
								output_low(TEKKA_Mortor2_IO_R);
								set_pwm2_duty(F);
							}
						}else						//���[�^�[�𓮂����Ȃ��Ƃ�
						{
							printf("tekka2_else\n\r");
							set_pwm2_duty(0);
							output_low(TEKKA_Mortor2_IO_L);
							output_low(TEKKA_Mortor2_IO_R);
						}
					break;
				default:
					break;
					
					}
					
				}
		//�ϐ�������
			cheaker=0;
			motasuu=0;
		}
	}
	return(0);
}