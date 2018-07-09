;
	list      p=12f675
	#include <p12f675.inc>

	errorlevel -302

	__CONFIG _CP_OFF & _CPD_OFF & _BODEN_OFF & _MCLRE_ON & _WDT_OFF & _PWRTE_ON & _INTRC_OSC_NOCLKOUT 

	radix	dec
;
;
;***** VARIABLE DEFINITIONS
;
w_temp		EQU	0x20
status_temp	EQU	0x21
;
;
;
	ORG	0x000
	goto	main

	ORG	0x004
	movwf	w_temp
	movf	STATUS,w
	movwf	status_temp

; isr code can go here or be located as a call subroutine elsewhere

	movf	status_temp,w
	movwf	STATUS
	swapf	w_temp,f
	swapf	w_temp,w
	retfie
;
;
;
main
	call	0x3FF
	bsf	STATUS,RP0	; bank 1
	movwf	OSCCAL
	bcf	STATUS,RP0	; bank 0
;
; remaining code goes here
;
	clrf	GPIO		; OUTPUT is Low
	movlw	B'00000111'
	movwf	CMCON
	bsf	STATUS,RP0	; bank1
	movlw	B'00001001'	; GP3,0:IN, other:OUT
	movwf	TRISIO
	clrf	ANSEL
	bcf	STATUS,RP0	; bank0
;
main00
	btfsc	GPIO,GP0
	goto	ledoff
ledon
	bcf	GPIO,GP5
	goto	main00
ledoff
	bsf	GPIO,GP5
	goto	main00
;
;
; initialize eeprom locations
;
;	ORG	0x2100
;	DE	0x00, 0x01, 0x02, 0x03
;
;
	END
