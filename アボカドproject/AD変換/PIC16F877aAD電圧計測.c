/*********************************************
���̃v���O�����͓d����}��v���O�����ł�


**********************************************/
#include<16F877a.h>
#include<stdio.h>
#include<string.h>
#include<math.h>

#use delay(clock = 20000000)
#fuses HS,NOWDT,NOPROTECT,PUT,BROWNOUT,NOLVP
#define RS_BAUD		9600
#define RS_TX		PIN_C6
#define RS_RX		PIN_C7
#define RUN_LED		PIN_C0
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)  //rs232c�ݒ�



#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9



main()
{
	long int adc = 0; 
	float DDD = 0;
	set_tris_a(0xFF);
	set_tris_b(0xFF);
	set_tris_c(0x80);
	set_tris_d(0xFF);
	set_tris_e(0xFF);
	
	setup_adc_ports(ALL_ANALOG);
	setup_adc(ADC_CLOCK_INTERNAL);
	printf("AD start\n");
	while(1)
	{
		set_adc_channel(7);  //7�`�����l�����g�p
		//delay_ms(1000);
		output_high(PIN_C3);
		//adc = read_adc();  //A/D�ϊ��l�̓ǂݍ���
		//printf("\r%ld\n",adc);
		//printf("A/D�ϊ��̒l\r%ld\n",adc);
		delay_ms(200);
		output_low(PIN_C3);
		adc = read_adc();  //A/D�ϊ��l�̓ǂݍ���
		DDD = adc * 0.048828;
		printf("\r%ld\n",adc);
		printf("\rVCC = %f\n",DDD);

	}
}

