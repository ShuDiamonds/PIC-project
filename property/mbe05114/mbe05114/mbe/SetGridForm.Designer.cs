namespace mbe
{
	partial class SetGridForm
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
            this.textPitch = new System.Windows.Forms.TextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spinDisplayEvery = new System.Windows.Forms.NumericUpDown();
            this.labelPitchRange = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textPitchV = new System.Windows.Forms.TextBox();
            this.listBoxMyStandard = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.spinDisplayEvery)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(216, 12);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 10;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.OnOK);
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(216, 41);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 11;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // textPitch
            // 
            this.textPitch.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textPitch.Location = new System.Drawing.Point(112, 16);
            this.textPitch.Name = "textPitch";
            this.textPitch.Size = new System.Drawing.Size(57, 19);
            this.textPitch.TabIndex = 1;
            this.textPitch.TextChanged += new System.EventHandler(this.OnTextPitchChanged);
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(168, 18);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(38, 14);
            this.labelUnit.TabIndex = 2;
            this.labelUnit.Text = "mm";
            this.labelUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Horizontal Pitch";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "&Display every";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // spinDisplayEvery
            // 
            this.spinDisplayEvery.Location = new System.Drawing.Point(112, 96);
            this.spinDisplayEvery.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinDisplayEvery.Name = "spinDisplayEvery";
            this.spinDisplayEvery.Size = new System.Drawing.Size(48, 19);
            this.spinDisplayEvery.TabIndex = 8;
            this.spinDisplayEvery.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinDisplayEvery.ValueChanged += new System.EventHandler(this.OnDispEveryValueChanged);
            // 
            // labelPitchRange
            // 
            this.labelPitchRange.AutoSize = true;
            this.labelPitchRange.Location = new System.Drawing.Point(42, 72);
            this.labelPitchRange.Name = "labelPitchRange";
            this.labelPitchRange.Size = new System.Drawing.Size(33, 12);
            this.labelPitchRange.TabIndex = 6;
            this.labelPitchRange.Text = "range";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "&Vertical Pitch";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(168, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "mm";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textPitchV
            // 
            this.textPitchV.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textPitchV.Location = new System.Drawing.Point(112, 48);
            this.textPitchV.Name = "textPitchV";
            this.textPitchV.Size = new System.Drawing.Size(57, 19);
            this.textPitchV.TabIndex = 4;
            // 
            // listBoxMyStandard
            // 
            this.listBoxMyStandard.FormattingEnabled = true;
            this.listBoxMyStandard.ItemHeight = 12;
            this.listBoxMyStandard.Location = new System.Drawing.Point(8, 16);
            this.listBoxMyStandard.Name = "listBoxMyStandard";
            this.listBoxMyStandard.Size = new System.Drawing.Size(192, 112);
            this.listBoxMyStandard.TabIndex = 0;
            this.listBoxMyStandard.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxMyStandard_MouseDoubleClick);
            this.listBoxMyStandard.SelectedIndexChanged += new System.EventHandler(this.listBoxMyStandard_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDown);
            this.groupBox1.Controls.Add(this.buttonUp);
            this.groupBox1.Controls.Add(this.buttonDelete);
            this.groupBox1.Controls.Add(this.listBoxMyStandard);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Location = new System.Drawing.Point(8, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 136);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "My Standard";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(208, 45);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(208, 16);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(208, 74);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(75, 23);
            this.buttonUp.TabIndex = 3;
            this.buttonUp.Text = "Up";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(208, 103);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(75, 23);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Text = "Down";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // SetGridForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(303, 268);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textPitchV);
            this.Controls.Add(this.labelPitchRange);
            this.Controls.Add(this.spinDisplayEvery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.textPitch);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetGridForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set GRID";
            this.Load += new System.EventHandler(this.FormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.spinDisplayEvery)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.TextBox textPitch;
		private System.Windows.Forms.Label labelUnit;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown spinDisplayEvery;
		private System.Windows.Forms.Label labelPitchRange;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textPitchV;
        private System.Windows.Forms.ListBox listBoxMyStandard;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
	}
}