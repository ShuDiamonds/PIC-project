/**********************************************
* �w�b�_�t�@�C���̃C���N���[�h
***********************************************/
#include <p18f67j60.h>
#include <delays.h>
/**********************************************
* config�ݒ�
***********************************************/
/*
#pragma config DEBUG = OFF		//�o�b�N�O���E���h�E�f�o�b�O�E���[�h�iBDM���g��Ȃ�
//#pragma config ETHLED = ON		//Ethernet LED ���g��		�G���[�ɂȂ�̂ŃR�����g�A�E�g
#pragma config XINST = OFF		//�g�����߃Z�b�g�ƃC���f�b�N�X�A�h���X�� �g ��Ȃ��i �]�� ���[�h�j�I���r�b�g
#pragma config STVR = OFF		//Stack Overflow Reset���g��Ȃ�
#pragma config WDT = OFF		//�E�H�b�`�h�b�O�^�C�}�[���g��Ȃ�
#pragma config WDTPS = 32768	//�E�H�b�`�h�b�O�^�C�}�[��1:32768�ɂ���
#pragma config CP0 = OFF		//�u���b�N�O�i000800�|001�e�e�e���j�̃R�[�h��ی삵�Ȃ�
#pragma config FCMEN = OFF		//Fail-Safe Clock Monitor���g��Ȃ�
#pragma config IESO = OFF		//Internal External Osc. Switch���g��Ȃ�
#pragma config FOSC = EC		//�����N���b�N���g��
//#pragma config FOSC = HS		//�O�t���U���q ���p �� ���� �N���b�N�i8�l�g�� �ȏ� �j ���U
#pragma config FOSC2 = OFF		//Default/Reset System Clock Select Bit�@�̂��Ƃ炵��

//#pragma code
*/

#pragma config XINST=ON
#pragma config WDT=OFF, FOSC2=ON, FOSC=HSPLL, ETHLED=ON
#pragma config CP0=OFF //2011.01.04 �R�[�h�v���e�N�g���n�e�e�ǉ�


