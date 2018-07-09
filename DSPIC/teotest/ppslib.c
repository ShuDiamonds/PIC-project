/**********************************************************/
/*ペリフェラルピンセレクトライブラリ                      */
/*2011.12.30                                              */
/**********************************************************/
#include "p33Fxxxx.h"
#include "ppslib.h"


//====入力機能割り当て==========================================================
void PPSIn(int RPPin, int Function)
{
	switch(Function)
	{
		case	RP_INT1:	RPINR0bits.INT1R = RPPin;	break;
		case	RP_INT2:	RPINR1bits.INT2R = RPPin;	break;
		case	RP_T2CK:	RPINR3bits.T2CKR = RPPin;	break;
		case	RP_T3CK:	RPINR3bits.T3CKR = RPPin;	break;
		case	RP_T4CK:	RPINR4bits.T4CKR = RPPin;	break;
		case	RP_T5CK:	RPINR4bits.T5CKR = RPPin;	break;
		case	RP_IC1:		RPINR7bits.IC1R = RPPin;	break;
		case	RP_IC2:		RPINR7bits.IC2R = RPPin;	break;
		case	RP_IC7:		RPINR10bits.IC7R = RPPin;	break;
		case	RP_IC8:		RPINR10bits.IC8R = RPPin;	break;
		case	RP_OCFA:	RPINR11bits.OCFAR = RPPin;	break;
		case	RP_FLTA1:	RPINR12bits.FLTA1R = RPPin;	break;
		case	RP_FLTA2:	RPINR13bits.FLTA2R = RPPin;	break;
		case	RP_QEA1:	RPINR14bits.QEA1R = RPPin;	break;
		case	RP_QEB1:	RPINR14bits.QEB1R = RPPin;	break;
		case	RP_INDX1:	RPINR15bits.INDX1R = RPPin;	break;
		case	RP_QEA2:	RPINR16bits.QEA2R = RPPin;	break;
		case	RP_QEB2:	RPINR16bits.QEB2R = RPPin;	break;
		case	RP_INDX2:	RPINR17bits.INDX2R = RPPin;	break;
		case	RP_U1RX:	RPINR18bits.U1RXR = RPPin;	break;
		case	RP_U1CTS:	RPINR18bits.U1CTSR = RPPin;	break;
		case	RP_U2RX:	RPINR19bits.U2RXR = RPPin;	break;
		case	RP_U2CTS:	RPINR19bits.U2CTSR = RPPin;	break;
		case	RP_SDI1:	RPINR20bits.SDI1R = RPPin;	break;
		case	RP_SCK1IN:	RPINR20bits.SCK1R = RPPin;	break;
		case	RP_SS1IN:	RPINR21bits.SS1R = RPPin;	break;
		case	RP_SDI2:	RPINR22bits.SDI2R = RPPin;	break;
		case	RP_SCK2IN:	RPINR22bits.SCK2R = RPPin;	break;
		case	RP_SS2IN:	RPINR23bits.SS2R = RPPin;	break;
		//case	RP_CIRX:	RPINR26bits.CIRXR = RPPin;	break;
	}
}

//====出力機能割り当て==========================================================
void PPSOut(int RPPin, int Function)
{
	switch(RPPin)
	{
		case	RP0:	RPOR0bits.RP0R = Function;		break;
		case	RP1:	RPOR0bits.RP1R = Function;		break;
		case	RP2:	RPOR1bits.RP2R = Function;		break;
		case	RP3:	RPOR1bits.RP3R = Function;		break;
		case	RP4:	RPOR2bits.RP4R = Function;		break;
		case	RP5:	RPOR2bits.RP5R = Function;		break;
		case	RP6:	RPOR3bits.RP6R = Function;		break;
		case	RP7:	RPOR3bits.RP7R = Function;		break;
		case	RP8:	RPOR4bits.RP8R = Function;		break;
		case	RP9:	RPOR4bits.RP9R = Function;		break;
		case	RP10:	RPOR5bits.RP10R = Function;		break;
		case	RP11:	RPOR5bits.RP11R = Function;		break;
		case	RP12:	RPOR6bits.RP12R = Function;		break;
		case	RP13:	RPOR6bits.RP13R = Function;		break;
		case	RP14:	RPOR7bits.RP14R = Function;		break;
		case	RP15:	RPOR7bits.RP15R = Function;		break;
//		case	RP16:	RPOR8bits.RP16R = Function;		break;
//		case	RP17:	RPOR8bits.RP17R = Function;		break;
//		case	RP18:	RPOR9bits.RP18R = Function;		break;
//		case	RP19:	RPOR9bits.RP19R = Function;		break;
//		case	RP20:	RPOR10bits.RP20R = Function;	break;
//		case	RP21:	RPOR10bits.RP21R = Function;	break;
//		case	RP22:	RPOR11bits.RP22R = Function;	break;
//		case	RP23:	RPOR11bits.RP23R = Function;	break;
//		case	RP24:	RPOR12bits.RP24R = Function;	break;
//		case	RP25:	RPOR12bits.RP25R = Function;	break;
	}
}


