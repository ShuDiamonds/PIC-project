using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class SetPolygonForm : Form
	{
		public SetPolygonForm()
		{
			InitializeComponent();
			priority = MbeObjPolygon.DEFAULT_PRIORITY;
			removeFloating = (MbeObjPolygon.DEFUALT_REMOVE_FLOATING ? CheckState.Checked:CheckState.Unchecked);
            restrictMask = CheckState.Unchecked;
			traceWidth = MbeObjPolygon.DEFAULT_TRACE_WIDTH;
			patternGap = MbeObjPolygon.DEFAULT_PATTERN_GAP;
            bulkMode = false;
		}

		public int Priority
		{
			get { return priority; }
			set { priority = Util.LimitRange(value,MbeObjPolygon.MIN_PRIORITY,
                                                  MbeObjPolygon.MAX_PRIORITY); }
		}

		public int TraceWidth
		{
			get { return traceWidth; }
			set { traceWidth = Util.LimitRange(value,MbeObjPolygon.MIN_TRACE_WIDTH,
													MbeObjPolygon.MAX_TRACE_WIDTH); }
		}


		public int PatternGap
		{
			get {	return patternGap; }
			set {	patternGap = value; 
					patternGap = Util.LimitRange(value,MbeObjPolygon.MIN_PATTERN_GAP,
													MbeObjPolygon.MAX_PATTERN_GAP); }
		}


        public CheckState RemoveFloating
		{
			get { return removeFloating; }
			set { removeFloating = value; }
		}

        public bool BulkMode
        {
            get { return bulkMode; }
            set { bulkMode = value; }
        }

		private void FormLoad(object sender, EventArgs e)
		{
			double min;
			double max;

			colorTextBack =  textTraceWidth.BackColor;

			min = MbeObjPolygon.MIN_TRACE_WIDTH / 10000.0F;
			max = MbeObjPolygon.MAX_TRACE_WIDTH / 10000.0F;
			this.labelTraceWidth.Text = String.Format("({0:##0.0###} - {1:##0.0###}mm)", min, max);

			min = MbeObjPolygon.MIN_PATTERN_GAP / 10000.0F;
			max = MbeObjPolygon.MAX_PATTERN_GAP / 10000.0F;
			this.labelRangeGAP.Text = String.Format("({0:##0.0###} - {1:##0.0###}mm)", min, max);

            SetValueToCtrl();
#if !MONO
            Properties.Settings.Default.Reload();
#endif
            LoadMyStandard();
            buttonDelete.Enabled = false;
            buttonUp.Enabled = false;   //20111008
            buttonDown.Enabled = false; //20111008


            if (bulkMode) {
                checkRemoveFloating.ThreeState = true;
                checkRemoveFloating.CheckState = CheckState.Indeterminate;
                checkBoxMask.ThreeState = true;
                checkBoxMask.CheckState = CheckState.Indeterminate;
                textPriority.Hide();
                labelExpPriority.Hide();
                labelPriority.Hide();
            } else {
                checkRemoveFloating.CheckState = removeFloating;
                checkBoxMask.CheckState = restrictMask;
                textPriority.Text = Convert.ToString(priority);
            }

            SetControlsEnable(restrictMask != CheckState.Checked);
		}

		private void OnOK(object sender, EventArgs e)
		{
            DoOnOK();
		}

		private int priority;
        private CheckState removeFloating;
        //private bool removeFloating;
		private int traceWidth;
		private int patternGap;
        private bool bulkMode;
        private CheckState restrictMask;

        public CheckState RestrictMask
        {
            get { return restrictMask; }
            set { restrictMask = value; }
        }

		private void OnTextPtnGapChanged(object sender, EventArgs e)
		{
			textPtnGAP.BackColor = colorTextBack;
		}

		private void OnTextTraceWidthChanged(object sender, EventArgs e)
		{
			textTraceWidth.BackColor = colorTextBack;
		}

		private Color colorTextBack;

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!CheckInputValue()) return;

            PolygonInfo newInfo = new PolygonInfo();

            newInfo.TraceWidth = traceWidth;
            newInfo.PatternGap = patternGap;

            int count = listBoxMyStandard.Items.Count;
            bool existFlag = false;
            for (int i = 0; i < count; i++) {
                if (newInfo.Equals((PolygonInfo)(listBoxMyStandard.Items[i]))) {
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

            TraceWidth = ((PolygonInfo)(listBoxMyStandard.Items[index])).TraceWidth;
            PatternGap = ((PolygonInfo)(listBoxMyStandard.Items[index])).PatternGap;
            SetValueToCtrl();
        }

        private void SetValueToCtrl()
        {
            this.textTraceWidth.Text = String.Format("{0:##0.0###}", traceWidth / 10000.0);
            this.textPtnGAP.Text = String.Format("{0:##0.0###}", patternGap / 10000.0);
        }

        private void LoadMyStandard()
        {
            //MyStandardílÇÃÉçÅ[Éh
            string strMyStdInfo = Properties.Settings.Default.MyStandardPolygonString;
            MbeMyStd[] myStdInfoArray = MbeMyStd.LoadMyStdInfoArray(strMyStdInfo);

            foreach (MbeMyStd info in myStdInfoArray) {
                listBoxMyStandard.Items.Add((PolygonInfo)info);
            }

        }

        private void SaveMyStandard()
        {
            int count = listBoxMyStandard.Items.Count;
            PolygonInfo[] myStdInfoArray = new PolygonInfo[count];
            for (int i = 0; i < count; i++) {
                myStdInfoArray[i] = (PolygonInfo)(listBoxMyStandard.Items[i]);
            }
            Properties.Settings.Default.MyStandardPolygonString = MbeMyStd.SaveMyStdInfoArray(myStdInfoArray);

        }


        private bool CheckInputValue()
        {
            int n;
            double d;
            bool err = false;

            
            d = LengthString.ToDouble(textTraceWidth.Text, MbeObjPolygon.DEFAULT_TRACE_WIDTH / 10000.0);
            n = (int)Math.Round(d * 10000);
            TraceWidth = n;
            if (TraceWidth != n) {
                textTraceWidth.BackColor = MbeColors.ColorInputErr;
                err = true;
            }


            d = LengthString.ToDouble(textPtnGAP.Text, MbeObjPolygon.DEFAULT_PATTERN_GAP / 10000.0);
            n = (int)Math.Round(d * 10000);
            PatternGap = n;
            if (PatternGap != n) {
                textPtnGAP.BackColor = MbeColors.ColorInputErr;
                err = true;
            }

            if (!bulkMode) {
                try { n = Convert.ToInt32(textPriority.Text); }
                catch (Exception) { n = MbeObjPolygon.DEFAULT_PRIORITY; }
                Priority = n;
                if (Priority != n) {
                    textPriority.BackColor = MbeColors.ColorInputErr;
                    err = true;
                }
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

            //int n;

            //try { n = Convert.ToInt32(textPriority.Text); }
            //catch (Exception) { n = MbeObjPolygon.DEFAULT_PRIORITY; }
            //Priority = n;

            removeFloating = checkRemoveFloating.CheckState;
            restrictMask = checkBoxMask.CheckState;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;

        }

        private void checkBoxMask_CheckStateChanged(object sender, EventArgs e)
        {
            bool maskEnabled = (checkBoxMask.CheckState == CheckState.Checked);
            SetControlsEnable(!maskEnabled);
            if (!maskEnabled) {
                Priority = MbeObjPolygon.DEFAULT_PRIORITY;
            } else {
                Priority = MbeObjPolygon.MAX_PRIORITY;
            }
            if(!bulkMode){
                textPriority.Text = Convert.ToString(priority);
            }
        }

        private void SetControlsEnable(bool enable)
        {
            checkRemoveFloating.Enabled = enable;
            textPtnGAP.Enabled = enable;
            textPriority.Enabled = enable;
            textTraceWidth.Enabled = enable;
            listBoxMyStandard.Enabled = enable;
            buttonAdd.Enabled = enable;
            buttonDelete.Enabled = enable;
        }

        // to change the order of my standard 20111008
        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listBoxMyStandard.SelectedIndex;
            if (index <= 0) {
                buttonUp.Enabled = false;
                return;
            }

            PolygonInfo myStdInfo = (PolygonInfo)(listBoxMyStandard.Items[index]);
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
            PolygonInfo myStdInfo = (PolygonInfo)(listBoxMyStandard.Items[index]);
            listBoxMyStandard.Items.RemoveAt(index);
            index++;
            listBoxMyStandard.Items.Insert(index, myStdInfo);
            listBoxMyStandard.SelectedIndex = index;

        }

	}
}