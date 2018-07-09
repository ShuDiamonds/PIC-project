namespace mbe
{
	partial class SetLinePropForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetLinePropForm));
            this.IDOK = new System.Windows.Forms.Button();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textWidth = new System.Windows.Forms.TextBox();
            this.labelWidthRange = new System.Windows.Forms.Label();
            this.radioStyleStraight = new System.Windows.Forms.RadioButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.radioStyleBending1 = new System.Windows.Forms.RadioButton();
            this.radioStyleBending2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listBoxMyStandard = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(176, 8);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 7;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.OnOK);
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(176, 40);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 8;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "mm";
            // 
            // textWidth
            // 
            this.textWidth.Location = new System.Drawing.Point(64, 16);
            this.textWidth.Name = "textWidth";
            this.textWidth.Size = new System.Drawing.Size(52, 19);
            this.textWidth.TabIndex = 1;
            this.textWidth.TextChanged += new System.EventHandler(this.OnTextWidthChanged);
            // 
            // labelWidthRange
            // 
            this.labelWidthRange.AutoSize = true;
            this.labelWidthRange.Location = new System.Drawing.Point(24, 40);
            this.labelWidthRange.Name = "labelWidthRange";
            this.labelWidthRange.Size = new System.Drawing.Size(33, 12);
            this.labelWidthRange.TabIndex = 3;
            this.labelWidthRange.Text = "range";
            // 
            // radioStyleStraight
            // 
            this.radioStyleStraight.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioStyleStraight.ImageIndex = 0;
            this.radioStyleStraight.ImageList = this.imageList1;
            this.radioStyleStraight.Location = new System.Drawing.Point(16, 64);
            this.radioStyleStraight.Name = "radioStyleStraight";
            this.radioStyleStraight.Size = new System.Drawing.Size(40, 24);
            this.radioStyleStraight.TabIndex = 4;
            this.radioStyleStraight.TabStop = true;
            this.radioStyleStraight.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "imageStraightLine.bmp");
            this.imageList1.Images.SetKeyName(1, "imageBending1Line.bmp");
            this.imageList1.Images.SetKeyName(2, "imageBending2Line.bmp");
            this.imageList1.Images.SetKeyName(3, "imageBendingLine000.bmp");
            this.imageList1.Images.SetKeyName(4, "imageBendingLine001.bmp");
            this.imageList1.Images.SetKeyName(5, "imageBendingLine010.bmp");
            this.imageList1.Images.SetKeyName(6, "imageBendingLine011.bmp");
            this.imageList1.Images.SetKeyName(7, "imageBendingLine100.bmp");
            this.imageList1.Images.SetKeyName(8, "imageBendingLine101.bmp");
            this.imageList1.Images.SetKeyName(9, "imageBendingLine110.bmp");
            this.imageList1.Images.SetKeyName(10, "imageBendingLine111.bmp");

            // 
            // radioStyleBending1
            // 
            this.radioStyleBending1.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioStyleBending1.ImageIndex = 1;
            this.radioStyleBending1.ImageList = this.imageList1;
            this.radioStyleBending1.Location = new System.Drawing.Point(64, 64);
            this.radioStyleBending1.Name = "radioStyleBending1";
            this.radioStyleBending1.Size = new System.Drawing.Size(40, 24);
            this.radioStyleBending1.TabIndex = 5;
            this.radioStyleBending1.TabStop = true;
            this.radioStyleBending1.UseVisualStyleBackColor = true;
            // 
            // radioStyleBending2
            // 
            this.radioStyleBending2.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioStyleBending2.ImageIndex = 2;
            this.radioStyleBending2.ImageList = this.imageList1;
            this.radioStyleBending2.Location = new System.Drawing.Point(112, 64);
            this.radioStyleBending2.Name = "radioStyleBending2";
            this.radioStyleBending2.Size = new System.Drawing.Size(40, 24);
            this.radioStyleBending2.TabIndex = 6;
            this.radioStyleBending2.TabStop = true;
            this.radioStyleBending2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDelete);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Controls.Add(this.listBoxMyStandard);
            this.groupBox1.Location = new System.Drawing.Point(8, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 112);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "My Standard";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(168, 48);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(168, 16);
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
            this.listBoxMyStandard.Size = new System.Drawing.Size(152, 88);
            this.listBoxMyStandard.TabIndex = 0;
            this.listBoxMyStandard.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxMyStandard_MouseDoubleClick);
            this.listBoxMyStandard.SelectedIndexChanged += new System.EventHandler(this.listBoxMyStandard_SelectedIndexChanged);
            // 
            // SetLinePropForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(264, 224);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.radioStyleBending2);
            this.Controls.Add(this.radioStyleBending1);
            this.Controls.Add(this.radioStyleStraight);
            this.Controls.Add(this.labelWidthRange);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textWidth);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetLinePropForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Line";
            this.Load += new System.EventHandler(this.FormLoad);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textWidth;
		private System.Windows.Forms.Label labelWidthRange;
		private System.Windows.Forms.RadioButton radioStyleStraight;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.RadioButton radioStyleBending1;
		private System.Windows.Forms.RadioButton radioStyleBending2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ListBox listBoxMyStandard;
	}
}