/*********************************************************************
*
*�@USB�ڑ� �f�[�^���K�[�V�X�e���v���O����
*	USB�t���[�����[�N�C�����
*�@�@�R���t�B�M�����[�V�����̐ݒ�ύX
*�@�@main.c�@�̕ύX�@�������폜�@IO�ݒ�ύX
*�@�@io_cfg.h�̕ύX�@LED�폜�@IO���j�^�폜
*�@�@user.c�̕ύX�@�@���s���e�폜
*�@�@usbcfg.h�̕ύX�@IO���j�^�폜
*
*********************************************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                      // Required
#include "system\usb\usb.h"                       // Required
#include "io_cfg.h"                               // Required
#include "system\usb\usb_compile_time_validation.h"
#include	"delays.h"
#include "lcd_lib.h"
#include	"i2c.h"
#include "string.h"

//****�ϐ���`�@********************************************* */
#pragma udata
char input_buffer[64];						// USB���̓o�b�t�@
char output_buffer[64];						// USB�o�̓o�b�t�@

char LogFlag, MsrFlag;						// ���O,�v���J�n�t���O
char Period;									// ���O�����萔�G���A
char PeriCounter;								// �����p�J�E���^
unsigned int Result;							// �A�i���O�v���f�[�^
float Battery;								// �o�b�e���d���ϐ�
char ErrFlag, FullFlag;						// �G���[�A�o�b�t�@��t�t���O
unsigned int	 WRAdrs, RDAdrs;					// EEPROM�A�h���X


char MsgStart[] ="Start!";						// �������b�Z�[�W
char MsgLCD1[] = "C0=xxxx  C1=xxxx";			// LCD���b�Z�[�W
char MsgLCD2[] = "BT=x.xxV xxxxxxx";			// �o�b�e���d���\��
char MsgFull[] = "Full!! ";					// �������I�[�o�[�t���[
char MsgErase[] ="EraseEEPROM ";				// EEPRO�������R�}���h
char MsgEnd[] = "End!";
char MsgLow[]  = "Low Bat";					// �d�r�d���ቺ
char MsgPer[]  = "Pxx00ms";					// ���O�����\��
char MsgMsr[]  = "Mesure ";					// �v���J�n
char MsgDmp[]  = "DumpAll";					// ���������M
char MsgErr[]  = "Cmd Err";					// �R�}���h�G���[�\��
char MsgTest[] = "EEPROMTest xxxx";				// �e�X�g
char MsgTX[] = "CH0=xxxx  CH1=xxxx  BAT=xxxx\r\n";
unsigned char TXBuffer[64];					// USB���M�o�b�t�@

//** �v���g�^�C�s���O *******************************/
unsigned int ADconv(char chan);
void SaveMem(unsigned int data);
void itostring(char digit, unsigned int data, char *buffer);
void ISRProcess(void);
void WriteMem(unsigned char data);
unsigned char ReadMem(unsigned int adrs);
void ftostring(int seisu, int shousu, float data, char *buffer);

////// ���荞�݃x�N�^��`�@/////
#pragma code high_vector=0x08
void interrupt_at_high_vector(void)
{
    _asm goto ISRProcess _endasm
}
#pragma code

////// �^�C�}0���荞�ݏ��� 100msec
#pragma interrupt ISRProcess
void ISRProcess(void)
{
	INTCONbits.T0IF = 0;				// Interrupt Flag Clear
	TMR0H = 0xB6;						// 12MHz/64/18750=10Hz
	TMR0L = 0xC1;	
	PeriCounter--;					// �����J�E���^�[�P
	if(PeriCounter == 0)				// ������v
	{
		LogFlag = 1;					// ���O�J�n�t���O�I��
		PeriCounter = Period;			// �����J�E���^�ăZ�b�g
	}
	if(PORTCbits.RC0 == 0)				// �J�n�A��~�{�^���`�F�b�N
	{
		if(MsrFlag == 2)				// �J�n�ς݂Ȃ��~
			MsrFlag = 0;
		else
			MsrFlag = 1;				// �J�n�I��
	}
}

#pragma code
/*********************************************************************
 * ���C������
 ********************************************************************/
