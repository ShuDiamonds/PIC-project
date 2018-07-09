using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class SetBoardFontForm : Form
	{
		public SetBoardFontForm()
		{
			InitializeComponent();
			useEmbeddedFont = true;
			fontPath = "";
		}

		public bool useEmbeddedFont;
		public string fontPath;

		private void FormLoad(object sender, EventArgs e)
		{
			checkUseEmbFont.Checked = useEmbeddedFont;
			textFontPath.Text = fontPath;
			SetEnableFontPathCtrl();
		}

		private void OnChangeCheckUseEmbFont(object sender, EventArgs e)
		{
			SetEnableFontPathCtrl();
		}

		private void SetEnableFontPathCtrl()
		{
			bool useEmb = checkUseEmbFont.Checked;
			textFontPath.Enabled = !useEmb;
			ButtonRef.Enabled = !useEmb;
		}

		private void OnButtonRef(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.FileName = textFontPath.Text;
			openFileDialog.Filter = "MBE board font data (*.dat)|*.dat";
			openFileDialog.DefaultExt = "dat";
			DialogResult dr = openFileDialog.ShowDialog();
			if (dr == DialogResult.OK) {
				textFontPath.Text = openFileDialog.FileName;
			}
		}

		private void OnOK(object sender, EventArgs e)
		{
			useEmbeddedFont = checkUseEmbFont.Checked;
			fontPath = textFontPath.Text;
            
			DialogResult = DialogResult.OK;
		}

	}
}