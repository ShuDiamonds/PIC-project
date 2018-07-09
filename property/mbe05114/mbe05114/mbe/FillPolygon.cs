using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace mbe
{
	class FillPolygon
	{
		protected const int LINE_STATUS_FILL  = 0;
		protected const int LINE_STATUS_FRAME = 1;
		protected const int LINE_DELETED = -1;



		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FillPolygon()
		{
			orgFrameLineList = new LinkedList<MbeGapChkObjLine>();
			frameLineList = new LinkedList<MbeGapChkObjLine>();
			fillLineList = new LinkedList<MbeGapChkObjLine>();
			outlineList = new LinkedList<MbeGapChkObjLine>();
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
			

			orgFrameLineList.Clear();
			frameLineList.Clear();
			fillLineList.Clear();
			outlineList.Clear();

			if (!SetFrame(polygon)) return false;
			PlaceFillLines();

			foreach (MbeGapChkObjLine obj in frameLineList) {
				objList.AddLast(obj);
			}
			foreach (MbeGapChkObjLine obj in fillLineList) {
				objList.AddLast(obj);
			}
			return true;
		}



		/// <summary>
		/// 塗りつぶしのアップデートを行う
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
			

			orgFrameLineList.Clear();
			frameLineList.Clear();
			fillLineList.Clear();
			outlineList.Clear();

			//-----------------------------------------
			sw.Start();
			if (!SetFrame(polygon)) return false;
            //PlaceFillLines();

			SetupWorkList(mainList);
			SetupNetInfo(polygon.GetPos(0),polygon.Layer);
			sw.Stop();
			time_ms = sw.ElapsedMilliseconds;
			System.Diagnostics.Debug.WriteLine("Fill polygon Step1: " + time_ms);
			sw.Reset();
			//-----------------------------------------
			sw.Start();
			PlaceOutlines();
            PlaceFillLines();
            sw.Stop();
			time_ms = sw.ElapsedMilliseconds;
			System.Diagnostics.Debug.WriteLine("Fill polygon Step2: " + time_ms);
			sw.Reset();
			//-----------------------------------------
			sw.Start();
			TrimFillLines();
			sw.Stop();
			time_ms = sw.ElapsedMilliseconds;
			System.Diagnostics.Debug.WriteLine("Fill polygon Step3: " + time_ms);
			sw.Reset();
			//-----------------------------------------
			sw.Start();

			foreach (MbeGapChkObjLine obj in outlineList) {
				fillLineList.AddLast(obj);
			}
			outlineList.Clear();
			if (removeFloating) {
				RemoveIsland();
			}
			sw.Stop();
			time_ms = sw.ElapsedMilliseconds;
			System.Diagnostics.Debug.WriteLine("Fill polygon Step4: " + time_ms);
			sw.Reset();
			
			ClearConnectCheck();//これを忘れると接続ネットの描画がハイライトになる。

			ClearWorkFlag();

			polygon.fillLineList.Clear();
			//foreach (MbeGapChkObjLine obj in frameLineList) {
			//    polygon.fillLineList.AddLast(obj);
			//}
			foreach (MbeGapChkObjLine obj in fillLineList) {
				if (obj.netNum != -1 || !removeFloating) {
					polygon.fillLineList.AddLast(obj);
				}
			}
			//foreach (MbeGapChkObjLine obj in outlineList) {
			//    polygon.fillLineList.AddLast(obj);
			//}

			//foreach (MbeGapChkObjLine obj in polygon.fillLineList) {
			//    Point pt0 = obj.p0;
			//    Point pt1 = obj.p1;
			//    System.Diagnostics.Debug.WriteLine("Fill  " + pt0.X + "," + pt0.Y + "," + pt1.X + "," + pt1.Y);
			//}


			return true;
		}


		/// <summary>
		/// 多角形の枠線の生成
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
		/// 塗りつぶしの横線の配置
		/// </summary>
		/// <returns></returns>
		protected bool PlaceFillLines()
		{
			//fillLineList.Clear();
			int count = orgFrameLineList.Count;
			int[] xpoint = new int[count];
			IComparer comparer = new xpointcomp();


			int step = traceWidth - 1500;

			double y = (double)rcArea.T-step - 0.5;//ポリゴン枠頂点と計算時に一致する面倒を避けるために、小数値でオフセット
			while (y > rcArea.B) {
				int xpIndex = 0;
				foreach (MbeGapChkObjLine obj in orgFrameLineList) {
					double refX;
					if (Util.LineCrossingY(obj.p0, obj.p1, y, out refX)) {
						xpoint[xpIndex] = (int)refX;
						xpIndex++;
						if (xpIndex == count) break;//これでbreakはありえないはず
					}
				}
				//System.Diagnostics.Debug.Write("FillPolygon dump xpoint1 ");
				//for (int i = 0; i < xpIndex; i++) System.Diagnostics.Debug.Write(xpoint[i] + " ");
				//System.Diagnostics.Debug.WriteLine("");

				Array.Sort(xpoint, 0, xpIndex, comparer);

				//System.Diagnostics.Debug.Write("FillPolygon dump xpoint2 ");
				//for (int i = 0; i < xpIndex; i++) System.Diagnostics.Debug.Write(xpoint[i] + " ");
				//System.Diagnostics.Debug.WriteLine("");

				int nLine = xpIndex / 2;
				int j = 0;
				int ny = (int)y;
				for (int i = 0; i < nLine; i++) {
					Point pt0 = new Point(xpoint[j], ny);
					Point pt1 = new Point(xpoint[j+1], ny);
					MbeGapChkObjLine objLine = new MbeGapChkObjLine();
					objLine.layer = layer;
					objLine.SetLineValue(pt0, pt1, traceWidth);
					j += 2;
					fillLineList.AddLast(objLine);
				}
				y = y - step;
			}


			return true;
		}

		/// <summary>
		/// 信号パターンの周縁線の配置
		/// </summary>
		protected void PlaceOutlines()
		{
			LinkedList<MbeGapChkObjLine> tempList1 = new LinkedList<MbeGapChkObjLine>();
			LinkedList<MbeGapChkObjLine> tempList2 = new LinkedList<MbeGapChkObjLine>();
			LinkedList<MbeGapChkObjLine> tempList3 = new LinkedList<MbeGapChkObjLine>();

			LinkedList<MbeObjPin> pinList = new LinkedList<MbeObjPin>();//アウトラインを生成するピンのリスト

			GenOutlineParam outlineParam;

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			long time_ms;

			outlineParam.rc = rcArea;
			outlineParam.layer = layer;
			outlineParam.traceWidth = traceWidth;
			outlineParam.option = 0;
			//--------------------------------------------------
			sw.Start();

//最初にランド、パッドだけの外形線を構築する。
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
							pinList.AddLast((MbeObjPin)obj);//アウトラインを生成するピンのリストに登録
						}
					} else {
						obj.GenerateGapChkData(connectPatterns, 0);
					}
				}
			}

			//四角枠上下左右外のものを取り除く
			foreach (MbeGapChkObjLine obj in tempList2) {
				if (Util.LineIsOutsideLTRB(obj.p0, obj.p1, rcArea)) {
					continue;
				}
				tempList1.AddLast(obj);
			}

			tempList2.Clear();
			optimizeVHLine(tempList1);
			//交点で切断する
			DoDivideLineAtCrossing(tempList1);
			RemovePtnTouchKeeepOut(tempList1, tempList2);
			tempList1.Clear();
