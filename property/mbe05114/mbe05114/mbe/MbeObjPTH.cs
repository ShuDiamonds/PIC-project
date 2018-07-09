using System;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

using CE3IO;

namespace mbe
{
	class MbeObjPTH : MbeObjPin
	{
		protected int dia;			//unit : 0.1 micro mater

        public ulong innerLayerConnectionInfo;

        public const int DEFAULT_DIA = 9000;//0.9mm

		public const int MIN_DIA =   1000; //0.1mm
		public const int MAX_DIA =  80000; //8mm

		public new const int MIN_PAD_SIZE = 3000; //0.3mm

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public MbeObjPTH()
		{
			dia = DEFAULT_DIA;
			Layer = MbeLayer.LayerValue.PTH;
			padSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
			shape = PadShape.Obround;
            innerLayerConnectionInfo = 0L;
		}

		/// <summary>
		/// �R�s�[�R���X�g���N�^
		/// </summary>
		/// <param name="mbeObjPin"></param>
		public MbeObjPTH(MbeObjPTH mbeObjPinTh)
			: base(mbeObjPinTh)
		{
			dia = mbeObjPinTh.dia;
		}

		/// <summary>
		/// ���C���[�l�̎擾�Ɛݒ�
		/// </summary>
		/// 
		public override MbeLayer.LayerValue Layer
		{
			get
			{
				return layer;
			}
			set
			{

				layer = MbeLayer.LayerValue.PTH;
                snapLayer = (ulong)MbeLayer.LayerValue.PLC |
                            (ulong)MbeLayer.LayerValue.PLS |
                            (ulong)MbeLayer.LayerValue.L2  |
                            (ulong)MbeLayer.LayerValue.L3  |
                            (ulong)MbeLayer.LayerValue.SOL |
							(ulong)MbeLayer.LayerValue.CMP |
							(ulong)MbeLayer.LayerValue.STC |
							(ulong)MbeLayer.LayerValue.STS |
							(ulong)MbeLayer.LayerValue.PTH |
							(ulong)MbeLayer.LayerValue.DRL;
			}
		}




		/// <summary>
		/// �z�u�w��\�ȃ��C���[
		/// </summary>
		/// <returns></returns>
		public static ulong SelectableLayer()
		{
			return (ulong)MbeLayer.LayerValue.PTH;
		}


