#include<16f877a.h>
#include<stdio.h>

#define RS_BAUD		9600
#define RS_TX		PIN_C6
#define RS_RX		PIN_C7
#define RUN_LED		PIN_C0

#use delay(clock = 20000000)
#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,BROWNOUT
#use rs232(BAUD = RS_BAUD , XMIT = RS_TX, RCV = RS_RX)  //rs232c設定
#use fast_io(a)
#use fast_io(b)
#use fast_io(c)
#use fast_io(d)
#use fast_io(e)
#byte port_a = 5
#byte port_b = 6
#byte port_c = 7
#byte port_d = 8
#byte port_e = 9

main()
{
	long int adc = 0;  
	set_tris_a(0x01);
	set_tris_b(0x00);
	set_tris_c(0x80);
	set_tris_d(0x00);
	set_tris_e(0x00);
	
	setup_adc_ports(ALL_ANALOG);
	setup_adc(ADC_CLOCK_INTERNAL);
	
	output_low(RUN_LED);   //動作確認
	delay_ms(30);
	while(1)
	{
	set_adc_channel(0);  //0チャンネルを使用
	delay_us(50);
	adc = read_adc();  //A/D変換値の読み込み
	printf("A/D変換の値\r%ld\n",adc);
	}
	return(0);
}