namespace mbe
{
	partial class BulkPropTextForm
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
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.IDOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listBoxMyStandard = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelWidthRange
            // 
            this.labelWidthRange.AutoSize = true;
            this.labelWidthRange.Location = new System.Drawing.Point(24, 80);
            this.labelWidthRange.Name = "labelWidthRange";
            this.labelWidthRange.Size = new System.Drawing.Size(65, 12);
            this.labelWidthRange.TabIndex = 7;
            this.labelWidthRange.Text = "WidthRange";
            // 
            // labelHeightRange
            // 
            this.labelHeightRange.AutoSize = true;
            this.labelHeightRange.Location = new System.Drawing.Point(24, 32);
            this.labelHeightRange.Name = "labelHeightRange";
            this.labelHeightRange.Size = new System.Drawing.Size(70, 12);
            this.labelHeightRange.TabIndex = 3;
            this.labelHeightRange.Text = "HeightRange";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(136, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "mm";
            // 
            // textBoxLineWidth
            // 
            this.textBoxLineWidth.Location = new System.Drawing.Point(80, 56);
            this.textBoxLineWidth.Name = "textBoxLineWidth";
            this.textBoxLineWidth.Size = new System.Drawing.Size(48, 19);
            this.textBoxLineWidth.TabIndex = 5;
            this.textBoxLineWidth.TextChanged += new System.EventHandler(this.OnTextBoxLineWidthChanged);
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(80, 8);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(48, 19);
            this.textBoxHeight.TabIndex = 1;
            this.textBoxHeight.TextChanged += new System.EventHandler(this.OnTextBoxHeightChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Line Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Height";
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(192, 40);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 10;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(192, 8);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 9;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.OnOK);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDelete);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Controls.Add(this.listBoxMyStandard);
            this.groupBox1.Location = new System.Drawing.Point(8, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 112);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "My Standard";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(184, 48);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(184, 16);
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
            this.listBoxMyStandard.Size = new System.Drawing.Size(168, 88);
            this.listBoxMyStandard.TabIndex = 0;
            this.listBoxMyStandard.SelectedIndexChanged += new System.EventHandler(this.listBoxMyStandard_SelectedIndexChanged);
            this.listBoxMyStandard.DoubleClick += new System.EventHandler(this.listBoxMyStandard_DoubleClick);
            // 
            // BulkPropTextForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(279, 222);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelWidthRange);
            this.Controls.Add(this.labelHeightRange);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxLineWidth);
            this.Controls.Add(this.textBoxHeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BulkPropTextForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Text";
            this.Load += new System.EventHandler(this.FormLoad);
            this.groupBox1.ResumeLayout(false);
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
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Button IDOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ListBox listBoxMyStandard;
	}
}