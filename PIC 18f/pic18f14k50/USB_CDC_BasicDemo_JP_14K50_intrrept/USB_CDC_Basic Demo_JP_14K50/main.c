/** INCLUDES *******************************************************/
#include "usb/usb.h"
#include "usb/usb_function_cdc.h"
#include "GenericTypeDefs.h"
#include "Compiler.h"
#include "usb_config.h"
#include "usb/usb_device.h"

#include "HardwareProfile.h"

/** CONFIGURATION **************************************************/
#if defined(LOW_PIN_COUNT_USB_DEVELOPMENT_KIT)
        //14K50
        #pragma config CPUDIV = NOCLKDIV
        #pragma config USBDIV = OFF
        #pragma config FOSC   = HS
        #pragma config PLLEN  = ON
        #pragma config FCMEN  = OFF
        #pragma config IESO   = OFF
        #pragma config PWRTEN = OFF
        #pragma config BOREN  = OFF
        #pragma config BORV   = 30
//        #pragma config VREGEN = ON
        #pragma config WDTEN  = OFF
        #pragma config WDTPS  = 32768
        #pragma config MCLRE  = OFF
        #pragma config HFOFST = OFF
        #pragma config STVREN = ON
        #pragma config LVP    = OFF
        #pragma config XINST  = OFF
        #pragma config BBSIZ  = OFF
        #pragma config CP0    = OFF
        #pragma config CP1    = OFF
        #pragma config CPB    = OFF
        #pragma config WRT0   = OFF
        #pragma config WRT1   = OFF
        #pragma config WRTB   = OFF
        #pragma config WRTC   = OFF
        #pragma config EBTR0  = OFF
        #pragma config EBTR1  = OFF
        #pragma config EBTRB  = OFF       

#else
    #error No hardware board defined, see "HardwareProfile.h" and __FILE__
#endif

/** V A R I A B L E S *****************************************************/
#pragma udata
char USB_In_Buffer[64];
char USB_Out_Buffer[64];

BOOL stringPrinted;
volatile BOOL buttonPressed;
volatile BYTE buttonCount;

/** P R I V A T E  P R O T O T Y P E S ************************************/
static void InitializeSystem(void);
void ProcessIO(void);
void USBDeviceTasks(void);
void YourHighPriorityISRCode();
void YourLowPriorityISRCode();
void USBCBSendResume(void);
void BlinkUSBStatus(void);
void UserInit(void);

/** VECTOR REMAPPING ***********************************************/

    //PIC18装置では、0x00、0x08と0x18が、リセット、高優先度割込と低優先度
    //割込のベクトルに使われます。
    //しかし、このMicrochip USBブートローダーは、0x00-0xFFFまたは
    //0x00-0x7FF 番地を使用しているためブートローダー・コードは、これらの
    //ベクトルを以下の新しい場所に再配置（リマップ）します。
    //再配置は、このプロジェクトでUSBブートローダーを利用するHEXファイルを
    //プログラムする場合に必要です。
    //もし、ブートローダーを利用しないならるusb_config.hファイルを編集の
    //以下の定義をコメントアウトしてください。
    //#define PROGRAMMABLE_WITH_USB_HID_BOOTLOADER
    //#define PROGRAMMABLE_WITH_USB_LEGACY_CUSTOM_CLASS_BOOTLOADER
    
        #define REMAPPED_RESET_VECTOR_ADDRESS            0x00
        #define REMAPPED_HIGH_INTERRUPT_VECTOR_ADDRESS   0x08
        #define REMAPPED_LOW_INTERRUPT_VECTOR_ADDRESS    0x18
        
    #pragma code REMAPPED_HIGH_INTERRUPT_VECTOR = REMAPPED_HIGH_INTERRUPT_VECTOR_ADDRESS
    void Remapped_High_ISR (void)
    {
         _asm goto YourHighPriorityISRCode _endasm
    }
    #pragma code REMAPPED_LOW_INTERRUPT_VECTOR = REMAPPED_LOW_INTERRUPT_VECTOR_ADDRESS
    void Remapped_Low_ISR (void)
    {
         _asm goto YourLowPriorityISRCode _endasm
    }
    
    #pragma code
        
    // ここから　ユーザーの実際に実行される　インタラプト　プログラム
    #pragma interrupt YourHighPriorityISRCode
    void YourHighPriorityISRCode()
    {
        // 何から インタラプトが発生したかインタラプト フラグをチェックし 
        // インタラプト処理ルーティンを記述し
        // インタラプト フラグをクリアする
        // など
        #if defined(USB_INTERRUPT)
            USBDeviceTasks();
        #endif
    
    }    //This return will be a "retfie fast", since this is in a #pragma interrupt section 
    #pragma interruptlow YourLowPriorityISRCode
    void YourLowPriorityISRCode()
    {
        // 何から インタラプトが発生したかインタラプト フラグをチェックし 
        // インタラプト処理ルーティンを記述し
        // インタラプト フラグをクリアする
        // など
    
    }    //この復帰は"retfie"命令, #pragma interruptlow section なので

