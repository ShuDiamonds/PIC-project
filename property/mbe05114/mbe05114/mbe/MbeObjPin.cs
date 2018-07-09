using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;

using CE3IO;

namespace mbe
{
	public abstract class MbeObjPin : MbeObj
	{
		protected static readonly string[] padShapeName = {
			"RECT",
			"OBROUND"	//追加する場合でもOBROUNDが常に最後になるようにする。
		};

		public enum PadShape
		{
			ERR = -1,
			Rect,
			Obround	//追加する場合でもObroundが常に最後になるようにする。
		}

        /// <summary>
        /// サーマルリリーフの種類
        /// </summary>
        protected static readonly string[] thermalReliefTypeName = {
           "THMLRLFINCOMP",//部品化したときサーマルリリーフとする
           "SOLID"         //常にソリッド接続
        };



        /// <summary>
        /// サーマルリリーフの種類
        /// </summary>
        public enum PadThermalRelief
        {
            ERR = -1,
            ThmlRlfInComp,       //部品化したときサーマルリリーフとする
            Solid                //常にソリッド接続
        }

		public const int MIN_PAD_SIZE =   2000; //0.2mm
		public const int MAX_PAD_SIZE = 100000; //10mm


        public PadThermalRelief ThermalRelief
        {
            get { return thermalRelief; }
            set { thermalRelief = value; }
        }


		/// <summary>
		/// ソルダレジストのマージン初期値
		/// </summary>
		protected const int DEFAULT_SRMARGIN = 1000; //0.1mm

		/// <summary>
		/// ソルダレジストのマージン最大値
		/// </summary>
		protected const int MAX_SRMARGIN = 10000; //1.0mm

		/// <summary>
		/// ソルダレジストのマージン最小値
		/// </summary>
		protected const int MIX_SRMARGIN = -1000; //-0.1mm

        public const int DEFAULT_WIDTH = 16000;
        public const int DEFAULT_HEIGHT = 16000;

        protected bool no_ResistMask;    //レジストを開けない

        public bool No_ResistMask
        {
            get { return no_ResistMask; }
            set { no_ResistMask = value; }
        }


		/// <summary>
		/// PadShape値に対応するパッド形状名を得る
		/// </summary>
		/// <param name="layer"></param>
		/// <returns></returns>
		public static string GetPadShapeName(PadShape shape)
		{
			if (shape == PadShape.ERR) {
				return "";
			}
			return padShapeName[(int)shape];
		}

		/// <summary>
		/// name に対応する PadShapeを返す。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static PadShape GetPadShapeValue(string name)
		{
			int namesCount = padShapeName.Length;
			for (int i = 0; i < namesCount; i++) {
				if (padShapeName[i] == name) {
					return (PadShape)i;
				}
			}
			return PadShape.ERR;
		}

        /// <summary>
        /// thmlrlf 値に対応するサーマルリリーフタイプ文字列を返す
        /// </summary>
        /// <param name="thmlrlf"></param>
        /// <returns></returns>
        public static string GetThermalReliefTypeName(PadThermalRelief thmlrlf)
		{
			if (thmlrlf == PadThermalRelief.ERR) {
				return "";
			}
			return thermalReliefTypeName[(int)thmlrlf];
		}

        /// <summary>
        /// サーマルリリーフ文字列に対応する値を返す
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PadThermalRelief GetThermalReliefTypeValue(string name)
        {
            int namesCount = thermalReliefTypeName.Length;
			for (int i = 0; i < namesCount; i++) {
				if (thermalReliefTypeName[i] == name) {
					return (PadThermalRelief)i;
				}
			}
			return PadThermalRelief.ERR;
        }





		protected Size padSize;
		protected PadShape shape;
		protected int srmargin;
		protected string num;
        private PadThermalRelief thermalRelief;


		
				

		/// <summary>
		/// コンストラクタ
		/// </summary>
		protected MbeObjPin()
		{
			posCount = 1;
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			selectFlag[0] = false;

			//padSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);
			//shape = PadShape.Obround;
			srmargin = DEFAULT_SRMARGIN;
			num = "";
            thermalRelief = PadThermalRelief.ThmlRlfInComp;
            no_ResistMask = false;
		}

		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="mbeObjPin"></param>
		protected MbeObjPin(MbeObjPin mbeObjPin)
			: base(mbeObjPin)
		{
			padSize = mbeObjPin.padSize;
			shape = mbeObjPin.shape;
			srmargin = mbeObjPin.srmargin;
			num = mbeObjPin.num;
            thermalRelief = mbeObjPin.thermalRelief;
            no_ResistMask = mbeObjPin.no_ResistMask;
		}


