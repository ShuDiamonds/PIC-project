/************************************************************
　　MPLAB-C18による入出力ポートの制御テストプログラム
　　スイッチの入力をポートDから行い、スイッチの状態に
　　従って、指定された発光ダイオードを点滅させる。
　　　　RD0のスイッチがOFFの時LED１
　　　　RD1のスイッチがOFFの時LED2
　　　　RD2のスイッチがOFFの時LED3
　　　　全スイッチがONの時全LED
*************************************************************/
#include <p18f67j60.h>    // PIC18C452のヘッダ・ファイル
#include <delays.h>
//***** コンフィギュレーションの設定
#pragma romdata configBits=0x300000
unsigned char rom configBitsArr[8] = {0xff,0xe2,0x06,0x00,0x00,0x01,0x03,0x00};
    // コンフィギュレーション・ビットの設定
    // コード・プロテクト=OFF,システム・クロック切り替え=OFF,
    // HS発振で,ウォッチドッグ・タイマ=OFF,
    // ポストスケーラ1:128,4.2Vブラウンアウト・リセット,
    //  パワーアップ・タイマ=ON,CCP2入出力はRC1ピンに割り当て,
    //スタック・オーバーフロー/アンダーフロー・リセット=ON

//***** メイン関数
void main(void)                     // メイン関数
{
    TRISA=0;                        // ポートAをすべて出力ピンにする
    TRISB=0;                        // ポートBをすべて出力ピンにする
    TRISC=0;                        // ポートCをすべて出力ピンにする
    TRISD=0x0F;                     // Upper is Output and Lower is Input

    while(1)    
    {
        if(PORTDbits.RD0)           // スイッチ１OFFか？
        {
            LATC=01;
            Delay10KTCYx(100);      // LED１を点滅
            LATC=00;
            Delay10KTCYx(100);
        }
        else if(PORTDbits.RD1)      // スイッチ２OFFか？
        {
            LATC=02;
            Delay10KTCYx(200);      // LED2を点滅
            LATC=00;
            Delay10KTCYx(200);
        }
        else if(PORTDbits.RD2)      // スイッチ３OFFか？
        {
            LATC=04;
            Delay10KTCYx(100);      // LED３を点滅
            LATC=00;
            Delay10KTCYx(100);
        }
        else                        // 全部ONか？
        {
            LATC=0xFF;              // 全LED点滅
            Delay10KTCYx(200);
            LATC=00;
            Delay10KTCYx(200);
        }
    }
}
