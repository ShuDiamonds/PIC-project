using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;

using CE3IO;

namespace mbe
{
	class MbeObjLine : MbeObj
	{

		public const int MIN_LINE_WIDTH   =      0; 	//0mm
		public const int MAX_LINE_WIDTH   =  80000;		//8mm
		public const int DEFAULT_LINE_WIDTH = 6000;		//0.6mm

		public enum MbeLineStyle
		{
			Straight = 0,
			Bending1,
			Bending2
		}

		protected static readonly string[] lineStyleName = {
			"S",
			"B1",
			"B2"
		};


		/// <summary>
		/// ライン幅の設定と取得 
		/// </summary>
		public int LineWidth
		{
			get { return lineWidth; }
			set { 
				lineWidth = value;
				if (lineWidth < MIN_LINE_WIDTH) lineWidth = MIN_LINE_WIDTH;
				else if (lineWidth > MAX_LINE_WIDTH) lineWidth = MAX_LINE_WIDTH;
			}
		}


		public MbeLineStyle LineStyle
		{
			get { return lineStyle; }
			set { lineStyle = value; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MbeObjLine()
		{
			posCount = 2;
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			for(int i=0;i<posCount;i++){
				selectFlag[i] = false;
			}
			lineStyle = MbeLineStyle.Straight;
			lineWidth = DEFAULT_LINE_WIDTH;
		}


		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="mbeObjPin"></param>
		public MbeObjLine(MbeObjLine mbeObjLine)
			: base(mbeObjLine)
		{
			lineStyle = mbeObjLine.lineStyle;
			lineWidth = mbeObjLine.lineWidth;
		}

		/// <summary>
		/// 有効かどうかを返す
		/// </summary>
		/// <returns></returns>
		public override bool IsValid()
		{
			if (GetPos(0).Equals(GetPos(1))) return false;
			return true;
		}

		/// <summary>
		/// 図面オブジェクトごとのID値を返す
		/// </summary>
		public override MbeObjID Id()
		{
		    return MbeObjID.MbeLine;
		}

		/// <summary>
		/// レイヤー値の取得と設定
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
             
				snapLayer = (ulong)layer|
					(ulong)MbeLayer.LayerValue.PTH;
			}
		}


        static public ulong SnapLayer(MbeLayer.LayerValue placeLayer)
        {
            return (ulong)placeLayer |
                    (ulong)MbeLayer.LayerValue.PTH;
        }


