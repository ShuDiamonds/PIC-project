/////////////////////////////////////////////////
//�@This program is test program of standard io.
//�@PIC16F84 is target PIC and drive a LCD.
//�@The PORT for RS232 is below.
//�@�@���M=RA3�@��M=RA4
/////////////////////////////////////////////////
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

//��������main�֐�
main() {
		//�ϐ���`
		char cmnd =0; 
		
	
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
	
		while(1) 
	{					//�i�v�J��Ԃ�
		delay_ms(30);
		printf("\nCommand= ");		//Command=�\��	
		cmnd=getc();				//�P��������
		printf(" Input= ");			//Input=�\��
		putc(cmnd);					//���͕����\��
 	}
} 
