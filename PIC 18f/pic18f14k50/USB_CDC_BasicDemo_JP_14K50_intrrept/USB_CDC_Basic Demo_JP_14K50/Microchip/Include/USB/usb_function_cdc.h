/************************************************************************
  File Information:
    FileName:       usb_function_cdc.h
    Dependencies:   See INCLUDES section
    Processor:      PIC18,PIC24, PIC32 and dsPIC33 USB Microcontrollers

    Complier:      Microchip C18 (for PIC18),C30 (for PIC24 and dsPIC33)
                    and C32 (for PIC32)
    Company:        Microchip Technology, Inc.

 著作権：ソフトウエア自体は、Microchip社のSoftware License Agreement
         に準じますが、コメントなどの追加部分の著作権は、作者が
         保有し、作者の許可を得ない、全体および一部分の転載・引用を
         禁止します。

  概要:
	このファイルには、CDC機能で使用する全ての、機能、マクロ、定義、変数を
	定義しています。後半に、CDC機能で使用するマクロが説明されています。
	このファイルは、CDCを使うプロジェクトに含まれなければなりません
	さらに、usb_descriptors.cファイル、CDC機能を使用するユーザー・ファイル
	で include する必要があります。
    
    通常このファイルは "\<Install Directory\>\Microchip\Include\USB"
    directoryに保存されています。

  Description:
    USB CDC Function Driver File
    
このファイルを新しいプロジェクトで使用するときは、このファイルを
新しいプロジェクトディレクトリから参照するか、このファイル自体を
直接新しいプロジェクトフォルダにコピーする必要があります。
はあることもできます
 最初の方法を取る場合、
MPLABの「Projegt > Build Options... > Project」のDirectoriesタブから
Include Serch Pathを選択し、このファイルパスを登録する必要があります。

現在のデモ・フォルダでは、プロジェクトフォルダ内にMicrochip\Include
というフォルダを作りコピーしてあるため、Include Serch Pathには

   Microchip\Include 

と登録されています。
    
参照する方法では、ファイルの位置により、以下のような例を参考に登録
をしてください。

    C:\\Microchip Solutions\\Microchip\\Include
    
    C:\\Microchip Solutions\\My Demo Application                       

*******************************************************************
 Change History:
  Rev    Description
  ----   -----------
  2.3    Decricated the mUSBUSARTIsTxTrfReady() macro.  It is 
         replaced by the USBUSARTIsTxTrfReady() function.

  2.6    Minor definition changes

  2.6a   No Changes
  
  2.9b   Added new CDCNotificationHandler() prototype and related 
         structure definitions useful when sending serial state
         notifications over the CDC interrupt endpoint.

  i_1.0  日本語に翻訳 2012/05/01
********************************************************************/

#ifndef CDC_H
#define CDC_H

/** I N C L U D E S **********************************************************/
#include "GenericTypeDefs.h"
#include "USB/usb.h"
#include "usb_config.h"

/** D E F I N I T I O N S ****************************************************/

/* Class-Specific Requests */
#define SEND_ENCAPSULATED_COMMAND   0x00
#define GET_ENCAPSULATED_RESPONSE   0x01
#define SET_COMM_FEATURE            0x02
#define GET_COMM_FEATURE            0x03
#define CLEAR_COMM_FEATURE          0x04
#define SET_LINE_CODING             0x20
#define GET_LINE_CODING             0x21
#define SET_CONTROL_LINE_STATE      0x22
#define SEND_BREAK                  0x23

/* Notifications *
 * Note: Notifications are polled over
 * Communication Interface (Interrupt Endpoint)
 */
#define NETWORK_CONNECTION          0x00
#define RESPONSE_AVAILABLE          0x01
#define SERIAL_STATE                0x20


/* Device Class Code */
#define CDC_DEVICE                  0x02

/* Communication Interface Class Code */
#define COMM_INTF                   0x02

/* Communication Interface Class SubClass Codes */
#define ABSTRACT_CONTROL_MODEL      0x02

