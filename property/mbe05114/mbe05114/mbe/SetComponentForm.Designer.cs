namespace mbe
{
	partial class SetComponentForm
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
            this.labelWidthRange = new System.Windows.Forms.Label();
            this.labelHeightRange = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLineWidth = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxRefNum = new System.Windows.Forms.TextBox();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.IDOK = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxDrawOnDoc = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxRemarks = new System.Windows.Forms.TextBox();
            this.checkBox_unlock_edit_angle = new System.Windows.Forms.CheckBox();
            this.textBox_angle = new System.Windows.Forms.TextBox();
            this.labelAngleRange = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxPackage = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelWidthRange
            // 
            this.labelWidthRange.AutoSize = true;
            this.labelWidthRange.Location = new System.Drawing.Point(32, 128);
            this.labelWidthRange.Name = "labelWidthRange";
            this.labelWidthRange.Size = new System.Drawing.Size(65, 12);
            this.labelWidthRange.TabIndex = 9;
            this.labelWidthRange.Text = "WidthRange";
            // 
            // labelHeightRange
            // 
            this.labelHeightRange.AutoSize = true;
            this.labelHeightRange.Location = new System.Drawing.Point(32, 80);
            this.labelHeightRange.Name = "labelHeightRange";
            this.labelHeightRange.Size = new System.Drawing.Size(70, 12);
            this.labelHeightRange.TabIndex = 5;
            this.labelHeightRange.Text = "HeightRange";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(144, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "mm";
            // 
            // textBoxLineWidth
            // 
            this.textBoxLineWidth.Location = new System.Drawing.Point(88, 104);
            this.textBoxLineWidth.Name = "textBoxLineWidth";
            this.textBoxLineWidth.Size = new System.Drawing.Size(48, 19);
            this.textBoxLineWidth.TabIndex = 7;
            this.textBoxLineWidth.TextChanged += new System.EventHandler(this.OnTextBoxLineWidthChanged);
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(88, 56);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(48, 19);
            this.textBoxHeight.TabIndex = 3;
            this.textBoxHeight.TextChanged += new System.EventHandler(this.OnTextBoxHeightChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Line Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Height";
            // 
            // textBoxRefNum
            // 
            this.textBoxRefNum.Location = new System.Drawing.Point(88, 24);
            this.textBoxRefNum.Name = "textBoxRefNum";
            this.textBoxRefNum.Size = new System.Drawing.Size(88, 19);
            this.textBoxRefNum.TabIndex = 1;
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(232, 40);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 9;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(232, 8);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 8;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.OnOK);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(72, 192);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(232, 19);
            this.textBoxName.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxDrawOnDoc);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxRefNum);
            this.groupBox1.Controls.Add(this.textBoxLineWidth);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelWidthRange);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelHeightRange);
            this.groupBox1.Controls.Add(this.textBoxHeight);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 176);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reference Number";
            // 
            // checkBoxDrawOnDoc
            // 
            this.checkBoxDrawOnDoc.AutoSize = true;
            this.checkBoxDrawOnDoc.Location = new System.Drawing.Point(24, 152);
            this.checkBoxDrawOnDoc.Name = "checkBoxDrawOnDoc";
            this.checkBoxDrawOnDoc.Size = new System.Drawing.Size(143, 16);
            this.checkBoxDrawOnDoc.TabIndex = 10;
            this.checkBoxDrawOnDoc.Text = "Draw on the DOC layer";
            this.checkBoxDrawOnDoc.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "Remarks";
            // 
            // textBoxRemarks
            // 
            this.textBoxRemarks.AcceptsReturn = true;
            this.textBoxRemarks.Location = new System.Drawing.Point(16, 264);
            this.textBoxRemarks.Multiline = true;
            this.textBoxRemarks.Name = "textBoxRemarks";
            this.textBoxRemarks.Size = new System.Drawing.Size(288, 80);
            this.textBoxRemarks.TabIndex = 6;
            // 
            // checkBox_unlock_edit_angle
            // 
            this.checkBox_unlock_edit_angle.AutoSize = true;
            this.checkBox_unlock_edit_angle.Location = new System.Drawing.Point(8, 24);
            this.checkBox_unlock_edit_angle.Name = "checkBox_unlock_edit_angle";
            this.checkBox_unlock_edit_angle.Size = new System.Drawing.Size(59, 16);
            this.checkBox_unlock_edit_angle.TabIndex = 0;
            this.checkBox_unlock_edit_angle.Text = "Unlock";
            this.checkBox_unlock_edit_angle.UseVisualStyleBackColor = true;
            this.checkBox_unlock_edit_angle.CheckedChanged += new System.EventHandler(this.checkBox_unlock_edit_angle_CheckedChanged);
            // 
            // textBox_angle
            // 
            this.textBox_angle.Location = new System.Drawing.Point(8, 48);
            this.textBox_angle.Name = "textBox_angle";
            this.textBox_angle.Size = new System.Drawing.Size(72, 19);
            this.textBox_angle.TabIndex = 1;
            this.textBox_angle.TextChanged += new System.EventHandler(this.textBox_angle_TextChanged);
            // 
            // labelAngleRange
            // 
            this.labelAngleRange.AutoSize = true;
            this.labelAngleRange.Location = new System.Drawing.Point(8, 72);
            this.labelAngleRange.Name = "labelAngleRange";
            this.labelAngleRange.Size = new System.Drawing.Size(51, 12);
            this.labelAngleRange.TabIndex = 2;
            this.labelAngleRange.Text = "0 - 359.9";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelAngleRange);
            this.groupBox2.Controls.Add(this.textBox_angle);
            this.groupBox2.Controls.Add(this.checkBox_unlock_edit_angle);
            this.groupBox2.Location = new System.Drawing.Point(208, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(96, 96);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current angle";
            // 
            // textBoxPackage
            // 
            this.textBoxPackage.Location = new System.Drawing.Point(72, 216);
            this.textBoxPackage.Name = "textBoxPackage";
            this.textBoxPackage.Size = new System.Drawing.Size(232, 19);
            this.textBoxPackage.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 224);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "Package";
            // 
            // SetComponentForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(321, 351);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxPackage);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBoxRemarks);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetComponentForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Component";
            this.Load += new System.EventHandler(this.FormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelWidthRange;
		private System.Windows.Forms.Label labelHeightRange;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxLineWidth;
		private System.Windows.Forms.TextBox textBoxHeight;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxRefNum;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textBoxRemarks;
		private System.Windows.Forms.CheckBox checkBoxDrawOnDoc;
        private System.Windows.Forms.CheckBox checkBox_unlock_edit_angle;
        private System.Windows.Forms.TextBox textBox_angle;
        private System.Windows.Forms.Label labelAngleRange;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxPackage;
        private System.Windows.Forms.Label label8;
	}
}