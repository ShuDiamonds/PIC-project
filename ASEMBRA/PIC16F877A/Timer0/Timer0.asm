;******************************************
;��1�b���ƂɊ��荞�݂����RC3��LED���`�J�`�J������
;*****************************************
	;delay���C�u����
	;20MHz�̏ꍇ
	;���Ӂ@COUNT01,COUNT02�@�Ƃ����ϐ���錾���Ă���
	;1�T�C�N����4�N���b�N�Ȃ̂� 1 / 20M = 0.000 000 005 = 0.05usec
	;�Ȃ̂� 1�T�C�N����0.2usec�Ȃ̂� 0.2usec * 500 = 100usec 
	;Z�t���O����			
;****************************************/
		list	P=PIC16F877A	
		#include	<P16F877A.inc>
		
		errorlevel  -302 
		
		__CONFIG	_HS_OSC & _WDT_OFF & _PWRTE_ON & _BODEN_OFF & _LVP_ON & _CPD_OFF & _WRT_OFF & _DEBUG_OFF  & _CP_OFF
		
;******************************
	;�ϐ���`
;******************************

BUFFER	equ			020H
COUNT01	equ			021H
COUNT02	equ			022H
DATA_1BYRE	equ		023H
WORK1	equ			024H	;���荞�ݎ��̃��C���v���O�����̃f�[�^��
							;�ޔ��ꏊ�D
TIME_COUNT01	equ	025H	;�^�C�}�[0�p�J�E���^


;************************************
		ORG     0x000
		GOTO	INIT
		ORG     0x004
		MOVWF WORK1			;W���W�X�^�̓��e��ޔ�
		BCF		INTCON,GIE	;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)���֎~
		
		CALL	Timer0_ISR		;Timer0���荞�ݏ����ɍs��
		
		BCF		INTCON,GIE	;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)������
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		MOVF WORK1, W		;W���W�X�^�̓��e��߂�
		RETFIE
	;RETFIE �i���荞�݂����Return�j�����s
	;����ƁC�X�^�b�N�Ɋi�[�����Ԓn���v���O
	;�����J�E���^�ɖ߂���C���荞�݂�������
	;�����̃��C���v���O�����ɖ߂�
;***************������*********************
INIT
		BCF     STATUS,IRP		;�o���N 0,1�Ɏw��
		BCF     STATUS,RP1		;����2�Ńo���N�P��
		BSF     STATUS,RP0		;�؂�ւ�
		MOVLW	B'10000000'		;RC7��IN
		MOVWF	TRISC			;PORTC
		CLRF	TRISA		
		MOVLW	B'00100111'		;RB5��IN
		MOVWF	TRISB			;PORT
		CLRF	TRISD
		CLRF	TRISE			;ALL OUT
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		CLRF	PORTA
		CLRF	PORTB
		CLRF	PORTC
		CLRF	PORTD
		CLRF	PORTE
;*****************************
	;���M���[�h�̏�����
	;
	;Clock	10MHz	20MHz
	;Baud	SPBRG	SPBRG
	;1200	81H		FFH
	;2400	40H		81H
	;9600	0FH		20H
	;19K	07H		0FH
;*****************************
;**************UART�ݒ�************************
		BCF     STATUS,RP1		;�o���N�P��
		BSF     STATUS,RP0		
		MOVLW	020H			;RC6��TX���[�h��
		MOVWF	TXSTA			;SET
		;�{�[���[�g�ݒ�	;�ᑬ���[�h
		MOVLW	20H				;9600BPS
		;MOVLW	0FH				;19.2KBPS
		MOVWF	SPBRG			;SET BRG
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		MOVLW	090H
		MOVWF	RCSTA			;RC7��RX�Ɂ@�A����M������ �WBIT�ʐM
;************���荞�ݐݒ�***********************
		;BSF		INTCON,GIE				;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)������
		BCF		INTCON,GIE				;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)��s����
;************Timer0���荞�ݐݒ�*********
;�J�E���g�l�̌v�Z�ɂ��Ă̓f�[�^�V�[�g�Q��
;***************************************
		BCF     STATUS,RP1		;�o���N�P��
		BSF     STATUS,RP0		
		;Timer0�̃N���b�N�I��
		BSF		OPTION_REG,T0CS		;T0CKI�s������N���b�N����
		BCF		OPTION_REG,T0CS		;�����N���b�N����N���b�N����
		;TMR0�̓��̓G�b�W�I��
		BSF		OPTION_REG,T0SE		;�����オ��G�b�W
		BCF		OPTION_REG,T0SE		;����������G�b�W
		;�v���X�P�[���؂�ւ��I��
		BSF		OPTION_REG,PSA		;WDT�Ɏg��
		BCF		OPTION_REG,PSA		;Timer0�Ɏg��
		;p�v���X�P�[���̃X�P�[���l�ݒ�
		MOVLW	B'00000111'		;1:256�ɐݒ�
		IORWF	OPTION_REG
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		BCF		INTCON,T0IE		;�^�C�}�[0���荞�ݕs����
		MOVLW	D'177'			;�J�E���g�l�̃��[�h
		MOVWF	TMR0			;TMR0�ɏo��
		
;*********���C���֐�****************
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'S'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'T'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'A'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'R'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'T'
		MOVWF	TXREG			;���M
		MOVLW	'\r'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		
		BSF		INTCON,GIE				;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)������
		BSF		INTCON,T0IE		;�^�C�}�[0���荞�݋���
		MOVLW	D'250'			
		MOVWF	TIME_COUNT01
		BSF		PORTC,RC3
		
MAINLOOP						
		NOP
		NOP
		GOTO	MAINLOOP
;*****���M�����҂����[�v���[�`��******

TX_CHEACK
		BCF     STATUS,RP1		;����2�Ńo���N�P��
		BSF     STATUS,RP0		;�؂�ւ�

TXLOOP		
		BTFSS	TXSTA,TRMT		;���M�o�b�t�@���󂩁H
		GOTO	TXLOOP
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		RETURN
;*****delay���[�`��*********/
		
DELAY_100US						;�T�C�N��
		MOVLW	D'165'			;1
		MOVWF	COUNT01			;1
LPLPLPLP
		DECFSZ	COUNT01,F		;1*164+2
		GOTO	LPLPLPLP		;2*164
		RETURN					;2
								;�v500 = 100usc
								
;*****delay���[�`��*********
		
DELAY_595US						;�T�C�N��
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		CALL	DELAY_100US
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		MOVLW	D'78'			;1
		MOVWF	COUNT01			;1
LLBAL
		DECFSZ	COUNT01,F		;1*155+2
		GOTO	LLBAL			;2*155
		RETURN					;2
								;�v475 = 95usc + 500sec
								;	   = 595usec
								
;*****delay���[�`��*********
NOP_10
		
		NOP						;1
		NOP						;1
		NOP						;1
		NOP						;1
		NOP						;1
		NOP						;1
		RETURN					;2
								;�v10

;************************************
;************************************
	;RP1	RP0
	; 0		0 �� Bank0
	; 0		1 �� Bank1
	; 1		0 �� Bank2
	; 1		1 �� Bank3
;************************************

;***************���荞�ݏ������[�`��******************

;**********Timer0���荞�ݏ������[�`��*********
Timer0_ISR
		MOVLW	D'177'			;�J�E���g�l�̃��[�h
		MOVWF	TMR0			;TMR0�ɏo��
		BCF		INTCON,T0IF
		;Timer0���荞�ݏ���
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