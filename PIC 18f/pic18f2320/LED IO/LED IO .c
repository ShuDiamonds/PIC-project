/************************************************************
�@�@MPLAB-C18�ɂ����o�̓|�[�g�̐���e�X�g�v���O����
�@�@�X�C�b�`�̓��͂��|�[�gD����s���A�X�C�b�`�̏�Ԃ�
�@�@�]���āA�w�肳�ꂽ�����_�C�I�[�h��_�ł�����B
�@�@�@�@RD0�̃X�C�b�`��OFF�̎�LED�P
�@�@�@�@RD1�̃X�C�b�`��OFF�̎�LED2
�@�@�@�@RD2�̃X�C�b�`��OFF�̎�LED3
�@�@�@�@�S�X�C�b�`��ON�̎��SLED
*************************************************************/
#include <p18f2320.h>    // PIC18C452�̃w�b�_�E�t�@�C��
#include <delays.h>
#include <usart.h>

//***** �R���t�B�M�����[�V�����̐ݒ�config


#pragma	config	OSC = HSPLL
#pragma	config	FSCM = OFF
#pragma	config	IESO = OFF
#pragma	config	PWRT = OFF
#pragma	config	BOR = OFF
#pragma	config	BORV = 20
#pragma	config	WDT = OFF
#pragma	config	WDTPS = 1
#pragma	config	MCLRE = ON
#pragma	config	PBAD = DIG
//	#pragma	config	PBAD = ANA
//	#pragma	config	CCP2MX = C1
#pragma	config	STVR = ON
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
		

//***** ���C���֐�
void main(void)                     // ���C���֐�
{
	//���[�J���ϐ���`
	
	
	//UART������
	
	
	//������
		TRISA=0x00;                        // �|�[�gA�����ׂďo�̓s���ɂ���
		TRISB=0;                        // �|�[�gB�����ׂďo�̓s���ɂ���
		TRISC=0xFF;                        // �|�[�gC�����ׂďo�̓s���ɂ���
	
	while(1)
	{
		if(PORTCbits.RC3)				//�X�C�b�`��OFF�̂Ƃ�
		{
		PORTA = 0xFF;					//LED OFF
		}
		else{							//�X�C�b�`��ON�̂Ƃ�
		PORTA = 0x00;					//LED ON
		}
		
	}
}