/* Communication Interface Class Control Protocol Codes */
#define V25TER                      0x01    // Common AT commands ("Hayes(TM)")


/* Data Interface Class Codes */
#define DATA_INTF                   0x0A

/* Data Interface Class Protocol Codes */
#define NO_PROTOCOL                 0x00    // No class specific protocol required


/* Communication Feature Selector Codes */
#define ABSTRACT_STATE              0x01
#define COUNTRY_SETTING             0x02

/* Functional Descriptors */
/* Type Values for the bDscType Field */
#define CS_INTERFACE                0x24
#define CS_ENDPOINT                 0x25

/* bDscSubType in Functional Descriptors */
#define DSC_FN_HEADER               0x00
#define DSC_FN_CALL_MGT             0x01
#define DSC_FN_ACM                  0x02    // ACM - Abstract Control Management
#define DSC_FN_DLM                  0x03    // DLM - Direct Line Managment
#define DSC_FN_TELEPHONE_RINGER     0x04
#define DSC_FN_RPT_CAPABILITIES     0x05
#define DSC_FN_UNION                0x06
#define DSC_FN_COUNTRY_SELECTION    0x07
#define DSC_FN_TEL_OP_MODES         0x08
#define DSC_FN_USB_TERMINAL         0x09
/* more.... see Table 25 in USB CDC Specification 1.1 */

/* CDC Bulk IN transfer states */
#define CDC_TX_READY                0
#define CDC_TX_BUSY                 1
#define CDC_TX_BUSY_ZLP             2       // ZLP: Zero Length Packet
#define CDC_TX_COMPLETING           3

#if defined(USB_CDC_SET_LINE_CODING_HANDLER) 
    #define LINE_CODING_TARGET &cdc_notice.SetLineCoding._byte[0]
    #define LINE_CODING_PFUNC &USB_CDC_SET_LINE_CODING_HANDLER
#else
    #define LINE_CODING_TARGET &line_coding._byte[0]
    #define LINE_CODING_PFUNC NULL
#endif

#if defined(USB_CDC_SUPPORT_HARDWARE_FLOW_CONTROL)
    #define CONFIGURE_RTS(a) UART_RTS = a;
#else
    #define CONFIGURE_RTS(a)
#endif

#if defined(USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D3)
    #error This option is not currently supported.
#else
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D3_VAL 0x00
#endif

#if defined(USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D2)
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D2_VAL 0x04
#else
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D2_VAL 0x00
#endif

#if defined(USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D1)
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D1_VAL 0x02
#else
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D1_VAL 0x00
#endif

#if defined(USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D0)
    #error This option is not currently supported.
#else
    #define USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D0_VAL 0x00
#endif

#define USB_CDC_ACM_FN_DSC_VAL  \
    USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D3_VAL |\
    USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D2_VAL |\
    USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D1_VAL |\
    USB_CDC_SUPPORT_ABSTRACT_CONTROL_MANAGEMENT_CAPABILITIES_D0_VAL

