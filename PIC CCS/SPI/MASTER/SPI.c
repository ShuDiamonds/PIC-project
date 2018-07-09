#include<16f877a.h>

#fuses HS,NOWDT,NOPROTECT,NOLVP,PUT,BROWNOUT
#use delay(clock = 20000000)

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
//グローバル変数定義
	int l = 0;	//タイマーカウント値



main()
{ 
	int a=0;
	char DATA = 0;
	set_tris_a(0xff);
	set_tris_b(0xff);
	set_tris_c(0xf0);
	set_tris_d(0x00);
	set_tris_e(0xff);
	output_high(PIN_C0);
	//SPI設定
	setup_spi(SPI_MASTER | SPI_L_TO_H | SPI_CLK_DIV_16);

	while(1)
		{
			while(!spi_data_is_in())
			{
				//データ受信
				if( DATA = 'F')
				{	
					output_low(PIN_C0);
					delay_ms(400);
					output_high(PIN_C0);
					delay_ms(400);
					output_low(PIN_C0);
					delay_ms(400);
					output_high(PIN_C0);
					delay_ms(400);
					output_low(PIN_C0);
					delay_ms(400);
				}
			}
		}
}

