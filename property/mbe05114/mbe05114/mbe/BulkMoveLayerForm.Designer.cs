namespace mbe
{
	partial class BulkMoveLayerForm
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
			this.comboBoxLayer = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// IDOK
			// 
			this.IDOK.Location = new System.Drawing.Point(208, 8);
			this.IDOK.Name = "IDOK";
			this.IDOK.Size = new System.Drawing.Size(75, 23);
			this.IDOK.TabIndex = 2;
			this.IDOK.Text = "OK";
			this.IDOK.UseVisualStyleBackColor = true;
			this.IDOK.Click += new System.EventHandler(this.OnOK);
			// 
			// IDCANCEL
			// 
			this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.IDCANCEL.Location = new System.Drawing.Point(208, 40);
			this.IDCANCEL.Name = "IDCANCEL";
			this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
			this.IDCANCEL.TabIndex = 3;
			this.IDCANCEL.Text = "Cancel";
			this.IDCANCEL.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "Move to layer";
			// 
			// comboBoxLayer
			// 
			this.comboBoxLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxLayer.FormattingEnabled = true;
			this.comboBoxLayer.Location = new System.Drawing.Point(96, 16);
			this.comboBoxLayer.Name = "comboBoxLayer";
			this.comboBoxLayer.Size = new System.Drawing.Size(96, 20);
			this.comboBoxLayer.TabIndex = 1;
			// 
			// BulkMoveLayerForm
			// 
			this.AcceptButton = this.IDOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.IDCANCEL;
			this.ClientSize = new System.Drawing.Size(292, 83);
			this.Controls.Add(this.comboBoxLayer);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.IDCANCEL);
			this.Controls.Add(this.IDOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BulkMoveLayerForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Move Layer";
			this.Load += new System.EventHandler(this.FormLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button IDOK;
		private System.Windows.Forms.Button IDCANCEL;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxLayer;
	}
}