
		;delay���C�u����
		;10MHz�̏ꍇ
		;���Ӂ@COUNT01,COUNT02�@�Ƃ����ϐ���錾���Ă���
		;1�T�C�N����4�N���b�N�Ȃ̂� 1 / 10M = 0.000 000 1 = 1usec
		;�Ȃ̂� 1�T�C�N����0.4usec�Ȃ̂� 0.4usec * 250 = 100usec 
		;Z�t���O����
		;					
					
DELAY_2US
	NOP				;1
	NOP				;1
	NOP				;1
	RETURN			;2
					;�v5
					
DELAY_10US
	CALL	DELAY_2US;2 + 2usec
	CALL	DELAY_2US;2 + 2usec
	CALL	DELAY_2US;2 + 2usec
	NOP				;1
	NOP				;1
	RETURN			;2
					;�v10
	
DELAY_100US				;�T�C�N��
		MOVLW	052H	;1
		MOVWF	COUNT01	;1
		NOP				;1
LOOP001
		DECFSZ	COUNT01,F	;1*81+2
		GOTO	LOOP001	;2*81
		RETURN			;2
						;�v250
						
						
						
DELAY_1MS				;�T�C�N��
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
						;�v900usec + 250�T�C�N��