/******************************************************************************
    Function:
        void CDCSetBaudRate(DWORD baudRate)
        
    Summary:
        この macro は、get line coding request のときに、PCに
        baud rate を回答する。 (optional)

    Description:

        使用例:
        <code>
            CDCSetBaudRate(19200);
        </code>

        この機能は、optional です。なぜなら CDC devices は、USB通信の
        際に実際にはUARTのハードをコントロールしないため  

    PreCondition:
        None
        
    Parameters:
        DWORD baudRate - 所望する baudrate
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetBaudRate(baudRate) {line_coding.dwDTERate.Val=baudRate;}

/******************************************************************************
    Function:
        void CDCSetCharacterFormat(BYTE charFormat)
        
    Summary:
        この macro は、get line coding request のときに、PCに
        character format を回答する。 (optional)

    Description:
        使用例:
        <code>
            CDCSetCharacterFormat(NUM_STOP_BITS_1);
        </code>
        
        この機能は、optional です。なぜなら CDC devices は、USB通信の
        際に実際にはUARTのハードをコントロールしないため  

    PreCondition:
        None
        
    Parameters:
        BYTE charFormat - stop bitの長さ 使用可能な option は:
         * NUM_STOP_BITS_1   - 1 Stop bit
         * NUM_STOP_BITS_1_5 - 1.5 Stop bits
         * NUM_STOP_BITS_2   - 2 Stop bits
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetCharacterFormat(charFormat) {line_coding.bCharFormat=charFormat;}
#define NUM_STOP_BITS_1     0   //1 stop bit - used by CDCSetLineCoding() and CDCSetCharacterFormat()
#define NUM_STOP_BITS_1_5   1   //1.5 stop bit - used by CDCSetLineCoding() and CDCSetCharacterFormat()
#define NUM_STOP_BITS_2     2   //2 stop bit - used by CDCSetLineCoding() and CDCSetCharacterFormat()

/******************************************************************************
    Function:
        void CDCSetParity(BYTE parityType)
        
    Summary:
        この macro は、get line coding request のときに、PCに
        parity format を回答する。 (optional)

    Description:
        使用例:
        <code>
            CDCSetParity(PARITY_NONE);
        </code>
        
        この機能は、optional です。なぜなら CDC devices は、USB通信の
        際に実際にはUARTのハードをコントロールしないため  

    PreCondition:
        None
        
    Parameters:
        BYTE parityType - Type of parity.  使用可能な option は:
            * PARITY_NONE
            * PARITY_ODD
            * PARITY_EVEN
            * PARITY_MARK
            * PARITY_SPACE
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetParity(parityType) {line_coding.bParityType=parityType;}
#define PARITY_NONE     0 //no parity - used by CDCSetLineCoding() and CDCSetParity()
#define PARITY_ODD      1 //odd parity - used by CDCSetLineCoding() and CDCSetParity()
#define PARITY_EVEN     2 //even parity - used by CDCSetLineCoding() and CDCSetParity()
#define PARITY_MARK     3 //mark parity - used by CDCSetLineCoding() and CDCSetParity()
#define PARITY_SPACE    4 //space parity - used by CDCSetLineCoding() and CDCSetParity()

/******************************************************************************
    Function:
        void CDCSetDataSize(BYTE dataBits)
        
    Summary:
        この macro は、get line coding request のときに、PCに
        データビット数を回答する。 (optional)

    Description:
        使用例:

        <code>
            CDCSetDataSize(8);
        </code>
        
        この機能は、optional です。なぜなら CDC devices は、USB通信の
        際に実際にはUARTのハードをコントロールしないため  

    PreCondition:
        None
        
    Parameters:
        BYTE dataBits - number of data bits.  The options are 5, 6, 7, 8, or 16.
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetDataSize(dataBits) {line_coding.bDataBits=dataBits;}

/******************************************************************************
    Function:
        void CDCSetLineCoding(DWORD baud, BYTE format, BYTE parity, BYTE dataSize)
        
    Summary:
        この macro は、get line coding request のときに、PCに
        関連設定値を回答する。 (optional)

    Description:
        使用例:
        <code>
            CDCSetLineCoding(19200, NUM_STOP_BITS_1, PARITY_NONE, 8);
        </code>
        
        この機能は、optional です。なぜなら CDC devices は、USB通信の
        際に実際にはUARTのハードをコントロールしないため  

    PreCondition:
        None
        
    Parameters:
        DWORD baud - The desired baudrate
        BYTE format - number of stop bits.  Available options are:
         * NUM_STOP_BITS_1 - 1 Stop bit
         * NUM_STOP_BITS_1_5 - 1.5 Stop bits
         * NUM_STOP_BITS_2 - 2 Stop bits
        BYTE parity - Type of parity.  The options are the following:
            * PARITY_NONE
            * PARITY_ODD
            * PARITY_EVEN
            * PARITY_MARK
            * PARITY_SPACE
        BYTE dataSize - number of data bits.  The options are 5, 6, 7, 8, or 16.
        
    Return Values:
        None
        
    Remarks:
        None
  
 *****************************************************************************/
