using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
    public partial class ExportFilePathForm : Form
    {
        public ExportFilePathForm()
        {
            InitializeComponent();
        }

        private string dlgTitle;

        public string DlgTitle
        {
            get { return dlgTitle; }
            set { dlgTitle = value; }
        }


        private string extension;

        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        private string fileType;

        public string FileType
        {
            get { return fileType; }
            set { fileType = value; }
        }

        private string pathName;

        public string PathName
        {
            get { return pathName; }
            set { pathName = value; }
        }


        private string fileDlgFilter;

        public string FileDlgFilter
        {
            get { return fileDlgFilter; }
            set { fileDlgFilter = value; }
        }


        private void ExportFilePathForm_Load(object sender, EventArgs e)
        {
            labelFileType.Text = fileType;
            textFileName.Text = pathName;
            this.Text = dlgTitle;
            
        }



        private void buttonRef_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = textFileName.Text;
            saveFileDialog.Filter = fileDlgFilter;
            saveFileDialog.DefaultExt = extension;
            saveFileDialog.Title = dlgTitle;
            saveFileDialog.OverwritePrompt = false;
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                textFileName.Text = saveFileDialog.FileName;
            }
        }

        private void IDOK_Click(object sender, EventArgs e)
        {
             pathName = textFileName.Text;
             DialogResult = DialogResult.OK;
        }

    }
}
