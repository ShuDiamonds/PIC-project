using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace mbe
{
	partial class MbeView
	{
		#region �f�t�H���g�J���[�萔
		public const Int32 DEFAULT_COLOR_ORIGIN_MARK = 0x808080;
		#endregion

		protected Int32 colorOriginMark = DEFAULT_COLOR_ORIGIN_MARK;
		

		//���_�w��F�̐ݒ�Ǝ擾
		public Int32 ColorOriginMark
		{
			get
			{
				return colorOriginMark;
			}
			set
			{
				colorOriginMark = value;
			}
		}


		//protected void InitColor()
		//{
		//    //Color colOriginMark = Color.FromArgb(colorOriginMark);
		//}


	}

}
