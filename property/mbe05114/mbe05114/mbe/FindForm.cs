using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
    partial class FindForm : Form
    {
        public FindForm()
        {
            InitializeComponent();
            mbeView = null;
            sortUp = true;
            sortColumn = 0;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            doSearch();
        }




        private void doSearch()
        {
            listView1.Items.Clear();
            listView1.Sorting = SortOrder.None;

            string searchWord = textBoxSearchWord.Text.ToUpper();

            ulong visibleLayer = mbeView.GetVisibleLayer();

            if (searchWord.Length == 0 ){
                return;
            }
 
            //LinkedList<MbeObj> mainList = mbeView.Document.MainList;
            SearchFromList(searchWord, mbeView.Document.MainList, visibleLayer);
            SearchFromList(searchWord, mbeView.Document.TempList, visibleLayer);
            listView1.ListViewItemSorter = new ListViewItemComparer(sortColumn, sortUp);

        }

        private void SearchFromList(string searchWord, LinkedList<MbeObj> mbeDataList, ulong visibleLayer)
        {
            if (mbeDataList == null || mbeDataList.Count == 0) return;

            foreach (MbeObj obj in mbeDataList) {

                if (obj.DeleteCount < 0) {
                    if (obj.Id() == MbeObjID.MbeText) {
                        if (((ulong)obj.Layer & visibleLayer) != 0) {

                            if (obj.SigName.ToUpper().Contains(searchWord)) {
                                addItem(obj.SigName, "Text", obj.GetPos(0));
                            }
                        }
                    } else if (obj.Id() == MbeObjID.MbeComponent) {
                        MbeObjComponent compObj = (MbeObjComponent)obj;
                        if (compObj.DrawRefOnDoc && (((ulong)MbeLayer.LayerValue.DOC & visibleLayer) != 0) ||
                            !compObj.DrawRefOnDoc && (((ulong)(compObj.Layer == MbeLayer.LayerValue.CMP ? MbeLayer.LayerValue.PLC : MbeLayer.LayerValue.PLS) & visibleLayer) != 0)) {
                            if (compObj.RefNumText.ToUpper().Contains(searchWord)) {
                                addItem(compObj.RefNumText, "Reference", compObj.RefnumPos());
                            }
                        }
                        if (((ulong)compObj.Layer & visibleLayer) != 0) {
                            if (compObj.PackageName.ToUpper().Contains(searchWord)) {
                                addItem(compObj.PackageName, "Package", compObj.GetPos(0));
                            }
                            if (compObj.SigName.ToUpper().Contains(searchWord)) {
                                addItem(compObj.SigName, "Name", compObj.GetPos(0));
                            }
                        }

                    }

                }
            }
        }

        private void addItem(string findWord,string wordType,Point ptPos)
        {
            ListViewItem item = new ListViewItem(findWord);
            item.SubItems.Add(wordType);
            item.Tag = ptPos;
            listView1.Items.Add(item);
        }



        public MbeView mbeView;

        private void listView1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("listView1_Click()");
            ListViewItem item = listView1.SelectedItems[0];
            Point pt = (Point)(item.Tag);
            System.Diagnostics.Debug.WriteLine(pt.ToString());
            mbeView.MoveCursor(pt);
        }

        private void FindForm_Load(object sender, EventArgs e)
        {
            Rectangle rcOwner = this.Owner.Bounds;
            Rectangle rcForm = Bounds;
            int xpos = rcOwner.X + rcOwner.Width - 20 - rcForm.Width;
            int ypos = rcOwner.Y + 20;
            rcForm.X = xpos;
            rcForm.Y = ypos;
            Bounds = rcForm;

        }

        private void textBoxSearchWord_KeyDown(object sender, KeyEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("textBoxSearchWord_KeyDown "+e.KeyCode);
            if (e.KeyCode == Keys.Return) {
                doSearch();
            }

        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            listView1.Columns[sortColumn].ImageIndex = 2;
            if (e.Column == sortColumn) {
                sortUp = !sortUp;
            } else {
                sortUp = true;
                sortColumn = e.Column;
            }
            listView1.Columns[sortColumn].ImageIndex = (sortUp ? 0 : 1);
            listView1.ListViewItemSorter = new ListViewItemComparer(e.Column,sortUp);
        }

        bool sortUp;
        int sortColumn;
    }

    class ListViewItemComparer : IComparer
    {
        private int col;
        private bool sup;

        public ListViewItemComparer()
        {
            col = 0;
            sup = true;
        }
        public ListViewItemComparer(int column,bool sortup)
        {
            col = column;
            sup = sortup;
        }
        public int Compare(object x, object y)
        {
            int retv =  String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            if (sup) return retv;
            else return -retv;
        }
    }

}
