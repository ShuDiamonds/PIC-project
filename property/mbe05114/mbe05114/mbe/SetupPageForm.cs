using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	partial class SetupPageForm : Form
	{
		public SetupPageForm()
		{
			InitializeComponent();
			printLeftMargin = 0;
			printBottomMargin = 0;
            printHeader = false;
            headerText = "";
            print2xMode = false;
            centerPunchMode = false;
            printToolMarkMode = false;
            printToolMarkModeEnable = false;
            printColorMode = 0;
            printCurrentView = true;

		}

		private void IDOK_Click(object sender, EventArgs e)
		{
			bool err = false;
			double _d;
			int _n;
			_d = LengthString.ToDouble(textLeftMargin.Text,-1);
			_n = (int)(_d * 10000);
			PrintLeftMargin = _n;
			if (PrintLeftMargin != _n) {
				textLeftMargin.BackColor = MbeColors.ColorInputErr;
				err = true;
			}

			_d = LengthString.ToDouble(textBottomMargin.Text, -1);
			_n = (int)(_d * 10000);
			PrintBottomMargin = _n;
			if (PrintBottomMargin != _n) {
				textBottomMargin.BackColor = MbeColors.ColorInputErr;
				err = true;
			}
            print2xMode = check2xPrintMode.Checked;
            printMirror = checkMirror.Checked;
            centerPunchMode = checkCenterPunchMode.Checked;
            printToolMarkMode = checkToolMarkMode.Checked;
            printHeader = checkBoxHeader.Checked;
            headerText = textBoxHeader.Text;

            //printColorMode = (MbeView.PrintColorModeValue)(checkPrintColor.Checked ? 1 : 0);
            if (radioBW.Checked) {
                printColorMode = MbeView.PrintColorModeValue.BlackAndWhite;
            } else if (radioColVector.Checked) {
                printColorMode = MbeView.PrintColorModeValue.ColorVector;
            } else {
                printColorMode = MbeView.PrintColorModeValue.ColorBitmap;
            }

            //Properties.Settings.Default.PrintCurrentView = checkCurrentView.Checked;
            printCurrentView = checkCurrentView.Checked;
			if (!err) {
				DialogResult = DialogResult.OK;
			}
		}

		private void SetupPageForm_Load(object sender, EventArgs e)
		{
			colorTextBack = textBottomMargin.BackColor;
			textLeftMargin.Text = String.Format("{0:##0.0###}", printLeftMargin / 10000.0);
			textBottomMargin.Text = String.Format("{0:##0.0###}", printBottomMargin / 10000.0);

            checkBoxHeader.Checked = printHeader;
            textBoxHeader.Text = headerText;
            textBoxHeader.Enabled = printHeader;

            //左右反転印刷設定は、printCurrentViewのときだけ有効
            //printCurrentViewではないときは、ページ設定での左右反転が有効になる。
            if (!printCurrentView) {
                printMirror = false;
            }
            checkMirror.Checked = printMirror;
            checkMirror.Enabled = printCurrentView;

            check2xPrintMode.Checked = print2xMode;
            checkCenterPunchMode.Checked = centerPunchMode;
            checkToolMarkMode.Checked = printToolMarkModeEnable && printToolMarkMode;
            checkToolMarkMode.Enabled = printToolMarkModeEnable;
            //checkPrintColor.Checked = (printColorMode != MbeView.PrintColorModeValue.BlackAndWhite);
            switch (printColorMode) {
                case MbeView.PrintColorModeValue.BlackAndWhite:
                    radioBW.Checked = true;
                    break;
                case MbeView.PrintColorModeValue.ColorVector:
                    radioColVector.Checked = true;
                    break;
                default:
                    radioColBmp.Checked = true;
                    break;
            }

            checkCurrentView.Checked = printCurrentView;
            //checkCurrentView.Checked = Properties.Settings.Default.PrintCurrentView;
            buttonSetLayer.Enabled = !checkCurrentView.Checked;
            
		}

        private string headerText;
        private bool printHeader;
        private bool print2xMode;
        private bool centerPunchMode;
        private bool printToolMarkMode;
        private bool printToolMarkModeEnable;
        private MbeView.PrintColorModeValue printColorMode;
        private bool printMirror;

        public bool PrintMirror
        {
            get { return printMirror; }
            set { printMirror = value; }
        }

        public MbeView.PrintColorModeValue PrintColorMode
        {
            get { return printColorMode; }
            set { printColorMode = value; }
        }

        private bool printCurrentView;

        public bool PrintCurrentView
        {
            get { return printCurrentView; }
            set { printCurrentView = value; }
        }

        public bool PrintToolMarkModeEnable
        {
            get { return printToolMarkModeEnable; }
            set { printToolMarkModeEnable = value; }
        }

        public bool Print2xMode
        {
            get { return print2xMode; }
            set { print2xMode = value; }
        }


        public string HeaderText
        {
            get { return headerText; }
            set { headerText = value; }
        }

        public bool PrintHeader
        {
            get { return printHeader; }
            set { printHeader = value; }
        }

        public bool CenterPunchMode
        {
            get { return centerPunchMode; }
            set { centerPunchMode = value; }
        }

        public bool PrintToolMarkMode
        {
            get { return printToolMarkMode; }
            set { printToolMarkMode = value; }
        }

		private int printLeftMargin;
		private int printBottomMargin;
		private Color colorTextBack;
        

		public int PrintLeftMargin
		{
			get { return printLeftMargin; }
			set
			{
				printLeftMargin = value;
				if (printLeftMargin < 0) {
					printLeftMargin = 0;
				} else if (printLeftMargin > MbeView.PRINT_MARGIN_MAX) {
					printLeftMargin = MbeView.PRINT_MARGIN_MAX;
				}
			}
		}

		public int PrintBottomMargin
		{
			get { return printBottomMargin; }
			set
			{
				printBottomMargin = value;
				if (printBottomMargin < 0) {
					printBottomMargin = 0;
				} else if (printBottomMargin > MbeView.PRINT_MARGIN_MAX) {
					printBottomMargin = MbeView.PRINT_MARGIN_MAX;
				}
			}
		}

		private void textLeftMargin_TextChanged(object sender, EventArgs e)
		{
			textLeftMargin.BackColor = colorTextBack;
		}

		private void textBottomMargin_TextChanged(object sender, EventArgs e)
		{
			textBottomMargin.BackColor = colorTextBack;
		}

        private void checkCurrentView_CheckedChanged(object sender, EventArgs e)
        {
            bool _printCurrentView = checkCurrentView.Checked;

            buttonSetLayer.Enabled = !_printCurrentView;
            if (_printCurrentView) {
                checkMirror.Enabled = true;
            } else {
                checkMirror.Enabled = false;
                checkMirror.Checked = false;
            }

        }

        private void buttonSetLayer_Click(object sender, EventArgs e)
        {
            SetPrintLayer dlg = new SetPrintLayer();

            dlg.ShowDialog();
        }

        private void checkBoxHeader_CheckedChanged(object sender, EventArgs e)
        {
            printHeader = checkBoxHeader.Checked;
            textBoxHeader.Enabled = printHeader;
        }
	}
}