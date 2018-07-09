/**********************************************************/
/*�^�C�}�֐����C�u����                                    */
/*2011.12.30                                              */
/**********************************************************/
#include "p33Fxxxx.h"
#include "timerlib.h"

//====Timer1������==============================================================
void Timer1_Init(int Prescaler)
{
	//----T1CON�F�^�C�vA�^�C�}���䃌�W�X�^----
	T1CONbits.TON = 0;				//Timer1���~����
	T1CONbits.TSIDL = 0;			//�A�C�h�����[�h�����^�C�}������p������
	T1CONbits.TGATE = 0;			//�Q�[�g���Ԃ̐ώZ�͖���
	T1CONbits.TCKPS = Prescaler;	//�v���X�P�[���l�I��
	T1CONbits.TSYNC = 0;			//�O���N���b�N�����A�����N���b�N�g�p����̂Ŗ���
	T1CONbits.TCS = 0;				//�����N���b�N(Tosc/2)�I��
	//----TMR1�F�^�C�}�J�E���g���W�X�^----
	TMR1 = 0;
	//----PR1�F�^�C�}�������W�X�^----
	PR1 = 9;
}

//====Timer2������==============================================================
void Timer2_Init(int Prescaler)
{
	//----T2CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T2CONbits.TON = 0;				//Timer2���~����
	T2CONbits.TSIDL = 0;			//�A�C�h�����[�h�����^�C�}������p������
	T2CONbits.TGATE = 0;			//�Q�[�g���Ԃ̐ώZ�͖���
	T2CONbits.TCKPS = Prescaler;	//�v���X�P�[���l�I��
	T2CONbits.T32 = 0;				//��16bit�^�C�}�Ƃ��Ďg�p
	T2CONbits.TCS = 0;				//�����N���b�N(Tosc/2)�I��
	//----TMR2�F�^�C�}�J�E���g���W�X�^----
	TMR2 = 0;
	//----PR2�F�^�C�}�������W�X�^----
	PR2 = 9;
}

//====Timer3������==============================================================
void Timer3_Init(int Prescaler)
{
	//----T3CON�F�^�C�vC�^�C�}���䃌�W�X�^----
	T3CONbits.TON = 0;				//Timer3���~����
	T3CONbits.TSIDL = 0;			//�A�C�h�����[�h�����^�C�}������p������
	T3CONbits.TGATE = 0;			//�Q�[�g���Ԃ̐ώZ�͖���
	T3CONbits.TCKPS = Prescaler;	//�v���X�P�[���l�I��
	T3CONbits.TCS = 0;				//�����N���b�N(Tosc/2)�I��
	//----TMR3�F�^�C�}�J�E���g���W�X�^----
	TMR3 = 0;
	//----PR3�F�^�C�}�������W�X�^----
	PR3 = 9;
}

//====Timer4������==============================================================
void Timer4_Init(int Prescaler)
{
	//----T4CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T4CONbits.TON = 0;				//Timer4���~����
	T4CONbits.TSIDL = 0;			//�A�C�h�����[�h�����^�C�}������p������
	T4CONbits.TGATE = 0;			//�Q�[�g���Ԃ̐ώZ�͖���
	T4CONbits.TCKPS = Prescaler;	//�v���X�P�[���l�I��
	T4CONbits.T32 = 0;				//��16bit�^�C�}�Ƃ��Ďg�p
	T4CONbits.TCS = 0;				//�����N���b�N(Tosc/2)�I��
	//----TMR4�F�^�C�}�J�E���g���W�X�^----
	TMR4 = 0;
	//----PR4�F�^�C�}�������W�X�^----
	PR4 = 9;
}

//====Timer5������==============================================================
void Timer5_Init(int Prescaler)
{
	//----T5CON�F�^�C�vC�^�C�}���䃌�W�X�^----
	T5CONbits.TON = 0;				//Timer5���~����
	T5CONbits.TSIDL = 0;			//�A�C�h�����[�h�����^�C�}������p������
	T5CONbits.TGATE = 0;			//�Q�[�g���Ԃ̐ώZ�͖���
	T5CONbits.TCKPS = Prescaler;	//�v���X�P�[���l�I��
	T5CONbits.TCS = 0;				//�����N���b�N(Tosc/2)�I��
	//----TMR5�F�^�C�}�J�E���g���W�X�^----
	TMR5 = 0;
	//----PR5�F�^�C�}�������W�X�^----
	PR5 = 9;
}

//====Timer23������=============================================================
void Timer23_Init(int Prescaler)
{
	//----T2CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T2CONbits.TON = 0;				//Timer2���~����
	T2CONbits.TSIDL = 0;			//�A�C�h�����[�h�����^�C�}������p������
	T2CONbits.TGATE = 0;			//�Q�[�g���Ԃ̐ώZ�͖���
	T2CONbits.TCKPS = Prescaler;	//�v���X�P�[���l�I��
	T2CONbits.T32 = 1;				//�^�C�}2,3��32bit�^�C�}�Ƃ��Ďg�p
	T2CONbits.TCS = 0;				//�����N���b�N(Tosc/2)�I��
	//----TMR2,3�F�^�C�}�J�E���g���W�X�^----
	TMR2 = 0;
	TMR3 = 0;
	//----PR2, 3�F�^�C�}�������W�X�^----
	PR2 = 18;
	PR3 = 18;
}