		public static MbeLayer.LayerValue NewSelectLayer(MbeLayer.LayerValue oldLayer)
		{
			return MbeLayer.LayerValue.PTH;

			//if (MbeLayer.IsComponentSide(oldLayer)) {
			//    return MbeLayer.LayerValue.CMP;
			//} else {
			//    return MbeLayer.LayerValue.SOL;
			//}
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
			return MbeObjID.MbePTH;
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
				case "DIA":
					try { Diameter = Convert.ToInt32(str2);}
					catch (Exception) { Diameter = DEFAULT_DIA;}
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
				if (str1[0] == '-'){
					if (str1 != "-MBE_PTH") {
						return ReadCE3.RdStatus.FormatError;
					} else {
						return ReadCE3.RdStatus.NoError;
					}
				} else {
					ReadCE3.RdStatus result = RdMb3Member(str1, str2, readCE3);
					if(result != ReadCE3.RdStatus.NoError){
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
			writeCE3.WriteRecord("+MBE_PTH");
			WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecord("-MBE_PTH");
			writeCE3.WriteNewLine();
			return true;
		}

		/// <summary>
		/// �������s��
		/// </summary>
		/// <returns></returns>
		public override MbeObj Duplicate()
		{
			MbeObj newObj = new MbeObjPTH(this);
			return newObj;
		}

		/// <summary>
		/// �`��
		/// </summary>
		/// <param name="dp"></param>
		public override void Draw(DrawParam dp)
		{
			if (((ulong)dp.layer & ((ulong)MbeLayer.LayerValue.PTH |
							 (ulong)MbeLayer.LayerValue.STS |
							 (ulong)MbeLayer.LayerValue.STC |
							 (ulong)MbeLayer.LayerValue.DRL)) == 0) return;

            if (((ulong)dp.layer & ((ulong)MbeLayer.LayerValue.STS |
                                    (ulong)MbeLayer.LayerValue.STC)) != 0) {
                if (no_ResistMask) return;
            }

			Point pt = this.GetPos(0);
			pt = ToDrawDim(pt, dp.scale);

			Color col;
			//Color cold;
			int w=0;
			int h=0;
			int wd=0;
			int hd=0;
			Rectangle rc;
			SolidBrush brush;
			Pen pen;


			#region �`��T�C�Y�̐ݒ�

			switch (dp.layer) {
				case MbeLayer.LayerValue.PTH:
					w = (int)(PadSize.Width / dp.scale) | 1;
					h = (int)(PadSize.Height / dp.scale) | 1;
					break;
				case MbeLayer.LayerValue.STC:
				case MbeLayer.LayerValue.STS:
					w = (int)((PadSize.Width + SrMargin*2) / dp.scale) | 1;
					h = (int)((PadSize.Height + SrMargin*2) / dp.scale) | 1;
					break;
			}
			if (w == 1) w++;
			if (h == 1) h++;
			if( (dp.layer==MbeLayer.LayerValue.PTH)||(dp.layer==MbeLayer.LayerValue.DRL)){

                
                int drillDia;
                if((dp.option & (uint)MbeDrawOption.CenterPunchMode) != 0){
                    drillDia = PrintCenterPunchModeDiameter(Diameter);
                } else {
                    drillDia = Diameter;
                }

                wd = (int)(drillDia / dp.scale) | 1;

				if (dp.layer == MbeLayer.LayerValue.PTH) {
					if (wd >= w) wd = w - 2;
					if (wd >= h) wd = h - 2;
				}
				hd = wd;
			}





			#endregion

			#region �F�̐ݒ�
			if (dp.mode == MbeDrawMode.Print) {
                if ((dp.option & (uint)MbeDrawOption.PrintColor) != 0) {    //�J���[���
                    uint nColor;
                    switch (dp.layer) {
                        case MbeLayer.LayerValue.PTH:
                            nColor = MbeColors.PRINT_PTH;
                            break;
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
					case MbeLayer.LayerValue.PTH:
						nColor = MbeColors.PTH;
						break;
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

				if(dp.mode == MbeDrawMode.Temp || connectionCheckActive){
					col = Color.FromArgb(MBE_OBJ_ALPHA,MbeColors.HighLighten(nColor));
				}else{
					col = Color.FromArgb(unchecked((int)nColor));
					col = Color.FromArgb(MBE_OBJ_ALPHA,col);
				}
			}
			#endregion

			#region ���ۂ̕`��̎��s
			switch (dp.layer) {
				case MbeLayer.LayerValue.PTH:
				case MbeLayer.LayerValue.STC:
				case MbeLayer.LayerValue.STS:
					rc = new Rectangle(pt.X - w / 2, pt.Y - h / 2, w, h);

					if (Shape == PadShape.Rect) {
						brush = new SolidBrush(col);
						dp.g.FillRectangle(brush, rc);
						brush.Dispose();
					} else {
						if (w == h) {
							brush = new SolidBrush(col);
							dp.g.FillEllipse(brush, rc);
							brush.Dispose();
						} else {
							pen = new Pen(col, (w < h ? w : h));
							pen.StartCap = LineCap.Round;
							pen.EndCap = LineCap.Round;
							if (w < h) {
								int k = (h - w + 1) / 2;
								dp.g.DrawLine(pen, pt.X, pt.Y - k, pt.X, pt.Y + k);
							} else {
								int k = (w - h + 1) / 2;
								dp.g.DrawLine(pen, pt.X - k, pt.Y, pt.X + k, pt.Y);
							}
							pen.Dispose();
						}
					}
					if (dp.layer == MbeLayer.LayerValue.PTH) {
						if (dp.mode != MbeDrawMode.Print) {
							if (wd >= 3) {
								rc = new Rectangle(pt.X - wd / 2, pt.Y - hd / 2, wd, hd);
								Color cold = MbeColors.ColorBackground;
								if (dp.mode == MbeDrawMode.Temp) {
									cold = Color.FromArgb(MBE_OBJ_ALPHA, cold);
								}
								brush = new SolidBrush(cold);
								//brush = new SolidBrush(MbeColors.ColorBackground);
								dp.g.FillEllipse(brush, rc);
								brush.Dispose();
							}
							if (drawSnapMark) {
								int marksize = (w < h ? w : h) / 2;
								DrawSnapPointMark(dp.g, pt, marksize,selectFlag[0]);
							}
							if (DrawPinNumFlag) {
								int fontsize = (w < h ? w : h) / 2;	//�����ƕ��̏�������
								bool vertical = (w < h);			//�����傫����Ώc����
								DrawPinNum(dp.g, PinNum, pt, fontsize, vertical);
							}
						} else {
							rc = new Rectangle(pt.X - wd / 2, pt.Y - hd / 2, wd, hd);
							brush = new SolidBrush(Color.White);
							dp.g.FillEllipse(brush, rc);
							brush.Dispose();
						}
					}
					break;
				case MbeLayer.LayerValue.DRL:
					rc = new Rectangle(pt.X - wd / 2, pt.Y - hd / 2, wd, hd);
					if (dp.mode == MbeDrawMode.Print) {


						brush = new SolidBrush(Color.White);
						dp.g.FillEllipse(brush, rc);
						brush.Dispose();
						if ((dp.visibleLayer & (ulong)MbeLayer.LayerValue.PTH) == 0) {
                            int penw = (int)(500 / dp.scale) | 1;

                            if ((dp.option & (uint)MbeDrawOption.ToolMarkMode) != 0) {
                                if ((dp.visibleLayer & MbeLayer.PatternFilmLayer) == 0) {
                                    MbeView.DrawDrillMark(GetPos(0), Diameter, dp, col, penw);
                                    return;
                                }
                            }



                            pen = new Pen(col,penw);
							dp.g.DrawEllipse(pen, rc);
							pen.Dispose();
						}

					} else {
						pen = new Pen(col);
						dp.g.DrawEllipse(pen, rc);
						pen.Dispose();
					}
					break;
			}
			#endregion

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


            if (param.layer == MbeLayer.LayerValue.L2 || param.layer == MbeLayer.LayerValue.L3) {
                //���w�̂Ƃ��͔�ڑ����̓h�����a+0.4mm�B(�������A�\�wpad�T�C�Y�𒴂��Ȃ��j�ڑ�����padSize��Width��Height�̏�������
                int landDia;
                if (((ulong)param.layer & innerLayerConnectionInfo) == 0) {
                    landDia = dia + 4000;
                    if (landDia > InnerLandDia) {
                        landDia = InnerLandDia;
                    }
                } else {
                    landDia = InnerLandDia;
                }
                Point pt0 = new Point(x, y);

                distance = (landDia + param.traceWidth) / 2 + param.gap * 11 / 10;

                int left = param.rc.L - distance;
                int top = param.rc.T + distance;
                int right = param.rc.R + distance;
                int bottom = param.rc.B - distance;

                MbeRect rcArea = new MbeRect(new Point(left, top), new Point(right, bottom));

                if (Util.LineIsOutsideLTRB(pt0, pt0, rcArea)) {
                    return;
                }
                bool dummyParam;
                Util.LineOutlineData(pt0, pt0, distance, out pt, out dummyParam);
                pointCount = 8;
            }else if (shape == PadShape.Rect) {
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

                distance = (lineW + param.traceWidth) / 2 + param.gap * 11 / 10;

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




		///// <summary>
		///// �|���S���̂��߂̗֊s�f�[�^�𐶐�����B
		///// </summary>
		///// <param name="outlineList"></param>
		///// <param name="param"></param>
		///// <remarks>�����Ő�������֊s�f�[�^�͓����ɏd�Ȃ��Ă��Ă��ǂ����̂Ƃ���</remarks>
		//public override void GenerateOutlineData(LinkedList<MbeGapChkObjLine> outlineList, GenOutlineParam param)
		//{
		//    //if (param.layer != Layer) return;
		//    int distance;
		//    Point ptCenter = GetPos(0);
		//    Point[] pt;
		//    int pointCount;
		//    int x = ptCenter.X;
		//    int y = ptCenter.Y;


		//    if (shape == PadShape.Rect) {
		//        distance = param.traceWidth / 2 + param.gap;
		//        pt = new Point[4];
		//        pointCount = 4;
		//        int xoffset = Width/2+distance;
		//        int yoffset = Height/2+distance;
		//        pt[0] = new Point(x - xoffset, y + yoffset);
		//        pt[1] = new Point(x + xoffset, y + yoffset);
		//        pt[2] = new Point(x + xoffset, y - yoffset);
		//        pt[3] = new Point(x - xoffset, y - yoffset);

		//    }else{
		//        int lineW;
		//        int lineL;
		//        Point pt0;
		//        Point pt1;
		//        if(Width<Height){
		//            lineW = Width;
		//            lineL = Height - lineW;
		//            pt0 = new Point(x,y-lineL/2);
		//            pt1 = new Point(x,y+lineL/2);
		//        }else{
		//            lineW = Height;
		//            lineL = Width - lineW;
		//            pt0 = new Point(x-lineL/2,y);
		//            pt1 = new Point(x+lineL/2,y);
		//        }
				
		//        distance = (lineW + param.traceWidth) / 2 + param.gap;

		//        int left = param.rc.L - distance;
		//        int top = param.rc.T + distance;
		//        int right = param.rc.R + distance;
		//        int bottom = param.rc.B - distance;

		//        MbeRect rcArea = new MbeRect(new Point(left, top), new Point(right, bottom));
	
		//        if (Util.LineIsOutsideLTRB(pt0, pt1, rcArea)) {
		//            return;
		//        }
		//        Util.LineOutlineData(pt0, pt1, distance, out pt);
		//        pointCount = 8;
		//    }

		//    for (int j = 0; j < pointCount; j++) {
		//        int j2 = j + 1;
		//        if (j2 == pointCount) {
		//            j2 = 0;
		//        }
		//        if (!Util.LineIsOutsideLTRB(pt[j], pt[j2], param.rc)) {
		//            MbeGapChkObjLine objLine = new MbeGapChkObjLine();
		//            objLine.SetLineValue(pt[j], pt[j2], param.traceWidth);
		//            outlineList.AddLast(objLine);
		//        }
		//    }
		//}

        public int InnerLandDia
        {
            get { return (padSize.Width < padSize.Height ? padSize.Width : padSize.Height); }
        }


		/// <summary>
		/// CAM�f�[�^�̐��� 
		/// </summary>
		/// <param name="camOut"></param>
		public override void GenerateCamData(CamOut camOut)
		{
			int width = PadSize.Width;
			int height = PadSize.Height;
			int stWidth = PadSize.Width + SrMargin * 2;
			int stHeight = PadSize.Height + SrMargin * 2;
			int drillSize = Diameter;
			Point pt0 = GetPos(0);
			Point pt1 = GetPos(0);
            int innerLandDia = InnerLandDia;


			CamOutBaseData camd;
			CamOutBaseData.Shape _shape = (shape == PadShape.Rect ? CamOutBaseData.Shape.Rect : CamOutBaseData.Shape.Obround);

			camd = new CamOutBaseData(MbeLayer.LayerValue.DRL,CamOutBaseData.CamType.DRILL,
									  CamOutBaseData.Shape.DrillPTH, drillSize, drillSize, pt0, pt1);
			camOut.Add(camd);

			camd = new CamOutBaseData(MbeLayer.LayerValue.CMP, CamOutBaseData.CamType.FLASH,
									_shape, width, height, pt0, pt1);
			camOut.Add(camd);

            if ((innerLayerConnectionInfo & (ulong)MbeLayer.LayerValue.L2) != 0) {
                camd = new CamOutBaseData(MbeLayer.LayerValue.L2, CamOutBaseData.CamType.FLASH,
                                        _shape, innerLandDia, innerLandDia, pt0, pt1);
                camOut.Add(camd);
            }

            if ((innerLayerConnectionInfo & (ulong)MbeLayer.LayerValue.L3) != 0) {
                camd = new CamOutBaseData(MbeLayer.LayerValue.L3, CamOutBaseData.CamType.FLASH,
                            _shape, innerLandDia, innerLandDia, pt0, pt1);
                camOut.Add(camd);
            }

			camd = new CamOutBaseData(MbeLayer.LayerValue.SOL, CamOutBaseData.CamType.FLASH,
									_shape, width, height, pt0, pt1);
			camOut.Add(camd);

            if (!no_ResistMask) {
                camd = new CamOutBaseData(MbeLayer.LayerValue.STC, CamOutBaseData.CamType.FLASH,
                                        _shape, stWidth, stHeight, pt0, pt1);
                camOut.Add(camd);

                camd = new CamOutBaseData(MbeLayer.LayerValue.STS, CamOutBaseData.CamType.FLASH,
                                        _shape, stWidth, stHeight, pt0, pt1);
                camOut.Add(camd);
            }

		}


        private readonly MbeLayer.LayerValue[] gapChkLayerTable =
		    {
			    MbeLayer.LayerValue.CMP,
                MbeLayer.LayerValue.L2,
                MbeLayer.LayerValue.L3,
			    MbeLayer.LayerValue.SOL
		    };

		public override void GenerateGapChkData(LinkedList<MbeGapChkObj> chkObjList,int _netNum)
		//public override void GenerateGapChkData(MbeGapChk gapChk, int _netNum)
		{
			for (int i = 0; i < gapChkLayerTable.Length; i++) {
                MbeLayer.LayerValue layerValue = gapChkLayerTable[i];
                if (layerValue == MbeLayer.LayerValue.L2 || layerValue == MbeLayer.LayerValue.L3) {
                    //���w�̂Ƃ��͔�ڑ����̓h�����a+0.4mm�B(�������A�\�wpad�T�C�Y�𒴂��Ȃ��j�ڑ�����padSize��Width��Height�̏�������
                    int landDia;
                    if (((ulong)layerValue & innerLayerConnectionInfo) == 0) {
                        landDia = dia + 4000;
                        if (landDia > InnerLandDia) {
                            landDia = InnerLandDia;
                        }
                    } else {
                        landDia = InnerLandDia;
                    }
                    MbeGapChkObjPoint gapChkObj = new MbeGapChkObjPoint();
                    gapChkObj.layer = layerValue;
                    gapChkObj.netNum = _netNum;
                    gapChkObj.mbeObj = this;
                    gapChkObj.SetPointValue(GetPos(0), landDia);
                    chkObjList.AddLast(gapChkObj);

                } else if (shape == PadShape.Rect) {
					MbeGapChkObjRect gapChkObj = new MbeGapChkObjRect();
					gapChkObj.layer = layerValue;
					gapChkObj.netNum = _netNum;
					gapChkObj.mbeObj = this;
					gapChkObj.SetRectValue(GetPos(0), PadSize.Width, PadSize.Height);
					//gapChk.Add(gapChkObj);
					chkObjList.AddLast(gapChkObj);
				} else {
					if (PadSize.Width == PadSize.Height) {
						MbeGapChkObjPoint gapChkObj = new MbeGapChkObjPoint();
						gapChkObj.layer = layerValue;
						gapChkObj.netNum = _netNum;
						gapChkObj.mbeObj = this;
						gapChkObj.SetPointValue(GetPos(0),PadSize.Width);
						//gapChk.Add(gapChkObj);
						chkObjList.AddLast(gapChkObj);
					} else {
						MbeGapChkObjLine gapChkObj = new MbeGapChkObjLine();
						gapChkObj.layer = layerValue;
						gapChkObj.netNum = _netNum;
						gapChkObj.mbeObj = this;
						gapChkObj.SetLineValue(GetPos(0), PadSize.Width, PadSize.Height);
						//gapChk.Add(gapChkObj);
						chkObjList.AddLast(gapChkObj);
					}
				}
			}
		}


	}
}
