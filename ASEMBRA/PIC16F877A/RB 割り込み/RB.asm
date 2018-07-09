;******************************************

;*****************************************
	;delayライブラリ
	;20MHzの場合
	;注意　COUNT01,COUNT02　という変数を宣言しておく
	;1サイクルは4クロックなので 1 / 20M = 0.000 000 005 = 0.05usec
	;なので 1サイクルは0.2usecなので 0.2usec * 500 = 100usec 
	;Zフラグ注意			
;****************************************/
		list	P=PIC16F877A	
		#include	<P16F877A.inc>
		
		errorlevel  -302 
		
		__CONFIG	_HS_OSC & _WDT_OFF & _PWRTE_ON & _BODEN_OFF & _LVP_ON & _CPD_OFF & _WRT_OFF & _DEBUG_OFF  & _CP_OFF
		
;******************************
	;変数定義
;******************************
	CBLOCK		020H
	BUFFER				;020番地
	COUNT01			;021番地
	COUNT02			;022番地
	DATA_1BYRE			;023番地
	WORK1		;割り込み時のメインプログラムのデータの
								;退避場所．
	ENDC

;************************************
		ORG     0x000
		GOTO	INIT
		ORG     0x004
		MOVWF WORK1			;Wレジスタの内容を退避
		BCF		INTCON,GIE	;グローバル割り込みイネーブルビット(7bit目)を禁止
		CALL	RB0_ISR		;RB0割り込み処理に行く
		BCF		INTCON,GIE	;グローバル割り込みイネーブルビット(7bit目)を許可
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
		BSF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を許可
		;BCF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を不許可
;************RB0割り込み設定*********
		
		BSF		INTCON,INTE				;RB0/INT 外部割込みイネーブルビット(4bit目)を許可
		;BCF		INTCON,INTE				;RB0/INT 外部割込みイネーブルビット(4bit目)を不許可
		BCF     STATUS,RP1		;この2つでバンク１に
		BSF     STATUS,RP0		;切り替え
		;BSF		OPTION_REG,INTEDG		;RB0/INT ピンの立ち上がりエッジにより割り込み(6bit目)
		BCF		OPTION_REG,INTEDG		;RB0/INT ピンの立ち下がりエッジにより割り込み(6bit目)
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
;*****delayルーチン*********/
		
DELAY_100US						;サイクル
		MOVLW	D'165'			;1
		MOVWF	COUNT01			;1
LPLPLPLP
		DECFSZ	COUNT01,F		;1*164+2
		GOTO	LPLPLPLP		;2*164
		RETURN					;2
								;計500 = 100usc
								
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
;************************************
	;RP1	RP0
	; 0		0 → Bank0
	; 0		1 → Bank1
	; 1		0 → Bank2
	; 1		1 → Bank3
;************************************

;***************割り込み処理ルーチン******************

;**********RB0割り込み処理ルーチン*********
RB0_ISR
		
		BCF		INTCON,INTF		;割り込みフラグクリア
		BCF		PORTC,RC3
		CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'R'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'B'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;送信
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
		CALL	DELAY_595US
				RETURN
;***********************************
;***********************************
		END