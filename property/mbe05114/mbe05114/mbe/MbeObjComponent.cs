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
	public class MbeObjComponent : MbeObj
	{

		public string RefNumText
		{
			get{ return refNum.SigName;}
			set{ refNum.SigName = value;}
		}

		public int RefNumLineWidth
		{
			get { return refNum.LineWidth; }
			set { refNum.LineWidth = value; }
		}

		public int RefNumTextHeight
		{
			get { return refNum.TextHeight; }
			set { refNum.TextHeight = value; }
		}

		//public bool RefNumVertical
		//{
		//    get { return refNum.Vertical; }
		//    set { refNum.Vertical = value; }
		//}

		public List<MbeObj> ContentsObj
		{
			get { return contentsObj; }
		}

        public string PackageName
        {
            get { return packageName; }
            set { packageName = value; }
        }

		public string RemarksText
		{
			get { return remarksText; }
			set { remarksText = value; }
		}

        public int AngleX10
        {
            get { return angleX10; }
            set
            {
                angleX10 = value;
                if (angleX10 > 0) {
                    angleX10 = angleX10 % 3600;
                }
            }
        }


		public bool DrawRefOnDoc
		{
			get { return refNum.Layer == MbeLayer.LayerValue.DOC; }
			set
			{
				if (value != true) {
					if (layer == MbeLayer.LayerValue.CMP) {
						refNum.Layer = MbeLayer.LayerValue.PLC;
					} else if (layer == MbeLayer.LayerValue.SOL) {
						refNum.Layer = MbeLayer.LayerValue.PLS;
					} else {
						refNum.Layer = MbeLayer.LayerValue.NUL;
					}
				} else {
					refNum.Layer = MbeLayer.LayerValue.DOC;
				}
			}
		}



		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public MbeObjComponent()
		{
			posCount = 1;
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			selectFlag[0] = false;
			layer = MbeLayer.LayerValue.NUL;	//����������\���B

			refNum = new MbeObjText();
			refNum.Layer = MbeLayer.LayerValue.NUL;	//����������\���B
			Point ptZero = new Point(10000, -10000);
			refNum.SetPos(ptZero, 0);
			contentsObj = null;
			posArray[0] = new Point(0,0);
            angleX10 = -1;
            packageName = "";
			remarksText = "";
		}

		/// <summary>
		/// �R�s�[�R���X�g���N�^
		/// </summary>
		/// <param name="mbeObjPin"></param>
		public MbeObjComponent(MbeObjComponent mbeObjComponent)
			: base(mbeObjComponent)
		{
			//layer = mbeObjComponent.layer;
			refNum = (MbeObjText)mbeObjComponent.refNum.Duplicate();
            packageName = mbeObjComponent.packageName;
			remarksText = mbeObjComponent.remarksText;
            angleX10 = mbeObjComponent.angleX10;
			//pinCount = mbeObjComponent.pinCount;
			if (mbeObjComponent.contentsObj != null) {
				int n=mbeObjComponent.contentsObj.Count;
				contentsObj = new List<MbeObj>(n);
				for(int i=0;i<n;i++){
					MbeObj obj = mbeObjComponent.contentsObj[i];
					if(obj!=null)contentsObj.Add(obj.Duplicate());
				}
			}
		}

		/// <summary>
		/// �R���X�g���N�^�BCMP�ɔz�u���邱�Ƃ�O��ɂ���LinkedList����\�z
		/// </summary>
		/// <param name="ptOrigin"></param>
		/// <param name="strReerence"></param>
		/// <param name="objLList"></param>
		public MbeObjComponent(Point ptOrigin, string strReference, LinkedList<MbeObj> objLList)
		{
			posCount = 1;
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			selectFlag[0] = false;

			layer = MbeLayer.LayerValue.CMP;

			refNum = new MbeObjText();
			refNum.Layer = MbeLayer.LayerValue.PLC;

            packageName = "";
			remarksText = "";
            angleX10 = -1;

			posArray[0] = ptOrigin;
			Point ptRef = new Point(10000, -10000);
			ptRef.Offset(ptOrigin);
			refNum.SetPos(ptRef, 0);
			refNum.SigName = strReference;

			SetContentsWithLinkedList(objLList);
		}


		/// <summary>
		/// LinkedList���g����contentsObj��ݒ肷��
		/// </summary>
		/// <param name="objLList"></param>
		protected void SetContentsWithLinkedList(LinkedList<MbeObj> objLList)
		{
			contentsObj = new List<MbeObj>();
			foreach (MbeObj obj in objLList) {
				if (obj != null && obj.Id() != MbeObjID.MbeComponent) {
					if (obj.IsValid()) {
						contentsObj.Add(obj);
					}
				}
			}
		}

		/// <summary>
		/// �ڑ��`�F�b�N�̃A�N�e�B�u��Ԃ̃N���A
		/// </summary>
		public override void ClearConnectCheck()
		{
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					obj.ClearConnectCheck();
				}
			}
		}

		/// <summary>
		/// �}�ʃI�u�W�F�N�g���Ƃ�ID�l��Ԃ�
		/// </summary>
		public override MbeObjID Id()
		{
			return MbeObjID.MbeComponent;
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
				MbeLayer.LayerValue _layer;
				if (MbeLayer.IsComponentSide(value)) {
					_layer = MbeLayer.LayerValue.CMP;
				} else {
					_layer = MbeLayer.LayerValue.SOL;
				}

				if((layer == MbeLayer.LayerValue.CMP || layer == MbeLayer.LayerValue.SOL)&&(layer != _layer)){
				//if(layer != _layer){
					Flip(posArray[0].X);
				}else{
					layer = _layer;
					return;
				}

			
				//if (refNum == null) {
				//    layer = _layer;
				//    return;
				//}

				//if (refNum.Layer == MbeLayer.LayerValue.NUL) {
				//    refNum.Layer = (_layer == MbeLayer.LayerValue.CMP ? MbeLayer.LayerValue.PLC : MbeLayer.LayerValue.PLS);
				//    layer = _layer;
				//    return;
				//}

				//if(layer == MbeLayer.LayerValue.NUL) {
				//    layer = _layer;
				//    return;
				//}


				//if (layer != _layer) {
				//    Flip(posArray[0].X);
				//}
			}
		}

		/// <summary>
		/// �z�u�w��\�ȃ��C���[
		/// </summary>
		/// <returns></returns>
		public static ulong SelectableLayer()
		{
			return
					(ulong)MbeLayer.LayerValue.CMP |
					(ulong)MbeLayer.LayerValue.SOL;
		}

		public override MbePointInfo[] GetSnapPointArray()
		{
			int nPoint = 1;
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					if (obj.Id() == MbeObjID.MbePTH ||
					   obj.Id() == MbeObjID.MbePinSMD ||
					   obj.Id() == MbeObjID.MbeHole) nPoint++;
				}
			}
			MbePointInfo[] infoArray = new MbePointInfo[nPoint];
			MbePointInfo sp;
			sp.layer = (ulong)refNum.Layer;
			sp.point = GetPos(0);
			sp.selectFlag = false;
            sp.componentPinObj = null;
			infoArray[0] = sp;
			int index = 1;
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					if (obj.Id() == MbeObjID.MbePTH ||
					   obj.Id() == MbeObjID.MbePinSMD ||
					   obj.Id() == MbeObjID.MbeHole) {
						sp.layer = obj.SnapLayer();
						sp.point = obj.GetPos(0);
                        if (obj.Id() == MbeObjID.MbePTH || obj.Id() == MbeObjID.MbePinSMD) {
                            sp.componentPinObj = obj; //RefNumText + "." + ((MbeObjPin)obj).PinNum;
                        } else {
                            sp.componentPinObj = null;
                        }
						sp.selectFlag = false;
						infoArray[index] = sp;
						index++;
					}
				}
			}

			return infoArray;
		}


        public Point RefnumPos()
        { 
            return refNum.GetPos(0);
        }


		public override MbePointInfo[] GetPosInfoArray()
		{
			int nPoint = 2;
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					if (obj.Id() == MbeObjID.MbePTH ||
					   obj.Id() == MbeObjID.MbePinSMD ||
					   obj.Id() == MbeObjID.MbeHole) nPoint++;
				}
			}
			MbePointInfo[] infoArray = new MbePointInfo[nPoint];

			MbePointInfo sp;
			sp.layer = (ulong)layer;
			sp.point = GetPos(0);
			sp.selectFlag = selectFlag[0];
            sp.componentPinObj = null;
			infoArray[0] = sp;

			sp.layer = (ulong)refNum.Layer;
			sp.point = refNum.GetPos(0);
			sp.selectFlag = refNum.GetSelectFlag(0);
			infoArray[1] = sp;

			int index = 2;
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					if (obj.Id() == MbeObjID.MbePTH ||
					   obj.Id() == MbeObjID.MbePinSMD ||
					   obj.Id() == MbeObjID.MbeHole) {
						//sp.layer = obj.SnapLayer();
                        sp.layer = (ulong)obj.Layer;
						sp.point = obj.GetPos(0);
                        if (obj.Id() == MbeObjID.MbePTH || obj.Id() == MbeObjID.MbePinSMD) {
                            sp.componentPinObj = obj;// RefNumText + "." + ((MbeObjPin)obj).PinNum;
                        } else {
                            sp.componentPinObj = null;
                        }
                        sp.selectFlag = selectFlag[0];
						infoArray[index] = sp;
						index++;
					}
				}
			}

			return infoArray;
		}

		/// <summary>
		/// �ړ�
		/// </summary>
		/// <param name="selectedOnly">�I���t���O�������Ă�����̂����ړ�����ꍇ��true</param>
		/// <param name="offset">�ړ���</param>
		public override void Move(bool selectedOnly, Point offset, Point ptAbs, bool moveSingle)
		{
			if (selectedOnly) {
				if (selectFlag[0] == false){
					if (refNum.GetSelectFlag(0) == true) {//���t�@�����X�����񂾂��̈ړ�
						refNum.Move(false, offset,ptAbs,false);
					}
					return;
				}
			}
			posArray[0].Offset(offset);
			refNum.Move(false, offset,ptAbs,false);
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					obj.Move(false, offset,ptAbs,false);
				}
			}
		}

		/// <summary>
		/// ��]
		/// </summary>
		/// <param name="selectedOnly"></param>
		/// <param name="ptCenter"></param>
		public override void Rotate90(bool selectedOnly, Point ptCenter)
		{
			if (selectedOnly) {
				if (selectFlag[0] == false) {
					if (refNum.GetSelectFlag(0) == true) {//���t�@�����X�����񂾂��̉�]
						refNum.Rotate90(false, ptCenter);
					}
					return;
				}
			}
			base.Rotate90(false, ptCenter);
			refNum.Rotate90(false, ptCenter);
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					obj.Rotate90(false, ptCenter);
				}
			}
            if (angleX10 >= 0) {
                if (layer == MbeLayer.LayerValue.CMP) {
                    angleX10 += 900;
                } else {
                    angleX10 += 2700;
                }
                angleX10 = angleX10 % 3600;
            }
		}


		public override void Flip(int hCenter)
		{
			int x = hCenter - (posArray[0].X - hCenter);
			int y = posArray[0].Y;
			posArray[0] = new Point(x, y);

			refNum.Flip(hCenter);
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					obj.Flip(hCenter);
				}
			}
			layer = MbeLayer.Flip(layer);
		}

		/// <summary>
		/// �S�Ă̑I���t���O�̃Z�b�g
		/// </summary>
		public override void SetAllSelectFlag()
		{
			selectFlag[0] = true;
			refNum.SetAllSelectFlag();
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					obj.SetAllSelectFlag();
				}
			}
		}

		/// <summary>
		/// �S�Ă̑I���t���O�̃N���A
		/// </summary>
		public override void ClearAllSelectFlag()
		{
			selectFlag[0] = false;
			refNum.ClearAllSelectFlag();
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					obj.ClearAllSelectFlag();
				}
			}
		}

		/// <summary>
		/// �I������Ă��邩�ǂ����̔���
		/// </summary>
		/// <returns>�I������Ă���Ƃ�true��Ԃ�</returns>
		public override bool IsSelected()
		{
			if (selectFlag[0]) return true;
			return refNum.IsSelected();
		}


		/// <summary>
		/// �L�����ǂ�����Ԃ�
		/// </summary>
		/// <returns></returns>
		public override bool IsValid()
		{
			if (refNum.Layer == MbeLayer.LayerValue.NUL) return false;
			if (contentsObj == null) return false;
			if (contentsObj.Count == 0) return false;
			return true;
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
			string _str1;
			string _str2;
			ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;
			switch (str1) {
				case "PACKAGE":
					packageName = ReadCE3.DecodeCE3String(str2);
					return ReadCE3.RdStatus.NoError;
                case "REMARKS":
                    remarksText = ReadCE3.DecodeCE3String(str2);
                    return ReadCE3.RdStatus.NoError;
                case "ANGLEX10":
                    try { AngleX10 = Convert.ToInt32(str2); }
                    catch (Exception) {
                        AngleX10 = -1; 
                    }
                    return ReadCE3.RdStatus.NoError;
                case "+REFNUM": {
						while (readCE3.GetRecord(out _str1, out _str2)) {
							if (_str1[0] == '-') {
								break;
							} else {
								if (_str1 == "") continue;
								MbeObj obj;
								result = MbeObjIO.ReadMbeObj(readCE3, _str1, out obj);
								if (obj != null && obj.Id() == MbeObjID.MbeText) {
									refNum = (MbeObjText)obj;
								}
							}
						}
						return result;
					}
				case "+CONTENTS": {
						LinkedList<MbeObj> objLList = new LinkedList<MbeObj>();
						while (readCE3.GetRecord(out _str1, out _str2)) {
							if (_str1[0] == '-') {
								break;
							} else {
								if (_str1 == "") continue;
								MbeObj obj;
								result = MbeObjIO.ReadMbeObj(readCE3, _str1, out obj);
								if (obj != null) {
									objLList.AddLast(obj);
								}
							}
						}
						int nCount = objLList.Count;
						if (nCount == 0) {
							contentsObj = null;
							return result;
						}
						SetContentsWithLinkedList(objLList);
						return result;
					}
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
            AngleX10 = -1;
			while (readCE3.GetRecord(out str1, out str2)) {
				if (str1[0] == '-') {
					if (str1 != "-MBE_COMPONENT") {
						return ReadCE3.RdStatus.FormatError;
					} else {
						//layer = refNum.Layer;


                        //foreach (MbeObj obj in contentsObj) {
                        //    if (obj.Id() == MbeObjID.MbeLine) {
                        //        if (obj.Layer == MbeLayer.LayerValue.PLC || obj.Layer == MbeLayer.LayerValue.PLS) {
                        //            ((MbeObjLine)obj).LineWidth = 2000;
                        //        }
                        //    }
                        //    if (obj.Id() == MbeObjID.MbeArc) {
                        //        if (obj.Layer == MbeLayer.LayerValue.PLC || obj.Layer == MbeLayer.LayerValue.PLS) {
                        //            ((MbeObjArc)obj).LineWidth = 2000;
                        //        }
                        //    }

                        //}




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

        public override string ToString()
        {
            if (signame.Length == 0) {
                return "NoName";
            } else {
                return SigName;
            }
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
			writeCE3.WriteNewLine();
            writeCE3.WriteRecordString("PACKAGE", packageName);
            writeCE3.WriteNewLine();
            writeCE3.WriteRecordString("REMARKS", remarksText);
			writeCE3.WriteNewLine();
            writeCE3.WriteRecordInt("ANGLEX10", AngleX10);
            writeCE3.WriteNewLine();
            writeCE3.WriteRecord("+REFNUM");
			writeCE3.WriteNewLine();
			refNum.WrMb3(writeCE3, origin);
			writeCE3.WriteRecord("-REFNUM");
			writeCE3.WriteNewLine();
			writeCE3.WriteRecord("+CONTENTS");
			writeCE3.WriteNewLine();
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					obj.WrMb3(writeCE3,origin);
				}
			}
			writeCE3.WriteRecord("-CONTENTS");
			writeCE3.WriteNewLine();
			return true;
		}

		/// <summary>
		/// Mb3�t�@�C���ւ̏�������
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public override bool WrMb3(WriteCE3 writeCE3, Point origin)
		{
			writeCE3.WriteRecord("+MBE_COMPONENT");
			WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecord("-MBE_COMPONENT");
			writeCE3.WriteNewLine();
			return true;
		}

		/// <summary>
		/// �������s��
		/// </summary>
		/// <returns></returns>
		public override MbeObj Duplicate()
		{
			MbeObj newObj = new MbeObjComponent(this);
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
			//if ((layerMask & (ulong)layer) == 0) return false;
			if (DeleteCount >= 0) return false;

            // Test selection of the REFERENCE-NUMBER.
            //21 May 2011. Version 0.51.01
            // Moved priority to top.
			if(refNum.SelectIt(rc,layerMask,false)){
				if(pointMode) return true;
			}

            // Test selection by the ORIGIN-POINT-of-COMPONENT.
            if ((layerMask & (ulong)layer) != 0 && rc.Contains(posArray[0])) {
                SetAllSelectFlag();
                return true;
            }

            // Test selection if it contains the MbeObjects(PTH, PAD, LINE and etc).
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					if (obj.Id() == MbeObjID.MbePTH ||
					   obj.Id() == MbeObjID.MbePinSMD ||
					   obj.Id() == MbeObjID.MbeHole) {
						if(obj.SelectIt(rc,layerMask,pointMode)){
							SetAllSelectFlag();
							return true;
						}
					}
				}
			}
			
			return false;

			//return refNum.SelectIt(rc,layerMask,false);
		}

		/// <summary>
		/// ConChk�̂��߂̎�̎擾
		/// </summary>
		/// <param name="rc"></param>
		/// <param name="layerMask"></param>
		/// <returns></returns>
		public override MbeObj ConChkSeed(MbeRect rc, ulong layerMask)
		{
			if (DeleteCount >= 0) return null;


			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					if (obj.Id() == MbeObjID.MbePTH ||
					   obj.Id() == MbeObjID.MbePinSMD ||
					   obj.Id() == MbeObjID.MbeLine) {
						if (obj.SelectIt(rc, layerMask, true)) {
							obj.ClearAllSelectFlag();
							return obj;
						}
					}
				}
			}
			
			return null;
		}


		/// <summary>
		/// �`��(abstract ���\�b�h)
		/// </summary>
		/// <param name="dp"></param>
		public override void Draw(DrawParam dp)
		{
			refNum.Draw(dp);
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					if (obj.Id() == MbeObjID.MbePTH ||
					   obj.Id() == MbeObjID.MbePinSMD ||
					   obj.Id() == MbeObjID.MbeHole) {
						obj.Draw(dp);
					} else {
                        bool restoreDrawSnapMark = drawSnapMark;
						drawSnapMark = false;
						obj.Draw(dp);
                        drawSnapMark = restoreDrawSnapMark;
					}
				}
			}
			if (dp.mode != MbeDrawMode.Print && dp.layer == layer) {
				Point pt0 = this.GetPos(0);
				pt0 = ToDrawDim(pt0, dp.scale);
				int marksize = (int)(10000 / dp.scale);
				if (marksize < 5) marksize = 5;
				DrawSnapPointMark(dp.g, pt0, marksize, selectFlag[0]);
			}
		}

		/// <summary>
		/// CAM�f�[�^�̐��� 
		/// </summary>
		/// <param name="camOut"></param>
		public override void GenerateCamData(CamOut camOut)
		{
			refNum.GenerateCamData(camOut);
			if (contentsObj != null) {
				foreach (MbeObj obj in contentsObj) {
					obj.GenerateCamData(camOut);
				}
			}
		}

        /// <summary>
        /// �`��͈͂𓾂�
        /// </summary>
        /// <returns></returns>
        /// <remarks>�s���S�� 2008/08/15</remarks>
        public override MbeRect OccupationRect()
        {
            MbeRect rc = new MbeRect();
            rc = new MbeRect(GetPos(0), GetPos(0));

            foreach (MbeObj obj in contentsObj) {
                rc.Or(obj.OccupationRect());
            }

            rc.Or(refNum.OccupationRect());

            return rc;

            //int wm = (rc.Width+40000) / 2;
            //int hm = (rc.Height+40000) / 2;

            //return new MbeRect(new Point(rc.L - wm, rc.T + hm), new Point(rc.R + wm, rc.B - hm));

        }

        public void parseRefnum(out string prefix, out int nsuffix) 
        {
            string suffix;
            string strRefNum = RefNumText;
            int i;
            for (i = 0; i < strRefNum.Length; i++) {
                if (Char.IsDigit(strRefNum[i])) break;
            }
            prefix = strRefNum.Substring(0, i);
            suffix = strRefNum.Substring(i);
            Int32.TryParse(suffix, out nsuffix);
        }


		protected List<MbeObj> contentsObj;
		protected MbeObjText refNum;
        protected string packageName;
		protected string remarksText;
        protected int angleX10;
	}



    public class ComponentRefComparer : IComparer<MbeObjComponent>
    {
        public int Compare(MbeObjComponent comp1, MbeObjComponent comp2)
        {
            string prefix1;
            int suffix1;
            string prefix2;
            int suffix2;

            comp1.parseRefnum(out prefix1, out suffix1);
            comp2.parseRefnum(out prefix2, out suffix2);

            int strCmpVal = string.Compare(prefix1, prefix2);
            if(strCmpVal == 0){
                return suffix1 - suffix2;
            }
            return strCmpVal;
        }

    }

  
}
