/**********************************************************/
/*ペリフェラルピンセレクトライブラリ                      */
/*2011.12.30                                              */
/**********************************************************/

//====関数プロトタイプ宣言======================================================
void PPSIn(int RPPin, int Function);
void PPSOut(int RPPin, int Function);

//----RPピンマクロ----------------------------------------------------
#define 	RP0		0
#define 	RP1		1
#define 	RP2		2
#define 	RP3		3
#define 	RP4		4
#define 	RP5		5
#define 	RP6		6
#define 	RP7		7
#define 	RP8		8
#define 	RP9		9
#define 	RP10	10
#define 	RP11	11
#define 	RP12	12
#define 	RP13	13
#define 	RP14	14
#define 	RP15	15
#define 	RP16	16
#define 	RP17	17
#define 	RP18	18
#define 	RP19	19
#define 	RP20	20
#define 	RP21	21
#define 	RP22	22
#define 	RP23	23
#define 	RP24	24
#define 	RP25	25

//----入力機能マクロ--------------------------------------------------
#define 	RP_INT1		1
#define 	RP_INT2		2
#define 	RP_T2CK		3
#define 	RP_T3CK		4
#define		RP_T4CK		5
#define		RP_T5CK		6
#define 	RP_IC1		7
#define 	RP_IC2		8
#define 	RP_IC7		9
#define 	RP_IC8		10
#define 	RP_OCFA		11
#define 	RP_FLTA1	12
#define 	RP_FLTA2	13
#define 	RP_QEA1		14
#define 	RP_QEB1		15
#define 	RP_INDX1	16
#define 	RP_QEA2		17
#define 	RP_QEB2		18
#define 	RP_INDX2	19
#define 	RP_U1RX		20
#define 	RP_U1CTS	21
#define 	RP_U2RX		22
#define 	RP_U2CTS	23
#define 	RP_SDI1		24
#define 	RP_SCK1IN	25
#define 	RP_SS1IN	26
#define 	RP_SDI2		27
#define 	RP_SCK2IN	28
#define 	RP_SS2IN	29
#define 	RP_CIRX		30

//----出力機能マクロ--------------------------------------------------
#define 	RP_NULL		0b00000
#define 	RP_C1OUT	0b00001
#define 	RP_C2OUT	0b00010
#define 	RP_U1TX		0b00011
#define 	RP_U1RTS	0b00100
#define 	RP_U2TX		0b00101
#define 	RP_U2RTS	0b00110
#define 	RP_SDO1		0b00111
#define 	RP_SCK1OUT	0b01000
#define 	RP_SS1OUT	0b01001
#define 	RP_SDO2		0b01010
#define 	RP_SCK2OUT	0b01011
#define 	RP_SS2OUT	0b01100
#define 	RP_C1TX		0b10000
#define 	RP_OC1		0b10010
#define 	RP_OC2		0b10011
#define 	RP_OC3		0b10100
#define 	RP_OC4		0b10101
#define 	RP_UPDN1	0b11010
#define 	RP_UPDN2	0b11011
