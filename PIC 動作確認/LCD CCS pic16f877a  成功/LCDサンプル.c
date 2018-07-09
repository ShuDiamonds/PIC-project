///////////////////////////////////////////
//　LCD test program
//　LCD is SC1602BSLB or SC1602BS*B
//////////////////////////////////////////
#include <16f877a.h>
#use fast_io(a)
#use delay(CLOCK=20000000)
#fuses HS,NOWDT,NOPROTECT
#define Bmode 0x0F			//port B initial mode
#define Amode 0				 //port A initial modeB
#byte db = 6				//port B

//////// Port define and link LCD library 
/*
#define rs PIN_D6		//chip select
#define rw PIN_D5			//read/write
#define stb PIN_D4			//strobe
*/
#define rs  PIN_D0
#define rw  PIN_D1
#define stb PIN_D2

#include <lcd_lib.c>

////////////////////////////////////////////////
// LCD test program main routine
// Display several message on LCD
// with some interval.
// Constant Message is send by lcd_data()
////////////////////////////////////////////// 

main(){
 set_tris_a(Amode);		 //mode set of port
 set_tris_b(Bmode);		 //lower is input
 set_tris_d(0);
	delay_ms(1000);
 lcd_init();				//initialize LCD
		output_high(PIN_C3);
	lcd_data("Hello World LCD");
	delay_ms(1000);
	 lcd_clear();
	lcd_cmd(0xC0);
	lcd_data("FUKUDA");
	delay_ms(1000);
	lcd_clear();
	
	
	
	
 while(1){					//endless loop
 	lcd_clear();			 //clear display
  lcd_data("Hello World LCD");
 	output_low(PIN_C3);
  delay_ms(1000);
  lcd_clear();			 //clear display
  lcd_data("message001");
 	output_high(PIN_C3);
  delay_ms(1000);			//wait 1sec
  lcd_clear();
  lcd_data("MESSAGE002");
 	output_low(PIN_C3);
  delay_ms(1000);
  lcd_clear();
  lcd_data("1234567890");
 	output_high(PIN_C3);
  delay_ms(1000);
  lcd_clear();
  lcd_data("abcdefghijklmnop");
  lcd_cmd(0xC0);			//second line
  lcd_data("qrstuvwxyz!#$%&'");
 	output_low(PIN_C3);
  delay_ms(3000);
 }
} 

      /*
      《解説》
#use delay(CLOCK=10000000)
　　これは液晶表示制御のライブラリ内でディレイ関数を使って
　　いるので必ずこの関数でクロックを指定しておく必要があり
　　ます。
#define Bmode 0x0F
#define Amode 0
#byte db = 6
　　ここで液晶表示器に使うポートの指定と初期モードの設定値を
　　定義しています。例では、データバスがポートB、制御はポートA
　　となっており、ポートBの下位４ビットは入力モードを初期値と
　　しています。
#define rs PIN_A2
#define rw PIN_A1
#define stb PIN_A0
　　ここで制御ピンの指定をしています。例ではポートAの0,1,2ピン
　　としています。
#include <lcd_lib.c>
　　これで関数ライブラリをインクルードします。
lcd_init();
　　初期化の関数で、パラメータは必要ありません。メインの最初で
　　実行する必要があります。
lcd_clear();
　　液晶表示を消去する関数で、パラメータは必要無く、無条件で
　　クリアします。
lcd_data("message001");
　　文字列を表示する関数の使用例で、""で囲われた文字列を
　　そのまま液晶に表示します。
　　この関数ではCCSのコンパイラの特徴である、パラメータが
　　charまたはint属性の時に、文字列をパラメータとして使うと、その
　　文字列を１文字の繰り返しとして実行することを利用しています。
　　これは標準のC関数と異なっています。
lcd_cmd(0xC0);
　　液晶表示器の制御コマンドの使用例で、これで２行目の最初に
　　カーソル位置が移動します。 
						*/



