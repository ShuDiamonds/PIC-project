using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using CE3IO;

namespace mbe
{
	class MbeObjHole : MbeObj
	{
		protected int dia;			//unit : 0.1 micro mater

		public const int DEFAULT_DIA = 32000;

		public const int MIN_DIA =   3000; //0.3mm
		public const int MAX_DIA =  80000; //8mm

		/// <summary>
		/// �\���_���W�X�g�̃}�[�W�������l
		/// </summary>
		protected const int DEFAULT_SRMARGIN = 2000; //0.2mm


			/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public MbeObjHole()
		{
			posCount = 1;
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			selectFlag[0] = false;

			dia = DEFAULT_DIA;
			layer = MbeLayer.LayerValue.DRL;
			snapLayer = (ulong)layer;
		}

		/// <summary>
		/// �R�s�[�R���X�g���N�^
		/// </summary>
		/// <param name="mbeObjPin"></param>
		public MbeObjHole(MbeObjHole mbeObjHole)
			: base(mbeObjHole)
		{
			dia = mbeObjHole.dia;
		}

		/// <summary>
		/// �h�����a�̎擾�Ɛݒ�
		/// </summary>
		public int Diameter
		{
			get
			{
				return dia;
			}
			set
			{
				if (value < MIN_DIA) value = MIN_DIA;
				else if (value > MAX_DIA) value = MAX_DIA;
				dia = value;
			}
		}

		/// <summary>
		/// �}�ʃI�u�W�F�N�g���Ƃ�ID�l��Ԃ�
		/// </summary>
		public override MbeObjID Id()
		{
			return MbeObjID.MbeHole;
		}

		/// <summary>
		/// ���C���[�l�̎擾�Ɛݒ�
		/// </summary>
		public override MbeLayer.LayerValue Layer
		{
			get
			{
				return MbeLayer.LayerValue.DRL;
			}
			set
			{
				layer = MbeLayer.LayerValue.DRL;
				snapLayer = (ulong)layer;
			}
		}


		/// <summary>
		/// �z�u�w��\�ȃ��C���[
		/// </summary>
		/// <returns></returns>
		public static ulong SelectableLayer()
		{
			return (ulong)MbeLayer.LayerValue.DRL;
		}

		public static MbeLayer.LayerValue NewSelectLayer(MbeLayer.LayerValue oldLayer)
		{
			return MbeLayer.LayerValue.DRL;
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
			switch (str1) {
				case "DIA":
					try { Diameter = Convert.ToInt32(str2); }
					catch (Exception) { Diameter = DEFAULT_DIA; }
					return ReadCE3.RdStatus.NoError;
				default:
					return base.RdMb3Member(str1, str2, readCE3);
			}
			//return true;
		}

		/// <summary>
		/// ���̃N���X��Mb3�t�@�C���̓ǂݍ���
		/// </summary>
		/// <param name="readCE3">�ǂݍ��ݑΏۂ�ReadCE3�N���X</param>
		/// <returns>����I������ReadCE3.RdStatus.NoError ��Ԃ�</returns>
		public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
		{
			string str1;
			string str2;
			while (readCE3.GetRecord(out str1, out str2)) {
				if (str1[0] == '-') {
					if (str1 != "-MBE_HOLE") {
						return ReadCE3.RdStatus.FormatError;
					} else {
						return ReadCE3.RdStatus.NoError;
					}
				} else {
					ReadCE3.RdStatus result = RdMb3Member(str1, str2, readCE3);
					if (result != ReadCE3.RdStatus.NoError) {
						return result;
					}
				}
			}
			return ReadCE3.RdStatus.FileError;
		}

		/// <summary>
		/// Mb3�t�@�C���փ����o�[�̏�������
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public override bool WrMb3Member(WriteCE3 writeCE3, Point origin)
		{
			base.WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecordInt("DIA", Diameter);
			return true;
		}

		/// <summary>
		/// Mb3�t�@�C���ւ̏�������
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public override bool WrMb3(WriteCE3 writeCE3, Point origin)
		{
			writeCE3.WriteRecord("+MBE_HOLE");
			WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecord("-MBE_HOLE");
			writeCE3.WriteNewLine();
			return true;
		}

		/// <summary>
		/// �������s��
		/// </summary>
		/// <returns></returns>
		public override MbeObj Duplicate()
		{
			MbeObj newObj = new MbeObjHole(this);
			return newObj;
		}

