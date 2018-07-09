using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{

	/// <summary>
	/// テキストのプロパティ編集ダイアログは、通常用とバルク用が兼用しにくい
	/// </summary>
	public partial class BulkPropTextForm : Form
	{
		public BulkPropTextForm()
		{
			InitializeComponent();
			textHeight = MbeObjText.DEFAULT_TEXT_HEIGHT;
			lineWidth = MbeObjText.DEFAULT_LINE_WIDTH;
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

            SetValueToCtrl();
#if !MONO
            Properties.Settings.Default.Reload();
#endif
            LoadMyStandard();
            buttonDelete.Enabled = false;
		}


		private void OnOK(object sender, EventArgs e)
		{
            DoOnOK();
		}

		private int textHeight;
		private int lineWidth;

		private void OnTextBoxHeightChanged(object sender, EventArgs e)
		{
			textBoxHeight.BackColor = colorTextBack;
		}

		private void OnTextBoxLineWidthChanged(object sender, EventArgs e)
		{
			textBoxLineWidth.BackColor = colorTextBack;
		}

		private Color colorTextBack;

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!CheckInputValue()) return;

            TextInfo newInfo = new TextInfo();

            newInfo.LineWidth = lineWidth;
            newInfo.TextHeight = textHeight;

            int count = listBoxMyStandard.Items.Count;
            bool existFlag = false;
            for (int i = 0; i < count; i++) {
                if (newInfo.Equals((TextInfo)(listBoxMyStandard.Items[i]))) {
                    existFlag = true;
                    break;
                }
            }
            if (existFlag) return;


            int index = listBoxMyStandard.SelectedIndex;
            if (index < 0) index = 0;
            listBoxMyStandard.Items.Insert(index, newInfo);
            listBoxMyStandard.SelectedIndex = -1;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index < 0) {
                buttonDelete.Enabled = false;
                return;
            }

            listBoxMyStandard.Items.RemoveAt(index);
        }

        private void listBoxMyStandard_DoubleClick(object sender, EventArgs e)
        {
            DoOnOK();
        }

        private void listBoxMyStandard_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index < 0) {
                buttonDelete.Enabled = false;
                return;
            }
            buttonDelete.Enabled = true;
            LineWidth = ((TextInfo)(listBoxMyStandard.Items[index])).LineWidth;
            TextHeight = ((TextInfo)(listBoxMyStandard.Items[index])).TextHeight;
            SetValueToCtrl();
        }

        private void SetValueToCtrl()
        {
            textBoxLineWidth.Text = String.Format("{0:##0.0###}", lineWidth / 10000.0);
            textBoxHeight.Text = String.Format("{0:##0.0###}", textHeight / 10000.0);
        }

        private void LoadMyStandard()
        {
            //MyStandard値のロード
            string strMyStdInfo = Properties.Settings.Default.MyStandardTextString;
            MbeMyStd[] myStdInfoArray = MbeMyStd.LoadMyStdInfoArray(strMyStdInfo);

            foreach (MbeMyStd info in myStdInfoArray) {
                listBoxMyStandard.Items.Add((TextInfo)info);
            }

        }

        private void SaveMyStandard()
        {
            int count = listBoxMyStandard.Items.Count;
            TextInfo[] myStdInfoArray = new TextInfo[count];
            for (int i = 0; i < count; i++) {
                myStdInfoArray[i] = (TextInfo)(listBoxMyStandard.Items[i]);
            }
            Properties.Settings.Default.MyStandardTextString = MbeMyStd.SaveMyStdInfoArray(myStdInfoArray);

        }


        private bool CheckInputValue()
        {
            double _width;
            double _height;
            int n;
            bool err = false;



            _width = LengthString.ToDouble(textBoxLineWidth.Text, -1.0);
            _height = LengthString.ToDouble(textBoxHeight.Text, -1.0);

            n = (int)Math.Round(_width * 10000);
            LineWidth = n;
            if (LineWidth != n) {
                textBoxLineWidth.BackColor = MbeColors.ColorInputErr;
                err = true;
            }

            n = (int)Math.Round(_height * 10000);
            TextHeight = n;
            if (TextHeight != n) {
                textBoxHeight.BackColor = MbeColors.ColorInputErr;
                err = true;
            }


            if (err) {
                return false;
            }

            return true;
        }

        private void DoOnOK()
        {
            if (!CheckInputValue()) return;
            SaveMyStandard();
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }



	}
}