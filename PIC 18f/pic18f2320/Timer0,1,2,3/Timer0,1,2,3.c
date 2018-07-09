#include <p18f2320.h>    // PIC18C452�̃w�b�_�E�t�@�C��
#include <delays.h>
#include <usart.h>
#include <timers.h>
#include <adc.h>

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
	char Message1[8]="Start!!";
	char Message2[7]="FUKUDA";
	char data;
	
	//UART������
	/*OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 64); 	//���Ӂ@OpenUSART�@�֐��̍Ō�̕����̓{�[���[�g�ݒ�ō���40Mhz��9600
	*/
	//Timer0������
	OpenTimer0(TIMER_INT_ON & T0_16BIT & T0_SOURCE_INT & T0_PS_1_256);
		//�J�E���g�l�̃��[�h
		WriteTimer0(0x676A);
	//������
		TRISA=0x00;                        // �|�[�gA�����ׂďo�̓s���ɂ���
		TRISB=0;                        // �|�[�gB�����ׂďo�̓s���ɂ���
		TRISC=0b10111111;                        // �|�[�gC�����ׂďo�̓s���ɂ���
		ADCON1 = 0b00001111;				//���ׂăf�W�^��
	//**** �����݂̋���
    INTCONbits.GIE=1;           // ���荞�݂��C�l�[�u���ɂ���
	
	putsUSART(Message1);	
	PORTA = 0b00111100;
	while(1)
	{
		
 		
		
		
		
	}
	//UART�I��
	CloseUSART( );
}
//******************************************************
//****** �����ݐ錾�i�D�揇�ʂ��g��Ȃ��j
#pragma interrupt isr save = PROD
//***** �����݃x�N�^�փW�����v���߃Z�b�g
#pragma code isrcode = 0x0000008
void isr_direct(void)
{ _asm  goto isr  _endasm }

//***** �����ݏ����֐�
#pragma code
void isr(void)                      // ���荞�݊֐�
{
		if(INTCONbits.T0IF)				// �^�C�}0���荞�݁H
		{
			//�J�E���g�l�̃��[�h
			WriteTimer0(0x676A);
			INTCONbits.T0IF=0;          // �^�C�}0���荞�݃t���O��0�ɂ���
			PORTA = PORTA^0b00111100;
		
		
		}                                   
}

