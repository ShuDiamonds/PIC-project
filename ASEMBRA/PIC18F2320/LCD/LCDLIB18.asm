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
		MOVFDPDT,W
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

