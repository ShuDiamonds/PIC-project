using System;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace mbe
{
	public class CamOutBaseData
	{
        /// <summary>
        /// ���H���@�������B�h�������A1�_�t���b�V���A�ړ��𔺂��x�N�g���f�[�^
        /// </summary>
		public enum CamType
		{
			DRILL,
			FLASH,
			VECTOR
		}

        /// <summary>
        /// �`��������B�h�������A���b�L�t�h�������A�K�[�o�[��`�A�K�[�o�[�U�^�A�ǂ�ɂ������Ȃ�Err
        /// </summary>
		public enum Shape
		{
			Drill,
			DrillPTH,
			Rect,
			Obround,
            Err
		}

		public CamOutBaseData(
			MbeLayer.LayerValue _layer,
			CamType _ctype,
			Shape _shape,
			int _width,
			int _height,
			Point _pt0,
			Point _pt1)
		{
			layer = _layer;
			code = "";
			ctype = _ctype;
			shape = _shape;
			width = _width;
			height = _height;
			pt0 = _pt0;
			pt1 = _pt1;
		}

		public void Move(Point offset)
		{
			pt0.Offset(offset);
			pt1.Offset(offset);
		}

		protected Point RotateStep90(Point pt, int dir, Point ptCenter)
		{
			if (dir == 0) {
				return pt;
			} else {
				int x;
				int y;

				int xd = pt.X - ptCenter.X;
				int yd = pt.Y - ptCenter.Y;

				if (dir == 90) {
					x = -yd;
					y = xd;
				} else if (dir == 180) {
					x = -xd;
					y = -yd;
				} else if (dir == 270) {
					x = yd;
					y = -xd;
				} else {
					return pt;
				}
				return new Point(x + ptCenter.X, y + ptCenter.Y);
			}
		}

		public void RotateStep90(int dir,Point ptCenter)
		{

			pt0 = RotateStep90(pt0, dir, ptCenter);
			pt1 = RotateStep90(pt1, dir, ptCenter);

			//int x;
			//int y;
			//x = pt0.X - ptCenter.X;
			//y = pt0.Y - ptCenter.Y;
			//pt0 = new Point(-y + ptCenter.X, x + ptCenter.Y);

			//x = pt1.X - ptCenter.X;
			//y = pt1.Y - ptCenter.Y;
			//pt1 = new Point(-y + ptCenter.X, x + ptCenter.Y);
		}

		public void Mirror(int hCenter)
		{
			int x;
			x = hCenter - (pt0.X - hCenter);
			pt0.X = x;
			x = hCenter - (pt1.X - hCenter);
			pt1.X = x;
		}



		public MbeLayer.LayerValue layer;
		public string code;
        /// <summary>
        /// ���H���@�������B�h�������A1�_�t���b�V���A�ړ��𔺂��x�N�g���f�[�^
        /// </summary>
		public CamType ctype;
        /// <summary>
        /// �`��������B�h�������A���b�L�t�h�������A�K�[�o�[��`�A�K�[�o�[�U�^�A�ǂ�ɂ������Ȃ�Err
        /// </summary>
		public Shape shape;
        /// <summary>
        /// �A�p�[�`���̕��Bwidth�̓h�����̏ꍇ�͒��a��\��
        /// </summary>
		public int width;
        /// <summary>
        /// �A�p�[�`���̍����B
        /// </summary>
		public int height;
		public Point pt0;
		public Point pt1;
	}
}
