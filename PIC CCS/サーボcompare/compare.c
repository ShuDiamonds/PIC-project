//////////////////////////////////////////////////
// �o�v�l�e�X�g�v���O�����P											//
// PIC16F873								RIKIYA 2002.12.15  //
//															picpwm1.C  //
// TMR1�������ăT�[�{�[���[�^�p��PWM�g�`���o��  //
// CCP2�R���y�A�}�b�`�����Ŏ���20mS�𐶐�			  //
// CCP1�R���y�A�}�b�`�����Ńp���X���𐶐�				//
//////////////////////////////////////////////////
#define	SorvoPIN1	PIN_C2
#define	SorvoPIN2	PIN_B2

#include <16f877a.h>
#fuses HS,NOWDT,NOPROTECT,PUT,BROWNOUT,NOLVP
#use delay(clock = 20000000)					   // clock 20MHz
#use fast_io(C)											 // �Œ���̓��[�h

void ccp1_int(void);									 // �v���g�^�C�v
void ccp2_int(void);									 // �v���g�^�C�v
void intval();


//���C���֐�////////////////////////////////////
main()
{
	set_tris_a(0x00);								  
	set_tris_b(0x00);
	set_tris_c(0x00);								  //RC 7-0:OUTs
	set_tris_d(0x00);
	
	setup_timer_1(T1_INTERNAL | T1_DIV_BY_2);
	setup_ccp1(CCP_COMPARE_INT);		  // CCP1�R���y�A�}�b�`�����ݐݒ�
	setup_ccp2(CCP_COMPARE_INT);		  // CCP2�R���y�A�}�b�`�����ݐݒ�
	set_timer1(15535);						
	
	CCP_1 = 50000;									  // �p���X�� 0
	CCP_2 = 50000;							   // ���� 0.2u * 2 * 50000 = 20mS
	
	enable_interrupts(INT_CCP1);			   //CCP1�R���y�A�}�b�`�`�����݋���
	enable_interrupts(INT_CCP2);			   //CCP2�R���y�A�}�b	// TMR1�N���A
	enable_interrupts(INT_TIMER1);
	enable_interrupts(GLOBAL);			  //�S�ݒ芄���݋���

	while(1)
	{
			CCP_1 = 18035;						  //�p���X�� 1.5mS
			delay_ms(1000);
			CCP_1 = 19285;						  //�p���X�� 1.0mS
			delay_ms(1000);
			CCP_1 = 20535;						  //�p���X�� 2.0mS
			delay_ms(1000);
	}
}

//�p���X�N���A///////////////////////////////////
#INT_CCP1
void ccp1_int()
{
	  output_bit(SorvoPIN1,0);
}

//�p���X�o��/////////////////////////////////////
#INT_CCP2
void ccp2_int()
{
	output_bit(SorvoPIN2,0);
}
//	timer1���荞�݊֐�
#int_timer1
void intval() 
{
	
	set_timer1(15535);
	output_bit(SorvoPIN1,1);
	output_bit(SorvoPIN2,1);
	
}

/*	���l
������20ms�ɂ��邽�߂ɁAtimer1�̒l��15535�����Ă���̂ŁA
��������J�E���g�A�b�v�����B
�܂�T�[�{�̊p�x��0�x�ɂ��悤�Ƃ���΁A1ms�ɂ���΂悢�̂ŁA
1000us=0.2us * 2 * x ���Ax=2500 �J�E���g�K�v�ƂȂ�B
����āA15535 + 2500 = 18035 �̒l��CCP_1�ɓ����΂悢

���l��90�x����ɂ�
1500 = 0.4 * x
x=3750
����āA3750 + 15535 = 19285

�܂�180�x�̏ꍇ��
2000 = 0.4 * x
5000 + 15535 = 20535

*/
