opt subtitle "Microchip Technology Omniscient Code Generator (Lite mode) build 54009"

opt pagewidth 120

	opt lm

	processor	18F2550
porta	equ	0F80h
portb	equ	0F81h
portc	equ	0F82h
portd	equ	0F83h
porte	equ	0F84h
lata	equ	0F89h
latb	equ	0F8Ah
latc	equ	0F8Bh
latd	equ	0F8Ch
late	equ	0F8Dh
trisa	equ	0F92h
trisb	equ	0F93h
trisc	equ	0F94h
trisd	equ	0F95h
trise	equ	0F96h
pie1	equ	0F9Dh
pir1	equ	0F9Eh
ipr1	equ	0F9Fh
pie2	equ	0FA0h
pir2	equ	0FA1h
ipr2	equ	0FA2h
t3con	equ	0FB1h
tmr3l	equ	0FB2h
tmr3h	equ	0FB3h
ccp1con	equ	0FBDh
ccpr1l	equ	0FBEh
ccpr1h	equ	0FBFh
adcon1	equ	0FC1h
adcon0	equ	0FC2h
adresl	equ	0FC3h
adresh	equ	0FC4h
sspcon2	equ	0FC5h
sspcon1	equ	0FC6h
sspstat	equ	0FC7h
sspadd	equ	0FC8h
sspbuf	equ	0FC9h
t2con	equ	0FCAh
pr2	equ	0FCBh
tmr2	equ	0FCCh
t1con	equ	0FCDh
tmr1l	equ	0FCEh
tmr1h	equ	0FCFh
rcon	equ	0FD0h
wdtcon	equ	0FD1h
lvdcon	equ	0FD2h
osccon	equ	0FD3h
t0con	equ	0FD5h
tmr0l	equ	0FD6h
tmr0h	equ	0FD7h
status	equ	0FD8h
fsr2	equ	0FD9h
fsr2l	equ	0FD9h
fsr2h	equ	0FDAh
plusw2	equ	0FDBh
preinc2	equ	0FDCh
postdec2	equ	0FDDh
postinc2	equ	0FDEh
indf2	equ	0FDFh
bsr	equ	0FE0h
fsr1	equ	0FE1h
fsr1l	equ	0FE1h
fsr1h	equ	0FE2h
plusw1	equ	0FE3h
preinc1	equ	0FE4h
postdec1	equ	0FE5h
postinc1	equ	0FE6h
indf1	equ	0FE7h
wreg	equ	0FE8h
fsr0	equ	0FE9h
fsr0l	equ	0FE9h
fsr0h	equ	0FEAh
plusw0	equ	0FEBh
preinc0	equ	0FECh
postdec0	equ	0FEDh
postinc0	equ	0FEEh
indf0	equ	0FEFh
intcon3	equ	0FF0h
intcon2	equ	0FF1h
intcon	equ	0FF2h
prod	equ	0FF3h
prodl	equ	0FF3h
prodh	equ	0FF4h
tablat	equ	0FF5h
tblptr	equ	0FF6h
tblptrl	equ	0FF6h
tblptrh	equ	0FF7h
tblptru	equ	0FF8h
pcl	equ	0FF9h
pclat	equ	0FFAh
pclath	equ	0FFAh
pclatu	equ	0FFBh
stkptr	equ	0FFCh
tosl	equ	0FFDh
tosh	equ	0FFEh
tosu	equ	0FFFh
skipnz macro
	btfsc	status,2
endm
pushw macro
	movwf postinc1
endm
pushf macro arg1
	movff arg1, postinc1
endm
popw macro
	movf postdec1,w
	movf indf1,w
endm
popf macro arg1
	movf postdec1,w
	movff indf1,arg1
endm
popfc macro arg1
	movff plusw1,arg1
	decfsz fsr1,f
endm
	global	__ramtop
	global	__accesstop
# 46 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UFRM equ 0F66h ;# 
# 52 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UFRML equ 0F66h ;# 
# 129 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UFRMH equ 0F67h ;# 
# 168 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UIR equ 0F68h ;# 
# 223 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UIE equ 0F69h ;# 
# 278 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEIR equ 0F6Ah ;# 
# 328 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEIE equ 0F6Bh ;# 
# 378 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
USTAT equ 0F6Ch ;# 
# 437 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UCON equ 0F6Dh ;# 
# 487 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UADDR equ 0F6Eh ;# 
# 550 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UCFG equ 0F6Fh ;# 
# 631 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP0 equ 0F70h ;# 
# 762 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP1 equ 0F71h ;# 
# 893 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP2 equ 0F72h ;# 
# 1024 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP3 equ 0F73h ;# 
# 1155 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP4 equ 0F74h ;# 
# 1286 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP5 equ 0F75h ;# 
# 1417 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP6 equ 0F76h ;# 
# 1548 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP7 equ 0F77h ;# 
# 1679 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP8 equ 0F78h ;# 
# 1766 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP9 equ 0F79h ;# 
# 1853 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP10 equ 0F7Ah ;# 
# 1940 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP11 equ 0F7Bh ;# 
# 2027 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP12 equ 0F7Ch ;# 
# 2114 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP13 equ 0F7Dh ;# 
# 2201 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP14 equ 0F7Eh ;# 
# 2288 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
UEP15 equ 0F7Fh ;# 
# 2375 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PORTA equ 0F80h ;# 
# 2531 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PORTB equ 0F81h ;# 
# 2640 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PORTC equ 0F82h ;# 
# 2793 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PORTE equ 0F84h ;# 
# 3026 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
LATA equ 0F89h ;# 
# 3161 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
LATB equ 0F8Ah ;# 
# 3293 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
LATC equ 0F8Bh ;# 
# 3408 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TRISA equ 0F92h ;# 
# 3413 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
DDRA equ 0F92h ;# 
# 3605 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TRISB equ 0F93h ;# 
# 3610 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
DDRB equ 0F93h ;# 
# 3826 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TRISC equ 0F94h ;# 
# 3831 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
DDRC equ 0F94h ;# 
# 3997 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
OSCTUNE equ 0F9Bh ;# 
# 4055 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PIE1 equ 0F9Dh ;# 
# 4128 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PIR1 equ 0F9Eh ;# 
# 4201 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
IPR1 equ 0F9Fh ;# 
# 4274 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PIE2 equ 0FA0h ;# 
# 4344 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PIR2 equ 0FA1h ;# 
# 4414 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
IPR2 equ 0FA2h ;# 
# 4484 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
EECON1 equ 0FA6h ;# 
# 4549 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
EECON2 equ 0FA7h ;# 
# 4555 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
EEDATA equ 0FA8h ;# 
# 4561 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
EEADR equ 0FA9h ;# 
# 4567 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
RCSTA equ 0FABh ;# 
# 4572 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
RCSTA1 equ 0FABh ;# 
# 4776 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TXSTA equ 0FACh ;# 
# 4781 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TXSTA1 equ 0FACh ;# 
# 5073 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TXREG equ 0FADh ;# 
# 5078 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TXREG1 equ 0FADh ;# 
# 5084 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
RCREG equ 0FAEh ;# 
# 5089 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
RCREG1 equ 0FAEh ;# 
# 5095 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
SPBRG equ 0FAFh ;# 
# 5100 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
SPBRG1 equ 0FAFh ;# 
# 5106 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
SPBRGH equ 0FB0h ;# 
# 5112 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
T3CON equ 0FB1h ;# 
# 5234 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR3 equ 0FB2h ;# 
# 5240 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR3L equ 0FB2h ;# 
# 5246 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR3H equ 0FB3h ;# 
# 5252 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CMCON equ 0FB4h ;# 
# 5347 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CVRCON equ 0FB5h ;# 
# 5431 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
ECCP1AS equ 0FB6h ;# 
# 5436 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCP1AS equ 0FB6h ;# 
# 5560 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
ECCP1DEL equ 0FB7h ;# 
# 5565 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCP1DEL equ 0FB7h ;# 
# 5599 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
BAUDCON equ 0FB8h ;# 
# 5604 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
BAUDCTL equ 0FB8h ;# 
# 5778 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCP2CON equ 0FBAh ;# 
# 5841 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCPR2 equ 0FBBh ;# 
# 5847 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCPR2L equ 0FBBh ;# 
# 5853 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCPR2H equ 0FBCh ;# 
# 5859 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCP1CON equ 0FBDh ;# 
# 5922 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCPR1 equ 0FBEh ;# 
# 5928 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCPR1L equ 0FBEh ;# 
# 5934 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
CCPR1H equ 0FBFh ;# 
# 5940 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
ADCON2 equ 0FC0h ;# 
# 6010 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
ADCON1 equ 0FC1h ;# 
# 6100 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
ADCON0 equ 0FC2h ;# 
# 6222 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
ADRES equ 0FC3h ;# 
# 6228 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
ADRESL equ 0FC3h ;# 
# 6234 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
ADRESH equ 0FC4h ;# 
# 6240 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
SSPCON2 equ 0FC5h ;# 
# 6301 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
SSPCON1 equ 0FC6h ;# 
# 6370 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
SSPSTAT equ 0FC7h ;# 
# 6636 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
SSPADD equ 0FC8h ;# 
# 6642 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
SSPBUF equ 0FC9h ;# 
# 6648 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
T2CON equ 0FCAh ;# 
# 6745 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PR2 equ 0FCBh ;# 
# 6750 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
MEMCON equ 0FCBh ;# 
# 6854 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR2 equ 0FCCh ;# 
# 6860 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
T1CON equ 0FCDh ;# 
# 6964 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR1 equ 0FCEh ;# 
# 6970 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR1L equ 0FCEh ;# 
# 6976 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR1H equ 0FCFh ;# 
# 6982 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
RCON equ 0FD0h ;# 
# 7130 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
WDTCON equ 0FD1h ;# 
# 7157 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
HLVDCON equ 0FD2h ;# 
# 7162 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
LVDCON equ 0FD2h ;# 
# 7426 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
OSCCON equ 0FD3h ;# 
# 7508 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
T0CON equ 0FD5h ;# 
# 7577 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR0 equ 0FD6h ;# 
# 7583 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR0L equ 0FD6h ;# 
# 7589 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TMR0H equ 0FD7h ;# 
# 7595 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
STATUS equ 0FD8h ;# 
# 7673 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
FSR2 equ 0FD9h ;# 
# 7679 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
FSR2L equ 0FD9h ;# 
# 7685 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
FSR2H equ 0FDAh ;# 
# 7691 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PLUSW2 equ 0FDBh ;# 
# 7697 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PREINC2 equ 0FDCh ;# 
# 7703 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
POSTDEC2 equ 0FDDh ;# 
# 7709 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
POSTINC2 equ 0FDEh ;# 
# 7715 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
INDF2 equ 0FDFh ;# 
# 7721 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
BSR equ 0FE0h ;# 
# 7727 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
FSR1 equ 0FE1h ;# 
# 7733 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
FSR1L equ 0FE1h ;# 
# 7739 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
FSR1H equ 0FE2h ;# 
# 7745 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PLUSW1 equ 0FE3h ;# 
# 7751 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PREINC1 equ 0FE4h ;# 
# 7757 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
POSTDEC1 equ 0FE5h ;# 
# 7763 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
POSTINC1 equ 0FE6h ;# 
# 7769 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
INDF1 equ 0FE7h ;# 
# 7775 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
WREG equ 0FE8h ;# 
# 7781 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
FSR0 equ 0FE9h ;# 
# 7787 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
FSR0L equ 0FE9h ;# 
# 7793 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
FSR0H equ 0FEAh ;# 
# 7799 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PLUSW0 equ 0FEBh ;# 
# 7805 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PREINC0 equ 0FECh ;# 
# 7811 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
POSTDEC0 equ 0FEDh ;# 
# 7817 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
POSTINC0 equ 0FEEh ;# 
# 7823 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
INDF0 equ 0FEFh ;# 
# 7829 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
INTCON3 equ 0FF0h ;# 
# 7920 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
INTCON2 equ 0FF1h ;# 
# 7996 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
INTCON equ 0FF2h ;# 
# 8132 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PROD equ 0FF3h ;# 
# 8138 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PRODL equ 0FF3h ;# 
# 8144 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PRODH equ 0FF4h ;# 
# 8150 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TABLAT equ 0FF5h ;# 
# 8158 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TBLPTR equ 0FF6h ;# 
# 8164 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TBLPTRL equ 0FF6h ;# 
# 8170 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TBLPTRH equ 0FF7h ;# 
# 8176 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TBLPTRU equ 0FF8h ;# 
# 8184 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PCLAT equ 0FF9h ;# 
# 8191 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PC equ 0FF9h ;# 
# 8197 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PCL equ 0FF9h ;# 
# 8203 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PCLATH equ 0FFAh ;# 
# 8209 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
PCLATU equ 0FFBh ;# 
# 8215 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
STKPTR equ 0FFCh ;# 
# 8290 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TOS equ 0FFDh ;# 
# 8296 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TOSL equ 0FFDh ;# 
# 8302 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TOSH equ 0FFEh ;# 
# 8308 "C:\Program Files (x86)\Microchip\xc8\v1.21\include\pic18f2550.h"
TOSU equ 0FFFh ;# 
	FNCALL	_main,_OpenUSART
	FNCALL	_main,_OpenADC
	FNCALL	_main,_SetChanADC
	FNCALL	_main,_putsUSART
	FNCALL	_main,_printf
	FNCALL	_main,_Delay100TCYx
	FNCALL	_main,_ConvertADC
	FNCALL	_main,_BusyADC
	FNCALL	_main,_ReadADC
	FNCALL	_printf,_putch
	FNCALL	_printf,___lldiv
	FNCALL	_printf,___llmod
	FNCALL	_putsUSART,_WriteUSART
	FNROOT	_main
	global	main@F4951
	global	main@F4953
