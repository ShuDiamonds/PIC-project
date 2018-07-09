using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
    public partial class ExpGerberForm : Form
    {
        public ExpGerberForm()
        {
            InitializeComponent();
            selectedDir = "";
            layerMask = 0xFFFFFFFFFFFFFFFF;
        }


        public string SelectedDir
        {
            get { return selectedDir; }
            set { selectedDir = value; }
        }

        private string selectedDir;

        private ulong layerMask;

        public ulong LayerMask
        {
            get { return layerMask; }
            set { layerMask = value; }
        }

        /// <summary>
        /// レイヤー名 文字列テーブル
        /// </summary>
        private static readonly string[] nameTable = 
		{ 
            "MMC",
			"PLC",
			"STC", 
			"CMP",
            "L2",
            "L3",
			"SOL", 
			"STS", 
			"PLS",
            "MMS",
			"DIM",
			"DRL"
		};

        /// <summary>
        /// レイヤー ビット値テーブル
        /// </summary>
        private static readonly MbeLayer.LayerValue[] valueTable =
		{
            MbeLayer.LayerValue.MMC,
			MbeLayer.LayerValue.PLC,
			MbeLayer.LayerValue.STC,
			MbeLayer.LayerValue.CMP,
            MbeLayer.LayerValue.L2,
            MbeLayer.LayerValue.L3,
			MbeLayer.LayerValue.SOL,
			MbeLayer.LayerValue.STS,
			MbeLayer.LayerValue.PLS,
			MbeLayer.LayerValue.MMS,
			MbeLayer.LayerValue.DIM,
			MbeLayer.LayerValue.DRL
		};


        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = selectedDir;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                selectedDir = folderBrowserDialog1.SelectedPath;
                textBoxFolder.Text = selectedDir;
            }
        }

        private void ExpGerberForm_Load(object sender, EventArgs e)
        {
            textBoxFolder.Text = selectedDir;
            for (int i = 0; i < valueTable.Length; i++) {
                checkedListBoxLayer.Items.Add(nameTable[i], (layerMask & (ulong)(valueTable[i]))!=0);
            }
        }

        private void IDOK_Click(object sender, EventArgs e)
        {
            layerMask = 0;
            for (int i = 0; i < valueTable.Length; i++) {
                if (checkedListBoxLayer.GetItemChecked(i)) {
                    layerMask |= (ulong)(valueTable[i]);
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
