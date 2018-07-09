using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using CE3IO;
using System.Windows.Forms;


namespace mbe
{
	class MbeDoc
	{
        /// <summary>
        /// ワークエリアの設定可能な最大幅(0.0001mm単位)
        /// </summary>
        public const int WORK_AREA_MAX_WIDTH = 10000000;

        /// <summary>
        /// ワークエリアのデフォルトの幅(0.0001mm単位)
        /// </summary>
        public const int WORK_AREA_DEFAULT_WIDTH = 3000000;

        /// <summary>
        /// ワークエリアの設定可能な最小幅(0.0001mm単位)
        /// </summary>
        public const int WORK_AREA_MIN_WIDTH = 500000;

        /// <summary>
        ///ワークエリアの設定可能な最大高(0.0001mm単位) 
        /// </summary>
        public const int WORK_AREA_MAX_HEIGHT = 10000000;

        /// <summary>
        /// ワークエリアのデフォルトの高さ(0.0001mm単位)
        /// </summary>
        public const int WORK_AREA_DEFAULT_HEIGHT = 3000000;

        /// <summary>
        ///ワークエリアの設定可能な最小高(0.0001mm単位) 
        /// </summary>
        public const int WORK_AREA_MIN_HEIGHT = 500000;




		/// <summary>
		/// メインリスト
		/// </summary>
		protected LinkedList<MbeObj> mainList;

		/// <summary>
		/// 一時オブジェクトリスト
		/// </summary>
		protected LinkedList<MbeObj> tempList;


		protected Stack<RedoInfo> redoStack;

		protected LinkedListNode<MbeObj> pointSelectNode;

		/// <summary>
		/// 操作カウント
		/// </summary>
		protected int opCount;

		/// <summary>
		/// UNDOを始めたときのopCountを保持する
		/// </summary>
		protected int redoActive;

		/// <summary>
		/// ポリゴンを最後にアップデートしたときの操作カウント
		/// </summary>
		protected int polygonLastUpdate;

		/// <summary>
		/// ドキュメントが要保存のときtrue
		/// </summary>
		protected bool docModified;

		public bool DocModified
		{
			get { return docModified; }
		}

		/// <summary>
		/// 一時オブジェクトが要保存のときtrue
		/// </summary>
		protected bool tempModified;

		/// <summary>
		/// ドキュメントのパス
		/// </summary>
		protected string docPath;

		public MbeDocInfo docInfo;

        protected DataFormats.Format mbeClipboardFormat;

		protected CamOut camOut;

		protected LinkedList<Point> ptLListGapChkResult;

		public LinkedList<Point> GapChkResult
		{
			get { return ptLListGapChkResult; }
		}

        private bool useTextClipboard;

        public bool UseTextClipboard
        {
            get { return useTextClipboard; }
            set { 
                useTextClipboard = value;
                if (useTextClipboard) {
                    mbeClipboardFormat = DataFormats.GetFormat(DataFormats.UnicodeText);
                } else {
                    mbeClipboardFormat = DataFormats.GetFormat("MbeClipboard-v1.0");
                }
                Properties.Settings.Default.ForceClipboardTextFormat = (useTextClipboard ? 1 : 0);
            }
        }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MbeDoc()
		{
			mainList = new LinkedList<MbeObj>();
			tempList = new LinkedList<MbeObj>();
			redoStack = new Stack<RedoInfo>();

			pointSelectNode = null;
			opCount = 1;
			docModified = false;
			tempModified = false;
			docPath = "";
			docInfo = new MbeDocInfo();

            bool bUseTextClipboard = false;

#if MONO
            bUseTextClipboard = true;
			//mbeClipboardFormat = DataFormats.GetFormat(DataFormats.UnicodeText);
#else
            bUseTextClipboard = (Properties.Settings.Default.ForceClipboardTextFormat > 0 ? true : false);
            
#endif

            UseTextClipboard = bUseTextClipboard;


			camOut = new CamOut();
			ptLListGapChkResult = new LinkedList<Point>();
			redoActive = -1;
		}

		/// <summary>
		/// 全クリア
		/// </summary>
		public void ClearAll()
		{
			mainList.Clear();
			tempList.Clear();
			pointSelectNode = null;
			opCount = 1;
			redoActive = -1;
			polygonLastUpdate = 0;
			docModified = false;
			tempModified = false;
			docPath = "";
			ptLListGapChkResult.Clear();
            docInfo.SizeWorkArea = new Size(MbeDoc.WORK_AREA_DEFAULT_WIDTH, MbeDoc.WORK_AREA_DEFAULT_HEIGHT);
		}

		/// <summary>
		/// メインリストの取得
		/// </summary>
		public LinkedList<MbeObj> MainList
		{
			get
			{
				return mainList;
			}
		}

		/// <summary>
		/// 一時オブジェクトリストの取得
		/// </summary>
		public LinkedList<MbeObj> TempList
		{
			get
			{
				return tempList;
			}
		}

		/// <summary>
		/// 操作カウントの取得
		/// </summary>
		public int OpCount
		{
			get
			{
				return opCount;
			}
		}

		/// <summary>
		/// 一時データが要保存かどうかの設定と取得
		/// </summary>
		public bool TempModified
		{
			get
			{
				return tempModified;
			}
			set
			{
				tempModified = value;
			}
		}


		/// <summary>
		/// 一時オブジェクトリストに図面要素を追加する
		/// </summary>
		/// <param name="obj"></param>
		public void AddToTemp(MbeObj obj)
		{
			tempList.AddLast(obj);
			tempModified = true;
		}


		/// <summary>
		/// メインリストリストに図面要素を追加する
		/// </summary>
		/// <param name="obj"></param>
		public void AddToMain(MbeObj obj)
		{
			ReleaseTemp();
			obj.AddCount = opCount;
			mainList.AddLast(obj);

			pointSelectNode = null;
			opCount++;	//メインリストに直接追加されることによるopCountのインクリメント
			System.Diagnostics.Debug.WriteLine("MbeDoc.AddToMain() opCount=" + opCount);
			DiscardRedo();
			docModified = true;
		}




		/// <summary>
		/// 一時オブジェクトリストを解放する
		/// </summary>
		public void ReleaseTemp()
		{
			if (tempList.Count == 0) {
				tempModified = false;
				return;
			}
			//一時データが要保存でなければ、メインデータの現在のopCountのdeleteCountを消去する。
			if (tempModified == false) {
				foreach (MbeObj obj in mainList) {
					if (obj.DeleteCount == opCount) {
						obj.ClearDeleteCount();
					}
				}

			} else {//要保存であれば、
				foreach (MbeObj obj in tempList) {
					if (obj.IsValid()) {
						obj.AddCount = opCount;
						obj.ClearAllSelectFlag();
						mainList.AddLast(obj);
					}
				}
				pointSelectNode = null;
				opCount++;	//一時オブジェクト解放によるopCountのインクリメント
				DiscardRedo();
				System.Diagnostics.Debug.WriteLine("MbeDoc.ReleaseTemp() opCount=" + opCount);
				docModified = true;
			}
			tempList.Clear();
			tempModified = false;
		}

