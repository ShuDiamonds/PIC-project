namespace mbe
{
    partial class ExportFilePathForm
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
            this.labelFileType = new System.Windows.Forms.Label();
            this.textFileName = new System.Windows.Forms.TextBox();
            this.buttonRef = new System.Windows.Forms.Button();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.IDOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelFileType
            // 
            this.labelFileType.AutoSize = true;
            this.labelFileType.Location = new System.Drawing.Point(16, 8);
            this.labelFileType.Name = "labelFileType";
            this.labelFileType.Size = new System.Drawing.Size(49, 12);
            this.labelFileType.TabIndex = 2;
            this.labelFileType.Text = "FileType";
            // 
            // textFileName
            // 
            this.textFileName.Location = new System.Drawing.Point(16, 24);
            this.textFileName.Name = "textFileName";
            this.textFileName.Size = new System.Drawing.Size(296, 19);
            this.textFileName.TabIndex = 3;
            // 
            // buttonRef
            // 
            this.buttonRef.Location = new System.Drawing.Point(320, 24);
            this.buttonRef.Name = "buttonRef";
            this.buttonRef.Size = new System.Drawing.Size(75, 23);
            this.buttonRef.TabIndex = 4;
            this.buttonRef.Text = "Ref...";
            this.buttonRef.UseVisualStyleBackColor = true;
            this.buttonRef.Click += new System.EventHandler(this.buttonRef_Click);
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(320, 64);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 1;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(232, 64);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 0;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.IDOK_Click);
            // 
            // ExportFilePathForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(409, 100);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.Controls.Add(this.labelFileType);
            this.Controls.Add(this.textFileName);
            this.Controls.Add(this.buttonRef);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportFilePathForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExportFilePathForm";
            this.Load += new System.EventHandler(this.ExportFilePathForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFileType;
        private System.Windows.Forms.TextBox textFileName;
        private System.Windows.Forms.Button buttonRef;
        private System.Windows.Forms.Button IDCANCEL;
        private System.Windows.Forms.Button IDOK;
    }
}