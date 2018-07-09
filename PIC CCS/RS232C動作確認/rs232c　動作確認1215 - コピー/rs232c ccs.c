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
main() {
		//�ϐ���`
		char  cmnd =0; 
		
	
		//������
		set_tris_a(TRIS_A);
		set_tris_b(TRIS_B);
		

	printf("Hello world\n\r");
		while(1) 
		{					//�i�v�J��Ԃ�
			delay_ms(30);
			printf("\nCommand= ");		//Command=�\��	
			cmnd=getc();				//�P��������
			printf(" Input= ");			//Input=�\��
			putc(cmnd);					//���͕����\��
		}
} 