		/// <summary>
		/// 点指定で選択 
		/// </summary>
		/// <param name="pt">選択ポイント</param>
		/// <param name="threshold">この値以下の範囲が選択対象</param>
		/// <param name="visibleLayer">可視レイヤー</param>
		/// <returns></returns>
		/// <remarks>ひとつだけ選択する</remarks>
		public bool SelectByPoint(Point pt, int threshold, ulong visibleLayer, bool append)
		{
			if (!append) {
				ReleaseTemp();
			}
			Point ptLT = new Point(pt.X - threshold, pt.Y + threshold);
			Point ptRB = new Point(pt.X + threshold, pt.Y - threshold);
			MbeRect rc = new MbeRect(ptLT, ptRB);

			if (mainList.Count == 0) return false;

			if (pointSelectNode == null || pointSelectNode.List != mainList) {
				pointSelectNode = mainList.First;
			} else {
				pointSelectNode = pointSelectNode.Next;
				if (pointSelectNode == null) {
					pointSelectNode = mainList.First;
				}
			}

			for(int i = 0;i<mainList.Count;i++){
				MbeObj obj = pointSelectNode.Value;
				if (obj.DeleteCount < 0) {
					if (obj.SelectIt(rc, visibleLayer, true)) {
						MbeObj tempObj = obj.Duplicate();
						obj.ClearAllSelectFlag();
						obj.DeleteCount = opCount;
						tempList.AddLast(tempObj);
						DiscardRedo();
						return true;
					}
				}
				pointSelectNode = pointSelectNode.Next;
				if (pointSelectNode == null) {
					pointSelectNode = mainList.First;
				}
			}

			//追加選択では、すでに選択済みのオブジェクトの未選択ポイントも対象とする。
			if (append) {
				foreach (MbeObj obj in tempList) {
					if (obj.SelectIt(rc, visibleLayer,true)) return true;
				}
			}

			return false;
		}


		/// <summary>
		/// 範囲を指定して選択
		/// </summary>
		/// <param name="pt1">選択範囲を示す端の点</param>
		/// <param name="pt2">選択範囲を示す端の点</param>
		/// <param name="visibleLayer">可視レイヤー</param>
		/// <param name="append">追加選択のときtrue(未検証)</param>
		/// <returns></returns>
		public bool Select(Point pt1, Point pt2, ulong visibleLayer,bool append)
		{
			bool selFlag = false;
			MbeRect rc = new MbeRect(pt1, pt2);
			if(!append){
				ReleaseTemp();
			}
			foreach (MbeObj obj in mainList) {
				if (obj.DeleteCount < 0) {
					if(obj.SelectIt(rc,visibleLayer,false)){
						MbeObj tempObj = obj.Duplicate();
						obj.ClearAllSelectFlag();
						obj.DeleteCount = opCount;
						tempList.AddLast(tempObj);
						selFlag = true;
					}
				}
			}
			//追加選択では、すでに選択済みのオブジェクトの未選択ポイントも対象とする。
			if (append) {
				foreach (MbeObj obj in tempList) {
					if (obj.SelectIt(rc, visibleLayer, false)) {
						selFlag = true;
					}
				}
			}
			if (selFlag) {
				DiscardRedo();
			}
			return (tempList.Count > 0);
		}

		/// <summary>
		/// 一時データを移動する
		/// </summary>
		/// <param name="selectedOnly">選択ポイントだけ移動するときtrue</param>
		/// <param name="offset">移動先の位置</param>
		public void MoveTemp(bool selectedOnly, Point offset,Point ptAbs)
		{
			if (offset.X == 0 && offset.Y == 0) return;
			bool moveSingle = (tempList.Count == 1);	//Arcの端点移動のときだけ有効
		    foreach (MbeObj obj in tempList) {
				obj.Move(selectedOnly, offset, ptAbs, moveSingle);
			}
			tempModified = true;
		}


        public void MoveTemp(int x,int y)
        {
            if (x == 0 && y == 0) return;
            Point offset = new Point(x, y);
            Point ptAbs = new Point(0, 0);
            foreach (MbeObj obj in tempList) {
                obj.Move(true, offset, ptAbs, false);
            }
            tempModified = true;
        }



		/// <summary>
		/// 二つの点のXまたはYの距離のうち、長い方を返す。
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		protected int PointDist(Point pt1,Point pt2)
		{
			int xd = Math.Abs(pt1.X-pt2.X);
			int yd = Math.Abs(pt1.Y-pt2.Y);
			return (xd>yd ? xd : yd);
		}


        public enum GetNearbySnapPointOption : ulong
        {
            None = 0,
            Measure = 0x1L
        }

		/// <summary>
		/// threshold以下の距離にある最も近いsnapポイントを得る
		/// </summary>
		/// <param name="pt">探索する位置</param>
		/// <param name="visibleLayer">可視レイヤー</param>
		/// <param name="threshold">この値以下の距離のときだけ探索対象になる</param>
		/// <param name="ptOut">ヒットして得られたポイント</param>
		/// <returns></returns>
        public bool GetNearbySnapPoint(Point pt, ulong layerMask, int threshold, out Point ptOut, out string pointProperty, GetNearbySnapPointOption option)
		{
			ptOut = pt;
			int dist = threshold + 100;	//100は適当に決めた値
            pointProperty = "";

			foreach (MbeObj obj in mainList) {
				if (obj.DeleteCount < 0) {
                    MbePointInfo[] pinfoArray;
                    if ((option & GetNearbySnapPointOption.Measure) != 0) {
                        pinfoArray = obj.GetSnapPointArrayForMeasure();
                    } else {
                        pinfoArray = obj.GetSnapPointArray();
                    }
                    int n = pinfoArray.Length;
					for (int i = 0; i < n; i++) {
						MbePointInfo pinfo = pinfoArray[i];
						if ((pinfo.layer & layerMask) != 0) {
							int d = PointDist(pt, pinfo.point);
                            if (d <= dist) {
                                if (obj.Id() == MbeObjID.MbeComponent && pinfo.componentPinObj != null) {
                                    pointProperty = ((MbeObjComponent)obj).RefNumText + "." + ((MbeObjPin)(pinfo.componentPinObj)).PinNum;
                                } else if (d < dist) {
                                    pointProperty = "";
                                }
                                dist = d;
								ptOut = pinfo.point;
							}
						}
					}
				}
			}

			return (dist <= threshold);
		}