/** DECLARATIONS ***************************************************/
#pragma code

/************************************************************************
 * Function:        void main(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        Main program entry point.
 * Note:            None
 ************************************************************************/

void main(void){   
    InitializeSystem();

    while(1){
        #if defined(USB_INTERRUPT)
            if(USB_BUS_SENSE && (USBGetDeviceState() == DETACHED_STATE)){
                USBDeviceAttach();
            }
        #endif

        #if defined(USB_POLLING)
        // Check bus status and service USB interrupts.
        USBDeviceTasks(); // Interrupt と polling方法。 
              // polling方法を採用するときは必ず定期的に
              // この関数を呼び出すこと。この関数はSETUP transactions
              // （例えば、最初にプラグインした時のenumeration process）
              // での処理と返答を行います。
              // ＵＳＢホストは、ＵＳＢデバイスがプロセスSETUPパケットを
              // 遅滞なく受信することを要求しています。したがって、
              // pollingを使う場合、ホストからＵＳＢデバイスに向けSETUP
              // パケットの送信が予想される時間帯はこの関数を頻繁（100μ
              // 秒毎）に呼び出す必要があります。 ほとんどの場合、
              // USBDeviceTasks（）関数は処理に時間を必要とするわけでは
              // ありません（50命令サイクル）。
              // Interrupt方式かpolling方法かはusb_config.hで定義されます。
        #endif
                      
        // 本来の目的とするアプリケーションタスク アプリケーションに関連
        // するタスクはここか、ProcessIO() 関数内に記載します。

        ProcessIO();        

    }//end while
}//end main

/********************************************************************
 * Function:        static void InitializeSystem(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        初期化はこのInitializeSystem で行われる
 *                  必要とするすべての USB initialization routines
 *                  はここから呼び出される。
 *
 *                  ユーザーの使用する初期化ルーティンも
 *                  ここから呼び出すこと。      
 *
 * Note:            None
 *******************************************************************/
