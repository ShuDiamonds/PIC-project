/*==========================================================================
;
;     Configuration Bits
;
;     Data Sheet    Include File                  Address
;     CONFIG1L    = Configuration Byte 0          300000h
;     CONFIG1H    = Configuration Byte 1          300001h
;     CONFIG2L    = Configuration Byte 3          300002h
;     CONFIG2H    = Configuration Byte 4          300003h
;     CONFIG3L    = Configuration Byte 5          300004h
;     CONFIG3H    = Configuration Byte 6          300005h
;     CONFIG4L    = Configuration Byte 7          300006h
;     CONFIG4H    = Configuration Byte 8          300007h
;
;==========================================================================
*/
//Configuration Byte 0 Options
#define		_CP_ON_0  	0x00	// Code Protect enable   
#define		_CP_OFF_0	0xFF

//Configuration Byte 1 Options
#define		_OSCS_ON_1	0xDF	// Oscillator Switch enable
#define		_OSCS_OFF_1	0xFF

#define		_LP_OSC_1	0xF8	// Oscillator type
#define		_XT_OSC_1 	0xF9
#define		_HS_OSC_1	0xFA
#define		_RC_OSC_1	0xFB
#define		_EC_OSC_1	0xFC	// External Clock w/OSC2 output divide by 4
#define		_ECIO_OSC_1  	0xFD	// w/OSC2 as an IO pin (RA6)
#define		_HSPLL_OSC_1	0xFE	// HS PLL
#define		_RCIO_OSC_1	0xFF	// RC w/OSC2 as an IO pin (RA6)

//Configuration Byte 2 Options
#define		_BOR_ON_2 	0xFF	// Brown-Out Reset enable
#define		_BOR_OFF_2 	0xFD
#define		_PWRT_OFF_2	0xFF	// Power-Up Timer enable
#define		_PWRT_ON_2	0xFE
#define		_BORV_25_2	0xFF	// BOR Voltage - 2.5v
#define		_BORV_27_2	0xFB	//               2.7v
#define		_BORV_42_2	0xF7 	//               4.2v
#define		_BORV_45_2	0xF3	//               4.5v

//Configuration Byte 3 Options
#define		_WDT_ON_3	0xFF	// Watch Dog Timer enable
#define		_WDT_OFF_3	0xFE
#define		_WDTPS_128_3 	0xFF	// Watch Dog Timer PostScaler count
#define		_WDTPS_64_3	0xFD
#define		_WDTPS_32_3	0xFB
#define		_WDTPS_16_3	0xF9
#define		_WDTPS_8_3	0xF7
#define		_WDTPS_4_3	0xF5
#define		_WDTPS_2_3	0xF3
#define		_WDTPS_1_3	0xF1

//Configuration Byte 5 Options
#define		_CCP2MX_ON_5	0xFF	// CCP2 pin Mux enable
#define		_CCP2MX_OFF_5	0xFE

//Configuration Byte 6 Options
#define		_STVR_ON_6	0xFF	// Stack over/underflow Reset enable
#define		_STVR_OFF_6	0xFE

/* To use the Configuration Bits, place the following lines in your source code
//  in the following format, and change the configuration value to the desired 
//  setting (such as CP_OFF to CP_ON).  
//  The following is a assignment of address values for all of the configuration
//  registers for the purpose of table reads
#define		_CONFIG0 	0x300000
#define		_CONFIG1	0x300001
#define		_CONFIG2	0x300002
#define		_CONFIG3	0x300003
#define		_CONFIG4	0x300004
#define		_CONFIG5	0x300005
#define		_CONFIG6	0x300006
#define		_CONFIG7	0x300007
*/

//Program Configuration Register 0
//#pragma	romdata CONFIG=0x300000
//unsigned char rom _CONFIG0 = _CP_OFF_0;

//Program Configuration Register 1
//unsigned char rom _CONFIG1 = _OSCS_OFF_1 & _HS_OSC_1;

//Program Configuration Register 2
//unsigned char rom _CONFIG2 = _BOR_ON_2 & _BORV_45_2 & _PWRT_ON_2;

//Program Configuration Register 3
//unsigned char rom _CONFIG3 = _WDT_OFF_3;

//Program Configuration Register 5
//unsigned char rom _CONFIG5 = _CCP2MX_ON_5;

//Program Configuration Register 6
//unsigned char rom _CONFIG6 = _STVR_ON_6;
