using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
	public struct MbePointInfo
	{
		public ulong layer;
		public Point point;
		public bool selectFlag;
        public MbeObj componentPinObj;
 	}

	public enum MbeDrawMode
	{
		Draw = 0,
		Print,
		Temp
	}

    public enum MbeDrawOption
    {
        NoOption = 0,
        CenterPunchMode = 0x0001,
        ToolMarkMode    = 0x0002,
        PrintColor      = 0x0004
    }



	public class DrawParam
	{
        public DrawParam()
        {
            option = (uint)MbeDrawOption.NoOption;
        }

		public Graphics g;
		public double scale;
		public MbeLayer.LayerValue layer;
		public MbeDrawMode mode;
        public uint option;                // 2008/09/19�ǉ�
		public ulong visibleLayer;	    // 2008/02/26�ǉ�
	}


	/// <summary>
	/// MbeObj�̎�ނ��Ƃ�ID
	/// </summary>
	public enum MbeObjID
	{
		MbeBase		= 0,
		MbePTH, 
		MbePinSMD,
		MbeLine,
		MbeArc,
		MbeText,
		MbeHole,
		MbeComponent,
		MbePolygon
	}

	public abstract class MbeObj
	{
		protected const int MBE_OBJ_ALPHA = 0x80;
        protected const int MBE_OBJ_PRINT_ALPHA = 0x80;



		protected int addCount;

		/// <summary>
		/// �폜�����Ƃ��̑���J�E���g��ێ�����
		/// </summary>
		/// <remarks>
		/// �폜�O�͕��̐��B
		/// �폜�O�̒ʏ�̒l�� -1�BNet�쐬��DRC�ňꎞ�I�ɑ��̕��l��^���邱�Ƃ�����B
		/// �폜����ƁA0�ȏ�̑���J�E���g��ێ�����B
		/// MainList�̃X�L�����ȂǂŁA�폜���݂��ǂ����𔻒肷��̂́A0�ȏォ�ۂ��ōs���B
		/// -1�Əƍ����Ȃ��B
		/// </remarks>
		protected int deleteCount;


		protected Point[] posArray;
		protected bool[] selectFlag;
		protected int posCount;

		protected MbeLayer.LayerValue layer;
		protected string signame;
		protected ulong snapLayer;

		protected static bool drawSnapMark = true;
		protected static bool drawPinNum = true;

		protected bool connectionCheckActive;

		/// <summary>
		/// �ꎞ�I�ȃv���p�e�B�p������
		/// </summary>
		/// <remarks>
		/// �t�@�C���ۑ��̑ΏۂƂ��Ȃ�
		/// �p�r
		/// �ENet�쐬�ADRC�̍ۂɕ��i���璊�o�����s���ɕ��i����ێ�������
		/// </remarks>
		protected string strTempProp;




		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		protected  MbeObj()
		{
			Layer = MbeLayer.LayerValue.CMP;
			addCount	= -1;
			deleteCount = -1;
			signame = "";
			snapLayer = 0;
			connectionCheckActive = false;
			strTempProp = "";
		}

		/// <summary>
		/// �R�s�[�R���X�g���N�^
		/// </summary>
		/// <param name="mbeObj"></param>
		protected  MbeObj(MbeObj mbeObj)
		{
			posCount = mbeObj.posCount;
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			for (int i = 0; i < posCount; i++) {
				posArray[i] = mbeObj.posArray[i];
				selectFlag[i] = mbeObj.selectFlag[i];
			}
			layer = MbeLayer.LayerValue.NUL;
			Layer = mbeObj.layer;
			signame = mbeObj.signame;
			addCount = mbeObj.addCount;
			deleteCount = mbeObj.deleteCount;
			connectionCheckActive = mbeObj.connectionCheckActive;
			strTempProp = mbeObj.strTempProp;
		}

		/// <summary>
		/// �����ꏊ�ɓ����I�u�W�F�N�g���d�Ȃ�̂�h��
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>��r�ΏۂƏd�Ȃ�Ȃ��Ƃ���true</returns>
		public virtual bool CheckRejectOverlay(MbeObj obj)
		{
			if (Id() != obj.Id()) return false;
			for (int i = 0; i < posCount; i++) {
				if (posArray[i] != obj.posArray[i]) return false;
			}
			if (Layer != obj.layer) return false;
			return true;
		}


		/// <summary>
		/// MBE�̃f�[�^�ɒǉ������Ƃ��̍�ƃJ�E���g�̎擾�Ɛݒ�
		/// </summary>
		/// <remarks>
		/// �擾�l��-1�͖��ݒ�B
		/// �ݒ�͐��̒l�̂݉\�B
		/// </remarks>
		public int AddCount
		{
			get
			{
				return addCount;
			}
			set
			{
				if (value < 0) value = 0;
				addCount = value;
			}
		}

		/// <summary>
		/// MBE�̃f�[�^����폜�����Ƃ��̍�ƃJ�E���g�̎擾�Ɛݒ�
		/// </summary>
		/// <remarks>
		/// �擾�l��-1�͖��ݒ�B
		/// �ݒ�͐��̒l�̂݉\�B
		/// </remarks>
		public int DeleteCount
		{
			get
			{
				return deleteCount;
			}
			set
			{
				deleteCount = value;
			}
		}

		/// <summary>
		/// MBE�̃f�[�^����폜�����Ƃ��̍�ƃJ�E���g�𖢐ݒ�ɖ߂�
		/// </summary>
		public void ClearDeleteCount()
		{
			deleteCount = -1;
		}

		/// <summary>
		/// ���C���[�l�̎擾�Ɛݒ�
		/// </summary>
		public virtual MbeLayer.LayerValue Layer
		{
			get
			{
				return layer;
			}
			set
			{
				layer = value;
				snapLayer = (ulong)layer;
			}
		}

 

        public ulong SnapLayer()
        {
            return snapLayer;
        }


		/// <summary>
		/// �\�������ׂ����C���[
		/// </summary>
		/// <returns></returns>
		public virtual ulong ShouldBeVisibleLayer()
		{
			return (ulong)layer;
		}


		public virtual MbePointInfo[] GetSnapPointArray()
		{

			MbePointInfo[] infoArray = new MbePointInfo[posCount];
			for (int i = 0; i < posCount; i++) {
				MbePointInfo sp;
				//sp.layer = snapLayer;
                sp.layer = (ulong)layer;
				sp.point = GetPos(i);
				sp.selectFlag = false;
                sp.componentPinObj = null;
				infoArray[i] = sp;
			}
			return infoArray;
		}

        public virtual MbePointInfo[] GetSnapPointArrayForMeasure()
        {
            return GetSnapPointArray();
        }



		public virtual MbePointInfo[] GetPosInfoArray()
		{

			MbePointInfo[] infoArray = new MbePointInfo[posCount];
			for (int i = 0; i < posCount; i++) {
				MbePointInfo sp;
				//sp.layer = snapLayer;
                sp.layer = (ulong)layer;
				sp.point = GetPos(i);
				sp.selectFlag = selectFlag[i];
                sp.componentPinObj = null;
				infoArray[i] = sp;
			}
			return infoArray;
		}
		


		/// <summary>
		/// �w�肵���C���f�b�N�X�̈ʒu�̐ݒ�
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		public virtual  void SetPos(Point pos, int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			posArray[index] = pos;
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X��X�ʒu�̐ݒ�
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		protected void SetXPos(int xpos, int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			posArray[index].X = xpos;
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X��Y�ʒu�̐ݒ�
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		protected void SetYPos(int ypos, int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			posArray[index].Y = ypos;
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�̈ʒu�̎擾
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		public Point GetPos(int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			return posArray[index];
		}

		public int PosCount
		{
			get { return posCount; }
		}


		/// <summary>
		/// �ړ�
		/// </summary>
		/// <param name="selectedOnly">�I���t���O�������Ă�����̂����ړ�����ꍇ��true</param>
		/// <param name="offset">�ړ���</param>
		public virtual void Move(bool selectedOnly, Point offset, Point ptAbs, bool moveSingle)
		{
			for (int i = 0; i < posCount; i++) {
				if (!selectedOnly || selectFlag[i]) {
					posArray[i].Offset(offset);
				}
			}
		}

		/// <summary>
		/// ��]
		/// </summary>
		/// <param name="selectedOnly"></param>
		/// <param name="ptCenter"></param>
		public virtual void Rotate90(bool selectedOnly, Point ptCenter)
		{
			for (int i = 0; i < posCount; i++) {
				if (!selectedOnly || selectFlag[i]) {
					int x=posArray[i].X - ptCenter.X;
					int y=posArray[i].Y - ptCenter.Y;
					int newx = -y + ptCenter.X;
					int newy = x  + ptCenter.Y;
					posArray[i] = new Point(newx,newy);
				}
			}
		}

		public virtual void Flip(int hCenter)
		{
			for (int i = 0; i < posCount; i++) {
				int x = hCenter - (posArray[i].X - hCenter);
				int y = posArray[i].Y;
				posArray[i] = new Point(x, y);
			}
			Layer = MbeLayer.Flip(layer);
		}







		/// <summary>
		/// �w�肵���C���f�b�N�X�̈ʒu�̑I���t���O�̎擾
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		public bool GetSelectFlag(int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			return selectFlag[index];
		}

		/// <summary>
		/// �S�Ă̑I���t���O�̃Z�b�g
		/// </summary>
		public virtual void SetAllSelectFlag()
		{
			for (int i = 0; i < posCount; i++) {
				selectFlag[i] = true;
			}
		}

		/// <summary>
		/// �S�Ă̑I���t���O�̃N���A
		/// </summary>
		public virtual void ClearAllSelectFlag()
		{
			for (int i = 0; i < posCount; i++) {
				selectFlag[i] = false;
			}
		}

		/// <summary>
		/// �I������Ă��邩�ǂ����̔���
		/// </summary>
		/// <returns>�I������Ă���Ƃ�true��Ԃ�</returns>
		public virtual bool IsSelected()
		{
			for (int i = 0; i < posCount; i++) {
				if(selectFlag[i])return true;
			}
			return false;
		}

		/// <summary>
		/// �M�����̐ݒ�Ǝ擾
		/// </summary>
		public string SigName
		{
			get
			{
				return signame;
			}
			set
			{
				signame = value;
			}
		}

		/// <summary>
		/// �L�����ǂ�����Ԃ�
		/// </summary>
		/// <returns></returns>
		public virtual bool IsValid()
		{
			return true;
		}


		/// <summary>
		/// �}�ʃI�u�W�F�N�g���Ƃ�ID�l��Ԃ�
		/// </summary>
		public abstract MbeObjID Id();

		/// <summary>
		/// CAM�f�[�^�̐��� 
		/// </summary>
		/// <param name="camOut"></param>
		public abstract void GenerateCamData(CamOut camOut);

		/// <summary>
		/// �|���S���̂��߂̗֊s�f�[�^�𐶐�����B
		/// </summary>
		/// <param name="outlineList"></param>
		/// <param name="param"></param>
		/// <remarks>�����Ő�������֊s�f�[�^�͓����ɏd�Ȃ��Ă��Ă��ǂ����̂Ƃ���</remarks>
		public virtual void GenerateOutlineData(LinkedList<MbeGapChkObjLine> outlineList, GenOutlineParam param)
		{
		}

		public virtual void GenerateGapChkData(LinkedList<MbeGapChkObj> chkObjList,int _netNum)
		//public virtual void GenerateGapChkData(MbeGapChk gapChk,int _netNum)
		{
		}


		/// <summary>
		/// Mb3�t�@�C���̓ǂݍ��ݎ��̃����o�[�̉��߂��s��
		/// </summary>
		/// <param name="str1">�ϐ����܂���"+"�Ŏn�܂�u���b�N�^�O</param>
		/// <param name="str2">�ϐ��l</param>
		/// <param name="readCE3">�u���b�N�ǂݍ��ݎ��Ɏg��ReadCE3�N���X</param>
		/// <returns>����I������ReadCE3.RdStatus.NoError��Ԃ�</returns>
		public virtual ReadCE3.RdStatus RdMb3Member(string str1, string str2, ReadCE3 readCE3)
		{
			//ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;

			switch (str1) {
				case "LAYER":
					Layer = MbeLayer.GetLayerValue(str2);
					return ReadCE3.RdStatus.NoError;
				case "SIGNAME":
					SigName = ReadCE3.DecodeCE3String(str2);
					return ReadCE3.RdStatus.NoError;
			}

			if ((str1.Substring(1, 3) == "POS") && (str1.Length > 4)) {
				string strIndex = str1.Substring(4);
				int index;
				int value;
				try {
					index = Convert.ToInt32(strIndex);
					value = Convert.ToInt32(str2);
				}
				catch (Exception) {
					return ReadCE3.RdStatus.FormatError;
				}
				if (str1[0] == 'X') {
					SetXPos(value, index);
					return ReadCE3.RdStatus.NoError;
				} else if (str1[0] == 'Y') {
					SetYPos(value, index);
					return ReadCE3.RdStatus.NoError;
				}
			}

			
			if (str1[0] == '+' && str1.Length >= 2) {
				string strSkipTo = "-" + str1.Substring(1);
				if (!readCE3.SkipTo(strSkipTo)) return ReadCE3.RdStatus.FileError;
			}

			return ReadCE3.RdStatus.NoError;
		}

		/// <summary>
		/// ���̃N���X��Mb3�t�@�C���̓ǂݍ���
		/// </summary>
		/// <param name="readCE3">�ǂݍ��ݑΏۂ�ReadCE3�N���X</param>
		/// <returns>����I������ReadCE3.RdStatus.NoError��Ԃ�</returns>
		public abstract ReadCE3.RdStatus RdMb3(ReadCE3 readCE3);

		/// <summary>
		/// Mb3�t�@�C���փ����o�[�̏�������
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public virtual bool WrMb3Member(WriteCE3 writeCE3,Point origin)
		{
			writeCE3.WriteRecordString("LAYER", MbeLayer.GetLayerName(layer));
			for (int i = 0; i < posCount; i++) {
				writeCE3.WriteRecordInt(string.Format("XPOS{0}", i), posArray[i].X - origin.X);
				writeCE3.WriteRecordInt(string.Format("YPOS{0}", i), posArray[i].Y - origin.Y);
			}
			writeCE3.WriteRecordString("SIGNAME", signame);
			return true;
		}
		
		/// <summary>
		/// Mb3�t�@�C���ւ̏�������
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public abstract bool WrMb3(WriteCE3 writeCE3,Point origin);

		/// <summary>
		/// �������s��
		/// </summary>
		/// <returns></returns>
		public abstract MbeObj Duplicate();

		/// <summary>
		/// �I�����s��
		/// </summary>
		/// <param name="rc"></param>
		/// <param name="layerMask"></param>
		/// <param name="pointMode"></param>
		/// <returns>�I��Ώۂ������Ƃ���true��Ԃ�</returns>
		/// <remarks>
		/// �I��Ώۂ������ꍇ�ɂ͑I���t���O���Z�b�g�����B
		/// </remarks>
		public virtual bool SelectIt(MbeRect rc, ulong layerMask, bool pointMode)
		{
			if ((layerMask & (ulong)layer) == 0) return false;
			if (DeleteCount >= 0) return false;

			bool result = false;
			for (int i = 0; i < posCount; i++) {
				if (rc.Contains(posArray[i])) {
					selectFlag[i] = true;
					result = true;
				}
			}
			return result;
		}


		/// <summary>
		/// ConChk�̂��߂̎�̎擾
		/// </summary>
		/// <param name="rc"></param>
		/// <param name="layerMask"></param>
		/// <returns></returns>
		public virtual MbeObj ConChkSeed(MbeRect rc, ulong layerMask)
		{
			return null;
		}

		/// <summary>
		/// �����ɓ_���߂����ǂ����̃`�F�b�N
		/// </summary>
		/// <param name="lineEnd1">�����̒[�_1</param>
		/// <param name="lineEnd2">�����̒[�_2</param>
		/// <param name="pt">�����\���₢���킹��_���W</param>
		/// <param name="threshold">���������̋���臒l</param>
		/// <param name="ptDivAt">�����_</param>
		/// <returns></returns>
		public static bool IsNearLine(Point lineEnd1, Point lineEnd2, Point pt, int threshold, out Point ptDivAt)
		{
			ptDivAt = pt;
			int x1 = lineEnd1.X;
			int y1 = lineEnd1.Y;
			int x2 = lineEnd2.X;
			int y2 = lineEnd2.Y;
			int xp = pt.X;
			int yp = pt.Y;
			if (x1 == x2) {			// �����͐���
				if (y1 < y2) {
					if (yp <= y1 || y2 <= yp) return false;
				} else {
					if (yp <= y2 || y1 <= yp) return false;
				}
				if (Math.Abs(xp - x1) > threshold) return false;
				ptDivAt = new Point(x1, yp);
				return true;
			} else if (y1 == y2) {	// �����͐���
				if (x1 < x2) {
					if (xp <= x1 || x2 <= xp) return false;
				} else {
					if (xp <= x2 || x1 <= xp) return false;
				}
				if (Math.Abs(yp - y1) > threshold) return false;
				ptDivAt = new Point(xp, y1);
				return true;
			} else {
				double a1 = (double)(y2 - y1) / (double)(x2 - x1);
				double b1 = (double)y1 - a1 * x1;
				double a2 = -1.0 / a1;
				double b2 = (double)yp - a2 * xp;
				double xc = (double)(b2 - b1) / (double)(a1 - a2);
				if (x1 < x2) {
					if (xc <= x1 || x2 <= xc) return false;
				} else {
					if (xc <= x2 || x1 <= xc) return false;
				}
				double yc = a1 * xc + b1;
					if (Math.Abs(xp - xc) > threshold || Math.Abs(yp - yc) > threshold) return false;
                    ptDivAt = new Point((int)Math.Round(xc), (int)Math.Round(yc));
				return true;
			}
		}



		/// <summary>
		/// �����\���ǂ�����Ԃ��B�����n�I�u�W�F�N�g�ňӖ������B
		/// </summary>
		/// <param name="pt">�₢���킹��_</param>
		/// <param name="lineIndex">�����\�Ȑ��̃C���f�b�N�X�B�P�����C���Ȃ���0</param>
		/// <param name="ptDivAt">��������|�C���g</param>
		/// <returns></returns>
		public virtual bool CanDivide(Point pt,ulong visibleLayer,int threshold, out int lineIndex, out Point ptDivAt)
		{
			ptDivAt = pt;
			lineIndex = 0;
			return false;
		}

        ///// <summary>
        ///// �����ŕ����B�����n�I�u�W�F�N�g�ňӖ������B
        ///// </summary>
        ///// <param name="lineIndex">�����\�Ȑ��̃C���f�b�N�X�B�P�����C���Ȃ疳���B</param>
        ///// <param name="ptDivAt"></param>
        ///// <returns></returns>
        //public virtual bool DivideAtCenter(int lineIndex, out MbeObj newObj)
        //{
        //    newObj = null;
        //    return false;
        //}

		/// <summary>
		/// �w��_�ŕ����B�����n�I�u�W�F�N�g�ňӖ������B
		/// </summary>
		/// <param name="lineIndex">�����\�Ȑ��̃C���f�b�N�X�B�P�����C���Ȃ疳���B</param>
		/// <param name="ptDivAt"></param>
		/// <returns></returns>
		public virtual bool DivideAtPoint(int lineIndex,Point pt, out MbeObj newObj)
		{
			newObj = null;
			return false;
		}



		/// <summary>
		/// �}�ʗv�f�̍��W��`����W�ɕϊ�����
		/// </summary>
		/// <param name="point">�}�ʗv�f�̍��W</param>
		/// <param name="scale">�k����</param>
		/// <returns>�`����W</returns>
		/// <remarks>
		/// MbeView��RealToDraw�Ɠ����@�\
		/// �`����W�́A�}�ʌ��_��(0,0)�Ƃ���B
		/// </remarks>
		public static Point ToDrawDim(Point point, double scale)
		{
            return new Point((int)Math.Round(point.X / scale), (int)Math.Round(-point.Y / scale));
		}

        public static int ToDrawDim(int value, double scale)
		{
            return (int)Math.Round((double)value / scale);
		}

		/// <summary>
		/// �`��(abstract ���\�b�h)
		/// </summary>
		/// <param name="dp"></param>
		public abstract void Draw(DrawParam dp);

		/// <summary>
		/// �X�i�b�v�}�[�N�̕`��ݒ�
		/// </summary>
		//public static bool DrawSnapMarkFlag
		//{
		//    get { return MbeObjPin.drawSnapMark; }
		//    set { MbeObjPin.drawSnapMark = value; }
		//}






		/// <summary>
		/// �s���ԍ��̕`��ݒ�
		/// </summary>
		public static bool DrawPinNumFlag
		{
			get { return MbeObjPin.drawPinNum; }
			set { MbeObjPin.drawPinNum = value; }
		}


		/// <summary>
		/// �X�i�b�v�}�[�N�̕`��
		/// </summary>
		/// <param name="g"></param>
		/// <param name="pt"></param>
		/// <param name="drawSize"></param>
		protected virtual void DrawSnapPointMark(Graphics g, Point pt, int drawSize,bool active)
		{
			//bool active = selectFlag[index];
			if (!active && drawSize < 10) return;
			if (active) {
				if (drawSize < 5) drawSize = 5;
			}
            Color col = (active ? MbeColors.ColorActiveSnapPoint : MbeColors.ColorSnapPoint);
            //Color col = (active ? Color.White : MbeColors.ColorSnapPoint);
			//int width = (active ? 3 : 1);
			//Pen pen = new Pen(col,width);
			Pen pen = new Pen(col,1);
			int n = drawSize / 2;
			g.DrawLine(pen, pt.X, pt.Y - n, pt.X, pt.Y + n);
			g.DrawLine(pen, pt.X - n, pt.Y, pt.X + n, pt.Y);
			pen.Dispose();
		}

		/// <summary>
		/// �^�[�Q�b�g�}�[�N�̕`��
		/// </summary>
		/// <param name="g"></param>
		/// <param name="pt"></param>
		/// <param name="drawSize"></param>
		protected virtual void DrawTargetMark(Graphics g, Point pt, int drawSize, bool active)
		{
			if (drawSize < 10) drawSize = 10;
			//drawSize = 1;
			Color col = (active ? MbeColors.ColorActiveSnapPoint : MbeColors.ColorSnapPoint);
            //Color col = (active ? Color.White : MbeColors.ColorSnapPoint);
            //int width = (active ? 3 : 1);
			//Pen pen = new Pen(col,width);
			Pen pen = new Pen(col, 1);
			int n = drawSize / 2;
			Rectangle rc = new Rectangle(pt.X - n, pt.Y - n, drawSize, drawSize);
			g.DrawEllipse(pen, rc);
			n = n * 15 / 10;
			g.DrawLine(pen, pt.X, pt.Y - n, pt.X, pt.Y + n);
			g.DrawLine(pen, pt.X - n, pt.Y, pt.X + n, pt.Y);
			pen.Dispose();
		}



		/// <summary>
		/// �ڑ��`�F�b�N�ŃA�N�e�B�u�ɂȂ��Ă��邩�ǂ�����Ԃ�
		/// </summary>
		public bool ConnectionCheckActive
		{
			get { return connectionCheckActive; }
			//set { connectCheckActive = value; }
		}

		/// <summary>
		/// �ڑ��`�F�b�N�̃A�N�e�B�u��Ԃ̐ݒ�
		/// </summary>
		/// <remarks>�h���N���X�ōĒ�`���Ȃ�����A�N�e�B�u�ɂł��Ȃ�</remarks>
		public virtual void SetConnectCheck()
		{
			connectionCheckActive = false;
		}

		/// <summary>
		/// �ڑ��`�F�b�N�̃A�N�e�B�u��Ԃ̃N���A
		/// </summary>
		public virtual void ClearConnectCheck()
		{
			connectionCheckActive = false;
		}

		public string TempPropString
		{
			get { return strTempProp; }
			set { strTempProp = value; }
		}


        /// <summary>
        /// �`��͈͂𓾂�
        /// </summary>
        /// <returns></returns>
        public virtual MbeRect OccupationRect()
        {
            MbeRect rc = new MbeRect();
            rc = new MbeRect(GetPos(0), GetPos(0));

            for (int i = 1; i < PosCount; i++) {
                rc.Or(GetPos(i));
            }
            return rc;
        }

        /// <summary>
        /// �Z���^�[�|���`���[�h�̂Ƃ��̃h�����a�����߂�
        /// </summary>
        public static int PrintCenterPunchModeDiameter(int drill)
        {
            return (drill < 4000 ? drill : 4000);            
        }


 


	}

	class MbeObjIO
	{
		
		/// <summary>
		/// ReadCE3�̃X�g���[������AstartWord�Ŏn�܂�MbeObj��ǂݎ��
		/// </summary>
		/// <param name="readMb3"></param>
		/// <param name="startWord"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static ReadCE3.RdStatus ReadMbeObj(ReadCE3 readMb3, string startWord, out MbeObj obj)
		{
			ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;
			obj = null;
			if (startWord == "+MBE_HOLE") {
				obj = new MbeObjHole();
			} else if (startWord == "+MBE_PTH") {
				obj = new MbeObjPTH();
			} else if (startWord == "+MBE_PINSMD") {
				obj = new MbeObjPinSMD(true);
            } else if (startWord == "+MBE_FLASHMARK") {
                obj = new MbeObjPinSMD(false);
            } else if (startWord == "+MBE_LINE") {
				obj = new MbeObjLine();
			} else if (startWord == "+MBE_POLYGON") {
				obj = new MbeObjPolygon();
			} else if (startWord == "+MBE_TEXT") {
				obj = new MbeObjText();
			} else if (startWord == "+MBE_ARC") {
				obj = new MbeObjArc();
			} else if (startWord == "+MBE_COMPONENT") {
				obj = new MbeObjComponent();
			} else {
				string strSkipTo = "-" + startWord.Substring(1);
				readMb3.SkipTo(strSkipTo);
			}

			if (obj != null) {
				result = obj.RdMb3(readMb3);
				if (result != ReadCE3.RdStatus.NoError) {
					obj = null;
				}
			}
			return result;
		}
	}
}
