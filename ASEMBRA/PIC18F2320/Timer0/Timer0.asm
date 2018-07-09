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
				list	P=PIC18F2320	
		#include	<P18F2320.inc>
		
		errorlevel  -302 
	
	CONFIG	OSC = HSPLL
	CONFIG	FSCM = OFF
	CONFIG	IESO = OFF
	CONFIG	PWRT = OFF
	CONFIG	BOR = OFF
	CONFIG	BORV = 20
	CONFIG	WDT = OFF
	CONFIG	WDTPS = 1
	CONFIG	MCLRE = ON
	CONFIG	PBAD = DIG
	;CONFIG	PBAD = ANA
	;CONFIG	CCP2MX = C1
	CONFIG	STVR = ON
	CONFIG	LVP = ON
	CONFIG	DEBUG = OFF
	CONFIG	CP0 = OFF
	CONFIG	CP1 = OFF
	CONFIG	CP2 = OFF
	CONFIG	CP3 = OFF
	CONFIG	CPB = OFF
	CONFIG	CPD = OFF
	CONFIG	WRT0 = OFF
	CONFIG	WRT1 = OFF
	CONFIG	WRT2 = OFF
	CONFIG	WRT3 = OFF
	CONFIG	WRTB = OFF
	CONFIG	WRTC = OFF
	CONFIG	WRTD = OFF
	CONFIG	EBTR0 = OFF
	CONFIG	EBTR1 = OFF
	CONFIG	EBTR2 = OFF
	CONFIG	EBTR3 = OFF
	CONFIG	EBTRB = OFF
