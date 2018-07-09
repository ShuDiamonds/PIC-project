using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace mbe
{
	/// <summary>
	/// 1文字分のフォントデータを扱うクラス
	/// </summary>
	class MbeBoardFontData
	{


		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MbeBoardFontData()
		{
			count = 0;
			pArray = null;
		}

		/// <summary>
		/// 文字列で1文字分のフォントを初期化する
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		/// <remarks>
		/// 文字列は16進数
		/// 先頭4文字がベクトル数
		/// 以後2文字ごとに(x0,y0),(x1,y1)。1ベクトルあたり4変数なので8文字
		/// </remarks>
		public bool SetData(string str)
		{
			count = 0;
			pArray = null;
			int length = str.Length;
			if (length < 4) return false;
			int n;
			try {
				n = Convert.ToInt32(str.Substring(0, 4), 16);
				if (n < 1) return false;
				if (length < (4 + 8 * n)) return false;
				pArray = new Point[n * 2];
				for (int i = 0; i < n; i++) {
					int x0;
					int y0;
					int x1;
					int y1;
					int m = 4 + i * 8;
					x0 = Convert.ToInt32(str.Substring(m, 2), 16);
					y0 = Convert.ToInt32(str.Substring(m + 2, 2), 16);
					x1 = Convert.ToInt32(str.Substring(m + 4, 2), 16);
					y1 = Convert.ToInt32(str.Substring(m + 6, 2), 16);
					pArray[i * 2] = new Point(x0, y0);
					pArray[i * 2 + 1] = new Point(x1, y1);
				}
			}
			catch {
				pArray = null;
				return false;
			}
			count = n;
			return true;
		}

		/// <summary>
		/// ベクトルの数を返す
		/// </summary>
		public int Count
		{
			get { return count; }
		}

		/// <summary>
		/// 指定したインデックスのラインデータを返す
		/// </summary>
		/// <param name="index"></param>
		/// <param name="pt0"></param>
		/// <param name="pt1"></param>
		public void LineData(int index, out Point pt0, out Point pt1)
		{
			if (index < 0 || index >= count) {
				pt0 = new Point(0, 0);
				pt1 = pt0;
			} else {
				pt0 = pArray[index * 2];
				pt1 = pArray[index * 2 + 1];
			}
		}

		private int count;
		private Point[] pArray;
	}


	/// <summary>
	/// 0x21～0x7Eの94文字ぶんのフォントを管理するクラス
	/// </summary>
	class MbeBoardFont
	{
		public MbeBoardFont()
		{
			fontData = new MbeBoardFontData[94];
			init();
			SetupEmbeddedFont();
		}

		public bool SetupEmbeddedFont()
		{
			ready = false;
			int lineCount = 0;
			for (lineCount = 0; lineCount < 94; lineCount++) {
				if (!fontData[lineCount].SetData(embeddedFont[lineCount])) break;
			}
			if (lineCount < 94) {
				init();
				return false;
			}
			ready = true;
			return true;
		}


		public bool ReadFontDataFile(string path)
		{
			//return true;

			ready = false;
			int lineCount=0;
			try {
				using (StreamReader reader = new StreamReader(path)) {
					string str;
					for (lineCount = 0; lineCount < 94; lineCount++) {
						if ((str = reader.ReadLine()) == null) break;
						if (!fontData[lineCount].SetData(str)) break;
					}
				}
			}
			catch {
			}
			if (lineCount < 94) {
				init();
				return false;
			}
			ready = true;
			return true;
		}

		public MbeBoardFontData[] fontData;

		public bool Ready
		{
			get { return ready; }
		}

		/// <summary>
		/// 1文字の描画
		/// </summary>
		/// <param name="g"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="reverse">左右反転</param>
		/// <param name="code">文字コード</param>
		/// <param name="height">高さピクセル単位</param>
		/// <param name="lineWidth">ライン幅ピクセル単位</param>
		/// <param name="col"></param>
		/// <remarks>
		/// フォントデータは高さ100を基準に作っている。
		/// nピクセル高さに出力するためには、n/100倍にすれば良いことになる。 
		/// </remarks>
		public void Draw(Graphics g, int x, int y,bool reverse, int code, int height, int lineWidth, Color col)
		{
			if (!Ready) return;
			if (code == 0x20) return;
			if (code < 0x21 || code > 0x7e) {
				code = '*';
			}
			double ymag = (double)height / 100.0;
			double xmag = (reverse ? -ymag : ymag);
			int index = code - 0x21;
			int vectCount = fontData[index].Count;
			Pen pen = new Pen(col, lineWidth);
			pen.StartCap = LineCap.Round;
			pen.EndCap = LineCap.Round;
			for (int i = 0; i < vectCount; i++) {
				Point pt0;
				Point pt1;
				fontData[index].LineData(i, out pt0, out pt1);
				int x0 = (int)(x + (double)pt0.X * xmag);
				int y0 = (int)(y - (double)pt0.Y * ymag);
				int x1 = (int)(x + (double)pt1.X * xmag);
				int y1 = (int)(y - (double)pt1.Y * ymag);
				g.DrawLine(pen, x0, y0, x1, y1);
			}
			pen.Dispose();
		}

        static public int DrawWidth(int height,string str)
        {
            return (int)(str.Length * 70.0 * (double)height / 100.0);
        }

		public void DrawString(Graphics g, int x, int y, bool reverse,string str, int height, int lineWidth, Color col)
		{
			if (str == null) return;
			int strLength = str.Length;
			if (strLength < 1) return;
			double charWidth = 70.0 * (double)height / 100.0;
			if (reverse) charWidth = -charWidth;
			for (int i = 0; i < strLength; i++) {
				int xpos = (int)(charWidth * i) + x;
				Draw(g, xpos, y, reverse, str[i], height, lineWidth, col);
			}
		}

		/// <summary>
		/// 1文字のCAMデータの生成
		/// </summary>
		/// <param name="camdataLList"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="reverse"></param>
		/// <param name="code">文字コード</param>
		/// <param name="height">0.1μm単位</param>
		/// <param name="lineWidth">0.1μm単位</param>
		/// <remarks>
		/// フォントデータは高さ100を基準に作っている。
		/// n(0.1μm)高さに出力するためには、n/100倍にすれば良いことになる。 
		/// </remarks>
		public void GenerateCamData(LinkedList<CamOutBaseData> camdataLList, int x, int y, bool reverse, int code, int height, int lineWidth)
		{
			if (!Ready) return;
			if (code < 0x21 || code > 0x7e) return;
			double ymag = (double)height / 100.0;
			double xmag = (reverse ? -ymag : ymag);
			int index = code - 0x21;
			int vectCount = fontData[index].Count;
			for (int i = 0; i < vectCount; i++) {
				Point pt0;
				Point pt1;
				fontData[index].LineData(i, out pt0, out pt1);
				int x0 = (int)(x + (double)pt0.X * xmag);
				int y0 = (int)(y + (double)pt0.Y * ymag);
				int x1 = (int)(x + (double)pt1.X * xmag);
				int y1 = (int)(y + (double)pt1.Y * ymag);
				pt0 = new Point(x0, y0);
				pt1 = new Point(x1, y1);
				CamOutBaseData camd = new CamOutBaseData(	MbeLayer.LayerValue.NUL, 
															CamOutBaseData.CamType.VECTOR,
															CamOutBaseData.Shape.Obround,
															lineWidth,lineWidth, pt0, pt1);
				camdataLList.AddLast(camd);
			}
		}

		
		/// <summary>
		/// 文字列のCAMデータの生成
		/// </summary>
		/// <param name="camdataLList"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="reverse"></param>
		/// <param name="str"></param>
		/// <param name="height"></param>
		/// <param name="lineWidth"></param>
		public void GenerateCamDataString(LinkedList<CamOutBaseData> camdataLList, int x, int y, bool reverse, string str, int height, int lineWidth)
		{
			if (str == null) return;
			int strLength = str.Length;
			if (strLength < 1) return;
			double charWidth = 70.0 * (double)height / 100.0;
			if (reverse) charWidth = -charWidth;
			for (int i = 0; i < strLength; i++) {
				int xpos = (int)(charWidth * i) + x;
				GenerateCamData(camdataLList,xpos,y, reverse, str[i], height, lineWidth);
			}
		}



		private bool ready;

		private void init()
		{
			for (int i = 0; i < 94; i++) {
				fontData[i] = new MbeBoardFontData();
			}
			ready = false;
		}

		private static readonly string[] embeddedFont =
		{
			"00021E141E0A1E461E23",
			"000414410F3C23411E3C144B1441234B2341",
			"00040A3C323C0A1E321E144B140A284B280A",
			"000C323C28462846144614460A3C1E501E050A3C0A370A37142D142D282D282D3223322332193219280F280F140F140F0A19",
			"00090A4B0A37320A321E0A371E371E371E4B1E4B0A4B1E1E1E0A1E0A320A321E1E1E323C0A19",
			"000C0A14140A140A230A230A2D140A140A232D142D2D0A2328370F373205234B144B0F460F3728372846144B0F462846234B",
			"00021941143C194B1941",
			"0003234B193C193C19191919230A",
			"0003194B233C233C23192319190A",
			"00040A2D322D2D410F190F412D191E4B1E0F",
			"00020A2D322D1E4B1E0F",
			"00051E1E1E141E14190F1E1E191E191E191919191E19",
			"00010A2D322D",
			"00041E1919191919191419141E141E191E14",
			"00012D410F19",
			"0008284B3241324132143214280A194B0F41194B284B0F410F140F14190A280A190A",
			"00031E4B1E0A140A280A1E4B1441",
			"0007284B324132373241194B0F41194B284B320A0F0A32370F140F140F0A",
			"000B284B3241323732413237282D282D192D282D3223322332143214280A194B0F41194B284B280A190A190A0F14",
			"0003283C280A051932191E500519",
			"00083214280A283232283228321428320F320F4B0F320F4B2D4B280A190A190A0F14",
			"000A3214280A282D322332233214280A190A190A0F14282D192D192D0F230F250F14143C1E4B143C0F25",
			"0004324B323C323C1E0A0F3C0F4B0F4B324B",
			"000F3214280A282D322332233214284B3241324132373237282D280A190A190A0F14282D192D192D0F230F230F14192D0F370F370F410F41194B194B284B",
			"000A32322828284B3241324132232828192819280F32284B194B194B0F410F410F32190A2D142D143223",
			"00081E4119411941193C193C1E3C1E411E3C1E1E191E191E191919191E191E1E1E19",
			"00091E4119411941193C193C1E3C1E411E3C1E1E1E141E14190F1E1E191E191E191919191E19",
			"000232460A2D0A2D3214",
			"00020A3C323C0A233223",
			"00020A46322D322D0A14",
			"0008284B3241284B144B144B0A410A410A37324132371E2832371E281E231E141E0A",
			"000A144B0A41144B284B284B32410A410A140A14140A32413219321919191919193719373237320A140A",
			"00030A0A1E4B1E4B320A0F192D19",
			"000A3214280A282D322332233214284B3241324132373237282D0F4B284B280A0F0A0F0A0F4B282D0F2D",
			"00073214280A284B3241284B194B194B0F410F410F140F14190A190A280A",
			"00063214280A284B32410F4B284B280A0F0A0F0A0F4B32413214",
			"00040F0A0F4B0F4B324B320A0F0A282D0F2D",
			"00030F0A0F4B0F4B324B282D0F2D",
			"00093214280A284B3241284B194B194B0F410F410F140F14190A190A280A3214322832282328",
			"00030F0A0F4B324B320A0F2D322D",
			"00031E4B1E0A140A280A144B284B",
			"00052D4B2D142D14230A230A190A190A0F140F4B324B",
			"00030F4B0F0A324B0F281932320A",
			"00020F4B0F0A0F0A320A",
			"00040A4B0A0A0A4B1E371E37324B324B320A",
			"00030F0A0F4B0F4B320A320A324B",
			"0008144B0A41144B284B284B32410A410A140A14140A324132143214280A280A140A",
			"0006284B3241324132373237282D0F4B284B0F0A0F4B282D0F2D",
			"0009144B0A41144B284B284B32410A410A190A19140F324132193219280F280F140F1928320A",
			"0007284B3241324132373237282D0F4B284B0F0A0F4B282D0F2D1E2D320A",
			"000B284B3241282D192D282D3223322332143214280A194B0F41194B284B280A190A190A0F140F370F410F37192D",
			"00021E4B1E0A0A4B324B",
			"00053214280A0F14190A190A280A0F4B0F143214324B",
			"00020A4B1E0A1E0A324B",
			"00040A4B0F0A0F0A1E2D2D0A324B1E2D2D0A",
			"00020A4B320A324B0A0A",
			"00030A4B1E2D1E2D324B1E2D1E0A",
			"00030F4B324B324B0F0A0F0A320A",
			"0003234B144B144B140A140A230A",
			"00010A4B320A",
			"0003144B234B234B230A230A140A",
			"00020F3C1E4B1E4B2D3C",
			"00010A0A320A",
			"00010F461E32",
			"0009190A0F14190A320A320A322D322D28372828321E0F140F1E0F1E19281928282828371937",
			"00070F4B0F0A0F0A280A280A321432143228322828322832193219320F28",
			"00070F14190A190A280A280A3214322828322832193219320F280F280F14",
			"0007324B320A320A190A190A0F140F140F280F2819321932283228323228",
			"00080F14190A322828322832193219320F280F280F140F1E321E3228321E190A320A",
			"00042346193C193C19050F28282823462D46",
			"000A0F32193C193C283C283C3232320F28052805190519050F0F0F320F280F28191E191E321E323C320F",
			"00050F4B0F0A0F23192D192D282D282D32233223320A",
			"0004140A280A1E3C1E371E281E0A1E282828",
			"0004193723372337231423141405234B2346",
			"00030F4B0F0A322D0F0A1E19320A",
			"0003144B1E4B1E4B1E0A140A280A",
			"00070A370A0A0A2D1437143728372837322D322D320A14371E2D1E2D1E0A",
			"00052837322D322D320A0F370F0A0F2D193719372837",
			"00080F14190A190A280A280A3214322828322832193219320F280F280F1432283214",
			"00080F32193C193C283C283C32320F28191E32323228191E281E3228281E0F3C0F05",
			"00070A32143C143C233C233C2D320A320A280A28141E141E2D1E2D3C2D05",
			"0004322828322832193219320F280F320F0A",
			"000A14320F2D0F230F2D141E0F232D3214322D0A320F32192D1E320F32192D1E141E0F0F140A140A2D0A",
			"0004190F1E0A1E0A280A194B190F0F322832",
			"00050F320F140F14190A190A280A280A32143232320A",
			"00020F321E0A1E0A2D32",
			"00040A320F0A2D0A32320F0A1E1E1E1E2D0A",
			"00020F32320A32320F0A",
			"00023232190514322319",
			"00030F32323232320F0A0F0A320A",
			"00061E0A23051E32142814281E1E1E1E1E0A234B1E461E461E32",
			"00011E4B1E05",
			"0006144B194619461932193223282328191E191E190A190A1405",
			"00030F41194B194B234123412D4B"
		};


	}
}