#define CDCSetLineCoding(baud,format,parity,dataSize) {\
            CDCSetBaudRate(baud);\
            CDCSetCharacterFormat(format);\
            CDCSetParity(parity);\
            CDCSetDataSize(dataSize);\
        }

/******************************************************************************
    Function:
        BOOL USBUSARTIsTxTrfReady(void)
        
    Summary:
        この macro は CDC bulk で、データを送ることができるか確認する

        使用例:
        <code>
            if(USBUSARTIsTxTrfReady())
            {
                putrsUSBUSART("Hello World");
            }
        </code>
        
    PreCondition:
        この戻り値は、デバイスがconfigured state のときにのみ有効である。
        (i.e. - USBDeviceGetState() returns CONFIGURED_STATE)
        
    Parameters:
        None
        
    Return Values:
        戻り値は、「１」または「０」で、CDCプログラムが新しいデータを受け取り、
        bulkのIN endpointからhost に送出できるかを示します。 戻り値の「１」
        （true）は、CDC handler が受け取り可能であり、putrsUSBUSART() や 
        putsUSBUSART()などのAPIをコールしても問題ないことを示します。
        戻り値の「０」（false）は、firmware が前のデータ処理をしており、
        この時点では、新しいデータの処理をする準備が出来ません。
        
    Remarks:
        ユーザーアプリケーションでCDCTxService() を定期的に呼び出し、
        てください、さもないと、USB IN transfers は、状況を進め、
        処理を完了させることができません。
  
 *****************************************************************************/
#define USBUSARTIsTxTrfReady()      (cdc_trf_state == CDC_TX_READY)

/******************************************************************************
    Function:
        void mUSBUSARTTxRam(BYTE *pData, BYTE len)
    
    Description:
        Depricated in MCHPFSUSB v2.3.
        この macro は、USBUSARTIsTxTrfReady()に置き換わった。
 *****************************************************************************/
#define mUSBUSARTIsTxTrfReady()     USBUSARTIsTxTrfReady()

/******************************************************************************
    Function:
        void mUSBUSARTTxRam(BYTE *pData, BYTE len)
        
    Description:
        データメモリにあるデータを処理するときにこの macro が使用される
        以下の条件でこの macro を使用する:
            1. データが null で終了しない
            2. 転送するバイト数が既知である
        注意: cdc_trf_state == CDC_TX_READY であること
        putsUSBUSART とは異なり、処理の際に送信状況をダブルチェックする
        ことはないので、cdc_trf_state != CDC_TX_READY に実行すると
        予期しない動作をする。
 
         使用例:
        <code>
            if(USBUSARTIsTxTrfReady())
            {
                mUSBUSARTTxRam(&UserDataBuffer[0], 200);
            }
        </code>
        
    PreCondition:
        cdc_trf_state は、CDC_TX_READY であること
        'len' の値は、255 byte 以下であること
        このAPIを最初に、実行するときは、USB 接続状況が、CONFIGURED_STATE
        になっていること
        
    Paramters:
        pDdata  : データの開始アドレスのPointer
        len     : 転送されるバイト数
        
    Return Values:
        None
        
    Remarks:
        この macro は、データ転送の準備を行うだけで、実際の転送は
        CDCTxService() により実行される。この macro は、データを
        "double buffer" していない。  すべてのデータが送出処理される
        までユーザーソフトでは、pData buffer の内容を変更しては
        いけない。処理の終了は、 USBUSARTIsTxTrfReady() で確認する
        ことができる
        
  
 *****************************************************************************/
#define mUSBUSARTTxRam(pData,len)   \
{                                   \
    pCDCSrc.bRam = pData;           \
    cdc_tx_len = len;               \
    cdc_mem_type = USB_EP0_RAM;     \
    cdc_trf_state = CDC_TX_BUSY;    \
}