static void InitializeSystem(void){

        ADCON1 |= 0x0F;                 // Default all pins to digital

// USB仕様によりＵＳＢ周辺装置はVbusピンに電源を供給してはならないことが
// 義務づけられています。 さらに、周辺機器はホスト/ハブがVbus線に電圧を
// 供給していないときにD+またはD-ピンに電圧を供給してはなりません。
// （バス電源を利用しない）自己電源を有するＵＳＢ周辺機器を設計する場合、
// 装置のファームウェアで、Vbusに電力が供給されない限り、ＵＳＢモジュール
// とD+およびD-ピンのプルアップ抵抗をONにしないようプログラムします。 
// したがってVbusがホストから駆動されているかを検出する方法を、ファーム
// ウェアが準備する必要があります。
// Vbusを（抵抗で分圧するなどし）5V入力に対応できるI/Oピンに接続すること
// により、Vbusに電圧が印加されている（ホストが電力を供給している）か、
// 印加されていない（ホストはシャットダウンか、電力を供給していない）か
// を検出します。
// USBファームウェアで定期的にこのI/Oピンを確認（Polling)することで、USB 
// module/D+/D-を有効にしてい良いか知ることができます。バス電源だけで動作
// する設計の周辺装置の場合ホストが、Vbusに電源を供給していないときにD+
// またはD-に電圧を供給することは不可能なため、このバス監視機能は必要では
// ありません。
// このファームウェアは、    HardwareProfile.hファイルの中で「USE_USB_BUS_
// SENSE_IO」を定義することでバス監視機能が組み込まれます。
    #if defined(USE_USB_BUS_SENSE_IO)
    tris_usb_bus_sense = INPUT_PIN; // See HardwareProfile.h
    #endif
    
// ホストPCがGetStatus(device)要求を送信した場合、ファームウェアは応答し、
// ホストにＵＳＢ周辺装置が現在、バス電源で動作しているか、自己電源で
// 動作しているかを知らせなければなりません。詳細は公式USB仕様書で第9章
// を確認してください。
// 周辺装置がどちらの方法でも動作する場合、現在は、どの電源で動作してい
// るかを調べその結果により通知する必要があります。 
// PICDEM FS USB Demo Boardと同様なハードウェア構成ならば、「RA2」ピンが
// 現在の電源供給元を示しています。この機能を使用するためには、
// HardwareProfile.hに「USE_SELF_POWER_SENSE_IO」が定義され、I/Oピンが適切
// に記述されていることが必要です。

    #if defined(USE_SELF_POWER_SENSE_IO)
    tris_self_power = INPUT_PIN;    // HardwareProfile.h 参照
    #endif
    
    UserInit();

    USBDeviceInit();    //usb_device.c. で  USB module SFRs や　プログラム
                        //変数を初期化する
}//end InitializeSystem

/*************************************************************************
 * Function:        void UserInit(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        ここに、demoプログラムで使用するデバイスの初期化
 *                  コードを記載します
 * Note:            
 ************************************************************************/
void UserInit(void){
    //チャタリング変数の初期化
    buttonCount = 0;
    buttonPressed = FALSE;
    stringPrinted = TRUE;

    //LED pinの初期化
    mInitAllLEDs();

    //pushbuttonsの初期化
    mInitAllSwitches();
}//end UserInit

/********************************************************************
 * Function:        void ProcessIO(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        この関数に、ユーザー・ルーチンを記述します。 
 *                  ここにはUSB関連および、USBに関連しない両方のタスクが
 *                  記載されています。
 * Note:            None
 *******************************************************************/
void ProcessIO(void){   
    BYTE numBytesRead;

    // USB device statusを反映してLEDを点滅させます
    BlinkUSBStatus();
    // User Application USB tasks
    if((USBDeviceState < CONFIGURED_STATE)||(USBSuspendControl==1)) return;
    // ここからユーザー・アプリを記載します
    // PushSwが押された時の処理

    if(buttonPressed){
        if(stringPrinted == FALSE){
            if(mUSBUSARTIsTxTrfReady()){
                putrsUSBUSART("Button Pressed -- \r\n");
                stringPrinted = TRUE;
            }
        }
    }else{
        stringPrinted = FALSE;
    }

    // 文字コードを受信したときの処理
    if(USBUSARTIsTxTrfReady()){
        numBytesRead = getsUSBUSART(USB_Out_Buffer,64);
        if(numBytesRead != 0){
            BYTE i;
            
            for(i=0;i<numBytesRead;i++){
                switch(USB_Out_Buffer[i]){
                    case 0x0A:
                    case 0x0D:
                        USB_In_Buffer[i] = USB_Out_Buffer[i];
                        break;
                    default:
                        USB_In_Buffer[i] = USB_Out_Buffer[i] + 1;
                        break;
                }
            }
            putUSBUSART(USB_In_Buffer,numBytesRead);
        }
    }

    CDCTxService();
}        //end ProcessIO

