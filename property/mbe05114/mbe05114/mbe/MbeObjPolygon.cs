using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;

using CE3IO;
namespace mbe
{
	/// <summary>
	/// �ʒu���C���f�b�N�X0�́A�ڑ���B1�ȍ~���|���S�����
	/// </summary>
	class MbeObjPolygon : MbeObj
	{
		public const int DEFAULT_TRACE_WIDTH	= 8000;		//0.8mm
		public const int DEFAULT_PATTERN_GAP	= 8000;		//0.8mm
		public const int DEFAULT_THERMAL_GAP	= 3000;		//0.3mm
		public const int DEFAULT_PRIORITY		= 10;
        public const int MAX_PRIORITY = Int32.MaxValue;
        public const int MIN_PRIORITY = 0;

        public const int RESTRICT_TRACE_WIDTH = 500;    //0.05mm
        public const int RESTRICT_GAP         = 500;    //0.05mm


		public const bool DEFUALT_REMOVE_FLOATING = false;
        public const bool DEFAULT_RESTRICT_MASK = false;

		public const int MAX_TRACE_WIDTH = 20000;		//2.0mm
		public const int MAX_PATTERN_GAP = 20000;		//2.0mm
		public const int MAX_THERMAL_GAP = 10000;		//1.0mm

		public const int MIN_TRACE_WIDTH = 3000;		//0.4mm
		public const int MIN_PATTERN_GAP = 3000;		//0.4mm
		public const int MIN_THERMAL_GAP = 2000;		//0.2mm


		/// <summary>
		/// �z�����̐ݒ�Ǝ擾
		/// </summary>
		public int TraceWidth
		{
			get { return traceWidth; }
			set { traceWidth = Util.LimitRange(value,MIN_TRACE_WIDTH,MAX_TRACE_WIDTH); }
		}

		/// <summary>
		/// ���̔z���Ƃ̃M���b�v�̐ݒ�Ǝ擾
		/// </summary>
		public int PatternGap
		{
			get { return patternGap; }
			set { patternGap = Util.LimitRange(value,MIN_PATTERN_GAP,MAX_PATTERN_GAP); }
		}

		/// <summary>
		/// �ڑ���z���̃s���Ƃ̃T�[�}���M���b�v�̐ݒ�Ǝ擾
		/// </summary>
		public int ThermalGap
		{
			get { return thermalGap; }
			set { thermalGap = Util.LimitRange(value,MIN_THERMAL_GAP,MAX_THERMAL_GAP); }
		}

		/// <summary>
		/// �h��Ԃ��D��x�̐ݒ�Ǝ擾
		/// </summary>
		public int FillingPriority
		{
			get { return fillingPriority; }
			set { fillingPriority = value; }
		}

		public bool RemoveFloating
		{
			get { return removeFloating; }
			set { removeFloating = value; }
		}

        /// <summary>
        /// true�̂Ƃ��A�h��Ԃ��֎~�}�X�N�Ƃ��ē���
        /// </summary>
        public bool RestrictMask
        {
            get { return restrictMask; }
            set {
                if (posArray.Length > 1) {
                    if (restrictMask && !value) {
                        posArray[0] = new Point(posArray[1].X + 20000, posArray[1].Y - 15000);
                    }
                }
                restrictMask = value;
            }
        }



		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public MbeObjPolygon()
		{
			posCount = 1;
			posArray = new Point[1];
			selectFlag = new bool[1];
			//posArray = null;
			//selectFlag = null;
			//fillLineList = null;
			traceWidth = DEFAULT_TRACE_WIDTH;
			patternGap = DEFAULT_PATTERN_GAP;
			thermalGap = DEFAULT_THERMAL_GAP;
			fillingPriority = DEFAULT_PRIORITY;
			removeFloating = DEFUALT_REMOVE_FLOATING;
            restrictMask = DEFAULT_RESTRICT_MASK;
			fillLineList = new LinkedList<MbeGapChkObjLine>();
            doneFillFlag = false;
		}


