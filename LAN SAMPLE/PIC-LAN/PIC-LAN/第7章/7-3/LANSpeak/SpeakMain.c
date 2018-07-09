/**************************************************
*  �����[�g�X�s�[�J�V�X�e��
*�@ICMP + ARP + DHCP + NBNS + UDP�A�v��
*  �v���W�F�N�g���FLANSpeak
**************************************************/
// �錾�ƃw�b�_�t�@�C���̃C���N���[�h
#include "TCPIP Stack/TCPIP.h"

// �X�^�b�N���Ŏg���ϐ��Ń��C���Œ�`���Ă���
APP_CONFIG AppConfig;
BYTE myDHCPBindCount = 0xFF;
#if !defined(STACK_USE_DHCP_CLIENT)
	#define DHCPBindCount	(1)
#endif

static  char AppMode __attribute__((far));
static char SW_Flag, Full_Flag, First __attribute__((far));
static int BufIndex;
const BYTE SinData[20] = {				// �e�X�g�p�����g�f�[�^
	127,202,248,247,200,125,50,5,
	7,56,132,206,249,246,196,120,
	46,4,9,60
};
#define MaxBuf 800
#pragma udata PINPON = 0x300
static BYTE BufferA[MaxBuf] __attribute__((far));
static BYTE BufferB[MaxBuf] __attribute__((far));
static BYTE TempBuffer[MaxBuf+10] __attribute__((far));

// �֐��v���g�^�C�s���O
static void InitAppConfig(void);
static void InitializeBoard(void);
static void DisplayIPValue(IP_ADDR IPVal);
void UDPControlTask(void);
void SpeakerControl(void);

/**********************************************
* ���荞�ݏ���
***********************************************/
// ��ʊ��荞�݁i�C���^�[�o���^�C�}�j
#pragma interruptlow LowISR
void LowISR(void){
    TickUpdate();
}
#pragma code lowVector=0x18
void LowVector(void){_asm goto LowISR _endasm}

// ���ʊ��荞�݁i�^�C�}1�j
#pragma code 		// Return to default code section
#pragma interrupt HighISR
void HighISR(void){
	SpeakerControl();
}
#pragma code highVector=0x8
void HighVector(void){_asm goto HighISR _endasm}

#pragma code 		// Return to default code section
/************************************************
*  �^�C�}�P���荞�ݏ���
*  0.�Đ��I���A�^�C�}1��~
*  1.�e�X�g���[�h�@�����g�o��
*  2.A/D���͂��^��
*  3.��M�f�[�^�Đ�
*  4.�܂�Ԃ��e�X�g
************************************************/
void SpeakerControl(void){
	LED1_IO = 1;								// �ڈ�@125usec
   	TMR1H = 0xFB;								// �^�C�}1�ăZ�b�g
   	TMR1L = 0xEA;								// 8kHz�ɂȂ�悤������
	/***** ���[�h�ɂ�菈������@*****/
	switch(AppMode) {
		case 0: 	break;						// �Đ��I��
		case 1:								// �e�X�g���[�h
			CCPR1L = SinData[BufIndex++];		// �����g�o��
			if(BufIndex >= 20)
				BufIndex = 0;					// �J��Ԃ�
			break;
		case 2:								// �Đ����[�h
			if(SW_Flag) {						// �o�b�t�@���ݐ؂�ւ�
				CCPR1L = BufferA[BufIndex++];	// 
				LED2_IO = 1;					// �ڈ�o�b�t�@�؂�ւ�
				if(BufIndex >= MaxBuf){		// �I���܂Ői�񂾂�؂�ւ�
					BufIndex = 0;				// �o�b�t�@�C���f�b�N�X�N���A
					Full_Flag = 1;			// �o�b�t�@��A���̃f�[�^�v��
					SW_Flag = 0;				// �o�b�t�@�؂�ւ�
				}
			}
			else {
				LED2_IO = 0;					// �ڈ�o�b�t�@�؂�ւ�
				CCPR1L = BufferB[BufIndex++];	
				if(BufIndex >= MaxBuf){		// �I���܂Ői�񂾂�؂�ւ�
					BufIndex = 0;				// �o�b�t�@�C���ŃN�X�N���A
					Full_Flag = 1;			// �o�b�t�@��A���̃f�[�^�v��
					SW_Flag = 1;				// �o�b�t�@�؂�ւ�
				}
			}
			break;
		case 3:								// �^�����[�h�̂Ƃ�
			if(SW_Flag)						// �o�b�t�@����
				BufferA[BufIndex++] = ADRESH;
			else
				BufferB[BufIndex++] = ADRESH;
			if(BufIndex >= MaxBuf){			// �o�b�t�@��t��
				BufIndex = 0;
				SW_Flag = !SW_Flag;			// �o�b�t�@�؂�ւ�
				Full_Flag = 1;				// �o�b�t�@��t�A���M���s
			}
			ADCON0 = 0x0B;					// ����A/D�ϊ��X�^�[�g	
			break;
		case 4:								// ���[�J���܂�Ԃ����[�h
			CCPR1L = ADRESH;					// ���̓f�[�^��PWM�֏o��
			ADCON0 = 0x0B;					// ����A/D�ϊ��X�^�[�g
			break;
		default: break;
	}
	PIR1bits.TMR1IF = 0;						// �^�C�}�P���荞�݃t���O�N���A
	LED1_IO = 0;								// �ڈ�@�^�C�}�P�������ԃ`�F�b�N
}


