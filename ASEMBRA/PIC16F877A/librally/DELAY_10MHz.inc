
		;delayライブラリ
		;10MHzの場合
		;注意　COUNT01,COUNT02　という変数を宣言しておく
		;1サイクルは4クロックなので 1 / 10M = 0.000 000 1 = 1usec
		;なので 1サイクルは0.4usecなので 0.4usec * 250 = 100usec 
		;Zフラグ注意
		;					
					
DELAY_2US
	NOP				;1
	NOP				;1
	NOP				;1
	RETURN			;2
					;計5
					
DELAY_10US
	CALL	DELAY_2US;2 + 2usec
	CALL	DELAY_2US;2 + 2usec
	CALL	DELAY_2US;2 + 2usec
	NOP				;1
	NOP				;1
	RETURN			;2
					;計10
	
DELAY_100US				;サイクル
		MOVLW	052H	;1
		MOVWF	COUNT01	;1
		NOP				;1
LOOP001
		DECFSZ	COUNT01,F	;1*81+2
		GOTO	LOOP001	;2*81
		RETURN			;2
						;計250
						
						
						
DELAY_1MS				;サイクル
		MOVLW	009H	;1
		MOVWF	COUNT02	;1
LOOP002	
		CALL	DELAY_100US	;2*9+100usec*9
		BCF		STATUS,Z;9*1
		DECFSZ	COUNT02,F	;1*8+2		
		GOTO	LOOP002	;2*8
		
		BCF		STATUS,Z;1
		MOVLW	05DH	;1
		MOVWF	COUNT02	;1
PPPP		
		DECFSZ	COUNT02,F	;1*92+2
		GOTO	PPPP	;1*92
		BCF		STATUS,Z;1
		NOP				;1
		NOP				;1
		NOP				;1
		RETURN			;2
						;計900usec + 250サイクル
