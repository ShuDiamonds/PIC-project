using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;



using CE3IO;
namespace mbe
{
	class MbeObjArc : MbeObj
	{

		public const int DEFAULT_RADIUS = 20000; //2mm
		public const int DEFAULT_START_ANGLE = 0; 	//0��
		public const int DEFAULT_END_ANGLE = 2700; 	//270��

        public const int CAMOUT_MAXLINELENGTH = 20000;
        public const int CAMOUT_DIVISION = 36;
		public const int OUTLINE_DIVISION =36;
        //public const int OUTLINE_DIVISION = 16;


		/// <summary>
		/// ���C�����̐ݒ�Ǝ擾 
		/// </summary>
		public int LineWidth
		{
			get { return lineWidth; }
			set
			{
				lineWidth = value;
				if (lineWidth < MbeObjLine.MIN_LINE_WIDTH) lineWidth = MbeObjLine.MIN_LINE_WIDTH;
				else if (lineWidth > MbeObjLine.MAX_LINE_WIDTH) lineWidth = MbeObjLine.MAX_LINE_WIDTH;
			}
		}

		public int Radius
		{
			get { return radius; }
			set { radius = value; }
		}


		public int StartAngle
		{
			get { return startAngle; }
			set { 
				startAngle = value % 3600;
				if(startAngle<0) startAngle+=3600;
			}
		}

		public int EndAngle
		{
			get { return endAngle; }
			set { 
				endAngle = value % 3600; 
				if(endAngle<0) endAngle+=3600;
			}
		}


		/// <summary>
		/// ���a�A�p�x��񂩂�Position��ݒ肷��
		/// </summary>
		public void SetupPosition()
		{
			int x;
			int y;
			double angleRad;
			angleRad = (double)startAngle/1800.0*Math.PI;
            x = (int)Math.Round(radius * Math.Cos(angleRad)) + posArray[0].X;
            y = (int)Math.Round(radius * Math.Sin(angleRad)) + posArray[0].Y;
			posArray[1] = new Point(x, y);

			angleRad = (double)endAngle / 1800.0 * Math.PI;
            x = (int)Math.Round(radius * Math.Cos(angleRad)) + posArray[0].X;
            y = (int)Math.Round(radius * Math.Sin(angleRad)) + posArray[0].Y;
			posArray[2] = new Point(x, y);
		}


		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public MbeObjArc()
		{
			posCount = 3;//���S�A�n�_�A�I�_
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			for(int i=0;i<posCount;i++){
				selectFlag[i] = false;
			}
			layer = MbeLayer.LayerValue.PLC;
			posArray[0] = new Point(0, 0);
			lineWidth = MbeObjLine.DEFAULT_LINE_WIDTH;
			startAngle = DEFAULT_START_ANGLE;
			endAngle = DEFAULT_END_ANGLE;
			radius = DEFAULT_RADIUS;
			SetupPosition();
		}

		/// <summary>
		/// �R�s�[�R���X�g���N�^
		/// </summary>
		/// <param name="mbeObjPin"></param>
		public MbeObjArc(MbeObjArc mbeObjArc)
			: base(mbeObjArc)
		{
			lineWidth = mbeObjArc.lineWidth;
			startAngle = mbeObjArc.startAngle;
			endAngle = mbeObjArc.endAngle;
			radius = mbeObjArc.radius;
			SetupPosition();
		}

		/// <summary>
		/// �L�����ǂ�����Ԃ�
		/// </summary>
		/// <returns></returns>
		public override bool IsValid()
		{
			if (GetPos(0).Equals(GetPos(1))) return false;
			if (GetPos(0).Equals(GetPos(2))) return false;
			//if (GetPos(1).Equals(GetPos(2))) return false;
			return true;
		}

		/// <summary>
		/// �}�ʃI�u�W�F�N�g���Ƃ�ID�l��Ԃ�
		/// </summary>
		public override MbeObjID Id()
		{
			return MbeObjID.MbeArc;
		}


