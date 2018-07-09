/********************************************************************************/
/*																				*/
/*			�������E�A�C�h�����O ����											*/
/*																				*/
/*			Cpu         :: PIC12F683											*/
/*			Language    :: PICC Lite�iPIC16F690�ź��߲فj						*/
/*			File Name   :: main.c												*/
/*																				*/
/********************************************************************************/

/**************************************************************************************/
/*                                                                                    */
/*  �o�[�W����                                                                        */
/*------------------------------------------------------------------------------------*/
/*  version 0.10  (T.shimakami) 2007-06/18  ����m�F ����(PIC12F675�ź��߲�)          */
/*  version 0.20  (T.shimakami) 2007-06/19  ���ׂ߲����߂�PIC16F690�ɕύX             */
/*  version 0.21  (T.shimakami) 2007-06/19  I/O���o�͊m�F                             */
/*  version 0.30  (T.shimakami) 2007-06/20  �A�v���P�[�V�����쐻                      */
/*  version 0.40  (T.shimakami) 2007-06/22  ���������E�œK��                          */
/*  version 0.41  (T.shimakami) 2007-06/25  ���[�h�ύX�^�C�~���O������                */
/*  version 0.42  (T.shimakami) 2007-06/25  ���Z�b�g��̕s��C��                    */
/*                                                                                    */
/**************************************************************************************/

#include <pic.h>
#include <conio.h>

#include "pic12f683.h"

void			sys_init(void) ;
void			pulse_init(void) ;
unsigned int	pulse_read(void) ;
void			motor_init(void) ;
void			motor_out(unsigned char , unsigned char) ;
unsigned char	motor_flag(void) ;

//						   /* "          1         2         " */
//						   /* "012345678901234567890123456789" */
unsigned char	ver_buf[32] = "Speed Controller Ver0.42" ;


