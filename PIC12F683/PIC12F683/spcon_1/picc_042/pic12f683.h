
#ifndef	_HTC_H_
#warning Header file pic12f683.h included directly. Use #include <htc.h> instead.
#endif


 /* header file for the MICROCHIP PIC microcontroller
 */

#ifndef	__PIC12F683_H
#define	__PIC12F683_H

// Special function register definitions

static volatile       unsigned char	CH_TMR0		@ 0x01;
static volatile       unsigned char	CH_PCL		@ 0x02;
static volatile       unsigned char	CH_STATUS	@ 0x03;
static                unsigned char	CH_FSR		@ 0x04;
static volatile       unsigned char	CH_GPIO		@ 0x05;
static volatile       unsigned char	CH_PCLATH	@ 0x0A;
static volatile       unsigned char	CH_INTCON	@ 0x0B;
static volatile       unsigned char	CH_PIR1		@ 0x0C;
static volatile       unsigned char	CH_TMR1L	@ 0x0E;
static volatile       unsigned char	CH_TMR1H	@ 0x0F;
static volatile       unsigned char	CH_T1CON	@ 0x10;
static volatile       unsigned char	CH_TMR2		@ 0x11;
static volatile       unsigned char	CH_T2CON	@ 0x12;
static volatile       unsigned char	CH_CCPR1L	@ 0x13;
static volatile       unsigned char	CH_CCPR1H	@ 0x14;
static volatile       unsigned char	CH_CCP1CON	@ 0x15;
static volatile       unsigned char	CH_WDTCON	@ 0x18;
static volatile       unsigned char	CH_CMCON0	@ 0x19;
static                unsigned char	CH_CMCON1	@ 0x1A;
static volatile       unsigned char	CH_ADRESH	@ 0x1E;
static volatile       unsigned char	CH_ADCON0	@ 0x1F;
static          bank1 unsigned char	CH_OPTION	@ 0x81;
static volatile bank1 unsigned char	CH_TRISIO	@ 0x85;
static          bank1 unsigned char	CH_PIE1		@ 0x8C;
static volatile bank1 unsigned char	CH_PCON		@ 0x8E;
static volatile bank1 unsigned char	CH_OSCCON	@ 0x8F;
static          bank1 unsigned char	CH_OSCTUNE	@ 0x90;
static          bank1 unsigned char	CH_PR2		@ 0x92;
static          bank1 unsigned char	CH_WPU		@ 0x95;
static          bank1 unsigned char	CH_IOC		@ 0x96;
static          bank1 unsigned char	CH_VRCON	@ 0x99;
static volatile bank1 unsigned char	CH_EEDAT	@ 0x9A;
/* Alternate definition */
static volatile bank1 unsigned char	CH_EEDATA	@ 0x9A;
static          bank1 unsigned char	CH_EEADR	@ 0x9B;
/* Alternate definition */
static          bank1 unsigned char	CH_EEADRL	@ 0x9B;
static volatile bank1 unsigned char	CH_EECON1	@ 0x9C;
static volatile bank1 unsigned char	CH_EECON2	@ 0x9D;
static volatile bank1 unsigned char	CH_ADRESL	@ 0x9E;
static          bank1 unsigned char	CH_ANSEL	@ 0x9F;


/* Definitions for STATUS register */
static volatile       bit	BI_CARRY	@ ((unsigned)&CH_STATUS*8)+0;
static volatile       bit	BI_DC		@ ((unsigned)&CH_STATUS*8)+1;
static volatile       bit	BI_ZERO		@ ((unsigned)&CH_STATUS*8)+2;
static volatile       bit	BI_PD		@ ((unsigned)&CH_STATUS*8)+3;
static volatile       bit	BI_TO		@ ((unsigned)&CH_STATUS*8)+4;
static                bit	BI_RP0		@ ((unsigned)&CH_STATUS*8)+5;
static                bit	BI_RP1		@ ((unsigned)&CH_STATUS*8)+6;
static                bit	BI_IRP		@ ((unsigned)&CH_STATUS*8)+7;

/* Definitions for GPIO register */
static volatile       bit	BI_GPIO0	@ ((unsigned)&CH_GPIO*8)+0;
static volatile       bit	BI_GPIO1	@ ((unsigned)&CH_GPIO*8)+1;
static volatile       bit	BI_GPIO2	@ ((unsigned)&CH_GPIO*8)+2;
static volatile       bit	BI_GPIO3	@ ((unsigned)&CH_GPIO*8)+3;
static volatile       bit	BI_GPIO4	@ ((unsigned)&CH_GPIO*8)+4;
static volatile       bit	BI_GPIO5	@ ((unsigned)&CH_GPIO*8)+5;

