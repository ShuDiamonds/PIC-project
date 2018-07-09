namespace mbe
{
	partial class SetSMDPadForm
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
            this.labelNum = new System.Windows.Forms.Label();
            this.textNum = new System.Windows.Forms.TextBox();
            this.textSizeRange = new System.Windows.Forms.Label();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.IDOK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textHeight = new System.Windows.Forms.TextBox();
            this.textWidth = new System.Windows.Forms.TextBox();
            this.radioRectangle = new System.Windows.Forms.RadioButton();
            this.radioObround = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.listBoxMyStandard = new System.Windows.Forms.ListBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.checkInhibitThermalRelief = new System.Windows.Forms.CheckBox();
            this.checkNoMetalMask = new System.Windows.Forms.CheckBox();
            this.checkNoResistMask = new System.Windows.Forms.CheckBox();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelNum
            // 
            this.labelNum.AutoSize = true;
            this.labelNum.Location = new System.Drawing.Point(168, 16);
            this.labelNum.Name = "labelNum";
            this.labelNum.Size = new System.Drawing.Size(64, 12);
            this.labelNum.TabIndex = 9;
            this.labelNum.Text = "Pin Number";
            // 
            // textNum
            // 
            this.textNum.Location = new System.Drawing.Point(168, 32);
            this.textNum.Name = "textNum";
            this.textNum.Size = new System.Drawing.Size(96, 19);
            this.textNum.TabIndex = 10;
            // 
            // textSizeRange
            // 
            this.textSizeRange.AutoSize = true;
            this.textSizeRange.Location = new System.Drawing.Point(16, 56);
            this.textSizeRange.Name = "textSizeRange";
            this.textSizeRange.Size = new System.Drawing.Size(33, 12);
            this.textSizeRange.TabIndex = 6;
            this.textSizeRange.Text = "range";
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(210, 96);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 15;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(210, 67);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 14;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.OnOK);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Height";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "mm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "mm";
            // 
            // textHeight
            // 
            this.textHeight.Location = new System.Drawing.Point(64, 32);
            this.textHeight.Name = "textHeight";
            this.textHeight.Size = new System.Drawing.Size(52, 19);
            this.textHeight.TabIndex = 4;
            this.textHeight.TextChanged += new System.EventHandler(this.OnTextHeightChanged);
            // 
            // textWidth
            // 
            this.textWidth.Location = new System.Drawing.Point(64, 8);
            this.textWidth.Name = "textWidth";
            this.textWidth.Size = new System.Drawing.Size(52, 19);
            this.textWidth.TabIndex = 1;
            this.textWidth.TextChanged += new System.EventHandler(this.OnTextWidthChanged);
            // 
            // radioRectangle
            // 
            this.radioRectangle.AutoSize = true;
            this.radioRectangle.Location = new System.Drawing.Point(16, 104);
            this.radioRectangle.Name = "radioRectangle";
            this.radioRectangle.Size = new System.Drawing.Size(90, 16);
            this.radioRectangle.TabIndex = 8;
            this.radioRectangle.TabStop = true;
            this.radioRectangle.Text = "RECTANGLE";
            this.radioRectangle.UseVisualStyleBackColor = true;
            // 
            // radioObround
            // 
            this.radioObround.AutoSize = true;
            this.radioObround.Location = new System.Drawing.Point(16, 80);
            this.radioObround.Name = "radioObround";
            this.radioObround.Size = new System.Drawing.Size(79, 16);
            this.radioObround.TabIndex = 7;
            this.radioObround.TabStop = true;
            this.radioObround.Text = "OBROUND";
            this.radioObround.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDown);
            this.groupBox1.Controls.Add(this.buttonUp);
            this.groupBox1.Controls.Add(this.buttonDelete);
            this.groupBox1.Controls.Add(this.listBoxMyStandard);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Location = new System.Drawing.Point(8, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 144);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "My Standard";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(79, 112);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(62, 23);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // listBoxMyStandard
            // 
            this.listBoxMyStandard.FormattingEnabled = true;
            this.listBoxMyStandard.ItemHeight = 12;
            this.listBoxMyStandard.Location = new System.Drawing.Point(8, 16);
            this.listBoxMyStandard.Name = "listBoxMyStandard";
            this.listBoxMyStandard.Size = new System.Drawing.Size(269, 88);
            this.listBoxMyStandard.TabIndex = 0;
            this.listBoxMyStandard.SelectedIndexChanged += new System.EventHandler(this.listBoxMyStandard_SelectedIndexChanged);
            this.listBoxMyStandard.DoubleClick += new System.EventHandler(this.listBoxMyStandard_DoubleClick);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(11, 112);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(62, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // checkInhibitThermalRelief
            // 
            this.checkInhibitThermalRelief.AutoSize = true;
            this.checkInhibitThermalRelief.Location = new System.Drawing.Point(16, 288);
            this.checkInhibitThermalRelief.Name = "checkInhibitThermalRelief";
            this.checkInhibitThermalRelief.Size = new System.Drawing.Size(194, 16);
            this.checkInhibitThermalRelief.TabIndex = 12;
            this.checkInhibitThermalRelief.Text = "Inhibit generating a thermal relief";
            this.checkInhibitThermalRelief.UseVisualStyleBackColor = true;
            // 
            // checkNoMetalMask
            // 
            this.checkNoMetalMask.AutoSize = true;
            this.checkNoMetalMask.Location = new System.Drawing.Point(16, 310);
            this.checkNoMetalMask.Name = "checkNoMetalMask";
            this.checkNoMetalMask.Size = new System.Drawing.Size(174, 16);
            this.checkNoMetalMask.TabIndex = 13;
            this.checkNoMetalMask.Text = "No perforation of metal mask";
            this.checkNoMetalMask.UseVisualStyleBackColor = true;
            // 
            // checkNoResistMask
            // 
            this.checkNoResistMask.AutoSize = true;
            this.checkNoResistMask.Location = new System.Drawing.Point(16, 332);
            this.checkNoResistMask.Name = "checkNoResistMask";
            this.checkNoResistMask.Size = new System.Drawing.Size(106, 16);
            this.checkNoResistMask.TabIndex = 16;
            this.checkNoResistMask.Text = "No Resist mask";
            this.checkNoResistMask.UseVisualStyleBackColor = true;
            this.checkNoResistMask.CheckedChanged += new System.EventHandler(this.checkNoResistMask_CheckedChanged);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(147, 112);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(62, 23);
            this.buttonUp.TabIndex = 3;
            this.buttonUp.Text = "Up";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(215, 112);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(62, 23);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Text = "Down";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // SetSMDPadForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(298, 354);
            this.Controls.Add(this.checkNoResistMask);
            this.Controls.Add(this.checkNoMetalMask);
            this.Controls.Add(this.checkInhibitThermalRelief);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelNum);
            this.Controls.Add(this.textNum);
            this.Controls.Add(this.textSizeRange);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textHeight);
            this.Controls.Add(this.textWidth);
            this.Controls.Add(this.radioRectangle);
            this.Controls.Add(this.radioObround);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetSMDPadForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Pad/Flash";
            this.Load += new System.EventHandler(this.FormLoad);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelNum;
		private System.Windows.Forms.TextBox textNum;
		private System.Windows.Forms.Label textSizeRange;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textHeight;
		private System.Windows.Forms.TextBox textWidth;
		private System.Windows.Forms.RadioButton radioRectangle;
		private System.Windows.Forms.RadioButton radioObround;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ListBox listBoxMyStandard;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.CheckBox checkInhibitThermalRelief;
        private System.Windows.Forms.CheckBox checkNoMetalMask;
        private System.Windows.Forms.CheckBox checkNoResistMask;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;

	}
}