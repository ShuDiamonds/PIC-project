;******************************************
;��������ADCON1�ݒ�
;RA0,RA1��2�`�����l��
;Vref�͊O�����͎͂g�p����
;�N���b�N��Fosc/32
;�f�[�^�~�ϑ҂��^�C�}�[��15sec
;*****************************************
	;delay���C�u����
	;20MHz�̏ꍇ
	;���Ӂ@COUNT01,COUNT02�@�Ƃ����ϐ���錾���Ă���
	;1�T�C�N����4�N���b�N�Ȃ̂� 1 / 20M = 0.000 000 005 = 0.05usec
	;�Ȃ̂� 1�T�C�N����0.2usec�Ȃ̂� 0.2usec * 500 = 100usec 
	;Z�t���O����			
;****************************************
;�`�����l��0��1����1�b���ƂɃf�[�^��ǂݍ��ݑ��M����v���O����
;****************************************
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
AD_Data_h	equ			025H
AD_Data_l	equ			026H


;************************************
		ORG     0x000
		GOTO	INIT
		ORG     0x004
		MOVWF WORK1			;W���W�X�^�̓��e��ޔ�
		BCF		INTCON,GIE	;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)���֎~
		
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

;************AD�ϊ��ݒ�*************************	
		BCF     STATUS,RP1
		BCF     STATUS,RP0		;�o���N�O��
		;Fosc�ݒ�
		;BCF		ADCON0,ADCS1	;FOSC/2
		;BCF		ADCON0,ADCS0
		
		;BCF		ADCON0,ADCS1	;FOSC/8
		;BSF		ADCON0,ADCS0
		
		BSF		ADCON0,ADCS1	;FOSC/32
		BCF		ADCON0,ADCS0
		
		;BSF		ADCON0,ADCS1	;FOSCRC(RC���M)
		;BSF		ADCON0,ADCS0
		;�A�i���O�`�����l���ݒ�
		BCF		ADCON0,CHS2		;channel 0, (RA0/AN0)
		BCF		ADCON0,CHS1
		BCF		ADCON0,CHS0
		;BCF		ADCON0,CHS2		;channel 1, (RA1/AN1)
		;BCF		ADCON0,CHS1
		;BSF		ADCON0,CHS0
		;BCF		ADCON0,CHS2		;channel 2, (RA2/AN2)
		;BSF		ADCON0,CHS1
		;BCF		ADCON0,CHS0
		;BCF		ADCON0,CHS2		;channel 3, (RA3/AN3)
		;BSF		ADCON0,CHS1
		;BSF		ADCON0,CHS0
		;BSF		ADCON0,CHS2		;channel 4, (RA5/AN4)
		;BCF		ADCON0,CHS1
		;BCF		ADCON0,CHS0
		;BSF		ADCON0,CHS2		;channel 5, (RE0/AN5)
		;BCF		ADCON0,CHS1
		;BSF		ADCON0,CHS0
		;BSF		ADCON0,CHS2		;channel 6, (RE1/AN6)
		;BSF		ADCON0,CHS1
		;BCF		ADCON0,CHS0
		;BSF		ADCON0,CHS2		;channel 7, (RE2/AN7)
		;BSF		ADCON0,CHS1
		;BSF		ADCON0,CHS0
		;A/D �|�[�g�\���R���g���[���r�b�g�ݒ�
		BCF     STATUS,RP1		;�o���N�P��
		BSF     STATUS,RP0	
		MOVLW		B'00000010'		;Vdd��GND�̓d������AD�ϊ�����
		MOVWF		ADCON1			;RA0�`3�̓A�i���O���͂��̑��̓f�W�^������
		;�f�[�^�͉E�l�߂łȂ��Ɠǂݍ��߂Ȃ�
		;BCF			ADCON1,ADFM		;�f�[�^�͍��l
		BSF			ADCON1,ADFM		;�f�[�^�͉E�l
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
		;AD�ϊ��`�����l��0
		BCF		ADCON0,CHS2		;channel 0, (RA0/AN0)
		BCF		ADCON0,CHS1
		BCF		ADCON0,CHS0
		BSF		ADCON0,ADON		;�f�[�^�~�ϊJ�n
		CALL	DELAY_15US		;�f�[�^�ϊ��҂�
		BSF		ADCON0,GO		;�f�[�^�𐔒l�ɕϊ�
AD_WAIT	BTFSC	ADCON0,GO
		GOTO	AD_WAIT			;�f�[�^�𐔒l�ɕϊ��҂�
		MOVF	ADRESL,W		;AD�ϊ����ʂWbit�擾
		MOVWF	AD_Data_l
		MOVF	ADRESH,W		;AD�ϊ����2bit�擾
		MOVWF	AD_Data_h
		CALL	TX_CHEACK
		MOVWF	TXREG			;���M
		
		CALL	DELAY_1000MS
		
		;AD�ϊ��`�����l��1
		BCF		ADCON0,CHS2		;channel 1, (RA1/AN1)
		BCF		ADCON0,CHS1
		BSF		ADCON0,CHS0
		BSF		ADCON0,ADON		;�f�[�^�~�ϊJ�n
		CALL	DELAY_15US		;�f�[�^�ϊ��҂�
		BSF		ADCON0,GO		;�f�[�^�𐔒l�ɕϊ�
ADDD	BTFSC	ADCON0,GO
		GOTO	ADDD			;�f�[�^�𐔒l�ɕϊ��҂�
		MOVF	ADRESL,W		;AD�ϊ����ʂWbit�擾
		MOVWF	AD_Data_l
		MOVF	ADRESH,W		;AD�ϊ����2bit�擾
		MOVWF	AD_Data_h
		
		CALL	TX_CHEACK
		MOVWF	TXREG			;���M
		
		CALL	DELAY_1000MS
		
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
;*****delay���[�`��*********
		
DELAY_100US						;�T�C�N��
		MOVLW	D'165'			;1
		MOVWF	COUNT01			;1
LPLPLPLP
		DECFSZ	COUNT01,F		;1*164+2
		GOTO	LPLPLPLP		;2*164
		RETURN					;2
								;�v500 = 100usc
								
;*****delay���[�`��*********
DELAY_15US						;�T�C�N��
		MOVLW	D'23'			;1
		MOVWF	COUNT01			;1
		NOP						;1
T_LP1
		DECFSZ	COUNT01,F		;1*22+2
		GOTO	T_LP1			;2*22
		RETURN					;2
								;�v75 = 100usc
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
		
		MOVLW	D'164'			;1
		MOVWF	COUNT01			;1
		NOP						;1
		NOP						;1
		NOP						;1
MLMLM
		DECFSZ	COUNT01,F		;1*163+2
		GOTO	MLMLM			;2*163
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
		
		MOVLW	D'164'			;1
		MOVWF	COUNT01			;1
		NOP						;1
		NOP						;1
		NOP						;1
AACCAAAA
		DECFSZ	COUNT01,F			;1*163+2
		GOTO	AACCAAAA			;2*163
		RETURN					;2
								;�v900usec + 498 
		
;*********************************************
;1000msecdelay���[�`��
;*********************************************
DELAY_1000MS						;�T�C�N��
		MOVLW	D'99'			;1
		MOVWF	COUNT01			;1
OPOPOPS
		CALL	DELAY_100US		;99��@CALL�����
		CALL	DELAY_10MS		;99��@CALL�����
		DECFSZ	COUNT01,F		;1*98+2
		GOTO	OPOPOPS			;2*98
								;�v298

		MOVLW	D'65'			;1
		MOVWF	COUNT01			;1
		NOP
		NOP
KLPIOJ
		DECFSZ	COUNT01,F		;1*64+2
		GOTO	KLPIOJ			;2*64
		RETURN					;2
								;�v200
								;���v498 + 999.9msec = 1000msec
;************************************
	;RP1	RP0
	; 0		0 �� Bank0
	; 0		1 �� Bank1
	; 1		0 �� Bank2
	; 1		1 �� Bank3
;************************************

;***************���荞�ݏ������[�`��******************


;***********************************
		END