/******************************************************************************
    Function:
        void mUSBUSARTTxRom(rom BYTE *pData, BYTE len)
        
    Description:
        プログラムメモリにあるデータを処理するときにこの macro が使用される
        以下の条件でこの macro を使用する:
            1. データが null で終了しない
            2. 転送するバイト数が既知である
        注意: cdc_trf_state == CDC_TX_READY であること
        putsUSBUSART とは異なり、処理の際に送信状況をダブルチェックする
        ことはないので、cdc_trf_state != CDC_TX_READY に実行すると
        予期しない動作をする。

          使用例:
        <code>
            if(USBUSARTIsTxTrfReady())
            {
                mUSBUSARTTxRom(&SomeRomString[0], 200);
            }
        </code>
       
    PreCondition:
        cdc_trf_state は、CDC_TX_READY であること
        'len' の値は、255 byte 以下であること
        このAPIを最初に、実行するときは、USB 接続状況が、CONFIGURED_STATE
        になっていること
        
    Paramters:
        pDdata  : データの開始アドレスのPointer
        len     : 転送されるバイト数
        
    Return Values:
        None
        
    Remarks:
        この macro は、データ転送の準備を行うだけで、実際の転送は
        CDCTxService() により実行される。
                    
 *****************************************************************************/
#define mUSBUSARTTxRom(pData,len)   \
{                                   \
    pCDCSrc.bRom = pData;           \
    cdc_tx_len = len;               \
    cdc_mem_type = USB_EP0_ROM;     \
    cdc_trf_state = CDC_TX_BUSY;    \
}

/**************************************************************************
  Function:
        void CDCInitEP(void)
    
  Summary:
    この関数は CDC function driver を初期化する。この関数は、必ず
    SET_CONFIGURATION コマンドの後に実行すること。
       (ex: within the context of the USBCBInitEP() function).
  Description:
    この関数は CDC function driver を初期化する。この関数は、line coding 
    の既定初期値 (baud rate, bit parity, ビット数,and format)設定を行う。
    同時に endpoint を有効化し ホストからの最初の転送を準備する。

    この関数は、必ず SET_CONFIGURATION コマンドの後に呼び出すこと。
    簡単には USBCBInitEP() を呼び出すことで満足される。
    
    使用例:
    <code>
        void USBCBInitEP(void)
        {
            CDCInitEP();
        }
    </code>
  Conditions:
    None
  Remarks:
    None                                                                   
  **************************************************************************/
void CDCInitEP(void);

/******************************************************************************
 	Function:
 		void USBCheckCDCRequest(void)
 
 	Description:
 		このルーティンは、受信した最新の SETUP data packet を確認し、
 		CDC class に関係する要求が無いかチェックする。
 		もし、CDC class に関係する要求があれば、その要求に対して、
 		適切な処理を実行する。

 	PreCondition:
 		この関数は、必ずホストから control transfer の SETUP packet 
 		が到着してから、呼び出すこと。

	Parameters:
		None
		
	Return Values:
		None
		
	Remarks:
 		SETUP packet に CDC class に関係する要求が無い場合、この関数は、
 		ステイタスの更新や処理は一切行わない。
  *****************************************************************************/
void USBCheckCDCRequest(void);


/**************************************************************************
  Function: void CDCNotificationHandler(void)
  Description: 
      DSR status をチェックし USB hostにレポートする。
  Conditions: 
      最初に CDCNotificationHandler() を呼び出す前に、CDCInitEP() が
      一度は呼び出され実行されていること。
  Remarks:
    この関数は、USB_CDC_SUPPORT_DSR_REPORTING オプションが有効となっている
    場合にのみ、必要とされ実行される。有効の場合、この関数は定期的に呼び出
    され DSRピンの状態がサンプルされ、情報が USB host に送信される。
    このために、CDCNotificationHandler() 自体を呼び出すか、初めに 
    CDCNotificationHandler() を呼び出した後は、 CDCTxService() を呼び出す。
  **************************************************************************/
void CDCNotificationHandler(void);