//ここまででランド、パッドだけの外形線がtempList2に保存される

//ランドパッド以外の外形線を追加する
			foreach (MbeObj obj in workList) {
				//ランドパッドは処理済みなのでcontinueで飛ばす
				if (obj.Id() == MbeObjID.MbePTH ||
					obj.Id() == MbeObjID.MbePinSMD){
					continue;
				}


				outlineParam.option = 0;
				if (!obj.ConnectionCheckActive) {

					//↓ランドやパッドに埋まったラインアウトラインの端点キャップは省けるかと考えた
					//  ギャップトレース設定が小さいときは端点のランドやパッドの検索に時間がかかって、
					//  あまり得にならない。
					//
					////ラインデータのときは端点にランドパッドがないかチェックする。
					////端点にランドパッドがあって、それが線幅より大きいものであれば、外形線のエンドキャップは要らない
					if (obj.Id() == MbeObjID.MbeLine && obj.Layer == layer) {
						int linewidth = ((MbeObjLine)obj).LineWidth;
						Point leP0 = ((MbeObjLine)obj).GetPos(0);
						Point leP1 = ((MbeObjLine)obj).GetPos(1);

						foreach (MbeObjPin objPin in pinList) {
							Point padPt = objPin.GetPos(0);
							if (padPt.Equals(leP0)) {
								Size padSize = objPin.PadSize;
								if (padSize.Width >= linewidth && padSize.Height >= linewidth) {
									outlineParam.option |= GenOutlineParam.P0_NO_LINECAP;
								}
							} else if (padPt.Equals(leP1)) {
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
					
					
					outlineParam.gap = patternGap;
					int c1 = tempList1.Count;
					obj.GenerateOutlineData(tempList1, outlineParam);
					if (c1 != tempList1.Count) {
						if (obj.Id() == MbeObjID.MbeText) {
							MbeGapChkObjRect gapChkObj = new MbeGapChkObjRect();
							gapChkObj.layer = layer;
							gapChkObj.netNum = -1;
							gapChkObj.mbeObj = obj;
							gapChkObj.SetRectValue(((MbeObjText)obj).AreaRect());
							keepOutPatterns.AddLast(gapChkObj);
						} else if (obj.Id() == MbeObjID.MbePolygon) {
							((MbeObjPolygon)obj).FullFillLineData(keepOutPatterns);
						} else if(obj.Id() == MbeObjID.MbeHole){
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

			//四角枠上下左右外のものを取り除く
			foreach (MbeGapChkObjLine obj in tempList1) {
				if(Util.LineIsOutsideLTRB(obj.p0, obj.p1, rcArea)){
					continue;
				}
				tempList2.AddLast(obj);
			}


			optimizeVHLine(tempList2);
			

			//サーマルパッド処理
			foreach (MbeObj obj in workList) {
				if (obj.Layer == layer || obj.Layer == MbeLayer.LayerValue.PTH) {
					if (obj.Id() == MbeObjID.MbePTH ||
						obj.Id() == MbeObjID.MbePinSMD) {
						if (obj.ConnectionCheckActive && obj.TempPropString.Length > 0) {
							Point pc = obj.GetPos(0);
							bool onFrame;//dummy
							if (!PointIsInside(pc, 0,out onFrame)) {
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
							thLine = new MbeGapChkObjLine();
							thLine.SetLineValue(pc, pe, lineWidth);
							thLine.netNum = 0;
							tempList2.AddLast(thLine);

							pe.X = pc.X + hlen;
							thLine = new MbeGapChkObjLine();
							thLine.SetLineValue(pc, pe, lineWidth);
							thLine.netNum = 0;
							tempList2.AddLast(thLine);

							pe = pc;
							pe.Y = pc.Y - vlen;
							lineWidth = (lineWidthBase < padWidth ? lineWidthBase : padWidth);
							thLine = new MbeGapChkObjLine();
							thLine.SetLineValue(pc, pe, lineWidth);
							thLine.netNum = 0;
							tempList2.AddLast(thLine);

							pe = pc;
							pe.Y = pc.Y + vlen;
							thLine = new MbeGapChkObjLine();
							thLine.SetLineValue(pc, pe, lineWidth);
							thLine.netNum = 0;
							tempList2.AddLast(thLine);


						}
					}
				}
			}





			sw.Stop();
			time_ms = sw.ElapsedMilliseconds;
			System.Diagnostics.Debug.WriteLine("Fill polygon Step2-2: " + time_ms);
			sw.Reset();
			//--------------------------------------------------
			sw.Start();



			//作業用枠線を追加する
			foreach(MbeGapChkObjLine obj in frameLineList){
			    tempList2.AddLast(obj);
			}
			frameLineList.Clear();


			//交点で切断する
			DoDivideLineAtCrossing(tempList2);

			sw.Stop();
			time_ms = sw.ElapsedMilliseconds;
			System.Diagnostics.Debug.WriteLine("Fill polygon Step2-3: " + time_ms);
			sw.Reset();
			//--------------------------------------------------
			sw.Start();

			RemovePtnTouchKeeepOut(tempList2, tempList1);

			sw.Stop();
			time_ms = sw.ElapsedMilliseconds;
			System.Diagnostics.Debug.WriteLine("Fill polygon Step2-4: " + time_ms);
			sw.Reset();
			//--------------------------------------------------
			sw.Start();


			//片端点が枠外のものを取り除く
			foreach (MbeGapChkObjLine obj in tempList1) {
				bool p0onFrame = false;
				bool p1onFrame = false;
				if (!PointIsInside(obj.p0,600,out p0onFrame) || !PointIsInside(obj.p1,600,out p1onFrame)) {
					continue;
				}
				//両端点が枠内と判定されても、両端点が枠線上にあって、中点が枠外のときは取り除く
				if(obj.status != LINE_STATUS_FRAME && p0onFrame && p1onFrame) {
					Point pc = new Point((obj.p0.X + obj.p1.X) / 2, (obj.p0.Y + obj.p1.Y) / 2);
					bool pconFrame;
					if (!PointIsInside(pc, 100, out pconFrame)) {
						continue;
					}
				}
				outlineList.AddLast(obj);
			}

			sw.Stop();
			time_ms = sw.ElapsedMilliseconds;
			System.Diagnostics.Debug.WriteLine("Fill polygon Step2-5: " + time_ms);
			sw.Reset();
			//--------------------------------------------------

		}

		protected void RemovePtnTouchKeeepOut(LinkedList<MbeGapChkObjLine> srcList, LinkedList<MbeGapChkObjLine> dstList)
		{
			//キープアウトパターンに近接しているものを取り除く
			dstList.Clear();
			foreach (MbeGapChkObjLine obj in srcList) {
				bool removeIt = false;
				foreach (MbeGapChkObj objKo in keepOutPatterns) {
					if (objKo.layer != layer) {
						continue;
					}
					if (objKo.netNum == 0 && obj.netNum == 0) {
						continue;
					}

					int gap;
					if (objKo.netNum == -1) {
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
		/// フィルラインのトリミング
		/// </summary>
		protected void TrimFillLines()
		{
			LinkedList<MbeGapChkObjLine> tempList1 = new LinkedList<MbeGapChkObjLine>();
			MbeGapChkObjLine line1 = new MbeGapChkObjLine();

			//outlineとの交点で切断する
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
				foreach (MbeGapChkObj objKo in keepOutPatterns) {
					if (objKo.layer != layer) {
						continue;
					}
					if (obj.netNum == 0 && objKo.netNum ==0) {
						continue;
					}


					int gap;
					if (objKo.netNum == -1) {
						gap = patternGap;
					} else {
						
						gap = thermalGap;
					}
					gap = gap * 95 / 100;
					Point pt;
					double dist = obj.Distance(objKo, out pt);
					if (dist < gap) {
						removeIt = true;
						break;
					}
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


		protected void RemoveIsland()
		{

			foreach (MbeGapChkObjLine obj in fillLineList) {
				if (obj.netNum == 0) {
					continue;
				}
				
				foreach (MbeGapChkObj objCon in connectPatterns) {
					if (objCon.layer == layer || objCon.layer == MbeLayer.LayerValue.PTH) {
						if(obj.IsCloseTo(objCon,-1000)){
							obj.netNum = 0;
							break;
						}


					}
				}
			}

			//int arraySize = fillLineList.Count;
			//MbeGapChkObjLine[] lineArray = new MbeGapChkObjLine[arraySize];
			//int i=0;
			//foreach (MbeGapChkObjLine lineObj in fillLineList) {
			//    lineArray[i] = lineObj;
			//    i++;
			//}

			//IComparer comparer = new fillLinecomp();
			//Array.Sort(lineArray, comparer);
			////↓水平線同士が接続するとき、Y値がmaxYDistanceHorzLineより離れることはない
			//int maxYDistanceHorzLine = (traceWidth + Math.Max(traceWidth, thermalGap * 15 / 10)) / 2;
			//bool detectCon = true;
			//while (detectCon) {
			//    detectCon = false;
			//    for (int index1 = 0; index1 < arraySize; index1++) {
			//        MbeGapChkObjLine line1 = lineArray[index1];
			//        if (line1.netNum == 1) continue;
			//        int line1StartNetNum = line1.netNum;
			//        for (int index2 = index1 + 1; index2 < arraySize; index2++) {
			//            MbeGapChkObjLine line2 = lineArray[index2];

			//            //line1が水平であるときlineArrayはソート済みなので、line2も水平となる。
			//            //line2のy値がline1のy値よりmaxYDistanceHorzLineぶん以上大きいときは接触しない
			//            if (line1.p0.Y == line1.p1.Y) {
			//                if ((line2.p0.Y - line1.p0.Y) > maxYDistanceHorzLine) {
			//                    break;
			//                }
			//            }

			//            //if (line1.netNum == -1 && line2.netNum == -1) {
			//            if (line1.netNum == line2.netNum) {
			//                continue;
			//            }


			//            int limitDistance = ((line1.lineWidth + line2.lineWidth) / 2) * 9 / 10;

			//            if (Util.LineIsCloseToLine(line1.p0, line1.p1, line2.p0, line2.p1, limitDistance)) {
			//                line1.netNum = 0;
			//                line2.netNum = 0;
			//                detectCon = true;
			//            }
			//        }
			//        if (line1StartNetNum == 0) {
			//            line1.netNum = 1;
			//        }
			//    }
			//}


			//bool detectCon = true;
			//while (detectCon) {
			//    detectCon = false;
			//    for (int index1 = 0; index1 < arraySize; index1++) {
			//        MbeGapChkObjLine line1 = lineArray[index1];
			//        if (line1.netNum != 0) {
			//            continue;
			//        }
			//        for (int index2 = 0; index2 < arraySize; index2++) {
			//            MbeGapChkObjLine line2 = lineArray[index2];
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


			bool detectCon = true;
			while (detectCon) {
				detectCon = false;
				foreach (MbeGapChkObjLine line1 in fillLineList) {
					if (line1.netNum == 0) {
						foreach (MbeGapChkObjLine line2 in fillLineList) {
							if (line2.netNum != -1) {
								continue;
							}

							int limitDistance = ((line1.lineWidth + line2.lineWidth) / 2) * 9 / 10;

							if (Util.LineIsCloseToLine(line1.p0, line1.p1, line2.p0, line2.p1, limitDistance)) {
								line2.netNum = 0;
								detectCon = true;
							}


						}
						line1.netNum = 1;
					}
				}
			}

		}


		/// <summary>
		/// ポリゴンの接続点を起点にして、workList内の要素にConnectCheckのフラグを立てる
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
			conChk.ScanDataConnectPoint(ptConnect, (ulong)polygonLayer , workList);

			return true;
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
		/// 回避データを作成するために必要な要素のリストを作成する
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
						obj.Layer == MbeLayer.LayerValue.SOL ||
						obj.Layer == MbeLayer.LayerValue.PTH) {
						obj.TempPropString = "";
						workList.AddLast(obj);
					}
				}else{
					foreach (MbeObj objContent in ((MbeObjComponent)obj).ContentsObj) {
						if (objContent.Layer != MbeLayer.LayerValue.CMP &&
							objContent.Layer != MbeLayer.LayerValue.SOL &&
							objContent.Layer != MbeLayer.LayerValue.PTH) continue;

						if (objContent.Id() == MbeObjID.MbePTH ||
								  objContent.Id() == MbeObjID.MbePinSMD) {
							objContent.TempPropString = "Component";//((MbeObjComponent)obj).RefNumText;
						}
						workList.AddLast(objContent);
					}
				}
			}
		}

		
		/// <summary>
		/// 指定したポイントが枠線の内側にあるかどうかを判定する
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		/// <remarks>
		/// ポイントの左側で枠線と交差する回数が奇数なら内側とみなす
		/// </remarks>
		protected bool PointIsInside(Point pt, int tolerance, out bool onFrame)
		{
			int crossCount = 0;

			onFrame = false;
			double y = pt.Y + 0.5;
			foreach (MbeGapChkObjLine obj in orgFrameLineList) {
				if (tolerance > 0) {
					double dist=Util.DistancePointLine(pt, obj.p0, obj.p1);
					if (dist < tolerance) {
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
			System.Diagnostics.Debug.WriteLine("DoDivideLineAtCrossing loopCount: " + loopCount);
			System.Diagnostics.Debug.WriteLine("DoDivideLineAtCrossing lineAddCount: " + lineAddCount);
		}

		/// <summary>
		/// 2本の線分が交差しているとき、交差点で分割する
		/// </summary>
		/// <param name="divideLine2only">trueのとき、line1は分割しない</param>
		/// <param name="tolerance">交点がこの値より端点に接近している場合は分割しない</param>
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

		protected LinkedList<MbeGapChkObjLine> orgFrameLineList;	//GUIで指定した多角形をトレースしたもの
		protected LinkedList<MbeGapChkObjLine> frameLineList;	//作業用の多角形
		protected LinkedList<MbeGapChkObjLine> fillLineList;	//塗りつぶしの横線
		protected LinkedList<MbeGapChkObjLine> outlineList;		//回避パターンの輪郭
		protected LinkedList<MbeObj> workList;	//回避パターンを生成するために必要なCMP/SOL/PTHのデータ

		protected LinkedList<MbeGapChkObj> keepOutPatterns;
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

				//水平でないものを前に置く
				bool horz1 = (lineX.p0.Y == lineX.p1.Y);
				bool horz2 = (lineY.p0.Y == lineY.p1.Y);
				if (horz1 && !horz2) return 1;
				if (!horz1 && horz2) return -1;

				////トレースラインの幅でないものを前に置く
				//bool traceline1 = (lineX.lineWidth == traceWidth);
				//bool traceline2 = (lineY.lineWidth == traceWidth);
				//if (traceline1 && !traceline2) return 1;
				//if (!traceline1 && traceline2) return -1;

				return lineX.p0.Y - lineY.p0.Y;
			}
		}



	}



    //public struct GenOutlineParam
    //{
    //    public MbeRect rc;
    //    public MbeLayer.LayerValue layer;
    //    public int traceWidth;
    //    public int gap;
    //    public int option;

    //    public const int P0_NO_LINECAP = 1;
    //    public const int P1_NO_LINECAP = 2;
    //}


}
