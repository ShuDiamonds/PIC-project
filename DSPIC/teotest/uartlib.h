/**********************************************************/
/*UART関数ライブラリ                                      */
/*2011.12.30                                              */
/**********************************************************/

//----マクロ宣言------------------------------------------------------
#define UART_115200BPS	9
#define UART_57600BPS	19
#define UART_38400BPS	30
#define UART_19200BPS	60
#define UART_9600BPS	120

#define UART_FLOW		2
#define UART_NOFLOW		0

//----関数プロトタイプ宣言--------------------------------------------
void UART1_Init(int baud, int flow);
void UART2_Init(int baud, int flow);
void UART1_RxIntEnable(int Level);
void UART2_RxIntEnable(int Level);
void UART1_RxIntDisable(void);
void UART2_RxIntDisable(void);
void UART1_ErrDetect(void);
void UART2_ErrDetect(void);


