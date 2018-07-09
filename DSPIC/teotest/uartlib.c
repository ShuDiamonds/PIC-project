/**********************************************************/
/*UART関数ライブラリ                                      */
/*2011.12.30                                              */
/**********************************************************/
#include "p33Fxxxx.h"
#include "stdio.h"
#include "uartlib.h"

//====UART1初期化関数===========================================================
void UART1_Init(int baud, int flow)
{
	//----UART1イネーブルビット---------
	U1MODEbits.UEN = flow;			//TxおよびRxピン、フロー制御ピンの有効化
	//----ボーレート設定----------------
	U1BRG = baud;		//ボーレート設定
	//UART1ステータスレジスタクリア
	U1STA = 0;
	//UART1イネーブル
	U1MODEbits.UARTEN = 1;
	//UART1送信イネーブル
	U1STAbits.UTXEN = 1;
	
}

//====UART1受信割り込み許可関数=====================================================
void UART1_RxIntEnable(int Level)
{
	//UART1受信割り込み優先度ビット
	IPC2bits.U1RXIP = Level;	//割り込み優先度設定
	//UART1受信割り込みステータスビット
	IFS0bits.U1RXIF = 0;		//割り込み要求クリア
	//UART1受信割り込みイネーブルビット
	IEC0bits.U1RXIE = 1;		//割り込み要求は有効
}

//====UART1受信割り込み禁止関数=====================================================
void UART1_RxIntDisable(void)
{
	//UART1受信割り込みイネーブルビット
	IEC0bits.U1RXIE = 0;		//割り込み要求は無効
}


//====UART2初期化関数===========================================================
void UART2_Init(int baud, int flow)
{
	//----UART2イネーブルビット---------
	U2MODEbits.UEN = flow;			//TxおよびRxピン、フロー制御ピンの有効化
	//----ボーレート設定----------------
	U2BRG = baud;		//ボーレート設定
	//UART2ステータスレジスタクリア
	U2STA = 0;
	//UART2イネーブル
	U2MODEbits.UARTEN = 1;
	//UART2送信イネーブル
	U2STAbits.UTXEN = 1;
	
}

//====UART2受信割り込み許可関数=====================================================
void UART2_RxIntEnable(int Level)
{
	//UART2受信割り込み優先度ビット
	IPC7bits.U2RXIP = Level;	//割り込み優先度設定
	//UART1受信割り込みステータスビット
	IFS1bits.U2RXIF = 0;		//割り込み要求クリア
	//UART1受信割り込みイネーブルビット
	IEC1bits.U2RXIE = 1;		//割り込み要求は有効
}

//====UART2受信割り込み禁止関数=====================================================
void UART2_RxIntDisable(void)
{
	//UART2受信割り込みイネーブルビット
	IEC0bits.U1RXIE = 0;		//割り込み要求は無効
}


//****UART1エラー検出関数****************************************************************************
void UART1_ErrDetect(void)
{
	//----エラー検出----------------------------------------------
	if(U1STAbits.OERR == 1)
	{
		printf("\n[!]UART1 OverRunErr[!]\n");
		U1STAbits.OERR = 0;
	}
	if(U1STAbits.FERR == 1)
	{
		printf("\n[!]UART1 FramingErr[!]\n");
		U1STAbits.FERR = 0;
	}
	if(U1STAbits.PERR == 1)
	{
		printf("\n[!]UART1 ParityErr[!]\n");
		U1STAbits.PERR = 0;
	}
}

//****UART2エラー検出関数****************************************************************************
void UART2_ErrDetect(void)
{
	//----エラー検出----------------------------------------------
	if(U2STAbits.OERR == 1)
	{
		printf("\n[!]UART2 OverRunErr[!]\n");
		U2STAbits.OERR = 0;
	}
	if(U2STAbits.FERR == 1)
	{
		printf("\n[!]UART2 FramingErr[!]\n");
		U2STAbits.FERR = 0;
	}
	if(U2STAbits.PERR == 1)
	{
		printf("\n[!]UART2 ParityErr[!]\n");
		U2STAbits.PERR = 0;
	}
}


