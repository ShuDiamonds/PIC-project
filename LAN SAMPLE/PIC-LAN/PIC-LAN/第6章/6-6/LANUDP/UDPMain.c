/**************************************************
*  UDP�f���A�v���P�[�V����
*�@ICMP + ARP + DHCP Client + NBNS + UDP�A�v��
*  �v���W�F�N�g���FLANUDP
**************************************************/
// �錾�ƃw�b�_�t�@�C���̃C���N���[�h
#include "TCPIP Stack/TCPIP.h"

// �X�^�b�N���Ŏg���ϐ��Ń��C���Œ�`���Ă���
APP_CONFIG AppConfig;
BYTE myDHCPBindCount = 0xFF;
#if !defined(STACK_USE_DHCP_CLIENT)
	#define DHCPBindCount	(1)
#endif

// �֐��v���g�^�C�s���O
static void InitAppConfig(void);
static void InitializeBoard(void);
static void DisplayIPValue(IP_ADDR IPVal);
void UDPControlTask(void);

/**********************************************
* ���荞�ݏ���
***********************************************/
// ��ʊ��荞�݁i�C���^�[�o���^�C�}�j
#pragma interruptlow LowISR
void LowISR(void)
{
    TickUpdate();
}
#pragma code lowVector=0x18
void LowVector(void){_asm goto LowISR _endasm}

// ���ʊ��荞�݁i���g�p�j
//	#pragma code highVector=0x8
//	void HighVector(void){_asm goto HighISR _endasm}
#pragma code 		// Return to default code section

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
		UDPControlTask();	
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
*  UDP�ėpI/O������
*�@�ELED�̃I���I�t����Ə�ԕԑ�
*  �E�X�C�b�`�̏�ԕԑ�
*  �E�v���l�̕ԑ�
*  �ELCD�ւ̕����\�� �ELCD�̏���
**********************************************/
#define REMOTE_PORT	10001					// UDP�̃|�[�g�ԍ��̎w��
#define LOCAL_PORT	10002
const BYTE MD[] = "MDOK";						// �ԑ����b�Z�[�W
const BYTE ME[] = "MEOK";

void UDPControlTask(void)
{
	/// ���[�J���ϐ��̒�`
	static enum {							// �X�e�[�g�}�V���̒�`	
		CONT_IDLE = 0,
		CONT_LISTEN,
		CONT_EXEC
	} ContSM = CONT_IDLE;	
	static UDP_SOCKET		MySocket;			// ��M�\�P�b�g���ێ�����	
	static char ANString[8];					// �A�i���O���͕ϊ��o�b�t�@
	unsigned int RcvLen;	
	static BYTE	buffer[40];					// ��M�f�[�^�i�[�o�b�t�@

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
			if(!UDPIsGetReady(MySocket))					// ��M�L���`�F�b�N
				return;									// ��M�Ȃ������^�[��
			RcvLen = UDPGetArray(buffer, sizeof(buffer));	// ��M�f�[�^�擾
			UDPDiscard();									// UDP�\�P�b�g�؂藣��
    			ContSM++;									// �X�e�[�^�X�X�V�ԑ���
			/// ��M�f�[�^ �L�[���[�h�`�F�b�N
			if(!(buffer[0] == 'S')) {						// �J�n�L�[
				ContSM = CONT_LISTEN;						// �L�[���[�h�s��v
				return;									// Listen�ɖ߂�
			}			
			else{		// �L�[���[�h��v�@�f�[�^�����J�n
				switch(buffer[1])	{						// �R�}���h���o��
					case 'C':							// LED����R�}���h
						switch(buffer[2])	{
							case '1':					// LED2�̏ꍇ
								LED2_IO = buffer[3] - '0';	// �I���I�t����
								break;
							case '2':					// LED3�̏ꍇ
								LED3_IO = buffer[3] - '0';	// �I���I�t����
								break;
							default : break;
						}
					case 'A':							// �v���R�}���h
						ADCON1 = 0b00001011;				// A/D�ݒ�
						ADCON2 = 0b10110110;
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
							memcpy((void *)LCDText, (void *)&buffer[2], 32);
						}
						LCDUpdate();						// LCD�ɕ\��
						break;
					case 'E':							// LCD�����R�}���h
						LCDErase();						// LCD�S�������s
						break;
					default: break;			
 				}
			}
			break;
		case CONT_EXEC:		/****** ���M  *****/
			/// ������s
			if(!UDPIsPutReady(MySocket)){					// ���M���f�B�[���H 
				return;									// �܂����^�[��
			}
			else {				// �܂�Ԃ��ԑ�
				switch(buffer[1])	{						// �R�}���h���
					case 'C':							// LED����̂Ƃ�L
						UDPPut('M');						// Key
						UDPPut('L');						// LED
						UDPPut(LED2_IO ? '1':'0');			// LED�̏�ԕԑ�
						UDPPut(LED3_IO ? '1':'0');	
						UDPPut('E');						
						break;
					case 'B':							// �X�C�b�`��ԕԑ�
						UDPPut('M');						// Key
						UDPPut('K');						// Button
						UDPPut(BUTTON0_IO ? '1':'0');
						UDPPut(BUTTON1_IO ? '1':'0');						
						UDPPut(BUTTON2_IO ? '1':'0');
						UDPPut(BUTTON3_IO ? '1':'0');
						UDPPut('E');						
						break;
					case 'A':							// �v���f�[�^�ԑ�
						UDPPut('M');						// Key
						UDPPut('A');						// Analog
						UDPPutString(ANString);
						UDPPut(0);
						UDPPut('E');
						break;
					case 'D':							// LCD�\��OK�ԑ�
						UDPPutString((BYTE*)MD);
						UDPPut(0);
						break;
					case 'E':
						UDPPutString((BYTE*)ME);			// LCD����OK�ԑ�
						UDPPut(0);
						break;
					default:	break;						// �R�}���h�G���[�̂Ƃ�
				}
				UDPFlush();								// UDP���M���s
				ContSM = CONT_LISTEN;						// �X�e�[�g��߂�	
			}											// case���ƂɎ��s
			break;
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
