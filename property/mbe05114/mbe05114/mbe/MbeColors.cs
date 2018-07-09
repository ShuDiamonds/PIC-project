using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace mbe
{
	/// <summary>
	/// �`��F�N���X
	/// </summary>
	class MbeColors
	{
		/// <summary>
		/// �����l�F���n�C���C�g����Color���쐬����
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

		public const uint DEFAULT_COLOR_BACKGROUND          = 0xFF000000;   // �w�i
        public const uint DEFAULT_COLOR_BG_NOT_WORK_AREA    = 0xFF303030;   // �񃏁[�N�G���A(���_�̍��A��)�̔w�i�F
        public const uint DEFAULT_COLOR_SELECT_FRAME        = 0xFF808080;   // �I��g
        public const uint DEFAULT_COLOR_CROSS_CURSOR        = 0xFFC08000;   // �\���J�[�\��
        public const uint DEFAULT_COLOR_ORIGIN_MARK         = 0xFF808080;   // ���_�}�[�N
        public const uint DEFAULT_COLOR_GRID_ORIGIN_MARK    = 0xFF808000;   // �O���b�h���_�}�[�N
        public const uint DEFAULT_COLOR_SNAP_POINT          = 0xFF808080;   // �X�i�b�v�ʒu�}�[�N
        public const uint DEFAULT_COLOR_ACTIVE_SNAP_POINT   = 0xFFFFFFFF;   // �A�N�e�B�u�ȃX�i�b�v�ʒu�}�[�N
        public const uint DEFAULT_COLOR_PIN_NUM             = 0xFF808080;   // �s���ԍ�
        public const uint DEFAULT_COLOR_GRID                = 0xFFD0D0D0;   // �O���b�h
        public const uint DEFAULT_COLOR_INPUTERR            = 0xFFFF8000;   // ���̓G���[�ʒm�F(�R���g���[���̔w�i)

        public const uint DEFAULT_COLOR_DIM                 = 0xFFB8C9C0;   // �O�`�F
        public const uint DEFAULT_COLOR_DRL                 = 0xFFC0C0C0;   // �h�������֊s
        public const uint DEFAULT_COLOR_STC                 = 0xFFAC5959;   // ���i�ʃ\���_���W�X�g
        public const uint DEFAULT_COLOR_STS                 = 0xFF4F4F9D;   // ���c�ʃ\���_���W�X�g
        public const uint DEFAULT_COLOR_DOC                 = 0xFFDCBCA5;   // ���i�ʃh�L�������g
        public const uint DEFAULT_COLOR_PLC                 = 0xFFFCC403;   // ���i�ʃV���N
        public const uint DEFAULT_COLOR_PLS                 = 0xFF3588A4;   // ���c�ʃV���N
        public const uint DEFAULT_COLOR_CMP                 = 0xFFFF8080;   // ���i�ʃp�^�[��
        public const uint DEFAULT_COLOR_L2                  = 0xFF800000;   // L2�p�^�[��
        public const uint DEFAULT_COLOR_L3                  = 0xFF403080;   // L3�p�^�[��
        public const uint DEFAULT_COLOR_MMC                 = 0xFF804040;   // ���i�ʃ��^���}�X�N
        public const uint DEFAULT_COLOR_MMS                 = 0xFF505080;   // ���c�ʃ��^���}�X�N
        public const uint DEFAULT_COLOR_SOL                 = 0xFF6060C0;   // ���c�ʃp�^�[��
        public const uint DEFAULT_COLOR_PTH                 = 0xFF00A000;   // PTH


        public const uint DEFAULT_PRINT_COLOR_DIM           = 0xFFB8C9C0;   // ����F �O�`�F
        public const uint DEFAULT_PRINT_COLOR_DRL           = 0xFFC0C0C0;   // ����F �h�������֊s
        public const uint DEFAULT_PRINT_COLOR_STC           = 0xFFAC5959;   // ����F ���i�ʃ\���_���W�X�g
        public const uint DEFAULT_PRINT_COLOR_STS           = 0xFF4F4F9D;   // ����F ���c�ʃ\���_���W�X�g
        public const uint DEFAULT_PRINT_COLOR_DOC           = 0xFFDCBCA5;   // ����F ���i�ʃh�L�������g
        public const uint DEFAULT_PRINT_COLOR_PLC           = 0xFFFCC403;   // ����F ���i�ʃV���N
        public const uint DEFAULT_PRINT_COLOR_PLS           = 0xFF3588A4;   // ����F ���c�ʃV���N
        public const uint DEFAULT_PRINT_COLOR_CMP           = 0xFFFF8080;   // ����F ���i�ʃp�^�[��
        public const uint DEFAULT_PRINT_COLOR_L2            = 0xFF800000;   // ����F L2�p�^�[��
        public const uint DEFAULT_PRINT_COLOR_L3            = 0xFF403080;   // ����F L3�p�^�[��
        public const uint DEFAULT_PRINT_COLOR_MMC           = 0xFF804040;   // ����F ���i�ʃ��^���}�X�N
        public const uint DEFAULT_PRINT_COLOR_MMS           = 0xFF505080;   // ����F ���c�ʃ��^���}�X�N
        public const uint DEFAULT_PRINT_COLOR_SOL           = 0xFF6060C0;   // ����F ���c�ʃp�^�[��
        public const uint DEFAULT_PRINT_COLOR_PTH           = 0xFF00A000;   // ����F PTH


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
		/// �ݒ�f�[�^����F�ݒ��ǂݍ���
		/// </summary>
		/// <remarks>
		/// �ݒ�f�[�^�̃f�t�H���g�l�͂��ׂ�0�B
		/// �ݒ�f�[�^����ǂݍ��܂ꂽ�l��0�Ȃ����l�ƂȂ�B
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
		/// �ݒ�f�[�^�ɐF�ݒ��ۑ�����
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
		/// �w�i�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint Background
		{
			get { return nColorBackground; }
			set { nColorBackground = value; }
		}


		/// <summary>
		/// �w�i�F�̎擾
		/// </summary>
		public static Color ColorBackground
		{
			get { return Color.FromArgb(unchecked((int)nColorBackground)); }
		}

		/// <summary>
		/// ���ƃG���A�̔w�i�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint BgNotWorkArea
		{
			get { return nColorBgNotWorkArea; }
			set { nColorBgNotWorkArea = value; }
		}


		/// <summary>
		/// ���ƃG���A�̔w�i�F�̎擾
		/// </summary>
		public static Color ColorBgNotWorkArea
		{
			get { return Color.FromArgb(unchecked((int)nColorBgNotWorkArea)); }
		}

		/// <summary>
		/// ���̓G���[�ʒm�F(�R���g���[���̔w�i)�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint InputErr
		{
			get { return nColorInputErr; }
			set { nColorInputErr = value; }
		}

		/// <summary>
		/// ���̓G���[�ʒm�F(�R���g���[���̔w�i)�̎擾
		/// </summary>
		public static Color ColorInputErr
		{
			get { return Color.FromArgb(unchecked((int)nColorInputErr)); }
		}


		/// <summary>
		/// �O�`�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint Outline
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.DIM]; }
			set { layerColors[(int)MbeLayer.LayerIndex.DIM] = value; }
		}

		/// <summary>
		/// �O�`�F�̎擾
		/// </summary>
		public static Color ColorOutline
		{
			get { return Color.FromArgb(unchecked((int)Outline)); }
		}



		/// <summary>
		/// �I��g�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint SelectFrame
		{
			get { return nColorSelectFrame; }
			set { nColorSelectFrame = value; }
		}

		/// <summary>
		/// �I��g�F�̎擾
		/// </summary>
		public static Color ColorSelectFrame
		{
			get { return Color.FromArgb(unchecked((int)nColorSelectFrame)); }
		}

		/// <summary>
		/// �\���J�[�\���F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint CrossCursor
		{
			get { return nColorCrossCursor; }
			set { nColorCrossCursor = value; }
		}

		/// <summary>
		/// �\���J�[�\���F�̎擾
		/// </summary>
		public static Color ColorCrossCursor
		{
			get { return Color.FromArgb(unchecked((int)nColorCrossCursor)); }
		}



		/// <summary>
		/// ���_�}�[�N�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint OriginMark
		{
			get{ return nColorOriginMark;}
			set{ nColorOriginMark = value;}
		}

		/// <summary>
		/// ���_�}�[�N�F�̎擾
		/// </summary>
		public static Color ColorOriginMark
		{
			get { return Color.FromArgb(unchecked((int)nColorOriginMark)); }
		}


		/// <summary>
		/// �O���b�h���_�}�[�N�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint GridOriginMark
		{
			get { return nColorGridOriginMark; }
			set { nColorGridOriginMark = value; }
		}

		/// <summary>
		/// �O���b�h���_�}�[�N�F�̎擾
		/// </summary>
		public static Color ColorGridOriginMark
		{
			get { return Color.FromArgb(unchecked((int)nColorGridOriginMark)); }
		}


		/// <summary>
		/// �X�i�b�v�ʒu�}�[�N�̐F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint SnapPoint
		{
			get { return MbeColors.nColorSnapPoint; }
			set { MbeColors.nColorSnapPoint = value; }
		}

		/// <summary>
		/// �X�i�b�v�ʒu�}�[�N�F�̎擾
		/// </summary>
		public static Color ColorSnapPoint
		{
			get { return Color.FromArgb(unchecked((int)nColorSnapPoint)); }
		}

        /// <summary>
        /// �X�i�b�v�ʒu�}�[�N�̐F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint ActiveSnapPoint
        {
            get { return MbeColors.nColorActiveSnapPoint; }
            set { MbeColors.nColorActiveSnapPoint = value; }
        }

        /// <summary>
        /// �X�i�b�v�ʒu�}�[�N�F�̎擾
        /// </summary>
        public static Color ColorActiveSnapPoint
        {
            get { return Color.FromArgb(unchecked((int)nColorActiveSnapPoint)); }
        }

        
        
        /// <summary>
		/// �s���ԍ��̐F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint PinNum
		{
			get { return MbeColors.nColorPinNum; }
			set { MbeColors.nColorPinNum = value; }
		}

		/// <summary>
		/// �s���ԍ��̐F�̎擾
		/// </summary>
		public static Color ColorPinNum
		{
			get { return Color.FromArgb(unchecked((int)nColorPinNum)); }
		}

		/// <summary>
		/// �O���b�h�̐F�̐����l�̐ݒ�Ǝ擾 
		/// </summary>
		public static uint Grid
		{
			get{ return nColorGrid;}
			set{ nColorGrid = value;}
		}

		/// <summary>
		/// �O���b�h�̐F�̎擾
		/// </summary>
		public static Color ColorGrid
		{
			get { return Color.FromArgb(unchecked((int)nColorGrid)); }
		}

		/// <summary>
		/// �X���[�z�[���p�b�h�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint PTH
		{
		    get { return  layerColors[(int)MbeLayer.LayerIndex.PTH]; }
			set { layerColors[(int)MbeLayer.LayerIndex.PTH] = value; }
		}


		/// <summary>
		/// ���c�ʃp�^�[���F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint SOL
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.SOL]; }   // MbeColors.nColorSTS; }
			set { layerColors[(int)MbeLayer.LayerIndex.SOL] = value; }
		}

		/// <summary>
		/// ���i�ʃp�^�[���F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint CMP
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.CMP]; }   // MbeColors.nColorSTS; }
			set { layerColors[(int)MbeLayer.LayerIndex.CMP] = value; }
		}



        /// <summary>
        /// L2�p�^�[���F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint L2
        {
            get { return layerColors[(int)MbeLayer.LayerIndex.L2]; }
            set { layerColors[(int)MbeLayer.LayerIndex.L2] = value; }
        }



        /// <summary>
        /// L3�p�^�[���F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint L3
        {
            get { return layerColors[(int)MbeLayer.LayerIndex.L3]; }
            set { layerColors[(int)MbeLayer.LayerIndex.L3] = value; }
        }


        /// <summary>
        /// MMC�F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint MMC
        {
            get { return layerColors[(int)MbeLayer.LayerIndex.MMC]; }
            set { layerColors[(int)MbeLayer.LayerIndex.MMC] = value; }
        }


        /// <summary>
        /// MMS�F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint MMS
        {
            get { return layerColors[(int)MbeLayer.LayerIndex.MMS]; }
            set { layerColors[(int)MbeLayer.LayerIndex.MMS] = value; }
        }



		/// <summary>
		/// ���c�ʃ\���_���W�X�g�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint STS
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.STS];	}   // MbeColors.nColorSTS; }
			set { layerColors[(int)MbeLayer.LayerIndex.STS] = value; }
		}


		/// <summary>
		/// ���i�ʃ\���_���W�X�g�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint STC
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.STC]; }
			set { layerColors[(int)MbeLayer.LayerIndex.STC] = value; }
		}



		/// <summary>
		/// ���c�ʃV���N�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint PLS
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.PLS]; }   // MbeColors.nColorSTS; }
			set { layerColors[(int)MbeLayer.LayerIndex.PLS] = value; }
		}


		/// <summary>
		/// ���i�ʃV���N�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint PLC
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.PLC]; }
			set { layerColors[(int)MbeLayer.LayerIndex.PLC] = value; }
		}


		/// <summary>
		/// �f�B�����W�����̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint DIM
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.DIM]; }
			set { layerColors[(int)MbeLayer.LayerIndex.DIM] = value; }
		}



		/// <summary>
		/// �h�����̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint DRL
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.DRL]; }
			set { layerColors[(int)MbeLayer.LayerIndex.DRL] = value; }
		}

        ///// <summary>
        ///// �h�������֊s�̐F�̐����l�̐ݒ�Ǝ擾
        ///// </summary>
        //public static uint Drl
        //{
        //    get { return layerColors[(int)MbeLayer.LayerIndex.DRL]; }
        //    set { layerColors[(int)MbeLayer.LayerIndex.DRL] = value; }
        //}

        ///// <summary>
        ///// �h�������֊s�̐F�̎擾
        ///// </summary>
        //public static Color ColorDrl
        //{
        //    get { return Color.FromArgb(unchecked((int)Drl)); }
        //}


		/// <summary>
		/// �h�L�������g�F�̐����l�̐ݒ�Ǝ擾
		/// </summary>
		public static uint DOC
		{
			get { return layerColors[(int)MbeLayer.LayerIndex.DOC]; }
			set { layerColors[(int)MbeLayer.LayerIndex.DOC] = value; }
		}


