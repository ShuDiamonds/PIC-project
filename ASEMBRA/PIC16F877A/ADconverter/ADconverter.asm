;******************************************
;初期化でADCON1設定
;RA0,RA1の2チャンネル
;Vrefは外部入力は使用せず
;クロックはFosc/32
;データ蓄積待ちタイマーは15sec
;*****************************************
	;delayライブラリ
	;20MHzの場合
	;注意　COUNT01,COUNT02　という変数を宣言しておく
	;1サイクルは4クロックなので 1 / 20M = 0.000 000 005 = 0.05usec
	;なので 1サイクルは0.2usecなので 0.2usec * 500 = 100usec 
	;Zフラグ注意			
;****************************************
;チャンネル0と1から1秒ごとにデータを読み込み送信するプログラム
;****************************************
		list	P=PIC16F877A	
		#include	<P16F877A.inc>
		
		errorlevel  -302 
		
		__CONFIG	_HS_OSC & _WDT_OFF & _PWRTE_ON & _BODEN_OFF & _LVP_ON & _CPD_OFF & _WRT_OFF & _DEBUG_OFF  & _CP_OFF
		
;******************************
	;変数定義
;******************************

BUFFER	equ			020H
COUNT01	equ			021H
COUNT02	equ			022H
DATA_1BYRE	equ		023H
WORK1	equ			024H	;割り込み時のメインプログラムのデータの
							;退避場所．
AD_Data_h	equ			025H
AD_Data_l	equ			026H


;************************************
		ORG     0x000
		GOTO	INIT
		ORG     0x004
		MOVWF WORK1			;Wレジスタの内容を退避
		BCF		INTCON,GIE	;グローバル割り込みイネーブルビット(7bit目)を禁止
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		MOVF WORK1, W		;Wレジスタの内容を戻す
		RETFIE
	;RETFIE （割り込みからのReturn）を実行
	;すると，スタックに格納した番地がプログ
	;ラムカウンタに戻され，割り込みがかかっ
	;た時のメインプログラムに戻る
;***************初期化*********************
INIT
		BCF     STATUS,IRP		;バンク 0,1に指定
		BCF     STATUS,RP1		;この2つでバンク１に
		BSF     STATUS,RP0		;切り替え
		MOVLW	B'10000000'		;RC7をIN
		MOVWF	TRISC			;PORTC
		CLRF	TRISA		
		MOVLW	B'00100111'		;RB5をIN
		MOVWF	TRISB			;PORT
		CLRF	TRISD
		CLRF	TRISE			;ALL OUT
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		CLRF	PORTA
		CLRF	PORTB
		CLRF	PORTC
		CLRF	PORTD
		CLRF	PORTE
;*****************************
	;送信モードの初期化
	;
	;Clock	10MHz	20MHz
	;Baud	SPBRG	SPBRG
	;1200	81H		FFH
	;2400	40H		81H
	;9600	0FH		20H
	;19K	07H		0FH
;*****************************
;**************UART設定************************
		BCF     STATUS,RP1		;バンク１に
		BSF     STATUS,RP0		
		MOVLW	020H			;RC6をTXモードに
		MOVWF	TXSTA			;SET
		;ボーレート設定	;低速モード
		MOVLW	20H				;9600BPS
		;MOVLW	0FH				;19.2KBPS
		MOVWF	SPBRG			;SET BRG
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		MOVLW	090H
		MOVWF	RCSTA			;RC7をRXに　連続受信を許可 ８BIT通信
;************割り込み設定***********************
		;BSF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を許可
		BCF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を不許可

;************AD変換設定*************************	
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		;Fosc設定
		;BCF		ADCON0,ADCS1	;FOSC/2
		;BCF		ADCON0,ADCS0
		
		;BCF		ADCON0,ADCS1	;FOSC/8
		;BSF		ADCON0,ADCS0
		
		BSF		ADCON0,ADCS1	;FOSC/32
		BCF		ADCON0,ADCS0
		
		;BSF		ADCON0,ADCS1	;FOSCRC(RC発信)
		;BSF		ADCON0,ADCS0
		;アナログチャンネル設定
		BCF		ADCON0,CHS2		;channel 0, (RA0/AN0)
		BCF		ADCON0,CHS1
		BCF		ADCON0,CHS0
		;BCF		ADCON0,CHS2		;channel 1, (RA1/AN1)
		;BCF		ADCON0,CHS1
		;BSF		ADCON0,CHS0
		;BCF		ADCON0,CHS2		;channel 2, (RA2/AN2)
		;BSF		ADCON0,CHS1
		;BCF		ADCON0,CHS0
		;BCF		ADCON0,CHS2		;channel 3, (RA3/AN3)
		;BSF		ADCON0,CHS1
		;BSF		ADCON0,CHS0
		;BSF		ADCON0,CHS2		;channel 4, (RA5/AN4)
		;BCF		ADCON0,CHS1
		;BCF		ADCON0,CHS0
		;BSF		ADCON0,CHS2		;channel 5, (RE0/AN5)
		;BCF		ADCON0,CHS1
		;BSF		ADCON0,CHS0
		;BSF		ADCON0,CHS2		;channel 6, (RE1/AN6)
		;BSF		ADCON0,CHS1
		;BCF		ADCON0,CHS0
		;BSF		ADCON0,CHS2		;channel 7, (RE2/AN7)
		;BSF		ADCON0,CHS1
		;BSF		ADCON0,CHS0
		;A/D ポート構成コントロールビット設定
		BCF     STATUS,RP1		;バンク１に
		BSF     STATUS,RP0	
		MOVLW		B'00000010'		;VddとGNDの電圧差をAD変換する
		MOVWF		ADCON1			;RA0～3はアナログ入力その他はデジタル入力
		;データは右詰めでないと読み込めない
		;BCF			ADCON1,ADFM		;データは左詰
		BSF			ADCON1,ADFM		;データは右詰
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
;*********メイン関数****************
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'S'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'T'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'A'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'R'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'T'
		MOVWF	TXREG			;送信
		MOVLW	'\r'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