/**************************************************
*config�ɂ��Ẳ��
**************************************************/
/*
		//config�̐ݒ��PIC�ɂ���ċL�q�̎d�����Ⴄ�̂Œ���
			���xconfig��pic16f67j60�̂��̂łق���PIC�ł̓R���p�C���ł��Ȃ�
			
		
	*Brown-out Reset�Ƃ́A�d���������肢���ɂȂ�ƃ��Z�b�g�������铮��
	*Stack Overflow Reset�Ƃ́A�X�^�b�N�I�[�o�[�t���[���N����ƃ��Z�b�g�������铮��
	*Fail-Safe Clock Monitor�Ƃ́A�O���N���b�N�����͂���Ȃ��Ȃ����ꍇ�ɁA��������m���ē����N���b�N�œ����悤�ɂ���I�v�V�����ł�
	*Internal External Osc. Switch�A�Ƃ͊O���N���b�N�����肷��܂œ����N���b�N�œ���B����������ōs���I�v�V������
	*Low Voltage Programming�Ƃ́A��d���������݃��[�h�̂��Ƃŗp�͂TV�iPIC�ɂ���ĕς��j�ŏ������ގ��ɕK�vPIC�͔������΂���̎���ON�i�P�j�ł���
	*�v�����P�[���Ƃ́A�N���b�N�̃X�s�[�h��x�����邱�ƗႦ��PLLDIV = 2		�Q�� �� �� �Z ����܂�WMHz���SMHz�ɂ���Ƃ�������
	*HCS08�}�C�R���́C�o�b�N�O���E���h�E�f�o�b�O�E���[�h�iBDM�j�ƌĂ΂��f�o�b�O�E���[�h�𑕔����Ă���C�����}�C�R���̓����W���邱�ƂȂ��}�C�R���̓�����Ԃ𒲂ׂ邱��
	*pic��pll�ɂ��āA#pragma config	FOSC = HS	�ŊO�t���U���q ���p �� ���� �N���b�N�i8�l�g�� �ȏ� �j ���U
	�ł����#pragma config	FOSC = HSPLL_HS�ɕς���ƊO�t���U���q���p�̍����N���b�N���U�ŁA�o�k�k�� �g ��(HSPLL)�@�����g���Y�ł͂��̃��[�h
	�ɂȂ邻�̂���PLL�́A
#pragma config	PLLDIV = 1	�v���X�P�[�� �g�p ���Ȃ��i4�l�g���� ���U�� ���� �� ���� ���p �j
#pragma config	PLLDIV = 2	�Q�� �� �� �Z ����i8�l�g���� ���U �� ���� �� ���p �j
#pragma config	PLLDIV = 3	�R�Ŋ���Z����i�P�Q�l�g���̔��U����͂𗘗p�j
#pragma config	PLLDIV = 4	�S�Ŋ���Z����i�P�U�l�g���̔��U����͂𗘗p�j
#pragma config	PLLDIV = 5	�T�Ŋ���Z����i�Q�O�l�g���̔��U����͂𗘗p�j ->���g���Y ��� 20�l�g���^5��4�l�g��
#pragma config	PLLDIV = 6	�U�Ŋ���Z����i�Q�S�l�g���̔��U����͂𗘗p�j
#pragma config	PLLDIV = 10	�P�O�Ŋ���Z����i�S�O�l�g���̔��U����͂𗘗p�j
#pragma config	PLLDIV = 12	�P�Q�Ŋ���Z����i�S�W�l�g���̔��U����͂𗘗p�j
		�ɂȂ�Ⴆ�΂P�OMHz��	#pragma config	PLLDIV = 2
								#pragma config	FOSC = HSPLL_HS
								�Ƃ����PIC��20MHz�ŋN������		http://homepage3.nifty.com/mitt/pic/pic1320_04.html

�g�p����ڂ��Ă���
��������

#pragma config MCLRE  = OFF		//�}�X�^�[�N���A���g��Ȃ�
#pragma config PWRTEN = OFF		//Power-up Timer���g��Ȃ�
#pragma config BOREN  = OFF		//Brown-out Reset���g��Ȃ�		
#pragma config BORV   = 30		//Brown-out Voltage��3V�ɂ���
#pragma config WDTEN  = OFF		//�E�H�b�`�h�b�O�^�C�}�[���g��Ȃ�
#pragma config WDTPS  = 32768		//�E�H�b�`�h�b�O�^�C�}�[��1:32768�ɂ���
#pragma config STVREN = ON		//Stack Overflow Reset���g��
#pragma config FOSC   = IRC		//  �����N���b�N
//#pragma config	FOSC = HS	//�O�t���U���q ���p �� ���� �N���b�N�i8�l�g�� �ȏ� �j ���U
#pragma config PLLEN  = ON		//�킩��Ȃ�
#pragma config CPUDIV = NOCLKDIV		//CPU System Clock Postscaler
#pragma config USBDIV = OFF		//USB��clock��OSC1/OSC2������
#pragma config FCMEN  = OFF		//Fail-Safe Clock Monitor���g��Ȃ�
#pragma config IESO   = OFF		//Internal External Osc. Switch���g��Ȃ�
#pragma config HFOFST = OFF		//
#pragma config LVP    = ON		//Low Voltage Programming���g��
#pragma config XINST  = OFF		//�g�����߃Z�b�g�ƃC���f�b�N�X�A�h���X�� �g ��Ȃ��i �]�� ���[�h�j�I���r�b�g
#pragma config BBSIZ  = OFF		//Boot Block Size�̃T�C�Y�w����ۂ�
#pragma config CP0    = OFF		//�u���b�N�O�i000800�|001�e�e�e���j�̃R�[�h��ی삵�Ȃ�
#pragma config CP1    = OFF		//�u���b�N�P�i00�Q�O00�|00�R�e�e�e���j�̃R�[�h��ی삵�Ȃ�
#pragma config CPB    = OFF		//�u�[�g�u���b�N�i0000�O00�|0007�e�e���j�̃R�[�h��ی삵�Ȃ�
#pragma config WRT0   = OFF		//�u���b�N0 (000800-001FFFh) �̏����ݕی삵�Ȃ�
#pragma config WRT1   = OFF		//�u���b�N1 (002000-003FFFh) �̏����ݕی삵�Ȃ�
#pragma config WRTB   = OFF		//Boot block (000000-0007FFh)�̏����ݕی삵�Ȃ�
#pragma config WRTC   = OFF		//Configuration registers (300000-3000FFh) �̏����ݕی삵�Ȃ�
#pragma config EBTR0  = OFF		//Block 0 (000800-001FFFh) �𑼂̃u���b�N���s���̃e�[�u���ǂݎ�肩��ی삵�Ȃ�
#pragma config EBTR1  = OFF		//Block 1 (002000-003FFFh) �𑼂̃u���b�N���s���̃e�[�u���ǂݎ�肩��ی삵�Ȃ�
#pragma config EBTRB  = OFF		//Boot block (000000-0007FFh)�𑼂̃u���b�N���s���̃e�[�u���ǂݎ�肩��ی삵�Ȃ�

#pragma code

�����܂�



*/
/**********************************************
* �s���}�N��
***********************************************/

	#define 	LED0_TRIS		(TRISEbits.TRISE2)
	#define 	LED0_IO			(PORTEbits.RE2)
	#define 	LED1_TRIS		(TRISEbits.TRISE3)
	#define 	LED1_IO			(PORTEbits.RE3)
	#define 	LED2_TRIS		(TRISEbits.TRISE5)
	#define 	LED2_IO			(PORTEbits.RE5)
	//#define 	LED3_TRIS		(TRISEbits.TRISD1)
	//#define 	LED3_IO			(PORTEbits.RD1)

	#define 	LED_RUN_TRIS		(TRISDbits.TRISD2)
	#define 	LED_RUN_IO			(PORTDbits.RD2)
