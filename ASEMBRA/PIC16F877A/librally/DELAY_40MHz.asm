;**********************************************
;40MHz�̏ꍇ

;*********************************************
;100usecdelay���[�`��
;*********************************************
DELAY_100US						;�T�C�N��
		MOVLW	D'165'			;1
		MOVWF	COUNT01			;1
LPLPLPLP
		DECFSZ	COUNT01,F		;1*164+2
		GOTO	LPLPLPLP		;2*164
MOVLW	D'165'			;1
		MOVWF	COUNT01			;1
sss
		DECFSZ	COUNT01,F		;1*164+2
		GOTO	sss				;2*164
		NOP
		NOP
		RETURN					;2
								;�v500 = 100usc
								
								
;*********************************************
;1msecdelay���[�`��
;*********************************************
DELAY_1MS						;�T�C�N��
		
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
		MOVLW	0xA4			;1
		MOVWF	COUNT01			;1
		NOP						;1
		NOP						;1
		NOP						;1
DSD
		DECFSZ	COUNT01,F		;1*163+2
		GOTO	DSD			;2*163
		NOP
		NOP
		RETURN					;2
								;�v900usec + 498 
								
;*********************************************
;10msecdelay���[�`��
;*********************************************
DELAY_10MS						;�T�C�N��
		
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
		MOVLW	0xA4			;1
		MOVWF	COUNT01			;1
		NOP						;1
		NOP						;1
		NOP						;1
fafff
		DECFSZ	COUNT01,F			;1*163+2
		GOTO	fafff			;2*163
		NOP
		NOP
		RETURN					;2
								;�v900usec + 498 
		
;*********************************************
;1000msecdelay���[�`��
;*********************************************
DELAY_1000MS						;�T�C�N��
		MOVLW	0x63			;1
		MOVWF	COUNT02			;1
OPOPOPS
		CALL	DELAY_100US		;99��@CALL�����
		CALL	DELAY_10MS		;99��@CALL�����
		DECFSZ	COUNT01,F		;1*98+2
		GOTO	OPOPOPS			;2*98
								;�v298

		MOVLW	0x41			;1
		MOVWF	COUNT01			;1
		NOP
		NOP
KLPIOJ
		DECFSZ	COUNT02,F		;1*64+2
		GOTO	KLPIOJ			;2*64
		
		MOVLW	0x63			;1
		MOVWF	COUNT02			;1
DWDWDDS
		CALL	DELAY_100US		;99��@CALL�����
		CALL	DELAY_10MS		;99��@CALL�����
		DECFSZ	COUNT01,F		;1*98+2
		GOTO	DWDWDDS			;2*98
								;�v298

		MOVLW	0x41			;1
		MOVWF	COUNT01			;1
		NOP
		NOP
FEFEEW
		DECFSZ	COUNT02,F		;1*64+2
		GOTO	FEFEEW			;2*64
		RETURN					;2
								;�v200
								;���v498 + 999.9msec = 1000msec
											