/************************************************
*  ���C���֐�
************************************************/
void main(void)
{
    static TICK t = 0;
	BYTE i;

    // �n�[�h�E�F�A������
    InitializeBoard();
	// �A�v���P�[�V�����ϐ�������
	AppMode = 1;								// �����l���e�X�g�Ƃ���
	First = 0;								// �ŏ��t���O�I��
	BufIndex = 0;								// �o�b�t�@�C���f�b�N�X�N���A
	// LCD�̏������Ə����\��
	LCDInit();
	strcpypgm2ram((char*)LCDText, "TCPStack " VERSION "                  ");
	LCDUpdate();								// LCD�\���o��
    // �C���^�[�o���^�C�}������
    TickInit();
    // �A�v���P�[�V�����̏������i�ϐ��̏������j
    InitAppConfig();
	// �X�^�b�N������(MAC, ARP, TCP, UDP)
    StackInit();
	/********** ���C�����[�v  ********************/
    while(1)
    {
		// LED0�̓_�Łi1�b�Ԋu�j
		if(TickGet() - t >= TICK_SECOND/2ul)
        	{
            	t = TickGet();
            	LED0_IO ^= 1;						// LED���]
        	}
		// �X�^�b�N�̑���M���s�^�X�N�i��莞�ԓ��Ɏ��s�K�{�j
        	StackTask();
		// NBNS�ɂ�閼�O����
		NBNSTask();
        // UDP����M�f��
		UDPControlTask();						// (�V�K�ǉ�)
        // �f�t�H���gIP�A�h���X��LCD�ւ̕\��
        if(DHCPBindCount != myDHCPBindCount)		// �ς�����Ƃ������\��
        {
            myDHCPBindCount = DHCPBindCount;
            DisplayIPValue(AppConfig.MyIPAddr);	// IP�A�h���XLCD�\��
        }
    }
}
/***************************************************
*  IP�A�h���X��LCD�ւ̕\���֐�
*   ***.***.***.***�`���@�i�[���T�v���X�j
**************************************************/
static void DisplayIPValue(IP_ADDR IPVal)
{
    BYTE IPDigit[4];							// ���[�J���ϐ�
	BYTE i;
	BYTE j;
	BYTE LCDPos=16;
	// �\�����s
	for(i = 0; i < sizeof(IP_ADDR); i++)
	{
	    uitoa((WORD)IPVal.v[i], IPDigit);		// IP1���̕����ϊ�
		for(j = 0; j < strlen((char*)IPDigit); j++)
		{
			LCDText[LCDPos++] = IPDigit[j];		// �o�b�t�@�Ɋi�[
		}
		if(i == sizeof(IP_ADDR)-1)
			break;
		LCDText[LCDPos++] = '.';				// �h�b�g�̊i�[
	}
	if(LCDPos < 32)							// 32�����ȉ��ŕ\��
		LCDText[LCDPos] = 0;
	LCDUpdate();								// �\�����s
}

