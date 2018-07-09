;******************************************
;約1秒ごとに割り込みを入れRC3のLEDをチカチカさせる
;*****************************************
	;delayライブラリ
	;20MHzの場合
	;注意　COUNT01,COUNT02　という変数を宣言しておく
	;1サイクルは4クロックなので 1 / 20M = 0.000 000 005 = 0.05usec
	;なので 1サイクルは0.2usecなので 0.2usec * 500 = 100usec 
	;Zフラグ注意			
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
	;変数定義
;******************************
	CBLOCK		020H
	BUFFER				;020番地
	COUNT01			;021番地
	COUNT02			;022番地
	DATA_1BYRE			;023番地
	WREG_TMP		;割り込み時のメインプログラムのデータの
	STATUS_TMP							;退避場所．
	BSR_TMP
	PORTB_DATA		;RB割り込みの退避場所
	ENDC

;************************************
		ORG		0
		BRA		INIT
		;高位割り込みベクタ
		ORG     0x008
		GOTO	HIGH_ISR
		RETFIE
		;低位割り込みベクタ
		ORG     0x018
		GOTO	LOW_ISR
	;RETFIE （割り込みからのReturn）を実行
	;すると，スタックに格納した番地がプログ
	;ラムカウンタに戻され，割り込みがかかっ
	;た時のメインプログラムに戻る
;********初期化*********************
INIT
		MOVLW		B'00001111'		;すべてデジタルモード
		MOVWF		ADCON1			;
		CLRF		TRISA			;
		MOVLW		B'11111111'		;
		MOVWF		TRISB			
		MOVLW		B'10111111'		;
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
;************割り込み設定***********************
		BSF		RCON,IPEN		;割り込みに優先順位を使う
	;	BCF		RCON,IPEN		;割り込みに優先順位を使わない
	
	;	BSF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を許可		高位割り込み許可
		BCF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を不許可		高位割り込み不許可
		
	;	BSF		INTCON,PEIE				;すべてのマスクされていない周辺機能の割り込みを使用可能にする	低位割り込み許可
		BCF		INTCON,PEIE				;すべての周辺機能の割り込みを使用不可にする							低位割り込み許可
;************RB0割り込み設定********
		;RB0/INT は割り込みに優先順位はなく高位低位の両方に割り込みがかかる
	;	BSF		INTCON,INT0IE			;RB0/INT 外部割込みイネーブルビット(4bit目)を許可
		BCF		INTCON,INT0IE			;RB0/INT 外部割込みイネーブルビット(4bit目)を不許可
	;	BSF		INTCON2,INTEDG0			;RB0/INT ピンの立ち上がりエッジにより割り込み(6bit目)
		BCF		INTCON2,INTEDG0			;RB0/INT ピンの立ち下がりエッジにより割り込み(6bit目)
;************RB1割り込み設定*********
		BSF		INTCON3,INT1IP			;RB1/INT 高位割り込みにする
	;	BCF		INTCON3,INT1IP			;RB1/INT 低位割り込みにする
	;	BSF		INTCON3,INT1IE			;RB1/INT 外部割込みイネーブルビット(4bit目)を許可
		BCF		INTCON3,INT1IE			;RB1/INT 外部割込みイネーブルビット(4bit目)を不許可
	;	BSF		INTCON2,INTEDG1			;RB1/INT ピンの立ち上がりエッジにより割り込み(6bit目)
		BCF		INTCON2,INTEDG1			;RB1/INT ピンの立ち下がりエッジにより割り込み(6bit目)
;************RB2割り込み設定*********
		BSF		INTCON3,INT2IP			;RB2/INT 高位割り込みにする
	;	BCF		INTCON3,INT2IP			;RB2/INT 低位割り込みにする
	;	BSF		INTCON3,INT2IE			;RB2/INT 外部割込みイネーブルビット(4bit目)を許可
		BCF		INTCON3,INT2IE			;RB2/INT 外部割込みイネーブルビット(4bit目)を不許可
	;	BSF		INTCON2,INTEDG2			;RB2/INT ピンの立ち上がりエッジにより割り込み(6bit目)
		BCF		INTCON2,INTEDG2			;RB2/INT ピンの立ち下がりエッジにより割り込み(6bit目)
;************RBPORT割り込み設定*********
	;	BSF		INTCON2,RBIP			;RB2/INT 高位割り込みにする
		BCF		INTCON2,RBIP			;RB2/INT 低位割り込みにする
	;	BSF		INTCON,RBIE			;RB2/INT 外部割込みイネーブルビット(4bit目)を許可
		BCF		INTCON,RBIE			;RB2/INT 外部割込みイネーブルビット(4bit目)を不許可
