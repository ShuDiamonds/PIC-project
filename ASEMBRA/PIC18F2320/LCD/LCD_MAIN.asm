
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
	;変数定義
;******************************
	CBLOCK		000H
	BUFFER
	COUNT01	
	COUNT02
	DATA_1BYRE	
	WORK1	;割り込み時のメインプログラムのデータの
							;退避場所．
	;*****LCDの変数定義と使用ポート定義
	;LCDの変数定義
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
	;使用ポート定義
	#DEFINE	LCDRS	PORTB,2
	#DEFINE	LCDSTB	PORTB,3
	#DEFINE	LCDDB	PORTB
;************************************
		ORG		0
		BRA		INIT
		;高位割り込みベクタ
		ORG     0x008
		
		;低位割り込みベクタ
		ORG     0x018
		
;************送信完了待ちループ**************

TX_CHEACK
		BTFSS	TXSTA,TRMT		;送信バッファが空か？
		GOTO	TX_CHEACK
		RETURN
;*****************************
;********初期化*********************
INIT
		MOVLW		B'00001111'		;すべてデジタルモード
		MOVWF		ADCON1			;
		CLRF		TRISA			;
		CLRF		TRISB
		CLRF		PORTB
		MOVLW		B'11111111'		;
		MOVWF		TRISC			
;**************UART設定************************
		MOVLW	B'00100000'			;RC6をTXモードに
		MOVWF	TXSTA			;SET
		;ボーレート設定	;低速モード
		MOVLW	D'64'				;9600BPS
		;MOVLW	D'32'				;19.2KBPS
		MOVWF	SPBRG			;SET BRG
		MOVLW	B'10010000'
		MOVWF	RCSTA			;RC7をRXに　連続受信を許可 ８BIT通信
;*********************************************
		;****LCD初期化
		CALL	TX_CHEACK
		MOVLW	'L'
		MOVWF	TXREG			;送信
		
		
		CALL		LCD_INI
		
		CALL	TX_CHEACK
		MOVLW	'C'
		MOVWF	TXREG			;送信
		
				MOVLW		'F'
		CALL		LCD_DATA
		CALL		DELAY_1000MS
		
		
		CALL	TX_CHEACK
		MOVLW	'D'
		MOVWF	TXREG			;送信
		
		CALL		LCD_CLR
		
		CALL	TX_CHEACK
		MOVLW	'L'
		MOVWF	TXREG			;送信
		
		
		
		
;*********メイン関数****************
MAINLOOP
		
		;startメッセージ表示
		MOVLW		H'C0'
		CALL		LCD_CMD
		MOVLW		2
		RCALL		LCDMSG
		CLRF		NUML
LOOP
		;***Numberメッセージ表示
		MOVLW		H'C0'
		CALL		LCD_CMD
		MOVLW		2
		RCALL		LCDMSG
		;****数値の表示
		MOVF		NUML,W
		CALL		LCDHEX2
		RCALL		DELAY_1000MS
		;RCALL		TIME100M
		;RCALL		TIME100M
		;**数値のカウントアップ
		INCF		NUML,F
		BNZ			LOOP
		;***ENDメッセージの表示
		MOVLW		H'02'
		CALL		LCD_CMD
		MOVLW		1
		RCALL		LCDMSG
		RCALL		DELAY_1000MS
		;***全消去
		CALL		LCD_CLR
		BRA			MAINLOOP
;******************************	
;*　16進数２桁表示関数
;*　　Wレジスタに表示する数値
;*
;******************************
LCDHEX2
	MOVWF	DPTEMP		;一時保存
	SWAPF	DPTEMP,W		;上位側
	ANDLW	0FH
	RCALL	TOHEX
	RCALL	LCD_DATA		;表示出力
	MOVF	DPTEMP,W		;下位側
	ANDLW	0FH
	RCALL	TOHEX
	RCALL	LCD_DATA		;表示出力
	RETURN
;***** バイナリから16進数ASCII変換関数
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
;*   文字列表示関数
;*		Wレジスタにメッセージ番号
;*************************************
LCDMSG		;***テーブルポインタ取得
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
MsgLoop	;***文字の取出し
		TBLRD		*+
		MOVF		TABLAT,W
		BTFSC		STATUS,Z
		RETURN
		RCALL		LCD_DATA
		BRA			MsgLoop
;***ポインタテーブル
MSGPTR	DW			STRTMSG
		DW			ENDMSG
		DW			NUMMSG
		DW			DUMYMSG
		DW			STOPPER
;***メッセージテーブル
;***任意の場所に作成していい長さも任意
;***最後に0x00を追加すること
STRTMSG	DB			"Start!	",0
ENDMSG	DB			"Test End!",0
NUMMSG	DB			"Number = ",0
DUMYMSG	DB			"Dumy Message",0
STOPPER	DB			0,0,0,0

;**********************************************
;40MHzの場合

;*********************************************
;100usecdelayルーチン
;*********************************************
DELAY_100US						;サイクル
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
								;計500 = 100usc
								
								
;*********************************************
;1msecdelayルーチン
;*********************************************
DELAY_1MS						;サイクル
		
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
								;計900usec + 498 
								
;*********************************************
;10msecdelayルーチン
;*********************************************
DELAY_10MS						;サイクル
		
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
								;計900usec + 498 
		
;*********************************************
;1000msecdelayルーチン
;*********************************************
DELAY_1000MS						;サイクル
		MOVLW	0x63			;1
		MOVWF	COUNT02			;1
OPOPOPS
		CALL	DELAY_100US		;99回　CALLされる
		CALL	DELAY_10MS		;99回　CALLされる
		DECFSZ	COUNT02,F		;1*98+2
		GOTO	OPOPOPS			;2*98
								;計298

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
		CALL	DELAY_100US		;99回　CALLされる
		CALL	DELAY_10MS		;99回　CALLされる
		DECFSZ	COUNT02,F		;1*98+2
		GOTO	DWDWDDS			;2*98
								;計298

		MOVLW	0x41			;1
		MOVWF	COUNT01			;1
		NOP
		NOP
FEFEEW
		DECFSZ	COUNT01,F		;1*64+2
		GOTO	FEFEEW			;2*64
		RETURN					;2
								;計200
								;総計498 + 999.9msec = 1000msec
								
;**************************************
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
		MOVF	DPDT,W
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
		END
											