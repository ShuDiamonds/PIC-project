; ========================== ここから ==============================
;	PIC16F84A
;	clock:20MHz
;
;	ＬＣＤを４ビットで制御
;
;	ＰＩＣのピン接続
;		RB0	LCD DB4
;		RB1	LCD DB5
;		RB2	LCD DB6
;		RB3	LCD DB7
;
;		RA0	LCD R/W	(6:Read/Write)
;		RA1	LCD E	(5:Enable Signal)
;		RA2	LCD RS	(4:Register Select)
;
;	使用タイマ（プログラムループ）
;		 15mS	ＬＣＤパワーオンリセット待ち
;		  5mS	ＬＣＤ初期化ルーチン
;		  1mS	ＬＣＤ初期化ルーチン
;		 50uS	ＬＣＤ初期化ルーチン，書き込み待ち
;


       LIST    P=PIC16F877A
       INCLUDE P16F877A.INC
       __CONFIG _HS_OSC & _WDT_OFF & _PWRTE_ON & _CP_OFF


	CBLOCK	020h
	save_st			;STATUSのセーブ
	save_w			;W-regのセーブ
	CNT15mS			;15ｍＳカウンタ
	CNT5mS			;5ｍＳカウンタ
	CNT1mS			;1ｍＳカウンタ
	CNT50uS			;50μＳカウンタ
	char			;LCD表示データ
	ENDC

RW	EQU	03h		;LCD R/W
E	EQU	04h		;LCD Enable
RS	EQU	05h		;LCD Register Select
BUSY	EQU	03h		;BUSY FLAG (PORTB,3)


; ==================== 初期処理 =====================
	org	0
init
	BCF     STATUS,RP1		;バンク１に
	BSF     STATUS,RP0
	MOVLW	H'00'
	MOVWF	TRISA		;RA0-2は出力
	MOVLW	H'F0'
	MOVWF	TRISB		;RB0-3は出力
	BCF     STATUS,RP1
	BCF     STATUS,RP0		;バンク０に
	CLRF	PORTA
	CLRF	PORTB

	CALL	LCD_init	;LCD 初期化

; ==================== メイン処理 =====================
main
	CALL	LCD_home	;カーソルを１行目の先頭に
	MOVLW	'H'
	CALL	LCD_write
	MOVLW	'e'
	CALL	LCD_write
	MOVLW	'l'
	CALL	LCD_write
	MOVLW	'l'
	CALL	LCD_write
	MOVLW	'o'
	CALL	LCD_write
	MOVLW	','
	CALL	LCD_write

	CALL	LCD_2line	;カーソルを２行目の先頭に
	MOVLW	'w'
	CALL	LCD_write
	MOVLW	'o'
	CALL	LCD_write
	MOVLW	'r'
	CALL	LCD_write
	MOVLW	'l'
	CALL	LCD_write
	MOVLW	'd'
	CALL	LCD_write
	MOVLW	'!'
	CALL	LCD_write

	CLRF	PORTA
	CLRF	PORTB
	;SLEEP

	GOTO	main


;================= ＬＣＤ表示をクリアする ===================
LCD_clear
	MOVLW	01h
	CALL	LCD_command
	RETURN

;================= ＬＣＤのカーソル位置を先頭に戻す =========
LCD_home
	MOVLW	02h
	CALL	LCD_command
	RETURN

;================= ＬＣＤのカーソル位置を２行目の先頭に =====
LCD_2line
	MOVLW	0C0h
	CALL	LCD_command
	RETURN

;================= ＬＣＤのディスプレイをＯＮにする =========
LCD_on
	MOVLW	0Ch
	CALL	LCD_command
	RETURN

;================= ＬＣＤのディスプレイとカーソルをＯＮにする ==
LCD_on_cur
	MOVLW	0Eh
	CALL	LCD_command
	RETURN

;================= ＬＣＤのディスプレイをＯＦＦにする =======
LCD_off
	MOVLW	08h
	CALL	LCD_command
	RETURN

;================= ＬＣＤにデータを送る =====================
LCD_write
	MOVWF	char
	CALL	LCD_BF_wait	;LCD busy 解除待ち

	BCF	PORTA,RW	;R/W=0(Write)
	BSF	PORTA,RS	;RS=1(Data)

	MOVLW	0F0h		;PORTBの下位４ビットを
	ANDWF	PORTB,F		;　クリア
	SWAPF	char,W		;上位
	ANDLW	0Fh		;４ビットを
	IORWF	PORTB,F		;PORTB(3-0)にセット（PORTB(7-4)はそのまま）
	BSF	PORTA,E		;ＬＣＤにデータ書き込み
	NOP
	BCF	PORTA,E

	MOVLW	0F0h		;PORTBの下位４ビットを
	ANDWF	PORTB,F		;　クリア
	MOVF	char,W		;下位
	ANDLW	0Fh		;４ビットを
	IORWF	PORTB,F		;PORTB(3-0)にセット（PORTB(7-4)はそのまま）
	BSF	PORTA,E		;ＬＣＤにデータ書き込み
	NOP
	BCF	PORTA,E

	RETURN

