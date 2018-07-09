
#include <p18f67j60.h>


#pragma config DEBUG = OFF		//バックグラウンド・デバッグ・モード（BDMを使わない
//#pragma config ETHLED = ON		//Ethernet LED を使う		エラーになるのでコメントアウト
#pragma config XINST = OFF		//拡張命令セットとインデックスアドレスは 使 わない（ 従来 モード）選択ビット
#pragma config STVR = OFF		//Stack Overflow Resetを使わない
#pragma config WDT = OFF		//ウォッチドッグタイマーを使わない
#pragma config WDTPS = 32768	//ウォッチドッグタイマーを1:32768にする
#pragma config CP0 = OFF		//ブロック０（000800－001ＦＦＦｈ）のコードを保護しない
#pragma config FCMEN = OFF		//Fail-Safe Clock Monitorを使わない
#pragma config IESO = OFF		//Internal External Osc. Switchを使わない
#pragma config FOSC = HS		//外付け振動子 利用 の 高速 クロック（8ＭＨｚ 以上 ） 発振
#pragma config FOSC2 = OFF		//Default/Reset System Clock Select Bit　のことらしい

#pragma code


/**************************************************
*configについての解説
**************************************************/
/*			//configの設定はPICによって記述の仕方が違うので注意
			上のxconfigはpic16f67j60のものでほかのPICではコンパイルできない
			
			

	*Brown-out Resetとは、電圧がある一定いかになるとリセットをかける動作
	*Stack Overflow Resetとは、スタックオーバーフローが起きるとリセットをかける動作
	*Fail-Safe Clock Monitorとは、外部クロックが入力されなくなった場合に、それを検知して内部クロックで動くようにするオプションです
	*Internal External Osc. Switch、とは外部クロックが安定するまで内部クロックで動作。これを自動で行うオプションで
	*Low Voltage Programmingとは、低電圧書き込みモードのことで用は５V（PICによって変わる）で書き込む時に必要PICは買ったばかりの時はON（１）である
	*プリすケーラとは、クロックのスピードを遅くすること例えばPLLDIV = 2		２で 割 り 算 するつまり８MHzを４MHzにするということ
	*HCS08マイコンは，バックグラウンド・デバッグ・モード（BDM）と呼ばれるデバッグ・モードを装備しており，原則マイコンの動作を妨げることなくマイコンの内部状態を調べること
	*picのpllについて、#pragma config	FOSC = HS	で外付け振動子 利用 の 高速 クロック（8ＭＨｚ 以上 ） 発振
	でそれを#pragma config	FOSC = HSPLL_HSに変えると外付け振動子利用の高速クロック発振で、ＰＬＬを 使 う(HSPLL)　→ラトルズではこのモード
	になるそのうちPLLは、
#pragma config	PLLDIV = 1	プリスケーラ 使用 しない（4ＭＨｚの 発振器 入力 を 直接 利用 ）
#pragma config	PLLDIV = 2	２で 割 り 算 する（8ＭＨｚの 発振 器 入力 を 利用 ）
#pragma config	PLLDIV = 3	３で割り算する（１２ＭＨｚの発振器入力を利用）
#pragma config	PLLDIV = 4	４で割り算する（１６ＭＨｚの発振器入力を利用）
#pragma config	PLLDIV = 5	５で割り算する（２０ＭＨｚの発振器入力を利用） ->ラトルズ 基板 20ＭＨｚ／5＝4ＭＨｚ
#pragma config	PLLDIV = 6	６で割り算する（２４ＭＨｚの発振器入力を利用）
#pragma config	PLLDIV = 10	１０で割り算する（４０ＭＨｚの発振器入力を利用）
#pragma config	PLLDIV = 12	１２で割り算する（４８ＭＨｚの発振器入力を利用）
		になり例えば１０MHzで	#pragma config	PLLDIV = 2
								#pragma config	FOSC = HSPLL_HS
								とするとPICは20MHzで起動する		http://homepage3.nifty.com/mitt/pic/pic1320_04.html

使用例を載せておく
ここから

#pragma config MCLRE  = OFF		//マスタークリアを使わない
#pragma config PWRTEN = OFF		//Power-up Timerを使わない
#pragma config BOREN  = OFF		//Brown-out Resetを使わない		
#pragma config BORV   = 30		//Brown-out Voltageを3Vにする
#pragma config WDTEN  = OFF		//ウォッチドッグタイマーを使わない
#pragma config WDTPS  = 32768		//ウォッチドッグタイマーを1:32768にする
#pragma config STVREN = ON		//Stack Overflow Resetを使う
#pragma config FOSC   = IRC		//  内部クロック
//#pragma config	FOSC = HS	//外付け振動子 利用 の 高速 クロック（8ＭＨｚ 以上 ） 発振
#pragma config PLLEN  = ON		//わからない
#pragma config CPUDIV = NOCLKDIV		//CPU System Clock Postscaler
#pragma config USBDIV = OFF		//USBのclockをOSC1/OSC2から取る
#pragma config FCMEN  = OFF		//Fail-Safe Clock Monitorを使わない
#pragma config IESO   = OFF		//Internal External Osc. Switchを使わない
#pragma config HFOFST = OFF		//
#pragma config LVP    = ON		//Low Voltage Programmingを使う
#pragma config XINST  = OFF		//拡張命令セットとインデックスアドレスは 使 わない（ 従来 モード）選択ビット
#pragma config BBSIZ  = OFF		//Boot Block Sizeのサイズ指定っぽい
#pragma config CP0    = OFF		//ブロック０（000800－001ＦＦＦｈ）のコードを保護しない
#pragma config CP1    = OFF		//ブロック１（00２０00－00３ＦＦＦｈ）のコードを保護しない
#pragma config CPB    = OFF		//ブートブロック（0000０00－0007ＦＦｈ）のコードを保護しない
#pragma config WRT0   = OFF		//ブロック0 (000800-001FFFh) の書込み保護しない
#pragma config WRT1   = OFF		//ブロック1 (002000-003FFFh) の書込み保護しない
#pragma config WRTB   = OFF		//Boot block (000000-0007FFh)の書込み保護しない
#pragma config WRTC   = OFF		//Configuration registers (300000-3000FFh) の書込み保護しない
#pragma config EBTR0  = OFF		//Block 0 (000800-001FFFh) を他のブロック実行時のテーブル読み取りから保護しない
#pragma config EBTR1  = OFF		//Block 1 (002000-003FFFh) を他のブロック実行時のテーブル読み取りから保護しない
#pragma config EBTRB  = OFF		//Boot block (000000-0007FFh)を他のブロック実行時のテーブル読み取りから保護しない

#pragma code

ここまで



*/
void main(void){
	unsigned int	i;		//変数の宣言
	unsigned char	vLED = 0x01;	//変数の宣言と初期値の設定
	OSCCON = 0b01010010;		//  内部クロック4Mhz
	TRISC=0xF0;					//入出力設定
	while(1){					//繰り返しループ
		if(PORTBbits.RB7==1){				//ボタンが押されているか
			vLED = vLED>>1;
			if(vLED==0x00){vLED=0x08;}
		}else{
			vLED = vLED<<1;
			if(vLED==0x10){vLED=0x01;}
		}
		PORTC = vLED;
		for(i=20000;i>0;i--);		//順送りの間隔遅延
	}
}



