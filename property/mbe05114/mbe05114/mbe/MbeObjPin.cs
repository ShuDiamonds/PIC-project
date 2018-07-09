using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;

using CE3IO;

namespace mbe
{
	public abstract class MbeObjPin : MbeObj
	{
		protected static readonly string[] padShapeName = {
			"RECT",
			"OBROUND"	//�ǉ�����ꍇ�ł�OBROUND����ɍŌ�ɂȂ�悤�ɂ���B
		};

		public enum PadShape
		{
			ERR = -1,
			Rect,
			Obround	//�ǉ�����ꍇ�ł�Obround����ɍŌ�ɂȂ�悤�ɂ���B
		}

        /// <summary>
        /// �T�[�}�������[�t�̎��
        /// </summary>
        protected static readonly string[] thermalReliefTypeName = {
           "THMLRLFINCOMP",//���i�������Ƃ��T�[�}�������[�t�Ƃ���
           "SOLID"         //��Ƀ\���b�h�ڑ�
        };



        /// <summary>
        /// �T�[�}�������[�t�̎��
        /// </summary>
        public enum PadThermalRelief
        {
            ERR = -1,
            ThmlRlfInComp,       //���i�������Ƃ��T�[�}�������[�t�Ƃ���
            Solid                //��Ƀ\���b�h�ڑ�
        }

		public const int MIN_PAD_SIZE =   2000; //0.2mm
		public const int MAX_PAD_SIZE = 100000; //10mm


        public PadThermalRelief ThermalRelief
        {
            get { return thermalRelief; }
            set { thermalRelief = value; }
        }


		/// <summary>
		/// �\���_���W�X�g�̃}�[�W�������l
		/// </summary>
		protected const int DEFAULT_SRMARGIN = 1000; //0.1mm

		/// <summary>
		/// �\���_���W�X�g�̃}�[�W���ő�l
		/// </summary>
		protected const int MAX_SRMARGIN = 10000; //1.0mm

		/// <summary>
		/// �\���_���W�X�g�̃}�[�W���ŏ��l
		/// </summary>
		protected const int MIX_SRMARGIN = -1000; //-0.1mm

        public const int DEFAULT_WIDTH = 16000;
        public const int DEFAULT_HEIGHT = 16000;

        protected bool no_ResistMask;    //���W�X�g���J���Ȃ�

        public bool No_ResistMask
        {
            get { return no_ResistMask; }
            set { no_ResistMask = value; }
        }


		/// <summary>
		/// PadShape�l�ɑΉ�����p�b�h�`�󖼂𓾂�
		/// </summary>
		/// <param name="layer"></param>
		/// <returns></returns>
		public static string GetPadShapeName(PadShape shape)
		{
			if (shape == PadShape.ERR) {
				return "";
			}
			return padShapeName[(int)shape];
		}

		/// <summary>
		/// name �ɑΉ����� PadShape��Ԃ��B
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static PadShape GetPadShapeValue(string name)
		{
			int namesCount = padShapeName.Length;
			for (int i = 0; i < namesCount; i++) {
				if (padShapeName[i] == name) {
					return (PadShape)i;
				}
			}
			return PadShape.ERR;
		}

        /// <summary>
        /// thmlrlf �l�ɑΉ�����T�[�}�������[�t�^�C�v�������Ԃ�
        /// </summary>
        /// <param name="thmlrlf"></param>
        /// <returns></returns>
        public static string GetThermalReliefTypeName(PadThermalRelief thmlrlf)
		{
			if (thmlrlf == PadThermalRelief.ERR) {
				return "";
			}
			return thermalReliefTypeName[(int)thmlrlf];
		}

        /// <summary>
        /// �T�[�}�������[�t������ɑΉ�����l��Ԃ�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PadThermalRelief GetThermalReliefTypeValue(string name)
        {
            int namesCount = thermalReliefTypeName.Length;
			for (int i = 0; i < namesCount; i++) {
				if (thermalReliefTypeName[i] == name) {
					return (PadThermalRelief)i;
				}
			}
			return PadThermalRelief.ERR;
        }





		protected Size padSize;
		protected PadShape shape;
		protected int srmargin;
		protected string num;
        private PadThermalRelief thermalRelief;


		
				

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		protected MbeObjPin()
		{
			posCount = 1;
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			selectFlag[0] = false;

			//padSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
			//shape = PadShape.Obround;
			srmargin = DEFAULT_SRMARGIN;
			num = "";
            thermalRelief = PadThermalRelief.ThmlRlfInComp;
            no_ResistMask = false;
		}