psect	idataCOMRAM,class=CODE,space=0,delta=1,noexec
global __pidataCOMRAM
__pidataCOMRAM:
	file	"C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic18f2550\AD\AD.c"
	line	54

;initializer for main@F4951
	db	low(0Dh)
	db	low(053h)
	db	low(074h)
	db	low(061h)
	db	low(072h)
	db	low(074h)
	db	low(021h)
	db	low(021h)
	db	low(0Ah)
	db	low(0)
	line	55

;initializer for main@F4953
	db	low(046h)
	db	low(055h)
	db	low(04Bh)
	db	low(055h)
	db	low(044h)
	db	low(041h)
	db	low(0)
	global	_dpowers
psect	smallconst,class=SMALLCONST,space=0,reloc=2,noexec
global __psmallconst
__psmallconst:
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\common\doprnt.c"
	line	354
_dpowers:
	dw	(01h) & 0xffff
	dw	highword(01h)
	dw	(0Ah) & 0xffff
	dw	highword(0Ah)
	dw	(064h) & 0xffff
	dw	highword(064h)
	dw	(03E8h) & 0xffff
	dw	highword(03E8h)
	dw	(02710h) & 0xffff
	dw	highword(02710h)
	dw	(0186A0h) & 0xffff
	dw	highword(0186A0h)
	dw	(0F4240h) & 0xffff
	dw	highword(0F4240h)
	dw	(0989680h) & 0xffff
	dw	highword(0989680h)
	dw	(05F5E100h) & 0xffff
	dw	highword(05F5E100h)
	dw	(03B9ACA00h) & 0xffff
	dw	highword(03B9ACA00h)
	global __end_of_dpowers
__end_of_dpowers:
	global	_dpowers
	global	_USART_Status
	global	_ADCON0
_ADCON0	set	0xFC2
	global	_ADCON0bits
_ADCON0bits	set	0xFC2
	global	_ADCON1
_ADCON1	set	0xFC1
	global	_ADCON2
_ADCON2	set	0xFC0
	global	_ADRESH
_ADRESH	set	0xFC4
	global	_ADRESL
_ADRESL	set	0xFC3
	global	_INTCONbits
_INTCONbits	set	0xFF2
	global	_PIE1bits
_PIE1bits	set	0xF9D
	global	_PIR1bits
_PIR1bits	set	0xF9E
	global	_RCSTA
_RCSTA	set	0xFAB
	global	_RCSTAbits
_RCSTAbits	set	0xFAB
	global	_SPBRG
_SPBRG	set	0xFAF
	global	_SPBRGH
_SPBRGH	set	0xFB0
	global	_TRISA
_TRISA	set	0xF92
	global	_TRISB
_TRISB	set	0xF93
	global	_TRISC
_TRISC	set	0xF94
	global	_TRISCbits
_TRISCbits	set	0xF94
	global	_TXREG
_TXREG	set	0xFAD
	global	_TXSTA
_TXSTA	set	0xFAC
	global	_TXSTAbits
_TXSTAbits	set	0xFAC
	global __stringdata
__stringdata:
	
STR_1:
	db	13
	db	72	;'H'
	db	101	;'e'
	db	108	;'l'
	db	108	;'l'
	db	111	;'o'
	db	32
	db	119	;'w'
	db	111	;'o'
	db	114	;'r'
	db	108	;'l'
	db	100	;'d'
	db	10
	db	0
	
STR_2:
	db	13
	db	100	;'d'
	db	97	;'a'
	db	116	;'t'
	db	97	;'a'
	db	61	;'='
	db	37
	db	108	;'l'
	db	100	;'d'
	db	10
	db	0
	global __end_of__stringdata
__end_of__stringdata:
; #config settings
global __CFG_CPUDIV$OSC1_PLL2
__CFG_CPUDIV$OSC1_PLL2 equ 0x0
global __CFG_PLLDIV$5
__CFG_PLLDIV$5 equ 0x0
global __CFG_USBDIV$2
__CFG_USBDIV$2 equ 0x0
global __CFG_IESO$OFF
__CFG_IESO$OFF equ 0x0
global __CFG_FOSC$HSPLL_HS
__CFG_FOSC$HSPLL_HS equ 0x0
global __CFG_FCMEN$OFF
__CFG_FCMEN$OFF equ 0x0
global __CFG_VREGEN$ON
__CFG_VREGEN$ON equ 0x0
global __CFG_BOR$ON
__CFG_BOR$ON equ 0x0
global __CFG_BORV$3
__CFG_BORV$3 equ 0x0
global __CFG_PWRT$OFF
__CFG_PWRT$OFF equ 0x0
global __CFG_WDTPS$32768
__CFG_WDTPS$32768 equ 0x0
global __CFG_WDT$OFF
__CFG_WDT$OFF equ 0x0
global __CFG_PBADEN$OFF
__CFG_PBADEN$OFF equ 0x0
global __CFG_LPT1OSC$OFF
__CFG_LPT1OSC$OFF equ 0x0
global __CFG_MCLRE$ON
__CFG_MCLRE$ON equ 0x0
global __CFG_STVREN$ON
__CFG_STVREN$ON equ 0x0
global __CFG_XINST$OFF
__CFG_XINST$OFF equ 0x0
global __CFG_LVP$OFF
__CFG_LVP$OFF equ 0x0
global __CFG_CP0$OFF
__CFG_CP0$OFF equ 0x0
global __CFG_CP1$OFF
__CFG_CP1$OFF equ 0x0
global __CFG_CPB$OFF
__CFG_CPB$OFF equ 0x0
global __CFG_WRT0$OFF
__CFG_WRT0$OFF equ 0x0
global __CFG_WRT1$OFF
__CFG_WRT1$OFF equ 0x0
global __CFG_WRTB$OFF
__CFG_WRTB$OFF equ 0x0
global __CFG_WRTC$OFF
__CFG_WRTC$OFF equ 0x0
global __CFG_EBTR0$OFF
__CFG_EBTR0$OFF equ 0x0
global __CFG_EBTR1$OFF
__CFG_EBTR1$OFF equ 0x0
global __CFG_EBTRB$OFF
__CFG_EBTRB$OFF equ 0x0
	file	"AD.as"
	line	#
psect	cinit,class=CODE,delta=1,reloc=2
global __pcinit
__pcinit:
global start_initialization
start_initialization:

global __initialization
__initialization:
psect	bssCOMRAM,class=COMRAM,space=1,noexec
global __pbssCOMRAM
__pbssCOMRAM:
	global	_USART_Status
_USART_Status:
       ds      1
psect	dataCOMRAM,class=COMRAM,space=1,noexec
global __pdataCOMRAM
__pdataCOMRAM:
	file	"C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic18f2550\AD\AD.c"
	line	54
main@F4951:
       ds      10
psect	dataCOMRAM
	file	"C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic18f2550\AD\AD.c"
	line	55
main@F4953:
       ds      7
	file	"AD.as"
	line	#
psect	cinit
; Clear objects allocated to COMRAM (1 bytes)
	global __pbssCOMRAM
clrf	(__pbssCOMRAM+0)&0xffh,c
	line	#
; Initialize objects allocated to COMRAM (17 bytes)
	global __pidataCOMRAM
	; load TBLPTR registers with __pidataCOMRAM
	movlw	low (__pidataCOMRAM)
	movwf	tblptrl
	movlw	high(__pidataCOMRAM)
	movwf	tblptrh
	movlw	low highword(__pidataCOMRAM)
	movwf	tblptru
	lfsr	0,__pdataCOMRAM
	lfsr	1,17
	copy_data0:
	tblrd	*+
	movff	tablat, postinc0
	movf	postdec1,w
	movf	fsr1l,w
	bnz	copy_data0
