//ヘッダファイルをインクルード
#include "p30f4012.h"
#include "uart.h"
//********************************************************************
//コンフィギュレーション・ワードの設定

	_FOSC(CSW_FSCM_OFF & XT_PLL8);     //10MHzセラロックを使ってPLLで80MHz
	_FWDT(WDT_OFF);
	_FBORPOR(PBOR_OFF & PWRT_64 & MCLR_EN);
	_FGS(CODE_PROT_OFF);

//********************************************************************
//UARTの設定パラメータ
 
unsigned int config1 = UART_EN & UART_IDLE_CON & UART_ALTRX_ALTTX & UART_NO_PAR_8BIT & UART_1STOPBIT
                     & UART_DIS_WAKE & UART_DIS_LOOPBACK & UART_DIS_ABAUD;
 
unsigned int config2 = UART_INT_TX_BUF_EMPTY & UART_TX_PIN_NORMAL & UART_TX_ENABLE & UART_INT_RX_CHAR 
                     & UART_ADR_DETECT_DIS & UART_RX_OVERRUN_CLEAR;
 
 


//********************************************************************
//メイン関数

int main(void)
{
	char temp;

	//==================================================================
	//UART初期設定
	//速度は115kbpsとします（80MHzクロックでは実用的に最も速い設定）

	OpenUART1(config1,config2,10);



	//==================================================================
	//メインループ

	while(1)
	{
		//１文字受信を待つ
		while(!DataRdyUART1());

		//受信文字をtempへ保存
		temp = ReadUART1();

		//tempを送信
		WriteUART1(temp);
	}

}