/**********************************************************************************
  Function:
    BOOL USBCDCEventHandler(USB_EVENT event, void *pdata, WORD size)
    
  Summary:
    USB stack からの CDC endpoint に係る event を処理する。 

  Description:
    USB stack からの event を処理する。この関数は、CDC driverにより
    処理されなければならない USB event が発生した時に、この関数が
    呼び出される必要がある。
    
  Conditions:
    引数の'len'は CDC class として、 USB host から受信される bulk data 
    用の endpoint size 以下でなければならない。
    引数の'buffer'は 'len'により指定される長さ以上の大きさを持つバッファ
    エリアのアドレスを point しなければならない。

  Input:
    event - 発生した event のタイプ
    pdata - pointer to the data that caused the event
    size - the size of the data that is pointed to by pdata
                                                                                   
  **********************************************************************************/
BOOL USBCDCEventHandler(USB_EVENT event, void *pdata, WORD size);


/**********************************************************************************
  Function:
        BYTE getsUSBUSART(char *buffer, BYTE len)
    
  Summary:
    getsUSBUSART は、USB CDC Bulk OUTのendpointから受信された文字列を
    ユーザーの指示する場所にコピーをする。
    この機能はデータを受信するまで待機はせずに、受信データが無い時には、
    戻り値を'0'として、受信データが無いことをフィードバックする。
   
    使用例:
    <code>
        BYTE numBytes;
        BYTE buffer[64]
    
        numBytes = getsUSBUSART(buffer,sizeof(buffer));
        if(numBytes \> 0)
        {
            // numBytes の受信データが"buffer"にコピーされたので
            //  ここでユーザーの必要な処理を行う
        }
    </code>
  Conditions:
    引数の'len'は CDC class として、 USB host から受信される bulk data 
    用の endpoint size 以下でなければならない。
    引数の'buffer'は 'len'により指定される長さ以上の大きさを持つバッファ
    エリアのアドレスを point しなければならない。

  Input:
    buffer -  受信されたデータの保存場所
    len -     予定される受信バイト数
  Output:
    BYTE -    戻り値は、実際に受信され指定の場所にコピーされたバイト数で 
              あり、この値は、0 から引数の'len'までの範囲にある。
              0 は、CDC bulk OUT endpoint に新たにデータが無いことを意味する
              indicates that no new CDC bulk OUT endpoint data was available.
  **********************************************************************************/
BYTE getsUSBUSART(char *buffer, BYTE len);

/******************************************************************************
  Function:
	void putUSBUSART(char *data, BYTE length)
		
  Summary:
    putUSBUSART は、一群のデータをUSBに書き込む。この機能を使用することで
    0x00 を送出することができる。(これは一般的に NULL 文字と言われる)
    
    使用例:
    <code>
        if(USBUSARTIsTxTrfReady())
        {
            char data[] = {0x00, 0x01, 0x02, 0x03, 0x04};
            putUSBUSART(data,5);
        }
    </code>
    device-to-host(put)の仕組みは、host-to-device(get)より、融通性に
    富んでおり、bulk IN endpoint のサイズより長い文字データを処理する
    こともできる。データが長い場合には、何回かに分けて転送を行い処理
    をする。これらの転送を継続的に実施するために CDCTxService() は定期的
    に呼び出される必要がある。

  Conditions:
    USBUSARTIsTxTrfReady() の結果が TRUE の必要がある。これは、最後の
    転送が完了し、新たにデータの受け入れ準備が整ったことを示している。
     'data' によりポイントされる文字列は、255バイト以下であること

  Input:
    char *data - host に送信される RAM エリアにあるデータのポインタ
    BYTE length - 送信されるバイト数 (255以下).
		
 *****************************************************************************/
void putUSBUSART(char *data, BYTE Length);

/******************************************************************************
	Function:
		void putsUSBUSART(char *data)
		
  Summary:
    putsUSBUSART は、null文字を含めた文字列を USB に送信する
    'puts'機能は、RAM エリアの文字列をに使用する

    使用例:
    <code>
        if(USBUSARTIsTxTrfReady())
        {
            char data[] = "Hello World";
            putsUSBUSART(data);
        }
    </code>
    
    device-to-host(put)の仕組みは、host-to-device(get)より、融通性に
    富んでおり、bulk IN endpoint のサイズより長い文字データを処理する
    こともできる。データが長い場合には、何回かに分けて転送を行い処理
    をする。これらの転送を継続的に実施するために CDCTxService() は定期的
    に呼び出される必要がある。

  Conditions:
    USBUSARTIsTxTrfReady() の結果が TRUE の必要がある。これは、最後の
    転送が完了し、新たにデータの受け入れ準備が整ったことを示している。
     'data' によりポイントされる文字列は、255バイト以下であること

  Input:
    char *data -  null文字により終わる文字列。もし、null文字が見つからない
                  場合には、255 バイトのデータが host に送信される
		
 *****************************************************************************/
