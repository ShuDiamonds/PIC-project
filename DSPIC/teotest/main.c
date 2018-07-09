//****�w�b�_�t�@�C���C���N���[�h********************************************************************
#include "p33FJ128MC802.h"	//�f�o�C�X�R���t�B�O
#include "stdio.h"			//�W�����o��
#include "stdlib.h"			//�W�����C�u����
#include "string.h"			//�����񑀍�
#include "math.h"			//���w

#include "interruptlib.h"	//���荞�ݏ���					��
#include "ppslib.h"			//�y���t�F�����s���Z���N�g		��
#include "timerlib.h"		//�^�C�}						��
#include "adclib.h"			//ADC							��
#include "uartlib.h"		//UART							��
#include "pwmlib.h"			//pwm							��
//#include "motorpwm.h"		//���[�^�[�R���g���[���ppwm		���얢�m�F
#include "delaylib.h"		//delay							��
//#include "dmalib.h"			//Direct Memory Access			���얢�m�F


//****�}�N����`************************************************************************************
//���o�͐ݒ�                                           FEDC BA98 7654 3210
#define		TRIS_APORT			0x03;				//           0000 0011
#define		TRIS_BPORT			0x8FA8;				// 1000 1111 1010 1000

//�s���}�N��
#define		LEDPIN0				LATAbits.LATA4

//�A�i���O���͐ݒ�
#define		ADC_PCFGBIT			ADC_AN0 & ADC_AN1 & ADC_AN4 & ADC_AN5	//�A�i���O���̓s��0,1,4,5���A�i���O���͂ɐݒ�

//****�q���[�Y�r�b�g�ݒ�****************************************************************************
_FBS(BSS_NO_FLASH);			//�u�[�g���[�h�ݒ�A�Ȃ񂩕�����񂩂�off
_FSS(SWRP_WRPROTECT_OFF)	//�Z�L���A�Z�O�����g�v���O�����̃��C�g�v���e�N�g
_FGS(GCP_OFF);				//�R�[�h�v���e�N�goff
_FOSCSEL(FNOSC_FRCPLL & IESO_ON);	//�����N���b�N��PLL�Ŏg�p�ATwo-speed Oscillator Startup�͂Ȃ񂩒m��񂯂�on
_FOSC(FCKSM_CSDCMD &  IOL1WAY_OFF & OSCIOFNC_ON & POSCMD_NONE);	//�N���b�N�؂�ւ��ƃN���b�N���j�^�����AOSCCON�r�b�g�͏������݂P��̂݁AOSC�s���̓f�W�^��IO�Ƃ��Ďg�p�A�N���b�N���[�h�w��Ȃ�
_FWDT(FWDTEN_OFF);			//�E�H�b�`�h�b�O�^�C�}���g�p���Ȃ�
_FPOR(ALTI2C_OFF);			//�Ȃ񂩒m��񂯂�off
_FICD(JTAGEN_OFF);			//�悤������񂯂�off


//****�֐��v���g�^�C�v�錾**************************************************************************
//�������֐�
void Setup(void);							//�y���t�F�����������֐�
//���荞�݊֐�
void U1RXInterrupt(void);					//UART1��M���荞�݊֐�
void U2RXInterrupt(void);					//UART2��M���荞�݊֐�
void ADC1Interrupt(void);					//ADC1�ϊ��I�����荞�݊֐�
void INT1Interrupt(void);					//�O�����荞��1���荞�݊֐�
void INT2Interrupt(void);					//�O�����荞��2���荞�݊֐�
void T1Interrupt(void);						//�^�C�}1���荞�݊֐�
void T2Interrupt(void);						//�^�C�}2���荞�݊֐�
void T3Interrupt(void);						//�^�C�}3���荞�݊֐�
void T4Interrupt(void);						//�^�C�}4���荞�݊֐�
void T5Interrupt(void);						//�^�C�}5���荞�݊֐�


//****�O���[�o���ϐ��錾****************************************************************************
int UART_RX_Flag = 1;
char UART_RX_Buffer[64] = {0};

