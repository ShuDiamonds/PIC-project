
	; Microchip MPLAB XC8 C Compiler V1.21
	; Copyright (C) 1984-2013 HI-TECH Software

	; Auto-generated runtime startup code for final link stage.

	;
	; Compiler options:
	;
	; -oLED.cof -mLED.map --summary=default --output=default LED.p1 \
	; --chip=10F200 -P --runtime=default --opt=default -N-1 -D__DEBUG=1 -g \
	; --asmlist --errformat=Error   [%n] %f; %l.%c %s \
	; --msgformat=Advisory[%n] %s --warnformat=Warning [%n] %f; %l.%c %s
	;


	processor	10F200

	global	_main,start,reset_vec
	fnroot	_main
	psect	config,class=CONFIG,delta=2,noexec
	psect	idloc,class=IDLOC,delta=2,noexec
	psect	code,class=CODE,delta=2
	psect	powerup,class=CODE,delta=2
	psect	reset_vec,class=CODE,delta=2
	psect	maintext,class=CODE,delta=2
	C	set	0
	Z	set	2
	PCL	set	2
	INDF	set	0

	STATUS	equ	3
	FSR	equ	4
OSCCAL	equ	0x5

	psect	reset_wrap,class=ENTRY,delta=2
	psect	text,class=CODE,delta=2
	psect	functab,class=ENTRY,delta=2
	psect	init,class=ENTRY,delta=2
	psect	cinit,class=ENTRY,delta=2
	psect	end_init,class=ENTRY,delta=2
	psect	clrtext,class=ENTRY,delta=2
	psect	jmp_tab,class=ENTRY,delta=2
	psect	strings,class=STRING,delta=2

	psect	reset_vec,class=CODE,delta=2
reset_vec:
	ds 1	;oscillator constant would be pre-programmed here
	psect	reset_wrap
reset_wrap:
	clrf	FSR		;select bank 0
	movwf	5		;calibrate oscillator
	; No powerup routine


	psect	init
start
	psect	end_init
	global start_initialization
	ljmp start_initialization	;jump to C runtime clear & initialization


psect bank0,class=BANK0,space=1
psect ram,class=RAM,space=1
psect abs1,class=ABS1,space=1
psect common,class=COMMON,space=1
psect sfr0,class=SFR0,space=1


	end	start
