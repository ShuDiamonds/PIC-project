;***液晶表示機の初期化ルーチン******
LCD_INI
		RCALL	TIME5M			;15 msec待ち
		RCALL	TIME5M
		RCALL	TIME5M
		RCALL	TIME5M			;15msec待ち
		RCALL	TIME5M
		RCALL	TIME5M
;*****8ビットモードで動作
		MOVLW	H'0F'
		ANDWF	LCDDB,F			;下位は現在値保存
;		MOVLW	H'30'			;８ビットモード
		IORWF	LCDDB,F			;モード出力
		RCALL	PUTCMD
		RCALL	TIME5M			;5msec待ち
		RCALL	PUTCMD			;再8ビットモード
		RCALL	TIME5M
		RCALL	PUTCMD			;再々8ビットモード
		RCALL	TIME5M
;****4ビットモードに設定
		MOVLW	H'0F'
		ANDWF	LCDDB,F			;下位は現在値保存
		MOVLW	H'20'			;4ビットモード
		IORWF	LCDDB,F			;8ビットモードで出力
		RCALL	PUTCMD
		RCALL	TIME5M
;ここから4ビットモードで機能詳細設定
		MOVLW	H'2E'			;FUNCION  DL = 0  N = F =1
		RCALL	LCD_CMD
		MOVLW	H'08'			;表示OFF D = C = B =0
		RCALL	LCD_CMD
		MOVLW	H'0D'			;表示ON	 D = 1 C = 0 B = 1
		RCALL	LCD_CMD
		MOVLW	H'06'			;Entry I/D = 1 S = 0
		RCALL	LCD_CMD
		RCALL	LCD_CLR
		RETURN
		
		
;****制御信号の出力
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
		
;****液晶表示器へ表示データ出力
LCD_DATA
		MOVWF	DPDT			;データ一時保存
		MOVLW	H'0F'
		ANDWF	LCDDB,F			;下位現在値保存
		MOVFDPDT,W
		ANDLW	H'F0'			;データ上位側出力
		IORWF	LCDDB,F			;下位現在値をOR出力
		RCALL	PUTDATA			;E制御
		MOVLW	H'0F'
		ANDWF	LCDDB,F			;下位現在値保存
		SWAPF	DPDT,W			;データ下位側出力
		ANDLW	H'F0'
		IORWF	LCDDB,F			;下位現在値とOR出力
		RCALL	PUTDATA			;E制御
		RCALL	TIME50			;実行完了待ち
		RETURN
		
;***LCD表示器への制御コマンド出力***
LCD_CMD
		RCALL	LCD_DATA
		;****全消去とホームのときだけディレイ追加
		MOVF	DPDT,W
		ANDLW	H'FC'
		BTFSC	STATUS,Z
		RCALL	TIME5M
		RETURN
;*****LCDの全消去
LCD_CLR
		MOVLW	H'01'
		RCALL	LCD_CMD
		RETURN
;*****その他ライブラリで使うルーチン
;*******クロックは40MHzで１サイクルは0.1usec
TIME50	
		MOVLW	D'165'			;1
		MOVWF	COUNT01			;1
KSIDBH
		DECFSZ	COUNT01,F		;1*164+2
		GOTO	KSIDBH		;2*164
		RETURN					;2
								;計500 = 50usc
;****
TIME5M							;サイクル
		MOVLW	D'100'			;1
		MOVWF	COUNT02			;1
ONEHUNDRED_001
		RCALL	TIME50
		DECFSZ	COUNT02,F		;1*99+2
		GOTO	ONEHUNDRED_001		;*99
		RETURN					;2
								;計305 + 5000usec = 5030.5usc