/********************************************************************
 * Function:        void BlinkUSBStatus(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        USB device statusを反映してLEDを点滅させます 
 *
 * Note:            mLED macros は HardwareProfile.hに記載されています
 *                  USBDeviceState は、usb_device.hで定義され
 *                  その状態のUpDateが行われます
 *******************************************************************/
void BlinkUSBStatus(void){
    static WORD led_count=0;
    
    if(led_count == 0)led_count = 10000U;
    led_count--;

    #define mLED_Both_Off()     {mLED_1_Off();mLED_2_Off();}
    #define mLED_Both_On()      {mLED_1_On();mLED_2_On();}
    #define mLED_Only_1_On()    {mLED_1_On();mLED_2_Off();}
    #define mLED_Only_2_On()    {mLED_1_Off();mLED_2_On();}

    if(USBSuspendControl == 1){
        if(led_count==0){
            mLED_1_Toggle();
            if(mGetLED_1()){
                mLED_2_On();
            }else{
                mLED_2_Off();
            }
        }//end if
    }else{
        if(USBDeviceState == DETACHED_STATE){
            mLED_Both_Off();
        }else if(USBDeviceState == ATTACHED_STATE){
            mLED_Both_On();
        }else if(USBDeviceState == POWERED_STATE){
            mLED_Only_1_On();
        }else if(USBDeviceState == DEFAULT_STATE){
            mLED_Only_2_On();
        }else if(USBDeviceState == ADDRESS_STATE){
            if(led_count == 0){
                mLED_1_Toggle();
                mLED_2_Off();
            }//end if
        }else if(USBDeviceState == CONFIGURED_STATE){
            if(led_count==0){
                mLED_1_Toggle();
                if(mGetLED_1()){
                    mLED_2_Off();
                }else{
                    mLED_2_On();
                }
            }//end if
        }//end if(...)
    }//end if(UCONbits.SUSPND...)
}//end BlinkUSBStatus

// ******************************************************************
// ************** USB Callback Functions ****************************
// ******************************************************************
// USBファームウェア・スタックは、特定のUSBイベントに応じてコールバック
// 関数USBCBxxx（）を呼び出します。
// たとえば、ホストPCが電源をOFFにするときは、USB装置へのStart of Frame
//  （SOF）の送信を止めます。 これに応じて、すべてのＵＳＢ装置は、USB 
// Vbusからの消費電流をそれぞれ2.5mA 未満に減少させます。
// ＵＳＢモジュールは、この状態（USB仕様によれば、3ms以上の何の通信/SOF
// パケットも行われていない状態）を検出すると、USBCBSuspend（）を呼びだ
// します。ユーザーは、これらの状況に適切な措置がとられるようコールバック
// 関数を修正しなければなりません。
// たとえば、USBCBSuspend（）では、（クロックを変更したり、LEDをオフにし
// たり、CPUをスリープさせたりして）消費電流を減少させるコードを実行しVbus
// からの消費電流を2.5mA未満にします。
// その後、USBCBWakeFromSuspend（）関数では、USBCBSuspend（）関数で実施
// した省電力を元に戻すコードを実行することになります。
// USBスタックがこれを自動的に呼ばないという点で、USBCBSendResume（）関数
// は特別です。 この関数は、アプリケーション・ファームウェアから呼ばれる
// 関数です。詳しくは別のコメントを参照してください。
/**************************************************************************
 * Function:        void USBCBSuspend(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        Call back that is invoked when a USB suspend is detected
 *
 * Note:            None
 **************************************************************************/
