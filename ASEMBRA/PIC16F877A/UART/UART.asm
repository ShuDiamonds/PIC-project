
;***�@UART���M�v���O�����@***
;1�������ł����炻��𑗂�Ԃ��v���O����
;�A�{�J�h�̓���m�FLED��RC�O�ɂȂ����Ă���
;
;	����	movlw	�̓��e�����f�[�^��w���W�X�^�ɏ�������
;
;****************************



		list	P=PIC16F877A	
		#include	<P16F877A.inc>
		
		errorlevel  -302 
		
		__CONFIG	_HS_OSC & _WDT_OFF & _PWRTE_ON & _BODEN_OFF & _LVP_ON & _CPD_OFF & _WRT_OFF & _DEBUG_OFF  & _CP_OFF
		
;******************************
;�ϐ���`
;******************************

BUFFER	equ		020H
WORK	equ		021H
COUNT01	equ		022H

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

		ORG     0x000
		goto	INIT
		ORG     0x004
		return
		
INIT
		BCF     STATUS,RP1		;����2�Ńo���N�P��
		BSF     STATUS,RP0		;�؂�ւ�
		MOVLW	B'10000000'		;RC7��IN
		MOVWF	TRISC			;PORTC
		CLRF	TRISA		
		MOVLW	B'00100111'		;RB5��IN
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
;**************UART�ݒ�************************
		BCF     STATUS,RP1		;�o���N�P��
		BSF     STATUS,RP0		
		MOVLW	B'00100000'			;RC6��TX���[�h��
		MOVWF	TXSTA			;SET
		;�{�[���[�g�ݒ�	;�ᑬ���[�h
		MOVLW	D'32'				;9600BPS
		;MOVLW	D'15'				;19.2KBPS
		MOVWF	SPBRG			;SET BRG
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		MOVLW	B'10010000'
		MOVWF	RCSTA			;RC7��RX�Ɂ@�A����M������ �WBIT�ʐM
		
		

MAIN	
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
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;���M
	
		BSF		PORTC,0			;����m�FLED��ON
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
RXLOOP		

		BTFSS	PIR1,RCIF		;RX�t���O��1��?
		GOTO	RXLOOP
		BCF		PORTC,0			;����m�FLED��OFF
		BCF		PIR1,RCIF		;RX�t���O��0�ɂ���
		

		;MOVLW	'H'
		;MOVWF	TXREG			;���M
		
		
;****�G���[�`�F�b�N*******
;
;�G���[�ɂ���
;�@�@�t���[�~���O�G���[�@�c�c�@�X�g�b�v�s�b�g���O�ɂȂ��Ă���ꍇ�̃G���[
;�A�@�I�[�o�[�����G���[�c�c�@�O�̃f�[�^�����o����Ȃ������Ɏ��̃f�[�^�����Ă��܂����ꍇ�̃G���[
;�B�@�p���e�B�G���[�@�@�@�c�c�@�p���e�B�`�F�b�N�Ō��o���ꂽ�G���[
;*************************
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		BTFSC	RCSTA,FERR		;�t���[�~���O�G���[�����邩�H
		GOTO	FRAME
		BTFSC	RCSTA,OERR		;�I�o�[�����G���[�����邩�H
		GOTO	OVER
		
		
;*******�o�b�t�@�[�Ɋi�[

INTOBUFFER
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		MOVF	RCREG,W			
		MOVWF	BUFFER			;BUFFER�Ɏ�M�f�[�^���i�[
		CALL	TX_CHEACK
		MOVF	BUFFER,W
		MOVWF	TXREG			;���M
		
		CALL	TX_CHEACK
		MOVF	BUFFER,W
		MOVWF	TXREG			;���M
		
		;CALL	TX_CHEACK
		;MOVLW	'B'
		;MOVWF	TXREG			;���M
		
		GOTO	SEND
	

;*****ERROR PROCES*********

;***�t���[�~���O�G���[****

FRAME	
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		MOVF	RCREG,W			;�_�~�[�����o��
		CALL	TX_CHEACK
		MOVLW	'F'
		MOVWF	TXREG			;���M
		
		BTFSS	RCSTA,OERR		;�I�[�o�[�G���[�����邩�H
		GOTO	RXLOOP
		
;***�I�[�o�[�G���[*******
		
OVER
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		BCF		RCSTA,CREN		;�A����M��s����
		BSF		RCSTA,CREN		;�A����M������
		CALL	TX_CHEACK
		MOVLW	'O'
		MOVWF	TXREG			;���M
		
		GOTO	RXLOOP
		
;************���M*************
		
SEND	

		;CALL	TX_CHEACK
		;MOVLW	'C'
		;MOVWF	TXREG			;���M
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		CALL	TX_CHEACK
		MOVF	BUFFER,W
		MOVWF	TXREG			;���M
		
		;RETURN
		
		;CALL	TX_CHEACK
		;MOVLW	'S'
		;MOVWF	TXREG			;���M
		
		MOVF	RCREG,W
		GOTO	RXLOOP
		
		
;************���M�����҂����[�v**************

TX_CHEACK
		BCF     STATUS,RP1		;����2�Ńo���N�P��
		BSF     STATUS,RP0		;�؂�ւ�

TXLOOP		
		BTFSS	TXSTA,TRMT		;���M�o�b�t�@���󂩁H
		GOTO	TXLOOP
		
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		RETURN
		
;**************END*****************
		
		END

;************************************
;RP1	RP0
; 0		0 �� Bank0
; 0		1 �� Bank1
; 1		0 �� Bank2
; 1		1 �� Bank3
;
;
;
;
;
;
;************************************