/********************************************************************************/
/*																				*/
/*			���Z�b�g��̏����� ����												*/
/*																				*/
/*			Cpu         :: PIC12F683											*/
/*			Language    :: PICC Lite											*/
/*			File Name   :: init.c												*/
/*																				*/
/********************************************************************************/
#include <pic.h>
#include <conio.h>

#include "pic12f683.h"

__CONFIG(FCMDIS & IESODIS & BORDIS & UNPROTECT & UNPROTECT & MCLRDIS & PWRTEN & WDTDIS & INTIO) ;

//__CONFIG(0x30c4);				/*Bit13:       :-:								*/
								/*Bit12:       :-:								*/
								/*Bit11:FCMEM  :0:̪�پ�� �ۯ� ��� �֎~			*/
								/*Bit10:IESO   :0:�������ް Ӱ�� �֎~			*/
								/*Bit09:BODEN1 :0:BOE �֎~						*/
								/*Bit08:BODEN0 :0:  �V							*/
								/*Bit07:*CPD   :1:�ް� ��� ���� �ی얳��		*/
								/*Bit06:*CP    :1:��۸��� ��� ���� �ی얳��		*/
								/*Bit05:MCLRE  :0:GP3 ���͂Ŏg�p				*/
								/*Bit04:*PWRTE :0:��ܰ���� ��� ����				*/
								/*Bit03:WDTE   :0:�����ޯ����� �֎~				*/
								/*Bit02:FSOC2  :1:�����ۯ��g�p,RA5&4��I/O		*/
								/*Bit01:FSOC1  :0:  �V							*/
								/*Bit00:FSOC0  :0:  �V							*/

void	sys_init(void);