psect cinit,class=CODE,delta=1
global end_of_initialization,__end_of__initialization

;End of C runtime variable initialization code

end_of_initialization:
__end_of__initialization:	GLOBAL	__Lmediumconst
	movlw	low highword(__Lmediumconst)
	movwf	tblptru
movlb 0
goto _main	;jump to C main() function
psect	cstackCOMRAM,class=COMRAM,space=1,noexec
global __pcstackCOMRAM
__pcstackCOMRAM:
	global	?_OpenUSART
?_OpenUSART:	; 0 bytes @ 0x0
	global	?_OpenADC
?_OpenADC:	; 0 bytes @ 0x0
	global	?_SetChanADC
?_SetChanADC:	; 0 bytes @ 0x0
	global	?_Delay100TCYx
?_Delay100TCYx:	; 0 bytes @ 0x0
?_ConvertADC:	; 0 bytes @ 0x0
??_ConvertADC:	; 0 bytes @ 0x0
??_BusyADC:	; 0 bytes @ 0x0
	global	?_putch
?_putch:	; 0 bytes @ 0x0
	global	?_WriteUSART
?_WriteUSART:	; 0 bytes @ 0x0
?_main:	; 0 bytes @ 0x0
?_BusyADC:	; 1 bytes @ 0x0
	global	?_ReadADC
?_ReadADC:	; 2 bytes @ 0x0
	global	?___lldiv
?___lldiv:	; 4 bytes @ 0x0
	global	OpenADC@config
OpenADC@config:	; 1 bytes @ 0x0
	global	SetChanADC@channel
SetChanADC@channel:	; 1 bytes @ 0x0
	global	OpenUSART@config
OpenUSART@config:	; 1 bytes @ 0x0
	global	WriteUSART@data
WriteUSART@data:	; 1 bytes @ 0x0
	global	Delay100TCYx@unit
Delay100TCYx@unit:	; 1 bytes @ 0x0
putch@c:	; 1 bytes @ 0x0
	global	___lldiv@dividend
___lldiv@dividend:	; 4 bytes @ 0x0
	ds   1
??_SetChanADC:	; 0 bytes @ 0x1
	global	?_putsUSART
?_putsUSART:	; 0 bytes @ 0x1
??_Delay100TCYx:	; 0 bytes @ 0x1
??_putch:	; 0 bytes @ 0x1
??_WriteUSART:	; 0 bytes @ 0x1
	global	OpenADC@config2
OpenADC@config2:	; 1 bytes @ 0x1
	global	OpenUSART@spbrg
OpenUSART@spbrg:	; 2 bytes @ 0x1
	global	putsUSART@data
putsUSART@data:	; 2 bytes @ 0x1
	ds   1
??_ReadADC:	; 0 bytes @ 0x2
	global	OpenADC@portconfig
OpenADC@portconfig:	; 1 bytes @ 0x2
	ds   1
??_OpenUSART:	; 0 bytes @ 0x3
??_OpenADC:	; 0 bytes @ 0x3
??_putsUSART:	; 0 bytes @ 0x3
	ds   1
	global	___lldiv@divisor
___lldiv@divisor:	; 4 bytes @ 0x4
	ds   4
??___lldiv:	; 0 bytes @ 0x8
	ds   1
	global	___lldiv@quotient
___lldiv@quotient:	; 4 bytes @ 0x9
	ds   4
	global	___lldiv@counter
___lldiv@counter:	; 1 bytes @ 0xD
	ds   1
	global	?___llmod
?___llmod:	; 4 bytes @ 0xE
	global	___llmod@dividend
___llmod@dividend:	; 4 bytes @ 0xE
	ds   4
	global	___llmod@divisor
___llmod@divisor:	; 4 bytes @ 0x12
	ds   4
??___llmod:	; 0 bytes @ 0x16
	ds   1
	global	___llmod@counter
___llmod@counter:	; 1 bytes @ 0x17
	ds   1
	global	?_printf
?_printf:	; 2 bytes @ 0x18
	global	printf@f
printf@f:	; 2 bytes @ 0x18
	ds   6
??_printf:	; 0 bytes @ 0x1E
	ds   2
	global	printf@prec
printf@prec:	; 2 bytes @ 0x20
	ds   2
	global	printf@ap
printf@ap:	; 2 bytes @ 0x22
	ds   2
	global	printf@flag
printf@flag:	; 1 bytes @ 0x24
	ds   1
	global	printf@_val
printf@_val:	; 5 bytes @ 0x25
	ds   5
	global	printf@c
printf@c:	; 1 bytes @ 0x2A
	ds   1
??_main:	; 0 bytes @ 0x2B
	ds   3
	global	main@Message2
main@Message2:	; 7 bytes @ 0x2E
	ds   7
	global	main@Message1
main@Message1:	; 10 bytes @ 0x35
	ds   10
	global	main@DATA
main@DATA:	; 3 bytes @ 0x3F
	ds   3
	global	main@data
main@data:	; 4 bytes @ 0x42
	ds   4
;!
;!Data Sizes:
;!    Strings     25
;!    Constant    40
;!    Data        17
;!    BSS         1
;!    Persistent  0
;!    Stack       0
;!
;!Auto Spaces:
;!    Space          Size  Autos    Used
;!    COMRAM           95     70      88
;!    BANK0           160      0       0
;!    BANK1           256      0       0
;!    BANK2           256      0       0
;!    BANK3           256      0       0
;!    BANK4           256      0       0
;!    BANK5           256      0       0
;!    BANK6           256      0       0
;!    BANK7           256      0       0

;!
;!Pointer List with Targets:
;!
;!    ?_ReadADC	int  size(2) Largest target is 0
;!
;!    ?___llmod	unsigned long  size(2) Largest target is 0
;!
;!    ?___lldiv	unsigned long  size(2) Largest target is 0
;!
;!    putsUSART@data	PTR unsigned char  size(2) Largest target is 10
;!		 -> main@Message1(COMRAM[10]), 
;!
;!    printf@f	PTR const unsigned char  size(2) Largest target is 14
;!		 -> STR_2(CODE[11]), STR_1(CODE[14]), 
;!
;!    ?_printf	int  size(2) Largest target is 0
;!
;!    printf@ap	PTR void [1] size(2) Largest target is 2
;!		 -> ?_printf(COMRAM[2]), 
;!
;!    S99$_cp	PTR const unsigned char  size(2) Largest target is 0
;!
;!    _val._str._cp	PTR const unsigned char  size(2) Largest target is 0
;!


;!
;!Critical Paths under _main in COMRAM
;!
;!    _main->_printf
;!    _printf->___llmod
;!    ___llmod->___lldiv
;!    _putsUSART->_WriteUSART
;!
;!Critical Paths under _main in BANK0
;!
;!    None.
;!
;!Critical Paths under _main in BANK1
;!
;!    None.
;!
;!Critical Paths under _main in BANK2
;!
;!    None.
;!
;!Critical Paths under _main in BANK3
;!
;!    None.
;!
;!Critical Paths under _main in BANK4
;!
;!    None.
;!
;!Critical Paths under _main in BANK5
;!
;!    None.
;!
;!Critical Paths under _main in BANK6
;!
;!    None.
;!
;!Critical Paths under _main in BANK7
;!
;!    None.

;;
;;Main: autosize = 0, tempsize = 3, incstack = 0, save=0
;;

;!
;!Call Graph Tables:
;!
;! ---------------------------------------------------------------------------------
;! (Depth) Function   	        Calls       Base Space   Used Autos Params    Refs
;! ---------------------------------------------------------------------------------
;! (0) _main                                                27    27      0     855
;!                                             43 COMRAM    27    27      0
;!                          _OpenUSART
;!                            _OpenADC
;!                         _SetChanADC
;!                          _putsUSART
;!                             _printf
;!                       _Delay100TCYx
;!                         _ConvertADC
;!                            _BusyADC
;!                            _ReadADC
;! ---------------------------------------------------------------------------------
;! (1) _ReadADC                                              2     0      2       0
;!                                              0 COMRAM     2     0      2
;! ---------------------------------------------------------------------------------
;! (1) _BusyADC                                              0     0      0       0
;! ---------------------------------------------------------------------------------
;! (1) _ConvertADC                                           0     0      0       0
;! ---------------------------------------------------------------------------------
;! (1) _Delay100TCYx                                         1     0      1      15
;!                                              0 COMRAM     1     0      1
;! ---------------------------------------------------------------------------------
;! (1) _printf                                              19    13      6     495
;!                                             24 COMRAM    19    13      6
;!                              _putch
;!                            ___lldiv
;!                            ___llmod
;! ---------------------------------------------------------------------------------
;! (2) ___llmod                                             10     2      8     105
;!                                             14 COMRAM    10     2      8
;!                            ___lldiv (ARG)
;! ---------------------------------------------------------------------------------
;! (2) ___lldiv                                             14     6      8     105
;!                                              0 COMRAM    14     6      8
;! ---------------------------------------------------------------------------------
;! (2) _putch                                                1     0      1       0
;!                                              0 COMRAM     1     0      1
;! ---------------------------------------------------------------------------------
;! (1) _putsUSART                                            2     0      2      45
;!                                              1 COMRAM     2     0      2
;!                         _WriteUSART
;! ---------------------------------------------------------------------------------
;! (2) _WriteUSART                                           1     0      1      15
;!                                              0 COMRAM     1     0      1
;! ---------------------------------------------------------------------------------
;! (1) _SetChanADC                                           2     1      1      15
;!                                              0 COMRAM     2     1      1
;! ---------------------------------------------------------------------------------
;! (1) _OpenADC                                              5     2      3     105
;!                                              0 COMRAM     5     2      3
;! ---------------------------------------------------------------------------------
;! (1) _OpenUSART                                            3     0      3     150
;!                                              0 COMRAM     3     0      3
;! ---------------------------------------------------------------------------------
;! Estimated maximum stack depth 2
;! ---------------------------------------------------------------------------------
;!
;! Call Graph Graphs:
;!
;! _main (ROOT)
;!   _OpenUSART
;!   _OpenADC
;!   _SetChanADC
;!   _putsUSART
;!     _WriteUSART
;!   _printf
;!     _putch
;!     ___lldiv
;!     ___llmod
;!       ___lldiv (ARG)
;!   _Delay100TCYx
;!   _ConvertADC
;!   _BusyADC
;!   _ReadADC
;!

;! Address spaces:

