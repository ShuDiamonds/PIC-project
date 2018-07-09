/************************************************************
�@�@MPLAB-C18�ɂ����o�̓|�[�g�̐���e�X�g�v���O����
�@�@�X�C�b�`�̓��͂��|�[�gD����s���A�X�C�b�`�̏�Ԃ�
�@�@�]���āA�w�肳�ꂽ�����_�C�I�[�h��_�ł�����B
�@�@�@�@RD0�̃X�C�b�`��OFF�̎�LED�P
�@�@�@�@RD1�̃X�C�b�`��OFF�̎�LED2
�@�@�@�@RD2�̃X�C�b�`��OFF�̎�LED3
�@�@�@�@�S�X�C�b�`��ON�̎��SLED
*************************************************************/
#include <p18f2320.h>	 // PIC18C452�̃w�b�_�E�t�@�C��
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <i2c.h>		//I2C�֐�

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
		
//****** �֐��v���g�^�C�s���O
unsigned char EPageWrite(unsigned char control, 
				unsigned int address, unsigned char *wtptr); 
unsigned char ESequentialRead( unsigned char control,unsigned int address,
				 unsigned char *rdptr, unsigned char length );


//***** ���C���֐�
void main(void)						// ���C���֐�
{
	
	//���[�J���ϐ���`
	char Memodata[17] = "EEPROM Memory!!!"; //�������݃f�[�^
	char buffer[17]="                 ";		//�ǂݏo���o�b�t�@
	char ermes[7]="       ";					//�G���[�p�o�b�t�@
	unsigned int memadrs;					//�������A�h���X
	int temp;

	
	//UART������
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 64); 	//���Ӂ@OpenUSART�@�֐��̍Ō�̕����̓{�[���[�g�ݒ�ō���40Mhz��9600�̎���64�ɂ���B�܂�48Mhz��9600�̎���77�ɂ���
	//I2C������
	OpenI2C(MASTER, SLEW_ON);               //I2C�}�X�^�[���[�h�w��
	SSPADD = 9;								//Board Rate 400kHz
	memadrs=0;
	//������
		TRISA=0x00;						   // �|�[�gA�����ׂďo�̓s���ɂ���
		TRISB=0;						// �|�[�gB�����ׂďo�̓s���ɂ���
		TRISC=0b10111111;						   // �|�[�gC�����ׂďo�̓s���ɂ���
	
	 while(1)
	{
		temp =	EPageWrite(0xA0, memadrs, Memodata);	//��������
		if (temp < 0) {						//�G���[�`�F�b�N
			itoa(-temp , ermes);
			printf("Error\n\r");
			putsUSART(ermes);
		}
		Delay10KTCYx(3);					//Wait 12msec
		itoa(memadrs, buffer);				//�A�h���X�\��
		putsUSART(buffer);
		//**** Read and Display
		temp = ESequentialRead (0xA0, memadrs, buffer, 16); //�ǂݏo��
		putsUSART(buffer);					//�ǂݏo���f�[�^�\��
		if ( temp < 0) {					//�G���[�`�F�b�N
			itoa(-temp , ermes);
			printf("Error\n\r");
			putsUSART(ermes);
		}
		memadrs = memadrs + 64;			   //���̃A�h���X��
		if(memadrs >= 0x7FFF) {
			memadrs = 0;
			printf("End of Write\n\r");			   //�I�����b�Z�[�W
			Delay10KTCYx(500);			   //�P�b�҂�
		}
	}

	
}



//******* �T�u�֐��Q  ***********************************
//******* I2C����֐�	******

