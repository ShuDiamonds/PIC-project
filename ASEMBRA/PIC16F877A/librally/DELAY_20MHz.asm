;**********************************************
;
;
;
;
;**********************************************
;20MHzの場合

;*********************************************
;100usecdelayルーチン
;*********************************************
DELAY_100US						;サイクル
		MOVLW	D'165'			;1
		MOVWF	COUNT01			;1
LPLPLPLP
		DECFSZ	COUNT01,F		;1*164+2
		GOTO	LPLPLPLP		;2*164
		RETURN					;2
								;計500 = 100usc
								
								
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
		
		MOVLW	0xA4			;1
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
		
		MOVLW	0xA4			;1
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
		MOVLW	0x63			;1
		MOVWF	COUNT01			;1
OPOPOPS
		CALL	DELAY_100US		;99回　CALLされる
		CALL	DELAY_10MS		;99回　CALLされる
		DECFSZ	COUNT01,F		;1*98+2
		GOTO	OPOPOPS			;2*98
								;計298

		MOVLW	0x41			;1
		MOVWF	COUNT01			;1
		NOP
		NOP
KLPIOJ
		DECFSZ	COUNT01,F		;1*64+2
		GOTO	KLPIOJ			;2*64
		RETURN					;2
								;計200
								;総計498 + 999.9msec = 1000msec
											