		/// <summary>
		/// ���C���[�l�̎擾�Ɛݒ�
		/// </summary>
		public override MbeLayer.LayerValue Layer
		{
			get
			{
				return layer;
			}
			set
			{
				if ((SelectableLayer() & (ulong)value) != 0) {
					layer = value;
				} else {
					layer = MbeLayer.LayerValue.DOC;
				}
				snapLayer = (ulong)layer |
					(ulong)MbeLayer.LayerValue.PTH;
			}
		}

		/// <summary>
		/// �z�u�w��\�ȃ��C���[
		/// </summary>
		/// <returns></returns>
		public static ulong SelectableLayer()
		{
			return (ulong)MbeLayer.LayerValue.PLC |
					(ulong)MbeLayer.LayerValue.PLS |
					(ulong)MbeLayer.LayerValue.STC |
					(ulong)MbeLayer.LayerValue.STS |
					(ulong)MbeLayer.LayerValue.CMP |
                    (ulong)MbeLayer.LayerValue.L2  |
                    (ulong)MbeLayer.LayerValue.L3  |
                    (ulong)MbeLayer.LayerValue.SOL |
					(ulong)MbeLayer.LayerValue.DIM |
					(ulong)MbeLayer.LayerValue.DOC;
		}

		public static MbeLayer.LayerValue NewSelectLayer(MbeLayer.LayerValue oldLayer)
		{
			if ((SelectableLayer() & (ulong)oldLayer) != 0) {
				return oldLayer;
			} else {
				if (MbeLayer.IsComponentSide(oldLayer)) {
					return MbeLayer.LayerValue.PLC;
				} else {
					return MbeLayer.LayerValue.PLS;
				}
			}
		}

		/// <summary>
		/// �w�肵���C���f�b�N�X�̈ʒu�̐ݒ�
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		public override void SetPos(Point pos, int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			double angleRad;
			if (index == 0) {
				posArray[0] = pos;
			}else{
				int xd = pos.X-posArray[0].X;
				int yd = pos.Y-posArray[0].Y;
                Radius = (int)Math.Round(Math.Sqrt(Math.Pow(xd, 2) + Math.Pow(yd, 2)));
				angleRad = Math.Atan2(yd,xd);
                int angle = (int)Math.Round(angleRad / Math.PI * 1800.0);
				if(index==1){
					StartAngle = angle;
				}else{
					EndAngle = angle;
				}
			}
			SetupPosition();
		}

		/// <summary>
		/// �ړ�
		/// </summary>
		/// <param name="selectedOnly">�I���t���O�������Ă�����̂����ړ�����ꍇ��true</param>
		/// <param name="offset">�ړ���</param>
		public override void Move(bool selectedOnly, Point offset, Point ptAbs, bool moveSingle)
		{
            //���S���W���I������Ă���Ƃ��A���[�_���I������Ă���Ƃ��͑S�̂��I������Ă���Ƃ݂Ȃ�
			if (!selectedOnly || selectFlag[0] || (selectFlag[1] && selectFlag[2])) {	
                Point newPos = posArray[0];
				newPos.Offset(offset);
				SetPos(newPos,0);
				return;
			}

			int index;
			if (selectFlag[1]) {
				index = 1;
			} else if (selectFlag[2]) {
				index = 2;
			} else {
				return;
			}
			if (moveSingle) {
				SetPos(ptAbs, index);
			} else {
				Point newPos = posArray[0];
				newPos.Offset(offset);
				SetPos(newPos, 0);
				return;
			}
		}

		/// <summary>
		/// ��]
		/// </summary>
		/// <param name="selectedOnly"></param>
		/// <param name="ptCenter"></param>
		public override void Rotate90(bool selectedOnly, Point ptCenter)
		{
			int x = posArray[0].X - ptCenter.X;
			int y = posArray[0].Y - ptCenter.Y;
			int newx = -y + ptCenter.X;
			int newy = x + ptCenter.Y;
			posArray[0] = new Point(newx, newy);

			StartAngle = StartAngle + 900;
			EndAngle = EndAngle + 900;

			SetupPosition();
		}

