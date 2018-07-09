using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	partial class SetSMDPadForm : Form
	{
		public SetSMDPadForm()
		{
			InitializeComponent();
			editPinNumberMode = false;
            thermalReliefSetting = MbeObjPin.PadThermalRelief.ThmlRlfInComp;
            no_metalMask = false;
            no_resistMask = false;
		}

		public int LandWidth
		{
			get { return landWidth; }
			set { landWidth = value; }
		}

		public int LandHeight
		{
			get { return landHeight; }
			set { landHeight = value; }
		}

		public MbeObjPin.PadShape Shape
		{
			get { return shape; }
			set { shape = value; }
		}

		public string PinNumber
		{
			get { return pinNumber; }
			set { pinNumber = value; }
		}

		public bool EditPinNumberMode
		{
			get { return editPinNumberMode; }
			set { editPinNumberMode = value; }
		}

        public MbeObjPin.PadThermalRelief ThermalReliefSetting
        {
            get { return thermalReliefSetting; }
            set { thermalReliefSetting = value; }
        }

        public bool No_metalMask
        {
            get { return no_metalMask; }
            set { no_metalMask = value; }
        }

        public bool No_resistMask
        {
            get { return no_resistMask; }
            set { no_resistMask = value; }
        }

 
		private void OnOK(object sender, EventArgs e)
		{
            DoOnOK();

		}

		private void FormLoad(object sender, EventArgs e)
		{
			double min;
			double max;
			colorTextBack = textWidth.BackColor;

			min = MbeObjPin.MIN_PAD_SIZE / 10000.0F;
			max = MbeObjPin.MAX_PAD_SIZE / 10000.0F;
			textSizeRange.Text = String.Format("({0:##0.0###} - {1:##0.0###}mm)", min, max);

            SetValueToCtrl();
#if !MONO
            Properties.Settings.Default.Reload();
#endif
            LoadMyStandard();
            buttonDelete.Enabled = false;
            buttonUp.Enabled = false;   //20111008
            buttonDown.Enabled = false; //20111008
	
	
			if (editPinNumberMode) {
				textNum.Text = pinNumber;
                //checkInhibitThermalRelief.Checked = (thermalReliefSetting == MbeObjPin.PadThermalRelief.Solid);
                //checkNoResistMask.Checked = no_resistMask;
                //InhibitNoMMcheckbox(no_resistMask == true);
                //if (!no_resistMask) {
                //    checkNoMetalMask.Checked = no_metalMask;
                //}
            } else {
                //labelNum.Hide();
                //textNum.Hide();
                labelNum.Enabled = false;
                textNum.Enabled = false;
                //checkInhibitThermalRelief.Enabled = false;
                //checkNoMetalMask.Enabled = false;
                //checkNoResistMask.Enabled = false;
                //checkNoMetalMask.Checked = false;
                //checkNoResistMask.Checked = false;
            }
            checkInhibitThermalRelief.Checked = (thermalReliefSetting == MbeObjPin.PadThermalRelief.Solid);
            checkNoResistMask.Checked = no_resistMask;
            InhibitNoMMcheckbox(no_resistMask == true);
            if (!no_resistMask) {
                checkNoMetalMask.Checked = no_metalMask;
            }
        }



		private int landWidth; //ランドの幅

		private int landHeight; //ランドの高さ

		private MbeObjPin.PadShape shape; //形状

		private string pinNumber;

		private bool editPinNumberMode;

        private MbeObjPin.PadThermalRelief thermalReliefSetting;

        private bool no_metalMask;

        private bool no_resistMask;






		private void OnTextWidthChanged(object sender, EventArgs e)
		{
			textWidth.BackColor = colorTextBack;
		}

		private void OnTextHeightChanged(object sender, EventArgs e)
		{
			textHeight.BackColor = colorTextBack;
		}
		private Color colorTextBack;

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!CheckInputValue()) return;

            PadInfo newInfo = new PadInfo();
            
            newInfo.Width = landWidth;
            newInfo.Height = landHeight;
            newInfo.Shape = shape;

            int count = listBoxMyStandard.Items.Count;
            bool existFlag = false;
            for (int i = 0; i < count; i++) {
                if (newInfo.Equals((PadInfo)(listBoxMyStandard.Items[i]))) {
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

            LandWidth = ((PadInfo)(listBoxMyStandard.Items[index])).Width;
            LandHeight = ((PadInfo)(listBoxMyStandard.Items[index])).Height;
            Shape = ((PadInfo)(listBoxMyStandard.Items[index])).Shape;
            SetValueToCtrl();
        }

        private void SetValueToCtrl()
        {
            radioObround.Checked = (shape == MbeObjPin.PadShape.Obround);
            radioRectangle.Checked = (shape == MbeObjPin.PadShape.Rect);
            textWidth.Text = String.Format("{0:##0.0###}", landWidth / 10000.0);
            textHeight.Text = String.Format("{0:##0.0###}", landHeight / 10000.0);
        }

        private void LoadMyStandard()
        {
            //MyStandard値のロード
            string strMyStdInfo = Properties.Settings.Default.MyStandardPadString;
            MbeMyStd[] myStdInfoArray = MbeMyStd.LoadMyStdInfoArray(strMyStdInfo);

            foreach (MbeMyStd info in myStdInfoArray) {
                listBoxMyStandard.Items.Add((PadInfo)info);
            }

        }

        private void SaveMyStandard()
        {
            int count = listBoxMyStandard.Items.Count;
            PadInfo[] myStdInfoArray = new PadInfo[count];
            for (int i = 0; i < count; i++) {
                myStdInfoArray[i] = (PadInfo)(listBoxMyStandard.Items[i]);
            }
            Properties.Settings.Default.MyStandardPadString = MbeMyStd.SaveMyStdInfoArray(myStdInfoArray);

        }



        private bool CheckInputValue()
        {
            double _width;
            double _height;
            bool err = false;


            _width = LengthString.ToDouble(textWidth.Text, -1.0);
            _height = LengthString.ToDouble(textHeight.Text, -1.0);

            int _nWidth = (int)(_width * 10000);
            int _nHeight = (int)(_height * 10000);


            if ((_nWidth < MbeObjPin.MIN_PAD_SIZE) || (_nWidth > MbeObjPin.MAX_PAD_SIZE)) {
                textWidth.BackColor = MbeColors.ColorInputErr;
                err = true;
            } else {
                landWidth = _nWidth;
            }


            if ((_nHeight < MbeObjPin.MIN_PAD_SIZE) || (_nHeight > MbeObjPin.MAX_PAD_SIZE)) {
                textHeight.BackColor = MbeColors.ColorInputErr;
                err = true;
            } else {
                landHeight = _nHeight;
            }



            if (err) {
                return false;
            }

            shape = (radioObround.Checked ? MbeObjPin.PadShape.Obround : MbeObjPin.PadShape.Rect);
            thermalReliefSetting = (checkInhibitThermalRelief.Checked ? MbeObjPin.PadThermalRelief.Solid : MbeObjPin.PadThermalRelief.ThmlRlfInComp);
            no_metalMask = checkNoMetalMask.Checked;
            no_resistMask = checkNoResistMask.Checked;

            return true;
        }

        private void DoOnOK()
        {
            if (!CheckInputValue()) return;

            if (editPinNumberMode) {
                pinNumber = textNum.Text;
            }

            SaveMyStandard();
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void checkNoResistMask_CheckedChanged(object sender, EventArgs e)
        {
            if (checkNoResistMask.Checked) {
                InhibitNoMMcheckbox(true);
            } else {
                InhibitNoMMcheckbox(false);
            }
        }

        private void InhibitNoMMcheckbox(bool inh)
        {
            if (inh) {
                checkNoMetalMask.Enabled = false;
                checkNoMetalMask.Checked = true;
            } else {
                checkNoMetalMask.Enabled = true;
                checkNoMetalMask.Checked = false;
            }
        }

        // to change the order of my standard 20111008
        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index <= 0) {
                buttonUp.Enabled = false;
                return;
            }

            PadInfo myStdInfo = (PadInfo)(listBoxMyStandard.Items[index]);
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
            PadInfo myStdInfo = (PadInfo)(listBoxMyStandard.Items[index]);
            listBoxMyStandard.Items.RemoveAt(index);
            index++;
            listBoxMyStandard.Items.Insert(index, myStdInfo);
            listBoxMyStandard.SelectedIndex = index;


        }

    }
}