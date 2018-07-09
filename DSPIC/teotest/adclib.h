/**********************************************************/
/*アナログ-デジタル変換関数ライブラリ                     */
/*2011.12.30                                              */
/**********************************************************/

//----マクロ宣言------------------------------------------------------
//アナログ/デジタルポートコンフィグレーション
#define ADC_AN0		0xFFFE		//1111 1111 1111 1110
#define ADC_AN1		0xFFFD		//1111 1111 1111 1101
#define ADC_AN2		0xFFFB		//1111 1111 1111 1011
#define ADC_AN3		0xFFF7		//1111 1111 1111 0111
#define ADC_AN4		0xFFEF		//1111 1111 1110 1111
#define ADC_AN5		0xFFDF		//1111 1111 1101 1111
#define ADC_AN6		0xFFBF		//1111 1111 1011 1111
#define ADC_AN7		0xFF7F		//1111 1111 0111 1111
#define ADC_AN8		0xFEFF		//1111 1110 1111 1111
#define ADC_AN9		0xFDFF		//1111 1101 1111 1111
#define ADC_AN10	0xFBFF		//1111 1011 1111 1111
#define ADC_AN11	0xF7FF		//1111 0111 1111 1111
#define ADC_AN12	0xEFFF		//1110 1111 1111 1111
#define ADC_AN13	0xDFFF		//1101 1111 1111 1111
#define ADC_AN14	0xBFFF		//1011 1111 1111 1111
#define ADC_AN15	0x7FFF		//0111 1111 1111 1111

//----関数プロトタイプ宣言--------------------------------------------
void ADC1_10bit_Init(int PCFG);
void ADC1_12bit_Init(int PCFG);
int	ADC1_GetData(int Ch);