void putsUSBUSART(char *data);


/**************************************************************************
  Function:
        void putrsUSBUSART(const ROM char *data)
    
    putsUSBUSART は、null文字を含めた文字列を USB に送信する
    'putrs'機能は、プログラムメモリ エリアの文字列をに使用する

    使用例:
    <code>
        if(USBUSARTIsTxTrfReady())
        {
            putrsUSBUSART("Hello World");
        }
    </code>
    
    device-to-host(put)の仕組みは、host-to-device(get)より、融通性に
    富んでおり、bulk IN endpoint のサイズより長い文字データを処理する
    こともできる。データが長い場合には、何回かに分けて転送を行い処理
    をする。これらの転送を継続的に実施するために CDCTxService() は定期的
    に呼び出される必要がある。

  Conditions:
    USBUSARTIsTxTrfReady() の結果が TRUE の必要がある。これは、最後の
    転送が完了し、新たにデータの受け入れ準備が整ったことを示している。
     'data' によりポイントされる文字列は、255バイト以下であること

  Input:
    const ROM char *data -  null文字により終わる文字列。もし、null文字が見つからない
                            場合には、255 バイトのデータが host に送信される
      
  **************************************************************************/
void putrsUSBUSART(const ROM char *data);

/************************************************************************
  Function:
        void CDCTxService(void)
    
  Summary:
    CDCTxService は、device-to-host の送信を処理する。この機能は
    configured state 以降は、メインプログラム・ループで毎回呼び出すこと

  Description:
    CDCTxService は、device-to-host の送信を処理する。この機能は
    configured state (CDCIniEP() 機能が実行された)以降は、メイン
    プログラムループで毎回呼び出すこと
    host に対しCDCシリアルデータを複数回送信を行う処理で、内部での
    ステート管理に必要な関数である。 CDCTxService() を定期的に実行できない
    場合、USB host への送信が行えない。
    
    Typical Usage:
    <code>
    void main(void)
    {
        USBDeviceInit();
        while(1)
        {
            USBDeviceTasks();
            if((USBGetDeviceState() \< CONFIGURED_STATE) ||
               (USBIsDeviceSuspended() == TRUE))
            {
                //device configured でないか、suspended しているので
                //  いかなるユーザーアプリも実行しない
                continue;   // ループの初めに戻る
            }
            else
            {
                //継続的にデータを PC に送信するために必要
                CDCTxService();
    
                //ここにユーザーアプリ
                UserApplication();
            }
        }
    }
    </code>

  Conditions:
    CDCIniEP() 関数が既に実行されており、device が、CONFIGURED_STATE
    にあること
  Remarks:
    None                                                                 
  ************************************************************************/
void CDCTxService(void);


/** S T R U C T U R E S ******************************************************/

/* Line Coding Structure */
#define LINE_CODING_LENGTH          0x07

typedef union _LINE_CODING
{
    struct
    {
        BYTE _byte[LINE_CODING_LENGTH];
    };
    struct
    {
        DWORD_VAL   dwDTERate;          // Complex data structure
        BYTE    bCharFormat;
        BYTE    bParityType;
        BYTE    bDataBits;
    };
} LINE_CODING;

typedef union _CONTROL_SIGNAL_BITMAP
{
    BYTE _byte;
    struct
    {
        unsigned DTE_PRESENT:1;       // [0] Not Present  [1] Present
        unsigned CARRIER_CONTROL:1;   // [0] Deactivate   [1] Activate
    };
} CONTROL_SIGNAL_BITMAP;