		/// <summary>
		/// 回転
		/// </summary>
		/// <param name="selectedOnly"></param>
		/// <param name="ptCenter"></param>
		public override void Rotate90(bool selectedOnly, Point ptCenter)
		{
			base.Rotate90(selectedOnly, ptCenter);
			int w = padSize.Width;
			int h = padSize.Height;
			padSize = new Size(h, w);
		}
		

		/// <summary>
		/// パッドサイズの取得と設定
		/// </summary>
		public Size PadSize
		{
			get
			{
				return padSize;
			}
			set
			{
				padSize = value;
			}
		}

		/// <summary>
		/// パッドの幅の取得と設定
		/// </summary>
		public int Width
		{
			get
			{
				return padSize.Width;
			}
			set
			{
				if (value < 0) value = 0;
				padSize.Width = value;
			}
		}

		/// <summary>
		/// パッドの高さの取得と設定
		/// </summary>
		public int Height
		{
			get
			{
				return padSize.Height;
			}
			set
			{
				if (value < 0) value = 0;
				padSize.Height = value;
			}
		}

		/// <summary>
		///  パッド形状の取得と設定
		/// </summary>
		public PadShape Shape
		{
			get
			{
				return shape;
			}
			set
			{
				if ((int)value < 0 || (int)value > (int)PadShape.Obround) {
					value = PadShape.Obround;
				}
				shape = value;
			}
		}

		/// <summary>
		/// ピン番号の取得と設定
		/// </summary>
		public string PinNum
		{
			get
			{
				return num;
			}
			set
			{
				num = value;
			}
		}

