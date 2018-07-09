namespace mbe
{
    partial class SetPrintLayer
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
            this.IDOK = new System.Windows.Forms.Button();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new mbe.PrintPageLayerListTitle();
            this.ppLayerListCtrl = new mbe.PrintPageLayerListCtrl();
            this.SuspendLayout();
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(288, 176);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 0;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.IDOK_Click);
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(376, 176);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 1;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(456, 56);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(24, 109);
            this.vScrollBar1.TabIndex = 4;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Right click to add or delete a page.";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(16, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 48);
            this.panel1.TabIndex = 5;
            // 
            // ppLayerListCtrl
            // 
            this.ppLayerListCtrl.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ppLayerListCtrl.DisplayTop = 0;
            this.ppLayerListCtrl.EnablePaint = false;
            this.ppLayerListCtrl.Location = new System.Drawing.Point(16, 56);
            this.ppLayerListCtrl.Name = "ppLayerListCtrl";
            this.ppLayerListCtrl.Size = new System.Drawing.Size(440, 109);
            this.ppLayerListCtrl.TabIndex = 3;
            this.ppLayerListCtrl.PageCountChanged += new mbe.PrintPageLayerListCtrl.PageCountChangedHandler(this.ppLayerListCtrl_PageCountChanged);
            // 
            // SetPrintLayer
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(489, 212);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.ppLayerListCtrl);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetPrintLayer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Print Layer";
            this.Load += new System.EventHandler(this.SetPrintLayer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.Button IDOK;
        private System.Windows.Forms.Button IDCANCEL;
        private PrintPageLayerListCtrl ppLayerListCtrl;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private PrintPageLayerListTitle panel1;
        private System.Windows.Forms.Label label1;
    }
}