		/// <summary>
		/// threshold以下の距離にある最も近いアクティブポイントを得る
		/// </summary>
		/// <param name="pt">探索する位置</param>
		/// <param name="visibleLayer">可視レイヤー</param>
		/// <param name="threshold">この値以下の距離のときだけ探索対象になる</param>
		/// <param name="ptOut">ヒットして得られたポイント</param>
		/// <returns></returns>
		public bool GetNearbyActivePoint(Point pt,ulong visibleLayer, int threshold, out Point ptOut,out string pointProperty)
		{
			ptOut = pt;
			int dist = threshold + 100;	//100は適当に決めた値
            pointProperty = "";

			foreach (MbeObj obj in tempList) {
				MbePointInfo[] pinfoArray = obj.GetPosInfoArray();
				int n = pinfoArray.Length;
				for (int i = 0; i < n; i++) {
					MbePointInfo pinfo = pinfoArray[i];
					if(pinfo.selectFlag){
						if ((pinfo.layer & visibleLayer) != 0) {
							int d = PointDist(pt, pinfo.point);

                            if (d <= dist) {
                                if (obj.Id() == MbeObjID.MbeComponent && pinfo.componentPinObj!=null) {
                                    pointProperty = ((MbeObjComponent)obj).RefNumText + "." + ((MbeObjPin)(pinfo.componentPinObj)).PinNum;
                                } else if (d < dist) {
                                    pointProperty = "";
                                }

                                //if (d < dist || pinfo.componentPinObj.Length > 0) {
                                //    pointProperty = pinfo.componentPinObj;
                                //}
                                dist = d;
                                ptOut = pinfo.point;
                            }
						}
					}
				}
			}
			return (dist <= threshold);
		}

		/// <summary>
		/// 一時データに分割可能なオブジェクトがあるかどうか
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="visibleLayer"></param>
		/// <param name="threshold"></param>
		/// <param name="obj"></param>
		/// <param name="lineIndex"></param>
		/// <param name="ptDivAt"></param>
		/// <returns></returns>
		public bool CanDivide(Point pt,ulong visibleLayer, int threshold, out MbeObj outObj, out int lineIndex, out Point ptDivAt)
		{
			foreach (MbeObj obj in tempList) {
				if (obj.CanDivide(pt, visibleLayer,threshold, out lineIndex, out ptDivAt)) {
					outObj = obj;
					return true;
				}
			}
			outObj = null;
			lineIndex = 0;
			ptDivAt = pt;
			return false;
		}


		/// <summary>
		/// 線の分割
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="lineIndex"></param>
		/// <returns></returns>
		public bool DivideLine(MbeObj obj, int lineIndex,Point divideAt)
		{
			foreach (MbeObj objTemp in tempList) {
				if (obj == objTemp) {
					MbeObj newObj;
					//if (!obj.DivideAtCenter(lineIndex, out newObj)) {
					//    return false;
					//}
					if (!obj.DivideAtPoint(lineIndex, divideAt,out newObj)) {
						return false;
					}
					if (newObj != null) {
						AddToTemp(newObj);
					}
					tempModified = true;
					ReleaseTemp();//いったん選択状態を解放

					//分割点で選択しなおす
					Point ptLT = new Point(divideAt.X - 100, divideAt.Y + 100);
					Point ptRB = new Point(divideAt.X + 100, divideAt.Y - 100);
					MbeRect rc = new MbeRect(ptLT, ptRB);

					if (obj.SelectIt(rc, 0xFFFFFFFF, false)) {
						MbeObj tempObj = obj.Duplicate();
						obj.ClearAllSelectFlag();
						obj.DeleteCount = opCount;
						tempList.AddLast(tempObj);
					}
					if (newObj != null) {
						if (newObj.SelectIt(rc, 0xFFFFFFFF, false)) {
							MbeObj tempObj = newObj.Duplicate();
							newObj.ClearAllSelectFlag();
							newObj.DeleteCount = opCount;
							tempList.AddLast(tempObj);
						}
					}

					return true;
				}
			}
			return false;
		}
			

		/// <summary>
		/// 一時データの範囲を返す
		/// </summary>
		/// <returns></returns>
		protected MbeRect TempRect()
		{
			MbeRect rc = new MbeRect();
			bool first = true;
			foreach (MbeObj obj in tempList) {
				MbePointInfo[] pinfoArray = obj.GetPosInfoArray();
				int n = pinfoArray.Length;
				for (int i = 0; i < n; i++) {
					MbePointInfo pinfo = pinfoArray[i];
					if (pinfo.selectFlag) {
						if (first) {
							rc = new MbeRect(pinfo.point, pinfo.point);
							first = false;
						} else {
							rc.Or(pinfo.point);
						}
					}
				}
				

			}
			return rc;
		}


        /// <summary>
        /// 一時データのFlip,回転の中心を返す
        /// </summary>
        /// <returns>Flip,回転の中心座標</returns>
        /// 
        protected Point TempFlipRotCenter()
        {
            MbeRect rc = new MbeRect();
            bool first = true;
            foreach (MbeObj obj in tempList) {
                MbePointInfo[] pinfoArray = obj.GetPosInfoArray();
                int n = pinfoArray.Length;
                if (obj.Id() == MbeObjID.MbeComponent) {
                    if(pinfoArray[0].selectFlag){
                        n = 1;
                    }
                }

                for (int i = 0; i < n; i++) {
                    MbePointInfo pinfo = pinfoArray[i];
                    if (pinfo.selectFlag) {
                        if (first) {
                            rc = new MbeRect(pinfo.point, pinfo.point);
                            first = false;
                        } else {
                            rc.Or(pinfo.point);
                        }
                    }
                }
            }
            return rc.Center();
        }




		/// <summary>
		/// ドキュメントのパスを取得する
		/// </summary>
		public string DocumentPath
		{
			get { return docPath; }
		}

		/// <summary>
		/// ドキュメントのファイル名+拡張子を取得する
		/// </summary>
		public string DocumentName
		{
			get {
				string fname;
				try {
					fname = Path.GetFileName(docPath);
				}
				catch (Exception) {
					fname = "";
				}
				return fname; 
			}
		}


        /// <summary>
        /// ドキュメントのファイル名(拡張子含まず)を取得する
        /// </summary>
        public string DocumentTitle
        {
            get
            {
                string fname;
                try {
                    fname = Path.GetFileNameWithoutExtension(docPath);
                }
                catch (Exception) {
                    fname = "";
                }
                return fname;
            }
        }


		/// <summary>
		/// 一時リストへのReadCEクラスからの読み込み
		/// </summary>
		/// <param name="readMb3"></param>
		/// <param name="docinfo"></param>
		/// <returns></returns>
		protected ReadCE3.RdStatus ReadMb3(ReadCE3 readMb3,MbeDocInfo docinfo)
		{
			ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;
			string str1;
			string str2;
			bool rdresult = readMb3.GetRecord(out str1, out str2);
			if (!rdresult || str1 != "+MBE_DATA_V.1.0") {
				result = ReadCE3.RdStatus.FormatError;
			} else {
				while (readMb3.GetRecord(out str1, out str2)) {
					if (str1[0] == '-') {
						break;
					} else {
						if (str1 == "") continue;
						else if (str1 == "+MBE_DOCINFO") {
							result = docinfo.RdMb3(readMb3);
						} else {
							MbeObj obj;
							result = MbeObjIO.ReadMbeObj(readMb3, str1, out obj);
							if (obj != null) {
								AddToTemp(obj);
							}
							
						}
					}
					if (result != ReadCE3.RdStatus.NoError) {
						break;
					}
				}
			}
			return result;
		}