/* Definitions for INTCON register */
static volatile       bit	BI_GPIF		@ ((unsigned)&CH_INTCON*8)+0;
static volatile       bit	BI_INTF		@ ((unsigned)&CH_INTCON*8)+1;
static volatile       bit	BI_T0IF		@ ((unsigned)&CH_INTCON*8)+2;
static                bit	BI_GPIE		@ ((unsigned)&CH_INTCON*8)+3;
static                bit	BI_INTE		@ ((unsigned)&CH_INTCON*8)+4;
static                bit	BI_T0IE		@ ((unsigned)&CH_INTCON*8)+5;
static                bit	BI_PEIE		@ ((unsigned)&CH_INTCON*8)+6;
static                bit	BI_GIE		@ ((unsigned)&CH_INTCON*8)+7;

/* Definitions for PIR1 register */
static volatile       bit	BI_TMR1IF	@ ((unsigned)&CH_PIR1*8)+0;
static volatile       bit	BI_TMR2IF	@ ((unsigned)&CH_PIR1*8)+1;
static volatile       bit	BI_OSFIF	@ ((unsigned)&CH_PIR1*8)+2;
static volatile       bit	BI_CMIF		@ ((unsigned)&CH_PIR1*8)+3;
static volatile       bit	BI_CCP1IF	@ ((unsigned)&CH_PIR1*8)+5;
static volatile       bit	BI_ADIF		@ ((unsigned)&CH_PIR1*8)+6;
static volatile       bit	BI_EEIF		@ ((unsigned)&CH_PIR1*8)+7;

/* Definitions for T1CON register */
static                bit	BI_TMR1ON	@ ((unsigned)&CH_T1CON*8)+0;
static                bit	BI_TMR1CS	@ ((unsigned)&CH_T1CON*8)+1;
static                bit	BI_T1SYNC	@ ((unsigned)&CH_T1CON*8)+2;
static                bit	BI_T1OSCEN	@ ((unsigned)&CH_T1CON*8)+3;
static                bit	BI_T1CKPS0	@ ((unsigned)&CH_T1CON*8)+4;
static                bit	BI_T1CKPS1	@ ((unsigned)&CH_T1CON*8)+5;
static                bit	BI_T1GE		@ ((unsigned)&CH_T1CON*8)+6;
static                bit	BI_T1GINV	@ ((unsigned)&CH_T1CON*8)+7;

/* Definitions for T2CON register */
static                bit	BI_T2CKPS0	@ ((unsigned)&CH_T2CON*8)+0;
static                bit	BI_T2CKPS1	@ ((unsigned)&CH_T2CON*8)+1;
static                bit	BI_TMR2ON	@ ((unsigned)&CH_T2CON*8)+2;
static                bit	BI_TOUTPS0	@ ((unsigned)&CH_T2CON*8)+3;
static                bit	BI_TOUTPS1	@ ((unsigned)&CH_T2CON*8)+4;
static                bit	BI_TOUTPS2	@ ((unsigned)&CH_T2CON*8)+5;
static                bit	BI_TOUTPS3	@ ((unsigned)&CH_T2CON*8)+6;


/* Definitions for CCP1CON register */
static                bit	BI_CCP1M0	@ ((unsigned)&CH_CCP1CON*8)+0;
static                bit	BI_CCP1M1	@ ((unsigned)&CH_CCP1CON*8)+1;
static                bit	BI_CCP1M2	@ ((unsigned)&CH_CCP1CON*8)+2;
static                bit	BI_CCP1M3	@ ((unsigned)&CH_CCP1CON*8)+3;
static                bit	BI_DC1B0	@ ((unsigned)&CH_CCP1CON*8)+4;
static                bit	BI_DC1B1	@ ((unsigned)&CH_CCP1CON*8)+5;

/* Definitions for WDTCON register */
static                bit	BI_SWDTEN	@ ((unsigned)&CH_WDTCON*8)+0;
static                bit	BI_WDTPS0	@ ((unsigned)&CH_WDTCON*8)+1;
static                bit	BI_WDTPS1	@ ((unsigned)&CH_WDTCON*8)+2;
static                bit	BI_WDTPS2	@ ((unsigned)&CH_WDTCON*8)+3;
static                bit	BI_WDTPS3	@ ((unsigned)&CH_WDTCON*8)+4;

/* Definitions for CMCON0 register */
static                bit	BI_CM0		@ ((unsigned)&CH_CMCON0*8)+0;
static                bit	BI_CM1		@ ((unsigned)&CH_CMCON0*8)+1;
static                bit	BI_CM2		@ ((unsigned)&CH_CMCON0*8)+2;
static                bit	BI_CIS		@ ((unsigned)&CH_CMCON0*8)+3;
static                bit	BI_CINV		@ ((unsigned)&CH_CMCON0*8)+4;
static volatile       bit	BI_COUT		@ ((unsigned)&CH_CMCON0*8)+6;