		/// <summary>
		/// �R�s�[�R���X�g���N�^
		/// </summary>
		/// <param name="mbeObjPolygon"></param>
		public MbeObjPolygon(MbeObjPolygon mbeObjPolygon)
			: base(mbeObjPolygon)
		{
			traceWidth = mbeObjPolygon.traceWidth;
			patternGap = mbeObjPolygon.patternGap;
			thermalGap = mbeObjPolygon.thermalGap;
			fillingPriority = mbeObjPolygon.fillingPriority;
			removeFloating = mbeObjPolygon.removeFloating;
            restrictMask = mbeObjPolygon.restrictMask;
			fillLineList = new LinkedList<MbeGapChkObjLine>();
			foreach (MbeGapChkObjLine obj in mbeObjPolygon.fillLineList) {
				fillLineList.AddLast(obj);
			}
		}

		/// <summary>
		/// �}�ʃI�u�W�F�N�g���Ƃ�ID�l��Ԃ�
		/// </summary>
		public override MbeObjID Id()
		{
			return MbeObjID.MbePolygon;
		}

		/// <summary>
		/// �L�����ǂ�����Ԃ�
		/// </summary>
		/// <returns></returns>
		public override bool IsValid()
		{
			CleanupOverlappingPoint();
			if (posCount<4) return false;
			return true;
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
                if (((ulong)value & SelectableLayer()) !=0) {
                    layer = value;
                    snapLayer = (ulong)value;
                } else if (MbeLayer.IsComponentSide(value)) {
                    layer = MbeLayer.LayerValue.CMP;
                    snapLayer = (ulong)MbeLayer.LayerValue.CMP;
                } else {
                    layer = MbeLayer.LayerValue.SOL;
                    snapLayer = (ulong)MbeLayer.LayerValue.SOL;
                }
			}
		}

        static public ulong SnapLayer(MbeLayer.LayerValue placeLayer)
        {
            return (ulong)placeLayer;
        }


		/// <summary>
		/// �z�u�w��\�ȃ��C���[
		/// </summary>
		/// <returns></returns>
		public static ulong SelectableLayer()
		{
			return  (ulong)MbeLayer.LayerValue.CMP |
                    (ulong)MbeLayer.LayerValue.L2 |
                    (ulong)MbeLayer.LayerValue.L3 |
					(ulong)MbeLayer.LayerValue.SOL;
		}

