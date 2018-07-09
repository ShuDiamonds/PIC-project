using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class BulkMoveLayerForm : Form
	{
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
			MbeLayer.LayerValue.DOC
		};


        private MbeLayer.LayerValue[] registeredLayerValue;
        
		public BulkMoveLayerForm()
		{
			InitializeComponent();
            registeredLayerValue = new MbeLayer.LayerValue[valueTable.Length];
		}

		public MbeLayer.LayerValue layer;
        public ulong selectableLayer;

		private void FormLoad(object sender, EventArgs e)
		{
			int selIndex = -1;
			int count = valueTable.Length;
            int registerdIndex = 0;
			for (int i = 0; i < count; i++) {
                if (((ulong)(valueTable[i]) & selectableLayer) == 0) {
                    continue;
                }
                if (valueTable[i] == layer) selIndex = registerdIndex;
				string layerName = MbeLayer.GetLayerName(valueTable[i]);
				comboBoxLayer.Items.Add(layerName);
                registeredLayerValue[registerdIndex] = valueTable[i];
                registerdIndex++;
			}
			if (selIndex == -1) {
				selIndex = 0;
			}
			comboBoxLayer.SelectedIndex = selIndex;
		}

		private void OnOK(object sender, EventArgs e)
		{
			//layer = valueTable[comboBoxLayer.SelectedIndex];
            layer = registeredLayerValue[comboBoxLayer.SelectedIndex];
            DialogResult = DialogResult.OK;
		} 
	}
}