;******************************************
;約1秒ごとに割り込みを入れRC3のLEDをチカチカさせる
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

BUFFER	equ			020H
COUNT01	equ			021H
COUNT02	equ			022H
DATA_1BYRE	equ		023H
WORK1	equ			024H	;割り込み時のメインプログラムのデータの
							;退避場所．
TIME_COUNT01	equ	025H	;タイマー0用カウンタ


;************************************
		ORG     0x000
		GOTO	INIT
		ORG     0x004
		MOVWF WORK1			;Wレジスタの内容を退避
		BCF		INTCON,GIE	;グローバル割り込みイネーブルビット(7bit目)を禁止
		
		CALL	Timer0_ISR		;Timer0割り込み処理に行く
		
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
		;BSF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を許可
		BCF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を不許可
;************Timer0割り込み設定*********
;カウント値の計算についてはデータシート参照
;***************************************
		BCF     STATUS,RP1		;バンク１に
		BSF     STATUS,RP0		
		;Timer0のクロック選択
		BSF		OPTION_REG,T0CS		;T0CKIピンからクロック入力
		BCF		OPTION_REG,T0CS		;内部クロックからクロック入力
		;TMR0の入力エッジ選択
		BSF		OPTION_REG,T0SE		;立ち上がりエッジ
		BCF		OPTION_REG,T0SE		;立ち下がりエッジ
		;プリスケーラ切り替え選択
		BSF		OPTION_REG,PSA		;WDTに使う
		BCF		OPTION_REG,PSA		;Timer0に使う
		;pプリスケーラのスケール値設定
		MOVLW	B'00000111'		;1:256に設定
		IORWF	OPTION_REG
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		BCF		INTCON,T0IE		;タイマー0割り込み不許可
		MOVLW	D'177'			;カウント値のロード
		MOVWF	TMR0			;TMR0に出力
		
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
		
		BSF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を許可
		BSF		INTCON,T0IE		;タイマー0割り込み許可
		MOVLW	D'250'			
		MOVWF	TIME_COUNT01
		BSF		PORTC,RC3
		
MAINLOOP						
		NOP
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
		MOVLW	D'78'			;1
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

;**********Timer0割り込み処理ルーチン*********
Timer0_ISR
		MOVLW	D'177'			;カウント値のロード
		MOVWF	TMR0			;TMR0に出力
		BCF		INTCON,T0IF
		;Timer0割り込み処理
		DECFSZ	TIME_COUNT01,F		;
		GOTO	END_POINT
		GOTO	Time1_ROTIN			;
END_POINT
		RETURN
		
Time1_ROTIN
		MOVLW	D'250'			
		MOVWF	TIME_COUNT01
		MOVLW	B'00001000'
		XORWF	PORTC
		RETURN
;***********************************
;***********************************
		END