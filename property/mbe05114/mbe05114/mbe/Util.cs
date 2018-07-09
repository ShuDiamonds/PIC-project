using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace mbe
{
	class Util
	{

		/// <summary>
		/// tがr0とr1の間にあるかどうかのテスト
		/// </summary>
		/// <param name="t"></param>
		/// <param name="r0"></param>
		/// <param name="r1"></param>
		/// <returns></returns>
		private static bool IsInRange(double t, double r0, double r1)
		{
			return ((r0 <= t && t <= r1) || (r1 <= t && t <= r0));
		}

        private static bool IsInRangeN(int t, int r0, int r1)
        {
            return ((r0 <= t && t <= r1) || (r1 <= t && t <= r0));
        }


		/// <summary>
		/// 整数値を上下限値内に収める
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int LimitRange(int value, int min, int max)
		{
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}


		public static bool IsOverlap(ref int from1, ref int to1, int from2, int to2)
		{
			int n;
			if (from1 > to1) {
				n = from1;
				from1 = to1;
				to1 = n;
			}
			if (from2 > to2) {
				n = from2;
				from2 = to2;
				to2 = n;
			}
			if (to1 < from2 || to2 < from1) return false;
			if (from1 > from2) from1 = from2;
			if (to1 < to2) to1 = to2;
			return true;
		}


		/// <summary>
		/// s0～l0の値とs1～l1の値の重なりの中間値を返す
		/// </summary>
		/// <param name="s0"></param>
		/// <param name="l0"></param>
		/// <param name="s1"></param>
		/// <param name="l1"></param>
		/// <returns></returns>
		protected static int OverlapCenter(int s0, int l0, int s1, int l1)
		{
			int s = (s0 > s1 ? s0 : s1);
			int l = (l0 < l1 ? l0 : l1);
			return (s + l) / 2;
		}


		/// <summary>
		/// 点を囲む8角形の頂点座標を求める
		/// </summary>
		/// <param name="pt0"></param>
		/// <param name="pt1"></param>
		/// <param name="distance"></param>
		/// <param name="ptOutline"></param>
		public static void PointOutlineData(Point pt0, int distance, out Point[] ptOutline)
		{
			ptOutline = new Point[8];

			int ext = distance * 45 / 100;


			ptOutline[(int)OutlineTag.L1B].Y = pt0.Y - ext;
			ptOutline[(int)OutlineTag.L2B].Y = pt0.Y - ext;
			ptOutline[(int)OutlineTag.L1E].Y = pt0.Y + ext;
			ptOutline[(int)OutlineTag.L2E].Y = pt0.Y + ext;

			ptOutline[(int)OutlineTag.L1B].X = pt0.X - distance;
			ptOutline[(int)OutlineTag.L1E].X = pt0.X - distance;
			ptOutline[(int)OutlineTag.L2B].X = pt0.X + distance;
			ptOutline[(int)OutlineTag.L2E].X = pt0.X + distance;

			ptOutline[(int)OutlineTag.CBL1].Y = pt0.Y - distance;
			ptOutline[(int)OutlineTag.CBL2].Y = pt0.Y - distance;
			ptOutline[(int)OutlineTag.CEL1].Y = pt0.Y + distance;
			ptOutline[(int)OutlineTag.CEL2].Y = pt0.Y + distance;

			ptOutline[(int)OutlineTag.CBL1].X = pt0.X - ext;
			ptOutline[(int)OutlineTag.CBL2].X = pt0.X + ext;
			ptOutline[(int)OutlineTag.CEL1].X = pt0.X - ext;
			ptOutline[(int)OutlineTag.CEL2].X = pt0.X + ext;
		}


		/// <summary>
		/// 線分を囲む8角形の頂点座標を求める
		/// </summary>
		/// <param name="pt0"></param>
		/// <param name="pt1"></param>
		/// <param name="distance"></param>
		/// <param name="ptOutline"></param>
        /// <remarks>
        /// 始点Xの方が小さくなるようにして(垂線の場合は始点Yが小さくなるように)頂点座標を求めている。
        /// 始点が入れ替わったときは、revBeginがtrueになる。
        /// </remarks>
		public static void LineOutlineData(Point pt0, Point pt1, int distance, out Point[] ptOutline, out bool revBeginEnd)
		{

			ptOutline = new Point[8];
			revBeginEnd = false;

			if (pt0.X == pt1.X) {//線が垂直
				int ext = distance * 45 / 100;
				int y0 = pt0.Y;
				int y1 = pt1.Y;
				if (y0 > y1) {	//始点のY値の方が小さくなるようにする
					int n = y1;
					y1 = y0;
					y0 = n;
					revBeginEnd = true;
				}

				ptOutline[(int)OutlineTag.L1B].Y = y0 - ext;
				ptOutline[(int)OutlineTag.L2B].Y = y0 - ext;
				ptOutline[(int)OutlineTag.L1E].Y = y1 + ext;
				ptOutline[(int)OutlineTag.L2E].Y = y1 + ext;

				ptOutline[(int)OutlineTag.L1B].X = pt0.X - distance;
				ptOutline[(int)OutlineTag.L1E].X = pt0.X - distance;
				ptOutline[(int)OutlineTag.L2B].X = pt0.X + distance;
				ptOutline[(int)OutlineTag.L2E].X = pt0.X + distance;

				ptOutline[(int)OutlineTag.CBL1].Y = y0 - distance;
				ptOutline[(int)OutlineTag.CBL2].Y = y0 - distance;
				ptOutline[(int)OutlineTag.CEL1].Y = y1 + distance;
				ptOutline[(int)OutlineTag.CEL2].Y = y1 + distance;

				ptOutline[(int)OutlineTag.CBL1].X = pt0.X - ext;
				ptOutline[(int)OutlineTag.CBL2].X = pt0.X + ext;
				ptOutline[(int)OutlineTag.CEL1].X = pt0.X - ext;
				ptOutline[(int)OutlineTag.CEL2].X = pt0.X + ext;

			} else if (pt0.Y == pt1.Y) { //線が水平
				int ext = distance * 4 / 10;
				int x0 = pt0.X;
				int x1 = pt1.X;
				if (x0 > x1) {	//始点のX値の方が小さくなるようにする
					int n = x1;
					x1 = x0;
					x0 = n;
					revBeginEnd = true;
				}
				ptOutline[(int)OutlineTag.L1B].X = x0 - ext;
				ptOutline[(int)OutlineTag.L2B].X = x0 - ext;
				ptOutline[(int)OutlineTag.L1E].X = x1 + ext;
				ptOutline[(int)OutlineTag.L2E].X = x1 + ext;

				ptOutline[(int)OutlineTag.L1B].Y = pt0.Y - distance;
				ptOutline[(int)OutlineTag.L1E].Y = pt0.Y - distance;
				ptOutline[(int)OutlineTag.L2B].Y = pt0.Y + distance;
				ptOutline[(int)OutlineTag.L2E].Y = pt0.Y + distance;

				ptOutline[(int)OutlineTag.CBL1].X = x0 - distance;
				ptOutline[(int)OutlineTag.CBL2].X = x0 - distance;
				ptOutline[(int)OutlineTag.CEL1].X = x1 + distance;
				ptOutline[(int)OutlineTag.CEL2].X = x1 + distance;

				ptOutline[(int)OutlineTag.CBL1].Y = pt0.Y - ext;
				ptOutline[(int)OutlineTag.CBL2].Y = pt0.Y + ext;
				ptOutline[(int)OutlineTag.CEL1].Y = pt0.Y - ext;
				ptOutline[(int)OutlineTag.CEL2].Y = pt0.Y + ext;

			} else {
				if (pt0.X > pt1.X) {
					Point pttemp = pt1;
					pt1 = pt0;
					pt0 = pttemp;
					revBeginEnd = true;
				}
				double a = ((double)(pt1.Y - pt0.Y)) / (pt1.X - pt0.X);
				double d2 = (double)distance * distance;

				double dx2 = d2 / (a * a + 1);
				double dy2 = d2 - dx2;
				//dx,dyは線分をdistanceぶん延長するときのx,y成分
                int dx = (int)Math.Round(Math.Sqrt(dx2));
                int dy = (int)Math.Round(Math.Sqrt(dy2));
				if (a < 0) {
					dy = -dy;
				}
				int ex = dx * 4 / 10;
				int ey = dy * 4 / 10;

				int x;
				int y;

				//側辺始点
				x = pt0.X - ex;
				y = pt0.Y - ey;
				ptOutline[(int)OutlineTag.L1B].X = x - dy;
				ptOutline[(int)OutlineTag.L1B].Y = y + dx;
				ptOutline[(int)OutlineTag.L2B].X = x + dy;
				ptOutline[(int)OutlineTag.L2B].Y = y - dx;

				//側辺終点
				x = pt1.X + ex;
				y = pt1.Y + ey;
				ptOutline[(int)OutlineTag.L1E].X = x - dy;
				ptOutline[(int)OutlineTag.L1E].Y = y + dx;
				ptOutline[(int)OutlineTag.L2E].X = x + dy;
				ptOutline[(int)OutlineTag.L2E].Y = y - dx;

				//始点側端辺
				x = pt0.X - dx;
				y = pt0.Y - dy;
				ptOutline[(int)OutlineTag.CBL1].X = x - ey;
				ptOutline[(int)OutlineTag.CBL1].Y = y + ex;
				ptOutline[(int)OutlineTag.CBL2].X = x + ey;
				ptOutline[(int)OutlineTag.CBL2].Y = y - ex;

				//終点側端辺
				x = pt1.X + dx;
				y = pt1.Y + dy;
				ptOutline[(int)OutlineTag.CEL1].X = x - ey;
				ptOutline[(int)OutlineTag.CEL1].Y = y + ex;
				ptOutline[(int)OutlineTag.CEL2].X = x + ey;
				ptOutline[(int)OutlineTag.CEL2].Y = y - ex;
			}
		}



		public static double DistancePointPoint(Point pt0, Point pt1)
		{
			//Point ptDummy;
			//return DistancePointPoint(pt0, pt1, out ptDummy);
            double dx = pt0.X - pt1.X;
            double dy = pt0.Y - pt1.Y;

            return Math.Sqrt(dx*dx + dy*dy);

			//return Math.Sqrt(Math.Pow(pt0.X - pt1.X, 2) + Math.Pow(pt0.Y - pt1.Y, 2));
		}


		public static bool PointIsCloseToPoint(Point pt0, Point pt1,int dist)
		{
            //double dx = pt0.X - pt1.X;
            long dx = pt0.X - pt1.X;
            if (dx < 0) dx = -dx;
            if (dx > dist) return false;
            //double dy = pt0.Y - pt1.Y;
            long dy = pt0.Y - pt1.Y;
            if (dy < 0) dy = -dy;
            if (dy > dist) return false;

            //return ((double)dist * (double)dist) > (dx*dx + dy*dy);

            
            return ((long)dist * (long)dist) > (dx*dx + dy*dy);
        }


		public static double DistancePointPoint(Point pt0, Point pt1,out Point ptMinCenter)
		{
            ptMinCenter = new Point((pt0.X + pt1.X) / 2, (pt0.Y + pt1.Y) / 2);

            long dx = pt0.X - pt1.X;
            long dy = pt0.Y - pt1.Y;
            long d2 = dx * dx + dy * dy;
            return Math.Sqrt(d2);

			//ptMinCenter = new Point((pt0.X + pt1.X) / 2, (pt0.Y + pt1.Y) / 2);
			//return Math.Sqrt(Math.Pow(pt0.X - pt1.X, 2) + Math.Pow(pt0.Y - pt1.Y, 2));
		}




		/// <summary>
		/// 点と線分の距離を返す
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <returns></returns>
		public static double DistancePointLine(Point pt, Point p0, Point p1)
		{
			//Point ptDummy;
			//return DistancePointLine(pt, p0, p1, out ptDummy);

			if (p0.Equals(p1)) {
				//return DistancePointPoint(p0, pt, out ptMinCenter);
				return DistancePointPoint(p0, pt);
			}

			if (p0.X == p1.X) {	//垂直線のとき
				if (IsInRangeN(pt.Y, p0.Y, p1.Y)) {
					//ptMinCenter = new Point((pt.X + p0.X) / 2, pt.Y);
					return Math.Abs(pt.X - p0.X);
				}
			} else if (p0.Y == p1.Y) { //水平線のとき
				if (IsInRangeN(pt.X, p0.X, p1.X)) {
					//ptMinCenter = new Point(pt.X, (pt.Y + p0.Y) / 2);
					return Math.Abs(pt.Y - p0.Y);
				}
			} else {
				double a1 = (double)(p1.Y - p0.Y) / (double)(p1.X - p0.X);
				double b1 = (double)p0.Y - a1 * p0.X;
				double a2 = -1.0 / a1;
				double b2 = (double)pt.Y - a2 * pt.X;
				double xc = (b2 - b1) / (a1 - a2);
				if (IsInRange(xc, p0.X, p1.X)) {
					double yc = a2 * xc + b2;
                    double xd = pt.X - xc;
                    double yd = pt.Y - yc;
					//ptMinCenter = new Point((int)(pt.X + xc) / 2, (int)(pt.Y + yc) / 2);
					//return Math.Sqrt(Math.Pow(pt.X - xc, 2) + Math.Pow(pt.Y - yc, 2));
                    return Math.Sqrt(xd*xd + yd*yd);
                }
			}

            long xd0 = pt.X - p0.X;
            long yd0 = pt.Y - p0.Y;
            long xd1 = pt.X - p1.X;
            long yd1 = pt.Y - p1.Y;

            long d0 = xd0 * xd0 + yd0 * yd0;
            long d1 = xd1 * xd1 + yd1 * yd1;

			//double d0 = Math.Pow(pt.X - p0.X, 2) + Math.Pow(pt.Y - p0.Y, 2);
			//double d1 = Math.Pow(pt.X - p1.X, 2) + Math.Pow(pt.Y - p1.Y, 2);
			if (d0 < d1) {
				//ptMinCenter = new Point((pt.X + p0.X) / 2, (pt.Y + p0.Y) / 2);
				return Math.Sqrt(d0);
			} else {
				//ptMinCenter = new Point((pt.X + p1.X) / 2, (pt.Y + p1.Y) / 2);
				return Math.Sqrt(d1);
			}
		}

		public static bool PointIsCloseToLine(Point pt, Point p0, Point p1, int dist)
		{
           	if (p0.Equals(p1)) {
				return PointIsCloseToPoint(p0, pt, dist);
			}

            //double dx;
            //double dy;
            long dx;
            long dy;


			if (p0.X == p1.X) {	//垂直線のとき
				if (IsInRangeN(pt.Y, p0.Y, p1.Y)) {
					//ptMinCenter = new Point((pt.X + p0.X) / 2, pt.Y);
					return (dist > Math.Abs(pt.X - p0.X));
				}
			} else if (p0.Y == p1.Y) { //水平線のとき
				if (IsInRangeN(pt.X, p0.X, p1.X)) {
					//ptMinCenter = new Point(pt.X , (pt.Y+p0.Y)/2);
					return (dist > Math.Abs(pt.Y - p0.Y));
				}
			} else {
				double a1 = (double)(p1.Y - p0.Y) / (double)(p1.X - p0.X);
				double b1 = (double)p0.Y - a1 * p0.X;
				double a2 = -1.0 / a1;
				double b2 = (double)pt.Y - a2 * pt.X;
				double xc = (b2 - b1) / (a1 - a2);
				if (IsInRange(xc, p0.X, p1.X)) {
					double yc = a2 * xc + b2;
					//ptMinCenter = new Point((int)(pt.X + xc) / 2, (int)(pt.Y + yc) / 2);
                    //dx = pt.X - xc;
                    //dy = pt.Y - yc;
                    dx = pt.X - (int)Math.Round(xc);
                    dy = pt.Y - (int)Math.Round(yc);
                    //return (((double)dist * (double)dist) > (dx * dx + dy * dy));
                    return (((long)dist * (long)dist) > (dx * dx + dy * dy));
                    //return (dist * dist) > (Math.Pow(pt.X - xc, 2) + Math.Pow(pt.Y - yc, 2));
				}
			}


            dx = pt.X - p0.X;
            dy = pt.Y - p0.Y;
            //double d0 = dx * dx + dy * dy;
            long d0 = dx * dx + dy * dy;

            dx = pt.X - p1.X;
            dy = pt.Y - p1.Y;
            //double d1 = dx * dx + dy * dy;
            long d1 = dx * dx + dy * dy;




            //double d0 = Math.Pow(pt.X - p0.X, 2) + Math.Pow(pt.Y - p0.Y, 2);
            //double d1 = Math.Pow(pt.X - p1.X, 2) + Math.Pow(pt.Y - p1.Y, 2);
			if (d0 < d1) {
				//ptMinCenter = new Point((pt.X + p0.X) / 2, (pt.Y + p0.Y) / 2);
                //return ((double)dist * (double)dist) > (d0);
                return ((long)dist * (long)dist) > (d0);
            } else {
				//ptMinCenter = new Point((pt.X + p1.X) / 2, (pt.Y + p1.Y) / 2);
                return ((long)dist * (long)dist) > (d1);
			}
		}




		public static double DistancePointLine(Point pt, Point p0, Point p1,out Point ptMinCenter)
		{
			if (p0.Equals(p1)) {
				return DistancePointPoint(p0, pt, out ptMinCenter);
			}

			if (p0.X == p1.X) {	//垂直線のとき
				if (IsInRangeN(pt.Y, p0.Y, p1.Y)) {
					ptMinCenter = new Point((pt.X + p0.X) / 2, pt.Y);
					return Math.Abs(pt.X - p0.X);
				}
			} else if (p0.Y == p1.Y) { //水平線のとき
				if (IsInRangeN(pt.X, p0.X, p1.X)) {
					ptMinCenter = new Point(pt.X , (pt.Y+p0.Y)/2);
					return Math.Abs(pt.Y - p0.Y);
				}
			} else {
				double a1 = (double)(p1.Y - p0.Y) / (double)(p1.X - p0.X);
				double b1 = (double)p0.Y - a1 * p0.X;
				double a2 = -1.0 / a1;
				double b2 = (double)pt.Y - a2 * pt.X;
				double xc = (b2 - b1) / (a1 - a2);
				if (IsInRange(xc, p0.X, p1.X)) {
					double yc = a2 * xc + b2;
                    ptMinCenter = new Point((int)Math.Round((pt.X + xc) / 2), (int)Math.Round((pt.Y + yc) / 2));
					return Math.Sqrt(Math.Pow(pt.X - xc, 2) + Math.Pow(pt.Y - yc, 2));
				}
			}

			double d0 = Math.Pow(pt.X - p0.X, 2) + Math.Pow(pt.Y - p0.Y, 2);
			double d1 = Math.Pow(pt.X - p1.X, 2) + Math.Pow(pt.Y - p1.Y, 2);
			if (d0 < d1) {
				ptMinCenter = new Point((pt.X + p0.X) / 2, (pt.Y + p0.Y) / 2);
				return Math.Sqrt(d0);
			} else {
				ptMinCenter = new Point((pt.X + p1.X) / 2, (pt.Y + p1.Y) / 2);
				return Math.Sqrt(d1);
			}
		}

		public static double DistanceRectPoint(MbeRect rc, Point pt, out Point ptMinCenter)
		{
			uint outFlag = 0;
			if (pt.X < rc.L) outFlag |= 0x8;
			if (pt.X > rc.R) outFlag |= 0x2;
			if (pt.Y < rc.B) outFlag |= 0x1;
			if (pt.Y > rc.T) outFlag |= 0x4;

			Point ptR;

			switch (outFlag) {
				case 0xC:
                    ptR = new Point(rc.L, rc.T);                           //20110709 rc.R to rc.L
					return DistancePointPoint(pt, ptR, out ptMinCenter);
				case 0x9:
                    ptR = new Point(rc.L, rc.B);                           //20110709 rc.R to rc.L
					return DistancePointPoint(pt, ptR, out ptMinCenter);
				case 0x3:
                    ptR = new Point(rc.R, rc.B);                           //20110709 rc.L to rc.R
					return DistancePointPoint(pt, ptR, out ptMinCenter);
				case 0x6:
                    ptR = new Point(rc.R, rc.T);                           //20110709 rc.L to rc.R
					return DistancePointPoint(pt, ptR, out ptMinCenter);
				case 0x4:
					ptMinCenter = new Point(pt.X, (pt.Y + rc.T) / 2);
					return pt.Y - rc.T;
				case 0x1:
					ptMinCenter = new Point(pt.X, (pt.Y + rc.B) / 2);
					return rc.B - pt.Y;
				case 0x8:
					ptMinCenter = new Point((pt.X + rc.L) / 2, pt.Y);
					return rc.L - pt.X;
				case 0x2:
					ptMinCenter = new Point((pt.X + rc.R) / 2, pt.Y);
					return pt.X - rc.R;
				default:
					ptR = rc.Center();
					ptMinCenter = new Point((pt.X + ptR.X) / 2, (pt.Y + ptR.Y) / 2);
					return 0.0;
			}
		}

        public static bool PointIsCloseToRect(MbeRect rc, Point pt, int dist)
        {
            uint outFlag = 0;
            if (pt.X < rc.L) outFlag |= 0x8;
            if (pt.X > rc.R) outFlag |= 0x2;
            if (pt.Y < rc.B) outFlag |= 0x1;
            if (pt.Y > rc.T) outFlag |= 0x4;

            Point ptR;

            switch (outFlag) {
                case 0xC:
                    ptR = new Point(rc.L, rc.T);                            //20110709 rc.R to rc.L
                    return PointIsCloseToPoint(pt, ptR, dist);
                    //return DistancePointPoint(pt, ptR, out ptMinCenter);
                case 0x9:
                    ptR = new Point(rc.L, rc.B);                            //20110709 rc.R to rc.L
                    return PointIsCloseToPoint(pt, ptR, dist);
                    //return DistancePointPoint(pt, ptR, out ptMinCenter);
                case 0x3:
                    ptR = new Point(rc.R, rc.B);                            //20110709 rc.L to rc.R
                    return PointIsCloseToPoint(pt, ptR, dist);
                    //return DistancePointPoint(pt, ptR, out ptMinCenter);
                case 0x6:
                    ptR = new Point(rc.R, rc.T);                            //20110709 rc.L to rc.R
                    return PointIsCloseToPoint(pt, ptR, dist);
                    //return DistancePointPoint(pt, ptR, out ptMinCenter);
                case 0x4:
                    //ptMinCenter = new Point(pt.X, (pt.Y + rc.T) / 2);
                    return (pt.Y - rc.T)<dist;
                case 0x1:
                    //ptMinCenter = new Point(pt.X, (pt.Y + rc.B) / 2);
                    return (rc.B - pt.Y)<dist;
                case 0x8:
                    //ptMinCenter = new Point((pt.X + rc.L) / 2, pt.Y);
                    return (rc.L - pt.X)<dist;
                case 0x2:
                    //ptMinCenter = new Point((pt.X + rc.R) / 2, pt.Y);
                    return (pt.X - rc.R)<dist;
                default:
                    return true;
                    //ptR = rc.Center();
                    //ptMinCenter = new Point((pt.X + ptR.X) / 2, (pt.Y + ptR.Y) / 2);
                    //return 0.0;
            }
        }






		public static bool LineIsCloseToRect(MbeRect rc, Point p0, Point p1,int dist)
		{
			if (rc.Contains(p0)) return true;
			if (rc.Contains(p1)) return true;

			if (p0.Y == p1.Y) {	//線が水平
				int lineL;
				int lineR;

				if (p0.X < p1.X) {
					lineL = p0.X; lineR = p1.X;
				} else {
					lineL = p1.X; lineR = p0.X;
				}

				if (p0.Y > rc.T) {
					if (rc.L > lineR) {
						return PointIsCloseToPoint(new Point(rc.L, rc.T), new Point(lineR, p0.Y), dist);
					} else if (rc.R < lineL) {
						return PointIsCloseToPoint(new Point(rc.R, rc.T), new Point(lineL, p0.Y), dist);
					} else {
						return dist>(p0.Y - rc.T);
					}
				}

				if (rc.B > p0.Y) {
					if (rc.L > lineR) {
						return PointIsCloseToPoint(new Point(rc.L, rc.B), new Point(lineR, p0.Y), dist);
					} else if (rc.R < lineL) {
						return PointIsCloseToPoint(new Point(rc.R, rc.B), new Point(lineL, p0.Y), dist);
					} else {
						return dist>(rc.B - p0.Y);
					}
				}

				if (rc.L > lineR) {
					return dist>(rc.L - lineR);
				} else if (rc.R < lineL) {
					return dist>(lineL - rc.R);
				} else{
					return true;
				}
			}

			if (p0.X == p1.X) {	//線が垂直
				int lineB;
				int lineT;
				if (p0.Y < p1.Y) {
					lineB = p0.Y; lineT = p1.Y;
				} else {
					lineB = p1.Y; lineT = p0.Y;
				}

				if (p0.X > rc.R) {
					if (rc.B > lineT) {
						return PointIsCloseToPoint(new Point(rc.R, rc.B), new Point(p0.X, lineT), dist);
					} else if (rc.T < lineB) {
						return PointIsCloseToPoint(new Point(rc.R, rc.T), new Point(p0.X, lineB), dist);
					} else {
						return dist>(p0.X - rc.R);
					}
				}

				if (rc.L > p0.X) {
					if (rc.B > lineT) {
						return PointIsCloseToPoint(new Point(rc.L, rc.B), new Point(p0.X, lineT), dist);
					} else if (rc.T < lineB) {
						return PointIsCloseToPoint(new Point(rc.L, rc.T), new Point(p0.X, lineB), dist);
					} else {
						return dist>(rc.L - p0.X);
					}
				}

				if (rc.B > lineT) {
					return dist>(rc.B - lineT);
				} else if (rc.T < lineB) {
					return dist>(lineB - rc.T);
				} else {
					return true;
				}
			}
            

			//大雑把な判定
			//対象線と、矩形中心の距離
			//矩形短辺の半分より短いときは間違いなくtrue
			//矩形対角の半分より長いときは間違いなくfalse
			int longSide;
			int shortSide;
			if (rc.Width > rc.Height) {
				longSide = rc.Width;
				shortSide = rc.Height;
			} else {
				shortSide = rc.Width;
				longSide = rc.Height;
			}

			long hLongSide = longSide/2;
			long hShortSide = shortSide/2;
            //double hLongSide = longSide / 2;
            //double hShortSide = shortSide / 2;

			double dDist = DistancePointLine(rc.Center(), p0, p1);
			if (dDist < (hShortSide+dist)) return true;


            //if (dDist > (Math.Sqrt(hShortSide * hShortSide + hLongSide * hLongSide) + dist)) return false; //2008/3/15 0.29o
            if (((dDist-dist)*(dDist-dist)) > (hShortSide * hShortSide + hLongSide * hLongSide)) return false; //2008/7/15 0.35



			Point sideP0;
			Point sideP1;

			sideP0 = new Point(rc.L, rc.T); sideP1 = new Point(rc.R, rc.T);
			if (LineIsCloseToLine(sideP0, sideP1, p0, p1, dist)) return true;

			sideP0 = new Point(rc.L, rc.B); sideP1 = new Point(rc.R, rc.B);
			if (LineIsCloseToLine(sideP0, sideP1, p0, p1, dist)) return true;

			sideP0 = new Point(rc.L, rc.T); sideP1 = new Point(rc.L, rc.B);
			if (LineIsCloseToLine(sideP0, sideP1, p0, p1, dist)) return true;

			sideP0 = new Point(rc.R, rc.T); sideP1 = new Point(rc.R, rc.B);
			if (LineIsCloseToLine(sideP0, sideP1, p0, p1, dist)) return true;


			//if(PointIsCloseToLine(new Point(rc.L, rc.T), p0, p1,dist)) return true;
			//if(PointIsCloseToLine(new Point(rc.R, rc.T), p0, p1,dist)) return true;
			//if(PointIsCloseToLine(new Point(rc.L, rc.B), p0, p1,dist)) return true;
			//if(PointIsCloseToLine(new Point(rc.R, rc.B), p0, p1,dist)) return true;
			return false;

			//Point pc0;
			//Point pc1;
			//Point pc2;
			//Point pc3;


			//double dist0 = DistancePointLine(new Point(rc.L, rc.T), p0, p1, out pc0);
			//double dist1 = DistancePointLine(new Point(rc.R, rc.T), p0, p1, out pc1);
			//double dist2 = DistancePointLine(new Point(rc.L, rc.B), p0, p1, out pc2);
			//double dist3 = DistancePointLine(new Point(rc.R, rc.B), p0, p1, out pc3);

			//if (dist0 > dist1) {
			//    dist0 = dist1;
			//    pc0 = pc1;
			//}
			//if (dist2 > dist3) {
			//    dist2 = dist3;
			//    pc2 = pc3;
			//}
			//if (dist0 > dist2) {
			//    dist0 = dist2;
			//    pc0 = pc2;
			//}
			//ptMinCenter = pc0;
			//return dist0;
		}



		public static double DistanceRectLine(MbeRect rc, Point p0, Point p1, out Point ptMinCenter)
		{
			if (rc.Contains(p0)) {
				Point pc = rc.Center();
				ptMinCenter = new Point((pc.X+p0.X)/2, (pc.Y + p0.Y) / 2);
				return 0.0;
			}
			if (rc.Contains(p1)){
				Point pc = rc.Center();
				ptMinCenter = new Point((pc.X + p1.X) / 2, (pc.Y + p1.Y) / 2);
				return 0.0;
			}



			if (p0.Y == p1.Y) {	//線が水平
				int lineL;
				int lineR;
				if (p0.X < p1.X) {
					lineL = p0.X; lineR = p1.X;
				} else {
					lineL = p1.X; lineR = p0.X;
				}

				if (p0.Y > rc.T) {
					if (rc.L > lineR) {
						return DistancePointPoint(new Point(rc.L, rc.T), new Point(lineR, p0.Y), out ptMinCenter);
					} else if (rc.R < lineL) {
						return DistancePointPoint(new Point(rc.R, rc.T), new Point(lineL, p0.Y), out ptMinCenter);
					} else {
						int xc = OverlapCenter(rc.L, rc.R, lineL, lineR);
						int yc = (p0.Y + rc.T) / 2;
						ptMinCenter = new Point(xc, yc);
						return (p0.Y - rc.T);
					}
				}

				if (rc.B > p0.Y) {
					if (rc.L > lineR) {
						return DistancePointPoint(new Point(rc.L, rc.B), new Point(lineR, p0.Y), out ptMinCenter);
					} else if (rc.R < lineL) {
						return DistancePointPoint(new Point(rc.R, rc.B), new Point(lineL, p0.Y), out ptMinCenter);
					} else {
						int xc = OverlapCenter(rc.L, rc.R, lineL, lineR);
						int yc = (p0.Y + rc.B) / 2;
						ptMinCenter = new Point(xc, yc);
						return (rc.B - p0.Y);
					}
				}

				if (rc.L > lineR) {
					ptMinCenter = new Point((lineR + rc.L) / 2, p0.Y);
					return rc.L - lineR;
				} else if (rc.R < lineL) {
					ptMinCenter = new Point((lineL + rc.R) / 2, p0.Y);
					return lineL - rc.R;
				} else{
					Point rcc = rc.Center();

					ptMinCenter = new Point(rcc.X, (rcc.Y + p0.Y) / 2);
					return 0.0;
				}
			}

			if (p0.X == p1.X) {	//線が垂直
				int lineB;
				int lineT;
				if (p0.Y < p1.Y) {
					lineB = p0.Y; lineT = p1.Y;
				} else {
					lineB = p1.Y; lineT = p0.Y;
				}

				if (p0.X > rc.R) {
					if (rc.B > lineT) {
						return DistancePointPoint(new Point(rc.R, rc.B), new Point(p0.X, lineT), out ptMinCenter);
					} else if (rc.T < lineB) {
						return DistancePointPoint(new Point(rc.R, rc.T), new Point(p0.X, lineB), out ptMinCenter);
					} else {
						int xc = (p0.X + rc.R) / 2;
						int yc = OverlapCenter(rc.B, rc.T, lineB, lineT);
						ptMinCenter = new Point(xc, yc);
						return (p0.X - rc.R);
					}
				}

				if (rc.L > p0.X) {
					if (rc.B > lineT) {
						return DistancePointPoint(new Point(rc.L, rc.B), new Point(p0.X, lineT), out ptMinCenter);
					} else if (rc.T < lineB) {
						return DistancePointPoint(new Point(rc.L, rc.T), new Point(p0.X, lineB), out ptMinCenter);
					} else {
						int xc = (p0.X + rc.L) / 2;
						int yc = OverlapCenter(rc.B, rc.T, lineB, lineT);
						ptMinCenter = new Point(xc, yc);
						return (rc.L - p0.X);
					}
				}

				if (rc.B > lineT) {
					ptMinCenter = new Point(p0.X, (lineT + rc.B) / 2);
					return rc.B - lineT;
				} else if (rc.T < lineB) {
					ptMinCenter = new Point(p0.X,(lineB + rc.T) / 2);
					return lineB - rc.T;
				} else {
					Point rcc = rc.Center();

					ptMinCenter = new Point((rcc.X + p0.X) / 2,rcc.Y);
					return 0.0;
				}

			}

			Point pc0;
			Point pc1;
			Point pc2;
			Point pc3;

			Point sideP0;
			Point sideP1;

			sideP0 = new Point(rc.L, rc.T); sideP1 = new Point(rc.R, rc.T);
			double dist0 = DistanceLineLine(sideP0, sideP1, p0, p1, out pc0);
			
			sideP0 = new Point(rc.L, rc.B); sideP1 = new Point(rc.R, rc.B);
			double dist1 = DistanceLineLine(sideP0, sideP1, p0, p1, out pc1);

			sideP0 = new Point(rc.L, rc.T); sideP1 = new Point(rc.L, rc.B);
			double dist2 = DistanceLineLine(sideP0, sideP1, p0, p1, out pc2);

			sideP0 = new Point(rc.R, rc.T); sideP1 = new Point(rc.R, rc.B);
			double dist3 = DistanceLineLine(sideP0, sideP1, p0, p1, out pc3);




			//double dist0 = DistancePointLine(new Point(rc.L, rc.T), p0, p1, out pc0);
			//double dist1 = DistancePointLine(new Point(rc.R, rc.T), p0, p1, out pc1);
			//double dist2 = DistancePointLine(new Point(rc.L, rc.B), p0, p1, out pc2);
			//double dist3 = DistancePointLine(new Point(rc.R, rc.B), p0, p1, out pc3);

			if (dist0 > dist1) {
				dist0 = dist1;
				pc0 = pc1;
			}
			if (dist2 > dist3) {
				dist2 = dist3;
				pc2 = pc3;
			}
			if (dist0 > dist2) {
				dist0 = dist2;
				pc0 = pc2;
			}
			ptMinCenter = pc0;
			return dist0;
		}


		
		public static double DistanceRectRect(MbeRect rc0, MbeRect rc1, out Point ptMinCenter)
		{
			// rc1 の 右はずれに rc0がある
			if (rc0.L > rc1.R) {
				if (rc0.T < rc1.B) {
					return DistancePointPoint(new Point(rc0.L, rc0.T), new Point(rc1.R, rc1.B), out ptMinCenter);
				} else if (rc0.B > rc1.T) {
					return DistancePointPoint(new Point(rc0.L, rc0.B), new Point(rc1.R, rc1.T), out ptMinCenter);
				} else {
					int yc = OverlapCenter(rc0.B, rc0.T, rc1.B, rc1.T);
					int xc = (rc0.L + rc1.R) / 2;
					ptMinCenter = new Point(xc, yc);
					return (rc0.L - rc1.R);
				}
			}

			// rc1 の 左はずれに rc0がある
			if (rc1.L > rc0.R) {
				if (rc0.T < rc1.B) {
					return DistancePointPoint(new Point(rc0.R, rc0.T), new Point(rc1.L, rc1.B), out ptMinCenter);
				} else if (rc0.B > rc1.T) {
					return DistancePointPoint(new Point(rc0.R, rc0.B), new Point(rc1.L, rc1.T), out ptMinCenter);
				} else {
					int yc = OverlapCenter(rc0.B, rc0.T, rc1.B, rc1.T);
					int xc = (rc1.L + rc0.R) / 2;
					ptMinCenter = new Point(xc, yc);
					return (rc1.L - rc0.R);
				}
			}

			// rc0 の 上はずれに rc1がある
			if (rc1.B > rc0.T) {
				int xc = OverlapCenter(rc0.L, rc0.R, rc1.L, rc1.R);
				int yc = (rc1.B + rc0.T) / 2;
				ptMinCenter = new Point(xc, yc);
				return (rc1.B - rc0.T);
			}

			// rc0 の 下はずれに rc1がある
			if (rc0.B > rc1.T) {
				int xc = OverlapCenter(rc0.L, rc0.R, rc1.L, rc1.R);
				int yc = (rc0.B + rc1.T) / 2;
				ptMinCenter = new Point(xc, yc);
				return (rc0.B - rc1.T);
			}

			Point rc0c = rc0.Center();
			Point rc1c = rc1.Center();

			ptMinCenter = new Point((rc0c.X+rc1c.X)/2,(rc0c.Y+rc1c.Y)/2);
			return 0.0;
		}

        public static bool RectIsCloseToRect(MbeRect rc0, MbeRect rc1, int dist)
        {
            // rc1 の 右はずれに rc0がある
            if (rc0.L > rc1.R) {
                if (rc0.T < rc1.B) {
                    return PointIsCloseToPoint(new Point(rc0.L, rc0.T), new Point(rc1.R, rc1.B), dist);
                    //return DistancePointPoint(new Point(rc0.L, rc0.T), new Point(rc1.R, rc1.B), out ptMinCenter);
                } else if (rc0.B > rc1.T) {
                    return Util.PointIsCloseToPoint(new Point(rc0.L, rc0.B), new Point(rc1.R, rc1.T), dist);
                    //return DistancePointPoint(new Point(rc0.L, rc0.B), new Point(rc1.R, rc1.T), out ptMinCenter);
                } else {
                    //int yc = OverlapCenter(rc0.B, rc0.T, rc1.B, rc1.T);
                    //int xc = (rc0.L + rc1.R) / 2;
                    //ptMinCenter = new Point(xc, yc);
                    return (rc0.L - rc1.R)<dist;
                }
            }

            // rc1 の 左はずれに rc0がある
            if (rc1.L > rc0.R) {
                if (rc0.T < rc1.B) {
                    return PointIsCloseToPoint(new Point(rc0.R, rc0.T), new Point(rc1.L, rc1.B), dist);
                    //return DistancePointPoint(new Point(rc0.R, rc0.T), new Point(rc1.L, rc1.B), out ptMinCenter);
                } else if (rc0.B > rc1.T) {
                    return PointIsCloseToPoint(new Point(rc0.R, rc0.B), new Point(rc1.L, rc1.T), dist);
                    //return DistancePointPoint(new Point(rc0.R, rc0.B), new Point(rc1.L, rc1.T), out ptMinCenter);
                } else {
                    //int yc = OverlapCenter(rc0.B, rc0.T, rc1.B, rc1.T);
                    //int xc = (rc1.L + rc0.R) / 2;
                    //ptMinCenter = new Point(xc, yc);
                    return (rc1.L - rc0.R)<dist;
                }
            }

            // rc0 の 上はずれに rc1がある
            if (rc1.B > rc0.T) {
                //int xc = OverlapCenter(rc0.L, rc0.R, rc1.L, rc1.R);
                //int yc = (rc1.B + rc0.T) / 2;
                //ptMinCenter = new Point(xc, yc);
                return (rc1.B - rc0.T)<dist;
            }

            // rc0 の 下はずれに rc1がある
            if (rc0.B > rc1.T) {
                //int xc = OverlapCenter(rc0.L, rc0.R, rc1.L, rc1.R);
                //int yc = (rc0.B + rc1.T) / 2;
                //ptMinCenter = new Point(xc, yc);
                return (rc0.B - rc1.T)<dist;
            }

            //Point rc0c = rc0.Center();
            //Point rc1c = rc1.Center();

            //ptMinCenter = new Point((rc0c.X + rc1c.X) / 2, (rc0c.Y + rc1c.Y) / 2);
            //return 0.0;
            return true;
        }






		/// <summary>
		/// p0が、rcの上下左右のいずれかに外れているときtrueを返す
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <param name="rc"></param>
		/// <returns></returns>
		public static bool PointIsOutsideLTRB(Point pt0, MbeRect rc)
		{
			if ((pt0.X < rc.L) ||
			   (pt0.X > rc.R) ||
			   (pt0.Y < rc.B) ||
			   (pt0.Y > rc.T)) {
				return true;
			} else {
				return false;
			}
		}


		/// <summary>
		/// p0,p1の端点の線分が、rcの上下左右のいずれかに外れているときtrueを返す
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <param name="rc"></param>
		/// <returns></returns>
		public static bool LineIsOutsideLTRB(Point pt0, Point pt1, MbeRect rc)
		{
			if ((pt0.X < rc.L && pt1.X < rc.L) ||
			   (pt0.X > rc.R && pt1.X > rc.R) ||
			   (pt0.Y < rc.B && pt1.Y < rc.B) ||
			   (pt0.Y > rc.T && pt1.Y > rc.T)) {
				return true;
			} else {
				return false;
			}
		}



		/// <summary>
		/// 線分がDoubleで与えられるY値と交差するX値を得る。
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <param name="y"></param>
		/// <param name="x"></param>
		/// <returns>交差するときはtrue</returns>
		/// <remarks>ポリゴン塗りつぶしの最初のステップで使う</remarks>
		public static bool LineCrossingY(Point p0, Point p1, double y, out double x)
		{
			x = 0.0;

			if (p0.Y == p1.Y) {	//線分が水平なら交差しない。
				return false;
			}
			if(!IsInRange(y,p0.Y,p1.Y)){ //線分がyを跨がない場合は交差しない
				return false;
			}
			if(p0.X == p1.X){	//線分が垂直のとき
				x=p0.X;
				return true;
			}
			double a =(double)(p1.Y - p0.Y) / (double)(p1.X - p0.X);
			double b =(double)p0.Y - a * p0.X;
			x= (y-b)/a;
			return true;
		}


        /// <summary>
        /// 線分がDoubleで与えられるX値と交差するY値を得る。
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns>交差するときはtrue</returns>
        /// <remarks>ポリゴン塗りつぶしの最初のステップで使う</remarks>
        public static bool LineCrossingX(Point p0, Point p1, double x, out double y)
        {
            y = 0.0;

            if (p0.X == p1.X) {	//線分が垂直なら交差しない。
                return false;
            }
            if (!IsInRange(x, p0.X, p1.X)) { //線分がxを跨がない場合は交差しない
                return false;
            }
            if (p0.Y == p1.Y) {	//線分が水平のとき
                y = p0.Y;
                return true;
            }
            double a = (double)(p1.Y - p0.Y) / (double)(p1.X - p0.X);
            double b = (double)p0.Y - a * p0.X;
            y = a * x + b;
            //x = (y - b) / a;
            return true;
        }



		public static bool LineCrossing(Point p00, Point p01, Point p10, Point p11, ref Point ptCrossing)
		{
			//ptCrossing = new Point(0, 0);



			//交差または重なった場合は距離はゼロになる。
            //if (!(p00.X >= p10.X && p01.X >= p10.X && p00.X >= p11.X && p01.X >= p11.X) && //(線分0の両Xが線分1の両Xよりも大)でないこと
            //    !(p00.X <= p10.X && p01.X <= p10.X && p00.X <= p11.X && p01.X <= p11.X) && //(線分0の両Xが線分1の両Xよりも小)でないこと
            //    !(p00.Y >= p10.Y && p01.Y >= p10.Y && p00.Y >= p11.Y && p01.Y >= p11.Y) && //(線分0の両Yが線分1の両Yよりも大)でないこと
            //    !(p00.Y <= p10.Y && p01.Y <= p10.Y && p00.Y <= p11.Y && p01.Y <= p11.Y)) { //(線分0の両Yが線分1の両Yよりも小)でないこと
            if (!(p00.X > p10.X && p01.X > p10.X && p00.X > p11.X && p01.X > p11.X) && //(線分0の両Xが線分1の両Xよりも大)でないこと
                !(p00.X < p10.X && p01.X < p10.X && p00.X < p11.X && p01.X < p11.X) && //(線分0の両Xが線分1の両Xよりも小)でないこと
                !(p00.Y > p10.Y && p01.Y > p10.Y && p00.Y > p11.Y && p01.Y > p11.Y) && //(線分0の両Yが線分1の両Yよりも大)でないこと
                !(p00.Y < p10.Y && p01.Y < p10.Y && p00.Y < p11.Y && p01.Y < p11.Y)) { //(線分0の両Yが線分1の両Yよりも小)でないこと

                //if (p00.X == p10.X && p00.Y == p10.Y ||
                //   p00.X == p11.X && p00.Y == p11.Y ||
                //   p01.X == p10.X && p01.Y == p10.Y ||
                //   p01.X == p11.X && p01.Y == p11.Y) return false;
                
                //交差または重なりチェック
				if ((p00.Y == p01.Y && p10.Y == p11.Y) || (p00.X == p01.X && p10.X == p11.X)) {//両方の線分が水平または垂直の場合
					//ptCrossing = new Point(0, 0);
					return false;
				} else {
					//線分の係数を求める y=ax+b
					double a0 = 0;
					double b0 = 0;
					double a1 = 0;
					double b1 = 0;
					double xc = 0;
					double yc = 0;

					if (p01.X != p00.X) {
						a0 = (double)(p01.Y - p00.Y) / (double)(p01.X - p00.X);
						b0 = (double)p00.Y - a0 * p00.X;
					}
					if (p11.X != p10.X) {
						a1 = (double)(p11.Y - p10.Y) / (double)(p11.X - p10.X);
						b1 = (double)p10.Y - a1 * p10.X;
					}

					if (p00.X == p01.X) { //線分0が垂直
						yc = a1 * p00.X + b1;
                        int nyc = (int)Math.Round(yc);
						if (IsInRangeN(nyc, p00.Y, p01.Y)) {
                            ptCrossing = new Point(p00.X, nyc);
							//ptCrossing.X = p00.X;
							//ptCrossing.Y = (int)yc;
							return true;
						} else {
							//ptCrossing = new Point(0, 0);
							return false;
						}
					}

					if (p10.X == p11.X) { //線分1が垂直
						yc = a0 * p10.X + b0;
                        int nyc = (int)Math.Round(yc);
						if (IsInRangeN(nyc, p10.Y, p11.Y)) {
                            ptCrossing = new Point(p10.X, nyc);
							//ptCrossing.X = p10.X;
							//ptCrossing.Y = (int)yc;
							return true;
						} else {
							//ptCrossing = new Point(0, 0);
							return false;
						}
					}

					if (a0 == a1) {	//線分の傾きが同じ場合
						//ptCrossing = new Point(0, 0);
						return false;
					}

					xc = (b1 - b0) / (a0 - a1);
                    int nxc = (int)Math.Round(xc);
					if (IsInRangeN(nxc, p00.X, p01.X) && IsInRangeN(nxc, p10.X, p11.X)) {
					//if (IsInRange(xc, p00.X, p01.X) && IsInRange(xc, p10.X, p11.X)) {
					    yc = a0 * xc + b0;
                        //int nxc = (int)Math.Round(xc);
                        int nyc = (int)Math.Round(yc);
						ptCrossing = new Point(nxc, nyc);
						//ptCrossing.X = (int)xc;
						//ptCrossing.Y = (int)yc;
						return true;
					} else {
						//ptCrossing = new Point(0, 0);
						return false;
					}
				}
			} else {
				//ptCrossing = new Point(0, 0);
				return false;
			}
		}


		public static bool LineIsCloseToLine(Point p00, Point p01, Point p10, Point p11, int dist)
		{
			int n;
			int m;

			n= p10.X-dist;
			m= p11.X-dist;
			if(p00.X<n && p01.X<n && p00.X<m && p01.X<m) return false;

  			n= p10.X+dist;
			m= p11.X+dist;
			if(p00.X>n && p01.X>n && p00.X>m && p01.X>m) return false;

			n= p10.Y-dist;
			m= p11.Y-dist;
			if(p00.Y<n && p01.Y<n && p00.Y<m && p01.Y<m) return false;

  			n= p10.Y+dist;
			m= p11.Y+dist;
			if(p00.Y>n && p01.Y>n && p00.Y>m && p01.Y>m) return false;


			//交差または重なった場合は距離はゼロになる。
			if (!(p00.X > p10.X && p01.X > p10.X && p00.X > p11.X && p01.X > p11.X) && //(線分0の両Xが線分1の両Xよりも大)でないこと
				!(p00.X < p10.X && p01.X < p10.X && p00.X < p11.X && p01.X < p11.X) && //(線分0の両Xが線分1の両Xよりも小)でないこと
				!(p00.Y > p10.Y && p01.Y > p10.Y && p00.Y > p11.Y && p01.Y > p11.Y) && //(線分0の両Yが線分1の両Yよりも大)でないこと
				!(p00.Y < p10.Y && p01.Y < p10.Y && p00.Y < p11.Y && p01.Y < p11.Y)) { //(線分0の両Yが線分1の両Yよりも小)でないこと

				//交差または重なりチェック
				if ((p00.Y == p01.Y && p10.Y == p11.Y) || (p00.X == p01.X && p10.X == p11.X)) {//両方の線分が水平または垂直の場合
					return true;
				} else {
					//線分の係数を求める y=ax+b
					double a0 = 0;
					double b0 = 0;
					double a1 = 0;
					double b1 = 0;
					double xc = 0;
					double yc = 0;

					if (p01.X != p00.X) {
						a0 = (double)(p01.Y - p00.Y) / (double)(p01.X - p00.X);
						b0 = (double)p00.Y - a0 * p00.X;
					}
					if (p11.X != p10.X) {
						a1 = (double)(p11.Y - p10.Y) / (double)(p11.X - p10.X);
						b1 = (double)p10.Y - a1 * p10.X;
					}

					if (p00.X == p01.X) { //線分0が垂直
						yc = a1 * p00.X + b1;
						if (IsInRange(yc, p00.Y, p01.Y)) {
							return true;
						}
					}

					if (p10.X == p11.X) { //線分1が垂直
						yc = a0 * p10.X + b0;
						if (IsInRange(yc, p10.Y, p11.Y)) {
							return true;
						}
					}

					if (a0 == a1) {	//線分の傾き、オフセットが同じ場合
						if (b0 == b1) {
							return true;
						}
					}

					xc = (b1 - b0) / (a0 - a1);
					if (IsInRange(xc, p00.X, p01.X) && IsInRange(xc, p10.X, p11.X)) {
						return true;
					}
				}
			}

			//Point ptCen00;
			//Point ptCen01;
			//Point ptCen10;
			//Point ptCen11;

			if(PointIsCloseToLine(p00, p10, p11,dist)) return true;
			if(PointIsCloseToLine(p01, p10, p11,dist)) return true;
			if(PointIsCloseToLine(p10, p00, p01,dist)) return true;
			if(PointIsCloseToLine(p11, p00, p01,dist)) return true;

			return false;
			//double dist00 = DistancePointLine(p00, p10, p11, out ptCen00);
			//double dist01 = DistancePointLine(p01, p10, p11, out ptCen01);
			//double dist10 = DistancePointLine(p10, p00, p01, out ptCen10);
			//double dist11 = DistancePointLine(p11, p00, p01, out ptCen11);

			//if(dist00 > dist01){
			//    dist00 = dist01;
			//    ptCen00 = ptCen01;
			//}
			//if(dist10 > dist11){
			//    dist10 = dist11;
			//    ptCen10 = ptCen11;
			//}

			//if (dist00 < dist10) {
			//    ptMinCenter = ptCen00;
			//    return dist00;
			//} else {
			//    ptMinCenter = ptCen10;
			//    return dist10;
			//}

		}
	



		/// <summary>
		/// 線分と線分の距離を返す
		/// </summary>
		/// <param name="p00">線分0の始点</param>
		/// <param name="p01">線分0の終点</param>
		/// <param name="p10">線分1の始点</param>
		/// <param name="p11">線分1の終点</param>
		/// <returns></returns>
		public static double DistanceLineLine(Point p00, Point p01, Point p10, Point p11, out Point ptMinCenter)
		{
			//if (LineCrossing(p00, p01, p10, p11, out ptMinCenter)) {
			//    return 0.0;
			//}

			//交差または重なった場合は距離はゼロになる。
			if (!(p00.X > p10.X && p01.X > p10.X && p00.X > p11.X && p01.X > p11.X) && //(線分0の両Xが線分1の両Xよりも大)でないこと
				!(p00.X < p10.X && p01.X < p10.X && p00.X < p11.X && p01.X < p11.X) && //(線分0の両Xが線分1の両Xよりも小)でないこと
				!(p00.Y > p10.Y && p01.Y > p10.Y && p00.Y > p11.Y && p01.Y > p11.Y) && //(線分0の両Yが線分1の両Yよりも大)でないこと
				!(p00.Y < p10.Y && p01.Y < p10.Y && p00.Y < p11.Y && p01.Y < p11.Y)) { //(線分0の両Yが線分1の両Yよりも小)でないこと

				//交差または重なりチェック
				if ((p00.Y == p01.Y && p10.Y == p11.Y) || (p00.X == p01.X && p10.X == p11.X)) {//両方の線分が水平または垂直の場合
					ptMinCenter = new Point((p00.X + p01.X + p10.X + p11.X) / 4, (p00.Y + p01.Y + p10.Y + p11.Y) / 4);
					return 0.0;
				} else {
					//線分の係数を求める y=ax+b
					double a0 = 0;
					double b0 = 0;
					double a1 = 0;
					double b1 = 0;
					double xc = 0;
					double yc = 0;

					if (p01.X != p00.X) {
						a0 = (double)(p01.Y - p00.Y) / (double)(p01.X - p00.X);
						b0 = (double)p00.Y - a0 * p00.X;
					}
					if (p11.X != p10.X) {
						a1 = (double)(p11.Y - p10.Y) / (double)(p11.X - p10.X);
						b1 = (double)p10.Y - a1 * p10.X;
					}

					if (p00.X == p01.X) { //線分0が垂直
						yc = a1 * p00.X + b1;
						if (IsInRange(yc, p00.Y, p01.Y)) {
                            ptMinCenter = new Point(p00.X, (int)Math.Round(yc));
							return 0.0;
						}
					}

					if (p10.X == p11.X) { //線分1が垂直
						yc = a0 * p10.X + b0;
						if (IsInRange(yc, p10.Y, p11.Y)) {
                            ptMinCenter = new Point(p10.X, (int)Math.Round(yc));
							return 0.0;
						}
					}

					if (a0 == a1) {	//線分の傾き、オフセットが同じ場合
						if (b0 == b1) {
							ptMinCenter = new Point((p00.X + p01.X + p10.X + p11.X) / 4, (p00.Y + p01.Y + p10.Y + p11.Y) / 4);
							return 0.0;
						}
					}

					xc = (b1 - b0) / (a0 - a1);
					if (IsInRange(xc, p00.X, p01.X) && IsInRange(xc, p10.X, p11.X)) {
						yc = a0 * xc + b0;
                        ptMinCenter = new Point((int)Math.Round(xc), (int)Math.Round(yc));
						return 0.0;
					}
				}
			}

			Point ptCen00;
			Point ptCen01;
			Point ptCen10;
			Point ptCen11;

			double dist00 = DistancePointLine(p00, p10, p11, out ptCen00);
			double dist01 = DistancePointLine(p01, p10, p11, out ptCen01);
			double dist10 = DistancePointLine(p10, p00, p01, out ptCen10);
			double dist11 = DistancePointLine(p11, p00, p01, out ptCen11);

			if(dist00 > dist01){
				dist00 = dist01;
				ptCen00 = ptCen01;
			}
			if(dist10 > dist11){
				dist10 = dist11;
				ptCen10 = ptCen11;
			}

			if (dist00 < dist10) {
				ptMinCenter = ptCen00;
				return dist00;
			} else {
				ptMinCenter = ptCen10;
				return dist10;
			}

		}
	}

	enum OutlineTag
	{
		L1B = 0,
		L1E,
		CEL1,
		CEL2,
		L2E,
		L2B,
		CBL2,
		CBL1
	}
}
