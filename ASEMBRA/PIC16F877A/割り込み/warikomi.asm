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

WORK1	equ			020H	;割り込み時のメインプログラムのWレジスタの
							;退避場所．
WORK2	equ			021H	;割り込み時のメインプログラムのSTATUSレジスタの
							;退避場所．
COUNT01	equ			022H
COUNT02	equ			023H
DATA_1BYRE	equ		024H
;************************************
		ORG     0x000
		GOTO	INIT
		ORG     0x004
		;GOTO	INTERRUPT

;***************割り込み処理ルーチン******************
INTERRUPT
		
		MOVWF	WORK1				;Wレジスタの内容を退避
		MOVF	STATUS,W			;STATUSをWに移動
		MOVWF	WORK2				;STATUSをWORK2に保存
		;BCF		INTCON,GIE		;グローバル割り込みイネーブルビット(7bit目)を禁止
		
		BTFSC	INTCON,INTF
		CALL	RB0_ISR			;RB0割り込み処理に行く
		BTFSC	INTCON,RBIF
		CALL	RBPORT_ISR		;RBPORT割り込み処理に行く
		BTFSS	PIR1,TXIF		;TXIFは基本０である
		CALL	UART_SEND_ISR	;UART送信割り込み処理に行く
		BTFSC	PIR1,RCIF
		CALL	UART_RECIVE_ISR	;UART受信割り込み処理に行く
		BTFSC	PIR1,ADIF
		CALL	AD_ISR			;AD変換割り込み処理に行く
		BTFSC	PIR1,PSPIF
		CALL	PSP_ISR			;PSP割り込み処理に行く
		
		
		MOVF	WORK2,W			;WORK2(STATUS)をWに移動
		MOVWF	STATUS
		MOVF WORK1, W			;Wレジスタの内容を戻す
		RETFIE
		;RETFIE （割り込みからのReturn）を実行
		;すると，スタックに格納した番地がプログ
		;ラムカウンタに戻され，割り込みがかかっ
		;た時のメインプログラムに戻る
;*********RBPORT割り込みルーチン***********
RBPORT_ISR
		MOVF	PORTB,W			;データ取り出し
		BTFSC	W,4
		GOTO	RB4_ISR			;RB4がhighのときJMP
RB5_CMP	BTFSC	W,5
		GOTO	RB5_ISR			;RB4がhighのときJMP
RB6_CMP	BTFSC	W,6
		GOTO	RB6_ISR			;RB4がhighのときJMP
RB7_CMP	BTFSC	W,7
		GOTO	RB7_ISR			;RB4がhighのときJMP
BR_END	CALL	TX_CHEACK
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
		
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		MOVLW	B'11111110'
		ANDWF	INTCON,F
		;BCF		INTCON,RBIF
		RETURN
RB4_ISR		;RB4がhighのときの動作
		
		GOTO	RB5_CMP
RB5_ISR		;RB5がhighのときの動作
		
		GOTO	RB6_CMP
RB6_ISR		;RB6がhighのときの動作
		
		GOTO	RB7_CMP
RB7_ISR		;RB7がhighのときの動作
		
		GOTO	BR_END
;**********RB0割り込み処理ルーチン*********
RB0_ISR
		
		
		BCF		INTCON,INTF
		RETURN
;*********UART送信割り込みルーチン***********
UART_SEND_ISR
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		
		CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'U'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;送信
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		BCF		PIR1,TXIF
		RETURN


;*********UART受信割り込みルーチン***********
UART_RECIVE_ISR
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		BCF		PORTC,RC3
		
		CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'R'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'E'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'C'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'I'
		MOVWF	TXREG			;送信
		MOVLW	'V'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'E'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK		
		MOVF	RCREG,W
		MOVWF	TXREG						;送信
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;送信
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		BCF		PIR1,RCIF		;受信割り込みフラグクリア
		RETURN
;********AD変換割り込みルーチン**************
AD_ISR
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		BCF		PIR1,ADIF
		RETURN
;********PSP割り込みルーチン*****************
PSP_ISR
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		BCF		PIR1,PSPIF
		RETURN
;***********************************

;***************初期化*********************
INIT
		BCF     STATUS,IRP		;バンク 0,1に指定
		BCF     STATUS,RP1		;この2つでバンク１に
		BSF     STATUS,RP0		;切り替え
		MOVLW	B'10000000'		;RC7をIN
		MOVWF	TRISC			;PORTC
		CLRF	TRISA		
		MOVLW	B'10100001'		;RB5をIN
		MOVWF	TRISB			;PORTN
		CLRF	TRISD
		CLRF	TRISE			;ALL OUT
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		CLRF	PORTA
		;CLRF	PORTB
		;CLRF	PORTC
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
		BSF		INTCON,PEIE					;すべてのマスクされていない周辺機能の割り込みを使用可能にする
	;	BCF		INTCON,PEIE				;すべての周辺機能の割り込みを使用不可にする
;************RB0割り込み設定*************
		
	;	BSF		INTCON,INTE				;RB0/INT 外部割込みイネーブルビット(4bit目)を許可
		BCF		INTCON,INTE				;RB0/INT 外部割込みイネーブルビット(4bit目)を不許可
		BCF     STATUS,RP1		;この2つでバンク１に
		BSF     STATUS,RP0		;切り替え
		BSF		OPTION_REG,INTEDG		;RB0/INT ピンの立ち上がりエッジにより割り込み(6bit目)
		;BCF		OPTION_REG,INTEDG		;RB0/INT ピンの立ち下がりエッジにより割り込み(6bit目)
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
;********RBPORT割り込み設定***************
	;	BSF		INTCON,RBIE				;RB ポート変化割り込みを使用可能にする(3bit目)
		BCF		INTCON,RBIE				;RB ポート変化割り込みを使用不可にする(3bit目)
;********UART送信割り込み設定************
		BCF     STATUS,RP1		;バンク１に
		BSF     STATUS,RP0		;
		;BSF		PIE1,TXIE				;USART 送信割り込みを使用可能にする
		BCF		PIE1,TXIE				;USART 送信割り込みを発生不可にする
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
;********UART受信割り込み設定************
		BCF     STATUS,RP1		;バンク１に
		BSF     STATUS,RP0		;
	;	BSF		PIE1,RCIE			;USART 受信割り込みを使用可能にする
		BCF		PIE1,RCIE				;USART 受信割り込みを発生不可にする
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
;********AD変換割り込み設定**************
		BCF     STATUS,RP1		;バンク１に
		BSF     STATUS,RP0		;
		;BSF		PIE1,ADIE			;AD コンバータ割り込みを使用可能にする
		BCF		PIE1,ADIE			;AD コンバータ割り込みを発生不可にする
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
;********************PSP(パラレルスレーブポートリード／ライト割り込み)割り込み設定*
		BCF     STATUS,RP1		;バンク１に
		BSF     STATUS,RP0		;
		;BSF		PIE1,PSPIE			;PSP リード／ライト割り込みを使用可能にする
		BCF		PIE1,PSPIE				;PSP リード／ライト割り込みを発生不可にする
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
;*********メイン関数****************
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		
		MOVLW	'\n'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
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
		
		BSF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を許可
		BSF		INTCON,RBIE				;RB ポート変化割り込みを使用可能にする(3bit目)
		
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
	;RP1	RP0
	; 0		0 → Bank0
	; 0		1 → Bank1
	; 1		0 → Bank2
	; 1		1 → Bank3
;************************************

		END