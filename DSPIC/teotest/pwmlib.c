/**********************************************************/
/*PWM�֐����C�u����                                       */
/*2012.3.9                                                */
/**********************************************************/

#include "p33Fxxxx.h"
#include "delaylib.h"


//====PWM�������֐�=============================================================
void PWM1_Init(int TimerNo)
{
	//----OC1CON�F�o�̓R���y�A1�̐��䃌�W�X�^----
	//�o�̓R���y�A1�̃��[�h�I���r�b�g
	OC1CONbits.OCM = 0;				//�o�̓R���y�A��~
	//�o�̓R���y�A1�̃A�C�h�����[�h����~�r�b�g
	OC1CONbits.OCSIDL = 0;			//�A�C�h�����[�h���ɏo�̓R���y�A�͓�����p������
	//�o�̓R���y�A1�̃^�C�}�I���r�b�g
	OC1CONbits.OCTSEL = TimerNo;	//�^�C�}�I��
	
	//----OC1R�F�o�̓R���y�A���W�X�^----
	OC1R = 0;
	
	//----OC1RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC1RS = 0;
}

void PWM2_Init(int TimerNo)
{
	//----OC2CON�F�o�̓R���y�A2�̐��䃌�W�X�^----
	//�o�̓R���y�A2�̃��[�h�I���r�b�g
	OC2CONbits.OCM = 0;				//�o�̓R���y�A��~
	//�o�̓R���y�A2�̃A�C�h�����[�h����~�r�b�g
	OC2CONbits.OCSIDL = 0;			//�A�C�h�����[�h���ɏo�̓R���y�A�͓�����p������
	//�o�̓R���y�A2�̃^�C�}�I���r�b�g
	OC2CONbits.OCTSEL = TimerNo;	//�^�C�}�I��
	
	//----OC2R�F�o�̓R���y�A���W�X�^----
	OC2R = 0;
	
	//----OC2RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC2RS = 0;
}

void PWM3_Init(int TimerNo)
{
	//----OC3CON�F�o�̓R���y�A3�̐��䃌�W�X�^----
	//�o�̓R���y�A3�̃��[�h�I���r�b�g
	OC3CONbits.OCM = 0;				//�o�̓R���y�A��~
	//�o�̓R���y�A3�̃A�C�h�����[�h����~�r�b�g
	OC3CONbits.OCSIDL = 0;			//�A�C�h�����[�h���ɏo�̓R���y�A�͓�����p������
	//�o�̓R���y�A3�̃^�C�}�I���r�b�g
	OC3CONbits.OCTSEL = TimerNo;	//�^�C�}�I��
	
	//----OC3R�F�o�̓R���y�A���W�X�^----
	OC3R = 0;
	
	//----OC3RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC3RS = 0;
}


void PWM4_Init(int TimerNo)
{
	//----OC4CON�F�o�̓R���y�A4�̐��䃌�W�X�^----
	//�o�̓R���y�A4�̃��[�h�I���r�b�g
	OC4CONbits.OCM = 0;				//�o�̓R���y�A��~
	//�o�̓R���y�A4�̃A�C�h�����[�h����~�r�b�g
	OC4CONbits.OCSIDL = 0;			//�A�C�h�����[�h���ɏo�̓R���y�A�͓�����p������
	//�o�̓R���y�A4�̃^�C�}�I���r�b�g
	OC4CONbits.OCTSEL = TimerNo;	//�^�C�}�I��
	
	//----OC4R�F�o�̓R���y�A���W�X�^----
	OC4R = 0;
	
	//----OC4RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC4RS = 0;
}


//====PWM�J�n�֐�===============================================================
void PWM1_Start(int Value)
{
	//----OC1RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC1RS = Value;
	
	//----OC1CON�F�o�̓R���y�A1�̐��䃌�W�X�^----
	//�o�̓R���y�A1�̃��[�h�I���r�b�g
	OC1CONbits.OCM = 0b110;				//�t�H���g�ی�Ȃ���PWM���[�h
}

void PWM2_Start(int Value)
{
	//----OC2RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC2RS = Value;
	
	//----OC2CON�F�o�̓R���y�A2�̐��䃌�W�X�^----
	//�o�̓R���y�A2�̃��[�h�I���r�b�g
	OC2CONbits.OCM = 0b110;				//�t�H���g�ی�Ȃ���PWM���[�h
}

void PWM3_Start(int Value)
{
	//----OC3RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC3RS = Value;
	
	//----OC3CON�F�o�̓R���y�A3�̐��䃌�W�X�^----
	//�o�̓R���y�A3�̃��[�h�I���r�b�g
	OC3CONbits.OCM = 0b110;				//�t�H���g�ی�Ȃ���PWM���[�h
}

void PWM4_Start(int Value)
{
	//----OC4RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC4RS = Value;
	
	//----OC4CON�F�o�̓R���y�A4�̐��䃌�W�X�^----
	//�o�̓R���y�A4�̃��[�h�I���r�b�g
	OC4CONbits.OCM = 0b110;				//�t�H���g�ی�Ȃ���PWM���[�h
}


//====PWM��~�֐�===============================================================
void PWM1_Stop()
{
	//----OC1CON�F�o�̓R���y�A1�̐��䃌�W�X�^----
	//�o�̓R���y�A1�̃��[�h�I���r�b�g
	OC1CONbits.OCM = 0;					//�o�̓R���y�A��~
}

void PWM2_Stop()
{
	//----OC2CON�F�o�̓R���y�A2�̐��䃌�W�X�^----
	//�o�̓R���y�A2�̃��[�h�I���r�b�g
	OC2CONbits.OCM = 0;					//�o�̓R���y�A��~
}

void PWM3_Stop()
{
	//----OC3CON�F�o�̓R���y�A3�̐��䃌�W�X�^----
	//�o�̓R���y�A3�̃��[�h�I���r�b�g
	OC3CONbits.OCM = 0;					//�o�̓R���y�A��~
}

void PWM4_Stop()
{
	//----OC4CON�F�o�̓R���y�A4�̐��䃌�W�X�^----
	//�o�̓R���y�A4�̃��[�h�I���r�b�g
	OC4CONbits.OCM = 0;					//�o�̓R���y�A��~
}


//====PWM�l�ݒ�֐�=============================================================
void PWM1_SetValue(int Value)
{
	//----OC1RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC1RS = Value;
}

void PWM2_SetValue(int Value)
{
	//----OC2RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC2RS = Value;
}

void PWM3_SetValue(int Value)
{
	//----OC3RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC3RS = Value;
}

void PWM4_SetValue(int Value)
{
	//----OC4RS�F�o�̓R���y�A�Z�J���_�����W�X�^----
	OC4RS = Value;
}