/* Definitions for CMCON1 register */
static                bit	BI_CMSYNC	@ ((unsigned)&CH_CMCON1*8)+0;
static                bit	BI_T1GSS	@ ((unsigned)&CH_CMCON1*8)+1;

/* Definitions for ADCON0 register */
static                bit	BI_ADON		@ ((unsigned)&CH_ADCON0*8)+0;
static volatile       bit	BI_GODONE	@ ((unsigned)&CH_ADCON0*8)+1;
static                bit	BI_CHS0		@ ((unsigned)&CH_ADCON0*8)+2;
static                bit	BI_CHS1		@ ((unsigned)&CH_ADCON0*8)+3;
static                bit	BI_CHS2		@ ((unsigned)&CH_ADCON0*8)+4;
static                bit	BI_VCFG		@ ((unsigned)&CH_ADCON0*8)+6;
static                bit	BI_ADFM		@ ((unsigned)&CH_ADCON0*8)+7;

/* Definitions for OPTION register */
static          bank1 bit	BI_PS0		@ ((unsigned)&CH_OPTION*8)+0;
static          bank1 bit	BI_PS1		@ ((unsigned)&CH_OPTION*8)+1;
static          bank1 bit	BI_PS2		@ ((unsigned)&CH_OPTION*8)+2;
static          bank1 bit	BI_PSA		@ ((unsigned)&CH_OPTION*8)+3;
static          bank1 bit	BI_T0SE		@ ((unsigned)&CH_OPTION*8)+4;
static          bank1 bit	BI_T0CS		@ ((unsigned)&CH_OPTION*8)+5;
static          bank1 bit	BI_INTEDG	@ ((unsigned)&CH_OPTION*8)+6;
static          bank1 bit	BI_GPPU		@ ((unsigned)&CH_OPTION*8)+7;

/* Definitions for TRISIO register */
static volatile bank1 bit	BI_TRISIO0	@ ((unsigned)&CH_TRISIO*8)+0;
static volatile bank1 bit	BI_TRISIO1	@ ((unsigned)&CH_TRISIO*8)+1;
static volatile bank1 bit	BI_TRISIO2	@ ((unsigned)&CH_TRISIO*8)+2;
static volatile bank1 bit	BI_TRISIO3	@ ((unsigned)&CH_TRISIO*8)+3;
static volatile bank1 bit	BI_TRISIO4	@ ((unsigned)&CH_TRISIO*8)+4;
static volatile bank1 bit	BI_TRISIO5	@ ((unsigned)&CH_TRISIO*8)+5;

/* Definitions for PIE1 register */
static          bank1 bit	BI_TMR1IE	@ ((unsigned)&CH_PIE1*8)+0;
static          bank1 bit	BI_TMR2IE	@ ((unsigned)&CH_PIE1*8)+1;
static          bank1 bit	BI_OSFIE	@ ((unsigned)&CH_PIE1*8)+2;
static          bank1 bit	BI_CMIE		@ ((unsigned)&CH_PIE1*8)+3;
static          bank1 bit	BI_CCP1IE	@ ((unsigned)&CH_PIE1*8)+5;
static          bank1 bit	BI_ADIE		@ ((unsigned)&CH_PIE1*8)+6;
static          bank1 bit	BI_EEIE		@ ((unsigned)&CH_PIE1*8)+7;

/* Definitions for PCON register */
static volatile bank1 bit	BI_BOD		@ ((unsigned)&CH_PCON*8)+0;
static volatile bank1 bit	BI_POR		@ ((unsigned)&CH_PCON*8)+1;
static          bank1 bit	BI_SBODEN	@ ((unsigned)&CH_PCON*8)+4;
static          bank1 bit	BI_ULPWUE	@ ((unsigned)&CH_PCON*8)+5;

/* Definitions for OSCCON register */
static          bank1 bit	BI_SCS		@ ((unsigned)&CH_OSCCON*8)+0;
static volatile bank1 bit	BI_LTS		@ ((unsigned)&CH_OSCCON*8)+1;
static volatile bank1 bit	BI_HTS		@ ((unsigned)&CH_OSCCON*8)+2;
static volatile bank1 bit	BI_OSTS		@ ((unsigned)&CH_OSCCON*8)+3;
static          bank1 bit	BI_IRCF0	@ ((unsigned)&CH_OSCCON*8)+4;
static          bank1 bit	BI_IRCF1	@ ((unsigned)&CH_OSCCON*8)+5;
static          bank1 bit	BI_IRCF2	@ ((unsigned)&CH_OSCCON*8)+6;