/* Functional Descriptor Structure - See CDC Specification 1.1 for details */

/* Header Functional Descriptor */
typedef struct __attribute__((packed)) _USB_CDC_HEADER_FN_DSC
{
    BYTE bFNLength;
    BYTE bDscType;
    BYTE bDscSubType;
    WORD bcdCDC;
} USB_CDC_HEADER_FN_DSC;

/* Abstract Control Management Functional Descriptor */
typedef struct __attribute__((packed)) _USB_CDC_ACM_FN_DSC
{
    BYTE bFNLength;
    BYTE bDscType;
    BYTE bDscSubType;
    BYTE bmCapabilities;
} USB_CDC_ACM_FN_DSC;

/* Union Functional Descriptor */
typedef struct __attribute__((packed)) _USB_CDC_UNION_FN_DSC
{
    BYTE bFNLength;
    BYTE bDscType;
    BYTE bDscSubType;
    BYTE bMasterIntf;
    BYTE bSaveIntf0;
} USB_CDC_UNION_FN_DSC;

/* Call Management Functional Descriptor */
typedef struct __attribute__((packed)) _USB_CDC_CALL_MGT_FN_DSC
{
    BYTE bFNLength;
    BYTE bDscType;
    BYTE bDscSubType;
    BYTE bmCapabilities;
    BYTE bDataInterface;
} USB_CDC_CALL_MGT_FN_DSC;

typedef union __attribute__((packed)) _CDC_NOTICE
{
    LINE_CODING GetLineCoding;
    LINE_CODING SetLineCoding;
    unsigned char packet[CDC_COMM_IN_EP_SIZE];
} CDC_NOTICE, *PCDC_NOTICE;

/* Bit structure definition for the SerialState notification byte */
typedef union
{
    BYTE byte;
    struct
    {
        BYTE    DCD             :1;
        BYTE    DSR             :1;
        BYTE    BreakState      :1;
        BYTE    RingDetect      :1;
        BYTE    FramingError    :1;
        BYTE    ParityError     :1;
        BYTE    Overrun         :1;
        BYTE    Reserved        :1;          
    }bits;    
}BM_SERIAL_STATE;  

/* Serial State Notification Packet Structure */
typedef struct
{
    BYTE    bmRequestType;  //Always 0xA1 for serial state notification packets
    BYTE    bNotification;  //Always SERIAL_STATE (0x20) for serial state notification packets
    UINT16  wValue;     //Always 0 for serial state notification packets
    UINT16  wIndex;     //Interface number
    UINT16  wLength;    //Should always be 2 for serial state notification packets
    BM_SERIAL_STATE SerialState;
    BYTE    Reserved;
}SERIAL_STATE_NOTIFICATION;   


/** E X T E R N S ************************************************************/
extern BYTE cdc_rx_len;
extern USB_HANDLE lastTransmission;

extern BYTE cdc_trf_state;
extern POINTER pCDCSrc;
extern BYTE cdc_tx_len;
extern BYTE cdc_mem_type;

extern volatile FAR CDC_NOTICE cdc_notice;
extern LINE_CODING line_coding;

extern volatile CTRL_TRF_SETUP SetupPkt;
extern ROM BYTE configDescriptor1[];

/** Public Prototypes *************************************************/
//------------------------------------------------------------------------------
//This is the list of public API functions provided by usb_function_cdc.c.
//This list is commented out, since the actual prototypes are declared above
//with associated inline documentation.
//------------------------------------------------------------------------------
//void USBCheckCDCRequest(void);
//void CDCInitEP(void);
//BOOL USBCDCEventHandler(USB_EVENT event, void *pdata, WORD size);
//BYTE getsUSBUSART(char *buffer, BYTE len);
//void putUSBUSART(char *data, BYTE Length);
//void putsUSBUSART(char *data);
//void putrsUSBUSART(const ROM char *data);
//void CDCTxService(void);
//void CDCNotificationHandler(void);
//------------------------------------------------------------------------------



#endif //CDC_H
