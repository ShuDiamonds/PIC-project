using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace mbe
{
    class PrintPageLayerListCtrl : Control
    {
        public List<PrintPageLayerInfo> infoList;

        public Font DiplayFont
        {
            get { return font; }
        }

        //public ulong LayerMask
        //{
        //    get { return layerMask; }
        //    set { 
        //        layerMask = value;
        //        SetLayerColumnCount();
        //    }
        //}
        public int DisplayTop
        {
            get { return displayTop; }
            set {
                int n = Count-displayLineCount;
                if (n <= 0) {
                    displayTop = 0;
                } else {
                    displayTop = (n < value ? n : value);
                }
                EditTitleEnd();
                Refresh();
            }
        }


        public bool EnablePaint
        {
            get { return enablePaint; }
            set { 
                enablePaint = value;
                Refresh();
            }
        }

        public int PageCount
        {
            get
            {
                if (infoList == null) return 0;
                return infoList.Count;
            }                
        }

        public int DisplayLineCount
        {
            get { return displayLineCount; }
        }

        public int Count
        {
            get
            {
                if (infoList == null) return 0;
                return infoList.Count;
            }
        }



        public int titleWidth;
        public const int CHECKBOX_WIDTH = 16;
        public const int LINE_NUM_WIDTH =  CHECKBOX_WIDTH * 2;
        public const int LINE_HEIGHT = 18;
        public const int TEXT_HEIGHT = 14;
        public const int MAX_PAGE_COUNT = 20;
        private const uint SELECTED_BACKGROUND = 0xFFE0E0FF;

        private const int CLICK_AT_ACTIVE   = -1;
        private const int CLICK_AT_MIRROR   = -2;
        private const int CLICK_AT_TITLE     = -3;
        private const int CLICK_AT_OUTSIDE = -4;



        //private void SetLayerColumnCount()
        //{
        //    layerColumnCount = 0;
        //    for (int i = 0; i < MbeLayer.valueTable.Length; i++) {
        //        if ((layerMask & (ulong)MbeLayer.valueTable[i]) != 0) {
        //            layerColumnCount++;
        //        }
        //    }
        //}

        /// <summary>
        /// ページ数が変わったときの通知デリゲート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PageCountChangedHandler(object sender, EventArgs e);

        /// <summary>
        /// ページ数が変わったときの通知ハンドラ
        /// </summary>
        public event PageCountChangedHandler PageCountChanged;



        public PrintPageLayerListCtrl()
        {
            infoList = null;
            enablePaint = false;
            //this.BorderStyle = BorderStyle.FixedSingle;
            //AutoScroll = true;
            //layerMask = 0;
            //AutoScrollMinSize = new Size(0, 500);
            checkIcon = new Icon(Properties.Resources.checkMark, 16, 16);
            layerColumnCount = MbeLayer.valueTable.Length;
            PageCountChanged = null;
            displayLineCount = 6;
            displayTop = 0;
            selectedIndex = 1;
            titleWidth = 150;
            editTitleActive = -1;

            font = new Font(FontFamily.GenericSansSerif, TEXT_HEIGHT, GraphicsUnit.Pixel);

            inputTitle = new System.Windows.Forms.TextBox();
            inputTitle.Name = "inputTitle";
            inputTitle.Enabled = false;
            inputTitle.Visible = false;
            inputTitle.Font = font;
            inputTitle.BorderStyle = BorderStyle.None;
            this.Controls.Add(inputTitle);
            

            InitializeContextMenu();
            this.ContextMenu = contextMenu;

            //ダブルバッファリング設定
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }

        private void DeletePage(int index)
        {
            EditTitleEnd();
            if (index < 0 || index >= infoList.Count) return;
            infoList.RemoveAt(index);
            if ((Count - displayTop) < DisplayLineCount) {
                displayTop = Count - DisplayLineCount;
                if (displayTop < 0) {
                    displayTop = 0;
                }
            }
            
            Refresh();
            if (PageCountChanged != null) {
                PageCountChanged(this, null);
            }
        }

        private void AddPage(int index)
        {
            EditTitleEnd();
            PrintPageLayerInfo info = new PrintPageLayerInfo();
            if (index < 0 || index >= infoList.Count) {
                infoList.Add(info);
            } else {
                infoList.Insert(index + 1, info);
            }
            Refresh();
            if (PageCountChanged != null) {
                PageCountChanged(this, null);
            }
        }

        private void InsertPage(int index)
        {
            EditTitleEnd();
            if (index < 0) {
                index = 0;
            } else if (index >= infoList.Count) {
                index = infoList.Count - 1;
            }
            PrintPageLayerInfo info = new PrintPageLayerInfo();
            infoList.Insert(index, info);
            Refresh();
            if (PageCountChanged != null) {
                PageCountChanged(this, null);
            }
        }


        public void InitDrawParam()
        {
            //タイトルの表示幅
            titleWidth = ClientSize.Width - (CHECKBOX_WIDTH * (4 + layerColumnCount) + 1);//行番号がチェックボックス2個分、ページアクティブチェック1個、ミラーチェック1個
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!enablePaint) return;

            int n;
            if (infoList == null) {
                n = 0;
            } else {
                n = infoList.Count;
            }
            //if (n == 0) return;

            Size sizeClient = ClientSize;
            int clwidth = sizeClient.Width;

            System.Diagnostics.Debug.WriteLine("PrintPageLayerListCtrl  OnPaint()");
            InitDrawParam();


            //背景色 (全体と選択行)
            e.Graphics.Clear(Color.White);
            if (selectedIndex >= 0) {
                int dindx = selectedIndex - displayTop;
                if (dindx < displayLineCount) {
                    Rectangle selRC = new Rectangle(0, dindx * LINE_HEIGHT, clwidth, LINE_HEIGHT);
                    Brush brush = new SolidBrush(Color.FromArgb(unchecked((int)SELECTED_BACKGROUND)));
                    e.Graphics.FillRectangle(brush, selRC);
                    brush.Dispose();
                }
            }


            //内容の表示
            int y = 0;
            int index = displayTop;
            for (int i = 0; i < displayLineCount; i++) {
                if(!DrawInfo(e.Graphics, index, y)){
                    break;
                }
                y += LINE_HEIGHT;
                index++;
            }

            //枠線描画
            //int r = lineNumWidth + checkboxWidth * (1 + layerColumnCount) + titleWidth;
            int r = clwidth - 1;
            Pen pen = new Pen(Color.Gray, 1);
            y = 0;
            for (int i = 0; i <= displayLineCount; i++) {
                e.Graphics.DrawLine(pen, 0, y, r, y);
                y += LINE_HEIGHT;
            }

            y = sizeClient.Height-1;

            int x = 0;
            e.Graphics.DrawLine(pen, x, 0, x, y);
            x += LINE_NUM_WIDTH;
            e.Graphics.DrawLine(pen, x, 0, x, y);
            x += CHECKBOX_WIDTH;
            e.Graphics.DrawLine(pen, x, 0, x, y);
            x += CHECKBOX_WIDTH;
            e.Graphics.DrawLine(pen, x, 0, x, y);
            x += titleWidth;
            e.Graphics.DrawLine(pen, x, 0, x, y);
            for (int i = 0; i < layerColumnCount; i++) {
                x += CHECKBOX_WIDTH;
                e.Graphics.DrawLine(pen, x, 0, x, y);
            }
            



            pen.Dispose();

        }

        protected bool DrawInfo(Graphics g, int index, int y)
        {

            if (index >= Count) {
                return false;
            }
            
            Brush brush;
            
            brush = new SolidBrush(Color.Black);
            //Font font = new Font(FontFamily.GenericSansSerif, TEXT_HEIGHT, GraphicsUnit.Pixel);
            string str = string.Format("{0}", index + 1);
            g.DrawString(str, font, brush, new PointF(0, y+1));
            g.DrawString(infoList[index].name, font, brush, new RectangleF(LINE_NUM_WIDTH + CHECKBOX_WIDTH*2, y+1, titleWidth, LINE_HEIGHT));
            //font.Dispose();
            brush.Dispose();

            int xpos = LINE_NUM_WIDTH;

            if (infoList[index].active) {
                g.DrawIcon(checkIcon, xpos, y);
            }

            xpos += CHECKBOX_WIDTH;

            if (infoList[index].mirror) {
                g.DrawIcon(checkIcon, xpos, y);
            }

            xpos += CHECKBOX_WIDTH + titleWidth;

            for (int i = 0; i < MbeLayer.valueTable.Length; i++) {
                if((infoList[index].checkvalue & (ulong)MbeLayer.valueTable[i]) != 0) {
                    g.DrawIcon(checkIcon, xpos, y);
                }
                xpos += CHECKBOX_WIDTH;
            }
            return true;

        }



        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    Refresh();
        //    //base.OnMouseUp(e);
        //}

        int CursorYtoIndex(int cursorY)
        {
            if (Count == 0) return -1;
            int index = cursorY/LINE_HEIGHT+displayTop;
            if (index >= Count) return -1;
            else return index;
        }

        int CursorXtoIndex(int cursorX)
        {
            int x = cursorX;
            if (x < LINE_NUM_WIDTH) return CLICK_AT_OUTSIDE;
            x -= LINE_NUM_WIDTH;
            if (x < CHECKBOX_WIDTH) return CLICK_AT_ACTIVE;
            x -= CHECKBOX_WIDTH;
            if (x < CHECKBOX_WIDTH) return CLICK_AT_MIRROR;
            x -= CHECKBOX_WIDTH;
            if (x < titleWidth) return CLICK_AT_TITLE;
            x -= titleWidth;
            x /= CHECKBOX_WIDTH;
            if (x >= MbeLayer.valueTable.Length) return CLICK_AT_OUTSIDE;
            return x;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //base.OnMouseMove(e);
            int index = CursorYtoIndex(e.Y);
            if (selectedIndex != index) {
                selectedIndex = index;
                Refresh();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            //base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left) {
                EditTitleEnd();
                int indexY = CursorYtoIndex(e.Y);
                int indexX = CursorXtoIndex(e.X);
                System.Diagnostics.Debug.WriteLine("CursorXtoIndex() : " + indexX+","+indexY);
                if (indexY < 0 || indexY >= Count) return;
                if (indexX == CLICK_AT_OUTSIDE) return;

                if (indexX == CLICK_AT_ACTIVE) {
                    infoList[indexY].active = !infoList[indexY].active;
                }else if (indexX == CLICK_AT_MIRROR) {
                    infoList[indexY].mirror = !infoList[indexY].mirror;
                } else if (indexX == CLICK_AT_TITLE) {
                    EditTitleStart(indexY);
                } else {
                    infoList[indexY].checkvalue ^= (ulong)MbeLayer.valueTable[indexX];
                }
                Refresh();
            }
        }

        private void EditTitleStart(int index)
        {
            if (editTitleActive >= 0) return;
            int ypos = (index - displayTop) * LINE_HEIGHT+1;
            inputTitle.Location = new Point(LINE_NUM_WIDTH + CHECKBOX_WIDTH*2 + 1, ypos);
            inputTitle.Size = new Size(titleWidth - 2, LINE_HEIGHT-1);
            inputTitle.Text = infoList[index].name;
            inputTitle.Select(infoList[index].name.Length, 0);
            editTitleActive = index;
            inputTitle.Enabled = true;
            inputTitle.Visible = true;
            inputTitle.Focus();
            //this.ContextMenu = null;
        }

        public void EditTitleEnd()
        {
            if (editTitleActive < 0) return;
            infoList[editTitleActive].name = inputTitle.Text;
            editTitleActive = -1;
            inputTitle.Enabled = false;
            inputTitle.Visible = false;
            //this.ContextMenu = contextMenu;
        }

        private void InitializeContextMenu()
        {

            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.contextMenuInsert = new MenuItem("Insert");
            this.contextMenuInsert.Click += new System.EventHandler(this.OnContextMenuInsert);
            this.contextMenuAdd = new MenuItem("Add");
            this.contextMenuAdd.Click += new System.EventHandler(this.OnContextMenuAdd);
            this.contextMenuDelete = new MenuItem("Delete");
            this.contextMenuDelete.Click += new System.EventHandler(this.OnContextMenuDelete);
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]{
                this.contextMenuInsert,
				this.contextMenuAdd,
				this.contextMenuDelete});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Opened);
        }

        private void contextMenu_Opened(object sender, EventArgs e)
        {
            contextMenuAdd.Enabled = (Count<MAX_PAGE_COUNT);
            contextMenuDelete.Enabled = (selectedIndex >= 0);
        }

        private void OnContextMenuInsert(object sender, EventArgs e)
        {
            InsertPage(selectedIndex);
        }

        private void OnContextMenuAdd(object sender, EventArgs e)
		{
			AddPage(selectedIndex);
		}

        private void OnContextMenuDelete(object sender, EventArgs e)
		{
			DeletePage(selectedIndex);
		}

        //rivate ulong layerMask;
        private int layerColumnCount;
        private Icon checkIcon;

        private bool enablePaint;

        private int displayLineCount;
        private int selectedIndex;
        private int displayTop;

        private ContextMenu contextMenu;
        private MenuItem contextMenuInsert;
        private MenuItem contextMenuAdd;
        private MenuItem contextMenuDelete;

        private System.Windows.Forms.TextBox inputTitle;

        private int editTitleActive;
        private Font font;

   



    }
}

