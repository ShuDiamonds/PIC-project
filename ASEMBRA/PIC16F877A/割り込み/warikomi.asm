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

WORK1	equ			020H	;���荞�ݎ��̃��C���v���O������W���W�X�^��
							;�ޔ��ꏊ�D
WORK2	equ			021H	;���荞�ݎ��̃��C���v���O������STATUS���W�X�^��
							;�ޔ��ꏊ�D
COUNT01	equ			022H
COUNT02	equ			023H
DATA_1BYRE	equ		024H
;************************************
		ORG     0x000
		GOTO	INIT
		ORG     0x004
		;GOTO	INTERRUPT

;***************���荞�ݏ������[�`��******************
INTERRUPT
		
		MOVWF	WORK1				;W���W�X�^�̓��e��ޔ�
		MOVF	STATUS,W			;STATUS��W�Ɉړ�
		MOVWF	WORK2				;STATUS��WORK2�ɕۑ�
		;BCF		INTCON,GIE		;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)���֎~
		
		BTFSC	INTCON,INTF
		CALL	RB0_ISR			;RB0���荞�ݏ����ɍs��
		BTFSC	INTCON,RBIF
		CALL	RBPORT_ISR		;RBPORT���荞�ݏ����ɍs��
		BTFSS	PIR1,TXIF		;TXIF�͊�{�O�ł���
		CALL	UART_SEND_ISR	;UART���M���荞�ݏ����ɍs��
		BTFSC	PIR1,RCIF
		CALL	UART_RECIVE_ISR	;UART��M���荞�ݏ����ɍs��
		BTFSC	PIR1,ADIF
		CALL	AD_ISR			;AD�ϊ����荞�ݏ����ɍs��
		BTFSC	PIR1,PSPIF
		CALL	PSP_ISR			;PSP���荞�ݏ����ɍs��
		
		
		MOVF	WORK2,W			;WORK2(STATUS)��W�Ɉړ�
		MOVWF	STATUS
		MOVF WORK1, W			;W���W�X�^�̓��e��߂�
		RETFIE
		;RETFIE �i���荞�݂����Return�j�����s
		;����ƁC�X�^�b�N�Ɋi�[�����Ԓn���v���O
		;�����J�E���^�ɖ߂���C���荞�݂�������
		;�����̃��C���v���O�����ɖ߂�
;*********RBPORT���荞�݃��[�`��***********
RBPORT_ISR
		MOVF	PORTB,W			;�f�[�^���o��
		BTFSC	W,4
		GOTO	RB4_ISR			;RB4��high�̂Ƃ�JMP
RB5_CMP	BTFSC	W,5
		GOTO	RB5_ISR			;RB4��high�̂Ƃ�JMP
RB6_CMP	BTFSC	W,6
		GOTO	RB6_ISR			;RB4��high�̂Ƃ�JMP
RB7_CMP	BTFSC	W,7
		GOTO	RB7_ISR			;RB4��high�̂Ƃ�JMP
BR_END	CALL	TX_CHEACK
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
		
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		MOVLW	B'11111110'
		ANDWF	INTCON,F
		;BCF		INTCON,RBIF
		RETURN
RB4_ISR		;RB4��high�̂Ƃ��̓���
		
		GOTO	RB5_CMP
RB5_ISR		;RB5��high�̂Ƃ��̓���
		
		GOTO	RB6_CMP
RB6_ISR		;RB6��high�̂Ƃ��̓���
		
		GOTO	RB7_CMP
RB7_ISR		;RB7��high�̂Ƃ��̓���
		
		GOTO	BR_END
;**********RB0���荞�ݏ������[�`��*********
RB0_ISR
		
		
		BCF		INTCON,INTF
		RETURN
;*********UART���M���荞�݃��[�`��***********
UART_SEND_ISR
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		
		CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'U'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;���M
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		BCF		PIR1,TXIF
		RETURN


;*********UART��M���荞�݃��[�`��***********
UART_RECIVE_ISR
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		BCF		PORTC,RC3
		
		CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'R'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'E'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'C'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'I'
		MOVWF	TXREG			;���M
		MOVLW	'V'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
		MOVLW	'E'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK		
		MOVF	RCREG,W
		MOVWF	TXREG						;���M
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;���M
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		BCF		PIR1,RCIF		;��M���荞�݃t���O�N���A
		RETURN
;********AD�ϊ����荞�݃��[�`��**************
AD_ISR
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		BCF		PIR1,ADIF
		RETURN
;********PSP���荞�݃��[�`��*****************
PSP_ISR
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		BCF		PIR1,PSPIF
		RETURN
;***********************************

;***************������*********************
INIT
		BCF     STATUS,IRP		;�o���N 0,1�Ɏw��
		BCF     STATUS,RP1		;����2�Ńo���N�P��
		BSF     STATUS,RP0		;�؂�ւ�
		MOVLW	B'10000000'		;RC7��IN
		MOVWF	TRISC			;PORTC
		CLRF	TRISA		
		MOVLW	B'10100001'		;RB5��IN
		MOVWF	TRISB			;PORTN
		CLRF	TRISD
		CLRF	TRISE			;ALL OUT
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		CLRF	PORTA
		;CLRF	PORTB
		;CLRF	PORTC
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
		BSF		INTCON,PEIE					;���ׂẴ}�X�N����Ă��Ȃ����Ӌ@�\�̊��荞�݂��g�p�\�ɂ���
	;	BCF		INTCON,PEIE				;���ׂĂ̎��Ӌ@�\�̊��荞�݂��g�p�s�ɂ���
;************RB0���荞�ݐݒ�*************
		
	;	BSF		INTCON,INTE				;RB0/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
		BCF		INTCON,INTE				;RB0/INT �O�������݃C�l�[�u���r�b�g(4bit��)��s����
		BCF     STATUS,RP1		;����2�Ńo���N�P��
		BSF     STATUS,RP0		;�؂�ւ�
		BSF		OPTION_REG,INTEDG		;RB0/INT �s���̗����オ��G�b�W�ɂ�芄�荞��(6bit��)
		;BCF		OPTION_REG,INTEDG		;RB0/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
;********RBPORT���荞�ݐݒ�***************
	;	BSF		INTCON,RBIE				;RB �|�[�g�ω����荞�݂��g�p�\�ɂ���(3bit��)
		BCF		INTCON,RBIE				;RB �|�[�g�ω����荞�݂��g�p�s�ɂ���(3bit��)
;********UART���M���荞�ݐݒ�************
		BCF     STATUS,RP1		;�o���N�P��
		BSF     STATUS,RP0		;
		;BSF		PIE1,TXIE				;USART ���M���荞�݂��g�p�\�ɂ���
		BCF		PIE1,TXIE				;USART ���M���荞�݂𔭐��s�ɂ���
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
;********UART��M���荞�ݐݒ�************
		BCF     STATUS,RP1		;�o���N�P��
		BSF     STATUS,RP0		;
	;	BSF		PIE1,RCIE			;USART ��M���荞�݂��g�p�\�ɂ���
		BCF		PIE1,RCIE				;USART ��M���荞�݂𔭐��s�ɂ���
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
;********AD�ϊ����荞�ݐݒ�**************
		BCF     STATUS,RP1		;�o���N�P��
		BSF     STATUS,RP0		;
		;BSF		PIE1,ADIE			;AD �R���o�[�^���荞�݂��g�p�\�ɂ���
		BCF		PIE1,ADIE			;AD �R���o�[�^���荞�݂𔭐��s�ɂ���
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
;********************PSP(�p�������X���[�u�|�[�g���[�h�^���C�g���荞��)���荞�ݐݒ�*
		BCF     STATUS,RP1		;�o���N�P��
		BSF     STATUS,RP0		;
		;BSF		PIE1,PSPIE			;PSP ���[�h�^���C�g���荞�݂��g�p�\�ɂ���
		BCF		PIE1,PSPIE				;PSP ���[�h�^���C�g���荞�݂𔭐��s�ɂ���
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
;*********���C���֐�****************
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		
		MOVLW	'\n'
		MOVWF	TXREG			;���M
		CALL	TX_CHEACK
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
		
		BSF		INTCON,GIE				;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)������
		BSF		INTCON,RBIE				;RB �|�[�g�ω����荞�݂��g�p�\�ɂ���(3bit��)
		
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
	;RP1	RP0
	; 0		0 �� Bank0
	; 0		1 �� Bank1
	; 1		0 �� Bank2
	; 1		1 �� Bank3
;************************************

		END