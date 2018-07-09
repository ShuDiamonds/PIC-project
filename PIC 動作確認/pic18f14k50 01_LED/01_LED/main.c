//------------------------------------------------
//	File name: PICトレーニング　01_LED
//	Description: LED OｎOff example with Bootloader
//  ４つのLEDが点滅する。
//  Notes:  １２MhzXtal ４８MHzシステムクロック
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
	TRISC=0xF0;				//入出力設定
	while(1){				//繰り返しループ
		PORTC = 0b00000011;
		Delay10KTCYx(240);	// 200mSecの遅延
		PORTC = 0b00001100;
		Delay10KTCYx(240);	// 200mSecの遅延
	}
}

