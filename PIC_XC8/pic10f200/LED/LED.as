opt subtitle "Microchip Technology Omniscient Code Generator (Lite mode) build 54009"

opt pagewidth 120

	opt lm

	processor	10F200
clrc	macro
	bcf	3,0
	endm
clrz	macro
	bcf	3,2
	endm
setc	macro
	bsf	3,0
	endm
setz	macro
	bsf	3,2
	endm
skipc	macro
	btfss	3,0
	endm
skipz	macro
	btfss	3,2
	endm
skipnc	macro
	btfsc	3,0
	endm
skipnz	macro
	btfsc	3,2
	endm
indf	equ	0
indf0	equ	0
pc	equ	2
pcl	equ	2
status	equ	3
fsr	equ	4
fsr0	equ	4
c	equ	1
z	equ	0
# 46 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic10f200.h"
INDF equ 00h ;# 
# 65 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic10f200.h"
TMR0 equ 01h ;# 
# 84 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic10f200.h"
PCL equ 02h ;# 
# 103 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic10f200.h"
STATUS equ 03h ;# 
# 170 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic10f200.h"
FSR equ 04h ;# 
# 189 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic10f200.h"
OSCCAL equ 05h ;# 
# 259 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic10f200.h"
GPIO equ 06h ;# 
	FNROOT	_main
	global	_GPIO
_GPIO	set	0x6
	global	_GPIObits
_GPIObits	set	0x6
psect	maintext,global,class=CODE,delta=2,split=1
global __pmaintext
__pmaintext:	;psect for function _main
; #config settings
	file	"LED.as"
	line	#
psect cinit,class=ENTRY,delta=2
global start_initialization
start_initialization:

global __initialization
__initialization:
psect cinit,class=ENTRY,delta=2,merge=1
global end_of_initialization,__end_of__initialization

;End of C runtime variable initialization code

end_of_initialization:
__end_of__initialization:clrf fsr
ljmp _main	;jump to C main() function
psect	cstackCOMMON,class=COMMON,space=1,noexec
global __pcstackCOMMON
__pcstackCOMMON:
??_main:	; 0 bytes @ 0x0
psect	cstackBANK0,class=BANK0,space=1,noexec
global __pcstackBANK0
__pcstackBANK0:
?_main:	; 2 bytes @ 0x0
	global	main@i
main@i:	; 2 bytes @ 0x0
	ds	2
;!
;!Data Sizes:
;!    Strings     0
;!    Constant    0
;!    Data        0
;!    BSS         0
;!    Persistent  0
;!    Stack       0
;!
;!Auto Spaces:
;!    Space          Size  Autos    Used
;!    COMMON            0      0       0
;!    BANK0            14      2       2

;!
;!Pointer List with Targets:
;!
;!    None.


;!
;!Critical Paths under _main in COMMON
;!
;!    None.
;!
;!Critical Paths under _main in BANK0
;!
;!    None.

;;
;;Main: autosize = 0, tempsize = 0, incstack = 0, save=0
;;

;!
;!Call Graph Tables:
;!
;! ---------------------------------------------------------------------------------
;! (Depth) Function   	        Calls       Base Space   Used Autos Params    Refs
;! ---------------------------------------------------------------------------------
;! (0) _main                                                 2     2      0      47
;!                                              0 BANK0      2     2      0
;! ---------------------------------------------------------------------------------
;! Estimated maximum stack depth 0
;! ---------------------------------------------------------------------------------
;!
;! Call Graph Graphs:
;!
;! _main (ROOT)
;!

;! Address spaces:

;!Name               Size   Autos  Total    Cost      Usage
;!STACK                0      0       0       0        0.0%
;!NULL                 0      0       0       0        0.0%
;!CODE                 0      0       0       0        0.0%
;!BITSFR0              0      0       0       1        0.0%
;!SFR0                 0      0       0       1        0.0%
;!DATA                 0      0       0       1        0.0%
;!BITCOMMON            0      0       0       2        0.0%
;!BITBANK0             E      0       0       3        0.0%
;!COMMON               0      0       0       4        0.0%
;!BANK0                E      2       2       5       14.3%
;!ABS                  0      0       0       6        0.0%

	global	_main