		/// <summary>
		/// �R�s�[�R���X�g���N�^
		/// </summary>
		/// <param name="mbeObjPin"></param>
		protected MbeObjPin(MbeObjPin mbeObjPin)
			: base(mbeObjPin)
		{
			padSize = mbeObjPin.padSize;
			shape = mbeObjPin.shape;
			srmargin = mbeObjPin.srmargin;
			num = mbeObjPin.num;
            thermalRelief = mbeObjPin.thermalRelief;
            no_ResistMask = mbeObjPin.no_ResistMask;
		}


		/// <summary>
		/// ��]
		/// </summary>
		/// <param name="selectedOnly"></param>
		/// <param name="ptCenter"></param>
		public override void Rotate90(bool selectedOnly, Point ptCenter)
		{
			base.Rotate90(selectedOnly, ptCenter);
			int w = padSize.Width;
			int h = padSize.Height;
			padSize = new Size(h, w);
		}
		

		/// <summary>
		/// �p�b�h�T�C�Y�̎擾�Ɛݒ�
		/// </summary>
		public Size PadSize
		{
			get
			{
				return padSize;
			}
			set
			{
				padSize = value;
			}
		}

		/// <summary>
		/// �p�b�h�̕��̎擾�Ɛݒ�
		/// </summary>
		public int Width
		{
			get
			{
				return padSize.Width;
			}
			set
			{
				if (value < 0) value = 0;
				padSize.Width = value;
			}
		}

		/// <summary>
		/// �p�b�h�̍����̎擾�Ɛݒ�
		/// </summary>
		public int Height
		{
			get
			{
				return padSize.Height;
			}
			set
			{
				if (value < 0) value = 0;
				padSize.Height = value;
			}
		}

		/// <summary>
		///  �p�b�h�`��̎擾�Ɛݒ�
		/// </summary>
		public PadShape Shape
		{
			get
			{
				return shape;
			}
			set
			{
				if ((int)value < 0 || (int)value > (int)PadShape.Obround) {
					value = PadShape.Obround;
				}
				shape = value;
			}
		}

		/// <summary>
		/// �s���ԍ��̎擾�Ɛݒ�
		/// </summary>
		public string PinNum
		{
			get
			{
				return num;
			}
			set
			{
				num = value;
			}
		}

