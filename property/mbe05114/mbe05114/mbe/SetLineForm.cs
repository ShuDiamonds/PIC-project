using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class SetLineForm : Form
	{
		public SetLineForm()
		{
			InitializeComponent();
		}

		public int LineWidth
		{
			get { return lineWidth; }
			set { lineWidth = value; }
		}

		private void OnOK(object sender, EventArgs e)
		{
            DoOnOK();
		}


		private void FormLoad(object sender, EventArgs e)
		{
			double min;
			double max;
			min = MbeObjLine.MIN_LINE_WIDTH / 10000.0F;
			max = MbeObjLine.MAX_LINE_WIDTH / 10000.0F;
			colorTextBack = textWidth.BackColor;

			labelWidthRange.Text = String.Format("({0:##0.0###} - {1:##0.0###}mm)", min, max);
            SetValueToCtrl();
#if !MONO
            Properties.Settings.Default.Reload();
#endif
            LoadMyStandard();
            buttonDelete.Enabled = false;
            buttonUp.Enabled = false;   //20111008
            buttonDown.Enabled = false; //20111008
		}

		private int lineWidth;
		private Color colorTextBack;

		private void OnTextWidthChanged(object sender, EventArgs e)
		{
			textWidth.BackColor = colorTextBack;
		}




        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!CheckInputValue()) return;

            LineInfo newInfo = new LineInfo();
            newInfo.Width = lineWidth;

            int count = listBoxMyStandard.Items.Count;
            bool existFlag = false;
            for (int i = 0; i < count; i++) {
                if (newInfo.Equals((LineInfo)(listBoxMyStandard.Items[i]))) {
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

        private void listBoxMyStandard_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index < 0) {
                buttonDelete.Enabled = false;
                buttonUp.Enabled = false;   //20111008
                buttonDown.Enabled = false; //20111008
                return;
            }
            buttonDelete.Enabled = true;
            buttonUp.Enabled = (index > 0);                                                 //20111008
            buttonDown.Enabled = ((listBoxMyStandard.Items.Count - 1) > index);             //20111008
            lineWidth = ((LineInfo)(listBoxMyStandard.Items[index])).Width;
            SetValueToCtrl();
        }

        private void listBoxMyStandard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DoOnOK();
        }

        private void LoadMyStandard()
        {
            //MyStandardílÇÃÉçÅ[Éh
            string strMyStdInfo = Properties.Settings.Default.MyStandardLineString;
            MbeMyStd[] myStdInfoArray = MbeMyStd.LoadMyStdInfoArray(strMyStdInfo);

            foreach (MbeMyStd info in myStdInfoArray) {
                listBoxMyStandard.Items.Add((LineInfo)info);
            }

        }

        private void SaveMyStandard()
        {
            int count = listBoxMyStandard.Items.Count;
            LineInfo[] myStdInfoArray = new LineInfo[count];
            for (int i = 0; i < count; i++) {
                myStdInfoArray[i] = (LineInfo)(listBoxMyStandard.Items[i]);
            }
            Properties.Settings.Default.MyStandardLineString = MbeMyStd.SaveMyStdInfoArray(myStdInfoArray);

        }


        private void SetValueToCtrl()
        {
            textWidth.Text = String.Format("{0:##0.0###}", lineWidth / 10000.0);
        }

        private bool CheckInputValue()
        {
            double _width;

            _width = LengthString.ToDouble(textWidth.Text, MbeObjLine.DEFAULT_LINE_WIDTH / 10000.0);

            int _nWidth = (int)(_width * 10000);

            if ((_nWidth < MbeObjLine.MIN_LINE_WIDTH) || (_nWidth > MbeObjLine.MAX_LINE_WIDTH)) {
                textWidth.BackColor = MbeColors.ColorInputErr;
                return false;
            }

            lineWidth = _nWidth;
            return true;
        }

        private void DoOnOK()
        {
            if (!CheckInputValue()) return;

            SaveMyStandard();
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        // to change the order of my standard 20111008
        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index <= 0) {
                buttonUp.Enabled = false;
                return;
            }

            LineInfo myStdInfo = (LineInfo)(listBoxMyStandard.Items[index]);
            listBoxMyStandard.Items.RemoveAt(index);
            index--;
            listBoxMyStandard.Items.Insert(index, myStdInfo);
            listBoxMyStandard.SelectedIndex = index;
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if ((listBoxMyStandard.Items.Count - 1) <= index) {
                buttonDown.Enabled = false;
                return;
            }
            LineInfo myStdInfo = (LineInfo)(listBoxMyStandard.Items[index]);
            listBoxMyStandard.Items.RemoveAt(index);
            index++;
            listBoxMyStandard.Items.Insert(index, myStdInfo);
            listBoxMyStandard.SelectedIndex = index;

        }



	}
}