using System;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

using CE3IO;

namespace mbe
{
	class MbeObjPinSMD : MbeObjPin
	{


		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MbeObjPinSMD()
		{
            constructedAsPad = true;
			Layer = MbeLayer.LayerValue.CMP;
			padSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
			shape = PadShape.Rect;
            mmreduce = DEFAULT_MMREDUCE;
            no_MM = false;
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MbeObjPinSMD(bool bPad)
        {
            constructedAsPad = bPad;
            if (constructedAsPad) {
                Layer = MbeLayer.LayerValue.CMP;
            } else {
                Layer = MbeLayer.LayerValue.PLC;
            }
            padSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
            shape = PadShape.Rect;
            mmreduce = DEFAULT_MMREDUCE;
            no_MM = false;
            //no_ResistMask = false;
        }


        /// <summary>
        /// メタルマスクの縮小量初期値
        /// </summary>
        protected const int DEFAULT_MMREDUCE = 0; //0mm

        /// <summary>
        /// メタルマスクの縮小量最大値
        /// </summary>
        protected const int MAX_MMREDUCE = 2000; //0.2mm

        /// <summary>
        /// メタルマスクの縮小量最小値
        /// </summary>
        protected const int MIN_MMREDUCE = 0; //0mm


        private bool constructedAsPad;
        protected int mmreduce;
        private bool no_MM;   //メタルマスク穴を開けない

        public bool No_MM
        {
            get { return no_MM; }
            set { no_MM = value; }
        }

        //private bool no_ResistMask;    //レジストを開けない
        //public bool No_ResistMask
        //{
        //    get { return no_ResistMask; }
        //    set { no_ResistMask = value; }
        //}


		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="mbeObjPin"></param>
		public MbeObjPinSMD(MbeObjPinSMD mbeObjPinSmd)
			: base(mbeObjPinSmd)
		{
            mmreduce = mbeObjPinSmd.mmreduce;
            no_MM = mbeObjPinSmd.no_MM;
            //no_ResistMask = mbeObjPinSmd.no_ResistMask;
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
                // >>> 2008/4/20 PADをシルクに配置可能にする
                if (value == MbeLayer.LayerValue.PLC || value == MbeLayer.LayerValue.PLS) {
                    layer = value;
                    snapLayer = (ulong)value;
                    return;
                }
                // <<< 2008/4/20 PADをシルクに配置可能にする

				if (MbeLayer.IsComponentSide(value)) {
					layer = MbeLayer.LayerValue.CMP;
					snapLayer =
                            //(ulong)MbeLayer.LayerValue.PTH |      //2010/01/06コメントアウト
							(ulong)MbeLayer.LayerValue.CMP |
                            (ulong)MbeLayer.LayerValue.MMC |
                            (ulong)MbeLayer.LayerValue.STC;

				} else {
					layer = MbeLayer.LayerValue.SOL;
					snapLayer =
                        //(ulong)MbeLayer.LayerValue.PTH |      //2010/01/06コメントアウト
							(ulong)MbeLayer.LayerValue.SOL |
                            (ulong)MbeLayer.LayerValue.MMS |
                            (ulong)MbeLayer.LayerValue.STS;
				}
			}
		}

