namespace mbe
{
    partial class ComponentSelectForm
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
            this.listBoxLib = new System.Windows.Forms.ListBox();
            this.listBoxComponent = new System.Windows.Forms.ListBox();
            this.IDOK = new System.Windows.Forms.Button();
            this.IDCANCEL = new System.Windows.Forms.Button();
            this.panelPreview = new mbe.ComponentPreview();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxLib
            // 
            this.listBoxLib.FormattingEnabled = true;
            this.listBoxLib.ItemHeight = 12;
            this.listBoxLib.Location = new System.Drawing.Point(8, 24);
            this.listBoxLib.Name = "listBoxLib";
            this.listBoxLib.Size = new System.Drawing.Size(104, 256);
            this.listBoxLib.TabIndex = 0;
            this.listBoxLib.SelectedIndexChanged += new System.EventHandler(this.listBoxLib_SelectedIndexChanged);
            // 
            // listBoxComponent
            // 
            this.listBoxComponent.FormattingEnabled = true;
            this.listBoxComponent.ItemHeight = 12;
            this.listBoxComponent.Location = new System.Drawing.Point(120, 24);
            this.listBoxComponent.Name = "listBoxComponent";
            this.listBoxComponent.Size = new System.Drawing.Size(128, 256);
            this.listBoxComponent.Sorted = true;
            this.listBoxComponent.TabIndex = 1;
            this.listBoxComponent.SelectedIndexChanged += new System.EventHandler(this.listBoxComponent_SelectedIndexChanged);
            // 
            // IDOK
            // 
            this.IDOK.Location = new System.Drawing.Point(80, 288);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new System.Drawing.Size(75, 23);
            this.IDOK.TabIndex = 3;
            this.IDOK.Text = "OK";
            this.IDOK.UseVisualStyleBackColor = true;
            this.IDOK.Click += new System.EventHandler(this.IDOK_Click);
            // 
            // IDCANCEL
            // 
            this.IDCANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IDCANCEL.Location = new System.Drawing.Point(168, 288);
            this.IDCANCEL.Name = "IDCANCEL";
            this.IDCANCEL.Size = new System.Drawing.Size(75, 23);
            this.IDCANCEL.TabIndex = 4;
            this.IDCANCEL.Text = "Cancel";
            this.IDCANCEL.UseVisualStyleBackColor = true;
            // 
            // panelPreview
            // 
            this.panelPreview.AutoScroll = true;
            this.panelPreview.AutoScrollMinSize = new System.Drawing.Size(1, 1);
            this.panelPreview.ComponentObj = null;
            this.panelPreview.DisplayScale = 1000;
            this.panelPreview.EnablePaint = false;
            this.panelPreview.Location = new System.Drawing.Point(256, 24);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(272, 288);
            this.panelPreview.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Library";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(128, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Component";
            // 
            // ComponentSelectForm
            // 
            this.AcceptButton = this.IDOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IDCANCEL;
            this.ClientSize = new System.Drawing.Size(534, 321);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IDCANCEL);
            this.Controls.Add(this.IDOK);
            this.Controls.Add(this.panelPreview);
            this.Controls.Add(this.listBoxComponent);
            this.Controls.Add(this.listBoxLib);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComponentSelectForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Component";
            this.Load += new System.EventHandler(this.ComponentSelectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxLib;
        private System.Windows.Forms.ListBox listBoxComponent;
        ComponentPreview panelPreview;
        //private System.Windows.Forms.Panel panelPreview;
        private System.Windows.Forms.Button IDOK;
        private System.Windows.Forms.Button IDCANCEL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}