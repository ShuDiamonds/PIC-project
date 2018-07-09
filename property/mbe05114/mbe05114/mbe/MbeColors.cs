using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace mbe
{
	/// <summary>
	/// 描画色クラス
	/// </summary>
	class MbeColors
	{
		/// <summary>
		/// 整数値色をハイライトしてColorを作成する
		/// </summary>
		/// <param name="colorint"></param>
		/// <returns></returns>
		public static Color HighLighten(uint colorint)
		{
			int a =(int)( (colorint & 0xFF000000) >> 24 );
			int r =(int)( (colorint & 0x00FF0000) >> 16 );
			int g =(int)( (colorint & 0x0000FF00) >> 8  );
			int b = (int)((colorint & 0x000000FF));

			r = r * 2 + 64; if (r > 0xFF) r = 0xFF;
			g = g * 2 + 64; if (g > 0xFF) g = 0xFF;
			b = b * 2 + 64; if (b > 0xFF) b = 0xFF;

			return Color.FromArgb(a, r, g, b);
		}

		public const uint DEFAULT_COLOR_BACKGROUND          = 0xFF000000;   // 背景
        public const uint DEFAULT_COLOR_BG_NOT_WORK_AREA    = 0xFF303030;   // 非ワークエリア(原点の左、下)の背景色
        public const uint DEFAULT_COLOR_SELECT_FRAME        = 0xFF808080;   // 選択枠
        public const uint DEFAULT_COLOR_CROSS_CURSOR        = 0xFFC08000;   // 十字カーソル
        public const uint DEFAULT_COLOR_ORIGIN_MARK         = 0xFF808080;   // 原点マーク
        public const uint DEFAULT_COLOR_GRID_ORIGIN_MARK    = 0xFF808000;   // グリッド原点マーク
        public const uint DEFAULT_COLOR_SNAP_POINT          = 0xFF808080;   // スナップ位置マーク
        public const uint DEFAULT_COLOR_ACTIVE_SNAP_POINT   = 0xFFFFFFFF;   // アクティブなスナップ位置マーク
        public const uint DEFAULT_COLOR_PIN_NUM             = 0xFF808080;   // ピン番号
        public const uint DEFAULT_COLOR_GRID                = 0xFFD0D0D0;   // グリッド
        public const uint DEFAULT_COLOR_INPUTERR            = 0xFFFF8000;   // 入力エラー通知色(コントロールの背景)

        public const uint DEFAULT_COLOR_DIM                 = 0xFFB8C9C0;   // 外形色
        public const uint DEFAULT_COLOR_DRL                 = 0xFFC0C0C0;   // ドリル穴輪郭
        public const uint DEFAULT_COLOR_STC                 = 0xFFAC5959;   // 部品面ソルダレジスト
        public const uint DEFAULT_COLOR_STS                 = 0xFF4F4F9D;   // 半田面ソルダレジスト
        public const uint DEFAULT_COLOR_DOC                 = 0xFFDCBCA5;   // 部品面ドキュメント
        public const uint DEFAULT_COLOR_PLC                 = 0xFFFCC403;   // 部品面シルク
        public const uint DEFAULT_COLOR_PLS                 = 0xFF3588A4;   // 半田面シルク
        public const uint DEFAULT_COLOR_CMP                 = 0xFFFF8080;   // 部品面パターン
        public const uint DEFAULT_COLOR_L2                  = 0xFF800000;   // L2パターン
        public const uint DEFAULT_COLOR_L3                  = 0xFF403080;   // L3パターン
        public const uint DEFAULT_COLOR_MMC                 = 0xFF804040;   // 部品面メタルマスク
        public const uint DEFAULT_COLOR_MMS                 = 0xFF505080;   // 半田面メタルマスク
        public const uint DEFAULT_COLOR_SOL                 = 0xFF6060C0;   // 半田面パターン
        public const uint DEFAULT_COLOR_PTH                 = 0xFF00A000;   // PTH


        public const uint DEFAULT_PRINT_COLOR_DIM           = 0xFFB8C9C0;   // 印刷色 外形色
        public const uint DEFAULT_PRINT_COLOR_DRL           = 0xFFC0C0C0;   // 印刷色 ドリル穴輪郭
        public const uint DEFAULT_PRINT_COLOR_STC           = 0xFFAC5959;   // 印刷色 部品面ソルダレジスト
        public const uint DEFAULT_PRINT_COLOR_STS           = 0xFF4F4F9D;   // 印刷色 半田面ソルダレジスト
        public const uint DEFAULT_PRINT_COLOR_DOC           = 0xFFDCBCA5;   // 印刷色 部品面ドキュメント
        public const uint DEFAULT_PRINT_COLOR_PLC           = 0xFFFCC403;   // 印刷色 部品面シルク
        public const uint DEFAULT_PRINT_COLOR_PLS           = 0xFF3588A4;   // 印刷色 半田面シルク
        public const uint DEFAULT_PRINT_COLOR_CMP           = 0xFFFF8080;   // 印刷色 部品面パターン
        public const uint DEFAULT_PRINT_COLOR_L2            = 0xFF800000;   // 印刷色 L2パターン
        public const uint DEFAULT_PRINT_COLOR_L3            = 0xFF403080;   // 印刷色 L3パターン
        public const uint DEFAULT_PRINT_COLOR_MMC           = 0xFF804040;   // 印刷色 部品面メタルマスク
        public const uint DEFAULT_PRINT_COLOR_MMS           = 0xFF505080;   // 印刷色 半田面メタルマスク
        public const uint DEFAULT_PRINT_COLOR_SOL           = 0xFF6060C0;   // 印刷色 半田面パターン
        public const uint DEFAULT_PRINT_COLOR_PTH           = 0xFF00A000;   // 印刷色 PTH


		private static uint nColorBackground = DEFAULT_COLOR_BACKGROUND;
		private static uint nColorSelectFrame = DEFAULT_COLOR_SELECT_FRAME;
		private static uint nColorBgNotWorkArea = DEFAULT_COLOR_BG_NOT_WORK_AREA;
		private static uint nColorInputErr = DEFAULT_COLOR_INPUTERR;
		private static uint nColorCrossCursor;
		private static uint nColorOriginMark;
		private static uint nColorGridOriginMark;
		private static uint nColorGrid;
		private static uint nColorSnapPoint;
        private static uint nColorActiveSnapPoint;
        private static uint nColorPinNum;
		
		private static uint[] layerColors;
        private static uint[] layerPrintColors;


        private static readonly uint[] layerDefaultColors = 
		{
			DEFAULT_COLOR_PTH,
            DEFAULT_COLOR_MMC,
			DEFAULT_COLOR_PLC,
			DEFAULT_COLOR_STC,
			DEFAULT_COLOR_CMP,
			DEFAULT_COLOR_L2,
			DEFAULT_COLOR_L3,
			DEFAULT_COLOR_SOL,
			DEFAULT_COLOR_STS,
			DEFAULT_COLOR_PLS,
            DEFAULT_COLOR_MMS,
			DEFAULT_COLOR_DIM,
			DEFAULT_COLOR_DRL,
			DEFAULT_COLOR_DOC
		};

        private static readonly uint[] layerDefaultPrintColors = 
		{
			DEFAULT_PRINT_COLOR_PTH,
            DEFAULT_PRINT_COLOR_MMC,
			DEFAULT_PRINT_COLOR_PLC,
			DEFAULT_PRINT_COLOR_STC,
			DEFAULT_PRINT_COLOR_CMP,
			DEFAULT_PRINT_COLOR_L2,
			DEFAULT_PRINT_COLOR_L3,
			DEFAULT_PRINT_COLOR_SOL,
			DEFAULT_PRINT_COLOR_STS,
			DEFAULT_PRINT_COLOR_PLS,
            DEFAULT_PRINT_COLOR_MMS,
			DEFAULT_PRINT_COLOR_DIM,
			DEFAULT_PRINT_COLOR_DRL,
			DEFAULT_PRINT_COLOR_DOC
		};
		



		public static void InitColor()
		{
			System.Diagnostics.Debug.WriteLine("InitColor()");
            layerColors = new uint[layerDefaultColors.Length];
            layerPrintColors = new uint[layerDefaultPrintColors.Length];
			SetDefaultColor();
            SetDefaultPrintColor();
		}

        public static void SetDefaultPrintColor()
        {
            int layerPrintColorCount = layerDefaultPrintColors.Length;
            for (int i = 0; i < layerPrintColorCount; i++) {
                layerPrintColors[i] = layerDefaultPrintColors[i];
            }
        }

		public static void SetDefaultColor()
		{
			int layerColorCount = layerDefaultColors.Length;
			for (int i = 0; i < layerColorCount; i++) {
				layerColors[i] = layerDefaultColors[i];
			}

			nColorCrossCursor = DEFAULT_COLOR_CROSS_CURSOR;
			nColorOriginMark = DEFAULT_COLOR_ORIGIN_MARK;
			nColorGridOriginMark = DEFAULT_COLOR_GRID_ORIGIN_MARK;
			nColorGrid = DEFAULT_COLOR_GRID;
			nColorSnapPoint = DEFAULT_COLOR_SNAP_POINT;
            nColorActiveSnapPoint = DEFAULT_COLOR_ACTIVE_SNAP_POINT;
            nColorPinNum = DEFAULT_COLOR_PIN_NUM;
            nColorBackground = DEFAULT_COLOR_BACKGROUND;
			nColorBgNotWorkArea = DEFAULT_COLOR_BG_NOT_WORK_AREA;
			nColorInputErr = DEFAULT_COLOR_INPUTERR;
		}

			

		/// <summary>
		/// 設定データから色設定を読み込む
		/// </summary>
		/// <remarks>
		/// 設定データのデフォルト値はすべて0。
		/// 設定データから読み込まれた値が0なら既定値となる。
		/// </remarks>
		public static void LoadSettings()
		{
			uint ncol;

#if !MONO
            Properties.Settings.Default.Reload();
#endif


            
            ncol = Properties.Settings.Default.CustomColorPTH;
			if(ncol!=0) PTH = ncol;

            ncol = Properties.Settings.Default.CustomColorMMC;
            if (ncol != 0) MMC = ncol;

			ncol = Properties.Settings.Default.CustomColorPLC;
			if(ncol!=0) PLC = ncol;

			ncol = Properties.Settings.Default.CustomColorSTC;
			if(ncol!=0) STC = ncol;

			ncol = Properties.Settings.Default.CustomColorCMP;
			if(ncol!=0) CMP = ncol;

            ncol = Properties.Settings.Default.CustomColorL2;
            if (ncol != 0) L2 = ncol;
            
            ncol = Properties.Settings.Default.CustomColorL3;
            if (ncol != 0) L3 = ncol;

			ncol = Properties.Settings.Default.CustomColorSOL;
			if(ncol!=0) SOL = ncol;

			ncol = Properties.Settings.Default.CustomColorSTS;
			if(ncol!=0) STS = ncol;

			ncol = Properties.Settings.Default.CustomColorPLS;
			if(ncol!=0) PLS = ncol;

            ncol = Properties.Settings.Default.CustomColorMMS;
            if (ncol != 0) MMS = ncol;

            ncol = Properties.Settings.Default.CustomColorDIM;
			if(ncol!=0) DIM = ncol;

			ncol = Properties.Settings.Default.CustomColorDRL;
			if (ncol != 0) DRL = ncol;

			ncol = Properties.Settings.Default.CustomColorDOC;
			if(ncol!=0) DOC = ncol;


            ncol = Properties.Settings.Default.CustomColorPrintPTH;
            if (ncol != 0) PRINT_PTH = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintMMC;
            if (ncol != 0) PRINT_MMC = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintPLC;
            if (ncol != 0) PRINT_PLC = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintSTC;
            if (ncol != 0) PRINT_STC = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintCMP;
            if (ncol != 0) PRINT_CMP = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintL2;
            if (ncol != 0) PRINT_L2 = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintL3;
            if (ncol != 0) PRINT_L3 = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintSOL;
            if (ncol != 0) PRINT_SOL = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintSTS;
            if (ncol != 0) PRINT_STS = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintPLS;
            if (ncol != 0) PRINT_PLS = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintMMS;
            if (ncol != 0) PRINT_MMS = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintDIM;
            if (ncol != 0) PRINT_DIM = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintDRL;
            if (ncol != 0) PRINT_DRL = ncol;

            ncol = Properties.Settings.Default.CustomColorPrintDOC;
            if (ncol != 0) PRINT_DOC = ncol;

			ncol = Properties.Settings.Default.CustomColorOriginMark;
			if (ncol != 0) OriginMark = ncol;

			ncol = Properties.Settings.Default.CustomColorGridOrigin;
			if (ncol != 0) GridOriginMark = ncol;

			ncol = Properties.Settings.Default.CustomColorGrid;
			if (ncol != 0) Grid = ncol;

			ncol = Properties.Settings.Default.CustomColorSnapMark;
			if (ncol != 0) SnapPoint = ncol;

            ncol = Properties.Settings.Default.CustomColorActiveSnapMark;
            if (ncol != 0) ActiveSnapPoint = ncol;

			ncol = Properties.Settings.Default.CustomColorPinNum;
			if (ncol != 0) PinNum = ncol;

			ncol = Properties.Settings.Default.CustomColorCursor;
			if (ncol != 0) CrossCursor = ncol;

			ncol = Properties.Settings.Default.CustomColorBgOutside;
			if (ncol != 0) BgNotWorkArea = ncol;

            ncol = Properties.Settings.Default.CustomColorBg;
            if (ncol != 0) Background = ncol;

			ncol = Properties.Settings.Default.CustomColorInputErr;
			if (ncol != 0) InputErr = ncol;

		}

		/// <summary>
		/// 設定データに色設定を保存する
		/// </summary>
		public static void StoreSettings()
		{
			Properties.Settings.Default.CustomColorPTH = PTH;
            Properties.Settings.Default.CustomColorMMC = MMC;
			Properties.Settings.Default.CustomColorPLC = PLC;
			Properties.Settings.Default.CustomColorSTC = STC;
			Properties.Settings.Default.CustomColorCMP = CMP;
            Properties.Settings.Default.CustomColorL2  = L2;
            Properties.Settings.Default.CustomColorL3  = L3;
            Properties.Settings.Default.CustomColorSOL = SOL;
			Properties.Settings.Default.CustomColorSTS = STS;
			Properties.Settings.Default.CustomColorPLS = PLS;
            Properties.Settings.Default.CustomColorMMS = MMS;
            Properties.Settings.Default.CustomColorDIM = DIM;
			Properties.Settings.Default.CustomColorDRL = DRL;
			Properties.Settings.Default.CustomColorDOC = DOC;

            Properties.Settings.Default.CustomColorPrintPTH = PRINT_PTH;
            Properties.Settings.Default.CustomColorPrintMMC = PRINT_MMC;
            Properties.Settings.Default.CustomColorPrintPLC = PRINT_PLC;
            Properties.Settings.Default.CustomColorPrintSTC = PRINT_STC;
            Properties.Settings.Default.CustomColorPrintCMP = PRINT_CMP;
            Properties.Settings.Default.CustomColorPrintL2 = PRINT_L2;
            Properties.Settings.Default.CustomColorPrintL3 = PRINT_L3;
            Properties.Settings.Default.CustomColorPrintSOL = PRINT_SOL;
            Properties.Settings.Default.CustomColorPrintSTS = PRINT_STS;
            Properties.Settings.Default.CustomColorPrintPLS = PRINT_PLS;
            Properties.Settings.Default.CustomColorPrintMMS = PRINT_MMS;
            Properties.Settings.Default.CustomColorPrintDIM = PRINT_DIM;
            Properties.Settings.Default.CustomColorPrintDRL = PRINT_DRL;
            Properties.Settings.Default.CustomColorPrintDOC = PRINT_DOC;


			Properties.Settings.Default.CustomColorOriginMark = OriginMark;
			Properties.Settings.Default.CustomColorGridOrigin = GridOriginMark;
			Properties.Settings.Default.CustomColorGrid = Grid;
			Properties.Settings.Default.CustomColorSnapMark = SnapPoint;
            Properties.Settings.Default.CustomColorActiveSnapMark = ActiveSnapPoint;
			Properties.Settings.Default.CustomColorPinNum = PinNum;
			Properties.Settings.Default.CustomColorCursor = CrossCursor;
			Properties.Settings.Default.CustomColorBgOutside = BgNotWorkArea;
            Properties.Settings.Default.CustomColorBg = Background;
            Properties.Settings.Default.CustomColorInputErr = InputErr;
            Properties.Settings.Default.Save();

		}


		public static uint GetLayerColor(int index)
		{
			if (index < 0) index = 0;
			else if (index >= layerColors.Length) index = layerColors.Length - 1;
			return layerColors[index];
		}

		public static void SetLayerColor(int index, uint col)
		{
			if (index < 0) index = 0;
			else if (index >= layerColors.Length) index = layerColors.Length - 1;
			layerColors[index] = col;
		}


        public static uint GetLayerPrintColor(int index)
        {
            if (index < 0) index = 0;
            else if (index >= layerPrintColors.Length) index = layerPrintColors.Length - 1;
            return layerPrintColors[index];
        }

        public static void SetLayerPrintColor(int index, uint col)
        {
            if (index < 0) index = 0;
            else if (index >= layerPrintColors.Length) index = layerPrintColors.Length - 1;
            layerPrintColors[index] = col;
        }





		/// <summary>
		/// 背景色の整数値の設定と取得
		/// </summary>
		public static uint Background
		{
			get { return nColorBackground; }
			set { nColorBackground = value; }
		}


		/// <summary>
		/// 背景色の取得
		/// </summary>
		public static Color ColorBackground
		{
			get { return Color.FromArgb(unchecked((int)nColorBackground)); }
		}

		/// <summary>
		/// 非作業エリアの背景色の整数値の設定と取得
		/// </summary>
		public static uint BgNotWorkArea
		{
			get { return nColorBgNotWorkArea; }
			set { nColorBgNotWorkArea = value; }
		}


		/// <summary>
		/// 非作業エリアの背景色の取得
		/// </summary>
		public static Color ColorBgNotWorkArea
		{
			get { return Color.FromArgb(unchecked((int)nColorBgNotWorkArea)); }
		}

		/// <summary>
		/// 入力エラー通知色(コントロールの背景)の整数値の設定と取得
		/// </summary>
		public static uint InputErr
		{
			get { return nColorInputErr; }
			set { nColorInputErr = value; }
		}

		/// <summary>
		/// 入力エラー通知色(コントロールの背景)の取得
		/// </summary>
		public static Color ColorInputErr
		{
			get { return Color.FromArgb(unchecked((int)nColorInputErr)); }
		}


		/// <summary>
		/// 外形色の整数値の設定と取得
		/// </summary>
		public static uint Outline
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.DIM]; }
			set { layerColors[(int)MbeLayer.LayerIndex.DIM] = value; }
		}

		/// <summary>
		/// 外形色の取得
		/// </summary>
		public static Color ColorOutline
		{
			get { return Color.FromArgb(unchecked((int)Outline)); }
		}



		/// <summary>
		/// 選択枠色の整数値の設定と取得
		/// </summary>
		public static uint SelectFrame
		{
			get { return nColorSelectFrame; }
			set { nColorSelectFrame = value; }
		}

		/// <summary>
		/// 選択枠色の取得
		/// </summary>
		public static Color ColorSelectFrame
		{
			get { return Color.FromArgb(unchecked((int)nColorSelectFrame)); }
		}

		/// <summary>
		/// 十字カーソル色の整数値の設定と取得
		/// </summary>
		public static uint CrossCursor
		{
			get { return nColorCrossCursor; }
			set { nColorCrossCursor = value; }
		}

		/// <summary>
		/// 十字カーソル色の取得
		/// </summary>
		public static Color ColorCrossCursor
		{
			get { return Color.FromArgb(unchecked((int)nColorCrossCursor)); }
		}



		/// <summary>
		/// 原点マーク色の整数値の設定と取得
		/// </summary>
		public static uint OriginMark
		{
			get{ return nColorOriginMark;}
			set{ nColorOriginMark = value;}
		}

		/// <summary>
		/// 原点マーク色の取得
		/// </summary>
		public static Color ColorOriginMark
		{
			get { return Color.FromArgb(unchecked((int)nColorOriginMark)); }
		}


		/// <summary>
		/// グリッド原点マーク色の整数値の設定と取得
		/// </summary>
		public static uint GridOriginMark
		{
			get { return nColorGridOriginMark; }
			set { nColorGridOriginMark = value; }
		}

		/// <summary>
		/// グリッド原点マーク色の取得
		/// </summary>
		public static Color ColorGridOriginMark
		{
			get { return Color.FromArgb(unchecked((int)nColorGridOriginMark)); }
		}


		/// <summary>
		/// スナップ位置マークの色の整数値の設定と取得
		/// </summary>
		public static uint SnapPoint
		{
			get { return MbeColors.nColorSnapPoint; }
			set { MbeColors.nColorSnapPoint = value; }
		}

		/// <summary>
		/// スナップ位置マーク色の取得
		/// </summary>
		public static Color ColorSnapPoint
		{
			get { return Color.FromArgb(unchecked((int)nColorSnapPoint)); }
		}

        /// <summary>
        /// スナップ位置マークの色の整数値の設定と取得
        /// </summary>
        public static uint ActiveSnapPoint
        {
            get { return MbeColors.nColorActiveSnapPoint; }
            set { MbeColors.nColorActiveSnapPoint = value; }
        }

        /// <summary>
        /// スナップ位置マーク色の取得
        /// </summary>
        public static Color ColorActiveSnapPoint
        {
            get { return Color.FromArgb(unchecked((int)nColorActiveSnapPoint)); }
        }

        
        
        /// <summary>
		/// ピン番号の色の整数値の設定と取得
		/// </summary>
		public static uint PinNum
		{
			get { return MbeColors.nColorPinNum; }
			set { MbeColors.nColorPinNum = value; }
		}

		/// <summary>
		/// ピン番号の色の取得
		/// </summary>
		public static Color ColorPinNum
		{
			get { return Color.FromArgb(unchecked((int)nColorPinNum)); }
		}

		/// <summary>
		/// グリッドの色の整数値の設定と取得 
		/// </summary>
		public static uint Grid
		{
			get{ return nColorGrid;}
			set{ nColorGrid = value;}
		}

		/// <summary>
		/// グリッドの色の取得
		/// </summary>
		public static Color ColorGrid
		{
			get { return Color.FromArgb(unchecked((int)nColorGrid)); }
		}

		/// <summary>
		/// スルーホールパッド色の整数値の設定と取得
		/// </summary>
		public static uint PTH
		{
		    get { return  layerColors[(int)MbeLayer.LayerIndex.PTH]; }
			set { layerColors[(int)MbeLayer.LayerIndex.PTH] = value; }
		}


		/// <summary>
		/// 半田面パターン色の整数値の設定と取得
		/// </summary>
		public static uint SOL
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.SOL]; }   // MbeColors.nColorSTS; }
			set { layerColors[(int)MbeLayer.LayerIndex.SOL] = value; }
		}

		/// <summary>
		/// 部品面パターン色の整数値の設定と取得
		/// </summary>
		public static uint CMP
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.CMP]; }   // MbeColors.nColorSTS; }
			set { layerColors[(int)MbeLayer.LayerIndex.CMP] = value; }
		}



        /// <summary>
        /// L2パターン色の整数値の設定と取得
        /// </summary>
        public static uint L2
        {
            get { return layerColors[(int)MbeLayer.LayerIndex.L2]; }
            set { layerColors[(int)MbeLayer.LayerIndex.L2] = value; }
        }



        /// <summary>
        /// L3パターン色の整数値の設定と取得
        /// </summary>
        public static uint L3
        {
            get { return layerColors[(int)MbeLayer.LayerIndex.L3]; }
            set { layerColors[(int)MbeLayer.LayerIndex.L3] = value; }
        }


        /// <summary>
        /// MMC色の整数値の設定と取得
        /// </summary>
        public static uint MMC
        {
            get { return layerColors[(int)MbeLayer.LayerIndex.MMC]; }
            set { layerColors[(int)MbeLayer.LayerIndex.MMC] = value; }
        }


        /// <summary>
        /// MMS色の整数値の設定と取得
        /// </summary>
        public static uint MMS
        {
            get { return layerColors[(int)MbeLayer.LayerIndex.MMS]; }
            set { layerColors[(int)MbeLayer.LayerIndex.MMS] = value; }
        }



		/// <summary>
		/// 半田面ソルダレジスト色の整数値の設定と取得
		/// </summary>
		public static uint STS
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.STS];	}   // MbeColors.nColorSTS; }
			set { layerColors[(int)MbeLayer.LayerIndex.STS] = value; }
		}


		/// <summary>
		/// 部品面ソルダレジスト色の整数値の設定と取得
		/// </summary>
		public static uint STC
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.STC]; }
			set { layerColors[(int)MbeLayer.LayerIndex.STC] = value; }
		}



		/// <summary>
		/// 半田面シルク色の整数値の設定と取得
		/// </summary>
		public static uint PLS
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.PLS]; }   // MbeColors.nColorSTS; }
			set { layerColors[(int)MbeLayer.LayerIndex.PLS] = value; }
		}


		/// <summary>
		/// 部品面シルク色の整数値の設定と取得
		/// </summary>
		public static uint PLC
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.PLC]; }
			set { layerColors[(int)MbeLayer.LayerIndex.PLC] = value; }
		}


		/// <summary>
		/// ディメンジョンの整数値の設定と取得
		/// </summary>
		public static uint DIM
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.DIM]; }
			set { layerColors[(int)MbeLayer.LayerIndex.DIM] = value; }
		}



		/// <summary>
		/// ドリルの整数値の設定と取得
		/// </summary>
		public static uint DRL
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.DRL]; }
			set { layerColors[(int)MbeLayer.LayerIndex.DRL] = value; }
		}

        ///// <summary>
        ///// ドリル穴輪郭の色の整数値の設定と取得
        ///// </summary>
        //public static uint Drl
        //{
        //    get { return layerColors[(int)MbeLayer.LayerIndex.DRL]; }
        //    set { layerColors[(int)MbeLayer.LayerIndex.DRL] = value; }
        //}

        ///// <summary>
        ///// ドリル穴輪郭の色の取得
        ///// </summary>
        //public static Color ColorDrl
        //{
        //    get { return Color.FromArgb(unchecked((int)Drl)); }
        //}


		/// <summary>
		/// ドキュメント色の整数値の設定と取得
		/// </summary>
		public static uint DOC
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.DOC]; }
			set { layerColors[(int)MbeLayer.LayerIndex.DOC] = value; }
		}


