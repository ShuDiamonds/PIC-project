//------------------------------------------------
//	File name: PIC�g���[�j���O�@01_LED
//	Description: LED O��Off example with Bootloader
//  �S��LED���_�ł���B
//  Notes:  �P�QMhzXtal �S�WMHz�V�X�e���N���b�N
//		LED RC0
//		LED RC1
//		LED RC2
//		LED RC3
//	Language: MPLAB C18
//	Target: PIC18F14K50
//------------------------------------------------

#include <p18cxxx.h>
#include <delays.h>

#define REMAPPED_RESET_VECTOR_ADDRESS	0x1000
extern void _startup (void);
#pragma code reset_vect = REMAPPED_RESET_VECTOR_ADDRESS
void _reset (void){_asm goto _startup _endasm}

#pragma code
void main(void){
	TRISC=0xF0;				//���o�͐ݒ�
	while(1){				//�J��Ԃ����[�v
		PORTC = 0b00000011;
		Delay10KTCYx(240);	// 200mSec�̒x��
		PORTC = 0b00001100;
		Delay10KTCYx(240);	// 200mSec�̒x��
	}
}