		public static MbeLayer.LayerValue NewSelectLayer(MbeLayer.LayerValue oldLayer)
		{
			if ((SelectableLayer() & (ulong)oldLayer) != 0) {
				return oldLayer;
			} else {
				if (MbeLayer.IsComponentSide(oldLayer)) {
					return MbeLayer.LayerValue.CMP;
				} else {
					return MbeLayer.LayerValue.SOL;
				}
			}
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
			int n;
			switch (str1) {
				case "POSCOUNT":
					try { n = Convert.ToInt32(str2); }
					catch (Exception) { n = 1; }
					posCount = n;
					posArray = new Point[n];
					selectFlag = new bool[n];
					return ReadCE3.RdStatus.NoError;
				case "PTNGAP":
					try { n = Convert.ToInt32(str2); }
					catch (Exception) { n = DEFAULT_PATTERN_GAP; }
					PatternGap = n;
					return ReadCE3.RdStatus.NoError;
				case "TRACEWIDTH":
					try { n = Convert.ToInt32(str2); }
					catch (Exception) { n = DEFAULT_TRACE_WIDTH; }
					TraceWidth = n;
					return ReadCE3.RdStatus.NoError;
				case "FILLPRIORITY":
					try { n = Convert.ToInt32(str2); }
					catch (Exception) { n = DEFAULT_PRIORITY; }
					FillingPriority = n;
					return ReadCE3.RdStatus.NoError;
				case "REMOVEFLOAT":
					try { n = Convert.ToInt32(str2); }
					catch (Exception) { n = 0; }
					removeFloating = (n != 0);
					return ReadCE3.RdStatus.NoError;
                case "RESTRICTMASK":
                    try { n = Convert.ToInt32(str2); }
                    catch (Exception) { n = 0; }
                    restrictMask = (n != 0);
                    return ReadCE3.RdStatus.NoError;


				//    if (str2 == lineStyleName[(int)MbeLineStyle.Bending1]) {
				//        lineStyle = MbeLineStyle.Bending1;
				//    }
				//    return ReadCE3.RdStatus.NoError;
				//case "WIDTH":
				//    try { LineWidth = Convert.ToInt32(str2); }
				//    catch (Exception) { LineWidth = DEFAULT_LINE_WIDTH; }
				//    return ReadCE3.RdStatus.NoError;
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
					if (str1 != "-MBE_POLYGON") {
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
		/// WriteCE3�N���X�փ����o�[�̏�������
		/// </summary>
		/// <param name="writeCE3">�������ݑΏ�WriteCE3�N���X</param>
		/// <param name="origin">�������ݎ��̌��_</param>
		/// <returns>����I����true</returns>
		public override bool WrMb3Member(WriteCE3 writeCE3, Point origin)
		{
			writeCE3.WriteRecordInt("POSCOUNT", posCount);
			writeCE3.WriteRecordInt("PTNGAP", patternGap);
			writeCE3.WriteRecordInt("TRACEWIDTH", traceWidth);
			writeCE3.WriteRecordInt("REMOVEFLOAT", (removeFloating==false?0:1));
            writeCE3.WriteRecordInt("RESTRICTMASK", (restrictMask == false ? 0 : 1));
            writeCE3.WriteRecordInt("FILLPRIORITY", fillingPriority);
			base.WrMb3Member(writeCE3, origin);
			//writeCE3.WriteRecordInt("WIDTH", LineWidth);
			//writeCE3.WriteRecordString("STYLE", lineStyleName[(int)lineStyle]);
			return true;
		}

		/// <summary>
		/// Mb3�t�@�C���ւ̏�������
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public override bool WrMb3(WriteCE3 writeCE3, Point origin)
		{
			writeCE3.WriteRecord("+MBE_POLYGON");
            
            //�|���S�����֎~�}�X�N�Ƃ��Ďg����Ƃ��ɁA�ߋ��̃o�[�W�����œǂݍ��񂾂Ƃ��Ƀg���u���ɂȂ邱�Ƃ��ł��邾��������
            //�ȉ��̏������s�����ƂŁA���S�ɃR���p�`�u���ł͂Ȃ����A�s�v�Ȑڑ����������郊�X�N�͒ጸ�ł���B
            if (restrictMask) {
                //posArray[0]�͐ڑ��_�B�����}�ʊO�ɒu��
                if (posArray.Length > 1) {
                    posArray[0] = new Point(posArray[1].X + 10000000, posArray[1].Y + 10000000);
                }
                //�p�^�[���M���b�v�A�g���[�X���͍ŏ��B�t���[�e�B���O�p�^�[�����폜����B�v���C�I���e�B�͍ō��B
                patternGap = MIN_PATTERN_GAP;
                traceWidth = MIN_TRACE_WIDTH;
                removeFloating = true;
                fillingPriority = MAX_PRIORITY;
            }


			WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecord("-MBE_POLYGON");
			writeCE3.WriteNewLine();
			return true;
		}

		/// <summary>
		/// �������s��
		/// </summary>
		/// <returns></returns>
		public override MbeObj Duplicate()
		{
			MbeObj newObj = new MbeObjPolygon(this);
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
		/// </remarks>
		public override bool SelectIt(MbeRect rc, ulong layerMask, bool pointMode)
		{
			if ((layerMask & (ulong)layer) == 0) return false;
			if (DeleteCount >= 0) return false;

			bool result = false;

			if (MbeView.ViewPolygonFrame) {

				if (!pointMode) {
					result = base.SelectIt(rc, layerMask, pointMode);
                    if (restrictMask) {
                        result = false;
                        selectFlag[0] = false;
                        for (int i = 1; i < posCount; i++) {
                            if(selectFlag[i]){
                                result = true;
                            }
                        }
                    }
                    if (result) return true;
				} else {
					Point ptC = rc.Center();

                    int threshold;
                    int startPointIndex;
                    if (restrictMask) {
                        threshold = MIN_TRACE_WIDTH / 2;
                        startPointIndex = 1;
                    } else {
                        threshold = this.TraceWidth / 2;
                        startPointIndex = 0;
                    }


                    for (int index = startPointIndex; index < posCount; index++) {
						if (threshold >= (int)Util.DistancePointPoint(ptC, GetPos(index))) {
							selectFlag[index] = true;
							return true;
						}
					}

					if (posCount >= 4) {
						for (int index = 1; index < posCount; index++) {
							int index2 = index + 1;
							if (index2 >= posCount) index2 = 1;

							if (threshold >= (int)Util.DistancePointLine(ptC, GetPos(index), GetPos(index2))) {
								result = true;
								break;
							}
						}
						if (result) {
                            for (int index = startPointIndex; index < posCount; index++) {
								selectFlag[index] = true;
							}
							return true;
						}
					}
				}
			}
			return false;
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
		/// �����\���ǂ�����Ԃ��B
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="visibleLayer"></param>
		/// <param name="threshold"></param>
		/// <param name="lineIndex"></param>
		/// <param name="ptDivAt"></param>
		/// <returns></returns>
		public override bool CanDivide(Point pt, ulong visibleLayer, int threshold, out int lineIndex, out Point ptDivAt)
		{

			lineIndex = 0;
			ptDivAt = pt;

			if (!MbeView.ViewPolygonFrame) return false;
			if ((visibleLayer & (ulong)layer) == 0 || posCount < 4) {
				return false;
			}

			for (lineIndex = 1; lineIndex < posCount; lineIndex++) {
				int index2 = lineIndex + 1;
				if (index2 >= posCount) index2 = 1;

				Point pt0 = this.GetPos(lineIndex);
				Point pt1 = this.GetPos(index2);
				if (IsNearLine(pt0, pt1, pt, threshold, out ptDivAt)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// index�̎��ɓ_��ǉ�����
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool AddPointAt(Point pt,int index)
		{
			//index��K�؂Ȓl�ɂ���
			if(index>=posCount) index = posCount-1;
			if(index<0)index=0;

			////�O��̓_�Ɠ����Ȃ�ǉ����Ȃ�
			//if (posCount > 1) {
			//    if (index > 0) {
			//        if (posArray[index].Equals(pt)) return false;
			//    }
			//    int index2 = index+1;
			//    if(index2>=posCount) index2=1;
			//    if (posArray[index2].Equals(pt)) return false;
			//}

			//�V�����z��̏���
			int newPosCount = posCount+1;
			Point[] newPosArray = new Point[newPosCount];
			bool[] newSelectFlag = new bool[newPosCount];

			//�ǉ����钼�O�܂ł̔z��̃R�s�[
			int indexOldArray = 0;
			int indexNewArray = 0;
			index++;
			while(indexOldArray<index){
				newPosArray[indexNewArray] = posArray[indexOldArray];
				newSelectFlag[indexNewArray] = selectFlag[indexOldArray];
				indexOldArray++;
				indexNewArray++;
			}

			//�ǉ�����_�̒l������
			newPosArray[indexNewArray] = pt;
			newSelectFlag[indexNewArray] = false;
			indexNewArray++;

			//�ǉ������_�ȍ~�̔z��̃R�s�[
			while(indexOldArray<posCount){
				newPosArray[indexNewArray] = posArray[indexOldArray];
				newSelectFlag[indexNewArray] = selectFlag[indexOldArray];
				indexOldArray++;
				indexNewArray++;
			}

			//posCount�̍X�V�Ɣz��̓���ւ�
			posCount = newPosCount;
			posArray = newPosArray;
			selectFlag = newSelectFlag;
			return true;
		}
		
		/// <summary>
		/// �z��̍Ō�ɓ_��ǉ�
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public bool AddLastPoint(Point pt)
		{
			System.Diagnostics.Debug.WriteLine("Polygon AddLastPoint:" + (posCount + 1));
			if (posCount == 1) {
				posArray[0] = new Point(pt.X + 20000, pt.Y - 15000);
			}
			return AddPointAt(pt, posCount);
		}

		/// <summary>
		/// �_�̈ړ��ɂƂ��Ȃ��āA�d�Ȃ����_��z�񂩂�폜����B
		/// </summary>
		/// <returns></returns>
		public bool CleanupOverlappingPoint()
		{
			if (posCount < 3) return false;
			int cleanupCount = 0;

			int index1 = 1;
			int index2 = 2;

			while (index2 < posCount) {
				if (posArray[index1].Equals(posArray[index2])) {
					cleanupCount++;
				} else {
					index1++;
					if (index1 < index2) {
						posArray[index1] = posArray[index2];
						selectFlag[index1] = selectFlag[index2];
					}
				}
				index2++;
			}
			if (posArray[0].Equals(posArray[index1])) {
				cleanupCount++;
			}

			//�V�����z��̏���
			int newPosCount = posCount - cleanupCount;
			Point[] newPosArray = new Point[newPosCount];
			bool[] newSelectFlag = new bool[newPosCount];

			for (int i = 0; i < newPosCount; i++) {
				newPosArray[i] = posArray[i];
				newSelectFlag[i] = selectFlag[i];
			}

			posCount = newPosCount;
			posArray = newPosArray;
			selectFlag = newSelectFlag;

			if (cleanupCount > 0) return true;
			else return false;
		}

		/// <summary>
		/// �z�u���̍Ō�̓_���폜
		/// </summary>
		/// <returns></returns>
		public bool DeleteLastPoint()
		{
			if (posCount < 4) return false;

			//�V�����z��̏���
			int newPosCount = posCount - 1;
			Point[] newPosArray = new Point[newPosCount];
			bool[] newSelectFlag = new bool[newPosCount];

			for (int i = 0; i < newPosCount; i++) {
				newPosArray[i] = posArray[i];
				newSelectFlag[i] = selectFlag[i];
			}

			posCount = newPosCount;
			posArray = newPosArray;
			selectFlag = newSelectFlag;
			return true;
		}


        ///// <summary>
        ///// �����B�����n�I�u�W�F�N�g�ňӖ������B
        ///// </summary>
        ///// <param name="lineIndex">�����\�Ȑ��̃C���f�b�N�X�B�P�����C���Ȃ疳���B</param>
        ///// <param name="ptDivAt"></param>
        ///// <returns></returns>
        //public override bool DivideAtCenter(int lineIndex, out MbeObj newObj)
        //{
        //    newObj = null;
        //    if (lineIndex >= posCount) return false;
        //    int index2 = lineIndex + 1;
        //    if (index2 >= posCount) index2 = 1;

        //    Point pt0 = this.GetPos(lineIndex);
        //    Point pt1 = this.GetPos(index2);
        //    Point ptDivAt = new Point((pt0.X + pt1.X) / 2, (pt0.Y + pt1.Y) / 2);

        //    return AddPointAt(ptDivAt, lineIndex);
        //}

		/// <summary>
		/// �w��_�ŕ����B�����n�I�u�W�F�N�g�ňӖ������B
		/// </summary>
		/// <param name="lineIndex">�����\�Ȑ��̃C���f�b�N�X�B�P�����C���Ȃ疳���B</param>
		/// <param name="ptDivAt"></param>
		/// <returns></returns>
		public override bool DivideAtPoint(int lineIndex, Point pt, out MbeObj newObj)
		{
			newObj = null;
			//if (lineIndex >= posCount) return false;
			//int index2 = lineIndex + 1;
			//if (index2 >= posCount) index2 = 1;

			//Point pt0 = this.GetPos(lineIndex);
			//Point pt1 = this.GetPos(index2);
			//Point ptDivAt = new Point((pt0.X + pt1.X) / 2, (pt0.Y + pt1.Y) / 2);

			return AddPointAt(pt, lineIndex);
		}

		/// <summary>
		/// �ڑ��`�F�b�N�̃A�N�e�B�u��Ԃ̐ݒ�
		/// </summary>
		public override void SetConnectCheck()
		{
			connectionCheckActive = true;
		}



		/// <summary>
		/// �`��
		/// </summary>
		/// <param name="dp"></param>
		public override void Draw(DrawParam dp)
		{
			if (posCount < 3) return;
			if (dp.layer != Layer) {
				return;
			}
			Color col;
			int w = 0;
			Pen pen;
			//�`�敝�̐ݒ�
            if (restrictMask) {
                w = 1;
            }else{
                w = (int)(TraceWidth / dp.scale) | 1;
            }

           

			if (dp.mode == MbeDrawMode.Print) {
                if ((dp.option & (uint)MbeDrawOption.PrintColor) != 0) {    //�J���[���
                    uint nColor;
                    switch (dp.layer) {
                        case MbeLayer.LayerValue.CMP:
                            nColor = MbeColors.PRINT_CMP;
                            break;
                        case MbeLayer.LayerValue.L2:
                            nColor = MbeColors.PRINT_L2;
                            break;
                        case MbeLayer.LayerValue.L3:
                            nColor = MbeColors.PRINT_L3;
                            break;
                        default:	//case MbeLayer.LayerValue.SOL:
                            nColor = MbeColors.PRINT_SOL;
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
					case MbeLayer.LayerValue.CMP:
						nColor = MbeColors.CMP;
						break;
                    case MbeLayer.LayerValue.L2:
                        nColor = MbeColors.L2;
                        break;
                    case MbeLayer.LayerValue.L3:
                        nColor = MbeColors.L3;
                        break;
                    default:	//case MbeLayer.LayerValue.SOL:
						nColor = MbeColors.SOL;
						break;
				}

				if (dp.mode == MbeDrawMode.Temp || connectionCheckActive) {
					col = Color.FromArgb(MBE_OBJ_ALPHA, MbeColors.HighLighten(nColor));
                    
				} else {
					col = Color.FromArgb(unchecked((int)nColor));
					col = Color.FromArgb(MBE_OBJ_ALPHA, col);
                    
                }
			}

			Point pt0;
			Point pt1;

			if (MbeView.ViewPolygonFrame) {
				pen = new Pen(col, 1);

                if (!restrictMask) {
                    pen.DashStyle = DashStyle.Dash;
                    pt0 = ToDrawDim(posArray[0], dp.scale);
                    pt1 = ToDrawDim(posArray[1], dp.scale);
                    dp.g.DrawLine(pen, pt0, pt1);
                }

				pen.Width = w;
				pen.DashStyle = DashStyle.Solid;
                if (restrictMask && !Program.monoRuntime) {
                    Point[] drawPointArray = new Point[posCount - 1];
                    for (int i = 1; i < posCount; i++) {
                        drawPointArray[i - 1] = ToDrawDim(posArray[i], dp.scale);
                    }
                    Color bgcolor = Color.FromArgb(0, 0, 0, 0);
                    HatchBrush brush = new HatchBrush(HatchStyle.Percent20, col, bgcolor);
                    dp.g.FillPolygon(brush, drawPointArray);
                    brush.Dispose();
                    dp.g.DrawPolygon(pen, drawPointArray);
                    drawPointArray = null;
                } else {
                    HatchBrush brush = null;
                    if (!Program.monoRuntime && !Program.inhibitHatchBrushPolygonframe) {
                        Color bgcolor = Color.FromArgb(col.A,((int)col.R)*2/3,((int)col.G)*2/3,((int)col.B)*2/3);
                        brush = new HatchBrush(HatchStyle.Percent30, col, bgcolor);
                        pen.Brush = brush;
                    }

                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    for (int i = 1; i < posCount; i++) {
                        int index2 = i + 1;
                        if (index2 == posCount) {
                            index2 = 1;
                        }
                        pt0 = ToDrawDim(posArray[i], dp.scale);
                        pt1 = ToDrawDim(posArray[index2], dp.scale);
                        //System.Diagnostics.Debug.WriteLine("Frame  " + pt0.X + "," + pt0.Y + "," + pt1.X + "," + pt1.Y);
                        dp.g.DrawLine(pen, pt0, pt1);
                    }
                    if (brush != null) {
                        brush.Dispose();
                    }
                }
                
				pen.Dispose();

				if (drawSnapMark && dp.mode != MbeDrawMode.Print) {
					for (int i = 0; i < posCount; i++) {
						pt0 = ToDrawDim(posArray[i], dp.scale);
						int marksize = w;
						if (i == 0) {
                            if (!restrictMask) {
                                if (marksize < 10) marksize = 10;
                                DrawTargetMark(dp.g, pt0, marksize, selectFlag[i]);
                            }
						} else {
							DrawSnapPointMark(dp.g, pt0, marksize, selectFlag[i]);
						}
					}
				}

			} else {

				if (fillLineList.Count > 0) {
					pen = new Pen(col);
					pen.StartCap = LineCap.Round;
					pen.EndCap = LineCap.Round;
					foreach (MbeGapChkObjLine obj in fillLineList) {
						pen.Width = (int)(obj.lineWidth / dp.scale) | 1;
						pt0 = ToDrawDim(obj.p0, dp.scale);
						pt1 = ToDrawDim(obj.p1, dp.scale);
						//System.Diagnostics.Debug.WriteLine("Fill  "+pt0.X + "," + pt0.Y + "," + pt1.X + "," + pt1.Y);
						dp.g.DrawLine(pen, pt0, pt1);
					}
					pen.Dispose();
				}			
			}
		}

        public override MbePointInfo[] GetPosInfoArray()
        {
            MbePointInfo[] pinfoArray = base.GetPosInfoArray();

            if (restrictMask) {
                pinfoArray[0].selectFlag = false;
            }
            return pinfoArray;
        }

		/// <summary>
		/// �|���S���̂��߂̗֊s�f�[�^�𐶐�����B
		/// </summary>
		/// <param name="outlineList"></param>
		/// <param name="param"></param>
		/// <remarks>�����Ő�������֊s�f�[�^�͓����ɏd�Ȃ��Ă��Ă��ǂ����̂Ƃ���</remarks>
		public override void GenerateOutlineData(LinkedList<MbeGapChkObjLine> outlineList, GenOutlineParam param)
		{
            if (!doneFillFlag) return;
            //if (fillLineList.Count == 0) return;
			if (param.layer != Layer) return;
			if (posCount<=3)return;

            int distance;
            if (restrictMask) {
                distance = (RESTRICT_TRACE_WIDTH + param.traceWidth) / 2 + RESTRICT_GAP * 11 / 10;
                //distance = (traceWidth + param.traceWidth) / 2 + param.gap * 11 / 10;
            } else {
                distance = (traceWidth + param.traceWidth) / 2 + param.gap * 11 / 10;
            }

			int left = param.rc.L - distance;
			int top = param.rc.T + distance;
			int right = param.rc.R + distance;
			int bottom = param.rc.B - distance;

			MbeRect rcArea = new MbeRect(new Point(left, top), new Point(right, bottom));
			Point[] ptOutline;

			for (int i = 1; i < PosCount; i++) {
				int i2 = i + 1;
				if (i2 == PosCount) {
					i2 = 1;
				}
				Point p0 = GetPos(i);
				Point p1 = GetPos(i2);
                if (Util.LineIsOutsideLTRB(p0, p1, rcArea)) {
                    continue;
                }
				bool dummyParam;
				Util.LineOutlineData(p0, p1, distance, out ptOutline, out dummyParam);
				for (int j = 0; j < 8; j++) {
					int j2 = j + 1;
					if (j2 == 8) {
						j2 = 0;
					}
					if (!Util.LineIsOutsideLTRB(ptOutline[j], ptOutline[j2], param.rc)) {
						MbeGapChkObjLine objLine = new MbeGapChkObjLine();
						objLine.SetLineValue(ptOutline[j], ptOutline[j2], param.traceWidth);
						outlineList.AddLast(objLine);
					}
				}
			}
		}

		public void FullFillLineData(LinkedList<MbeGapChkObj> chkObjList)
		{
			//FillPolygon fP = new FillPolygon();
            FillPolygon2 fP = new FillPolygon2();
            fP.FullFill(this, chkObjList);
		}

		/// <summary>
		/// CAM�f�[�^�̐��� 
		/// </summary>
		/// <param name="camOut"></param>
		public override void GenerateCamData(CamOut camOut)
		{
			CamOutBaseData camd;
			foreach (MbeGapChkObjLine obj in fillLineList) {
				Point pt0 = obj.p0;
				Point pt1 = obj.p1;
				camd = new CamOutBaseData(layer,
										  CamOutBaseData.CamType.VECTOR,
										  CamOutBaseData.Shape.Obround,
										  traceWidth, traceWidth, pt0, pt1);
				camOut.Add(camd);
			}
		}

		//public override void GenerateGapChkData(MbeGapChk gapChk, int _netNum)
		//{
		//    if (layer != MbeLayer.LayerValue.CMP && layer != MbeLayer.LayerValue.SOL) return;

		//    Point pt0 = GetPos(0);
		//    Point pt1 = GetPos(1);

		//    if (pt0.Equals(pt1)) {
		//        MbeGapChkObjPoint gapChkObj = new MbeGapChkObjPoint();
		//        gapChkObj.layer = layer;
		//        gapChkObj.netNum = _netNum;
		//        gapChkObj.mbeObj = this;
		//        gapChkObj.SetPointValue(pt0, LineWidth);
		//        gapChk.Add(gapChkObj);
		//    } else {
		//        Point ptVia;
		//        bool bendMode = getPointVia(out ptVia);

		//        if (bendMode) {
		//            MbeGapChkObjLine gapChkObj = new MbeGapChkObjLine();
		//            gapChkObj.layer = layer;
		//            gapChkObj.netNum = _netNum;
		//            gapChkObj.mbeObj = this;
		//            gapChkObj.SetLineValue(pt0, ptVia, LineWidth);
		//            gapChk.Add(gapChkObj);

		//            gapChkObj = new MbeGapChkObjLine();
		//            gapChkObj.layer = layer;
		//            gapChkObj.netNum = _netNum;
		//            gapChkObj.mbeObj = this;
		//            gapChkObj.SetLineValue(ptVia, pt1, LineWidth);
		//            gapChk.Add(gapChkObj);
		//        } else {
		//            MbeGapChkObjLine gapChkObj = new MbeGapChkObjLine();
		//            gapChkObj.layer = layer;
		//            gapChkObj.netNum = _netNum;
		//            gapChkObj.mbeObj = this;
		//            gapChkObj.SetLineValue(pt0, pt1, LineWidth);
		//            gapChk.Add(gapChkObj);
		//        }
		//    }
		//}


		/// <summary>
		/// �z����
		/// </summary>
		protected int traceWidth;
		
		/// <summary>
		/// ���̔z���Ƃ̃M���b�v
		/// </summary>
		protected int patternGap;

		/// <summary>
		/// �ڑ���z���̃s���Ƃ̃T�[�}���M���b�v
		/// </summary>
		protected int thermalGap;

		/// <summary>
		/// �h��Ԃ��D��x
		/// </summary>
		protected int fillingPriority;

		/// <summary>
		/// true�̂Ƃ��t���[�e�B���O�p�^�[������菜��
		/// </summary>
		protected bool removeFloating;

        protected bool restrictMask;

 


		//public LinkedList<MbeObjLine> fillLineList;

		public LinkedList<MbeGapChkObjLine> fillLineList;

        public bool doneFillFlag;


		//protected MbeLineStyle lineStyle; 
	}
}
