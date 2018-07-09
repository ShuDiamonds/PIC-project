#pragma once
#include <stdio.h>
#include "windows.h"
#include "mpusbapi.h"                   // MPUSBAPI Header File

///// グローバル変数定義
char vid_pid[]= "vid_09b9&pid_0048";    // Universal I/O
char out_pipe[]= "\\MCHP_EP1";
char in_pipe[]= "\\MCHP_EP1";
char dumy[] = "          ";

BYTE send_buf[64];						// 送信バッファ
BYTE receive_buf[64];					// 受信バッファ
DWORD RecvLength = 1;					// 受信データ数
DWORD temp;
DWORD DevID = 0;						// 選択デバイス番号

HINSTANCE libHandle;					// 実行中のハンドル番号
HANDLE myOutPipe;						// 使用中のOUTパイプのハンドル
HANDLE myInPipe;						// 使用中のIBパイプハンドル


namespace GIO
{
	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary> 
	/// Form1 の概要
	///
	/// 警告 : このクラスの名前を変更する場合、このクラスが依存するすべての .resx ファイルに関連付けられた 
	///          マネージ リソース コンパイラ ツールに対して 'Resource File Name' プロパティを
	///          変更する必要があります。この変更を行わないと、
	///          デザイナと、このフォームに関連付けられたローカライズ済みリソースとが正しく相互に利用できなくなります。
	/// </summary>
	public __gc class Form1 : public System::Windows::Forms::Form
	{	
	public:
		Form1(void)
		{
			InitializeComponent();
		}
  
	protected:
		void Dispose(Boolean disposing)
		{
			if (disposing && components)
			{
				components->Dispose();
			}
			__super::Dispose(disposing);
		}
	private: System::Windows::Forms::Button *  button1;
	private: System::Windows::Forms::TextBox *  textBox1;
	private: System::Windows::Forms::TextBox *  textBox2;
	private: System::Windows::Forms::Button *  button2;
	private: System::Windows::Forms::TextBox *  textBox3;
	private: System::Windows::Forms::Label *  label1;
	private: System::Windows::Forms::Button *  button3;
	private: System::Windows::Forms::Button *  button4;
	private: System::Windows::Forms::Button *  button5;
	private: System::Windows::Forms::Button *  button6;
	private: System::Windows::Forms::Button *  button7;
	private: System::Windows::Forms::TextBox *  textBox4;
	private: System::Windows::Forms::Button *  button8;
	private: System::Timers::Timer *  timer1;
	private: System::Windows::Forms::Label *  label2;
	private: System::Windows::Forms::GroupBox *  groupBox1;








	private: System::Windows::Forms::TextBox *  textBox13;
	private: System::Windows::Forms::TextBox *  textBox14;
	private: System::Windows::Forms::Label *  label3;
	private: System::Windows::Forms::GroupBox *  groupBox2;
	private: System::Windows::Forms::TextBox *  textBox15;
	private: System::Windows::Forms::TextBox *  textBox16;
	private: System::Windows::Forms::TextBox *  textBox17;
	private: System::Windows::Forms::TextBox *  textBox18;
	private: System::Windows::Forms::TextBox *  textBox19;
	private: System::Windows::Forms::TextBox *  textBox20;
	private: System::Windows::Forms::TextBox *  textBox21;
	private: System::Windows::Forms::TextBox *  textBox22;
	private: System::Windows::Forms::Label *  label4;
	private: System::Windows::Forms::Label *  label5;
	private: System::Windows::Forms::Label *  label6;
	private: System::Windows::Forms::Label *  label7;
	private: System::Windows::Forms::Label *  label8;
	private: System::Windows::Forms::Label *  label9;
	private: System::Windows::Forms::Label *  label10;
	private: System::Windows::Forms::Label *  label11;
	private: System::Windows::Forms::Label *  label12;








	private: System::Windows::Forms::Button *  button17;








	private: System::Windows::Forms::Label *  label14;
	private: System::Windows::Forms::Label *  label15;
	private: System::Windows::Forms::Label *  label16;
	private: System::Windows::Forms::Label *  label17;



	private: System::Windows::Forms::Button *  button13;
	private: System::Windows::Forms::Button *  LAT0;
	private: System::Windows::Forms::Button *  LAT1;
	private: System::Windows::Forms::Button *  LAT2;
	private: System::Windows::Forms::Button *  LAT3;
	private: System::Windows::Forms::TextBox *  RD0;
	private: System::Windows::Forms::TextBox *  RD1;
	private: System::Windows::Forms::TextBox *  RD2;
	private: System::Windows::Forms::TextBox *  RD3;
	private: System::Windows::Forms::TextBox *  RD4;
	private: System::Windows::Forms::TextBox *  RD5;
	private: System::Windows::Forms::TextBox *  RD6;
	private: System::Windows::Forms::TextBox *  RD7;

	private: System::Windows::Forms::Label *  label13;








	////////////////////////////////////////////////////////////
	//
	//    共通関数群
	//
	/////////////////////////////////////////////////////////////
	//
	// 送信後折り返しのデータ受信を連続して行う関数
	//
	// SendData - 送信バッファのポインタ
	// SendLength - 送信バイト数
	// ReceiveData - 受信バッファのポインタ
	// ReceiveLength - 受信バイト数
	// SendDelay - 書き込みのタイムアウト時間(msec)
	// ReceiveDelay - 受信のタイムアウト時間(msec)
	/////////////////////////////////////////////////////////////

	DWORD SendReceivePacket(BYTE *SendData, DWORD SendLength, BYTE *ReceiveData,
						DWORD *ReceiveLength, UINT SendDelay, UINT ReceiveDelay)
	{
		DWORD SentDataLength;
		DWORD ExpectedReceiveLength = *ReceiveLength;
		/// パイプがオープン中か確認してから送受信
		if(myOutPipe != INVALID_HANDLE_VALUE && myInPipe != INVALID_HANDLE_VALUE)
		{
			if(MPUSBWrite(myOutPipe,SendData,SendLength,&SentDataLength,SendDelay))
			{
				/// 続いて直ぐ受信する
				if(MPUSBRead(myInPipe,ReceiveData, ExpectedReceiveLength,
							ReceiveLength,ReceiveDelay))
				{
					/// 受信データ確認
					if(*ReceiveLength == ExpectedReceiveLength)
					{
						return 1;   // Success!
					}
					else if(*ReceiveLength < ExpectedReceiveLength)
					{
						return 2;   // Partially failed, incorrect receive length
					}
				}
				else
					CheckInvalidHandle();
			}
			else
				CheckInvalidHandle();
		}

		return 0;  // Operation Failed
	}
	/////////// エラー処理関数
	void CheckInvalidHandle(void)
	{
		if(GetLastError() == ERROR_INVALID_HANDLE)
		{
			MPUSBClose(myOutPipe);
			MPUSBClose(myInPipe);
			myOutPipe = myInPipe = INVALID_HANDLE_VALUE;
		}
		else
			textBox2->Text = "Error Code " + GetLastError();
	}

	/////////////////////////////////////////
	//// 16進数2文字からバイナリへ変換
	////////////////////////////////////////
	BYTE toCMND(BYTE *str)
	{
		BYTE data;

		data = 0;
		if((*str >= '0') && (*str <= '9'))
			data = (BYTE)(16 * (*str - '0'));
		else
		{
			if((*str >= 'A') && (*str <= 'F'))
				data = (BYTE)(16 * (*str - 'A' + 10));
			else
			{
				if((*str >= 'a') && (*str <= 'f'))
					data = (BYTE)(16 * (*str - 'a' + 10));
			}
		}
		str++;
		if((*str >= '0') && (*str <= '9'))
			data += (BYTE) (*str - '0');
		else
		{
			if((*str >= 'A') && (*str <= 'F'))
				data += (BYTE)(*str - 'A' + 10);
			else
			{
				if((*str >= 'a') && (*str <= 'f'))
					data += (BYTE)(*str - 'a' + 10);
			}
		}	
		return(data);
	}
	
	////////////////////////////////////////////
	//// パイプのオープン関数
	///////////////////////////////////////////
	int OpenPipe(void)
	{
			DWORD selection;
			DWORD state = 0;

			selection = DevID;
			//// パイプのオープン
			myOutPipe = MPUSBOpen(selection, vid_pid, out_pipe, MP_WRITE, 0);
			myInPipe = MPUSBOpen(selection, vid_pid, out_pipe, MP_READ, 0);
			if(myOutPipe == INVALID_HANDLE_VALUE || myInPipe == INVALID_HANDLE_VALUE)
			{
				textBox2->Text = "USB Failed";			// 接続異常メッセージ
				return(0);
			}
			else
			{
				textBox2->Text = "Open Pipe";			// 正常接続メッセージ
				return(1);
			}
	}

	//////////////////////////////////////////
	//// パイプのクローズ関数
	/////////////////////////////////////////
	void ClosePipe(void)
	{
		MPUSBClose(myOutPipe);
		MPUSBClose(myInPipe);
		myOutPipe = myInPipe = INVALID_HANDLE_VALUE;
	}

	/////////////////////////////////////////////////////////////////////

	private:
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		System::ComponentModel::Container * components;

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		void InitializeComponent(void)	
		{
			this->button1 = new System::Windows::Forms::Button();
			this->textBox1 = new System::Windows::Forms::TextBox();
			this->textBox2 = new System::Windows::Forms::TextBox();
			this->button2 = new System::Windows::Forms::Button();
			this->textBox3 = new System::Windows::Forms::TextBox();
			this->label1 = new System::Windows::Forms::Label();
			this->button3 = new System::Windows::Forms::Button();
			this->button4 = new System::Windows::Forms::Button();
			this->button5 = new System::Windows::Forms::Button();
			this->button6 = new System::Windows::Forms::Button();
			this->button7 = new System::Windows::Forms::Button();
			this->textBox4 = new System::Windows::Forms::TextBox();
			this->button8 = new System::Windows::Forms::Button();
			this->timer1 = new System::Timers::Timer();
			this->label2 = new System::Windows::Forms::Label();
			this->groupBox1 = new System::Windows::Forms::GroupBox();
			this->button13 = new System::Windows::Forms::Button();
			this->LAT0 = new System::Windows::Forms::Button();
			this->LAT1 = new System::Windows::Forms::Button();
			this->LAT2 = new System::Windows::Forms::Button();
			this->LAT3 = new System::Windows::Forms::Button();
			this->RD0 = new System::Windows::Forms::TextBox();
			this->RD1 = new System::Windows::Forms::TextBox();
			this->RD2 = new System::Windows::Forms::TextBox();
			this->RD3 = new System::Windows::Forms::TextBox();
			this->RD4 = new System::Windows::Forms::TextBox();
			this->RD5 = new System::Windows::Forms::TextBox();
			this->RD6 = new System::Windows::Forms::TextBox();
			this->RD7 = new System::Windows::Forms::TextBox();
			this->textBox13 = new System::Windows::Forms::TextBox();
			this->textBox14 = new System::Windows::Forms::TextBox();
			this->label3 = new System::Windows::Forms::Label();
			this->groupBox2 = new System::Windows::Forms::GroupBox();
			this->button17 = new System::Windows::Forms::Button();
			this->label11 = new System::Windows::Forms::Label();
			this->label10 = new System::Windows::Forms::Label();
			this->label9 = new System::Windows::Forms::Label();
			this->label8 = new System::Windows::Forms::Label();
			this->label7 = new System::Windows::Forms::Label();
			this->label6 = new System::Windows::Forms::Label();
			this->label5 = new System::Windows::Forms::Label();
			this->label4 = new System::Windows::Forms::Label();
			this->textBox22 = new System::Windows::Forms::TextBox();
			this->textBox21 = new System::Windows::Forms::TextBox();
			this->textBox20 = new System::Windows::Forms::TextBox();
			this->textBox18 = new System::Windows::Forms::TextBox();
			this->textBox17 = new System::Windows::Forms::TextBox();
			this->textBox16 = new System::Windows::Forms::TextBox();
			this->textBox15 = new System::Windows::Forms::TextBox();
			this->textBox19 = new System::Windows::Forms::TextBox();
			this->label12 = new System::Windows::Forms::Label();
			this->label13 = new System::Windows::Forms::Label();
			this->label14 = new System::Windows::Forms::Label();
			this->label15 = new System::Windows::Forms::Label();
			this->label16 = new System::Windows::Forms::Label();
			this->label17 = new System::Windows::Forms::Label();
			(__try_cast<System::ComponentModel::ISupportInitialize *  >(this->timer1))->BeginInit();
			this->groupBox1->SuspendLayout();
			this->groupBox2->SuspendLayout();
			this->SuspendLayout();
			// 
			// button1
			// 
			this->button1->BackColor = System::Drawing::SystemColors::Control;
			this->button1->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->button1->Location = System::Drawing::Point(16, 24);
			this->button1->Name = S"button1";
			this->button1->Size = System::Drawing::Size(88, 32);
			this->button1->TabIndex = 0;
			this->button1->Text = S"ＵＳＢ接続";
			this->button1->Click += new System::EventHandler(this, button1_Click);
			// 
			// textBox1
			// 
			this->textBox1->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox1->Location = System::Drawing::Point(120, 32);
			this->textBox1->Name = S"textBox1";
			this->textBox1->Size = System::Drawing::Size(72, 23);
			this->textBox1->TabIndex = 1;
			this->textBox1->Text = S"Check";
			// 
			// textBox2
			// 
			this->textBox2->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox2->Location = System::Drawing::Point(200, 32);
			this->textBox2->Name = S"textBox2";
			this->textBox2->Size = System::Drawing::Size(88, 23);
			this->textBox2->TabIndex = 2;
			this->textBox2->Text = S"******";
			// 
			// button2
			// 
			this->button2->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->button2->Location = System::Drawing::Point(16, 88);
			this->button2->Name = S"button2";
			this->button2->Size = System::Drawing::Size(88, 32);
			this->button2->TabIndex = 3;
			this->button2->Text = S"LED1";
			this->button2->Click += new System::EventHandler(this, button2_Click);
			// 
			// textBox3
			// 
			this->textBox3->Font = new System::Drawing::Font(S"MS UI Gothic", 15.75F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox3->Location = System::Drawing::Point(112, 264);
			this->textBox3->Name = S"textBox3";
			this->textBox3->Size = System::Drawing::Size(120, 28);
			this->textBox3->TabIndex = 4;
			this->textBox3->Text = S"*****";
			// 
			// label1
			// 
			this->label1->Font = new System::Drawing::Font(S"MS UI Gothic", 9.75F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label1->ImageAlign = System::Drawing::ContentAlignment::MiddleLeft;
			this->label1->Location = System::Drawing::Point(192, 16);
			this->label1->Name = S"label1";
			this->label1->RightToLeft = System::Windows::Forms::RightToLeft::Yes;
			this->label1->Size = System::Drawing::Size(80, 16);
			this->label1->TabIndex = 5;
			this->label1->Text = S"パイプの状態";
			this->label1->TextAlign = System::Drawing::ContentAlignment::BottomLeft;
			// 
			// button3
			// 
			this->button3->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->button3->Location = System::Drawing::Point(144, 88);
			this->button3->Name = S"button3";
			this->button3->Size = System::Drawing::Size(88, 32);
			this->button3->TabIndex = 6;
			this->button3->Text = S"LED2";
			this->button3->Click += new System::EventHandler(this, button3_Click);
			// 
			// button4
			// 
			this->button4->BackColor = System::Drawing::SystemColors::Control;
			this->button4->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->button4->Location = System::Drawing::Point(16, 152);
			this->button4->Name = S"button4";
			this->button4->Size = System::Drawing::Size(88, 32);
			this->button4->TabIndex = 7;
			this->button4->Text = S"表示出力";
			this->button4->Click += new System::EventHandler(this, button4_Click);
			// 
			// button5
			// 
			this->button5->BackColor = System::Drawing::SystemColors::Control;
			this->button5->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->button5->Location = System::Drawing::Point(144, 152);
			this->button5->Name = S"button5";
			this->button5->Size = System::Drawing::Size(88, 32);
			this->button5->TabIndex = 8;
			this->button5->Text = S"表示消去";
			this->button5->Click += new System::EventHandler(this, button5_Click);
			// 
			// button6
			// 
			this->button6->BackColor = System::Drawing::SystemColors::Control;
			this->button6->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->button6->Location = System::Drawing::Point(16, 264);
			this->button6->Name = S"button6";
			this->button6->Size = System::Drawing::Size(88, 32);
			this->button6->TabIndex = 9;
			this->button6->Text = S"計測指示";
			this->button6->Click += new System::EventHandler(this, button6_Click);
			// 
			// button7
			// 
			this->button7->BackColor = System::Drawing::SystemColors::Control;
			this->button7->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->button7->Location = System::Drawing::Point(16, 328);
			this->button7->Name = S"button7";
			this->button7->Size = System::Drawing::Size(88, 32);
			this->button7->TabIndex = 10;
			this->button7->Text = S"受信制御";
			this->button7->Click += new System::EventHandler(this, button7_Click);
			// 
			// textBox4
			// 
			this->textBox4->Font = new System::Drawing::Font(S"MS UI Gothic", 15.75F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox4->Location = System::Drawing::Point(112, 328);
			this->textBox4->Name = S"textBox4";
			this->textBox4->Size = System::Drawing::Size(160, 28);
			this->textBox4->TabIndex = 11;
			this->textBox4->Text = S"Message";
			// 
			// button8
			// 
			this->button8->BackColor = System::Drawing::SystemColors::Control;
			this->button8->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->button8->Location = System::Drawing::Point(376, 328);
			this->button8->Name = S"button8";
			this->button8->Size = System::Drawing::Size(88, 32);
			this->button8->TabIndex = 12;
			this->button8->Text = S"終了";
			this->button8->Click += new System::EventHandler(this, button8_Click);
			// 
			// timer1
			// 
			this->timer1->Enabled = true;
			this->timer1->Interval = 1000;
			this->timer1->SynchronizingObject = this;
			this->timer1->Elapsed += new System::Timers::ElapsedEventHandler(this, timer1_Elapsed);
			// 
			// label2
			// 
			this->label2->Font = new System::Drawing::Font(S"MS UI Gothic", 9.75F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label2->Location = System::Drawing::Point(112, 16);
			this->label2->Name = S"label2";
			this->label2->Size = System::Drawing::Size(80, 16);
			this->label2->TabIndex = 13;
			this->label2->Text = S"接続デバイス";
			this->label2->TextAlign = System::Drawing::ContentAlignment::BottomCenter;
			// 
			// groupBox1
			// 
			this->groupBox1->Controls->Add(this->button13);
			this->groupBox1->Controls->Add(this->LAT0);
			this->groupBox1->Controls->Add(this->LAT1);
			this->groupBox1->Controls->Add(this->LAT2);
			this->groupBox1->Controls->Add(this->LAT3);
			this->groupBox1->Controls->Add(this->RD0);
			this->groupBox1->Controls->Add(this->RD1);
			this->groupBox1->Controls->Add(this->RD2);
			this->groupBox1->Controls->Add(this->RD3);
			this->groupBox1->Controls->Add(this->RD4);
			this->groupBox1->Controls->Add(this->RD5);
			this->groupBox1->Controls->Add(this->RD6);
			this->groupBox1->Controls->Add(this->RD7);
			this->groupBox1->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->groupBox1->Location = System::Drawing::Point(328, 16);
			this->groupBox1->Name = S"groupBox1";
			this->groupBox1->Size = System::Drawing::Size(216, 128);
			this->groupBox1->TabIndex = 14;
			this->groupBox1->TabStop = false;
			this->groupBox1->Text = S"ディジタルＩ／Ｏ";
			// 
			// button13
			// 
			this->button13->BackColor = System::Drawing::SystemColors::Control;
			this->button13->Location = System::Drawing::Point(136, 48);
			this->button13->Name = S"button13";
			this->button13->Size = System::Drawing::Size(64, 32);
			this->button13->TabIndex = 12;
			this->button13->Text = S"制御";
			this->button13->Click += new System::EventHandler(this, button13_Click);
			// 
			// LAT0
			// 
			this->LAT0->Location = System::Drawing::Point(88, 88);
			this->LAT0->Name = S"LAT0";
			this->LAT0->Size = System::Drawing::Size(24, 24);
			this->LAT0->TabIndex = 11;
			this->LAT0->Text = S"０";
			this->LAT0->Click += new System::EventHandler(this, LAT0_Click);
			// 
			// LAT1
			// 
			this->LAT1->Location = System::Drawing::Point(64, 88);
			this->LAT1->Name = S"LAT1";
			this->LAT1->Size = System::Drawing::Size(24, 24);
			this->LAT1->TabIndex = 10;
			this->LAT1->Text = S"１";
			this->LAT1->Click += new System::EventHandler(this, LAT1_Click);
			// 
			// LAT2
			// 
			this->LAT2->Location = System::Drawing::Point(40, 88);
			this->LAT2->Name = S"LAT2";
			this->LAT2->Size = System::Drawing::Size(24, 24);
			this->LAT2->TabIndex = 9;
			this->LAT2->Text = S"２";
			this->LAT2->Click += new System::EventHandler(this, LAT2_Click);
			// 
			// LAT3
			// 
			this->LAT3->Location = System::Drawing::Point(16, 88);
			this->LAT3->Name = S"LAT3";
			this->LAT3->Size = System::Drawing::Size(24, 24);
			this->LAT3->TabIndex = 8;
			this->LAT3->Text = S"３";
			this->LAT3->Click += new System::EventHandler(this, LAT3_Click);
			// 
			// RD0
			// 
			this->RD0->Location = System::Drawing::Point(88, 56);
			this->RD0->Name = S"RD0";
			this->RD0->Size = System::Drawing::Size(24, 22);
			this->RD0->TabIndex = 7;
			this->RD0->Text = S"０";
			this->RD0->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// RD1
			// 
			this->RD1->Location = System::Drawing::Point(64, 56);
			this->RD1->Name = S"RD1";
			this->RD1->Size = System::Drawing::Size(24, 22);
			this->RD1->TabIndex = 6;
			this->RD1->Text = S"１";
			this->RD1->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// RD2
			// 
			this->RD2->Location = System::Drawing::Point(40, 56);
			this->RD2->Name = S"RD2";
			this->RD2->Size = System::Drawing::Size(24, 22);
			this->RD2->TabIndex = 5;
			this->RD2->Text = S"２";
			this->RD2->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// RD3
			// 
			this->RD3->Location = System::Drawing::Point(16, 56);
			this->RD3->Name = S"RD3";
			this->RD3->Size = System::Drawing::Size(24, 22);
			this->RD3->TabIndex = 4;
			this->RD3->Text = S"３";
			this->RD3->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// RD4
			// 
			this->RD4->Location = System::Drawing::Point(88, 24);
			this->RD4->Name = S"RD4";
			this->RD4->Size = System::Drawing::Size(24, 22);
			this->RD4->TabIndex = 3;
			this->RD4->Text = S"４";
			this->RD4->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// RD5
			// 
			this->RD5->Location = System::Drawing::Point(64, 24);
			this->RD5->Name = S"RD5";
			this->RD5->Size = System::Drawing::Size(24, 22);
			this->RD5->TabIndex = 2;
			this->RD5->Text = S"５";
			this->RD5->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// RD6
			// 
			this->RD6->Location = System::Drawing::Point(40, 24);
			this->RD6->Name = S"RD6";
			this->RD6->Size = System::Drawing::Size(24, 22);
			this->RD6->TabIndex = 1;
			this->RD6->Text = S"６";
			this->RD6->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// RD7
			// 
			this->RD7->Location = System::Drawing::Point(16, 24);
			this->RD7->Name = S"RD7";
			this->RD7->Size = System::Drawing::Size(24, 22);
			this->RD7->TabIndex = 0;
			this->RD7->Text = S"７";
			this->RD7->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// textBox13
			// 
			this->textBox13->Font = new System::Drawing::Font(S"MS UI Gothic", 14.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox13->Location = System::Drawing::Point(32, 208);
			this->textBox13->Name = S"textBox13";
			this->textBox13->Size = System::Drawing::Size(32, 26);
			this->textBox13->TabIndex = 15;
			this->textBox13->Text = S"C0";
			this->textBox13->TextAlign = System::Windows::Forms::HorizontalAlignment::Center;
			// 
			// textBox14
			// 
			this->textBox14->Font = new System::Drawing::Font(S"MS UI Gothic", 14.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox14->Location = System::Drawing::Point(88, 208);
			this->textBox14->Name = S"textBox14";
			this->textBox14->Size = System::Drawing::Size(200, 26);
			this->textBox14->TabIndex = 16;
			this->textBox14->Text = S"Out Message 01234567";
			// 
			// label3
			// 
			this->label3->Location = System::Drawing::Point(240, 264);
			this->label3->Name = S"label3";
			this->label3->Size = System::Drawing::Size(72, 32);
			this->label3->TabIndex = 17;
			this->label3->Text = S"Timerで連続再表示";
			this->label3->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// groupBox2
			// 
			this->groupBox2->Controls->Add(this->button17);
			this->groupBox2->Controls->Add(this->label11);
			this->groupBox2->Controls->Add(this->label10);
			this->groupBox2->Controls->Add(this->label9);
			this->groupBox2->Controls->Add(this->label8);
			this->groupBox2->Controls->Add(this->label7);
			this->groupBox2->Controls->Add(this->label6);
			this->groupBox2->Controls->Add(this->label5);
			this->groupBox2->Controls->Add(this->label4);
			this->groupBox2->Controls->Add(this->textBox22);
			this->groupBox2->Controls->Add(this->textBox21);
			this->groupBox2->Controls->Add(this->textBox20);
			this->groupBox2->Controls->Add(this->textBox18);
			this->groupBox2->Controls->Add(this->textBox17);
			this->groupBox2->Controls->Add(this->textBox16);
			this->groupBox2->Controls->Add(this->textBox15);
			this->groupBox2->Controls->Add(this->textBox19);
			this->groupBox2->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->groupBox2->Location = System::Drawing::Point(328, 152);
			this->groupBox2->Name = S"groupBox2";
			this->groupBox2->Size = System::Drawing::Size(216, 168);
			this->groupBox2->TabIndex = 18;
			this->groupBox2->TabStop = false;
			this->groupBox2->Text = S"アナログ計測";
			// 
			// button17
			// 
			this->button17->BackColor = System::Drawing::SystemColors::Control;
			this->button17->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->button17->Location = System::Drawing::Point(48, 128);
			this->button17->Name = S"button17";
			this->button17->Size = System::Drawing::Size(88, 32);
			this->button17->TabIndex = 17;
			this->button17->Text = S"データ入力";
			this->button17->Click += new System::EventHandler(this, button17_Click);
			// 
			// label11
			// 
			this->label11->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label11->Location = System::Drawing::Point(160, 96);
			this->label11->Name = S"label11";
			this->label11->Size = System::Drawing::Size(40, 24);
			this->label11->TabIndex = 15;
			this->label11->Text = S"AN7";
			this->label11->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// label10
			// 
			this->label10->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label10->Location = System::Drawing::Point(160, 72);
			this->label10->Name = S"label10";
			this->label10->Size = System::Drawing::Size(40, 24);
			this->label10->TabIndex = 14;
			this->label10->Text = S"AN6";
			this->label10->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// label9
			// 
			this->label9->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label9->Location = System::Drawing::Point(160, 48);
			this->label9->Name = S"label9";
			this->label9->Size = System::Drawing::Size(40, 24);
			this->label9->TabIndex = 13;
			this->label9->Text = S"AN5";
			this->label9->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// label8
			// 
			this->label8->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label8->Location = System::Drawing::Point(160, 24);
			this->label8->Name = S"label8";
			this->label8->Size = System::Drawing::Size(40, 24);
			this->label8->TabIndex = 12;
			this->label8->Text = S"AN4";
			this->label8->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// label7
			// 
			this->label7->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label7->Location = System::Drawing::Point(56, 96);
			this->label7->Name = S"label7";
			this->label7->Size = System::Drawing::Size(40, 24);
			this->label7->TabIndex = 11;
			this->label7->Text = S"AN3";
			this->label7->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// label6
			// 
			this->label6->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label6->Location = System::Drawing::Point(56, 72);
			this->label6->Name = S"label6";
			this->label6->Size = System::Drawing::Size(40, 24);
			this->label6->TabIndex = 10;
			this->label6->Text = S"AN2";
			this->label6->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// label5
			// 
			this->label5->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label5->Location = System::Drawing::Point(56, 48);
			this->label5->Name = S"label5";
			this->label5->Size = System::Drawing::Size(40, 24);
			this->label5->TabIndex = 9;
			this->label5->Text = S"AN1";
			this->label5->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// label4
			// 
			this->label4->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label4->Location = System::Drawing::Point(56, 24);
			this->label4->Name = S"label4";
			this->label4->Size = System::Drawing::Size(40, 24);
			this->label4->TabIndex = 8;
			this->label4->Text = S"AN0";
			this->label4->TextAlign = System::Drawing::ContentAlignment::MiddleLeft;
			// 
			// textBox22
			// 
			this->textBox22->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox22->Location = System::Drawing::Point(120, 96);
			this->textBox22->Name = S"textBox22";
			this->textBox22->Size = System::Drawing::Size(40, 23);
			this->textBox22->TabIndex = 7;
			this->textBox22->Text = S"0000";
			this->textBox22->TextAlign = System::Windows::Forms::HorizontalAlignment::Right;
			// 
			// textBox21
			// 
			this->textBox21->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox21->Location = System::Drawing::Point(120, 72);
			this->textBox21->Name = S"textBox21";
			this->textBox21->Size = System::Drawing::Size(40, 23);
			this->textBox21->TabIndex = 6;
			this->textBox21->Text = S"0000";
			this->textBox21->TextAlign = System::Windows::Forms::HorizontalAlignment::Right;
			// 
			// textBox20
			// 
			this->textBox20->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox20->Location = System::Drawing::Point(120, 48);
			this->textBox20->Name = S"textBox20";
			this->textBox20->Size = System::Drawing::Size(40, 23);
			this->textBox20->TabIndex = 5;
			this->textBox20->Text = S"0000";
			this->textBox20->TextAlign = System::Windows::Forms::HorizontalAlignment::Right;
			// 
			// textBox18
			// 
			this->textBox18->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox18->Location = System::Drawing::Point(16, 96);
			this->textBox18->Name = S"textBox18";
			this->textBox18->Size = System::Drawing::Size(40, 23);
			this->textBox18->TabIndex = 3;
			this->textBox18->Text = S"0000";
			this->textBox18->TextAlign = System::Windows::Forms::HorizontalAlignment::Right;
			// 
			// textBox17
			// 
			this->textBox17->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox17->Location = System::Drawing::Point(16, 72);
			this->textBox17->Name = S"textBox17";
			this->textBox17->Size = System::Drawing::Size(40, 23);
			this->textBox17->TabIndex = 2;
			this->textBox17->Text = S"0000";
			this->textBox17->TextAlign = System::Windows::Forms::HorizontalAlignment::Right;
			// 
			// textBox16
			// 
			this->textBox16->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox16->Location = System::Drawing::Point(16, 48);
			this->textBox16->Name = S"textBox16";
			this->textBox16->Size = System::Drawing::Size(40, 23);
			this->textBox16->TabIndex = 1;
			this->textBox16->Text = S"0000";
			this->textBox16->TextAlign = System::Windows::Forms::HorizontalAlignment::Right;
			// 
			// textBox15
			// 
			this->textBox15->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox15->Location = System::Drawing::Point(16, 24);
			this->textBox15->Name = S"textBox15";
			this->textBox15->Size = System::Drawing::Size(40, 23);
			this->textBox15->TabIndex = 0;
			this->textBox15->Text = S"0000";
			this->textBox15->TextAlign = System::Windows::Forms::HorizontalAlignment::Right;
			// 
			// textBox19
			// 
			this->textBox19->Font = new System::Drawing::Font(S"MS UI Gothic", 12, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->textBox19->Location = System::Drawing::Point(120, 24);
			this->textBox19->Name = S"textBox19";
			this->textBox19->Size = System::Drawing::Size(40, 23);
			this->textBox19->TabIndex = 16;
			this->textBox19->Text = S"0000";
			this->textBox19->TextAlign = System::Windows::Forms::HorizontalAlignment::Right;
			// 
			// label12
			// 
			this->label12->Location = System::Drawing::Point(16, 192);
			this->label12->Name = S"label12";
			this->label12->Size = System::Drawing::Size(72, 16);
			this->label12->TabIndex = 19;
			this->label12->Text = S"Command";
			this->label12->TextAlign = System::Drawing::ContentAlignment::BottomCenter;
			// 
			// label13
			// 
			this->label13->Location = System::Drawing::Point(88, 192);
			this->label13->Name = S"label13";
			this->label13->Size = System::Drawing::Size(72, 16);
			this->label13->TabIndex = 20;
			this->label13->Text = S"Message";
			this->label13->TextAlign = System::Drawing::ContentAlignment::BottomCenter;
			// 
			// label14
			// 
			this->label14->Font = new System::Drawing::Font(S"MS UI Gothic", 9.75F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label14->Location = System::Drawing::Point(16, 72);
			this->label14->Name = S"label14";
			this->label14->Size = System::Drawing::Size(104, 16);
			this->label14->TabIndex = 21;
			this->label14->Text = S"LEDの制御";
			// 
			// label15
			// 
			this->label15->Font = new System::Drawing::Font(S"MS UI Gothic", 9.75F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label15->Location = System::Drawing::Point(16, 136);
			this->label15->Name = S"label15";
			this->label15->Size = System::Drawing::Size(112, 16);
			this->label15->TabIndex = 22;
			this->label15->Text = S"液晶表示器の制御";
			// 
			// label16
			// 
			this->label16->Font = new System::Drawing::Font(S"MS UI Gothic", 9.75F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label16->Location = System::Drawing::Point(16, 248);
			this->label16->Name = S"label16";
			this->label16->Size = System::Drawing::Size(80, 16);
			this->label16->TabIndex = 23;
			this->label16->Text = S"データ受信";
			// 
			// label17
			// 
			this->label17->Font = new System::Drawing::Font(S"MS UI Gothic", 9.75F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->label17->Location = System::Drawing::Point(16, 312);
			this->label17->Name = S"label17";
			this->label17->Size = System::Drawing::Size(88, 16);
			this->label17->TabIndex = 24;
			this->label17->Text = S"メッセージ受信";
			// 
			// Form1
			// 
			this->AutoScaleBaseSize = System::Drawing::Size(7, 15);
			this->BackColor = System::Drawing::Color::FromArgb((System::Byte)224, (System::Byte)224, (System::Byte)224);
			this->ClientSize = System::Drawing::Size(560, 374);
			this->Controls->Add(this->label17);
			this->Controls->Add(this->label16);
			this->Controls->Add(this->label15);
			this->Controls->Add(this->label14);
			this->Controls->Add(this->label13);
			this->Controls->Add(this->label12);
			this->Controls->Add(this->groupBox2);
			this->Controls->Add(this->label3);
			this->Controls->Add(this->textBox14);
			this->Controls->Add(this->textBox13);
			this->Controls->Add(this->groupBox1);
			this->Controls->Add(this->label2);
			this->Controls->Add(this->button8);
			this->Controls->Add(this->textBox4);
			this->Controls->Add(this->button7);
			this->Controls->Add(this->button6);
			this->Controls->Add(this->button5);
			this->Controls->Add(this->button4);
			this->Controls->Add(this->button3);
			this->Controls->Add(this->label1);
			this->Controls->Add(this->textBox3);
			this->Controls->Add(this->button2);
			this->Controls->Add(this->textBox2);
			this->Controls->Add(this->textBox1);
			this->Controls->Add(this->button1);
			this->Font = new System::Drawing::Font(S"MS UI Gothic", 11.25F, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point, (System::Byte)128);
			this->Name = S"Form1";
			this->Text = S"General I/O";
			this->Load += new System::EventHandler(this, Form1_Load);
			(__try_cast<System::ComponentModel::ISupportInitialize *  >(this->timer1))->EndInit();
			this->groupBox1->ResumeLayout(false);
			this->groupBox2->ResumeLayout(false);
			this->ResumeLayout(false);

		}	









	
	////////////////////////////////////////////////////////
	// 汎用USBドライバDLLの関数宣言
	///////////////////////////////////////////////////////
	private: System::Void Form1_Load(System::Object *  sender, System::EventArgs *  e)
		{
			//// 汎用USBドライバのロードと内蔵関数の宣言
			libHandle = NULL;
			libHandle = LoadLibrary("mpusbapi");						/// 汎用USBドライバのロード
			if(libHandle == NULL)
			{
				printf("Error loading mpusbapi.dll\r\n");				// ロードエラー
			}
			else
			{
				// 各関数の宣言
				MPUSBGetDLLVersion=(DWORD(*)(void))\
					GetProcAddress(libHandle,"_MPUSBGetDLLVersion");
				MPUSBGetDeviceCount=(DWORD(*)(PCHAR))\
					GetProcAddress(libHandle,"_MPUSBGetDeviceCount");
				MPUSBOpen=(HANDLE(*)(DWORD,PCHAR,PCHAR,DWORD,DWORD))\
					GetProcAddress(libHandle,"_MPUSBOpen");
				MPUSBWrite=(DWORD(*)(HANDLE,PVOID,DWORD,PDWORD,DWORD))\
					GetProcAddress(libHandle,"_MPUSBWrite");
				MPUSBRead=(DWORD(*)(HANDLE,PVOID,DWORD,PDWORD,DWORD))\
					GetProcAddress(libHandle,"_MPUSBRead");
				MPUSBReadInt=(DWORD(*)(HANDLE,PVOID,DWORD,PDWORD,DWORD))\
					GetProcAddress(libHandle,"_MPUSBReadInt");
				MPUSBClose=(BOOL(*)(HANDLE))GetProcAddress(libHandle,"_MPUSBClose");

				if((MPUSBGetDeviceCount == NULL) || (MPUSBOpen == NULL) ||
					(MPUSBWrite == NULL) || (MPUSBRead == NULL) ||
					(MPUSBClose == NULL) || (MPUSBGetDLLVersion == NULL) ||
					(MPUSBReadInt == NULL))
					printf("GetProcAddress Error\r\n");
				timer1->Enabled = false;
				timer1->set_Interval(1000);
			}
		}

	///////////////////////////////////////////////////////////
	//
	//   Formの各イベント処理
	//
	///////////////////////////////////////////////////////////

	private: System::Void button1_Click(System::Object *  sender, System::EventArgs *  e)
		{
			/// USBの接続　接続結果を表示
			DWORD Flag;											// 接続結果フラグ
			DWORD max_count;									// 接続デバイス数

			Flag = 0;
			max_count = MPUSBGetDeviceCount(vid_pid);			// 接続しているデバイス数を調査
			// 接続確認	
			for(int DevID = 0; DevID < max_count; DevID++)		// デバイス数だけ繰り返す
			{
				if(OpenPipe())									// 接続実行
				{
					//// 接続確認と折り返しのOK応答受信
					send_buf[0] = 0x30;							// Command
					RecvLength = 3;
					if(SendReceivePacket(send_buf,1,receive_buf,&RecvLength,1000,1000) == 1)
					{
						/// 応答が"OK"かをチェック
						if((receive_buf[0] == 'O') && (receive_buf[1] == 'K'))
						{
							textBox1->Text =  "Connect";		// 正常接続確認
							Flag = 1;
							ClosePipe();						// 一旦接続切り離し
							break;
						}
					}
				}
			}//end for
			if(Flag == 0)
				textBox1->Text = "Fault";						// 接続異常メッセージ
		}

	private: System::Void button2_Click(System::Object *  sender, System::EventArgs *  e)
		{

			//// LED１のオンオフ制御とボタンの色変更
			if(OpenPipe())
			{
				//// LED1 オンオフ制御出力と折り返しの応答受信
				send_buf[0] = 0x31;			// Command
				RecvLength = 1;
				if(SendReceivePacket(send_buf,1,receive_buf,&RecvLength,1000,1000) == 1)
				{
					if(receive_buf[0] == '0')						// 折り返しの状態により色変更
						button2->BackColor =Color::Red;				// オンの時赤
					else
						button2->BackColor = Color::Green;			// オフの時緑
				}
			}
			//// 終了でパイプを閉じる
			ClosePipe();
		}

	private: System::Void button3_Click(System::Object *  sender, System::EventArgs *  e)
		{
			//// LED2のオンオフ制御とボタンの色変更
			if(OpenPipe())
			{
				//// LED2のオンオフ制御と折り返しの状態受信
				send_buf[0] = 0x32;			// Command
				RecvLength = 1;
				if(SendReceivePacket(send_buf,1,receive_buf,&RecvLength,1000,1000) == 1)
				{
					if(receive_buf[0] == '0')					// 受信した状態で色変更
						button3->BackColor =Color::Red;
					else
						button3->BackColor = Color::Green;
				}
			}
			//// 終了でパイプを閉じる
			ClosePipe();
		}
	private: System::Void button4_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			String* ss;
			int i;

			//// 液晶表示器へメッセージ表示
			if(OpenPipe())
			{
				//// 液晶表示器の表示制御
				send_buf[0] = 0x33;			// Command
				ss = textBox13->Text;
				send_buf[1] = (BYTE)(ss->get_Chars(0));
				send_buf[2] = (BYTE)(ss->get_Chars(1));
				send_buf[1] = toCMND(&send_buf[1]);
				ss = textBox14->Text;
				for(i=0; i<ss->Length; i++)
					send_buf[i+3] = (BYTE)(ss->get_Chars(i));
				send_buf[i+3] = 0;									// 文字列の終端
				RecvLength = 0;
				if(SendReceivePacket(send_buf,i+4,receive_buf,&RecvLength,1000,1000) == 1)
				{
				}
			}
			//// 終了でパイプを閉じる
			ClosePipe();
		 }

	private: System::Void button5_Click(System::Object *  sender, System::EventArgs *  e)
		 {

			 //// 液晶表示器の全消去
			if(OpenPipe())
			{
				//// 液晶表示器の全消去制御
				send_buf[0] = 0x34;			// Command
				RecvLength = 0;
				if(SendReceivePacket(send_buf,1,receive_buf,&RecvLength,1000,1000) == 1)
				{
				}
			}
			//// 終了でパイプを閉じる
			ClosePipe();
		 }


	private: System::Void button6_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			///// 可変抵抗値の要求と表示 
			if(OpenPipe())
			{
				//// 計測値の要求
				send_buf[0] = 0x35;			// Command
				RecvLength = 15;
				if(SendReceivePacket(send_buf,1,receive_buf,&RecvLength,1000,1000) == 1)
				{
					/// 折り返しのデータ表示
					textBox3->Text = (char *)receive_buf;
					timer1->Enabled = true;
				}
			}
			///// パイプを閉じる
			ClosePipe();
		 }

	private: System::Void button7_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			//// 受信メッセージ表示 
			if(OpenPipe())
			{
				//// メッセージ送信要求と表示
				RecvLength = 17;   
				send_buf[0] = 0x38;			// Command38
				if(SendReceivePacket(send_buf,1,receive_buf,&RecvLength,1000,1000) == 1)
				{
					/// 受信メッセージ表示
					textBox4->Text = (char *)receive_buf;
				}
			}
			///// 終了でパイプを閉じる
			ClosePipe();
		 }


	private: System::Void timer1_Elapsed(System::Object *  sender, System::Timers::ElapsedEventArgs *  e)
		{
			//// タイマによる一定間隔の計測要求と表示
			///// 可変抵抗値の要求と表示 
			if(OpenPipe())
			{
				//// 計測値の要求
				send_buf[0] = 0x35;			// Command
				RecvLength = 15;
				if(SendReceivePacket(send_buf,1,receive_buf,&RecvLength,1000,1000) == 1)
				{
					/// 折り返しのデータ表示
					textBox3->Text = (char *)receive_buf;
					timer1->set_Interval(100);
				}
			}
			///// パイプを閉じる
			ClosePipe();
		}

	private: System::Void button8_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			 ////// 終了処理
			 timer1->Close();
			 Close();
		 }


private: System::Void button17_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			///// 可変抵抗値の要求と表示 
			if(OpenPipe())
			{
				//// 計測値の要求
				send_buf[0] = 0x37;			// Command
				RecvLength = 32;
				if(SendReceivePacket(send_buf,1,receive_buf,&RecvLength,1000,1000) == 1)
				{
					/// 折り返しのデータ表示
					strncpy(dumy,(char *)receive_buf, 4);
                    textBox15->Text = dumy;
					strncpy(dumy,(char *)(receive_buf+4), 4);
					textBox16->Text = dumy;
					strncpy(dumy,(char *)(receive_buf+8), 4);
					textBox17->Text = dumy;
					strncpy(dumy,(char *)(receive_buf+12), 4);
					textBox18->Text = dumy;
					strncpy(dumy,(char *)(receive_buf+16), 4);
					textBox19->Text = dumy;
					strncpy(dumy,(char *)(receive_buf+20), 4);
					textBox20->Text = dumy;
					strncpy(dumy,(char *)(receive_buf+24), 4);
					textBox21->Text = dumy;
					strncpy(dumy,(char *)(receive_buf+28), 4);
					textBox22->Text = dumy;
				}
			}
			///// パイプを閉じる
			ClosePipe();
		 }

private: System::Void button13_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			if(OpenPipe())
			{
				//// ポートのオンオフ制御と折り返しの状態受信
				send_buf[0] = 0x36;			// Command
				if(RD3->BackColor == Color::Red)
					send_buf[1] &= 0xF7;
				else
					send_buf[1] |= 0x08;
				if(RD2->BackColor == Color::Red)
					send_buf[1] &= 0xFB;
				else
					send_buf[1] |= 0x04;
				if(RD1->BackColor == Color::Red)
					send_buf[1] &= 0xFD;
				else
					send_buf[1] |= 0x02;
				if(RD0->BackColor == Color::Red)
					send_buf[1] &= 0xFE;
				else
					send_buf[1] |= 0x01;
				RecvLength = 1;
				if(SendReceivePacket(send_buf,2,receive_buf,&RecvLength,1000,1000) == 1)
				{
					if((receive_buf[0] & 0x01)==0)					// 受信した状態で色変更
						RD0->BackColor = Color::Red;
					else
						RD0->BackColor = Color::Green;
					if((receive_buf[0] & 0x02)==0)					// 受信した状態で色変更
						RD1->BackColor = Color::Red;
					else
						RD1->BackColor = Color::Green;
					if((receive_buf[0] & 0x04)==0)					// 受信した状態で色変更
						RD2->BackColor = Color::Red;
					else
						RD2->BackColor = Color::Green;
					if((receive_buf[0] & 0x08)==0)					// 受信した状態で色変更
						RD3->BackColor = Color::Red;
					else
						RD3->BackColor = Color::Green;
					if((receive_buf[0] & 0x10)==0)					// 受信した状態で色変更
						RD4->BackColor = Color::Red;
					else
						RD4->BackColor = Color::Green;
					if((receive_buf[0] & 0x20)==0)					// 受信した状態で色変更
						RD5->BackColor = Color::Red;
					else
						RD5->BackColor = Color::Green;
					if((receive_buf[0] & 0x40)==0)					// 受信した状態で色変更
						RD6->BackColor = Color::Red;
					else
						RD6->BackColor = Color::Green;
					if((receive_buf[0] & 0x80)==0)					// 受信した状態で色変更
						RD7->BackColor = Color::Red;
					else
						RD7->BackColor = Color::Green;
				}
			}
			//// 終了でパイプを閉じる
			ClosePipe();
		 }

private: System::Void LAT0_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			if(RD0->BackColor == Color::Red)
				RD0->BackColor = Color::Green;
			else
				RD0->BackColor = Color::Red;
		 }

private: System::Void LAT1_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			if(RD1->BackColor == Color::Red)
				RD1->BackColor = Color::Green;
			else
				RD1->BackColor = Color::Red;
		 }

private: System::Void LAT2_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			if(RD2->BackColor == Color::Red)
				RD2->BackColor = Color::Green;
			else
				RD2->BackColor = Color::Red;
		 }

private: System::Void LAT3_Click(System::Object *  sender, System::EventArgs *  e)
		 {
			if(RD3->BackColor == Color::Red)
				RD3->BackColor = Color::Green;
			else
				RD3->BackColor = Color::Red;
		 }

};
}


