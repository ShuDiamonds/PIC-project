
#include <p18f67j60.h>


#pragma config DEBUG = OFF		//�o�b�N�O���E���h�E�f�o�b�O�E���[�h�iBDM���g��Ȃ�
//#pragma config ETHLED = ON		//Ethernet LED ���g��		�G���[�ɂȂ�̂ŃR�����g�A�E�g
#pragma config XINST = OFF		//�g�����߃Z�b�g�ƃC���f�b�N�X�A�h���X�� �g ��Ȃ��i �]�� ���[�h�j�I���r�b�g
#pragma config STVR = OFF		//Stack Overflow Reset���g��Ȃ�
#pragma config WDT = OFF		//�E�H�b�`�h�b�O�^�C�}�[���g��Ȃ�
#pragma config WDTPS = 32768	//�E�H�b�`�h�b�O�^�C�}�[��1:32768�ɂ���
#pragma config CP0 = OFF		//�u���b�N�O�i000800�|001�e�e�e���j�̃R�[�h��ی삵�Ȃ�
#pragma config FCMEN = OFF		//Fail-Safe Clock Monitor���g��Ȃ�
#pragma config IESO = OFF		//Internal External Osc. Switch���g��Ȃ�
#pragma config FOSC = HS		//�O�t���U���q ���p �� ���� �N���b�N�i8�l�g�� �ȏ� �j ���U
#pragma config FOSC2 = OFF		//Default/Reset System Clock Select Bit�@�̂��Ƃ炵��

#pragma code


/**************************************************
*config�ɂ��Ẳ��
**************************************************/
/*			//config�̐ݒ��PIC�ɂ���ċL�q�̎d�����Ⴄ�̂Œ���
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
void main(void){
	unsigned int	i;		//�ϐ��̐錾
	unsigned char	vLED = 0x01;	//�ϐ��̐錾�Ə����l�̐ݒ�
	OSCCON = 0b01010010;		//  �����N���b�N4Mhz
	TRISC=0xF0;					//���o�͐ݒ�
	while(1){					//�J��Ԃ����[�v
		if(PORTBbits.RB7==1){				//�{�^����������Ă��邩
			vLED = vLED>>1;
			if(vLED==0x00){vLED=0x08;}
		}else{
			vLED = vLED<<1;
			if(vLED==0x10){vLED=0x01;}
		}
		PORTC = vLED;
		for(i=20000;i>0;i--);		//������̊Ԋu�x��
	}
}



