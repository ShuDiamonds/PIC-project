#include <p18f2320.h>    // PIC18C452�̃w�b�_�E�t�@�C��
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
		

//***** ���C���֐�
void main(void)                     // ���C���֐�
{
	//���[�J���ϐ���`
	char Message1[10]="\rStart!!\n";
	char Message2[7]="FUKUDA";
	long int data=0;
	float DATA=0;
	
	//UART������
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 64); 	//���Ӂ@OpenUSART�@�֐��̍Ō�̕����̓{�[���[�g�ݒ�ō���40Mhz��9600
	//AD�ϊ�������
	//define mode of A/D
	//OpenADC(ADC_FOSC_16 & ADC_RIGHT_JUST & ADC_8ANA_0REF,ADC_CH0 & ADC_INT_OFF);
	
		OpenADC(ADC_FOSC_64 &           //AD�ϊ��p�N���b�N�@�@�V�X�e���N���b�N��1/64�@0.05��sec�~64��3.2��sec�@>=�@1.6��sec�@���@OK
				ADC_RIGHT_JUST &        //�ϊ����ʂ̕ۑ����@�@���l�߁@
				ADC_8_TAD,              //AD�ϊ��̃A�N�C�W�V�����^�C���I���@3.2��sec�i=1Tad�j�~8Tad=25.6��sec�@�����@12.8��sec�@���@OK
				ADC_CH0 &                       //AD�ϊ�����̃`�����l���I���iPIC18F�͓����ɕ�����AD�ϊ��͂ł��Ȃ��j
				ADC_INT_OFF &           //AD�ϊ��ł̊����ݎg�p�̗L��
				ADC_VREFPLUS_VDD &      //Vref+�̐ݒ�@�@�@�o�h�b�̓d���d���Ɠ����FADC_VREFPLUS_VDD �@or�@�O���i�`�m�R�j�̓d���FADC_VREFPLUS_EXT
				ADC_VREFMINUS_VSS,      //Vref-�̐ݒ�@�@�@�o�h�b��0�u�FADC_VREFMINUS_VSS    or�@�O���iAN2)�̓d���FADC_VREFMINUS_EXT
				0b1110  //�|�[�g�̃A�i���O�E�f�W�^���I���@�iADCON1�̉��ʂS�r�b�g���L�ځj�@�@AN0�̂݃A�i���O�|�[�g��I���A���̓f�W�^���|�[�g��I��
				//�� �@�A�i���O�|�[�g���@AN0�̂� �� 0b1110�@�@�AAN0 & AN1�@���@0b1011�A AN0 & AN1 & AN2 ��1100�@���@�ڍ׃f�[�^�V�[�g�Q��
		);

	
	SetChanADC(ADC_CH0);	//Select Channel 0
	//������
		TRISA=0x0F;                        // �|�[�gA�����ׂďo�̓s���ɂ���
		TRISB=0;                        // �|�[�gB�����ׂďo�̓s���ɂ���
		TRISC=0b10111111;                        // �|�[�gC�����ׂďo�̓s���ɂ���
	putsUSART(Message1);
	//fprintf(_H_USART,"\rHello world\n");		//�uHello�v�Əo��
	while(1)
	{
		SetChanADC(ADC_CH0);	//Select Channel 0
		Delay100TCYx(5);		//20usec delay
		ConvertADC();		//Start A/D
		while(BusyADC());	//Wait end of conversion
		data = ReadADC();	//Get A/D data
		
		fprintf(_H_USART,"\rData=%ld\n",data);		
		
		/*
		float�^��UART�ł̏o�͖͂����炵��
		DATA = data * 0.0488;
		fprintf(_H_USART,"\rDATA=%f\n",DATA);
		*/
		Delay10KTCYx(200);		//1�b�҂�
		Delay10KTCYx(200);
		Delay10KTCYx(200);
		Delay10KTCYx(200);
		Delay10KTCYx(200);
		
	}
	//UART�I��
	//CloseUSART( );
	//AD�ϊ��I��
	//CloseADC();

}
