//�w�b�_�t�@�C�����C���N���[�h
#include "p30f4012.h"
#include "uart.h"
//********************************************************************
//�R���t�B�M�����[�V�����E���[�h�̐ݒ�

	_FOSC(CSW_FSCM_OFF & XT_PLL8);     //10MHz�Z�����b�N���g����PLL��80MHz
	_FWDT(WDT_OFF);
	_FBORPOR(PBOR_OFF & PWRT_64 & MCLR_EN);
	_FGS(CODE_PROT_OFF);

//********************************************************************
//UART�̐ݒ�p�����[�^
 
unsigned int config1 = UART_EN & UART_IDLE_CON & UART_ALTRX_ALTTX & UART_NO_PAR_8BIT & UART_1STOPBIT
                     & UART_DIS_WAKE & UART_DIS_LOOPBACK & UART_DIS_ABAUD;
 
unsigned int config2 = UART_INT_TX_BUF_EMPTY & UART_TX_PIN_NORMAL & UART_TX_ENABLE & UART_INT_RX_CHAR 
                     & UART_ADR_DETECT_DIS & UART_RX_OVERRUN_CLEAR;
 
 


//********************************************************************
//���C���֐�

int main(void)
{
	char temp;

	//==================================================================
	//UART�����ݒ�
	//���x��115kbps�Ƃ��܂��i80MHz�N���b�N�ł͎��p�I�ɍł������ݒ�j

	OpenUART1(config1,config2,10);



	//==================================================================
	//���C�����[�v

	while(1)
	{
		//�P������M��҂�
		while(!DataRdyUART1());

		//��M������temp�֕ۑ�
		temp = ReadUART1();

		//temp�𑗐M
		WriteUART1(temp);
	}

}
