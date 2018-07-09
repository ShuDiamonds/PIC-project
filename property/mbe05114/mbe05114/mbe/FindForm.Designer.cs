namespace mbe
{
    partial class FindForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindForm));
            this.textBoxSearchWord = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderText = new System.Windows.Forms.ColumnHeader(0);
            this.columnHeaderType = new System.Windows.Forms.ColumnHeader();
            this.imageListSort = new System.Windows.Forms.ImageList(this.components);
            this.buttonSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxSearchWord
            // 
            this.textBoxSearchWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchWord.Location = new System.Drawing.Point(8, 8);
            this.textBoxSearchWord.Name = "textBoxSearchWord";
            this.textBoxSearchWord.Size = new System.Drawing.Size(155, 19);
            this.textBoxSearchWord.TabIndex = 0;
            this.textBoxSearchWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearchWord_KeyDown);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderText,
            this.columnHeaderType});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(8, 40);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(230, 112);
            this.listView1.SmallImageList = this.imageListSort;
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            // 
            // columnHeaderText
            // 
            this.columnHeaderText.Text = "Text";
            this.columnHeaderText.Width = 106;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 96;
            // 
            // imageListSort
            // 
            this.imageListSort.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSort.ImageStream")));
            this.imageListSort.TransparentColor = System.Drawing.Color.White;
            this.imageListSort.Images.SetKeyName(0, "upimage.bmp");
            this.imageListSort.Images.SetKeyName(1, "downimage.bmp");
            this.imageListSort.Images.SetKeyName(2, "noupdnimage.bmp");
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(174, 8);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(64, 23);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // FindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 157);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBoxSearchWord);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Find";
            this.Load += new System.EventHandler(this.FindForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSearchWord;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderText;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.ImageList imageListSort;
    }
}