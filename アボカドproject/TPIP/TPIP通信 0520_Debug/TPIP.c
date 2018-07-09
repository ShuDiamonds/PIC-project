#include<16f877a.h>
//#include<stdio.h>

//#define RS_BAUD		115200
#define RS_BAUD		9600
#define RS_TX		PIN_C6
#define RS_RX		PIN_C7
#define RUN_LED		PIN_C0
//�҂�}�N��
/*
//�S�Ί������[�^�[I/O
	#define	TEKKA_Mortor1_IO_R	PIN_A2
	#define	TEKKA_Mortor1_IO_L	PIN_A4
	#define	TEKKA_LED			PIN_E1
	#define	TEKKA_NOPIN			PIN_E0
	#define	TEKKA_Mortor2_IO_R	PIN_A3
	#define	TEKKA_Mortor2_IO_L	PIN_A5
	*/
	//���[�^�[PWM
	#define	TEKKA_Mortor1_PWM	PIN_C1
	#define	TEKKA_Mortor2_PWM	PIN_C2
	//�����ϊ������[�^�[I/O
	#define	KAPPA_LED1			PIN_D5	
	#define	KAPPA_LED2			PIN_D4	
	#define	KAPPA_Mortor1_IO_R	PIN_D0	
	#define	KAPPA_Mortor1_IO_L	PIN_C3
	#define	KAPPA_Mortor2_IO_R	PIN_D2	
	#define	KAPPA_Mortor2_IO_L	PIN_D3	
	#define	KAPPA_Mortor3_IO_R	PIN_C4	
	#define	KAPPA_Mortor3_IO_L	PIN_C5	
	#define	KAPPA_Mortor4_IO_R	PIN_D1	
	#define	KAPPA_Mortor4_IO_L	PIN_A0	
	//�����ϊ������[�^�[2I/O
	#define	KAPPA2_LED1			PIN_B6	
	#define	KAPPA2_LED2			PIN_B7	
	#define	KAPPA2_Mortor1_IO_R	PIN_D6
	#define	KAPPA2_Mortor1_IO_L	PIN_D7
	#define	KAPPA2_Mortor2_IO_R	PIN_B0	
	#define	KAPPA2_Mortor2_IO_L	PIN_B1	
	#define	KAPPA2_Mortor3_IO_R	PIN_B2	
	#define	KAPPA2_Mortor3_IO_L	PIN_B3	
	#define	KAPPA2_Mortor4_IO_R	PIN_B4	
	#define	KAPPA2_Mortor4_IO_L	PIN_B5	



#use delay(clock = 20000000)
#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,NOBROWNOUT
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)  //rs232c�ݒ�


