/**********************************************************/
/*PWM関数ライブラリ                                       */
/*2012.3.9                                                */
/**********************************************************/

//----マクロ定義----------------------------------------------------------------
//タイマ番号マクロ		＊タイマはTimer4や5があっても、2か3しか使えない
#define TIMER2			0
#define TIMER3			1


//----関数プロトタイプ宣言------------------------------------------------------

//PWM初期化関数
void PWM1_Init(int TimerNo);
void PWM2_Init(int TimerNo);
void PWM3_Init(int TimerNo);
void PWM4_Init(int TimerNo);

//PWM開始関数
void PWM1_Start(int Value);
void PWM2_Start(int Value);
void PWM3_Start(int Value);
void PWM4_Start(int Value);

//PWM停止関数
void PWM1_Stop();
void PWM2_Stop();
void PWM3_Stop();
void PWM4_Stop();

//PWM値設定関数
void PWM1_SetValue(int Value);
void PWM2_SetValue(int Value);
void PWM3_SetValue(int Value);
void PWM4_SetValue(int Value);























