/*********************************************************************
*
*　USB接続 フルカラー照明
*	USBフレームワーク修正情報
*　　コンフィギュレーションの設定変更
*　　main.c　の変更　初期化削除　IO設定変更
*　　io_cfg.hの変更　LED削除　IOモニタ削除
*　　user.cの変更　　実行内容削除
*　　usbcfg.hの変更　IOモニタ削除
*
*********************************************************************
/** I N C L U D E S *************************************************/
#include <p18cxxx.h>
#include "system\typedefs.h"                        // Required
#include "system\usb\usb.h"                         // Required
#include "io_cfg.h"                                 // Required
#include "system\usb\usb_compile_time_validation.h" // Optional
#include	"delays.h"
#include "stdlib.h"

/*** Configuration *******/
#pragma config FOSC = HSPLL_HS
#pragma config WDT = OFF
#pragma config PLLDIV = 5
#pragma config CPUDIV = OSC1_PLL2
#pragma config USBDIV = 2
#pragma config PWRT = ON
#pragma config BOR = ON
#pragma config BORV = 43
#pragma config LVP = OFF
#pragma config VREGEN = ON
#pragma config MCLRE = ON
#pragma config PBADEN = OFF

/** V A R I A B L E S ***********************************************/
#pragma udata
char input_buffer[64];					// USB入力バッファ
char output_buffer[64];					// USB出力バッファ

struct PWMCont
{
	char	UpDown;						// 上昇下降フラグ
	unsigned int Value;					// PWM値
};
struct PWMCont PWM[6];					// 6色分 

unsigned int		Period;					// PWM周期
unsigned int		Count;
unsigned int		Interval;				// 色更新周期
char 			Flag;					// 色更新フラグ
char			Color;					// 色相値
char			ColorIndex = 0;			// 色相値取得インデックス
unsigned int		Max = 1023;				// 色更限界値設定
unsigned int		Min = 1;
unsigned int		ChangeInterval = 10;		// 色相更新周期
char			ChangeFlag = 0;			// 色相変更指示フラグ 
unsigned int		Random;

unsigned char MsgOK[3] = "OK";

/** P R I V A T E  P R O T O T Y P E S *******************************/
void ISRProcess(void);
void Illuminate(void);
unsigned char EERead(unsigned char adrs);
void EEWrite(unsigned char adrs, unsigned char data);
void InitEEPROM(void);
void Setup(void);

/** V E C T O R  M A P P I N G ***********************************/
#pragma code high_vector=0x08
void interrupt_at_high_vector(void)
{
    _asm goto ISRProcess _endasm
}
#pragma code

////// Timer0 Interrupt Process
////// 20msec Interval
#pragma interrupt ISRProcess
void ISRProcess(void)
{
	INTCONbits.TMR0IF = 0;							// Interrupt Flag Clear
	TMR0H = 0xF1;									// 
	TMR0L = 0x59;	
	if(Interval > 0)								// PWM更新時期か？
		Interval--;								// PWM更新周期-1	
	else	
	{
		Interval = (unsigned int)EERead(2)*256 + (unsigned int)EERead(3);
		Flag = 1;
		// 色相更新
		if(ChangeInterval > 0)						// 色相更新時期か？
	  		ChangeInterval--;						// 			
		else
		{
			ChangeInterval = (unsigned int)EERead(0xA)*256 + (unsigned int)EERead(0xB);
			ChangeFlag = 1;
		}
	}
}
	
#pragma code
/*********************************************************************
 * Function:        void main(void)
 ********************************************************************/
