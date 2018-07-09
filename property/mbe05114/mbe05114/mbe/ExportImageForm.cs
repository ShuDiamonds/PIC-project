using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class ExportImageForm : Form
	{
		private const int MIN_RESOLUTION = 100;
		private const int MAX_RESOLUTION = 2400;

		public ExportImageForm()
		{
			InitializeComponent();
			ExpAreaLeft = Properties.Settings.Default.ExpBmpLeft;
			ExpAreaBottom = Properties.Settings.Default.ExpBmpBottom;
			ExpAreaWidth = Properties.Settings.Default.ExpBmpWidth;
			ExpAreaHeight = Properties.Settings.Default.ExpBmpHeight;
			Resolution = Properties.Settings.Default.ExpBmpResolution;
            ColorMode = Properties.Settings.Default.ImageColorMode;
			destPath = "";
            
		}
		


		private void ExportBitmapForm_Load(object sender, EventArgs e)
		{
			colorTextBack = textLeft.BackColor;
			LabelResRange.Text = String.Format("({0} - {1}dpi)", MIN_RESOLUTION, MAX_RESOLUTION);

			textLeft.Text = String.Format("{0:##0.0###}", ExpAreaLeft / 10000.0);
			textBottom.Text = String.Format("{0:##0.0###}", ExpAreaBottom / 10000.0);
			textWidth.Text = String.Format("{0:##0.0###}", ExpAreaWidth / 10000.0);
			textHeight.Text = String.Format("{0:##0.0###}", ExpAreaHeight / 10000.0);
			textResolution.Text = resolution.ToString();
			if (destPath.Length == 0) {
				try {
					textFileName.Text = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Untitled.png");
				}
				catch {
					textFileName.Text = "";
				}
			} else {
				textFileName.Text = destPath;
			}
            checkColorMode.Checked = colorMode;
		}

		private int expAreaLeft;
		private int expAreaBottom;
		private int expAreaWidth;
		private int expAreaHeight;


		public int ExpAreaLeft
		{
			get { return expAreaLeft; }
			set { expAreaLeft = value; }
		}

		public int ExpAreaBottom
		{
			get { return expAreaBottom; }
			set { expAreaBottom = value; }
		}

		public int ExpAreaWidth
		{
			get { return expAreaWidth; }
			set { 
				expAreaWidth = value;
				if (expAreaWidth < 100000) {
					expAreaWidth = 100000;
				}
			}
		}

		public int ExpAreaHeight
		{
			get { return expAreaHeight; }
			set { 
				expAreaHeight = value;
				if (expAreaHeight < 100000) {
					expAreaHeight = 100000;
				}
			}
		}


        private bool colorMode;

        public bool ColorMode
        {
            get { return colorMode; }
            set { colorMode = value; }
        }

		private int resolution;
		public int Resolution
		{
			set
			{
				resolution = value;
				if (resolution < MIN_RESOLUTION) {
					resolution = MIN_RESOLUTION;
				} else if (resolution > MAX_RESOLUTION) {
					resolution = MAX_RESOLUTION;
				}
			}
			get
			{
				return resolution;
			}
		}

		private Color colorTextBack;

		private void textLeft_TextChanged(object sender, EventArgs e)
		{
			textLeft.BackColor = colorTextBack;
		}

		private void textWidth_TextChanged(object sender, EventArgs e)
		{
			textWidth.BackColor = colorTextBack;
		}

		private void textBottom_TextChanged(object sender, EventArgs e)
		{
			textBottom.BackColor = colorTextBack;
		}

		private void textHeight_TextChanged(object sender, EventArgs e)
		{
			textHeight.BackColor = colorTextBack;
		}

		private void textResolution_TextChanged(object sender, EventArgs e)
		{
			textResolution.BackColor = colorTextBack;
		}

		private void textFileName_TextChanged(object sender, EventArgs e)
		{
			textFileName.BackColor = colorTextBack;
		}

		
		private void IDOK_Click(object sender, EventArgs e)
		{
			bool err = false;
			double _d;
			int _n;
			
			_d = LengthString.ToDouble(textLeft.Text, -1);
			_n = (int)(_d * 10000);
			ExpAreaLeft = _n;
			if (ExpAreaLeft != _n) {
				textLeft.BackColor = MbeColors.ColorInputErr;
				err = true;
			}

			_d = LengthString.ToDouble(textBottom.Text, -1);
			_n = (int)(_d * 10000);
			ExpAreaBottom = _n;
			if (ExpAreaBottom != _n) {
				textBottom.BackColor = MbeColors.ColorInputErr;
				err = true;
			}

			_d = LengthString.ToDouble(textWidth.Text, -1);
			_n = (int)(_d * 10000);
			ExpAreaWidth = _n;
			if (ExpAreaWidth != _n) {
				textWidth.BackColor = MbeColors.ColorInputErr;
				err = true;
			}

			_d = LengthString.ToDouble(textHeight.Text, -1);
			_n = (int)(_d * 10000);
			ExpAreaHeight = _n;
			if (ExpAreaHeight != _n) {
				textHeight.BackColor = MbeColors.ColorInputErr;
				err = true;
			}

			try {
				_n = Convert.ToInt32(textResolution.Text);
			}
			catch {
				_n = 0;
			}
			Resolution = _n;
			if (Resolution != _n) {
				textResolution.BackColor = MbeColors.ColorInputErr;
				err = true;
			}

			DestPath = textFileName.Text;
			if(DestPath.Length==0){
				textFileName.BackColor = MbeColors.ColorInputErr;
				err = true;
			}

			if (err) return;

            ColorMode = checkColorMode.Checked;

			Properties.Settings.Default.ExpBmpLeft = ExpAreaLeft;
			Properties.Settings.Default.ExpBmpBottom = ExpAreaBottom;
			Properties.Settings.Default.ExpBmpWidth = ExpAreaWidth;
			Properties.Settings.Default.ExpBmpHeight = ExpAreaHeight;
			Properties.Settings.Default.ExpBmpResolution = Resolution;
            Properties.Settings.Default.ImageColorMode = ColorMode;

			DialogResult = DialogResult.OK;

		}

		private void buttonRef_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.FileName = destPath;
			saveFileDialog.Filter = "PNG file (*.png)|*.png";
			saveFileDialog.DefaultExt = "png";
			saveFileDialog.Title = "Export Image";
            saveFileDialog.OverwritePrompt = false;
			DialogResult dr= saveFileDialog.ShowDialog();
			if (dr == DialogResult.OK) {
				destPath = saveFileDialog.FileName;
				textFileName.Text = destPath;
			}
		}

		private string destPath;

		public string DestPath
		{
			get { return destPath; }
			set { destPath = value; }
		}


	}
}