/********************************************************************
*	  Function Name:	EPageWrite									*
*	  Return Value:		error condition status and/or data byte		*
*	  Parameters:		EE memory control byte with R/W set to 1	*
*	  Description:		Reads 1 byte from passed address to EE memory*
*						device. This routine can be used for any I2C*
*						EE memory device, which only uses 2 byte of *
*						address data as in the 24LC64B/256B.		*
*																	*  
********************************************************************/
unsigned char EPageWrite(unsigned char control, 
					unsigned int address, unsigned char *wtptr)
{
	IdleI2C();							// �A�C�h���m�F
	SSPCON2bits.SEN = 1;				// START condition�o��
	while ( SSPCON2bits.SEN );			// start condition���M�҂�
	if ( PIR2bits.BCLIF )				// �o�X�Փ˃`�F�b�N
		{ return ( -1 ); }				// �o�X�Փ˔��� 
	else								// ����
	{
		SSPBUF = control;				// control�o��
		while(SSPSTATbits.BF);			// ���M�҂�
		IdleI2C();						// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
			{ return(-2); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A

		SSPBUF = address >> 8;			// EEPROM�A�h���X��ʏo��
		while(SSPSTATbits.BF);			// ���M�҂�
		IdleI2C();						// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
			{ return(-3); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A

		SSPBUF = address;				// EEPROM�A�h���X���ʏo��
		while(SSPSTATbits.BF);			// ���M�҂�
		IdleI2C();						// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
			{ return(-4); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A

		if (putsI2C ( wtptr) )			// �f�[�^�A����������
			{ return (-4); }			// �G���[�`�F�b�N
	
		IdleI2C();						// �A�C�h���m�F
		SSPCON2bits.PEN = 1;			// STOP condition�o��
		while ( SSPCON2bits.PEN );		// stop condition���M�҂�
		PIR1bits.SSPIF = 0;				// SSPIF�N���A
		if ( PIR2bits.BCLIF )			// �o�X�Փ˃`�F�b�N
			{ return ( -1 ); }			// �o�X�Փ˔����G���[ 
		return ( 0 );					// ����I��
	}
}


//***** I2C�ǂ݂����֐��@�@*****
/********************************************************************
*	  Function Name:   ESequentialRead								*
*	  Return Value:	   error condition status						*
*	  Parameters:	   EE memory control, address, pointer and		*
*					   length bytes.								*
*	  Description:	   Reads data string from I2C EE memory			*
*					   device. This routine can be used for any I2C *
*					   EE memory device, which only uses 2 bytes of *
*					   address data as in the 24LC256B.				*
*																	*  
********************************************************************/

unsigned char ESequentialRead( unsigned char control, 
			unsigned int address, unsigned char *rdptr, unsigned char length )
{
	IdleI2C();								// �A�C�h���m�F
	SSPCON2bits.SEN = 1;					// START condition�o��
	while ( SSPCON2bits.SEN );				// start condition���M�҂�
	if ( PIR2bits.BCLIF )					// �o�X�Փ˃`�F�b�N
		{ return ( -1 ); }					// �o�X�Փ˔����G���[ 
	else									// ����
	{
		SSPBUF = control;					// control Write Mode�o��
		while(SSPSTATbits.BF);				// ���M�҂�
		IdleI2C();							// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )			// ACK��M�҂�
			{ return(-2); }					// ACK����
		PIR1bits.SSPIF = 0;					// SSPIF�N���A

		SSPBUF = address >> 8;				// EEPROM�A�h���X��ʏo��
		while(SSPSTATbits.BF);				// ���M�҂�
		IdleI2C();							// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )			// ACK��M�҂�
			{ return(-3); }					// ACK����
		PIR1bits.SSPIF = 0;					// SSPIF�N���A

		SSPBUF = address;					// EEPROM�A�h���X���ʏo��
		while(SSPSTATbits.BF);				// ���M�҂�
		IdleI2C();							// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )			// ACK��M�҂�
			{ return(-4); }					// ACK����
		PIR1bits.SSPIF = 0;					// SSPIF�N���A

		SSPCON2bits.RSEN = 1;				// RESTART condition�o��
		while ( SSPCON2bits.RSEN );			// re-start condition���M�҂�
		if ( PIR2bits.BCLIF )				// �o�X�Փ˃`�F�b�N
			{ return ( -5 ); }				// �o�X�Փ˔����G���[
		else								// ����
		{
			SSPBUF = control+1;				// control Read Mode�o��
			while(SSPSTATbits.BF);			// ���M�҂�
			IdleI2C();						// �A�C�h���m�F
			if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
				{ return(-6); }				// ACK����
			PIR1bits.SSPIF = 0;				// SSPIF�N���A

			if (getsI2C( rdptr, length ) )	// �A���ǂݍ���
				{ return (-7); }			// �G���[�`�F�b�N

			SSPCON2bits.ACKDT = 1;			// NACK����(Not ACK)
			SSPCON2bits.ACKEN = 1;			// NACK���M�J�n
			while ( SSPCON2bits.ACKEN );	// ���M�҂�
			PIR1bits.SSPIF = 0;				// SSPIF�N���A

			SSPCON2bits.PEN = 1;			// stop condition�o��
			while ( SSPCON2bits.PEN );		// ���M�҂�
			if ( PIR2bits.BCLIF )			// �o�X�Փ˃`�F�b�N
				{ return ( -7 ); }			// �o�X�Փ˔����G���[
			else							// ����
			{
				PIR1bits.SSPIF = 0;			// SSPIF�N���A
				return ( 0 );				// ����I��
			}
		}
	}
}

//******* I2C�����݊֐�	  ******
/********************************************************************
*	  Function Name:	EByteWrite								   *
*	  Return Value:		error condition status						*
*	  Parameters:		EE memory device control, address and data	*
*						bytes.										*
*	  Description:		Write single data byte to I2C EE memory		*
*						device. This routine can be used for any I2C*
*						EE memory device, which only uses 2 byte of *
*						address data as in the 24LC64B/256B.		*
*																	*  
********************************************************************/
unsigned char EByteWrite( unsigned char control, 
			unsigned int address, unsigned char data )
{
	IdleI2C();							// �A�C�h���m�F
	SSPCON2bits.SEN = 1;				// start condition�o��
	while ( SSPCON2bits.SEN );			// start condition�I���҂�
	if ( PIR2bits.BCLIF )				// �o�X�Փ˃`�F�b�N
		{ return ( -1 ); }				// �Փ˃G���[����
	else								// ����̎�
	{
		SSPBUF = control;				// control�f�[�^�o��
		while(SSPSTATbits.BF);			// ���M�҂�
		IdleI2C();						// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
			{ return(-2); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A

		SSPBUF = address >> 8;			// EEPROM�A�h���X��ʏo��
		while(SSPSTATbits.BF);			// ���M�҂�
		IdleI2C();						// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
			{ return(-3); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A

		SSPBUF = address;				// EEPROM�A�h���X���ʏo��
		while(SSPSTATbits.BF);			// ���M�҂�
		IdleI2C();						// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )		// ASCK��M�҂�
			{ return(-4); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A

		SSPBUF = data;					// �������݃f�[�^�o��
		while(SSPSTATbits.BF);			// ���M�҂�
		IdleI2C();						// �A�C�h���m�F	 
		if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
			{ return(-5); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A
		
		SSPCON2bits.PEN = 1;			// STOP condition�o��
		while ( SSPCON2bits.PEN );		// stop condition�I���҂�
		PIR1bits.SSPIF = 0;				// SSPIF�N���A
		if ( PIR2bits.BCLIF )			// �o�X�Փˊm�F
			{ return ( -1 ); }			// �Փ˔��� 
		return ( 0 );					// ����I��
	}
}

/********************************************************************
*	  Function Name:	EByteRead									*
*	  Return Value:		error condition status and/or data byte		*
*	  Parameters:		EE memory control byte with R/W set to 1	*
*	  Description:		Reads 1 byte from passed address to EE memory*
*						device. This routine can be used for any I2C*
*						EE memory device, which only uses 2 byte of *
*						address data as in the 24LC64B/256B.		*
********************************************************************/
unsigned int EByteRead( unsigned char control, unsigned int address )
{
	IdleI2C();							// �A�C�h���m�F
	SSPCON2bits.SEN = 1;				// START condition�o��
	while ( SSPCON2bits.SEN );			// start condition�I���҂�
	if ( PIR2bits.BCLIF )				// �o�X�Փ˃`�F�b�N
		{ return ( -1 ); }				// �o�X�Փ˔����G���[ 
	else
	{
		SSPBUF = control;				// control Write Mode�o��
		while(SSPSTATbits.BF);			// ���M�҂�
		IdleI2C();						// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
			{ return(-2); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A

		SSPBUF = address >> 8;			// EEPROM�A�h���X��ʏo��
		while(SSPSTATbits.BF);			// ���M�҂�
		IdleI2C();						// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
			{ return(-3); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A

		SSPBUF = address;				// EEPROM�A�h���X���ʏo��
		while(SSPSTATbits.BF);			// ���M�I���҂�
		IdleI2C();						// �A�C�h���m�F
		if ( SSPCON2bits.ACKSTAT )		// ACK��M�҂�
			{ return(-4); }				// ACK����
		PIR1bits.SSPIF = 0;				// SSPIF�N���A

		SSPCON2bits.RSEN = 1;			// RESTART condition�o��	   
		while ( SSPCON2bits.RSEN );		// RESART condition���M�҂� 
		if ( PIR2bits.BCLIF )			// �o�X�Փ˃`�F�b�N
			{ return ( -5 ); }			// �o�X�Փ˔����G���[  
		else							// ����
		{
			SSPBUF = control+1;			// cntorol Read Mode�o��
			while(SSPSTATbits.BF);		// ���M�҂�
			IdleI2C();					// �A�C�h���m�F
			if ( SSPCON2bits.ACKSTAT )	// ACK��M�҂�
				{ return(-6); }			// ACK����
			PIR1bits.SSPIF = 0;			// SSPIF�N���A

			SSPCON2bits.RCEN = 1;		// �}�X�^�[���[�h�P�o�C�g��M
			while ( SSPCON2bits.RCEN ); // ��M�����҂�
			SSPCON2bits.ACKDT = 1;		// ACK��ԐM���Ȃ��iNot ACK)
			SSPCON2bits.ACKEN = 1;		// �o�X�Փ˃N���A
			while ( SSPCON2bits.ACKEN ); // ACK���M
			PIR1bits.SSPIF = 0;			// SSPIF�N���A

			SSPCON2bits.PEN = 1;		// stop condition�o��
			while ( SSPCON2bits.PEN );	// stop condition���M�҂�
			if ( PIR2bits.BCLIF )		// �o�X�Փ˃`�F�b�N
				{ return ( -7 ); }		// �o�X�Փ˔����G���[
			else						// ����
			{
				PIR1bits.SSPIF = 0;		// SSPIF�N���A
				return ( (unsigned int) SSPBUF );	// ����I��
			}
		}
	}
}


