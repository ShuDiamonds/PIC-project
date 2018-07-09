namespace mbe
{
	partial class SetPolygonForm
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
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.IDOK = new System.Windows.Forms.Button();
            this.textPriority = new System.Windows.Forms.TextBox();
            this.labelPriority = new System.Windows.Forms.Label();
            this.labelExpPriority = new System.Windows.Forms.Label();
            this.checkRemoveFloating = new System.Windows.Forms.CheckBox();
            this.textPtnGAP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelRangeGAP = new System.Windows.Forms.Label();
            this.labelTraceWidth = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textTraceWidth = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listBoxMyStandard = new System.Windows.Forms.ListBox();
            this.checkBoxMask = new System.Windows.Forms.CheckBox();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(240, 40);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 15;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(240, 8);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 14;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.OnOK);
            // 
            // textPriority
            // 
            this.textPriority.Location = new System.Drawing.Point(88, 104);
            this.textPriority.MaxLength = 8;
            this.textPriority.Name = "textPriority";
            this.textPriority.Size = new System.Drawing.Size(56, 19);
            this.textPriority.TabIndex = 9;
            // 
            // labelPriority
            // 
            this.labelPriority.AutoSize = true;
            this.labelPriority.Location = new System.Drawing.Point(16, 112);
            this.labelPriority.Name = "labelPriority";
            this.labelPriority.Size = new System.Drawing.Size(42, 12);
            this.labelPriority.TabIndex = 8;
            this.labelPriority.Text = "Priority";
            // 
            // labelExpPriority
            // 
            this.labelExpPriority.AutoSize = true;
            this.labelExpPriority.Location = new System.Drawing.Point(32, 128);
            this.labelExpPriority.Name = "labelExpPriority";
            this.labelExpPriority.Size = new System.Drawing.Size(178, 12);
            this.labelExpPriority.TabIndex = 11;
            this.labelExpPriority.Text = "Large number means high priority.";
            // 
            // checkRemoveFloating
            // 
            this.checkRemoveFloating.AutoSize = true;
            this.checkRemoveFloating.Location = new System.Drawing.Point(168, 104);
            this.checkRemoveFloating.Name = "checkRemoveFloating";
            this.checkRemoveFloating.Size = new System.Drawing.Size(147, 16);
            this.checkRemoveFloating.TabIndex = 10;
            this.checkRemoveFloating.Text = "Remove floating pattern";
            this.checkRemoveFloating.UseVisualStyleBackColor = true;
            // 
            // textPtnGAP
            // 
            this.textPtnGAP.Location = new System.Drawing.Point(88, 8);
            this.textPtnGAP.Name = "textPtnGAP";
            this.textPtnGAP.Size = new System.Drawing.Size(56, 19);
            this.textPtnGAP.TabIndex = 1;
            this.textPtnGAP.TextChanged += new System.EventHandler(this.OnTextPtnGapChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(152, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "GAP";
            // 
            // labelRangeGAP
            // 
            this.labelRangeGAP.AutoSize = true;
            this.labelRangeGAP.Location = new System.Drawing.Point(32, 32);
            this.labelRangeGAP.Name = "labelRangeGAP";
            this.labelRangeGAP.Size = new System.Drawing.Size(28, 12);
            this.labelRangeGAP.TabIndex = 3;
            this.labelRangeGAP.Text = "GAP";
            // 
            // labelTraceWidth
            // 
            this.labelTraceWidth.AutoSize = true;
            this.labelTraceWidth.Location = new System.Drawing.Point(32, 80);
            this.labelTraceWidth.Name = "labelTraceWidth";
            this.labelTraceWidth.Size = new System.Drawing.Size(33, 12);
            this.labelTraceWidth.TabIndex = 7;
            this.labelTraceWidth.Text = "Width";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "Trace Width";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(152, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "mm";
            // 
            // textTraceWidth
            // 
            this.textTraceWidth.Location = new System.Drawing.Point(88, 56);
            this.textTraceWidth.Name = "textTraceWidth";
            this.textTraceWidth.Size = new System.Drawing.Size(56, 19);
            this.textTraceWidth.TabIndex = 5;
            this.textTraceWidth.TextChanged += new System.EventHandler(this.OnTextTraceWidthChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDown);
            this.groupBox1.Controls.Add(this.buttonUp);
            this.groupBox1.Controls.Add(this.buttonDelete);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Controls.Add(this.listBoxMyStandard);
            this.groupBox1.Location = new System.Drawing.Point(8, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 136);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "My Standard";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(232, 45);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(232, 16);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listBoxMyStandard
            // 
            this.listBoxMyStandard.FormattingEnabled = true;
            this.listBoxMyStandard.ItemHeight = 12;
            this.listBoxMyStandard.Location = new System.Drawing.Point(8, 16);
            this.listBoxMyStandard.Name = "listBoxMyStandard";
            this.listBoxMyStandard.Size = new System.Drawing.Size(216, 112);
            this.listBoxMyStandard.TabIndex = 0;
            this.listBoxMyStandard.SelectedIndexChanged += new System.EventHandler(this.listBoxMyStandard_SelectedIndexChanged);
            this.listBoxMyStandard.DoubleClick += new System.EventHandler(this.listBoxMyStandard_DoubleClick);
            // 
            // checkBoxMask
            // 
            this.checkBoxMask.AutoSize = true;
            this.checkBoxMask.Location = new System.Drawing.Point(16, 152);
            this.checkBoxMask.Name = "checkBoxMask";
            this.checkBoxMask.Size = new System.Drawing.Size(96, 16);
            this.checkBoxMask.TabIndex = 12;
            this.checkBoxMask.Text = "Restrict mask";
            this.checkBoxMask.UseVisualStyleBackColor = true;
            this.checkBoxMask.CheckStateChanged += new System.EventHandler(this.checkBoxMask_CheckStateChanged);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(232, 74);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(75, 23);
            this.buttonUp.TabIndex = 3;
            this.buttonUp.Text = "Up";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(232, 103);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(75, 23);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Text = "Down";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // SetPolygonForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(325, 324);
            this.Controls.Add(this.checkBoxMask);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelTraceWidth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textTraceWidth);
            this.Controls.Add(this.labelRangeGAP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textPtnGAP);
            this.Controls.Add(this.checkRemoveFloating);
            this.Controls.Add(this.labelExpPriority);
            this.Controls.Add(this.labelPriority);
            this.Controls.Add(this.textPriority);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetPolygonForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Polygon";
            this.Load += new System.EventHandler(this.FormLoad);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.TextBox textPriority;
		private System.Windows.Forms.Label labelPriority;
		private System.Windows.Forms.Label labelExpPriority;
		private System.Windows.Forms.CheckBox checkRemoveFloating;
		private System.Windows.Forms.TextBox textPtnGAP;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelRangeGAP;
		private System.Windows.Forms.Label labelTraceWidth;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textTraceWidth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ListBox listBoxMyStandard;
        private System.Windows.Forms.CheckBox checkBoxMask;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
	}
}