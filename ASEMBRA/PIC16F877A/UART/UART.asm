
;***　UART送信プログラム　***
;1文字飛んできたらそれを送り返すプログラム
;アボカドの動作確認LEDはRC０につながっている
;
;	注意	movlw	はリテラルデータをwレジスタに書き込む
;
;****************************



		list	P=PIC16F877A	
		#include	<P16F877A.inc>
		
		errorlevel  -302 
		
		__CONFIG	_HS_OSC & _WDT_OFF & _PWRTE_ON & _BODEN_OFF & _LVP_ON & _CPD_OFF & _WRT_OFF & _DEBUG_OFF  & _CP_OFF
		
;******************************
;変数定義
;******************************

BUFFER	equ		020H
WORK	equ		021H
COUNT01	equ		022H

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

		ORG     0x000
		goto	INIT
		ORG     0x004
		return
		
INIT
		BCF     STATUS,RP1		;この2つでバンク１に
		BSF     STATUS,RP0		;切り替え
		MOVLW	B'10000000'		;RC7をIN
		MOVWF	TRISC			;PORTC
		CLRF	TRISA		
		MOVLW	B'00100111'		;RB5をIN
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
;**************UART設定************************
		BCF     STATUS,RP1		;バンク１に
		BSF     STATUS,RP0		
		MOVLW	B'00100000'			;RC6をTXモードに
		MOVWF	TXSTA			;SET
		;ボーレート設定	;低速モード
		MOVLW	D'32'				;9600BPS
		;MOVLW	D'15'				;19.2KBPS
		MOVWF	SPBRG			;SET BRG
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		MOVLW	B'10010000'
		MOVWF	RCSTA			;RC7をRXに　連続受信を許可 ８BIT通信
		
		

MAIN	
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
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;送信
	
		BSF		PORTC,0			;動作確認LEDをON
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
RXLOOP		

		BTFSS	PIR1,RCIF		;RXフラグが1か?
		GOTO	RXLOOP
		BCF		PORTC,0			;動作確認LEDをOFF
		BCF		PIR1,RCIF		;RXフラグを0にする
		

		;MOVLW	'H'
		;MOVWF	TXREG			;送信
		
		
;****エラーチェック*******
;
;エラーについて
;①　フレーミングエラー　……　ストップピットが０になっている場合のエラー
;②　オーバーランエラー……　前のデータが取り出されないうちに次のデータが来てしまった場合のエラー
;③　パリティエラー　　　……　パリティチェックで検出されたエラー
;*************************
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		BTFSC	RCSTA,FERR		;フレーミングエラーがあるか？
		GOTO	FRAME
		BTFSC	RCSTA,OERR		;オバーランエラーがあるか？
		GOTO	OVER
		
		
;*******バッファーに格納

INTOBUFFER
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		MOVF	RCREG,W			
		MOVWF	BUFFER			;BUFFERに受信データを格納
		CALL	TX_CHEACK
		MOVF	BUFFER,W
		MOVWF	TXREG			;送信
		
		CALL	TX_CHEACK
		MOVF	BUFFER,W
		MOVWF	TXREG			;送信
		
		;CALL	TX_CHEACK
		;MOVLW	'B'
		;MOVWF	TXREG			;送信
		
		GOTO	SEND
	

;*****ERROR PROCES*********

;***フラーミングエラー****

FRAME	
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		MOVF	RCREG,W			;ダミーを取り出す
		CALL	TX_CHEACK
		MOVLW	'F'
		MOVWF	TXREG			;送信
		
		BTFSS	RCSTA,OERR		;オーバーエラーもあるか？
		GOTO	RXLOOP
		
;***オーバーエラー*******
		
OVER
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		BCF		RCSTA,CREN		;連続受信を不許可
		BSF		RCSTA,CREN		;連続受信を許可
		CALL	TX_CHEACK
		MOVLW	'O'
		MOVWF	TXREG			;送信
		
		GOTO	RXLOOP
		
;************送信*************
		
SEND	

		;CALL	TX_CHEACK
		;MOVLW	'C'
		;MOVWF	TXREG			;送信
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		CALL	TX_CHEACK
		MOVF	BUFFER,W
		MOVWF	TXREG			;送信
		
		;RETURN
		
		;CALL	TX_CHEACK
		;MOVLW	'S'
		;MOVWF	TXREG			;送信
		
		MOVF	RCREG,W
		GOTO	RXLOOP
		
		
;************送信完了待ちループ**************

TX_CHEACK
		BCF     STATUS,RP1		;この2つでバンク１に
		BSF     STATUS,RP0		;切り替え

TXLOOP		
		BTFSS	TXSTA,TRMT		;送信バッファが空か？
		GOTO	TXLOOP
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;バンク０に
		RETURN
		
;**************END*****************
		
		END

;************************************
;RP1	RP0
; 0		0 → Bank0
; 0		1 → Bank1
; 1		0 → Bank2
; 1		1 → Bank3
;
;
;
;
;
;
;************************************