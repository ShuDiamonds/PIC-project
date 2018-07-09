;******************************************

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
	CBLOCK		020H
	BUFFER				;020�Ԓn
	COUNT01			;021�Ԓn
	COUNT02			;022�Ԓn
	DATA_1BYRE			;023�Ԓn
	WORK1		;���荞�ݎ��̃��C���v���O�����̃f�[�^��
								;�ޔ��ꏊ�D
	ENDC

;************************************
		ORG     0x000
		GOTO	INIT
		ORG     0x004
		MOVWF WORK1			;W���W�X�^�̓��e��ޔ�
		BCF		INTCON,GIE	;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)���֎~
		CALL	RB0_ISR		;RB0���荞�ݏ����ɍs��
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
		BSF		INTCON,GIE				;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)������
		;BCF		INTCON,GIE				;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)��s����
;************RB0���荞�ݐݒ�*********
		
		BSF		INTCON,INTE				;RB0/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
		;BCF		INTCON,INTE				;RB0/INT �O�������݃C�l�[�u���r�b�g(4bit��)��s����
		BCF     STATUS,RP1		;����2�Ńo���N�P��
		BSF     STATUS,RP0		;�؂�ւ�
		;BSF		OPTION_REG,INTEDG		;RB0/INT �s���̗����オ��G�b�W�ɂ�芄�荞��(6bit��)
		BCF		OPTION_REG,INTEDG		;RB0/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		
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
MAINLOOP						
		BSF		PORTC,RC3
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
		MOVLW	D'156'			;1
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

;**********RB0���荞�ݏ������[�`��*********
RB0_ISR
		
		BCF		INTCON,INTF		;���荞�݃t���O�N���A
		BCF		PORTC,RC3
		CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'R'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'B'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;���M
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