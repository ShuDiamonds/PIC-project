/**********************************************************/
/*�A�i���O-�f�W�^���ϊ��֐����C�u����                     */
/*2011.12.30                                              */
/**********************************************************/
#include "p33Fxxxx.h"
#include "adclib.h"
#include "delaylib.h"

//====ADC1 10bit���[�h�������֐�=============================================================
void ADC1_10bit_Init(int PCFG)
{
	//----AD1CON1:ADC1���䃌�W�X�^1----
	//ADC���샂�[�h�r�b�g
	AD1CON1bits.ADON = 0;		//ADC���W���[������
	//�A�C�h�����[�h����~�r�b�g
	AD1CON1bits.ADSIDL = 0;		//�A�C�h�����[�h�������W���[���̓�����p��
	//DMA�o�b�t�@�r���h���[�h�r�b�g
	//AD1CON1bits.ADDMABM = 0;	//Scatter/Gather���[�h��DMA�o�b�t�@�ɏ�������
	//10�r�b�g/12�r�b�g���샂�[�h�r�b�g
	AD1CON1bits.AD12B = 0;		//10�r�b�g
	//�f�[�^�o�̓t�H�[�}�b�g�r�b�g
	AD1CON1bits.FORM = 0;		//0000 00 DD DDDD DDDD
	//�T���v���N���b�N���I���r�b�g
	AD1CON1bits.SSRC = 0;		//�T���v�����O�r�b�g���N���A���ꂽ�Ƃ��ɃT���v�����O�I��/�ϊ��J�n���g���K����
	//�����T���v�����O�I���r�b�g
	AD1CON1bits.SIMSAM = 0;		//�����`�����l���𒀎��T���v�����O����
	//ADC�T���v�������J�n�r�b�g
	AD1CON1bits.ASAM = 0;	//�ϊ��㎟�̃T���v�����O�������I�ɊJ�n����
	//ADC�T���v���C�l�[�u���r�b�g
	AD1CON1bits.SAMP = 0;		//ADC S&H�A���v�̓T���v�����O��ҋ@����
	//ADC�ϊ��X�e�[�^�X�r�b�g
	AD1CON1bits.DONE = 0;		//�ǂݍ��ݐ��A�\�t�g�E�F�A�łP�͏������߂܂���
	
	//----ADC1CON2:ADC1���䃌�W�X�^2----
	//�R���o�[�^�d�����t�@�����X�R���t�B�O���[�V�����r�b�g
	AD1CON2bits.VCFG = 0;		//VREFH = AVDD/VREFL = AVSS
	//���̓X�L�����I���r�b�g
	AD1CON2bits.CSCNA = 0;		//���͂��X�L�������Ȃ�
	//�`�����l���I���r�b�g
	AD1CON2bits.CHPS = 0;		//CH0��ϊ�����
	//�o�b�t�@�������݃X�e�[�^�X�r�b�g
	AD1CON2bits.BUFS = 0;		//ADC�͌��݃o�b�t�@�̑O�����ɏ�������ł���
	//�T���v�����O/�ϊ�����r�b�g
	AD1CON2bits.SMPI = 0;		//1��̃T���v�����O/�ϊ����삪�������邲�Ƃ�ADC���荞�݂𔭐�������
	//�o�b�t�@�������݃��[�h�I���r�b�g
	AD1CON2bits.BUFM = 0;		//��ɐ擪�A�h���X����o�b�t�@�̏������݂��J�n����B
	//���ݓ��̓T���v�����[�h�I���r�b�g
	AD1CON2bits.ALTS = 0;		//��ɃT���v��A�p�`�����l�����͑I�����g�p����
	
	//----AD1CON3:ADC1���䃌�W�X�^3----
	//ADC�ϊ��N���b�N���r�b�g
	AD1CON3bits.ADRC = 0;		//���������V�X�e���N���b�N���g�p����
	//�����T���v�����O���ԃr�b�g
	AD1CON3bits.SAMC = 0x04;	//4TAD
	//ADC�ϊ��N���b�N�I���r�b�g
	AD1CON3bits.ADCS = 0x05;	//TCY * (ADCS + 1) = TCY * 6 = TAD
	
	//----AD1CON4:ADC1���䃌�W�X�^4----
	//�A�i���O���͂������DMA�o�b�t�@���蓖�ăr�b�g
	//AD1CON4bits.DMABL = 0;		//�e�A�i���O���͂�1���[�h�o�b�t�@�����蓖�Ă�
	
	//----AD1CH123:ADC1���̓`�����l��1/2/3�I�����W�X�^----
	//�T���v��B�̃`�����l��1/2/3���ɐ����͑I���r�b�g
	AD1CHS123bits.CH123NB = 0;	//CH1,CH2,CH3���ɐ����͂�VREFL��I������
	//�T���v��B�̃`�����l��1/2/3���ɐ����͑I���r�b�g
	AD1CHS123bits.CH123SB = 0;	//CH1���ɐ����͂�AN0�ACH2���ɐ����͂�AN1�ACH3���ɐ����͂�AN2��I������
	//�T���v��A�̃`�����l��1/2/3���ɐ����͑I���r�b�g
	AD1CHS123bits.CH123NA = 0;	//CH1�ACH2�ACH3���ɐ����͂�VREFL��I������
	//�T���v��A �̃`�����l��1/2/3 ���ɐ����͑I���r�b�g
	AD1CHS123bits.CH123SA = 0;	//CH1���ɐ����͂�AN0�ACH2���ɐ����͂�AN1�ACH3���ɐ����͂�AN2��I������
	
	//----AD1CHS0:ADC1���̓`�����l��0�I�����W�X�^----
	//�T���v��B �̃`�����l��0 ���ɐ����͑I���r�b�g
	AD1CHS0bits.CH0NB = 0;		//�`�����l��0 ���ɐ����͂�VREFL ��I������
	//�T���v��B �̃`�����l��0 ���ɐ����͑I���r�b�g
	AD1CHS0bits.CH0SB = 0;		//�`�����l��0 ���ɐ����͂�AN0 ��I������
	//�T���v��A �̃`�����l��0 ���ɐ����͑I���r�b�g
	AD1CHS0bits.CH0NA = 0;		//�`�����l��0 ���ɐ����͂�VREFL ��I������
	//�T���v��A �̃`�����l��0 ���ɐ����͑I���r�b�g
	AD1CHS0bits.CH0SA = 0;		//�`�����l��0 ���ɐ����͂�AN0 ��I������
	
	//----AD1CSSH:ADC1���̓X�L�����I�����W�X�^HIGH----
	//ADC���̓X�L�����I���r�b�g
	//AD1CSSHbits.CSS = 0;		//���̓X�L������ANx��I�����Ȃ�
	
	//----AD1CSSL:ADC1���̓X�L�����I�����W�X�^LOW----
	//ADC���̓X�L�����I���r�b�g
	//AD1CSSL = 0;				//���̓X�L������ANx��I�����Ȃ�
	
	//----AD1PCFGH:ADC1�|�[�g�R���t�B�O���[�V�������W�X�^HIGH----
	//ADC�|�[�g�R���t�B�O���[�V��������r�b�g
	//AD1PCFGHbits.PCFG = 0xFFFF;	//AN16~AN31���ׂăf�W�^�����[�h�Ŏg�p����
	
	//----AD1PCFGL:ADC1�|�[�g�R���t�B�O���[�V�������W�X�^LOW----
	//ADC�|�[�g�R���t�B�O���[�V��������r�b�g
	//1111 11
	//5432 1098 7654 3210
	//1111 0001 1111 1111
	AD1PCFGL = PCFG;			//�f�W�^�����[�h�A�A�i���O���[�h�̐ݒ�
}

