//�w�b�_�t�@�C���̓ǂݍ���
#include <16f88.h>		
#include <stdio.h>			//�W�����o�͂̃w�b�_�t�@�C��

// ���o�̓s���̐ݒ�
#define TRIS_A	0x00		//ALL,OUT
#define TRIS_B	0x04		//ALL,OUT

//�s����define
#define RUN_LED PIN_A1

//RS232C�̐ݒ�R�}���h
#define RS_BAUD		9600	//Baud-Reat��9600bps
#define RS_TX		PIN_B5	//TX�s����PIN_C6
#define RS_RX		PIN_B2	//RX�s����PIN_C7


//�R���t�B�M�����[�V�����r�b�g�̐ݒ�
#fuses HS,NOWDT,NOPUT,NOPROTECT,NOBROWNOUT,NOLVP
//�ڍׂ͈�ԉ��̃��������ɂ�


//�N���b�N���x�̎w��i20MHz)
#use delay(clock = 20000000)

//RS232C�̐ݒ�
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)

//�����ݒ�




//��������main�֐�
main()
{
	long int i = 0;
	char j = 0;

		
	
		//������
		set_tris_a(TRIS_A);
		set_tris_b(TRIS_B);
		

	printf("start");
	output_low(RUN_LED);						//����m�F�pLED�����炷
	delay_ms(50);
	while(1)
	{
			delay_ms(1000);
		output_high(RUN_LED);
			printf("yahoo");
		delay_ms(1000);
		output_low(RUN_LED);
		//j = getc();
		printf("yahoo");
		//printf("Hell, sofmap would!!No.%ld\n\r",i);
		//i++;
	}
}




/*���܂�߄t�)���� ����...
�R���t�B�O���[�V�����r�b�g�ɂ���
HS(High Speed)
NOWDT(no-Watch Dog Time)
NOPROTECT
NOLVP(no Low-Voltage-Programming)
PUT(Power-Up-Timer)
BROWNOUT*/