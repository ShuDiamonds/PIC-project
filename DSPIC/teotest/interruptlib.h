/**********************************************************/
/*���荞�݊֐����C�u����                                  */
/*2011.12.30                                              */
/**********************************************************/

//****�O�����荞��**************************************************************
//----�}�N���錾------------------------------------------------------
#define HI_TO_LOW		1
#define LOW_TO_HI		0

//----�֐��v���g�^�C�v�錾--------------------------------------------
void ExternalInt0_Enable(int Mode, int Level);
void ExternalInt1_Enable(int Mode, int Level);
void ExternalInt2_Enable(int Mode, int Level);

void ExternalInt0_Disable();
void ExternalInt1_Disable();
void ExternalInt2_Disable();

//****���荞��******************************************************************
void EnableInterrupt(void);					//���荞�݋��֐�
void DisableInterrupt(void);				//���荞�݋֎~�֐�

