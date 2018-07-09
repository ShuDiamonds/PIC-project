/********************************************************************************/
/*																				*/
/*			リセット後の初期化 処理												*/
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
								/*Bit11:FCMEM  :0:ﾌｪｰﾙｾｰﾌ ｸﾛｯｸ ﾓﾆﾀ 禁止			*/
								/*Bit10:IESO   :0:ｽｲｯﾁｵｰﾊﾞｰ ﾓｰﾄﾞ 禁止			*/
								/*Bit09:BODEN1 :0:BOE 禁止						*/
								/*Bit08:BODEN0 :0:  〃							*/
								/*Bit07:*CPD   :1:ﾃﾞｰﾀ ﾒﾓﾘ ｺｰﾄﾞ 保護無し		*/
								/*Bit06:*CP    :1:ﾌﾟﾛｸﾞﾗﾑ ﾒﾓﾘ ｺｰﾄﾞ 保護無し		*/
								/*Bit05:MCLRE  :0:GP3 入力で使用				*/
								/*Bit04:*PWRTE :0:ﾊﾟﾜｰｱｯﾌﾟ ﾀｲﾏ 許可				*/
								/*Bit03:WDTE   :0:ｳｫｯﾁﾄﾞｯｸﾞﾀｲﾏ 禁止				*/
								/*Bit02:FSOC2  :1:内部ｸﾛｯｸ使用,RA5&4はI/O		*/
								/*Bit01:FSOC1  :0:  〃							*/
								/*Bit00:FSOC0  :0:  〃							*/

void	sys_init(void);

