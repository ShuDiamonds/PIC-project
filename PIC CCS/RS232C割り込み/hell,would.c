//�w�b�_�t�@�C���̓ǂݍ���
#include <16f877a.h>		//16F873�̃w�b�_�t�@�C��
#include <stdio.h>			//�W�����o�͂̃w�b�_�t�@�C��

// ���o�̓s���̐ݒ�
#define TRIS_A	0x00		//ALL,OUT
#define TRIS_B	0x00		//ALL,OUT
#define TRIS_C	0x80		//PIN_C7:IN
#define TRIS_D	0x00		//ALL,OUT
#define TRIS_E	0x00		//ALL,OUT

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

//�O���[�o���ϐ���`
char data = 0;

//RS232C���荞��
#int_rda
void isr_rcv()
{
	data = getc();
	return;
}
//��������main�֐�
void main(void)
{
	long int i = 0;
	char j = 0;

	//������
	set_tris_a(TRIS_A);
	set_tris_b(TRIS_B);
	set_tris_c(TRIS_C);
	set_tris_d(TRIS_D);
	set_tris_e(TRIS_E);
	
	port_a = 0;
	port_b = 0;
	//port_c = 0;
	port_d = 0;
	port_e = 0;
	//���荞�݋���
	enable_interrupts(int_rda);
	enable_interrupts(global);
	printf("start\n\r");
	output_low(RUN_LED);						//����m�F�pLED�����炷
	delay_ms(50);
	while(1)
	{
			delay_ms(1000);
		output_high(RUN_LED);
			printf("\n\rdata = ");
		putc(data);
		delay_ms(1000);
		output_low(RUN_LED);
	}
}

//�Q�lURL	http://amahime.main.jp/sirial/main.php?name=siri


/*���܂�߄t�)���� ����...
�R���t�B�O���[�V�����r�b�g�ɂ���
HS(High Speed)
NOWDT(no-Watch Dog Time)
NOPROTECT
NOLVP(no Low-Voltage-Programming)
PUT(Power-Up-Timer)
BROWNOUT*/