		public override void Flip(int hCenter)
		{
			int x = hCenter - (posArray[0].X - hCenter);
			int y = posArray[0].Y;
			posArray[0] = new Point(x, y);
			int newStartAngle = 1800 - EndAngle;
			int newEndAngle = 1800 - StartAngle;
			StartAngle = newStartAngle;
			EndAngle = newEndAngle;
			SetupPosition();
			Layer = MbeLayer.Flip(layer);
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
				case "WIDTH":
					try { LineWidth = Convert.ToInt32(str2); }
					catch (Exception) { LineWidth = MbeObjLine.DEFAULT_LINE_WIDTH; }
					return ReadCE3.RdStatus.NoError;
				case "RADWIDTH":
					try { Radius = Convert.ToInt32(str2); }
					catch (Exception) { Radius = DEFAULT_RADIUS; }
					return ReadCE3.RdStatus.NoError;
				case "S_ANGLE":
					try { StartAngle = Convert.ToInt32(str2); }
					catch (Exception) { StartAngle = DEFAULT_START_ANGLE; }
					return ReadCE3.RdStatus.NoError;
				case "E_ANGLE":
					try { EndAngle = Convert.ToInt32(str2); }
					catch (Exception) { EndAngle = DEFAULT_END_ANGLE; }
					return ReadCE3.RdStatus.NoError;
				default:
					return base.RdMb3Member(str1, str2, readCE3);
			}
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
					if (str1 != "-MBE_ARC") {
						return ReadCE3.RdStatus.FormatError;
					} else {
						SetupPosition();
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
		/// WriteCE3�N���X�փ����o�[�̏�������
		/// </summary>
		/// <param name="writeCE3">�������ݑΏ�WriteCE3�N���X</param>
		/// <param name="origin">�������ݎ��̌��_</param>
		/// <returns>����I����true</returns>
		public override bool WrMb3Member(WriteCE3 writeCE3, Point origin)
		{
			posCount = 1;
			base.WrMb3Member(writeCE3, origin);
			posCount = 3;
			writeCE3.WriteRecordInt("WIDTH", LineWidth);
			writeCE3.WriteRecordInt("RADWIDTH", Radius);
			writeCE3.WriteRecordInt("S_ANGLE", StartAngle);
			writeCE3.WriteRecordInt("E_ANGLE", EndAngle);
			return true;
		}

		/// <summary>
		/// Mb3�t�@�C���ւ̏�������
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public override bool WrMb3(WriteCE3 writeCE3, Point origin)
		{
			writeCE3.WriteRecord("+MBE_ARC");
			WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecord("-MBE_ARC");
			writeCE3.WriteNewLine();
			return true;
		}

		/// <summary>
		/// �������s��
		/// </summary>
		/// <returns></returns>
		public override MbeObj Duplicate()
		{
			MbeObj newObj = new MbeObjArc(this);
			return newObj;
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
		/// ���[�_���I�΂��Ǝ����I�ɒ��S���I��
		/// </remarks>
		public override bool SelectIt(MbeRect rc, ulong layerMask, bool pointMode)
		{
			bool result = base.SelectIt(rc, layerMask, pointMode);
			if(result){
				if(selectFlag[0]){
					//selectFlag[1]=true;
					//selectFlag[2]=true;
				}else if(selectFlag[1] && selectFlag[2]){
					if (!selectFlag[0]) {
						if (LimitStartEnd()) {
							selectFlag[2] = false;
						} else {
							//selectFlag[0] = true;
						}
					}
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
        public override MbeObj ConChkSeed(MbeRect rc, ulong layerMask)
        {
            if (SelectIt(rc, layerMask, true)) {
                if (selectFlag[1] || selectFlag[2]) {   //�I�����W�����S�������Ƃ��͐ڑ��Ώۂƌ��􂳂Ȃ��B
                    ClearAllSelectFlag();
                    return this;
                }
            }
            ClearAllSelectFlag();
            return null;
        }


        /// <summary>
        /// �ڑ��`�F�b�N�̃A�N�e�B�u��Ԃ̐ݒ�
        /// </summary>
        public override void SetConnectCheck()
        {
            connectionCheckActive = true;
        }


		public bool LimitStartEnd()
		{
			int n = (Math.Abs(StartAngle - EndAngle)) % 3600;
			if (n < 100 || n > 3500) return true;
			return false;
		}
			


		/// <summary>
		/// �`��(abstract ���\�b�h)
		/// </summary>
		/// <param name="dp"></param>
		public override void Draw(DrawParam dp)
		{
			if (dp.layer != Layer) {
				return;
			}

            //0.46.04 �`��Y���̒ጸ
            //0.49.02 �`��Y���̒ጸ
            Point lt = GetPos(0);
            Point rb = lt;
            lt.Offset(-Radius, Radius);
            lt = ToDrawDim(lt, dp.scale);
            rb.Offset(Radius, -Radius);
            rb = ToDrawDim(rb, dp.scale);
            
            
            int drawWidth = rb.X - lt.X;
            int drawHeight = rb.Y - lt.Y;


           
            if (drawWidth == 0) {       //0.49.20 
                drawWidth = 1;
            }
            if (drawHeight == 0) {      //0.49.20 
                drawHeight = 1;
            }


            Rectangle rc = new Rectangle(lt.X, lt.Y, drawWidth, drawHeight);

			
			Color col;
			int w = 0;
			Pen pen;


			//�`�敝�̐ݒ�
			w = (int)(LineWidth / dp.scale) | 1;

			if (dp.mode == MbeDrawMode.Print) {
                if ((dp.option & (uint)MbeDrawOption.PrintColor) != 0) {    //�J���[���
                    uint nColor;
                    switch (dp.layer) {
                        case MbeLayer.LayerValue.PLC:
                            nColor = MbeColors.PRINT_PLC;
                            break;
                        case MbeLayer.LayerValue.PLS:
                            nColor = MbeColors.PRINT_PLS;
                            break;
                        case MbeLayer.LayerValue.STC:
                            nColor = MbeColors.PRINT_STC;
                            break;
                        case MbeLayer.LayerValue.STS:
                            nColor = MbeColors.PRINT_STS;
                            break;
                        case MbeLayer.LayerValue.CMP:
                            nColor = MbeColors.PRINT_CMP;
                            break;
                        case MbeLayer.LayerValue.L2:
                            nColor = MbeColors.PRINT_L2;
                            break;
                        case MbeLayer.LayerValue.L3:
                            nColor = MbeColors.PRINT_L3;
                            break;
                        case MbeLayer.LayerValue.SOL:
                            nColor = MbeColors.PRINT_SOL;
                            break;
                        case MbeLayer.LayerValue.DIM:
                            nColor = MbeColors.PRINT_DIM;
                            break;
                        default:
                            nColor = MbeColors.PRINT_DOC;
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
					case MbeLayer.LayerValue.PLC:
						nColor = MbeColors.PLC;
						break;
					case MbeLayer.LayerValue.PLS:
						nColor = MbeColors.PLS;
						break;
					case MbeLayer.LayerValue.STC:
						nColor = MbeColors.STC;
						break;
					case MbeLayer.LayerValue.STS:
						nColor = MbeColors.STS;
						break;
					case MbeLayer.LayerValue.CMP:
						nColor = MbeColors.CMP;
						break;
                    case MbeLayer.LayerValue.L2:
                        nColor = MbeColors.L2;
                        break;
                    case MbeLayer.LayerValue.L3:
                        nColor = MbeColors.L3;
                        break;
                    case MbeLayer.LayerValue.SOL:
						nColor = MbeColors.SOL;
						break;
					case MbeLayer.LayerValue.DIM:
						nColor = MbeColors.DIM;
						break;
					default:
						nColor = MbeColors.DOC;
						break;
				}

				if (dp.mode == MbeDrawMode.Temp || connectionCheckActive) {
					col = Color.FromArgb(MBE_OBJ_ALPHA, MbeColors.HighLighten(nColor));
				} else {
					col = Color.FromArgb(unchecked((int)nColor));
					col = Color.FromArgb(MBE_OBJ_ALPHA, col);
				}
			}

			pen = new Pen(col, w);
			pen.StartCap = LineCap.Round;
			pen.EndCap = LineCap.Round;

			int sweepAngle;

			if (LimitStartEnd()) {
				sweepAngle = 3600;
			} else 	if (StartAngle > EndAngle) {
				sweepAngle = 3600 - StartAngle + EndAngle;
			} else {
				sweepAngle = EndAngle-StartAngle;
			}
			if (drawWidth > 0) {
				dp.g.DrawArc(pen, rc, ((float)-StartAngle) / 10, ((float)-sweepAngle) / 10);
			}
			pen.Dispose();

			if (drawSnapMark && dp.mode != MbeDrawMode.Print) {
				Point pt0 = GetPos(0);
				Point pt1 = GetPos(1);
				Point pt2 = GetPos(2);
				pt0 = ToDrawDim(pt0, dp.scale);
				pt1 = ToDrawDim(pt1, dp.scale);
				pt2 = ToDrawDim(pt2, dp.scale);

				int marksize = (int)(drawWidth/10);
				if (marksize < 10) marksize = 10;
				DrawSnapPointMark(dp.g, pt0, marksize, selectFlag[0]);

				marksize = w;
				if (marksize < 5) marksize = 5;
				if (!selectFlag[2]) {
					DrawSnapPointMark(dp.g, pt2, marksize, selectFlag[2]);
					DrawSnapPointMark(dp.g, pt1, marksize, selectFlag[1]);
				} else {
					DrawSnapPointMark(dp.g, pt1, marksize, selectFlag[1]);
					DrawSnapPointMark(dp.g, pt2, marksize, selectFlag[2]);
				}
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
			if (param.layer != Layer) {
				return;
			}
			int distance = (LineWidth + param.traceWidth) / 2 + param.gap;
			distance = distance * 11 / 10;
			int insideRadius = radius - distance;
            int outsideRadius = (radius + distance) *1005 / 1000;

            distance = outsideRadius - radius;

			int left = param.rc.L - distance;
			int top = param.rc.T + distance;
			int right = param.rc.R + distance;
			int bottom = param.rc.B - distance;
			Point[] pt;
			MbeRect rcArea = new MbeRect(new Point(left, top), new Point(right, bottom));

			//--------------------------------------------
			int _endAngle;

			if (LimitStartEnd()) {
				_endAngle = startAngle + 3600;
			} else {
				_endAngle = endAngle;
				if (_endAngle == startAngle) _endAngle = startAngle + 3600;
				else if (_endAngle < startAngle) _endAngle += 3600;
			}

			if ((startAngle % 3600) != (_endAngle % 3600)) {
				for (int i = 1; i <= 2; i++) {
					if (!Util.PointIsOutsideLTRB(GetPos(i), rcArea)) {
						Util.PointOutlineData(GetPos(i), distance, out pt);
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
				}
			}


			//--------------------------------------------
			//int _endAngle;

			//if (LimitStartEnd()) {
			//    _endAngle = startAngle + 3600;
			//} else {
			//    _endAngle = endAngle;
			//    if (_endAngle == startAngle) _endAngle = startAngle + 3600;
			//    else if (_endAngle < startAngle) _endAngle += 3600;
			//}

			int angle = startAngle;

			bool endFlag = false;
			int x;
			int y;
			double angleRad;
			angleRad = (double)angle / 1800.0 * Math.PI;
            x = (int)Math.Round(outsideRadius * Math.Cos(angleRad)) + posArray[0].X;
            y = (int)Math.Round(outsideRadius * Math.Sin(angleRad)) + posArray[0].Y;
			Point pt0 = new Point(x, y);
			Point pt1;
			while (!endFlag) {
				angle += 3600 / OUTLINE_DIVISION;
				if (angle >= _endAngle) {
					endFlag = true;
					angle = _endAngle;
				}
				angleRad = (double)angle / 1800.0 * Math.PI;
                x = (int)Math.Round(outsideRadius * Math.Cos(angleRad)) + posArray[0].X;
                y = (int)Math.Round(outsideRadius * Math.Sin(angleRad)) + posArray[0].Y;
				pt1 = new Point(x, y);

				if (!pt0.Equals(pt1)) {//�[�����f�[�^�͍폜
					if (!Util.LineIsOutsideLTRB(pt0, pt1, param.rc)) {
						MbeGapChkObjLine objLine = new MbeGapChkObjLine();
						objLine.SetLineValue(pt0, pt1, param.traceWidth);
						outlineList.AddLast(objLine);
					}
					pt0 = pt1;
				}
			}

			//--------------------------------------------

			if (insideRadius > 10) {
				int division = (insideRadius > 10000 ? OUTLINE_DIVISION : 8);
				angle = startAngle;
				endFlag = false;
				angleRad = (double)angle / 1800.0 * Math.PI;
                x = (int)Math.Round(insideRadius * Math.Cos(angleRad)) + posArray[0].X;
                y = (int)Math.Round(insideRadius * Math.Sin(angleRad)) + posArray[0].Y;
				pt0 = new Point(x, y);
				while (!endFlag) {
					angle += 3600 / division;
					if (angle >= _endAngle) {
						endFlag = true;
						angle = _endAngle;
					}
					angleRad = (double)angle / 1800.0 * Math.PI;
                    x = (int)Math.Round(insideRadius * Math.Cos(angleRad)) + posArray[0].X;
                    y = (int)Math.Round(insideRadius * Math.Sin(angleRad)) + posArray[0].Y;
					pt1 = new Point(x, y);

					if (!pt0.Equals(pt1)) {//�[�����f�[�^�͍폜
						if (!Util.LineIsOutsideLTRB(pt0, pt1, param.rc)) {
							MbeGapChkObjLine objLine = new MbeGapChkObjLine();
							objLine.SetLineValue(pt0, pt1, param.traceWidth);
							outlineList.AddLast(objLine);
						}
						pt0 = pt1;
					}
				}
			}
		}

		/// <summary>
		/// CAM�f�[�^�̐��� 
		/// </summary>
		/// <param name="camOut"></param>
		public override void GenerateCamData(CamOut camOut)
		{
			int _endAngle;

			if (LimitStartEnd()) {
				_endAngle = startAngle + 3600;
			} else {
				_endAngle = endAngle;
				if (_endAngle == startAngle) _endAngle = startAngle + 3600;
				else if (_endAngle < startAngle) _endAngle += 3600;
			}



			int angle = startAngle;

			bool endFlag = false;
			int x;
			int y;
			double angleRad;
			angleRad = (double)angle/1800.0*Math.PI;
            x = (int)Math.Round(radius * Math.Cos(angleRad)) + posArray[0].X;
            y = (int)Math.Round(radius * Math.Sin(angleRad)) + posArray[0].Y;
			Point pt0 = new Point(x,y);
			Point pt1;

            double division = radius * Math.PI * 2 / CAMOUT_MAXLINELENGTH;
            if (division < CAMOUT_DIVISION) {
                division = CAMOUT_DIVISION;
            }

			while (!endFlag) {
                angle += (int)Math.Round(3600 / division);
				if (angle >= _endAngle) {
					endFlag = true;
					angle = _endAngle;
				}
				angleRad = (double)angle / 1800.0 * Math.PI;
                x = (int)Math.Round(radius * Math.Cos(angleRad)) + posArray[0].X;
                y = (int)Math.Round(radius * Math.Sin(angleRad)) + posArray[0].Y;
				pt1 = new Point(x, y);

				if (!pt0.Equals(pt1)) {//�[�����f�[�^�͏o�͂��Ȃ�
					CamOutBaseData camd = new CamOutBaseData(layer,
											  CamOutBaseData.CamType.VECTOR,
											  CamOutBaseData.Shape.Obround,
											  lineWidth, lineWidth, pt0, pt1);
					camOut.Add(camd);
					pt0 = pt1;
				}
			}
		}

		public override void GenerateGapChkData(LinkedList<MbeGapChkObj> chkObjList,int _netNum)
		//public override void GenerateGapChkData(MbeGapChk gapChk, int _netNum)
		{
			if (layer != MbeLayer.LayerValue.CMP &&
                layer != MbeLayer.LayerValue.L2 &&
                layer != MbeLayer.LayerValue.L3 &&
                layer != MbeLayer.LayerValue.SOL) {
				return;
			} else {
				int _endAngle;

				if (LimitStartEnd()) {
					_endAngle = startAngle + 3600;
				} else {
					_endAngle = endAngle;
					if (_endAngle == startAngle) _endAngle = startAngle + 3600;
					else if (_endAngle < startAngle) _endAngle += 3600;
				}



				int angle = startAngle;

				bool endFlag = false;
				int x;
				int y;
				double angleRad;
				angleRad = (double)angle / 1800.0 * Math.PI;
                x = (int)Math.Round(radius * Math.Cos(angleRad)) + posArray[0].X;
                y = (int)Math.Round(radius * Math.Sin(angleRad)) + posArray[0].Y;
				Point pt0 = new Point(x, y);
				Point pt1;

                double division = radius * Math.PI * 2 / CAMOUT_MAXLINELENGTH;
                if (division < CAMOUT_DIVISION) {
                    division = CAMOUT_DIVISION;
                }

				while (!endFlag) {
                    angle += (int)Math.Round(3600 / division);
					//angle += 3600 / CAMOUT_DIVISION;
					if (angle >= _endAngle) {
						endFlag = true;
						angle = _endAngle;
					}
					angleRad = (double)angle / 1800.0 * Math.PI;
                    x = (int)Math.Round(radius * Math.Cos(angleRad)) + posArray[0].X;
                    y = (int)Math.Round(radius * Math.Sin(angleRad)) + posArray[0].Y;
					pt1 = new Point(x, y);

					if (!pt0.Equals(pt1)) {
						MbeGapChkObjLine gapChkObj = new MbeGapChkObjLine();
						gapChkObj.layer = layer;
						gapChkObj.netNum = _netNum;
						gapChkObj.mbeObj = this;
						gapChkObj.SetLineValue(pt0, pt1, lineWidth);
						//gapChk.Add(gapChkObj);
						chkObjList.AddLast(gapChkObj);
						pt0 = pt1;
					}
				}
			
			}
		}

        /// <summary>
        /// �`��͈͂𓾂�
        /// </summary>
        /// <returns></returns>
        public override MbeRect OccupationRect()
        {
            int x = GetPos(0).X;
            int y = GetPos(0).Y;
            int l;
            int r;
            int t;
            int b;

            if (LimitStartEnd()) {
                l = x - radius;
                t = y + radius;
                r = x + radius;
                b = y - radius;
            } else {
                if (startAngle < endAngle) {
                    if (startAngle < 900 && 900 < endAngle) {
                        t = y + radius;
                    } else {
                        t = (GetPos(1).Y > GetPos(2).Y ? GetPos(1).Y : GetPos(2).Y);
                    }
                    if (startAngle < 1800 && 1800 < endAngle) {
                        l = y + radius;
                    } else {
                        l = (GetPos(1).X < GetPos(2).X ? GetPos(1).X : GetPos(2).X);
                    }
                    if (startAngle < 2700 && 2700 < endAngle) {
                        b = y + radius;
                    } else {
                        b = (GetPos(1).Y < GetPos(2).Y ? GetPos(1).Y : GetPos(2).Y);
                    }
                    r = (GetPos(1).X > GetPos(2).X ? GetPos(1).X : GetPos(2).X);
                } else {
                    if (startAngle < 900 || 900 < endAngle) {
                        t = y + radius;
                    } else {
                        t = (GetPos(1).Y > GetPos(2).Y ? GetPos(1).Y : GetPos(2).Y);
                    }
                    if (startAngle < 1800 || 1800 < endAngle) {
                        l = y + radius;
                    } else {
                        l = (GetPos(1).X < GetPos(2).X ? GetPos(1).X : GetPos(2).X);
                    }
                    if (startAngle < 2700 || 2700 < endAngle) {
                        b = y + radius;
                    } else {
                        b = (GetPos(1).Y < GetPos(2).Y ? GetPos(1).Y : GetPos(2).Y);
                    }
                    r = x + radius;
                }
            }
            return new MbeRect(new Point(l, t), new Point(r, b));
        }

		protected int lineWidth;
		protected int radius;
		protected int startAngle;
		protected int endAngle;
	}
}
