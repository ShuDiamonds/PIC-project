using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace mbe
{
	class DragInfo
	{
		private const int DEFAULT_STAY_LIMIT = 2;


		public DragInfo()
		{
			Clear(DEFAULT_STAY_LIMIT);
		}

		public void Clear(int _stayLimit)
		{
			ptDragFrom = new Point(0, 0);
			ptDragTo = new Point(0, 0);
			ptDragFromClient = new Point(0, 0);
			ptDragToClient = new Point(0, 0);
			isDragging = false;
			isMoved = false;
			stayLimit = _stayLimit;
		}


		public void MouseDown(Point pt, Point ptClient)
		{
			ptDragFrom = pt;
			ptDragTo = pt;
			ptDragFromClient = ptClient;
			ptDragToClient = ptClient;

			isDragging = true;
			isMoved = false;
			//System.Diagnostics.Debug.WriteLine("DragInfo.MouseDown");
		}

		public void MouseMove(Point pt, Point ptClient)
		{
			if (isDragging) {
				ptDragTo = pt;
				ptDragToClient = ptClient;

				int x = Math.Abs(ptDragFromClient.X - ptDragToClient.X);
				int y = Math.Abs(ptDragFromClient.Y - ptDragToClient.Y);
				if (x > stayLimit || y > stayLimit) {
					isMoved = true;
				}
			}
		}

		public bool MouseUp(Point pt, Point ptClient)
		{
			if (isDragging) {
				ptDragTo = pt;
				ptDragToClient = ptClient;
				isDragging = false;
				//System.Diagnostics.Debug.WriteLine("DragInfo.MouseUp true");
				return true;
			} else {
				//System.Diagnostics.Debug.WriteLine("DragInfo.MouseUp false");
				return false;
			}
		}

		public Point PtDragFrom
		{
			get { return ptDragFrom; }
		}

		public Point PtDragTo
		{
			get { return ptDragTo; }
		}

		public Point PtDragFromClient
		{
			get { return ptDragFromClient; }
		}

		public Point PtDragToClient
		{
			get { return ptDragToClient; }
		}

		public bool IsDragging
		{
			get { return isDragging; }
		}

		/// <summary>
		/// クライアント座標でのXY移動量がthreshold以下かどうかを返す
		/// </summary>
		/// <returns></returns>
		public bool IsClicked()
		//public bool IsClicked(int threshold)
		{
			//int x = Math.Abs(ptDragFromClient.X - ptDragToClient.X);
			//int y = Math.Abs(ptDragFromClient.Y - ptDragToClient.Y);
			//return (x <= threshold && y <= threshold);
			return !isMoved;
		}

		public bool IsMoved
		{
			get { return isMoved; }
		}

		private Point ptDragFrom;
		private Point ptDragTo;
		private Point ptDragFromClient;
		private Point ptDragToClient;
		private bool isDragging;
		private bool isMoved;
		private int stayLimit;
	}
}