/**********************************************
*  �����[�g�X�s�[�J �A�v���P�[�V����
*  �R�}���h��M�Ńf�[�^����M�J�n�A�I��
**********************************************/
#define REMOTE_PORT	10001						// UDP�̃|�[�g�ԍ��̎w��
#define LOCAL_PORT	10002
const BYTE MD[] = "MDOK";							// �ԑ����b�Z�[�W
const BYTE END[] = "END";

void UDPControlTask(void)
{
	/// ���[�J���ϐ��̒�`
	static enum {								// �X�e�[�g�}�V���̒�`	
		CONT_IDLE = 0,
		CONT_LISTEN,
		CONT_EXEC
	} ContSM = CONT_IDLE;	
	static UDP_SOCKET		MySocket;				// ��M�\�P�b�g���ێ�����	
	unsigned int RcvLen;	

	/***** �֐����s���@�X�e�[�g�}�V���ɂ��i�s  ******/
	switch(ContSM){
		case CONT_IDLE:		/**** �A�C�h�������M�҂��� *****/
			MySocket = UDPOpen(LOCAL_PORT, NULL, REMOTE_PORT);
			if(MySocket == INVALID_UDP_SOCKET)
				return;									// �I�[�v�����s���^�[��
			else
				ContSM++;								// �I�[�v��������M�҂���
			break;
		case CONT_LISTEN:		/****** ��M�@*****/
			if(!UDPIsGetReady(MySocket)){					// ��M�L���`�F�b�N
				return;									// ��M�Ȃ������^�[��
			}
			RcvLen = UDPGetArray(TempBuffer, sizeof(TempBuffer));	// ��M�f�[�^�擾
			UDPDiscard();									// UDP�\�P�b�g�؂藣��
    			ContSM++;									// �X�e�[�^�X�X�V�ԑ���
			/// ��M�f�[�^ �L�[���[�h�`�F�b�N
			if(!(TempBuffer[0] == 'S')) {					// �J�n�L�[
				ContSM = CONT_LISTEN;						// �L�[���[�h�s��v
				return;									// Listen�ɖ߂�
			}			
			else{			/******* �f�[�^����  *****/
				// �^�C�}��~�A�ĊJ����
				if(TempBuffer[1] == 'E')					// ��M�I���R�}���h�`�F�b�N
					T1CONbits.TMR1ON = 0;					// �^�C�}1��~
				else
					T1CONbits.TMR1ON = 1;					// �^�C�}�P�ăX�^�[�g
				// ��M�R�}���h���
				switch(TempBuffer[1])	{					// �R�}���h���o��
					case 'T':							// �e�X�g���[�h�R�}���h
						AppMode = 1;						// �e�X�g���[�h�ɐ؂�ւ�
						BufIndex = 0;
						First = 0;
						break;
					case 'O':	/**** �����f�[�^��M(�Đ��j  ****/
						AppMode = 2;						// ��M���[�h�ɐ؂�ւ�
						if(!First){						// �ŏ��̏ꍇ�`�F�b�N
							BufIndex = 0;					// �ŏ��̂Ƃ������N���A
							memcpy((void *)BufferA, (void *)TempBuffer+2, MaxBuf);							
							SW_Flag = 1;					// ���݃t���O�Z�b�g
							Full_Flag = 0;				// �������̃f�[�^�v��
							First = 1;					// �ŏ��t���O�Z�b�g
						}
						else{							// 2��ڈȍ~
							if(SW_Flag)					// �o�b�t�@�Ɍ��݂Ɋi�[
								memcpy((void *)BufferB, (void *)TempBuffer+2, MaxBuf);
							else
								memcpy((void *)BufferA, (void *)TempBuffer+2, MaxBuf);
						}
						break;
					case 'I':	/*** �����f�[�^���M(�^���j ***/						
						AppMode = 3;						// ���M���[�h�ɐ؂�ւ�
						BufIndex = 0;						// ���M�����N���A
						First = 0;
						SW_Flag = 1;
						Full_Flag = 0;
						break;
					case 'M':	/**** �܂�Ԃ��e�X�g���[�h ****/
						AppMode = 4;						// �܂�Ԃ����[�h�ɐ؂�ւ�
						First = 0;
						break;
					case 'E':	/**** �Đ��I���R�}���h *****/
						AppMode = 0;						// ��M�I�����[�h��
						First = 0;
					default: break;			
 				}
			}
			break;
		case CONT_EXEC:		/****** �������M  *****/
			if(!UDPIsPutReady(MySocket)){					// ���M���f�B�[���H 
				return;		
			}
			else {
				switch(AppMode){		// ���[�h�ɂ�菈���؂�ւ�
					case 0:
						First = 0;						// ��M�I��������
						SW_Flag =0;						// �o�b�t�@���݃t���O�N���A
						Full_Flag = 0;					// �o�b�t�@��t�A��t���O�N���A
						BufIndex = 0;						// �o�b�t�@�C���f�b�N�X�N���A
						ContSM = CONT_LISTEN;				// �X�e�[�g����M�҂���
						break;
					case 1:			// �e�X�g���[�h����
					case 4:			// �܂�Ԃ��e�X�g���[�h����
						UDPPutString((BYTE*)MD);			// �������M
						UDPPut(TempBuffer[1]);
						UDPPut(0);
						UDPFlush();						// UDP���M���s
						ContSM = CONT_LISTEN;				// �X�e�[�g��M�҂���	
						break;
					case 2:			// �����f�[�^��M�i�Đ��j
						if(Full_Flag)	{					// �o�b�t�@��?
							Full_Flag = 0;				// ��t���O�N���A
							UDPPutString((BYTE*)MD);		// ���̃f�[�^����
							UDPPut('O');
							UDPPut(0);
							UDPFlush();					// UDP���M���s
							ContSM = CONT_LISTEN;			// �X�e�[�g����M�҂���
						}
						break;
					case 3:			// �����f�[�^���M(�^���j
						if(BUTTON1_IO == 0){				// �{�^���P�������ꂽ��I��
							First = 0;					// �ŏ��t���O�Z�b�g
							UDPPutString((BYTE*)END);		// END����
							UDPPut(0);
							UDPFlush();					// UDP���M���s
							ContSM = CONT_LISTEN;			// �X�e�[�g��߂�	
						}
						else {		// �����f�[�^���M���s�i�A�����M���s)
							/// �f�[�^���M
							if(Full_Flag){				// �o�b�t�@��t��҂�
								Full_Flag = 0;			// ��t�t���O�N���A
								if(SW_Flag){				// �ǂ��炩�̃o�b�t�@���M
									LED3_IO = 1;			// �ڈ�
									UDPPutArray(BufferB, MaxBuf);
								}
								else{
									LED3_IO = 0;			// �ڈ�
									UDPPutArray(BufferA, MaxBuf);
								}
								UDPPut(0);				// ���M���s
								UDPFlush();
							}
						}
						break;
					default: break;
				}
			}
		default: break;
	}			
}

