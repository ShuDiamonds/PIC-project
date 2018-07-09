#include<16f877a.h>
#include<stdio.h>

//#define RS_BAUD		115200
#define RS_BAUD		9600
#define RS_TX		PIN_C6
#define RS_RX		PIN_C7
#define RUN_LED		PIN_C0

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
	set_pwm1_duty(700);
	
	output_low(RUN_LED);   //����m�F
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
			for(i=0;motasuu == i;i++)
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
					
				//�f�[�^���s
					switch (ID)						//���[�^�[����
					{
				case 0x00000001:
					break;
				case 0x00000010:
					break;
				case 0x00000011:
					break;
				case 0x00000100:
					break;
				case 0x00000101:
					break;
				case 0x00000110:
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