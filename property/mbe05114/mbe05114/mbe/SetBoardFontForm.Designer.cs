namespace mbe
{
	partial class SetBoardFontForm
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
			this.textFontPath = new System.Windows.Forms.TextBox();
			this.ButtonRef = new System.Windows.Forms.Button();
			this.checkUseEmbFont = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// IDOK
			// 
			this.IDOK.Location = new System.Drawing.Point(216, 88);
			this.IDOK.Name = "IDOK";
			this.IDOK.Size = new System.Drawing.Size(75, 23);
			this.IDOK.TabIndex = 4;
			this.IDOK.Text = "OK";
			this.IDOK.UseVisualStyleBackColor = true;
			this.IDOK.Click += new System.EventHandler(this.OnOK);
			// 
			// IDCANCEL
			// 
			this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.IDCANCEL.Location = new System.Drawing.Point(296, 88);
			this.IDCANCEL.Name = "IDCANCEL";
			this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
			this.IDCANCEL.TabIndex = 5;
			this.IDCANCEL.Text = "Cancel";
			this.IDCANCEL.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "Board font path";
			// 
			// textFontPath
			// 
			this.textFontPath.Location = new System.Drawing.Point(16, 56);
			this.textFontPath.Name = "textFontPath";
			this.textFontPath.Size = new System.Drawing.Size(288, 19);
			this.textFontPath.TabIndex = 2;
			// 
			// ButtonRef
			// 
			this.ButtonRef.Location = new System.Drawing.Point(312, 56);
			this.ButtonRef.Name = "ButtonRef";
			this.ButtonRef.Size = new System.Drawing.Size(56, 24);
			this.ButtonRef.TabIndex = 3;
			this.ButtonRef.Text = "&Ref...";
			this.ButtonRef.UseVisualStyleBackColor = true;
			this.ButtonRef.Click += new System.EventHandler(this.OnButtonRef);
			// 
			// checkUseEmbFont
			// 
			this.checkUseEmbFont.AutoSize = true;
			this.checkUseEmbFont.Location = new System.Drawing.Point(16, 8);
			this.checkUseEmbFont.Name = "checkUseEmbFont";
			this.checkUseEmbFont.Size = new System.Drawing.Size(123, 16);
			this.checkUseEmbFont.TabIndex = 0;
			this.checkUseEmbFont.Text = "Use embedded font";
			this.checkUseEmbFont.UseVisualStyleBackColor = true;
			this.checkUseEmbFont.CheckedChanged += new System.EventHandler(this.OnChangeCheckUseEmbFont);
			// 
			// SetBoardFontForm
			// 
			this.AcceptButton = this.IDOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.IDCANCEL;
			this.ClientSize = new System.Drawing.Size(377, 120);
			this.Controls.Add(this.checkUseEmbFont);
			this.Controls.Add(this.ButtonRef);
			this.Controls.Add(this.textFontPath);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.IDCANCEL);
			this.Controls.Add(this.IDOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetBoardFontForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Set Board Font";
			this.Load += new System.EventHandler(this.FormLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textFontPath;
		private System.Windows.Forms.Button ButtonRef;
		private System.Windows.Forms.CheckBox checkUseEmbFont;
	}
}