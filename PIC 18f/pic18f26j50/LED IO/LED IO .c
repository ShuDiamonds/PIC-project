/************************************************************
�@�@MPLAB-C18�ɂ����o�̓|�[�g�̐���e�X�g�v���O����
�@�@�X�C�b�`�̓��͂��|�[�gD����s���A�X�C�b�`�̏�Ԃ�
�@�@�]���āA�w�肳�ꂽ�����_�C�I�[�h��_�ł�����B
�@�@�@�@RD0�̃X�C�b�`��OFF�̎�LED�P
�@�@�@�@RD1�̃X�C�b�`��OFF�̎�LED2
�@�@�@�@RD2�̃X�C�b�`��OFF�̎�LED3
�@�@�@�@�S�X�C�b�`��ON�̎��SLED
*************************************************************/
#include <p18f26j50.h>    // PIC18F26j50�̃w�b�_�E�t�@�C��
#include <delays.h>
#include <usart.h>

//***** �R���t�B�M�����[�V�����̐ݒ�config

	#pragma config WDTEN = OFF          //WDT disabled (enabled by SWDTEN bit)
	#pragma config PLLDIV = 2           //Divide by 2 (8 MHz intl osc input)
	#pragma config STVREN = ON          //stack overflow/underflow reset enabled
	#pragma config XINST = OFF          //Extended instruction set disabled
	#pragma config CPUDIV = OSC1        //No CPU system clock divide
	#pragma config CP0 = OFF            //Program memory is not code-protected
	#pragma config OSC = INTOSCPLL      //Internal oscillator, PLL enenabled
	#pragma config T1DIG = OFF          //S-Osc may not be selected, unless T1OSCEN = 1
	#pragma config LPT1OSC = OFF        //high power Timer1 mode
	#pragma config FCMEN = OFF          //Fail-Safe Clock Monitor disabled
	#pragma config IESO = OFF           //Two-Speed Start-up disabled
	#pragma config WDTPS = 32768        //1:32768
	#pragma config DSWDTOSC = INTOSCREF //DSWDT uses INTOSC/INTRC as clock
	#pragma config RTCOSC = T1OSCREF    //RTCC uses T1OSC/T1CKI as clock
	#pragma config DSBOREN = OFF        //Zero-Power BOR disabled in Deep Sleep
	#pragma config DSWDTEN = OFF        //Disabled
	#pragma config DSWDTPS = 8192       //1:8,192 (8.5 seconds)
	#pragma config IOL1WAY = OFF        //IOLOCK bit can be set and cleared
	#pragma config MSSP7B_EN = MSK7     //7 Bit address masking
	#pragma config WPFP = PAGE_1        //Write Protect Program Flash Page 0
	#pragma config WPEND = PAGE_0       //Start protection at page 0
	#pragma config WPCFG = OFF          //Write/Erase last page protect Disabled
	#pragma config WPDIS = OFF          //WPFP[5:0], WPEND, and WPCFG bits ignored 


		
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
	
	//�N���b�N������
		//48Mhz�̐ݒ�
	OSCTUNEbits.PLLEN = 1;        // PLL���N��
	wait_ms(2);
	
	//������
		TRISA=0x00;                        // �|�[�gA�����ׂďo�̓s���ɂ���
		TRISB=0;                        // �|�[�gB�����ׂďo�̓s���ɂ���
		TRISC=0xFF;                        // �|�[�gC�����ׂďo�̓s���ɂ���
	
	
	while(1)
	{
		PORTB = 0x00;
		wait_ms(1000);
		PORTB = 0xFF;
		wait_ms(1000);
		PORTB = 0x00;
		wait_ms(1000);
		PORTB = 0xFF;
		wait_ms(1000);
		PORTB = 0x00;
		
	}
}