		/// <summary>
		/// �`��
		/// </summary>
		/// <param name="dp"></param>
		public override void Draw(DrawParam dp)
		{
			if (((ulong)dp.layer & ((ulong)MbeLayer.LayerValue.STS |
					 (ulong)MbeLayer.LayerValue.STC |
					 (ulong)MbeLayer.LayerValue.DRL)) == 0) return;

			//if (dp.layer != MbeLayer.LayerValue.DRL) return;

			Point pt = this.GetPos(0);
			pt = ToDrawDim(pt, dp.scale);

			Color col;
			Color cold;
			//int w = 0;
			//int h = 0;
			int wd = 0;
			//int hd = 0;
			Rectangle rc;
			SolidBrush brush;
			Pen pen;
            bool centerPunchMode = false;

            int drillDia;
            if ((dp.option & (uint)MbeDrawOption.CenterPunchMode) != 0) {
                drillDia = PrintCenterPunchModeDiameter(Diameter);
                centerPunchMode = true;
            } else {
                drillDia = Diameter;
            }


			switch (dp.layer) {
				case MbeLayer.LayerValue.DRL:
                    wd = (int)(drillDia / dp.scale) | 1;
					break;
				case MbeLayer.LayerValue.STC:
				case MbeLayer.LayerValue.STS:
					wd = (int)((Diameter + DEFAULT_SRMARGIN * 2) / dp.scale) | 1;
					break;
			}


			//wd = (int)(Diameter / dp.scale) | 1;


			#region �F�̐ݒ�
			if (dp.mode == MbeDrawMode.Print) {
                cold = Color.White;
                if ((dp.option & (uint)MbeDrawOption.PrintColor) != 0) {    //�J���[���
                    uint nColor;

                    switch (dp.layer) {
                        case MbeLayer.LayerValue.STC:
                            nColor = MbeColors.PRINT_STC;
                            break;
                        case MbeLayer.LayerValue.STS:
                            nColor = MbeColors.PRINT_STS;
                            break;
                        case MbeLayer.LayerValue.DRL:
                        default:
                            nColor = MbeColors.PRINT_DRL;
                            break;
                    }
                    col = Color.FromArgb(unchecked((int)nColor));
                    col = Color.FromArgb(MBE_OBJ_PRINT_ALPHA, col);
                } else {
                    col = Color.Black;
                }
			} else {
				uint nColor;
				switch (dp.layer) {
					case MbeLayer.LayerValue.STC:
						nColor = MbeColors.STC;
						break;
					case MbeLayer.LayerValue.STS:
						nColor = MbeColors.STS;
						break;
					case MbeLayer.LayerValue.DRL:
					default:
						nColor = MbeColors.DRL;
						break;
				}

				cold = MbeColors.ColorBackground;
				if (dp.mode == MbeDrawMode.Temp) {
					col = Color.FromArgb(MBE_OBJ_ALPHA, MbeColors.HighLighten(nColor));
					cold = Color.FromArgb(0x20, cold);
				} else {
					col = Color.FromArgb(unchecked((int)nColor));
					col = Color.FromArgb(MBE_OBJ_ALPHA, col);
				}
			}
			#endregion


			rc = new Rectangle(pt.X - wd / 2, pt.Y - wd / 2, wd, wd);

			if (dp.layer == MbeLayer.LayerValue.DRL) {
				if (dp.mode != MbeDrawMode.Print) {
					cold = Color.FromArgb(0x90, cold);
				}
				brush = new SolidBrush(cold);
				dp.g.FillEllipse(brush, rc);
				brush.Dispose();

                //DRL���C���[�ւ̕`��ł́A�֊s��`���B
                //������̓v�����^�̔\�͂ɍ��E����ɂ����悤�ɕ���ݒ肷��B
                //�Z���^�[�|���`���[�h�ł́A������ɂ�鎩���z�肵�Ă�⑾�߂̃��C���Ƃ���B
				int w = 1;

				if (dp.mode == MbeDrawMode.Print) {

                    if (centerPunchMode && (dp.visibleLayer & ((ulong)MbeLayer.LayerValue.PTH |
                                                              (ulong)MbeLayer.LayerValue.CMP |
                                                              (ulong)MbeLayer.LayerValue.SOL)) != 0) {
                        w = 1000;
                    } else {
                        w = 500;
                    }

                    w = (int)(w / dp.scale) | 1;

                    if((dp.option & (uint)MbeDrawOption.ToolMarkMode) != 0){
                        if ((dp.visibleLayer & MbeLayer.PatternFilmLayer) == 0) {
                            MbeView.DrawDrillMark(GetPos(0), Diameter, dp, col, w);
                            return;
                        }
                    }
				}
				pen = new Pen(col, w);
				dp.g.DrawEllipse(pen, rc);
				pen.Dispose();
				if (drawSnapMark && dp.mode != MbeDrawMode.Print) {
					int marksize = wd / 2;
					DrawSnapPointMark(dp.g, pt, marksize, selectFlag[0]);
				}
			} else {
				brush = new SolidBrush(col);
				dp.g.FillEllipse(brush, rc);
				brush.Dispose();
			}

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
			Point pt0 = GetPos(0);

			Point[] pt;

			distance = (Diameter + param.traceWidth) / 2 + param.gap * 11 / 10; ;

			int left = param.rc.L - distance;
			int top = param.rc.T + distance;
			int right = param.rc.R + distance;
			int bottom = param.rc.B - distance;
			MbeRect rcArea = new MbeRect(new Point(left, top), new Point(right, bottom));

			if (Util.PointIsOutsideLTRB(pt0, rcArea)) {
				return;
			}
			Util.PointOutlineData(pt0, distance, out pt);

			for (int j = 0; j < 8; j++) {
				int j2 = j + 1;
				if (j2 == 8) {
					j2 = 0;
				}
				if (!Util.LineIsOutsideLTRB(pt[j], pt[j2], param.rc)) {
					MbeGapChkObjLine objLine = new MbeGapChkObjLine();
					objLine.SetLineValue(pt[j], pt[j2], param.traceWidth);
					outlineList.AddLast(objLine);
				}
			}
		}

