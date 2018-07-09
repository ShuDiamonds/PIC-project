/**********************************************************/
/*タイマ関数ライブラリ                                    */
/*2011.12.30                                              */
/**********************************************************/
#include "p33Fxxxx.h"
#include "timerlib.h"

//====Timer1初期化==============================================================
void Timer1_Init(int Prescaler)
{
	//----T1CON：タイプAタイマ制御レジスタ----
	T1CONbits.TON = 0;				//Timer1を停止する
	T1CONbits.TSIDL = 0;			//アイドルモード時もタイマ動作を継続する
	T1CONbits.TGATE = 0;			//ゲート時間の積算は無効
	T1CONbits.TCKPS = Prescaler;	//プリスケール値選択
	T1CONbits.TSYNC = 0;			//外部クロック同期、内部クロック使用するので無視
	T1CONbits.TCS = 0;				//内部クロック(Tosc/2)選択
	//----TMR1：タイマカウントレジスタ----
	TMR1 = 0;
	//----PR1：タイマ周期レジスタ----
	PR1 = 9;
}

//====Timer2初期化==============================================================
void Timer2_Init(int Prescaler)
{
	//----T2CON：タイプBタイマ制御レジスタ----
	T2CONbits.TON = 0;				//Timer2を停止する
	T2CONbits.TSIDL = 0;			//アイドルモード時もタイマ動作を継続する
	T2CONbits.TGATE = 0;			//ゲート時間の積算は無効
	T2CONbits.TCKPS = Prescaler;	//プリスケール値選択
	T2CONbits.T32 = 0;				//個別16bitタイマとして使用
	T2CONbits.TCS = 0;				//内部クロック(Tosc/2)選択
	//----TMR2：タイマカウントレジスタ----
	TMR2 = 0;
	//----PR2：タイマ周期レジスタ----
	PR2 = 9;
}

//====Timer3初期化==============================================================
void Timer3_Init(int Prescaler)
{
	//----T3CON：タイプCタイマ制御レジスタ----
	T3CONbits.TON = 0;				//Timer3を停止する
	T3CONbits.TSIDL = 0;			//アイドルモード時もタイマ動作を継続する
	T3CONbits.TGATE = 0;			//ゲート時間の積算は無効
	T3CONbits.TCKPS = Prescaler;	//プリスケール値選択
	T3CONbits.TCS = 0;				//内部クロック(Tosc/2)選択
	//----TMR3：タイマカウントレジスタ----
	TMR3 = 0;
	//----PR3：タイマ周期レジスタ----
	PR3 = 9;
}

//====Timer4初期化==============================================================
void Timer4_Init(int Prescaler)
{
	//----T4CON：タイプBタイマ制御レジスタ----
	T4CONbits.TON = 0;				//Timer4を停止する
	T4CONbits.TSIDL = 0;			//アイドルモード時もタイマ動作を継続する
	T4CONbits.TGATE = 0;			//ゲート時間の積算は無効
	T4CONbits.TCKPS = Prescaler;	//プリスケール値選択
	T4CONbits.T32 = 0;				//個別16bitタイマとして使用
	T4CONbits.TCS = 0;				//内部クロック(Tosc/2)選択
	//----TMR4：タイマカウントレジスタ----
	TMR4 = 0;
	//----PR4：タイマ周期レジスタ----
	PR4 = 9;
}

//====Timer5初期化==============================================================
void Timer5_Init(int Prescaler)
{
	//----T5CON：タイプCタイマ制御レジスタ----
	T5CONbits.TON = 0;				//Timer5を停止する
	T5CONbits.TSIDL = 0;			//アイドルモード時もタイマ動作を継続する
	T5CONbits.TGATE = 0;			//ゲート時間の積算は無効
	T5CONbits.TCKPS = Prescaler;	//プリスケール値選択
	T5CONbits.TCS = 0;				//内部クロック(Tosc/2)選択
	//----TMR5：タイマカウントレジスタ----
	TMR5 = 0;
	//----PR5：タイマ周期レジスタ----
	PR5 = 9;
}

//====Timer23初期化=============================================================
void Timer23_Init(int Prescaler)
{
	//----T2CON：タイプBタイマ制御レジスタ----
	T2CONbits.TON = 0;				//Timer2を停止する
	T2CONbits.TSIDL = 0;			//アイドルモード時もタイマ動作を継続する
	T2CONbits.TGATE = 0;			//ゲート時間の積算は無効
	T2CONbits.TCKPS = Prescaler;	//プリスケール値選択
	T2CONbits.T32 = 1;				//タイマ2,3で32bitタイマとして使用
	T2CONbits.TCS = 0;				//内部クロック(Tosc/2)選択
	//----TMR2,3：タイマカウントレジスタ----
	TMR2 = 0;
	TMR3 = 0;
	//----PR2, 3：タイマ周期レジスタ----
	PR2 = 18;
	PR3 = 18;
}