//	#define 	LED_IO			(*((volatile unsigned char*)(&PORTE)))

	#define 	BUTTON0_TRIS		(TRISBbits.TRISB3)
	#define		BUTTON0_IO		(PORTBbits.RB3)
	#define 	BUTTON1_TRIS		(TRISBbits.TRISB2)
	#define		BUTTON1_IO		(PORTBbits.RB2)
	#define 	BUTTON2_TRIS		(TRISBbits.TRISB1)
	#define		BUTTON2_IO		(PORTBbits.RB1)
	#define 	BUTTON3_TRIS		(TRISBbits.TRISB0)
	#define		BUTTON3_IO		(PORTBbits.RB0)

	// LCD I/O pins
	#define 	LCD_DATA_TRIS	(TRISF)
	#define 	LCD_DATA_IO		(LATF)
	#define 	LCD_RD_WR_TRIS	(TRISFbits.TRISF1)
	#define 	LCD_RD_WR_IO	(LATFbits.LATF1)
	#define 	LCD_RS_TRIS		(TRISFbits.TRISF2)
	#define 	LCD_RS_IO		(LATFbits.LATF2)
	#define 	LCD_E_TRIS		(TRISFbits.TRISF3)
	#define 	LCD_E_IO		(LATFbits.LATF3)

	// Servo Output
	#define		RCS1_TRIS		(TRISCbits.TRISC2)
	#define 	RCS1_IO			(LATCbits.LATC2)
	#define		RCS2_TRIS		(TRISCbits.TRISC0)
	#define 	RCS2_IO			(LATCbits.LATC0)
	#define		UIO1_TRIS		(TRISCbits.TRISA4)
	#define		UIO1_IO			(LATCbits.LATA4)
	#define		UIO2_TRIS		(TRISCbits.TRISC7)
	#define		UIO2_IO			(LATCbits.LATC7)

