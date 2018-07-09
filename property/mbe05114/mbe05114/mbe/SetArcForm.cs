using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class SetArcForm : Form
	{
		public SetArcForm()
		{
			InitializeComponent();
		}

		public int LineWidth
		{
			get { return lineWidth; }
			set { 
				if (value < MbeObjLine.MIN_LINE_WIDTH) {
					lineWidth = MbeObjLine.MIN_LINE_WIDTH;
				} else if (value > MbeObjLine.MAX_LINE_WIDTH) {
					lineWidth = MbeObjLine.MAX_LINE_WIDTH;
				} else {
					lineWidth = value;
				}
			}
		}

		public int Radius
		{
			get {	return radius;}
			set {	if(value<=0) radius = 1000;
			else radius = value;
			}
		}

		public int StartAngle
		{
			get { return startAngle;}
			set{ startAngle = value % 3600;
				 if(startAngle<0) startAngle+=3600;
			}
		}

		public int EndAngle
		{
			get { return endAngle; }
			set{
                endAngle = value % 3600;
				 if(endAngle<0) endAngle+=3600;
			}
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
		private int radius;
		private int startAngle;
		private int endAngle;

		private Color colorTextBack;

		private void OnTextRadiusChanged(object sender, EventArgs e)
		{
			textRadius.BackColor = colorTextBack;
		}

		private void OnTextStartAngleChanged(object sender, EventArgs e)
		{
			textStartAngle.BackColor = colorTextBack;
		}

		private void OnTextEndAngleChanged(object sender, EventArgs e)
		{
			textEndAngle.BackColor = colorTextBack;
		}

		private void OnTextWidthChanged(object sender, EventArgs e)
		{
			textWidth.BackColor = colorTextBack;
		}

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!CheckInputValue()) return;

            ArcInfo newInfo = new ArcInfo();

            newInfo.Width = lineWidth;
            newInfo.StartAngle = startAngle;
            newInfo.EndAngle = endAngle;
            newInfo.Radius = radius;

            int count = listBoxMyStandard.Items.Count;
            bool existFlag = false;
            for (int i = 0; i < count; i++) {
                if (newInfo.Equals((ArcInfo)(listBoxMyStandard.Items[i]))) {
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
                buttonUp.Enabled = false;   //20111008
                buttonDown.Enabled = false; //20111008
                return;
            }
            buttonDelete.Enabled = true;

            buttonUp.Enabled = (index > 0);                                                 //20111008
            buttonDown.Enabled = ((listBoxMyStandard.Items.Count - 1) > index);             //20111008

            LineWidth = ((ArcInfo)(listBoxMyStandard.Items[index])).Width;
            StartAngle = ((ArcInfo)(listBoxMyStandard.Items[index])).StartAngle;
            EndAngle = ((ArcInfo)(listBoxMyStandard.Items[index])).EndAngle;
            Radius = ((ArcInfo)(listBoxMyStandard.Items[index])).Radius;
            SetValueToCtrl();

        }

        private void SetValueToCtrl()
        {
            textWidth.Text = String.Format("{0:##0.0###}", lineWidth / 10000.0);
            textRadius.Text = String.Format("{0:##0.0###}", radius / 10000.0);
            textStartAngle.Text = String.Format("{0:##0.0}", startAngle / 10.0);
            textEndAngle.Text = String.Format("{0:##0.0}", endAngle / 10.0);
        }

        private void LoadMyStandard()
        {
            //MyStandardílÇÃÉçÅ[Éh
            string strMyStdInfo = Properties.Settings.Default.MyStandardArcString;
            MbeMyStd[] myStdInfoArray = MbeMyStd.LoadMyStdInfoArray(strMyStdInfo);

            foreach (MbeMyStd info in myStdInfoArray) {
                listBoxMyStandard.Items.Add((ArcInfo)info);
            }

        }

        private void SaveMyStandard()
        {
            int count = listBoxMyStandard.Items.Count;
            ArcInfo[] myStdInfoArray = new ArcInfo[count];
            for (int i = 0; i < count; i++) {
                myStdInfoArray[i] = (ArcInfo)(listBoxMyStandard.Items[i]);
            }
            Properties.Settings.Default.MyStandardArcString = MbeMyStd.SaveMyStdInfoArray(myStdInfoArray);

        }



        private bool CheckInputValue()
        {
            double val;
            int nVal;
            bool err = false;
            val = LengthString.ToDouble(textWidth.Text, -1.0);
            nVal = (int)(val * 10000);
            LineWidth = nVal;
            if (LineWidth != nVal) {
                textWidth.BackColor = MbeColors.ColorInputErr;
                err = true;
            }

            val = LengthString.ToDouble(textRadius.Text, -1.0);
            nVal = (int)(val * 10000);
            Radius = nVal;
            if (Radius != nVal) {
                textRadius.BackColor = MbeColors.ColorInputErr;
                err = true;
            }


            try { val = Convert.ToDouble(textStartAngle.Text); }
            catch (Exception) { val = -1; }
            nVal = (int)(val * 10);
            StartAngle = nVal;
            if (StartAngle != nVal) {
                textStartAngle.BackColor = MbeColors.ColorInputErr;
                err = true;
            }


            try { val = Convert.ToDouble(textEndAngle.Text); }
            catch (Exception) { val = -1; }
            nVal = (int)(val * 10);
            EndAngle = nVal;
            if (EndAngle != nVal) {
                textEndAngle.BackColor = MbeColors.ColorInputErr;
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

        // to change the order of my standard 20111008
        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index <= 0) {
                buttonUp.Enabled = false;
                return;
            }

            ArcInfo myStdInfo = (ArcInfo)(listBoxMyStandard.Items[index]);
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
            ArcInfo myStdInfo = (ArcInfo)(listBoxMyStandard.Items[index]);
            listBoxMyStandard.Items.RemoveAt(index);
            index++;
            listBoxMyStandard.Items.Insert(index, myStdInfo);
            listBoxMyStandard.SelectedIndex = index;

        }

	}
}