/*******************************************************************
*  �n�[�h�E�F�A�������֐�
*
********************************************************************/
static void InitializeBoard(void)
{	
	// LEDs
	LED0_TRIS = 0;
	LED1_TRIS = 0;
	LED2_TRIS = 0;
	LED3_TRIS = 0;
	LED0_IO = 0;
	LED1_IO = 0;
	LED2_IO = 0;
	LED3_IO = 0;
	// Enable 4x/5x PLL on PIC18F87J10, PIC18F97J60, etc.
    OSCTUNE = 0x40;
    // Enable internal PORTB pull-ups
    INTCON2bits.RBPU = 0;

	// �^�C�}�P�̏������֐�
    TMR1H = 0xFB;  		  	// Initialize the time for 8kHz
    TMR1L = 0xFA;				// 8kHz=125usec
	IPR1bits.TMR1IP = 1;		// High priority
    	PIR1bits.TMR1IF = 0;
    	PIE1bits.TMR1IE = 1;		// Enable interrupt
    T1CON = 0x81;   			 // On, 16-bit, internal, 1:1
	
	// ECCP1���W���[���̏������֐�
	RCS1_TRIS = 0;			// Output Mode
	TMR2 = 0;				// Timer2 initialize
	T2CON = 0x04;				// postscale 1:1 prescale 1:1 enable
	PR2 = 0xFF;				// 41kHz 10bit resolution
	// ECCP1 initialize Duty low 2bits always 0
	T3CONbits.T3CCP2 = 0;		// select timer2 for timebase
	T3CONbits.T3CCP1 = 0;
	CCP1CON = 0x0F;			// single out on P1A, Active High
	CCPR1L = 0x80;			// Duty always 50%

	// A/D�R���o�[�^�̏�����
	ADCON1 = 0x0B;			// AN0 to AN3 VSS-VDD
	ADCON2 = 0x22;			// Right, 8TAD, FOSC/32
	ADCON0 = 0x0B;			// Select AN2 StartA/D

	// Enable Interrupts
	RCONbits.IPEN = 1;		// Enable interrupt priorities
    INTCONbits.GIEH = 1;
    INTCONbits.GIEL = 1;
}