void main(void)
{
	char i;
	
	CMCON = 0x07;						// コンパレータオフ
   	LATB = 0;						// LED全消去
  	LATC  = 0x03;						// LED off
 	TRISA = 0x03;						// 
	TRISB = 0x0;						// 液晶表示
 	TRISC = 0x30;						// LED Output Mode
	// Timer0 初期化
	T0CON = 0x85;						// Internal 1/64
	TMR0H = 0xF1;						// 12MHz/64/3750=50Hz
	TMR0L = 0x59;
 	// A/D初期設定
 	ADCON1 = 0x0B;					// AN0-AN1 Analog Input
 	ADCON2 = 0x95;					// 4Tad, Fosc/16
 	ADCON0 = 0x01;					// AD on
	/// USB初期化
	mInitializeUSBDriver();         	// See usbdrv.h
	/// EEPROMデフォルト設定（デバッグ用）
//	InitEEPROM();
	/// 変数初期化 EEPROMから読み出し
	Setup();
	/// 割り込み許可
	INTCONbits.TMR0IE = 1;
	INTCONbits.GIE = 1;
    /// メインループ
    while(1)
    {
    		USBCheckBusStatus();        					// USB接続チェック
    		if(UCFGbits.UTEYE!=1)							// アイパターンモードオフか
        		USBDriverService();						// USBイベントポーリング
		CDCTxService();								// クラス送受信実行
		/// 受信データの処理実行
		if((usb_device_state >= CONFIGURED_STATE)&&(UCONbits.SUSPND==0))
		{
    			if(getsUSBUSART(input_buffer,64))			// データ受信ポール
    			{
            		LATCbits.LATC0 = !LATCbits.LATC0;		// LED反転
	    			switch(input_buffer[0])				// 最初の１文字チェック
	    			{
		    			case '0':						// OK応答
						if(mUSBUSARTIsTxTrfReady())
							putrsUSBUSART("OK");
          			 	break;
					case '1':
						for(i=0; i<0x1F; i++)			// Write except Random
						{
							EEWrite(i, input_buffer[i+1]);
							Setup();
						}
						break;
					case '9':
		    				InitEEPROM();
		    				Setup();				
						break;						
		    			default: break;
		    		}
    			}
    		}
   		/// PWM制御（Countを＋１しMaxで最初に戻る） 		
    		Count++;										// PWMステップアップ
    		if(Count == Period)							// 周期終わりか？
    		{ 
	    		Count = 0;								// 周期最初に戻す
	    		Period = (unsigned int)EERead(0)*256 + (unsigned int)EERead(1);
  			LATB = 0x3F;								// 全LEDオン
  			// PWM幅更新周期ならPWM更新（Interval)
			if(Flag)									// 更新周期か？
			{
				Flag = 0;							// 色更新フラグオフ
				Illuminate();							// イルミネーション実行
			}
 		}
		//// PWMオフ制御　（CountとPWM値が一致）
		if(Count == PWM[0].Value)
			LEDR1 = 0;
		if(Count == PWM[1].Value)
			LEDG1 = 0;
		if(Count == PWM[2].Value)
			LEDB1 = 0;
		if(Count == PWM[3].Value)
			LEDR2 = 0;
		if(Count == PWM[4].Value)
			LEDG2 = 0;
		if(Count == PWM[5].Value)
			LEDB2 = 0;
     }
}

//// イルミネート実行関数
void Illuminate(void)
{
	char i, Mask;
	
	Mask = 1;
	for(i=0; i<6; i++) {
		if(Color & Mask) {							// 色指定ありか？
			if(PWM[i].UpDown) {						// 上昇中か？
				PWM[i].Value++;						// PWM幅アップ
				if(PWM[i].Value >= Max) {				// 最大値まで到達か？
					PWM[i].Value = Max;				// 最大値で制限
					PWM[i].UpDown = 0;				// 下降に変更
				}
			}
			else {									// PWM下降中
				PWM[i].Value--;						// PWM値ダウン
				if(PWM[i].Value <= Min) {				// 最小値まで達したか？
					PWM[i].Value = Min;				// 最小値で制限
					PWM[i].UpDown = 1;				// 上昇に変更
					if(ChangeFlag == 1) {				// 色相変更指示ありか？
						ChangeFlag = 0;				// 指示クリア
						ColorIndex++;					// 色相インデックス更新
						Color = EERead(ColorIndex);	// 色相取得
						if((Color == 0) ||(ColorIndex > 0x20))
							Setup();					// 色相初期化				
					}								
				}
			}
		}
		else										// 色指定無しの場合
			PWM[i].Value = Min;						// MIN値に設定
		Mask = Mask * 2;								// 色指定ビットマスク更新
	}
}	