void USBCBSuspend(void){
// Example power saving code 
// アプリケーションに望ましい適切な省電力コードをここに記載する。
// もし マイクロコントローラをスリープさせるなら、下記に示す手順に
// 類似したプロセスが使用される：
/*
	ConfigureIOPinsForLowPower();
	SaveStateOfAllInterruptEnableBits();
	DisableAllInterruptEnableBits();
	EnableOnlyTheInterruptsWhichWillBeUsedToWakeTheMicro(); 
//    再起動（wake）のため少なくともUSBActivityIFは稼働させる
 Sleep();
	RestoreStateOfAllPreviouslySavedInterruptEnableBits(); 
//    出来れば、USBCBWakeFromSuspend（）関数中にこの代替機能を盛込む。
	RestoreIOPinsToNormal(); 
//    出来れば、USBCBWakeFromSuspend（）関数中にこの代替機能を盛込む。
// 
// 重要事項： ここでUSBActivityIF（ACTVIF）ビットをクリアしてはならない。 
// このビットはusb_device.cファイルでクリアされる。ここでUSBActivityIFを
// クリアすると意図する動作を行えなくなる。    
// 例 省電力化コード。 必要により適切なコードをここに挿入
    */
}

/****************************************************************************
 * Function:        void USBCBWakeFromSuspend(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        ホストは、（3+msのアイドルを「送る」ことで）ＵＳＢ周辺装置
 *                    低電力Suspendモードにすることがある。 Suspendモードの時に、
 *                    をホストは、アイドル以外の信号を送るこで、装置を再び起動
 *                    （Wake)させる。
 *                    このコールバックは、USB suspend からの起動（Wakeup)が検出
 *                    されたときに実施される。
 * Note:            None
 ************************************************************************/
void USBCBWakeFromSuspend(void){
 // クロックの変更や他の省電力処置をとり、USBCBSuspend（）を実行して、
 // その後、通常のフルパワー動作モードへの移行する時　ホストは
 // 2 - 3ミリ秒のスリープ解除時間を設けています。この間に装置は通常
 // 動作に戻り、正常のUSBパケットを受けて、処理できるようになる必要
 // があります。こうするために、ＵＳＢモジュールは、適切なクロック
 // に戻る必要があります（IE：フルスピードUSBのSIEを利用するには
 // 48MHzのクロックが必要です）。
}

/********************************************************************
 * Function:        void USBCB_SOF_Handler(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        ＵＳＢホストは、full-speed装置に1msおきにSOFpacketを
 *                  送出します。 この割り込みは定期的なアイソクロナス転送
 *                  動作に 利用する場合があります。
 *                  必要に応じてコールバック・ルーチンに実装してください。
 *                  この例では、定期的に実行されることを利用し、PushSWの
 *                  チャタリング防止機能を実装しています。
 * Note:            None
 *******************************************************************/
void USBCB_SOF_Handler(void){
    // UIRbits.SOFIF を 0 にクリアする必要はありません
    // Callback caller が既にこの処理を実施しています。

    //------------------- SWのチャタリング除去 ---------------------------
    //SWのチャタリングを除去し、SWの状態をbuttonPressed(bPと書きます)で
    //表します。SWを押したときbuttonPressed(bP)は「１」になります。 
    //SWの状態が変位した後、100カウントの読取り禁止時間が設けられています。 
    if(buttonPressed == sw2){  // (bP)と実際のSWの状態が異なっている
        if(buttonCount != 0){  //    チャタリングの可能性がある期間か？
            buttonCount--;     //       読取り禁止時間を進める
        }else{                 //    読取り禁止時間は終了
            // SW状態を（負論理のため）反転して読取る
            buttonPressed = !sw2;    
            buttonCount = 100; //       読取り禁止時間を初期化
        }
    }else{                     // (bP)と実際のSWの状態が一致している。
        if(buttonCount != 0){  //    チャタリングの可能性がある期間なら
            buttonCount--;     //       読取り禁止時間を進める。
        }
    }  // ----- SWのチャタリング除去 終了 -----
}

/*******************************************************************
 * Function:        void USBCBErrorHandler(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:    このコールバックの目的は、主に開発時のデバッグです。
 *              UEIRをチェックしどのエラーが、割り込みを引き起こしたか
 *              調査することができます 
 *
 * Note:            None
 *******************************************************************/
