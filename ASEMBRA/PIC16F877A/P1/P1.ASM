;**********************************************************************
;   This file is a basic code template for assembly code generation   *
;   on the PIC12F675. This file contains the basic code               *
;   building blocks to build upon.                                    *
;                                                                     *
;   Refer to the MPASM User's Guide for additional information on     *
;   features of the assembler (Document DS33014).                     *
;                                                                     *
;   Refer to the respective PIC data sheet for additional             *
;   information on the instruction set.                               *
;                                                                     *
;**********************************************************************
;                                                                     *
;    Filename:	    xxx.asm                                           *
;    Date:                                                            *
;    File Version:                                                    *
;                                                                     *
;    Author:                                                          *
;    Company:                                                         *
;                                                                     *
;                                                                     *
;**********************************************************************
;                                                                     *
;    Files Required: P12F675.INC                                      *
;                                                                     *
;**********************************************************************
;                                                                     *
;    Notes:                                                           *
;                                                                     *
;**********************************************************************

	list      p=12f675           ; list directive to define processor
	#include <p12f675.inc>        ; processor specific variable definitions

	errorlevel  -302              ; suppress message 302 from list file

	__CONFIG   _CP_OFF & _CPD_OFF & _BODEN_OFF & _MCLRE_OFF & _WDT_OFF & _PWRTE_ON & _INTRC_OSC_NOCLKOUT 

	;radix	dec
; '__CONFIG' directive is used to embed configuration word within .asm file.
; The lables following the directive are located in the respective .inc file.
; See data sheet for additional information on configuration word settings.




;***** VARIABLE DEFINITIONS


w_temp		EQU	0x20
status_temp	EQU	0x21
COUNT01		EQU	0x22


;**********************************************************************
	ORG     0x000             ; processor reset vector
	goto    main              ; go to beginning of program
	
	ORG     0x004             ; interrupt vector location
	movwf   w_temp            ; save off current W register contents
	movf	STATUS,w          ; move status register into W register
	movwf	status_temp       ; save off contents of STATUS register


; isr code can go here or be located as a call subroutine elsewhere


	movf    status_temp,w     ; retrieve copy of STATUS register
	movwf	STATUS            ; restore pre-isr STATUS register contents
	swapf   w_temp,f
	swapf   w_temp,w          ; restore pre-isr W register contents
	retfie                    ; return from interrupt


; these first 4 instructions are not required if the internal oscillator is not used
main
	call    0x3FF             ; retrieve factory calibration value
	bsf     STATUS,RP0        ; set file register bank to 1 
	movwf   OSCCAL            ; update register with factory cal value 
	bcf     STATUS,RP0        ; set file register bank to 0
	bcf	STATUS,RP0	 ; バンク０ に切り換えます

	clrf	GPIO	 ; 一旦 GPIO を クリア（00000000）します
	movlw	B'00000111'	 ; コンパレータ使用しない に設定します
	movwf	CMCON	 ; CMCON ← ｗ
	bsf	STATUS,RP0	 ; バンク１ に切り換えます
	movlw	B'00001001'	 ; GP0, 3 = 入力, GP2, 4, 5 = 出力　に設定
	movwf	TRISIO	 ; TRISO ← ｗ
	clrf	ANSEL	 ; アナログ入力を使用しない に設定します
	bcf	STATUS,RP0	 ; バンク０ に戻します
main00			
	;call	DELAY_100US
	btfsc	GPIO,GP0	; GPIOレジスタの GP0ビットをチェック
	goto	ledoff	 	; GPIOレジスタの GP0ビットが １ の時のみ実行される
ledon			 	 	; ラベル
	bcf	GPIO,GP5		; GPIOレジスタの GP5ビットに ０を書き込む（LED点灯）
	goto	main00		; main00 に戻る
ledoff			 	 	; ラベル
	bsf	GPIO,GP5		; GPIOレジスタの GP5ビットに １を書き込む（LED消灯）
	goto	main00		; main00 に戻る
; remaining code goes here

DELAY_100US				;サイクル
		;MOVLW	052H	;1
		MOVLW	09H	;1
		MOVWF	COUNT01	;1
		NOP				;1
LOOP001
		DECFSZ	COUNT01	;1*81+2
		GOTO	LOOP001	;2*81
		RETURN			;2
						;計250

; initialize eeprom locations

;		ORG	0x2100
;		DE	0x00, 0x01, 0x02, 0x03


		END                       ; directive 'end of program'

