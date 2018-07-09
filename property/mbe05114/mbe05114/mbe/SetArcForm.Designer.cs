namespace mbe
{
	partial class SetArcForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textWidth = new System.Windows.Forms.TextBox();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.IDOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textRadius = new System.Windows.Forms.TextBox();
            this.textStartAngle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textEndAngle = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.listBoxMyStandard = new System.Windows.Forms.ListBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelWidthRange
            // 
            this.labelWidthRange.AutoSize = true;
            this.labelWidthRange.Location = new System.Drawing.Point(37, 53);
            this.labelWidthRange.Name = "labelWidthRange";
            this.labelWidthRange.Size = new System.Drawing.Size(33, 12);
            this.labelWidthRange.TabIndex = 6;
            this.labelWidthRange.Text = "range";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Line Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "mm";
            // 
            // textWidth
            // 
            this.textWidth.Location = new System.Drawing.Point(80, 32);
            this.textWidth.Name = "textWidth";
            this.textWidth.Size = new System.Drawing.Size(52, 19);
            this.textWidth.TabIndex = 4;
            this.textWidth.TextChanged += new System.EventHandler(this.OnTextWidthChanged);
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(239, 42);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 13;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(239, 11);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 12;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.OnOK);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Radius";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "mm";
            // 
            // textRadius
            // 
            this.textRadius.Location = new System.Drawing.Point(80, 8);
            this.textRadius.Name = "textRadius";
            this.textRadius.Size = new System.Drawing.Size(52, 19);
            this.textRadius.TabIndex = 1;
            this.textRadius.TextChanged += new System.EventHandler(this.OnTextRadiusChanged);
            // 
            // textStartAngle
            // 
            this.textStartAngle.Location = new System.Drawing.Point(88, 80);
            this.textStartAngle.Name = "textStartAngle";
            this.textStartAngle.Size = new System.Drawing.Size(52, 19);
            this.textStartAngle.TabIndex = 8;
            this.textStartAngle.TextChanged += new System.EventHandler(this.OnTextStartAngleChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "Start Angle";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(152, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "End Angle";
            // 
            // textEndAngle
            // 
            this.textEndAngle.Location = new System.Drawing.Point(224, 80);
            this.textEndAngle.Name = "textEndAngle";
            this.textEndAngle.Size = new System.Drawing.Size(52, 19);
            this.textEndAngle.TabIndex = 10;
            this.textEndAngle.TextChanged += new System.EventHandler(this.OnTextEndAngleChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDown);
            this.groupBox1.Controls.Add(this.buttonUp);
            this.groupBox1.Controls.Add(this.buttonDelete);
            this.groupBox1.Controls.Add(this.listBoxMyStandard);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Location = new System.Drawing.Point(8, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 144);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "My Standard";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(84, 110);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(70, 23);
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
            this.listBoxMyStandard.Size = new System.Drawing.Size(298, 88);
            this.listBoxMyStandard.TabIndex = 0;
            this.listBoxMyStandard.SelectedIndexChanged += new System.EventHandler(this.listBoxMyStandard_SelectedIndexChanged);
            this.listBoxMyStandard.DoubleClick += new System.EventHandler(this.listBoxMyStandard_DoubleClick);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(8, 110);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(70, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(160, 110);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(70, 23);
            this.buttonUp.TabIndex = 3;
            this.buttonUp.Text = "Up";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(236, 110);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(70, 23);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Text = "Down";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // SetArcForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(333, 261);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textEndAngle);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textStartAngle);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textRadius);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelWidthRange);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textWidth);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetArcForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Arc";
            this.Load += new System.EventHandler(this.FormLoad);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelWidthRange;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textWidth;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textRadius;
		private System.Windows.Forms.TextBox textStartAngle;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textEndAngle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ListBox listBoxMyStandard;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
	}
}