;!Name               Size   Autos  Total    Cost      Usage
;!BIGRAM             7FF      0       0      21        0.0%
;!EEDATA             100      0       0       0        0.0%
;!BITBANK7           100      0       0      18        0.0%
;!BANK7              100      0       0      19        0.0%
;!BITBANK6           100      0       0      16        0.0%
;!BANK6              100      0       0      17        0.0%
;!BITBANK5           100      0       0      14        0.0%
;!BANK5              100      0       0      15        0.0%
;!BITBANK4           100      0       0      12        0.0%
;!BANK4              100      0       0      13        0.0%
;!BITBANK3           100      0       0      10        0.0%
;!BANK3              100      0       0      11        0.0%
;!BITBANK2           100      0       0       8        0.0%
;!BANK2              100      0       0       9        0.0%
;!BITBANK1           100      0       0       6        0.0%
;!BANK1              100      0       0       7        0.0%
;!BITBANK0            A0      0       0       4        0.0%
;!BANK0               A0      0       0       5        0.0%
;!BITCOMRAM           5F      0       0       0        0.0%
;!COMRAM              5F     46      58       1       92.6%
;!BITSFR               0      0       0      40        0.0%
;!SFR                  0      0       0      40        0.0%
;!STACK                0      0       2       2        0.0%
;!NULL                 0      0       0       0        0.0%
;!ABS                  0      0      58      20        0.0%
;!DATA                 0      0      5A       3        0.0%
;!CODE                 0      0       0       0        0.0%

	global	_main

;; *************** function _main *****************
;; Defined at:
;;		line 52 in file "C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic18f2550\AD\AD.c"
;; Parameters:    Size  Location     Type
;;		None
;; Auto vars:     Size  Location     Type
;;  Message1       10   53[COMRAM] unsigned char [10]
;;  Message2        7   46[COMRAM] unsigned char [7]
;;  data            4   66[COMRAM] long 
;;  DATA            3   63[COMRAM] float 
;; Return value:  Size  Location     Type
;;		None               void
;; Registers used:
;;		wreg, fsr1l, fsr1h, fsr2l, fsr2h, status,2, status,0, tblptrl, tblptrh, tblptru, prodl, prodh, cstack
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         0       0       0       0       0       0       0       0       0
;;      Locals:        24       0       0       0       0       0       0       0       0
;;      Temps:          3       0       0       0       0       0       0       0       0
;;      Totals:        27       0       0       0       0       0       0       0       0
;;Total ram usage:       27 bytes
;; Hardware stack levels required when called:    2
;; This function calls:
;;		_OpenUSART
;;		_OpenADC
;;		_SetChanADC
;;		_putsUSART
;;		_printf
;;		_Delay100TCYx
;;		_ConvertADC
;;		_BusyADC
;;		_ReadADC
;; This function is called by:
;;		Startup code after reset
;; This function uses a non-reentrant model
;;
psect	text0,class=CODE,space=0,reloc=2
global __ptext0
__ptext0:
psect	text0
	file	"C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic18f2550\AD\AD.c"
	line	52
	global	__size_of_main
	__size_of_main	equ	__end_of_main-_main
	
_main:
;incstack = 0
	opt	stack 29
;incstack = 0
	line	54
	
l963:
;AD.c: 54: char Message1[10]="\rStart!!\n";
	lfsr	2,(main@F4951)
	lfsr	1,(main@Message1)
	movlw	10
u311:
	movff	postinc2,postinc1
	decfsz	wreg
	goto	u311
	line	55
;AD.c: 55: char Message2[7]="FUKUDA";
	lfsr	2,(main@F4953)
	lfsr	1,(main@Message2)
	movlw	7
u321:
	movff	postinc2,postinc1
	decfsz	wreg
	goto	u321
	line	56
	
l965:
;AD.c: 56: long int data=0;
	movlw	low(0)
	movwf	((c:main@data)),c
	movlw	high(0)
	movwf	((c:main@data+1)),c
	movlw	low highword(0)
	movwf	((c:main@data+2)),c
	movlw	high highword(0)
	movwf	((c:main@data+3)),c
	line	57
	
l967:
;AD.c: 57: float DATA=0;
	movlw	low(float24(0.0000000000000000))
	movwf	((c:main@DATA)),c
	movlw	high(float24(0.0000000000000000))
	movwf	((c:main@DATA+1)),c
	movlw	low highword(float24(0.0000000000000000))
	movwf	((c:main@DATA+2)),c

	line	62
	
l969:
;AD.c: 60: OpenUSART(0b01111111 & 0b10111111 &
;AD.c: 61: 0b11111110 & 0b11111101 &
;AD.c: 62: 0b11111111 & 0b11101111, 77);
	movwf	(??_main+0+0)&0ffh,c
	movlw	low(02Ch)
	movwf	((c:?_OpenUSART)),c
	movf	(??_main+0+0)&0ffh,c,w
	movlw	high(04Dh)
	movwf	(1+((c:?_OpenUSART)+01h)),c
	movlw	low(04Dh)
	movwf	(0+((c:?_OpenUSART)+01h)),c
	call	_OpenUSART	;wreg free
	line	76
	
l971:
;AD.c: 67: OpenADC(0b11101111 &
;AD.c: 68: 0b11111111 &
;AD.c: 69: 0b11111001,
;AD.c: 70: 0b10000111 &
;AD.c: 71: 0b01111111 &
;AD.c: 72: 0b11111110 &
;AD.c: 73: 0b11111101,
;AD.c: 74: 0b1110
;AD.c: 76: );
	movwf	(??_main+0+0)&0ffh,c
	movlw	low(0E9h)
	movwf	((c:?_OpenADC)),c
	movf	(??_main+0+0)&0ffh,c,w
	movwf	(??_main+1+0)&0ffh,c
	movlw	low(04h)
	movwf	(0+((c:?_OpenADC)+01h)),c
	movf	(??_main+1+0)&0ffh,c,w
	movwf	(??_main+2+0)&0ffh,c
	movlw	low(0Eh)
	movwf	(0+((c:?_OpenADC)+02h)),c
	movf	(??_main+2+0)&0ffh,c,w
	call	_OpenADC	;wreg free
	line	79
	
l973:
;AD.c: 79: SetChanADC(0b10000111);
	movwf	(??_main+0+0)&0ffh,c
	movlw	low(087h)
	movwf	((c:?_SetChanADC)),c
	movf	(??_main+0+0)&0ffh,c,w
	call	_SetChanADC	;wreg free
	line	81
	
l975:
;AD.c: 81: TRISA=0x0F;
	movlw	low(0Fh)
	movwf	((c:3986)),c	;volatile
	line	82
	
l977:
;AD.c: 82: TRISB=0;
	movlw	low(0)
	movwf	((c:3987)),c	;volatile
	line	83
	
l979:
;AD.c: 83: TRISC=0b10111111;
	movlw	low(0BFh)
	movwf	((c:3988)),c	;volatile
	line	84
	
l981:
;AD.c: 84: putsUSART(Message1);
	movlw	high((c:main@Message1))
	movwf	((c:?_putsUSART+1)),c
	movlw	low((c:main@Message1))
	movwf	((c:?_putsUSART)),c
	call	_putsUSART	;wreg free
	line	85
	
l983:
;AD.c: 85: printf("\rHello world\n");
	movlw	high(STR_1)
	movwf	((c:?_printf+1)),c
	movlw	low(STR_1)
	movwf	((c:?_printf)),c
	call	_printf	;wreg free
	goto	l985
	line	86
;AD.c: 86: while(1)
	
l31:
	line	90
	