void sys_init()
{
//
// 全ての割込み禁止
// ピンの定義 GP0＆GP1：デジタル出力（ﾊｲｻｲﾄﾞMOSFETのｺﾝﾄﾛｰﾙ・ﾛｰｻｲﾄﾞの切替）
//            GP2     ：CCP＆Timer2に依るPWM出力（ﾛｰｻｲﾄﾞMOSFETのﾄﾞﾗｲﾌﾞ）
//            GP3     ：外部デジタル入力
//            GP4     ：ﾊﾟﾙｽ幅計測の為、Timer1のｹﾞｰﾄ入力
//            GP5     ：設定ＳＷ用デジタル入力（弱プルアップ設定）
//

//
//    システム 初期化
//
	CH_OSCCON  = 0b01110001 ;	/*Bit7:        :-:								*/
								/*Bit6:IRCF2   :1:内部ｸﾛｯｸ8MHz					*/
								/*Bit5:IRCF1   :1:  〃							*/
								/*Bit4:IRCF0   :1:  〃							*/
								/*Bit3:OSTS    :0:ｵｼﾚｰﾀ ｽﾀｰﾄｱｯﾌﾟ ﾀｲﾏｱｳﾄ ｽﾃｰﾀｽ	*/
								/*Bit2:HTS     :0:HFINTOSC ｽﾃｰﾀｽ				*/
								/*Bit1:LTS     :0:LFINTOSC ｽﾃｰﾀｽ				*/
								/*Bit0:SCS     :1:ｼｽﾃﾑ ｸﾛｯｸ 内部				*/

	CH_PCON    = 0b00000000 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:ULPWUP  :0:ｳﾙﾄﾗ ﾛｰﾊﾟﾜｰ ｳｪｲｸｱｯﾌﾟ 禁止		*/
								/*Bit4:SBODEN  :0:BOD ｿﾌﾄｳｪｱ 禁止				*/
								/*Bit3:        :-:								*/
								/*Bit2:        :-:								*/
								/*Bit1:*POR    :0:ﾊﾟﾜｰONﾘｾｯﾄ 発生				*/
								/*Bit0:*BOD    :0:BOD 発生						*/

	CH_WDTCON  = 0b00001000 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:        :-:								*/
								/*Bit4:WDTPS3  :0:WDT周期						*/
								/*Bit3:WDTPS2  :1:  〃							*/
								/*Bit2:WDTPS1  :0:  〃							*/
								/*Bit1:WDTPS0  :0:  〃							*/
								/*Bit0:SWDTEN  :0:WDTを使用しない				*/

	CH_INTCON  = 0b00000000 ;	/*Bit7:GIE     :0:全割込 禁止					*/
								/*Bit6:PEIE    :0:周辺機能割込 禁止				*/
								/*Bit5:TOIE    :0:TMR0ｵｰﾊﾞｰﾌﾛｰ割込 禁止			*/
								/*Bit4:INTE    :0:INTﾋﾟﾝ割込 禁止				*/
								/*Bit3:GPIE    :0:GPIOﾋﾟﾝ割込 禁止				*/
								/*Bit2:TOIF    :0:TMR0ｵｰﾊﾞｰﾌﾛｰ ﾌﾗｸﾞ				*/
								/*Bit1:INTF    :0:INTﾋﾟﾝ割込 ﾌﾗｸﾞ				*/
								/*Bit0:GPIF    :0:GPIO変化割込 ﾌﾗｸﾞ				*/

	CH_PIR1    = 0b00000000 ;	/*Bit7:EEIE    :0:EEPROM書込み完了割込 禁止		*/
								/*Bit6:ADIE    :0:A/D割込 禁止					*/
								/*Bit5:CCP1IE  :0:CCP1割込 禁止					*/
								/*Bit4:        :-:								*/
								/*Bit3:CMIE    :0:ｺﾝﾊﾟﾚｰﾀ割込 禁止				*/
								/*Bit2:OSFIE   :0:ｵｼﾚｰﾀ異常割込 禁止			*/
								/*Bit1:TMR2IE  :0:ﾀｲﾏｰ2ﾏｯﾁ割込 禁止				*/
								/*Bit0:TMR1IE  :0:ﾀｲﾏｰ1ｵｰﾊﾞｰﾌﾛｰ割込 禁止		*/

//
//    未使用
//
//	CH_CMCON0  = 0b00000000 ;	/*  ｺﾝﾊﾟﾚｰﾀ 未使用								*/
	CH_CMCON1  = 0b00000010 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:        :-:								*/
								/*Bit4:        :-:								*/
								/*Bit3:        :-:								*/
								/*Bit2:        :-:								*/
								/*Bit1:T1GSS   :1:ﾀｲﾏｰ1ｹﾞｰﾄはGP4(DIにする事)	*/
								/*Bit0:CMSYNC  :0:COUTはﾀｲﾏｰ1に同期しない		*/
//	CH_VRCON   = 0b00000000 ;	/*  ｺﾝﾊﾟﾚｰﾀ 未使用								*/
//	CH_ADCON0  = 0b00000000 ;	/*  A/Dｺﾝﾊﾞｰﾀ 未使用							*/
//	CH_EECON1  = 0b00000000 ;	/*  EEPROM 未使用								*/
//								/*  Timer 0 未使用								*/

//
//    ＧＰＩＯ 初期化
//
	CH_GPIO    = 0b00000000 ;	/* GP5～GP0 の出力ﾃﾞｰﾀを一旦"0"にする			*/

	CH_OPTION  = 0b00001111 ;	/*Bit7:*GPPU   :0:弱ﾌﾟﾙｱｯﾌﾟ許可					*/
								/*Bit6:INTEDG  :0:INTﾋﾟﾝ立上がりで割込未使用	*/
								/*Bit5:TOCS    :0:TMR0 内部ｸﾛｯｸ使用				*/
								/*Bit4:TOSE    :0:TMR0 T0CKIﾋﾟﾝ立上りでｲﾝｸﾘﾒﾝﾄ	*/
								/*Bit3:SPA     :1:ﾌﾟﾘｽｹｰﾗはWDTへ割当			*/
								/*Bit2:SP2     :1:ﾌﾟﾘｽｹｰﾗ 1/128					*/
								/*Bit1:SP1     :1:  〃							*/
								/*Bit0:SP0     :1:  〃							*/

	CH_IOC     = 0b00000000 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:IOC5    :0:GP5 割込禁止					*/
								/*Bit4:IOC4    :0:GP4 割込禁止					*/
								/*Bit3:IOC3    :0:GP3 割込禁止					*/
								/*Bit2:IOC2    :0:GP2 割込禁止					*/
								/*Bit1:IOC1    :0:GP1 割込禁止					*/
								/*Bit0:IOC0    :0:GP0 割込禁止					*/

	CH_ANSEL   = 0b00000000 ;	/*Bit7:        :-:								*/
								/*Bit6:ADCS2   :0:A/Dｸﾛｯｸ選択					*/
								/*Bit5:ADCS1   :0:  〃							*/
								/*Bit4:ADCS0   :0:  〃							*/
								/*Bit3:ANS3    :0:GP3 ﾃﾞｼﾞﾀﾙI/O選択				*/
								/*Bit2:ANS2    :0:GP2 ﾃﾞｼﾞﾀﾙI/O選択				*/
								/*Bit1:ANS1    :0:GP1 ﾃﾞｼﾞﾀﾙI/O選択				*/
								/*Bit0:ANS0    :0:GP0 ﾃﾞｼﾞﾀﾙI/O選択				*/

	CH_WPU     = 0b00100000 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:WPU5    :1:GP5 弱ﾌﾟﾙｱｯﾌﾟ使用				*/
								/*Bit4:WPU4    :0:GP4 弱ﾌﾟﾙｱｯﾌﾟ禁止				*/
								/*Bit3:        :-:								*/
								/*Bit2:WPU2    :0:GP2 弱ﾌﾟﾙｱｯﾌﾟ禁止				*/
								/*Bit1:WPU1    :0:GP1 弱ﾌﾟﾙｱｯﾌﾟ禁止				*/
								/*Bit0:WPU0    :0:GP0 弱ﾌﾟﾙｱｯﾌﾟ禁止				*/

	CH_TRISIO  = 0b00111100 ;	/*Bit7:        :-:								*/
								/*Bit6:        :-:								*/
								/*Bit5:GPIO5   :1:GP5 設定ｽｲｯﾁ入力				*/
								/*Bit4:GPIO4   :1:GP4 ﾊﾟﾙｽ入力					*/
								/*Bit3:GPIO3   :1:GP3 出力許可入力				*/
								/*Bit2:GPIO2   :1:GP2 CCP1 PWM (取合えず入力)	*/
								/*Bit1:GPIO1   :0:GP1 FET上段右出力				*/
								/*Bit0:GPIO0   :0:GP0 FET上段左出力				*/

}
