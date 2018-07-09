///////////////////////////////////////////////
//  �t���\���퐧�䃉�C�u�����@for PIC18Fxxxx
//  �����֐��͈ȉ�
//    lcd_init()    ----- ������
//    lcd_cmd(cmd)  ----- �R�}���h�o��
//    lcd_data(chr) ----- �P�����\���o��
//    lcd_clear()   ----- �S����
//////////////////////////////////////////////
#include <p18cxxx.h>
#include "io_cfg.h"
#include	"delays.h"
#include "lcd_lib.h"

//////// �f�[�^�o�̓T�u�֐�
void lcd_out(char code, char flag)
{
	lcd_port = code & 0xF0;
	if (flag == 0)
		lcd_rs = 1;			//�\���f�[�^�̏ꍇ
	else
		lcd_rs = 0;			//�R�}���h�f�[�^�̏ꍇ
	Delay1TCY();				//NOP		
	lcd_stb = 1;				//strobe out
	Delay10TCYx(1);			//10NOP
	lcd_stb = 0;				//reset strobe
}
//////// �P�����\���֐�
void lcd_data(char asci)
{
	lcd_out(asci, 0);			//��ʂS�r�b�g�o��
	lcd_out(asci<<4, 0);		//���ʂS�r�b�g�o��
	Delay10TCYx(50);			//50��sec�҂�
}
/////// �R�}���h�o�͊֐�
void lcd_cmd(char cmd)
{
	lcd_out(cmd, 1);			//��ʂS�r�b�g�o��
	lcd_out(cmd<<4, 1);		//���ʂS�r�b�g�o��
	if((cmd & 0x03) != 0)
		Delay10KTCYx(2);		//2msec�҂�
	else
		Delay10TCYx(50);		//50usec�҂�
}
/////// �S�����֐�
void lcd_clear(void)
{
	lcd_cmd(0x01);			//�������R�}���h�o��
}

/////// ������o�͊֐�
void lcd_str(char *str)
{
	while(*str != 0x00)			//������̏I��蔻��
	{
		lcd_data(*str);			//������P�����o��
		str++;					//�|�C���^�{�P
	}
}

/////// �������֐�
void lcd_init(void)
{
	Delay10KTCYx(20);
	lcd_out(0x30, 1);			//8bit mode set
	Delay10KTCYx(5);
	lcd_out(0x30, 1);			//8bit mode set
	Delay10KTCYx(1);
	lcd_out(0x30, 1);			//8bit mode set
	Delay10KTCYx(1);
	lcd_out(0x20, 1);			//4bit mode set
	Delay10KTCYx(1);
	lcd_cmd(0x2E);			//DL=0 4bit mode
	lcd_cmd(0x08);			//display off C=D=B=0
	lcd_cmd(0x0D);			//display on C=D=1 B=0
	lcd_cmd(0x06);			//entry I/D=1 S=0
	lcd_cmd(0x01);			//all clear
}