l985:
;AD.c: 87: {
;AD.c: 90: SetChanADC(0b10000111);
	movwf	(??_main+0+0)&0ffh,c
	movlw	low(087h)
	movwf	((c:?_SetChanADC)),c
	movf	(??_main+0+0)&0ffh,c,w
	call	_SetChanADC	;wreg free
	line	91
	
l987:
;AD.c: 91: Delay100TCYx(5);
	movwf	(??_main+0+0)&0ffh,c
	movlw	low(05h)
	movwf	((c:?_Delay100TCYx)),c
	movf	(??_main+0+0)&0ffh,c,w
	call	_Delay100TCYx	;wreg free
	line	92
	
l989:
;AD.c: 92: ConvertADC();
	call	_ConvertADC	;wreg free
	line	93
;AD.c: 93: while(BusyADC());
	goto	l991
	
l33:
	goto	l991
	
l32:
	
l991:
	call	_BusyADC	;wreg free
	iorlw	0
	btfss	status,2
	goto	u331
	goto	u330
u331:
	goto	l991
u330:
	goto	l993
	
l34:
	line	94
	
l993:
;AD.c: 94: data = ReadADC();
	call	_ReadADC	;wreg free
	movff	0+?_ReadADC,(c:main@data)
	movff	1+?_ReadADC,(c:main@data+1)
	movlw	0
	btfsc	((c:main@data+1)),c,7
	movlw	-1
	movwf	((c:main@data+2)),c
	movwf	((c:main@data+3)),c
	line	97
	
l995:
;AD.c: 97: printf("\rdata=%ld\n",data);
	movlw	high(STR_2)
	movwf	((c:?_printf+1)),c
	movlw	low(STR_2)
	movwf	((c:?_printf)),c
	movff	(c:main@data),0+((c:?_printf)+02h)
	movff	(c:main@data+1),1+((c:?_printf)+02h)
	movff	(c:main@data+2),2+((c:?_printf)+02h)
	movff	(c:main@data+3),3+((c:?_printf)+02h)
	call	_printf	;wreg free
	goto	l985
	line	140
	
l35:
	line	86
	goto	l985
	
l36:
	line	146
	
l37:
	global	start
	goto	start
	opt stack 0
GLOBAL	__end_of_main
	__end_of_main:
	signat	_main,88
	global	_ReadADC

;; *************** function _ReadADC *****************
;; Defined at:
;;		line 25 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcread.c"
;; Parameters:    Size  Location     Type
;;		None
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;                  2    0[COMRAM] int 
;; Registers used:
;;		wreg, status,2
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         2       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          0       0       0       0       0       0       0       0       0
;;      Totals:         2       0       0       0       0       0       0       0       0
;;Total ram usage:        2 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_main
;; This function uses a non-reentrant model
;;
psect	text1,class=CODE,space=0,reloc=2
global __ptext1
__ptext1:
psect	text1
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcread.c"
	line	25
	global	__size_of_ReadADC
	__size_of_ReadADC	equ	__end_of_ReadADC-_ReadADC
	
_ReadADC:
;incstack = 0
	opt	stack 30
;incstack = 0
	line	26
	
l851:
	movf	((c:4036)),c,w	;volatile
	movwf	((c:?_ReadADC+1)),c
	movf	((c:4035)),c,w	;volatile
	movwf	((c:?_ReadADC)),c
	goto	l106
	
l853:
	line	27
	
l106:
	return
	opt stack 0
GLOBAL	__end_of_ReadADC
	__end_of_ReadADC:
	signat	_ReadADC,90
	global	_BusyADC

;; *************** function _BusyADC *****************
;; Defined at:
;;		line 27 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcbusy.c"
;; Parameters:    Size  Location     Type
;;		None
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;                  1    wreg      unsigned char 
;; Registers used:
;;		wreg, status,2, status,0
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         0       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          0       0       0       0       0       0       0       0       0
;;      Totals:         0       0       0       0       0       0       0       0       0
;;Total ram usage:        0 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_main
;; This function uses a non-reentrant model
;;
psect	text2,class=CODE,space=0,reloc=2
global __ptext2
__ptext2:
psect	text2
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcbusy.c"
	line	27
	global	__size_of_BusyADC
	__size_of_BusyADC	equ	__end_of_BusyADC-_BusyADC
	
_BusyADC:
;incstack = 0
	opt	stack 30
;incstack = 0
	line	28
	
l847:
	rrcf	((c:4034)),c,w	;volatile
	andlw	(1<<1)-1
	goto	l76
	
l849:
	line	29
	
l76:
	return
	opt stack 0
GLOBAL	__end_of_BusyADC
	__end_of_BusyADC:
	signat	_BusyADC,89
	global	_ConvertADC

;; *************** function _ConvertADC *****************
;; Defined at:
;;		line 23 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcconv.c"
;; Parameters:    Size  Location     Type
;;		None
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;		None               void
;; Registers used:
;;		None
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         0       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          0       0       0       0       0       0       0       0       0
;;      Totals:         0       0       0       0       0       0       0       0       0
;;Total ram usage:        0 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_main
;; This function uses a non-reentrant model
;;
psect	text3,class=CODE,space=0,reloc=2
global __ptext3
__ptext3:
psect	text3
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcconv.c"
	line	23
	global	__size_of_ConvertADC
	__size_of_ConvertADC	equ	__end_of_ConvertADC-_ConvertADC
	
_ConvertADC:
;incstack = 0
	opt	stack 30
;incstack = 0
	line	24
	
l845:
	bsf	((c:4034)),c,1	;volatile
	line	25
	
l81:
	return
	opt stack 0
GLOBAL	__end_of_ConvertADC
	__end_of_ConvertADC:
	signat	_ConvertADC,88
	global	_Delay100TCYx

;; *************** function _Delay100TCYx *****************
;; Defined at:
;;		line 9 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\d100tcyx.c"
;; Parameters:    Size  Location     Type
;;  unit            1    0[COMRAM] unsigned char 
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;		None               void
;; Registers used:
;;		wreg
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         1       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          0       0       0       0       0       0       0       0       0
;;      Totals:         1       0       0       0       0       0       0       0       0
;;Total ram usage:        1 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_main
;; This function uses a non-reentrant model
;;
psect	text4,class=CODE,space=0,reloc=2
global __ptext4
__ptext4:
psect	text4
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\d100tcyx.c"
	line	9
	global	__size_of_Delay100TCYx
	__size_of_Delay100TCYx	equ	__end_of_Delay100TCYx-_Delay100TCYx
	
_Delay100TCYx:
;incstack = 0
	opt	stack 30
;incstack = 0
	line	10
	
l378:
	line	11
	
l887:
	movlw	33
u347:
decfsz	wreg,f
	goto	u347
	nop

	line	12
	
l889:
	decfsz	((c:Delay100TCYx@unit)),c
	
	goto	l378
	goto	l380
	
l379:
	line	13
	
l380:
	return
	opt stack 0
GLOBAL	__end_of_Delay100TCYx
	__end_of_Delay100TCYx:
	signat	_Delay100TCYx,4216
	global	_printf

;; *************** function _printf *****************
;; Defined at:
;;		line 465 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\common\doprnt.c"
;; Parameters:    Size  Location     Type
;;  f               2   24[COMRAM] PTR const unsigned char 
;;		 -> STR_2(11), STR_1(14), 
;; Auto vars:     Size  Location     Type
;;  _val            5   37[COMRAM] struct .
;;  ap              2   34[COMRAM] PTR void [1]
;;		 -> ?_printf(2), 
;;  prec            2   32[COMRAM] int 
;;  c               1   42[COMRAM] char 
;;  flag            1   36[COMRAM] unsigned char 
;; Return value:  Size  Location     Type
;;                  2   24[COMRAM] int 
;; Registers used:
;;		wreg, fsr2l, fsr2h, status,2, status,0, tblptrl, tblptrh, tblptru, prodl, prodh, cstack
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         6       0       0       0       0       0       0       0       0
;;      Locals:        11       0       0       0       0       0       0       0       0
;;      Temps:          2       0       0       0       0       0       0       0       0
;;      Totals:        19       0       0       0       0       0       0       0       0
;;Total ram usage:       19 bytes
;; Hardware stack levels used:    1
;; Hardware stack levels required when called:    1
;; This function calls:
;;		_putch
;;		___lldiv
;;		___llmod
;; This function is called by:
;;		_main
;; This function uses a non-reentrant model
;;
psect	text5,class=CODE,space=0,reloc=2
global __ptext5
__ptext5:
psect	text5
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\common\doprnt.c"
	line	465
	global	__size_of_printf
	__size_of_printf	equ	__end_of_printf-_printf
	
_printf:
;incstack = 0
	opt	stack 29
;incstack = 0
	line	542
	
l907:
;doprnt.c: 466: va_list ap;
;doprnt.c: 499: signed char c;
;doprnt.c: 504: int prec;
;doprnt.c: 508: unsigned char flag;
;doprnt.c: 527: union {
;doprnt.c: 528: unsigned long _val;
;doprnt.c: 529: struct {
;doprnt.c: 530: const char * _cp;
;doprnt.c: 531: unsigned _len;
;doprnt.c: 532: } _str;
;doprnt.c: 533: } _val;
;doprnt.c: 542: *ap = __va_start();
	movlw	high((c:?_printf)+02h)
	movwf	((c:printf@ap+1)),c
	movlw	low((c:?_printf)+02h)
	movwf	((c:printf@ap)),c
	line	545
;doprnt.c: 545: while(c = *f++) {
	goto	l961
	
l49:
	line	547
	
l909:
;doprnt.c: 547: if(c != '%')
	movf	((c:printf@c)),c,w
	xorlw	37

	btfsc	status,2
	goto	u221
	goto	u220
u221:
	goto	l913
u220:
	line	550
	
l911:
;doprnt.c: 549: {
;doprnt.c: 550: (putch(c) );
	movff	(c:printf@c),(c:?_putch)
	call	_putch	;wreg free
	line	551
;doprnt.c: 551: continue;
	goto	l961
	line	552
	
l50:
	line	557
	
l913:
;doprnt.c: 552: }
;doprnt.c: 557: flag = 0;
	movwf	(??_printf+0+0)&0ffh,c
	movlw	low(0)
	movwf	((c:printf@flag)),c
	movf	(??_printf+0+0)&0ffh,c,w
	goto	l919
	line	642
;doprnt.c: 642: loop:
	
l51:
	line	644
;doprnt.c: 644: switch(c = *f++) {
	goto	l919
	line	646
;doprnt.c: 646: case 0:
	
l53:
	line	647
;doprnt.c: 647: goto alldone;
	goto	l71
	line	650
;doprnt.c: 650: case 'l':
	
l55:
	line	652
	
l915:
;doprnt.c: 652: flag |= 0x10;
	bsf	(0+(4/8)+(c:printf@flag)),c,(4)&7
	line	653
;doprnt.c: 653: goto loop;
	goto	l919
	line	706
;doprnt.c: 706: case 'd':
	
l56:
	goto	l58
	line	707
	
l57:
	line	708
;doprnt.c: 707: case 'i':
;doprnt.c: 708: break;
	goto	l58
	line	811
;doprnt.c: 811: default:
	
l59:
	line	822
;doprnt.c: 822: continue;
	goto	l961
	line	831
	
l917:
;doprnt.c: 831: }
	goto	l58
	line	644
	
l52:
	
l919:
	movff	(c:printf@f),tblptrl
	movff	(c:printf@f+1),tblptrh
	infsnz	((c:printf@f)),c
	incf	((c:printf@f+1)),c
	tblrd	*
	
	movff	tablat,(c:printf@c)
	movf	((c:printf@c))&0ffh,w
	; Switch size 1, requested type "space"
; Number of cases is 4, Range of values is 0 to 108
; switch strategies available:
; Name         Instructions Cycles
; simple_byte           13     7 (average)
;	Chosen strategy is simple_byte

	xorlw	0^0	; case 0
	skipnz
	goto	l71
	xorlw	100^0	; case 100
	skipnz
	goto	l58
	xorlw	105^100	; case 105
	skipnz
	goto	l58
	xorlw	108^105	; case 108
	skipnz
	goto	l915
	goto	l961

	line	831
	
l58:
	line	1262
;doprnt.c: 1260: {
;doprnt.c: 1262: if(flag & 0x10)
	
	btfss	((c:printf@flag)),c,(4)&7
	goto	u231
	goto	u230
u231:
	goto	l925
u230:
	line	1263
	
l921:
;doprnt.c: 1263: _val._val = (*(long *)__va_arg((*(long **)ap), (long)0));
	movff	(c:printf@ap),fsr2l
	movff	(c:printf@ap+1),fsr2h
	movff	postinc2,(c:printf@_val)
	movff	postinc2,(c:printf@_val+1)
	movff	postinc2,(c:printf@_val+2)
	movff	postinc2,(c:printf@_val+3)
	
l923:
	movlw	04h
	addwf	((c:printf@ap)),c
	movlw	0
	addwfc	((c:printf@ap+1)),c
	goto	l929
	line	1264
	
l60:
	line	1266
	
l925:
;doprnt.c: 1264: else
;doprnt.c: 1266: _val._val = (long)(*(int *)__va_arg((*(int **)ap), (int)0));
	movff	(c:printf@ap),fsr2l
	movff	(c:printf@ap+1),fsr2h
	movff	postinc2,(c:printf@_val)
	movff	postdec2,(c:printf@_val+1)
	movlw	0
	btfsc	((c:printf@_val+1)),c,7
	movlw	-1
	movwf	((c:printf@_val+2)),c
	movwf	((c:printf@_val+3)),c
	
l927:
	movlw	02h
	addwf	((c:printf@ap)),c
	movlw	0
	addwfc	((c:printf@ap+1)),c
	goto	l929
	
l61:
	line	1268
	
l929:
;doprnt.c: 1268: if((long)_val._val < 0) {
	btfss	((c:printf@_val+3)),c,7
	goto	u241
	goto	u240
u241:
	goto	l935
u240:
	line	1269
	
l931:
;doprnt.c: 1269: flag |= 0x03;
	movlw	(03h)&0ffh
	iorwf	((c:printf@flag)),c
	line	1270
	
l933:
;doprnt.c: 1270: _val._val = -_val._val;
	comf	((c:printf@_val+3)),c
	comf	((c:printf@_val+2)),c
	comf	((c:printf@_val+1)),c
	negf	((c:printf@_val)),c
	movlw	0
	addwfc	((c:printf@_val+1)),c
	addwfc	((c:printf@_val+2)),c
	addwfc	((c:printf@_val+3)),c
	goto	l935
	line	1271
	
l62:
	line	1312
	
l935:
;doprnt.c: 1271: }
;doprnt.c: 1273: }
;doprnt.c: 1312: for(c = 1 ; c != sizeof dpowers/sizeof dpowers[0] ; c++)
	movwf	(??_printf+0+0)&0ffh,c
	movlw	low(01h)
	movwf	((c:printf@c)),c
	movf	(??_printf+0+0)&0ffh,c,w
	
l937:
	movf	((c:printf@c)),c,w
	xorlw	10

	btfss	status,2
	goto	u251
	goto	u250
u251:
	goto	l941
u250:
	goto	l949
	
l939:
	goto	l949
	line	1313
	
l63:
	
l941:
;doprnt.c: 1313: if(_val._val < dpowers[c])
	movf	((c:printf@c)),c,w
	mullw	04h
	movlw	low((_dpowers))
	addwf	(prodl),c,w
	movwf	tblptrl
	movlw	high((_dpowers))
	addwfc	(prodh),c,w
	movwf	tblptrh
	tblrd	*+
	
	movf	tablat,w
	subwf	((c:printf@_val)),c,w
	tblrd	*+
	
	movf	tablat,w
	subwfb	((c:printf@_val+1)),c,w
	tblrd	*+
	
	movf	tablat,w
	subwfb	((c:printf@_val+2)),c,w
	tblrd	*+
	
	movf	tablat,w
	subwfb	((c:printf@_val+3)),c,w
	btfsc	status,0
	goto	u261
	goto	u260
u261:
	goto	l945
u260:
	goto	l949
	line	1314
	
l943:
;doprnt.c: 1314: break;
	goto	l949
	
l65:
	line	1312
	
l945:
	incf	((c:printf@c)),c
	
l947:
	movf	((c:printf@c)),c,w
	xorlw	10

	btfss	status,2
	goto	u271
	goto	u270
u271:
	goto	l941
u270:
	goto	l949
	
l64:
	line	1445
	
l949:
;doprnt.c: 1429: {
;doprnt.c: 1445: if(flag & 0x03)
	movf	((c:printf@flag)),c,w
	andlw	low(03h)
	btfsc	status,2
	goto	u281
	goto	u280
u281:
	goto	l953
u280:
	line	1446
	
l951:
;doprnt.c: 1446: (putch('-') );
	movwf	(??_printf+0+0)&0ffh,c
	movlw	low(02Dh)
	movwf	((c:?_putch)),c
	movf	(??_printf+0+0)&0ffh,c,w
	call	_putch	;wreg free
	goto	l953
	
l66:
	line	1479
	
l953:
;doprnt.c: 1476: }
;doprnt.c: 1479: prec = c;
	movf	((c:printf@c)),c,w
	movwf	((c:printf@prec)),c
	clrf	((c:printf@prec+1)),c
	btfsc	((c:printf@prec)),c,7
	decf	((c:printf@prec+1)),c
	line	1481
;doprnt.c: 1481: while(prec--) {
	goto	l959
	
l68:
	line	1496
	
l955:
;doprnt.c: 1485: {
;doprnt.c: 1496: c = (_val._val / dpowers[prec]) % 10 + '0';
	movff	(c:printf@prec),??_printf+0+0
	movff	(c:printf@prec+1),??_printf+0+0+1
	bcf	status,0
	rlcf	(??_printf+0+0),c
	rlcf	(??_printf+0+1),c
	bcf	status,0
	rlcf	(??_printf+0+0),c
	rlcf	(??_printf+0+1),c
	movlw	low((_dpowers))
	addwf	(??_printf+0+0),c,w
	movwf	tblptrl
	movlw	high((_dpowers))
	addwfc	(??_printf+0+1),c,w
	movwf	tblptrh
	tblrd*+
	
	movff	tablat,0+((c:?___lldiv)+04h)
	tblrd*+
	
	movff	tablat,1+((c:?___lldiv)+04h)
	tblrd*+
	
	movff	tablat,2+((c:?___lldiv)+04h)
	tblrd*-
	
	movff	tablat,3+((c:?___lldiv)+04h)
	movff	(c:printf@_val),(c:?___lldiv)
	movff	(c:printf@_val+1),(c:?___lldiv+1)
	movff	(c:printf@_val+2),(c:?___lldiv+2)
	movff	(c:printf@_val+3),(c:?___lldiv+3)
	call	___lldiv	;wreg free
	
	movff	3+?___lldiv,(c:?___llmod+3)
	movff	2+?___lldiv,(c:?___llmod+2)
	movff	1+?___lldiv,(c:?___llmod+1)
	movff	0+?___lldiv,(c:?___llmod)
	
	movlw	low(0Ah)
	movwf	(0+((c:?___llmod)+04h)),c
	movlw	high(0Ah)
	movwf	(1+((c:?___llmod)+04h)),c
	movlw	low highword(0Ah)
	movwf	(2+((c:?___llmod)+04h)),c
	movlw	high highword(0Ah)
	movwf	(3+((c:?___llmod)+04h)),c
	call	___llmod	;wreg free
	movf	(0+?___llmod),c,w
	addlw	low(030h)
	movwf	((c:printf@c)),c
	line	1531
	
l957:
;doprnt.c: 1530: }
;doprnt.c: 1531: (putch(c) );
	movff	(c:printf@c),(c:?_putch)
	call	_putch	;wreg free
	goto	l959
	line	1532
	
l67:
	line	1481
	
l959:
	decf	((c:printf@prec)),c
	btfss	status,0
	decf	((c:printf@prec+1)),c
	incf	((c:printf@prec))&0ffh,w
	btfsc	status,2
	incf ((c:printf@prec+1))&0ffh,w

	btfss	status,2
	goto	u291
	goto	u290
u291:
	goto	l955
u290:
	goto	l961
	
l69:
	goto	l961
	line	1540
	
l48:
	line	545
	
l961:
	movff	(c:printf@f),tblptrl
	movff	(c:printf@f+1),tblptrh
	infsnz	((c:printf@f)),c
	incf	((c:printf@f+1)),c
	tblrd	*
	
	movff	tablat,(c:printf@c)
	tstfsz	((c:printf@c))&0ffh
	goto	u301
	goto	u300
u301:
	goto	l909
u300:
	goto	l71
	
l70:
	goto	l71
	line	1542
;doprnt.c: 1532: }
;doprnt.c: 1540: }
;doprnt.c: 1542: alldone:
	
l54:
	line	1548
;doprnt.c: 1547: return 0;
;	Return value of _printf is never used
	
l71:
	return
	opt stack 0
GLOBAL	__end_of_printf
	__end_of_printf:
	signat	_printf,602
	global	___llmod

;; *************** function ___llmod *****************
;; Defined at:
;;		line 10 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\common\llmod.c"
;; Parameters:    Size  Location     Type
;;  dividend        4   14[COMRAM] unsigned long 
;;  divisor         4   18[COMRAM] unsigned long 
;; Auto vars:     Size  Location     Type
;;  counter         1   23[COMRAM] unsigned char 
;; Return value:  Size  Location     Type
;;                  4   14[COMRAM] unsigned long 
;; Registers used:
;;		wreg, status,2, status,0
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         8       0       0       0       0       0       0       0       0
;;      Locals:         1       0       0       0       0       0       0       0       0
;;      Temps:          1       0       0       0       0       0       0       0       0
;;      Totals:        10       0       0       0       0       0       0       0       0
;;Total ram usage:       10 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_printf
;; This function uses a non-reentrant model
;;
psect	text6,class=CODE,space=0,reloc=2
global __ptext6
__ptext6:
psect	text6
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\common\llmod.c"
	line	10
	global	__size_of___llmod
	__size_of___llmod	equ	__end_of___llmod-___llmod
	
___llmod:
;incstack = 0
	opt	stack 29
;incstack = 0
	line	13
	
l891:
	movf	((c:___llmod@divisor+3)),c,w
	iorwf ((c:___llmod@divisor)),c,w
	iorwf ((c:___llmod@divisor+1)),c,w
	iorwf ((c:___llmod@divisor+2)),c,w

	btfsc	status,2
	goto	u191
	goto	u190
u191:
	goto	l621
u190:
	line	14
	
l893:
	movwf	(??___llmod+0+0)&0ffh,c
	movlw	low(01h)
	movwf	((c:___llmod@counter)),c
	movf	(??___llmod+0+0)&0ffh,c,w
	line	15
	goto	l897
	
l623:
	line	16
	
l895:
	bcf	status,0
	rlcf	((c:___llmod@divisor)),c
	rlcf	((c:___llmod@divisor+1)),c
	rlcf	((c:___llmod@divisor+2)),c
	rlcf	((c:___llmod@divisor+3)),c
	line	17
	incf	((c:___llmod@counter)),c
	goto	l897
	line	18
	
l622:
	line	15
	
l897:
	
	btfss	((c:___llmod@divisor+3)),c,(31)&7
	goto	u201
	goto	u200
u201:
	goto	l895
u200:
	goto	l899
	
l624:
	goto	l899
	line	19
	
l625:
	line	20
	
l899:
	movf	((c:___llmod@divisor)),c,w
	subwf	((c:___llmod@dividend)),c,w
	movf	((c:___llmod@divisor+1)),c,w
	subwfb	((c:___llmod@dividend+1)),c,w
	movf	((c:___llmod@divisor+2)),c,w
	subwfb	((c:___llmod@dividend+2)),c,w
	movf	((c:___llmod@divisor+3)),c,w
	subwfb	((c:___llmod@dividend+3)),c,w
	btfss	status,0
	goto	u211
	goto	u210
u211:
	goto	l903
u210:
	line	21
	
l901:
	movf	((c:___llmod@divisor)),c,w
	subwf	((c:___llmod@dividend)),c
	movf	((c:___llmod@divisor+1)),c,w
	subwfb	((c:___llmod@dividend+1)),c
	movf	((c:___llmod@divisor+2)),c,w
	subwfb	((c:___llmod@dividend+2)),c
	movf	((c:___llmod@divisor+3)),c,w
	subwfb	((c:___llmod@dividend+3)),c
	goto	l903
	
l626:
	line	22
	
l903:
	bcf	status,0
	rrcf	((c:___llmod@divisor+3)),c
	rrcf	((c:___llmod@divisor+2)),c
	rrcf	((c:___llmod@divisor+1)),c
	rrcf	((c:___llmod@divisor)),c
	line	23
	
l905:
	decfsz	((c:___llmod@counter)),c
	
	goto	l899
	goto	l621
	
l627:
	line	24
	
l621:
	line	25
	movff	(c:___llmod@dividend),(c:?___llmod)
	movff	(c:___llmod@dividend+1),(c:?___llmod+1)
	movff	(c:___llmod@dividend+2),(c:?___llmod+2)
	movff	(c:___llmod@dividend+3),(c:?___llmod+3)
	line	26
	
l628:
	return
	opt stack 0
GLOBAL	__end_of___llmod
	__end_of___llmod:
	signat	___llmod,8316
	global	___lldiv

;; *************** function ___lldiv *****************
;; Defined at:
;;		line 10 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\common\lldiv.c"
;; Parameters:    Size  Location     Type
;;  dividend        4    0[COMRAM] unsigned long 
;;  divisor         4    4[COMRAM] unsigned long 
;; Auto vars:     Size  Location     Type
;;  quotient        4    9[COMRAM] unsigned long 
;;  counter         1   13[COMRAM] unsigned char 
;; Return value:  Size  Location     Type
;;                  4    0[COMRAM] unsigned long 
;; Registers used:
;;		wreg, status,2, status,0
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         8       0       0       0       0       0       0       0       0
;;      Locals:         5       0       0       0       0       0       0       0       0
;;      Temps:          1       0       0       0       0       0       0       0       0
;;      Totals:        14       0       0       0       0       0       0       0       0
;;Total ram usage:       14 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_printf
;; This function uses a non-reentrant model
;;
psect	text7,class=CODE,space=0,reloc=2
global __ptext7
__ptext7:
psect	text7
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\common\lldiv.c"
	line	10
	global	__size_of___lldiv
	__size_of___lldiv	equ	__end_of___lldiv-___lldiv
	
___lldiv:
;incstack = 0
	opt	stack 29
;incstack = 0
	line	14
	
l861:
	movlw	low(0)
	movwf	((c:___lldiv@quotient)),c
	movlw	high(0)
	movwf	((c:___lldiv@quotient+1)),c
	movlw	low highword(0)
	movwf	((c:___lldiv@quotient+2)),c
	movlw	high highword(0)
	movwf	((c:___lldiv@quotient+3)),c
	line	15
	
l863:
	movf	((c:___lldiv@divisor+3)),c,w
	iorwf ((c:___lldiv@divisor)),c,w
	iorwf ((c:___lldiv@divisor+1)),c,w
	iorwf ((c:___lldiv@divisor+2)),c,w

	btfsc	status,2
	goto	u141
	goto	u140
u141:
	goto	l611
u140:
	line	16
	
l865:
	movwf	(??___lldiv+0+0)&0ffh,c
	movlw	low(01h)
	movwf	((c:___lldiv@counter)),c
	movf	(??___lldiv+0+0)&0ffh,c,w
	line	17
	goto	l869
	
l613:
	line	18
	
l867:
	bcf	status,0
	rlcf	((c:___lldiv@divisor)),c
	rlcf	((c:___lldiv@divisor+1)),c
	rlcf	((c:___lldiv@divisor+2)),c
	rlcf	((c:___lldiv@divisor+3)),c
	line	19
	incf	((c:___lldiv@counter)),c
	goto	l869
	line	20
	
l612:
	line	17
	
l869:
	
	btfss	((c:___lldiv@divisor+3)),c,(31)&7
	goto	u151
	goto	u150
u151:
	goto	l867
u150:
	goto	l871
	
l614:
	goto	l871
	line	21
	
l615:
	line	22
	
l871:
	bcf	status,0
	rlcf	((c:___lldiv@quotient)),c
	rlcf	((c:___lldiv@quotient+1)),c
	rlcf	((c:___lldiv@quotient+2)),c
	rlcf	((c:___lldiv@quotient+3)),c
	line	23
	
l873:
	movf	((c:___lldiv@divisor)),c,w
	subwf	((c:___lldiv@dividend)),c,w
	movf	((c:___lldiv@divisor+1)),c,w
	subwfb	((c:___lldiv@dividend+1)),c,w
	movf	((c:___lldiv@divisor+2)),c,w
	subwfb	((c:___lldiv@dividend+2)),c,w
	movf	((c:___lldiv@divisor+3)),c,w
	subwfb	((c:___lldiv@dividend+3)),c,w
	btfss	status,0
	goto	u161
	goto	u160
u161:
	goto	l879
u160:
	line	24
	
l875:
	movf	((c:___lldiv@divisor)),c,w
	subwf	((c:___lldiv@dividend)),c
	movf	((c:___lldiv@divisor+1)),c,w
	subwfb	((c:___lldiv@dividend+1)),c
	movf	((c:___lldiv@divisor+2)),c,w
	subwfb	((c:___lldiv@dividend+2)),c
	movf	((c:___lldiv@divisor+3)),c,w
	subwfb	((c:___lldiv@dividend+3)),c
	line	25
	
l877:
	bsf	(0+(0/8)+(c:___lldiv@quotient)),c,(0)&7
	goto	l879
	line	26
	
l616:
	line	27
	
l879:
	bcf	status,0
	rrcf	((c:___lldiv@divisor+3)),c
	rrcf	((c:___lldiv@divisor+2)),c
	rrcf	((c:___lldiv@divisor+1)),c
	rrcf	((c:___lldiv@divisor)),c
	line	28
	
l881:
	decfsz	((c:___lldiv@counter)),c
	
	goto	l871
	goto	l611
	
l617:
	line	29
	
l611:
	line	30
	movff	(c:___lldiv@quotient),(c:?___lldiv)
	movff	(c:___lldiv@quotient+1),(c:?___lldiv+1)
	movff	(c:___lldiv@quotient+2),(c:?___lldiv+2)
	movff	(c:___lldiv@quotient+3),(c:?___lldiv+3)
	line	31
	
l618:
	return
	opt stack 0
GLOBAL	__end_of___lldiv
	__end_of___lldiv:
	signat	___lldiv,8316
	global	_putch

;; *************** function _putch *****************
;; Defined at:
;;		line 8 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\common\putch.c"
;; Parameters:    Size  Location     Type
;;  c               1    0[COMRAM] unsigned char 
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;		None               void
;; Registers used:
;;		None
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         1       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          0       0       0       0       0       0       0       0       0
;;      Totals:         1       0       0       0       0       0       0       0       0
;;Total ram usage:        1 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_printf
;; This function uses a non-reentrant model
;;
psect	text8,class=CODE,space=0,reloc=2
global __ptext8
__ptext8:
psect	text8
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\common\putch.c"
	line	8
	global	__size_of_putch
	__size_of_putch	equ	__end_of_putch-_putch
	
_putch:
;incstack = 0
	opt	stack 29
;incstack = 0
	line	9
	
l710:
	return
	opt stack 0
GLOBAL	__end_of_putch
	__end_of_putch:
	signat	_putch,4216
	global	_putsUSART

;; *************** function _putsUSART *****************
;; Defined at:
;;		line 15 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\USART\uputs.c"
;; Parameters:    Size  Location     Type
;;  data            2    1[COMRAM] PTR unsigned char 
;;		 -> main@Message1(10), 
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;		None               void
;; Registers used:
;;		wreg, fsr2l, fsr2h, status,2, status,0, cstack
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         2       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          0       0       0       0       0       0       0       0       0
;;      Totals:         2       0       0       0       0       0       0       0       0
;;Total ram usage:        2 bytes
;; Hardware stack levels used:    1
;; Hardware stack levels required when called:    1
;; This function calls:
;;		_WriteUSART
;; This function is called by:
;;		_main
;; This function uses a non-reentrant model
;;
psect	text9,class=CODE,space=0,reloc=2
global __ptext9
__ptext9:
psect	text9
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\USART\uputs.c"
	line	15
	global	__size_of_putsUSART
	__size_of_putsUSART	equ	__end_of_putsUSART-_putsUSART
	
_putsUSART:
;incstack = 0
	opt	stack 29
;incstack = 0
	line	16
	
l151:
	line	18
	goto	l152
	
l153:
	
l152:
	
	btfss	((c:4012)),c,1	;volatile
	goto	u171
	goto	u170
u171:
	goto	l152
u170:
	goto	l883
	
l154:
	line	19
	
l883:
	movff	(c:putsUSART@data),fsr2l
	movff	(c:putsUSART@data+1),fsr2h
	movf	indf2,w
	movwf	((c:?_WriteUSART)),c
	call	_WriteUSART	;wreg free
	line	20
	
l885:
	movff	(c:putsUSART@data),fsr2l
	movff	(c:putsUSART@data+1),fsr2h
	infsnz	((c:putsUSART@data)),c
	incf	((c:putsUSART@data+1)),c
	movf	indf2,w
	btfss	status,2
	goto	u181
	goto	u180
u181:
	goto	l151
u180:
	goto	l156
	
l155:
	line	21
	
l156:
	return
	opt stack 0
GLOBAL	__end_of_putsUSART
	__end_of_putsUSART:
	signat	_putsUSART,4216
	global	_WriteUSART

;; *************** function _WriteUSART *****************
;; Defined at:
;;		line 14 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\USART\uwrite.c"
;; Parameters:    Size  Location     Type
;;  data            1    0[COMRAM] unsigned char 
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;		None               void
;; Registers used:
;;		None
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         1       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          0       0       0       0       0       0       0       0       0
;;      Totals:         1       0       0       0       0       0       0       0       0
;;Total ram usage:        1 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_putsUSART
;; This function uses a non-reentrant model
;;
psect	text10,class=CODE,space=0,reloc=2
global __ptext10
__ptext10:
psect	text10
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\USART\uwrite.c"
	line	14
	global	__size_of_WriteUSART
	__size_of_WriteUSART	equ	__end_of_WriteUSART-_WriteUSART
	
_WriteUSART:
;incstack = 0
	opt	stack 29
;incstack = 0
	line	15
	
l855:
	
	btfss	((c:4012)),c,6	;volatile
	goto	u121
	goto	u120
u121:
	goto	l165
u120:
	line	17
	
l857:
	bcf	((c:4012)),c,0	;volatile
	line	18
	
	btfss	((c:_USART_Status)),c,1
	goto	u131
	goto	u130
u131:
	goto	l165
u130:
	line	19
	
l859:
	bsf	((c:4012)),c,0	;volatile
	goto	l165
	
l166:
	line	20
	
l165:
	line	22
	movff	(c:WriteUSART@data),(c:4013)	;volatile
	line	23
	
l167:
	return
	opt stack 0
GLOBAL	__end_of_WriteUSART
	__end_of_WriteUSART:
	signat	_WriteUSART,4216
	global	_SetChanADC

;; *************** function _SetChanADC *****************
;; Defined at:
;;		line 31 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcsetch.c"
;; Parameters:    Size  Location     Type
;;  channel         1    0[COMRAM] unsigned char 
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;		None               void
;; Registers used:
;;		wreg, status,2, status,0
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         1       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          1       0       0       0       0       0       0       0       0
;;      Totals:         2       0       0       0       0       0       0       0       0
;;Total ram usage:        2 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_main
;; This function uses a non-reentrant model
;;
psect	text11,class=CODE,space=0,reloc=2
global __ptext11
__ptext11:
psect	text11
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcsetch.c"
	line	31
	global	__size_of_SetChanADC
	__size_of_SetChanADC	equ	__end_of_SetChanADC-_SetChanADC
	
_SetChanADC:
;incstack = 0
	opt	stack 30
;incstack = 0
	line	34
	
l843:
	movff	(c:4034),??_SetChanADC+0+0	;volatile
	movlw	0C3h
	andwf	(??_SetChanADC+0+0),c
	bcf	status,0
	rrcf	((c:SetChanADC@channel)),c,w
	andlw	low(03Ch)
	iorwf	(??_SetChanADC+0+0),c,w
	movwf	((c:4034)),c	;volatile
	line	36
	
l111:
	return
	opt stack 0
GLOBAL	__end_of_SetChanADC
	__end_of_SetChanADC:
	signat	_SetChanADC,4216
	global	_OpenADC

;; *************** function _OpenADC *****************
;; Defined at:
;;		line 71 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcopen.c"
;; Parameters:    Size  Location     Type
;;  config          1    0[COMRAM] unsigned char 
;;  config2         1    1[COMRAM] unsigned char 
;;  portconfig      1    2[COMRAM] unsigned char 
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;		None               void
;; Registers used:
;;		wreg, status,2, status,0
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         3       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          2       0       0       0       0       0       0       0       0
;;      Totals:         5       0       0       0       0       0       0       0       0
;;Total ram usage:        5 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_main
;; This function uses a non-reentrant model
;;
psect	text12,class=CODE,space=0,reloc=2
global __ptext12
__ptext12:
psect	text12
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\ADC\adcopen.c"
	line	71
	global	__size_of_OpenADC
	__size_of_OpenADC	equ	__end_of_OpenADC-_OpenADC
	
_OpenADC:
;incstack = 0
	opt	stack 30
;incstack = 0
	line	72
	
l831:
	movlw	low(0)
	movwf	((c:4034)),c	;volatile
	line	73
	movlw	low(0)
	movwf	((c:4032)),c	;volatile
	line	75
	
l833:
	bcf	status,0
	rrcf	((c:OpenADC@config2)),c,w
	andlw	low(03Ch)
	movwf	((c:4034)),c	;volatile
	line	77
	
l835:
	movff	(c:OpenADC@config2),??_OpenADC+0+0
	swapf	(??_OpenADC+0+0),c
	movlw	(0ffh shl 4) & 0ffh
	andwf	(??_OpenADC+0+0),c
	movlw	030h
	andwf	(??_OpenADC+0+0),c
	movf	((c:OpenADC@portconfig)),c,w
	andlw	low(0Fh)
	iorwf	(??_OpenADC+0+0),c,w
	movwf	((c:4033)),c	;volatile
	line	80
	
l837:
	movff	(c:OpenADC@config),??_OpenADC+0+0
	bcf	status,0
	rlcf	(??_OpenADC+0+0),c
	bcf	status,0
	rlcf	(??_OpenADC+0+0),c

	movlw	038h
	andwf	(??_OpenADC+0+0),c
	movff	(c:OpenADC@config),??_OpenADC+1+0
	swapf	(??_OpenADC+1+0),c
	movlw	(0ffh shr 4) & 0ffh
	andwf	(??_OpenADC+1+0),c
	movlw	07h
	andwf	(??_OpenADC+1+0),c
	movf	((c:OpenADC@config)),c,w
	andlw	low(080h)
	iorwf	(??_OpenADC+1+0),c,w
	iorwf	(??_OpenADC+0+0),c,w
	movwf	((c:4032)),c	;volatile
	line	82
	
l839:
	
	btfss	((c:OpenADC@config2)),c,(7)&7
	goto	u111
	goto	u110
u111:
	goto	l98
u110:
	line	84
	
l841:
	bcf	((c:3998)),c,6	;volatile
	line	85
	bsf	((c:3997)),c,6	;volatile
	line	86
	bsf	((c:4082)),c,6	;volatile
	line	87
	
l98:
	line	88
	bsf	((c:4034)),c,0	;volatile
	line	89
	
l99:
	return
	opt stack 0
GLOBAL	__end_of_OpenADC
	__end_of_OpenADC:
	signat	_OpenADC,12408
	global	_OpenUSART

;; *************** function _OpenUSART *****************
;; Defined at:
;;		line 74 in file "C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\USART\uopen.c"
;; Parameters:    Size  Location     Type
;;  config          1    0[COMRAM] unsigned char 
;;  spbrg           2    1[COMRAM] unsigned int 
;; Auto vars:     Size  Location     Type
;;		None
;; Return value:  Size  Location     Type
;;		None               void
;; Registers used:
;;		wreg, status,2
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         3       0       0       0       0       0       0       0       0
;;      Locals:         0       0       0       0       0       0       0       0       0
;;      Temps:          0       0       0       0       0       0       0       0       0
;;      Totals:         3       0       0       0       0       0       0       0       0
;;Total ram usage:        3 bytes
;; Hardware stack levels used:    1
;; This function calls:
;;		Nothing
;; This function is called by:
;;		_main
;; This function uses a non-reentrant model
;;
psect	text13,class=CODE,space=0,reloc=2
global __ptext13
__ptext13:
psect	text13
	file	"C:\Program Files (x86)\Microchip\xc8\v1.21\sources\pic18\plib\USART\uopen.c"
	line	74
	global	__size_of_OpenUSART
	__size_of_OpenUSART	equ	__end_of_OpenUSART-_OpenUSART
	
_OpenUSART:
;incstack = 0
	opt	stack 30
;incstack = 0
	line	75
	
l795:
	movlw	low(0)
	movwf	((c:4012)),c	;volatile
	line	76
	movlw	low(0)
	movwf	((c:4011)),c	;volatile
	line	78
	
l797:
	
	btfss	((c:OpenUSART@config)),c,(0)&7
	goto	u11
	goto	u10
u11:
	goto	l132
u10:
	line	79
	
l799:
	bsf	((c:4012)),c,4	;volatile
	
l132:
	line	81
	
	btfss	((c:OpenUSART@config)),c,(1)&7
	goto	u21
	goto	u20
u21:
	goto	l133
u20:
	line	83
	
l801:
	bsf	((c:4012)),c,6	;volatile
	line	84
	bsf	((c:4011)),c,6	;volatile
	line	85
	
l133:
	line	87
	
	btfss	((c:OpenUSART@config)),c,(2)&7
	goto	u31
	goto	u30
u31:
	goto	l134
u30:
	line	88
	
l803:
	bsf	((c:4012)),c,7	;volatile
	
l134:
	line	90
	
	btfss	((c:OpenUSART@config)),c,(3)&7
	goto	u41
	goto	u40
u41:
	goto	l135
u40:
	line	91
	
l805:
	bsf	((c:4011)),c,4	;volatile
	goto	l136
	line	92
	
l135:
	line	93
	bsf	((c:4011)),c,5	;volatile
	
l136:
	line	95
	
	btfss	((c:OpenUSART@config)),c,(4)&7
	goto	u51
	goto	u50
u51:
	goto	l137
u50:
	line	96
	
l807:
	bsf	((c:4012)),c,2	;volatile
	
l137:
	line	98
	bcf	((c:3998)),c,4	;volatile
	line	100
	
	btfss	((c:OpenUSART@config)),c,(5)&7
	goto	u61
	goto	u60
u61:
	goto	l138
u60:
	line	101
	
l809:
	bsf	((c:4011)),c,3	;volatile
	
l138:
	line	103
	
	btfss	((c:OpenUSART@config)),c,(6)&7
	goto	u71
	goto	u70
u71:
	goto	l139
u70:
	line	104
	
l811:
	bsf	((c:3997)),c,5	;volatile
	goto	l140
	line	105
	
l139:
	line	106
	bcf	((c:3997)),c,5	;volatile
	
l140:
	line	108
	bcf	((c:3998)),c,5	;volatile
	line	110
	
	btfss	((c:OpenUSART@config)),c,(7)&7
	goto	u81
	goto	u80
u81:
	goto	l141
u80:
	line	111
	
l813:
	bsf	((c:3997)),c,4	;volatile
	goto	l142
	line	112
	
l141:
	line	113
	bcf	((c:3997)),c,4	;volatile
	
l142:
	line	115
	movff	(c:OpenUSART@spbrg),(c:4015)	;volatile
	line	116
	
l815:
	movf	((c:OpenUSART@spbrg+1)),c,w
	movwf	((c:4016)),c	;volatile
	line	118
	
l817:
	bsf	((c:4012)),c,5	;volatile
	line	119
	
l819:
	bsf	((c:4011)),c,7	;volatile
	line	137
	
l821:
	bcf	((c:3988)),c,6	;volatile
	
l823:
	bsf	((c:3988)),c,7	;volatile
	line	138
	
l825:
	
	btfss	((c:4012)),c,4	;volatile
	goto	u91
	goto	u90
u91:
	goto	l144
u90:
	
l827:
	
	btfsc	((c:4012)),c,7	;volatile
	goto	u101
	goto	u100
u101:
	goto	l144
u100:
	line	139
	
l829:
	bsf	((c:3988)),c,6	;volatile
	goto	l144
	
l143:
	line	143
	
l144:
	return
	opt stack 0
GLOBAL	__end_of_OpenUSART
	__end_of_OpenUSART:
	signat	_OpenUSART,8312
psect	smallconst
	db 0	; dummy byte at the end
	global	__smallconst
	global	__mediumconst
	GLOBAL	__activetblptr
__activetblptr	EQU	2
	psect	intsave_regs,class=BIGRAM,space=1,noexec
	PSECT	rparam,class=COMRAM,space=1,noexec
	GLOBAL	__Lrparam
	FNCONF	rparam,??,?
GLOBAL	__Lparam, __Hparam
GLOBAL	__Lrparam, __Hrparam
__Lparam	EQU	__Lrparam
__Hparam	EQU	__Hrparam
	end
