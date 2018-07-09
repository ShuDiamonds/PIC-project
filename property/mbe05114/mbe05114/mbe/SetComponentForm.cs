using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class SetComponentForm : Form
	{
		public SetComponentForm()
		{
			InitializeComponent();
			componentName = "";
			refNumString ="";
			remarksText ="";
			textHeight = MbeObjText.DEFAULT_TEXT_HEIGHT;
			lineWidth = MbeObjText.DEFAULT_LINE_WIDTH;
			drawRefOnDoc = false;
		}

		public string ComponentName
		{
			get { return componentName; }
			set { componentName = value; }
		}

		public string RefNumString
		{
			get { return refNumString; }
			set { refNumString = value; }
		}

        public string PackageName
        {
            get { return packageName; }
            set { packageName = value; }
        }

		public string RemarksText
		{
			get { return remarksText; }
			set { remarksText = value; }
		}

		public bool DrawRefOnDoc
		{
			get { return drawRefOnDoc; }
			set { drawRefOnDoc = value; }
		}

		public int TextHeight
		{
			get { return textHeight; }
			set
			{
				textHeight = value;
				if (textHeight < MbeObjText.MIN_TEXT_HEIGHT) {
					textHeight = MbeObjText.MIN_TEXT_HEIGHT;
				} else if (textHeight > MbeObjText.MAX_TEXT_HEIGHT) {
					textHeight = MbeObjText.MAX_TEXT_HEIGHT;
				}
			}
		}

		public int LineWidth
		{
			get { return lineWidth; }
			set
			{
				lineWidth = value;
				if (lineWidth < MbeObjText.MIN_LINE_WIDTH) {
					lineWidth = MbeObjText.MIN_LINE_WIDTH;
				} else if (lineWidth > MbeObjText.MAX_LINE_WIDTH) {
					lineWidth = MbeObjText.MAX_LINE_WIDTH;
				}
			}
		}

        public int Anglex10
        {
            get { return anglex10; }
            set { anglex10 = value; }
        }


		private string componentName;
		private string refNumString;
        private string packageName;
		private string remarksText;
		private int textHeight;
		private int lineWidth;
		private bool drawRefOnDoc;
        private int anglex10;


		private void FormLoad(object sender, EventArgs e)
		{
			double min;
			double max;
			colorTextBack = textBoxHeight.BackColor;
			min = MbeObjText.MIN_LINE_WIDTH / 10000.0F;
			max = MbeObjText.MAX_LINE_WIDTH / 10000.0F;
			labelWidthRange.Text = String.Format("({0:##0.0###} - {1:##0.0###}mm)", min, max);
			min = MbeObjText.MIN_TEXT_HEIGHT / 10000.0F;
			max = MbeObjText.MAX_TEXT_HEIGHT / 10000.0F;
			labelHeightRange.Text = String.Format("({0:##0.0###} - {1:##0.0###}mm)", min, max);

			textBoxLineWidth.Text = String.Format("{0:##0.0###}", lineWidth / 10000.0);
			textBoxHeight.Text = String.Format("{0:##0.0###}", textHeight / 10000.0);

			textBoxName.Text = componentName;
            textBoxPackage.Text = packageName;
			textBoxRefNum.Text = refNumString;
			textBoxRemarks.Text = remarksText;

            checkBox_unlock_edit_angle.Checked = false;
            textBox_angle.Enabled = false;

            if (anglex10 < 0) {
                textBox_angle.Text = "Undefined";
            } else {
                textBox_angle.Text = String.Format("{0:##0.0}", anglex10 / 10.0);
            }

			checkBoxDrawOnDoc.Checked = drawRefOnDoc;
		}

		private void OnOK(object sender, EventArgs e)
		{
			double _width;
			double _height;
            double _angle;
			int nVal;
			bool err = false;

			//try { _width = Convert.ToDouble(textBoxLineWidth.Text); }
			//catch (Exception) { _width = -1; }
			//try { _height = Convert.ToDouble(textBoxHeight.Text); }
			//catch (Exception) { _height = -1; }

			_width = LengthString.ToDouble(textBoxLineWidth.Text, -1.0);
			nVal = (int)(_width * 10000);
			LineWidth = nVal;
			if (LineWidth != nVal) {
				textBoxLineWidth.BackColor = MbeColors.ColorInputErr;
				err = true;
			}
			
			_height = LengthString.ToDouble(textBoxHeight.Text, -1.0);
			nVal = (int)(_height * 10000);
			TextHeight = nVal;
			if (TextHeight != nVal) {
				textBoxHeight.BackColor = MbeColors.ColorInputErr;
				err = true;
			}

            try {
                if (textBox_angle.Text == "Undefined") {
                    Anglex10 = -1;
                } else {
                    _angle = Convert.ToDouble(textBox_angle.Text);
                    nVal = (int)(_angle * 10);
                    Anglex10 = nVal;
                    if (Anglex10 != nVal) {
                        textBox_angle.BackColor = MbeColors.ColorInputErr;
                        err = true;
                    }
                }

            }
            catch{
                textBox_angle.BackColor = MbeColors.ColorInputErr;
                err = true;
            }

			if (err) {
				return;
			}

			componentName = textBoxName.Text;
			refNumString = textBoxRefNum.Text;
            packageName = textBoxPackage.Text;
			remarksText = textBoxRemarks.Text;
			drawRefOnDoc = checkBoxDrawOnDoc.Checked;
			DialogResult = DialogResult.OK;
		}

		private void OnTextBoxHeightChanged(object sender, EventArgs e)
		{
			textBoxHeight.BackColor = colorTextBack;
		}

		private void OnTextBoxLineWidthChanged(object sender, EventArgs e)
		{
			textBoxLineWidth.BackColor = colorTextBack;
		}
		private Color colorTextBack;

        private void checkBox_unlock_edit_angle_CheckedChanged(object sender, EventArgs e)
        {
            textBox_angle.Enabled = checkBox_unlock_edit_angle.Checked;
        }

        private void textBox_angle_TextChanged(object sender, EventArgs e)
        {
            textBox_angle.BackColor = colorTextBack;
        }

	}
}