;************Timer0割り込み設定********
		CLRF	T0CON
		BSF		INTCON2,TMR0IP			;Timer0 高位割り込みにする
	;	BCF		INTCON2,TMR0IP			;Timer0 低位割り込みにする
		BSF		INTCON,TMR0IE			;Timer0 外部割込みイネーブルビットを許可
	;	BCF		INTCON,TMR0IE			;Timer0 外部割込みイネーブルビットを不許可
		BSF		T0CON,TMR0ON			;Timer0 を使う
	;	BCF		T0CON,TMR0ON			;Timer0 を使わない
	;	BSF		T0CON,T08BIT			;8bitモード
		BCF		T0CON,T08BIT			;16bitモード
	;	BSF		T0CON,T0CS				;T0CKIピン の入力をクロックとする
		BCF		T0CON,T0CS				;内部クロック をクロックとする
	;	BSF		T0CON,T0SE				;立ち上がりエッジ
		BCF		T0CON,T0SE				;立ち下がりエッジ
	;	BSF		T0CON,PSA				;プリスケーラの不使用
		BCF		T0CON,PSA				;プリスケーラの使用
		MOVLW	B'00000111'				;プリスケーラを256にする
		;MOVLW	B'00000110'				;プリスケーラを128にする
		;MOVLW	B'00000100'				;プリスケーラを32にする
		;MOVLW	B'00000000'				;プリスケーラを2にする
		IORWF	T0CON
		;カウント値のロード
		MOVLW	H'67'
		MOVWF	TMR0H
		MOVLW	H'6A'
		MOVWF	TMR0L
;*********メイン関数****************
		CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'S'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'T'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'A'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'R'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'T'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;送信
;*******割り込み許可**********
		BSF		INTCON,PEIE				;すべてのマスクされていない周辺機能の割り込みを使用可能にする	低位割り込み許可
		BSF		INTCON,GIE				;グローバル割り込みイネーブルビット(7bit目)を許可		高位割り込み許可
;*****************************
MAINLOOP
		NOP
		GOTO	MAINLOOP
		
;************送信完了待ちループ**************
TX_CHEACK
		BTFSS	TXSTA,TRMT		;送信バッファが空か？
		GOTO	TX_CHEACK
		RETURN
		
;*************高位割り込み分岐***************
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
;*************低位割り込み分岐***************
LOW_ISR
		MOVWF	WREG_TMP
		MOVFF	STATUS,STATUS_TMP							;退避
		MOVFF	BSR,BSR_TMP
		
		BTFSC	INTCON,RBIF
		RCALL	RB_PORT_ISR
		
		MOVF	WREG_TMP,W
		MOVFF	STATUS_TMP,STATUS							;退避
		MOVFF	BSR_TMP,BSR
		RETFIE
;*************割り込み処理***************
;*********Timer0割り込み処理******
Timer0_ISR
		;カウント値のロード
		MOVLW	H'67'
		MOVWF	TMR0H
		MOVLW	H'6A'
		MOVWF	TMR0L
		BCF		INTCON,TMR0IF
		MOVLW	B'00111100'
		XORWF	PORTA
		RETURN
;**********RB0割り込み*************
RB0_ISR		
		BCF		INTCON,INT0IF	;割り込みフラグクリア
		
		
		RETURN
;**********RB1割り込み*************
RB1_ISR		
		BCF		INTCON3,INT1IF	;割り込みフラグクリア
		
		
		RETURN
;**********RB2割り込み*************
RB2_ISR		
		BCF		INTCON3,INT2IF	;割り込みフラグクリア
		
		
		RETURN
;**********RBPORT割り込み*************
RB_PORT_ISR
		MOVF	PORTB,W
		MOVWF	PORTB_DATA
		BCF		INTCON,RBIF	;割り込みフラグクリア
		BTFSC	PORTB_DATA,RB4
		GOTO	RB4_ISR			;RB4がhighのときJMP
RB5_CMP	BTFSC	PORTB_DATA,RB5
		GOTO	RB5_ISR			;RB4がhighのときJMP
RB6_CMP	BTFSC	PORTB_DATA,RB6
		GOTO	RB6_ISR			;RB4がhighのときJMP
RB7_CMP	BTFSC	PORTB_DATA,RB7
		GOTO	RB7_ISR			;RB4がhighのときJMP
RB_END	CALL	TX_CHEACK
		MOVLW	'\n'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'R'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'B'
		MOVWF	TXREG			;送信
		CALL	TX_CHEACK
		MOVLW	'\r'
		MOVWF	TXREG			;送信
		RETURN
RB4_ISR		;RB4がhighのときの動作
		NOP
		GOTO	RB5_CMP
RB5_ISR		;RB5がhighのときの動作
		NOP
		GOTO	RB6_CMP
RB6_ISR		;RB6がhighのときの動作
		NOP
		GOTO	RB7_CMP
RB7_ISR		;RB7がhighのときの動作
		NOP
		GOTO	RB_END
;*****************************
		END

