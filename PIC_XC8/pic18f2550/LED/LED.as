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
	FNCALL	_main,_InitPortIOReg
	FNROOT	_main
	global	_PORTAbits
_PORTAbits	set	0xF80
	global	_TRISAbits
_TRISAbits	set	0xF92
psect	text0,class=CODE,space=0,reloc=2
global __ptext0
__ptext0:
; #config settings
	file	"LED.as"
	line	#
psect	cinit,class=CODE,delta=1,reloc=2
global __pcinit
__pcinit:
global start_initialization
start_initialization:

global __initialization
__initialization:
psect cinit,class=CODE,delta=1
global end_of_initialization,__end_of__initialization

;End of C runtime variable initialization code

end_of_initialization:
__end_of__initialization:movlb 0
goto _main	;jump to C main() function
psect	cstackCOMRAM,class=COMRAM,space=1,noexec
global __pcstackCOMRAM
__pcstackCOMRAM:
?_InitPortIOReg:	; 0 bytes @ 0x0
??_InitPortIOReg:	; 0 bytes @ 0x0
??_main:	; 0 bytes @ 0x0
?_main:	; 2 bytes @ 0x0
	global	main@i
main@i:	; 2 bytes @ 0x0
	ds   2
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
;!    COMRAM           95      2       2
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
;!    None.


;!
;!Critical Paths under _main in COMRAM
;!
;!    None.
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
;;Main: autosize = 0, tempsize = 0, incstack = 0, save=0
;;

;!
;!Call Graph Tables:
;!
;! ---------------------------------------------------------------------------------
;! (Depth) Function   	        Calls       Base Space   Used Autos Params    Refs
;! ---------------------------------------------------------------------------------
;! (0) _main                                                 2     2      0      30
;!                                              0 COMRAM     2     2      0
;!                      _InitPortIOReg
;! ---------------------------------------------------------------------------------
;! (1) _InitPortIOReg                                        0     0      0       0
;! ---------------------------------------------------------------------------------
;! Estimated maximum stack depth 1
;! ---------------------------------------------------------------------------------
;!
;! Call Graph Graphs:
;!
;! _main (ROOT)
;!   _InitPortIOReg
;!

;! Address spaces:

;!Name               Size   Autos  Total    Cost      Usage
;!BITCOMRAM           5F      0       0       0        0.0%
;!EEDATA             100      0       0       0        0.0%
;!NULL                 0      0       0       0        0.0%
;!CODE                 0      0       0       0        0.0%
;!COMRAM              5F      2       2       1        2.1%
;!STACK                0      0       1       2        0.0%
;!DATA                 0      0       0       3        0.0%
;!BITBANK0            A0      0       0       4        0.0%
;!BANK0               A0      0       0       5        0.0%
;!BITBANK1           100      0       0       6        0.0%
;!BANK1              100      0       0       7        0.0%
;!BITBANK2           100      0       0       8        0.0%
;!BANK2              100      0       0       9        0.0%
;!BITBANK3           100      0       0      10        0.0%
;!BANK3              100      0       0      11        0.0%
;!BITBANK4           100      0       0      12        0.0%
;!BANK4              100      0       0      13        0.0%
;!BITBANK5           100      0       0      14        0.0%
;!BANK5              100      0       0      15        0.0%
;!BITBANK6           100      0       0      16        0.0%
;!BANK6              100      0       0      17        0.0%
;!BITBANK7           100      0       0      18        0.0%
;!BANK7              100      0       0      19        0.0%
;!ABS                  0      0       0      20        0.0%
;!BIGRAM             7FF      0       0      21        0.0%
;!BITSFR               0      0       0      40        0.0%
;!SFR                  0      0       0      40        0.0%

	global	_main