;******************************
	;�ϐ���`
;******************************
	CBLOCK		020H
	BUFFER				;020�Ԓn
	COUNT01			;021�Ԓn
	COUNT02			;022�Ԓn
	DATA_1BYRE			;023�Ԓn
	WREG_TMP		;���荞�ݎ��̃��C���v���O�����̃f�[�^��
	STATUS_TMP							;�ޔ��ꏊ�D
	BSR_TMP
	PORTB_DATA		;RB���荞�݂̑ޔ��ꏊ
	ENDC

;************************************
		ORG		0
		BRA		INIT
		;���ʊ��荞�݃x�N�^
		ORG     0x008
		GOTO	HIGH_ISR
		RETFIE
		;��ʊ��荞�݃x�N�^
		ORG     0x018
		GOTO	LOW_ISR
	;RETFIE �i���荞�݂����Return�j�����s
	;����ƁC�X�^�b�N�Ɋi�[�����Ԓn���v���O
	;�����J�E���^�ɖ߂���C���荞�݂�������
	;�����̃��C���v���O�����ɖ߂�
;********������*********************
INIT
		MOVLW		B'00001111'		;���ׂăf�W�^�����[�h
		MOVWF		ADCON1			;
		CLRF		TRISA			;
		MOVLW		B'11111111'		;
		MOVWF		TRISB			
		MOVLW		B'10111111'		;
		MOVWF		TRISC			
;**************UART�ݒ�************************
		MOVLW	B'00100000'			;RC6��TX���[�h��
		MOVWF	TXSTA			;SET
		;�{�[���[�g�ݒ�	;�ᑬ���[�h
		MOVLW	D'64'				;9600BPS
		;MOVLW	D'32'				;19.2KBPS
		MOVWF	SPBRG			;SET BRG
		MOVLW	B'10010000'
		MOVWF	RCSTA			;RC7��RX�Ɂ@�A����M������ �WBIT�ʐM
;************���荞�ݐݒ�***********************
		BSF		RCON,IPEN		;���荞�݂ɗD�揇�ʂ��g��
	;	BCF		RCON,IPEN		;���荞�݂ɗD�揇�ʂ��g��Ȃ�
	
	;	BSF		INTCON,GIE				;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)������		���ʊ��荞�݋���
		BCF		INTCON,GIE				;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)��s����		���ʊ��荞�ݕs����
		
	;	BSF		INTCON,PEIE				;���ׂẴ}�X�N����Ă��Ȃ����Ӌ@�\�̊��荞�݂��g�p�\�ɂ���	��ʊ��荞�݋���
		BCF		INTCON,PEIE				;���ׂĂ̎��Ӌ@�\�̊��荞�݂��g�p�s�ɂ���							��ʊ��荞�݋���
;************RB0���荞�ݐݒ�********
		;RB0/INT �͊��荞�݂ɗD�揇�ʂ͂Ȃ����ʒ�ʂ̗����Ɋ��荞�݂�������
	;	BSF		INTCON,INT0IE			;RB0/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
		BCF		INTCON,INT0IE			;RB0/INT �O�������݃C�l�[�u���r�b�g(4bit��)��s����
	;	BSF		INTCON2,INTEDG0			;RB0/INT �s���̗����オ��G�b�W�ɂ�芄�荞��(6bit��)
		BCF		INTCON2,INTEDG0			;RB0/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)
;************RB1���荞�ݐݒ�*********
		BSF		INTCON3,INT1IP			;RB1/INT ���ʊ��荞�݂ɂ���
	;	BCF		INTCON3,INT1IP			;RB1/INT ��ʊ��荞�݂ɂ���
	;	BSF		INTCON3,INT1IE			;RB1/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
		BCF		INTCON3,INT1IE			;RB1/INT �O�������݃C�l�[�u���r�b�g(4bit��)��s����
	;	BSF		INTCON2,INTEDG1			;RB1/INT �s���̗����オ��G�b�W�ɂ�芄�荞��(6bit��)
		BCF		INTCON2,INTEDG1			;RB1/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)
;************RB2���荞�ݐݒ�*********
		BSF		INTCON3,INT2IP			;RB2/INT ���ʊ��荞�݂ɂ���
	;	BCF		INTCON3,INT2IP			;RB2/INT ��ʊ��荞�݂ɂ���
	;	BSF		INTCON3,INT2IE			;RB2/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
		BCF		INTCON3,INT2IE			;RB2/INT �O�������݃C�l�[�u���r�b�g(4bit��)��s����
	;	BSF		INTCON2,INTEDG2			;RB2/INT �s���̗����オ��G�b�W�ɂ�芄�荞��(6bit��)
		BCF		INTCON2,INTEDG2			;RB2/INT �s���̗���������G�b�W�ɂ�芄�荞��(6bit��)
;************RBPORT���荞�ݐݒ�*********
	;	BSF		INTCON2,RBIP			;RB2/INT ���ʊ��荞�݂ɂ���
		BCF		INTCON2,RBIP			;RB2/INT ��ʊ��荞�݂ɂ���
	;	BSF		INTCON,RBIE			;RB2/INT �O�������݃C�l�[�u���r�b�g(4bit��)������
		BCF		INTCON,RBIE			;RB2/INT �O�������݃C�l�[�u���r�b�g(4bit��)��s����
;************Timer0���荞�ݐݒ�********
		CLRF	T0CON
		BSF		INTCON2,TMR0IP			;Timer0 ���ʊ��荞�݂ɂ���
	;	BCF		INTCON2,TMR0IP			;Timer0 ��ʊ��荞�݂ɂ���
		BSF		INTCON,TMR0IE			;Timer0 �O�������݃C�l�[�u���r�b�g������
	;	BCF		INTCON,TMR0IE			;Timer0 �O�������݃C�l�[�u���r�b�g��s����
		BSF		T0CON,TMR0ON			;Timer0 ���g��
	;	BCF		T0CON,TMR0ON			;Timer0 ���g��Ȃ�
	;	BSF		T0CON,T08BIT			;8bit���[�h
		BCF		T0CON,T08BIT			;16bit���[�h
	;	BSF		T0CON,T0CS				;T0CKI�s�� �̓��͂��N���b�N�Ƃ���
		BCF		T0CON,T0CS				;�����N���b�N ���N���b�N�Ƃ���
	;	BSF		T0CON,T0SE				;�����オ��G�b�W
		BCF		T0CON,T0SE				;����������G�b�W
	;	BSF		T0CON,PSA				;�v���X�P�[���̕s�g�p
		BCF		T0CON,PSA				;�v���X�P�[���̎g�p
		MOVLW	B'00000111'				;�v���X�P�[����256�ɂ���
		;MOVLW	B'00000110'				;�v���X�P�[����128�ɂ���
		;MOVLW	B'00000100'				;�v���X�P�[����32�ɂ���
		;MOVLW	B'00000000'				;�v���X�P�[����2�ɂ���
		IORWF	T0CON
		;�J�E���g�l�̃��[�h
		MOVLW	H'67'
		MOVWF	TMR0H
		MOVLW	H'6A'
		MOVWF	TMR0L
;*********���C���֐�****************
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
;*******���荞�݋���**********
		BSF		INTCON,PEIE				;���ׂẴ}�X�N����Ă��Ȃ����Ӌ@�\�̊��荞�݂��g�p�\�ɂ���	��ʊ��荞�݋���
		BSF		INTCON,GIE				;�O���[�o�����荞�݃C�l�[�u���r�b�g(7bit��)������		���ʊ��荞�݋���
;*****************************
MAINLOOP
		NOP
		GOTO	MAINLOOP
		
;************���M�����҂����[�v**************
TX_CHEACK
		BTFSS	TXSTA,TRMT		;���M�o�b�t�@���󂩁H
		GOTO	TX_CHEACK
		RETURN
		
;*************���ʊ��荞�ݕ���***************
HIGH_ISR
		BTFSC	INTCON,TMR0IF
		RCALL	Timer0_ISR
		BTFSC	INTCON,INT0IF
		RCALL	RB0_ISR
		BTFSC	INTCON3,INT1IF
		RCALL	RB1_ISR
		BTFSC	INTCON3,INT2IF
		RCALL	RB2_ISR
		
		RETFIE
;*************��ʊ��荞�ݕ���***************
LOW_ISR
		MOVWF	WREG_TMP
		MOVFF	STATUS,STATUS_TMP							;�ޔ�
		MOVFF	BSR,BSR_TMP
		
		BTFSC	INTCON,RBIF
		RCALL	RB_PORT_ISR
		
		MOVF	WREG_TMP,W
		MOVFF	STATUS_TMP,STATUS							;�ޔ�
		MOVFF	BSR_TMP,BSR
		RETFIE
;*************���荞�ݏ���***************
;*********Timer0���荞�ݏ���******
Timer0_ISR
		;�J�E���g�l�̃��[�h
		MOVLW	H'67'
		MOVWF	TMR0H
		MOVLW	H'6A'
		MOVWF	TMR0L
		BCF		INTCON,TMR0IF
		MOVLW	B'00111100'
		XORWF	PORTA
		RETURN
;**********RB0���荞��*************
RB0_ISR		
		BCF		INTCON,INT0IF	;���荞�݃t���O�N���A
		
		
		RETURN
;**********RB1���荞��*************
RB1_ISR		
		BCF		INTCON3,INT1IF	;���荞�݃t���O�N���A
		
		
		RETURN
;**********RB2���荞��*************
RB2_ISR		
		BCF		INTCON3,INT2IF	;���荞�݃t���O�N���A
		
		
		RETURN
;**********RBPORT���荞��*************
RB_PORT_ISR
		MOVF	PORTB,W
		MOVWF	PORTB_DATA
		BCF		INTCON,RBIF	;���荞�݃t���O�N���A
		BTFSC	PORTB_DATA,RB4
		GOTO	RB4_ISR			;RB4��high�̂Ƃ�JMP
RB5_CMP	BTFSC	PORTB_DATA,RB5
		GOTO	RB5_ISR			;RB4��high�̂Ƃ�JMP
RB6_CMP	BTFSC	PORTB_DATA,RB6
		GOTO	RB6_ISR			;RB4��high�̂Ƃ�JMP
RB7_CMP	BTFSC	PORTB_DATA,RB7
		GOTO	RB7_ISR			;RB4��high�̂Ƃ�JMP
RB_END	CALL	TX_CHEACK
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
		RETURN
RB4_ISR		;RB4��high�̂Ƃ��̓���
		NOP
		GOTO	RB5_CMP
RB5_ISR		;RB5��high�̂Ƃ��̓���
		NOP
		GOTO	RB6_CMP
RB6_ISR		;RB6��high�̂Ƃ��̓���
		NOP
		GOTO	RB7_CMP
RB7_ISR		;RB7��high�̂Ƃ��̓���
		NOP
		GOTO	RB_END
;*****************************
		END

