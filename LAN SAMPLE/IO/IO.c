/************************************************************
�@�@MPLAB-C18�ɂ����o�̓|�[�g�̐���e�X�g�v���O����
�@�@�X�C�b�`�̓��͂��|�[�gD����s���A�X�C�b�`�̏�Ԃ�
�@�@�]���āA�w�肳�ꂽ�����_�C�I�[�h��_�ł�����B
�@�@�@�@RD0�̃X�C�b�`��OFF�̎�LED�P
�@�@�@�@RD1�̃X�C�b�`��OFF�̎�LED2
�@�@�@�@RD2�̃X�C�b�`��OFF�̎�LED3
�@�@�@�@�S�X�C�b�`��ON�̎��SLED
*************************************************************/
#include <p18f67j60.h>    // PIC18C452�̃w�b�_�E�t�@�C��
#include <delays.h>
//***** �R���t�B�M�����[�V�����̐ݒ�
#pragma romdata configBits=0x300000
unsigned char rom configBitsArr[8] = {0xff,0xe2,0x06,0x00,0x00,0x01,0x03,0x00};
    // �R���t�B�M�����[�V�����E�r�b�g�̐ݒ�
    // �R�[�h�E�v���e�N�g=OFF,�V�X�e���E�N���b�N�؂�ւ�=OFF,
    // HS���U��,�E�H�b�`�h�b�O�E�^�C�}=OFF,
    // �|�X�g�X�P�[��1:128,4.2V�u���E���A�E�g�E���Z�b�g,
    //  �p���[�A�b�v�E�^�C�}=ON,CCP2���o�͂�RC1�s���Ɋ��蓖��,
    //�X�^�b�N�E�I�[�o�[�t���[/�A���_�[�t���[�E���Z�b�g=ON

//***** ���C���֐�
void main(void)                     // ���C���֐�
{
    TRISA=0;                        // �|�[�gA�����ׂďo�̓s���ɂ���
    TRISB=0;                        // �|�[�gB�����ׂďo�̓s���ɂ���
    TRISC=0;                        // �|�[�gC�����ׂďo�̓s���ɂ���
    TRISD=0x0F;                     // Upper is Output and Lower is Input

    while(1)    
    {
        if(PORTDbits.RD0)           // �X�C�b�`�POFF���H
        {
            LATC=01;
            Delay10KTCYx(100);      // LED�P��_��
            LATC=00;
            Delay10KTCYx(100);
        }
        else if(PORTDbits.RD1)      // �X�C�b�`�QOFF���H
        {
            LATC=02;
            Delay10KTCYx(200);      // LED2��_��
            LATC=00;
            Delay10KTCYx(200);
        }
        else if(PORTDbits.RD2)      // �X�C�b�`�ROFF���H
        {
            LATC=04;
            Delay10KTCYx(100);      // LED�R��_��
            LATC=00;
            Delay10KTCYx(100);
        }
        else                        // �S��ON���H
        {
            LATC=0xFF;              // �SLED�_��
            Delay10KTCYx(200);
            LATC=00;
            Delay10KTCYx(200);
        }
    }
}