/* Definitions for OSCTUNE register */
static          bank1 bit	BI_TUN0		@ ((unsigned)&CH_OSCTUNE*8)+0;
static          bank1 bit	BI_TUN1		@ ((unsigned)&CH_OSCTUNE*8)+1;
static          bank1 bit	BI_TUN2		@ ((unsigned)&CH_OSCTUNE*8)+2;
static          bank1 bit	BI_TUN3		@ ((unsigned)&CH_OSCTUNE*8)+3;
static          bank1 bit	BI_TUN4		@ ((unsigned)&CH_OSCTUNE*8)+4;

/* Definitions for WPU register */
static          bank1 bit	BI_WPU0		@ ((unsigned)&CH_WPU*8)+0;
static          bank1 bit	BI_WPU1		@ ((unsigned)&CH_WPU*8)+1;
static          bank1 bit	BI_WPU2		@ ((unsigned)&CH_WPU*8)+2;
static          bank1 bit	BI_WPU3		@ ((unsigned)&CH_WPU*8)+3;
static          bank1 bit	BI_WPU4		@ ((unsigned)&CH_WPU*8)+4;
static          bank1 bit	BI_WPU5		@ ((unsigned)&CH_WPU*8)+5;

/* Definitions for IOC register */
static          bank1 bit	BI_IOC0		@ ((unsigned)&CH_IOC*8)+0;
static          bank1 bit	BI_IOC1		@ ((unsigned)&CH_IOC*8)+1;
static          bank1 bit	BI_IOC2		@ ((unsigned)&CH_IOC*8)+2;
static          bank1 bit	BI_IOC3		@ ((unsigned)&CH_IOC*8)+3;
static          bank1 bit	BI_IOC4		@ ((unsigned)&CH_IOC*8)+4;
static          bank1 bit	BI_IOC5		@ ((unsigned)&CH_IOC*8)+5;

/* Definitions for VRCON register */
static          bank1 bit	BI_VR0		@ ((unsigned)&CH_VRCON*8)+0;
static          bank1 bit	BI_VR1		@ ((unsigned)&CH_VRCON*8)+1;
static          bank1 bit	BI_VR2		@ ((unsigned)&CH_VRCON*8)+2;
static          bank1 bit	BI_VR3		@ ((unsigned)&CH_VRCON*8)+3;
static          bank1 bit	BI_VRR		@ ((unsigned)&CH_VRCON*8)+5;
static          bank1 bit	BI_VREN		@ ((unsigned)&CH_VRCON*8)+7;

/* Definitions for EECON1 register */
static volatile bank1 bit	BI_RD		@ ((unsigned)&CH_EECON1*8)+0;
static volatile bank1 bit	BI_WR		@ ((unsigned)&CH_EECON1*8)+1;
static          bank1 bit	BI_WREN		@ ((unsigned)&CH_EECON1*8)+2;
static volatile bank1 bit	BI_WRERR	@ ((unsigned)&CH_EECON1*8)+3;

/* Definitions for ANSEL register */
static          bank1 bit	BI_ANS0		@ ((unsigned)&CH_ANSEL*8)+0;
static          bank1 bit	BI_ANS1		@ ((unsigned)&CH_ANSEL*8)+1;
static          bank1 bit	BI_ANS2		@ ((unsigned)&CH_ANSEL*8)+2;
static          bank1 bit	BI_ANS3		@ ((unsigned)&CH_ANSEL*8)+3;
static          bank1 bit	BI_ADCS0	@ ((unsigned)&CH_ANSEL*8)+4;
static          bank1 bit	BI_ADCS1	@ ((unsigned)&CH_ANSEL*8)+5;
static          bank1 bit	BI_ADCS2	@ ((unsigned)&CH_ANSEL*8)+6;

// Configuration Mask Definitions
#define CONFIG_ADDR	0x2007
// Fail Clock Monitor Enable 
#define FCMEN		0x3FFF
#define FCMDIS		0x37FF
// Internal External Switch Over 
#define IESOEN		0x3FFF
#define IESODIS		0x3BFF
// Brown-out detect modes 
#define BOREN		0x3FFF
#define BOREN_XSLP	0x3EFF
#define SBOREN		0x3DFF
#define BORDIS		0x3CFF
// Protection of data block 
#define UNPROTECT	0x3FFF
#define CPD		0x3F7F
// Protection of program code 
#define UNPROTECT	0x3FFF
#define PROTECT		0x3FBF
// Master clear reset
#define MCLREN		0x3FFF
#define MCLRDIS		0x3FDF
// Power up timer enable 
#define PWRTDIS		0x3FFF
#define PWRTEN		0x3FEF
// Watchdog timer enable 
#define WDTEN		0x3FFF
#define WDTDIS		0x3FF7
// Oscillator configurations 
#define RCCLK		0x3FFF
#define RCIO		0x3FFE
#define INTCLK		0x3FFD
#define INTIO		0x3FFC
#define EC		0x3FFB
#define HS		0x3FFA
#define XT		0x3FF9
#define LP		0x3FF8


#endif