//====Timer45������=============================================================
void Timer45_Init(int Prescaler)
{
	//----T4CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T4CONbits.TON = 0;				//Timer4���~����
	T4CONbits.TSIDL = 0;			//�A�C�h�����[�h�����^�C�}������p������
	T4CONbits.TGATE = 0;			//�Q�[�g���Ԃ̐ώZ�͖���
	T4CONbits.TCKPS = Prescaler;	//�v���X�P�[���l�I��
	T4CONbits.T32 = 1;				//�^�C�}4,5��32bit�^�C�}�Ƃ��Ďg�p
	T4CONbits.TCS = 0;				//�����N���b�N(Tosc/2)�I��
	//----TMR4,5�F�^�C�}�J�E���g���W�X�^----
	TMR4 = 0;
	TMR5 = 0;
	//----PR4, 5�F�^�C�}�������W�X�^----
	PR4 = 18;
	PR5 = 18;
}

//====Timer1���荞�݋��֐�====================================================
void Timer1_IntEnable(int Level)
{
	//----�^�C�}���荞�݋���----
	IPC0bits.T1IP = Level;			//�^�C�}���荞�݃��x���ݒ�
	IFS0bits.T1IF = 0;				//�^�C�}���荞�݃t���O�N���A
	IEC0bits.T1IE = 1;				//�^�C�}���荞�݋���
}

//====Timer2���荞�݋��֐�====================================================
void Timer2_IntEnable(int Level)
{
	//----�^�C�}���荞�݋���----
	IPC1bits.T2IP = Level;			//�^�C�}���荞�݃��x���ݒ�
	IFS0bits.T2IF = 0;				//�^�C�}���荞�݃t���O�N���A
	IEC0bits.T2IE = 1;				//�^�C�}���荞�݋���
}

//====Timer3���荞�݋��֐�====================================================
void Timer3_IntEnable(int Level)
{
	//----�^�C�}���荞�݋���----
	IPC2bits.T3IP = Level;			//�^�C�}���荞�݃��x���ݒ�
	IFS0bits.T3IF = 0;				//�^�C�}���荞�݃t���O�N���A
	IEC0bits.T3IE = 1;				//�^�C�}���荞�݋���
}

//====Timer4���荞�݋��֐�====================================================
void Timer4_IntEnable(int Level)
{
	//----�^�C�}���荞�݋���----
	IPC6bits.T4IP = Level;			//�^�C�}���荞�݃��x���ݒ�
	IFS1bits.T4IF = 0;				//�^�C�}���荞�݃t���O�N���A
	IEC1bits.T4IE = 1;				//�^�C�}���荞�݋���
}

//====Timer5���荞�݋��֐�====================================================
void Timer5_IntEnable(int Level)
{
	//----�^�C�}���荞�݋���----
	IPC7bits.T5IP = Level;			//�^�C�}���荞�݃��x���ݒ�
	IFS1bits.T5IF = 0;				//�^�C�}���荞�݃t���O�N���A
	IEC1bits.T5IE = 1;				//�^�C�}���荞�݋���
}

//====Timer1���荞�݋֎~�֐�====================================================
void Timer1_IntDisable(void)
{
	IEC0bits.T1IE = 0;				//�^�C�}���荞�݋֎~
}

//====Timer2���荞�݋֎~�֐�====================================================
void Timer2_IntDisable(void)
{
	IEC0bits.T2IE = 0;				//�^�C�}���荞�݋֎~
}

//====Timer3���荞�݋֎~�֐�====================================================
void Timer3_IntDisable(void)
{
	IEC0bits.T3IE = 0;				//�^�C�}���荞�݋֎~
}

//====Timer4���荞�݋֎~�֐�====================================================
void Timer4_IntDisable(void)
{
	IEC1bits.T4IE = 0;				//�^�C�}���荞�݋֎~
}

//====Timer5���荞�݋֎~�֐�====================================================
void Timer5_IntDisable(void)
{
	IEC1bits.T5IE = 0;				//�^�C�}���荞�݋֎~
}

//====Timer1�J�n�֐�============================================================
void Timer1_Start(int Value)
{
	//----TMR1�F�^�C�}�J�E���g���W�X�^----
	TMR1 = 0;
	//----PR1�F�^�C�}�������W�X�^----
	PR1 = Value;
	//----T1CON�F�^�C�vA�^�C�}���䃌�W�X�^----
	T1CONbits.TON = 1;				//Timer1���J�n����
}

//====Timer2�J�n�֐�============================================================
void Timer2_Start(int Value)
{
	//----TMR2�F�^�C�}�J�E���g���W�X�^----
	TMR2 = 0;
	//----PR2�F�^�C�}�������W�X�^----
	PR2 = Value;
	//----T2CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T2CONbits.TON = 1;				//Timer2���J�n����
}

