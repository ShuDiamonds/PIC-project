using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	partial class SetLinePropForm : Form
	{
		public SetLinePropForm()
		{
			InitializeComponent();
		}

		public int LineWidth
		{
			get { return lineWidth; }
			set { lineWidth = value; }
		}

		public MbeObjLine.MbeLineStyle LineStyle
		{
			get { return lineStyle; }
			set { lineStyle = value; }
		}


        public Point P0
        {
            //get { return p0; }
            set { p0 = value; }
        }
        public Point P1
        {
            //get { return p1; }
            set { p1 = value; }
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

            //Bendingのタイプのボタンイメージの選択
            int imageIndex = 0;
            //右上がり?
            imageIndex |= ((p0.X < p1.X && p0.Y < p1.Y || p0.X > p1.X && p0.Y > p1.Y) ? 4 : 0);  
            //縦長?
            imageIndex |= ((Math.Abs(p0.X - p1.X) < Math.Abs(p0.Y - p1.Y)) ? 2 : 0);
            //下始点?
            imageIndex |= ((p0.Y < p1.Y) ? 1 : 0);

            radioStyleBending1.ImageIndex = (imageIndex ^ 1) + 3;
            radioStyleBending2.ImageIndex = imageIndex + 3;



			switch (lineStyle) {
				case MbeObjLine.MbeLineStyle.Bending1:
					radioStyleBending1.Checked = true;
					break;
				case MbeObjLine.MbeLineStyle.Bending2:
					radioStyleBending2.Checked = true;
					break;
				default:
					radioStyleStraight.Checked = true;
					break;
			}
#if !MONO
            Properties.Settings.Default.Reload();
#endif
            LoadMyStandard();
            buttonDelete.Enabled = false;


		}

		private int lineWidth;

        private Point p0;

        private Point p1;


		private MbeObjLine.MbeLineStyle lineStyle;



		private Color colorTextBack;

		private void OnTextWidthChanged(object sender, EventArgs e)
		{
			textWidth.BackColor = colorTextBack;
		}







        private void listBoxMyStandard_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index < 0) {
                buttonDelete.Enabled = false;
                return;
            }
            buttonDelete.Enabled = true;
            lineWidth = ((LineInfo)(listBoxMyStandard.Items[index])).Width;
            SetValueToCtrl();
        }

        private void listBoxMyStandard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DoOnOK();
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

        private void LoadMyStandard()
        {
            //MyStandard値のロード
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

            if (radioStyleBending1.Checked) {
                lineStyle = MbeObjLine.MbeLineStyle.Bending1;
            } else if (radioStyleBending2.Checked) {
                lineStyle = MbeObjLine.MbeLineStyle.Bending2;
            } else {
                lineStyle = MbeObjLine.MbeLineStyle.Straight;
            }

            SaveMyStandard();
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

	}
}