//// EEPROMからパラメータ設定（初期スタート時）
void Setup(void)
{
	char i;
	
	Period = (unsigned int)EERead(0)*256 + (unsigned int)EERead(1);
	Interval =(unsigned int)EERead(2)*256 + (unsigned int)EERead(3);
	Max = (unsigned int)EERead(4)*256 + (unsigned int)EERead(5);
	if(Max > Period) {
		Max = Period;
		EEWrite(4, (unsigned char)(Max / 256));		// 修正
		EEWrite(5, (unsigned char)(Max % 256));
	}
	Min = (unsigned int)EERead(8)*256 + (unsigned int)EERead(9);
	if((Min < 1) || (Min > 255)) {
		Min = 1;
		EEWrite(8, (unsigned char)(Min / 256));		// 修正
		EEWrite(9, (unsigned char)(Min % 256));
	}		
	ChangeInterval = (unsigned int)EERead(0x0A)*256 + (unsigned int)EERead(0x0B);
	ColorIndex = 0x10;
	Color = EERead(ColorIndex);
	Count = 0;
	/// PWM初期値ランダム設定（記憶値から設定）
	Random = (unsigned int)EERead(0x30)*256 + (unsigned int)EERead(0x31);
	Random = Random + 10;								// ランダム基数更新
	if(Random > Period-10)
		Random = 10;
	EEWrite(0x30, (unsigned char)(Random / 256));		// ランダム基数記憶
	EEWrite(0x31, (unsigned char)(Random % 256));	
	for(i=0; i<6; i++)
	{
		srand(Random++);								// PWM値ランダム設定
		PWM[i].Value = rand() >> 6;
//		PWM[i].Value = 1;							// for Debug
		PWM[i].UpDown = 0;
	}
}
	
/// EEPROM書き込み関数
void EEWrite(unsigned char adrs, unsigned char data)
{	
 	EECON1bits.EEPGD = 0;
 	EECON1bits.CFGS = 0;
  	EEADR = adrs;
  	EEDATA = data;
  	EECON1bits.WREN = 1;
  	EECON2 = 0x55; 				// 書き込みシーケンス  
  	EECON2 = 0xaa;  
  	EECON1bits.WR = 1; 
  	while (EECON1bits.WR);
  	PIR2bits.EEIF = 0;
}
/// EEPROM読み出し関数
unsigned char EERead(unsigned char adrs)
{
  	EECON1bits.EEPGD = 0; 
	EECON1bits.CFGS = 0;  	
  	EEADR = adrs;
  	EECON1bits.RD = 1;  
  	return(EEDATA); 
}

///// EEPROMデフォルト値設定
void InitEEPROM(void)
{
	EEWrite(0, 0x03);				// Period
	EEWrite(1, 0xFF);
	EEWrite(2, 0x00);				// Interval
	EEWrite(3, 0x01);
	EEWrite(4, 0x01);				// Max
	EEWrite(5, 0xFF);
	EEWrite(6, 0x00);				// 
	EEWrite(7, 0xFF);
	EEWrite(8, 0x00);				// Min
	EEWrite(9, 0x01);
	EEWrite(0x0A, 0x04);			// ChangeInterval
	EEWrite(0x0B, 0xFF);
	EEWrite(0x10, 0x09);			// pattern0
	EEWrite(0x11, 0x12);			// pattern1
	EEWrite(0x12, 0x24);			// pattern2
	EEWrite(0x13, 0x1B);			// pattern3
	EEWrite(0x14, 0x1E);			// pattern4
	EEWrite(0x15, 0x36);			// pattern5
	EEWrite(0x16, 0x2D);			// pattern6
	EEWrite(0x17, 0x3F);
	EEWrite(0x18, 0x21);
	EEWrite(0x19, 0x14);
	EEWrite(0x1A, 0x0C);
	EEWrite(0x1B, 0x33);
	EEWrite(0x1C, 0);
	EEWrite(0x1D, 0);
	EEWrite(0x1E, 0);
	EEWrite(0x1F, 0);
}
			

/******** EEPROM Memory Map
	 0 : Period High
	 1 : Period Low
	 2 : Interval High
	 3 : Interval Low
	 4 : Max High
	 5 : Max Low
	 6 : 
	 7 : 
	 8 : Min High
	 9 : Min Low
	 A : ChangeInterval High
	 B : ChangeInterval Low
	 C : 
	 D : 
	 E :
	 F :	
	10 : pattern[0]
	11 : pattern[1]
	12 : pattern[2]
	13 : pattern[3]
	14 : pattern[4]
	-------
	1F : pattern[16]
	30 : Random High
	31 : Random Low
*************************************/