void sys_init()
{
//
// �S�Ă̊����݋֎~
// �s���̒�` GP0��GP1�F�f�W�^���o�́iʲ����MOSFET�̺��۰فE۰���ނ̐ؑցj
//            GP2     �FCCP��Timer2�Ɉ˂�PWM�o�́i۰����MOSFET����ײ�ށj
//            GP3     �F�O���f�W�^������
//            GP4     �F��ٽ���v���ׁ̈ATimer1�̹ްē���
//            GP5     �F�ݒ�r�v�p�f�W�^�����́i��v���A�b�v�ݒ�j
//

//
//    �V�X�e�� ������
//
	CH_OSCCON  = 0b01110001 ;	/*Bit7:        :-:								*/
								/*Bit6:IRCF2   :1:�����ۯ�8MHz					*/
								/*Bit5:IRCF1   :1:  �V							*/
								/*Bit4:IRCF0   :1:  �V							*/
								/*Bit3:OSTS    :0:��ڰ� ���ı��� ��ϱ�� �ð��	*/
								/*Bit2:HTS     :0:HFINTOSC �ð��				*/
								/*Bit1:LTS     :0:LFINTOSC �ð��				*/
								/*Bit0:SCS     :1:���� �ۯ� ����				*/

	CH_PCON    = 0b00000000 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:ULPWUP  :0:���� ۰��ܰ �������� �֎~		*/
								/*Bit4:SBODEN  :0:BOD ��ĳ�� �֎~				*/
								/*Bit3:        :-:								*/
								/*Bit2:        :-:								*/
								/*Bit1:*POR    :0:��ܰONؾ�� ����				*/
								/*Bit0:*BOD    :0:BOD ����						*/

	CH_WDTCON  = 0b00001000 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:        :-:								*/
								/*Bit4:WDTPS3  :0:WDT����						*/
								/*Bit3:WDTPS2  :1:  �V							*/
								/*Bit2:WDTPS1  :0:  �V							*/
								/*Bit1:WDTPS0  :0:  �V							*/
								/*Bit0:SWDTEN  :0:WDT���g�p���Ȃ�				*/

	CH_INTCON  = 0b00000000 ;	/*Bit7:GIE     :0:�S���� �֎~					*/
								/*Bit6:PEIE    :0:���Ӌ@�\���� �֎~				*/
								/*Bit5:TOIE    :0:TMR0���ް�۰���� �֎~			*/
								/*Bit4:INTE    :0:INT��݊��� �֎~				*/
								/*Bit3:GPIE    :0:GPIO��݊��� �֎~				*/
								/*Bit2:TOIF    :0:TMR0���ް�۰ �׸�				*/
								/*Bit1:INTF    :0:INT��݊��� �׸�				*/
								/*Bit0:GPIF    :0:GPIO�ω����� �׸�				*/

	CH_PIR1    = 0b00000000 ;	/*Bit7:EEIE    :0:EEPROM�����݊������� �֎~		*/
								/*Bit6:ADIE    :0:A/D���� �֎~					*/
								/*Bit5:CCP1IE  :0:CCP1���� �֎~					*/
								/*Bit4:        :-:								*/
								/*Bit3:CMIE    :0:����ڰ����� �֎~				*/
								/*Bit2:OSFIE   :0:��ڰ��ُ튄�� �֎~			*/
								/*Bit1:TMR2IE  :0:��ϰ2ϯ����� �֎~				*/
								/*Bit0:TMR1IE  :0:��ϰ1���ް�۰���� �֎~		*/

//
//    ���g�p
//
//	CH_CMCON0  = 0b00000000 ;	/*  ����ڰ� ���g�p								*/
	CH_CMCON1  = 0b00000010 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:        :-:								*/
								/*Bit4:        :-:								*/
								/*Bit3:        :-:								*/
								/*Bit2:        :-:								*/
								/*Bit1:T1GSS   :1:��ϰ1�ްĂ�GP4(DI�ɂ��鎖)	*/
								/*Bit0:CMSYNC  :0:COUT����ϰ1�ɓ������Ȃ�		*/
//	CH_VRCON   = 0b00000000 ;	/*  ����ڰ� ���g�p								*/
//	CH_ADCON0  = 0b00000000 ;	/*  A/D���ް� ���g�p							*/
//	CH_EECON1  = 0b00000000 ;	/*  EEPROM ���g�p								*/
//								/*  Timer 0 ���g�p								*/

//
//    �f�o�h�n ������
//
	CH_GPIO    = 0b00000000 ;	/* GP5�`GP0 �̏o���ް�����U"0"�ɂ���			*/

	CH_OPTION  = 0b00001111 ;	/*Bit7:*GPPU   :0:����ٱ��ߋ���					*/
								/*Bit6:INTEDG  :0:INT��ݗ��オ��Ŋ������g�p	*/
								/*Bit5:TOCS    :0:TMR0 �����ۯ��g�p				*/
								/*Bit4:TOSE    :0:TMR0 T0CKI��ݗ����Ųݸ����	*/
								/*Bit3:SPA     :1:��ؽ��ׂ�WDT�֊���			*/
								/*Bit2:SP2     :1:��ؽ��� 1/128					*/
								/*Bit1:SP1     :1:  �V							*/
								/*Bit0:SP0     :1:  �V							*/

	CH_IOC     = 0b00000000 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:IOC5    :0:GP5 �����֎~					*/
								/*Bit4:IOC4    :0:GP4 �����֎~					*/
								/*Bit3:IOC3    :0:GP3 �����֎~					*/
								/*Bit2:IOC2    :0:GP2 �����֎~					*/
								/*Bit1:IOC1    :0:GP1 �����֎~					*/
								/*Bit0:IOC0    :0:GP0 �����֎~					*/

	CH_ANSEL   = 0b00000000 ;	/*Bit7:        :-:								*/
								/*Bit6:ADCS2   :0:A/D�ۯ��I��					*/
								/*Bit5:ADCS1   :0:  �V							*/
								/*Bit4:ADCS0   :0:  �V							*/
								/*Bit3:ANS3    :0:GP3 �޼���I/O�I��				*/
								/*Bit2:ANS2    :0:GP2 �޼���I/O�I��				*/
								/*Bit1:ANS1    :0:GP1 �޼���I/O�I��				*/
								/*Bit0:ANS0    :0:GP0 �޼���I/O�I��				*/

	CH_WPU     = 0b00100000 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:WPU5    :1:GP5 ����ٱ��ߎg�p				*/
								/*Bit4:WPU4    :0:GP4 ����ٱ��ߋ֎~				*/
								/*Bit3:        :-:								*/
								/*Bit2:WPU2    :0:GP2 ����ٱ��ߋ֎~				*/
								/*Bit1:WPU1    :0:GP1 ����ٱ��ߋ֎~				*/
								/*Bit0:WPU0    :0:GP0 ����ٱ��ߋ֎~				*/

	CH_TRISIO  = 0b00111100 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:GPIO5   :1:GP5 �ݒ轲������				*/
								/*Bit4:GPIO4   :1:GP4 ��ٽ����					*/
								/*Bit3:GPIO3   :1:GP3 �o�͋�����				*/
								/*Bit2:GPIO2   :1:GP2 CCP1 PWM (�捇��������)	*/
								/*Bit1:GPIO1   :0:GP1 FET��i�E�o��				*/
								/*Bit0:GPIO0   :0:GP0 FET��i���o��				*/

}
