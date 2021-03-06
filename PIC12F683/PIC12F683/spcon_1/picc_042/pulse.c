/********************************************************************************/
/*																				*/
/*			パルス入力 処理														*/
/*																				*/
/*			Cpu         :: PIC12F683											*/
/*			Language    :: PICC Lite											*/
/*			File Name   :: pulse.c												*/
/*																				*/
/********************************************************************************/
#include <pic.h>
#include <conio.h>

#include "pic12f683.h"

void			pulse_init(void);
unsigned int	pulse_read(void);

/****************************************************************************/
/*																			*/
/*	パルス入力 初期化（Timer 1 使用）										*/
/*																			*/
/****************************************************************************/
void pulse_init()
{
	CH_T1CON   = 0b11000101 ;	/*Bit7:T1GINV  :1:ﾀｲﾏｰ1ｹﾞｰﾄ反転 (1ﾚﾍﾞﾙでｱｸﾃｨﾌﾞ)	*/
								/*Bit6:TMR1GE  :1:ﾀｲﾏｰ1 TMR1ON=1でON			*/
								/*Bit5:T1CKPS1 :0:ｸﾛｯｸ ﾌﾟﾘｽｹｰﾗ 1:1				*/
								/*Bit4:T1CKPS0 :0:  〃							*/
								/*Bit3:T1OSCEN :0:LPｵｼﾚｰﾀ off					*/
								/*Bit2:*T1SYNC :1:内部ｸﾛｯｸ選択で無視(非同期)	*/
								/*Bit1:TMR1CS  :0:内部ｸﾛｯｸ選択					*/
								/*Bit0:TMR1ON  :1:ﾀｲﾏｰ1動作						*/

}

/****************************************************************************/
/*																			*/
/*	パルス幅読込															*/
/*																			*/
/****************************************************************************/
static volatile       unsigned int	WD_TMR1		@ ((unsigned)&CH_TMR1L);

unsigned int pulse_read()
{
	unsigned int	wwk ;


	BI_TMR1ON = 0 ;							/* Timer 1 停止						*/

	if(BI_TMR1IF == 0){						/* オーバーフローしてないか？		*/
		wwk = WD_TMR1 ;						/* 値の読み出し						*/
	}else{
		BI_TMR1IF = 0 ;						/* オーバーフロー フラグ クリア		*/
		wwk = 0xffff ;						/* 最大値セット						*/
	}
	WD_TMR1 = 0 ;							/* 次の計測の為 Timer 1 ｸﾘｱ			*/

	BI_TMR1ON = 1 ;							/* Timer 1 動作						*/

	return(wwk) ;
}