//****main�֐�**************************************************************************************
int main(void)
{
	//----���[�J���ϐ���`--------------------------------------------
	int a=0;
	char charbuffer = 0;
	Setup();
	
	
	printf("awaawaaaaa\r\n");
	
	
	
	while(1)
	{
		a = (a + 1) % 65535;
		PWM1_SetValue(a);
		
		
		
	
	charbuffer = U1RXREG;
	
	printf("%c", charbuffer );
		//printf("helloworld a= %d\r\n",a);
		delay_ms(10);
	}
	
	return(0);
}



//****UART1��M���荞�݊֐�*************************************************************************
void  __attribute__((__interrupt__, __auto_psv__)) _U1RXInterrupt(void)
{
	char charbuffer = 0;
	
	charbuffer = U1RXREG;
	
	printf("%c", charbuffer + 1);
	
	//UART1��M���荞�݃X�e�[�^�X�r�b�g
	IFS0bits.U1RXIF = 0;	//���荞�ݗv���N���A
}

//****UART2��M���荞�݊֐�*************************************************************************
void  __attribute__((__interrupt__, __auto_psv__)) _U2RXInterrupt(void)
{
	//UART2��M���荞�݃X�e�[�^�X�r�b�g
	IFS1bits.U2RXIF = 0;	//���荞�ݗv���N���A
}


//****ADC1���荞�݊֐�******************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _ADC1Interrupt(void)
{
	IFS0bits.AD1IF = 0;		//ADC1���荞�݃t���O�N���A
}

//****�O�����荞��1�֐�*****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _INT1Interrupt(void)
{
	IFS1bits.INT1IF = 0;	//Int1���荞�݃t���O�N���A
}

//****�O�����荞��2�֐�*****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _INT2Interrupt(void)
{
	IFS1bits.INT2IF = 0;	//Int1���荞�݃t���O�N���A
}

//****Timer1���荞�݊֐�****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T1Interrupt(void)
{
	IFS0bits.T1IF = 0;		//Timer1���荞�݃t���O�N���A
}

//****Timer2���荞�݊֐�****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T2Interrupt(void)
{
	IFS0bits.T2IF = 0;		//Timer2���荞�݃t���O�N���A
}

//****Timer3���荞�݊֐�****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T3Interrupt(void)
{
	
	IFS0bits.T3IF = 0;		//Timer3���荞�݃t���O�N���A
}

//****Timer4���荞�݊֐�****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T4Interrupt(void)
{
	IFS1bits.T4IF = 0;		//Timer4���荞�݃t���O�N���A
}

//****Timer5���荞�݊֐�****************************************************************************
void __attribute__((__interrupt__, __auto_psv__)) _T5Interrupt(void)
{
	IFS1bits.T5IF = 0;		//Timer5���荞�݃t���O�N���A
}

