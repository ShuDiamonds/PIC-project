/**********************************************************/
/*PWM�֐����C�u����                                       */
/*2012.3.9                                                */
/**********************************************************/

//----�}�N����`----------------------------------------------------------------
//�^�C�}�ԍ��}�N��		���^�C�}��Timer4��5�������Ă��A2��3�����g���Ȃ�
#define TIMER2			0
#define TIMER3			1


//----�֐��v���g�^�C�v�錾------------------------------------------------------

//PWM�������֐�
void PWM1_Init(int TimerNo);
void PWM2_Init(int TimerNo);
void PWM3_Init(int TimerNo);
void PWM4_Init(int TimerNo);

//PWM�J�n�֐�
void PWM1_Start(int Value);
void PWM2_Start(int Value);
void PWM3_Start(int Value);
void PWM4_Start(int Value);

//PWM��~�֐�
void PWM1_Stop();
void PWM2_Stop();
void PWM3_Stop();
void PWM4_Stop();

//PWM�l�ݒ�֐�
void PWM1_SetValue(int Value);
void PWM2_SetValue(int Value);
void PWM3_SetValue(int Value);
void PWM4_SetValue(int Value);























