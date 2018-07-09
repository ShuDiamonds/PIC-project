/********************************************************************************/
/*																				*/
/*			���Z�b�g��̏����� ����												*/
/*																				*/
/*			Cpu         :: PIC12F683											*/
/*			Language    :: PICC Lite											*/
/*			File Name   :: init.c												*/
/*																				*/
/********************************************************************************/
#include <pic.h>
#include <conio.h>

#include "pic12f683.h"

void			motor_init(void);
void			motor_out(unsigned char , unsigned char) ;
unsigned char	motor_flag(void);

/****************************************************************************/
/*																			*/
/*	���[�^�o�� �������iTimer 2 & CCP1 �g�p�j								*/
/*																			*/
/****************************************************************************/
void motor_init()
{
	CH_T2CON   = 0b00000000 ;	/* Timer2����U ���g�p�ɂ���					*/
	CH_CCP1CON = 0b00000000 ;	/* CCP1����U ���g�p�ɂ���						*/

// �����F120��s �� (PR2�{1)�~{1��(�����ۯ�8MHz��4)}
	CH_PR2     = 240-1 ;		/* PWM�����ݒ�									*/
	CH_CCPR1L  = 0 ;			/* PWM�o�� ���8Bit�ر							*/
	CH_CCP1CON &= ~0x30 ;		/* PWM�o�� ����2Bit�ر							*/
	BI_TRISIO2  = 0 ;			/* GP2(PWM)���o�͂ɐݒ�							*/

	CH_T2CON   = 0b00000100 ;	/*Bit7:        :-:								*/
								/*Bit6:TOUTPS3 :0:�߽Ľ��� 1:1					*/
								/*Bit5:TOUTPS2 :0:  �V							*/
								/*Bit4:TOUTPS1 :0:  �V							*/
								/*Bit3:TOUTPS0 :0:  �V							*/
								/*Bit2:TMR2ON  :1:��ϰ2����						*/
								/*Bit1:T2CKPS1 :0:��ؽ��� 1						*/
								/*Bit0:T2CKPS0 :0:  �V							*/

	CH_CCP1CON = 0b00001100 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:DC1B1   :0:PWM LSB1						*/
								/*Bit4:DC1B0   :0:PWM LSB0						*/
								/*Bit3:CCP1M3  :1:PWMӰ��						*/
								/*Bit2:CCP1M2  :1:  �V							*/
								/*Bit1:CCP1M1  :0:x �V							*/
								/*Bit0:CCP1M0  :0:x �V							*/

}

/****************************************************************************/
/*																			*/
/*	���[�^�o�͏���															*/
/*																			*/
/****************************************************************************/
void motor_out(unsigned char pwm_do , unsigned char pwm_out)
{
	CH_GPIO = pwm_do & 0x03 ;
	CH_CCPR1L = pwm_out ;
}

/****************************************************************************/
/*																			*/
/*	TMR2�FPR2 �R���y�A�}�b�`�t���O �`�F�b�N����								*/
/*																			*/
/****************************************************************************/
unsigned char motor_flag()
{
	unsigned char	wk ;

	if(BI_TMR2IF == 0){
		wk = 0 ;							/* �R���y�A�}�b�`����			*/
	}else{
		wk = 1 ;							/* �R���y�A�}�b�`����			*/
		BI_TMR2IF = 0 ;						/* �t���O �N���A				*/
	}
	return(wk) ;
}