/****************************************************************************/
/*																			*/
/*	���C�������i�A�C�h���j													*/
/*																			*/
/****************************************************************************/
main()
{
	unsigned char	pls_his , pls_bit ;		/* �p���X���͂c�h					*/
	unsigned char	sw_his , sw_bit ;		/* �ݒ�X�C�b�`						*/
	unsigned char	di_his , di_bit ;		/* �O�����͂c�h						*/

	unsigned char	gate_old ;				/* �p���X���͂̃^�C�~���O�m�F�p		*/
	unsigned int	pulse_data ;			/* �p���X�� 0.7�` 1.5�` 2.3 ms		*/
											/* Timer2  1400�`3000�`4600 ����	*/
	unsigned int	time_cnt ;				/* �p���X�̃^�C���A�E�g �J�E���^	*/
	unsigned char	pnon_cnt ;				/* ����Ƀp���X��ǂݎ̂Ă��		*/
	unsigned char	prg_cnt ;				/* �����J�E���^						*/

	unsigned char	zoffcnt ;				/* ����OFF�J�E���g					*/
	unsigned char	dmode ;					/*      1:OFF						*/
											/*      2:�O�i						*/
											/*      3:���						*/
											/*      4:�u���[�L					*/
	unsigned char	mmode ;					/* ���ۂ̃��[�h						*/
											/*      1:OFF						*/
											/*      2:�O�i						*/
											/*      3:���						*/
											/*      4:�u���[�L					*/
											/* ���̑�:�ُ�l					*/
	unsigned char	power ;					/* PWM�l 0�`250 �w�ߒl				*/

	unsigned char	pwmdo ;					/* PowerMOSFET ��i�o�͗p(GP0,GP0)	*/
	unsigned char	pwr ;					/* PWM�l 0�`250						*/
	int				drive_data ;			/* ���[�^�쓮�f�[�^					*/
	int				motor_data ;			/* ���[�^�o�̓f�[�^					*/

	int				wk_int ;


//
//	�h�^�n ������
//
	sys_init() ;							/* ���Z�b�g��̓������W�X�^������	*/
	pulse_init() ;							/* �p���X���͗p ������				*/
	motor_init() ;							/* ���[�^�o�� ������				*/

//
//	�f�[�^���t���O �������h�^�n
//
	pls_his = 0x07 , pls_bit =1 ;			/* �p���X���͂c�h�g�P�h�Z�b�g		*/
	sw_his = 0x07 , sw_bit = 1 ;			/* �ݒ�X�C�b�`�g�P�h�Z�b�g			*/
	di_his = 0x07 , di_bit = 1 ;			/* �O�����͂c�h�g�P�h�Z�b�g			*/
	gate_old = di_bit ;						/* ���̃Q�[�g�̏��					*/

	time_cnt = 0 ;							/* �^�C���A�E�g �J�E���^ �N���A		*/
	pnon_cnt = 5 ;							/* �p���X�f�[�^���T��ǂݎ̂Ă�		*/
	pulse_data = 3000 ;						/* �p���X�� �����l					*/
	drive_data = 0 ;						/* ���[�^�쓮�g�O�h					*/
	motor_data = 0 ;						/* ���[�^�o�́g�O�h					*/
	zoffcnt = 30 ;							/* ����OFF�J�E���g max				*/
	dmode = mmode = 1 ;						/* ���[�h 1:OFF						*/
	pwmdo = 0x00 ;							/* PowerMOSFET ��i OFF				*/
	pwr = 0 ;								/* PWM�l�g�O�h						*/
	prg_cnt = 0 ;							/* �����J�E���^�g�O�h				*/

	while(1){
		while(motor_flag() != 1){			/* 120��s ���̃t���O�`�F�b�N		*/
		}
//
//		CH_GPIO = 0x01 ;					/* ���s���ԑ���						*/
//

//
//	�f�W�^�����́E�ݒ�X�C�b�` �t�B���^�����i�P�Q�O�ʂ����̏����j
//
		if(BI_GPIO4 == 1){					/* �p���X���͂cI					*/
			pls_his = ((pls_his << 1) | 0x01) & 0x07 ;
			if(pls_his == 0x07){
				pls_bit = 1 ;
			}
		}else{
			pls_his = ((pls_his << 1) & ~0x01) & 0x07 ;
			if(pls_his == 0x00){
				pls_bit = 0 ;
			}
		}
		if(BI_GPIO5 == 1){					/* �ݒ�X�C�b�`						*/
			sw_his = ((sw_his << 1) | 0x01) & 0x07 ;
			if(sw_his == 0x07){
				sw_bit = 1 ;
			}
		}else{
			sw_his = ((sw_his << 1) & ~0x01) & 0x07 ;
			if(sw_his == 0x00){
				sw_bit = 0 ;
			}
		}
		if(BI_GPIO3 == 1){					/* �O�����͂c�h						*/
			di_his = ((di_his << 1) | 0x01) & 0x07 ;
			if(di_his == 0x07){
				di_bit = 1 ;
			}
		}else{
			di_his = ((di_his << 1) & ~0x01) & 0x07 ;
			if(di_his == 0x00){
				di_bit = 0 ;
			}
		}
//
//	�S�W�O�ʂ����̏���
//
		prg_cnt++ ;
		if(prg_cnt >= 5){
			prg_cnt = 1 ;
		}
		switch (prg_cnt){
//
//	�p���X���͏���
//
			case 1 :{
				if((gate_old != 0) && (pls_bit == 0)){	/* �Q�[�g���������H	*/
					time_cnt = 0 ;				/* �^�C���A�E�g �J�E���^ �N���A	*/
					if(pnon_cnt == 0){					/* �ʏ�H				*/
						pulse_data = pulse_read() ;		/* �p���X����Ǐo��		*/
						if((pulse_data<=1000)||(5000<=pulse_data)){	/* �ُ�l�H	*/
							pulse_data = 3000 ;	/* �p���X�����l					*/
							pnon_cnt = 2 ;		/* ���A���p���X�f�[�^�Q��ǎ̂�	*/
						}
					}else{
						pnon_cnt-- ;			/* �ǂݎ̂Ă�					*/
						pulse_read() ;			/* �_�~�[�̓Ǐo��				*/
					}
				}else{
					time_cnt++ ;
					if(time_cnt > 8333){		/* ��P�b�p���X�f�[�^�������H	*/
						pulse_read() ;			/* �_�~�[�̓Ǐo��				*/
						pulse_data = 3000 ;		/* �p���X�����l					*/
						pnon_cnt = 2 ;			/* ���A���p���X�f�[�^�Q��ǎ̂�	*/
						time_cnt = 0 ;			/* �^�C���A�E�g �J�E���^ �N���A	*/
					}
				}
				gate_old = pls_bit ;
			}break ;
//
//	���䒆������A���䒆�Ȃ�w�ߒl�Z�b�g
//
			case 2 :{
				if(((sw_bit==1)&&(di_bit==1)) || ((sw_bit==0)&&(di_bit==0))){
					wk_int = 3000 - pulse_data ;	/* �p���X���쓮�f�[�^�ɕϊ�	*/
//		�q�X�e���V�X����
					if((wk_int>drive_data+3)||(wk_int<drive_data-3)){
						drive_data = (wk_int + 2) & 0xfffc ;	/* �l�̌ܓ�		*/
					}
				}else{
					drive_data = 0 ;			/* ����OFF����					*/
				}
			}break ;
//
//	���[�^�o�̓f�[�^��� ����
//
			case 3 :{
//		���݂̎w�߂����
				motor_data = drive_data ;		/* �o�̓f�[�^�X�V				*/
				if ( 1000 < motor_data ) {		/* ����ȏ�H{(240+10)�~4}		*/
					motor_data = 1000 ;			/* �ő�l�Z�b�g					*/
				}
				if ( -1000 > motor_data ) {		/* �����ȉ��H{(-240-10)�~4}		*/
					motor_data = -1000 ;		/* �ŏ��l�Z�b�g					*/
				}

				if (-43 >= motor_data) {		/* ��ށH(-10�~4-3�ȉ�)			*/
					dmode = 3 ;					/* ��ރ��[�h					*/
					power = ((-1 * motor_data) >> 2) - 4 ;	/* ��΁�8Bit��		*/
				} else {
					if ( 43 <= motor_data ) {	/* �O�i�H(10�~4+3�ȏ�)			*/
						dmode = 2 ;				/* �O�i���[�h					*/
						power = (motor_data >> 2) - 4 ;		/* 8Bit��			*/
					} else {					/* �s����						*/
						dmode = 1 ;				/* �n�e�e���[�h					*/
						power = 0 ;
					} 
				}
//
//		����̓u���[�L�ݒ薳��
				if(0){							/* �u���[�L�v���L�邩�H			*/
					dmode = 4 ;					/* �u���[�L���[�h				*/
					power = 250 ;
				}
			}break ;
//
//	���[�^�o�̓f�[�^ ����
//
			case 4 :{
//		�o�̓f�[�^�쐬
				switch( mmode ) {				/* ���[�h �H					*/
					case 1 : {					/* �n�e�e���[�h					*/
						pwmdo = 0x00 ;			/* �SFET�o�͋֎~ GP0&1			*/
						pwr = 0 ;				/* PWM�o�͏�����				*/
						if ( zoffcnt != 0 ) {	/* ����OFF���H					*/
							zoffcnt-- ;			/* ����OFF�����޳�				*/
						}else{
							mmode = dmode ;		/* ���[�h�ύX					*/
						}
					} break ;
					case 2 : {					/* �O�i���[�h					*/
						pwmdo = 0x01 ;			/* FET�o�͑O�i�ݒ� GP0&1		*/
						if(dmode == 2){			/* �O�i���[�h�p���H				*/
							if(pwr < power) pwr++ ;	/* �o�͒l					*/
							if(pwr > power) pwr-- ;	
						}else{
							if(pwr != 0){
								pwr-- ;			/* �o�͒l�g�O�h��				*/
							}else{
								mmode = 1 ;		/* ��UOFFӰ�ނɐݒ�			*/
								zoffcnt = 30 ;	/* ����OFF�����������			*/
							}
						}
					} break ;
					case 3 : {					/* ��ރ��[�h					*/
						pwmdo = 0x02 ;			/* FET�o�͌�ސݒ� GP0&1		*/
						if(dmode == 3){			/* ��ރ��[�h�p���H				*/
							if(pwr < power) pwr++ ;	/* �o�͒l					*/
							if(pwr > power) pwr-- ;	
						}else{
							if(pwr != 0){
								pwr-- ;			/* �o�͒l�g�O�h��				*/
							}else{
								mmode = 1 ;		/* ��UOFFӰ�ނɐݒ�			*/
								zoffcnt = 30 ;	/* ����OFF�����������			*/
							}
						}
					} break ;
					case 4 : {					/* �u���[�L���[�h				*/
						pwmdo = 0x03 ;			/* FET�o����ڰ��ݒ� GP0&1		*/
						if(dmode == 4) {		/* �u���[�L�p���H				*/
							if(pwr < power) pwr++ ;	/* �o�͒l					*/
							if(pwr > power) pwr-- ;	
						}else{
							if(pwr != 0){
								pwr-- ;			/* �o�͒l�g�O�h��				*/
							}else{
								mmode = 1 ;		/* ��UOFFӰ�ނɐݒ�			*/
								zoffcnt = 30 ;	/* ����OFF�����������			*/
							}
						}
					} break ;
					default : {					/* ���[�h�ُ�l					*/
						mmode = 1 ;				/* ��UOFFӰ�ނɐݒ�			*/
						zoffcnt = 30 ;			/* ����OFF�����������			*/
						pwr = 0 ;				/* PWM�o�͏�����				*/
						pwmdo = 0x00;			/* �SFET�o�͋֎~ GP0&1			*/
					}
				}
//		�o�͏���
//
//				pwr = pulse_data >> 4  ;		/* �p���X�̃��j�^				*/
//				pwr = (drive_data >> 4) + 128 ;	/* �쓮�f�[�^�̃��j�^			*/
//				pwmdo = 0x01 ;					/* ���s���ԑ���					*/
//
				motor_out(pwmdo,pwr) ;			/* ���[�^�o��					*/
			}break ;
//
//	�݂蓾�Ȃ���������̏���
//
			default :{
				prg_cnt = 0 ;
			}
		}
//
//		CH_GPIO = 0x00 ;					/* ���s���ԑ���						*/
//
	}
}

