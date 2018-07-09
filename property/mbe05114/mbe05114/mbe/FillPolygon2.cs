using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace mbe
{
	class FillPolygon2
	{
		protected const int LINE_STATUS_FILL  = 0;
		protected const int LINE_STATUS_FRAME = 1;
		protected const int LINE_DELETED = -1;

        protected const int PROCESS_UNIT_SIZE = 500;

        protected const int MAX_DIVISION_HEIGHT = 200000;
        protected const int MIN_DIVISION_HEIGHT = 50000;

        protected const int MIN_DIVISION_WIDTH = 200000;


		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public FillPolygon2()
		{
			orgFrameLineList = new LinkedList<MbeGapChkObjLine>();
			frameLineList = new LinkedList<MbeGapChkObjLine>();
            fillLineListArray = null;   //new LinkedList<MbeGapChkObjLine>();
            outlineListArray = null;    // new LinkedList<MbeGapChkObjLine>();
            thermalJunctionList = new LinkedList<MbeGapChkObjLine>();
			keepOutPatterns = new LinkedList<MbeGapChkObj>();
			connectPatterns = new LinkedList<MbeGapChkObj>();
		}

        public bool FullFill(MbeObjPolygon polygon, LinkedList<MbeGapChkObj> objList)
        {
            int posCouont = polygon.PosCount;
            layer = polygon.Layer;
            traceWidth = polygon.TraceWidth;
            patternGap = polygon.PatternGap;
            thermalGap = polygon.ThermalGap;

            bool restrictPolygon = false;
            if (polygon.RestrictMask) {
                restrictPolygon = true;
                traceWidth = MbeObjPolygon.MIN_TRACE_WIDTH;
            }


            orgFrameLineList.Clear();
            frameLineList.Clear();
            //fillLineList.Clear();
            //outlineList.Clear();

            if (!SetFrame(polygon)) return false;
            if (!PlaceFillLines(1, Int32.MaxValue)) return false;

            //return true;

            foreach (MbeGapChkObjLine obj in frameLineList) {
                if (restrictPolygon) {
                    obj.lineWidth = 0;
                }
                objList.AddLast(obj);
            }
            foreach (MbeGapChkObjLine obj in fillLineListArray[0]) {
                if (restrictPolygon) {
                    obj.lineWidth = 0;
                }
                objList.AddLast(obj);
            }
            return true;
        }



		/// <summary>
		/// �h��Ԃ��̃A�b�v�f�[�g���s��
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="objList"></param>
		/// <returns></returns>
		public bool UpdateFill(MbeObjPolygon polygon, LinkedList<MbeObj> mainList)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            long time_ms;

            int posCouont = polygon.PosCount;
            layer = polygon.Layer;
            traceWidth = polygon.TraceWidth;
            patternGap = polygon.PatternGap;
            thermalGap = polygon.ThermalGap;
            removeFloating = polygon.RemoveFloating;

            polygon.fillLineList.Clear();   //Version 0.49.00 2010/1/1 ���̕�����ړ�

            orgFrameLineList.Clear();
            frameLineList.Clear();
            thermalJunctionList.Clear();
            //fillLineList.Clear();
            //outlineList.Clear();

            if (polygon.RestrictMask) return true;  //Version 0.49.00 2010/1/1 


            //-----------------------------------------

            sw.Start();
            if (!SetFrame(polygon)) return false;
            SetupWorkList(mainList);
            SetupNetInfo(polygon.GetPos(0), polygon.Layer);
            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Fill polygon Step1: " + time_ms);
            sw.Reset();
            


            //-----------------------------------------
            sw.Start();
            PlaceOutlines();
            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Fill polygon Step2: " + time_ms);
            sw.Reset();
            //-----------------------------------------
            sw.Start();

            PlaceFillLines(verticalDivision, verticalDivisionHeight);


            for (int i = 0; i < verticalDivision; i++) {
                TrimFillLines(fillLineListArray[i], outlineListArray[i], keepOutPatternsPerAreaArray[i]);
            }



            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Fill polygon Step3: " + time_ms);
            sw.Reset();
            //-----------------------------------------
            sw.Start();

            LinkedList<MbeGapChkObjLine> tempLineList = new LinkedList<MbeGapChkObjLine>();
            //for(int i=0;i<verticalDivision;i++){
            //    foreach (MbeGapChkObjLine obj in outlineListArray[i]) {
            //        tempLineList.AddLast(obj);
            //    }
            //    foreach (MbeGapChkObjLine obj in fillLineListArray[i]) {
            //        tempLineList.AddLast(obj);
            //    }
            //}

            //foreach (MbeGapChkObjLine obj in thermalJunctionList) {
            //    tempLineList.AddLast(obj);
            //}

            //outlineList.Clear();
            if (removeFloating) {
                //for (int i = 0; i < verticalDivision; i++) {
                //    foreach (MbeGapChkObjLine obj in outlineListArray[i]) {
                //        tempLineList.AddLast(obj);
                //    }
                //    foreach (MbeGapChkObjLine obj in fillLineListArray[i]) {
                //        tempLineList.AddLast(obj);
                //    }
                //}

                //foreach (MbeGapChkObjLine obj in thermalJunctionList) {
                //    tempLineList.AddLast(obj);
                //}

                RemoveFloating(tempLineList);
            } else {
                for (int i = 0; i < verticalDivision; i++) {
                    foreach (MbeGapChkObjLine obj in outlineListArray[i]) {
                        tempLineList.AddLast(obj);
                    }
                    foreach (MbeGapChkObjLine obj in fillLineListArray[i]) {
                        tempLineList.AddLast(obj);
                    }
                }

                foreach (MbeGapChkObjLine obj in thermalJunctionList) {
                    tempLineList.AddLast(obj);
                }

            }

            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Fill polygon Step4: " + time_ms);
            sw.Reset();

            ClearConnectCheck();//�����Y���Ɛڑ��l�b�g�̕`�悪�n�C���C�g�ɂȂ�B

            ClearWorkFlag();

            //			polygon.fillLineList.Clear();

            foreach (MbeGapChkObjLine obj in tempLineList) {
                if (obj.netNum != -1 || !removeFloating) {
                    polygon.fillLineList.AddLast(obj);
                }
            }

            //foreach (MbeGapChkObjLine obj in polygon.fillLineList) {
            //    Point pt0 = obj.p0;
            //    Point pt1 = obj.p1;
            //    System.Diagnostics.Debug.WriteLine("Fill  " + pt0.X + "," + pt0.Y + "," + pt1.X + "," + pt1.Y);
            //}


            return true;
        }


		/// <summary>
		/// ���p�`�̘g���̐���
		/// </summary>
		/// <param name="polygon"></param>
		/// <returns></returns>
		protected bool SetFrame(MbeObjPolygon polygon)
		{
			if (!polygon.IsValid()) {
				return false;
			}

			//frameLineList.Clear();

			int posCouont = polygon.PosCount;
			//layer = polygon.Layer;
			//traceWidth = polygon.TraceWidth;

			ptConnect = polygon.GetPos(0);
			for (int i = 1; i < posCouont; i++) {
				int index2 = i + 1;
				if (index2 >= posCouont) {
					index2 = 1;
				}
				Point pt0 = polygon.GetPos(i);
				Point pt1 = polygon.GetPos(index2);
				if (i == 1) {
					rcArea = new MbeRect(pt0, pt1);
				} else if(index2>1){
					rcArea.Or(pt1);
				}
				MbeGapChkObjLine objLine;
				objLine = new MbeGapChkObjLine();
				objLine.status = LINE_STATUS_FRAME;
				objLine.layer = layer;
				objLine.SetLineValue(pt0, pt1, traceWidth);
				orgFrameLineList.AddLast(objLine);
				objLine = new MbeGapChkObjLine();
				objLine.status = LINE_STATUS_FRAME;
				objLine.layer = layer;
				objLine.SetLineValue(pt0, pt1, traceWidth);
				frameLineList.AddLast(objLine);
			}
			return true;
		}

		/// <summary>
		/// �h��Ԃ��̉����̔z�u
		/// </summary>
		/// <returns></returns>
        /// <remarks>
        /// �|���S���̘g�����𐅕����C���Ŗ��߂�
        /// </remarks>
		protected bool PlaceFillLines(int divisionCount,int divisionHeight)
		{
			//fillLineList.Clear();
            if (divisionCount <= 0 || divisionHeight<=10000) return false;

			int count = orgFrameLineList.Count;
			int[] xpoint = new int[count];
			IComparer comparer = new xpointcomp();

            fillLineListArray = new LinkedList<MbeGapChkObjLine>[divisionCount];
            for (int i = 0; i < divisionCount; i++) {
                fillLineListArray[i] = new LinkedList<MbeGapChkObjLine>();
            }
            int listIndex = 0;
            int areaBottom = rcArea.T - divisionHeight;

			//int step = traceWidth - 1500;
            int step = traceWidth *2/3;

			double y = (double)rcArea.T-step - 0.5;//�|���S���g���_�ƌv�Z���Ɉ�v����ʓ|������邽�߂ɁA�����l�ŃI�t�Z�b�g

            while (y > rcArea.B) {
                int xpIndex = 0;
                foreach (MbeGapChkObjLine obj in orgFrameLineList) {
                    double refX;
                    if (Util.LineCrossingY(obj.p0, obj.p1, y, out refX)) {
                        xpoint[xpIndex] = (int)Math.Round(refX);
                        xpIndex++;
                        if (xpIndex == count) break;//�����break�͂��肦�Ȃ��͂�
                    }
                }
                Array.Sort(xpoint, 0, xpIndex, comparer);

                int nLine = xpIndex / 2;
                int j = 0;
                int ny = (int)Math.Round(y);
                for (int i = 0; i < nLine; i++) {
                    Point pt0 = new Point(xpoint[j], ny);
                    Point pt1 = new Point(xpoint[j + 1], ny);
                    MbeGapChkObjLine objLine = new MbeGapChkObjLine();
                    objLine.layer = layer;
                    objLine.SetLineValue(pt0, pt1, traceWidth);
                    j += 2;
                    fillLineListArray[listIndex].AddLast(objLine);
                }
                y = y - step;
                if (y < areaBottom) {
                    areaBottom -= divisionHeight;
                    listIndex++;
                    if (listIndex >= divisionCount) break;
                }
            }

			return true;
		}

		/// <summary>
		/// �M���p�^�[���̎������̔z�u
		/// </summary>
        /// <remarks>
        /// �����h�A�p�b�h�̊O�`�����\�z���AtempList2�ɕۑ�����B
        ///    �Ώۂ͔�ڑ��̃����h�A�p�b�h�ƁA���i�̃����h�A�p�b�h(�T�[�}���̌��Ԃ��m�ۂ��邽�߁B
        /// tempList2���痼�[�_���A�|���S���ŊO���̎l�p�`�̊O�ɂ�����̂���菜���A����ȊO��tempList1�Ɉڂ��B
        /// tempList2���N���A����B
        /// ����ꂽ�O�`���̘A���d�������������������œK������BQFP�p�b�h�ɑ΂��Č��ʂ�����B
        /// ��_�Őؒf����B
        /// KeeepOut�I�u�W�F�N�g�Ƀ^�b�`������̂���菜���A����ȊO��tempList2�Ɉڂ��B
        /// tempList1���N���A����B
        /// 
        /// �����h�p�b�h�ȊO�̊O�`�����\�z���AtempList1�ɕۑ�����B
        /// ---- �����܂ł�Step 2-1
        /// 
        /// tempList1���痼�[�_���A�|���S���ŊO���̎l�p�`�̊O�ɂ�����̂���菜���A����ȊO��tempList2�ɒǉ�����B
        ///      tempList2�ɂ́A�����h�A�p�b�h�Ƃ���ȊO�̂��̂̊O�`�����ۑ�����Ă���
        /// ����ꂽ�O�`���̘A���d�������������������œK������B
        /// �������ƕ������������肷��B
        /// outlineListArray�̊m�ۂ��s���B
        /// �T�[�}�������ڑ����C��(�T�[�}���p�b�h�̏\����)�𐶐�����thermalJunctionList�Ɋi�[����B
        /// 
        /// ---- �����܂ł�Step 2-2
        /// �|���S���g����tempList2�̊O�`���ɒǉ�����B
        /// �O�`�����G���A��������
        /// �G���A���ƂɊO�`����_�Őؒf���A�ڐG�p�^�[�����폜����
        /// 
        /// ---- �����܂ł�Step 2-3
        /// �T�[�}�������ڑ����C�����O�`���Őؒf���āA�ڐG�p�^�[�����폜����
        /// 
        /// ---- �����܂ł�Step 2-4
        /// 
        /// �В[�_���A�|���S���O�̂��̂���菜���B
        /// ---- �����܂ł�Step 2-5
        /// </remarks>
		protected void PlaceOutlines()
        {
            LinkedList<MbeGapChkObjLine> tempList1 = new LinkedList<MbeGapChkObjLine>();
            LinkedList<MbeGapChkObjLine> tempList2 = new LinkedList<MbeGapChkObjLine>();
            LinkedList<MbeGapChkObjLine> tempList3 = new LinkedList<MbeGapChkObjLine>();

            LinkedList<MbeObjPin> pinList = new LinkedList<MbeObjPin>();//�A�E�g���C���𐶐�����s���̃��X�g

            GenOutlineParam outlineParam;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            long time_ms;

            outlineParam.rc = rcArea;
            outlineParam.layer = layer;
            outlineParam.traceWidth = traceWidth;
            outlineParam.option = 0;
            //--------------------------------------------------
            sw.Start();

            //2012/04/22 ���w�M���b�v�̖���FIX�̂��߂̃p�b�`
            foreach (MbeObj obj in workList) {
                if ((layer == MbeLayer.LayerValue.L2 || layer == MbeLayer.LayerValue.L3) && (obj.Layer == layer || obj.Layer == MbeLayer.LayerValue.PTH)) {
                    if (obj.Id() == MbeObjID.MbePTH) {
                        if (obj.ConnectionCheckActive && obj.TempPropString.Length > 0) {
                            Point pc = obj.GetPos(0);
                            bool onFrame;//dummy
                            if (!PointIsInside(pc, 0, out onFrame)) {
                                continue;
                            }
                            ((MbeObjPTH)obj).innerLayerConnectionInfo |= (ulong)layer;
                        }
                    }
                }
            }
            //2012/04/22 ���w�M���b�v�̖���FIX�̂��߂̃p�b�` ***�����܂�***

            //�ŏ��Ƀ����h�A�p�b�h�����̊O�`�����\�z����B
            foreach (MbeObj obj in workList) {
                if (obj.Id() == MbeObjID.MbePTH ||
                    obj.Id() == MbeObjID.MbePinSMD) {
                    if (obj.Id() == MbeObjID.MbePinSMD && obj.Layer != layer) {
                        continue;
                    }
                    if (!obj.ConnectionCheckActive || obj.TempPropString.Length > 0) {
                        int netnum;
                        if (!obj.ConnectionCheckActive) {
                            outlineParam.gap = patternGap;
                            netnum = -1;
                        } else {
                            outlineParam.gap = thermalGap;
                            netnum = 0;
                        }
                        int c1 = tempList2.Count;
                        obj.GenerateOutlineData(tempList2, outlineParam);
                        if (c1 != tempList2.Count) {
                            obj.GenerateGapChkData(keepOutPatterns, netnum);
                            pinList.AddLast((MbeObjPin)obj);//�A�E�g���C���𐶐�����s���̃��X�g�ɓo�^
                        }
                    } else {
                        obj.GenerateGapChkData(connectPatterns, 0);
                    }
                }
            }

            //�l�p�g�㉺���E�O�̂��̂���菜��
            foreach (MbeGapChkObjLine obj in tempList2) {
                if (Util.LineIsOutsideLTRB(obj.p0, obj.p1, rcArea)) {
                    continue;
                }
                tempList1.AddLast(obj);
            }

            tempList2.Clear();
            
            optimizeVHLine(tempList1);
            
            ////��_�Őؒf����
            //DoDivideLineAtCrossing(tempList1);
            //RemovePtnTouchKeeepOut(tempList1, tempList2,keepOutPatterns);
            //tempList1.Clear();

            tempList2.Clear();
            foreach (MbeGapChkObjLine line in tempList1) {
                tempList2.AddLast(line);
            }


            tempList1.Clear();  //2008/01/03


            //�����܂łŃ����h�A�p�b�h�����̊O�`����tempList2�ɕۑ������

            //�����h�p�b�h�ȊO�̊O�`����ǉ�����
            foreach (MbeObj obj in workList) {
                //�����h�p�b�h�͏����ς݂Ȃ̂�continue�Ŕ�΂�
                if (obj.Id() == MbeObjID.MbePTH ||
                    obj.Id() == MbeObjID.MbePinSMD) {
                    continue;
                }


                //outlineParam.option = 0;
                if (!obj.ConnectionCheckActive) {

                    //�������h��p�b�h�ɖ��܂������C���A�E�g���C���̒[�_�L���b�v�͏Ȃ��邩�ƍl����
                    //  �M���b�v�g���[�X�ݒ肪�������Ƃ��͒[�_�̃����h��p�b�h�̌����Ɏ��Ԃ��������āA
                    //  ���܂蓾�ɂȂ�Ȃ��B
                    //
                    ////���C���f�[�^�̂Ƃ��͒[�_�Ƀ����h�p�b�h���Ȃ����`�F�b�N����B
                    ////�[�_�Ƀ����h�p�b�h�������āA���ꂪ�������傫�����̂ł���΁A�O�`���̃G���h�L���b�v�͗v��Ȃ�
                    outlineParam.option = 0;

                    // 2009/01/03 �[�_�L���b�v�ȗ����� MbeObjLine.GenerateOutlineData()�̃o�O�C��  >>>>2009/01/01 ���C���A�E�g���C���̒[�_�L���b�v�ȗ����~�߂�B��ɒ[�_�L���b�v����
                    if (obj.Id() == MbeObjID.MbeLine && obj.Layer == layer) {
                        int linewidth = ((MbeObjLine)obj).LineWidth;
                        Point leP0 = ((MbeObjLine)obj).GetPos(0);
                        Point leP1 = ((MbeObjLine)obj).GetPos(1);

                        foreach (MbeObjPin objPin in pinList) {
                            Point padPt = objPin.GetPos(0);
                            if ((outlineParam.option & GenOutlineParam.P0_NO_LINECAP)==0 &&  padPt.Equals(leP0)) {
                                Size padSize = objPin.PadSize;
                                if (padSize.Width >= linewidth && padSize.Height >= linewidth) {
                                    outlineParam.option |= GenOutlineParam.P0_NO_LINECAP;
                                }
                            } else if ((outlineParam.option & GenOutlineParam.P1_NO_LINECAP) == 0 && padPt.Equals(leP1)) {
                                Size padSize = objPin.PadSize;
                                if (padSize.Width >= linewidth && padSize.Height >= linewidth) {
                                    outlineParam.option |= GenOutlineParam.P1_NO_LINECAP;
                                }
                            }
                            if (outlineParam.option == (GenOutlineParam.P0_NO_LINECAP | GenOutlineParam.P1_NO_LINECAP)) {
                                break;
                            }
                        }
                    }
                    // 2009/01/03 �[�_�L���b�v�ȗ����� MbeObjLine.GenerateOutlineData()�̃o�O�C�� <<<< 2009/01/01 ���C���A�E�g���C���̒[�_�L���b�v�ȗ����~�߂�B��ɒ[�_�L���b�v����


                    outlineParam.gap = patternGap;
                    int c1 = tempList1.Count;
                    obj.GenerateOutlineData(tempList1, outlineParam);
                    if (c1 != tempList1.Count) {
                        if (obj.Id() == MbeObjID.MbeText) {
                            MbeGapChkObjRect gapChkObj = new MbeGapChkObjRect();
                            gapChkObj.layer = layer;
                            gapChkObj.netNum = -1;
                            gapChkObj.mbeObj = obj;
                            gapChkObj.SetRectValue(((MbeObjText)obj).OccupationRect());
                            keepOutPatterns.AddLast(gapChkObj);
                        } else if (obj.Id() == MbeObjID.MbePolygon) {
                            ((MbeObjPolygon)obj).FullFillLineData(keepOutPatterns);
                        } else if (obj.Id() == MbeObjID.MbeHole) {
                            ((MbeObjHole)obj).KeepOutData(keepOutPatterns, -1);
                        } else {
                            obj.GenerateGapChkData(keepOutPatterns, -1);
                        }
                    }
                } else {
                    obj.GenerateGapChkData(connectPatterns, -1);
                }

            }

            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Fill polygon Step2-1: " + time_ms);
            sw.Reset();
            //--------------------------------------------------
            sw.Start();

            //if (tempList1.Count == 0) return;

            //�l�p�g�㉺���E�O�̂��̂���菜��
            foreach (MbeGapChkObjLine obj in tempList1) {
                if (Util.LineIsOutsideLTRB(obj.p0, obj.p1, rcArea)) {
                    continue;
                }
                tempList2.AddLast(obj);
            }


            //optimizeVHLine(tempList2);

            //�������ƕ��������̌���
            int countBeforeVivideLine = tempList2.Count;
            verticalDivision = countBeforeVivideLine / PROCESS_UNIT_SIZE + 1;
            verticalDivisionHeight = rcArea.Height / verticalDivision + 1;

            if (verticalDivisionHeight > MAX_DIVISION_HEIGHT) {
                verticalDivisionHeight = MAX_DIVISION_HEIGHT;
                verticalDivision = (rcArea.Height + verticalDivisionHeight - 1) / verticalDivisionHeight;
            } else if (verticalDivisionHeight < MIN_DIVISION_HEIGHT) {
                verticalDivisionHeight = MIN_DIVISION_HEIGHT;
                verticalDivision = (rcArea.Height + verticalDivisionHeight - 1) / verticalDivisionHeight;
            }

            //outlineListArray�̊m��
            outlineListArray = new LinkedList<MbeGapChkObjLine>[verticalDivision];
            for (int i = 0; i < verticalDivision; i++) {
                outlineListArray[i] = new LinkedList<MbeGapChkObjLine>();
            }

            //�ڐG�`�F�b�N���s�����߂�keepOutPattern���G���A���Ƃɕ����Ă���
            keepOutPatternsPerAreaArray = new LinkedList<MbeGapChkObj>[verticalDivision];
            for (int i = 0; i < verticalDivision; i++) {
                keepOutPatternsPerAreaArray[i] = new LinkedList<MbeGapChkObj>();

                int divideKeepOutMargin = traceWidth + ((patternGap > thermalGap) ? patternGap : thermalGap) + 5000;
                MbeGapChkObjRect rcKeepOut = new MbeGapChkObjRect();
                rcKeepOut.rc = rcArea;

                rcKeepOut.rc.LT = new Point(rcArea.L,rcArea.T - i * verticalDivisionHeight);
                rcKeepOut.rc.RB = new Point(rcArea.R,rcArea.T - (i+1)*verticalDivisionHeight);
                foreach (MbeGapChkObj obj in keepOutPatterns) {
                    if (rcKeepOut.IsCloseTo(obj, divideKeepOutMargin)) {
                        keepOutPatternsPerAreaArray[i].AddLast(obj);
                    }
                }
            }

            
            CreateThermalJunction();


            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Fill polygon Step2-2: " + time_ms);
            sw.Reset();
            //--------------------------------------------------
            sw.Start();

            //��Ɨp�g����ǉ�����
            foreach (MbeGapChkObjLine obj in frameLineList) {
                tempList2.AddLast(obj);
            }
            frameLineList.Clear();




            //�G���A��������
            for (int i = 0; i < verticalDivision; i++) {
                int y = rcArea.T - (i + 1) * verticalDivisionHeight;
                DoDivideAreaV(tempList2, outlineListArray[i], y);
            }
            tempList2.Clear();


            //�����������ƕ��̐ݒ�
            //�����������ƕ��̓��[�J���ϐ�
            int horizontalDivisionWidth;
            int horizontalDivision;
            horizontalDivisionWidth = (verticalDivisionHeight > MIN_DIVISION_WIDTH ? verticalDivisionHeight : MIN_DIVISION_WIDTH);
            horizontalDivision = (rcArea.Width + horizontalDivisionWidth - 1) / horizontalDivisionWidth;
            //horizontalDivisionWidth = 1000000;// (verticalDivisionHeight > MIN_DIVISION_WIDTH ? verticalDivisionHeight : MIN_DIVISION_WIDTH);
            //horizontalDivision = 1;// (rcArea.Width + horizontalDivisionWidth - 1) / horizontalDivisionWidth;



            //�G���A���Ƃɓ����̊O�`������_�Őؒf���āA�ڐG�p�^�[�����폜����
            for (int i = 0; i < verticalDivision; i++) {
                LinkedList<MbeGapChkObjLine> outputList = new LinkedList<MbeGapChkObjLine>();

                for (int j = 0; j < horizontalDivision; j++) {
                    int divx = rcArea.L + (j + 1) * horizontalDivisionWidth;
                    LinkedList<MbeGapChkObjLine> divhList = new LinkedList<MbeGapChkObjLine>();
                    DoDivideAreaH(outlineListArray[i], divhList, divx);
                    DoDivideLineAtCrossing(divhList);
                    foreach (MbeGapChkObjLine lineObj in divhList) {
                        outputList.AddLast(lineObj);
                    }
                }

                //DoDivideLineAtCrossing(outlineListArray[i]);

                
                LinkedList<MbeGapChkObjLine> tempList = new LinkedList<MbeGapChkObjLine>();
                //RemovePtnTouchKeeepOut(outlineListArray[i], tempList,keepOutPatternsPerAreaArray[i]);
                RemovePtnTouchKeeepOut(outputList, tempList, keepOutPatternsPerAreaArray[i]);
                //RemovePtnTouchKeeepOut(outputList, tempList, keepOutPatterns);
                outlineListArray[i].Clear();
                foreach (MbeGapChkObjLine line in tempList) {
                    outlineListArray[i].AddLast(line);
                }
            }

            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Fill polygon Step2-3: " + time_ms);
            sw.Reset();
            //--------------------------------------------------
            sw.Start();


            //�T�[�}���p�^�[���̏\�������O�`���ŕ���
            //for (int i = 0; i < verticalDivision; i++) {
            //    DoDivideThermalJunction(outlineListArray[i]);
            //}

            //�T�[�}���p�^�[���̐ڐG�p�^�[���̍폜
            {
                LinkedList<MbeGapChkObjLine> tempList = new LinkedList<MbeGapChkObjLine>();
                RemovePtnTouchKeeepOut(thermalJunctionList, tempList,keepOutPatterns);
                thermalJunctionList.Clear();
                foreach (MbeGapChkObjLine line in tempList) {
                    thermalJunctionList.AddLast(line);
                }
            }

            //RemovePtnTouchKeeepOut(tempList2, tempList1);

            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Fill polygon Step2-4: " + time_ms);
            sw.Reset();
            //--------------------------------------------------
            sw.Start();

            //�В[�_���g�O�̂��̂���菜��
            for (int i = 0; i < verticalDivision; i++) {
                RemoveLineOutOfPolygon(outlineListArray[i]);
            }
            RemoveLineOutOfPolygon(thermalJunctionList);


            //thermalJunctionList����В[�_�������폜 Version 0.44.01
            {
                LinkedList<MbeGapChkObjLine> tempList = new LinkedList<MbeGapChkObjLine>();

                //thermalJunction���m���^�b�`���Ă�����̂��c��(tempList�ɃR�s�[���ă��X�g����폜)
                LinkedListNode<MbeGapChkObjLine> nodeDel;
                LinkedListNode<MbeGapChkObjLine> node1 = thermalJunctionList.First;
                while (node1 != null) {
                    MbeGapChkObjLine line1 = node1.Value;
                    MbeGapChkObjLine line2;
                    bool touchFlag = false;
                    int distance = line1.lineWidth;
                    LinkedListNode<MbeGapChkObjLine> node2 = node1.Next;
                    while (node2 != null) {
                        line2 = node2.Value;
                        if (Util.PointIsCloseToLine(line1.p1, line2.p0, line2.p1, distance)) {
                            tempList.AddLast(line2);
                            nodeDel = node2;
                            node2 = node2.Next;
                            thermalJunctionList.Remove(nodeDel);
                            touchFlag = true;
                        } else {
                            node2 = node2.Next;
                        }
                    }
                    if (touchFlag) {
                        tempList.AddLast(line1);
                        nodeDel = node1;
                        node1 = node1.Next;
                        thermalJunctionList.Remove(nodeDel);
                    } else {
                        node1 = node1.Next;
                    }
                }

                //�A�E�g���C���ƃ^�b�`���Ă�����̂��c��(tempList�ɃR�s�[)
                foreach(MbeGapChkObjLine tline in thermalJunctionList){
                    Point pe = tline.p1;
                    int distance = tline.lineWidth;
                    bool touchFlag = false;
                    for (int i = 0; i < verticalDivision; i++) {
                        foreach (MbeGapChkObjLine oline in outlineListArray[i]) {
                            if (Util.PointIsCloseToLine(pe, oline.p0, oline.p1, distance)) {
                                tempList.AddLast(tline);
                                touchFlag = true;
                                break;
                            }
                        }
                        if (touchFlag) {
                            break;
                        }
                    }
                }
                thermalJunctionList.Clear();
                foreach (MbeGapChkObjLine tline in tempList) {
                    thermalJunctionList.AddLast(tline);
                }
            }


            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Fill polygon Step2-5: " + time_ms);
            sw.Reset();
            //--------------------------------------------------

        }

        protected void RemoveLineOutOfPolygon(LinkedList<MbeGapChkObjLine> lineList)
        {
            LinkedList<MbeGapChkObjLine> tempList = new LinkedList<MbeGapChkObjLine>();

            foreach (MbeGapChkObjLine obj in lineList) {
                bool p0onFrame = false;
                bool p1onFrame = false;
                if (!PointIsInside(obj.p0, 600, out p0onFrame) || !PointIsInside(obj.p1, 600, out p1onFrame)) {
                    continue;
                }
                //���[�_���g���Ɣ��肳��Ă��A���[�_���g����ɂ����āA���_���g�O�̂Ƃ��͎�菜��
                if (obj.status != LINE_STATUS_FRAME && p0onFrame && p1onFrame) {
                    Point pc = new Point((obj.p0.X + obj.p1.X) / 2, (obj.p0.Y + obj.p1.Y) / 2);
                    bool pconFrame;
                    if (!PointIsInside(pc, 100, out pconFrame)) {
                        continue;
                    }
                }
                tempList.AddLast(obj);
            }
            lineList.Clear();
            foreach (MbeGapChkObjLine obj in tempList) {
                lineList.AddLast(obj);
            }
        }




        /// <summary>
        /// �T�[�}���p�b�h�̐ڑ����̐���
        /// </summary>
        private void CreateThermalJunction()
        {
            foreach (MbeObj obj in workList) {
                if (obj.Layer == layer || obj.Layer == MbeLayer.LayerValue.PTH) {
                    if (obj.Id() == MbeObjID.MbePTH ||
                        obj.Id() == MbeObjID.MbePinSMD) {
                        if (obj.ConnectionCheckActive && obj.TempPropString.Length > 0) {
                            Point pc = obj.GetPos(0);
                            bool onFrame;//dummy
                            if (!PointIsInside(pc, 0, out onFrame)) {
                                continue;
                            }
                            int padWidth = ((MbeObjPin)obj).PadSize.Width;
                            int padHeight = ((MbeObjPin)obj).PadSize.Height;

                            int hlen = (padWidth + traceWidth) / 2 + thermalGap;
                            int vlen = (padHeight + traceWidth) / 2 + thermalGap;


                            MbeGapChkObjLine thLine;

                            int lineWidthBase = thermalGap * 15 / 10;
                            if (lineWidthBase < 2000) {
                                lineWidthBase = 2000;
                            }



                            int lineWidth;
                            Point pe;
                            pe = pc;
                            pe.X = pc.X - hlen;
                            lineWidth = (lineWidthBase < padHeight ? lineWidthBase : padHeight);
                            if (lineWidth > traceWidth) {
                                lineWidth = traceWidth;
                            }
                            thLine = new MbeGapChkObjLine();
                            thLine.SetLineValue(pc, pe, lineWidth);
                            thLine.netNum = 0;
                            thermalJunctionList.AddLast(thLine);

                            pe.X = pc.X + hlen;
                            thLine = new MbeGapChkObjLine();
                            thLine.SetLineValue(pc, pe, lineWidth);
                            thLine.netNum = 0;
                            thermalJunctionList.AddLast(thLine);

                            pe = pc;
                            pe.Y = pc.Y - vlen;
                            lineWidth = (lineWidthBase < padWidth ? lineWidthBase : padWidth);
                            if (lineWidth > traceWidth) {
                                lineWidth = traceWidth;
                            }
                            thLine = new MbeGapChkObjLine();
                            thLine.SetLineValue(pc, pe, lineWidth);
                            thLine.netNum = 0;
                            thermalJunctionList.AddLast(thLine);

                            pe = pc;
                            pe.Y = pc.Y + vlen;
                            thLine = new MbeGapChkObjLine();
                            thLine.SetLineValue(pc, pe, lineWidth);
                            thLine.netNum = 0;
                            thermalJunctionList.AddLast(thLine);

                            //PTH�̂Ƃ��͓��w�|���S���ڑ��t���O�𗧂Ă�
                            if (obj.Id() == MbeObjID.MbePTH) {
                                ((MbeObjPTH)obj).innerLayerConnectionInfo |= (ulong)layer;
                            }


                        }
                    }
                }
            }
        }

		protected void RemovePtnTouchKeeepOut(LinkedList<MbeGapChkObjLine> srcList, LinkedList<MbeGapChkObjLine> dstList,LinkedList<MbeGapChkObj> koList)
		{
			//�L�[�v�A�E�g�p�^�[���ɋߐڂ��Ă�����̂���菜��
			dstList.Clear();
			foreach (MbeGapChkObjLine obj in srcList) {
				bool removeIt = false;
                foreach (MbeGapChkObj objKo in koList) {
					if (objKo.layer != layer) {
						continue;
					}
					if (objKo.netNum == 0 && obj.netNum == 0) {
						continue;
					}

					int gap;
                    if (objKo.Shape() == MbeGapChkShape.LINE && ((MbeGapChkObjLine)objKo).lineWidth == 0) {
                        gap = MbeObjPolygon.RESTRICT_GAP;
                    } else if (objKo.netNum == -1) {
						gap = patternGap;
					} else {
						gap = thermalGap;
					}
					gap = gap * 95 / 100;
					if (obj.IsCloseTo(objKo, gap)) {
						removeIt = true;
						break;
					}
				}
				if (!removeIt) {
					dstList.AddLast(obj);
				}
			}
		}



		/// <summary>
		/// �t�B�����C���̃g���~���O
		/// </summary>
		protected void TrimFillLines(LinkedList<MbeGapChkObjLine>fillLineList, LinkedList<MbeGapChkObjLine> outlineList,LinkedList<MbeGapChkObj> koList)
		{
			LinkedList<MbeGapChkObjLine> tempList1 = new LinkedList<MbeGapChkObjLine>();
			MbeGapChkObjLine line1 = new MbeGapChkObjLine();

			//outline�Ƃ̌�_�Őؒf����
			foreach (MbeGapChkObjLine line0 in outlineList) {

				line1.p0 = line0.p0;
				line1.p1 = line0.p1;
				if (line1.p0.Y < line1.p1.Y) {
					line1.p0.Y = line1.p0.Y - 200;
					line1.p1.Y = line1.p1.Y + 200;
				} else if (line1.p0.Y > line1.p1.Y) {
					line1.p0.Y = line1.p0.Y + 200;
					line1.p1.Y = line1.p1.Y - 200;
				}



				LinkedListNode<MbeGapChkObjLine> fillNode = fillLineList.First;
				while (fillNode != null) {
					

					MbeGapChkObjLine line2 = fillNode.Value;
					MbeGapChkObjLine nLine1;//dummy
					MbeGapChkObjLine nLine2;


					if (DivideLineAtCrossing(true,100, line1, line2, out nLine1, out nLine2)) {
						if (nLine2 != null) {
							fillLineList.AddLast(nLine2);
						}
					}
					fillNode = fillNode.Next;
				}
			}



			tempList1.Clear();
			foreach (MbeGapChkObjLine obj in fillLineList) {
				bool removeIt = false;
                foreach (MbeGapChkObj objKo in koList){
				//foreach (MbeGapChkObj objKo in keepOutPatterns) {
					if (objKo.layer != layer) {
						continue;
					}
					if (obj.netNum == 0 && objKo.netNum ==0) {
						continue;
					}


					int gap;
                    if (objKo.Shape() == MbeGapChkShape.LINE && ((MbeGapChkObjLine)objKo).lineWidth == 0) {
                        gap = MbeObjPolygon.RESTRICT_GAP;
                    }else if(objKo.netNum == -1) {
						gap = patternGap;
					} else {	
						gap = thermalGap;
					}
					gap = gap * 95 / 100;

                    if (obj.IsCloseTo(objKo, gap)) {
                        removeIt = true;
                        break;
                    }

                    //Point pt;
                    //double dist = obj.Distance(objKo, out pt);
                    //if (dist < gap) {
                    //    removeIt = true;
                    //    break;
                    //}
				}
				if (!removeIt) {
					tempList1.AddLast(obj);
				}
			}
			fillLineList.Clear();
			foreach (MbeGapChkObjLine obj in tempList1) {
				fillLineList.AddLast(obj);
			}
		}

		protected void optimizeVHLine(LinkedList<MbeGapChkObjLine> lineList)
		{
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			int optimizeCount = 0;

			sw.Start();

			LinkedList<MbeGapChkObjLine> tempList = new LinkedList<MbeGapChkObjLine>();

			foreach (MbeGapChkObjLine obj in lineList) {
				tempList.AddLast(obj);
			}
			lineList.Clear();

			bool optimizeFlag = true;
			while (optimizeFlag) {
				optimizeFlag = false;
				LinkedListNode<MbeGapChkObjLine> node1 = tempList.First;
				while (node1 != null) {
					bool horiz;
					MbeGapChkObjLine line1 = node1.Value;

					if (line1.status == LINE_DELETED) {
						node1 = node1.Next;
						continue;
					}

					int from;
					int to;

					if (line1.p0.X == line1.p1.X) {
						horiz = false;
						from = line1.p0.Y;
						to = line1.p1.Y;
					} else if (line1.p0.Y == line1.p1.Y) {
						horiz = true;
						from = line1.p0.X;
						to = line1.p1.X;
					} else {
						node1 = node1.Next;
						continue;
					}
					LinkedListNode<MbeGapChkObjLine> node2 = node1.Next;
					while (node2 != null) {
						MbeGapChkObjLine line2 = node2.Value;

						if (line2.status == LINE_DELETED) {
							node2 = node2.Next;
							continue;
						}
						if (!horiz && line2.p0.X == line2.p1.X && line1.p0.X == line2.p0.X) {
							if (Util.IsOverlap(ref from, ref to, line2.p0.Y, line2.p1.Y)) {
								line2.status = LINE_DELETED;
								optimizeCount++;
								optimizeFlag = true;
							}
						} else if (horiz && line2.p0.Y == line2.p1.Y && line1.p0.Y == line2.p0.Y) {
							if (Util.IsOverlap(ref from, ref to, line2.p0.X, line2.p1.X)) {
								line2.status = LINE_DELETED;
								optimizeCount++;
								optimizeFlag = true;
							}
						}
						node2 = node2.Next;
					}

					if (!horiz) {
						line1.p0.Y = from;
						line1.p1.Y = to;
					} else {
						line1.p0.X = from;
						line1.p1.X = to;
					}

					node1 = node1.Next;
				}
			}

			foreach (MbeGapChkObjLine obj in tempList) {
				if (obj.status != LINE_DELETED) {
					lineList.AddLast(obj);
				}
			}

			sw.Stop();
			long time_ms = sw.ElapsedMilliseconds;
			System.Diagnostics.Debug.WriteLine("optimizeVHLine() takes(ms): " + time_ms);
			System.Diagnostics.Debug.WriteLine("        optimize count: " + optimizeCount);
		}


        protected void RemoveFloating(LinkedList<MbeGapChkObjLine> dstLineList)
        //protected void RemoveFloating()
		{
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            long time_ms;
            sw.Start();


            LinkedList<MbeGapChkObjLine> outlinelist = new LinkedList<MbeGapChkObjLine>();
            LinkedList<MbeGapChkObjLine> filllinelist = new LinkedList<MbeGapChkObjLine>();

            for (int i = 0; i < verticalDivision; i++) {
                foreach (MbeGapChkObjLine obj in outlineListArray[i]) {
                    outlinelist.AddLast(obj);
                }
                foreach (MbeGapChkObjLine obj in fillLineListArray[i]) {
                    filllinelist.AddLast(obj);
                }
            }

            foreach (MbeGapChkObjLine obj in thermalJunctionList) {
                outlinelist.AddLast(obj);
            }


            //�ڑ��p�^�[���ɐڐG���Ă������netNum��0���Z�b�g����B

            foreach (MbeGapChkObjLine obj in outlinelist) {
                if (obj.netNum == 0) {
                    continue;
                }

                foreach (MbeGapChkObj objCon in connectPatterns) {
                    if (objCon.layer == layer || objCon.layer == MbeLayer.LayerValue.PTH) {
                        if (obj.IsCloseTo(objCon, -1000)) {
                            obj.netNum = 0;
                            break;
                        }
                    }
                }
            }

            foreach (MbeGapChkObjLine obj in filllinelist) {
                if (obj.netNum == 0) {
                    continue;
                }

                foreach (MbeGapChkObj objCon in connectPatterns) {
                    if (objCon.layer == layer || objCon.layer == MbeLayer.LayerValue.PTH) {
                        if (obj.IsCloseTo(objCon, -1000)) {
                            obj.netNum = 0;
                            break;
                        }
                    }
                }
            }

            LinkedList<MbeGapChkObjLine> linelist = new LinkedList<MbeGapChkObjLine>();
            foreach (MbeGapChkObjLine obj in outlinelist) {
                linelist.AddLast(obj);
            }
            foreach (MbeGapChkObjLine obj in filllinelist) {
                linelist.AddLast(obj);
            }

            int filllineCount = filllinelist.Count;

            MbeGapChkObjLine[] filllineArray = new MbeGapChkObjLine[filllineCount];
            filllinelist.CopyTo(filllineArray, 0);
            IComparer comparer = new fillLinecomp();
            Array.Sort(filllineArray, 0, filllineCount, comparer);



            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Removefloating 1: " + time_ms);
            sw.Reset();
            //--------------------------------------------------
            sw.Start();

			bool detectCon = true;
            while (detectCon) {
                detectCon = false;
                int limitDistance;

                for (int i = 0; i < filllineCount; i++) {
                    MbeGapChkObjLine line1 = filllineArray[i];
                    if (line1.netNum == 0) {
                        foreach (MbeGapChkObjLine line2 in outlinelist) {
                            if (line2.netNum != -1) {
                                continue;
                            }
                            limitDistance = ((line1.lineWidth + line2.lineWidth) / 2) * 9 / 10;
                            if (Util.LineIsCloseToLine(line1.p0, line1.p1, line2.p0, line2.p1, limitDistance)) {
                                line2.netNum = 0;
                                detectCon = true;
                                checkOutLineTouch(outlinelist, line2);
                            }
                        }
                        //checkFillLineTouch(filllineArray, i);
                        line1.netNum = 1;
                    }
                }

                {
                    int remainCount = 0;
                    foreach (MbeGapChkObjLine line in filllineArray) {
                        if (line.netNum != 1) remainCount++;
                    }
                    if ((filllineCount * 8 / 10) > remainCount) {
                        MbeGapChkObjLine[] newFilllineArray = new MbeGapChkObjLine[remainCount];
                        int j = 0;
                        foreach (MbeGapChkObjLine line in filllineArray) {
                            if (line.netNum != 1) {
                                newFilllineArray[j] = line;
                                j++;
                            }
                        }
                        filllineArray = newFilllineArray;
                        filllineCount = remainCount;
                    }

                }

                foreach (MbeGapChkObjLine line1 in outlinelist) {
                    if (line1.netNum == 0) {
                        //foreach (MbeGapChkObjLine line2 in outlinelist) {
                        //    if (line2.netNum != -1) {
                        //        continue;
                        //    }
                        //    limitDistance = ((line1.lineWidth + line2.lineWidth) / 2) * 9 / 10;
                        //    if (Util.LineIsCloseToLine(line1.p0, line1.p1, line2.p0, line2.p1, limitDistance)) {
                        //        line2.netNum = 0;
                        //        detectCon = true;
                        //    }
                        //}
                        if (checkOutLineTouch(outlinelist, line1)) {
                            detectCon = true;
                        }

                        limitDistance = ((line1.lineWidth + traceWidth) / 2) * 9 / 10;
                        for (int i = 0; i < filllineCount; i++) {
                            if (filllineArray[i].netNum != -1) {
                                continue;
                            }
                            if (Util.LineIsCloseToLine(line1.p0, line1.p1, filllineArray[i].p0, filllineArray[i].p1, limitDistance)) {
                                filllineArray[i].netNum = 0;
                                detectCon = true;
                                checkFillLineTouch(filllineArray, i);
                            }
                        }
                        line1.netNum = 1;
                    }
                }


                {
                    int remainCount = 0;
                    foreach (MbeGapChkObjLine line in filllineArray) {
                        if (line.netNum != 1) remainCount++;
                    }
                    if ((filllineCount * 8 / 10) > remainCount) {
                        MbeGapChkObjLine[] newFilllineArray = new MbeGapChkObjLine[remainCount];
                        int j = 0;
                        foreach (MbeGapChkObjLine line in filllineArray) {
                            if (line.netNum != 1) {
                                newFilllineArray[j] = line;
                                j++;
                            }
                        }
                        filllineArray = newFilllineArray;
                        filllineCount = remainCount;
                    }
                }




                /////////////////////////////////////////////////////
                //foreach (MbeGapChkObjLine line1 in linelist) {
                //    if (line1.netNum == 0) {
                //        foreach (MbeGapChkObjLine line2 in linelist) {
                //            if (line2.netNum != -1) {
                //                continue;
                //            }

                //            int limitDistance = ((line1.lineWidth + line2.lineWidth) / 2) * 9 / 10;

                //            if (Util.LineIsCloseToLine(line1.p0, line1.p1, line2.p0, line2.p1, limitDistance)) {
                //                line2.netNum = 0;
                //                detectCon = true;
                //            }


                //        }
                //        line1.netNum = 1;
                //    }
                //}
                /////////////////////////////////////////////////////



            }


            foreach (MbeGapChkObjLine obj in linelist) {
                dstLineList.AddLast(obj);
            }

            sw.Stop();
            time_ms = sw.ElapsedMilliseconds;
            System.Diagnostics.Debug.WriteLine("Removefloating 2: " + time_ms);

		}

        /// <summary>
        /// linelistint���X�g���ŐڐG�`�F�b�N���s��
        /// </summary>
        /// <param name="linelistint"></param>
        /// <param name="line1"></param>
        /// <remarks>outlinelist���̑��݃`�F�b�N�Ɏg���B�ڐG�������������̂�netNum�Ƃ���0��ݒ肷��B</remarks>
        protected bool checkOutLineTouch(LinkedList<MbeGapChkObjLine> linelistint, MbeGapChkObjLine line1)
        {
            bool resultFlag = false;
            bool detectFlag = true;
            while (detectFlag) {
                detectFlag = false;
                foreach (MbeGapChkObjLine line2 in linelistint) {
                    if (line2.netNum != -1) {
                        continue;
                    }
                    int limitDistance = ((line1.lineWidth + line2.lineWidth) / 2) * 9 / 10;
                    if (Util.LineIsCloseToLine(line1.p0, line1.p1, line2.p0, line2.p1, limitDistance)) {
                        line2.netNum = 0;
                        resultFlag = true;
                        detectFlag = true;
                        line1 = line2;
                        break;
                    }
                }
            }
            return resultFlag;
        }





        /// <summary>
        /// fillline�z����ŐڐG�`�F�b�N���s��
        /// </summary>
        /// <param name="lineArray"></param>
        /// <param name="startIndex"></param>
        /// <remarks>
        /// �ڐG�������������̂�netNum�Ƃ���0��ݒ肷��B
        /// �Ƃ肠�����A������FloodFill�̂悤�ɕ�����l�߂Ă������Ƃ͂��Ȃ��B
        /// </remarks>
        protected void checkFillLineTouch(MbeGapChkObjLine[] lineArray,int startIndex)
        {
            int index;
            int arrayCount = lineArray.Length;
            
            int ydiffLimit = traceWidth - 1000;
            int y;
            int x00;
            int x01;

            y = lineArray[startIndex].p0.Y;
            x00 = lineArray[startIndex].p0.X;
            x01 = lineArray[startIndex].p1.X;
            index = startIndex - 1;
            while (index >= 0) {
                MbeGapChkObjLine line1 = lineArray[index];
                if (line1.p0.Y < (y - ydiffLimit)) break;
                int x10 = line1.p0.X;
                int x11 = line1.p1.X;
                if (Util.IsOverlap(ref x00, ref x01, x10, x11)) {
                    if (line1.netNum == -1) {
                        line1.netNum = 0;
                    }
                    y = line1.p0.Y;
                    x00 = line1.p0.X;
                    x01 = line1.p1.X;
                }
                index--;
            }

            y = lineArray[startIndex].p0.Y;
            x00 = lineArray[startIndex].p0.X;
            x01 = lineArray[startIndex].p1.X;
            index = startIndex + 1;
            while (index < arrayCount){
                MbeGapChkObjLine line1 = lineArray[index];
                if (line1.p0.Y > (y + ydiffLimit)) break;
                int x10 = line1.p0.X;
                int x11 = line1.p1.X;
                if (Util.IsOverlap(ref x00, ref x01, x10, x11)) {
                    if (line1.netNum == -1) {
                        line1.netNum = 0;
                    }
                    y = line1.p0.Y;
                    x00 = line1.p0.X;
                    x01 = line1.p1.X;
                }
                index++;
            }

        }


		/// <summary>
		/// �|���S���̐ڑ��_���N�_�ɂ��āAworkList���̗v�f��ConnectCheck�̃t���O�𗧂Ă�
		/// </summary>
		/// <param name="ptConnect"></param>
		/// <param name="layer"></param>
		/// <param name="objList"></param>
		/// <returns></returns>
		protected bool SetupNetInfo(Point ptConnect,MbeLayer.LayerValue polygonLayer)
		{
			ClearConnectCheck();
			MbeConChk conChk = new MbeConChk();
			//conChk.ScanDataConnectPoint(ptConnect, (ulong)polygonLayer | (ulong)MbeLayer.LayerValue.PTH, workList);
			return conChk.ScanDataConnectPoint(ptConnect, (ulong)polygonLayer , workList);
		}

		protected void ClearConnectCheck()
		{
			foreach (MbeObj obj in workList) {
				if (obj.DeleteCount < 0) {
					obj.ClearConnectCheck();
				}
			} 
		}

		protected void ClearWorkFlag()
		{
			foreach (MbeObj obj in workList) {
				if (obj.DeleteCount < 0) {
					obj.TempPropString = "";
				}
			} 
		}
		
		/// <summary>
		/// ����f�[�^���쐬���邽�߂ɕK�v�ȗv�f�̃��X�g���쐬����
		/// </summary>
		/// <param name="mbeList"></param>
		protected void SetupWorkList(LinkedList<MbeObj> mbeList)
		{
			workList = new LinkedList<MbeObj>();
			foreach (MbeObj obj in mbeList) {
				if (obj.DeleteCount >= 0) continue;

				if (obj.Id() != MbeObjID.MbeComponent) {
					if (obj.Layer == MbeLayer.LayerValue.DRL ||
						obj.Layer == MbeLayer.LayerValue.CMP ||
                        obj.Layer == MbeLayer.LayerValue.L2  ||
                        obj.Layer == MbeLayer.LayerValue.L3  ||
                        obj.Layer == MbeLayer.LayerValue.SOL ||
						obj.Layer == MbeLayer.LayerValue.PTH) {
						obj.TempPropString = "";
						workList.AddLast(obj);
					}
				}else{
					foreach (MbeObj objContent in ((MbeObjComponent)obj).ContentsObj) {
                        if (objContent.Layer != MbeLayer.LayerValue.DRL &&
                            objContent.Layer != MbeLayer.LayerValue.CMP &&
                            objContent.Layer != MbeLayer.LayerValue.L2 &&
                            objContent.Layer != MbeLayer.LayerValue.L3 &&
                            objContent.Layer != MbeLayer.LayerValue.SOL &&
							objContent.Layer != MbeLayer.LayerValue.PTH) continue;

						if (objContent.Id() == MbeObjID.MbePTH ||
								  objContent.Id() == MbeObjID.MbePinSMD) {
                            if (((MbeObjPin)objContent).ThermalRelief == MbeObjPin.PadThermalRelief.ThmlRlfInComp) {
                                objContent.TempPropString = "Component";//((MbeObjComponent)obj).RefNumText;
                            }
						}
						workList.AddLast(objContent);
					}
				}
			}
		}

		
		/// <summary>
		/// �w�肵���|�C���g���g���̓����ɂ��邩�ǂ����𔻒肷��
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		/// <remarks>
		/// �|�C���g�̍����Řg���ƌ�������񐔂���Ȃ�����Ƃ݂Ȃ�
		/// </remarks>
		protected bool PointIsInside(Point pt, int tolerance, out bool onFrame)
		{
			int crossCount = 0;

			onFrame = false;
			double y = pt.Y + 0.5;
			foreach (MbeGapChkObjLine obj in orgFrameLineList) {
				if (tolerance > 0) {
                    if(Util.PointIsCloseToLine(pt, obj.p0, obj.p1,tolerance)){
					//double dist=Util.DistancePointLine(pt, obj.p0, obj.p1);
					//if (dist < tolerance) {
						onFrame = true;
						return true;
					}
				}

				
				double refX;
				if (Util.LineCrossingY(obj.p0, obj.p1,y, out refX)) {
					if (refX < pt.X) {
						crossCount++;
					}
				}
			}
			if ((crossCount & 1) == 1) {
				return true;
			} else {
				return false;
			}
		}

        /// <summary>
        /// ���X�g�Ɏ��߂�ꂽ����y����̕����𒊏o����
        /// </summary>
        /// <param name="srcList"></param>
        /// <param name="dstList"></param>
        /// <param name="y"></param>
        protected void DoDivideAreaV(LinkedList<MbeGapChkObjLine> srcList, LinkedList<MbeGapChkObjLine> dstList, int y)
        {
            //MbeGapChkObjLine testLine = new MbeGapChkObjLine();
            //testLine.p0 = new Point(Int32.MinValue,y);
            //testLine.p1 = new Point(Int32.MaxValue,y);
            //testLine.layer = layer;
           
            //objLine.layer = layer;

            LinkedListNode<MbeGapChkObjLine> node = srcList.First;
			while (node != null) {
				MbeGapChkObjLine obj = node.Value;
                LinkedListNode<MbeGapChkObjLine> nodeCurrent = node;
                node = node.Next;
                if (obj.p0.Y >= y && obj.p1.Y >= y) {
                    dstList.AddLast(obj);
                    srcList.Remove(nodeCurrent);
                } else if (obj.p0.Y < y && obj.p1.Y < y) {
                } else {
                    MbeGapChkObjLine newLine = null;
                    if (DivideLineWithHorizLine(obj, y, out newLine)) {
                        dstList.AddLast(newLine);
                    }
                }
            }
        }

        /// <summary>
        /// ���X�g�Ɏ��߂�ꂽ����x��荶�̕����𒊏o����
        /// </summary>
        /// <param name="srcList"></param>
        /// <param name="dstList"></param>
        /// <param name="y"></param>
        protected void DoDivideAreaH(LinkedList<MbeGapChkObjLine> srcList, LinkedList<MbeGapChkObjLine> dstList, int x)
        {
            //MbeGapChkObjLine testLine = new MbeGapChkObjLine();
            //testLine.p0 = new Point(x,Int32.MinValue);
            //testLine.p1 = new Point(x,Int32.MaxValue);
            //testLine.layer = layer;

            //objLine.layer = layer;

            //********** x�Ɉ�v���鐂�����̓R�s�[���K�v?


            LinkedListNode<MbeGapChkObjLine> node = srcList.First;
            while (node != null) {
                MbeGapChkObjLine obj = node.Value;
                LinkedListNode<MbeGapChkObjLine> nodeCurrent = node;
                node = node.Next;
                if (obj.p0.X <= x && obj.p1.X <= x) {
                    dstList.AddLast(obj);
                    srcList.Remove(nodeCurrent);
                } else if (obj.p0.X > x && obj.p1.X > x) {
                } else {
                    MbeGapChkObjLine newLine = null;
                    if (DivideLineWithVertLine(obj, x, out newLine)) {
                        dstList.AddLast(newLine);
                    }
                }
            }
        }




        /// <summary>
        /// �T�[�}���p�^�[���̏\�������O�`���Őؒf����B
        /// </summary>
        /// <param name="lineList"></param>
        protected void DoDivideThermalJunction(LinkedList<MbeGapChkObjLine> lineList)
        {
            LinkedListNode<MbeGapChkObjLine> node = thermalJunctionList.First;
            while (node != null) {
                MbeGapChkObjLine line2 = node.Value;
                foreach (MbeGapChkObjLine line1 in lineList) {
                    MbeGapChkObjLine newLine1;
                    MbeGapChkObjLine newLine2;
                    if (DivideLineAtCrossing(true, 100, line1, line2, out newLine1, out newLine2)) {
                        if (newLine2 != null) {
                            thermalJunctionList.AddLast(newLine2);
                        }
                    }
                }
                node = node.Next;
            }
        }



		protected void DoDivideLineAtCrossing(LinkedList<MbeGapChkObjLine> lineList)
		{
			int loopCount = 0;
			int lineAddCount = 0;
			LinkedListNode<MbeGapChkObjLine> node1 = lineList.First;
			while (node1 != null) {
				MbeGapChkObjLine line1 = node1.Value;
				LinkedListNode<MbeGapChkObjLine> node2 = node1.Next;
				while (node2 != null) {
					MbeGapChkObjLine line2 = node2.Value;
					MbeGapChkObjLine nLine1;
					MbeGapChkObjLine nLine2;

   
                    
                    if (DivideLineAtCrossing(false,100,line1, line2, out nLine1, out nLine2)) {
						if (nLine1 != null) {
							lineList.AddLast(nLine1);
							lineAddCount++;
						}
						if (nLine2 != null) {
							lineList.AddLast(nLine2);
							lineAddCount++;
						}
					}

					loopCount++;
					node2 = node2.Next;
				}
				node1 = node1.Next;
			}
			//System.Diagnostics.Debug.WriteLine("DoDivideLineAtCrossing loopCount: " + loopCount);
			//System.Diagnostics.Debug.WriteLine("DoDivideLineAtCrossing lineAddCount: " + lineAddCount);
		}

        /// <summary>
        /// �����𐅕����ŕ�������B
        /// newLine�ɂ́Ay����̐�������
        /// </summary>
        /// <param name="line"></param>
        /// <param name="y"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        protected bool DivideLineWithHorizLine(MbeGapChkObjLine line, int y, out MbeGapChkObjLine newLine)
        {
            newLine = null;
            if((line.p0.Y >= y && line.p1.Y >= y)||(line.p0.Y < y && line.p1.Y < y)) return false;
            double dx;
            if(!Util.LineCrossingY(line.p0,line.p1,(double)y,out dx)) return false;
            Point pnew = new Point((int)Math.Round(dx), y);
            newLine = new MbeGapChkObjLine();
            //newLine.p0 = line.p0;
            //newLine.p1 = line.p1;
            newLine.lineWidth = line.lineWidth;
            newLine.netNum = line.netNum;
            newLine.status = line.status;
            if(line.p0.Y < line.p1.Y){
                newLine.p0 = pnew;
                newLine.p1 = line.p1;
                line.p1 = pnew;
            } else {
                newLine.p0 = line.p0;
                newLine.p1 = pnew;
                line.p0 = pnew;
            }
            return true;
        }

        /// <summary>
        /// �����𐂒����ŕ�������B
        /// newLine�ɂ́Ax��荶�̐�������
        /// </summary>
        /// <param name="line"></param>
        /// <param name="y"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        protected bool DivideLineWithVertLine(MbeGapChkObjLine line, int x, out MbeGapChkObjLine newLine)
        {
            newLine = null;
            if ((line.p0.X <= x && line.p1.X <= x) || (line.p0.X > x && line.p1.X > x)) return false;
            double dy;
            if (!Util.LineCrossingX(line.p0, line.p1, (double)x, out dy)) return false;
            Point pnew = new Point(x, (int)Math.Round(dy));
            newLine = new MbeGapChkObjLine();
            //newLine.p0 = line.p0;
            //newLine.p1 = line.p1;
            newLine.lineWidth = line.lineWidth;
            newLine.netNum = line.netNum;
            newLine.status = line.status;
            if (line.p0.X > line.p1.X) {
                newLine.p0 = pnew;
                newLine.p1 = line.p1;
                line.p1 = pnew;
            } else {
                newLine.p0 = line.p0;
                newLine.p1 = pnew;
                line.p0 = pnew;
            }
            return true;
        }




		/// <summary>
		/// 2�{�̐������������Ă���Ƃ��A�����_�ŕ�������
		/// </summary>
		/// <param name="divideLine2only">true�̂Ƃ��Aline1�͕������Ȃ�</param>
		/// <param name="tolerance">��_�����̒l���[�_�ɐڋ߂��Ă���ꍇ�͕������Ȃ�</param>
		/// <param name="line1"></param>
		/// <param name="line2"></param>
		/// <param name="nLine1"></param>
		/// <param name="nLine2"></param>
		/// <returns></returns>
		protected bool DivideLineAtCrossing(bool divideLine2only, 
											int tolerance,
											MbeGapChkObjLine line1, MbeGapChkObjLine line2, 
											out MbeGapChkObjLine nLine1, out MbeGapChkObjLine nLine2)
		{
			nLine1 = null;
			nLine2 = null;
			Point ptCrossing = new Point(0,0);
			bool divFlag = false;

            //if ((line1.p0.X == line1.p1.X && line1.p0.X == 711290) &&
            //    (line2.p0.X == 711279 || line2.p1.X == 711279)) {
            //    System.Diagnostics.Debug.WriteLine("Debug 20090101");
            //}

            //if ((line2.p0.X == line2.p1.X && line2.p0.X == 711290) &&
            //    (line1.p0.X == 711279 || line1.p1.X == 711279)) {
            //    System.Diagnostics.Debug.WriteLine("Debug 20090101");
            //}

            //if (line1.p0.X == line1.p1.X && line1.p0.X == 711290 && line2.p0.X == 711290) {
            //    System.Diagnostics.Debug.WriteLine("Debug 20090103");
            //}

            //if (line2.p0.X == line2.p1.X && line2.p0.X == 711290 && line1.p0.X == 711290) {
            //    System.Diagnostics.Debug.WriteLine("Debug 20090103");
            //}

			if (line1.netNum == 0 && line2.netNum == 0) return false;
			if (line1.status == LINE_STATUS_FRAME && line2.status == LINE_STATUS_FRAME) return false; 



			if (Util.LineCrossing(line1.p0, line1.p1, line2.p0, line2.p1, ref ptCrossing)) {

				//if(	Util.PointIsCloseToPoint(line1.p0, ptCrossing, tolerance) ||
				//    Util.PointIsCloseToPoint(line1.p1, ptCrossing, tolerance) ||
				//    Util.PointIsCloseToPoint(line2.p0, ptCrossing, tolerance) ||
				//    Util.PointIsCloseToPoint(line2.p1, ptCrossing, tolerance)) {
				//    return false;
				//}

				
				
				//if (!divideLine2only && 
				//    line2.netNum != 0 &&
				//    Util.DistancePointPoint(line1.p0,ptCrossing)>tolerance &&
				//    Util.DistancePointPoint(line1.p1,ptCrossing)>tolerance){
				if (!divideLine2only && 
					line2.netNum != 0 &&
					!Util.PointIsCloseToPoint(line1.p0, ptCrossing, tolerance) &&
					!Util.PointIsCloseToPoint(line1.p1, ptCrossing, tolerance)
					){
					nLine1 = new MbeGapChkObjLine();
					nLine1.p0 = ptCrossing;
					nLine1.p1 = line1.p1;
					nLine1.lineWidth = line1.lineWidth;
					nLine1.netNum = line1.netNum;
					nLine1.status = line1.status;
					line1.p1 = ptCrossing;
					divFlag = true;
				}

				//if (line1.netNum != 0 &&
				//    Util.DistancePointPoint(line2.p0,ptCrossing)>tolerance &&
				//    Util.DistancePointPoint(line2.p1,ptCrossing)>tolerance){
				if (line1.netNum != 0 &&
					!Util.PointIsCloseToPoint(line2.p0, ptCrossing, tolerance)&&
					!Util.PointIsCloseToPoint(line2.p1, ptCrossing, tolerance)
					){
					nLine2 = new MbeGapChkObjLine();
					nLine2.p0 = ptCrossing;
					nLine2.p1 = line2.p1;
					nLine2.lineWidth = line2.lineWidth;
					nLine2.netNum = line2.netNum;
					nLine2.status = line2.status;
					line2.p1 = ptCrossing;
					divFlag = true;
				}
			}
			return divFlag;
		}



	


		protected MbeRect rcArea;
		protected MbeLayer.LayerValue layer;
		protected Point ptConnect;
		protected int traceWidth;
		protected int patternGap;
		protected int thermalGap;
		protected bool removeFloating;

        protected int verticalDivision;
        protected int verticalDivisionHeight;


		protected LinkedList<MbeGapChkObjLine> orgFrameLineList;	//GUI�Ŏw�肵�����p�`���g���[�X��������
		protected LinkedList<MbeGapChkObjLine> frameLineList;	//��Ɨp�̑��p�`
        protected LinkedList<MbeGapChkObjLine>[] fillLineListArray;	//�h��Ԃ��̉���
        protected LinkedList<MbeGapChkObjLine>[] outlineListArray;
        protected LinkedList<MbeGapChkObjLine> thermalJunctionList; //�T�[�}���p�^�[���̏\����

		protected LinkedList<MbeObj> workList;	//����p�^�[���𐶐����邽�߂ɕK�v��CMP/SOL/PTH�̃f�[�^

		protected LinkedList<MbeGapChkObj> keepOutPatterns;
        protected LinkedList<MbeGapChkObj>[] keepOutPatternsPerAreaArray;

		protected LinkedList<MbeGapChkObj> connectPatterns;


		public class xpointcomp : IComparer
		{
			public int Compare(Object x, Object y)
			{
				return (int)x - (int)y;
			}
		}

		public class fillLinecomp : IComparer
		{
			public int Compare(Object x, Object y)
			{
				MbeGapChkObjLine lineX = (MbeGapChkObjLine)x;
				MbeGapChkObjLine lineY = (MbeGapChkObjLine)y;

			
                //System.Diagnostics.Debug.WriteLineIf((Math.Abs(lineX.p0.Y - lineX.p1.Y) > 100), "Unexpected slant fillline");
                //System.Diagnostics.Debug.WriteLineIf((Math.Abs(lineY.p0.Y - lineY.p1.Y) > 100), "Unexpected slant fillline");


				return lineX.p0.Y - lineY.p0.Y;
			}
		}



	}

}
