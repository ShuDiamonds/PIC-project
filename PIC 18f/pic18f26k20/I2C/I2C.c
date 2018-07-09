/************************************************************
　　MPLAB-C18による入出力ポートの制御テストプログラム

*************************************************************/
#include <p18f26k20.h>	 // PIC18C452のヘッダ・ファイル
#include <delays.h>
#include <usart.h>
#include <stdlib.h>
#include <stdio.h>
#include <i2c.h>		//I2C関数

//***** コンフィギュレーションの設定config

	#pragma	config	FOSC = INTIO67
	#pragma	config	FCMEN = OFF
	#pragma	config	IESO = OFF
	#pragma	config	PWRT = OFF
	#pragma	config	BOREN = OFF
	#pragma	config	BORV = 18
	#pragma	config	WDTEN = OFF
	#pragma	config	WDTPS = 1
	#pragma	config	MCLRE = ON
	#pragma	config	PBADEN = OFF		//デジタルに設定
	//	#pragma	config	PBAD = ANA
	//	#pragma	config	CCP2MX = C1
	#pragma	config	STVREN = ON
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
//******I2Cアドレスマクロ****
#define	I2c_Adress_write	0b10100000
#define	I2c_Adress_read		0b10100000
//---Wait---//
//64Mhz駆動の時
#define WAIT_MS Delay1KTCYx(16)// Wait 1ms
#define WAIT_US Delay10TCYx(16)	// Wait 1us

//=======wait[ms]======//
void wait_ms(unsigned long int t) {
	while(t--) {
		WAIT_MS;
	}
}

//=======wait[us]======//
void wait_us(unsigned int t) {
	while(t--) {
		WAIT_US;
	}
}
//***** メイン関数
void main(void)						// メイン関数
{
	
	//ローカル変数定義
		unsigned char demo[17] = "jyj\n\r";
		unsigned char Memodata[20] = "EEPROM Memory!!!"; //書き込みデータ
		unsigned char buffer[17]="                ";		//読み出しバッファ
		unsigned char i2c_data[20]="                   ";		//読み出しバッファ
		unsigned char ermes[7]="      ";					//エラー用バッファ
		unsigned int memadrs;					//メモリアドレス
		 int temp;

	//内部クロック初期化
		OSCCONbits.IRCF0 = 1;		//64MHzに設定
		OSCCONbits.IRCF1 = 1;
		OSCCONbits.IRCF2 = 1;
		OSCTUNEbits.PLLEN = 1;        // PLLを起動する
		wait_ms(2);					//クロックの安定化
	
	//UART初期化
	OpenUSART(USART_TX_INT_OFF & USART_RX_INT_OFF & //
			USART_ASYNCH_MODE & USART_EIGHT_BIT &	//
			USART_CONT_RX & USART_BRGH_LOW, 103); 	//注意　OpenUSART　関数の最後の文字はボーレート設定で今は40Mhzで9600の時で64にする。また48Mhzで9600の時で77にする
	printf("Start\n\r");
	printf("%s",demo);
	//I2C初期化
	/****I2Cの初期化********
	SSPADDの値は次の式で求まる
	
	SSPADD=(PICのシステムクロック[kHz]/伝送速度[kHz])-1
	
	***********************/
		OpenI2C(MASTER, SLEW_ON);               //I2Cマスターモード指定
		SSPADD = 39;								//Board Rate 400kHz		
		memadrs=0;
	//初期化
		TRISA=0x00;						   // ポートAをすべて出力ピンにする
		TRISB=0;						// ポートBをすべて出力ピンにする
		TRISC=0b10111111;						   // ポートCをすべて出力ピンにする
	
		 while(1)
		{
			

			temp =	EPageWrite(I2c_Adress_write, memadrs, Memodata);	//書き込み
			printf("writed data is  %s\n\r",Memodata);
			wait_ms(12);
			if (temp < 0)
			{						//エラーチェック
				itoa(-temp , ermes);
				printf("Error");
				printf(" %s\n\r",ermes);
			}
			
			itoa(memadrs, buffer);				//アドレス表示
			printf("read  adress = %s \n\r",buffer);
			//**** Read and Display
			wait_ms(400);
			temp = ESequentialRead (I2c_Adress_read, memadrs, i2c_data, 16); //読み出し
			wait_ms(12);
			printf("data = %s \n\r",i2c_data);					//読み出しデータ表示
			if ( temp < 0)
			{					//エラーチェック
				itoa(-temp , ermes);
				printf("Error");
				printf("%s\n\r",ermes);
			}
			
			memadrs = memadrs + 64;			   //次のアドレスへ
			if(memadrs >= 0x7FFF)
			{
				memadrs = 0;
				printf("End of Write\n\r");			   //終了メッセージ
				
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


