/**************************************************
*  �r�f�I�J���������R���v���O����
*�@ICMP + ARP + NBNS + DHCP + UDP + TCP
*  �v���W�F�N�g���FLANServo
**************************************************/
// �錾�ƃw�b�_�t�@�C���̃C���N���[�h
#include "TCPIP Stack/TCPIP.h"

// �X�^�b�N���Ŏg���ϐ��Ń��C���Œ�`���Ă���
APP_CONFIG AppConfig;
BYTE myDHCPBindCount = 0xFF;
#if !defined(STACK_USE_DHCP_CLIENT)
	#define DHCPBindCount	(1)
#endif

static int Width1, Width2;			// ECCP�̃p���X��

// �֐��v���g�^�C�s���O
static void InitAppConfig(void);
static void InitializeBoard(void);
static void DisplayIPValue(IP_ADDR IPVal);
void TCPControlTask(void);
void RCServo(void);

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
#pragma code 		// Return to default code section
// ���ʊ��荞��(�^�C�}�R�j 20mmsec�����Ŋ��荞��
#pragma interrupt HighISR
void HighISR(void){
	RCServo();
}
#pragma code highVector=0x8
void HighVector(void){_asm goto HighISR _endasm}
#pragma code 		// Return to default code section

/************************************************
*  �^�C�}3���荞�ݏ����A
*  RC�T�[�{����o�́@20msec�����̃p���X����
************************************************/
void RCServo(void){
	if(PIR2bits.TMR3IF) {				// �^�C�}3�̊��荞�݊m�F
    		TMR3H = 0x9A;					// 20msec�ɍĐݒ�
    		TMR3L = 0x46;		
  		T1CONbits.TMR1ON = 0;			// �^�C�}�P�͒�~�iPWM�����p�j
	  	TMR1H = 0;					// �^�C�}�P�N���A
    		TMR1L = 0;
		// �`���l���P�@ECCP1�p���X���ݒ� 1bit = 0.77usec
		// 0.9msec�`2.1msec�͈̔�
		if(Width1 > 2730)				// Max����
			Width1 = 2730;
		if(Width1 < 1170)				// Min����
			Width1 = 1170;
		CCPR1H = (char)(Width1 >> 8);	// �p���X���Ƃ��Đݒ�
		CCPR1L = (char)Width1;
		// �`���l���Q�@ECCP2�p���X���ݒ�
		if(Width2 > 2730)				// Max����
			Width2 = 2730;
		if(Width2 < 1170)				// Min����
			Width2 = 1170;
		CCPR2H = (char)(Width2 >> 8);	// �p���X���Ƃ��Đݒ�
		CCPR2L = (char)Width2;
		// �����V���b�g�p���X���[�h�œ���J�n
		CCP1CON = 0x08;
		CCP2CON = 0x08;
		T1CONbits.TMR1ON = 1;			// �^�C�}�P�ăX�^�[�g		
		PIR2bits.TMR3IF = 0;			// �^�C�}3���荞�݃t���O�N���A
	}
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
    while(1)   {
		// LED0�̓_�Łi1�b�Ԋu�j
		if(TickGet() - t >= TICK_SECOND/2ul){
            	t = TickGet();
            	LED0_IO ^= 1;						// LED���]
        	}
		// �X�^�b�N�̑���M���s�^�X�N�i��莞�ԓ��Ɏ��s�K�{�j
        	StackTask();
		// NBNS�ɂ�閼�O����	
		NBNSTask();
        	// TCP����M�f��
		TCPControlTask();
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
	for(i = 0; i < sizeof(IP_ADDR); i++){
	    uitoa((WORD)IPVal.v[i], IPDigit);		// IP1���̕����ϊ�
		for(j = 0; j < strlen((char*)IPDigit); j++)	{
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
*  TCP�ėpI/O������
*�@�E�O���o�͐���Ə�ԕԑ�
*  �E���x�̌v��
*  �ELCD�ւ̕����\��
*  �ELCD�̏���
**********************************************/
/// TCP�|�[�g�ԍ��̎w��
#define LOCAL_PORT	50001
const BYTE MD[] = "MDOK";						// �ԑ����b�Z�[�W
const BYTE ME[] = "MEOK";

void TCPControlTask(void)
{
	//// �X�e�[�g�}�V���̒�`	
	static enum {
		SM_IDLE = 0,				// �������
		SM_LISTEN_WAIT,			// �ڑ��҂�
		SM_CONNECTED,				// �ڑ���
		SM_EXEC,					// ����M���s��
		SM_SEND_WAIT,				// ���M�҂���
		SM_DISCONNECT				// �ڑ��؂藣���v��
	} ContSM = SM_IDLE;
	/***** ���[�J���ϐ��̒�`  ****/
	static TCP_SOCKET	 MySocket = INVALID_SOCKET;			// ��M�\�P�b�g���ێ�����	
	static char ANString[8];								// �A�i���O���͕ϊ��o�b�t�@
	static unsigned int  RcvLen;
	static BYTE	buffer[128];								// ��M�f�[�^�i�[�o�b�t�@

	/**** �֐����s���@�X�e�[�g�}�V���ɂ��i�s *****/
	switch(ContSM)
	{
		case SM_IDLE:		/****  �A�C�h������Listen�� ****/
			MySocket = TCPListen(LOCAL_PORT);				// TCP�T�[�o�Ƃ���Listen��Ԃɂ���
			if(MySocket == INVALID_SOCKET)
				return;									// �I�[�v�����s�Ȃ�J��Ԃ�
			else
				ContSM = SM_LISTEN_WAIT;					// LISTEN�ֈڍs
			break;
		case SM_LISTEN_WAIT:	/***** LISTEN �ڑ��҂� *******/
			LED1_IO = 1;									// LED1�I��
			if(TCPIsConnected(MySocket))					// 
				ContSM = SM_CONNECTED;						// ��M�҂��ֈڍs
			break;;
		case SM_CONNECTED:	/***** Establish ��M ****/
			LED1_IO = 0;									// LED1�I�t
			if(!TCPIsConnected(MySocket))					// �؂藣���ꂽ�ꍇ
				ContSM = SM_LISTEN_WAIT;					// LISTEN WAIT�ɖ߂�	
			if(!TCPIsGetReady(MySocket))					// ��M�f�[�^���肩�H
				return;									// ������Α����^�[��
			RcvLen =TCPGetArray(MySocket, buffer, sizeof(buffer));// ��M�f�[�^�擾
			TCPDiscard(MySocket);							// ��M�\�P�b�g�J��
    			ContSM = SM_EXEC;								// ���M�ֈڍs
			/// ��M�f�[�^ �L�[���[�h�`�F�b�N
			if(!(buffer[0] == 'S')) {						// �J�n�L�[
				ContSM = SM_CONNECTED;						// �L�[���[�h�s��v
				break;;									// Listen�ɖ߂�
			}			
			else{		/***** �L�[���[�h��v�@�f�[�^�����J�n *****/
				switch(buffer[1])	{						// �R�}���h���o��
					case 'C':							// LED����R�}���h
						switch(buffer[2])	{
							case '1':					// �ėp�o��1�̏ꍇ
								UIO1_IO = buffer[3] - '0';	// �I���I�t����
								break;
							case '2':					// �ėp�o��2�̏ꍇ
								UIO2_IO = buffer[3] - '0'; // �I���I�t����
								break;
							default :
								break;
						}
					case 'A':							// �v���R�}���h
						ADCON0 = 0;
						ADCON1 = 0b00001011;				// A/D�ݒ�
						ADCON2 = 0b10111110;
						TRISA = 0x2C;
						if(buffer[2] == '1')				// �`���l������
							ADCON0 = 0b00001001;			// Channel 2
						else
							ADCON0 = 0b00001101;			// Channel 3
    						ADCON0bits.GO = 1;				// A/D�ϊ��J�n	
    						while(ADCON0bits.GO);				// A/D�ϊ��I���҂�
    						ltoa(*((WORD*)(&ADRESL)), ANString);// ASCII�ɕϊ�
						break;
					case 'D':							// LCD�\���R�}���h
						if(RcvLen < 36){					// 32�����Ő���
							memset(LCDText, ' ', 32);
							memcpy((void *)LCDText, (void *)&buffer[2], RcvLen-2);
						}
						else{
							memset(LCDText, ' ', 32);
							memcpy((void *)LCDText, (void *)&buffer[2], 32);
						}
						LCDUpdate();						// LCD�ɕ\��
						break;
					case 'E':							// LCD�����R�}���h
						LCDErase();						// LCD�S�������s
						break;
					case 'R':							// RC�T�[�{�̐���
						switch(buffer[2]) {				
							case '1':					// �`���l��1��
								Width1 = atol(&buffer[3]);	// �p���X���Ƃ��ĕۑ�
								break;
							case '2':					// �`���l��2��
								Width2 = atol(&buffer[3]);	// �p���X���Ƃ��ĕۑ�
								break;
							default : break;
						}
					default: break;			
 				}
			}
			break;
		case SM_EXEC:		/***** Establish ���M ****/
			if(!TCPIsPutReady(MySocket))					// �ڑ����킩�H 
				return;
			else {
				// �܂�Ԃ��ԑ�
				switch(buffer[1])	{						// �R�}���h���
					case 'C':							// LED����̂Ƃ�
						TCPPut(MySocket,'M');				// Key
						TCPPut(MySocket,'L');				// �ėp�o��
						TCPPut(MySocket,UIO1_IO ? '1':'0');	// �ėp�o�͂̏�ԕԑ�							
						TCPPut(MySocket,UIO2_IO ? '1':'0');	
						TCPPut(MySocket,'E');						
						break;
					case 'B':							// �X�C�b�`��ԕԑ�
						TCPPut(MySocket,'M');				// Key
						TCPPut(MySocket,'K');				// Button
						TCPPut(MySocket,BUTTON0_IO ? '1':'0');
						TCPPut(MySocket,BUTTON1_IO ? '1':'0');						
						TCPPut(MySocket,BUTTON2_IO ? '1':'0');
						TCPPut(MySocket,BUTTON3_IO ? '1':'0');
						TCPPut(MySocket,'E');						
						break;
					case 'A':							// �v���f�[�^�ԑ�
						TCPPut(MySocket,'M');				// Key
						TCPPut(MySocket,'A');				// Analog
						TCPPutString(MySocket, ANString);
						TCPPut(MySocket, 0);
						TCPPut(MySocket,'E');
						break;
					case 'D':							// LCD�\��OK�ԑ�
						TCPPutString(MySocket, (BYTE*)MD);
						TCPPut(MySocket, 0);
						break;
					case 'E':
						TCPPutString(MySocket,(BYTE*)ME);	// LCD����OK�ԑ�
						TCPPut(MySocket, 0);
						break;
					default:		break;					// �R�}���h�G���[�̂Ƃ�
				}
			}											// �ԑ����s
			TCPFlush(MySocket);							// ���M�v��
			ContSM = SM_SEND_WAIT;							// ���M�����҂��ֈڍs
			break;
		case SM_SEND_WAIT:								// ���M�����҂�
			TCPDiscard(MySocket);							// ���M�\�P�b�g�����[�X
			ContSM = SM_CONNECTED;							// ��M�ֈڍs
			break;;
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
	UIO1_TRIS = 0;
	UIO2_TRIS = 0;
	UIO1_IO = 0;
	UIO2_IO = 0;

	// Enable 4x/5x PLL on PIC18F87J10, PIC18F97J60, etc.
    OSCTUNE = 0x40;
	// Set up analog features of PORTA
	ADCON0 = 0x0D;			// ADON Channel 3
	ADCON1 = 0x0B;			// Vdd/Vss AN0 to AN3 are analog
	ADCON2 = 0xBE;			// Right justify, 20TAD ACQ time, Fosc/64 (~21.0kHz)
    // Enable internal PORTB pull-ups
    INTCON2bits.RBPU = 0;
	// Enable Interrupts
	RCONbits.IPEN = 1;		// Enable interrupt priorities
    INTCONbits.GIEH = 1;
    INTCONbits.GIEL = 1;
	// Calibration of ADC
	ADCON0bits.ADCAL = 1;
	ADCON0bits.GO = 1;
	while(ADCON0bits.GO);
	ADCON0bits.ADCAL = 0;

	// �^�C�}�P�̏������֐�
    // Initialize the time for Capture
    TMR1H = 0;
    TMR1L = 0;
	// Disable timer interrupt
    PIE1bits.TMR1IE = 0;		// Dsiable interrupt
    // Timer1 off, 16-bit, internal timer, 1:8 prescalar
	// 1bit = 0.77usec
    T1CON = 0xB0;

	// �^�C�}3�̏������֐� 20msec
    TMR3H = 0x9A;
    TMR3L = 0x46;
	// Set up the timer interrupt
	IPR2bits.TMR3IP = 1;		// Low priority
    	PIR2bits.TMR3IF = 0;
    	PIE2bits.TMR3IE = 1;		// Enable interrupt
    	// Timer3 on, 16-bit, internal timer, 1:8 prescalar
	// all ECCP timebase is Timer1 and Timer2
    	T3CON = 0xB1;

	//ECCP���W���[���̏������֐�
	// IO
	RCS1_TRIS = 0;			// Output Mode
	RCS2_TRIS = 0;
	// ECCP1 initialize Duty low 2bits always 0
	T3CONbits.T3CCP2 = 0;	// select timer1 for timebase
	T3CONbits.T3CCP1 = 0;
	// Stop Timer1
	T1CONbits.TMR1ON = 0;
	// Setup Compare Mode
	CCPR1H = 0;
	CCPR1L = 0;
	CCP1CON = 0x08;			// Compare mode Initial Low
	CCPR2H = 0;
	CCPR2L = 0;
	CCP2CON = 0x08; 
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
	Width1 = 1950;
	Width2 = 1950;
}