;================= ＬＣＤにコマンドを送る ===================
LCD_command
	MOVWF	char
	CALL	LCD_BF_wait	;LCD busy 解除待ち

	BCF	PORTA,RW	;R/W=0(Write)
	BCF	PORTA,RS	;RS=0(Command)

	MOVLW	0F0h		;PORTBの下位４ビットを
	ANDWF	PORTB,F		;　クリア
	SWAPF	char,W		;上位
	ANDLW	0Fh		;４ビットを
	IORWF	PORTB,F		;PORTB(3-0)にセット（PORTB(7-4)はそのまま）
	BSF	PORTA,E		;ＬＣＤにデータ書き込み
	NOP
	BCF	PORTA,E

	MOVLW	0F0h		;PORTBの下位４ビットを
	ANDWF	PORTB,F		;　クリア
	MOVF	char,W		;下位
	ANDLW	0Fh		;４ビットを
	IORWF	PORTB,F		;PORTB(3-0)にセット（PORTB(7-4)はそのまま）
	BSF	PORTA,E		;ＬＣＤにデータ書き込み
	NOP
	BCF	PORTA,E

	RETURN

;================= LCD Busy 解除待ち ========================
LCD_BF_wait
	BCF	PORTA,E
	BCF	PORTA,RS	;RS=0(Control)
	BSF	PORTA,RW	;R/W=1(Read) Busy Flag read

	BCF     STATUS,RP1		;バンク１に
	BSF     STATUS,RP0
	MOVLW	0FFh
	MOVWF	TRISB		;RB0-7は入力
	BCF     STATUS,RP1
	BCF     STATUS,RP0		;バンク０に
	BSF	PORTA,E		;ＬＣＤ上位４ビット読み込み
	NOP
	BTFSS	PORTB,BUSY	;LCD Busy ?
	GOTO	LCD_BF_wait1	; No
	BCF	PORTA,E
	NOP
	BSF	PORTA,E		;ＬＣＤ下位４ビット読み飛ばし
	NOP
	BCF	PORTA,E
	GOTO	LCD_BF_wait

LCD_BF_wait1
	BSF	PORTA,E		;ＬＣＤ下位４ビット読み飛ばし
	NOP
	BCF	PORTA,E
	BCF     STATUS,RP1		;バンク１に
	BSF     STATUS,RP0
	MOVLW	0F0h		;RB0-3は出力
	MOVWF	TRISB
	BCF     STATUS,RP1
	BCF     STATUS,RP0		;バンク０に

	RETURN

;================= LCD初期化 ================================
LCD_init
	CALL	wait15ms	;15mS待つ
	BCF	PORTA,RW	;R/W=0
	BCF	PORTA,RS	;RS=0
	BCF	PORTA,E		;E=0

	MOVLW	0F0h		;PORTBの上位４ビットを
	ANDWF	PORTB,W		;取り出す（変更しないように）
	IORLW	03h		;下位４ビットに'３'をセット
	MOVWF	PORTB
	BSF	PORTA,E		;ファンクションセット（１回目）
	NOP
	BCF	PORTA,E
	CALL	wait5ms		;5mS待つ

	MOVLW	0F0h		;PORTBの上位４ビットを
	ANDWF	PORTB,W		;取り出す（変更しないように）
	IORLW	03h		;下位４ビットに'３'をセット
	MOVWF	PORTB
	BSF	PORTA,E		;ファンクションセット（２回目）
	NOP
	BCF	PORTA,E
	CALL	wait5ms		;5mS待つ

	MOVLW	0F0h		;PORTBの上位４ビットを
	ANDWF	PORTB,W		;取り出す（変更しないように）
	IORLW	03h		;下位４ビットに'３'をセット
	MOVWF	PORTB
	BSF	PORTA,E		;ファンクションセット（３回目）
	NOP
	BCF	PORTA,E
	CALL	wait5ms		;5mS待つ

	MOVLW	0F0h		;PORTBの上位４ビットを
	ANDWF	PORTB,W		;取り出す（変更しないように）
	IORLW	02h		;４ビットモード
	MOVWF	PORTB		;に
	BSF	PORTA,E		;設定
	NOP
	BCF	PORTA,E
	CALL	wait1ms		;1mS待つ

	MOVLW	028h		;４ビットモード，２行表示，７ドット
	CALL	LCD_command
	CALL	LCD_off		;ディスプレイＯＦＦ
	CALL	LCD_clear	;ＬＣＤクリア
	MOVLW	06h		;
	CALL	LCD_command	;カーソルモードセット (Increment)
	CALL	LCD_on		;ディスプレイＯＮ，カーソルＯＦＦ

	RETURN

;================= 15mS WAIT ================================
wait15ms
	MOVLW	d'3'
	MOVWF	CNT15mS
wait15ms_loop
	CALL	wait5ms
	DECFSZ	CNT15mS,F
	GOTO	wait15ms_loop
	RETURN

;================= 5mS WAIT =================================
wait5ms
	MOVLW	d'100'
	MOVWF	CNT5mS
wait5ms_loop
	CALL	wait50us
	DECFSZ	CNT5mS,F
	GOTO	wait5ms_loop
	RETURN

;================= 1mS WAIT =================================
wait1ms
	MOVLW	d'20'
	MOVWF	CNT1mS
wait1ms_loop
	CALL	wait50us
	DECFSZ	CNT1mS,F
	GOTO	wait1ms_loop
	RETURN

;================= 50μS WAIT ===============================
wait50us
	; １サイクル（４クロック）：０．２μＳ
	; ５０μＳ＝０．２μＳ×２５０サイクル

	MOVLW	d'82'		;1
	MOVWF	CNT50uS		;1
wait50us_loop
	DECFSZ	CNT50uS,F	;1
	GOTO	wait50us_loop	;2
	RETURN			;2+1

	END
; ========================== ここまで ==============================

 