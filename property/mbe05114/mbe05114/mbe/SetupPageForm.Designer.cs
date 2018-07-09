namespace mbe
{
	partial class SetupPageForm
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textLeftMargin = new System.Windows.Forms.TextBox();
            this.textBottomMargin = new System.Windows.Forms.TextBox();
            this.IDOK = new System.Windows.Forms.Button();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.check2xPrintMode = new System.Windows.Forms.CheckBox();
            this.checkCenterPunchMode = new System.Windows.Forms.CheckBox();
            this.checkToolMarkMode = new System.Windows.Forms.CheckBox();
            this.checkCurrentView = new System.Windows.Forms.CheckBox();
            this.buttonSetLayer = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioColBmp = new System.Windows.Forms.RadioButton();
            this.radioColVector = new System.Windows.Forms.RadioButton();
            this.radioBW = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxHeader = new System.Windows.Forms.CheckBox();
            this.textBoxHeader = new System.Windows.Forms.TextBox();
            this.checkMirror = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Left margin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Bottom margin";
            // 
            // textLeftMargin
            // 
            this.textLeftMargin.Location = new System.Drawing.Point(104, 8);
            this.textLeftMargin.Name = "textLeftMargin";
            this.textLeftMargin.Size = new System.Drawing.Size(64, 19);
            this.textLeftMargin.TabIndex = 1;
            this.textLeftMargin.TextChanged += new System.EventHandler(this.textLeftMargin_TextChanged);
            // 
            // textBottomMargin
            // 
            this.textBottomMargin.Location = new System.Drawing.Point(104, 40);
            this.textBottomMargin.Name = "textBottomMargin";
            this.textBottomMargin.Size = new System.Drawing.Size(64, 19);
            this.textBottomMargin.TabIndex = 5;
            this.textBottomMargin.TextChanged += new System.EventHandler(this.textBottomMargin_TextChanged);
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(320, 8);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 15;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.IDOK_Click);
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(320, 40);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 16;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "mm";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(208, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "(0 - 30mm)";
            // 
            // check2xPrintMode
            // 
            this.check2xPrintMode.AutoSize = true;
            this.check2xPrintMode.Location = new System.Drawing.Point(24, 112);
            this.check2xPrintMode.Name = "check2xPrintMode";
            this.check2xPrintMode.Size = new System.Drawing.Size(67, 16);
            this.check2xPrintMode.TabIndex = 10;
            this.check2xPrintMode.Text = "x2 mode";
            this.check2xPrintMode.UseVisualStyleBackColor = true;
            // 
            // checkCenterPunchMode
            // 
            this.checkCenterPunchMode.AutoSize = true;
            this.checkCenterPunchMode.Location = new System.Drawing.Point(24, 136);
            this.checkCenterPunchMode.Name = "checkCenterPunchMode";
            this.checkCenterPunchMode.Size = new System.Drawing.Size(123, 16);
            this.checkCenterPunchMode.TabIndex = 11;
            this.checkCenterPunchMode.Text = "Center punch mode";
            this.checkCenterPunchMode.UseVisualStyleBackColor = true;
            // 
            // checkToolMarkMode
            // 
            this.checkToolMarkMode.AutoSize = true;
            this.checkToolMarkMode.Location = new System.Drawing.Point(24, 160);
            this.checkToolMarkMode.Name = "checkToolMarkMode";
            this.checkToolMarkMode.Size = new System.Drawing.Size(106, 16);
            this.checkToolMarkMode.TabIndex = 12;
            this.checkToolMarkMode.Text = "Tool mark mode";
            this.checkToolMarkMode.UseVisualStyleBackColor = true;
            // 
            // checkCurrentView
            // 
            this.checkCurrentView.AutoSize = true;
            this.checkCurrentView.Location = new System.Drawing.Point(16, 24);
            this.checkCurrentView.Name = "checkCurrentView";
            this.checkCurrentView.Size = new System.Drawing.Size(89, 16);
            this.checkCurrentView.TabIndex = 0;
            this.checkCurrentView.Text = "Current view";
            this.checkCurrentView.UseVisualStyleBackColor = true;
            this.checkCurrentView.CheckedChanged += new System.EventHandler(this.checkCurrentView_CheckedChanged);
            // 
            // buttonSetLayer
            // 
            this.buttonSetLayer.Location = new System.Drawing.Point(8, 48);
            this.buttonSetLayer.Name = "buttonSetLayer";
            this.buttonSetLayer.Size = new System.Drawing.Size(120, 23);
            this.buttonSetLayer.TabIndex = 1;
            this.buttonSetLayer.Text = "Set print layer";
            this.buttonSetLayer.UseVisualStyleBackColor = true;
            this.buttonSetLayer.Click += new System.EventHandler(this.buttonSetLayer_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSetLayer);
            this.groupBox1.Controls.Add(this.checkCurrentView);
            this.groupBox1.Location = new System.Drawing.Point(248, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 80);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Layer";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioColBmp);
            this.groupBox2.Controls.Add(this.radioColVector);
            this.groupBox2.Controls.Add(this.radioBW);
            this.groupBox2.Location = new System.Drawing.Point(8, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 88);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color mode";
            // 
            // radioColBmp
            // 
            this.radioColBmp.AutoSize = true;
            this.radioColBmp.Location = new System.Drawing.Point(16, 64);
            this.radioColBmp.Name = "radioColBmp";
            this.radioColBmp.Size = new System.Drawing.Size(352, 16);
            this.radioColBmp.TabIndex = 2;
            this.radioColBmp.TabStop = true;
            this.radioColBmp.Text = "Color Bitmap (Recommended for color printing to a PDF printer)";
            this.radioColBmp.UseVisualStyleBackColor = true;
            // 
            // radioColVector
            // 
            this.radioColVector.AutoSize = true;
            this.radioColVector.Location = new System.Drawing.Point(16, 40);
            this.radioColVector.Name = "radioColVector";
            this.radioColVector.Size = new System.Drawing.Size(304, 16);
            this.radioColVector.TabIndex = 1;
            this.radioColVector.TabStop = true;
            this.radioColVector.Text = "Color Vector (Recommended for general color printing)";
            this.radioColVector.UseVisualStyleBackColor = true;
            // 
            // radioBW
            // 
            this.radioBW.AutoSize = true;
            this.radioBW.Location = new System.Drawing.Point(16, 16);
            this.radioBW.Name = "radioBW";
            this.radioBW.Size = new System.Drawing.Size(106, 16);
            this.radioBW.TabIndex = 0;
            this.radioBW.TabStop = true;
            this.radioBW.Text = "Black and White";
            this.radioBW.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(208, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "(0 - 30mm)";
            // 
            // checkBoxHeader
            // 
            this.checkBoxHeader.AutoSize = true;
            this.checkBoxHeader.Location = new System.Drawing.Point(24, 80);
            this.checkBoxHeader.Name = "checkBoxHeader";
            this.checkBoxHeader.Size = new System.Drawing.Size(60, 16);
            this.checkBoxHeader.TabIndex = 8;
            this.checkBoxHeader.Text = "Header";
            this.checkBoxHeader.UseVisualStyleBackColor = true;
            this.checkBoxHeader.CheckedChanged += new System.EventHandler(this.checkBoxHeader_CheckedChanged);
            // 
            // textBoxHeader
            // 
            this.textBoxHeader.Location = new System.Drawing.Point(104, 80);
            this.textBoxHeader.Name = "textBoxHeader";
            this.textBoxHeader.Size = new System.Drawing.Size(288, 19);
            this.textBoxHeader.TabIndex = 9;
            // 
            // checkMirror
            // 
            this.checkMirror.AutoSize = true;
            this.checkMirror.Location = new System.Drawing.Point(163, 112);
            this.checkMirror.Name = "checkMirror";
            this.checkMirror.Size = new System.Drawing.Size(54, 16);
            this.checkMirror.TabIndex = 13;
            this.checkMirror.Text = "Mirror";
            this.checkMirror.UseVisualStyleBackColor = true;
            // 
            // SetupPageForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(402, 284);
            this.Controls.Add(this.checkMirror);
            this.Controls.Add(this.textBoxHeader);
            this.Controls.Add(this.checkBoxHeader);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkToolMarkMode);
            this.Controls.Add(this.checkCenterPunchMode);
            this.Controls.Add(this.check2xPrintMode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.Controls.Add(this.textBottomMargin);
            this.Controls.Add(this.textLeftMargin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupPageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup Page";
            this.Load += new System.EventHandler(this.SetupPageForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textLeftMargin;
		private System.Windows.Forms.TextBox textBottomMargin;
		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox check2xPrintMode;
        private System.Windows.Forms.CheckBox checkCenterPunchMode;
        private System.Windows.Forms.CheckBox checkToolMarkMode;
        private System.Windows.Forms.CheckBox checkCurrentView;
        private System.Windows.Forms.Button buttonSetLayer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioColBmp;
        private System.Windows.Forms.RadioButton radioColVector;
        private System.Windows.Forms.RadioButton radioBW;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxHeader;
        private System.Windows.Forms.TextBox textBoxHeader;
        private System.Windows.Forms.CheckBox checkMirror;
	}
}