		/// <summary>
		/// ファイルを開く
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public ReadCE3.RdStatus FileOpen(string path)
		{
			ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;
			this.ClearAll();
            bool isZeroLengthFile = false;

            try {
                FileInfo fi = new FileInfo(path);
                if (fi.Length == 0) {
                    isZeroLengthFile = true;
                }
            }
            catch (Exception) {
                return  ReadCE3.RdStatus.FileError;
            }

            if (!isZeroLengthFile) {
                StreamReader streamReader = null;
                MbeDocInfo docinfo = new MbeDocInfo();
                try {
                    streamReader = new StreamReader(path);
                    ReadCE3 readMb3 = new ReadCE3(streamReader);
                    result = ReadMb3(readMb3, docinfo);
                }
                catch (Exception) {
                    result = ReadCE3.RdStatus.FileError;
                }
                finally {
                    if (streamReader != null) {
                        streamReader.Close();
                    }
                }

                if (result != ReadCE3.RdStatus.NoError) {
                    return result;
                }

			    //if (tempList.Count > 0) {     //2012/01/25 Version 0.51.06
				    docInfo = docinfo;
				    opCount = 0;	//ファイルからの読み込みがundoの対象にならないようにする。
				    ReleaseTemp();
                    docModified = false;

                    if (docinfo.FileDataVersion > MbeDocInfo.DATA_VERSION) {
                        MessageBox.Show("This file was created by the later version. \nSome data may not be loaded correctly.", "MBE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
			    //}
            }

			docPath = path;
			//}
			return result;
		}

		/// <summary>
		/// 上書き保存
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool FileSave()
		{
			return FileSaveAs(docPath);
		}

		/// <summary>
		/// WriteCE3クラスに対して、指定したリストのデータを書き込む
		/// </summary>
		/// <param name="writeMb3"></param>
		/// <param name="mbeObjList"></param>
		/// <param name="origin"></param>
		/// <param name="docinfo"></param>
		protected void WriteMb3(WriteCE3 writeMb3, LinkedList<MbeObj> mbeObjList, Point origin, MbeDocInfo docinfo)
		{
			Point offset = new Point(-origin.X, -origin.Y);
			writeMb3.WriteRecord("+MBE_DATA_V.1.0");
			writeMb3.WriteNewLine();
			docinfo.WrMb3(writeMb3);
			foreach (MbeObj obj in mbeObjList) {
				if (obj.DeleteCount < 0) {
					obj.WrMb3(writeMb3, origin);
				}
			}
			writeMb3.WriteRecord("-MBE_DATA_V.1.0");
			writeMb3.WriteNewLine();
		}

		/// <summary>
		/// 名前指定でファイルへ保存
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool FileSaveAs(string path)
		{
			bool result =true;
			ReleaseTemp();
			StreamWriter streamWriter = null;

			try {
				streamWriter = new StreamWriter(path);
				WriteCE3 writeMb3 = new WriteCE3(streamWriter);
				WriteMb3(writeMb3, mainList, new Point(0, 0), docInfo);
				streamWriter.Flush();
			}
			catch (Exception) {
				result = false;
			}
			finally {
				if (streamWriter != null) {
					streamWriter.Close();
					//streamWriter.Dispose();
				}
			}
			if (result){
				docModified = false;
				tempModified = false;
				docPath = path;
			}
			return result;
		}


		/// <summary>
		/// コピーできるかどうかを返す
		/// </summary>
		/// <returns></returns>
		public bool CanCopy()
		{
			return (tempList.Count > 0);
		}

		/// <summary>
		/// tempListの内容をクリップボードにコピーする 
		/// </summary>
		/// <returns></returns>
		public bool Copy(out Point ptLT)
		{
            ptLT = new Point(0, 0);
			if(!CanCopy()) return false;
			MbeRect tempRc = TempRect();
			//if (tempRc == null) return false;

            ptLT = tempRc.LT;
			MbeDocInfo docinfo = new MbeDocInfo();
			docinfo.SizeWorkArea = tempRc.SizeRect();

			StringBuilder strBuilder = new StringBuilder();
			StringWriter stringWriter = null;
			try{
				stringWriter = new StringWriter(strBuilder);
				WriteCE3 writeMb3 = new WriteCE3(stringWriter);
                WriteMb3(writeMb3, tempList, ptLT, docinfo);
				stringWriter.Flush();
				string strBuff = strBuilder.ToString();
				//Clipboard.SetDataObject(strBuff);
				Clipboard.SetData(mbeClipboardFormat.Name,strBuff);
			}
			catch (Exception) {
				return false;
			}
			return true;
		}


		/// <summary>
		/// 回転
		/// </summary>
		/// <param name="selectedOnly"></param>
		/// <returns></returns>
		public bool Rotate(bool selectedOnly)
		{
			if (!CanCopy()) return false;
            //MbeRect tempRc = TempRect();
            //Point ptCenter = tempRc.Center();

            Point ptCenter = TempFlipRotCenter();

			foreach (MbeObj obj in tempList) {
				obj.Rotate90(selectedOnly, ptCenter);
			}
			tempModified = true;
			return true;
		}

		/// <summary>
		/// 裏返し
		/// </summary>
		/// <returns></returns>
		public bool Flip(out ulong layers)
		{
			layers = 0;
			if (!CanCopy()) return false;
			//MbeRect tempRc = TempRect();
			//int hCenter = tempRc.Center().X;

            Point ptCenter = TempFlipRotCenter();
            int hCenter = ptCenter.X;

			foreach (MbeObj obj in tempList) {
				obj.Flip(hCenter);
				layers |= (ulong)obj.Layer;
			}
			tempModified = true;
			return true;
		}


		/// <summary>
		/// ペーストできるかどうかを返す
		/// </summary>
		/// <returns></returns>
		public bool CanPaste()
		{
			//if (Clipboard.ContainsData(DataFormats.UnicodeText)) {
			//    //string str = (string)System.Windows.Forms.Clipboard.GetData(DataFormats.UnicodeText);
			//    //return str.StartsWith("+MBE_DATA_V.1.0");
			//    return true;
			//} else {
			//    return false;
			//}
#if MONO
			return true;
#else
            if (useTextClipboard) {
                return true;
            } else {
                return Clipboard.ContainsData(mbeClipboardFormat.Name);
            }
#endif
		}



		/// <summary>
		/// ペースト
		/// </summary>
		/// <param name="offset">ペースト位置(左上)</param>
		/// <param name="layers">ペースト時に可視であるべきレイヤー</param>
		/// <returns>図面要素の数を返す</returns>
		public int Paste(Point offset, out ulong layers)
		{
			layers = 0L;
			ReleaseTemp();
			if (!CanPaste()) return 0;
			string str = (string)System.Windows.Forms.Clipboard.GetData(mbeClipboardFormat.Name);
			//string str = (string)System.Windows.Forms.Clipboard.GetData(DataFormats.UnicodeText);
			if (str == null) return 0;
			if (!str.StartsWith("+MBE_DATA_V.1.0")) return 0;
			StringReader stringReader = new StringReader(str);
			MbeDocInfo docinfo = new MbeDocInfo();
			ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;
			try {
				ReadCE3 readMb3 = new ReadCE3(stringReader);
				result = ReadMb3(readMb3, docinfo);
			}
			catch {
				return 0;
			}
			finally {
				stringReader.Dispose();
			}
			if (tempList.Count == 0) return 0;

			foreach (MbeObj obj in tempList) {
				obj.SetAllSelectFlag();
				obj.Move(false, offset, offset,false);
				layers |= obj.ShouldBeVisibleLayer();
			}

			return tempList.Count;
		}

		/// <summary>
		/// 消去
		/// </summary>
		public void Delete()
		{
			if (tempList.Count == 0) {
				return;
			}
			pointSelectNode = null;
			opCount++;	//Delete操作によるopCountのインクリメント
			DiscardRedo();
			System.Diagnostics.Debug.WriteLine("MbeDoc.ReleaseTemp() opCount=" + opCount);

			docModified = true;
			tempList.Clear();
			tempModified = false;
		}

		/// <summary>
		/// Undo可能かどうかを返す
		/// </summary>
		/// <returns></returns>
		public bool CanUndo()
		{
			if ((tempList.Count > 0) && tempModified) return true;
			if (opCount > 1) return true;
			return false;
		}

		/// <summary>
		/// REDOできるときはtrueを返す
		/// </summary>
		/// <returns></returns>
		public bool CanRedo()
		{
			if(redoActive<0)return false;
			else return true;
		}
		
		/// <summary>
		/// REDOの実行
		/// </summary>
		/// <returns></returns>
		public bool Redo()
		{
			if (!CanRedo()) return false;
			if (tempList.Count > 0) {
				ReleaseTemp();
				DiscardRedo();
				return false;
			}
			bool doRedo = false;
			for (; ; ) {
				if (redoStack.Count == 0) break;
				RedoInfo redoInfo = redoStack.Pop();
				if (redoInfo.undoCount == opCount) {
					if (redoInfo.undoType== RedoInfo.UndoType.UndoAddMoveToStack) {
						mainList.AddLast(redoInfo.obj);
					} else { //obj.RedoType == MbeRedoType.UndoDeleteCopyToStack
						System.Diagnostics.Debug.WriteLine("Redoによる再消去");
						redoInfo.obj.DeleteCount = opCount;
					}

					doRedo = true;
				} else {
					redoStack.Push(redoInfo);//スタックに戻す
					break;
				}
			}
			opCount++;
			if (redoActive <= opCount) {	//Undoを始めた地点まで戻ったら
				DiscardRedo();				//Redoを破棄する。
			}
			return doRedo;
		}


		/// <summary>
		/// Undoの実行
		/// </summary>
		/// <returns></returns>
		public bool Undo()
		{
			if (!CanUndo()) return false;
			if (tempList.Count > 0) {
				tempModified = false;
				ReleaseTemp();
				return true;
			} else {
				//ReleaseTemp();
				System.Diagnostics.Debug.WriteLine("MbeDoc.Undo()1 opCount = "+opCount+", mainList.Count = " + mainList.Count);

				if (redoActive < 0) {
					redoActive = opCount;
				}

				pointSelectNode = null;
				opCount--;//Undoにともなうデクリメント
				polygonLastUpdate = 0;//Undoしたらポリゴンはアップデートが必要になる。

				LinkedListNode<MbeObj> node1;
				LinkedListNode<MbeObj> node2;


				node1 = mainList.First;
				while (node1 != null) {
					node2 = node1;
					node1 = node2.Next;
					MbeObj obj = node2.Value;
					if (obj.AddCount >= opCount) {
						mainList.Remove(node2);
						RedoInfo redoInfo = new RedoInfo();
						redoInfo.undoCount = opCount;
						redoInfo.undoType = RedoInfo.UndoType.UndoAddMoveToStack;
						redoInfo.obj = obj;
						//obj.RedoStatus = opCount;
						System.Diagnostics.Debug.WriteLine("Undo MOVE opCount=" + opCount);
						//obj.RedoType = MbeRedoType.UndoAddMoveToStack;
						redoStack.Push(redoInfo);
					} else if (obj.DeleteCount >= opCount) {
						obj.ClearDeleteCount();
						RedoInfo redoInfo = new RedoInfo();
						redoInfo.undoCount = opCount;
						redoInfo.undoType = RedoInfo.UndoType.UndoDeleteCopyToStack;
						redoInfo.obj = obj;
						//obj.RedoStatus = opCount;
						System.Diagnostics.Debug.WriteLine("Undo COPY opCount=" + opCount);
						//obj.RedoType = MbeRedoType.UndoDeleteCopyToStack;
						redoStack.Push(redoInfo);
					}
				}

				System.Diagnostics.Debug.WriteLine("MbeDoc.Undo()2 opCount = " + opCount + ",  mainList.Count = " + mainList.Count);

				return true;
			}
		}

		/// <summary>
		/// プロパティの編集ができるかどうかを返す
		/// </summary>
		/// <returns></returns>
		/// <remarks>実際のプロパティ編集の動作はViewで定義する</remarks>
		public bool CanEditProperty()
		{
			return (tempList.Count == 1);
		}

        /// <summary>
        /// ラインのプロパティが編集できるかどうかを返す
        /// </summary>
        /// <returns></returns>
        public bool CanEditLineProperty()
        {
            if (tempList.Count == 1) {
                MbeObj obj = tempList.First.Value;
                if (obj.Id() == MbeObjID.MbeLine) return true;
            }
            return false;
        }




		/// <summary>
		/// コンポーネント化できるかどうかを返す
		/// </summary>
		/// <returns></returns>
		public bool CanComponenting()
		{
			int n = 0;
			foreach (MbeObj obj in tempList) {
				if (obj.Id() == MbeObjID.MbeComponent ||
					obj.Id() == MbeObjID.MbePolygon) return false;
				if (obj.IsValid()) n++;
			}
			return (n > 1);
		}

		/// <summary>
		/// コンポーネント解除できるかどうかを返す
		/// </summary>
		/// <returns></returns>
		public bool CanUncomponenting()
		{
			int n = tempList.Count;
			if (n != 1) return false;
			return (tempList.First.Value.Id() == MbeObjID.MbeComponent);
		}

		/// <summary>
		/// コンポーネント化
		/// </summary>
		/// <param name="ptOrigin"></param>
		/// <param name="strReference"></param>
		public void Componenting(Point ptOrigin,String strReference)
		{
			if(!CanComponenting())return;
			MbeObjComponent componentObj = new MbeObjComponent(ptOrigin,strReference,tempList);
			componentObj.SetAllSelectFlag();
			tempList.Clear();
			tempList.AddLast(componentObj);
			tempModified = true;
		}

		/// <summary>
		/// コンポーネント解除
		/// </summary>
		public void Uncomponenting(out ulong layers)
		{
			layers = 0L;
			if (!CanUncomponenting()) return;
			MbeObj objTemp = tempList.First.Value;
			if (objTemp.Id() != MbeObjID.MbeComponent) return;
			MbeObjComponent componentObj = (MbeObjComponent)objTemp;
			tempList.Clear();
			List<MbeObj> objList = componentObj.ContentsObj;
			tempModified = true;
			if (objList == null) return;
			foreach (MbeObj obj in objList) {
				obj.SetAllSelectFlag();
				tempList.AddLast(obj);
				layers |= obj.ShouldBeVisibleLayer();
			}
		}





		/// <summary>
		/// バルクプロパティが実行できるかどうかを返す
		/// </summary>
		/// <returns></returns>
		public CanBulkPropResult CanBulkProperty()
		{
			CanBulkPropResult result = new CanBulkPropResult();
            result.Clear();
			foreach (MbeObj obj in tempList) {
				switch (obj.Id()) {
					case MbeObjID.MbeHole:
						result.CanPropHole = true;
						break;
					case MbeObjID.MbePTH:
						result.CanPropPTH = true;
						break;
					case MbeObjID.MbePinSMD:
						result.CanPropPad = true;
                        result.CanMoveLayer = true;
                        result.SelectableLayer &= MbeObjPinSMD.SelectableLayer();
                        break;
					case MbeObjID.MbeLine:
						result.CanPropLine = true;
						result.CanMoveLayer = true;
                        result.SelectableLayer &= MbeObjLine.SelectableLayer();
						break;
					case MbeObjID.MbeArc:
						result.CanPropLine = true;
						result.CanMoveLayer = true;
                        result.SelectableLayer &= MbeObjArc.SelectableLayer();
						break;
					case MbeObjID.MbeText:
						result.CanPropText = true;
						result.CanMoveLayer = true;
                        result.SelectableLayer &= MbeObjText.SelectableLayer();
						break;
					case MbeObjID.MbeComponent:
						result.CanPropText = true;
						break;
                    case MbeObjID.MbePolygon:
                        result.CanPropPolygon = true;
						result.CanMoveLayer = true;
                        result.SelectableLayer &= MbeObjPolygon.SelectableLayer();
						break;
				}
			}
	
			return result;
		}


		/// <summary>
		/// フォント変換の1文字単位の実行メソッド。指定範囲のラインを文字列化。
		/// </summary>
		/// <param name="pt1"></param>
		/// <param name="pt2"></param>
		/// <returns></returns>
		/// <remarks>
		/// 先頭4文字:ベクトル数
		/// 以後2文字:始点x 2文字:始点y 2文字:終点x 2文字:終点y の繰り返し
		/// 行末は改行
		/// </remarks>
		protected string ConvertBoardFont1(Point pt1, Point pt2)
		{
		    MbeRect rc = new MbeRect(pt1, pt2);
			int left = rc.LT.X;
			int bottom = rc.RB.Y;
			StringBuilder strBuilder = new StringBuilder();
			int count = 0;

		    foreach (MbeObj obj in mainList) {
				if(count==255) break;
				if ((obj.DeleteCount < 0) && (obj.Id() == MbeObjID.MbeLine)) {
					if (obj.SelectIt(rc, 0xFFFFFFFFFFFFFFFF,false)) {
						count++;
						int x0 = ((obj.GetPos(0).X - left) / 1000) & 0xFF;
						int y0 = ((obj.GetPos(0).Y - bottom) / 1000) & 0xFF;
						int x1 = ((obj.GetPos(1).X - left) / 1000) & 0xFF;
						int y1 = ((obj.GetPos(1).Y - bottom) / 1000) & 0xFF;
						strBuilder.AppendFormat("{0:X2}{1:X2}{2:X2}{3:X2}", x0, y0, x1, y1);
						obj.ClearAllSelectFlag();
					}
				}
		    }
			string strCount = String.Format("{0:X4}", count);

			return strCount + strBuilder.ToString();
		}

		/// <summary>
		/// フォント変換 0x21～0x7e
		/// </summary>
		/// <returns></returns>
		public bool ConvertBoardFont(string path)
		{
			const int fontDesignHeight = 100000;
			const int fontDesignWidth = 70000;
			Point pt1;
			Point pt2;
			
			StreamWriter streamWriter = null;

			try {
				streamWriter = new StreamWriter(path);

				for (int ccode = 0x21; ccode < 0x7f; ccode++) {
					int row = (ccode - 0x20) / 16;
					int col = ccode % 16;
					int top = (6 - row) * fontDesignHeight;
					int bottom = top - fontDesignHeight;
					int left = col * fontDesignWidth;
					int right = left + fontDesignWidth;

					pt1 = new Point(left, top);
					pt2 = new Point(right, bottom);
					streamWriter.WriteLine(ConvertBoardFont1(pt1, pt2));
				}
				streamWriter.Flush();
			}
			catch (Exception) {
				return false;
			}
			finally {
				if (streamWriter != null) {
					streamWriter.Close();
				}
			}
			return true;

			//return strBuilder.ToString();
		}

		public bool CanConvertBoardFont()
		{
			return DocumentName.Equals("fontdesign.mb3", StringComparison.OrdinalIgnoreCase);
		}

		public bool BulkPropHole(MbeObjHole objR)
		{
			bool result = false;
			foreach (MbeObj obj in tempList) {
				if (obj.Id() == MbeObjID.MbeHole) {
					((MbeObjHole)obj).Diameter = objR.Diameter;
					result = true;
				}
			}
			if (result) {
				tempModified = true;
			}
			return result;
		}

		public bool BulkPropPTH(MbeObjPTH objR)
		{
			bool result = false;
			foreach (MbeObj obj in tempList) {
				if (obj.Id() == MbeObjID.MbePTH) {
					((MbeObjPTH)obj).Diameter = objR.Diameter;
					((MbeObjPTH)obj).Shape = objR.Shape;
					((MbeObjPTH)obj).PadSize = objR.PadSize;
                    ((MbeObjPTH)obj).No_ResistMask = objR.No_ResistMask;
                    ((MbeObjPTH)obj).ThermalRelief = objR.ThermalRelief;
					result = true;
				}
			}
			if (result) {
				tempModified = true;
			}
			return result;
		}

		public bool BulkPropPad(MbeObjPinSMD objR)
		{
			bool result = false;
			foreach (MbeObj obj in tempList) {
				if (obj.Id() == MbeObjID.MbePinSMD) {
					((MbeObjPinSMD)obj).Shape = objR.Shape;
					((MbeObjPinSMD)obj).PadSize = objR.PadSize;
                    ((MbeObjPinSMD)obj).No_MM = objR.No_MM;
                    ((MbeObjPinSMD)obj).No_ResistMask = objR.No_ResistMask;
                    ((MbeObjPinSMD)obj).ThermalRelief = objR.ThermalRelief;
					result = true;
				}
			}
			if (result) {
				tempModified = true;
			}
			return result;
		}

		public bool BulkPropLine(MbeObjLine objR)
		{
			bool result = false;
			foreach (MbeObj obj in tempList) {
				if (obj.Id() == MbeObjID.MbeLine) {
					((MbeObjLine)obj).LineWidth = objR.LineWidth;
					result = true;
				} else if (obj.Id() == MbeObjID.MbeArc) {
					((MbeObjArc)obj).LineWidth = objR.LineWidth;
					result = true;
				}
			}
			if (result) {
				tempModified = true;
			}
			return result;
		}

		public bool BulkPropText(MbeObjText objR)
		{
			bool result = false;
			foreach (MbeObj obj in tempList) {
				if (obj.Id() == MbeObjID.MbeText) {
					((MbeObjText)obj).LineWidth = objR.LineWidth;
					((MbeObjText)obj).TextHeight = objR.TextHeight;
					result = true;
				} else if (obj.Id() == MbeObjID.MbeComponent) {
					((MbeObjComponent)obj).RefNumLineWidth = objR.LineWidth;
					((MbeObjComponent)obj).RefNumTextHeight = objR.TextHeight;
					result = true;
				}
			}
			if (result) {
				tempModified = true;
			}
			return result;
		}


        public bool BulkPropPolygon(int traceWidth, int patternGap, CheckState removeFloating, CheckState restrictMask)
        {
            bool result = false;
            foreach (MbeObj obj in tempList) {
                if (obj.Id() == MbeObjID.MbePolygon) {
                    ((MbeObjPolygon)obj).TraceWidth = traceWidth;
                    ((MbeObjPolygon)obj).PatternGap = patternGap;
                    if (removeFloating != CheckState.Indeterminate) {
                        ((MbeObjPolygon)obj).RemoveFloating = (removeFloating == CheckState.Checked);
                    }
                    if (restrictMask != CheckState.Indeterminate) {
                        ((MbeObjPolygon)obj).RestrictMask = (restrictMask == CheckState.Checked);
                    }
                    result = true;
                } 
            }
            if (result) {
                tempModified = true;
            }
            return result;
        }





//   		public bool BulkPropLayer(CanBulkPropResult canBulkPropResult)
		public bool BulkPropLayer(MbeLayer.LayerValue moveToLayer)
		{
			bool result = false;
			foreach (MbeObj obj in tempList) {
				if (obj.Id() == MbeObjID.MbeText || 
					obj.Id() == MbeObjID.MbeLine ||
					obj.Id() == MbeObjID.MbePolygon ||
                    obj.Id() == MbeObjID.MbePinSMD ||
					obj.Id() == MbeObjID.MbeArc
					) {
					obj.Layer = moveToLayer;
					result = true;
				}
			}
			if (result) {
				tempModified = true;
			}
			return result;
		}

		public CamOutResult ExportGerber(string outputDir,ulong layerMask)
		{
            string filename = Path.Combine(outputDir, Path.GetFileName(docPath));
//            string filename = docPath;
//			filename = Path.ChangeExtension(filename, "dump");
			//ReleaseTemp();
			FillPolygon();

            //SetPTHinnerLayerConnectionInfo(mainList);

			camOut.Init();
			foreach (MbeObj obj in mainList) {
				if (obj.DeleteCount < 0) {
					obj.GenerateCamData(camOut);
				}
			}
            return camOut.GerberOut(filename, layerMask);
		}

        private bool DoExportComponetPositionList(string path,LinkedList<MbeObj>mbelist)
        {
            bool result = true;
   			StreamWriter streamWriter = null;

            List<MbeObjComponent> componentList = new List<MbeObjComponent>();

            foreach(MbeObj obj in mbelist){
                if(obj.Id() == MbeObjID.MbeComponent){
                    componentList.Add((MbeObjComponent)obj);
                }
            }

            ComponentRefComparer crcmpr = new ComponentRefComparer();
            componentList.Sort(crcmpr);




			try {
                if (Program.listFileUseLocalEncoding) {
                    streamWriter = new StreamWriter(path, false, Encoding.Default);
                } else {
                    streamWriter = new StreamWriter(path);
                }
				streamWriter.WriteLine("Ref,X(mm),Y(mm),Rotation(CCW),Side,Name,Package");
                foreach(MbeObjComponent obj in componentList){
                    string str;
                    MbeObjComponent mbeComp = (MbeObjComponent)obj;
                    str = string.Format("{0},{1:##0.0####},{2:##0.0####},", mbeComp.RefNumText,
                                                                            ((double)(mbeComp.GetPos(0).X))/10000,
                                                                            ((double)(mbeComp.GetPos(0).Y))/10000);
                    streamWriter.Write(str);
                    if(mbeComp.AngleX10 <0){
                        str = "#####,";
                    }else{
                        str = string.Format("{0},",((double)(mbeComp.AngleX10))/10);
                    }
                    streamWriter.Write(str);
                    str = string.Format("{0},{1},{2}",(mbeComp.Layer== MbeLayer.LayerValue.CMP ? "Component":"Solder"),mbeComp.SigName,mbeComp.PackageName);
                    streamWriter.WriteLine(str);
                }
            }
			catch (Exception) {
				result = false;
			}
			finally {
				if (streamWriter != null) {
					streamWriter.Close();
				}
			}
			return result;
		}



        public bool ExportComponentPosition(string filename)
        {
			//string filename = docPath;
			//filename = Path.ChangeExtension(filename, "csv");
            return DoExportComponetPositionList(filename, mainList);
        }

		public bool ExportNetlist(string filename)
		{
			//string filename = docPath;
			//filename = Path.ChangeExtension(filename, "net");

			MbeNetOut netOut = new MbeNetOut();
			return netOut.DoExport(filename, mainList);

		}

		/// <summary>
		/// DRCの実行
		/// </summary>
		/// <param name="drcParam"></param>
		/// <returns></returns>
		/// <remarks>
		/// Version 0.26 パターンのギャップチェック
		/// </remarks>
		public int Drc(MbeDrcParam drcParam)
		{
			ReleaseTemp();
			MbeGapChk gapChk = new MbeGapChk();
			ptLListGapChkResult.Clear();
			int chkLimit = drcParam.checkLimit;
			gapChk.DoGapChk(mainList, drcParam.patternGap, ptLListGapChkResult, chkLimit);
			return ptLListGapChkResult.Count;
		}


		/// <summary>
		/// 接続フラグをクリアする
		/// </summary>
		public void ClearConnectCheckFlag()
		{
			foreach (MbeObj obj in mainList) {
				if (obj.DeleteCount < 0) {
					obj.ClearConnectCheck();
				}
			}
		}

		protected void DiscardRedo()
		{
			System.Diagnostics.Debug.WriteLine("DiscardRedo()");
			redoActive = -1;
			redoStack.Clear();
		}

        /// <summary>
        /// PTHの内層接続フラグをクリアする
        /// </summary>
        /// <param name="objList"></param>
        protected void ClearPTHinnerLayerConnectionInfo(LinkedList<MbeObj> objList)
        {
            foreach (MbeObj obj in objList) {
                if (obj.DeleteCount < 0) {
                    if (obj.Id() == MbeObjID.MbePTH) {
                        ((MbeObjPTH)obj).innerLayerConnectionInfo = 0;
                    } else if (obj.Id() == MbeObjID.MbeComponent) {
                        foreach (MbeObj contentObj in ((MbeObjComponent)obj).ContentsObj) {
                            if(contentObj.Id()==MbeObjID.MbePTH) {
                                ((MbeObjPTH)contentObj).innerLayerConnectionInfo = 0;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// PTHのLineによる内層接続フラグを設定する(CamOutでPTHの内層ランド有無判定に使う。ここ以外にFillPolygonでポリゴンのとの接続でも判定が行われる)
        /// </summary>
        /// <param name="objList"></param>
        protected void SetPTHinnerLayerConnectionInfo(LinkedList<MbeObj> objList)
        {
            LinkedList<MbeObjPTH>  pthList = new LinkedList<MbeObjPTH>();
            LinkedList<MbeObjLine> lineList = new LinkedList<MbeObjLine>();

            //PTHと内装ラインの抽出
            foreach (MbeObj obj in objList) {
                if (obj.DeleteCount < 0) {
                    if (obj.Id() == MbeObjID.MbePTH) {
                        pthList.AddLast((MbeObjPTH)obj);
                    } else if (obj.Id() == MbeObjID.MbeLine) {
                        if (obj.Layer == MbeLayer.LayerValue.L2 || obj.Layer == MbeLayer.LayerValue.L3) {
                            lineList.AddLast((MbeObjLine)obj);
                        }
                    }else if (obj.Id() == MbeObjID.MbeComponent) {
                        foreach (MbeObj contentObj in ((MbeObjComponent)obj).ContentsObj) {
                            if (contentObj.Id() == MbeObjID.MbePTH) {
                                pthList.AddLast((MbeObjPTH)contentObj);
                            } else if (contentObj.Id() == MbeObjID.MbeLine) {
                                if (contentObj.Layer == MbeLayer.LayerValue.L2 || contentObj.Layer == MbeLayer.LayerValue.L3) {
                                    lineList.AddLast((MbeObjLine)contentObj);
                                }
                            }
                        }
                    }
                }
            }

            foreach (MbeObjPTH pthObj in pthList) {
                foreach (MbeObjLine lineObj in lineList) {
                    if (((pthObj.innerLayerConnectionInfo & (ulong)MbeLayer.LayerValue.L2) != 0) &&
                        ((pthObj.innerLayerConnectionInfo & (ulong)MbeLayer.LayerValue.L3) != 0)) {
                        break;
                    }
                    if ((pthObj.innerLayerConnectionInfo & (ulong)lineObj.Layer) != 0) {
                        continue;
                    }
                    if (pthObj.GetPos(0) == lineObj.GetPos(0) || pthObj.GetPos(0) == lineObj.GetPos(1)) {
                        pthObj.innerLayerConnectionInfo |= (ulong)lineObj.Layer;
                    }
                }
            }
        }




		public void FillPolygon()
		{
			ReleaseTemp();

			if (polygonLastUpdate == opCount){
                //SetPTHinnerLayerConnectionInfo(mainList);
                return;
            }


            ClearPTHinnerLayerConnectionInfo(mainList);
            SetPTHinnerLayerConnectionInfo(mainList);

            LinkedList<MbeObjPolygon> polygonList = new LinkedList<MbeObjPolygon>();

			//すべてのポリゴンのfillデータをクリアする。
			//プライオリティの順になるようにpolygonListに登録する
			foreach (MbeObj obj in mainList) {
                if (obj.DeleteCount < 0) {
                    if (obj.Id() == MbeObjID.MbePolygon) {
                        MbeObjPolygon polygonObj = ((MbeObjPolygon)obj);
                        polygonObj.fillLineList.Clear();
                        polygonObj.doneFillFlag = false;
                        LinkedListNode<MbeObjPolygon> node = polygonList.First;

                        while (node != null) {
                            MbeObjPolygon regObj = node.Value;
                            if (polygonObj.RestrictMask || (!regObj.RestrictMask && regObj.FillingPriority <= polygonObj.FillingPriority)) {
                                polygonList.AddBefore(node, polygonObj);
                                break;
                            }
                            node = node.Next;
                        }
                        if (node == null) {
                            polygonList.AddLast(polygonObj);
                        }
                    }
                }
			}

			foreach (MbeObjPolygon obj in polygonList) {
				if (obj.Id() == MbeObjID.MbePolygon) {
					//FillPolygon fP = new FillPolygon();
					//fP.UpdateFill((MbeObjPolygon)obj, mainList);
                    FillPolygon2 fP = new FillPolygon2();
                    fP.UpdateFill((MbeObjPolygon)obj, mainList);
                    ((MbeObjPolygon)obj).doneFillFlag = true;
                }
			}
			polygonLastUpdate = opCount;
		}

	}

	public struct CanBulkPropResult
	{
		public bool CanPropHole;
		public bool CanPropPTH;
		public bool CanPropPad;
		public bool CanPropLine;
		public bool CanPropText;
		public bool CanMoveLayer;
        public bool CanPropPolygon;
        public ulong SelectableLayer;

        public void Clear()
        {
            CanPropHole = false;
            CanPropPTH = false;
            CanPropPad = false;
            CanPropLine = false;
            CanPropText = false;
            CanMoveLayer = false;
            CanPropPolygon = false;
            SelectableLayer = 0xFFFFFFFFFFFFFFFFL;
        }

	}

	public class RedoInfo
	{
		public enum UndoType
		{
			UndoAddMoveToStack = 0,
			UndoDeleteCopyToStack
		}
		public RedoInfo()
		{
			undoCount = -1;
			undoType = UndoType.UndoAddMoveToStack;
			obj = null;
		}

		public int undoCount;
		public UndoType undoType;
		public MbeObj obj;
	}


}
