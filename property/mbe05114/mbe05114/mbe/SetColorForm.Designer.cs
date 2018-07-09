namespace mbe
{
	partial class SetColorForm
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
            this.listBoxColorDisplay = new System.Windows.Forms.ListBox();
            this.buttonDefaultDisplay = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabTarget = new System.Windows.Forms.TabControl();
            this.tabDisplay = new System.Windows.Forms.TabPage();
            this.tabPrint = new System.Windows.Forms.TabPage();
            this.listBoxColorPrint = new System.Windows.Forms.ListBox();
            this.buttonDefaultPrint = new System.Windows.Forms.Button();
            this.tabTarget.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.tabPrint.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxColorDisplay
            // 
            this.listBoxColorDisplay.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxColorDisplay.FormattingEnabled = true;
            this.listBoxColorDisplay.ItemHeight = 16;
            this.listBoxColorDisplay.Location = new System.Drawing.Point(8, 8);
            this.listBoxColorDisplay.Name = "listBoxColorDisplay";
            this.listBoxColorDisplay.Size = new System.Drawing.Size(184, 132);
            this.listBoxColorDisplay.TabIndex = 0;
            this.listBoxColorDisplay.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawItem);
            this.listBoxColorDisplay.Click += new System.EventHandler(this.OnClickListBoxColor);
            // 
            // buttonDefaultDisplay
            // 
            this.buttonDefaultDisplay.Location = new System.Drawing.Point(8, 144);
            this.buttonDefaultDisplay.Name = "buttonDefaultDisplay";
            this.buttonDefaultDisplay.Size = new System.Drawing.Size(75, 23);
            this.buttonDefaultDisplay.TabIndex = 1;
            this.buttonDefaultDisplay.Text = "Default";
            this.buttonDefaultDisplay.UseVisualStyleBackColor = true;
            this.buttonDefaultDisplay.Click += new System.EventHandler(this.ObButtonDefault);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(144, 216);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.OnButtonClose);
            // 
            // tabTarget
            // 
            this.tabTarget.Controls.Add(this.tabDisplay);
            this.tabTarget.Controls.Add(this.tabPrint);
            this.tabTarget.Location = new System.Drawing.Point(8, 8);
            this.tabTarget.Name = "tabTarget";
            this.tabTarget.SelectedIndex = 0;
            this.tabTarget.Size = new System.Drawing.Size(208, 200);
            this.tabTarget.TabIndex = 0;
            // 
            // tabDisplay
            // 
            this.tabDisplay.Controls.Add(this.listBoxColorDisplay);
            this.tabDisplay.Controls.Add(this.buttonDefaultDisplay);
            this.tabDisplay.Location = new System.Drawing.Point(4, 22);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplay.Size = new System.Drawing.Size(200, 174);
            this.tabDisplay.TabIndex = 0;
            this.tabDisplay.Text = "Display";
            this.tabDisplay.UseVisualStyleBackColor = true;
            // 
            // tabPrint
            // 
            this.tabPrint.Controls.Add(this.listBoxColorPrint);
            this.tabPrint.Controls.Add(this.buttonDefaultPrint);
            this.tabPrint.Location = new System.Drawing.Point(4, 22);
            this.tabPrint.Name = "tabPrint";
            this.tabPrint.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrint.Size = new System.Drawing.Size(200, 174);
            this.tabPrint.TabIndex = 1;
            this.tabPrint.Text = "Print/Export Image";
            this.tabPrint.UseVisualStyleBackColor = true;
            // 
            // listBoxColorPrint
            // 
            this.listBoxColorPrint.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxColorPrint.FormattingEnabled = true;
            this.listBoxColorPrint.ItemHeight = 16;
            this.listBoxColorPrint.Location = new System.Drawing.Point(8, 8);
            this.listBoxColorPrint.Name = "listBoxColorPrint";
            this.listBoxColorPrint.Size = new System.Drawing.Size(184, 132);
            this.listBoxColorPrint.TabIndex = 0;
            this.listBoxColorPrint.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawItemPrintColor);
            this.listBoxColorPrint.Click += new System.EventHandler(this.listBoxColorPrint_Click);
            // 
            // buttonDefaultPrint
            // 
            this.buttonDefaultPrint.Location = new System.Drawing.Point(8, 144);
            this.buttonDefaultPrint.Name = "buttonDefaultPrint";
            this.buttonDefaultPrint.Size = new System.Drawing.Size(75, 23);
            this.buttonDefaultPrint.TabIndex = 1;
            this.buttonDefaultPrint.Text = "Default";
            this.buttonDefaultPrint.UseVisualStyleBackColor = true;
            this.buttonDefaultPrint.Click += new System.EventHandler(this.buttonDefaultPrint_Click);
            // 
            // SetColorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 247);
            this.Controls.Add(this.tabTarget);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetColorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Color";
            this.Load += new System.EventHandler(this.FormLoad);
            this.tabTarget.ResumeLayout(false);
            this.tabDisplay.ResumeLayout(false);
            this.tabPrint.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listBoxColorDisplay;
		private System.Windows.Forms.Button buttonDefaultDisplay;
		private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TabControl tabTarget;
        private System.Windows.Forms.TabPage tabDisplay;
        private System.Windows.Forms.TabPage tabPrint;
        private System.Windows.Forms.Button buttonDefaultPrint;
        private System.Windows.Forms.ListBox listBoxColorPrint;
	}
}