//====Timer45初期化=============================================================
void Timer45_Init(int Prescaler)
{
	//----T4CON：タイプBタイマ制御レジスタ----
	T4CONbits.TON = 0;				//Timer4を停止する
	T4CONbits.TSIDL = 0;			//アイドルモード時もタイマ動作を継続する
	T4CONbits.TGATE = 0;			//ゲート時間の積算は無効
	T4CONbits.TCKPS = Prescaler;	//プリスケール値選択
	T4CONbits.T32 = 1;				//タイマ4,5で32bitタイマとして使用
	T4CONbits.TCS = 0;				//内部クロック(Tosc/2)選択
	//----TMR4,5：タイマカウントレジスタ----
	TMR4 = 0;
	TMR5 = 0;
	//----PR4, 5：タイマ周期レジスタ----
	PR4 = 18;
	PR5 = 18;
}

//====Timer1割り込み許可関数====================================================
void Timer1_IntEnable(int Level)
{
	//----タイマ割り込み許可----
	IPC0bits.T1IP = Level;			//タイマ割り込みレベル設定
	IFS0bits.T1IF = 0;				//タイマ割り込みフラグクリア
	IEC0bits.T1IE = 1;				//タイマ割り込み許可
}

//====Timer2割り込み許可関数====================================================
void Timer2_IntEnable(int Level)
{
	//----タイマ割り込み許可----
	IPC1bits.T2IP = Level;			//タイマ割り込みレベル設定
	IFS0bits.T2IF = 0;				//タイマ割り込みフラグクリア
	IEC0bits.T2IE = 1;				//タイマ割り込み許可
}

//====Timer3割り込み許可関数====================================================
void Timer3_IntEnable(int Level)
{
	//----タイマ割り込み許可----
	IPC2bits.T3IP = Level;			//タイマ割り込みレベル設定
	IFS0bits.T3IF = 0;				//タイマ割り込みフラグクリア
	IEC0bits.T3IE = 1;				//タイマ割り込み許可
}

//====Timer4割り込み許可関数====================================================
void Timer4_IntEnable(int Level)
{
	//----タイマ割り込み許可----
	IPC6bits.T4IP = Level;			//タイマ割り込みレベル設定
	IFS1bits.T4IF = 0;				//タイマ割り込みフラグクリア
	IEC1bits.T4IE = 1;				//タイマ割り込み許可
}

//====Timer5割り込み許可関数====================================================
void Timer5_IntEnable(int Level)
{
	//----タイマ割り込み許可----
	IPC7bits.T5IP = Level;			//タイマ割り込みレベル設定
	IFS1bits.T5IF = 0;				//タイマ割り込みフラグクリア
	IEC1bits.T5IE = 1;				//タイマ割り込み許可
}

//====Timer1割り込み禁止関数====================================================
void Timer1_IntDisable(void)
{
	IEC0bits.T1IE = 0;				//タイマ割り込み禁止
}

//====Timer2割り込み禁止関数====================================================
void Timer2_IntDisable(void)
{
	IEC0bits.T2IE = 0;				//タイマ割り込み禁止
}

//====Timer3割り込み禁止関数====================================================
void Timer3_IntDisable(void)
{
	IEC0bits.T3IE = 0;				//タイマ割り込み禁止
}

//====Timer4割り込み禁止関数====================================================
void Timer4_IntDisable(void)
{
	IEC1bits.T4IE = 0;				//タイマ割り込み禁止
}

//====Timer5割り込み禁止関数====================================================
void Timer5_IntDisable(void)
{
	IEC1bits.T5IE = 0;				//タイマ割り込み禁止
}

//====Timer1開始関数============================================================
void Timer1_Start(int Value)
{
	//----TMR1：タイマカウントレジスタ----
	TMR1 = 0;
	//----PR1：タイマ周期レジスタ----
	PR1 = Value;
	//----T1CON：タイプAタイマ制御レジスタ----
	T1CONbits.TON = 1;				//Timer1を開始する
}

//====Timer2開始関数============================================================
void Timer2_Start(int Value)
{
	//----TMR2：タイマカウントレジスタ----
	TMR2 = 0;
	//----PR2：タイマ周期レジスタ----
	PR2 = Value;
	//----T2CON：タイプBタイマ制御レジスタ----
	T2CONbits.TON = 1;				//Timer2を開始する
}