        private readonly MbeLayer.LayerValue[] gapChkLayerTable =
		    {
			    MbeLayer.LayerValue.CMP,
                MbeLayer.LayerValue.L2,
                MbeLayer.LayerValue.L3,
			    MbeLayer.LayerValue.SOL
		    };

		public void KeepOutData(LinkedList<MbeGapChkObj> chkObjList, int _netNum)
		{
            for (int i = 0; i < gapChkLayerTable.Length; i++) {
                MbeLayer.LayerValue layerValue = gapChkLayerTable[i];
                MbeGapChkObjPoint gapChkObj = new MbeGapChkObjPoint();
				gapChkObj.layer = layerValue;
				gapChkObj.netNum = _netNum;
				gapChkObj.mbeObj = this;
				gapChkObj.SetPointValue(GetPos(0), Diameter);
				//gapChk.Add(gapChkObj);
				chkObjList.AddLast(gapChkObj);
			}
		}


		/// <summary>
		/// CAM�f�[�^�̐��� 
		/// </summary>
		/// <param name="camOut"></param>
		public override void GenerateCamData(CamOut camOut)
		{
			int drillSize = Diameter;
			int stSize = Diameter + DEFAULT_SRMARGIN * 2;
			Point pt0 = GetPos(0);
			Point pt1 = GetPos(0);
			CamOutBaseData camd;

			camd = new CamOutBaseData(MbeLayer.LayerValue.DRL,
									  CamOutBaseData.CamType.DRILL,
									  CamOutBaseData.Shape.Drill,
									  drillSize,drillSize, pt0, pt1);
			camOut.Add(camd);

			camd = new CamOutBaseData(MbeLayer.LayerValue.STC,
									  CamOutBaseData.CamType.FLASH,
									  CamOutBaseData.Shape.Obround,
									  stSize, stSize, pt0, pt1);
			camOut.Add(camd);
			camd = new CamOutBaseData(MbeLayer.LayerValue.STS,
									CamOutBaseData.CamType.FLASH,
									CamOutBaseData.Shape.Obround,
									stSize, stSize, pt0, pt1);
			camOut.Add(camd);
		}

        /// <summary>
        /// �`��͈͂𓾂�
        /// </summary>
        /// <returns></returns>
        public override MbeRect OccupationRect()
        {

            int x = GetPos(0).X;
            int y = GetPos(0).Y;
            int l = x - dia / 2;
            int r = x + dia / 2;
            int t = y + dia / 2;
            int b = y - dia / 2;

            return new MbeRect(new Point(l, t), new Point(r, b));
        }

	}
}