void USBCBErrorHandler(void){
  // ここでUEIRを0にクリアする必要はありません。すでにコールバック元が
  // クリアしています。
  // 一般的に、ユーザー・ファームウェアでは、特に何も処理する必要はあり
  // ません。USBエラーが発生した時。たとえば、 ホストがOUTパケットを送信
  // したが、装置へそのパケットが届かなかったとき（例：接触不良や、誤って
  // ＵＳＢケーブルのプラグを抜いたなど）、これにより、通常一つ以上のUSB
  // エラー割り込みが発生します。しかし、SIEが自動的に「NAK」パケットを
  // ホストに送信している限り、特定に何もする必要はありません。
  // 通常ホストはパケットを再送するためにデータ損失は起こりません。
  // システムは、一般的にアプリケーション・ファームウェアの処理を必要と
  // せず、自動的に復旧します。つまり、このコールバック機能はデバッグの
  // 目的のために提供されています。 
}

/*******************************************************************
 * Function:        void USBCBCheckOtherReq(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        SETUPパケットがホストから到着したとき、
 *                  ファームウェアによっては、要求を満たすために
 *                  適切に、データを送信したり、応答しなければなりません。
 *                  SETUPパケットの一部は、standardUSB "chapter 9"
 *                  （公式USB仕様書の第9章を満たすため）に従いますが、
 *                  USBデバイスクラス特有のものもあります。 たとえば、HID
 *                  装置は「GET REPORT」に応答する必要があります。
 *                   これは仕様書の第9章の要求でないため、usb_device.cでは
 *                  取り扱われません。その代わりに、特定のクラスを取り扱う
 *                  ファームウェアたとえば usb_function_hid.c.に含まれます
 *
 * Note:            None
 *******************************************************************/
void USBCBCheckOtherReq(void){
    USBCheckCDCRequest();
}//end

/*******************************************************************
 * Function:        void USBCBStdSetDscHandler(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        USBCBStdSetDscHandler（）コールバック機能は、SETUP,
 *                    bRequest：SET_DESCRIPTOR要求の受信時に呼ばれます。
 *                    一般的に、SET_DESCRIPTOR要求は大多数のアプリケー
 *                    ションで使われません。
 *                    この種の要求をサポートすることはオプションです。
 *
 * Note:            None
 *******************************************************************/
void USBCBStdSetDscHandler(void){
    // Must claim session ownership if supporting this request
}//end

/*******************************************************************
 * Function:        void USBCBInitEP(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        装置が初期化されるとき、この機能が呼ばれます
 *                  ホストがSET_CONFIGURATION(wValue not = 0)を送ったあとに
 *                  発生する要求です。このコールバック機能は、装置の実施
 *                  する処理に必要となるエンドポイントの初期化を行います
 *
 * Note:            None
 *******************************************************************/
void USBCBInitEP(void){
    CDCInitEP();
}