//�֐��錾
void LED_tikatika( unsigned int16 a)
{
	output_high(RUN_LED);   //����m�F
	delay_ms(a);
	output_low(RUN_LED);   //����m�F
	delay_ms(a);
	output_high(RUN_LED);   //����m�F
	delay_ms(a);
	output_low(RUN_LED);   //����m�F
	delay_ms(a);
	output_high(RUN_LED);   //����m�F
	
	return;	
}
/*
#use fast_io(a)
#use fast_io(b)
#use fast_io(c)
#use fast_io(d)
#use fast_io(e)
*/
#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9
#byte ADCON1 = 0x9F			//�A�i���O�f�W�^���s���ݒ�	
main()
{
	//���[�J���ϐ���`
	char cheaker=0;	
	char data_H[10];
	char data_L[10];		//��M�f�[�^�i�[�X�y�[�X
	int16 E=0,F=0;				
	int ID=0,hugou=0,i=0;
	char motasuu1=0;
	int motasuu = 0;
	//������
	set_tris_a(0x01);
	set_tris_b(0x00);
	set_tris_c(0x80);
	set_tris_d(0x00);
	set_tris_e(0x00);
	ADCON1 = 0b00000111;		//�f�W�^���s���ݒ�	
	//PWM������
	/*
	setup_ccp1(CCP_PWM);
	setup_ccp2(CCP_PWM);
	SETUP_TIMER_2(T2_DIV_BY_16,0xFF,1);
	*/
	//���[�^�[������
	//�S�Ί���������
	/*
	set_pwm1_duty(0);
	set_pwm2_duty(0);
	output_low(TEKKA_Mortor1_IO_L);
	output_low(TEKKA_Mortor1_IO_R);
	output_low(TEKKA_Mortor2_IO_L);
	output_low(TEKKA_Mortor2_IO_R);
	*/
	//�����ϊ���������
	output_low(KAPPA_Mortor1_IO_L);
	output_low(KAPPA_Mortor1_IO_R);
	output_low(KAPPA_Mortor2_IO_L);
	output_low(KAPPA_Mortor2_IO_R);
	output_low(KAPPA_Mortor3_IO_L);
	output_low(KAPPA_Mortor3_IO_R);
	output_low(KAPPA_Mortor4_IO_L);
	output_low(KAPPA_Mortor4_IO_R);
		//�����ϊ���2������
	output_low(KAPPA2_Mortor1_IO_L);
	output_low(KAPPA2_Mortor1_IO_R);
	output_low(KAPPA2_Mortor2_IO_L);
	output_low(KAPPA2_Mortor2_IO_R);
	output_low(KAPPA2_Mortor3_IO_L);
	output_low(KAPPA2_Mortor3_IO_R);
	output_low(KAPPA2_Mortor4_IO_L);
	output_low(KAPPA2_Mortor4_IO_R);
	
	//LED������������m�F
	LED_tikatika(500);
	
	//output_high(TEKKA_LED);   //����m�F
	output_high(KAPPA_LED1);   //����m�F
	output_high(KAPPA_LED2);   //����m�F
	output_high(KAPPA2_LED1);   //����m�F
	output_high(KAPPA2_LED2);   //����m�F
	
	//delay_ms(30);
	while(1)		//�A�C�h�����[�v
	{
		//printf("1\n\r");
		while(1)			//��M���[�v
		{	
		//�X�^�[�g�f�[�^�҂�
		//	printf("2\n\r");
			while(cheaker != '@')
			{
				//printf("3\n\r");
				if(kbhit())
				{
				cheaker = getc();
				}
			}
			//LED_tikatika(500);
			//printf("\n\r receive StartBit \n\r");
		//���[�^�̐����m�F
			motasuu1 = getc();
			motasuu = motasuu1;
		//�f�[�^��M
			for(i=1;motasuu >= i;i++)
			{
				//printf("4\n\r");
				data_H[i] =getc();
				data_L[i] =getc();
			}
		//�X�g�b�v�f�[�^�m�F
			cheaker = getc();
			
			if(cheaker != '*')
			{
				putc('N');
				//printf("\n\r No stop bit so breaked \n\r");
				break;
			}
			
		//�f�[�^����
			for(i=1;motasuu >= i;i++)
				{
					//printf("\n\r ID convert \n\r");
					//ID�̎��o��
					ID = data_H[i] & 0b11111000;
					ID = ID>>3;
					//�������o��
					hugou = data_H[i] & 0b0000100;
					hugou = hugou>>2;
					//PWM�f�[�^���o��
					F = data_H[i] & 0b0000011;		//PWM���2bit���o��
					F = F<<8;						//PWM�f�[�^��8�r�b�g���ɃV�t�g
					E = data_L[i];					
					F = F | E;						//PWM�̃f�[�^�̏�ʂƉ��ʂ�OR����(F��PWM�p�̃f�[�^���͂����Ă�)
					
					//��M�f�[�^������
					data_H[i]=0;
					data_L[i]=0;
					
					
					/*
					//PWM�f�[�^����
					if(F <=370 && F != 0)			//�X�g�b�v�ł͂Ȃ��APWM��370�ȉ��̂Ƃ�
					{
						printf("6\n\r");
						//printf("\n\r Less PWM data \n\r");
						F = 420;
					}
					*/
					
					
					printf("\n\r switching \n\r");
				//�f�[�^���s
					switch (ID)						//���[�^�[����
					{
						case 0x01:					//�����σ��[�^�[�P
								if(F)						//���[�^�[����������
								{
									
									if(hugou)
									{
										printf("\n\r kappa moter1 hugou=1 \n\r");
										output_high(KAPPA_Mortor1_IO_R);
										output_low(KAPPA_Mortor1_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter1 hugou=0 \n\r");
										output_high(KAPPA_Mortor1_IO_L);
										output_low(KAPPA_Mortor1_IO_R);
										break;
									}
								}else						//���[�^�[�𓮂����Ȃ��Ƃ�
								{
										printf("\n\r kappa moter1 No action \n\r");
										output_low(KAPPA_Mortor1_IO_L);
										output_low(KAPPA_Mortor1_IO_R);
									break;
								}
						case 0x02:					//�����σ��[�^�[�Q
								if(F)						//���[�^�[����������
								{
									if(hugou)
									{
										printf("\n\r kappa moter2 hugou=1 \n\r");
										output_high(KAPPA_Mortor2_IO_R);
										output_low(KAPPA_Mortor2_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter2 hugou=0 \n\r");
										output_high(KAPPA_Mortor2_IO_L);
										output_low(KAPPA_Mortor2_IO_R);
										break;
									}
								}else						//���[�^�[�𓮂����Ȃ��Ƃ�
								{
										printf("\n\r kappa moter2 No action \n\r");
										output_low(KAPPA_Mortor2_IO_L);
										output_low(KAPPA_Mortor2_IO_R);
									break;
								}
						case 0x03:					//�����σ��[�^�[�R
								if(F)						//���[�^�[����������
								{
									if(hugou)
									{
										printf("\n\r kappa moter3 hugou=1 \n\r");
										output_high(KAPPA_Mortor3_IO_R);
										output_low(KAPPA_Mortor3_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter3 hugou=0 \n\r");
										output_high(KAPPA_Mortor3_IO_L);
										output_low(KAPPA_Mortor3_IO_R);
										break;
									}
								}else						//���[�^�[�𓮂����Ȃ��Ƃ�
								{
										printf("\n\r kappa moter3 No action \n\r");
										output_low(KAPPA_Mortor3_IO_L);
										output_low(KAPPA_Mortor3_IO_R);
									break;
								}
						case 0x04:					//�����σ��[�^�[�S
								if(F)						//���[�^�[����������
								{
									if(hugou)
									{
										printf("\n\r kappa moter4 hugou=1 \n\r");
										output_high(KAPPA_Mortor4_IO_R);
										output_low(KAPPA_Mortor4_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter4 hugou=0 \n\r");
										output_high(KAPPA_Mortor4_IO_L);
										output_low(KAPPA_Mortor4_IO_R);
										break;
									}
								}else						//���[�^�[�𓮂����Ȃ��Ƃ�
								{
										printf("\n\r kappa moter4 No action \n\r");
										output_low(KAPPA_Mortor4_IO_L);
										output_low(KAPPA_Mortor4_IO_R);
									break;
								}
						
						//�������炩���σ|�[�g2�Ԗ�
						case 0x05:					//�����σ��[�^�[5
								if(F)						//���[�^�[����������
								{
									if(hugou)
									{
										printf("\n\r kappa moter5 hugou=1 \n\r");
										output_high(KAPPA2_Mortor1_IO_R);
										output_low(KAPPA2_Mortor1_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter5 hugou=0 \n\r");
										output_high(KAPPA2_Mortor1_IO_L);
										output_low(KAPPA2_Mortor1_IO_R);
										break;
									}
								}else						//���[�^�[�𓮂����Ȃ��Ƃ�
								{
									printf("\n\r kappa moter5 No action \n\r");
										output_low(KAPPA2_Mortor1_IO_L);
										output_low(KAPPA2_Mortor1_IO_R);
									break;
								}
						
						case 0x06:					//�����σ��[�^�[6
								if(F)						//���[�^�[����������
								{
									if(hugou)
									{
										printf("\n\r kappa moter6 hugou=1 \n\r");
										output_high(KAPPA2_Mortor2_IO_R);
										output_low(KAPPA2_Mortor2_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter6 hugou=0 \n\r");
										output_high(KAPPA2_Mortor2_IO_L);
										output_low(KAPPA2_Mortor2_IO_R);
										break;
									}
								}else						//���[�^�[�𓮂����Ȃ��Ƃ�
								{
									printf("\n\r kappa moter6 No action \n\r");
										output_low(KAPPA2_Mortor2_IO_L);
										output_low(KAPPA2_Mortor2_IO_R);
									break;
								}
						
						case 0x07:					//�����σ��[�^�[7
								if(F)						//���[�^�[����������
								{
									if(hugou)
									{
										printf("\n\r kappa moter7 hugou=1 \n\r");
										output_high(KAPPA2_Mortor3_IO_R);
										output_low(KAPPA2_Mortor3_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter7 hugou=0 \n\r");
										output_high(KAPPA2_Mortor3_IO_L);
										output_low(KAPPA2_Mortor3_IO_R);
										break;
									}
								}else						//���[�^�[�𓮂����Ȃ��Ƃ�
								{
									printf("\n\r kappa moter7 No action \n\r");
										output_low(KAPPA2_Mortor3_IO_L);
										output_low(KAPPA2_Mortor3_IO_R);
									break;
								}
						
						case 0x08:					//�����σ��[�^�[8
								if(F)						//���[�^�[����������
								{
									if(hugou)
									{
										printf("\n\r kappa moter8 hugou=1 \n\r");
										output_high(KAPPA2_Mortor4_IO_R);
										output_low(KAPPA2_Mortor4_IO_L);
										break;
									}else if(!hugou)
									{
										printf("\n\r kappa moter8 hugou=0 \n\r");
										output_high(KAPPA2_Mortor4_IO_L);
										output_low(KAPPA2_Mortor4_IO_R);
										break;
									}
								}else						//���[�^�[�𓮂����Ȃ��Ƃ�
								{
									printf("\n\r kappa moter8 No action \n\r");
										output_low(KAPPA2_Mortor4_IO_L);
										output_low(KAPPA2_Mortor4_IO_R);
									break;
								}						
						
						
						
						default:	printf("\n\r default ID \n\r");
							break;
					
					}
					
				}	//for�������o��
			
			
		//�ϐ�������
			printf("\n\r format of data \n\r");
			cheaker=0;
			motasuu=0;
			motasuu1=0;
			 E=0;
			F=0;
			ID=0;
			hugou=0;
			i=0;
		}
		printf("\n\r End of one communication \n\r");		//����I�������ꍇ���s����Ȃ�
	}		//�A�C�h�����[�v�I��
	
	printf("\n\r End main \n\r");
	return(0);
}

/*�Q�l	getc�ɂ���
�@�\: RS-232C RCV �s�����當����ǂݍ��݁A������Ԃ��܂��B
���̊֐��͕������͂��L��܂ő҂������܂��B���̏�Ԃ���������ꍇ�́A���̊֐��̎g�p���O��
�֐�kbhit()���g���ĕ������͉\���ۂ��̃e�X�g�����ĉ������B
�����A����USART �@�\������΁A�n�[�h�E�G�A�͂R�������o�b�t�@���邱�Ƃ��o���܂��B
������΁APIC �ɂ���ĕ�������M����Ă���ԁA�����̎�肱�ڂ��𖳂����ׁAGETC �Ŏ�荞
�݂��p������K�v������܂��Bfgetc()�g�p���́A�w�肳�ꂽ�X�g���[������̓��͂����܂��B
getc()�̃f�t�H���g�E�X�g���[����STDIN �ł��B�Ⴕ���́A�v���O�����I�ɂ���ȑO�Ŏg�p���ꂽ�A
�X�g���[�����w�肳�ꂽ�ƌ��Ȃ��ď������܂��B
*/