		/// <summary>
		/// 配置指定可能なレイヤー
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
                    (ulong)MbeLayer.LayerValue.MMC |
                    (ulong)MbeLayer.LayerValue.MMS |
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
					return MbeLayer.LayerValue.CMP;
				} else {
					return MbeLayer.LayerValue.SOL;
				}
			}
		}

		/// <summary>
		/// Mb3ファイルの読み込み時のメンバーの解釈を行う
		/// </summary>
		/// <param name="str1">変数名または"+"で始まるブロックタグ</param>
		/// <param name="str2">変数値</param>
		/// <param name="readCE3">ブロック読み込み時に使うReadCE3クラス</param>
		/// <returns>正常終了時にReadCE3.RdStatus.NoErrorを返す</returns>
		public override ReadCE3.RdStatus RdMb3Member(string str1, string str2, ReadCE3 readCE3)
		{
			switch (str1) {
				case "STYLE":
					if (str2 == lineStyleName[(int)MbeLineStyle.Bending1]) {
						lineStyle = MbeLineStyle.Bending1;
					} else if (str2 == lineStyleName[(int)MbeLineStyle.Bending2]) {
						lineStyle = MbeLineStyle.Bending2;
					} else {
						lineStyle = MbeLineStyle.Straight;
					}
					return ReadCE3.RdStatus.NoError;
				case "WIDTH":
					try { LineWidth = Convert.ToInt32(str2); }
					catch (Exception) { LineWidth = DEFAULT_LINE_WIDTH; }
					return ReadCE3.RdStatus.NoError;
				default:
					return base.RdMb3Member(str1, str2, readCE3);
			}
		}


		/// <summary>
		/// このクラスのMb3ファイルの読み込み
		/// </summary>
		/// <param name="readCE3">読み込み対象のReadCE3クラス</param>
		/// <returns>正常終了時にReadCE3.RdStatus.NoError を返す</returns>
		public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
		{
			string str1;
			string str2;
			while (readCE3.GetRecord(out str1, out str2)) {
				if (str1[0] == '-') {
					if (str1 != "-MBE_LINE") {
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
		/// WriteCE3クラスへメンバーの書き込み
		/// </summary>
		/// <param name="writeCE3">書き込み対象WriteCE3クラス</param>
		/// <param name="origin">書き込み時の原点</param>
		/// <returns>正常終了でtrue</returns>
		public override bool WrMb3Member(WriteCE3 writeCE3, Point origin)
		{
			//保存時は互換性のためにLineStyleのBending2を使わない。
			//Bending2のラインのときは始点終点を入れ替えて保存する。

			Point pt0 = this.GetPos(0);
			Point pt1 = this.GetPos(1);

			if (lineStyle == MbeLineStyle.Bending2) {
				SetPos(pt1, 0);
				SetPos(pt0, 1);
			}
			base.WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecordInt("WIDTH", LineWidth);
					
			if (lineStyle == MbeLineStyle.Straight) {
				writeCE3.WriteRecordString("STYLE", lineStyleName[(int)MbeLineStyle.Straight]);
			} else {
				writeCE3.WriteRecordString("STYLE", lineStyleName[(int)MbeLineStyle.Bending1]);
			}
			if (lineStyle == MbeLineStyle.Bending2) {
				SetPos(pt0, 0);
				SetPos(pt1, 1);
			}
			return true;
		}

		/// <summary>
		/// Mb3ファイルへの書き込み
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public override bool WrMb3(WriteCE3 writeCE3, Point origin)
		{
			writeCE3.WriteRecord("+MBE_LINE");
			WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecord("-MBE_LINE");
			writeCE3.WriteNewLine();
			return true;
		}

		/// <summary>
		/// 複製を行う
		/// </summary>
		/// <returns></returns>
		public override MbeObj Duplicate()
		{
			MbeObj newObj = new MbeObjLine(this);
			return newObj;
		}


		/// <summary>
		/// 選択を行う
		/// </summary>
		/// <param name="rc"></param>
		/// <param name="layerMask"></param>
		/// <param name="pointMode"></param>
		/// <returns>選択対象だったときにtrueを返す</returns>
		/// <remarks>
		/// 選択対象だった場合には選択フラグがセットされる。
		/// </remarks>
		public override bool SelectIt(MbeRect rc, ulong layerMask, bool pointMode)
		{
			if ((layerMask & (ulong)layer) == 0) return false;
			if (DeleteCount >= 0) return false;

			bool result = false;

			if (!pointMode) {
				result = base.SelectIt(rc, layerMask, pointMode);
				if (result) return true;
			}else{
				Point ptC = rc.Center();
				
				int threshold = LineWidth / 2;

				if (threshold >= (int)Util.DistancePointPoint(ptC, GetPos(0))) {
					selectFlag[0] = true;
					return true;
				}

				if (threshold >= (int)Util.DistancePointPoint(ptC, GetPos(1))) {
					selectFlag[1] = true;
					return true;
				}

				Point ptVia;
				//Point ptDummy;
				bool bendMode = getPointVia(out ptVia);

				if (bendMode) {
					if (threshold >= (int)Util.DistancePointLine(ptC, GetPos(0), ptVia) ||
						threshold >= (int)Util.DistancePointLine(ptC, GetPos(1), ptVia)) {
						result = true;
					}
				} else {
					if (threshold >= (int)Util.DistancePointLine(ptC, GetPos(0), GetPos(1))) {
						result = true;
					}
				}
				if(result){
					selectFlag[0] = true;
					selectFlag[1] = true;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// ConChkのための種の取得
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
		/// 分割可能かどうかを返す。
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="visibleLayer"></param>
		/// <param name="threshold"></param>
		/// <param name="lineIndex"></param>
		/// <param name="ptDivAt"></param>
		/// <returns></returns>
		public override bool CanDivide(Point pt, ulong visibleLayer, int threshold, out int lineIndex, out Point ptDivAt)
        {

            //if (lineStyle!=MbeLineStyle.Straight || (visibleLayer & (ulong)layer) == 0) {
            if ((visibleLayer & (ulong)layer) == 0) {
                lineIndex = 0;
                ptDivAt = pt;
                return false;
            }

            Point ptVia;
            Point pt0;
            Point pt1;
            if (lineStyle == MbeLineStyle.Straight || !getPointVia(out ptVia)) {
                lineIndex = 0;
                pt0 = this.GetPos(0);
                pt1 = this.GetPos(1);
                return IsNearLine(pt0, pt1, pt, threshold, out ptDivAt);
            }

            //Point ptVia;
            //if(getPointVia(out ptVia)) 
                //Point pt0;
                //Point pt1;

                pt0 = this.GetPos(0);
                pt1 = ptVia;
                if (IsNearLine(pt0, pt1, pt, threshold, out ptDivAt)) {
                    lineIndex = 0;
                    return true;
                }

                pt0 = ptVia;
                pt1 = this.GetPos(1);
                if (IsNearLine(pt0, pt1, pt, threshold, out ptDivAt)) {
                    lineIndex = 1;
                    return true;
                }
            //}

            lineIndex = 0;
            ptDivAt = pt;
            return false;


        }

        ///// <summary>
        ///// 分割。線分系オブジェクトで意味を持つ。
        ///// </summary>
        ///// <param name="lineIndex">分割可能な線のインデックス。単純ラインなら無視。</param>
        ///// <param name="ptDivAt"></param>
        ///// <returns></returns>
        //public override bool DivideAtCenter(int lineIndex, out MbeObj newObj)
        //{
        //    Point pt0 = this.GetPos(0);
        //    Point pt1 = this.GetPos(1);
        //    Point ptDivAt = new Point((pt0.X + pt1.X) / 2, (pt0.Y + pt1.Y) / 2);
        //    newObj = Duplicate();
        //    SetPos(ptDivAt, 1);
        //    newObj.SetPos(ptDivAt, 0);
        //    return true;
        //}

		/// <summary>
		/// 指定点で分割。線分系オブジェクトで意味を持つ。
		/// </summary>
		/// <param name="lineIndex">分割可能な線のインデックス。単純ラインなら無視。</param>
		/// <param name="ptDivAt"></param>
		/// <returns></returns>
		public override bool DivideAtPoint(int lineIndex, Point pt, out MbeObj newObj)
		{
			newObj = Duplicate();
            if (lineIndex == 0) {
                SetPos(pt, 1);
                newObj.SetPos(pt, 0);
            } else {
                SetPos(pt, 0);
                newObj.SetPos(pt, 1);
            }

			return true;
		}


        //public virtual MbePointInfo[] GetSnapPointArray()
        //{

        //    MbePointInfo[] infoArray = new MbePointInfo[posCount];
        //    for (int i = 0; i < posCount; i++) {
        //        MbePointInfo sp;
        //        //sp.layer = snapLayer;
        //        sp.layer = (ulong)layer;
        //        sp.point = GetPos(i);
        //        sp.selectFlag = false;
        //        sp.componentPinObj = null;
        //        infoArray[i] = sp;
        //    }
        //    return infoArray;
        //}

        public override MbePointInfo[] GetSnapPointArrayForMeasure()
        {
            MbePointInfo[] infoArray = GetSnapPointArray();
            Point ptVia;
            if (getPointVia(out ptVia)) {
                MbePointInfo sp;
                sp.layer = (ulong)layer;
                sp.point = ptVia;
                sp.selectFlag = false;
                sp.componentPinObj = null;
                MbePointInfo[] infoArray_2 = new MbePointInfo[3];
                for (int i = 0; i < 2; i++) infoArray_2[i] = infoArray[i];
                infoArray_2[2] = sp;
                return infoArray_2;
            } else {
                return infoArray;
            }
        }


		private bool getPointVia(out Point pt)
		{
			if (lineStyle == MbeLineStyle.Straight) {
				pt = new Point(0, 0);
				return false;
			}
			Point pt0;
			Point pt1;
			if (lineStyle == MbeLineStyle.Bending1) {
				pt0 = this.GetPos(0);
				pt1 = this.GetPos(1);
			} else {// Bending2
				pt0 = this.GetPos(1);
				pt1 = this.GetPos(0);
			}
			int x0 = pt0.X;
			int y0 = pt0.Y;
			int x1 = pt1.X;
			int y1 = pt1.Y;
			int xv;
			int yv;
			int xDiffAbs = Math.Abs(x1-x0);
			int yDiffAbs = Math.Abs(y1-y0);
			if(xDiffAbs == yDiffAbs){
                pt = new Point(0,0);
				return false;
			} else if (xDiffAbs > yDiffAbs) {
				yv = y0;
				if (x1 > x0) {
					xv = x1 - yDiffAbs;
				} else {
					xv = x1 + yDiffAbs;
				}
			} else {
				xv = x0;
				if (y1 > y0) {
					yv = y1 - xDiffAbs;
				} else {
					yv = y1 + xDiffAbs;
				}
			}
			pt = new Point(xv, yv);
			return true;
		}

		/// <summary>
		/// 接続チェックのアクティブ状態の設定
		/// </summary>
		public override void SetConnectCheck()
		{
			connectionCheckActive = true;
		}



		/// <summary>
		/// 描画
		/// </summary>
		/// <param name="dp"></param>
		public override void Draw(DrawParam dp)
		{
			if (dp.layer != Layer) {
				return;
			}
			Point pt0 = this.GetPos(0);
			Point pt1 = this.GetPos(1);
			Point ptVia;

            //両端点が一致していたら描画しない。Version 0.48.02 2009/04/24
            if (pt0.X == pt1.X && pt0.Y == pt1.Y) {
                return;
            }

            bool bendMode = getPointVia(out ptVia);
            //折れ曲がり点が端点と一致していたら、折れ曲がり線ではないと見做す。Version 0.48.02 2009/04/24
            if (bendMode) {
                if ((pt0.X == ptVia.X && pt0.Y == ptVia.Y) || (pt1.X == ptVia.X && pt1.Y == ptVia.Y)) {
                    bendMode = false;
                } else {
                    ptVia = ToDrawDim(ptVia, dp.scale);
                }
            }

            pt0 = ToDrawDim(pt0, dp.scale);
            pt1 = ToDrawDim(pt1, dp.scale);
			
            
			Color col;
			int w = 0;
			Pen pen;


			//描画幅の設定
			w = (int)(LineWidth / dp.scale) | 1;

			if (dp.mode == MbeDrawMode.Print) {
                if ((dp.option & (uint)MbeDrawOption.PrintColor) != 0) {    //カラー印刷
                    uint nColor;
                    switch (dp.layer) {
                        case MbeLayer.LayerValue.PLC:
                            nColor = MbeColors.PRINT_PLC;
                            break;
                        case MbeLayer.LayerValue.PLS:
                            nColor = MbeColors.PRINT_PLS;
                            break;
                        case MbeLayer.LayerValue.MMC:
                            nColor = MbeColors.PRINT_MMC;
                            break;
                        case MbeLayer.LayerValue.MMS:
                            nColor = MbeColors.PRINT_MMS;
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
                    case MbeLayer.LayerValue.MMC:
                        nColor = MbeColors.MMC;
                        break;
                    case MbeLayer.LayerValue.MMS:
                        nColor = MbeColors.MMS;
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

			pen = new Pen(col,w);
			pen.StartCap = LineCap.Round;
			pen.EndCap = LineCap.Round;
			if (!bendMode) {
                //Version 0.48.02 非常に短いラインデータの描画が消えないようにする
                //DrawLine()を直接使わずに、メンバーのDrawLineData()を呼ぶ
                //dp.g.DrawLine(pen, pt0, pt1);
                DrawLineData(dp.g,pen, pt0, pt1);
			} else {
                //Version 0.48.02 非常に短いラインデータの描画が消えないようにする
                //DrawLine()を直接使わずに、メンバーのDrawLineData()を呼ぶ
                //dp.g.DrawLine(pen, pt0, ptVia);
                //dp.g.DrawLine(pen, ptVia, pt1);
                DrawLineData(dp.g, pen, pt0, ptVia);
                DrawLineData(dp.g, pen, ptVia, pt1);

			}
			pen.Dispose();

			if (drawSnapMark && dp.mode != MbeDrawMode.Print) {
				int marksize = w;
				DrawSnapPointMark(dp.g, pt0, marksize, selectFlag[0]);
				DrawSnapPointMark(dp.g, pt1, marksize, selectFlag[1]);
			}
		}


        //Version 0.48.02 非常に短いラインデータの描画が消えないようにする
        //実際には長さがあるのに、短いせいで描画座標に変換した結果、長さがゼロになったものも描画を強制する。
        private void DrawLineData(Graphics g,Pen pen, Point p0, Point p1)
        {
            if (p0.X == p1.X && p0.Y == p1.Y) {
                //長さに1を足して、整数引数のDrawLine()を呼んだら、横長になることがあった。
                //すっきりしないが、float引数のDrawLine()にして、長さを0.5にした。
                g.DrawLine(pen, (float)(p0.X), (float)(p0.Y), (float)(p1.X)+(float)0.5, (float)(p1.Y));
            }
            g.DrawLine(pen, p0, p1);
        }

		
		/// <summary>
		/// ポリゴンのための輪郭データを生成する。
		/// </summary>
		/// <param name="outlineList"></param>
		/// <param name="param"></param>
		/// <remarks>
        /// ここで生成する輪郭データは内側に重なっていても良いものとする
        /// 2009/01/01 ラインアウトラインの端点キャップ省略を止める。常に端点キャップ生成
        /// </remarks>
		public override void GenerateOutlineData(LinkedList<MbeGapChkObjLine> outlineList, GenOutlineParam param)
		{
			if (param.layer != Layer) return;

			Point ptVia;
			bool bendMode = getPointVia(out ptVia);
			Point[] pt = new Point[3];
			int lines;
			if (bendMode) {
				pt[0] = GetPos(0);
				pt[1] = ptVia;
				pt[2] = GetPos(1);
				lines = 2;
			} else {
				pt[0] = GetPos(0);
				pt[1] = GetPos(1);
				lines = 1;
			}

			int distance = (LineWidth + param.traceWidth) / 2 + param.gap*11/10;


			int left = param.rc.L - distance;
			int top = param.rc.T + distance;
			int right = param.rc.R + distance;
			int bottom = param.rc.B - distance;

			MbeRect rcArea = new MbeRect(new Point(left, top), new Point(right, bottom));
			Point[] ptOutline;


			for(int i = 0;i<lines;i++){
				if (Util.LineIsOutsideLTRB(pt[i], pt[i+1], rcArea)) {
					continue;
				}
				bool revBeginEnd;
				Util.LineOutlineData(pt[i], pt[i + 1], distance, out ptOutline, out revBeginEnd);


                bool p0Nocap = false;
                bool p1Nocap = false;
                if (bendMode) {
                    if(i==0){
                        if (revBeginEnd) {
                            p1Nocap = (param.option & GenOutlineParam.P0_NO_LINECAP) != 0;
                        }else{
                            p0Nocap = (param.option & GenOutlineParam.P0_NO_LINECAP) != 0;
                        }
                    }else if(i==1){
                       if (revBeginEnd) {
                            p0Nocap = (param.option & GenOutlineParam.P1_NO_LINECAP) != 0;
                        }else{
                            p1Nocap = (param.option & GenOutlineParam.P1_NO_LINECAP) != 0;
                        }
                    }
                }else{
                    if (revBeginEnd) {
                        p1Nocap = (param.option & GenOutlineParam.P0_NO_LINECAP) != 0;
                        p0Nocap = (param.option & GenOutlineParam.P1_NO_LINECAP) != 0;
                    }else{
                        p0Nocap = (param.option & GenOutlineParam.P0_NO_LINECAP) != 0;
                        p1Nocap = (param.option & GenOutlineParam.P1_NO_LINECAP) != 0;
                    }
                }

                


				for (int j = 0; j < 8; j++) {
					int j2 = j + 1;
					if (j2 == 8) {
						j2 = 0;
					}

					//不要なときはラインキャップを生成しない
                    //if (bendMode) {
                    //    if (p0Nocap && i == 0) {
                    //        if (j >= 5 && j <= 7) continue;
                    //    }
                    //    if (p1Nocap && i == 1) {
                    //        if (j >= 1 && j <= 3) continue;
                    //    }
                    //} else {
						if (p0Nocap) {
							//System.Diagnostics.Debug.WriteLine("Skip begin cap");
							if (j >= 5 && j <= 7) continue;
						}
						if (p1Nocap) {
							//System.Diagnostics.Debug.WriteLine("Skip end cap");
							if (j >= 1 && j <= 3) continue;
						}
                    //}


					if (!Util.LineIsOutsideLTRB(ptOutline[j], ptOutline[j2], param.rc)) {
						MbeGapChkObjLine objLine = new MbeGapChkObjLine();
						objLine.SetLineValue(ptOutline[j], ptOutline[j2], param.traceWidth);
						outlineList.AddLast(objLine);
					}
				}
			}
		}


		/// <summary>
		/// CAMデータの生成 
		/// </summary>
		/// <param name="camOut"></param>
		public override void GenerateCamData(CamOut camOut)
		{
			Point pt0 = GetPos(0);
			Point pt1 = GetPos(1);
			Point ptVia;
			bool bendMode = getPointVia(out ptVia);

			CamOutBaseData camd;
			if (!bendMode) {
				camd = new CamOutBaseData(layer,
										  CamOutBaseData.CamType.VECTOR,
										  CamOutBaseData.Shape.Obround,
										  lineWidth, lineWidth, pt0, pt1);
				camOut.Add(camd);
			} else {
				camd = new CamOutBaseData(layer,
										  CamOutBaseData.CamType.VECTOR,
										  CamOutBaseData.Shape.Obround,
										  lineWidth, lineWidth, pt0, ptVia);
				camOut.Add(camd);
				camd = new CamOutBaseData(layer,
										  CamOutBaseData.CamType.VECTOR,
										  CamOutBaseData.Shape.Obround,
										  lineWidth, lineWidth, ptVia, pt1);
				camOut.Add(camd);
			}
		}

		public override void GenerateGapChkData(LinkedList<MbeGapChkObj> chkObjList,int _netNum)
		//public override void GenerateGapChkData(MbeGapChk gapChk, int _netNum)
		{
			if (layer != MbeLayer.LayerValue.CMP &&
                layer != MbeLayer.LayerValue.L2 &&
                layer != MbeLayer.LayerValue.L3 &&
                layer != MbeLayer.LayerValue.SOL) return;

			Point pt0 = GetPos(0);
			Point pt1 = GetPos(1);

			if (pt0.Equals(pt1)) {
				MbeGapChkObjPoint gapChkObj = new MbeGapChkObjPoint();
				gapChkObj.layer = layer;
				gapChkObj.netNum = _netNum;
				gapChkObj.mbeObj = this;
				gapChkObj.SetPointValue(pt0, LineWidth);
				//gapChk.Add(gapChkObj);
				chkObjList.AddLast(gapChkObj);
			} else {
				Point ptVia;
				bool bendMode = getPointVia(out ptVia);

				if (bendMode) {
					MbeGapChkObjLine gapChkObj = new MbeGapChkObjLine();
					gapChkObj.layer = layer;
					gapChkObj.netNum = _netNum;
					gapChkObj.mbeObj = this;
					gapChkObj.SetLineValue(pt0, ptVia, LineWidth);
					//gapChk.Add(gapChkObj);
					chkObjList.AddLast(gapChkObj);

					gapChkObj = new MbeGapChkObjLine();
					gapChkObj.layer = layer;
					gapChkObj.netNum = _netNum;
					gapChkObj.mbeObj = this;
					gapChkObj.SetLineValue(ptVia, pt1, LineWidth);
					//gapChk.Add(gapChkObj);
					chkObjList.AddLast(gapChkObj);
				} else {
					MbeGapChkObjLine gapChkObj = new MbeGapChkObjLine();
					gapChkObj.layer = layer;
					gapChkObj.netNum = _netNum;
					gapChkObj.mbeObj = this;
					gapChkObj.SetLineValue(pt0, pt1, LineWidth);
					//gapChk.Add(gapChkObj);
					chkObjList.AddLast(gapChkObj);
				}
			}
		}

		protected int lineWidth;
		protected MbeLineStyle lineStyle; 
	}
}