;; *************** function _main *****************
;; Defined at:
;;		line 9 in file "C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic10f200\LED\LED.c"
;; Parameters:    Size  Location     Type
;;		None
;; Auto vars:     Size  Location     Type
;;  i               2    0[BANK0 ] int 
;; Return value:  Size  Location     Type
;;                  2    6[BANK0 ] int 
;; Registers used:
;;		wreg, fsr0l, fsr0h, status,2, status,0, btemp+0, btemp+2, btemp+3
;; Tracked objects:
;;		On entry : 17F/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMMON   BANK0
;;      Params:         0       0
;;      Locals:         0       2
;;      Temps:          0       0
;;      Totals:         0       2
;;Total ram usage:        2 bytes
;; This function calls:
;;		Nothing
;; This function is called by:
;;		Startup code after reset
;; This function uses a non-reentrant model
;;
psect	maintext
psect	maintext
	file	"C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic10f200\LED\LED.c"
	line	9
	global	__size_of_main
	__size_of_main	equ	__end_of_main-_main
	
_main:	
;incstack = 0
	opt	stack 2
; Regs used in _main: [wreg-fsr0h+status,2-btemp+0+btemp+2+btemp+3]
	line	11
	
l401:	
;LED.c: 11: int i = 0;
	clrf	(main@i)
	clrf	(main@i+1)
	line	19
	
l403:	
;LED.c: 19: GPIO = 0;
	clrf	(6)	;volatile
	goto	l405
	line	21
;LED.c: 21: while(1)
	
l7:	
	line	24
	
l405:	
;LED.c: 22: {
;LED.c: 24: GPIObits.GP0 ^= 1;
	
	movf	(6),w	;volatile
	andlw	(1<<1)-1
	movwf	(btemp+3)
	movlw	low(01h)
	xorwf	btemp+3,f
	movf	(6),w	;volatile
	xorwf	(btemp+3),w
	andlw	not ((1<<1)-1)
	xorwf	(btemp+3),w
	movwf	(6)	;volatile
	line	26
	
l407:	
;LED.c: 26: for(i=0;i<1000;i++);
	clrf	(main@i)
	clrf	(main@i+1)
	
l409:	
	movf	(main@i+1),w
	movwf	btemp+3
	movf	(main@i),w
	movwf	btemp+2
	movf	1+wtemp1,w
	xorlw	80h
	movwf	btemp+0
	movlw	(high(03E8h))^80h
	subwf	btemp+0,w
	skipz
	goto	u35
	movlw	low(03E8h)
	subwf	0+wtemp1,w
u35:

	skipc
	goto	u31
	goto	u30
u31:
	goto	l413
u30:
	goto	l405
	
l411:	
	goto	l405
	
l8:	
	
l413:	
	movlw	01h
	movwf	btemp+2
	clrf	btemp+3
	movf	0+wtemp1,w
	addwf	(main@i),f
	skipnc
	incf	(main@i+1),f
	movf	1+wtemp1,w
	addwf	(main@i+1),f
	
l415:	
	movf	(main@i+1),w
	movwf	btemp+3
	movf	(main@i),w
	movwf	btemp+2
	movf	1+wtemp1,w
	xorlw	80h
	movwf	btemp+0
	movlw	(high(03E8h))^80h
	subwf	btemp+0,w
	skipz
	goto	u45
	movlw	low(03E8h)
	subwf	0+wtemp1,w
u45:

	skipc
	goto	u41
	goto	u40
u41:
	goto	l413
u40:
	goto	l405
	
l9:	
	goto	l405
	line	27
	
l10:	
	line	21
	goto	l405
	
l11:	
	line	31
;LED.c: 27: }
;LED.c: 29: return (0);
;	Return value of _main is never used
	
l12:	
	global	start
	ljmp	start
	opt stack 0
GLOBAL	__end_of_main
	__end_of_main:
	signat	_main,90
	global	btemp
	btemp set 01Ch

	DABS	1,28,4	;btemp
	global	wtemp0
	wtemp0 set btemp
	global	wtemp1
	wtemp1 set btemp+2
	global	wtemp2
	wtemp2 set btemp+4
	global	wtemp3
	wtemp3 set btemp+6
	global	ttemp0
	ttemp0 set btemp
	global	ttemp1
	ttemp1 set btemp+3
	global	ltemp0
	ltemp0 set btemp
	global	ltemp1
	ltemp1 set btemp+4
	end
