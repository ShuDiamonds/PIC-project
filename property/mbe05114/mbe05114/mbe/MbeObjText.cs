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
	public class MbeObjText : MbeObj
	{
		public const int MIN_LINE_WIDTH = 1500;
		public const int MAX_LINE_WIDTH = 10000;
		public const int DEFAULT_LINE_WIDTH = 2540;	//10mil
		public const int MIN_TEXT_HEIGHT = 10000;
		public const int MAX_TEXT_HEIGHT = 100000;
		public const int DEFAULT_TEXT_HEIGHT = 20000;

		/// <summary>
		/// ライン幅の設定と取得 
		/// </summary>
		public int LineWidth
		{
			get { return lineWidth; }
			set
			{
				lineWidth = value;
				if (lineWidth < MIN_LINE_WIDTH) lineWidth = MIN_LINE_WIDTH;
				else if (lineWidth > MAX_LINE_WIDTH) lineWidth = MAX_LINE_WIDTH;
			}
		}

		/// <summary>
		/// 文字高さの設定と取得
		/// </summary>
		public int TextHeight
		{
			get { return textHeight; }
			set 
			{ 
				textHeight = value;
				if (textHeight < MIN_TEXT_HEIGHT) textHeight = MIN_TEXT_HEIGHT;
				else if (textHeight > MAX_TEXT_HEIGHT) textHeight = MAX_TEXT_HEIGHT;
			}
		}

		/// <summary>
		/// 垂直設定
		/// </summary>
		//public bool Vertical
		//{
		//    set { vertical = value; }
		//    get { return vertical; }
		//}

		public int Dir
		{
			get { return dir; }
			set
			{
				dir = value % 360;
				if (dir < 0) dir += 360;
			}
		}



		///// <summary>
		///// 文字列の設定と取得
		///// </summary>
		//public string Text
		//{
		//    set { text = value; }
		//    get { return text; }
		//}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MbeObjText()
		{
			posCount = 1;
			posArray = new Point[posCount];
			selectFlag = new bool[posCount];
			selectFlag[0] = false;

			lineWidth = DEFAULT_LINE_WIDTH;
			textHeight = DEFAULT_TEXT_HEIGHT;
			dir = 0;
			//vertical = false;
			//text = "";
		}

		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="mbeObjPin"></param>
		public MbeObjText(MbeObjText mbeObjText)
			: base(mbeObjText)
		{
			lineWidth = mbeObjText.lineWidth;
			textHeight = mbeObjText.textHeight;
			Dir = mbeObjText.dir;
			//vertical = mbeObjText.vertical;
			//text = mbeObjText.text;
		}

		/// <summary>
		/// 有効かどうかを返す
		/// </summary>
		/// <returns></returns>
		public override bool IsValid()
		{
			if (signame.Length == 0) return false;
			return true;
		}

		/// <summary>
		/// 図面オブジェクトごとのID値を返す
		/// </summary>
		public override MbeObjID Id()
		{
			return MbeObjID.MbeText;
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
				snapLayer = (ulong)layer;
			}
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
		/// Mb3ファイルの読み込み時のメンバーの解釈を行う
		/// </summary>
		/// <param name="str1">変数名または"+"で始まるブロックタグ</param>
		/// <param name="str2">変数値</param>
		/// <param name="readCE3">ブロック読み込み時に使うReadCE3クラス</param>
		/// <returns>正常終了時にReadCE3.RdStatus.NoErrorを返す</returns>
		public override ReadCE3.RdStatus RdMb3Member(string str1, string str2, ReadCE3 readCE3)
		{
			switch (str1) {
				case "WIDTH":
					try { LineWidth = Convert.ToInt32(str2); }
					catch (Exception) { LineWidth = DEFAULT_LINE_WIDTH; }
					return ReadCE3.RdStatus.NoError;
				case "HEIGHT":
					try { TextHeight = Convert.ToInt32(str2); }
					catch (Exception) { TextHeight = DEFAULT_TEXT_HEIGHT; }
					return ReadCE3.RdStatus.NoError;
				case "DIR": 
					{
						//int n = 0;
						try { Dir = Convert.ToInt32(str2); }
						catch (Exception) { Dir = 0; }
						//Vertical = (n != 0);
						return ReadCE3.RdStatus.NoError;
					}

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
					if (str1 != "-MBE_TEXT") {
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
			base.WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecordInt("WIDTH", LineWidth);
			writeCE3.WriteRecordInt("HEIGHT",TextHeight);
			writeCE3.WriteRecordInt("DIR", Dir);
			//writeCE3.WriteRecordInt("DIR", (Vertical ? 90:0));
			return true;
		}

		/// <summary>
		/// 回転
		/// </summary>
		/// <param name="selectedOnly"></param>
		/// <param name="ptCenter"></param>
		public override void Rotate90(bool selectedOnly, Point ptCenter)
		{
			base.Rotate90(selectedOnly, ptCenter);
			Dir = dir + 90;
			//vertical = !vertical;
		}


        
        /// <summary>
        /// Flip動作のバグのパッチ。
        /// </summary>
        /// <param name="hCenter"></param>
        public override void Flip(int hCenter)
        {
            base.Flip(hCenter);
            int angle = dir % 360;
            Dir = 360 - angle;
        }


		/// <summary>
		/// Mb3ファイルへの書き込み
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public override bool WrMb3(WriteCE3 writeCE3, Point origin)
		{
			writeCE3.WriteRecord("+MBE_TEXT");
			WrMb3Member(writeCE3, origin);
			writeCE3.WriteRecord("-MBE_TEXT");
			writeCE3.WriteNewLine();
			return true;
		}

		/// <summary>
		/// 複製を行う
		/// </summary>
		/// <returns></returns>
		public override MbeObj Duplicate()
		{
			MbeObj newObj = new MbeObjText(this);
			return newObj;
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
			pt0 = ToDrawDim(pt0, dp.scale);

			Color col;
			int w = 0;
			int h = 0;
			//bool reverse = false;

			//描画幅の設定
			w = (int)(LineWidth / dp.scale) | 1;

			//描画文字高さの設定
			h = (int)(TextHeight / dp.scale);

			if (dp.mode == MbeDrawMode.Print) {
                if ((dp.option & (uint)MbeDrawOption.PrintColor) != 0) {    //カラー印刷
                    uint nColor;
                    switch (dp.layer) {
                        case MbeLayer.LayerValue.PLC:
                            nColor = MbeColors.PRINT_PLC;
                            break;
                        case MbeLayer.LayerValue.PLS:
                            nColor = MbeColors.PRINT_PLS;
                            //reverse = true;
                            break;
                        case MbeLayer.LayerValue.STC:
                            nColor = MbeColors.PRINT_STC;
                            break;
                        case MbeLayer.LayerValue.STS:
                            nColor = MbeColors.PRINT_STS;
                            //reverse = true;
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
                            //reverse = true;
                            break;
                        case MbeLayer.LayerValue.DIM:
                            nColor = MbeColors.PRINT_DIM;
                            break;
                        default:
                            nColor = MbeColors.PRINT_DOC;
                            break;
                    }
                    col = Color.FromArgb(unchecked((int)nColor));
                    col = Color.FromArgb(255, col);
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
						//reverse = true;
						break;
					case MbeLayer.LayerValue.STC:
						nColor = MbeColors.STC;
						break;
					case MbeLayer.LayerValue.STS:
						nColor = MbeColors.STS;
						//reverse = true;
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
						//reverse = true;
						break;
					case MbeLayer.LayerValue.DIM:
						nColor = MbeColors.DIM;
						break;
					default:
						nColor = MbeColors.DOC;
						break;
				}

				if (dp.mode == MbeDrawMode.Temp) {
					col = Color.FromArgb(255, MbeColors.HighLighten(nColor));
					//col = Color.FromArgb(MBE_OBJ_ALPHA, MbeColors.HighLighten(nColor));
				} else {
					col = Color.FromArgb(unchecked((int)nColor));
					col = Color.FromArgb(255, col);
				}

			}

			bool reverse = false;
			switch (dp.layer) {
				case MbeLayer.LayerValue.PLS:
				case MbeLayer.LayerValue.STS:
				case MbeLayer.LayerValue.SOL:
					reverse = true;
					break;
				default:
					reverse = false;
					break;
			}

            //文字列描画にアルファブレンドを使うかどうかの判定
            bool bDrawUsingAlphaBlend = true;
            if (Program.drawTextSolidly) {
                bDrawUsingAlphaBlend = false;
            } else if (dp.mode == MbeDrawMode.Print) {
                if ((dp.option & (uint)MbeDrawOption.PrintColor) == 0) { //白黒印刷時はアルファブレンドにしない
                    bDrawUsingAlphaBlend = false;
                }
            }


			if (dp.layer != MbeLayer.LayerValue.DOC) {
				GraphicsState gState = dp.g.Save();	//座標系保存


                dp.g.TranslateTransform(pt0.X, pt0.Y);
                if (dir != 0) {
                    dp.g.RotateTransform(-dir);
                }
                if (reverse) {
                    dp.g.ScaleTransform(-1.0F, 1.0F);
                }

 

                if (bDrawUsingAlphaBlend) {
                    int drawWidth = MbeBoardFont.DrawWidth(h, signame);
                    int buffCX = drawWidth + w + w;
                    int buffCY = h+w+w;
                    Bitmap bmp = new Bitmap(buffCX, buffCY);

                    
                    Graphics gBmp = Graphics.FromImage(bmp);
                    Color colBg = Color.FromArgb(unchecked((int)MbeColors.Background));
                    gBmp.Clear(colBg);
                    MbeView.boardFont.DrawString(gBmp, w, h+w, false, signame, h, w, col);

                    ColorMatrix cm = new ColorMatrix();
                    cm.Matrix00 = 1.0F;
                    cm.Matrix11 = 1.0F;
                    cm.Matrix22 = 1.0F;
                    cm.Matrix33 = 0.5F;
                    cm.Matrix44 = 1.0F;

                    


                    ImageAttributes ia = new ImageAttributes();
                    ia.SetColorMatrix(cm);
                    ia.SetColorKey(colBg, colBg);

                    Rectangle rc = new Rectangle(-w, w-buffCY, buffCX, buffCY);
                    dp.g.DrawImage(bmp, rc, 0, 0, buffCX, buffCY, GraphicsUnit.Pixel, ia);




                } else {
                    MbeView.boardFont.DrawString(dp.g, 0, 0, false, signame, h, w, col);
                }
                dp.g.Restore(gState);					//座標系復帰

			} else {
                if (bDrawUsingAlphaBlend) {
                    col = Color.FromArgb(MBE_OBJ_ALPHA, col);
                }

                if (h >= 2) {
					Font font = new Font(FontFamily.GenericSerif, h, GraphicsUnit.Pixel);
					Brush brush = new SolidBrush(col);
					StringFormat format = new StringFormat(StringFormatFlags.NoClip);
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Far;

					GraphicsState gState = dp.g.Save();	//座標系保存
					dp.g.TranslateTransform(pt0.X, pt0.Y);
					if (dir != 0) {
						dp.g.RotateTransform(-dir);
					}
					dp.g.DrawString(signame, font, brush, new PointF(0, 0), format);
					dp.g.Restore(gState);					//座標系復帰

					format.Dispose();
					brush.Dispose();
					font.Dispose();
				}
			}
			

			if (drawSnapMark && dp.mode != MbeDrawMode.Print) {
				int marksize = h / 3;
				if (marksize > 20) marksize = 20;
				DrawSnapPointMark(dp.g, pt0, marksize, selectFlag[0]);
			}
		}

        public override MbeRect OccupationRect()
		{
			LinkedList<CamOutBaseData> camdataLList = new LinkedList<CamOutBaseData>();
			bool reverse;
			switch (layer) {
				case MbeLayer.LayerValue.PLS:
				case MbeLayer.LayerValue.STS:
				case MbeLayer.LayerValue.SOL:
					reverse = true;
					break;
				default:
					reverse = false;
					break;
			}
			MbeView.boardFont.GenerateCamDataString(camdataLList, 0, 0, reverse, signame, TextHeight, LineWidth);
			int l = System.Int32.MaxValue;
			int t = System.Int32.MinValue;
			int r = System.Int32.MinValue;
			int b = System.Int32.MaxValue;
			Point ptz = new Point(0, 0);
			Point ptOrigin = GetPos(0);
			foreach (CamOutBaseData camd in camdataLList) {
				if (dir != 0) {
					camd.RotateStep90(dir, ptz);
				}
				camd.Move(ptOrigin);
				if (camd.ctype == CamOutBaseData.CamType.VECTOR) {
					if (l > camd.pt0.X) l = camd.pt0.X;
					if (l > camd.pt1.X) l = camd.pt1.X;
					if (r < camd.pt0.X) r = camd.pt0.X;
					if (r < camd.pt1.X) r = camd.pt1.X;

					if (b > camd.pt0.Y) b = camd.pt0.Y;
					if (b > camd.pt1.Y) b = camd.pt1.Y;
					if (t < camd.pt0.Y) t = camd.pt0.Y;
					if (t < camd.pt1.Y) t = camd.pt1.Y;
				}
			}
			l -= LineWidth/2;
			t += LineWidth / 2;
			r += LineWidth / 2;
			b -= LineWidth / 2;
			MbeRect rc = new MbeRect(new Point(l, t), new Point(r, b));
			return rc;
		}

		/// <summary>
		/// ポリゴンのための輪郭データを生成する。
		/// </summary>
		/// <param name="outlineList"></param>
		/// <param name="param"></param>
		/// <remarks>ここで生成する輪郭データは内側に重なっていても良いものとする
		/// 文字列をマジメに一画ごとに処理すると、線分の量が半端でなく大きくなる
		/// とりあえずは全体を矩形で囲む輪郭を生成する。
		/// </remarks>
		public override void GenerateOutlineData(LinkedList<MbeGapChkObjLine> outlineList, GenOutlineParam param)
		{
			if (layer != param.layer) return;

			MbeRect rc = OccupationRect();

			//LinkedList<CamOutBaseData> camdataLList = new LinkedList<CamOutBaseData>();

			//bool reverse;
			//switch (layer) {
			//    case MbeLayer.LayerValue.PLS:
			//    case MbeLayer.LayerValue.STS:
			//    case MbeLayer.LayerValue.SOL:
			//        reverse = true;
			//        break;
			//    default:
			//        reverse = false;
			//        break;
			//}
			//MbeView.boardFont.GenerateCamDataString(camdataLList, 0, 0, reverse, signame, TextHeight, LineWidth);
			//int l = System.Int32.MaxValue;
			//int t = System.Int32.MinValue;
			//int r = System.Int32.MinValue;
			//int b = System.Int32.MaxValue;
			//Point ptz = new Point(0, 0);
			//Point ptOrigin = GetPos(0);
			//foreach (CamOutBaseData camd in camdataLList) {
			//    if (dir != 0) {
			//        camd.RotateStep90(dir, ptz);
			//    }
			//    camd.Move(ptOrigin);
			//    if (camd.ctype == CamOutBaseData.CamType.VECTOR) {
			//        if (l > camd.pt0.X) l = camd.pt0.X;
			//        if (l > camd.pt1.X) l = camd.pt1.X;
			//        if (r < camd.pt0.X) r = camd.pt0.X;
			//        if (r < camd.pt1.X) r = camd.pt1.X;

			//        if (b > camd.pt0.Y) b = camd.pt0.Y;
			//        if (b > camd.pt1.Y) b = camd.pt1.Y;
			//        if (t < camd.pt0.Y) t = camd.pt0.Y;
			//        if (t < camd.pt1.Y) t = camd.pt1.Y;
			//    }
			//}
			int distance = param.traceWidth/ 2 + param.gap;

			int l = rc.L - distance;
			int t = rc.T + distance;
			int r = rc.R + distance;
			int b = rc.B - distance;
			Point[] pt = new Point[4];
			pt[0] = new Point(l, t);
			pt[1] = new Point(r, t);
			pt[2] = new Point(r, b);
			pt[3] = new Point(l, b);
			for (int j = 0; j < 4; j++) {
				int j2 = j + 1;
				if (j2 == 4) {
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
		/// CAMデータの生成 
		/// </summary>
		/// <param name="camOut"></param>
		public override void GenerateCamData(CamOut camOut)
		{
			if (layer == MbeLayer.LayerValue.DOC) return;
			LinkedList<CamOutBaseData> camdataLList = new LinkedList<CamOutBaseData>();
			bool reverse;
			switch (layer) {
				case MbeLayer.LayerValue.PLS:
				case MbeLayer.LayerValue.STS:
				case MbeLayer.LayerValue.SOL:
					reverse = true;
					break;
				default:
					reverse = false;
					break;
			}
			MbeView.boardFont.GenerateCamDataString(camdataLList, 0, 0, reverse, signame, TextHeight, LineWidth);
			Point ptz= new Point(0, 0);
			Point pt = GetPos(0);
			foreach (CamOutBaseData camd in camdataLList) {
				if (dir!=0) {
					camd.RotateStep90(dir,ptz);
				}
				camd.Move(pt);
				camd.layer = layer;
				camOut.Add(camd);
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
				bool reverse = (layer == MbeLayer.LayerValue.SOL);
				LinkedList<CamOutBaseData> camdataLList = new LinkedList<CamOutBaseData>();

				MbeView.boardFont.GenerateCamDataString(camdataLList, 0, 0, reverse, signame, TextHeight, LineWidth);
				Point ptz = new Point(0, 0);
				Point pt = GetPos(0);
				foreach (CamOutBaseData camd in camdataLList) {
					if (dir != 0) {
						camd.RotateStep90(dir, ptz);
					}
					camd.Move(pt);
					if (camd.pt0.Equals(camd.pt1)) {
						MbeGapChkObjPoint gapChkObj = new MbeGapChkObjPoint();
						gapChkObj.layer = layer;
						gapChkObj.netNum = _netNum;
						gapChkObj.mbeObj = this;
						gapChkObj.SetPointValue(camd.pt0, LineWidth);
						//gapChk.Add(gapChkObj);
						chkObjList.AddLast(gapChkObj);
					} else {
						MbeGapChkObjLine gapChkObj = new MbeGapChkObjLine();
						gapChkObj.layer = layer;
						gapChkObj.netNum = _netNum;
						gapChkObj.mbeObj = this;
						gapChkObj.SetLineValue(camd.pt0, camd.pt1, LineWidth);
						//gapChk.Add(gapChkObj);
						chkObjList.AddLast(gapChkObj);
					}
				}
			}
		}


 
		//protected string text;
		protected int lineWidth;
		protected int textHeight;
		//protected bool vertical;
		protected int dir;

	}
}
