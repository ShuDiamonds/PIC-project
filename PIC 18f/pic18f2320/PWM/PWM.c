#include <p18f2320.h>
#include <pwm.h>
#include <delays.h>

//config�ݒ�
#pragma config OSC = INTIO2 //�������U��𗘗p
#pragma config WDT = OFF //�E�H�b�`�h�b�O�^�C�}OFF
#pragma config MCLRE = OFF //MCLR������v���A�b�v
#pragma config LVP = OFF //��d��ICSP����OFF
//���̑���config�̓f�t�H���g�ݒ�̂܂�

void main(void)
{
	unsigned int i; //�g�p�ϐ��̒�`
	
	//������
	PORTC = 0b00000000; //PortC�̒��g�����ꂢ�ɂ���
	OSCCON = 0b01110000; //�������U���8MHz�g�p�ɐݒ�
	TRISC = 0b00000000; //PortC 8�S��0:�o�͐ݒ�

	PORTCbits.RC3 = 1; //�Ɠx��r�pLED ON
	OpenPWM1(255); //PWM1���I�[�v��
	OpenPWM2(255); //PWM2���I�[�v��
	
	while(1)
	{
		//LED�̖��邳�����X�ɕς���(1024����\)
		for (i = 0; i < 1024; i++)
		{
			SetDCPWM1(i); //PWM1��Duty�ύX
			SetDCPWM2(1023 - i); //PWM2��Duty�ύX
			Delay10KTCYx(1); //5msec�҂�
		}
	}
}
