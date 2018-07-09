using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class AboutBoxForm : Form
	{
		public AboutBoxForm()
		{
			InitializeComponent();

            string cultureName = System.Globalization.CultureInfo.CurrentCulture.Name;
            if (cultureName != "ja-JP") {
                linkLabelSuigyodo.Text = "http://www.suigyodo.com/online/e";
            }
            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(
                    System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal);

            usercfgFolder.Text = config.FilePath;
		}

		private void OnOK(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void LinkLabelClicked(object sender, EventArgs e)
		{
            string vendorURL;
            string cultureName = System.Globalization.CultureInfo.CurrentCulture.Name;
            if (cultureName == "ja-JP") {
                vendorURL = "http://www.suigyodo.com/online";
            } else {
                vendorURL = "http://www.suigyodo.com/online/e";
            }

            //System.Diagnostics.Debug.WriteLine(System.Globalization.CultureInfo.CurrentCulture.Name);


			linkLabelSuigyodo.LinkVisited = true;
			try {
                System.Diagnostics.Process.Start(vendorURL);
			}
			catch {
			}
		}
	}
}