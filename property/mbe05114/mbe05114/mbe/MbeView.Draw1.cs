using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace mbe
{
	partial class MbeView
	{
		#region 描画定数
		public const int ORIGIN_MARK_SIZE = 8;
		public const int MINIMUM_GRID_PITCH = 10;
		#endregion

		protected void DrawSelectFrame(Graphics g, double scale, Point pt1, Point pt2)
		{
			pt1 = RealToDraw(pt1, scale);
			pt2 = RealToDraw(pt2, scale);

			Rectangle rc = new Rectangle();
			rc.X = (pt1.X < pt2.X ? pt1.X : pt2.X);
			rc.Y = (pt1.Y < pt2.Y ? pt1.Y : pt2.Y);
			rc.Width = Math.Abs(pt1.X - pt2.X);
			rc.Height = Math.Abs(pt1.Y - pt2.Y);
			Pen pen = new Pen(MbeColors.ColorSelectFrame, 1);
			pen.DashStyle = DashStyle.Dot;
			g.DrawRectangle(pen, rc);
			pen.Dispose();
		}


		/// <summary>
		/// 十字カーソルの描画
		/// </summary>
		/// <param name="g"></param>
		/// <param name="scale"></param>
		protected void DrawCrossCursor(Graphics g, double scale)
		{
			Point pt = CursorPos;
			Color col = Color.FromArgb(0x80, MbeColors.ColorCrossCursor);
			pt = RealToDraw(pt, scale);
			Pen pen = new Pen(col, 1);
			Point ptLT = ClientToDraw(new Point(0, 0),scale);
			Point ptRB = ClientToDraw((Point)sizeClient, scale);
			g.DrawLine(pen, pt.X,ptLT.Y, pt.X, ptRB.Y);
			g.DrawLine(pen, ptLT.X, pt.Y, ptRB.X, pt.Y);
			pen.Dispose();
		}


		/// <summary>
		/// カレントアクティブポイントマークの描画
		/// </summary>
		/// <param name="g"></param>
		/// <param name="scale"></param>
		protected void DrawCurrentActivePointMark(Graphics g, double scale)
		{
			Point pt = currentActivePoint;
            uint ncol = MbeColors.ActiveSnapPoint;
            Color col = Color.FromArgb(unchecked((int)ncol)); 
            //Color col = Color.White;
			pt = RealToDraw(pt, scale);
			Pen pen = new Pen(col, 1);
			g.DrawEllipse(pen, pt.X - 4, pt.Y - 4, 8, 8);
            col = Color.FromArgb(unchecked((int)(~ncol | 0xff000000)));
            pen.Color = col;
            g.DrawEllipse(pen, pt.X - 5, pt.Y - 5, 10, 10);
            pen.Dispose();
		}

		/// <summary>
		/// 線分分割マークの描画
		/// </summary>
		/// <param name="g"></param>
		/// <param name="scale"></param>
		protected void DrawDividablePointMark(Graphics g, double scale)
		{
			if(dividableMarkIcon == null){
				dividableMarkIcon = new Icon(Properties.Resources.divideMark, 16, 16);
			}

			Point pt = divideablePoint;
			pt = RealToDraw(pt, scale);
			g.DrawIcon(dividableMarkIcon,pt.X-8,pt.Y-8);
		}



        protected void DrawMeasureInfo(Graphics g, double scale)
        {
            double distance = Math.Sqrt(((double)sizeMeasure.Width)*((double)sizeMeasure.Width)+((double)sizeMeasure.Height)*((double)sizeMeasure.Height));
            
            string str = String.Format("X:{0:##0.0###}mm\nY:{1:##0.0###}mm\nd:{2:##0.0###}mm",
                    (double)sizeMeasure.Width / 10000,
                    (double)sizeMeasure.Height / 10000,
                    distance / 10000);
            if (modeMinor >= 2) {
                string lengthStr = String.Format("\nTotal:{0:##0.0###}mm", (measureLength+distance) / 10000);
                str += lengthStr;
            }
            
            Point pt = RealToDraw(CursorPos, scale);
            Color col = Color.White;
            Font font = new Font(FontFamily.GenericMonospace, 12, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(col);
            g.DrawString(str, font, brush, pt.X+12,pt.Y+12);
            brush.Dispose();
            font.Dispose();

            Pen pen = new Pen(col, 1);
            Point ptstart = RealToDraw(listMeasurePoint[0], scale);
            for (int i = 1; i < modeMinor; i++) {
                Point ptend = RealToDraw(listMeasurePoint[i], scale);
                g.DrawLine(pen, ptstart, ptend);
                ptstart = ptend;
            }
            g.DrawLine(pen, ptstart, pt);
            pen.Dispose();


        }

		protected void DrawGapChkMark(Graphics g, double scale)
		{
			ImageAttributes ia = new ImageAttributes();
			ia.SetColorKey(Color.White, Color.White);
			Image bmpMark = Properties.Resources.imageGapChkMark;
			int width = bmpMark.Width;
			int height = bmpMark.Height;
			Rectangle rc = new Rectangle(0, 0, width, height);
			foreach (Point pt in document.GapChkResult) {
				Point ptD = RealToDraw(pt, scale);
				rc.X = ptD.X - 15;
				rc.Y = ptD.Y - 31;
				g.DrawImage(bmpMark, rc, 0, 0, width, height, GraphicsUnit.Pixel, ia);
			}




			//if (gapChkMarkIcon == null) { 
			//    gapChkMarkIcon = new Icon(Properties.Resources.GapChkMarkIcon, 32, 32);
			//}
			//foreach (Point pt in document.GapChkResult) {
			//    Point ptD = RealToDraw(pt, scale);
			//    g.DrawIcon(gapChkMarkIcon, ptD.X - 15, ptD.Y - 31);
			//}
		}

		/// <summary>
		/// 図面原点マークの描画 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="scale"></param>
		protected void DrawOriginMark(Graphics g, double scale)
		{
			Point pt = new Point(0,0);
			Color col = Color.FromArgb(0xFF, MbeColors.ColorOriginMark);
			pt=RealToDraw(pt,scale);
			Pen pen = new Pen(col,1);
			g.DrawLine(pen,pt.X-ORIGIN_MARK_SIZE,pt.Y,pt.X+ORIGIN_MARK_SIZE ,pt.Y);
			g.DrawLine(pen,pt.X,pt.Y-ORIGIN_MARK_SIZE,pt.X,pt.Y+ORIGIN_MARK_SIZE );
			pen.Dispose();
		}

		/// <summary>
		/// グリッド原点マークの描画 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="scale"></param>
		protected void DrawGridOriginMark(Graphics g, double scale)
		{
			if (gridOrigin.X == 0 && gridOrigin.Y == 0) return;
			Point pt = GridOrigin;
			Color col = Color.FromArgb(0xFF, MbeColors.ColorGridOriginMark);
			pt = RealToDraw(pt, scale);
			Pen pen = new Pen(col, 1);
			g.DrawLine(pen, pt.X - ORIGIN_MARK_SIZE, pt.Y, pt.X + ORIGIN_MARK_SIZE, pt.Y);
			g.DrawLine(pen, pt.X, pt.Y - ORIGIN_MARK_SIZE, pt.X, pt.Y + ORIGIN_MARK_SIZE);
			pen.Dispose();
		}


		/// <summary>
		/// メインリストのデータの描画
		/// </summary>
		/// <param name="g"></param>
		/// <param name="scale"></param>
		protected void DrawMain(Graphics g, double scale)
		{
			int layerCount = MbeLayer.valueTable.Length;
			DrawParam dp = new DrawParam();
			dp.g = g;
			dp.mode = MbeDrawMode.Draw;
			dp.scale = scale;
			dp.visibleLayer = visibleLayer;


			//ulong visibleLayer = document.docInfo.VisibleLayer;

			for (int i = 0; i < layerCount; i++) {
				if ((visibleLayer & (ulong)(MbeLayer.valueTable[i])) != 0) {
					dp.layer = MbeLayer.valueTable[i];
					foreach (MbeObj obj in document.MainList) {
						if (obj.DeleteCount < 0) {
							obj.Draw(dp);
						}
					}
				}
			}
		}


		/// <summary>
		/// 一時リストのデータの描画
		/// </summary>
		/// <param name="g"></param>
		/// <param name="scale"></param>
		protected void DrawTemp(Graphics g, double scale)
		{
			int layerCount = MbeLayer.valueTable.Length;
			DrawParam dp = new DrawParam();
			dp.g = g;
			dp.mode = MbeDrawMode.Temp;
			dp.scale = scale;
			dp.visibleLayer = visibleLayer;


			for (int i = 0; i < layerCount; i++) {
				if ((visibleLayer & (ulong)(MbeLayer.valueTable[i])) != 0) {
					dp.layer = MbeLayer.valueTable[i];
					foreach (MbeObj obj in document.TempList) {
						obj.Draw(dp);
					}
				}
			}
		}



		//グリッドの描画
		protected void DrawGrid(Graphics g, double scale)
		{
            int gridPitch;

            Point lt = new Point(0, 0);
            Point rb = new Point(sizeClient);

			
			Color col = Color.FromArgb(0x40, MbeColors.ColorGrid);
			Pen pen = new Pen(col,1);

            int maxPx = rb.X;
            int maxPy = rb.Y;

            lt = ClientToVw(lt);
            lt = VwToReal(lt, scale);
            rb = ClientToVw(rb);
            rb = VwToReal(rb, scale);



            gridPitch = currentGridInfo.Horizontal * currentGridInfo.DisplayEvery;
            if ((gridPitch / scale) >= MINIMUM_GRID_PITCH) {

                int x = ((lt.X - gridOrigin.X) / gridPitch) * gridPitch + gridOrigin.X;
                while (x <= rb.X) {
                    Point pt1 = new Point(x, lt.Y);
                    pt1 = RealToDraw(pt1, scale);
                    Point pt2 = pt1;
                    pt2.Y = pt2.Y + sizeClient.Height;
                    g.DrawLine(pen, pt1, pt2);
                    x += gridPitch;
                }
            }


            gridPitch = currentGridInfo.Vertical * currentGridInfo.DisplayEvery;
            if ((gridPitch / scale) >= MINIMUM_GRID_PITCH) {

                int y = ((rb.Y - gridOrigin.Y) / gridPitch) * gridPitch + gridOrigin.Y;
                while (y <= lt.Y) {
                    Point pt1 = new Point(lt.X, y);
                    pt1 = RealToDraw(pt1, scale);
                    Point pt2 = pt1;
                    pt2.X = pt2.X + sizeClient.Width;
                    g.DrawLine(pen, pt1, pt2);
                    y += gridPitch;
                }

            }

			pen.Dispose();
		}

		/// <summary>
		/// 印刷のためのメインリストのデータの描画
		/// </summary>
		/// <param name="g"></param>
		/// <param name="scale"></param>
		protected void DrawMainForPrint(Graphics g, double scale,ulong layerFlags,uint option)
		{
			int layerCount = MbeLayer.valueTable.Length;
			DrawParam dp = new DrawParam();
			dp.g = g;
			dp.mode = MbeDrawMode.Print;
			dp.scale = scale;
            dp.visibleLayer = layerFlags;
            dp.option = option;

            //if(CenterPunchMode) dp.option |= (uint)MbeDrawOption.CenterPunchMode;
            //if(PrintToolMarkMode) dp.option |= (uint)MbeDrawOption.ToolMarkMode;
            //if (PrintColorMode) dp.option |= (uint)MbeDrawOption.PrintColor;

			for (int i = 0; i < layerCount; i++) {
                if ((layerFlags & (ulong)(MbeLayer.valueTable[i])) != 0) {
					dp.layer = MbeLayer.valueTable[i];
					foreach (MbeObj obj in document.MainList) {
						if (obj.DeleteCount < 0) {
							obj.Draw(dp);
						}
					}
				}
			}
		}


	}

}
