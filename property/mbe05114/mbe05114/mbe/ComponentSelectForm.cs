using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
     partial class ComponentSelectForm : Form
    {
        public ComponentSelectForm()
        {
            InitializeComponent();
            DisplayScale = 1000;
            selectedComponent = null;
        }

        private MbeLibs libs;
        private double displayScale;

        public double DisplayScale
        {
            get { return displayScale; }
            set { 
                displayScale = value;
                panelPreview.DisplayScale = value;
            }
        }

        private MbeObjComponent selectedComponent;

        public MbeObjComponent SelectedComponent
        {
            get { return selectedComponent; }
            //set { selectedComponent = value; }
        }

        public MbeLibs Libs
        {
            get { return libs; }
            set { libs = value; }
        }

        private void setListboxComponent()
        {
            listBoxComponent.Items.Clear();

            int libIndex = listBoxLib.SelectedIndex;
            if (libIndex < 0) return;
            MbeLib lib = (MbeLib)(listBoxLib.Items[libIndex]);
            foreach (MbeObjComponent obj in lib.componentArray) {
                listBoxComponent.Items.Add(obj);
            }
            listBoxComponent.SelectedIndex = 0;
        }

        private void listBoxLib_SelectedIndexChanged(object sender, EventArgs e)
        {
            setListboxComponent();
        }

        private void listBoxComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxComponent.Items.Count > 0 && listBoxComponent.SelectedIndex >= 0) {
                panelPreview.ComponentObj = (MbeObjComponent)(listBoxComponent.SelectedItem);
            } else {
                panelPreview.ComponentObj = null;
            }
        }

        private void ComponentSelectForm_Load(object sender, EventArgs e)
        {
            int libCount = libs.LibCount;
            if (libCount == 0) return;
            for (int i = 0; i < libCount; i++) {
                listBoxLib.Items.Add(libs.libArray[i]);
            }
            listBoxLib.SelectedIndex = 0;
            setListboxComponent();
            if (listBoxComponent.Items.Count > 0 && listBoxComponent.SelectedIndex >= 0) {
                panelPreview.ComponentObj = (MbeObjComponent)(listBoxComponent.SelectedItem);
            } else {
                panelPreview.ComponentObj = null;
            }
            panelPreview.EnablePaint = true;
            //SetPanelSize();
            //panelPreview.Refresh();

        }

        private void IDOK_Click(object sender, EventArgs e)
        {
            if (listBoxComponent.Items.Count > 0 && listBoxComponent.SelectedIndex >= 0) {
                selectedComponent = (MbeObjComponent)((MbeObjComponent)listBoxComponent.SelectedItem).Duplicate();
            } else {
                selectedComponent = null;
            }

            
            DialogResult = DialogResult.OK;
        }

    }
}
