///////////////////////////////////////////
//�@LCD test program
//�@LCD is SC1602BSLB or SC1602BS*B
//////////////////////////////////////////
#include <16f877a.h>
#use fast_io(a)
#use delay(CLOCK=20000000)
#fuses HS,NOWDT,NOPROTECT
#define Bmode 0x0F			//port B initial mode
#define Amode 0				 //port A initial modeB
#byte db = 6				//port B

//////// Port define and link LCD library 
/*
#define rs PIN_D6		//chip select
#define rw PIN_D5			//read/write
#define stb PIN_D4			//strobe
*/
#define rs  PIN_D0
#define rw  PIN_D1
#define stb PIN_D2

#include <lcd_lib.c>

////////////////////////////////////////////////
// LCD test program main routine
// Display several message on LCD
// with some interval.
// Constant Message is send by lcd_data()
////////////////////////////////////////////// 

main(){
 set_tris_a(Amode);		 //mode set of port
 set_tris_b(Bmode);		 //lower is input
 set_tris_d(0);
	delay_ms(1000);
 lcd_init();				//initialize LCD
		output_high(PIN_C3);
	lcd_data("Hello World LCD");
	delay_ms(1000);
	 lcd_clear();
	lcd_cmd(0xC0);
	lcd_data("FUKUDA");
	delay_ms(1000);
	lcd_clear();
	
	
	
	
 while(1){					//endless loop
 	lcd_clear();			 //clear display
  lcd_data("Hello World LCD");
 	output_low(PIN_C3);
  delay_ms(1000);
  lcd_clear();			 //clear display
  lcd_data("message001");
 	output_high(PIN_C3);
  delay_ms(1000);			//wait 1sec
  lcd_clear();
  lcd_data("MESSAGE002");
 	output_low(PIN_C3);
  delay_ms(1000);
  lcd_clear();
  lcd_data("1234567890");
 	output_high(PIN_C3);
  delay_ms(1000);
  lcd_clear();
  lcd_data("abcdefghijklmnop");
  lcd_cmd(0xC0);			//second line
  lcd_data("qrstuvwxyz!#$%&'");
 	output_low(PIN_C3);
  delay_ms(3000);
 }
} 

      /*
      �s����t
#use delay(CLOCK=10000000)
�@�@����͉t���\������̃��C�u�������Ńf�B���C�֐����g����
�@�@����̂ŕK�����̊֐��ŃN���b�N���w�肵�Ă����K�v������
�@�@�܂��B
#define Bmode 0x0F
#define Amode 0
#byte db = 6
�@�@�����ŉt���\����Ɏg���|�[�g�̎w��Ə������[�h�̐ݒ�l��
�@�@��`���Ă��܂��B��ł́A�f�[�^�o�X���|�[�gB�A����̓|�[�gA
�@�@�ƂȂ��Ă���A�|�[�gB�̉��ʂS�r�b�g�͓��̓��[�h�������l��
�@�@���Ă��܂��B
#define rs PIN_A2
#define rw PIN_A1
#define stb PIN_A0
�@�@�����Ő���s���̎w������Ă��܂��B��ł̓|�[�gA��0,1,2�s��
�@�@�Ƃ��Ă��܂��B
#include <lcd_lib.c>
�@�@����Ŋ֐����C�u�������C���N���[�h���܂��B
lcd_init();
�@�@�������̊֐��ŁA�p�����[�^�͕K�v����܂���B���C���̍ŏ���
�@�@���s����K�v������܂��B
lcd_clear();
�@�@�t���\������������֐��ŁA�p�����[�^�͕K�v�����A��������
�@�@�N���A���܂��B
lcd_data("message001");
�@�@�������\������֐��̎g�p��ŁA""�ň͂�ꂽ�������
�@�@���̂܂܉t���ɕ\�����܂��B
�@�@���̊֐��ł�CCS�̃R���p�C���̓����ł���A�p�����[�^��
�@�@char�܂���int�����̎��ɁA��������p�����[�^�Ƃ��Ďg���ƁA����
�@�@��������P�����̌J��Ԃ��Ƃ��Ď��s���邱�Ƃ𗘗p���Ă��܂��B
�@�@����͕W����C�֐��ƈقȂ��Ă��܂��B
lcd_cmd(0xC0);
�@�@�t���\����̐���R�}���h�̎g�p��ŁA����łQ�s�ڂ̍ŏ���
�@�@�J�[�\���ʒu���ړ����܂��B 
						*/