//====Timer3�J�n�֐�============================================================
void Timer3_Start(int Value)
{
	//----TMR3�F�^�C�}�J�E���g���W�X�^----
	TMR3 = 0;
	//----PR3�F�^�C�}�������W�X�^----
	PR3 = Value;
	//----T3CON�F�^�C�vC�^�C�}���䃌�W�X�^----
	T3CONbits.TON = 1;				//Timer3���J�n����
}

//====Timer4�J�n�֐�============================================================
void Timer4_Start(int Value)
{
	//----TMR4�F�^�C�}�J�E���g���W�X�^----
	TMR4 = 0;
	//----PR4�F�^�C�}�������W�X�^----
	PR4 = Value;
	//----T4CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T4CONbits.TON = 1;				//Timer4���J�n����
}

//====Timer5�J�n�֐�============================================================
void Timer5_Start(int Value)
{
	//----TMR5�F�^�C�}�J�E���g���W�X�^----
	TMR5 = 0;
	//----PR5�F�^�C�}�������W�X�^----
	PR5 = Value;
	//----T5CON�F�^�C�vC�^�C�}���䃌�W�X�^----
	T5CONbits.TON = 1;				//Timer5���J�n����
}

//====Timer23�J�n�֐�===========================================================
void Timer23_Start(long Value)
{
	//----TMR2, TMR3HLD�F�^�C�}�J�E���g���W�X�^----
	TMR3HLD = 0;
	TMR2 = 0;
	//----PR2,3�F�^�C�}�������W�X�^----
	PR2 = Value >> 16;
	PR3 = 0x0000FFFF & Value;
	//----T2CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T2CONbits.TON = 1;				//Timer23���J�n����
}

//====Timer45�J�n�֐�===========================================================
void Timer45_Start(long Value)
{
	//----TMR4, TMR5HLD�F�^�C�}�J�E���g���W�X�^----
	TMR5HLD = 0;
	TMR4 = 0;
	//----PR4,5�F�^�C�}�������W�X�^----
	PR4 = Value >> 16;
	PR5 = 0x0000FFFF & Value;
	//----T4CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T4CONbits.TON = 1;				//Timer45���J�n����
}

//====Timer1��~�֐�============================================================
void Timer1_Stop()
{
	//----T1CON�F�^�C�vA�^�C�}���䃌�W�X�^----
	T1CONbits.TON = 0;				//Timer1���~����
}

//====Timer2��~�֐�============================================================
void Timer2_Stop()
{
	//----T2CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T2CONbits.TON = 0;				//Timer2���~����
}

//====Timer3��~�֐�============================================================
void Timer3_Stop()
{
	//----T3CON�F�^�C�vC�^�C�}���䃌�W�X�^----
	T3CONbits.TON = 0;				//Timer3���~����
}

//====Timer4��~�֐�============================================================
void Timer4_Stop()
{
	//----T4CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T4CONbits.TON = 0;				//Timer4���~����
}

//====Timer5��~�֐�============================================================
void Timer5_Stop()
{
	//----T5CON�F�^�C�vC�^�C�}���䃌�W�X�^----
	T5CONbits.TON = 0;				//Timer5���~����
}

//====Timer23��~�֐�===========================================================
void Timer23_Stop()
{
	//----T2CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T2CONbits.TON = 0;				//Timer23���~����
}

//====Timer45��~�֐�===========================================================
void Timer45_Stop()
{
	//----T4CON�F�^�C�vB�^�C�}���䃌�W�X�^----
	T4CONbits.TON = 0;				//Timer45���~����
}

//====Timer1�̒l��Ԃ�==========================================================
int  Timer1_Value()
{
	return(TMR1);
}

//====Timer2�̒l��Ԃ�==========================================================
int  Timer2_Value()
{
	return(TMR2);
}

//====Timer3�̒l��Ԃ�==========================================================
int  Timer3_Value()
{
	return(TMR3);
}

//====Timer4�̒l��Ԃ�==========================================================
int  Timer4_Value()
{
	return(TMR4);
}

//====Timer5�̒l��Ԃ�==========================================================
int  Timer5_Value()
{
	return(TMR5);
}

//====Timer23�̒l��Ԃ�=========================================================
long Timer23_Value()
{
	long Value = 0;
	
	//TMR2��ǂ�ł���TMR3HLD��ǂݍ��ނ���
	Value = 0x0000FFFF & TMR2;
	Value += 0xFFFF0000 & ((long)TMR3HLD << 16);
	
	return(Value);
}

//====Timer45�̒l��Ԃ�=========================================================
long Timer45_Value()
{
	long Value = 0;
	
	//TMR4��ǂ�ł���TMR5HLD��ǂݍ��ނ���
	Value = 0x0000FFFF & TMR4;
	Value += 0xFFFF0000 & ((long)TMR5HLD << 16);
	
	return(Value);
}

