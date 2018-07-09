/********************************************************************************/
/*																				*/
/*			初期化・アイドリング 処理											*/
/*																				*/
/*			Cpu         :: PIC12F683											*/
/*			Language    :: PICC Lite（PIC16F690でｺﾝﾊﾟｲﾙ）						*/
/*			File Name   :: main.c												*/
/*																				*/
/********************************************************************************/

/**************************************************************************************/
/*                                                                                    */
/*  バージョン                                                                        */
/*------------------------------------------------------------------------------------*/
/*  version 0.10  (T.shimakami) 2007-06/18  動作確認 初版(PIC12F675でｺﾝﾊﾟｲﾙ)          */
/*  version 0.20  (T.shimakami) 2007-06/19  ｺﾝﾊﾟｲﾗのﾁｯﾌﾟをPIC16F690に変更             */
/*  version 0.21  (T.shimakami) 2007-06/19  I/O入出力確認                             */
/*  version 0.30  (T.shimakami) 2007-06/20  アプリケーション作製                      */
/*  version 0.40  (T.shimakami) 2007-06/22  処理分割・最適化                          */
/*  version 0.41  (T.shimakami) 2007-06/25  モード変更タイミング見直し                */
/*  version 0.42  (T.shimakami) 2007-06/25  リセット後の不具合修正                    */
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
/*	メイン処理（アイドル）													*/
/*																			*/
/****************************************************************************/
main()
{
	unsigned char	pls_his , pls_bit ;		/* パルス入力ＤＩ					*/
	unsigned char	sw_his , sw_bit ;		/* 設定スイッチ						*/
	unsigned char	di_his , di_bit ;		/* 外部入力ＤＩ						*/

	unsigned char	gate_old ;				/* パルス入力のタイミング確認用		*/
	unsigned int	pulse_data ;			/* パルス幅 0.7～ 1.5～ 2.3 ms		*/
											/* Timer2  1400～3000～4600 ｶｳﾝﾄ	*/
	unsigned int	time_cnt ;				/* パルスのタイムアウト カウンタ	*/
	unsigned char	pnon_cnt ;				/* 初回にパルスを読み捨てる回数		*/
	unsigned char	prg_cnt ;				/* 処理カウンタ						*/

	unsigned char	zoffcnt ;				/* 強制OFFカウント					*/
	unsigned char	dmode ;					/*      1:OFF						*/
											/*      2:前進						*/
											/*      3:後退						*/
											/*      4:ブレーキ					*/
	unsigned char	mmode ;					/* 実際のモード						*/
											/*      1:OFF						*/
											/*      2:前進						*/
											/*      3:後退						*/
											/*      4:ブレーキ					*/
											/* その他:異常値					*/
	unsigned char	power ;					/* PWM値 0～250 指令値				*/

	unsigned char	pwmdo ;					/* PowerMOSFET 上段出力用(GP0,GP0)	*/
	unsigned char	pwr ;					/* PWM値 0～250						*/
	int				drive_data ;			/* モータ駆動データ					*/
	int				motor_data ;			/* モータ出力データ					*/

	int				wk_int ;


//
//	Ｉ／Ｏ 初期化
//
	sys_init() ;							/* リセット後の内部レジスタ初期化	*/
	pulse_init() ;							/* パルス入力用 初期化				*/
	motor_init() ;							/* モータ出力 初期化				*/

//
//	データ＆フラグ 初期化Ｉ／Ｏ
//
	pls_his = 0x07 , pls_bit =1 ;			/* パルス入力ＤＩ“１”セット		*/
	sw_his = 0x07 , sw_bit = 1 ;			/* 設定スイッチ“１”セット			*/
	di_his = 0x07 , di_bit = 1 ;			/* 外部入力ＤＩ“１”セット			*/
	gate_old = di_bit ;						/* 今のゲートの状態					*/

	time_cnt = 0 ;							/* タイムアウト カウンタ クリア		*/
	pnon_cnt = 5 ;							/* パルスデータを５回読み捨てる		*/
	pulse_data = 3000 ;						/* パルス幅 中央値					*/
	drive_data = 0 ;						/* モータ駆動“０”					*/
	motor_data = 0 ;						/* モータ出力“０”					*/
	zoffcnt = 30 ;							/* 強制OFFカウント max				*/
	dmode = mmode = 1 ;						/* モード 1:OFF						*/
	pwmdo = 0x00 ;							/* PowerMOSFET 上段 OFF				*/
	pwr = 0 ;								/* PWM値“０”						*/
	prg_cnt = 0 ;							/* 処理カウンタ“０”				*/

	while(1){
		while(motor_flag() != 1){			/* 120μs 毎のフラグチェック		*/
		}
//
//		CH_GPIO = 0x01 ;					/* 実行時間測定						*/
//

//
//	デジタル入力・設定スイッチ フィルタ処理（１２０μｓ毎の処理）
//
		if(BI_GPIO4 == 1){					/* パルス入力ＤI					*/
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
		if(BI_GPIO5 == 1){					/* 設定スイッチ						*/
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
		if(BI_GPIO3 == 1){					/* 外部入力ＤＩ						*/
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
//	４８０μｓ毎の処理
//
		prg_cnt++ ;
		if(prg_cnt >= 5){
			prg_cnt = 1 ;
		}
		switch (prg_cnt){
//
//	パルス入力処理
//
			case 1 :{
				if((gate_old != 0) && (pls_bit == 0)){	/* ゲートが閉じたか？	*/
					time_cnt = 0 ;				/* タイムアウト カウンタ クリア	*/
					if(pnon_cnt == 0){					/* 通常？				*/
						pulse_data = pulse_read() ;		/* パルス幅を読出す		*/
						if((pulse_data<=1000)||(5000<=pulse_data)){	/* 異常値？	*/
							pulse_data = 3000 ;	/* パルス中央値					*/
							pnon_cnt = 2 ;		/* 復帰時パルスデータ２回読捨て	*/
						}
					}else{
						pnon_cnt-- ;			/* 読み捨てる					*/
						pulse_read() ;			/* ダミーの読出し				*/
					}
				}else{
					time_cnt++ ;
					if(time_cnt > 8333){		/* 約１秒パルスデータ無しか？	*/
						pulse_read() ;			/* ダミーの読出し				*/
						pulse_data = 3000 ;		/* パルス中央値					*/
						pnon_cnt = 2 ;			/* 復帰時パルスデータ２回読捨て	*/
						time_cnt = 0 ;			/* タイムアウト カウンタ クリア	*/
					}
				}
				gate_old = pls_bit ;
			}break ;
//
//	制御中か判定、制御中なら指令値セット
//
			case 2 :{
				if(((sw_bit==1)&&(di_bit==1)) || ((sw_bit==0)&&(di_bit==0))){
					wk_int = 3000 - pulse_data ;	/* パルスを駆動データに変換	*/
//		ヒステリシス処理
					if((wk_int>drive_data+3)||(wk_int<drive_data-3)){
						drive_data = (wk_int + 2) & 0xfffc ;	/* 四捨五入		*/
					}
				}else{
					drive_data = 0 ;			/* 制御OFF処理					*/
				}
			}break ;
//
//	モータ出力データ解析 処理
//
			case 3 :{
//		現在の指令を解析
				motor_data = drive_data ;		/* 出力データ更新				*/
				if ( 1000 < motor_data ) {		/* 上限以上？{(240+10)×4}		*/
					motor_data = 1000 ;			/* 最大値セット					*/
				}
				if ( -1000 > motor_data ) {		/* 下限以下？{(-240-10)×4}		*/
					motor_data = -1000 ;		/* 最小値セット					*/
				}

				if (-43 >= motor_data) {		/* 後退？(-10×4-3以下)			*/
					dmode = 3 ;					/* 後退モード					*/
					power = ((-1 * motor_data) >> 2) - 4 ;	/* 絶対＆8Bit化		*/
				} else {
					if ( 43 <= motor_data ) {	/* 前進？(10×4+3以上)			*/
						dmode = 2 ;				/* 前進モード					*/
						power = (motor_data >> 2) - 4 ;		/* 8Bit化			*/
					} else {					/* 不感帯						*/
						dmode = 1 ;				/* ＯＦＦモード					*/
						power = 0 ;
					} 
				}
//
//		今回はブレーキ設定無し
				if(0){							/* ブレーキ要求有るか？			*/
					dmode = 4 ;					/* ブレーキモード				*/
					power = 250 ;
				}
			}break ;
//
//	モータ出力データ 処理
//
			case 4 :{
//		出力データ作成
				switch( mmode ) {				/* モード ？					*/
					case 1 : {					/* ＯＦＦモード					*/
						pwmdo = 0x00 ;			/* 全FET出力禁止 GP0&1			*/
						pwr = 0 ;				/* PWM出力初期化				*/
						if ( zoffcnt != 0 ) {	/* 強制OFF中？					*/
							zoffcnt-- ;			/* 強制OFFｶｳﾝﾄﾀﾞｳﾝ				*/
						}else{
							mmode = dmode ;		/* モード変更					*/
						}
					} break ;
					case 2 : {					/* 前進モード					*/
						pwmdo = 0x01 ;			/* FET出力前進設定 GP0&1		*/
						if(dmode == 2){			/* 前進モード継続？				*/
							if(pwr < power) pwr++ ;	/* 出力値					*/
							if(pwr > power) pwr-- ;	
						}else{
							if(pwr != 0){
								pwr-- ;			/* 出力値“０”へ				*/
							}else{
								mmode = 1 ;		/* 一旦OFFﾓｰﾄﾞに設定			*/
								zoffcnt = 30 ;	/* 強制OFFｶｳﾝﾀｰ初期化			*/
							}
						}
					} break ;
					case 3 : {					/* 後退モード					*/
						pwmdo = 0x02 ;			/* FET出力後退設定 GP0&1		*/
						if(dmode == 3){			/* 後退モード継続？				*/
							if(pwr < power) pwr++ ;	/* 出力値					*/
							if(pwr > power) pwr-- ;	
						}else{
							if(pwr != 0){
								pwr-- ;			/* 出力値“０”へ				*/
							}else{
								mmode = 1 ;		/* 一旦OFFﾓｰﾄﾞに設定			*/
								zoffcnt = 30 ;	/* 強制OFFｶｳﾝﾀｰ初期化			*/
							}
						}
					} break ;
					case 4 : {					/* ブレーキモード				*/
						pwmdo = 0x03 ;			/* FET出力ﾌﾞﾚｰｷ設定 GP0&1		*/
						if(dmode == 4) {		/* ブレーキ継続？				*/
							if(pwr < power) pwr++ ;	/* 出力値					*/
							if(pwr > power) pwr-- ;	
						}else{
							if(pwr != 0){
								pwr-- ;			/* 出力値“０”へ				*/
							}else{
								mmode = 1 ;		/* 一旦OFFﾓｰﾄﾞに設定			*/
								zoffcnt = 30 ;	/* 強制OFFｶｳﾝﾀｰ初期化			*/
							}
						}
					} break ;
					default : {					/* モード異常値					*/
						mmode = 1 ;				/* 一旦OFFﾓｰﾄﾞに設定			*/
						zoffcnt = 30 ;			/* 強制OFFｶｳﾝﾀｰ初期化			*/
						pwr = 0 ;				/* PWM出力初期化				*/
						pwmdo = 0x00;			/* 全FET出力禁止 GP0&1			*/
					}
				}
//		出力処理
//
//				pwr = pulse_data >> 4  ;		/* パルスのモニタ				*/
//				pwr = (drive_data >> 4) + 128 ;	/* 駆動データのモニタ			*/
//				pwmdo = 0x01 ;					/* 実行時間測定					*/
//
				motor_out(pwmdo,pwr) ;			/* モータ出力					*/
			}break ;
//
//	在り得ないが万が一の処理
//
			default :{
				prg_cnt = 0 ;
			}
		}
//
//		CH_GPIO = 0x00 ;					/* 実行時間測定						*/
//
	}
}