;; *************** function _main *****************
;; Defined at:
;;		line 10 in file "C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic18f2550\LED\LED.c"
;; Parameters:    Size  Location     Type
;;		None
;; Auto vars:     Size  Location     Type
;;  i               2    0[COMRAM] int 
;; Return value:  Size  Location     Type
;;                  2    8[COMRAM] int 
;; Registers used:
;;		wreg, status,2, status,0, cstack
;; Tracked objects:
;;		On entry : 0/0
;;		On exit  : 0/0
;;		Unchanged: 0/0
;; Data sizes:     COMRAM   BANK0   BANK1   BANK2   BANK3   BANK4   BANK5   BANK6   BANK7
;;      Params:         0       0       0       0       0       0       0       0       0
;;      Locals:         2       0       0       0       0       0       0       0       0
;;      Temps:          0       0       0       0       0       0       0       0       0
;;      Totals:         2       0       0       0       0       0       0       0       0
;;Total ram usage:        2 bytes
;; Hardware stack levels required when called:    1
;; This function calls:
;;		_InitPortIOReg
;; This function is called by:
;;		Startup code after reset
;; This function uses a non-reentrant model
;;
psect	text0
psect	text0
	file	"C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic18f2550\LED\LED.c"
	line	10
	global	__size_of_main
	__size_of_main	equ	__end_of_main-_main
	
_main:
;incstack = 0
	opt	stack 30
;incstack = 0
	line	12
	
l595:
;LED.c: 12: int i = 0;
	movlw	high(0)
	movwf	((c:main@i+1)),c
	movlw	low(0)
	movwf	((c:main@i)),c
	line	14
	
l597:
;LED.c: 14: InitPortIOReg();
	call	_InitPortIOReg	;wreg free
	goto	l599
	line	16
;LED.c: 16: while(1)
	
l9:
	line	18
	
l599:
;LED.c: 17: {
;LED.c: 18: PORTAbits.RA0 ^= 1;
	btg	((c:3968)),c,0	;volatile
	line	19
	
l601:
;LED.c: 19: for(i=0;i<1000;i++);
	movlw	high(0)
	movwf	((c:main@i+1)),c
	movlw	low(0)
	movwf	((c:main@i)),c
	
l603:
	movf	((c:main@i+1)),c,w
	xorlw	80h
	addlw	-((03h)^80h)
	movlw	0E8h
	btfsc	status,2
	subwf	((c:main@i)),c,w
	btfss	status,0
	goto	u11
	goto	u10
u11:
	goto	l607
u10:
	goto	l599
	
l605:
	goto	l599
	
l10:
	
l607:
	infsnz	((c:main@i)),c
	incf	((c:main@i+1)),c
	
l609:
	movf	((c:main@i+1)),c,w
	xorlw	80h
	addlw	-((03h)^80h)
	movlw	0E8h
	btfsc	status,2
	subwf	((c:main@i)),c,w
	btfss	status,0
	goto	u21
	goto	u20
u21:
	goto	l607
u20:
	goto	l599
	
l11:
	goto	l599
	line	20
	
l12:
	line	16
	goto	l599
	
l13:
	line	24
;LED.c: 20: }
;LED.c: 22: return (0);
;	Return value of _main is never used
	
l14:
	global	start
	goto	start
	opt stack 0
GLOBAL	__end_of_main
	__end_of_main:
	signat	_main,90
	global	_InitPortIOReg

;; *************** function _InitPortIOReg *****************
;; Defined at:
;;		line 27 in file "C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic18f2550\LED\LED.c"
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
psect	text1,class=CODE,space=0,reloc=2
global __ptext1
__ptext1:
psect	text1
	file	"C:\Users\FMV\Dropbox\PIC-project\PIC_XC8\pic18f2550\LED\LED.c"
	line	27
	global	__size_of_InitPortIOReg
	__size_of_InitPortIOReg	equ	__end_of_InitPortIOReg-_InitPortIOReg
	
_InitPortIOReg:
;incstack = 0
	opt	stack 30
;incstack = 0
	line	29
	
l593:
;LED.c: 29: TRISAbits.TRISA0 = 0;
	bcf	((c:3986)),c,0	;volatile
	line	31
	
l17:
	return
	opt stack 0
GLOBAL	__end_of_InitPortIOReg
	__end_of_InitPortIOReg:
	signat	_InitPortIOReg,88
	GLOBAL	__activetblptr
__activetblptr	EQU	0
	psect	intsave_regs,class=BIGRAM,space=1,noexec
	PSECT	rparam,class=COMRAM,space=1,noexec
	GLOBAL	__Lrparam
	FNCONF	rparam,??,?
GLOBAL	__Lparam, __Hparam
GLOBAL	__Lrparam, __Hrparam
__Lparam	EQU	__Lrparam
__Hparam	EQU	__Hrparam
	end
