/*********************************************************************
 * �C���^�[�l�b�g���W�I�@�A�v���P�[�V����
 * �v���W�F�N�g���FLANRadio
 *********************************************************************/
#define THIS_IS_STACK_APPLICATION

// This header includes all headers for any enabled TCPIP Stack functions
#include "TCPIP Stack/TCPIP.h"
#include "SPIRAM2.h"
#include "MP3Client.h"
#include "glcd_lib2.h"				// �O���t�B�b�NLCD���C�u����

// �R���t�B�M�����[�V�����ݒ�(HardwareProfile.h�Ɋ܂܂�Ȃ�����)
#pragma config XINST=OFF
#pragma config WDT=OFF, FOSC2=ON, FOSC=HSPLL, ETHLED=ON

// �A�v���P�[�V���������l�ݒ�
APP_CONFIG AppConfig;
BYTE myDHCPBindCount = 0xFF;

#if !defined(STACK_USE_DHCP_CLIENT)
	#define DHCPBindCount	(1)
#endif

// �֐��v���g�^�C�v
static void InitializeBoard(void);
static void DisplayIPValue(IP_ADDR IPVal);
void AppInit(void);

/************************************************
*  ���荞�ݏ���
***********************************************/
	#pragma interruptlow LowISR
	void LowISR(void)
	{
	    TickUpdate();
	}
	
	#pragma interrupt HighISR
	void HighISR(void)
	{
		MP3Timer3Interrupt();
	}
	
	#pragma code lowVector=0x18
	void LowVector(void){_asm goto LowISR _endasm}
	#pragma code highVector=0x8
	void HighVector(void){_asm goto HighISR _endasm}
	#pragma code // Return to default code section

// �\���p�f�[�^�o�b�t�@
#pragma udata asdf
BYTE strStationName[55];
#pragma udata asdf2
BYTE strSongName[55];
#pragma udata asdf3
BYTE lenStationName, lenSongName;
BYTE tPtr, gPtr;
BYTE StationOnce, NewStationName, NewSongName;
BYTE bitrate;

// ���W�I�ǃf�[�^
// To add new stations, simply add them at the end of this array.  
// Be very careful to follow the proper C compiler syntax though 
// (commas after the first three parameters, none for the last 
// string, which is concatentated together to make one string)
// The structure format is:
// 	ROM BYTE *HumanName,// Human name -- unused
// 	ROM BYTE *HostName,	// Host name string of the server (can be an IP address, ex: "192.168.1.1")
// 	WORD port,			// TCP Port the remote Shoutcast server is listening on
//	ROM BYTE *Message	// The message that is send to the remote server to request the MP3 stream

