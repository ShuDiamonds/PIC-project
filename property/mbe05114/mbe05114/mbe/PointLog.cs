using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
//using System.Drawing.Drawing2D;

namespace mbe
{
	class PointLog
	{
		/// <summary>
		/// ボタン押下座標配列のサイズ
		/// </summary>
		protected const int PT_ARY_LENGTH = 64;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public PointLog()
		{
			ptAry = new Point[PT_ARY_LENGTH];
			ptIndex = 0;
		}

		/// <summary>
		/// インデックスの初期化
		/// </summary>
		public void InitIndex()
		{
			ptIndex = 0;
		}

		/// <summary>
		/// 現在のインデックス値を返す
		/// </summary>
		public int CurrentIndex
		{
			get { return ptIndex; }
		}
		
		/// <summary>
		/// 座標を追加する
		/// </summary>
		/// <param name="pt"></param>
		public void SetPoint(Point pt)
		{
			ptAry[ptIndex++] = pt;
			if (ptIndex >= PT_ARY_LENGTH) {
				ptIndex = PT_ARY_LENGTH - 1;
			}
		}

		/// <summary>
		/// 指定したインデックスの座標を取得する
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Point this[int index]
		{
			get
			{
				if (index >= ptIndex) index = ptIndex - 1;
				if (index < 0) index = 0;
				return ptAry[index];
			}
		}

		protected Point[] ptAry;
		protected int ptIndex;

	}

}
