#include <p18f2550.h>
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <adc.h>
//***** �s���}�N�� **********************//
#define PIN_Signal_R_A		PORTBbits.RB3
#define PIN_Signal_R_B		PORTBbits.RB2
#define PIN_Signal_L_A		PORTBbits.RB1
#define PIN_Signal_L_B		PORTBbits.RB0
#define PIN_LED_RUN			PORTBbits.RB4
//***** �֐��v���g�^�C�v ****************//
void MOTOR(unsigned char MRA,unsigned char MRB,unsigned char MLA,unsigned char MLB);

//***** �R���t�B�M�����[�V�����̐ݒ�config


		#pragma config PLLDIV   = 5         // (20 MHz crystal on PICDEM FS USB board)
		#pragma config CPUDIV   = OSC1_PLL2   
		#pragma config USBDIV   = 2         // Clock source from 96MHz PLL/2
		#pragma config FOSC     = HSPLL_HS
		#pragma config FCMEN    = OFF
		#pragma config IESO     = OFF
		#pragma config PWRT     = OFF
		#pragma config BOR      = OFF
		#pragma config BORV     = 3
	//	#pragma config VREGEN   = ON      //USB Voltage Regulator
		#pragma config VREGEN   = OFF      //USB Voltage Regulator
		#pragma config WDT      = OFF
		#pragma config WDTPS    = 32768
		#pragma config MCLRE    = ON
		#pragma config LPT1OSC  = OFF
		#pragma config PBADEN   = OFF
		//      #pragma config CCP2MX   = ON
		#pragma config STVREN   = ON
		#pragma config LVP      = OFF
		//      #pragma config ICPRT    = OFF       // Dedicated In-Circuit Debug/Programming
		#pragma config XINST    = OFF       // Extended Instruction Set
		#pragma config CP0      = OFF
		#pragma config CP1      = OFF
		//      #pragma config CP2      = OFF
		//      #pragma config CP3      = OFF
		#pragma config CPB      = OFF
		//      #pragma config CPD      = OFF
		#pragma config WRT0     = OFF
		#pragma config WRT1     = OFF
		//      #pragma config WRT2     = OFF
		//      #pragma config WRT3     = OFF
		#pragma config WRTB     = OFF       // Boot Block Write Protection
		#pragma config WRTC     = OFF
		//      #pragma config WRTD     = OFF
		#pragma config EBTR0    = OFF
		#pragma config EBTR1    = OFF
		//      #pragma config EBTR2    = OFF
		//      #pragma config EBTR3    = OFF
		#pragma config EBTRB    = OFF


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
			USART_CONT_RX & USART_BRGH_LOW, 77); 	//���� OpenUSART �֐��̍Ō�̕����͂ځ[��[�Ɛݒ�ō���40Mhz��9600�̎���64�ɂ���B�܂�48Mhz��9600�̎���77�ɂ���
	
		

	
	//������
		TRISA=0x0F;                        // �|�[�gA�𔼕��o�͂҂�ɂ���
		TRISB=0x00;                        // �|�[�gB�����ׂďo�͂҂�ɂ���
		TRISC=0b10111111;                        // �|�[�gC�����ׂďo�͂҂�ɂ���
	//������
		PORTB = 0x00;
	
	
	/*
	putsUSART(Message1);
	printf("\rHello world\n");		//�uHello�v�Əo��
	
	PIN_LED_RUN = 0;
	wait_ms(1000);
	PIN_LED_RUN = 1;
	wait_ms(1000);
	PIN_LED_RUN = 0;
	wait_ms(1000);
	PIN_LED_RUN = 1;
	wait_ms(1000);
	PIN_LED_RUN = 0;
	
	
	*/
	PORTB = 0x00;
	while(1)
	{
		
		
		wait_ms(1);
		//MOTOR(1,1,1,1);						//�u���[�L
		
		
		
			while(!DataRdyUSART());			//�f�[�^�҂�
			data = ReadUSART();				//�f�[�^��M
			//�f�[�^���
			switch(data)
			{
			case	'w':		//��
				MOTOR(1,0,1,0);
				break;
			case	's':		//��
				MOTOR(0,1,0,1);
				break;
			case	'd':		//�E
				MOTOR(1,0,0,1);
				break;
			case	'a':		//��
				MOTOR(0,1,1,0);
				break;
			case	'z':		//�u���[�L
				MOTOR(1,1,1,1);						
				break;
			}
			WriteUSART(data);
		
		
	}
	//UART�I��
	//CloseUSART( );
	

}


void MOTOR(unsigned char  MRA,unsigned char MRB,unsigned char MLA,unsigned char MLB)
{
	/******** �֐��̐���*********
	�ϐ���1�r�b�g�̌^�Ȃ̂ŁA
	�ϐ���1��0�ȊO�̐���������ƕϐ��ɑ������Ȃ�
	
	***************************/
	
	//���[�^�[����֐�
		PIN_Signal_R_A = MRA;		
		PIN_Signal_R_B = MRB;
		//���[�^�[B
		PIN_Signal_L_A = MLA;		
		PIN_Signal_L_B = MLB;
	
	return;
}
