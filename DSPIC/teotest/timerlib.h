/**********************************************************/
/*タイマ関数ライブラリ                                    */
/*2011.12.30                                              */
/**********************************************************/

//----マクロ定義----------------------------------------------------------------
//プリスケーラ値マクロ
#define PRESCALER_255	0b11
#define PRESCALER_64	0b10
#define PRESCALER_8		0b01
#define PRESCALER_1		0b00

//----関数プロトタイプ宣言------------------------------------------------------
//タイマ初期化関数
void Timer1_Init(int Prescaler);
void Timer2_Init(int Prescaler);
void Timer3_Init(int Prescaler);
void Timer4_Init(int Prescaler);
void Timer5_Init(int Prescaler);
void Timer23_Init(int Prescaler);
void Timer45_Init(int Prescaler);

//タイマ割り込み許可関数
void Timer1_IntEnable(int Level);
void Timer2_IntEnable(int Level);
void Timer3_IntEnable(int Level);
void Timer4_IntEnable(int Level);
void Timer5_IntEnable(int Level);

//タイマ割り込み禁止関数
void Timer1_IntDisable(void);
void Timer2_IntDisable(void);
void Timer3_IntDisable(void);
void Timer4_IntDisable(void);
void Timer5_IntDisable(void);

//タイマ開始関数
void Timer1_Start(int Value);
void Timer2_Start(int Value);
void Timer3_Start(int Value);
void Timer4_Start(int Value);
void Timer5_Start(int Value);
void Timer23_Start(long Value);
void Timer45_Start(long Value);

//タイマ停止関数
void Timer1_Stop(void);
void Timer2_Stop(void);
void Timer3_Stop(void);
void Timer4_Stop(void);
void Timer5_Stop(void);
void Timer23_Stop(void);
void Timer45_Stop(void);

//タイマ値取得関数
int  Timer1_Value(void);
int  Timer2_Value(void);
int  Timer3_Value(void);
int  Timer4_Value(void);
int  Timer5_Value(void);
long Timer23_Value(void);
long Timer45_Value(void);



