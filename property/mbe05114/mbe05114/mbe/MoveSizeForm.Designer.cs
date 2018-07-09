namespace mbe
{
    partial class MoveSizeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.IDOK = new System.Windows.Forms.Button();
            this.labelSizeRange = new System.Windows.Forms.Label();
            this.checkBoxDuplicate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y";
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(40, 8);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(64, 19);
            this.textBoxX.TabIndex = 1;
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(40, 32);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(64, 19);
            this.textBoxY.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "mm";
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(160, 40);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(67, 23);
            this.IDCANCEL.TabIndex = 9;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(160, 8);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(67, 23);
            this.IDOK.TabIndex = 8;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.IDOK_Click);
            // 
            // labelSizeRange
            // 
            this.labelSizeRange.AutoSize = true;
            this.labelSizeRange.Location = new System.Drawing.Point(48, 56);
            this.labelSizeRange.Name = "labelSizeRange";
            this.labelSizeRange.Size = new System.Drawing.Size(33, 12);
            this.labelSizeRange.TabIndex = 6;
            this.labelSizeRange.Text = "range";
            // 
            // checkBoxDuplicate
            // 
            this.checkBoxDuplicate.AutoSize = true;
            this.checkBoxDuplicate.Location = new System.Drawing.Point(40, 73);
            this.checkBoxDuplicate.Name = "checkBoxDuplicate";
            this.checkBoxDuplicate.Size = new System.Drawing.Size(72, 16);
            this.checkBoxDuplicate.TabIndex = 7;
            this.checkBoxDuplicate.Text = "Duplicate";
            this.checkBoxDuplicate.UseVisualStyleBackColor = true;
            // 
            // MoveSizeForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(238, 101);
            this.Controls.Add(this.checkBoxDuplicate);
            this.Controls.Add(this.labelSizeRange);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MoveSizeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Move size";
            this.Load += new System.EventHandler(this.MoveSizeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button IDCANCEL;
        private System.Windows.Forms.Button IDOK;
        private System.Windows.Forms.Label labelSizeRange;
        private System.Windows.Forms.CheckBox checkBoxDuplicate;
    }
}