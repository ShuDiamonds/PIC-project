using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
    public partial class SetLibForm : Form
    {
        public SetLibForm()
        {
            InitializeComponent();
            //libPathNameArray = null;
        }

        private void SetLibForm_Load(object sender, EventArgs e)
        {
            LoadMyStandard();
            SetButtonState();
        }

        //private string[] libPathNameArray;

        //public string[] LibPathNameArray
        //{
        //    get { return libPathNameArray; }
        //    //set { libPathNameArray = value; }
        //}

        private void LoadMyStandard()
        {
            //MyStandard値のロード
            string strMyStdInfo = Properties.Settings.Default.MyStandardLibString;
            MbeMyStd[] myStdInfoArray = MbeMyStd.LoadMyStdInfoArray(strMyStdInfo);

            foreach (MbeMyStd info in myStdInfoArray) {
                listBoxLib.Items.Add(((LibInfo)info).LibPath);
            }

        }

        private void SaveMyStandard()
        {
            int count = listBoxLib.Items.Count;
            //libPathNameArray = new string[count];
            LibInfo[] myStdInfoArray = new LibInfo[count];
            for (int i = 0; i < count; i++) {
                LibInfo info = new LibInfo();
                info.LibPath = listBoxLib.Items[i].ToString();
                //libPathNameArray[i] = pathname;
                //info.LibPath = pathname;
                myStdInfoArray[i] = info;
            }
            Properties.Settings.Default.MyStandardLibString = MbeMyStd.SaveMyStdInfoArray(myStdInfoArray);

        }

        private void IDOK_Click(object sender, EventArgs e)
        {
            SaveMyStandard();
            Properties.Settings.Default.Save(); //2009/10/19
            DialogResult = DialogResult.OK;
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = listBoxLib.SelectedIndex;
            if (index < 1) {
                return;
            }
            string filename =(string) listBoxLib.Items[index];
            listBoxLib.Items.RemoveAt(index);
            index--;
            listBoxLib.Items.Insert(index, filename);
            listBoxLib.SelectedIndex = index;
            SetButtonState();
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int count = listBoxLib.Items.Count;
            int index = listBoxLib.SelectedIndex;
            if (index+1 >= count || index<0) {
                return;
            }
            string filename = (string)listBoxLib.Items[index];
            listBoxLib.Items.RemoveAt(index);
            index++;
            listBoxLib.Items.Insert(index, filename);
            listBoxLib.SelectedIndex = index;
            SetButtonState();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                string[] filenameArray = openFileDialog.FileNames;
                if (filenameArray.Length < 1) return;
                int index = listBoxLib.SelectedIndex;
                if (index < 0) {
                    index = 0;
                }
                for (int i = 0; i < filenameArray.Length; i++) {
                    listBoxLib.Items.Insert(index,filenameArray[i]);
                }
                listBoxLib.SelectedIndex = index;
                SetButtonState();
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int index  = listBoxLib.SelectedIndex;
            if (index < 0) {
                return;
            }
            listBoxLib.Items.RemoveAt(index);
            if (listBoxLib.Items.Count <= index) {
                index = listBoxLib.Items.Count - 1;
            }
            if (index >= 0) {
                listBoxLib.SelectedIndex = index;
            }
            SetButtonState();
        }

        private void listBoxLib_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButtonState();
        }

        private void SetButtonState()
        {
            int index = listBoxLib.SelectedIndex;
            int count = listBoxLib.Items.Count;
            buttonRemove.Enabled = (index >= 0);
            buttonDown.Enabled = (index + 1 < count && index >= 0);
            buttonUp.Enabled = (index > 0);
        }

        private void IDCANCEL_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