void main(void)
{
	unsigned char i;
	unsigned int L;
	
	// I/O�ݒ�
	CMCON = 0x07;						// �R���p���[�^�I�t
   	LATB = 0;							// �t���\���|�[�g���Z�b�g
  	LATC  = 0xC0;						// LED off
 	TRISA = 0xFF;						// �S����
	TRISB = 0x0;						// �t���\��
 	TRISC = 0x31;						// LED Output Mode, SW input
 	// A/D�����ݒ�
 	ADCON1 = 0x1A;					// AN0-AN2,AN4 Analog Input Vref+
 	ADCON2 = 0x95;					// 4Tad, Fosc/16
 	ADCON0 = 0x01;					// AD on
 	// �^�C�}������
	T0CON = 0x85;						// Int 1/64
	TMR0H = 0xB6;						// 12MHz/64/18750=10Hz
	TMR0L = 0xC1;
	/// I2C������
	OpenI2C(MASTER, SLEW_ON);
	SSPADD = 27;						//400kHz @48MHz
	/// USB������
	mInitializeUSBDriver();         	// See usbdrv.h
	/// �t���\���평����
	lcd_init();						// LCD������
	lcd_str(MsgStart);				// �������b�Z�[�W�\��
	Delay10KTCYx(500);				// 0.5�b
	/// �ϐ�������
	Period = 10;
	PeriCounter = 10;
	WRAdrs = 0;
	RDAdrs = 0;
	LogFlag = 0;
	MsrFlag = 0;
	FullFlag = 0;
	// Enable Interrupt
	RCONbits.IPEN = 0;				// No Priority
	INTCON = 0xA0;					// Enable Interrupt
	
    /// ���C�����[�v
    while(1)
    {
    		USBCheckBusStatus();        						// USB�ڑ��`�F�b�N
    		if(UCFGbits.UTEYE!=1)								// �A�C�p�^�[�����[�h�I�t��
    			USBDriverService();							// USB�C�x���g�|�[�����O
		CDCTxService();									// �N���X����M���s
		/// ����M�f�[�^�̏������s
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
    			if(getsUSBUSART(input_buffer,64))				// �f�[�^��M�|�[��
    			{
            		LATCbits.LATC7 = !LATCbits.LATC7;			// LED���]
	    			switch(input_buffer[0])					// �ŏ��̂P�����`�F�b�N
	    			{
		    			case '0':	// �����ݒ�ƃI�[�v��
						RDAdrs = 0;						// �A�h���X�����l�Z�b�g
						WRAdrs = 0;
						MsrFlag = 0;						// �v����~		    				
           			    	if(mUSBUSARTIsTxTrfReady())		// ���M�\���H
        		   			 	putrsUSBUSART("OK");			// OK�f�[�^���M
        		   			 break;         			 	
		    			case '1':	// �����ݒ�
						Period = input_buffer[1];			// �����f�[�^�擾
						strcpy(MsgLCD2+9, MsgPer);			// �����\��
						itostring(2, Period, MsgLCD2+10);
						lcd_cmd(0xC0);
						lcd_str(MsgLCD2);
           			 	break;
           			case '2':	// �f�[�^���ڑ��M
 						lcd_cmd(0xC0);
						strcpy(MsgLCD2+9, MsgMsr);			// �v���J�n�\��
						lcd_str(MsgLCD2);	          			
           			    	if(mUSBUSARTIsTxTrfReady())		// ���M�\���H
        					{
		    					Result = ADconv(0);			// CH0����
		    					itostring(4, Result, MsgTX+4);
	           			 	Result = ADconv(1);			// CH1����
    		       			 	itostring(4, Result, MsgTX+14);
	           			 	Result = ADconv(4);			// CH4����
     	      			 	itostring(4, Result, MsgTX+24);
     	      			 	
        		   			 	mUSBUSARTTxRam((byte*)(MsgTX), 30);	// �f�[�^���M
         		   		}
           				break;
           			case '3':	// �������_���v
  						lcd_cmd(0xC0);
						strcpy(MsgLCD2+9, MsgDmp);			// �������_���v�\��
						lcd_str(MsgLCD2);	          			
       					if(mUSBUSARTIsTxTrfReady())		// ���M�\�� 
	       				{
							for(i=0; i<64; i++)			// 64�o�C�g���ǂݏo��
							{
         						TXBuffer[i] = ReadMem(RDAdrs);	// ���݃A�h���X�̃f�[�^
        							RDAdrs++;				// �A�h���X�{�P
        						}
							mUSBUSARTTxRam(TXBuffer, 64);	// �f�[�^���M
						}
						break;
					case '4':	// ������Read�e�X�g
       					if(mUSBUSARTIsTxTrfReady())		// ���M�\��
						{
							lcd_clear();
							itostring(4, RDAdrs, MsgTest+11);
							lcd_str(MsgTest);				// ���݃A�h���X�\��
							for(i=0; i<64; i++)			// 64�o�C�g�ǂݏo��
							{
								TXBuffer[i] = ReadMem(RDAdrs);	// ASCII�R�[�h�Ƃ��ĕ\��
								RDAdrs ++;
							}
							mUSBUSARTTxRam(TXBuffer, 64);	// �f�[�^���M	
						}
						break;
					case '5':	// ������Write�e�X�g���s
						lcd_clear();
						itostring(4, WRAdrs, MsgTest+11);	// �������݃e�X�g�\��
						lcd_str(MsgTest);
						for(i=0; i<64; i++)				// 64�o�C�g��������
						{
							WriteMem(i+0x20);				// ASCII�R�[�h��������
							WRAdrs++;
						}
						break;
					case '6':	// �������S�����@������
						lcd_clear();
						lcd_str(MsgErase);				// EERPOM�������b�Z�[�W
						for(L=0; L<0x7FFF; L++)			// �P�`�b�v��32k�o�C�g
						{
							WriteMem(0xFF);				// FF�ŏ���
							WRAdrs++;
						}
						lcd_str(MsgEnd);					// �Q�`�b�v��32k�o�C�g
						for(L=0; L<0x7FFF; L++)
						{
							WriteMem(0xFF);				// FF�ŏ���
							WRAdrs++;
						}
						lcd_str(MsgEnd);					// �I�����b�Z�[�W
						WRAdrs = 0;
						break;
					case '7':	// �v���J�n/��~
					 	if(MsrFlag == 2)
					 		MsrFlag = 0;					// �J�n�ς݂Ȃ��~
					 	else
						 	MsrFlag = 1;					// �J�n
					 	break;
		    			default:
  						lcd_cmd(0xC0);					// �G���[�R�}���h�\��
						strcpy(MsgLCD2+9, MsgErr);
						lcd_str(MsgLCD2);
						RDAdrs = 0;						// �A�h���X�����l�Z�b�g
						WRAdrs = 0;
		    			 	break;
		    		}
    			}
    		}
		//// ���O���s�����@������
  		if((LogFlag == 1) && (MsrFlag != 0))				// ���O�t���O�I�����H
    		{	    		
	    		LATCbits.LATC6 = !LATCbits.LATC6;				// LED���]
	    		LogFlag = 0;									// �P��݂̂ŃN���A
	    		// Mesure CH0
			Result = ADconv(0);							// CH0�v��
			SaveMem(Result);								// �������ɕۑ�
			itostring(4, Result, MsgLCD1+3);				// LCD�\��
			// Mesure CH1
			Result = ADconv(1);							// CH1�v��
			SaveMem(Result);								// �������ɕۑ�
			itostring(4, Result, MsgLCD1+12);				// LCD�\��
			lcd_clear();
			lcd_str(MsgLCD1);
		    	Result = ADconv(4);							// CH4�v��
		    	SaveMem(Result);								// �������ɕۑ�
			Battery = (Result*5.00)/1024.0;					// �d���l�ɕϊ�
		    	ftostring(1, 2, Battery, MsgLCD2+3);		  		// LCD�\��
		    	lcd_cmd(0xC0);
			lcd_str(MsgLCD2);
			// �o�b�e����d�����o(3V�ȉ��Œ�~) USB�o�X�p���[�łȂ��Ƃ�		

/*			if((PORTCbits.RC1 == 0) && (Result < 614))		// ��d���`�F�b�N
			{
				lcd_cmd(0xC0);
				strcpy(MsgLCD2+9, MsgLow);
				lcd_str(MsgLCD2);
	    			_asm sleep _endasm						// Then sleep
	    		}
*/
	    		SaveMem(0xFFFF);								// �I���t���O�ۑ�	
	    		WRAdrs = WRAdrs - 2;							// �A�h���X��߂�
			if(WRAdrs > 0xFFFA)							// �ŏI�A�h���X���H
			{
				WRAdrs = 0;								// �I���A�A�h���X0��
				lcd_cmd(0xC0);
				strcpy(MsgLCD2+9, MsgFull);				// LCD�\��
				lcd_str(MsgLCD2);
			}
			MsrFlag = 2;									// �v���J�n���� 
	    }
     }
}

