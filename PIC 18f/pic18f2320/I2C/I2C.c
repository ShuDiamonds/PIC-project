/************************************************************
　　MPLAB-C18による入出力ポートの制御テストプログラム
　　スイッチの入力をポートDから行い、スイッチの状態に
　　従って、指定された発光ダイオードを点滅させる。
　　　　RD0のスイッチがOFFの時LED１
　　　　RD1のスイッチがOFFの時LED2
　　　　RD2のスイッチがOFFの時LED3
　　　　全スイッチがONの時全LED
*************************************************************/
#include <p18f2320.h>	 // PIC18C452のヘッダ・ファイル
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <i2c.h>		//I2C関数

//***** コンフィギュレーションの設定config


#pragma	config	OSC = HSPLL
#pragma	config	FSCM = OFF
#pragma	config	IESO = OFF
#pragma	config	PWRT = OFF
#pragma	config	BOR = OFF
#pragma	config	BORV = 20
#pragma	config	WDT = OFF
#pragma	config	WDTPS = 1
#pragma	config	MCLRE = ON
#pragma	config	PBAD = DIG
//	#pragma	config	PBAD = ANA
//	#pragma	config	CCP2MX = C1
#pragma	config	STVR = ON
#pragma	config	LVP = ON
#pragma	config	DEBUG = OFF
#pragma	config	CP0 = OFF
#pragma	config	CP1 = OFF
#pragma	config	CP2 = OFF
#pragma	config	CP3 = OFF
#pragma	config	CPB = OFF
#pragma	config	CPD = OFF
#pragma	config	WRT0 = OFF
#pragma	config	WRT1 = OFF
#pragma	config	WRT2 = OFF
#pragma	config	WRT3 = OFF
#pragma	config	WRTB = OFF
#pragma	config	WRTC = OFF
#pragma	config	WRTD = OFF
#pragma	config	EBTR0 = OFF
#pragma	config	EBTR1 = OFF
#pragma	config	EBTR2 = OFF
#pragma	config	EBTR3 = OFF
#pragma	config	EBTRB = OFF
		
//****** 関数プロトタイピング
unsigned char EPageWrite(unsigned char control, 
				unsigned int address, unsigned char *wtptr); 
unsigned char ESequentialRead( unsigned char control,unsigned int address,
				 unsigned char *rdptr, unsigned char length );


//***** メイン関数
void main(void)						// メイン関数
{
	
	//ローカル変数定義
	char Memodata[17] = "EEPROM Memory!!!"; //書き込みデータ
	char buffer[17]="                 ";		//読み出しバッファ
	char ermes[7]="       ";					//エラー用バッファ
	unsigned int memadrs;					//メモリアドレス
	int temp;

	
	//UART初期化
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 64); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600の時で64にする。また48Mhzで9600の時で77にする
	//I2C初期化
	OpenI2C(MASTER, SLEW_ON);               //I2Cマスターモード指定
	SSPADD = 9;								//Board Rate 400kHz
	memadrs=0;
	//初期化
		TRISA=0x00;						   // ポートAをすべて出力ピンにする
		TRISB=0;						// ポートBをすべて出力ピンにする
		TRISC=0b10111111;						   // ポートCをすべて出力ピンにする
	
	 while(1)
	{
		temp =	EPageWrite(0xA0, memadrs, Memodata);	//書き込み
		if (temp < 0) {						//エラーチェック
			itoa(-temp , ermes);
			printf("Error\n\r");
			putsUSART(ermes);
		}
		Delay10KTCYx(3);					//Wait 12msec
		itoa(memadrs, buffer);				//アドレス表示
		putsUSART(buffer);
		//**** Read and Display
		temp = ESequentialRead (0xA0, memadrs, buffer, 16); //読み出し
		putsUSART(buffer);					//読み出しデータ表示
		if ( temp < 0) {					//エラーチェック
			itoa(-temp , ermes);
			printf("Error\n\r");
			putsUSART(ermes);
		}
		memadrs = memadrs + 64;			   //次のアドレスへ
		if(memadrs >= 0x7FFF) {
			memadrs = 0;
			printf("End of Write\n\r");			   //終了メッセージ
			Delay10KTCYx(500);			   //１秒待ち
		}
	}

	
}



//******* サブ関数群  ***********************************
//******* I2C制御関数	******

