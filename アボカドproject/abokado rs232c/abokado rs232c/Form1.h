#pragma once

namespace abokadors232c {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Form1 の概要
	/// </summary>
	public ref class Form1 : public System::Windows::Forms::Form
	{
	public:
		Form1(void)
		{
			InitializeComponent();
			//
			//TODO: ここにコンストラクター コードを追加します
			//
		}

	protected:
		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		~Form1()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::Button^  button1;
	protected: 
	private: System::IO::Ports::SerialPort^  serialPort1;
	private: System::Windows::Forms::TextBox^  textBox1;
	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::Button^  button2;
	private: System::Windows::Forms::ComboBox^  comboBox1;
	private: System::Windows::Forms::Timer^  timer1;
	private: System::Windows::Forms::Button^  button3;



	private: System::ComponentModel::IContainer^  components;

	private:
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>


#pragma region Windows Form Designer generated code
		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		void InitializeComponent(void)
		{
			this->components = (gcnew System::ComponentModel::Container());
			this->button1 = (gcnew System::Windows::Forms::Button());
			this->serialPort1 = (gcnew System::IO::Ports::SerialPort(this->components));
			this->textBox1 = (gcnew System::Windows::Forms::TextBox());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->button2 = (gcnew System::Windows::Forms::Button());
			this->comboBox1 = (gcnew System::Windows::Forms::ComboBox());
			this->timer1 = (gcnew System::Windows::Forms::Timer(this->components));
			this->button3 = (gcnew System::Windows::Forms::Button());
			this->SuspendLayout();
			// 
			// button1
			// 
			this->button1->Location = System::Drawing::Point(56, 212);
			this->button1->Name = L"button1";
			this->button1->Size = System::Drawing::Size(52, 22);
			this->button1->TabIndex = 0;
			this->button1->Text = L"OPEN";
			this->button1->UseVisualStyleBackColor = true;
			this->button1->Click += gcnew System::EventHandler(this, &Form1::button1_Click);
			// 
			// serialPort1
			// 
			this->serialPort1->PortName = L"COM3";
			this->serialPort1->ReadTimeout = 300;
			// 
			// textBox1
			// 
			this->textBox1->Location = System::Drawing::Point(37, 27);
			this->textBox1->Multiline = true;
			this->textBox1->Name = L"textBox1";
			this->textBox1->ScrollBars = System::Windows::Forms::ScrollBars::Vertical;
			this->textBox1->Size = System::Drawing::Size(216, 132);
			this->textBox1->TabIndex = 1;
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Location = System::Drawing::Point(35, 179);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(55, 12);
			this->label1->TabIndex = 2;
			this->label1->Text = L"SerialPort";
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->Location = System::Drawing::Point(222, 185);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(0, 12);
			this->label2->TabIndex = 3;
			// 
			// button2
			// 
			this->button2->Location = System::Drawing::Point(195, 212);
			this->button2->Name = L"button2";
			this->button2->Size = System::Drawing::Size(58, 22);
			this->button2->TabIndex = 4;
			this->button2->Text = L"CLOSE";
			this->button2->UseVisualStyleBackColor = true;
			this->button2->Click += gcnew System::EventHandler(this, &Form1::button2_Click);
			// 
			// comboBox1
			// 
			this->comboBox1->FormattingEnabled = true;
			this->comboBox1->Items->AddRange(gcnew cli::array< System::Object^  >(9) {L"COM1", L"COM2", L"COM3", L"COM4", L"COM5", L"COM6", 
				L"COM7", L"COM8", L"COM9"});
			this->comboBox1->Location = System::Drawing::Point(95, 176);
			this->comboBox1->Name = L"comboBox1";
			this->comboBox1->Size = System::Drawing::Size(121, 20);
			this->comboBox1->TabIndex = 5;
			// 
			// timer1
			// 
			this->timer1->Tick += gcnew System::EventHandler(this, &Form1::timer1_Tick);
			// 
			// button3
			// 
			this->button3->Location = System::Drawing::Point(228, 176);
			this->button3->Name = L"button3";
			this->button3->Size = System::Drawing::Size(47, 24);
			this->button3->TabIndex = 6;
			this->button3->Text = L"送信";
			this->button3->UseVisualStyleBackColor = true;
			this->button3->Click += gcnew System::EventHandler(this, &Form1::button3_Click);
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 12);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(304, 262);
			this->Controls->Add(this->button3);
			this->Controls->Add(this->comboBox1);
			this->Controls->Add(this->button2);
			this->Controls->Add(this->label2);
			this->Controls->Add(this->label1);
			this->Controls->Add(this->textBox1);
			this->Controls->Add(this->button1);
			this->Name = L"Form1";
			this->Text = L"Form1";
			this->Load += gcnew System::EventHandler(this, &Form1::Form1_Load);
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: System::Void button1_Click(System::Object^  sender, System::EventArgs^  e) {
				 String^  message = "";
				


   
				try
	 { 
  

		  if(serialPort1->IsOpen == false)	//シリアルポートが閉じてるとき
			  {
	  //============================================================== 
	 //ポートの設定 
	 // serialPort1->PortName = "COM3"; 
	  serialPort1->PortName = comboBox1->Text;

	  serialPort1->BaudRate = 9600; 
	
 	  //============================================================== 
	  //ポートを開く 
	  serialPort1->Open(); 

			 }	
	  //serialPort1->Write("F"); 
	  
	   // i = serialPort1->ReadChar();
	  
	  //============================================================== 
	  //“right”を送信する 
	  //serialPort1->Write("7"); 
	  //message = Console::ReadLine();
	 // i = serialPort1->ReadChar ();


	 //string message = _serialPort.ReadLine();
	  //============================================================== 
	  //dsPIC側のプログラムに合わせて、改行文字も送信しておく 
	  //serialPort1->Write("\r"); 
  
	  //============================================================== 
	  //とりあえず、ポートをクローズ 
	 // serialPort1->Close(); 
  
	 }
		 catch(TimeoutException^ e)
		 {
			 MessageBox::Show(e->ToString());
			 serialPort1->Close(); 
  
		 }
	 catch(SystemException^ e) 
	 {
		MessageBox::Show(e->ToString()); 
	 } 
 //TextBox1::Text = message;
  //textSum->Text = sum.ToString();
  //textBox1->Text = i.ToString();
			 }

	private: System::Void button2_Click(System::Object^  sender, System::EventArgs^  e) {
				serialPort1->Close(); 
				// textBox1->Text =message;
			 }
private: System::Void Form1_Load(System::Object^  sender, System::EventArgs^  e) {

			 timer1->Interval = 1000; // 何ﾐﾘ秒毎にｲﾍﾞﾝﾄ発生させるか指定
			 timer1->Enabled = true; // ﾀｲﾏｰを停止状態で初期化

		 }
private: System::Void timer1_Tick(System::Object^  sender, System::EventArgs^  e) {
			 //timer処理部

	try
	 { 
			  String^  buffer = "";
			  if(serialPort1->IsOpen == true)	//シリアルポートが開いてるとき
			  {
				  if(serialPort1->BytesToRead)
				  {
					  buffer = serialPort1->ReadLine();		//RS232C受信
					  textBox1->Text = buffer;					//表示
				  }
			  }

	}
	catch(TimeoutException^ e)
	{
		MessageBox::Show(e->ToString());
		serialPort1->Close(); 
  
	}
	catch(SystemException^ e) 
	{
	MessageBox::Show(e->ToString()); 
	} 

		 }
private: System::Void button3_Click(System::Object^  sender, System::EventArgs^  e) {

				array<System::Byte>^ kappa1 = {0x40,1,0x08,0x3f,0x2b};
				int offset=0; 
				int count=5;

			 try
	 { 
			  String^  buffer = "";
			  if(serialPort1->IsOpen == true)	//シリアルポートが開いてるとき
			  {
				    serialPort1->Write(kappa1,offset,count); 
			  }

	}
	catch(TimeoutException^ e)
	{
		MessageBox::Show(e->ToString());
		serialPort1->Close(); 
  
	}
	catch(SystemException^ e) 
	{
	MessageBox::Show(e->ToString()); 
	} 

		 }
};
}

