/**********************************************************/
/*UART�֐����C�u����                                      */
/*2011.12.30                                              */
/**********************************************************/
#include "p33Fxxxx.h"
#include "stdio.h"
#include "uartlib.h"

//====UART1�������֐�===========================================================
void UART1_Init(int baud, int flow)
{
	//----UART1�C�l�[�u���r�b�g---------
	U1MODEbits.UEN = flow;			//Tx�����Rx�s���A�t���[����s���̗L����
	//----�{�[���[�g�ݒ�----------------
	U1BRG = baud;		//�{�[���[�g�ݒ�
	//UART1�X�e�[�^�X���W�X�^�N���A
	U1STA = 0;
	//UART1�C�l�[�u��
	U1MODEbits.UARTEN = 1;
	//UART1���M�C�l�[�u��
	U1STAbits.UTXEN = 1;
	
}

//====UART1��M���荞�݋��֐�=====================================================
void UART1_RxIntEnable(int Level)
{
	//UART1��M���荞�ݗD��x�r�b�g
	IPC2bits.U1RXIP = Level;	//���荞�ݗD��x�ݒ�
	//UART1��M���荞�݃X�e�[�^�X�r�b�g
	IFS0bits.U1RXIF = 0;		//���荞�ݗv���N���A
	//UART1��M���荞�݃C�l�[�u���r�b�g
	IEC0bits.U1RXIE = 1;		//���荞�ݗv���͗L��
}

//====UART1��M���荞�݋֎~�֐�=====================================================
void UART1_RxIntDisable(void)
{
	//UART1��M���荞�݃C�l�[�u���r�b�g
	IEC0bits.U1RXIE = 0;		//���荞�ݗv���͖���
}


//====UART2�������֐�===========================================================
void UART2_Init(int baud, int flow)
{
	//----UART2�C�l�[�u���r�b�g---------
	U2MODEbits.UEN = flow;			//Tx�����Rx�s���A�t���[����s���̗L����
	//----�{�[���[�g�ݒ�----------------
	U2BRG = baud;		//�{�[���[�g�ݒ�
	//UART2�X�e�[�^�X���W�X�^�N���A
	U2STA = 0;
	//UART2�C�l�[�u��
	U2MODEbits.UARTEN = 1;
	//UART2���M�C�l�[�u��
	U2STAbits.UTXEN = 1;
	
}

//====UART2��M���荞�݋��֐�=====================================================
void UART2_RxIntEnable(int Level)
{
	//UART2��M���荞�ݗD��x�r�b�g
	IPC7bits.U2RXIP = Level;	//���荞�ݗD��x�ݒ�
	//UART1��M���荞�݃X�e�[�^�X�r�b�g
	IFS1bits.U2RXIF = 0;		//���荞�ݗv���N���A
	//UART1��M���荞�݃C�l�[�u���r�b�g
	IEC1bits.U2RXIE = 1;		//���荞�ݗv���͗L��
}

//====UART2��M���荞�݋֎~�֐�=====================================================
void UART2_RxIntDisable(void)
{
	//UART2��M���荞�݃C�l�[�u���r�b�g
	IEC0bits.U1RXIE = 0;		//���荞�ݗv���͖���
}


//****UART1�G���[���o�֐�****************************************************************************
void UART1_ErrDetect(void)
{
	//----�G���[���o----------------------------------------------
	if(U1STAbits.OERR == 1)
	{
		printf("\n[!]UART1 OverRunErr[!]\n");
		U1STAbits.OERR = 0;
	}
	if(U1STAbits.FERR == 1)
	{
		printf("\n[!]UART1 FramingErr[!]\n");
		U1STAbits.FERR = 0;
	}
	if(U1STAbits.PERR == 1)
	{
		printf("\n[!]UART1 ParityErr[!]\n");
		U1STAbits.PERR = 0;
	}
}

//****UART2�G���[���o�֐�****************************************************************************
void UART2_ErrDetect(void)
{
	//----�G���[���o----------------------------------------------
	if(U2STAbits.OERR == 1)
	{
		printf("\n[!]UART2 OverRunErr[!]\n");
		U2STAbits.OERR = 0;
	}
	if(U2STAbits.FERR == 1)
	{
		printf("\n[!]UART2 FramingErr[!]\n");
		U2STAbits.FERR = 0;
	}
	if(U2STAbits.PERR == 1)
	{
		printf("\n[!]UART2 ParityErr[!]\n");
		U2STAbits.PERR = 0;
	}
}