/**********************************************
* �֐��v���g�^�C�v
***********************************************/
static void InitializeBoard(void);
/**********************************************
* ���荞�ݏ���
***********************************************/
/*
// ��ʊ��荞�݁i�C���^�[�o���^�C�}�j
	#pragma interruptlow LowISR
	void LowISR(void)
	{
		
	}
	#pragma code lowVector=0x18
	void LowVector(void){_asm goto LowISR _endasm}
// ���ʊ��荞�݁i���g�p�j
//	#pragma code highVector=0x8
//	void HighVector(void){_asm goto HighISR _endasm}

#pragma code 		// Return to default code section
*/
/********************************************
*  ���C���֐�
************************************************/

void main(void)
{
	//���[�J���ϐ���`
	
	int p;
	
	LED_RUN_IO = 0;		//����m�F
	LED0_IO	= 0;
	LED1_IO	= 0;
	while(1)
	{
	
		LED_RUN_IO = 0;		//����m�F
		for(p=0;1000000000000000000000000<=p;p++)
			{
				Nop();
			}
		LED_RUN_IO = 1;
		 for(p=0;100000000000000000000000<=p;p++)
		{
			Nop();
		};
		LED_RUN_IO = 0;
		for(p=0;1000000000000000000000000<=p;p++)
		{
			Nop();
		}
		LED_RUN_IO = 1;
		for(p=0;10000000000000000000000<=p;p++)
		{
			Nop();
		}
		LED_RUN_IO = 0;
	}
}

/*******************************************************************
*  �n�[�h�E�F�A�������֐�
*
********************************************************************/

static void InitializeBoard(void)
{	
	/*
	// LEDs
	LED0_TRIS = 0;
	LED1_TRIS = 0;
	LED2_TRIS = 0;
	//LED3_TRIS = 0;
	LED_RUN_TRIS = 0;
	LED0_IO = 0;
	LED1_IO = 0;
	LED2_IO = 0;
	//LED3_IO = 0;
	LED_RUN_IO = 0;
	
	//button
	BUTTON0_TRIS = 1;
	BUTTON1_TRIS = 1;
	BUTTON2_TRIS = 1;
	BUTTON3_TRIS = 1;
	
	// Servo 
	RCS1_TRIS = 0;
	RCS1_IO = 0;
	RCS2_TRIS = 0;
	RCS2_IO = 0;
	//UIO1_TRIS = 0;
	//UIO1_IO = 0;
	*/
	
	
	TRISA=0;                        // �|�[�gA�����ׂďo�̓s���ɂ���
	TRISB=0;                        // �|�[�gB�����ׂďo�̓s���ɂ���
	TRISC=0;                        // �|�[�gC�����ׂďo�̓s���ɂ���
	TRISD=0x00;                     // Upper is Output and Lower is Input
	
	
	// Enable 4x/5x PLL on PIC18F87J10, PIC18F97J60, etc.
	OSCTUNE = 0x40;			
	// Set up analog features of PORTA
	ADCON0 = 0x0D;		// ADON Channel 3
	ADCON1 = 0x0B;		// Vdd/Vss AN0 to AN3 are analog
	ADCON2 = 0xBE;			// Right justify, 20TAD ACQ time, Fosc/64 (~21.0kHz)
    /*
	// Enable internal PORTB pull-ups
   INTCON2bits.RBPU = 0;
	// Enable Interrupts
	RCONbits.IPEN = 1;		// Enable interrupt priorities
    INTCONbits.GIEH = 1;
    INTCONbits.GIEL = 1;
	ADCON0bits.ADCAL = 1;
    	ADCON0bits.GO = 1;
	while(ADCON0bits.GO);
	ADCON0bits.ADCAL = 0;
	*/
}

