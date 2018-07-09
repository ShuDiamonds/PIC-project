namespace mbe
{
	partial class SetDrcForm
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
			this.textPtnGap = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textErrChkLimit = new System.Windows.Forms.TextBox();
			this.IDOK = new System.Windows.Forms.Button();
			this.IDCANCEL = new System.Windows.Forms.Button();
			this.labelRangeGap = new System.Windows.Forms.Label();
			this.labelRangeLimit = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(66, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "Pattern Gap";
			// 
			// textPtnGap
			// 
			this.textPtnGap.Location = new System.Drawing.Point(80, 8);
			this.textPtnGap.Name = "textPtnGap";
			this.textPtnGap.Size = new System.Drawing.Size(64, 19);
			this.textPtnGap.TabIndex = 3;
			this.textPtnGap.TextChanged += new System.EventHandler(this.OnTextPtnGapChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(144, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(23, 12);
			this.label6.TabIndex = 4;
			this.label6.Text = "mm";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "Error Limit";
			// 
			// textErrChkLimit
			// 
			this.textErrChkLimit.Location = new System.Drawing.Point(80, 32);
			this.textErrChkLimit.Name = "textErrChkLimit";
			this.textErrChkLimit.Size = new System.Drawing.Size(64, 19);
			this.textErrChkLimit.TabIndex = 6;
			this.textErrChkLimit.TextChanged += new System.EventHandler(this.OnTextErrChkLimit);
			// 
			// IDOK
			// 
			this.IDOK.Location = new System.Drawing.Point(144, 72);
			this.IDOK.Name = "IDOK";
			this.IDOK.Size = new System.Drawing.Size(75, 23);
			this.IDOK.TabIndex = 0;
			this.IDOK.Text = "OK";
			this.IDOK.UseVisualStyleBackColor = true;
			this.IDOK.Click += new System.EventHandler(this.OnOK);
			// 
			// IDCANCEL
			// 
			this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.IDCANCEL.Location = new System.Drawing.Point(232, 72);
			this.IDCANCEL.Name = "IDCANCEL";
			this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
			this.IDCANCEL.TabIndex = 1;
			this.IDCANCEL.Text = "Cancel";
			this.IDCANCEL.UseVisualStyleBackColor = true;
			// 
			// labelRangeGap
			// 
			this.labelRangeGap.AutoSize = true;
			this.labelRangeGap.Location = new System.Drawing.Point(184, 16);
			this.labelRangeGap.Name = "labelRangeGap";
			this.labelRangeGap.Size = new System.Drawing.Size(53, 12);
			this.labelRangeGap.TabIndex = 7;
			this.labelRangeGap.Text = "rangeGap";
			// 
			// labelRangeLimit
			// 
			this.labelRangeLimit.AutoSize = true;
			this.labelRangeLimit.Location = new System.Drawing.Point(184, 40);
			this.labelRangeLimit.Name = "labelRangeLimit";
			this.labelRangeLimit.Size = new System.Drawing.Size(58, 12);
			this.labelRangeLimit.TabIndex = 8;
			this.labelRangeLimit.Text = "rangeLimit";
			// 
			// SetDrcForm
			// 
			this.AcceptButton = this.IDOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.IDCANCEL;
			this.ClientSize = new System.Drawing.Size(314, 102);
			this.Controls.Add(this.labelRangeLimit);
			this.Controls.Add(this.labelRangeGap);
			this.Controls.Add(this.IDCANCEL);
			this.Controls.Add(this.IDOK);
			this.Controls.Add(this.textErrChkLimit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textPtnGap);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetDrcForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DRC";
			this.Load += new System.EventHandler(this.FormLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textPtnGap;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textErrChkLimit;
		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Label labelRangeGap;
		private System.Windows.Forms.Label labelRangeLimit;
	}
}