/********************************************************************
*	  Function Name:	EPageWrite									*
*	  Return Value:		error condition status and/or data byte		*
*	  Parameters:		EE memory control byte with R/W set to 1	*
*	  Description:		Reads 1 byte from passed address to EE memory*
*						device. This routine can be used for any I2C*
*						EE memory device, which only uses 2 byte of *
*						address data as in the 24LC64B/256B.		*
*																	*  
********************************************************************/
unsigned char EPageWrite(unsigned char control, 
					unsigned int address, unsigned char *wtptr)
{
	IdleI2C();							// アイドル確認
	SSPCON2bits.SEN = 1;				// START condition出力
	while ( SSPCON2bits.SEN );			// start condition送信待ち
	if ( PIR2bits.BCLIF )				// バス衝突チェック
		{ return ( -1 ); }				// バス衝突発生 
	else								// 正常
	{
		SSPBUF = control;				// control出力
		while(SSPSTATbits.BF);			// 送信待ち
		IdleI2C();						// アイドル確認
		if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
			{ return(-2); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア

		SSPBUF = address >> 8;			// EEPROMアドレス上位出力
		while(SSPSTATbits.BF);			// 送信待ち
		IdleI2C();						// アイドル確認
		if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
			{ return(-3); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア

		SSPBUF = address;				// EEPROMアドレス下位出力
		while(SSPSTATbits.BF);			// 送信待ち
		IdleI2C();						// アイドル確認
		if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
			{ return(-4); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア

		if (putsI2C ( wtptr) )			// データ連続書き込み
			{ return (-4); }			// エラーチェック
	
		IdleI2C();						// アイドル確認
		SSPCON2bits.PEN = 1;			// STOP condition出力
		while ( SSPCON2bits.PEN );		// stop condition送信待ち
		PIR1bits.SSPIF = 0;				// SSPIFクリア
		if ( PIR2bits.BCLIF )			// バス衝突チェック
			{ return ( -1 ); }			// バス衝突発生エラー 
		return ( 0 );					// 正常終了
	}
}


//***** I2C読みだし関数　　*****
/********************************************************************
*	  Function Name:   ESequentialRead								*
*	  Return Value:	   error condition status						*
*	  Parameters:	   EE memory control, address, pointer and		*
*					   length bytes.								*
*	  Description:	   Reads data string from I2C EE memory			*
*					   device. This routine can be used for any I2C *
*					   EE memory device, which only uses 2 bytes of *
*					   address data as in the 24LC256B.				*
*																	*  
********************************************************************/

unsigned char ESequentialRead( unsigned char control, 
			unsigned int address, unsigned char *rdptr, unsigned char length )
{
	IdleI2C();								// アイドル確認
	SSPCON2bits.SEN = 1;					// START condition出力
	while ( SSPCON2bits.SEN );				// start condition送信待ち
	if ( PIR2bits.BCLIF )					// バス衝突チェック
		{ return ( -1 ); }					// バス衝突発生エラー 
	else									// 正常
	{
		SSPBUF = control;					// control Write Mode出力
		while(SSPSTATbits.BF);				// 送信待ち
		IdleI2C();							// アイドル確認
		if ( SSPCON2bits.ACKSTAT )			// ACK受信待ち
			{ return(-2); }					// ACK無し
		PIR1bits.SSPIF = 0;					// SSPIFクリア

		SSPBUF = address >> 8;				// EEPROMアドレス上位出力
		while(SSPSTATbits.BF);				// 送信待ち
		IdleI2C();							// アイドル確認
		if ( SSPCON2bits.ACKSTAT )			// ACK受信待ち
			{ return(-3); }					// ACK無し
		PIR1bits.SSPIF = 0;					// SSPIFクリア

		SSPBUF = address;					// EEPROMアドレス下位出力
		while(SSPSTATbits.BF);				// 送信待ち
		IdleI2C();							// アイドル確認
		if ( SSPCON2bits.ACKSTAT )			// ACK受信待ち
			{ return(-4); }					// ACK無し
		PIR1bits.SSPIF = 0;					// SSPIFクリア

		SSPCON2bits.RSEN = 1;				// RESTART condition出力
		while ( SSPCON2bits.RSEN );			// re-start condition送信待ち
		if ( PIR2bits.BCLIF )				// バス衝突チェック
			{ return ( -5 ); }				// バス衝突発生エラー
		else								// 正常
		{
			SSPBUF = control+1;				// control Read Mode出力
			while(SSPSTATbits.BF);			// 送信待ち
			IdleI2C();						// アイドル確認
			if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
				{ return(-6); }				// ACK無し
			PIR1bits.SSPIF = 0;				// SSPIFクリア

			if (getsI2C( rdptr, length ) )	// 連続読み込み
				{ return (-7); }			// エラーチェック

			SSPCON2bits.ACKDT = 1;			// NACK準備(Not ACK)
			SSPCON2bits.ACKEN = 1;			// NACK送信開始
			while ( SSPCON2bits.ACKEN );	// 送信待ち
			PIR1bits.SSPIF = 0;				// SSPIFクリア

			SSPCON2bits.PEN = 1;			// stop condition出力
			while ( SSPCON2bits.PEN );		// 送信待ち
			if ( PIR2bits.BCLIF )			// バス衝突チェック
				{ return ( -7 ); }			// バス衝突発生エラー
			else							// 正常
			{
				PIR1bits.SSPIF = 0;			// SSPIFクリア
				return ( 0 );				// 正常終了
			}
		}
	}
}

//******* I2C書込み関数	  ******
/********************************************************************
*	  Function Name:	EByteWrite								   *
*	  Return Value:		error condition status						*
*	  Parameters:		EE memory device control, address and data	*
*						bytes.										*
*	  Description:		Write single data byte to I2C EE memory		*
*						device. This routine can be used for any I2C*
*						EE memory device, which only uses 2 byte of *
*						address data as in the 24LC64B/256B.		*
*																	*  
********************************************************************/
unsigned char EByteWrite( unsigned char control, 
			unsigned int address, unsigned char data )
{
	IdleI2C();							// アイドル確認
	SSPCON2bits.SEN = 1;				// start condition出力
	while ( SSPCON2bits.SEN );			// start condition終了待ち
	if ( PIR2bits.BCLIF )				// バス衝突チェック
		{ return ( -1 ); }				// 衝突エラー発生
	else								// 正常の時
	{
		SSPBUF = control;				// controlデータ出力
		while(SSPSTATbits.BF);			// 送信待ち
		IdleI2C();						// アイドル確認
		if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
			{ return(-2); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア

		SSPBUF = address >> 8;			// EEPROMアドレス上位出力
		while(SSPSTATbits.BF);			// 送信待ち
		IdleI2C();						// アイドル確認
		if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
			{ return(-3); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア

		SSPBUF = address;				// EEPROMアドレス下位出力
		while(SSPSTATbits.BF);			// 送信待ち
		IdleI2C();						// アイドル確認
		if ( SSPCON2bits.ACKSTAT )		// ASCK受信待ち
			{ return(-4); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア

		SSPBUF = data;					// 書き込みデータ出力
		while(SSPSTATbits.BF);			// 送信待ち
		IdleI2C();						// アイドル確認	 
		if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
			{ return(-5); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア
		
		SSPCON2bits.PEN = 1;			// STOP condition出力
		while ( SSPCON2bits.PEN );		// stop condition終了待ち
		PIR1bits.SSPIF = 0;				// SSPIFクリア
		if ( PIR2bits.BCLIF )			// バス衝突確認
			{ return ( -1 ); }			// 衝突発生 
		return ( 0 );					// 正常終了
	}
}

/********************************************************************
*	  Function Name:	EByteRead									*
*	  Return Value:		error condition status and/or data byte		*
*	  Parameters:		EE memory control byte with R/W set to 1	*
*	  Description:		Reads 1 byte from passed address to EE memory*
*						device. This routine can be used for any I2C*
*						EE memory device, which only uses 2 byte of *
*						address data as in the 24LC64B/256B.		*
********************************************************************/
unsigned int EByteRead( unsigned char control, unsigned int address )
{
	IdleI2C();							// アイドル確認
	SSPCON2bits.SEN = 1;				// START condition出力
	while ( SSPCON2bits.SEN );			// start condition終了待ち
	if ( PIR2bits.BCLIF )				// バス衝突チェック
		{ return ( -1 ); }				// バス衝突発生エラー 
	else
	{
		SSPBUF = control;				// control Write Mode出力
		while(SSPSTATbits.BF);			// 送信待ち
		IdleI2C();						// アイドル確認
		if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
			{ return(-2); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア

		SSPBUF = address >> 8;			// EEPROMアドレス上位出力
		while(SSPSTATbits.BF);			// 送信待ち
		IdleI2C();						// アイドル確認
		if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
			{ return(-3); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア

		SSPBUF = address;				// EEPROMアドレス下位出力
		while(SSPSTATbits.BF);			// 送信終了待ち
		IdleI2C();						// アイドル確認
		if ( SSPCON2bits.ACKSTAT )		// ACK受信待ち
			{ return(-4); }				// ACK無し
		PIR1bits.SSPIF = 0;				// SSPIFクリア

		SSPCON2bits.RSEN = 1;			// RESTART condition出力	   
		while ( SSPCON2bits.RSEN );		// RESART condition送信待ち 
		if ( PIR2bits.BCLIF )			// バス衝突チェック
			{ return ( -5 ); }			// バス衝突発生エラー  
		else							// 正常
		{
			SSPBUF = control+1;			// cntorol Read Mode出力
			while(SSPSTATbits.BF);		// 送信待ち
			IdleI2C();					// アイドル確認
			if ( SSPCON2bits.ACKSTAT )	// ACK受信待ち
				{ return(-6); }			// ACK無し
			PIR1bits.SSPIF = 0;			// SSPIFクリア

			SSPCON2bits.RCEN = 1;		// マスターモード１バイト受信
			while ( SSPCON2bits.RCEN ); // 受信完了待ち
			SSPCON2bits.ACKDT = 1;		// ACKを返信しない（Not ACK)
			SSPCON2bits.ACKEN = 1;		// バス衝突クリア
			while ( SSPCON2bits.ACKEN ); // ACK送信
			PIR1bits.SSPIF = 0;			// SSPIFクリア

			SSPCON2bits.PEN = 1;		// stop condition出力
			while ( SSPCON2bits.PEN );	// stop condition送信待ち
			if ( PIR2bits.BCLIF )		// バス衝突チェック
				{ return ( -7 ); }		// バス衝突発生エラー
			else						// 正常
			{
				PIR1bits.SSPIF = 0;		// SSPIFクリア
				return ( (unsigned int) SSPBUF );	// 正常終了
			}
		}
	}
}