//====ADC1 12bit���[�h�������֐�================================================
void ADC1_12bit_Init(int PCFG)
{
	ADC1_10bit_Init(PCFG);
	
	//----AD1CON1:ADC1���䃌�W�X�^1----
	//10�r�b�g/12�r�b�g���샂�[�h�r�b�g
	AD1CON1bits.AD12B = 1;	//12�r�b�g
	
	//----AD1CON3:ADC1���䃌�W�X�^3---
	//ADC�ϊ��N���b�N�I���r�b�g
	AD1CON3bits.ADCS = 0x06;	//TCY * (ADCS + 1) = TCY * 4 = TAD
}

//====ADC�P�`�����l���f�[�^�擾�֐�=============================================
int ADC1_GetData(int Ch)
{
	AD1CON1bits.ADON = 0;		//ADC���W���[������
	AD1CHS0 = Ch;				//�`�����l���I��
	AD1CON1bits.ADON = 1;		//ADC���W���[���L��
	AD1CON1bits.SAMP = 1;		//ADC S&H�A���v�̓T���v�����O�����s����
	delay_us(20);				//�R���f���T�ɓd�ׂ����߂�
	AD1CON1bits.SAMP = 0;		//ADC S&H�A���v�̓T���v�����O��ҋ@
	while(!AD1CON1bits.DONE);	//DONE�r�b�g�����܂őҋ@
	return(ADC1BUF0);			//�ϊ��l��Ԃ�
}


