using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class SetHoleForm : Form
	{
		public int Drill
		{
			get { return drill; }
			set { drill = value; }
		}

		public SetHoleForm()
		{
			InitializeComponent();
		}

		private void FormLoad(object sender, EventArgs e)
		{
			double min = MbeObjHole.MIN_DIA / 10000.0F;
			double max = MbeObjHole.MAX_DIA / 10000.0F;
			textBack = textDrill.BackColor;

			textDrillRange.Text = String.Format("({0:##0.0###} - {1:##0.0###}mm)", min, max);
            SetValueToCtrl();
#if !MONO
            Properties.Settings.Default.Reload();
#endif
            LoadMyStandard();
            buttonDelete.Enabled = false;
            buttonUp.Enabled = false;   //20111008
            buttonDown.Enabled = false; //20111008
		}

		private int drill;//ドリル径

		private void OnOK(object sender, EventArgs e)
		{
            DoOnOK();

		}

        private bool CheckInputValue()
        {
            double _drill;

            _drill = LengthString.ToDouble(textDrill.Text, -1.0);

            int _nDrill = (int)(_drill * 10000);


            //入力値の範囲チェック
            if ((_nDrill < MbeObjHole.MIN_DIA) || (_nDrill > MbeObjHole.MAX_DIA)) {
                textDrill.BackColor = MbeColors.ColorInputErr;
                return false;
            }

            drill = _nDrill;
            return true;
        }

        private void DoOnOK()
        {
            if (!CheckInputValue()) return;
            SaveMyStandard();
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

		private void OnTextDrillChanged(object sender, EventArgs e)
		{
			textDrill.BackColor = textBack;
		}

		private Color textBack;


        private void SetValueToCtrl()
        {
            textDrill.Text = String.Format("{0:##0.0###}", drill / 10000.0);
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!CheckInputValue()) return;

            HoleInfo newInfo = new HoleInfo();
            newInfo.Dia = drill;

            int count = listBoxMyStandard.Items.Count;
            bool existFlag = false;
            for (int i = 0; i < count; i++) {
                if (newInfo.Equals((HoleInfo)(listBoxMyStandard.Items[i]))) {
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
            

            drill = ((HoleInfo)(listBoxMyStandard.Items[index])).Dia;
            SetValueToCtrl();
        }

        private void LoadMyStandard()
        {
            //MyStandard値のロード
            string strMyStdInfo = Properties.Settings.Default.MyStandardHoleString;
            MbeMyStd[] myStdInfoArray = MbeMyStd.LoadMyStdInfoArray(strMyStdInfo);

            foreach (MbeMyStd info in myStdInfoArray) {
                listBoxMyStandard.Items.Add((HoleInfo)info);
            }

        }

        private void SaveMyStandard()
        {
            int count = listBoxMyStandard.Items.Count;
            HoleInfo[] myStdInfoArray = new HoleInfo[count];
            for (int i = 0; i < count; i++) {
                myStdInfoArray[i] = (HoleInfo)(listBoxMyStandard.Items[i]);
            }
            Properties.Settings.Default.MyStandardHoleString = MbeMyStd.SaveMyStdInfoArray(myStdInfoArray);

        }


        // to change the order of my standard 20111008
        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index <= 0) {
                buttonUp.Enabled = false;
                return;
            }

            HoleInfo myStdInfo = (HoleInfo)(listBoxMyStandard.Items[index]);
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
            HoleInfo myStdInfo = (HoleInfo)(listBoxMyStandard.Items[index]);
            listBoxMyStandard.Items.RemoveAt(index);
            index++;
            listBoxMyStandard.Items.Insert(index, myStdInfo);
            listBoxMyStandard.SelectedIndex = index;

        }
		
	}
}