//****�y���t�F�����������֐�************************************************************************
void Setup(void)
{
	//----���荞�݋֎~����--------------------------------------------
	DisableInterrupt();
	
	//----�N���b�N�ݒ�------------------------------------------------
	OSCCON = 0x00300;				// PRIPLL�w��(���܂����킩���)
	CLKDIV = 0x0100;				// �N���b�N�̕���1:2�ɐݒ�
	PLLFBD = 0x0026;				// 40�{�@8MHz��2�~40��2 = 80MHz(�炵��)
	
	//----�|�[�g���o�͐ݒ�--------------------------------------------
	TRISA = TRIS_APORT;
	TRISB = TRIS_BPORT;
	
	//----UART������--------------------------------------------------
	
	
	//XBee��FT232�o�R�̂Ƃ�
	//UART1_Init(UART_115200BPS, UART_FLOW);	//115200bps�A�t���[���䂠��
	UART1_Init(UART_115200BPS, UART_NOFLOW);	//115200bps�A�t���[����Ȃ�
	UART1_RxIntEnable(5);		//��M���荞�ݗL����
	PPSOut(RP2, RP_U1TX);		//TX�����蓖�Ă�
	PPSIn(RP3, RP_U1RX);		//RX�����蓖�Ă�
	//PPSIn(RP5, RP_U1CTS);		//CTS�����蓖�Ă�
	//PPSOut(RP4, RP_U1RTS);		//RTS�����蓖�Ă�
	
	/*
	//PICKIT2�o�R�̂Ƃ�
	TRISBbits.TRISB0 = 0;		//���o�͐ݒ�ύX
	TRISBbits.TRISB1 = 1;		//���o�͐ݒ�ύX
	UART1_Init(UART_9600BPS, UART_NOFLOW);	//9600bps�A�t���[����Ȃ�
	UART1_RxIntEnable(5);		//��M���荞�ݗL����
	PPSOut(RP0, RP_U1TX);		//Tx�����蓖�Ă�
	PPSIn(RP1, RP_U1RX);		//Rx�����蓖�Ă�
	*/
	
	
	//----ADC������---------------------------------------------------
	//ADC1_10bit_Init(ADC_PCFGBIT);
	
	
	
	//----Timer1������------------------------------------------------
	//Timer1��ADC����^�C�}
	Timer1_Init(PRESCALER_8);		//�v���X�P�[��1:8�Ń^�C�}1������
	//Timer1_IntEnable(6);			//���荞�݋���
	Timer1_IntDisable();
	//Timer1_Start(0x1000);			//0xFFFF�Ŋ��荞�ݔ���
	
	//----Timer2������------------------------------------------------
	//Timer2��PWM�^�C���x�[�X�^�C�}
	Timer2_Init(PRESCALER_1);		//�v���X�P�[��1:1�Ń^�C�}2������
	//Timer2_IntEnable(3);			//���荞�݋���
	Timer2_IntDisable();			//���荞�݋֎~
	Timer2_Start(0xFFFF);			//0xFFFF��J�E���g
	
	//----Timer3������------------------------------------------------
	//Timer3�̓��[�^�[����p�^�C�}
	Timer3_Init(PRESCALER_8);		//�v���X�P�[��1:8�Ń^�C�}3������
	//Timer3_IntEnable(6);			//���荞�݋���
	Timer3_IntDisable();			//���荞�݋֎~
	//Timer3_Start(0xFFFF);			//0xFFFF��J�E���g
	
	//----Timer45������-----------------------------------------------
	//Timer45�͋N�����Ԍv���^�C�}
	Timer45_Init(PRESCALER_255);	//�v���X�P�[��1:255�Ń^�C�}45������
	Timer4_IntDisable();			//���荞�݋֎~
	Timer5_IntDisable();
	Timer45_Start(0xFFFFFFFF);		//0xFFFFFFFF��J�E���g
	
	
	//----PWM������---------------------------------------------------
	PWM1_Init(TIMER2);				//PWM���^�C�}2�ŏ�����
	PWM2_Init(TIMER2);
	PWM3_Init(TIMER2);
	PWM4_Init(TIMER2);
	PPSOut(RP0, RP_OC1);			//RP�s���ݒ�
	PPSOut(RP13, RP_OC2);
	PPSOut(RP14, RP_OC3);
	PPSOut(RP15, RP_OC4);
	PWM1_Start(0);					//PWM�J�n�A�����l0
	PWM2_Start(0);
	PWM3_Start(0);
	PWM4_Start(0);
	
	
	//----�O�����荞�݋���--------------------------------------------
	//�����R���p���XA���͊��荞��
	ExternalInt1_Enable(LOW_TO_HI, 1);		//���荞�݃��x��1
	PPSIn(RP9, RP_INT1);

	//�X�C�b�`1���͊��荞��
	ExternalInt2_Enable(HI_TO_LOW, 1);
	PPSIn(RP10, RP_INT2);
	
	
	//----�R�}���h���C���J�n------------------------------------------
	//printf("Please Enter Comand\n> ");
	
	//----���荞�݋�����--------------------------------------------
	EnableInterrupt();
	
	//----LED�\��off--------------------------------------------------
	LEDPIN0 = 1;
}

/*
0	0000
1	0001
2	0010
3	0011
4	0100
5	0101
6	0110
7	0111
8	1000
9	1001
A	1010
B	1011
C	1100
D	1101
E	1110
F	1111
*/