//====Timer3開始関数============================================================
void Timer3_Start(int Value)
{
	//----TMR3：タイマカウントレジスタ----
	TMR3 = 0;
	//----PR3：タイマ周期レジスタ----
	PR3 = Value;
	//----T3CON：タイプCタイマ制御レジスタ----
	T3CONbits.TON = 1;				//Timer3を開始する
}

//====Timer4開始関数============================================================
void Timer4_Start(int Value)
{
	//----TMR4：タイマカウントレジスタ----
	TMR4 = 0;
	//----PR4：タイマ周期レジスタ----
	PR4 = Value;
	//----T4CON：タイプBタイマ制御レジスタ----
	T4CONbits.TON = 1;				//Timer4を開始する
}

//====Timer5開始関数============================================================
void Timer5_Start(int Value)
{
	//----TMR5：タイマカウントレジスタ----
	TMR5 = 0;
	//----PR5：タイマ周期レジスタ----
	PR5 = Value;
	//----T5CON：タイプCタイマ制御レジスタ----
	T5CONbits.TON = 1;				//Timer5を開始する
}

//====Timer23開始関数===========================================================
void Timer23_Start(long Value)
{
	//----TMR2, TMR3HLD：タイマカウントレジスタ----
	TMR3HLD = 0;
	TMR2 = 0;
	//----PR2,3：タイマ周期レジスタ----
	PR2 = Value >> 16;
	PR3 = 0x0000FFFF & Value;
	//----T2CON：タイプBタイマ制御レジスタ----
	T2CONbits.TON = 1;				//Timer23を開始する
}

//====Timer45開始関数===========================================================
void Timer45_Start(long Value)
{
	//----TMR4, TMR5HLD：タイマカウントレジスタ----
	TMR5HLD = 0;
	TMR4 = 0;
	//----PR4,5：タイマ周期レジスタ----
	PR4 = Value >> 16;
	PR5 = 0x0000FFFF & Value;
	//----T4CON：タイプBタイマ制御レジスタ----
	T4CONbits.TON = 1;				//Timer45を開始する
}

//====Timer1停止関数============================================================
void Timer1_Stop()
{
	//----T1CON：タイプAタイマ制御レジスタ----
	T1CONbits.TON = 0;				//Timer1を停止する
}

//====Timer2停止関数============================================================
void Timer2_Stop()
{
	//----T2CON：タイプBタイマ制御レジスタ----
	T2CONbits.TON = 0;				//Timer2を停止する
}

//====Timer3停止関数============================================================
void Timer3_Stop()
{
	//----T3CON：タイプCタイマ制御レジスタ----
	T3CONbits.TON = 0;				//Timer3を停止する
}

//====Timer4停止関数============================================================
void Timer4_Stop()
{
	//----T4CON：タイプBタイマ制御レジスタ----
	T4CONbits.TON = 0;				//Timer4を停止する
}

//====Timer5停止関数============================================================
void Timer5_Stop()
{
	//----T5CON：タイプCタイマ制御レジスタ----
	T5CONbits.TON = 0;				//Timer5を停止する
}

//====Timer23停止関数===========================================================
void Timer23_Stop()
{
	//----T2CON：タイプBタイマ制御レジスタ----
	T2CONbits.TON = 0;				//Timer23を停止する
}

//====Timer45停止関数===========================================================
void Timer45_Stop()
{
	//----T4CON：タイプBタイマ制御レジスタ----
	T4CONbits.TON = 0;				//Timer45を停止する
}

//====Timer1の値を返す==========================================================
int  Timer1_Value()
{
	return(TMR1);
}

//====Timer2の値を返す==========================================================
int  Timer2_Value()
{
	return(TMR2);
}

//====Timer3の値を返す==========================================================
int  Timer3_Value()
{
	return(TMR3);
}

//====Timer4の値を返す==========================================================
int  Timer4_Value()
{
	return(TMR4);
}

//====Timer5の値を返す==========================================================
int  Timer5_Value()
{
	return(TMR5);
}

//====Timer23の値を返す=========================================================
long Timer23_Value()
{
	long Value = 0;
	
	//TMR2を読んでからTMR3HLDを読み込むこと
	Value = 0x0000FFFF & TMR2;
	Value += 0xFFFF0000 & ((long)TMR3HLD << 16);
	
	return(Value);
}

//====Timer45の値を返す=========================================================
long Timer45_Value()
{
	long Value = 0;
	
	//TMR4を読んでからTMR5HLDを読み込むこと
	Value = 0x0000FFFF & TMR4;
	Value += 0xFFFF0000 & ((long)TMR5HLD << 16);
	
	return(Value);
}