		/// <summary>
		/// 配置指定可能なレイヤー
		/// </summary>
		/// <returns></returns>
		public static ulong SelectableLayer()
		{
			return
                // >>> 2008/4/20 PADをシルクに配置可能にする
                    (ulong)MbeLayer.LayerValue.PLC |
                    (ulong)MbeLayer.LayerValue.PLS |
                // <<< 2008/4/20 PADをシルクに配置可能にする
                    (ulong)MbeLayer.LayerValue.CMP |
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
        ///  メタルマスク縮小量の設定
        /// </summary>
        public int MmReduce
        {
            get { return mmreduce; }
            set
            {
                mmreduce = value;
                if (mmreduce < MIN_MMREDUCE) mmreduce = MIN_MMREDUCE;
                else if (mmreduce > MAX_MMREDUCE) mmreduce = MAX_MMREDUCE;
            }
        }



		/// <summary>
		/// 図面オブジェクトごとのID値を返す
		/// </summary>
		public override MbeObjID Id()
		{
			return MbeObjID.MbePinSMD;
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
            int n;
            switch (str1) {
                //case "MMREDUCE":
                //    try { MmReduce = Convert.ToInt32(str2); }
                //    catch (Exception) { MmReduce = DEFAULT_MMREDUCE; }
                //    return ReadCE3.RdStatus.NoError;
                //case "NO_RM":
                //    try {
                //        n = Convert.ToInt32(str2);
                //        no_ResistMask = (n != 0);
                //    }
                //    catch (Exception) { no_ResistMask = false; }
                //    return ReadCE3.RdStatus.NoError;
                case "NO_MM":
                    try {
                        n = Convert.ToInt32(str2);
                        no_MM = (n != 0);
                    }
                    catch (Exception) { no_MM = false; }
                    return ReadCE3.RdStatus.NoError;
                default:
                    return base.RdMb3Member(str1, str2, readCE3);
            }
            //return true;
        }



		/// <summary>
		/// このクラスのMb3ファイルの読み込み
		/// </summary>
		/// <param name="readCE3">読み込み対象のReadCE3クラス</param>
		/// <returns>正常終了時に ReadCE3.RdStatus.NoErrorを返す</returns>
		public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
		{
			string str1;
			string str2;
			while (readCE3.GetRecord(out str1, out str2)) {
				if (str1[0] == '-'){
                    if (str1 != "-MBE_PINSMD" && str1 != "-MBE_FLASHMARK") {
						return ReadCE3.RdStatus.FormatError;
					} else {
                        if (no_ResistMask) {
                            no_MM = true;
                        }
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
            base.WrMb3Member(writeCE3, origin);
            if (no_MM || no_ResistMask) {
                writeCE3.WriteRecordInt("NO_MM", 1);
            }
            //if (no_ResistMask) {
            //    writeCE3.WriteRecordInt("NO_RM", 1);
            //}
            //if (layer == MbeLayer.LayerValue.CMP || layer == MbeLayer.LayerValue.SOL) {
            //    writeCE3.WriteRecordInt("MMREDUCE", MmReduce);
            //}
            return true;
        }


		/// <summary>
		/// Mb3ファイルへの書き込み
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public override bool WrMb3(WriteCE3 writeCE3, Point origin)
		{
            string tagString;
            if (layer == MbeLayer.LayerValue.CMP || layer == MbeLayer.LayerValue.SOL) {
                tagString = "MBE_PINSMD";
            } else {
                tagString = "MBE_FLASHMARK";
            }
			writeCE3.WriteRecord("+"+tagString);
			WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecord("-"+tagString);
			writeCE3.WriteNewLine();
			return true;
		}

		/// <summary>
		/// 複製を行う
		/// </summary>
		/// <returns></returns>
		public override MbeObj Duplicate()
		{
			MbeObj newObj = new MbeObjPinSMD(this);
			return newObj;
		}

		public override void Draw(DrawParam dp)
		{
			if (((ulong)dp.layer & snapLayer) == 0) return;

            if (((ulong)dp.layer & ((ulong)MbeLayer.LayerValue.STS |
                        (ulong)MbeLayer.LayerValue.STC)) != 0) {
                if (no_ResistMask) return;
            }


			Point pt = this.GetPos(0);
			pt = ToDrawDim(pt, dp.scale);

			Color col;
			//Color cold;
			int w = 0;
			int h = 0;
			//int wd = 0;
			//int hd = 0;
			Rectangle rc;
			SolidBrush brush;
			Pen pen;


			#region 描画サイズの設定

			switch (dp.layer) {
				case MbeLayer.LayerValue.PLC:
                case MbeLayer.LayerValue.PLS:
                case MbeLayer.LayerValue.CMP:
                case MbeLayer.LayerValue.SOL:
					w = (int)(PadSize.Width / dp.scale) | 1;
					h = (int)(PadSize.Height / dp.scale) | 1;
					break;
				case MbeLayer.LayerValue.STC:
				case MbeLayer.LayerValue.STS:
                    //if (no_ResistMask) return;  //Version 0.50
					w = (int)((PadSize.Width + SrMargin * 2) / dp.scale) | 1;
					h = (int)((PadSize.Height + SrMargin * 2) / dp.scale) | 1;
					break;
                case MbeLayer.LayerValue.MMC:
                case MbeLayer.LayerValue.MMS:
                    if (no_MM) return;
                    w = (int)((PadSize.Width - MmReduce * 2) / dp.scale) | 1;
                    h = (int)((PadSize.Height - MmReduce * 2) / dp.scale) | 1;
                    break;

			}
			if (w == 1) w++;
			if (h == 1) h++;
			//if ((dp.layer == MbeLayer.LayerValue.PTH) || (dp.layer == MbeLayer.LayerValue.DRL)) {
			//    wd = (int)(Diameter / dp.scale) | 1;

			//    if (dp.layer == MbeLayer.LayerValue.PTH) {
			//        if (wd >= w) wd = w - 2;
			//        if (wd >= h) wd = h - 2;
			//    }
			//    hd = wd;
			//}





			#endregion

			#region 色の設定
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
                        case MbeLayer.LayerValue.CMP:
                            nColor = MbeColors.PRINT_CMP;
                            break;
                        case MbeLayer.LayerValue.SOL:
                            nColor = MbeColors.PRINT_SOL;
                            break;
                        case MbeLayer.LayerValue.STC:
                            nColor = MbeColors.PRINT_STC;
                            break;
                        case MbeLayer.LayerValue.MMC:
                            nColor = MbeColors.PRINT_MMC;
                            break;
                        case MbeLayer.LayerValue.MMS:
                            nColor = MbeColors.PRINT_MMS;
                            break;
                        default:
                            nColor = MbeColors.PRINT_STS;
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
                    case MbeLayer.LayerValue.CMP:
						nColor = MbeColors.CMP;
						break;
					case MbeLayer.LayerValue.SOL:
						nColor = MbeColors.SOL;
						break;
					case MbeLayer.LayerValue.STC:
						nColor = MbeColors.STC;
						break;
                    case MbeLayer.LayerValue.MMC:
                        nColor = MbeColors.MMC;
                        break;
                    case MbeLayer.LayerValue.MMS:
                        nColor = MbeColors.MMS;
                        break;
					//case MbeLayer.LayerValue.STS:
					default:
						nColor = MbeColors.STS;
						break;
					//	break;
					//case MbeLayer.LayerValue.DRL:
					//default:
					//    nColor = MbeColors.Drl;
					//    break;
				}

				if (dp.mode == MbeDrawMode.Temp || connectionCheckActive) {
					col = Color.FromArgb(MBE_OBJ_ALPHA, MbeColors.HighLighten(nColor));
				} else {
					col = Color.FromArgb(unchecked((int)nColor));
					col = Color.FromArgb(MBE_OBJ_ALPHA, col);
				}
			}
			#endregion

			#region 実際の描画の実行
			switch (dp.layer) {
                case MbeLayer.LayerValue.PLC:
                case MbeLayer.LayerValue.PLS:
                case MbeLayer.LayerValue.CMP:
				case MbeLayer.LayerValue.SOL:
				case MbeLayer.LayerValue.STC:
				case MbeLayer.LayerValue.STS:
                case MbeLayer.LayerValue.MMC:
                case MbeLayer.LayerValue.MMS:
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

                    if (dp.layer == MbeLayer.LayerValue.PLC ||
                        dp.layer == MbeLayer.LayerValue.PLS || 
                        dp.layer == MbeLayer.LayerValue.CMP || 
                        dp.layer == MbeLayer.LayerValue.SOL) {
						//if (wd >= 3) {
						//    rc = new Rectangle(pt.X - wd / 2, pt.Y - hd / 2, wd, hd);
						//    brush = new SolidBrush(MbeColors.ColorBackground);
						//    dp.g.FillEllipse(brush, rc);
						//    brush.Dispose();
						//}
						//if (DrawSnapMarkFlag) {
						if (dp.mode != MbeDrawMode.Print) {
							if (drawSnapMark) {
								int marksize = (w < h ? w : h) / 2;
								DrawSnapPointMark(dp.g, pt, marksize, selectFlag[0]);
								//}
                                if (dp.layer == MbeLayer.LayerValue.CMP ||
                                    dp.layer == MbeLayer.LayerValue.SOL) {
                                    if (DrawPinNumFlag) {
                                        int fontsize = (w < h ? w : h) / 2;	//高さと幅の小さい方
                                        bool vertical = (w < h);			//幅が大きければ縦書き
                                        DrawPinNum(dp.g, PinNum, pt, fontsize, vertical);
                                    }
                                }
							}
						}
					}
					break;
				//case MbeLayer.LayerValue.DRL:
				//    rc = new Rectangle(pt.X - wd / 2, pt.Y - hd / 2, wd, hd);
				//    pen = new Pen(col);
				//    dp.g.DrawEllipse(pen, rc);
				//    pen.Dispose();
				//    break;
			}
			#endregion



		}

		/// <summary>
		/// CAMデータの生成 
		/// </summary>
		/// <param name="camOut"></param>
		public override void GenerateCamData(CamOut camOut)
		{
			int width = PadSize.Width;
			int height = PadSize.Height;

            
			Point pt0 = GetPos(0);
			Point pt1 = GetPos(0);
            CamOutBaseData camd;
			CamOutBaseData.Shape _shape = (shape == PadShape.Rect ? CamOutBaseData.Shape.Rect : CamOutBaseData.Shape.Obround);


			camd = new CamOutBaseData(layer, CamOutBaseData.CamType.FLASH,
									_shape, width, height, pt0, pt1);
			camOut.Add(camd);


            if (layer == MbeLayer.LayerValue.CMP || layer == MbeLayer.LayerValue.SOL) {
                if (!no_ResistMask) {   //Version 0.50
                    int stWidth = PadSize.Width + SrMargin * 2;
                    int stHeight = PadSize.Height + SrMargin * 2;
                    MbeLayer.LayerValue stLayer = (layer == MbeLayer.LayerValue.CMP ? MbeLayer.LayerValue.STC : MbeLayer.LayerValue.STS);
                    camd = new CamOutBaseData(stLayer, CamOutBaseData.CamType.FLASH,
                                            _shape, stWidth, stHeight, pt0, pt1);
                    camOut.Add(camd);
                }

                if (!no_MM) {
                    int stWidth = PadSize.Width - MmReduce * 2;
                    int stHeight = PadSize.Height - MmReduce * 2;
                    MbeLayer.LayerValue stLayer = (layer == MbeLayer.LayerValue.CMP ? MbeLayer.LayerValue.MMC : MbeLayer.LayerValue.MMS);
                    camd = new CamOutBaseData(stLayer, CamOutBaseData.CamType.FLASH,
                                            _shape, stWidth, stHeight, pt0, pt1);
                    camOut.Add(camd);
                }
            
            }

		}


		/// <summary>
		/// ポリゴンのための輪郭データを生成する。
		/// </summary>
		/// <param name="outlineList"></param>
		/// <param name="param"></param>
		/// <remarks>ここで生成する輪郭データは内側に重なっていても良いものとする</remarks>
		public override void GenerateOutlineData(LinkedList<MbeGapChkObjLine> outlineList, GenOutlineParam param)
		{
			if (param.layer != Layer) return;
			base.GenerateOutlineData(outlineList, param);
		}

		public override void GenerateGapChkData(LinkedList<MbeGapChkObj> chkObjList,int _netNum)
		//public override void GenerateGapChkData(MbeGapChk gapChk,int _netNum)
		{
            if (layer != MbeLayer.LayerValue.CMP && layer != MbeLayer.LayerValue.SOL) return;

			if (shape == PadShape.Rect) {
				MbeGapChkObjRect gapChkObj = new MbeGapChkObjRect();
				gapChkObj.layer = layer;
				gapChkObj.netNum = _netNum;
				gapChkObj.mbeObj = this;
				gapChkObj.SetRectValue(GetPos(0), PadSize.Width, PadSize.Height);
				//gapChk.Add(gapChkObj);
				chkObjList.AddLast(gapChkObj);
			} else {
				if (PadSize.Width == PadSize.Height) {
					MbeGapChkObjPoint gapChkObj = new MbeGapChkObjPoint();
					gapChkObj.layer = layer;
					gapChkObj.netNum = _netNum;
					gapChkObj.mbeObj = this;
					gapChkObj.SetPointValue(GetPos(0),PadSize.Width);
					//gapChk.Add(gapChkObj);
					chkObjList.AddLast(gapChkObj);
				} else {
					MbeGapChkObjLine gapChkObj = new MbeGapChkObjLine();
					gapChkObj.layer = layer;
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
