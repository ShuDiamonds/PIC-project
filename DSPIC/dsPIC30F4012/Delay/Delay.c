//�w�b�_�t�@�C�����C���N���[�h
#include "p30f4012.h"
//********************************************************************
//�R���t�B�M�����[�V�����E���[�h�̐ݒ�

	_FOSC(FRC & CSW_FSCM_OFF & FRC_PLL16);		//�����N���b�N�WMhz�g�p��PLL16��96MHz
	_FWDT(WDT_OFF);
	_FBORPOR(PBOR_OFF & PWRT_64 & MCLR_EN);
	_FGS(CODE_PROT_OFF);



//********************************************************************
//���C���֐�

int main(void)
{

//===================================================================
//�ϐ����`

	char a=0;
	int i=0;
	int j=0;
	int k=0;

	//===================================================================
	//���o�̓|�[�g�̐ݒ�A������

	TRISB = 0x00;
	TRISE = 0x00;
	PORTB = 0x00;
	PORTE = 0x00;  

//===================================================================

	while(1)
	{

		a = 0x01; 

		for(k=0;k<5;k++)
		{

			//�|�[�gB�ɏo��
			PORTB = a;

			//=================================================================
			//���ԉ҂��̂��߂�for���ŉ񂵂܂��B
			for(i = 0;i<10000;i++)
				{
					for(j=0;j<10;j++){}
				}
				//=================================================================

				//�P�V�t�g
				a = a<<1; 
		}

		//�Ȃ����|�[�gB�͂Ubit�����Ȃ��̂ŁA�|�[�gE���g���܂��B
		PORTB = 0x00;
		PORTE = 0x01;

		//=================================================================
		//��̂��Ƃ��A���ԉ҂��B
		for(i = 0;i<10000;i++)
		{
			
			for(j=0;j<10;j++){}
		
		}
		//=================================================================



		PORTE = 0x02;

		//=================================================================
		//���ԉ҂�
		for(i = 0;i<10000;i++)
		{
			for(j=0;j<10;j++){}
		}
		//=================================================================

		//�����Ƃ��B
		PORTE = 0x00;

	}
}