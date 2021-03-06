;******************************
;時計プログラム
;******************************
list	P=PIC18F2320	
		#include	<P18F2320.inc>
		
		errorlevel  -302 
		
		;__CONFIG	_CONFIG1H,  _IESO_OFF_1H & _FSCM_OFF_1H & _HSPLL_OSC_1H
		CONFIG		
		OSC = LP,	
		FSCM = OFF,	
		IESO = OFF,
		PWRT = OFF,
		BOR = OFF,
		BORV = 20,
		WDT = OFF,
		WDTPS = 1,
		MCLRE = ON,
		PBAD = DIG,
		;PBAD = ANA,
		CCP2MX = C1,
		STVR = ON,
		LVP = ON,
		DEBUG = OFF,
		CP0 = OFF,
		CP1 = OFF,
		CP2 = OFF,
		CP3 = OFF,
		CPB = OFF,
		CPD = OFF,
		WRT0 = OFF,
		WRT1 = OFF,
		WRT2 = OFF,
		WRT3 = OFF,
		WRTB = OFF,
		WRTC = OFF,
		WRTD = OFF,
		EBTR0 = OFF,
		EBTR1 = OFF,
		EBTR2 = OFF,
		EBTR3 = OFF,
		EBTRB = OFF
		
		
		
;/**************************************************
;*configについての解説
;**************************************************/
;/*
;
;	*Brown-out Resetとは、電圧がある一定いかになるとリセットをかける動作
;	*Stack Overflow Resetとは、スタックオーバーフローが起きるとリセットをかける動作
;	*Fail-Safe Clock Monitorとは、外部クロックが入力されなくなった場合に、それを検知して内部クロックで動くようにするオプションです
;	*Internal External Osc. Switch、とは外部クロックが安定するまで内部クロックで動作。これを自動で行うオプションで
;	*Low Voltage Programmingとは、低電圧書き込みモードのことで用は５Vで書き込む時に必要PICは買ったばかりの時はON（１）である
;	*プリすケーラとは、クロックのスピードを遅くすること例えばPLLDIV = 2	２で 割 り 算 するつまり８MHzを４MHzにするということ
;	*picのpllについて、#pragma config	FOSC = HS	で外付け振動子 利用 の 高速 クロック（8ＭＨｚ 以上 ） 発振
;	でそれを#pragma config	FOSC = HSPLL_HSに変えると外付け振動子利用の高速クロック発振で、ＰＬＬを 使 う(HSPLL)　→ラトルズではこのモード
;	になるそのうちPLLは、
;#pragma config	PLLDIV = 1	プリスケーラ 使用 しない（4ＭＨｚの 発振器 入力 を 直接 利用 ）
;#pragma config	PLLDIV = 2	２で 割 り 算 する（8ＭＨｚの 発振 器 入力 を 利用 ）
;#pragma config	PLLDIV = 3	３で割り算する（１２ＭＨｚの発振器入力を利用）
;#pragma config	PLLDIV = 4	４で割り算する（１６ＭＨｚの発振器入力を利用）;
;#pragma config	PLLDIV = 5	５で割り算する（２０ＭＨｚの発振器入力を利用） ->ラトルズ 基板 20ＭＨｚ／5＝4ＭＨｚ
;#pragma config	PLLDIV = 6	６で割り算する（２４ＭＨｚの発振器入力を利用）
;#pragma config	PLLDIV = 10	１０で割り算する（４０ＭＨｚの発振器入力を利用）
;#pragma config	PLLDIV = 12	１２で割り算する（４８ＭＨｚの発振器入力を利用）
;		になり例えば１０MHzで	#pragma config	PLLDIV = 2
;								#pragma config	FOSC = HSPLL_HS
;								とするとPICは20MHzで起動する
;
;使用例を載せておく
;ここから
;
;#pragma config MCLRE  = OFF		//マスタークリアを使わない
;#pragma config PWRTEN = OFF		//Power-up Timerを使わない
;#pragma config BOREN  = OFF		//Brown-out Resetを使わない		
;#pragma config BORV   = 30		//Brown-out Voltageを3Vにする
;#pragma config WDTEN  = OFF		//ウォッチドッグタイマーを使わない
;#pragma config WDTPS  = 32768		//ウォッチドッグタイマーを1:32768にする
;#pragma config STVREN = ON		//Stack Overflow Resetを使う
;#pragma config FOSC   = IRC		//  内部クロック
;//#pragma config	FOSC = HS	//外付け振動子 利用 の 高速 クロック（8ＭＨｚ 以上 ） 発振
;#pragma config PLLEN  = ON		//わからない
;#pragma config CPUDIV = NOCLKDIV		//CPU System Clock Postscaler
;#pragma config USBDIV = OFF		//USBのclockをOSC1/OSC2から取る
;#pragma config FCMEN  = OFF		//Fail-Safe Clock Monitorを使わない
;#pragma config IESO   = OFF		//Internal External Osc. Switchを使わない
;#pragma config HFOFST = OFF		//
;#pragma config LVP    = ON		//Low Voltage Programmingを使う
;#pragma config XINST  = OFF		//拡張命令セットとインデックスアドレスは 使 わない（ 従来 モード）選択ビット
;#pragma config BBSIZ  = OFF		//Boot Block Sizeのサイズ指定っぽい
;#pragma config CP0    = OFF		//ブロック０（000800−001ＦＦＦｈ）のコードを保護しない
;#pragma config CP1    = OFF		//ブロック１（00２０00−00３ＦＦＦｈ）のコードを保護しない
;#pragma config CPB    = OFF		//ブートブロック（0000０00−0007ＦＦｈ）のコードを保護しない
;#pragma config WRT0   = OFF		//ブロック0 (000800-001FFFh) の書込み保護しない
;#pragma config WRT1   = OFF		//ブロック1 (002000-003FFFh) の書込み保護しない
;#pragma config WRTB   = OFF		//Boot block (000000-0007FFh)の書込み保護しない
;#pragma config WRTC   = OFF		//Configuration registers (300000-3000FFh) の書込み保護しない
;#pragma config EBTR0  = OFF		//Block 0 (000800-001FFFh) を他のブロック実行時のテーブル読み取りから保護しない
;#pragma config EBTR1  = OFF		//Block 1 (002000-003FFFh) を他のブロック実行時のテーブル読み取りから保護しない
;pragma config EBTRB  = OFF		//Boot block (000000-0007FFh)を他のブロック実行時のテーブル読み取りから保護しない
;
;#pragma code
;
;ここまで

;******************************
;変数定義
;******************************






































