using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
	public struct MbePointInfo
	{
		public ulong layer;
		public Point point;
		public bool selectFlag;
        public MbeObj componentPinObj;
 	}

	public enum MbeDrawMode
	{
		Draw = 0,
		Print,
		Temp
	}

    public enum MbeDrawOption
    {
        NoOption = 0,
        CenterPunchMode = 0x0001,
        ToolMarkMode    = 0x0002,
        PrintColor      = 0x0004
    }



	public class DrawParam
	{
        public DrawParam()
        {
            option = (uint)MbeDrawOption.NoOption;
        }

		public Graphics g;
		public double scale;
		public MbeLayer.LayerValue layer;
		public MbeDrawMode mode;
        public uint option;                // 2008/09/19追加
		public ulong visibleLayer;	    // 2008/02/26追加
	}


	/// <summary>
	/// MbeObjの種類ごとのID
	/// </summary>
	public enum MbeObjID
	{
		MbeBase		= 0,
		MbePTH, 
		MbePinSMD,
		MbeLine,
		MbeArc,
		MbeText,
		MbeHole,
		MbeComponent,
		MbePolygon
	}

	public abstract class MbeObj
	{
		protected const int MBE_OBJ_ALPHA = 0x80;
        protected const int MBE_OBJ_PRINT_ALPHA = 0x80;



		protected int addCount;

		/// <summary>
		/// 削除したときの操作カウントを保持する
		/// </summary>
		/// <remarks>
		/// 削除前は負の数。
		/// 削除前の通常の値は -1。Net作成やDRCで一時的に他の負値を与えることがある。
		/// 削除すると、0以上の操作カウントを保持する。
		/// MainListのスキャンなどで、削除ずみかどうかを判定するのは、0以上か否かで行う。
		/// -1と照合しない。
		/// </remarks>
		protected int deleteCount;


		protected Point[] posArray;
		protected bool[] selectFlag;
		protected int posCount;

		protected MbeLayer.LayerValue layer;
		protected string signame;
		protected ulong snapLayer;

		protected static bool drawSnapMark = true;
		protected static bool drawPinNum = true;

		protected bool connectionCheckActive;

		/// <summary>
		/// 一時的なプロパティ用文字列
		/// </summary>
		/// <remarks>
		/// ファイル保存の対象としない
		/// 用途
		/// ・Net作成、DRCの際に部品から抽出したピンに部品名を保持させる
		/// </remarks>
		protected string strTempProp;




		/// <summary>
		/// コンストラクタ
		/// </summary>
		protected  MbeObj()
		{
			Layer = MbeLayer.LayerValue.CMP;
			addCount	= -1;
			deleteCount = -1;
			signame = "";
			snapLayer = 0;
			connectionCheckActive = false;
			strTempProp = "";
		}

		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="mbeObj"></param>
		protected  MbeObj(MbeObj mbeObj)
		{
			posCount = mbeObj.posCount;
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			for (int i = 0; i < posCount; i++) {
				posArray[i] = mbeObj.posArray[i];
				selectFlag[i] = mbeObj.selectFlag[i];
			}
			layer = MbeLayer.LayerValue.NUL;
			Layer = mbeObj.layer;
			signame = mbeObj.signame;
			addCount = mbeObj.addCount;
			deleteCount = mbeObj.deleteCount;
			connectionCheckActive = mbeObj.connectionCheckActive;
			strTempProp = mbeObj.strTempProp;
		}

		/// <summary>
		/// 同じ場所に同じオブジェクトが重なるのを防ぐ
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>比較対象と重ならないときにtrue</returns>
		public virtual bool CheckRejectOverlay(MbeObj obj)
		{
			if (Id() != obj.Id()) return false;
			for (int i = 0; i < posCount; i++) {
				if (posArray[i] != obj.posArray[i]) return false;
			}
			if (Layer != obj.layer) return false;
			return true;
		}


		/// <summary>
		/// MBEのデータに追加したときの作業カウントの取得と設定
		/// </summary>
		/// <remarks>
		/// 取得値の-1は未設定。
		/// 設定は正の値のみ可能。
		/// </remarks>
		public int AddCount
		{
			get
			{
				return addCount;
			}
			set
			{
				if (value < 0) value = 0;
				addCount = value;
			}
		}

		/// <summary>
		/// MBEのデータから削除したときの作業カウントの取得と設定
		/// </summary>
		/// <remarks>
		/// 取得値の-1は未設定。
		/// 設定は正の値のみ可能。
		/// </remarks>
		public int DeleteCount
		{
			get
			{
				return deleteCount;
			}
			set
			{
				deleteCount = value;
			}
		}

		/// <summary>
		/// MBEのデータから削除したときの作業カウントを未設定に戻す
		/// </summary>
		public void ClearDeleteCount()
		{
			deleteCount = -1;
		}

		/// <summary>
		/// レイヤー値の取得と設定
		/// </summary>
		public virtual MbeLayer.LayerValue Layer
		{
			get
			{
				return layer;
			}
			set
			{
				layer = value;
				snapLayer = (ulong)layer;
			}
		}

 

        public ulong SnapLayer()
        {
            return snapLayer;
        }


		/// <summary>
		/// 表示されるべきレイヤー
		/// </summary>
		/// <returns></returns>
		public virtual ulong ShouldBeVisibleLayer()
		{
			return (ulong)layer;
		}


		public virtual MbePointInfo[] GetSnapPointArray()
		{

			MbePointInfo[] infoArray = new MbePointInfo[posCount];
			for (int i = 0; i < posCount; i++) {
				MbePointInfo sp;
				//sp.layer = snapLayer;
                sp.layer = (ulong)layer;
				sp.point = GetPos(i);
				sp.selectFlag = false;
                sp.componentPinObj = null;
				infoArray[i] = sp;
			}
			return infoArray;
		}

        public virtual MbePointInfo[] GetSnapPointArrayForMeasure()
        {
            return GetSnapPointArray();
        }



		public virtual MbePointInfo[] GetPosInfoArray()
		{

			MbePointInfo[] infoArray = new MbePointInfo[posCount];
			for (int i = 0; i < posCount; i++) {
				MbePointInfo sp;
				//sp.layer = snapLayer;
                sp.layer = (ulong)layer;
				sp.point = GetPos(i);
				sp.selectFlag = selectFlag[i];
                sp.componentPinObj = null;
				infoArray[i] = sp;
			}
			return infoArray;
		}
		


		/// <summary>
		/// 指定したインデックスの位置の設定
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		public virtual  void SetPos(Point pos, int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			posArray[index] = pos;
		}

		/// <summary>
		/// 指定したインデックスのX位置の設定
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		protected void SetXPos(int xpos, int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			posArray[index].X = xpos;
		}

		/// <summary>
		/// 指定したインデックスのY位置の設定
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		protected void SetYPos(int ypos, int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			posArray[index].Y = ypos;
		}

		/// <summary>
		/// 指定したインデックスの位置の取得
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		public Point GetPos(int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			return posArray[index];
		}

		public int PosCount
		{
			get { return posCount; }
		}


		/// <summary>
		/// 移動
		/// </summary>
		/// <param name="selectedOnly">選択フラグが立っているものだけ移動する場合はtrue</param>
		/// <param name="offset">移動量</param>
		public virtual void Move(bool selectedOnly, Point offset, Point ptAbs, bool moveSingle)
		{
			for (int i = 0; i < posCount; i++) {
				if (!selectedOnly || selectFlag[i]) {
					posArray[i].Offset(offset);
				}
			}
		}

		/// <summary>
		/// 回転
		/// </summary>
		/// <param name="selectedOnly"></param>
		/// <param name="ptCenter"></param>
		public virtual void Rotate90(bool selectedOnly, Point ptCenter)
		{
			for (int i = 0; i < posCount; i++) {
				if (!selectedOnly || selectFlag[i]) {
					int x=posArray[i].X - ptCenter.X;
					int y=posArray[i].Y - ptCenter.Y;
					int newx = -y + ptCenter.X;
					int newy = x  + ptCenter.Y;
					posArray[i] = new Point(newx,newy);
				}
			}
		}

		public virtual void Flip(int hCenter)
		{
			for (int i = 0; i < posCount; i++) {
				int x = hCenter - (posArray[i].X - hCenter);
				int y = posArray[i].Y;
				posArray[i] = new Point(x, y);
			}
			Layer = MbeLayer.Flip(layer);
		}







		/// <summary>
		/// 指定したインデックスの位置の選択フラグの取得
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="index"></param>
		public bool GetSelectFlag(int index)
		{
			if (index < 0) index = 0;
			if (index >= posCount) index = posCount - 1;
			return selectFlag[index];
		}

		/// <summary>
		/// 全ての選択フラグのセット
		/// </summary>
		public virtual void SetAllSelectFlag()
		{
			for (int i = 0; i < posCount; i++) {
				selectFlag[i] = true;
			}
		}

		/// <summary>
		/// 全ての選択フラグのクリア
		/// </summary>
		public virtual void ClearAllSelectFlag()
		{
			for (int i = 0; i < posCount; i++) {
				selectFlag[i] = false;
			}
		}

		/// <summary>
		/// 選択されているかどうかの判定
		/// </summary>
		/// <returns>選択されているときtrueを返す</returns>
		public virtual bool IsSelected()
		{
			for (int i = 0; i < posCount; i++) {
				if(selectFlag[i])return true;
			}
			return false;
		}

		/// <summary>
		/// 信号名の設定と取得
		/// </summary>
		public string SigName
		{
			get
			{
				return signame;
			}
			set
			{
				signame = value;
			}
		}

		/// <summary>
		/// 有効かどうかを返す
		/// </summary>
		/// <returns></returns>
		public virtual bool IsValid()
		{
			return true;
		}


		/// <summary>
		/// 図面オブジェクトごとのID値を返す
		/// </summary>
		public abstract MbeObjID Id();

		/// <summary>
		/// CAMデータの生成 
		/// </summary>
		/// <param name="camOut"></param>
		public abstract void GenerateCamData(CamOut camOut);

		/// <summary>
		/// ポリゴンのための輪郭データを生成する。
		/// </summary>
		/// <param name="outlineList"></param>
		/// <param name="param"></param>
		/// <remarks>ここで生成する輪郭データは内側に重なっていても良いものとする</remarks>
		public virtual void GenerateOutlineData(LinkedList<MbeGapChkObjLine> outlineList, GenOutlineParam param)
		{
		}

		public virtual void GenerateGapChkData(LinkedList<MbeGapChkObj> chkObjList,int _netNum)
		//public virtual void GenerateGapChkData(MbeGapChk gapChk,int _netNum)
		{
		}


		/// <summary>
		/// Mb3ファイルの読み込み時のメンバーの解釈を行う
		/// </summary>
		/// <param name="str1">変数名または"+"で始まるブロックタグ</param>
		/// <param name="str2">変数値</param>
		/// <param name="readCE3">ブロック読み込み時に使うReadCE3クラス</param>
		/// <returns>正常終了時にReadCE3.RdStatus.NoErrorを返す</returns>
		public virtual ReadCE3.RdStatus RdMb3Member(string str1, string str2, ReadCE3 readCE3)
		{
			//ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;

			switch (str1) {
				case "LAYER":
					Layer = MbeLayer.GetLayerValue(str2);
					return ReadCE3.RdStatus.NoError;
				case "SIGNAME":
					SigName = ReadCE3.DecodeCE3String(str2);
					return ReadCE3.RdStatus.NoError;
			}

			if ((str1.Substring(1, 3) == "POS") && (str1.Length > 4)) {
				string strIndex = str1.Substring(4);
				int index;
				int value;
				try {
					index = Convert.ToInt32(strIndex);
					value = Convert.ToInt32(str2);
				}
				catch (Exception) {
					return ReadCE3.RdStatus.FormatError;
				}
				if (str1[0] == 'X') {
					SetXPos(value, index);
					return ReadCE3.RdStatus.NoError;
				} else if (str1[0] == 'Y') {
					SetYPos(value, index);
					return ReadCE3.RdStatus.NoError;
				}
			}

			
			if (str1[0] == '+' && str1.Length >= 2) {
				string strSkipTo = "-" + str1.Substring(1);
				if (!readCE3.SkipTo(strSkipTo)) return ReadCE3.RdStatus.FileError;
			}

			return ReadCE3.RdStatus.NoError;
		}

		/// <summary>
		/// このクラスのMb3ファイルの読み込み
		/// </summary>
		/// <param name="readCE3">読み込み対象のReadCE3クラス</param>
		/// <returns>正常終了時にReadCE3.RdStatus.NoErrorを返す</returns>
		public abstract ReadCE3.RdStatus RdMb3(ReadCE3 readCE3);

		/// <summary>
		/// Mb3ファイルへメンバーの書き込み
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public virtual bool WrMb3Member(WriteCE3 writeCE3,Point origin)
		{
			writeCE3.WriteRecordString("LAYER", MbeLayer.GetLayerName(layer));
			for (int i = 0; i < posCount; i++) {
				writeCE3.WriteRecordInt(string.Format("XPOS{0}", i), posArray[i].X - origin.X);
				writeCE3.WriteRecordInt(string.Format("YPOS{0}", i), posArray[i].Y - origin.Y);
			}
			writeCE3.WriteRecordString("SIGNAME", signame);
			return true;
		}
		
		/// <summary>
		/// Mb3ファイルへの書き込み
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public abstract bool WrMb3(WriteCE3 writeCE3,Point origin);

		/// <summary>
		/// 複製を行う
		/// </summary>
		/// <returns></returns>
		public abstract MbeObj Duplicate();

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
		public virtual bool SelectIt(MbeRect rc, ulong layerMask, bool pointMode)
		{
			if ((layerMask & (ulong)layer) == 0) return false;
			if (DeleteCount >= 0) return false;

			bool result = false;
			for (int i = 0; i < posCount; i++) {
				if (rc.Contains(posArray[i])) {
					selectFlag[i] = true;
					result = true;
				}
			}
			return result;
		}


		/// <summary>
		/// ConChkのための種の取得
		/// </summary>
		/// <param name="rc"></param>
		/// <param name="layerMask"></param>
		/// <returns></returns>
		public virtual MbeObj ConChkSeed(MbeRect rc, ulong layerMask)
		{
			return null;
		}

		/// <summary>
		/// 線分に点が近いかどうかのチェック
		/// </summary>
		/// <param name="lineEnd1">線分の端点1</param>
		/// <param name="lineEnd2">線分の端点2</param>
		/// <param name="pt">分割可能か問い合わせる点座標</param>
		/// <param name="threshold">水平垂直の距離閾値</param>
		/// <param name="ptDivAt">分割点</param>
		/// <returns></returns>
		public static bool IsNearLine(Point lineEnd1, Point lineEnd2, Point pt, int threshold, out Point ptDivAt)
		{
			ptDivAt = pt;
			int x1 = lineEnd1.X;
			int y1 = lineEnd1.Y;
			int x2 = lineEnd2.X;
			int y2 = lineEnd2.Y;
			int xp = pt.X;
			int yp = pt.Y;
			if (x1 == x2) {			// 線分は垂直
				if (y1 < y2) {
					if (yp <= y1 || y2 <= yp) return false;
				} else {
					if (yp <= y2 || y1 <= yp) return false;
				}
				if (Math.Abs(xp - x1) > threshold) return false;
				ptDivAt = new Point(x1, yp);
				return true;
			} else if (y1 == y2) {	// 線分は水平
				if (x1 < x2) {
					if (xp <= x1 || x2 <= xp) return false;
				} else {
					if (xp <= x2 || x1 <= xp) return false;
				}
				if (Math.Abs(yp - y1) > threshold) return false;
				ptDivAt = new Point(xp, y1);
				return true;
			} else {
				double a1 = (double)(y2 - y1) / (double)(x2 - x1);
				double b1 = (double)y1 - a1 * x1;
				double a2 = -1.0 / a1;
				double b2 = (double)yp - a2 * xp;
				double xc = (double)(b2 - b1) / (double)(a1 - a2);
				if (x1 < x2) {
					if (xc <= x1 || x2 <= xc) return false;
				} else {
					if (xc <= x2 || x1 <= xc) return false;
				}
				double yc = a1 * xc + b1;
					if (Math.Abs(xp - xc) > threshold || Math.Abs(yp - yc) > threshold) return false;
                    ptDivAt = new Point((int)Math.Round(xc), (int)Math.Round(yc));
				return true;
			}
		}



		/// <summary>
		/// 分割可能かどうかを返す。線分系オブジェクトで意味を持つ。
		/// </summary>
		/// <param name="pt">問い合わせる点</param>
		/// <param name="lineIndex">分割可能な線のインデックス。単純ラインなら常に0</param>
		/// <param name="ptDivAt">分割するポイント</param>
		/// <returns></returns>
		public virtual bool CanDivide(Point pt,ulong visibleLayer,int threshold, out int lineIndex, out Point ptDivAt)
		{
			ptDivAt = pt;
			lineIndex = 0;
			return false;
		}

        ///// <summary>
        ///// 中央で分割。線分系オブジェクトで意味を持つ。
        ///// </summary>
        ///// <param name="lineIndex">分割可能な線のインデックス。単純ラインなら無視。</param>
        ///// <param name="ptDivAt"></param>
        ///// <returns></returns>
        //public virtual bool DivideAtCenter(int lineIndex, out MbeObj newObj)
        //{
        //    newObj = null;
        //    return false;
        //}

		/// <summary>
		/// 指定点で分割。線分系オブジェクトで意味を持つ。
		/// </summary>
		/// <param name="lineIndex">分割可能な線のインデックス。単純ラインなら無視。</param>
		/// <param name="ptDivAt"></param>
		/// <returns></returns>
		public virtual bool DivideAtPoint(int lineIndex,Point pt, out MbeObj newObj)
		{
			newObj = null;
			return false;
		}



		/// <summary>
		/// 図面要素の座標を描画座標に変換する
		/// </summary>
		/// <param name="point">図面要素の座標</param>
		/// <param name="scale">縮小率</param>
		/// <returns>描画座標</returns>
		/// <remarks>
		/// MbeViewのRealToDrawと同等機能
		/// 描画座標は、図面原点を(0,0)とする。
		/// </remarks>
		public static Point ToDrawDim(Point point, double scale)
		{
            return new Point((int)Math.Round(point.X / scale), (int)Math.Round(-point.Y / scale));
		}

        public static int ToDrawDim(int value, double scale)
		{
            return (int)Math.Round((double)value / scale);
		}

		/// <summary>
		/// 描画(abstract メソッド)
		/// </summary>
		/// <param name="dp"></param>
		public abstract void Draw(DrawParam dp);

		/// <summary>
		/// スナップマークの描画設定
		/// </summary>
		//public static bool DrawSnapMarkFlag
		//{
		//    get { return MbeObjPin.drawSnapMark; }
		//    set { MbeObjPin.drawSnapMark = value; }
		//}






		/// <summary>
		/// ピン番号の描画設定
		/// </summary>
		public static bool DrawPinNumFlag
		{
			get { return MbeObjPin.drawPinNum; }
			set { MbeObjPin.drawPinNum = value; }
		}


		/// <summary>
		/// スナップマークの描画
		/// </summary>
		/// <param name="g"></param>
		/// <param name="pt"></param>
		/// <param name="drawSize"></param>
		protected virtual void DrawSnapPointMark(Graphics g, Point pt, int drawSize,bool active)
		{
			//bool active = selectFlag[index];
			if (!active && drawSize < 10) return;
			if (active) {
				if (drawSize < 5) drawSize = 5;
			}
            Color col = (active ? MbeColors.ColorActiveSnapPoint : MbeColors.ColorSnapPoint);
            //Color col = (active ? Color.White : MbeColors.ColorSnapPoint);
			//int width = (active ? 3 : 1);
			//Pen pen = new Pen(col,width);
			Pen pen = new Pen(col,1);
			int n = drawSize / 2;
			g.DrawLine(pen, pt.X, pt.Y - n, pt.X, pt.Y + n);
			g.DrawLine(pen, pt.X - n, pt.Y, pt.X + n, pt.Y);
			pen.Dispose();
		}

		/// <summary>
		/// ターゲットマークの描画
		/// </summary>
		/// <param name="g"></param>
		/// <param name="pt"></param>
		/// <param name="drawSize"></param>
		protected virtual void DrawTargetMark(Graphics g, Point pt, int drawSize, bool active)
		{
			if (drawSize < 10) drawSize = 10;
			//drawSize = 1;
			Color col = (active ? MbeColors.ColorActiveSnapPoint : MbeColors.ColorSnapPoint);
            //Color col = (active ? Color.White : MbeColors.ColorSnapPoint);
            //int width = (active ? 3 : 1);
			//Pen pen = new Pen(col,width);
			Pen pen = new Pen(col, 1);
			int n = drawSize / 2;
			Rectangle rc = new Rectangle(pt.X - n, pt.Y - n, drawSize, drawSize);
			g.DrawEllipse(pen, rc);
			n = n * 15 / 10;
			g.DrawLine(pen, pt.X, pt.Y - n, pt.X, pt.Y + n);
			g.DrawLine(pen, pt.X - n, pt.Y, pt.X + n, pt.Y);
			pen.Dispose();
		}



		/// <summary>
		/// 接続チェックでアクティブになっているかどうかを返す
		/// </summary>
		public bool ConnectionCheckActive
		{
			get { return connectionCheckActive; }
			//set { connectCheckActive = value; }
		}

		/// <summary>
		/// 接続チェックのアクティブ状態の設定
		/// </summary>
		/// <remarks>派生クラスで再定義しない限りアクティブにできない</remarks>
		public virtual void SetConnectCheck()
		{
			connectionCheckActive = false;
		}

		/// <summary>
		/// 接続チェックのアクティブ状態のクリア
		/// </summary>
		public virtual void ClearConnectCheck()
		{
			connectionCheckActive = false;
		}

		public string TempPropString
		{
			get { return strTempProp; }
			set { strTempProp = value; }
		}


        /// <summary>
        /// 描画範囲を得る
        /// </summary>
        /// <returns></returns>
        public virtual MbeRect OccupationRect()
        {
            MbeRect rc = new MbeRect();
            rc = new MbeRect(GetPos(0), GetPos(0));

            for (int i = 1; i < PosCount; i++) {
                rc.Or(GetPos(i));
            }
            return rc;
        }

        /// <summary>
        /// センターポンチモードのときのドリル径を決める
        /// </summary>
        public static int PrintCenterPunchModeDiameter(int drill)
        {
            return (drill < 4000 ? drill : 4000);            
        }


 


	}

	class MbeObjIO
	{
		
		/// <summary>
		/// ReadCE3のストリームから、startWordで始まるMbeObjを読み取る
		/// </summary>
		/// <param name="readMb3"></param>
		/// <param name="startWord"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static ReadCE3.RdStatus ReadMbeObj(ReadCE3 readMb3, string startWord, out MbeObj obj)
		{
			ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;
			obj = null;
			if (startWord == "+MBE_HOLE") {
				obj = new MbeObjHole();
			} else if (startWord == "+MBE_PTH") {
				obj = new MbeObjPTH();
			} else if (startWord == "+MBE_PINSMD") {
				obj = new MbeObjPinSMD(true);
            } else if (startWord == "+MBE_FLASHMARK") {
                obj = new MbeObjPinSMD(false);
            } else if (startWord == "+MBE_LINE") {
				obj = new MbeObjLine();
			} else if (startWord == "+MBE_POLYGON") {
				obj = new MbeObjPolygon();
			} else if (startWord == "+MBE_TEXT") {
				obj = new MbeObjText();
			} else if (startWord == "+MBE_ARC") {
				obj = new MbeObjArc();
			} else if (startWord == "+MBE_COMPONENT") {
				obj = new MbeObjComponent();
			} else {
				string strSkipTo = "-" + startWord.Substring(1);
				readMb3.SkipTo(strSkipTo);
			}

			if (obj != null) {
				result = obj.RdMb3(readMb3);
				if (result != ReadCE3.RdStatus.NoError) {
					obj = null;
				}
			}
			return result;
		}
	}
}