		/// <summary>
		/// ソルダレジストマージンの設定
		/// </summary>
		public int SrMargin
		{
			get { return srmargin; }
			set
			{
				srmargin = value;
				if (srmargin < MIX_SRMARGIN) srmargin = MIX_SRMARGIN;
				else if (srmargin > MAX_SRMARGIN) srmargin = MAX_SRMARGIN;
			}
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

			if (!pointMode) {
				return base.SelectIt(rc, layerMask, pointMode);
			}

			int areaWidth = 0;
			int areaHeight = 0;
			Point ptC = rc.Center();

			//まず、パッドの矩形範囲内に入っているかどうかのチェック
			//長円パッドでは、両端の半円を除くエリアをチェックする
			if (Shape == PadShape.Obround) {
				if (Height < Width) {
					areaHeight = Height;
					areaWidth = Width - Height;
				} else if (Height > Width) {
					areaHeight = Height - Width;
					areaWidth = Width;
				} else {
					//幅と高さが同じ場合は、矩形エリアが存在しない。
					//中心点から検査点までの距離が半径内に収まっているかどうかのチェックだけ
					if(Util.DistancePointPoint(ptC,GetPos(0))<=Width/2){
						selectFlag[0] = true;
						return true;
					}else{
						return false;
					}
				}
			} else {	//矩形パッドなら矩形パッド全体でいい
				areaHeight = Height;
				areaWidth = Width;
			}

			int x = GetPos(0).X;
			int y = GetPos(0).Y;
			int l = x - areaWidth / 2;
			int r = x + areaWidth / 2;
			int t = y + areaHeight / 2;
			int b = y - areaHeight / 2;


			//矩形エリア内チェック
			if (l <= ptC.X && ptC.X <= r && b <= ptC.Y && ptC.Y < t) {
				selectFlag[0] = true;
				return true;
			}

			//長円の場合は、両端半円内チェックを行う。
			if (Shape == PadShape.Obround) {
				int x0 = 0;
				int y0 = 0;
				int x1 = 0;
				int y1 = 0;
				int rad = 0;	//半径
				if (Height < Width) {
					rad = Height /2;
					x0 = l;
					x1 = r;
					y0 = y1 = y;
				}else if(Height > Width){
					rad = Width /2;
					x0 = x1 = x;
					y0 = b;
					y1 = t;
				}
				if( Util.DistancePointPoint(ptC,new Point(x0,y0))<=rad ||
					Util.DistancePointPoint(ptC,new Point(x1,y1))<=rad){
					selectFlag[0] = true;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// ポリゴンのための輪郭データを生成する。
		/// </summary>
		/// <param name="outlineList"></param>
		/// <param name="param"></param>
		/// <remarks>ここで生成する輪郭データは内側に重なっていても良いものとする</remarks>
		public override void GenerateOutlineData(LinkedList<MbeGapChkObjLine> outlineList, GenOutlineParam param)
		{
			//if (param.layer != Layer) return;
			int distance;
			Point ptCenter = GetPos(0);
			Point[] pt;
			int pointCount;
			int x = ptCenter.X;
			int y = ptCenter.Y;


			if (shape == PadShape.Rect) {
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

				distance = (lineW + param.traceWidth) / 2 + param.gap*11/10;

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
		/// Mb3ファイルの読み込み時のメンバーの解釈を行う
		/// </summary>
		/// <param name="str1">変数名または"+"で始まるブロックタグ</param>
		/// <param name="str2">変数値</param>
		/// <param name="readCE3">ブロック読み込み時に使うReadCE3クラス</param>
		/// <returns>正常終了時にReadCE3.RdStatus.NoErrorを返す</returns>
		public override ReadCE3.RdStatus RdMb3Member(string str1, string str2, ReadCE3 readCE3)
		{
			switch(str1){
				case  "HEIGHT":
					try { Height = Convert.ToInt32(str2);}
					catch (Exception) { Height = DEFAULT_HEIGHT; }
					return ReadCE3.RdStatus.NoError;
				case "WIDTH":
					try { Width = Convert.ToInt32(str2); }
					catch (Exception) { Width = DEFAULT_WIDTH;}
					return ReadCE3.RdStatus.NoError;
				case "SHAPE":
					shape = GetPadShapeValue(str2);
                    if (shape == PadShape.ERR) {
                        shape = PadShape.Obround;
                    }
					return ReadCE3.RdStatus.NoError;
                case "THMLRLF":
                    thermalRelief = GetThermalReliefTypeValue(str2);
                    if (thermalRelief == PadThermalRelief.ERR) {
                        thermalRelief = PadThermalRelief.ThmlRlfInComp;
                    }
                    return ReadCE3.RdStatus.NoError;
                case "PINNUM":
					PinNum = ReadCE3.DecodeCE3String(str2);
					return ReadCE3.RdStatus.NoError;
				case "SRMARGIN":
					try { SrMargin = Convert.ToInt32(str2); }
					catch (Exception) { SrMargin = DEFAULT_SRMARGIN; }
					return ReadCE3.RdStatus.NoError;
                case "NO_RM":
                    try {
                        int n = Convert.ToInt32(str2);
                        no_ResistMask = (n != 0);
                    }
                    catch (Exception) { no_ResistMask = false; }
                    return ReadCE3.RdStatus.NoError;
				default:
					return base.RdMb3Member(str1, str2, readCE3);
			}
			//return true;
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
			writeCE3.WriteRecordInt("HEIGHT", Height);
			writeCE3.WriteRecordInt("WIDTH", Width);
			writeCE3.WriteRecordString("SHAPE", GetPadShapeName(shape));
            writeCE3.WriteRecordString("THMLRLF", GetThermalReliefTypeName(thermalRelief));
			writeCE3.WriteRecordString("PINNUM", PinNum);
			writeCE3.WriteRecordInt("SRMARGIN", SrMargin);
            if (no_ResistMask) {
                writeCE3.WriteRecordInt("NO_RM", 1);
            }
			return true;
		}

		

		/// <summary>
		/// ピン番号の描画
		/// </summary>
		/// <param name="g">描画対象</param>
		/// <param name="str">文字列</param>
		/// <param name="pt">位置(ここにセンタリングされる)</param>
		/// <param name="fontSize">フォントのピクセルサイズ</param>
		/// <param name="vertical">垂直に描画するときtrue</param>
		protected virtual void DrawPinNum(Graphics g, string str, Point pt, int fontSize, bool vertical)
		{
			if (fontSize < 10 || str.Length == 0) return;
			Font font = new Font(FontFamily.GenericSerif, fontSize, GraphicsUnit.Pixel);
			Brush brush = new SolidBrush(MbeColors.ColorPinNum);
			StringFormat format = new StringFormat(StringFormatFlags.NoClip);
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			//文字列は座標系を平行移動してから描画する
			GraphicsState gState = g.Save();	//座標系保存
			g.TranslateTransform(pt.X, pt.Y);
			if (vertical) {						//回転
				g.RotateTransform(-90F);
			}
			g.DrawString(str, font, brush, new PointF(0, 0),format);
			g.Restore(gState);					//座標系復帰

			format.Dispose();
			brush.Dispose();
			font.Dispose();
		}

		/// <summary>
		/// 接続チェックのアクティブ状態の設定
		/// </summary>
		public override void SetConnectCheck()
		{
			connectionCheckActive = true;
		}

        /// <summary>
        /// 描画範囲を得る
        /// </summary>
        /// <returns></returns>
        public override MbeRect OccupationRect()
        {
			
			int x = GetPos(0).X;
			int y = GetPos(0).Y;
			int l = x - Width / 2;
			int r = x + Width / 2;
			int t = y + Height / 2;
			int b = y - Height / 2;

            return new MbeRect(new Point(l, t), new Point(r, b));
        }

	}
}
