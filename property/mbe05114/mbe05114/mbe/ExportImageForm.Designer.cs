namespace mbe
{
	partial class ExportImageForm
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
            this.IDOK = new System.Windows.Forms.Button();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textLeft = new System.Windows.Forms.TextBox();
            this.textBottom = new System.Windows.Forms.TextBox();
            this.textWidth = new System.Windows.Forms.TextBox();
            this.textHeight = new System.Windows.Forms.TextBox();
            this.textResolution = new System.Windows.Forms.TextBox();
            this.LabelResRange = new System.Windows.Forms.Label();
            this.buttonRef = new System.Windows.Forms.Button();
            this.textFileName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.checkColorMode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(312, 8);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 20;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.IDOK_Click);
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(312, 40);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 21;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Left";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "Resolution";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "Bottom";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(144, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "dpi";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(128, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "mm";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(128, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "mm";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(272, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 8;
            this.label9.Text = "mm";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(272, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 12);
            this.label10.TabIndex = 11;
            this.label10.Text = "mm";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(168, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Height";
            // 
            // textLeft
            // 
            this.textLeft.Location = new System.Drawing.Point(72, 8);
            this.textLeft.Name = "textLeft";
            this.textLeft.Size = new System.Drawing.Size(56, 19);
            this.textLeft.TabIndex = 1;
            this.textLeft.TextChanged += new System.EventHandler(this.textLeft_TextChanged);
            // 
            // textBottom
            // 
            this.textBottom.Location = new System.Drawing.Point(72, 40);
            this.textBottom.Name = "textBottom";
            this.textBottom.Size = new System.Drawing.Size(56, 19);
            this.textBottom.TabIndex = 4;
            this.textBottom.TextChanged += new System.EventHandler(this.textBottom_TextChanged);
            // 
            // textWidth
            // 
            this.textWidth.Location = new System.Drawing.Point(216, 8);
            this.textWidth.Name = "textWidth";
            this.textWidth.Size = new System.Drawing.Size(56, 19);
            this.textWidth.TabIndex = 7;
            this.textWidth.TextChanged += new System.EventHandler(this.textWidth_TextChanged);
            // 
            // textHeight
            // 
            this.textHeight.Location = new System.Drawing.Point(216, 40);
            this.textHeight.Name = "textHeight";
            this.textHeight.Size = new System.Drawing.Size(56, 19);
            this.textHeight.TabIndex = 10;
            this.textHeight.TextChanged += new System.EventHandler(this.textHeight_TextChanged);
            // 
            // textResolution
            // 
            this.textResolution.Location = new System.Drawing.Point(88, 72);
            this.textResolution.Name = "textResolution";
            this.textResolution.Size = new System.Drawing.Size(56, 19);
            this.textResolution.TabIndex = 13;
            this.textResolution.TextChanged += new System.EventHandler(this.textResolution_TextChanged);
            // 
            // LabelResRange
            // 
            this.LabelResRange.AutoSize = true;
            this.LabelResRange.Location = new System.Drawing.Point(184, 80);
            this.LabelResRange.Name = "LabelResRange";
            this.LabelResRange.Size = new System.Drawing.Size(95, 12);
            this.LabelResRange.TabIndex = 15;
            this.LabelResRange.Text = "Resolution Range";
            // 
            // buttonRef
            // 
            this.buttonRef.Location = new System.Drawing.Point(312, 152);
            this.buttonRef.Name = "buttonRef";
            this.buttonRef.Size = new System.Drawing.Size(75, 23);
            this.buttonRef.TabIndex = 19;
            this.buttonRef.Text = "Ref...";
            this.buttonRef.UseVisualStyleBackColor = true;
            this.buttonRef.Click += new System.EventHandler(this.buttonRef_Click);
            // 
            // textFileName
            // 
            this.textFileName.Location = new System.Drawing.Point(8, 152);
            this.textFileName.Name = "textFileName";
            this.textFileName.Size = new System.Drawing.Size(296, 19);
            this.textFileName.TabIndex = 18;
            this.textFileName.TextChanged += new System.EventHandler(this.textFileName_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 136);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 12);
            this.label11.TabIndex = 17;
            this.label11.Text = "Image file";
            // 
            // checkColorMode
            // 
            this.checkColorMode.AutoSize = true;
            this.checkColorMode.Location = new System.Drawing.Point(24, 112);
            this.checkColorMode.Name = "checkColorMode";
            this.checkColorMode.Size = new System.Drawing.Size(82, 16);
            this.checkColorMode.TabIndex = 16;
            this.checkColorMode.Text = "Color mode";
            this.checkColorMode.UseVisualStyleBackColor = true;
            // 
            // ExportImageForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(396, 187);
            this.Controls.Add(this.checkColorMode);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textFileName);
            this.Controls.Add(this.buttonRef);
            this.Controls.Add(this.LabelResRange);
            this.Controls.Add(this.textResolution);
            this.Controls.Add(this.textHeight);
            this.Controls.Add(this.textWidth);
            this.Controls.Add(this.textBottom);
            this.Controls.Add(this.textLeft);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportImageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Image File";
            this.Load += new System.EventHandler(this.ExportBitmapForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textLeft;
		private System.Windows.Forms.TextBox textBottom;
		private System.Windows.Forms.TextBox textWidth;
		private System.Windows.Forms.TextBox textHeight;
		private System.Windows.Forms.TextBox textResolution;
		private System.Windows.Forms.Label LabelResRange;
		private System.Windows.Forms.Button buttonRef;
		private System.Windows.Forms.TextBox textFileName;
		private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkColorMode;
	}
}