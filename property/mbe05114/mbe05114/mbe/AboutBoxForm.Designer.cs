namespace mbe
{
	partial class AboutBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBoxForm));
            this.IDOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabelSuigyodo = new System.Windows.Forms.LinkLabel();
            this.usercfgFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(296, 16);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 0;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.OnOK);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Minimal Board Editor Version 0.51.14";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(16, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(236, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Copyright (C) 2007-2012 H.Okada (Suigyodo)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(88, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "EXPERIMENTAL VERSION ";
            // 
            // linkLabelSuigyodo
            // 
            this.linkLabelSuigyodo.AutoSize = true;
            this.linkLabelSuigyodo.Location = new System.Drawing.Point(88, 88);
            this.linkLabelSuigyodo.Name = "linkLabelSuigyodo";
            this.linkLabelSuigyodo.Size = new System.Drawing.Size(169, 12);
            this.linkLabelSuigyodo.TabIndex = 5;
            this.linkLabelSuigyodo.TabStop = true;
            this.linkLabelSuigyodo.Text = "http://www.suigyodo.com/online";
            this.linkLabelSuigyodo.Click += new System.EventHandler(this.LinkLabelClicked);
            // 
            // usercfgFolder
            // 
            this.usercfgFolder.Location = new System.Drawing.Point(8, 128);
            this.usercfgFolder.Name = "usercfgFolder";
            this.usercfgFolder.ReadOnly = true;
            this.usercfgFolder.Size = new System.Drawing.Size(368, 19);
            this.usercfgFolder.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "user.config file path (Read only)";
            // 
            // AboutBoxForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 156);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.usercfgFolder);
            this.Controls.Add(this.linkLabelSuigyodo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IDOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBoxForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel linkLabelSuigyodo;
        private System.Windows.Forms.TextBox usercfgFolder;
        private System.Windows.Forms.Label label4;
	}
}