/************************************************************
	MPLAB-C18�ɂ����o�̓|�[�g�̐���e�X�g�v���O����
	pic18f26k20��64MHz�œ������ALED��1�b�Ԋu�ł�������������
*************************************************************/
#include <p18f26k20.h>	 // PIC18F26k20�̃w�b�_�E�t�@�C��
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>

//***** �R���t�B�M�����[�V�����̐ݒ�config

#pragma	config	FOSC = INTIO67
#pragma	config	FCMEN = OFF
#pragma	config	IESO = OFF
#pragma	config	PWRT = OFF
#pragma	config	BOREN = OFF
#pragma	config	BORV = 18
#pragma	config	WDTEN = OFF
#pragma	config	WDTPS = 1
#pragma	config	MCLRE = ON
#pragma	config	PBADEN = OFF		//�f�W�^���ɐݒ�
//	#pragma	config	PBAD = ANA
//	#pragma	config	CCP2MX = C1
#pragma	config	STVREN = ON
#pragma	config	LVP = ON
#pragma	config	DEBUG = OFF
#pragma	config	CP0 = OFF
#pragma	config	CP1 = OFF
#pragma	config	CP2 = OFF
#pragma	config	CP3 = OFF
#pragma	config	CPB = OFF
#pragma	config	CPD = OFF
#pragma	config	WRT0 = OFF
#pragma	config	WRT1 = OFF
#pragma	config	WRT2 = OFF
#pragma	config	WRT3 = OFF
#pragma	config	WRTB = OFF
#pragma	config	WRTC = OFF
#pragma	config	WRTD = OFF
#pragma	config	EBTR0 = OFF
#pragma	config	EBTR1 = OFF
#pragma	config	EBTR2 = OFF
#pragma	config	EBTR3 = OFF
#pragma	config	EBTRB = OFF	
		
//---Wait---//
//64Mhz�쓮�̎�
#define WAIT_MS Delay1KTCYx(16)// Wait 1ms
#define WAIT_US Delay10TCYx(16)	// Wait 1us

//=======wait[ms]======//
void wait_ms(unsigned long int t) {
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[us]======//
void wait_us(unsigned int t) {
	while(t--) {
		WAIT_US;
	}
}
//***** ���C���֐�
void main(void)                     // ���C���֐�
{
	//���[�J���ϐ���`
	
	//�����N���b�N������
	OSCCONbits.IRCF0 = 1;		//16MHz�ɐݒ�
	OSCCONbits.IRCF1 = 1;
	OSCCONbits.IRCF2 = 1;

	OSCTUNEbits.PLLEN = 1;        // PLL���N������(16*4=64Mhz�ɐݒ�)
	
	//UART������
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 103); 	//���Ӂ@OpenUSART�@�֐��̍Ō�̕����̓{�[���[�g�ݒ�ō���40Mhz��9600�̎���64�ɂ���B�܂�48Mhz��9600�̎���77�ɂ��� 64��103�ɂ���
	
	
	//wait_ms(1000);
	
	//������
		TRISA=0x00;                        // �|�[�gA�����ׂďo�̓s���ɂ���
		TRISB=0;                        // �|�[�gB�����ׂďo�̓s���ɂ���
		TRISC=0xFF;                        // �|�[�gC�����ׂďo�̓s���ɂ���
	
	while(1)
	{
		
		PORTA = 0xFF;					//LED OFF
		PORTB = 0xFF;
		wait_ms(1000);
		PORTA = 0x00;					//LED ON
		PORTB = 0x00;
		wait_ms(1000);
		
	}
}