/********************************************************************
 * Function:        void USBCBSendResume(void)
 * PreCondition:    None
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        TUSB仕様は、ある種のUSB周辺装置にホストPCをwake upさ
 *        せることを許可します（たとえばlow power suspendするRAMなど）。
 *        これは、いくつかのＵＳＢアプリケーションで非常に役に立つ機能です
 *        （例えば赤外線リモコン）。 ユーザーが赤外線リモコンの「電源」ボタ
 *        ンを押した時、赤外線レシーバーは、この信号を受信し、PCにUSB
 *         "command" を送信してPCを wake up出来ればそれは素晴らしいことです。
 *                    
 *        USBCBSendResume（）コールバック機能が、この特別なUSB信号を送信し
 *        PCをwakes upするのに使われます。この機能は、アプリケーション・
 *        ファームウェアからPCをwakes upするのに呼び出すことができます。 
 *        ただし、この機能は、下記の全てを満たす必要があります：
 *                    
 *                    1.  PC使われているUSBドライバーが、remote wakeup機能
 *                        をサポートする
 *                    2.  USB configuration descriptorのbmAttributes
 *                        フィールドにremote wakeup capableと記述している。
 *                    3.  ホストPCは現在スリープしていて、以前このUSB装置
 *                        からremote wakeup機能を"armed"にするSET FEATURE 
 *                        のセット アップパケットを送信している
 *
 *                  ホストがremote wakeupを armed 状態にしていない場合、
 *                  この機能はremote wakeupを起動せずリターンします。
 *                  これは、定められた動作です、 remote wakeupを armed 
 *                  状態にしなかったＵＳＢ装置はremote wakeup信号をバスの
 *                  上に送信してはいけません;もし送信すると、
 *                  USB compliance testing failureを発生させます。
 *
 *                  このコールバックは、1-15msの間、RESUME信号を送らなけ
 *                  ればなりません。
 *
 * Note:            USBバスとホストがsuspended状態、言い換えれば、remote 
 *                  wakeup ready状態にないならば、この機能は何もしないで、
 *                  すぐに戻ります。
*                   したがって、定期的にこの機能を呼んでも安全であり、
 *                  アプリケーションが任意のタイミングで呼び出しても、
 *                  バスが本当にremote wakeupを受け入れられる状態でない
 *                  限り影響を与えません。
 *
  *                  この機能を実行に合わせ、USBCBWakeFromSuspend（）の中
 *                  でクロック変更をしておく必要があるかもしれません。
 *                  なぜならUSBバスはこの機能から、リターンするとき既に、
 *                  suspendedではないため、USBモジュールはホストからの
 *                  信号を受信する準備ができているできることが必要です。
 *
 *                  このルーチンの修正可能セクションは、アプリケーション・
 *                  ニーズを満たすために変更する必要があります。 
 *                  現在の暫定設定は、代表的なクロック周波数で他の機能の
 *                  実行を3-15 msの期間ブロックします。
 *
 *                  USB 2.0仕様書のsection7.1.7.7によると、「remote wakeup 
 *                  装置は、少なくとも1 ms以上、15 ms以下の間resume信号を
 *                  保持しなければなりません」 この動作は、delay counter 
 *                  loopを使用することで実現しています。以下の一般な値を
 *                  使用することで広範囲にわたるコア周波数で動作可能です。

 *                  That value selected is 1800. See table below:
 *                  =======================================================
 *                  Core Freq(MHz)      MIP      RESUME Signal Period (ms)
 *                  =======================================================
 *                      48              12          1.05
 *                       4              1           12.6
 *                  =======================================================
 *                  * これらのタイミングはコード最適化または拡張命令モード
 *                    の使用や他の割り込みを可能にしておくとすると、異なる
 *                    場合があります。 必ずMPLAB SIMのStopwatch機能やオシ
 *                    ロスコープで実際の信号を確かめるようにしてください。 
*******************************************************************/
void USBCBSendResume(void){
    static WORD delay_count;
    
    // 第1には、ホストがremote wakeupを実行するためにarmedされていることを
    // 確かめてください。remote wakeupを可能にするには、通常ホストが
    // standby modeになる直前にSET_FEATUREを送信することによって行います、
    // メモ： configuration descriptorにremote wakeup capableと記述して
    // おり、ホストの機能がイネーブルされていれば、SET_FEATURE要求を送信
    // するだけです。
    // Windowsベースのホストの機能イネーブルは、デバイスマネージャの
    // ＵＳＢ装置プロパティ―からパワーマネジメント・タブを選び
    // "Allow this device to bring the computer out of standby."  
    // チェックボックスがチェックされていること

    if(USBGetRemoteWakeupStatus() == TRUE) {
        //Verify that the USB bus is in fact suspended, before we send
        //remote wakeup signalling.
        if(USBIsBusSuspended() == TRUE){
            USBMaskInterrupts();
            
            //Clock switch to settings consistent with normal USB operation.
            USBCBWakeFromSuspend();
            USBSuspendControl = 0; 
            USBBusIsSuspended = FALSE;  //So we don't execute this code again, 
                                        //until a new suspend condition is detected.

            // USB 2.0仕様の第7.1.7.7節は、USB装置は、remote wakeupを
            // 送信する前に、バスでアイドルを連続的に5ms+監視しなけれ
            // ばなりません。この要求を実現する一つの方法として、2ms+の
            // blocking delayを追加しています。
            //  USBIsBusSuspended() == TRUEで確認されている少なくとも3ms
            // のバスアイドルに2msを追加することでアイドルのスタートから
            // 合計5ms+となります。

            delay_count = 3600U;        
            do
    {
                delay_count--;
            }while(delay_count);
            
            //Now drive the resume K-state signalling onto the USB bus.
            USBResumeControl = 1;       // Start RESUME signaling
            delay_count = 1800U;        // Set RESUME line for 1-13 ms
            do
    {
                delay_count--;
            }while(delay_count);
            USBResumeControl = 0;     //Finished driving resume signalling

            USBUnmaskInterrupts();
        }
    }
}