STATION_INFO stations[] = 
{

	/*D I G I T A L L Y - I M P O R T E D - House - silky sexy deep house music direct from New York city! */
	{
		"DIGITALLY - IMPORTED, 96K",
		"scfire-chi-aa03.stream.aol.com",
		80,
		"GET /stream/1007 HTTP/1.0\r\n"
		"Host: scfire-dll-aa02.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	/*D I G I T A L L Y - I M P O R T E D - Lounge - sit back and enjoy the lounge grooves! */
	{
		"DIGITALLY - IMPORTED, 96K",
		"scfire-chi-aa05.stream.aol.com",
		80,
		"GET /stream/1009 HTTP/1.0\r\n"
		"Host: scfire-dll-aa02.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	/*SKY.FM-Top Hits Music - who cares about the chart order, less rap & more hits! */
	{
		"SKY.FM Top Hits, 96K",
		"scfire-dll-aa02.stream.aol.com",
		80,
		"GET /stream/1014 HTTP/1.0\r\n"
		"Host: scfire-dll-aa02.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	/* -=[HOT 108 JAMZ]=- #1 FOR HIP HOP - 128K HD) - 128kbps*/
	{
		"HOT 108 JAMZ, 128K",
		"scfire-dll-aa03.stream.aol.com",
		80,
		"GET /stream/1071 HTTP/1.0\r\n"
		"Host: scfire-dll-aa03.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// .977 The Hitz Channel - 128kbps
	{
		".977 The Hitz, 128K",
		"scfire-dll-aa04.stream.aol.com",
		80,
		"GET /stream/1074 HTTP/1.0\r\n"
		"Host: scfire-dll-aa04.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// 181.fm - POWER 181 -=- #1 For The Hitz! 
	// [Top 40 Pop Rock]
	{
		"POWER 181 Hitz, 128K",
		"scfire-dll-aa04.stream.aol.com",
		80,
		"GET /stream/1023 HTTP/1.0\r\n"
		"Host: scfire-dll-aa04.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// 181.FM - The Beat * #1 For HipHop 
	// [hip hop urban rap rnb]
	{
		"POWER 181 HipHop, 128K",
		"scfire-dll-aa04.stream.aol.com",
		80,
		"GET /stream/1092 HTTP/1.0\r\n"
		"Host: scfire-dll-aa04.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// KCRW World News
	// [News]
	{
		"KCRW World News, 64K",
		"scfire-dll-aa02.stream.aol.com",
		80,
		"GET /stream/1047 HTTP/1.0\r\n"
		"Host: scfire-dll-aa02.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// 90.9 WBUR Talk Radio
	{
		"90.9 WBUR Talk, 24K",
		"wbur-sc.streamguys.com",
		80,
		"GET / HTTP/1.0\r\n"
		"Host: wbur-sc.streamguys.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// S K Y . F M - 80s, 80s, 80s! - Hear your classic favorites right here! (www.sky.fm)
	// [80s Pop Rock Oldies]
	{
		"SKY.FM - 80s, 80s, 80s!, 96K",
		"scfire-nyk-aa03.stream.aol.com",
		80,
		"GET /stream/1013 HTTP/1.0\r\n"
		"Host: scfire-nyk-aa03.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// .977 The 80s Channel
	// [80s Pop Rock]
	{
		".977 The 80s Channel, 128K",
		"scfire-chi-aa03.stream.aol.com",
		80,
		"GET /stream/1040 HTTP/1.0\r\n"
		"Host: scfire-chi-aa03.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// S K Y . F M - Absolutely Smooth Jazz - the world's smoothest jazz 24 hours a day 
	// [Soft Smooth Jazz]
	{
		"SKY.FM - Absolutely Smooth Jazz, 96K",
		"scfire-ntc-aa03.stream.aol.com",
		80,
		"GET /stream/1010 HTTP/1.0\r\n"
		"Host: scfire-ntc-aa03.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// S K Y . F M - Country - today's Hit Country with a mix of your favorites 
	// [Country Hits Favorites]
	{
		"SKY.FM Country, 128K",
		"scfire-ntc-aa02.stream.aol.com",
		80,
		"GET /stream/1019 HTTP/1.0\r\n"
		"Host: scfire-ntc-aa02.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// 181.fm - Kickin' Country (Todays Best Country!)
	// [Country Oldies Top 40]
	{
		"181FM Kickin' Country, 128K",
		"scfire-nyk-aa04.stream.aol.com",
		80,
		"GET /stream/1075 HTTP/1.0\r\n"
		"Host: scfire-nyk-aa04.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// S K Y . F M - Mostly Classical - Listen and Relax 
	{
		"SKY.FM - Mostly Classical, 128K",
		"scfire-ntc-aa04.stream.aol.com",
		80,
		"GET /stream/1006 HTTP/1.0\r\n"
		"Host: scfire-ntc-aa02.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},
	// SMOOTHJAZZ.COM - The Internet's Original Smooth Jazz Radio Station
	{
		"SMOOTHJAZZ.COM, 128K",
		"scfire-ntc-aa02.stream.aol.com",
		80,
		"GET /stream/1005 HTTP/1.0\r\n"
		"Host: scfire-ntc-aa02.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	// Radio Paradise - DJ-mixed modern & classic rock, world
	{
		"Radio Paradise, 128K",
		"scfire-nyk-aa04.stream.aol.com",
		80,
		"GET /stream/1048 HTTP/1.0\r\n"
		"Host: scfire-ntc-aa02.stream.aol.com\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	/* 1.FM - Top 40 */
	{
		"1.FM - Top 40, 128K",
		"38.99.107.34",
		8075,
		"GET / HTTP/1.0\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	/* 1.FM - Otto's Baroque Musick */
	{
		"1.FM - Otto's Baroque Musick, 96K",
		"72.13.81.178",
		8045,
		"GET / HTTP/1.0\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	},

	/* ........::::::: Blue FM ::::::::....... Easy Listening */
	{
		"........::::::: Blue FM ::::::::....... Easy Listening, 128K",
		"222.122.131.94",
		8080,
		"GET / HTTP/1.0\r\n"
		"Accept: */*\r\n"
		"Icy-MetaData:1\r\n"
		"Connection: close\r\n\r\n"
	}
};
#define STATION_COUNT	(sizeof(stations)/sizeof(stations[0]))

BYTE station = 0;
/**** State  *****/
#define	MENU 	0
#define	STATION	1
#define 	BASS	2
#define 	VOLUME	3
#define 	IDLE	4

/*** Message Data for Graphic LCD ***/
rom const char SpMsg[] =  "                  ";
rom const char BgnMsg[] = "**Internet Radio**";
rom const char StMsg[]  = "Station Select    ";
rom const char BasMsg[] = "Bass Boost Up/Down";
rom const char VolMsg[] = "Volume Up/Down    ";
rom const char UpMsg[]  = "Up                ";
rom const char DwnMsg[] = "Down              ";
rom const char ExtMsg[] = "Exit              ";
rom const char PreMsg[] = "Prev Station      ";
rom const char NxtMsg[] = "Next Station      ";
rom const char IPMsg[]  = "IP:               ";
rom const char MnuMsg[] = "Station->Vol->Bass";
rom const char SelMsg[] = "Select Menu!!     ";

/**************************************
* ���C���֐�
**************************************/
void main(void)
{
    static TICK t = 0, tpress = 0, tdisp = 0;
	// �ϐ��̒�`�Ə�����
	BYTE i, j;
	ROM BYTE *cptr;
	BYTE menuState;
	BYTE vol, bass, iPtr, iDir;
	BYTE brate[5];
	BYTE istr[16];

	menuState = MENU;				// �X�e�[�g�ϐ������l
	vol = 0x1F;					// ���ʁABass�����l
	bass = 0;
	lenStationName = 0;			// �\���t���O�N���A
	lenSongName = 0;
	tPtr = 0;
	gPtr = 0;
	StationOnce = 1;				// �\���t���O���Z�b�g
	NewStationName = 0;
	NewSongName = 0;
	iPtr = 32;
	iDir = 0;
	brate[0] = ' ';				// ������
	brate[1] = ' ';
	brate[2] = ' ';
	brate[3] = 0;
	// �A�v���P�[�V�����Ɋ֘A����f�[�^�̃f�t�H���g�ݒ�
	AppInit();	
    // �n�[�h�E�F�A�̏�����
    InitializeBoard();
    // �C���^�[�o���^�C�}0�̏�����
    TickInit();
	// �X�^�b�N�̏����� (MAC, ARP, TCP, UDP)
    StackInit();
    if(!AppConfig.Flags.bIsDHCPEnabled)
    {
        DHCPDisable();
    }
	// LCD�������ƊJ�n���b�Z�[�W�\��
	lcd_Init();
	lcd_Str(3,0,BgnMsg);
	// VLSI VS1011 MP3�f�R�[�_�̏����� 
	VS1011_Init();
//	VS1011_SineTest();						// �e�X�g�����g�o��
	// ���ʂƒቹ�����̏����l�Z�b�g
	SetVolume(0x10, 0x10);
	SetBassBoost(3,15);
	// ���W�I�̏������ƍŏ��̃��W�I�ǂƂ̐ڑ�
	MP3ClientInit();
	MP3OpenStation(&stations[0]);
	// ���j���[�\��
	lcd_Str(6,0,SelMsg);						// �������j���[�\��
	lcd_Str(7,0,MnuMsg);

	/*******************************
	* ���C�����[�v
	*******************************/
    	while(1)  	{
       	// ����ڈ�LED�_��(0.5�b����)
        	if(TickGet() - t >= TICK_SECOND/2ul) {
            t = TickGet();
            LED0_IO ^= 1;
        }
        	// �v���g�R������M���s
        	StackTask();
		// �X�g���[�~���O���s
		MP3ClientTask();
		// 16�b�ŕ\�������j���[�ɖ߂�
        if(TickGetDiff(TickGet(), tpress) >= TICK_SECOND*16) {
			if(menuState != MENU && menuState != IDLE){
				menuState = MENU;					// MENU�X�e�[�g�ɖ߂�
				lcd_Clear(0);
				lcd_Str(6,0,SelMsg);
				lcd_Str(7,0,MnuMsg);
			}
        }
		/***** ���W�I�ǖ��Ɖ��y�Ȗ��\��  ***/
        if(TickGetDiff(TickGet(), tdisp) >= TICK_SECOND) {
			tdisp = TickGet();					// Max54����
			if(NewStationName && StationOnce)
				StationOnce = 0;
			tPtr = 0;							// 1�s�ڎw��
			i = 0; j=0;
			while(i < lenStationName){
				lcd_Char1(tPtr,j++,(int)strStationName[i++]);
				if(j==18)						// 18�����ŉ��s
				{
					tPtr++;						// �s�J�E���^�A�b�v
					j=0;
				}
			}
			NewStationName = 0;
			/******* ���y�Ȗ��\���@ ************/	
			gPtr = 3;							// 4�s�ڎw��
			i=0; j=0;
			while(i < lenSongName){				// Max 54����	
				lcd_Char1(gPtr,j++,(int)strSongName[i++]);
				if(j==18){						// 18�����ŉ��s
					gPtr++;						// �s�J�E���^�A�b�v
					j=0;
				}
			}
			NewSongName = 0;
		}
		/*** �X�C�b�`�P�������ꂽ�Ƃ��̏���  ***/
		{
			static BOOL SwitchBouncing = TRUE;
			static TICK SwitchTime = 0;

			if(SwitchBouncing){					// �`���^�����O���
				if(BUTTON0_IO == 0)
					SwitchTime = TickGet();
				else if(SwitchTime - TickGet() >= TICK_SECOND/7)
					SwitchBouncing = FALSE;
			}
			else if(BUTTON0_IO == 0){				// �X�C�b�`1�̏���
	       		SwitchBouncing = TRUE;
	            tpress = TickGet();

				switch(menuState)	{
					case MENU:
						menuState = STATION;
						lcd_Str(6,0,StMsg);		// �ǑI��
						lcd_Str(7,0,SpMsg);
						break;
					case STATION:
						menuState = VOLUME;
						lcd_Str(6,0,VolMsg);		// ���ʑI��
						lcd_Str(7,0,SpMsg);
						break;
					case VOLUME:
						menuState = BASS;
						lcd_Str(6,0,BasMsg);		// �ቹ�����I��
						lcd_Str(7,0,SpMsg);
						break;
					case BASS:
						menuState = MENU;
						lcd_Clear(0);
						lcd_Str(6,0,SelMsg);		// ���j���[�\��
						lcd_Str(7,0,MnuMsg);
						break;
					case IDLE:
						menuState = MENU;			// IP�A�h���X�����\��
						lcd_Clear(0);
						DisplayIPValue(AppConfig.MyIPAddr); 
						lcd_Str(6,0,SelMsg);
						lcd_Str(7,0,MnuMsg);
						break;
					default:
						break;
				}
			}
		}
		/****** �X�C�b�`�Q�������ꂽ�Ƃ��̏��� *****/
		{
			static BOOL SwitchBouncing = TRUE;
			static TICK SwitchTime = 0;

			if(SwitchBouncing){						// �`���^�����O���
				if(BUTTON1_IO == 0)
					SwitchTime = TickGet();
				else if(SwitchTime - TickGet() >= TICK_SECOND/7)
					SwitchBouncing = FALSE;
			}
			else if(BUTTON1_IO == 0) {					// �X�C�b�`�Q�̏���
	       		SwitchBouncing = TRUE;
	            	tpress = TickGet();
				switch(menuState)	{
					case MENU:
						lcd_Str(6,0,SelMsg);			// ���j���[�I��\��
						lcd_Str(7,0,MnuMsg);
						break;
					case STATION:					// ���̋ǑI��
						// increment station #
						// if less than 0, reset to 3
						if(++station >= STATION_COUNT)	// ���̋ǃf�[�^�擾
							station = 0;
						MP3OpenStation(&stations[station]);	// �ǂƐڑ�
						strStationName[0] = ' ';		
						strStationName[1] = 0;			// �f�[�^������
						lenStationName = 1;
						strSongName[0] = ' ';
						strSongName[1] = 0;
						lenSongName = 1;
						StationOnce = 1;
						lcd_Clear(0);
						lcd_Str(6,0, StMsg);			// �ǑI��\��
						lcd_Str(7,0, NxtMsg);			// Next�\��
						break;
					case VOLUME:
						if(vol >= 3)					// ���ʃA�b�v����
							vol -= 3;
						// send command to increment volume
						SetVolume(vol,vol);			// ���ʐ���
						lcd_Str(6,0,VolMsg);
						lcd_Str(7,0,UpMsg);			// Up�\��
						break;
					case BASS:
						lcd_Str(6,0,BasMsg);			// �ቹ�A�b�v����
						lcd_Str(7,0,UpMsg);
						if(bass < 15)
							bass++;
						// send command to increment bass
						SetBassBoost(bass,15);			// �ቹ��������
						break;
					case IDLE:
						lcd_Clear(0);					// IP�A�h���X�����\��
						DisplayIPValue(AppConfig.MyIPAddr);
						lcd_Str(6,0,SelMsg); 
						lcd_Str(7,0,MnuMsg);
						break;
					default:
						break;
				}
			}
		}
		/****** �X�C�b�`�R�������ꂽ�Ƃ��̏��� ****/
		{
			static BOOL SwitchBouncing = TRUE;
			static TICK SwitchTime = 0;

			if(SwitchBouncing){						// �`���^�����O���
				if(BUTTON2_IO == 0)
					SwitchTime = TickGet();
				else if(SwitchTime - TickGet() >= TICK_SECOND/7)
					SwitchBouncing = FALSE;
			}
			else if(BUTTON2_IO == 0){					// �X�C�b�`�R�̏���
	       		SwitchBouncing = TRUE;
	            tpress = TickGet();

				switch(menuState)	{
					case MENU:
						lcd_Str(6,0,SelMsg);			// ���j���[�\��
						lcd_Str(7,0,MnuMsg);
						break;
					case STATION:
						// decrement station #
						// if less than 0, reset to 3	// �O�̋ǑI��
						if(station == 0)
							station = STATION_COUNT-1;	// �O�̋ǃf�[�^�擾
						else
							station--;
						MP3OpenStation(&stations[station]);	// �ڑ�
						lcd_Clear(0);
						lcd_Str(6,0,StMsg);
						lcd_Str(7,0,PreMsg);			// ������
						strStationName[0] = ' ';
						strStationName[1] = 0;
						lenStationName = 1;
						strSongName[0] = ' ';
						strSongName[1] = 0;
						lenSongName = 1;
						StationOnce = 1;
						break;
					case VOLUME:
						if(vol < 240)					// ���ʃ_�E������
							vol += 3;
						// send command to increment volume
						SetVolume(vol,vol);			// ���ʐ���
						lcd_Str(6,0,VolMsg);
						lcd_Str(7,0,DwnMsg);			// Down�\��
						break;				
					case BASS:
						if(bass > 0)					// �ቹ�_�E������
							bass--;
						// send command to decrement bass
						SetBassBoost(bass,15);			// �ቹ��������
						lcd_Str(6,0,BasMsg);
						lcd_Str(7,0,DwnMsg);			// Down�\��
						break;
					case IDLE:
						lcd_Clear(0);					// IP�A�h���X�����\��
						DisplayIPValue(AppConfig.MyIPAddr);
						lcd_Str(6,0,SelMsg); 
						lcd_Str(7,0,MnuMsg);
						break;
					default:
						break;
				}
			}
		}
        // DHCP�̂Ƃ��ŏ���1�񂾂�IP�A�h���X�\��
        if(DHCPBindCount != myDHCPBindCount)
        {
            myDHCPBindCount = DHCPBindCount;

			if(menuState != IDLE)
				DisplayIPValue(AppConfig.MyIPAddr);		// IP�A�h���X�\�� 
        }
    }
}

/**************************
*  IP�A�h���X�\���֐�
 **************************/
static void DisplayIPValue(IP_ADDR IPVal)
{
    BYTE IPDigit[4];
    BYTE i,j;
    BYTE pos;
 
 	pos = 3;
 	// Erase the old IP address stored here
	lcd_Str(0,0,IPMsg);
	lcd_Char1(0,0,(int)'I');
	lcd_Char1(0,1,(int)'P');
	
    for(i = 0; i < sizeof(IP_ADDR); i++)
    {
        uitoa((WORD)IPVal.v[i], IPDigit);
 
		if(i != 0) 
		{
			lcd_Char1(0,pos,(int)'.');
        	pos += 1;
        }

        for(j = 0; j < strlen((char*)IPDigit); j+=1)
        {
			lcd_Char1(0,pos,(int)IPDigit[j]);
			pos += 1;
        }
	}
}

/*********************************************************
 * �n�[�h�E�F�A�������֐�
 *********************************************************/
static void InitializeBoard(void)
{
	BYTE i;
	
	// LEDs
	LED0_TRIS = 0;
	LED0_IO = 0;

	// Enable 4x/5x PLL on PIC18F87J10, PIC18F97J60, etc.
    OSCTUNE = 0x40;
	
	// Set up analog features of PORTA
	ADCON0 = 0x01;		// ADON, Channel 0
	ADCON1 = 0x0E;		// Vdd/Vss is +/-REF, AN0 is analog
	ADCON2 = 0xBE;			// Right justify, 20TAD ACQ time, Fosc/64 (~21.0kHz)

    // Enable internal PORTB pull-ups
    INTCON2bits.RBPU = 0;

	// Enable Interrupts
	RCONbits.IPEN = 1;		// Enable interrupt priorities
    INTCONbits.GIEH = 1;
    INTCONbits.GIEL = 1;

    // Do a calibration A/D conversion
	ADCON0bits.ADCAL = 1;
    ADCON0bits.GO = 1;
	while(ADCON0bits.GO);
	ADCON0bits.ADCAL = 0;

#if defined(SPIRAM_CS_TRIS)
	SPIRAMInit();
#endif
#if defined(SPIRAM2_CS_TRIS)
	SPIRAM2Init();
#endif
#if defined(SPIFLASH_CS_TRIS)
	SPIFlashInit();
#endif
}

/*********************************************************************
* �A�v���P�[�V�����p�f�t�H���g�l�ݒ�
 ********************************************************************/
void AppInit(void)
{
	// Load up the AppConfig defaults
	AppConfig.Flags.bIsDHCPEnabled = TRUE;
	AppConfig.Flags.bInConfigMode = TRUE;
	AppConfig.MyMACAddr.v[0] = MY_DEFAULT_MAC_BYTE1;
	AppConfig.MyMACAddr.v[1] = MY_DEFAULT_MAC_BYTE2;
	AppConfig.MyMACAddr.v[2] = MY_DEFAULT_MAC_BYTE3;
	AppConfig.MyMACAddr.v[3] = MY_DEFAULT_MAC_BYTE4;
	AppConfig.MyMACAddr.v[4] = MY_DEFAULT_MAC_BYTE5;
	AppConfig.MyMACAddr.v[5] = MY_DEFAULT_MAC_BYTE6;
	AppConfig.MyIPAddr.Val = MY_DEFAULT_IP_ADDR_BYTE1 | MY_DEFAULT_IP_ADDR_BYTE2<<8ul | MY_DEFAULT_IP_ADDR_BYTE3<<16ul | MY_DEFAULT_IP_ADDR_BYTE4<<24ul;
	AppConfig.MyMask.Val = MY_DEFAULT_MASK_BYTE1 | MY_DEFAULT_MASK_BYTE2<<8ul | MY_DEFAULT_MASK_BYTE3<<16ul | MY_DEFAULT_MASK_BYTE4<<24ul;
	AppConfig.DefaultIPAddr.Val = AppConfig.MyIPAddr.Val;
	AppConfig.DefaultMask.Val = AppConfig.MyMask.Val;
	AppConfig.MyGateway.Val = MY_DEFAULT_GATE_BYTE1 | MY_DEFAULT_GATE_BYTE2<<8ul | MY_DEFAULT_GATE_BYTE3<<16ul | MY_DEFAULT_GATE_BYTE4<<24ul;
	AppConfig.PrimaryDNSServer.Val = MY_DEFAULT_PRIMARY_DNS_BYTE1 | MY_DEFAULT_PRIMARY_DNS_BYTE2<<8ul  | MY_DEFAULT_PRIMARY_DNS_BYTE3<<16ul  | MY_DEFAULT_PRIMARY_DNS_BYTE4<<24ul;
	AppConfig.SecondaryDNSServer.Val = MY_DEFAULT_SECONDARY_DNS_BYTE1 | MY_DEFAULT_SECONDARY_DNS_BYTE2<<8ul  | MY_DEFAULT_SECONDARY_DNS_BYTE3<<16ul  | MY_DEFAULT_SECONDARY_DNS_BYTE4<<24ul;
	memcpypgm2ram(AppConfig.NetBIOSName, (ROM void*)MY_DEFAULT_HOST_NAME, 16);
	FormatNetBIOSName(AppConfig.NetBIOSName);
}

/*********************************************************************
* Function:		void NewServerTitleProc(BYTE *strServerTitle)
*
* PreCondition:	None
*
* Input:		*ServerTitle: Pointer to a null terminated string 
*							  containing the server name/title
*
* Output:		None
*
* Side Effects:	None
*
* Overview:		This is a callback function.  The MP3ClientTask() 
*				automatically calls this function when the server 
*				sends the icy-name header (just before playing any 
*				data).
*
* Note:			The string is volatile, so you must copy it if you 
*				wish to use it later after returning from this 
*				callback.
********************************************************************/
void NewServerTitleProc(BYTE *strServerTitle){
	BYTE i;
	
	if(!StationOnce)
		return;
	for(i = 0; i < 54u; i++)	{			// 3�s54�����܂ŕ\��
		strStationName[i] = *strServerTitle;
		strServerTitle++;
	}
	for(i = 0; i < 54u; i++)	{
		if(!strStationName[i])	{
			if(i < 54u)	{
				if(strStationName[i+1] == ';')
					break;
				strStationName[i] = ' ';
			}
		}
		if(strStationName[i] == ';'){
			strStationName[i] = 0;
			break;
		}
	}
	if(i < 54u)
		lenStationName = i;
	else{
		lenStationName = 54;
		strStationName[54] = 0;
	}
	tPtr = 0;
	NewStationName = 1;
}


/*********************************************************************
* Function:		void NewStreamTitleProc(BYTE *strStreamTitle)
*
* PreCondition:	None
*
* Input:		*strStreamTitle: Pointer to a null terminated string 
*								 containing the stream/song title
*
* Output:		None
*
* Side Effects:	None
*
* Overview:		This is a callback function.  The MP3ClientTask() 
*				automatically calls this function when the server 
*				sends the StreamTitle meta data (typically shortly 
*				after connection startup and whenever the song 
*				changes).
*
* Note:			The string is non-volatile, so you can continue to 
*				use the strStreamTitle pointer without being in this 
*				function.
********************************************************************/
void NewStreamTitleProc(BYTE *strStreamTitle) {
	BYTE i;
	
	for(i = 0; i < 54u; i++){					// 54�����܂ŕ\��
		strSongName[i] = *strStreamTitle;
		strStreamTitle++;
	}
	for(i = 0; i < 54u; i++){
		if(!strSongName[i]){
			if(i < 54u){
				if(strSongName[i+1] == ';')
					break;
				strSongName[i] = ' ';
			}
		}
		if(strSongName[i] == ';'){
			strSongName[i] = 0;
			break;
		}
	}
	if(i < 54u)
		lenSongName = i;
	else{
		lenSongName = 54;
		strSongName[54] = 0;
	}
	gPtr = 0;
	NewSongName = 1;
	bitrate = GetBitrate();
}