//////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �X���[�z�[���p�b�h����F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_PTH
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.PTH]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.PTH] = value; }
        }


        /// <summary>
        /// ���c�ʃp�^�[������F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_SOL
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.SOL]; }   // MbeColors.nColorSTS; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.SOL] = value; }
        }


        /// <summary>
        /// ���i�ʃp�^�[������F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_CMP
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.CMP]; }   // MbeColors.nColorSTS; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.CMP] = value; }
        }

        /// <summary>
        /// L2�p�^�[������F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_L2
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.L2]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.L2] = value; }
        }



        /// <summary>
        /// L3�p�^�[������F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_L3
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.L3]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.L3] = value; }
        }



        /// <summary>
        /// MMC����F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_MMC
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.MMC]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.MMC] = value; }
        }


        /// <summary>
        /// MMS����F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_MMS
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.MMS]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.MMS] = value; }
        }


        /// <summary>
        /// ���c�ʃ\���_���W�X�g����F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_STS
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.STS]; }   // MbeColors.nColorSTS; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.STS] = value; }
        }


        /// <summary>
        /// ���i�ʃ\���_���W�X�g����F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_STC
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.STC]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.STC] = value; }
        }

        /// <summary>
        /// ���c�ʃV���N����F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_PLS
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.PLS]; }   // MbeColors.nColorSTS; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.PLS] = value; }
        }


        /// <summary>
        /// ���i�ʃV���N����F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_PLC
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.PLC]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.PLC] = value; }
        }


        /// <summary>
        /// �f�B�����W��������F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_DIM
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.DIM]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.DIM] = value; }
        }



        /// <summary>
        /// �h��������F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_DRL
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.DRL]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.DRL] = value; }
        }


        /// <summary>
        /// �h�L�������g����F�̐����l�̐ݒ�Ǝ擾
        /// </summary>
        public static uint PRINT_DOC
        {
            get { return layerPrintColors[(int)MbeLayer.LayerIndex.DOC]; }
            set { layerPrintColors[(int)MbeLayer.LayerIndex.DOC] = value; }
        }

	}
}
