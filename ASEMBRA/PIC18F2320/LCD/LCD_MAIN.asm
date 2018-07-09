
		list	P=PIC18F2320	
		#include	<P18F2320.inc>
	;	#include	<LCDLIB18.asm>
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
	CBLOCK		000H
	BUFFER
	COUNT01	
	COUNT02
	DATA_1BYRE	
	WORK1	;���荞�ݎ��̃��C���v���O�����̃f�[�^��
							;�ޔ��ꏊ�D
	;*****LCD�̕ϐ���`�Ǝg�p�|�[�g��`
	;LCD�̕ϐ���`
	CNT1
	CNT2
	DPDT
	DISP
	DPTEMP
	DPADR
	NUML
	CNT3
	TEMP
	
	ENDC
	;�g�p�|�[�g��`
	#DEFINE	LCDRS	PORTB,2
	#DEFINE	LCDSTB	PORTB,3
	#DEFINE	LCDDB	PORTB
;************************************
		ORG		0
		BRA		INIT
		;���ʊ��荞�݃x�N�^
		ORG     0x008
		
		;��ʊ��荞�݃x�N�^
		ORG     0x018
		
;************���M�����҂����[�v**************

TX_CHEACK
		BTFSS	TXSTA,TRMT		;���M�o�b�t�@���󂩁H
		GOTO	TX_CHEACK
		RETURN
;*****************************
;********������*********************
INIT
		MOVLW		B'00001111'		;���ׂăf�W�^�����[�h
		MOVWF		ADCON1			;
		CLRF		TRISA			;
		CLRF		TRISB
		CLRF		PORTB
		MOVLW		B'11111111'		;
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
;*********************************************
		;****LCD������
		CALL	TX_CHEACK
		MOVLW	'L'
		MOVWF	TXREG			;���M
		
		
		CALL		LCD_INI
		
		CALL	TX_CHEACK
		MOVLW	'C'
		MOVWF	TXREG			;���M
		
				MOVLW		'F'
		CALL		LCD_DATA
		CALL		DELAY_1000MS
		
		
		CALL	TX_CHEACK
		MOVLW	'D'
		MOVWF	TXREG			;���M
		
		CALL		LCD_CLR
		
		CALL	TX_CHEACK
		MOVLW	'L'
		MOVWF	TXREG			;���M
		
		
		
		
;*********���C���֐�****************
MAINLOOP
		
		;start���b�Z�[�W�\��
		MOVLW		H'C0'
		CALL		LCD_CMD
		MOVLW		2
		RCALL		LCDMSG
		CLRF		NUML
LOOP
		;***Number���b�Z�[�W�\��
		MOVLW		H'C0'
		CALL		LCD_CMD
		MOVLW		2
		RCALL		LCDMSG
		;****���l�̕\��
		MOVF		NUML,W
		CALL		LCDHEX2
		RCALL		DELAY_1000MS
		;RCALL		TIME100M
		;RCALL		TIME100M
		;**���l�̃J�E���g�A�b�v
		INCF		NUML,F
		BNZ			LOOP
		;***END���b�Z�[�W�̕\��
		MOVLW		H'02'
		CALL		LCD_CMD
		MOVLW		1
		RCALL		LCDMSG
		RCALL		DELAY_1000MS
		;***�S����
		CALL		LCD_CLR
		BRA			MAINLOOP
;******************************	
;*�@16�i���Q���\���֐�
;*�@�@W���W�X�^�ɕ\�����鐔�l
;*
;******************************
LCDHEX2
	MOVWF	DPTEMP		;�ꎞ�ۑ�
	SWAPF	DPTEMP,W		;��ʑ�
	ANDLW	0FH
	RCALL	TOHEX
	RCALL	LCD_DATA		;�\���o��
	MOVF	DPTEMP,W		;���ʑ�
	ANDLW	0FH
	RCALL	TOHEX
	RCALL	LCD_DATA		;�\���o��
	RETURN
;***** �o�C�i������16�i��ASCII�ϊ��֐�
TOHEX
	SUBLW	9		;9-W
	BTFSS	STATUS,C
	BRA	TOH21
	SUBLW	039H
	RETURN
TOH21
	SUBLW	0FFH
	ADDLW	041H
	RETURN

;*************************************
;*   ������\���֐�
;*		W���W�X�^�Ƀ��b�Z�[�W�ԍ�
;*************************************
LCDMSG		;***�e�[�u���|�C���^�擾
		MOVWF		TEMP
		MOVLW	LOW	MSGPTR
		MOVWF		TBLPTRL
		MOVLW	HIGH	MSGPTR
		MOVWF		TBLPTRH
		BCF			STATUS,C
		RLCF		TEMP,W
		ADDWFC		TBLPTRL,F
		MOVLW		0
		ADDWFC		TBLPTRH,F
		CLRF		TBLPTRU
		TBLRD		*+
		MOVFF		TABLAT,TEMP
		TBLRD		*+
		MOVFF		TABLAT,TBLPTRH
		MOVFF		TEMP,TBLPTRL
MsgLoop	;***�����̎�o��
		TBLRD		*+
		MOVF		TABLAT,W
		BTFSC		STATUS,Z
		RETURN
		RCALL		LCD_DATA
		BRA			MsgLoop
;***�|�C���^�e�[�u��
MSGPTR	DW			STRTMSG
		DW			ENDMSG
		DW			NUMMSG
		DW			DUMYMSG
		DW			STOPPER
;***���b�Z�[�W�e�[�u��
;***�C�ӂ̏ꏊ�ɍ쐬���Ă����������C��
;***�Ō��0x00��ǉ����邱��
STRTMSG	DB			"Start!	",0
ENDMSG	DB			"Test End!",0
NUMMSG	DB			"Number = ",0
DUMYMSG	DB			"Dumy Message",0
STOPPER	DB			0,0,0,0

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
		DECFSZ	COUNT02,F		;1*98+2
		GOTO	OPOPOPS			;2*98
								;�v298

		MOVLW	0x41			;1
		MOVWF	COUNT01			;1
		NOP
		NOP
KLPIOJ
		DECFSZ	COUNT01,F		;1*64+2
		GOTO	KLPIOJ			;2*64
		
		
		
		
		
		MOVLW	0x63			;1
		MOVWF	COUNT02			;1
DWDWDDS
		CALL	DELAY_100US		;99��@CALL�����
		CALL	DELAY_10MS		;99��@CALL�����
		DECFSZ	COUNT02,F		;1*98+2
		GOTO	DWDWDDS			;2*98
								;�v298

		MOVLW	0x41			;1
		MOVWF	COUNT01			;1
		NOP
		NOP
FEFEEW
		DECFSZ	COUNT01,F		;1*64+2
		GOTO	FEFEEW			;2*64
		RETURN					;2
								;�v200
								;���v498 + 999.9msec = 1000msec
								
;**************************************
;***�t���\���@�̏��������[�`��******
LCD_INI
		RCALL	TIME5M			;15 msec�҂�
		RCALL	TIME5M
		RCALL	TIME5M
		RCALL	TIME5M			;15msec�҂�
		RCALL	TIME5M
		RCALL	TIME5M
;*****8�r�b�g���[�h�œ���
		MOVLW	H'0F'
		ANDWF	LCDDB,F			;���ʂ͌��ݒl�ۑ�
;		MOVLW	H'30'			;�W�r�b�g���[�h
		IORWF	LCDDB,F			;���[�h�o��
		RCALL	PUTCMD
		RCALL	TIME5M			;5msec�҂�
		RCALL	PUTCMD			;��8�r�b�g���[�h
		RCALL	TIME5M
		RCALL	PUTCMD			;�āX8�r�b�g���[�h
		RCALL	TIME5M
;****4�r�b�g���[�h�ɐݒ�
		MOVLW	H'0F'
		ANDWF	LCDDB,F			;���ʂ͌��ݒl�ۑ�
		MOVLW	H'20'			;4�r�b�g���[�h
		IORWF	LCDDB,F			;8�r�b�g���[�h�ŏo��
		RCALL	PUTCMD
		RCALL	TIME5M
;��������4�r�b�g���[�h�ŋ@�\�ڍאݒ�
		MOVLW	H'2E'			;FUNCION  DL = 0  N = F =1
		RCALL	LCD_CMD
		MOVLW	H'08'			;�\��OFF D = C = B =0
		RCALL	LCD_CMD
		MOVLW	H'0D'			;�\��ON	 D = 1 C = 0 B = 1
		RCALL	LCD_CMD
		MOVLW	H'06'			;Entry I/D = 1 S = 0
		RCALL	LCD_CMD
		RCALL	LCD_CLR
		RETURN
		
		
;****����M���̏o��
PUTCMD	
		BCF		LCDRS			;RS LOW
		BSF		LCDSTB			;E  HIGH
		NOP
		NOP						;MIN230nsec
		NOP
		NOP
		NOP
		BCF		LCDSTB			;E LOW
		RETURN
PUTDATA	
		BSF		LCDRS			;RS LOW
		BSF		LCDSTB			;E  HIGH
		NOP
		NOP						;MIN230nsec
		NOP
		NOP
		NOP
		BCF		LCDSTB			;E LOW
		RETURN
		
;****�t���\����֕\���f�[�^�o��
LCD_DATA
		MOVWF	DPDT			;�f�[�^�ꎞ�ۑ�
		MOVLW	H'0F'
		ANDWF	LCDDB,F			;���ʌ��ݒl�ۑ�
		MOVF	DPDT,W
		ANDLW	H'F0'			;�f�[�^��ʑ��o��
		IORWF	LCDDB,F			;���ʌ��ݒl��OR�o��
		RCALL	PUTDATA			;E����
		MOVLW	H'0F'
		ANDWF	LCDDB,F			;���ʌ��ݒl�ۑ�
		SWAPF	DPDT,W			;�f�[�^���ʑ��o��
		ANDLW	H'F0'
		IORWF	LCDDB,F			;���ʌ��ݒl��OR�o��
		RCALL	PUTDATA			;E����
		RCALL	TIME50			;���s�����҂�
		RETURN
		
;***LCD�\����ւ̐���R�}���h�o��***
LCD_CMD
		RCALL	LCD_DATA
		;****�S�����ƃz�[���̂Ƃ������f�B���C�ǉ�
		MOVF	DPDT,W
		ANDLW	H'FC'
		BTFSC	STATUS,Z
		RCALL	TIME5M
		RETURN
;*****LCD�̑S����
LCD_CLR
		MOVLW	H'01'
		RCALL	LCD_CMD
		RETURN
;*****���̑����C�u�����Ŏg�����[�`��
;*******�N���b�N��40MHz�łP�T�C�N����0.1usec
TIME50	
		MOVLW	D'165'			;1
		MOVWF	COUNT01			;1
KSIDBH
		DECFSZ	COUNT01,F		;1*164+2
		GOTO	KSIDBH		;2*164
		RETURN					;2
								;�v500 = 50usc
;****
TIME5M							;�T�C�N��
		MOVLW	D'100'			;1
		MOVWF	COUNT02			;1
ONEHUNDRED_001
		RCALL	TIME50
		DECFSZ	COUNT02,F		;1*99+2
		GOTO	ONEHUNDRED_001		;*99
		RETURN					;2
								;�v305 + 5000usec = 5030.5usc
		END
											