//////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// スルーホールパッド印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_PTH
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.PTH]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.PTH] = value; }
        }


        /// <summary>
        /// 半田面パターン印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_SOL
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.SOL]; }   // MbeColors.nColorSTS; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.SOL] = value; }
        }


        /// <summary>
        /// 部品面パターン印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_CMP
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.CMP]; }   // MbeColors.nColorSTS; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.CMP] = value; }
        }

        /// <summary>
        /// L2パターン印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_L2
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.L2]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.L2] = value; }
        }



        /// <summary>
        /// L3パターン印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_L3
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.L3]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.L3] = value; }
        }



        /// <summary>
        /// MMC印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_MMC
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.MMC]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.MMC] = value; }
        }


        /// <summary>
        /// MMS印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_MMS
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.MMS]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.MMS] = value; }
        }


        /// <summary>
        /// 半田面ソルダレジスト印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_STS
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.STS]; }   // MbeColors.nColorSTS; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.STS] = value; }
        }


        /// <summary>
        /// 部品面ソルダレジスト印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_STC
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.STC]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.STC] = value; }
        }

        /// <summary>
        /// 半田面シルク印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_PLS
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.PLS]; }   // MbeColors.nColorSTS; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.PLS] = value; }
        }


        /// <summary>
        /// 部品面シルク印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_PLC
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.PLC]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.PLC] = value; }
        }


        /// <summary>
        /// ディメンジョン印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_DIM
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.DIM]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.DIM] = value; }
        }



        /// <summary>
        /// ドリル印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_DRL
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.DRL]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.DRL] = value; }
        }


        /// <summary>
        /// ドキュメント印刷色の整数値の設定と取得
        /// </summary>
        public static uint PRINT_DOC
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.DOC]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.DOC] = value; }
        }

	}
}