MAINLOOP						
		BSF		PORTC,RC3
		NOP
		;AD変換チャンネル0
		BCF		ADCON0,CHS2		;channel 0, (RA0/AN0)
		BCF		ADCON0,CHS1
		BCF		ADCON0,CHS0
		BSF		ADCON0,ADON		;データ蓄積開始
		CALL	DELAY_15US		;データ変換待ち
		BSF		ADCON0,GO		;データを数値に変換
AD_WAIT	BTFSC	ADCON0,GO
		GOTO	AD_WAIT			;データを数値に変換待ち
		MOVF	ADRESL,W		;AD変換下位８bit取得
		MOVWF	AD_Data_l
		MOVF	ADRESH,W		;AD変換上位2bit取得
		MOVWF	AD_Data_h
		CALL	TX_CHEACK
		MOVWF	TXREG			;送信
		
		CALL	DELAY_1000MS
		
		;AD変換チャンネル1
		BCF		ADCON0,CHS2		;channel 1, (RA1/AN1)
		BCF		ADCON0,CHS1
		BSF		ADCON0,CHS0
		BSF		ADCON0,ADON		;データ蓄積開始
		CALL	DELAY_15US		;データ変換待ち
		BSF		ADCON0,GO		;データを数値に変換
ADDD	BTFSC	ADCON0,GO
		GOTO	ADDD			;データを数値に変換待ち
		MOVF	ADRESL,W		;AD変換下位８bit取得
		MOVWF	AD_Data_l
		MOVF	ADRESH,W		;AD変換上位2bit取得
		MOVWF	AD_Data_h
		
		CALL	TX_CHEACK
		MOVWF	TXREG			;送信
		
		CALL	DELAY_1000MS
		
		GOTO	MAINLOOP
;*****送信完了待ちループルーチン******

TX_CHEACK
		BCF     STATUS,RP1		;この2つでバンク１に
		BSF     STATUS,RP0		;切り替え

TXLOOP		
		BTFSS	TXSTA,TRMT		;送信バッファが空か？
		GOTO	TXLOOP
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		RETURN
;*****delayルーチン*********
		
DELAY_100US						;サイクル
		MOVLW	D'165'			;1
		MOVWF	COUNT01			;1
LPLPLPLP
		DECFSZ	COUNT01,F		;1*164+2
		GOTO	LPLPLPLP		;2*164
		RETURN					;2
								;計500 = 100usc
								
;*****delayルーチン*********
DELAY_15US						;サイクル
		MOVLW	D'23'			;1
		MOVWF	COUNT01			;1
		NOP						;1
T_LP1
		DECFSZ	COUNT01,F		;1*22+2
		GOTO	T_LP1			;2*22
		RETURN					;2
								;計75 = 100usc
;*****delayルーチン*********
		
DELAY_595US						;サイクル
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		MOVLW	D'156'			;1
		MOVWF	COUNT01			;1
LLBAL
		DECFSZ	COUNT01,F		;1*155+2
		GOTO	LLBAL			;2*155
		RETURN					;2
								;計475 = 95usc + 500sec
								;	   = 595usec
								
;*****delayルーチン*********
NOP_10
		
		NOP						;1
		NOP						;1
		NOP						;1
		NOP						;1
		NOP						;1
		NOP						;1
		RETURN					;2
								;計10

;************************************
;*********************************************
;1msecdelayルーチン
;*********************************************
DELAY_1MS						;サイクル
		
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		
		MOVLW	D'164'			;1
		MOVWF	COUNT01			;1
		NOP						;1
		NOP						;1
		NOP						;1
MLMLM
		DECFSZ	COUNT01,F		;1*163+2
		GOTO	MLMLM			;2*163
		RETURN					;2
								;計900usec + 498 
								
;*********************************************
;10msecdelayルーチン
;*********************************************
DELAY_10MS						;サイクル
		
		CALL	DELAY_1MS
		CALL	DELAY_1MS
		CALL	DELAY_1MS
		CALL	DELAY_1MS
		CALL	DELAY_1MS
		CALL	DELAY_1MS
		CALL	DELAY_1MS
		CALL	DELAY_1MS
		CALL	DELAY_1MS
		
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		
		MOVLW	D'164'			;1
		MOVWF	COUNT01			;1
		NOP						;1
		NOP						;1
		NOP						;1
AACCAAAA
		DECFSZ	COUNT01,F			;1*163+2
		GOTO	AACCAAAA			;2*163
		RETURN					;2
								;計900usec + 498 
		
;*********************************************
;1000msecdelayルーチン
;*********************************************
DELAY_1000MS						;サイクル
		MOVLW	D'99'			;1
		MOVWF	COUNT01			;1
OPOPOPS
		CALL	DELAY_100US		;99回　CALLされる
		CALL	DELAY_10MS		;99回　CALLされる
		DECFSZ	COUNT01,F		;1*98+2
		GOTO	OPOPOPS			;2*98
								;計298

		MOVLW	D'65'			;1
		MOVWF	COUNT01			;1
		NOP
		NOP
KLPIOJ
		DECFSZ	COUNT01,F		;1*64+2
		GOTO	KLPIOJ			;2*64
		RETURN					;2
								;計200
								;総計498 + 999.9msec = 1000msec
;************************************
	;RP1	RP0
	; 0		0 → Bank0
	; 0		1 → Bank1
	; 1		0 → Bank2
	; 1		1 → Bank3
;************************************

;***************割り込み処理ルーチン******************


;***********************************
		END