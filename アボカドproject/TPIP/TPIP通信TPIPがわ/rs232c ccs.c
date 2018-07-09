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


#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,NOBROWNOUT
//�ڍׂ͈�ԉ��̃��������ɂ�


//�N���b�N���x�̎w��i20MHz)
#use delay(clock = 20000000)

//RS232C�̐ݒ�
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)

//�����ݒ�
#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9

//��������main�֐�
main()
{
	//�ϐ���`
	int cmnd =0; 
	char data[7];
	
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
	
	//�Q�o�C�g�f�[�^�̂Ƃ�
	data[0] = '@';
	data[1] = 0x02;
	data[2] = 0x09;
	data[3] = 0x04;
	data[4] = 0x11;
	data[5] = 0x04;
	data[6] = '*';
	
	
	
		while(1) {					//�i�v�J��Ԃ�
			
			
			/*		�Q�o�C�g�f�[�^
			delay_ms(1000);
			putc(data[0]);
			putc(data[1]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[6]);
			*/
		
			
			/*
			//�P�o�C�g�f�[�^�̂Ƃ�
			delay_ms(1000);
			putc(data[0]);
			putc(data[1]);
			putc(data[2]);
			putc(data[3]);
			putc(data[6]);
			
			*/
			
			data[1] = 0x08;
			
			//		8�o�C�g�f�[�^
			delay_ms(500);
			putc(data[0]);
			putc(data[1]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[2]);
			putc(data[3]);
			putc(data[4]);
			putc(data[5]);
			putc(data[6]);
			
			
 }
} 