/// �A�i���O�f�[�^����
unsigned int ADconv(char chan)
{
	unsigned int data;
	 
	ADCON0 = (chan << 2) + 0x01;			// �`���l���I��
	Delay10TCYx(20);	    					// �A�N�C�W�V�����^�C��
	ADCON0bits.GO = 1;					// �ϊ��J�n
	while(ADCON0bits.GO);					// �ϊ��I���҂�
	data = ADRESH * 256 + ADRESL;			// �f�[�^����
	return(data);					
}
/// EEPROM�Q�o�C�g�ۑ�
void SaveMem(unsigned int data)
{
	WriteMem(data >> 8);					// ��ʕۑ�
	WRAdrs++;
	WriteMem(data & 0xFF);					// ���ʕۑ�
	WRAdrs++;
}

/// EEPROM�o�C�g��������
void WriteMem(unsigned char data)
{
	unsigned char Err, Chip;
	
	if(WRAdrs < 0x8000)					// �`�b�v�I��
		Chip = 0xA0;
	else
		Chip = 0xA2;
	///
	IdleI2C();
	StartI2C();							// �X�^�[�g�V�[�P���X
	while ( SSPCON2bits.SEN );
	WriteI2C(Chip);						// �R���g���[��
	IdleI2C();
	WriteI2C((WRAdrs >> 8) & 0x7F);			// �A�h���X���
	IdleI2C();
	WriteI2C(WRAdrs);						// �A�h���X����
	IdleI2C();
	WriteI2C(data);						// �f�[�^�o��
	IdleI2C();
	StopI2C();							// �X�g�b�v�V�[�P���X
	while ( SSPCON2bits.PEN );
	Delay10KTCYx(5);						// 5msec�҂�
}
//// EEPROM�o�C�g�ǂݏo��
unsigned char ReadMem(unsigned int adrs)
{
	unsigned char data, Chip;

	if(adrs < 0x8000)						// �`�b�v�I��
		Chip = 0xA0;
	else
		Chip = 0xA2;
	////
	IdleI2C();
	StartI2C();							// �X�^�[�g�V�[�P���X
	while ( SSPCON2bits.SEN );
	WriteI2C(Chip);						// �R���g���[��
	IdleI2C();
	WriteI2C((adrs >> 8) & 0x7F);			// �A�h���X���
	IdleI2C();
	WriteI2C(adrs);						// �A�h���X����
	IdleI2C();
	RestartI2C();							// ���X�^�[�g�V�[�P���X
	while ( SSPCON2bits.RSEN );
	WriteI2C(Chip + 0x01);					// ���[�h�R���g���[��
	IdleI2C();
	SSPCON2bits.RCEN = 1;
	while ( SSPCON2bits.RCEN ); 			// �f�[�^�҂�
	NotAckI2C();              				// NAK���M
	while ( SSPCON2bits.ACKEN );  
	StopI2C();              				// �X�g�b�v�V�[�P���X
	while ( SSPCON2bits.PEN );  
	return(SSPBUF);						// �f�[�^���o��
}
///// ���l���當����ɕϊ�
void itostring(char digit, unsigned int data, char *buffer)
{
	char i;
	buffer += digit;						// ������̍Ō�
	for(i=digit; i>0; i--) {				// �ŉ��ʌ������ʂ�
		buffer--;						// �|�C���^�[�P
		*buffer = (data % 10) + '0';		// ���̌����l�𕶎��ɂ��Ċi�[
		data = data / 10;					// ��-1
	}
}
////// Float���當����֕ϊ�
//////�@���v�L�����͂T���ȉ��Ƃ��邱��
void ftostring(int seisu, int shousu, float data, char *buffer)
{
	int i, dumy;

	if(shousu != 0)						//�����������肩
		buffer += seisu+shousu+1;			//�S�̌����{�����_
	else								//���������Ȃ��̂Ƃ�
		buffer += seisu + shousu;			//�S�̌����̂�
	buffer--;							//�z��|�C���^-1
	for(i=0; i<shousu; i++)				//�������𐮐��ɕϊ�
		data = data * 10;					//�P�O�{
	dumy = (int) (data + 0.5);				//�l�̌ܓ����Đ����ɕϊ�
	for(i=shousu; i>0; i--)	{			//�����������J��Ԃ�
		*buffer =(dumy % 10)+'0';			//���l�𕶎��ɕϊ��i�[
		buffer--;						//�i�[�ꏊ���ʂ����ʂ�
		dumy /= 10;						//���̌���
	}
	if(shousu != 0) {						//������0�Ȃ珬���_�Ȃ�
		*buffer = '.';					//�����_���i�[
		buffer--;						//�|�C���^-1
	}
	for(i=seisu; i>0; i--) {				//���������J��Ԃ�
		*buffer = (dumy % 10)+'0';			//���l�𕶎��ɕϊ��i�[
		buffer--;						//�|�C���^-1
		dumy /= 10;						//���̌���
	}
}    
