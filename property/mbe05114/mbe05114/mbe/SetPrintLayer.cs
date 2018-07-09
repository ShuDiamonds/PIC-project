using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
    partial class SetPrintLayer : Form
    {
        public SetPrintLayer()
        {
            InitializeComponent();
        }

        private void SetPrintLayer_Load(object sender, EventArgs e)
        {
            ppLayerListCtrl.EnablePaint = true;
            //panel2.LayerMask = 0xFFFFFFFFFFFFFFFF;
            ppLayerListCtrl.infoList = PrintPageLayerInfo.LoadMyStandard(); ;
            //layerListCtrl.EnablePaint = true;
            //layerListCtrl.Size = new Size(200, 300);
            ppLayerListCtrl.InitDrawParam();
            panel1.font = ppLayerListCtrl.DiplayFont;
            panel1.TitleWidth = ppLayerListCtrl.titleWidth;
            SetVScrollParam();

            vScrollBar1.SmallChange = 1;
            vScrollBar1.LargeChange = 1;
        }

        private void IDOK_Click(object sender, EventArgs e)
        {
            ppLayerListCtrl.EditTitleEnd();
            PrintPageLayerInfo.SaveMyStandard(ppLayerListCtrl.infoList);
            DialogResult = DialogResult.OK;
        }

        //private List<PrintPageLayerInfo> infoList;

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            int pos = e.NewValue; //vScrollBar1.Value;
            System.Diagnostics.Debug.WriteLine("vScrollBar1_Scroll() :" + pos);
            ppLayerListCtrl.DisplayTop = pos;
        }

        private void ppLayerListCtrl_PageCountChanged(object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ppLayerListCtrl_PageCountChanged()");
            SetVScrollParam();
        }

        private void SetVScrollParam()
        {
            int pc = ppLayerListCtrl.PageCount;
            int lc = ppLayerListCtrl.DisplayLineCount;

            vScrollBar1.Minimum = 0;
            vScrollBar1.Maximum = (pc > lc ? pc - lc : 0);
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("SetPrintLayer  OnPaint()");
        //    base.OnPaint(e);
        //} 


    }
}
