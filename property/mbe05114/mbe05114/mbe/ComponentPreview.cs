using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace mbe
{
    class ComponentPreview : ScrollableControl
    {
        public ComponentPreview()
        {
            mbeComponent = null;
            displayScale = 1000;
            enablePaint = false;
            mouseWheelValue = 0;

            //ダブルバッファリング設定
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private double displayScale;
        private int mouseWheelValue;

        public double DisplayScale
        {
            get { return displayScale; }
            set { displayScale = value; }
        }
        private MbeObjComponent mbeComponent;

        public MbeObjComponent ComponentObj
        {
            get { return mbeComponent; }
            set { 
                mbeComponent = value;
                SetPanelSize();
                Refresh();
            }
        }

        /// <summary>
        /// ペイントの許可
        /// </summary>
        public bool EnablePaint
        {
            set
            {
                enablePaint = value;
                Refresh();
            }
            get
            {
                return enablePaint;
            }
        }

        /// <summary>
        /// OnPaint()での描画許可フラグ
        /// </summary>
        protected bool enablePaint;


        public void SetPanelSize()
        {
            if (mbeComponent == null) {
                AutoScrollMinSize = new Size(1, 1);
                return;
            }

            MbeRect rc = mbeComponent.OccupationRect();
            int width = ((-rc.L > rc.R ? -rc.L : rc.R)+10000) * 2;
            int height = ((-rc.B > rc.T ? -rc.B : rc.T)+10000) * 2;


            //int wm = (rc.Width+40000) / 2;
            //int hm = (rc.Height+40000) / 2;
            //rc = new MbeRect(new Point(rc.L - wm, rc.T + hm), new Point(rc.R + wm, rc.B - hm));


            width = (int)Math.Round(width / displayScale);
            height = (int)Math.Round(height / displayScale);

            AutoScrollMinSize = new Size(width, height);

            
            int scrollWidth = width - ClientSize.Width;
            if (scrollWidth < 0) scrollWidth = 0;

            int scrollHeight = height - ClientSize.Height;
            if (scrollHeight < 0) scrollHeight = 0;

            AutoScrollPosition = new Point(scrollWidth / 2, scrollHeight / 2);
            
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            mouseWheelValue += e.Delta;
            int stepValue = mouseWheelValue / MbeView.MOUSE_WHEEL_STEP;
            mouseWheelValue = mouseWheelValue % MbeView.MOUSE_WHEEL_STEP;
            if (stepValue > 0) {
                ZoomInOut(true);
            } else if (stepValue < 0) {
                ZoomInOut(false);
            }
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    System.Diagnostics.Debug.WriteLine("OnKeyDown");
        //}

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    base.OnMouseDown(e);
        //    Focus();
        //    System.Diagnostics.Debug.WriteLine("OnMouseDown  "+Focused);
        //}
        /// <summary>
        /// ズームイン、アウトを行う
        /// </summary>
        /// <param name="zoomIn">trueのときズームイン(拡大表示)</param>
        public void ZoomInOut(bool zoomIn)
        {
            if (zoomIn) {
                displayScale = displayScale / MbeView.DEFAULT_ZOOM_STEP;
            } else {
                displayScale = displayScale * MbeView.DEFAULT_ZOOM_STEP;
            }
            SetPanelSize();
            Refresh();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!enablePaint) return;

            Size sizeClient = ClientSize;
            Graphics g = e.Graphics;
            g.Clear(MbeColors.ColorBackground);
            Point ptScroll = AutoScrollPosition;
            int scrollWidth = AutoScrollMinSize.Width - sizeClient.Width;
            if (scrollWidth < 0) scrollWidth = 0;

            int scrollHeight = AutoScrollMinSize.Height - sizeClient.Height;
            if (scrollHeight < 0) scrollHeight = 0;


            g.TranslateTransform(sizeClient.Width / 2 + (ptScroll.X + scrollWidth / 2), sizeClient.Height / 2 + (ptScroll.Y + scrollHeight / 2));

            int layerCount = MbeLayer.valueTable.Length;
            DrawParam dp = new DrawParam();
            dp.g = g;
            dp.mode = MbeDrawMode.Draw;
            dp.scale = displayScale;
            dp.visibleLayer = 0xFFFFFFFFFFFFFFFF;

            for (int i = 0; i < layerCount; i++) {
                //if ((visibleLayer & (ulong)(MbeLayer.valueTable[i])) != 0) {
                dp.layer = MbeLayer.valueTable[i];
                mbeComponent.Draw(dp);
   
            }

        }



    }
}
