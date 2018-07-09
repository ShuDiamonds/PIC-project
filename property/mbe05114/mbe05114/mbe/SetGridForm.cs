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
	/// グリッドの設定フォーム
	/// </summary>
	public partial class SetGridForm : Form
	{

        private GridInfo currentGridInfo;

        public GridInfo CurrentGridInfo
        {
            get { return currentGridInfo; }
            set { currentGridInfo = new GridInfo(value); }
        }

 		public SetGridForm()
		{
			InitializeComponent();
            currentGridInfo = new GridInfo();
		}

		private void OnOK(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("SetGridForm.OnOK");

            DoOnOK();
		}

        private void DoOnOK()
        {
            if (!SaveGridInfoSetting()) {
                return;
            }


            int count = listBoxMyStandard.Items.Count;
            GridInfo[] myStandardGridArray = new GridInfo[count];
            for (int i = 0; i < count; i++) {
                myStandardGridArray[i] = (GridInfo)(listBoxMyStandard.Items[i]);
                System.Diagnostics.Debug.WriteLine("SetGridForm-OnOK " + listBoxMyStandard.Items[i].ToString());
            }
            Properties.Settings.Default.MyStandardGridString = MbeMyStd.SaveMyStdInfoArray(myStandardGridArray);
            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
        }

        private bool SaveGridInfoSetting()
        {
            double dPitch;
            int n;
            bool err = false;

            dPitch = LengthString.ToDouble(textPitch.Text, 0.0);
            n = (int)Math.Round(dPitch * 10000);
            if (n >= GridInfo.MIN_GRID_VALUE && n <= GridInfo.MAX_GRID_VALUE) {
                currentGridInfo.Horizontal = n;
            } else {
                textPitch.BackColor = MbeColors.ColorInputErr;
                err = true;
            }


            dPitch = LengthString.ToDouble(textPitchV.Text, 0.0);
            n = (int)Math.Round(dPitch * 10000);
            if (n >= GridInfo.MIN_GRID_VALUE && n <= GridInfo.MAX_GRID_VALUE) {
                currentGridInfo.Vertical = n;
            } else {
                textPitchV.BackColor = MbeColors.ColorInputErr;
                err = true;
            }
            currentGridInfo.DisplayEvery = (int)spinDisplayEvery.Value;
            return !err;
        }



		private void FormLoad(object sender, EventArgs e)
		{
            double min = GridInfo.MIN_GRID_VALUE / 10000.0F;
            double max = GridInfo.MAX_GRID_VALUE / 10000.0F;
			colorTextBack = textPitch.BackColor;

			labelPitchRange.Text = String.Format("({0:##0.0###} - {1:##0.0###}mm)", min, max);
            spinDisplayEvery.Maximum = GridInfo.MIN_GRID_DISPLAY_EVERY;
            spinDisplayEvery.Maximum = GridInfo.MAX_GRID_DISPLAY_EVERY;

            SetCurrentGridInfoToCtrl();

            buttonDelete.Enabled = false;
            buttonUp.Enabled = false;   //20111008
            buttonDown.Enabled = false; //20111008


            //MyStandard値のロード
#if !MONO
            Properties.Settings.Default.Reload();
#endif
            string strGridInfo = Properties.Settings.Default.MyStandardGridString;
            MbeMyStd[] myStandardGridArray = MbeMyStd.LoadMyStdInfoArray(strGridInfo);

            foreach (MbeMyStd gridInfo in myStandardGridArray) {
                listBoxMyStandard.Items.Add((GridInfo)gridInfo);
            }
		}

        private void SetCurrentGridInfoToCtrl()
        {
            double dPitch;
            dPitch = currentGridInfo.Horizontal / 10000.0F;
            textPitch.Text = String.Format("{0:##0.0###}", dPitch);

            dPitch = currentGridInfo.Vertical / 10000.0F;
            textPitchV.Text = String.Format("{0:##0.0###}", dPitch);

            spinDisplayEvery.Value = currentGridInfo.DisplayEvery;
        }

		private Color colorTextBack;

		private void OnTextPitchChanged(object sender, EventArgs e)
		{
			textPitch.BackColor = colorTextBack;
		}

		private void OnDispEveryValueChanged(object sender, EventArgs e)
		{
			spinDisplayEvery.BackColor = colorTextBack;
		}

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!SaveGridInfoSetting()) return;

            int count = listBoxMyStandard.Items.Count;
            bool existFlag = false;
            for(int i=0;i<count;i++){
                if(currentGridInfo.Equals((GridInfo)(listBoxMyStandard.Items[i]))){
                    existFlag = true;
                    break;
                }
            }
            if(existFlag) return;


            int index = listBoxMyStandard.SelectedIndex;
            if(index<0)index = 0;
            GridInfo newGridInfo = new GridInfo(currentGridInfo);
            listBoxMyStandard.Items.Insert(index, newGridInfo);
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

            currentGridInfo = new GridInfo((GridInfo)(listBoxMyStandard.Items[index]));
            SetCurrentGridInfoToCtrl();
        }

        private void listBoxMyStandard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DoOnOK();
        }

        // to change the order of my standard 20111008
        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index <= 0) {
                buttonUp.Enabled = false;
                return;
            }

            GridInfo myStdInfo = (GridInfo)(listBoxMyStandard.Items[index]);
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
            GridInfo myStdInfo = (GridInfo)(listBoxMyStandard.Items[index]);
            listBoxMyStandard.Items.RemoveAt(index);
            index++;
            listBoxMyStandard.Items.Insert(index, myStdInfo);
            listBoxMyStandard.SelectedIndex = index;

        }
	}
}