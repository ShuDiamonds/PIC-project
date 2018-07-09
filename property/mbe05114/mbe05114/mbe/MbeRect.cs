using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace mbe
{
	/// <summary>
	/// 実図面座標で使う矩形構造体。
	/// Rectangleと違い、二点で矩形を決めている
	/// 上に行くに従って大きな値になる。
	/// </summary>
	public struct MbeRect
	{
		public MbeRect(Point _pt1, Point _pt2)
		{
			pt1 = _pt1;
			pt2 = _pt2;
			Normalize();
		}


		public void Normalize()
		{
			int l,t,r,b;
			if(pt1.X<pt2.X){
				l=pt1.X; r=pt2.X;
			}else{
				r=pt1.X; l=pt2.X;
			}
			if(pt1.Y>pt2.Y){
				t=pt1.Y; b=pt2.Y;
			}else{
				b=pt1.Y; t=pt2.Y;
			}
			pt1 = new Point(l,t);
			pt2 = new Point(r,b);
		}

		public Point LT{
			get{ return pt1;}
			set{ pt1 = value; Normalize();}
		}

		public Point RB{
			get{ return pt2;}
			set{ pt2 = value; Normalize();}
		}

		public int L
		{
			get { return pt1.X; }
		}

		public int T
		{
			get { return pt1.Y; }
		}

		public int R
		{
			get { return pt2.X; }
		}

		public int B
		{
			get { return pt2.Y; }
		}


		public void Offset(Point offset)
		{
			pt1.Offset(offset);
			pt2.Offset(offset);
		}

		public bool Contains(Point pt)
		{
			return (pt1.X <= pt.X && pt.X <= pt2.X && pt1.Y >= pt.Y && pt.Y >= pt2.Y);
		}

		public void Or(Point pt)
		{
			if (pt1.X > pt.X) pt1.X = pt.X;
			if (pt2.X < pt.X) pt2.X = pt.X;
			if (pt1.Y < pt.Y) pt1.Y = pt.Y;
			if (pt2.Y > pt.Y) pt2.Y = pt.Y;
		}

        public void Or(MbeRect rc)
        {
            if (pt1.X > rc.L) pt1.X = rc.L;
            if (pt2.X < rc.R) pt2.X = rc.R;
            if (pt1.Y < rc.T) pt1.Y = rc.T;
            if (pt2.Y > rc.B) pt2.Y = rc.B;
        }


		public Size SizeRect()
		{
			return new Size((pt2.X - pt1.X), (pt1.Y - pt2.Y));
		}

		public int Width
		{
			get { return (pt2.X - pt1.X); }
		}

		public int Height
		{
			get { return (pt1.Y - pt2.Y); }
		}

		public Point Center()
		{
			return new Point((pt2.X + pt1.X) / 2, (pt1.Y + pt2.Y) / 2);
		}

		private Point pt1;
		private Point pt2;
	}
}
