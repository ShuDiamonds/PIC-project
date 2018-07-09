/**********************************************************/
/*割り込み関数ライブラリ                                  */
/*2011.12.30                                              */
/**********************************************************/

//****外部割り込み**************************************************************
//----マクロ宣言------------------------------------------------------
#define HI_TO_LOW		1
#define LOW_TO_HI		0

//----関数プロトタイプ宣言--------------------------------------------
void ExternalInt0_Enable(int Mode, int Level);
void ExternalInt1_Enable(int Mode, int Level);
void ExternalInt2_Enable(int Mode, int Level);

void ExternalInt0_Disable();
void ExternalInt1_Disable();
void ExternalInt2_Disable();

//****割り込み******************************************************************
void EnableInterrupt(void);					//割り込み許可関数
void DisableInterrupt(void);				//割り込み禁止関数