/*******************************************************************
 * Function:        void USBCBEP0DataReceived(void)
 * PreCondition:    ENABLE_EP0_DATA_RECEIVED_CALLBACK must be
 *                  defined already (in usb_config.h)
 * Input:           None
 * Output:          None
 * Side Effects:    None
 * Overview:        EP0データパケットが受信された時はいつでも、この機能が
 *                  呼ばれますこれは、いろいろなクラスで利用されるcontrol
 *                  endpointを使用したデータ取得方法を例示します。
 *                  この機能は、USBCBCheckOtherReq（）と一緒に使われる必要
 *                  があります。USBCBCheckOtherReq（）機能は、データが到着
 *                  する前に最初のcontrol を受信するアプリです。

 *
 * Note:            None
 *******************************************************************/
#if defined(ENABLE_EP0_DATA_RECEIVED_CALLBACK)
void USBCBEP0DataReceived(void){
}
#endif

/*******************************************************************
 * Function:        BOOL USER_USB_CALLBACK_EVENT_HANDLER(
 *                        USB_EVENT event, void *pdata, WORD size)
 * PreCondition:    None
 * Input:           USB_EVENT event - the type of event
 *                  void *pdata - pointer to the event data
 *                  WORD size - size of the event data
 * Output:          None
 * Side Effects:    None
 * Overview:        この機能は、USBイベントが発生したことをユーザアプリケ
 *                  ーションに通知するために、USBスタックから呼ばれます。
 *                   USB_INTERRUPTオプションが選択されているとき、この
 *                  コールバックは割込み処理におかれます。
 *
 * Note:            None
 *******************************************************************/
BOOL USER_USB_CALLBACK_EVENT_HANDLER(USB_EVENT event, void *pdata, WORD size){
    switch(event){
        case EVENT_TRANSFER:
            //Add application specific callback task or callback function here if desired.
            break;
        case EVENT_SOF:
            USBCB_SOF_Handler();
            break;
        case EVENT_SUSPEND:
            USBCBSuspend();
            break;
        case EVENT_RESUME:
            USBCBWakeFromSuspend();
            break;
        case EVENT_CONFIGURED: 
            USBCBInitEP();
            break;
        case EVENT_SET_DESCRIPTOR:
            USBCBStdSetDscHandler();
            break;
        case EVENT_EP0_REQUEST:
            USBCBCheckOtherReq();
            break;
        case EVENT_BUS_ERROR:
            USBCBErrorHandler();
            break;
        case EVENT_TRANSFER_TERMINATED:
            //必要に応じて必要なアプリケーション用コールバックタスクまたは
            //コールバック機能をここに加えてください。
            //EVENT_TRANSFER_TERMINATEDは、armed （UOWN= 1）されているアプ
            //リケーション・エンドポイントに、ホストがCLEARFEATURE
            //(endpoint halt)要求を行ったときに発生します。
            //以下の機能は、ここに置くと良いでしょう：
            //1.  *pdataのハンドル値をチェックすることによって、どのエンド
            //    ポイントで、通信が終了したのかを示す。
            //2.  必要に応じてエンドポイントをRe-armする
            //     （一般的にはOUTエンドポイントの場合）
            break;
        default:
            break;
    }      
    return TRUE; 
}

/** EOF main.c *************************************************/

