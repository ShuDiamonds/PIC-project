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
		/// �{�^���������W�z��̃T�C�Y
		/// </summary>
		protected const int PT_ARY_LENGTH = 64;

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public PointLog()
		{
			ptAry = new Point[PT_ARY_LENGTH];
			ptIndex = 0;
		}

		/// <summary>
		/// �C���f�b�N�X�̏�����
		/// </summary>
		public void InitIndex()
		{
			ptIndex = 0;
		}

		/// <summary>
		/// ���݂̃C���f�b�N�X�l��Ԃ�
		/// </summary>
		public int CurrentIndex
		{
			get { return ptIndex; }
		}
		
		/// <summary>
		/// ���W��ǉ�����
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
		/// �w�肵���C���f�b�N�X�̍��W���擾����
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