		/// <summary>
		/// �\���_���W�X�g�}�[�W���̐ݒ�
		/// </summary>
		public int SrMargin
		{
			get { return srmargin; }
			set
			{
				srmargin = value;
				if (srmargin < MIX_SRMARGIN) srmargin = MIX_SRMARGIN;
				else if (srmargin > MAX_SRMARGIN) srmargin = MAX_SRMARGIN;
			}
		}


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
		public override bool SelectIt(MbeRect rc, ulong layerMask, bool pointMode)
		{
			if ((layerMask & (ulong)layer) == 0) return false;
			if (DeleteCount >= 0) return false;

			if (!pointMode) {
				return base.SelectIt(rc, layerMask, pointMode);
			}

			int areaWidth = 0;
			int areaHeight = 0;
			Point ptC = rc.Center();

			//�܂��A�p�b�h�̋�`�͈͓��ɓ����Ă��邩�ǂ����̃`�F�b�N
			//���~�p�b�h�ł́A���[�̔��~�������G���A���`�F�b�N����
			if (Shape == PadShape.Obround) {
				if (Height < Width) {
					areaHeight = Height;
					areaWidth = Width - Height;
				} else if (Height > Width) {
					areaHeight = Height - Width;
					areaWidth = Width;
				} else {
					//���ƍ����������ꍇ�́A��`�G���A�����݂��Ȃ��B
					//���S�_���猟���_�܂ł̋��������a���Ɏ��܂��Ă��邩�ǂ����̃`�F�b�N����
					if(Util.DistancePointPoint(ptC,GetPos(0))<=Width/2){
						selectFlag[0] = true;
						return true;
					}else{
						return false;
					}
				}
			} else {	//��`�p�b�h�Ȃ��`�p�b�h�S�̂ł���
				areaHeight = Height;
				areaWidth = Width;
			}

			int x = GetPos(0).X;
			int y = GetPos(0).Y;
			int l = x - areaWidth / 2;
			int r = x + areaWidth / 2;
			int t = y + areaHeight / 2;
			int b = y - areaHeight / 2;


			//��`�G���A���`�F�b�N
			if (l <= ptC.X && ptC.X <= r && b <= ptC.Y && ptC.Y < t) {
				selectFlag[0] = true;
				return true;
			}

			//���~�̏ꍇ�́A���[���~���`�F�b�N���s���B
			if (Shape == PadShape.Obround) {
				int x0 = 0;
				int y0 = 0;
				int x1 = 0;
				int y1 = 0;
				int rad = 0;	//���a
				if (Height < Width) {
					rad = Height /2;
					x0 = l;
					x1 = r;
					y0 = y1 = y;
				}else if(Height > Width){
					rad = Width /2;
					x0 = x1 = x;
					y0 = b;
					y1 = t;
				}
				if( Util.DistancePointPoint(ptC,new Point(x0,y0))<=rad ||
					Util.DistancePointPoint(ptC,new Point(x1,y1))<=rad){
					selectFlag[0] = true;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// �|���S���̂��߂̗֊s�f�[�^�𐶐�����B
		/// </summary>
		/// <param name="outlineList"></param>
		/// <param name="param"></param>
		/// <remarks>�����Ő�������֊s�f�[�^�͓����ɏd�Ȃ��Ă��Ă��ǂ����̂Ƃ���</remarks>
		public override void GenerateOutlineData(LinkedList<MbeGapChkObjLine> outlineList, GenOutlineParam param)
		{
			//if (param.layer != Layer) return;
			int distance;
			Point ptCenter = GetPos(0);
			Point[] pt;
			int pointCount;
			int x = ptCenter.X;
			int y = ptCenter.Y;


			if (shape == PadShape.Rect) {
				distance = param.traceWidth / 2 + param.gap;
				pt = new Point[4];
				pointCount = 4;
				int xoffset = Width / 2 + distance;
				int yoffset = Height / 2 + distance;
				pt[0] = new Point(x - xoffset, y + yoffset);
				pt[1] = new Point(x + xoffset, y + yoffset);
				pt[2] = new Point(x + xoffset, y - yoffset);
				pt[3] = new Point(x - xoffset, y - yoffset);

			} else {
				int lineW;
				int lineL;
				Point pt0;
				Point pt1;
				if (Width < Height) {
					lineW = Width;
					lineL = Height - lineW;
					pt0 = new Point(x, y - lineL / 2);
					pt1 = new Point(x, y + lineL / 2);
				} else {
					lineW = Height;
					lineL = Width - lineW;
					pt0 = new Point(x - lineL / 2, y);
					pt1 = new Point(x + lineL / 2, y);
				}

				distance = (lineW + param.traceWidth) / 2 + param.gap*11/10;

				int left = param.rc.L - distance;
				int top = param.rc.T + distance;
				int right = param.rc.R + distance;
				int bottom = param.rc.B - distance;

				MbeRect rcArea = new MbeRect(new Point(left, top), new Point(right, bottom));

				if (Util.LineIsOutsideLTRB(pt0, pt1, rcArea)) {
					return;
				}
				bool dummyParam;
				Util.LineOutlineData(pt0, pt1, distance, out pt, out dummyParam);
				pointCount = 8;
			}

			for (int j = 0; j < pointCount; j++) {
				int j2 = j + 1;
				if (j2 == pointCount) {
					j2 = 0;
				}
				if (!Util.LineIsOutsideLTRB(pt[j], pt[j2], param.rc)) {
					MbeGapChkObjLine objLine = new MbeGapChkObjLine();
					objLine.SetLineValue(pt[j], pt[j2], param.traceWidth);
					outlineList.AddLast(objLine);
				}
			}
		}



		/// <summary>
		/// ConChk�̂��߂̎�̎擾
		/// </summary>
		/// <param name="rc"></param>
		/// <param name="layerMask"></param>
		/// <returns></returns>
		public override MbeObj ConChkSeed(MbeRect rc, ulong layerMask)
		{
			if (SelectIt(rc, layerMask, true)) {
				ClearAllSelectFlag();
				return this;
			}
			return null;
		}

		/// <summary>
		/// Mb3�t�@�C���̓ǂݍ��ݎ��̃����o�[�̉��߂��s��
		/// </summary>
		/// <param name="str1">�ϐ����܂���"+"�Ŏn�܂�u���b�N�^�O</param>
		/// <param name="str2">�ϐ��l</param>
		/// <param name="readCE3">�u���b�N�ǂݍ��ݎ��Ɏg��ReadCE3�N���X</param>
		/// <returns>����I������ReadCE3.RdStatus.NoError��Ԃ�</returns>
		public override ReadCE3.RdStatus RdMb3Member(string str1, string str2, ReadCE3 readCE3)
		{
			switch(str1){
				case  "HEIGHT":
					try { Height = Convert.ToInt32(str2);}
					catch (Exception) { Height = DEFAULT_HEIGHT; }
					return ReadCE3.RdStatus.NoError;
				case "WIDTH":
					try { Width = Convert.ToInt32(str2); }
					catch (Exception) { Width = DEFAULT_WIDTH;}
					return ReadCE3.RdStatus.NoError;
				case "SHAPE":
					shape = GetPadShapeValue(str2);
                    if (shape == PadShape.ERR) {
                        shape = PadShape.Obround;
                    }
					return ReadCE3.RdStatus.NoError;
                case "THMLRLF":
                    thermalRelief = GetThermalReliefTypeValue(str2);
                    if (thermalRelief == PadThermalRelief.ERR) {
                        thermalRelief = PadThermalRelief.ThmlRlfInComp;
                    }
                    return ReadCE3.RdStatus.NoError;
                case "PINNUM":
					PinNum = ReadCE3.DecodeCE3String(str2);
					return ReadCE3.RdStatus.NoError;
				case "SRMARGIN":
					try { SrMargin = Convert.ToInt32(str2); }
					catch (Exception) { SrMargin = DEFAULT_SRMARGIN; }
					return ReadCE3.RdStatus.NoError;
                case "NO_RM":
                    try {
                        int n = Convert.ToInt32(str2);
                        no_ResistMask = (n != 0);
                    }
                    catch (Exception) { no_ResistMask = false; }
                    return ReadCE3.RdStatus.NoError;
				default:
					return base.RdMb3Member(str1, str2, readCE3);
			}
			//return true;
		}

		
		/// <summary>
		/// WriteCE3�N���X�փ����o�[�̏�������
		/// </summary>
		/// <param name="writeCE3">�������ݑΏ�WriteCE3�N���X</param>
		/// <param name="origin">�������ݎ��̌��_</param>
		/// <returns>����I����true</returns>
		public override bool WrMb3Member(WriteCE3 writeCE3, Point origin)
		{
			base.WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecordInt("HEIGHT", Height);
			writeCE3.WriteRecordInt("WIDTH", Width);
			writeCE3.WriteRecordString("SHAPE", GetPadShapeName(shape));
            writeCE3.WriteRecordString("THMLRLF", GetThermalReliefTypeName(thermalRelief));
			writeCE3.WriteRecordString("PINNUM", PinNum);
			writeCE3.WriteRecordInt("SRMARGIN", SrMargin);
            if (no_ResistMask) {
                writeCE3.WriteRecordInt("NO_RM", 1);
            }
			return true;
		}

		

		/// <summary>
		/// �s���ԍ��̕`��
		/// </summary>
		/// <param name="g">�`��Ώ�</param>
		/// <param name="str">������</param>
		/// <param name="pt">�ʒu(�����ɃZ���^�����O�����)</param>
		/// <param name="fontSize">�t�H���g�̃s�N�Z���T�C�Y</param>
		/// <param name="vertical">�����ɕ`�悷��Ƃ�true</param>
		protected virtual void DrawPinNum(Graphics g, string str, Point pt, int fontSize, bool vertical)
		{
			if (fontSize < 10 || str.Length == 0) return;
			Font font = new Font(FontFamily.GenericSerif, fontSize, GraphicsUnit.Pixel);
			Brush brush = new SolidBrush(MbeColors.ColorPinNum);
			StringFormat format = new StringFormat(StringFormatFlags.NoClip);
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			//������͍��W�n�𕽍s�ړ����Ă���`�悷��
			GraphicsState gState = g.Save();	//���W�n�ۑ�
			g.TranslateTransform(pt.X, pt.Y);
			if (vertical) {						//��]
				g.RotateTransform(-90F);
			}
			g.DrawString(str, font, brush, new PointF(0, 0),format);
			g.Restore(gState);					//���W�n���A

			format.Dispose();
			brush.Dispose();
			font.Dispose();
		}

		/// <summary>
		/// �ڑ��`�F�b�N�̃A�N�e�B�u��Ԃ̐ݒ�
		/// </summary>
		public override void SetConnectCheck()
		{
			connectionCheckActive = true;
		}

        /// <summary>
        /// �`��͈͂𓾂�
        /// </summary>
        /// <returns></returns>
        public override MbeRect OccupationRect()
        {
			
			int x = GetPos(0).X;
			int y = GetPos(0).Y;
			int l = x - Width / 2;
			int r = x + Width / 2;
			int t = y + Height / 2;
			int b = y - Height / 2;

            return new MbeRect(new Point(l, t), new Point(r, b));
        }

	}
}
