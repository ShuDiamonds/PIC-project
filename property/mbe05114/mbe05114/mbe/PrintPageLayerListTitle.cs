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
    class PrintPageLayerListTitle : Control
    {
        private int titleWidth;

        public int TitleWidth
        {
            get { return titleWidth; }
            set { titleWidth = value; }
        }

        public Font font;

        public PrintPageLayerListTitle()
        {
            titleWidth = 150;
            font = null;
            
        }


        protected void DrawText(Graphics g, int x, int y, int angle, string str)
        {

            GraphicsState gState = g.Save();	//座標系保存
            g.TranslateTransform(x, y);
            g.RotateTransform(-angle);
            g.DrawString(str, font, new SolidBrush(Color.Black), new PointF(0, 0));
            g.Restore(gState);					//座標系復帰
        }


       

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //titleWidth = ClientSize.Width - (PrintPageLayerListCtrl.CHECKBOX_WIDTH * (4 + MbeLayer.nameTable.Length) + 1);

            if (font == null) return;
            int y = ClientSize.Height - PrintPageLayerListCtrl.TEXT_HEIGHT / 2;
            int x1 = PrintPageLayerListCtrl.LINE_NUM_WIDTH;
            int x2 = x1+PrintPageLayerListCtrl.CHECKBOX_WIDTH;

            int x = (x1+x2-PrintPageLayerListCtrl.TEXT_HEIGHT)/2;
            DrawText(e.Graphics, x, y, 90, "Print");
            x += PrintPageLayerListCtrl.CHECKBOX_WIDTH;
            DrawText(e.Graphics, x, y, 90, "Mirror");

            x2 += PrintPageLayerListCtrl.CHECKBOX_WIDTH;
            x = x2 + (titleWidth - PrintPageLayerListCtrl.TEXT_HEIGHT) / 2;
            DrawText(e.Graphics, x, y, 90, "Note");


            x1 = x2 + titleWidth;
            x2 = x1 + PrintPageLayerListCtrl.CHECKBOX_WIDTH;
            x = (x1 + x2 - PrintPageLayerListCtrl.TEXT_HEIGHT) / 2;

            for (int i = 0; i < MbeLayer.nameTable.Length; i++) {
                DrawText(e.Graphics, x, y, 90, MbeLayer.nameTable[i]);
                x += PrintPageLayerListCtrl.CHECKBOX_WIDTH;
            }
  
        }


    }
}
