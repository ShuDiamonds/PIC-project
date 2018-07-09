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
#include <timers.h>
#include <stdio.h>


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

//�O���[�o���ϐ���`
// --------- ���v�����l BCD�Őݒ肷�� ---------
		unsigned char year  = 0x12;   // 2012�N
		unsigned char month = 0x01;     //  1��
		unsigned char day   = 0x01;     //  1��
		unsigned char hour  = 0x12;     // 12��
		unsigned char min   = 0x00;     //  0��
		unsigned char sec   = 0x00;     //  0�b

		char CntSW ;                    // SW��ԁE�J�E���g
		char tens, ones ;               // SW��ԁE�J�E���g
		char temp_Buf[20] = {0};
//+++++++++define*++++++++++//
// �����ARTCC �N���b�N������Ă���悤�Ȃ�A
// RTCC �L�����u���[�V�����̒l���Z�b�g���A�����̃N���b�N���𑝌�����B
#define RTCCALIBRATION    0
#define    LED2     LATBbits.LATB7      // ���b�Ԋu�œ_��
#define    LED1     LATBbits.LATB6

// �v���g�^�C�v�̐錾
void UART_RC_isr(void);
void UART_TX_isr(void);

void PRINT_TIME(void)
{
	RTCCFGbits.RTCPTR1 = 1;		 // RTCPTR �̃Z�b�g 
	RTCCFGbits.RTCPTR0 = 1;
	while(RTCCFGbits.RTCSYNC != 1); // RTC�̈ʏグ���ɁA�ǂݍ��ʂ悤
	while(RTCCFGbits.RTCSYNC == 1); // SYC���u�O�v�ɂȂ�܂ő҂�
	LED2=!LED2;
	year  = RTCVALL;
	day	  = RTCVALH;	 // RTCPTR �� -1 ���邽�߂̃_�~�[�ǂݍ��� 
	day	  = RTCVALL;
	month = RTCVALH;
	hour  = RTCVALL;
	sec	  = RTCVALH;	 // RTCPTR �� -1 ���邽�߂̃_�~�[�ǂݍ��� 
	sec	  = RTCVALL;
	min	  = RTCVALH;
	// --------- RTCC �����̕\�� -----------------
	printf("20%02X/%02X/%02X:", year, month, day);
	printf("%02X:%02X:%02X\n\r", hour, min, sec);
	year  = 0x12;   // 2012�N
	month = 0x01;     //  1��
	day   = 0x01;     //  1��

}


//***** ���C���֐�
void main(void)                     // ���C���֐�
{
	//���[�J���ϐ���`
	
	int ii=0;
	
	
	//�N���b�N������
		//48Mhz�̐ݒ�
	OSCTUNEbits.PLLEN = 1;        // PLL���N��
	wait_ms(2);
	
	//������
		TRISA=0x0F;
		TRISB=0; 
		TRISC=0b10111111;
	
	//RTC�ݒ�(32kHz)
	 OpenTimer1(TIMER_INT_OFF &		 // Secondary Osc�� RTCC 
		T1_SOURCE_PINOSC &			// �ŗ��p���邽��
		T1_PS_1_1 &					// OSC1��L���ɂ���
		T1_OSC1EN_ON & 
		T1_SYNC_EXT_OFF, 0);
	
	// --------- RTCC �����̐ݒ� (�N�����̂�) ---------
	
	EECON2 = 0x55;                  // RTCC ���W�X�^��
	EECON2 = 0xAA;                  // unlock ��RTCC��
	RTCCFGbits.RTCWREN = 1;         // �ݒ菀��������
	RTCCFGbits.RTCPTR1 = 1;         // RTCVALH�ɃA�N�Z�X���閈��
	RTCCFGbits.RTCPTR0 = 1;         // RTCPTR�͎����I�� -1 ����
	/*
	RTCVALL = year;                 // �N
	RTCVALH = 0xFF;                 // ���ݒ�
	RTCVALL = day;                  // ��
	RTCVALH = month;                // ��
	RTCVALL = hour;                 // ��
	RTCVALH = 0x01;                 // �j���i0:���A1:���A�c�j
	RTCVALL = sec;                  // �b
	RTCVALH = min;                  // ��
	*/
	
	
	RTCCFGbits.RTCEN = 1;           // RTCC ��L����
	RTCCAL = RTCCALIBRATION;        // �L�����u���[�V�����l�Z�b�g
	
	
	//UART������
	//----UART������-------//
	Open1USART(USART_TX_INT_OFF & USART_RX_INT_ON &
			USART_ASYNCH_MODE & USART_EIGHT_BIT &
			USART_CONT_RX & USART_BRGH_LOW, 77); 	//���Ӂ@OpenUSART�@�֐��̍Ō�̕����̓{�[���[�g�ݒ�ō���40Mhz��9600
	
	printf("Hello world\n\r");		//�uHello�v�Əo��

	//***********���荞�ݐݒ�***********************
	//***** �D�揇�ʊ����ݎg�p�錾
	RCONbits.IPEN=1;
	//***** �჌�x���g�p���ӂ̒�`
	IPR1bits.TMR1IP=0;
	//************UART��M���荞��*************
	IPR1bits.RCIP = 0;		//UART�ϊ����荞�݂��ʊ��荞�݂ɐݒ�
	PIE1bits.RCIE= 1;		//UART�ϊ����荞�݂�����
	
	//***** �����݋���
	INTCONbits.GIEH=1;          // �����x������
	INTCONbits.GIEL=1;          // �჌�x������


	
	while(1)
	{
		//PORTB = !PORTB;
		//wait_ms(1000);
		//printf("%d\n\r",ii++);
		PRINT_TIME();
	}
}

