using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class SetDrcForm : Form
	{
		//public const int MIN_CHECK_LIMIT = 10;
		//public const int MAX_CHECK_LIMIT = 200;


		public int PatternGap
		{
			get { return patternGap; }
			set
			{
				patternGap = value;
				if (patternGap < MbeGapChk.MIN_GAP) {
					patternGap = MbeGapChk.MIN_GAP;
				} else if (patternGap > MbeGapChk.MAX_GAP) {
					patternGap = MbeGapChk.MAX_GAP;
				}
			}
		}

		public int ErrorChkLimit
		{
			get { return errorChkLimit; }
			set
			{
				errorChkLimit = value;
				if (errorChkLimit < MbeDrc.MIN_CHECK_LIMIT) {
					errorChkLimit = MbeDrc.MIN_CHECK_LIMIT;
				} else if (errorChkLimit > MbeDrc.MAX_CHECK_LIMIT) {
					errorChkLimit = MbeDrc.MAX_CHECK_LIMIT;
				}
			}
		}

		public SetDrcForm()
		{
			patternGap = 2000;
			errorChkLimit = 100;
			InitializeComponent();
		}

		private int patternGap;
		private int errorChkLimit;

		private void OnOK(object sender, EventArgs e)
		{
			double d;
			int n;
			bool err = false;

			//try { d = Convert.ToDouble(textPtnGap.Text); }
			//catch (Exception) { d = 2000; }
			d = LengthString.ToDouble(textPtnGap.Text, 2000);
			n = (int)(d * 10000);
			PatternGap = n;
			if (PatternGap != n) {
				textPtnGap.BackColor = MbeColors.ColorInputErr;
				err = true;
			}

			try { n = Convert.ToInt32(textErrChkLimit.Text); }
			catch (Exception) { n = 100; }
			ErrorChkLimit = n;
			if (ErrorChkLimit != n) {
				textErrChkLimit.BackColor = MbeColors.ColorInputErr;
				err = true;
			}
			if (err) {
				return;
			}

			DialogResult = DialogResult.OK;
		}

		private void FormLoad(object sender, EventArgs e)
		{
			colorTextBack = textPtnGap.BackColor;
			labelRangeGap.Text = String.Format("({0:##0.0} - {1:##0.0}mm)", MbeGapChk.MIN_GAP / 10000.0, MbeGapChk.MAX_GAP / 10000.0);
			labelRangeLimit.Text = String.Format("({0} - {1})", MbeDrc.MIN_CHECK_LIMIT, MbeDrc.MAX_CHECK_LIMIT);
			textPtnGap.Text = String.Format("{0:##0.0###}", patternGap / 10000.0);
			textErrChkLimit.Text = Convert.ToString(errorChkLimit);
		}

		private Color colorTextBack;

		private void OnTextPtnGapChanged(object sender, EventArgs e)
		{
			textPtnGap.BackColor = colorTextBack;
		}

		private void OnTextErrChkLimit(object sender, EventArgs e)
		{
			textErrChkLimit.BackColor = colorTextBack;
		}

	}
}