/*******************************************************************
* �A�v���P�[�V�����̏������֐�
*
********************************************************************/
// MAC�A�h���X�̏����ݒ�
static ROM BYTE SerializedMACAddress[6] = {MY_DEFAULT_MAC_BYTE1, MY_DEFAULT_MAC_BYTE2, 
		MY_DEFAULT_MAC_BYTE3, MY_DEFAULT_MAC_BYTE4, MY_DEFAULT_MAC_BYTE5, MY_DEFAULT_MAC_BYTE6};
// IP�A�h���X���̑��̏����ݒ�
static void InitAppConfig(void)
{
	AppConfig.Flags.bIsDHCPEnabled = TRUE;
	AppConfig.Flags.bInConfigMode = TRUE;
	memcpypgm2ram((void*)&AppConfig.MyMACAddr, (ROM void*)SerializedMACAddress, 
				sizeof(AppConfig.MyMACAddr));
	AppConfig.MyIPAddr.Val = MY_DEFAULT_IP_ADDR_BYTE1 | MY_DEFAULT_IP_ADDR_BYTE2<<8ul 
					| MY_DEFAULT_IP_ADDR_BYTE3<<16ul | MY_DEFAULT_IP_ADDR_BYTE4<<24ul;
	AppConfig.DefaultIPAddr.Val = AppConfig.MyIPAddr.Val;
	AppConfig.MyMask.Val = MY_DEFAULT_MASK_BYTE1 | MY_DEFAULT_MASK_BYTE2<<8ul 
					| MY_DEFAULT_MASK_BYTE3<<16ul | MY_DEFAULT_MASK_BYTE4<<24ul;
	AppConfig.DefaultMask.Val = AppConfig.MyMask.Val;
	AppConfig.MyGateway.Val = MY_DEFAULT_GATE_BYTE1 | MY_DEFAULT_GATE_BYTE2<<8ul 
					| MY_DEFAULT_GATE_BYTE3<<16ul | MY_DEFAULT_GATE_BYTE4<<24ul;
	AppConfig.PrimaryDNSServer.Val = MY_DEFAULT_PRIMARY_DNS_BYTE1 | MY_DEFAULT_PRIMARY_DNS_BYTE2<<8ul  
					| MY_DEFAULT_PRIMARY_DNS_BYTE3<<16ul  | MY_DEFAULT_PRIMARY_DNS_BYTE4<<24ul;
	AppConfig.SecondaryDNSServer.Val = MY_DEFAULT_SECONDARY_DNS_BYTE1 | MY_DEFAULT_SECONDARY_DNS_BYTE2<<8ul  
					| MY_DEFAULT_SECONDARY_DNS_BYTE3<<16ul  | MY_DEFAULT_SECONDARY_DNS_BYTE4<<24ul;

	// Load the default NetBIOS Host Name
	memcpypgm2ram(AppConfig.NetBIOSName, (ROM void*)MY_DEFAULT_HOST_NAME, 16);
	FormatNetBIOSName(AppConfig.NetBIOSName);
}