//****** �����݂̐錾�@�D�揇�ʎg�p
#pragma interrupt High_isr save = PROD
#pragma interruptlow Low_isr save = WREG,BSR,STATUS,PROD

//***** �����݃x�N�^�W�����v���߃Z�b�g
#pragma code isrcode = 0x8
void isr_direct(void)
{
	_asm
	goto High_isr
	_endasm
}
#pragma code lowcode = 0x18
void low_direct(void)
{	_asm
	goto Low_isr
	_endasm
}
//**** �����x���@�����ݏ����֐�
#pragma code
void High_isr(void)                      // ���荞�݊֐�
{
	INTCONbits.GIEH=0;          // �����x���s����
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART���M���荞�݁H
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART��M���荞�݁H
	//INTCONbits.GIEH=1;          // �����x������
	
}                                   
//***** �჌�x�������ݏ����֐�
void Low_isr(void)                     // ���荞�݊֐�
{
	INTCONbits.GIEL=0;          // �჌�x���s����
	if(PIR1bits.TXIF)	UART_TX_isr();		//UART���M���荞�݁H
	if(PIR1bits.RCIF)	UART_RC_isr();		//UART��M���荞�݁H
	
}

//**********UART��M���荞��*************
void UART_RC_isr(void)
{
	static int i =0;
	int hour2=0,sec2=0,min2=0;
	char hoge[3]="";
	

	temp_Buf[i] = getc1USART();
	PIR1bits.RCIF = 0;		//���荞�݃t���O�N���A
	
	i++;
	printf("i=%d\n\r",i);
	printf("temp_Buf=");
	//puts1USART(temp_Buf);
	printf("%d",temp_Buf[i-1]);
	printf("\n\r");
	if(temp_Buf[i-1]==27 && i>=8)//ESC�̎�
	{
		if(temp_Buf[i-4]==':' &&temp_Buf[i-7]==':')
		{
			hoge[0]=temp_Buf[i-3];
			hoge[1]=temp_Buf[i-2];
			hoge[2]='\n';			//���s�R�[�h
			sec2=atoi(hoge);
			printf("sec2=");
			//puts1USART(hoge);
			printf("%d\n\r",sec2);
			
			hoge[0]=temp_Buf[i-6];
			hoge[1]=temp_Buf[i-5];
			hoge[2]='\n';			//���s�R�[�h
			min2=atoi(hoge);
			printf("min2=");
			//puts1USART(hoge);
			printf("%d\n\r",min2);
			
			hoge[0]=temp_Buf[i-9];
			hoge[1]=temp_Buf[i-8];
			hoge[2]='\n';			//���s�R�[�h
			hour2=atoi(hoge);
			printf("hour2=");
			//puts1USART(hoge);
			printf("%d\n\r",hour2);
			
			// --------- RTCC �����̐ݒ� ---------
			/*
			EECON2 = 0x55;                  // RTCC ���W�X�^��
			EECON2 = 0xAA;                  // unlock ��RTCC��
			RTCCFGbits.RTCWREN = 1;         // �ݒ菀��������
			RTCCFGbits.RTCPTR1 = 1;         // RTCVALH�ɃA�N�Z�X���閈��
			RTCCFGbits.RTCPTR0 = 1;         // RTCPTR�͎����I�� -1 ����
			*/
			RTCVALL = year;                 // �N
			RTCVALH = 0xFF;                 // ���ݒ�
			RTCVALL = day;                  // ��
			RTCVALH = month;                // ��
			
			RTCVALL = hour2;                 // ��
			RTCVALH = 0x01;                 // �j���i0:���A1:���A�c�j
			RTCVALL = sec2;                  // �b
			RTCVALH = min2;                  // ��
			/*
			RTCCFGbits.RTCEN = 1;           // RTCC ��L����
			RTCCAL = RTCCALIBRATION;        // �L�����u���[�V�����l�Z�b�g
			
			*/
		}
	//������
		for(;i>0;i--)
		{
			temp_Buf[i]=0;
		}
		temp_Buf[0]=0;
		i=0;
		printf("reset\n\r");
	
	}
	
}

//**********UART���M���荞��*************
void UART_TX_isr(void)
{
	/*
	int k =0;
	putcUSART(Send_Buf);
		for(k=0;k>=20;k++)
		{
			Send_Buf[k] = 0;
		}
		PIR1bits.TXIF = 0;		//���荞�݃t���O�N���A

	*/
}

/*************************************
�Q�lURL

http://sky.geocities.jp/home_iwamoto/page/P26J50/P26_B04.htm		//PIC18f RTC

**************************************/