#include <p18f2320.h>
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
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


//---Wait---//
//48Mhz�쓮�̎�
#define WAIT_MS Delay1KTCYx(12)// Wait 1ms
#define WAIT_US Delay10TCYx(12)	// Wait 1us

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
	char Message1[10]="\rStart!!\n";
	char Message2[7]="FUKUDA";
	char  data=0;
	float DATA=0;
	
	//UART������
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 77); 	//���Ӂ@OpenUSART�@�֐��̍Ō�̕����̓{�[���[�g�ݒ�ō���40Mhz��9600�̎���64�ɂ���B�܂�48Mhz��9600�̎���77�ɂ���
	
		

	
	//������
		TRISA=0x0F;                        // �|�[�gA�𔼕��o�̓s���ɂ���
		TRISB=0;                        // �|�[�gB�����ׂďo�̓s���ɂ���
		TRISC=0b10111111;                        // �|�[�gC�����ׂďo�̓s���ɂ���
	putsUSART(Message1);
	printf("\rHello world\n");		//�uHello�v�Əo��
	
	PORTB = 0x00;
	wait_ms(1000);
	PORTB = 0xFF;
	wait_ms(1000);
	PORTB = 0x00;
	wait_ms(1000);
	PORTB = 0xFF;
	wait_ms(1000);
	PORTB = 0x00;
	
	while(1)
	{
		
		
		PORTB = 0x00;
		Delay100TCYx(5);		//20usec delay
		//if(DataRdyUSART());
		//{
			
			while(!DataRdyUSART());
			data = ReadUSART();
			WriteUSART(data);
			PORTB = 0xFF;
		//}
		
		
		
	}
	//UART�I��
	//CloseUSART( );
	

}
