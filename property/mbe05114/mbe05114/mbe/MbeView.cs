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



namespace mbe
{
	partial class  MbeView : ScrollableControl
	{
		#region �萔�̒�`

		/// <summary>
		/// �r�b�g�}�b�v���[�h�ł̃J���[����̍ő�𑜓x(dpi)
		/// </summary>
        public const int COLOR_PRINT_BMP_MAX_RESOLUTION = 600;

        ///// <summary>
        ///// ���[�N�G���A�̐ݒ�\�ȍő啝(0.0001mm�P��)
        ///// </summary>
        //public const int WORK_AREA_MAX_WIDTH = 8000000;

        ///// <summary>
        ///// ���[�N�G���A�̃f�t�H���g�̕�(0.0001mm�P��)
        ///// </summary>
        //public const int WORK_AREA_DEFAULT_WIDTH = 3000000;

        ///// <summary>
        ///// ���[�N�G���A�̐ݒ�\�ȍŏ���(0.0001mm�P��)
        ///// </summary>
        //public const int WORK_AREA_MIN_WIDTH = 500000;

        ///// <summary>
        /////���[�N�G���A�̐ݒ�\�ȍő卂(0.0001mm�P��) 
        ///// </summary>
        //public const int WORK_AREA_MAX_HEIGHT = 8000000;

        ///// <summary>
        ///// ���[�N�G���A�̃f�t�H���g�̍���(0.0001mm�P��)
        ///// </summary>
        //public const int WORK_AREA_DEFAULT_HEIGHT = 3000000;

        ///// <summary>
        /////���[�N�G���A�̐ݒ�\�ȍŏ���(0.0001mm�P��) 
        ///// </summary>
        //public const int WORK_AREA_MIN_HEIGHT = 500000;

		/// <summary>
		///���[�N�G���A�̎��͂̏����}�[�W�� 
		/// </summary>
		private const int WORK_AREA_MARGIN_BASE = 500000;

		/// <summary>
		/// �\���ɂ�����ő�k�����B1�s�N�Z��������̎����@�B(0.0001mm�P��)
		/// </summary>
		public const double MAX_DISPLAY_SCALE = 10000;

		/// <summary>
		/// �\���ɂ�����ŏ��k�����B1�s�N�Z��������̎����@�B(0.0001mm�P��)
		/// </summary>
		public const double MIN_DISPLAY_SCALE = 1;

		/// <summary>
		/// �\���ɂ����鏉���k�����B1�s�N�Z��������̎����@�B(0.0001mm�P��)
		/// </summary>
		public const double INITIAL_DISPLAY_SCALE = 1270.0F;

		/// <summary>
		///�����X�N���[���ʒu 
		/// </summary>
		public const double INITIAL_SCROLL_POS = 0.2F;

        ///// <summary>
        ///// �����O���b�h�Ԋu(0.0001mm�P��)
        ///// </summary>
        //public const int INITIAL_GRID_VALUE = 12700;

        ///// <summary>
        ///// �ő�O���b�h�Ԋu(0.0001mm�P��)
        ///// </summary>
        //public const int MAX_GRID_VALUE = 100000;

        ///// <summary>
        ///// �ŏ��O���b�h�Ԋu(0.0001mm�P��)
        ///// </summary>
        //public const int MIN_GRID_VALUE = 10;

        ///// <summary>
        ///// �����O���b�h�\���Ԋu(�O���b�h�̖{��)
        ///// </summary>
        //public const int INITIAL_GRID_DISPLAY_EVERY = 10;

        ///// <summary>
        ///// �ő�O���b�h�\���Ԋu(�O���b�h�̖{��)
        ///// </summary>
        //public const int MAX_GRID_DISPLAY_EVERY = 1000;
		
        ///// <summary>
        ///// �ŏ��O���b�h�\���Ԋu(�O���b�h�̖{��)
        ///// </summary>
        //public const int MIN_GRID_DISPLAY_EVERY = 1;


		/// <summary>
		/// Zoom In/Out�����Ƃ��́A�\���ɂ�����k�����̕ω����̃f�t�H���g�l
		/// </summary>
		public const double DEFAULT_ZOOM_STEP = 1.5F;

		/// <summary>
		/// �}�E�X�̃z�C�[���̃X�e�b�v 
		/// </summary>
		/// <remarks>
		/// �z�C�[���̕ω��ʂ�������z���邽�т�ZoomIn/Out���s��
		/// </remarks>
		public const int MOUSE_WHEEL_STEP = 120;

		
		/// <summary>
		/// ����}�[�W���̍ő�l
		/// </summary>
		public const int PRINT_MARGIN_MAX = 300000;


		/// <summary>
		/// ����}�[�W���̃f�t�H���g
		/// </summary>
		public const int PRINT_MARGIN_DEFAULT = 254000;

        
        public string ComponentPinProperty
        {
            get { return componentPinProperty; }
            //set { componentPinProperty = value; }
        }

        private string componentPinProperty;

        public static MbeLib toolMarkLib = null;

		/// <summary>
		/// View�̃��[�h
		/// </summary>
		public enum ModeMajor
		{
			/// <summary>
			/// �Z���N�^�c�[�����[�h
			/// </summary>
			SelectorMode = 1,
			/// <summary>
			/// �O���b�h���_�̐ݒ胂�[�h
			/// </summary>
			SetGridOriginMode,
			/// <summary>
			/// �ڑ��`�F�b�N���[�h
			/// </summary>
			ConChkMode,
			/// <summary>
			/// �z�[���z�u
			/// </summary>
			PlaceHole,
			/// <summary>
			/// �X���[�z�[���s���z�u
			/// </summary>
			PlacePTH,
			/// <summary>
			/// ���C���z�u
			/// </summary>
			PlaceLine,
			/// <summary>
			/// �~�ʔz�u
			/// </summary>
			PlaceArc,
			/// <summary>
			/// ������z�u
			/// </summary>
			PlaceText,
			/// <summary>
			/// �p�b�h�z�u
			/// </summary>
			PlacePad,
			/// <summary>
			/// �|���S���z�u
			/// </summary>
			PlacePolygon,
			/// <summary>
			/// �R���|�[�l���g�z�u
			/// </summary>
			PlaceComponent,
            /// <summary>
            /// ruler
            /// </summary>
            Measure

		};

		#endregion

		/// <summary>
		/// ���C���[��񂪕ς�����Ƃ��̒ʒm�f���Q�[�g
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void LayerSelectChangeEventHandler(object sender, LayerInfoEventArgs e);

		/// <summary>
		/// ���C���[��񂪕ς�����Ƃ��̒ʒm�n���h��
		/// </summary>
		public event LayerSelectChangeEventHandler LayerSelectChange;

		/// <summary>
		/// �r���[���g�����[�h��ύX�����Ƃ��̒ʒm�f���Q�[�g
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//public delegate void MBeViewChangeModeEventHandler(object sender, EventArgs e);

		//public event MBeViewChangeModeEventHandler ViewChangeModeEvent;



		#region private, protected �t�B�[���h

		/// <summary>
		/// �h�L�������g�N���X�̃C���X�^���X
		/// </summary>
		protected MbeDoc document;

		public MbeDoc Document
		{
			get { return document; }
		}

		public bool BoardFontReady
		{
			get { return boardFont.Ready; }
		}

		/// <summary>
		/// �`��p�o�b�t�@
		/// </summary>
		private BufferedGraphicsContext bgContext;
		private BufferedGraphics bgraphics;
		#region		�J�[�\��

		/// <summary>
		/// ���_��(0,0)�Ƃ����}�ʍ��W
		/// </summary>
        protected Point cursorPos;
		
		/// <summary>
		/// ���z�E�B���h�E���W 
		/// </summary>
		//protected Point cursorPosVW;


		/// <summary>
		/// �O���b�h���_���W�ݒ胂�[�h�̃J�[�\���Ƃ��Ďg��ICON
		/// </summary>
		protected Icon gridOriginCursorIcon;

		/// <summary>
		/// ���������}�[�N�Ɏg��ICON
		/// </summary>
		protected Icon dividableMarkIcon;


		protected Icon gapChkMarkIcon;


		#endregion

		/// <summary>
		/// ���샂�[�h
		/// </summary>
		protected ModeMajor modeMajor;
		protected int modeMinor;


        protected enum ContectMenuOptionMode
        {
            IDLE = 0,
            LINEBENDING
        }

        protected ContectMenuOptionMode contextMenuOptionMode;


        public MbeLibs mbeLibs;

		/// <summary>
		/// OnPaint�ɂ����ĕ`��o�b�t�@�X�V������
		/// </summary>
		/// <remarks>
		/// �X�N���[�������Ƃ��͕`��o�b�t�@�͎����I�ɍX�V����邪
		/// ����ȊO�ŁA�`��o�b�t�@���X�V����K�v������ꍇ�́A
		/// Invalidate()��Refresh()�����s����O�ɁAupdateBuffForce��false�ɂ��Ă����B
		/// </remarks>
		protected bool updateBuffForce;

		/// <summary>
		/// �Ō�ɕ`��o�b�t�@���X�V�����Ƃ��̃X�N���[���|�W�V����
		/// </summary>
		/// <remarks>
		/// ���݂̃X�N���[���|�W�V���������̒l�ƈقȂ�Ƃ��AupdateBuffForce�̒l��
		/// �֌W�Ȃ��A�`��o�b�t�@���X�V����B
		/// </remarks>
		protected Point lastDrawBuffScrollPos;

		/// <summary>
		/// OnPaint()�ł̕`�拖�t���O
		/// </summary>
		protected bool enablePaint;

		/// <summary>
		/// DRC�}�[�N��\�����邩�ǂ���
		/// </summary>
		protected bool displayDrcMark;


		/// <summary>
		/// �\����̃��[�N�G���A�̃T�C�Y
		/// </summary>
		/// <remarks>
		/// SetVirtualWindowSize()�Őݒ肳���B
		/// </remarks>
		protected Size sizeViewableWorkArea;

		/// <summary>
		/// ���[�N�G���A�̍��}�[�W��
		/// </summary>
		protected int leftMargin;


		/// <summary>
		/// �N���C�A���g�G���A�̃T�C�Y
		/// </summary>
		protected Size sizeClient;
		
		/// <summary>
		/// �`��k����
		/// </summary>
		/// <remarks>
		/// 1�s�N�Z��������̎��T�C�Y����m�P�ʂŕ\���������́B
		/// �k����100.0F�́A0.1mm��1�s�N�Z���ň������ƂɂȂ�B
		/// </remarks>
		protected double displayScale;

		/// <summary>
		/// �o�͗p�X�g���[�N�t�H���g���Ǘ�����
		/// </summary>
		public static MbeBoardFont boardFont;

		protected ulong selectableLayer;
		protected MbeLayer.LayerValue selectLayer;
		protected ulong visibleLayer;

        protected MbeLayer.LayerValue placeComponentLayer;

		protected MbeObj placeObj;
		protected MbeObjPTH placeObjPTH;
		protected MbeObjArc placeObjArc;
		protected MbeObjLine placeObjLine;
		protected MbeObjPolygon placeObjPolygon;
		protected MbeObjPolygon propertyObjPolygon;//�z�u�O�Ƀv���p�e�B��ҏW����̂Ɏg��
        protected MbeLayer.LayerValue polygonLayer;

		protected MbeObjText placeObjText;
		protected MbeObjPinSMD placeObjPinSMD;
		private System.Drawing.Printing.PrintDocument printDocument1;
		private PrintDialog printDialog1;
		protected MbeObjHole placeObjHole;
		
		private IContainer components;

		protected bool releaseTempOnMouseDown;


        private FindForm findForm;

//#if MONO
//        private ContextMenu contextMenuView;
//        private MenuItem contextMenuProperty;
//        private MenuItem contextMenuAddNode;
//        //private MenuItem toolStripSeparator1;
//        private MenuItem contextMenuCut;
//        private MenuItem contextMenuCopy;
//        private MenuItem contextMenuPaste;
//#else
		private ContextMenuStrip contextMenuOnView;
		private ToolStripMenuItem contextMenuProperty;
		private ToolStripMenuItem contextMenuAddNode;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem contextMenuCut;
		private ToolStripMenuItem contextMenuCopy;
		private ToolStripMenuItem contextMenuPaste;
        private ToolStripMenuItem contextMenuBulkProperty;
//#endif

        public enum PrintColorModeValue
        {
            BlackAndWhite = 0,
            ColorVector = 1,
            ColorBitmap = 2
        }

        private string headerText;
        private bool printHeader;
		private int printLeftMargin;
		private int printBottomMargin;
        private bool print2xMode;
        private bool printMirrorMode;

        public bool PrintMirrorMode
        {
            get { return printMirrorMode; }
            set { printMirrorMode = value; }
        }
        private bool centerPunchMode;
        private bool printToolMarkMode;
        private PrintColorModeValue printColorMode;
        private bool exportImageColorMode;

        public bool ExportImageColorMode
        {
            get { return exportImageColorMode; }
            set { exportImageColorMode = value; }
        }


        private int printPageCount;     //����y�[�W��
        private int currentPrintPage;   //���݂̈���y�[�W
        private ToolStripMenuItem MenuBulkPropHole;
        private ToolStripMenuItem MenuBulkPropPTH;
        private ToolStripMenuItem MenuBulkPropPad;
        private ToolStripMenuItem MenuBulkPropLine;
        private ToolStripMenuItem MenuBulkPropText;
        private ToolStripMenuItem MenuBulkPropLayer;
        private ToolStripMenuItem MenuBulkPropPolygon;
        private ToolStripMenuItem contextMenuOption1;
        private ToolStripMenuItem contextMenuOption2;
        private ImageList imageListLineBending;
        private ToolStripMenuItem contextMenuOption0;


        private bool enableOneKeyToolSelection;

           
        private List<PrintPageLayerInfo> printPageLayerList;


        private void SetupPrintPageInfo()
        {
            currentPrintPage = 0;
            if(Properties.Settings.Default.PrintCurrentView){
                printPageLayerList = null;
                printPageCount = 1;
            }else{
                List<PrintPageLayerInfo> ppll = PrintPageLayerInfo.LoadMyStandard();
                if (ppll != null) {
                    int count = ppll.Count;
                    printPageLayerList = new List<PrintPageLayerInfo>();
                    foreach (PrintPageLayerInfo info in ppll) {
                        if (info.active) {
                            printPageLayerList.Add(info);
                        }
                    }
                    printPageCount = printPageLayerList.Count;
                }
                if(ppll == null || printPageCount == 0){
                    printPageCount = 1;
                    printPageLayerList = null;
                }
            }
        }

        public string HeaderText
        {
            get { return headerText; }
            set { headerText = value; }
        }

        public bool PrintHeader
        {
            get { return printHeader; }
            set { printHeader = value; }
        }

        
        public bool Print2xMode
        {
            get { return print2xMode; }
            set { print2xMode = value; }
        }

        public bool CenterPunchMode
        {
            get { return centerPunchMode; }
            set { centerPunchMode = value; }
        }

        public bool PrintToolMarkMode
        {
            get { return printToolMarkMode; }
            set { printToolMarkMode = value; }
        }

        public PrintColorModeValue PrintColorMode
        {
            get { return printColorMode; }
            set { printColorMode = value; }
        }


		public int PrintLeftMargin
		{
			get { return printLeftMargin; }
			set { 
				printLeftMargin = value;
				if (printLeftMargin < 0) {
					printLeftMargin = 0;
				} else if (printLeftMargin > PRINT_MARGIN_MAX) {
					printLeftMargin = PRINT_MARGIN_MAX;
				}

			}
		}

		public int PrintBottomMargin
		{
			get { return printBottomMargin; }
			set { 
				printBottomMargin = value;
				if (printBottomMargin < 0) {
					printBottomMargin = 0;
				} else if (printBottomMargin > PRINT_MARGIN_MAX) {
					printBottomMargin = PRINT_MARGIN_MAX;
				}
			}
		}


		protected bool dblClkPlacePolygon;

		static public bool ViewPolygonFrame
		{
			get { return viewPolygonFrame; }
		}

		public void SetViewPolygonFrameOnly(bool frameOnlyMode)
		{
			viewPolygonFrame = frameOnlyMode;
			if (!viewPolygonFrame) {
				FillPolygon();
			} else {
				RedrawAll();
			}
		}

        public bool EnableOneKeyToolSelection
        {
            get { return enableOneKeyToolSelection; }
            set { enableOneKeyToolSelection = value; }
        }


		static protected bool viewPolygonFrame; 

		/// <summary>
		/// ���C���[�I�����̐ݒ�B�C�x���g�𔭍s����B
		/// </summary>
		/// <param name="select"></param>
		/// <param name="selectable"></param>
		protected void SetLayerSelectInfo(MbeLayer.LayerValue select,
										  ulong selectable)
		{
			//relateiveLayer = relative;
			SelectLayer = select;
			selectableLayer = selectable;
			ulong visible = this.GetVisibleLayer();
			visible |= (ulong)SelectLayer;
			this.SetVisibleLayer(visible);
			if (LayerSelectChange != null) {
				LayerInfoEventArgs e = new LayerInfoEventArgs();
				e.selectableLayer = SelectableLayer;
				e.selectLayer = SelectLayer;
				e.visibleLayer = visible;
				LayerSelectChange(this, e);
			}
		}

		/// <summary>
		/// �r���[���g�����[�h��ύX����ۂɂ�����Ă�
		/// </summary>
		/// <param name="major"></param>
		//protected void ViewChangeMode(ModeMajor major)
		//{
		//    SetMode(major);
		//    if (ViewChangeModeEvent != null) {
		//        ViewChangeModeEvent(this, null);
		//    }
		//}


		/// <summary>
		/// Zoom In/Out�����Ƃ��́A�\���ɂ�����k�����̕ω���
		/// </summary>
		private double zoomStep;

		/// <summary>
		/// Cursor.Hide()/Show()�̉񐔂��Ǘ�����
		/// </summary>
		//private int cursorHideCount;

		// �}�E�X�{�^�����쎞�̎����W
		//protected PointLog leftButtonEventPoints; 

		protected int mouseWheelValue;

		//
		//�}�E�X�̃h���b�O����Ɋւ��ϐ�
		//

		/// <summary>
		/// ���{�^�������h���b�O������
		/// </summary>
		protected DragInfo lbDragInfo;

		/// <summary>
		/// �E�{�^�������h���b�O������
		/// </summary>
		protected DragInfo rbDragInfo;

       

		/// <summary>
		/// �A�N�e�B�u�|�C���g�ɐڋ߂��Ă��邩�ǂ���
		/// </summary>
		protected bool closeToActivePoint;

		/// <summary>
		/// �A�N�e�B�u�|�C���g
		/// </summary>
		protected Point currentActivePoint;

		/// <summary>
		/// �����\�ȓ_���L��
		/// </summary>
		protected bool divideablePointValid;

		/// <summary>
		/// �����\�ȓ_
		/// </summary>
		protected Point divideablePoint;

		protected MbeObj dividableLineObj;

		protected int dividableLineIndex;


		/// <summary>
		/// �A�N�e�B�u�|�C���g�ƃ}�E�X�|�C���^�̍��W�̃I�t�Z�b�g
		/// </summary>
		protected Size activePointOffset;

		/// <summary>
		/// �ꎞ�f�[�^�̈ړ�����true
		/// </summary>
		protected bool movingTempData;
        protected bool movedTempData;


		//protected Point ptDragFrom;
		//protected Point ptDragTo;
		//protected bool draggingNow;


        /// <summary>
        /// Measure �̌��_
        /// </summary>
        //protected Point ptMeasureOrigin;
        protected Size sizeMeasure;
        protected double measureLength;
        protected List<Point> listMeasurePoint;



		// �O���b�h�֌W�ϐ�
		// gridOrigin�͐}�ʎ����W�ŕ\���O���b�h���_
//		protected int gridValue;
		protected Point gridOrigin;
//		protected int gridDisplayEvery;				//�O���b�h�̕\���Ԋu
        
//        protected List<GridInfo> myStandardGrid;
        protected GridInfo currentGridInfo;


 


		#endregion


		public bool CanDivideLine()
		{
			return divideablePointValid;
		}

		//public bool CanDeleteNode()
		//{
		//    return false;
		//}


        /// <summary>
        /// �v���p�e�B�̕ҏW���ł��邩�ǂ�����Ԃ�
        /// </summary>
        /// <returns></returns>
        public bool CanEditProperty()
        {
            if (document.CanEditProperty()) return true;
            //switch (modeMajor) {
            //    case ModeMajor.PlaceArc:
            //    case ModeMajor.PlaceHole:
            //    case ModeMajor.PlaceLine:
            //    case ModeMajor.PlacePad:
            //    case ModeMajor.PlacePolygon:
            //    case ModeMajor.PlacePTH:
            //        return true;
            //        //break;
            //    default:
            //        break;
            //}
            return false;
        }


		public void GetMode(out ModeMajor major, out int minor)
		{
			major = modeMajor;
			minor = modeMinor;
		}


		/// <summary>
		/// View���샂�[�h�̐ݒ�
		/// </summary>
		/// <param name="major"></param>
		/// <remarks>
		/// �t���[���E�B���h�E����ύX����ꍇ�͂�����ĂԁB
		/// View�������烂�[�h��ύX����ꍇ�́AViewChangeMode()���ĂԂ��ƁB
		/// </remarks>
		public void SetMode(ModeMajor major)//,MbeLayer.LayerValue candidateLayer)
		{
			System.Diagnostics.Debug.WriteLine("MbeView.SetMode");
			document.ReleaseTemp();
			
            //post process of previous mode
            
            switch (modeMajor){
                case ModeMajor.ConChkMode:
    				document.ClearConnectCheckFlag();
                    break;
                case ModeMajor.PlaceArc:
                case ModeMajor.PlacePad:
                    break;
                case ModeMajor.PlaceText:
                    placeObjText.Layer = selectLayer;
                    break;
                case ModeMajor.PlaceLine:
                    placeObjLine.Layer = selectLayer;
                    break;
                case ModeMajor.PlacePolygon:
                    polygonLayer = selectLayer;
                    break;
                case ModeMajor.Measure:
                    listMeasurePoint.Clear();
                    break;
                default:
                    break;
            }



			modeMajor = major;
			modeMinor = 0;
			//leftButtonEventPoints.InitIndex();
			placeObj = null;

			//leftButtonDownPtIndex = 0;
			//SetCursor();

			//if (major == MbeView.ModeMajor.SelectorMode) {
			//    this.ContextMenuStrip = this.contextMenuView;
			//} else {
			//    this.ContextMenuStrip = null;
			//}



			switch (modeMajor) {
				case ModeMajor.PlaceHole:
					placeObj = placeObjHole;
					SetLayerSelectInfo(MbeObjHole.NewSelectLayer(SelectLayer),
										MbeObjHole.SelectableLayer());
					break;
				case ModeMajor.PlacePTH:
                    placeObjPTH.ThermalRelief = MbeObjPin.PadThermalRelief.ThmlRlfInComp;
                    placeObjPTH.No_ResistMask = false;
					placeObj = placeObjPTH;
					SetLayerSelectInfo(MbeObjPTH.NewSelectLayer(SelectLayer),
										MbeObjPTH.SelectableLayer());
					break;
				case ModeMajor.PlacePad:
                    placeObjPinSMD.ThermalRelief = MbeObjPin.PadThermalRelief.ThmlRlfInComp;
                    placeObjPinSMD.No_MM = false;
                    placeObjPinSMD.No_ResistMask = false;
					placeObj = placeObjPinSMD;
					SetLayerSelectInfo(placeObjPinSMD.Layer ,
										MbeObjPinSMD.SelectableLayer());
					break;
				case ModeMajor.PlaceLine:
					SetLayerSelectInfo(placeObjLine.Layer ,
										MbeObjLine.SelectableLayer());
					break;
				case ModeMajor.PlacePolygon:
					MbeView.viewPolygonFrame = true;
					dblClkPlacePolygon = false;

                    SetLayerSelectInfo(polygonLayer,
										MbeObjPolygon.SelectableLayer());
                    //SetLayerSelectInfo(placeObjLine.Layer,
                    //                    MbeObjPolygon.SelectableLayer());
					RedrawAll();
					break;
				case ModeMajor.PlaceArc:
					placeObj = placeObjArc;
					SetLayerSelectInfo(placeObjArc.Layer,
										MbeObjArc.SelectableLayer());
					break;
                case ModeMajor.PlaceComponent:
                    SetLayerSelectInfo(placeComponentLayer,
                                        ((ulong)MbeLayer.LayerValue.CMP) | ((ulong)MbeLayer.LayerValue.SOL));
                    break;
                case ModeMajor.PlaceText:
					SetLayerSelectInfo(placeObjText.Layer,
										MbeObjText.SelectableLayer());
					//SetLayerSelectInfo(MbeObjLine.NewSelectLayer(SelectLayer),
					//                    MbeObjLine.SelectableLayer());
					break;
				case ModeMajor.ConChkMode:
					SetLayerSelectInfo(MbeLayer.LayerValue.NUL,
									   0);
					break;
				case ModeMajor.SelectorMode:
				case ModeMajor.SetGridOriginMode:
					SetLayerSelectInfo(MbeLayer.LayerValue.NUL,
									   0);
									   //0xFFFFFFFFFFFFFFFF);
					break;
                case ModeMajor.Measure:
                    SetLayerSelectInfo(MbeLayer.LayerValue.NUL,
                                       0);
                    break;
                default:
					RedrawAll();
					break;

			}
			//if (enablePaint) {
			//    RedrawAll();
			//    //Invalidate();
			//}
		}

		/// <summary>
		/// �J�[�\���ʒu�̐}�ʍ��W�̎擾
		/// </summary>
		public Point CursorPos
		{
			get { return cursorPos; }
		}

		/// <summary>
		/// �J�[�\���ʒu�̃O���b�h���_���W�̎擾
		/// </summary>
		public Point CursorGridCoodPos
		{
			get { return cursorPos-(Size)gridOrigin; }
		}


		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public MbeView()
        {
            AutoScroll = true;
            InitializeComponent();
            //InitializeContextMenu();
            visibleLayer = (ulong)MbeLayer.LayerValue.PTH |
                    (ulong)MbeLayer.LayerValue.CMP |
                    (ulong)MbeLayer.LayerValue.L2 |
                    (ulong)MbeLayer.LayerValue.L3 |
                    (ulong)MbeLayer.LayerValue.SOL |
                    (ulong)MbeLayer.LayerValue.PLC |
                    (ulong)MbeLayer.LayerValue.PLS |
                    (ulong)MbeLayer.LayerValue.DOC |
                    (ulong)MbeLayer.LayerValue.DRL |
                    (ulong)MbeLayer.LayerValue.DIM;

            document = new MbeDoc();
            enablePaint = false;
            sizeClient = new Size(100, 100);	//�K���ȏ����l

            MbeMyStd[] giArray = MbeMyStd.LoadMyStdInfoArray(Properties.Settings.Default.DefaultGridInfoString);
            if (giArray.Length == 0) {
                currentGridInfo = new GridInfo();
            } else {
                currentGridInfo = (GridInfo)giArray[0];
            }

 
            //currentGridInfo = new GridInfo();
            //currentGridInfo.FromString(Properties.Settings.Default.DefaultGridInfoString);
            
            //myStandardGrid = Properties.Settings.Default.MyStandardGrid;
            //if (myStandardGrid == null) {
            //    myStandardGrid = new List<GridInfo>();
            //}


            gridOrigin = new Point(0, 0);	//�����O���b�h���_�́A�}�ʌ��_�ƈ�v

            //int margin;
            //margin = Properties.Settings.Default.PrintLeftMargin;
            //if (margin < 0) {
            //    PrintLeftMargin = PRINT_MARGIN_DEFAULT;
            //} else {
            //    PrintLeftMargin = margin;
            //}

            //margin = Properties.Settings.Default.PrintBottomMargin;
            //if (margin < 0) {
            //    PrintBottomMargin = PRINT_MARGIN_DEFAULT;
            //} else {
            //    PrintBottomMargin = margin;
            //}

            //print2xMode     = Properties.Settings.Default.Print2xMode;
            //centerPunchMode = Properties.Settings.Default.CenterPunchMode;
            //printToolMarkMode = Properties.Settings.Default.ToolMarkPrint;
            //printColorMode = (PrintColorModeValue)Properties.Settings.Default.PrintColorMode;
            //exportImageColorMode = Properties.Settings.Default.ImageColorMode;


            boardFont = new MbeBoardFont();
            if (!Properties.Settings.Default.UseEmbeddedFont) {
                if (!boardFont.ReadFontDataFile(Properties.Settings.Default.PathBoardFont)) {
                    boardFont.SetupEmbeddedFont();
                }
            }

            MbeView.viewPolygonFrame = true;

            SetMode(ModeMajor.SelectorMode);

            lbDragInfo = new DragInfo();
            rbDragInfo = new DragInfo();
            

            bgContext = BufferedGraphicsManager.Current;
            bgContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            bgraphics = bgContext.Allocate(this.CreateGraphics(),
                 new Rectangle(0, 0, this.Width + 1, this.Height + 1));

            //�_�u���o�b�t�@�����O�ݒ�
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //�_�u���N���b�N����
            //this.SetStyle(ControlStyles.StandardDoubleClick, true);
            this.SetStyle(ControlStyles.StandardClick, true);


            displayScale = INITIAL_DISPLAY_SCALE;
            mouseWheelValue = 0;
            zoomStep = DEFAULT_ZOOM_STEP;
            //			SetDisplayScale(INITIAL_DISPLAY_SCALE);
            //			this.SetWorkAreaSize(WORK_AREA_MAX_WIDTH, WORK_AREA_MAX_HEIGHT);
            this.SetWorkAreaSize(MbeDoc.WORK_AREA_DEFAULT_WIDTH, MbeDoc.WORK_AREA_DEFAULT_HEIGHT);
            updateBuffForce = true;

            displayDrcMark = true;

            //this.KeyPreview = true;
            //enablePaint = true;

            //MbeObj mbeObj1 = new MbeObjPinSMD();
            //MbeObj mbeObj2 = mbeObj1.Duplicate();

            placeObj = null;
            placeObjPTH = new MbeObjPTH();
            placeObjPinSMD = new MbeObjPinSMD();
            placeObjLine = new MbeObjLine();
            placeObjPolygon = null;//new MbeObjPolygon();
            polygonLayer = MbeLayer.LayerValue.CMP;
            propertyObjPolygon = new MbeObjPolygon();
            placeObjArc = new MbeObjArc();
            placeObjText = new MbeObjText();
            placeObjText.Layer = MbeLayer.LayerValue.PLC;
            placeObjHole = new MbeObjHole();
            placeComponentLayer = MbeLayer.LayerValue.CMP;

            gridOriginCursorIcon = null;
            gapChkMarkIcon = null;

            dividableMarkIcon = null;
            closeToActivePoint = false;
            divideablePointValid = false;
            currentActivePoint = new Point(0, 0);
            activePointOffset = new Size(0, 0);
            movingTempData = false;
            movedTempData = false;

            releaseTempOnMouseDown = false;
            dblClkPlacePolygon = false;
            mbeLibs = null;

            findForm = null;
            findFormMovesCursor = null;

            componentPinProperty = "";

            lastCanBulkPropResult = new CanBulkPropResult();

            szLastMoveValue = new Size(0,0);

            contextMenuOptionMode = ContectMenuOptionMode.IDLE;

            enableOneKeyToolSelection = false;

            listMeasurePoint = new List<Point>();

            //ulong layerMask = (ulong)MbeLayer.LayerBit.CMP | (ulong)MbeLayer.LayerBit.DIM;

            mbeLibs = new MbeLibs();

        }

		/// <summary>
		/// DRC�}�[�N��\�����邩�ǂ���
		/// </summary>
		public bool DisplayDrcMark
		{
			get{
				return displayDrcMark;
			}
			set{
				displayDrcMark = value;
			}
		}


		/// <summary>
		/// �`��k�����̎擾�v���p�e�B
		/// </summary>
		public double DisplayScale
		{
			get
			{
				return displayScale;
			}
		}

		/// <summary>
		/// �`�惌�C���[�̐ݒ�Ǝ擾
		/// </summary>
		//public ulong DrawLayerMask
		//{
			//get { return drawLayerMask; }
			//set { 
			//    drawLayerMask = value;
			//    if (((drawLayerMask & (ulong)MbeLayer.LayerValue.CMP)!=0) ||
			//        ((drawLayerMask & (ulong)MbeLayer.LayerValue.SOL)!=0)) {
			//        drawLayerMask |= (ulong)(MbeLayer.LayerValue.HOLE);
			//    } else {
			//        drawLayerMask &= ~((ulong)MbeLayer.LayerValue.HOLE);
			//    }
			//}
		//}

		public ulong SelectableLayer
		{
			get { return selectableLayer; }
		}

		public MbeLayer.LayerValue SelectLayer
		{
			get { return selectLayer; }
			set { 
				selectLayer = value; 
				if (placeObj != null) {
					placeObj.Layer = selectLayer;
					Invalidate();
				}
			}
		}

		public ulong GetVisibleLayer()
		{ 
			return visibleLayer;
			//return document.docInfo.VisibleLayer;
		}

		public void SetVisibleLayer(ulong value)
		{
			visibleLayer = value;
			RedrawAll();
		}


		/// <summary>
		/// �Y�[���ɂ�����`��k�����̕ω����̐ݒ�Ǝ擾�v���p�e�B
		/// </summary>
		public double ZoomStep
		{
			get { return zoomStep; }
			set {
				if (value < 1.2) {
					value = 1.2;
				} else if (value > 2) {
					value = 2;
				}
				zoomStep = value;
			}
		}

		/// <summary>
		/// �Y�[���C���A�A�E�g���s��
		/// </summary>
		/// <param name="zoomIn">true�̂Ƃ��Y�[���C��(�g��\��)</param>
		public void ZoomInOut(bool zoomIn)
		{
			if (zoomIn) {
				SetDisplayScale(displayScale / zoomStep);
			} else {
				SetDisplayScale(displayScale * zoomStep);
			}
		}

		/// <summary>
		/// �`��k�����̐ݒ�
		/// </summary>
		/// <param name="value">�k����</param>
		/// <param name="ptR">�g��k���̒��S�ƂȂ�����W</param>
		/// <param name="ptC">�g��k���̒��S�ƂȂ�N���C�A���g���W</param>
		public void SetDisplayScale(double value,Point ptR,Point ptC)
		{
			if (displayScale == value) return;
			displayScale = value;
			if (displayScale < MIN_DISPLAY_SCALE) {
				displayScale = MIN_DISPLAY_SCALE;
			}else if(displayScale > MAX_DISPLAY_SCALE) {
				displayScale = MAX_DISPLAY_SCALE;
			}
            ////�O���b�h�l��1/10�Ƌ߂��Ƃ��ɂ͂���ɍ��킹�� Version 0.29p��xy�ʁX�̒l��ݒ�ł���悤�ɂȂ����̂ŃR�����g�A�E�g
            //double dispalyScale10 = dispalyScale * 10;
            //if ((gridValue * 0.9F) < dispalyScale10 && (gridValue * 1.1F) > dispalyScale10) {
            //    dispalyScale = gridValue / 10.0F;
            //}
			SetVirtualWindowSize();
			ScrollTo(ptR, ptC, displayScale);
		}

		/// <summary>
		/// �`��k�����̐ݒ�
		/// </summary>
		/// <param name="value">�k����</param>
		/// <remarks>
		/// �V�����{����ݒ肵�ă}�E�X�J�[�\���̈ʒu�܂��̓E�B���h�E�̒��S�Ŋg��k��
		/// </remarks>
		public void SetDisplayScale(double value)
		{
			Point ptMouse = MousePosition;
			System.Diagnostics.Debug.WriteLine("SetDisplayScale " + ptMouse.X + "," + ptMouse.Y);
			Point ptMouseC = PointToClient(ptMouse);
			//Point ptMouseR = ptMouseC;
			//ClientToVw(ref ptMouseR);
			//VwToReal(ref ptMouseR, dispalyScale);
			//if (ptMouseR.X > 0 && ptMouseR.X < sizeWorkArea.Width &&
			//    ptMouseR.Y > 0 && ptMouseR.Y < sizeWorkArea.Height) {
			//    SetDisplayScale(value, ptMouseR, ptMouseC);
			//} else {
			if( ptMouseC.X>0 && ptMouseC.X<sizeClient.Width &&
				ptMouseC.Y>0 && ptMouseC.Y<sizeClient.Height){
				Point ptR = ptMouseC;
				ptR = ClientToVw(ptR);
				ptR = VwToReal(ptR, displayScale);
				SetDisplayScale(value, ptR, ptMouseC);
			} else {
				Point ptC = new Point(sizeClient.Width / 2, sizeClient.Height / 2);
				Point ptR = ptC;
				ptR = ClientToVw(ptR);
				ptR = VwToReal(ptR, displayScale);
				SetDisplayScale(value, ptR, ptC);
			}
		}

		/// <summary>
		/// �y�C���g�̋���
		/// </summary>
		public bool EnablePaint
		{
			set
			{
				enablePaint = value;
				Invalidate();
			}
			get
			{
				return enablePaint;
			}
		}

		///// <summary>
		///// �}�ʌ��_�̉��z�E�B���h�E���W��Ԃ�
		///// </summary>
		///// <returns></returns>
		//public Point GetOriginPointVW()
		//{
		//    int x = WORK_AREA_MARGIN_BASE;
		//    int y = WORK_AREA_MARGIN_BASE + WORK_AREA_MAX_HEIGHT;
		//    return new Point(x, y);
		//}

		#region ���W�ϊ�
		/// <summary>
		/// ���z�E�B���h�E���W�������W�ɕϊ�����
		/// </summary>
		/// <param name="point">�ϊ�������W�̎Q��</param>
		/// <param name="scale">�k����</param>
		protected Point VwToReal(Point point,double scale)
		{
			return new Point(	(int)(point.X * scale) - leftMargin,
								(sizeViewableWorkArea.Height) - (int)(point.Y * scale));
		}

		/// <summary>
		/// �����W�����z�E�B���h�E���W�ɕϊ�����
		/// </summary>
		/// <param name="point">�ϊ�������W�̎Q��</param>
		/// <param name="scale">�k����</param>
		protected Point RealToVw(Point point, double scale)
		{
			return new Point(	(int)((point.X + leftMargin) / scale),
								(int)((sizeViewableWorkArea.Height - point.Y) / scale));
		}

		/// <summary>
		/// �����W��`����W�ɕϊ�����
		/// </summary>
		/// <param name="point">�ϊ�������W�̎Q��</param>
		/// <param name="scale">�k����</param>
		protected Point RealToDraw(Point point, double scale)
		{
            return new Point((int)Math.Round(point.X / scale), (int)Math.Round(-point.Y / scale));
		}


  


		/// <summary>
		/// �N���C�A���g���W�����z�E�B���h�E���W�ɕϊ�����
		/// </summary>
		/// <param name="point">�ϊ�������W�̎Q��</param>
		protected Point ClientToVw(Point point)
		{
			return new Point(	(point.X - this.AutoScrollPosition.X),
								(point.Y - this.AutoScrollPosition.Y));
		}


		/// <summary>
		/// �N���C�A���g���W��`����W�ɕϊ�����
		/// </summary>
		/// <param name="point"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		protected Point ClientToDraw(Point point, double scale)
		{
			point = ClientToVw(point);
			point = VwToReal(point, scale);
			return RealToDraw(point, scale);
		}



		#endregion

		#region �O���b�h�֌W

 
        //public List<GridInfo> MyStandardGrid
        //{
        //    get { return myStandardGrid; }
        //    set { 
        //        myStandardGrid = value;
        //        Properties.Settings.Default.MyStandardGrid = myStandardGrid;
        //    }
        //}


		/// <summary>
		/// �O���b�h�Ԋu�̎擾
		/// </summary>
        public GridInfo CurrentGridInfo
        {
            get { return currentGridInfo; }
        }
        
        /// <summary>
		/// �O���b�h�Ԋu�̐ݒ�
		/// </summary>
		/// <param name="value">�O���b�h�Ԋu��0.001mm�P�ʂ̐����Ŏw��</param>
        public void SetCurrentGridInfo(GridInfo gridInfo)
        {
            currentGridInfo.Horizontal = gridInfo.Horizontal;
            currentGridInfo.Vertical = gridInfo.Vertical;
            currentGridInfo.DisplayEvery = gridInfo.DisplayEvery;

            GridInfo[] giArray = new GridInfo[1];
            giArray[0] = currentGridInfo;
            Properties.Settings.Default.DefaultGridInfoString = MbeMyStd.SaveMyStdInfoArray(giArray);
            Properties.Settings.Default.Save();

            //Properties.Settings.Default.DefaultGridInfoString = currentGridInfo.SaveToString();
            SetVirtualWindowSize();
        }




		/// <summary>
		/// �O���b�h���_�̎擾
		/// </summary>
		public Point GridOrigin
		{
			get
			{
				return gridOrigin;
			}
		}
		
		/// <summary>
		/// �O���b�h���_�̐ݒ�
		/// </summary>
		/// <param name="pt">�O���b�h���_�ƂȂ�}�ʍ��W</param>
		public void SetGridOrigin(Point pt)
		{
			gridOrigin = pt;
			updateBuffForce = true;
			if (enablePaint) {
				Invalidate();
			}
		}

		#endregion

		/// <summary>
		/// �J�[�\���ʒu�̃X�i�b�v
		/// </summary>
		/// <param name="ptR">�X�i�b�v�������J�[�\���ʒu��}�ʍ��W�Ŏw��</param>
		/// <returns>�X�i�b�v������̍��W</returns>
		protected Point snapCursorPos(Point ptR)
		{
			//if (modeMajor == ModeMajor.SelectorMode) return ptR;
            Point ptSnapToGrid = snapToGrid(ptR);
            double xd = ptR.X -ptSnapToGrid.X;
            double yd = ptR.Y -ptSnapToGrid.Y;
            int  distGrid =(int)Math.Sqrt(xd * xd + yd * yd);

            //System.Diagnostics.Debug.WriteLine("snapCursorPos " + ptR + ptSnapToGrid);

            int snaptoObjectMin = (int)(5 * displayScale);


            int threshold = (snaptoObjectMin > distGrid ? snaptoObjectMin : distGrid);
                
            


            Point ptSnapToObj;
            if (snapToObject(ptR, out ptSnapToObj, threshold)) {
                return ptSnapToObj;
			}


            return ptSnapToGrid;
		}

		/// <summary>
		/// �O���b�h�ւ̃X�i�b�v
		/// </summary>
		/// <param name="ptR">�X�i�b�v�������J�[�\���ʒu��}�ʍ��W�Ŏw��</param>
		/// <returns>�X�i�b�v������̍��W</returns>
		protected Point snapToGrid(Point ptR)
		{
            int gridValueX = currentGridInfo.Horizontal;
            int gridValueY = currentGridInfo.Vertical;


			int xTrim = gridValueX / 2;
			if ((ptR.X - gridOrigin.X) < 0) {
				xTrim = -xTrim;
			}
			int yTrim = gridValueY / 2;
			if ((ptR.Y - gridOrigin.Y) < 0) {
				yTrim = -yTrim;
			}
			int x = ((ptR.X - gridOrigin.X + xTrim) / gridValueX) * gridValueX + gridOrigin.X;
			int y = ((ptR.Y - gridOrigin.Y + yTrim) / gridValueY) * gridValueY + gridOrigin.Y;
			return new Point(x, y);
		}

		protected bool snapToObject(Point ptR,out Point ptOut, int threshold)
		{
            ulong layerMask = visibleLayer;


            if (placeObj != null) {
                layerMask &= placeObj.SnapLayer();
            } else if(modeMajor == ModeMajor.PlaceLine){
                layerMask &= MbeObjLine.SnapLayer(selectLayer);
            } else if (modeMajor == ModeMajor.PlacePolygon) {
                layerMask &= MbeObjPolygon.SnapLayer(selectLayer);
            }
            //} else {
            //    layerMask = visibleLayer;
            //}


            //if (modeMajor == ModeMajor.SelectorMode || modeMajor == ModeMajor.SetGridOriginMode || modeMajor == ModeMajor.ConChkMode) {
            //    //System.Diagnostics.Debug.WriteLine("snapToObject()"+ ptR);
            //    layerMask = visibleLayer;
            //} else {
            //    layerMask = (ulong)selectLayer;
            //}

            //System.Diagnostics.Debug.WriteLine("snapToObject()" + layerMask);
            //string pointProperty;

            MbeDoc.GetNearbySnapPointOption option = MbeDoc.GetNearbySnapPointOption.None;
            if (modeMajor == ModeMajor.Measure) {
                option |= MbeDoc.GetNearbySnapPointOption.Measure;
            }


            return document.GetNearbySnapPoint(ptR, layerMask, threshold, out ptOut, out componentPinProperty, option);
		
			//return document.GetNearbySnapPoint(ptR, layerMask, (int)(5 * displayScale), out ptOut);
		}


		/// <summary>
		/// ���[�N�G���A�̃T�C�Y��Ԃ�
		/// </summary>
		/// <returns></returns>
		public Size SizeWorkArea
		{
			get
			{
				//return sizeWorkArea;
				return document.docInfo.SizeWorkArea;
			}
		}

		/// <summary>
		/// ���[�N�G���A�̃T�C�Y�̐ݒ�
		/// </summary>
		/// <param name="width">��(0.001mm�P��)</param>
		/// <param name="height">����(0.001mm�P��)</param>
		/// <remarks>
		/// ���z�E�B���h�E�͏㉺���E�Ƀ}�[�W��������
		/// </remarks>
		public void SetWorkAreaSize(int width, int height)
		{
            //if (width > MbeDoc.WORK_AREA_MAX_WIDTH) {
            //    width = MbeDoc.WORK_AREA_MAX_WIDTH;
            //} else if (width < MbeDoc.WORK_AREA_MIN_WIDTH) {
            //    width = MbeDoc.WORK_AREA_MIN_WIDTH;
            //}

            //if (height > MbeDoc.WORK_AREA_MAX_HEIGHT) {
            //    height = MbeDoc.WORK_AREA_MAX_HEIGHT;
            //} else if (height < MbeDoc.WORK_AREA_MIN_HEIGHT) {
            //    height = MbeDoc.WORK_AREA_MIN_HEIGHT;
            //}
			//sizeWorkArea = new Size(width, height);
			document.docInfo.SizeWorkArea = new Size(width, height);
			SetVirtualWindowSize();
			ScrollToInitPos(displayScale);
			mouseWheelValue = 0;
		}


		/// <summary>
		/// ���[�N�G���A�̃T�C�Y�����Ƃɉ��z�E�B���h�E�̃T�C�Y��ݒ肷��
		/// </summary>
		protected void SetVirtualWindowSize()
		{
			//�N���C�A���g�G���A�̎����W�ł̃T�C�Y�����߂�
			int width = (int)(sizeClient.Width * displayScale);
			int height = (int)(sizeClient.Height * displayScale);

			if (width < (SizeWorkArea.Width + WORK_AREA_MARGIN_BASE * 2)) {
				width = SizeWorkArea.Width + WORK_AREA_MARGIN_BASE * 2;
			}

			if (height < (SizeWorkArea.Height + WORK_AREA_MARGIN_BASE * 2)) {
				height = SizeWorkArea.Height + WORK_AREA_MARGIN_BASE * 2;
			}
			leftMargin = (width - SizeWorkArea.Width) / 2;
			sizeViewableWorkArea.Width = leftMargin + SizeWorkArea.Width;
			sizeViewableWorkArea.Height = (height - SizeWorkArea.Height) / 2 + SizeWorkArea.Height;

			width =(int)(width/ displayScale);
			height=(int)(height/ displayScale);

			AutoScrollMinSize = new Size(width, height);
			//System.Diagnostics.Debug.WriteLine("SetVirtualWindowSize() width:" + width + ",height:" + height + ",dispalyScale:" + dispalyScale);
			//Console.WriteLine("SetVirtualWindowSize() width:" + width + ",height:" + height + ",dispalyScale:" + dispalyScale);
			
//#if MONO
//			Rectangle rc = Bounds;
//			rc.Width = rc.Width - 1;
//			Bounds = rc;
//			rc.Width = rc.Width + 1;
//			Bounds = rc;
//#endif
			RedrawAll();
		}

		/// <summary>
		/// �w��ʒu�ւ̃X�N���[��
		/// </summary>
		/// <param name="pt">�����W</param>
		/// <param name="ptC">�N���C�A���g���W</param>
		/// <param name="scale">�k����</param>
		/// <remarks>
		/// �����W pt ���A�N���C�A���g���W ptC �̈ʒu�ɗ���悤�ɃX�N���[������
		/// </remarks>
		protected void ScrollTo(Point point, Point ptC, double scale)
		{
			Point pt = RealToVw(point, scale);	//�����W�����z�E�B���h�E���W�ɕϊ�
			AutoScrollPosition = pt - (Size)ptC;
		}

        public delegate void FindFormMovesCursor(object sender, MouseEventArgs e);
        public event FindFormMovesCursor findFormMovesCursor;


        public void MoveCursor(Point ptTo)
        {
            cursorPos = ptTo;
            int scrollInitX = (int)(sizeClient.Width * 0.5);
            int scrollInitY = (int)(sizeClient.Height * (1 - 0.5));
            Point ptC = new Point(scrollInitX, scrollInitY);
            ScrollTo(ptTo, ptC, displayScale);
            Refresh();
            if (findFormMovesCursor != null) {
                findFormMovesCursor(this, null);
            }
        }



		/// <summary>
		/// �����ʒu�ւ̃X�N���[��
		/// </summary>
		/// <param name="scale">�k����</param>
		protected void ScrollToInitPos(double scale)
		{
			int scrollInitX = (int)(sizeClient.Width * INITIAL_SCROLL_POS);
			int scrollInitY = (int)(sizeClient.Height * (1-INITIAL_SCROLL_POS));
			Point ptC = new Point(scrollInitX, scrollInitY);
			Point pt  = new Point(0,0);

			ScrollTo(pt, ptC, scale);
		}



		/// <summary>
		/// �N���C�A���g�G���A�T�C�Y�ύX���̃n���h��
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClientSizeChanged(EventArgs e)
		{
			base.OnClientSizeChanged(e);

			sizeClient = new Size(this.ClientSize.Width, this.ClientSize.Height);

            System.Diagnostics.Debug.WriteLine("OnClientSizeChanged()");

            if (this.ClientSize.Height <= 0) {
                System.Diagnostics.Debug.WriteLine("Client height <= 0");
            } else {

                bgContext.MaximumBuffer = new Size(sizeClient.Width + 1, sizeClient.Height + 1);

                if (bgraphics != null) {
                    bgraphics.Dispose();
                    bgraphics = null;
                }
                bgraphics = bgContext.Allocate(this.CreateGraphics(),
                    new Rectangle(0, 0, sizeClient.Width + 1, sizeClient.Height + 1));
                DrawToBuffer(bgraphics.Graphics);
                this.Refresh();
            }
		}

		/// <summary>
		/// �ĕ`��̋���
		/// </summary>
		public void RedrawAll()
		{
			updateBuffForce = true;
			if (enablePaint) {
				Invalidate();
			}
			//System.Diagnostics.Debug.WriteLine("MbeView.RedrawAll()");
		}
			

		/// <summary>
		/// �o�b�t�@�ւ̕`��
		/// </summary>
		/// <param name="g"></param>
		/// <remarks>
		/// �ꎞ�I�u�W�F�N�g�ȊO�̕`��͂����Ƀo�b�t�@�ɑ΂��čs��
		/// </remarks>
		private void DrawToBuffer(Graphics g)
		{
			//g.Clear(Color.Black);
			updateBuffForce = false;

			lastDrawBuffScrollPos = this.AutoScrollPosition;

			GraphicsState gState = g.Save();

			Point pt;
			//�}�ʌ��_(�ʏ�͊�̍���)��`����W�̌��_�Ƃ���
			pt = new Point(0, 0);
			pt = RealToVw(pt, displayScale);
			pt.Offset(lastDrawBuffScrollPos);
			g.TranslateTransform(pt.X, pt.Y);

			////�w�i�F�ŃN���A
			//g.Clear(MbeColors.ColorBackground);

			g.Clear(MbeColors.ColorBgNotWorkArea);

			{
				//Point ptWorkAreaLT = new Point(0,3000000);
				//Point ptWorkAreaRB = new Point(3000000,0);
				Point ptWorkAreaLT = new Point(0, document.docInfo.SizeWorkArea.Height);
				Point ptWorkAreaRB = new Point(document.docInfo.SizeWorkArea.Width, 0);
				ptWorkAreaLT = MbeObj.ToDrawDim(ptWorkAreaLT, displayScale);
				ptWorkAreaRB = MbeObj.ToDrawDim(ptWorkAreaRB,displayScale);
				Rectangle rc = new Rectangle(ptWorkAreaLT,new Size(ptWorkAreaRB.X-ptWorkAreaLT.X,ptWorkAreaRB.Y-ptWorkAreaLT.Y));
				Brush brush = new SolidBrush(MbeColors.ColorBackground);
				g.FillRectangle(brush, rc);
				brush.Dispose();
			}




			
			DrawMain(g, displayScale);

            //System.Diagnostics.Debug.WriteLine("display scale = "+dispalyScale);

			DrawGrid(g, displayScale);

			if (displayDrcMark) {
				DrawGapChkMark(g, displayScale);
			}

			g.Restore(gState);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			switch (e.KeyData) {
				case Keys.ShiftKey:
					if (modeMajor == ModeMajor.PlaceLine) {
						placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Straight;
						Refresh();
					}
					break;
				case Keys.ControlKey:
					if (modeMajor == ModeMajor.PlaceLine) {
						placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Straight;
						Refresh();
					}
					break;
			}
			base.OnKeyUp(e);
		}

		/// <summary>
		/// �L�[�����n���h��
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("OnKeyDown");
			switch (e.KeyData) {
				case Keys.Escape:
                case (Keys.Shift | Keys.Escape):
                    if (modeMinor == 0) {
    					SetMode(ModeMajor.SelectorMode);
                    }else{
                        if (modeMajor == ModeMajor.Measure) {
                            if ((e.KeyData & Keys.Shift) != 0) {
                                modeMinor = 0;
                                listMeasurePoint.Clear();
                            } else  if (modeMinor > 0) {
                                listMeasurePoint.RemoveAt(modeMinor-1);
                                modeMinor--;
                            }

                            Refresh();
                        } else {
                            CancelCurrentPlacementOnEscape((e.KeyData & Keys.Shift)!=0);
                        }
                        //if(modeMajor == ModeMajor.PlaceLine){
                        //    OnKeyEscapePlaceLine();
                        //} else if (modeMajor == ModeMajor.PlacePolygon) {
                        //    OnKeyEscapePlacePolygon();
                        //}
					}
					break;


                case Keys.S:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.SelectorMode);
                    }
                    break;
                case Keys.K:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.ConChkMode);
                    }
                    break;
                case Keys.M:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.Measure);
                    }
                    break;
                case Keys.H:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.PlaceHole);
                    }
                    break;
                case Keys.P:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.PlacePTH);
                    }
                    break;
                case Keys.D:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.PlacePad);
                    }
                    break;
                case Keys.L:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.PlaceLine);
                    }
                    break;
                case Keys.A:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.PlaceArc);
                    }
                    break;
                case Keys.T:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.PlaceText);
                    }
                    break;
                case Keys.G:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.PlacePolygon);
                    }
                    break;
                case Keys.N:
                    if (enableOneKeyToolSelection) {
                        SetMode(ModeMajor.PlaceComponent);
                    }
                    break;



				case Keys.PageDown:
					System.Diagnostics.Debug.WriteLine("PageDown");
					ZoomInOut(false);
					break;
				case Keys.PageUp:
					System.Diagnostics.Debug.WriteLine("PageUp");
					ZoomInOut(true);
					break;
				case (Keys.ShiftKey|Keys.Shift):
					if (modeMajor == ModeMajor.PlaceLine) {
						if (placeObjLine.LineStyle != MbeObjLine.MbeLineStyle.Bending1) {
							placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Bending1;
							Refresh();
						}
					}
					break;
				case (Keys.ControlKey | Keys.Control):
					if (modeMajor == ModeMajor.PlaceLine) {
						if (placeObjLine.LineStyle != MbeObjLine.MbeLineStyle.Bending2) {
							placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Bending2;
							Refresh();
						}
					}
					break;
			}
			base.OnKeyDown(e);
		}


        public void Undo()
        {
            if (modeMinor > 0) {
                if (CancelCurrentPlacementOnEscape(false)) {
                    return;
                }
            }
            Document.Undo();
            RedrawAll();
        }

        /// <summary>
        /// modeMinor ��1�ȏ�̂Ƃ��ɔz�u���̍�Ƃ�ESCAPE�L�[�Ŗ߂�
        /// </summary>
        private bool CancelCurrentPlacementOnEscape(bool shift)
        {
            if (modeMajor == ModeMajor.PlaceLine) {
                OnKeyEscapePlaceLine();
                return true;
            } else if (modeMajor == ModeMajor.PlacePolygon) {
                OnKeyEscapePlacePolygon(shift);
                return true;
            }
            return false;
        }



		/// <summary>
		/// PlaceLine ���[�h�ɂ����� modeMinor��1�ȏ�̂Ƃ��ɌĂ΂��
		/// </summary>
		protected void OnKeyEscapePlaceLine()
		{
			modeMinor = 0;
			placeObj = null;
			Invalidate();
		}

		/// <summary>
		/// PlacePolygon ���[�h�ɂ����� modeMinor��1�ȏ�̂Ƃ��ɌĂ΂��
		/// </summary>
		protected void OnKeyEscapePlacePolygon(bool shift)
		{
            if(modeMinor <= 1 || shift){
                modeMinor = 0;
                placeObj = null;
            }else{
//			if (modeMinor > 1) {
				modeMinor--;
				placeObjPolygon.DeleteLastPoint();
				placeObjPolygon.SetPos(CursorPos, modeMinor + 1);
			}
			Invalidate();
		}

		/// <summary>
		/// �}�E�X�{�^���������ꂽ�Ƃ��̉���
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			System.Diagnostics.Debug.WriteLine("OnMouseUp");
			SetCursorPosition(e.Location);
            movedTempData = false;

			
			//   ���{�^��
			if (e.Button == MouseButtons.Left) {
				bool lbDragEnd = lbDragInfo.MouseUp(cursorPos, e.Location);
				if (modeMajor == ModeMajor.SelectorMode) {
					if (lbDragEnd) {
						bool appendMode = ((int)(Control.ModifierKeys & Keys.Control) !=0);
						//bool appendMode = ((Control.ModifierKeys && Keys.Control) !=0);
						bool clicked = lbDragInfo.IsClicked();	//�N���b�N����̃}�E�X�ړ���
						if (!movingTempData) {
							if(clicked){
								document.SelectByPoint(cursorPos, (int)(5 * displayScale), visibleLayer, appendMode);
							}else{
								document.Select(lbDragInfo.PtDragFrom, lbDragInfo.PtDragTo, visibleLayer, appendMode);
							}
						}
					}
					movingTempData = false;
					activePointOffset = new Size(0, 0);
					//updateBuffForce = true;
					RedrawAll();
                } else if (!lbDragEnd) {        // 2008/4/13 0.29p
                    return;
                } else if (modeMajor == ModeMajor.ConChkMode) {
                    document.ClearConnectCheckFlag();
                    MbeConChk conChk = new MbeConChk();
                    if (conChk.ScanData(cursorPos, visibleLayer, Document.MainList)) {

                    }
                    RedrawAll();
                } else if (modeMajor == ModeMajor.SetGridOriginMode) {
                    SetGridOrigin(cursorPos);
                    return;
                } else if (modeMajor == ModeMajor.PlaceHole) {
                    if (placeObj == null) return;
                    if (placeObj.Id() != MbeObjID.MbeHole) return;
                    MbeObj obj = placeObj.Duplicate();
                    //obj.Layer = SelectLayer;
                    document.AddToMain(obj);
                    RedrawAll();
                    return;
                } else if (modeMajor == ModeMajor.PlacePTH) {
                    if (placeObj == null) return;
                    if (placeObj.Id() != MbeObjID.MbePTH) return;
                    MbeObj obj = placeObj.Duplicate();
                    //obj.Layer = SelectLayer;
                    document.AddToMain(obj);
                    RedrawAll();
                    return;
                } else if (modeMajor == ModeMajor.PlacePad) {
                    if (placeObj == null) return;
                    if (placeObj.Id() != MbeObjID.MbePinSMD) return;
                    MbeObj obj = placeObj.Duplicate();
                    //obj.Layer = SelectLayer;
                    document.AddToMain(obj);
                    RedrawAll();
                    return;
                } else if (modeMajor == ModeMajor.PlaceLine) {

                    if (modeMinor == 0) {
                        placeObj = placeObjLine;
                        placeObj.Layer = SelectLayer;
                        placeObj.SetPos(CursorPos, 0);
                        placeObj.SetPos(CursorPos, 1);
                        modeMinor++;
                    } else {
                        placeObj.SetPos(CursorPos, 1);
                        if (placeObj.IsValid()) {
                            if ((int)(Control.ModifierKeys & Keys.Shift) != 0) {
                                placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Bending1;
                            } else if ((int)(Control.ModifierKeys & Keys.Control) != 0) {
                                placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Bending2;
                            } else {
                                placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Straight;
                            }

                            //bool bendMode = ((int)(Control.ModifierKeys & Keys.Shift) != 0);
                            //placeObjLine.LineStyle = (bendMode ? MbeObjLine.MbeLineStyle.Bending1 : MbeObjLine.MbeLineStyle.Straight);
                            MbeObj obj = placeObj.Duplicate();
                            document.AddToMain(obj);
                            modeMinor = 0;
                            placeObj = null;
                            RedrawAll();
                        }
                        return;
                    }
                } else if (modeMajor == ModeMajor.PlacePolygon) {
                    if (dblClkPlacePolygon) {
                        dblClkPlacePolygon = false;
                        return;
                    }
                    if (modeMinor == 0) {
                        placeObjPolygon = new MbeObjPolygon();
                        placeObjPolygon.FillingPriority = propertyObjPolygon.FillingPriority;
                        placeObjPolygon.RemoveFloating = propertyObjPolygon.RemoveFloating;
                        placeObjPolygon.PatternGap = propertyObjPolygon.PatternGap;
                        placeObjPolygon.TraceWidth = propertyObjPolygon.TraceWidth;
                        placeObjPolygon.RestrictMask = propertyObjPolygon.RestrictMask;


                        placeObjPolygon.AddLastPoint(CursorPos);
                        placeObjPolygon.AddLastPoint(CursorPos);
                        placeObj = placeObjPolygon;
                        placeObj.Layer = SelectLayer;
                        modeMinor++;
                    } else {
                        placeObjPolygon.SetPos(CursorPos, modeMinor + 1);
                        placeObjPolygon.AddLastPoint(CursorPos);
                        modeMinor++;
                        //placeObj.SetPos(CursorPos, 1);
                        //if (placeObj.IsValid()) {
                        //    bool bendMode = ((int)(Control.ModifierKeys & Keys.Shift) != 0);
                        //    placeObjLine.LineStyle = (bendMode ? MbeObjLine.MbeLineStyle.Bending1 : MbeObjLine.MbeLineStyle.Straight);
                        //    MbeObj obj = placeObj.Duplicate();
                        //    document.AddToMain(obj);
                        //    modeMinor = 0;
                        //    placeObj = null;
                        //    RedrawAll();
                        //}
                        return;
                    }

                } else if (modeMajor == ModeMajor.PlaceText) {
                    if (document.TempList.Count == 0) {
                        if (!releaseTempOnMouseDown) {
                            placeObjText.Layer = SelectLayer;
                            if (EditPropertyText(placeObjText)) {
                                if (placeObjText.IsValid()) {
                                    MbeObj obj = placeObjText.Duplicate();
                                    obj.SetPos(CursorPos, 0);
                                    obj.SetAllSelectFlag();// SetSelectFlag(true, 0);
                                    document.AddToTemp(obj);
                                    RedrawAll();
                                }
                            }
                        }
                    } else {
                        movingTempData = false;
                        activePointOffset = new Size(0, 0);
                        Invalidate();
                    }
                    return;
                } else if (modeMajor == ModeMajor.PlaceComponent) {
                    if (document.TempList.Count == 0) {
                        if (!releaseTempOnMouseDown) {
                            ComponentSelectForm dlg = new ComponentSelectForm();
                            dlg.Libs = mbeLibs;
                            dlg.DisplayScale = displayScale;
                            if (dlg.ShowDialog() == DialogResult.OK) {
                                MbeObjComponent obj = dlg.SelectedComponent;
                                if (obj != null) {
                                    obj.Layer = SelectLayer;
                                    placeComponentLayer = SelectLayer;
                                    obj.Move(false, CursorPos, CursorPos, false);
                                    obj.SetAllSelectFlag();
                                    document.AddToTemp(obj);
                                    RedrawAll();
                                }
                            }
                        }
                    } else {
                        movingTempData = false;
                        activePointOffset = new Size(0, 0);
                        Invalidate();
                    }
                    return;
                } else if (modeMajor == ModeMajor.PlaceArc) {
                    if (placeObj == null) return;
                    if (placeObj.Id() != MbeObjID.MbeArc) return;
                    MbeObj obj = placeObj.Duplicate();
                    document.AddToMain(obj);
                    RedrawAll();
                    return;


                    //if (document.TempList.Count == 0) {
                    //    if (!releaseTempOnMouseDown) {
                    //        placeObjArc.Layer = SelectLayer;
                    //        MbeObj obj = placeObjArc.Duplicate();
                    //        obj.SetPos(CursorPos, 0);
                    //        obj.SetAllSelectFlag();
                    //        document.AddToTemp(obj);
                    //        RedrawAll();
                    //    }
                    //} else {
                    //    movingTempData = false;
                    //    Invalidate();
                    //}
                } else if (modeMajor == ModeMajor.Measure) {
                    if (modeMinor == 0) {
                        listMeasurePoint.Clear();
                    }
                    modeMinor++;
                    listMeasurePoint.Add(CursorPos);
                    //    modeMinor = 1;
                    //    ptMeasureOrigin = CursorPos;
                    //} else {
                    //    modeMinor = 0;
                    //}
                }

			} else if (e.Button == MouseButtons.Right) {	//�E�{�^��
				bool rbDragEnd = rbDragInfo.MouseUp(cursorPos, e.Location);
                if (rbDragInfo.IsMoved) {

                } else {
                    ToolProperty();
                }
			}
		}

        public void ToolProperty()
        {
            if (modeMajor == ModeMajor.PlaceHole) {
                if (placeObjHole != null) {
                    if (EditPropertyHole(placeObjHole)) {
                        Invalidate();
                    }
                }
            } else if (modeMajor == ModeMajor.PlacePolygon) {
                if (propertyObjPolygon != null) {
                    if (EditPropertyPolygon(propertyObjPolygon)) {
                        if (placeObjPolygon != null) {
                            placeObjPolygon.FillingPriority = propertyObjPolygon.FillingPriority;
                            placeObjPolygon.RemoveFloating = propertyObjPolygon.RemoveFloating;
                            placeObjPolygon.PatternGap = propertyObjPolygon.PatternGap;
                            placeObjPolygon.TraceWidth = propertyObjPolygon.TraceWidth;
                            placeObjPolygon.RestrictMask = propertyObjPolygon.RestrictMask;

                        }
                    }
                }
            } else if (modeMajor == ModeMajor.PlacePTH) {
                if (placeObjPTH != null) {
                    if (EditPropertyPTH(placeObjPTH, false)) {
                        Invalidate();
                    }
                }
            } else if (modeMajor == ModeMajor.PlacePad) {
                if (placeObjPinSMD != null) {
                    if (EditPropertySMDPad(placeObjPinSMD, false)) {
                        Invalidate();
                    }
                }
            } else if (modeMajor == ModeMajor.PlaceArc) {
                if (placeObjArc != null) {
                    if (EditPropertyArc(placeObjArc)) {
                        Invalidate();
                    }
                }
            } else if (modeMajor == ModeMajor.PlaceLine) {
                if (placeObjLine != null) {
                    if (EditPropertyLine(placeObjLine)) {
                        Invalidate();
                    }
                }
            }
        }

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
            System.Diagnostics.Debug.WriteLine("OnMouseDoubleClick");

			if (e.Button == MouseButtons.Left) {
				if (modeMajor == ModeMajor.PlacePolygon) {
					if (modeMinor > 2) {
						//placeObjPolygon.CleanupOverlappingPoint();
						//System.Diagnostics.Debug.WriteLine("Place polygon PosCount:" + placeObjPolygon.PosCount);
						if (placeObjPolygon.IsValid()) {
							System.Diagnostics.Debug.WriteLine("Add polygon");
							document.AddToMain(placeObjPolygon);
						}
					}
					dblClkPlacePolygon = true;
					modeMinor = 0;
					placeObj = null;
					placeObjPolygon = null;
					RedrawAll();
				} else {
                    if (document.CanEditProperty() && !movedTempData) {
                        EditProperty();
                    }
				}
			}

		}

		protected void EnableContextMenu(bool enable)
		{
//#if MONO
//            if (enable) {
//                this.ContextMenu = contextMenuView;
//            }else{
//                this.ContextMenu = null;
//            }

//#else
			if(enable){
				this.ContextMenuStrip = contextMenuOnView;
			}else{
				this.ContextMenuStrip = null;
			}
//#endif
		}




		/// <summary>
		/// �}�E�X�{�^�������n���h��
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			System.Diagnostics.Debug.WriteLine("OnMouseDown");
			SetCursorPosition(e.Location);
			SetCursorPointInfo();
			releaseTempOnMouseDown = false;
            movedTempData = false;

			if(e.Button == MouseButtons.Right) {
				if (modeMajor == MbeView.ModeMajor.SelectorMode) {
					EnableContextMenu(true);
					//this.ContextMenuStrip = this.contextMenuView;
				} else {
					EnableContextMenu(false);
					//this.ContextMenuStrip = null;
				}
				rbDragInfo.MouseDown(cursorPos, e.Location);
			}

			//���[�h���ʂ̏���			
			if (e.Button == MouseButtons.Left) {
				lbDragInfo.MouseDown(cursorPos, e.Location);
				//�ꎞ�f�[�^�����݂�����
				if (document.TempList.Count > 0) {
					if (closeToActivePoint) {
						//�ړ��̂��߂̏����ݒ���s��
						movingTempData = true;
						activePointOffset = new Size(currentActivePoint.X - cursorPos.X,
							currentActivePoint.Y - cursorPos.Y);
						//System.Diagnostics.Debug.WriteLine("activePointOffset" + activePointOffset.Width + "," + activePointOffset.Height);
					} else {
						if ((int)(Control.ModifierKeys & Keys.Control) == 0) {
							//�ꎞ�f�[�^�̉��
							document.ReleaseTemp();
							releaseTempOnMouseDown = true;
							RedrawAll();
							//updateBuffForce = true;
							//Invalidate();
						}
					}
					return;
				}
			}

		}


		/// <summary>
        /// cursorPos��ݒ肷��B(cursorPosVW�͎g��Ȃ��Ȃ���)
		/// </summary>
		/// <param name="ptClient">�N���C�A���g���W</param>
		protected void SetCursorPosition(Point ptClient)
		{
			//Point pt = e.Location;	// new Point(e.X, e.Y);
			Point pt = ClientToVw(ptClient);
			//cursorPosVW = pt;		// new Point(pt.X, pt.Y);
			pt = VwToReal(pt, displayScale);
			pt = pt + activePointOffset;

            //System.Diagnostics.Debug.WriteLine("activePointOffset" + activePointOffset.Width + "," + activePointOffset.Height);


            //Point ptSnap = snapCursorPos(pt);
            Point ptDummy;
            //�X�i�b�v�ƃJ�[�\���ʒu�̊m��
            if (movingTempData) {
                cursorPos = snapCursorPos(pt);
            } else if (modeMajor == ModeMajor.Measure) {
                if ((int)(Control.ModifierKeys & Keys.Shift) != 0) {
                    cursorPos = snapCursorPos(pt);
                } else {
                    snapToObject(pt, out ptDummy, (int)(5 * displayScale)); //call it for set pin-property.
                    cursorPos = pt;
                }
            } else if (modeMajor == ModeMajor.ConChkMode) {
                snapToObject(pt, out ptDummy, (int)(5 * displayScale)); //call it for set pin-property.
                cursorPos = pt;
            } else if (modeMajor != ModeMajor.SelectorMode && !document.CanCopy()) {
                cursorPos = snapCursorPos(pt);
            } else {
                snapToObject(pt, out ptDummy, (int)(5 * displayScale));  //call it for set pin-property.
                cursorPos = pt;
            }

      
            ////�X�i�b�v�ƃJ�[�\���ʒu�̊m��
            //if (movingTempData){
            //    pt = snapCursorPos(pt);
            //} else if (modeMajor == ModeMajor.ConChkMode) {
            //    //�X�i�b�v���Ȃ�
            //} else if (modeMajor != ModeMajor.SelectorMode && !document.CanCopy()) {
            //    pt = snapCursorPos(pt);
            //}
            //cursorPos = pt;

		}

		/// <summary>
		/// �J�[�\���ʒu�̃|�C���g�̏���ݒ肷��B
		/// </summary>
		/// <remarks>
		/// �I���I�u�W�F�N�g�̋߂���
		/// �����\�Ȑ��̋߂���
		/// </remarks>
		protected void SetCursorPointInfo()
		{
			closeToActivePoint = false;
			divideablePointValid = false;
            string pinProperty;
			if (modeMajor != ModeMajor.SelectorMode && !document.CanCopy()) return;
			//�J�[�\�����A�N�e�B�u�|�C���g�̋߂����ǂ����̔���
			if (movingTempData) {
				closeToActivePoint = true;	//�ړ����͋߂��ɂ���ƌ���
			} else if (document.TempList.Count > 0) {
				closeToActivePoint = document.GetNearbyActivePoint(cursorPos, visibleLayer,
                    (int)(5 * displayScale), out currentActivePoint, out pinProperty);
                    //(int)(5 * displayScale), out currentActivePoint, out componentPinProperty);
                if (componentPinProperty.Length == 0 && pinProperty.Length > 0) {
                    componentPinProperty = pinProperty;
                }
			}
			if (!closeToActivePoint) {
				divideablePointValid = document.CanDivide(cursorPos, visibleLayer,
					(int)(5 * displayScale), out dividableLineObj, out dividableLineIndex, out divideablePoint);
			}
		}


		/// <summary>
		/// �����I�u�W�F�N�g�ւ̃m�[�h�̒ǉ�
		/// </summary>
		public void AddNode()
		{
			if (!divideablePointValid) return;
			document.DivideLine(dividableLineObj, dividableLineIndex,divideablePoint);
			RedrawAll();
		}


		/// <summary>
		/// �}�E�X�ړ��n���h��
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("OnMouseMove");
			SetCursorPosition(e.Location);
			SetCursorPointInfo();
            

			if (((int)(e.Button)& (int)(MouseButtons.Right))!=0) {
				if (rbDragInfo.IsMoved) {
					ScrollTo(rbDragInfo.PtDragFrom, e.Location, displayScale);
					SetCursorPosition(e.Location);
					SetCursorPointInfo();
				}
				System.Diagnostics.Debug.WriteLine("OnMouseMove(MouseEventArgs e)-Right");
				rbDragInfo.MouseMove(cursorPos, e.Location);
				if (rbDragInfo.IsMoved) {
					EnableContextMenu(false);
					////this.ContextMenuStrip = null;
				}
			}

			if (((int)(e.Button)&(int)(MouseButtons.Left))!=0) {
				lbDragInfo.MouseMove(cursorPos, e.Location);
			}

			if (movingTempData) {
				if (!currentActivePoint.Equals(cursorPos)) {
					Point offset = new Point(cursorPos.X - currentActivePoint.X,
											cursorPos.Y - currentActivePoint.Y);
					document.MoveTemp(true, offset, cursorPos);
					currentActivePoint = cursorPos;
                    movedTempData = true;
				}
			}

			if(placeObj != null){
				if (modeMajor == ModeMajor.PlacePolygon) {
					placeObj.SetPos(this.CursorPos, modeMinor + 1);
				} else {
					placeObj.SetPos(this.CursorPos, modeMinor);
				}
				if (modeMajor == ModeMajor.PlaceLine) {
					if ((int)(Control.ModifierKeys & Keys.Shift) != 0) {
						placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Bending1;
					} else if ((int)(Control.ModifierKeys & Keys.Control) != 0) {
						placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Bending2;
					} else {
						placeObjLine.LineStyle = MbeObjLine.MbeLineStyle.Straight;
					}

					//bool bendMode = ((int)(Control.ModifierKeys & Keys.Shift) != 0);
					//placeObjLine.LineStyle = (bendMode ? MbeObjLine.MbeLineStyle.Bending1 : MbeObjLine.MbeLineStyle.Straight);
				}
			}

            if (modeMajor== ModeMajor.Measure && modeMinor > 0) {
                sizeMeasure.Width = CursorPos.X - listMeasurePoint[modeMinor-1].X;
                sizeMeasure.Height = CursorPos.Y - listMeasurePoint[modeMinor-1].Y;

                measureLength = 0;
                for (int i = 1; i < modeMinor; i++) {
                    double w = listMeasurePoint[i].X - listMeasurePoint[i - 1].X;
                    double h = listMeasurePoint[i].Y - listMeasurePoint[i - 1].Y;
                    measureLength += Math.Sqrt(w * w + h * h);
                }

                    
            }
				
			base.OnMouseMove(e);
			Refresh();
		}

		/// <summary>
		/// �}�E�X�̃z�C�[���̏���
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>
		/// Zoom IN/OUT���s���B
		/// �f�t�H���g�̃X�N���[��������֎~���邽�߂ɁAbase�N���X�̓����\�b�h���Ă�ł��Ȃ��B
		/// </remarks>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
            //0.48.04�Ń}�E�X�J�[�\�����r���[�̃N���C�A���g�G���A�O�ɂ���Ƃ��̓X�N���[�����󂯕t���Ȃ��悤�ɕύX
   			Point ptMouse = MousePosition;
			Point ptMouseC = PointToClient(ptMouse);
            if (ptMouseC.X > 0 && ptMouseC.X < sizeClient.Width &&
                ptMouseC.Y > 0 && ptMouseC.Y < sizeClient.Height) {

                System.Diagnostics.Debug.WriteLine("OnMouseWheel " + e.Delta);
                mouseWheelValue += e.Delta;
                int stepValue = mouseWheelValue / MOUSE_WHEEL_STEP;
                mouseWheelValue = mouseWheelValue % MOUSE_WHEEL_STEP;
                if (stepValue > 0) {
                    ZoomInOut(true);
                } else if (stepValue < 0) {
                    ZoomInOut(false);
                }
            }
		}

		/// <summary>
		/// �X�N���[���n���h��
		/// </summary>
		/// <param name="se"></param>
		protected override void OnScroll(ScrollEventArgs se)
		{

            //2008/12/20 Version 0.45.03
            //�u�h���b�O���ɃE�B���h�E�̓��e��\������v��OFF�ɂȂ��Ă���Ƃ��ɁA
            //ThumbTrack��AutoScrollPosition���X�V����Ȃ����Ƃ̑΍�
            //System.Diagnostics.Debug.WriteLine("OnScroll():" + se.Type + "," + se.NewValue);
            //System.Diagnostics.Debug.WriteLine("OnPaint():" + AutoScrollPosition);

//#if !MONO   //MONO 2.2�ł��̏���������Ɩʔ�������������B

            //Windows�̌��ʂɂ����āA�T���̃h���b�O���ɃE�B���h�E���X�N���[�����Ȃ��ݒ�ɂȂ��Ă���ꍇ�ł�
            //�r���[���X�N���[�����邽�߂̏��׍H
            //Mono���ł́Ase.ScrollOrientation ����� ScrollOrientation.HorizontalScroll �ɂȂ�̂�NG������
            if (!Program.monoRuntime) {
                if (se.Type == ScrollEventType.ThumbTrack) {
                    Point ptScrlPos = AutoScrollPosition;

                    if (se.ScrollOrientation == ScrollOrientation.HorizontalScroll) {
                        if (ptScrlPos.X != -se.NewValue) {
                            AutoScrollPosition = new Point(se.NewValue, -ptScrlPos.Y);
                        }
                    } else {
                        if (ptScrlPos.Y != -se.NewValue) {
                            AutoScrollPosition = new Point(-ptScrlPos.X, se.NewValue);
                        }
                    }

                }
            }
//#endif
			base.OnScroll(se);

            if (Program.monoRuntime && se.Type == ScrollEventType.ThumbTrack) {
                RedrawAll();
            }


		}

		/// <summary>
		/// �y�C���g�n���h��
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>
		/// �`��o�b�t�@�̃����_�[�̂��ƂŁA�ꎞ�I�u�W�F�N�g��`�悷��B
		/// </remarks>
		protected override void OnPaint(PaintEventArgs e)
		{
			// If there is an image and it has a location, 
			// paint it when the Form is repainted.
			base.OnPaint(e);

			if (!enablePaint) return;

			Graphics g=e.Graphics;
			
			Point ptAutoScrollPos = this.AutoScrollPosition;
            //System.Diagnostics.Debug.WriteLine("OnPaint():" + ptAutoScrollPos);


			if (updateBuffForce || ptAutoScrollPos != lastDrawBuffScrollPos) {
				DrawToBuffer(bgraphics.Graphics);
			}

			bgraphics.Render(g);

			Point pt;
			//�}�ʌ��_(�ʏ�͊�̍���)��`����W�̌��_�Ƃ���
			pt = new Point(0, 0);
			pt = RealToVw(pt, displayScale);
			pt.Offset(lastDrawBuffScrollPos);
			g.TranslateTransform(pt.X, pt.Y);

			pt = this.CursorPos;
            pt = RealToDraw(pt, displayScale);
			//pt.X = (int)(pt.X / displayScale);
			//pt.Y = (int)(-pt.Y / displayScale);

			//RealToVw(ref pt, dispalyScale);

			//int x = lastDrawBuffScrollPos.X;
			//int y = lastDrawBuffScrollPos.Y;
			//g.TranslateTransform(x, y);



			if (placeObj != null) {
				//System.Diagnostics.Debug.WriteLine("drawcursoeObj");
				//placeObj.SetPos(this.CursorPos, modeMinor);
				DrawCursorObj(g, placeObj);
			}
			DrawTemp(g, displayScale);
			DrawCrossCursor(g, displayScale);
			DrawOriginMark(g, displayScale);
			DrawGridOriginMark(g, displayScale);


			//boardFont.DrawString(g, 100, 50, false, "ABCD EFGHIJKopqrstu{}%$", 40, 5, Color.White);
			//GraphicsState gState = g.Save();	//���W�n�ۑ�
			//g.TranslateTransform(100, 50);
			//g.RotateTransform(-90F);
			//boardFont.DrawString(g, 0, 0, true, "ABCDEFGHIJKopqrstu{}%$", 40, 5, Color.White);
			//g.Restore(gState);					//���W�n���A




			switch (modeMajor) {
				case ModeMajor.SelectorMode: {

						if (!movingTempData && lbDragInfo.IsDragging) {
							DrawSelectFrame(g, displayScale, lbDragInfo.PtDragFrom, lbDragInfo.PtDragTo);
						}

						break;
					}
				case ModeMajor.SetGridOriginMode: {
						if (gridOriginCursorIcon == null) {
							gridOriginCursorIcon = new Icon(Properties.Resources.gridOriginCursor, 32, 32);
						}
						g.DrawIcon(gridOriginCursorIcon, pt.X-8,pt.Y-8);
						break;
					}
			}

			if (closeToActivePoint) {
				DrawCurrentActivePointMark(g, displayScale);
			}

			if (divideablePointValid) {
				DrawDividablePointMark(g, displayScale);
			}

            if (modeMajor == ModeMajor.Measure && modeMinor > 0) {
                DrawMeasureInfo(g, displayScale);
            }

			return;

		}

		protected void DrawCursorObj(Graphics g, MbeObj obj)
		{
			if (obj == null) return;
			int layerCount = MbeLayer.valueTable.Length;
			DrawParam dp = new DrawParam();
			dp.g = g;
			dp.mode = MbeDrawMode.Temp;
			dp.scale = this.displayScale;
			dp.visibleLayer = visibleLayer;


			for (int i = 0; i < layerCount; i++) {
				if ((visibleLayer & (ulong)(MbeLayer.valueTable[i])) != 0) {
					//System.Diagnostics.Debug.WriteLine("drawcursoeObj:"+i);
					dp.layer = MbeLayer.valueTable[i];
					obj.Draw(dp);
				}
			}

		}

		/// <summary>
		/// View��3D�G�b�W���������邽�߂̃E�B���h�E�X�^�C���̐ݒ���s��
		/// </summary>
//		protected override CreateParams CreateParams
//		{
//			get
//			{
//				CreateParams cp = base.CreateParams;
//#if !MONO
//				cp.Style &= (~Win32WindowProperty.WS_BORDER);
//				cp.ExStyle |= Win32WindowProperty.WS_EX_CLIENTEDGE;
//#endif
//				return cp;
//			}
//		}

		/// <summary>
		/// �t�H�[����Property�R�}���h�̎��s
		/// </summary>
		/// <remarks>�I������Ă���I�u�W�F�N�g�ɑΉ������_�C�A���O���N������</remarks>
		public void EditProperty()
		{
			if (document.CanEditProperty()) {
                MbeObj obj = document.TempList.First.Value;
                bool modified = false;
                switch (obj.Id()) {
                    case MbeObjID.MbeHole:
                        modified = EditPropertyHole((MbeObjHole)obj);
                        break;
                    case MbeObjID.MbePTH:
                        modified = EditPropertyPTH((MbeObjPTH)obj, true);
                        break;
                    case MbeObjID.MbePinSMD:
                        modified = this.EditPropertySMDPad((MbeObjPinSMD)obj, true);
                        break;
                    case MbeObjID.MbeLine:
                        modified = EditPropertyLineStyle((MbeObjLine)obj);
                        break;
                    case MbeObjID.MbePolygon:
                        modified = EditPropertyPolygon((MbeObjPolygon)obj);
                        break;
                    case MbeObjID.MbeArc:
                        modified = EditPropertyArc((MbeObjArc)obj);
                        break;
                    case MbeObjID.MbeText:
                        modified = EditPropertyText((MbeObjText)obj);
                        break;
                    case MbeObjID.MbeComponent:
                        modified = EditPropertyComponent((MbeObjComponent)obj);
                        break;
                    default:
                        return;
                }
                if (modified) {
                    document.TempModified = true;
                    RedrawAll();
                    //Invalidate();
                }
            } else {
                ToolProperty();
            }
		}


		public void Cut()
		{
            Point dummyPt;
            if (Document.Copy(out dummyPt)) {
				Document.Delete();
				RedrawAll();
			}
		}

		public void Copy(out Point ptLT)
		{
            Document.Copy(out ptLT);
		}

        private bool pasteAtCurrsor;

        public bool PasteAtCurrsor
        {
            get { return pasteAtCurrsor; }
            set { pasteAtCurrsor = value; }
        }


		/// <summary>
		/// �y�[�X�g�B���C���t�H�[������Ă΂��B
		/// </summary>
		public void Paste()
		{
			SetMode(ModeMajor.SelectorMode);

            Point pt;


            if (pasteAtCurrsor) {
                Point ptMouse = MousePosition;
                Point ptMouseC = PointToClient(ptMouse);
                if (ptMouseC.X > 0 && ptMouseC.X < sizeClient.Width &&
                    ptMouseC.Y > 0 && ptMouseC.Y < sizeClient.Height) {
                    pt = ptMouseC;
                } else {
                    pt = new Point(sizeClient.Width / 2, sizeClient.Height / 2);
                }
            } else {
                    pt = new Point(sizeClient.Width / 2, sizeClient.Height / 2);
            }
			pt = ClientToVw(pt);
			pt = VwToReal(pt, displayScale);
            pt = snapCursorPos(pt);

            Paste(pt);
            return;
		}

        public void Paste(Point pt)
        {
            SetMode(ModeMajor.SelectorMode);
             //}
            ulong layers;
            if (document.Paste(pt, out layers) == 0) {
                System.Diagnostics.Debug.WriteLine("No paste data");
            } else {
                visibleLayer |= layers;

                if (LayerSelectChange != null) {
                    LayerInfoEventArgs e = new LayerInfoEventArgs();
                    e.selectableLayer = SelectableLayer;
                    e.selectLayer = SelectLayer;
                    e.visibleLayer = visibleLayer;
                    LayerSelectChange(this, e);
                }
            }
            RedrawAll();
        }




        private Size szLastMoveValue;

        public void MoveByValue()
        {
            MoveSizeForm dlg = new MoveSizeForm();
            dlg.MoveX = szLastMoveValue.Width;
            dlg.MoveY = szLastMoveValue.Height;
            if (dlg.ShowDialog() == DialogResult.OK) {
                if (dlg.MoveX != 0 || dlg.MoveY != 0) {
                    szLastMoveValue = new Size(dlg.MoveX, dlg.MoveY);
                    if (dlg.Duplicate) {
                        Point basePt;
                        Copy(out basePt);
                        Paste(basePt);
                    }
                    document.MoveTemp(szLastMoveValue.Width, szLastMoveValue.Height);
                    Refresh();
                }
            }

        }

        /// <summary>
        /// Flip ���C���[�ʂ̔��]
        /// </summary>
		public void Flip()
		{
			ulong layers;
			document.Flip(out layers);
			visibleLayer |= layers;

			if (LayerSelectChange != null) {
				LayerInfoEventArgs e = new LayerInfoEventArgs();
				e.selectableLayer = SelectableLayer;
				e.selectLayer = SelectLayer;
				e.visibleLayer = visibleLayer;
				LayerSelectChange(this, e);
			}
			RedrawAll();
		}

        public void Find(Form owner)
        {
            if (findForm != null && findForm.Visible) {
                findForm.Close();
                findForm.Dispose();
            }

            if (findForm == null || !findForm.Visible) {
                findForm = new FindForm();
                findForm.Owner = owner;
                findForm.mbeView = this;

                

                //findForm.mainList = document.MainList;
                findForm.Show();
            }
        }



		public void Componenting()
		{
			document.Componenting(gridOrigin, "NUMBER");
			Invalidate();
		}

		public void UnComponenting()
		{
			ulong layers;
			document.Uncomponenting(out layers);
			visibleLayer |= layers;
			if (LayerSelectChange != null) {
				LayerInfoEventArgs e = new LayerInfoEventArgs();
				e.selectableLayer = SelectableLayer;
				e.selectLayer = SelectLayer;
				e.visibleLayer = visibleLayer;
				LayerSelectChange(this, e);
			}
			RedrawAll();
		}


		/// <summary>
		/// �o���N�v���p�e�B Hole 
		/// </summary>
		public void BulkPropHole()
		{
			MbeObjHole objR = (MbeObjHole)placeObjHole.Duplicate();
			if (EditPropertyHole(objR)) {
				if (document.BulkPropHole(objR)) {
					Invalidate();
				}
			}
		}

		/// <summary>
		/// �o���N�v���p�e�B PTH 
		/// </summary>
		public void BulkPropPTH()
		{
			MbeObjPTH objR = (MbeObjPTH)placeObjPTH.Duplicate();
			if (EditPropertyPTH(objR, false)) {
				if (document.BulkPropPTH(objR)) {
					Invalidate();
				}
			}
		}

		/// <summary>
		/// �o���N�v���p�e�B Pad 
		/// </summary>
		public void BulkPropPad()
		{
			MbeObjPinSMD objR = (MbeObjPinSMD)placeObjPinSMD.Duplicate();
			if (EditPropertySMDPad(objR, false)) {
				if (document.BulkPropPad(objR)) {
					Invalidate();
				}
			}
		}

		/// <summary>
		/// �o���N�v���p�e�B Line
		/// </summary>
		public void BulkPropLine()
		{
			MbeObjLine objR = (MbeObjLine)placeObjLine.Duplicate();
			if (EditPropertyLine(objR)) {
				if (document.BulkPropLine(objR)) {
					Invalidate();
				}
			}
		}

		/// <summary>
		/// �o���N�v���p�e�B Text
		/// </summary>
		public void BulkPropText()
		{
			MbeObjText objR = (MbeObjText)placeObjText.Duplicate();
			if (BulkPropertyText(objR)) {
				if (document.BulkPropText(objR)) {
					Invalidate();
				}
			}
		}

        /// <summary>
        /// �o���N�v���p�e�B Polygon 
        /// </summary>
        public void BulkPropPolygon()
        {
            SetPolygonForm dlg = new SetPolygonForm();
            dlg.BulkMode = true;
            
            DialogResult retv = dlg.ShowDialog();
            if (retv == DialogResult.OK) {
                int pattenGap = dlg.PatternGap;
                int traceWidth = dlg.TraceWidth;
                CheckState removeFloating = dlg.RemoveFloating;
                CheckState restrictMask = dlg.RestrictMask;

                if (document.BulkPropPolygon(traceWidth, pattenGap, removeFloating, restrictMask)) {
                    Invalidate();
                }
            }
        }



        public void BulkPropLayer(CanBulkPropResult canBulkPropResult)     //public void BulkPropLayer(MbeObjID id)
		{
			MbeLayer.LayerValue layer;
            //if (id == MbeObjID.MbeLine) {
            //    layer = placeObjLine.Layer;
            //} else {
            //    layer = placeObjText.Layer;
            //}
            layer = MbeLayer.LayerValue.CMP;
			if (BulkLayerMove(ref layer, canBulkPropResult.SelectableLayer)) {
				if (document.BulkPropLayer(layer)) {
					if ((visibleLayer & (ulong)layer) == 0) {
						visibleLayer |= (ulong)layer;
						RedrawAll();
						if (LayerSelectChange != null) {
							LayerInfoEventArgs e = new LayerInfoEventArgs();
							e.selectableLayer = SelectableLayer;
							e.selectLayer = SelectLayer;
							e.visibleLayer = visibleLayer;
							LayerSelectChange(this, e);
						}
					} else {
						Invalidate();
					}
				}
			}
		}


		public void Drc()
		{

			MbeDrcParam drcParam = new MbeDrcParam();
			drcParam.patternGap = Properties.Settings.Default.DrcPatternGap;
			drcParam.checkLimit = Properties.Settings.Default.DrcErrorCheckLimit;
			
			SetDrcForm dlg = new SetDrcForm();
			dlg.PatternGap = drcParam.patternGap;
			dlg.ErrorChkLimit = drcParam.checkLimit;
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				drcParam.patternGap = dlg.PatternGap;
				drcParam.checkLimit = dlg.ErrorChkLimit;
				Properties.Settings.Default.DrcPatternGap = drcParam.patternGap;
				Properties.Settings.Default.DrcErrorCheckLimit = drcParam.checkLimit;

                Properties.Settings.Default.Save();

				int errCount = document.Drc(drcParam);
				DisplayDrcMark = true;
				RedrawAll();

				string msg = String.Format("{0} errors found.", errCount);
				MessageBox.Show(msg,
							"DRC",
							MessageBoxButtons.OK);

			} else {
				RedrawAll();
			}
			
		}

		public void FillPolygon()
		{
			//Cursor.Current = Cursors.WaitCursor;
			document.FillPolygon();
			RedrawAll();
		}


		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MbeView));
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.contextMenuOnView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuAddNode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuBulkProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBulkPropHole = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBulkPropPTH = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBulkPropPad = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBulkPropLine = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBulkPropText = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBulkPropPolygon = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBulkPropLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuOption0 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuOption1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuOption2 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListLineBending = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuOnView.SuspendLayout();
            this.SuspendLayout();
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // contextMenuOnView
            // 
            this.contextMenuOnView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuAddNode,
            this.toolStripSeparator1,
            this.contextMenuCut,
            this.contextMenuCopy,
            this.contextMenuPaste,
            this.contextMenuProperty,
            this.contextMenuBulkProperty,
            this.contextMenuOption0,
            this.contextMenuOption1,
            this.contextMenuOption2});
            this.contextMenuOnView.Name = "contextMenuView";
            this.contextMenuOnView.Size = new System.Drawing.Size(194, 208);
            this.contextMenuOnView.Opened += new System.EventHandler(this.contextMenuView_Opened);
            // 
            // contextMenuAddNode
            // 
            this.contextMenuAddNode.Name = "contextMenuAddNode";
            this.contextMenuAddNode.Size = new System.Drawing.Size(193, 22);
            this.contextMenuAddNode.Text = "AddNode";
            this.contextMenuAddNode.Click += new System.EventHandler(this.OnContextMenuAddNode);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(190, 6);
            // 
            // contextMenuCut
            // 
            this.contextMenuCut.Name = "contextMenuCut";
            this.contextMenuCut.Size = new System.Drawing.Size(193, 22);
            this.contextMenuCut.Text = "Cut";
            this.contextMenuCut.Click += new System.EventHandler(this.OnContextMenuCut);
            // 
            // contextMenuCopy
            // 
            this.contextMenuCopy.Name = "contextMenuCopy";
            this.contextMenuCopy.Size = new System.Drawing.Size(193, 22);
            this.contextMenuCopy.Text = "Copy";
            this.contextMenuCopy.Click += new System.EventHandler(this.OnContextMenuCopy);
            // 
            // contextMenuPaste
            // 
            this.contextMenuPaste.Name = "contextMenuPaste";
            this.contextMenuPaste.Size = new System.Drawing.Size(193, 22);
            this.contextMenuPaste.Text = "Paste";
            this.contextMenuPaste.Click += new System.EventHandler(this.OnContextMenuPaste);
            // 
            // contextMenuProperty
            // 
            this.contextMenuProperty.Name = "contextMenuProperty";
            this.contextMenuProperty.Size = new System.Drawing.Size(193, 22);
            this.contextMenuProperty.Text = "Property";
            this.contextMenuProperty.Click += new System.EventHandler(this.OnContextMenuProperty);
            // 
            // contextMenuBulkProperty
            // 
            this.contextMenuBulkProperty.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuBulkPropHole,
            this.MenuBulkPropPTH,
            this.MenuBulkPropPad,
            this.MenuBulkPropLine,
            this.MenuBulkPropText,
            this.MenuBulkPropPolygon,
            this.MenuBulkPropLayer});
            this.contextMenuBulkProperty.Name = "contextMenuBulkProperty";
            this.contextMenuBulkProperty.Size = new System.Drawing.Size(193, 22);
            this.contextMenuBulkProperty.Text = "Bulk Property";
            this.contextMenuBulkProperty.DropDownOpened += new System.EventHandler(this.OnContextMenuBulkPropOpen);
            // 
            // MenuBulkPropHole
            // 
            this.MenuBulkPropHole.Name = "MenuBulkPropHole";
            this.MenuBulkPropHole.Size = new System.Drawing.Size(133, 22);
            this.MenuBulkPropHole.Text = "Hole";
            this.MenuBulkPropHole.Click += new System.EventHandler(this.OnBulkPropHole);
            // 
            // MenuBulkPropPTH
            // 
            this.MenuBulkPropPTH.Name = "MenuBulkPropPTH";
            this.MenuBulkPropPTH.Size = new System.Drawing.Size(133, 22);
            this.MenuBulkPropPTH.Text = "PTH";
            this.MenuBulkPropPTH.Click += new System.EventHandler(this.OnBulkPropPTH);
            // 
            // MenuBulkPropPad
            // 
            this.MenuBulkPropPad.Name = "MenuBulkPropPad";
            this.MenuBulkPropPad.Size = new System.Drawing.Size(133, 22);
            this.MenuBulkPropPad.Text = "Pad/Flash";
            this.MenuBulkPropPad.Click += new System.EventHandler(this.OnBulkPropPad);
            // 
            // MenuBulkPropLine
            // 
            this.MenuBulkPropLine.Name = "MenuBulkPropLine";
            this.MenuBulkPropLine.Size = new System.Drawing.Size(133, 22);
            this.MenuBulkPropLine.Text = "Line";
            this.MenuBulkPropLine.Click += new System.EventHandler(this.OnBulkPropLine);
            // 
            // MenuBulkPropText
            // 
            this.MenuBulkPropText.Name = "MenuBulkPropText";
            this.MenuBulkPropText.Size = new System.Drawing.Size(133, 22);
            this.MenuBulkPropText.Text = "Text";
            this.MenuBulkPropText.Click += new System.EventHandler(this.OnBulkPropText);
            // 
            // MenuBulkPropPolygon
            // 
            this.MenuBulkPropPolygon.Name = "MenuBulkPropPolygon";
            this.MenuBulkPropPolygon.Size = new System.Drawing.Size(133, 22);
            this.MenuBulkPropPolygon.Text = "Polygon";
            this.MenuBulkPropPolygon.Click += new System.EventHandler(this.OnBulkPropPolygon);
            // 
            // MenuBulkPropLayer
            // 
            this.MenuBulkPropLayer.Name = "MenuBulkPropLayer";
            this.MenuBulkPropLayer.Size = new System.Drawing.Size(133, 22);
            this.MenuBulkPropLayer.Text = "Layer";
            this.MenuBulkPropLayer.Click += new System.EventHandler(this.OnBulkPropLayer);
            // 
            // contextMenuOption0
            // 
            this.contextMenuOption0.Name = "contextMenuOption0";
            this.contextMenuOption0.Size = new System.Drawing.Size(193, 22);
            this.contextMenuOption0.Text = "toolStripMenuItem1";
            this.contextMenuOption0.Visible = false;
            this.contextMenuOption0.Click += new System.EventHandler(this.contextMenuOption0_Click);
            // 
            // contextMenuOption1
            // 
            this.contextMenuOption1.Name = "contextMenuOption1";
            this.contextMenuOption1.Size = new System.Drawing.Size(193, 22);
            this.contextMenuOption1.Text = "option1";
            this.contextMenuOption1.Visible = false;
            this.contextMenuOption1.Click += new System.EventHandler(this.contextMenuOption1_Click);
            // 
            // contextMenuOption2
            // 
            this.contextMenuOption2.Name = "contextMenuOption2";
            this.contextMenuOption2.Size = new System.Drawing.Size(193, 22);
            this.contextMenuOption2.Text = "option2";
            this.contextMenuOption2.Visible = false;
            this.contextMenuOption2.Click += new System.EventHandler(this.contextMenuOption2_Click);
            // 
            // imageListLineBending
            // 
            this.imageListLineBending.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLineBending.ImageStream")));
            this.imageListLineBending.TransparentColor = System.Drawing.Color.White;
            this.imageListLineBending.Images.SetKeyName(0, "imageBendingLine000.bmp");
            this.imageListLineBending.Images.SetKeyName(1, "imageBendingLine001.bmp");
            this.imageListLineBending.Images.SetKeyName(2, "imageBendingLine010.bmp");
            this.imageListLineBending.Images.SetKeyName(3, "imageBendingLine011.bmp");
            this.imageListLineBending.Images.SetKeyName(4, "imageBendingLine100.bmp");
            this.imageListLineBending.Images.SetKeyName(5, "imageBendingLine101.bmp");
            this.imageListLineBending.Images.SetKeyName(6, "imageBendingLine110.bmp");
            this.imageListLineBending.Images.SetKeyName(7, "imageBendingLine111.bmp");
            this.imageListLineBending.Images.SetKeyName(8, "imageStraightLine.bmp");
            this.contextMenuOnView.ResumeLayout(false);
            this.ResumeLayout(false);

		}


//		private void InitializeContextMenu()
//		{
//#if MONO
//            this.contextMenuView = new System.Windows.Forms.ContextMenu();
//            //this.contextMenuAddNode = new System.Windows.
//            //this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
//            //this.contextMenuCut = new System.Windows.Forms.ToolStripMenuItem();
//            //this.contextMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
//            //this.contextMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
//            //this.contextMenuProperty = new System.Windows.Forms.ToolStripMenuItem();
//            //this.contextMenuView.SuspendLayout();

//            // 
//            // contextMenuAddNode
//            // 
//            this.contextMenuAddNode = new MenuItem("AddNode");
//            this.contextMenuAddNode.Click += new System.EventHandler(this.OnContextMenuAddNode);
//            //// 
//            //// toolStripSeparator1
//            //// 
//            //this.toolStripSeparator1.Name = "toolStripSeparator1";
//            //this.toolStripSeparator1.Size = new System.Drawing.Size(113, 6);
//            // 
//            // contextMenuCut
//            // 
//            this.contextMenuCut = new MenuItem("Cut");
//            this.contextMenuCut.Click += new System.EventHandler(this.OnContextMenuCut);
//            // 
//            // contextMenuCopy
//            // 
//            this.contextMenuCopy = new MenuItem("Copy");
//            this.contextMenuCopy.Click += new System.EventHandler(this.OnContextMenuCopy);
//            // 
//            // contextMenuPaste
//            // 
//            this.contextMenuPaste = new MenuItem("Paste");
//            this.contextMenuPaste.Click += new System.EventHandler(this.OnContextMenuPaste);
//            // 
//            // contextMenuProperty
//            // 
//            this.contextMenuProperty = new MenuItem("Property");
//            this.contextMenuProperty.Click += new System.EventHandler(this.OnContextMenuProperty);
			
//            // 
//            // contextMenuView
//            // 
//            this.contextMenuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]{
//                this.contextMenuAddNode,
//                //this.toolStripSeparator1,
//                this.contextMenuCut,
//                this.contextMenuCopy,
//                this.contextMenuPaste,
//                this.contextMenuProperty});


//            this.contextMenuView.Name = "contextMenuView";
//            //this.contextMenuView.Size = new System.Drawing.Size(117, 120);
//            this.contextMenuView.Popup += new System.EventHandler(this.contextMenuView_Opened);


//# else
            //this.contextMenuView = new System.Windows.Forms.ContextMenuStrip(this.components);
            //this.contextMenuAddNode = new System.Windows.Forms.ToolStripMenuItem();
            //this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            //this.contextMenuCut = new System.Windows.Forms.ToolStripMenuItem();
            //this.contextMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            //this.contextMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
            //this.contextMenuProperty = new System.Windows.Forms.ToolStripMenuItem();
            //this.contextMenuView.SuspendLayout();

            //// 
            //// contextMenuView
            //// 
            //this.contextMenuView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.contextMenuAddNode,
            //this.toolStripSeparator1,
            //this.contextMenuCut,
            //this.contextMenuCopy,
            //this.contextMenuPaste,
            //this.contextMenuProperty});
            //this.contextMenuView.Name = "contextMenuView";
            //this.contextMenuView.Size = new System.Drawing.Size(117, 120);
            //this.contextMenuView.Opened += new System.EventHandler(this.contextMenuView_Opened);
            //// 
            //// contextMenuAddNode
            //// 
            //this.contextMenuAddNode.Name = "contextMenuAddNode";
            //this.contextMenuAddNode.Size = new System.Drawing.Size(116, 22);
            //this.contextMenuAddNode.Text = "AddNode";
            //this.contextMenuAddNode.Click += new System.EventHandler(this.OnContextMenuAddNode);
            //// 
            //// toolStripSeparator1
            //// 
            //this.toolStripSeparator1.Name = "toolStripSeparator1";
            //this.toolStripSeparator1.Size = new System.Drawing.Size(113, 6);
            //// 
            //// contextMenuCut
            //// 
            //this.contextMenuCut.Name = "contextMenuCut";
            //this.contextMenuCut.Size = new System.Drawing.Size(116, 22);
            //this.contextMenuCut.Text = "Cut";
            //this.contextMenuCut.Click += new System.EventHandler(this.OnContextMenuCut);
            //// 
            //// contextMenuCopy
            //// 
            //this.contextMenuCopy.Name = "contextMenuCopy";
            //this.contextMenuCopy.Size = new System.Drawing.Size(116, 22);
            //this.contextMenuCopy.Text = "Copy";
            //this.contextMenuCopy.Click += new System.EventHandler(this.OnContextMenuCopy);
            //// 
            //// contextMenuPaste
            //// 
            //this.contextMenuPaste.Name = "contextMenuPaste";
            //this.contextMenuPaste.Size = new System.Drawing.Size(116, 22);
            //this.contextMenuPaste.Text = "Paste";
            //this.contextMenuPaste.Click += new System.EventHandler(this.OnContextMenuPaste);
            //// 
            //// contextMenuProperty
            //// 
            //this.contextMenuProperty.Name = "contextMenuProperty";
            //this.contextMenuProperty.Size = new System.Drawing.Size(116, 22);
            //this.contextMenuProperty.Text = "Property";
            //this.contextMenuProperty.Click += new System.EventHandler(this.OnContextMenuProperty);
            //this.contextMenuView.ResumeLayout(false);
//#endif
//		}



		public void ExportImage()
		{
			ExportImageForm dlg = new ExportImageForm();

			string path =document.DocumentPath;
			if (path.Length != 0) {
				dlg.DestPath = Path.ChangeExtension(path, "png");
			}


			DialogResult dr = dlg.ShowDialog();
			if (dr != DialogResult.OK) {
				return;
			}
            path = dlg.DestPath;

            document.ReleaseTemp();
            RedrawAll();


            exportImageColorMode = dlg.ColorMode;

			int dpi = dlg.Resolution;
			double printScale = 254000.0 / dpi;

			int bmpWidth = (int)(dlg.ExpAreaWidth / printScale);
			int bmpHeight = (int)(dlg.ExpAreaHeight / printScale);
			int bmpOriginX = (int)(-dlg.ExpAreaLeft / printScale);
			int bmpOriginY = (int)((dlg.ExpAreaHeight + dlg.ExpAreaBottom) / printScale);
			Bitmap bmp;
			try {
				bmp = new Bitmap(bmpWidth, bmpHeight);
			}
			catch {
				MessageBox.Show("Internal error. (Perhaps too large image data)", "MBE", MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			Graphics g = Graphics.FromImage(bmp);
			g.Clear(Color.White);
			g.PageUnit = GraphicsUnit.Pixel;
			g.TranslateTransform(bmpOriginX, bmpOriginY);

            uint option = 0;
            if (exportImageColorMode) option |= (uint)MbeDrawOption.PrintColor;
			DrawMainForPrint(g, printScale,visibleLayer,option);

			try {
				string ext = Path.GetExtension(path);
				if (ext.Length == 0) {
					path = Path.ChangeExtension(path, "png");
				}
				bmp.Save(path);
			}
			catch {
				MessageBox.Show("Failed to save the image file.", "MBE", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			g.Dispose();
			bmp.Dispose();
			
			

		}



		private System.Drawing.Printing.PageSettings printPagesettings;
		private System.Drawing.Printing.PrinterSettings printerSettings;
        DateTime printStartTime;
        DateTime lastWriteTime;
        bool lastWriteTimeIsValid;

		public void Print()
		{
			document.ReleaseTemp();
			RedrawAll();

            printStartTime = DateTime.Now;
            lastWriteTimeIsValid = true;
            try {
                lastWriteTime = File.GetLastWriteTime(document.DocumentPath);
            }catch{
                lastWriteTimeIsValid = false;
            }




            if (printerSettings == null) {
                printerSettings = new System.Drawing.Printing.PrinterSettings();
            }
            printDialog1.PrinterSettings = printerSettings;

            if (printDialog1.ShowDialog() == DialogResult.OK) {
                printerSettings = printDialog1.PrinterSettings;
                if (printerSettings.PrinterName.Length != 0) {
                    printDocument1.PrinterSettings = printerSettings;
                    printPagesettings = printerSettings.DefaultPageSettings;
                    if (printPagesettings != null) {
                        String docName = document.DocumentName;
                        if (docName == "") docName = "Untitled";
                        printDocument1.DocumentName = docName;
                        SetupPrintPageInfo();
                        printDocument1.Print();
                    }
                }
            }
        
		}



		public void PrintPreview()
		{
			document.ReleaseTemp();
			RedrawAll();

            printStartTime = DateTime.Now;
            lastWriteTimeIsValid = true;
            try {
                lastWriteTime = File.GetLastWriteTime(document.DocumentPath);
            }
            catch {
                lastWriteTimeIsValid = false;
            }



			if (printerSettings == null) {
				printerSettings = new System.Drawing.Printing.PrinterSettings();
			}
			printDialog1.PrinterSettings = printerSettings;

			if (printDialog1.ShowDialog() == DialogResult.OK) {
				printerSettings = printDialog1.PrinterSettings;
                if (printerSettings.PrinterName.Length != 0) {
                    printDocument1.PrinterSettings = printerSettings;
                    printPagesettings = printerSettings.DefaultPageSettings;

                    if (printPagesettings != null) {
                        String docName = document.DocumentName;
                        if (docName == "") docName = "Untitled";
                        printDocument1.DocumentName = docName;
                        SetupPrintPageInfo();
                        PrintPreviewDialog printPreviewDlg = new PrintPreviewDialog();
                        printPreviewDlg.Document = printDocument1;
                        printPreviewDlg.ShowDialog();
                    }
                }
			}
		}

        /// <summary>
        /// �w�b�_�[�̕ϐ������񂩂�l�ւ̕Ԋ�
        /// </summary>
        /// <param name="var">�ϐ�������</param>
        /// <param name="printLayer">������C���[�̃r�b�gOR</param>
        /// <returns></returns>
        private string printHeaderVarValue(string var, ulong printLayer)
        {
            switch (var) {
                case "DOCNAME":
                    return document.DocumentTitle;
                case "DOCPATH":
                    return document.DocumentPath;
                case "LAYERNOTE":
                    if (printPageLayerList == null) {
                        return "";
                    } else {
                        return printPageLayerList[currentPrintPage].name;
                    }
                case "LAYER":
                    {
                        StringBuilder sbLayer = new StringBuilder();
                        int layerCount = MbeLayer.valueTable.Length;
                        bool firstflag = true;
                        for(int i=0;i<layerCount;i++){
                            if((((ulong)MbeLayer.valueTable[i]) & printLayer) !=0){
                                if (firstflag) {
                                    firstflag = false;
                                } else {
                                    sbLayer.Append("+");
                                }
                                sbLayer.Append(MbeLayer.nameTable[i]);
                            }
                        }
                        return sbLayer.ToString();
                    }
                case "SAVETIME":
                    if (lastWriteTimeIsValid) {
                        return lastWriteTime.ToString();
                    } else {
                        return "";
                    }
                    //break;
                case "TIME":
                    return printStartTime.ToString();
                case "PAGE":
                    return (currentPrintPage+1).ToString();
                case "PAGES":
                    return printPageCount.ToString();
            }
            return "";
        }

        /// <summary>
        /// �_�C�A���O�{�b�N�X�Ŏw�肵���w�b�_�[����������ۂɈ������w�b�_�[������ɓW�J����B
        /// </summary>
        /// <param name="printLayer"></param>
        /// <returns></returns>
        private string makePrintHeaderText(ulong printLayer)
        {
            string formatText = headerText;
            int length = formatText.Length;
            int index = 0;
            StringBuilder sbActualHeaderText = new StringBuilder();
            StringBuilder sbVar = new StringBuilder(); ;
            int parseVarState = 0;

            while(index<length){
                char c = formatText[index];
                index++;
                if(parseVarState==0){           //parseVarState==0 �͕ϐ��ǂݏo�����J�n
                    if(c == '$'){
                        parseVarState = 1;
                        continue;
                    }
                }else if(parseVarState==1){     //parseVarState==1 �͕ϐ��ǂݏo���̐擪��$�ǂݎ���
                    if(c != '$'){               //$���A�����Ȃ�������ϐ��̐擪
                        parseVarState = 2;
                        sbVar.Length = 0;
                        sbVar.Append(Char.ToUpper(c));
                        continue;
                    }else{
                        parseVarState = 0;
                    }
                }else{  //if(parseVarState==2){
                    if(c == '$'){               //�ϐ��I�[��$�̌��o
                        sbActualHeaderText.Append(printHeaderVarValue(sbVar.ToString(),printLayer));
                        parseVarState = 0;
                    }else{
                        sbVar.Append(Char.ToUpper(c));
                    }
                    continue;
                }
                sbActualHeaderText.Append(c);
            }
            return sbActualHeaderText.ToString();
        }

        /// <summary>
        /// �w�b�_�[�̈��
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="height"></param>
        /// <param name="text"></param>
        private void DrawHeader(Graphics g,int x,int y, int height, string text)
        {
            Font font = new Font(FontFamily.GenericSerif, height, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            g.DrawString(text, font, brush, new PointF(x, y));
            brush.Dispose();
            font.Dispose();
        }

		private void printDocument1_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e)
		{

			Graphics g = e.Graphics;

			int dpi = (int)g.DpiX;	//Version 0.23b
			//int dpi = printPagesettings.PrinterResolution.X;
            System.Diagnostics.Debug.WriteLine("printDocument1_PrintPage()  DPI = " + dpi + "   PAGE:"+ currentPrintPage);
			Rectangle rc = printPagesettings.Bounds;

            System.Diagnostics.Debug.WriteLine("left margin =  " + printPagesettings.Margins.Left + "   bottom margin = " + printPagesettings.Margins.Bottom);

            bool bLandscape = printPagesettings.Landscape;
            int hardMarginLeft;
            int hardMarginTop;
            int printAreaHeight;
            int printAreaWidth;

            if (!bLandscape) {
                hardMarginLeft = (int)printPagesettings.HardMarginX;
                hardMarginTop = (int)printPagesettings.HardMarginY;
                printAreaHeight = (int)printPagesettings.PrintableArea.Height;
                printAreaWidth = (int)printPagesettings.PrintableArea.Width;
            }else{
                hardMarginLeft = (int)printPagesettings.HardMarginY;
                hardMarginTop = (int)printPagesettings.HardMarginX;
                printAreaHeight = (int)printPagesettings.PrintableArea.Width;
                printAreaWidth = (int)printPagesettings.PrintableArea.Height;
            }

            int leftMargin = PrintLeftMargin / 2540;        //�ݒ�l��1/10000mm�P�ʂ� 1/100�C���`�P�ʂɕϊ�
            int bottomMargin = PrintBottomMargin / 2540;    //�ݒ�l��1/10000mm�P�ʂ� 1/100�C���`�P�ʂɕϊ�

            int printDpi;
            int bmpWidth =0;
            int bmpHeight =0;
            Bitmap bmp = new Bitmap(1,1);
            //�r�b�g�}�b�v�J���[����̂Ƃ��́A����𑜓x�ƈ���G���A�����߂Ă���r�b�g�}�b�v���쐬����B
            //�r�b�g�}�b�v�̍쐬�Ɏ��s�����甒�����[�h�ɂ���B
            if (PrintColorMode == PrintColorModeValue.ColorBitmap) {
                printDpi = (COLOR_PRINT_BMP_MAX_RESOLUTION < dpi ? COLOR_PRINT_BMP_MAX_RESOLUTION : dpi);
                bmpWidth = printAreaWidth * printDpi / 100;
                bmpHeight = printAreaHeight * printDpi / 100;
                try {
                    bmp = new Bitmap(bmpWidth, bmpHeight);
                }
                catch {
                    PrintColorMode = PrintColorModeValue.BlackAndWhite;
                    printDpi = dpi;
                }
            } else {
                printDpi = dpi;
            }
            
            double printScale = 254000.0 / printDpi;

            //double drawOriginX = (rc.Left + leftMargin) / 100.0 * dpi;      // 1/100�C���`�P�ʂ��s�N�Z���P�ʂɕϊ�
            //double drawOriginY = (rc.Bottom - bottomMargin) / 100.0 * dpi;  // 1/100�C���`�P�ʂ��s�N�Z���P�ʂɕϊ�

            bool _printMirror;
            if(printPageLayerList == null){
                _printMirror = printMirrorMode;
            }else{
                _printMirror = printPageLayerList[currentPrintPage].mirror;
            }

            double drawOriginX;

            if (!_printMirror) {
                drawOriginX = rc.Left + (leftMargin - hardMarginLeft);      
            } else {
                drawOriginX = rc.Right - (leftMargin - hardMarginLeft);
            }

            drawOriginX = drawOriginX / 100.0 * printDpi;   // 1/100�C���`�P�ʂ��s�N�Z���P�ʂɕϊ�
            double drawOriginY = (printAreaHeight - (bottomMargin - (rc.Bottom - printAreaHeight - hardMarginTop))) / 100.0 * printDpi;  // 1/100�C���`�P�ʂ��s�N�Z���P�ʂɕϊ�

			System.Diagnostics.Debug.WriteLine("drawOriginX = "+ drawOriginX);
			System.Diagnostics.Debug.WriteLine("drawOriginY = "+ drawOriginY);

            Graphics gDrawTo;
            if (PrintColorMode == PrintColorModeValue.ColorBitmap) {
                gDrawTo = Graphics.FromImage(bmp);
                gDrawTo.Clear(Color.White);
            } else {
                gDrawTo = g;
            }

            gDrawTo.PageUnit = GraphicsUnit.Pixel;

            ulong printLayer;
            if (printPageLayerList == null) {
                printLayer = visibleLayer;
            } else {
                printLayer = printPageLayerList[currentPrintPage].checkvalue;
            }

            //�w�b�_�[�̈��
            if (printHeader) {
                string header = makePrintHeaderText(printLayer);
                System.Diagnostics.Debug.WriteLine(header);
                int x = (int)(0.8 * printDpi - (hardMarginLeft / 100.0 * printDpi));
                int y = (int)(0.2 * printDpi - (hardMarginTop  / 100.0 * printDpi));
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                DrawHeader(g, x, y, printDpi / 8, header);
            }



            
            gDrawTo.TranslateTransform((int)drawOriginX, (int)drawOriginY);
            if (_printMirror) {
                gDrawTo.ScaleTransform(-1.0F, 1.0F);
            }

            if (Print2xMode) {
                printScale /= 2;
            }

            uint option = 0;
            if(CenterPunchMode) option |= (uint)MbeDrawOption.CenterPunchMode;
            if(PrintToolMarkMode) option |= (uint)MbeDrawOption.ToolMarkMode;
            if (PrintColorMode != PrintColorModeValue.BlackAndWhite) option |= (uint)MbeDrawOption.PrintColor;
            DrawMainForPrint(gDrawTo, printScale, printLayer,option);

            if (PrintColorMode == PrintColorModeValue.ColorBitmap) {
                //printPagesettings.Margins.Left = 0;
                //printPagesettings.Margins.Top = 0;

                g.ResetTransform();
                //gDrawTo.ResetTransform();
                g.PageUnit = GraphicsUnit.Display;


                g.DrawImage(bmp, 0, 0, printAreaWidth,printAreaHeight);

                //g.DrawImage(bmp, 0, 0, (int)((printAreaHeight * dpi) / 100.0), (int)((printAreaWidth * dpi) / 100.0));
                //bmp.Save("C:\\Users\\hitoshi\\Documents\\test.bmp");
                bmp.Dispose();
            }



            currentPrintPage++;
            if (printPageCount <= currentPrintPage) {
                e.HasMorePages = false;
                currentPrintPage = 0;
            } else {
                e.HasMorePages = true;
            }


		}

        protected void LinePropLineBending(MbeObjLine.MbeLineStyle bendval)
        {
            MbeObj obj = document.TempList.First.Value;
            if (obj.Id() == MbeObjID.MbeLine) {
                ((MbeObjLine)obj).LineStyle = bendval;
                document.TempModified = true;
                RedrawAll();
            }
        }


        private void contextMenuOption0_Click(object sender, EventArgs e)
        {
            if (contextMenuOptionMode == ContectMenuOptionMode.IDLE) return;
            else if (contextMenuOptionMode == ContectMenuOptionMode.LINEBENDING) {
                LinePropLineBending(MbeObjLine.MbeLineStyle.Straight);
            }
        }

        private void contextMenuOption1_Click(object sender, EventArgs e)
        {
            if (contextMenuOptionMode == ContectMenuOptionMode.IDLE) return;
            else if (contextMenuOptionMode == ContectMenuOptionMode.LINEBENDING) {
                LinePropLineBending(MbeObjLine.MbeLineStyle.Bending1);
            }
        }

        private void contextMenuOption2_Click(object sender, EventArgs e)
        {
            if (contextMenuOptionMode == ContectMenuOptionMode.IDLE) return;
            else if (contextMenuOptionMode == ContectMenuOptionMode.LINEBENDING) {
                LinePropLineBending(MbeObjLine.MbeLineStyle.Bending2);
            }
        }



        /// <summary>
        /// �R���e�L�X�g���j���[�̃I�v�V�������ڂ̐ݒ�
        /// </summary>
        private void setContextMenuOption()
        {
            contextMenuOptionMode = ContectMenuOptionMode.IDLE;
            contextMenuOption0.Visible = false;
            contextMenuOption1.Visible = false;
            contextMenuOption2.Visible = false;
            if (Document.CanEditLineProperty()) {
                MbeObj obj = document.TempList.First.Value;
                if (obj.Id() == MbeObjID.MbeLine) {
                    MbeObjLine objLine = (MbeObjLine)obj;
                    Point p0 = objLine.GetPos(0);
                    Point p1 = objLine.GetPos(1);

                    //Bending�̃^�C�v�̃{�^���C���[�W�̑I��  SetLinePropForm����R�s�y 2010/01/16
                    int imageIndex = 0;
                    //�E�オ��?
                    imageIndex |= ((p0.X < p1.X && p0.Y < p1.Y || p0.X > p1.X && p0.Y > p1.Y) ? 4 : 0);
                    //�c��?
                    imageIndex |= ((Math.Abs(p0.X - p1.X) < Math.Abs(p0.Y - p1.Y)) ? 2 : 0);
                    //���n�_?
                    imageIndex |= ((p0.Y < p1.Y) ? 1 : 0);

                    contextMenuOption0.Image = this.imageListLineBending.Images[8];
                    contextMenuOption1.Image = this.imageListLineBending.Images[imageIndex ^ 1];
                    contextMenuOption2.Image = this.imageListLineBending.Images[imageIndex];

                    contextMenuOption0.Text = "Straight";
                    contextMenuOption1.Text = "Bended";
                    contextMenuOption2.Text = "Bended";


                    contextMenuOption0.Visible = (objLine.LineStyle!=MbeObjLine.MbeLineStyle.Straight);
                    contextMenuOption1.Visible = (objLine.LineStyle!=MbeObjLine.MbeLineStyle.Bending1);
                    contextMenuOption2.Visible = (objLine.LineStyle!=MbeObjLine.MbeLineStyle.Bending2);
                    contextMenuOptionMode = ContectMenuOptionMode.LINEBENDING;

                }
            }
        }


        /// <summary>
        /// �R���e�L�X�g���j���[���J�����Ƃ��̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void contextMenuView_Opened(object sender, EventArgs e)
		{
			contextMenuProperty.Enabled = Document.CanEditProperty();
			contextMenuAddNode.Enabled = CanDivideLine();
			contextMenuCut.Enabled = Document.CanCopy();
			contextMenuCopy.Enabled = Document.CanCopy();
			contextMenuPaste.Enabled = Document.CanPaste();

            setContextMenuOption();
		}

		private void OnContextMenuAddNode(object sender, EventArgs e)
		{
			AddNode();
		}

		private void OnContextMenuProperty(object sender, EventArgs e)
		{
			EditProperty();
		}

		private void OnContextMenuCut(object sender, EventArgs e)
		{
			Cut();
		}

		private void OnContextMenuCopy(object sender, EventArgs e)
		{
            Point dummyPt;
			Copy(out dummyPt);
		}

		private void OnContextMenuPaste(object sender, EventArgs e)
		{
			Paste();
		}

        private void OnContextMenuBulkPropOpen(object sender, EventArgs e)
        {
            CanBulkPropResult result = Document.CanBulkProperty();
            this.MenuBulkPropHole.Enabled = result.CanPropHole;
            this.MenuBulkPropPTH.Enabled = result.CanPropPTH;
            this.MenuBulkPropPad.Enabled = result.CanPropPad;
            this.MenuBulkPropLine.Enabled = result.CanPropLine;
            this.MenuBulkPropText.Enabled = result.CanPropText;
            this.MenuBulkPropLayer.Enabled = result.CanMoveLayer;
            this.MenuBulkPropPolygon.Enabled = result.CanPropPolygon;
            lastCanBulkPropResult = result;
        }

        public CanBulkPropResult lastCanBulkPropResult;

        private void OnBulkPropHole(object sender, EventArgs e)
        {
            BulkPropHole();
        }

        private void OnBulkPropPTH(object sender, EventArgs e)
        {
            BulkPropPTH();
        }

        private void OnBulkPropPad(object sender, EventArgs e)
        {
            BulkPropPad();
        }

        private void OnBulkPropLine(object sender, EventArgs e)
        {
            BulkPropLine();
        }

        private void OnBulkPropText(object sender, EventArgs e)
        {
            BulkPropText();
        }

        private void OnBulkPropPolygon(object sender, EventArgs e)
        {
            BulkPropPolygon();
        }

        private void OnBulkPropLayer(object sender, EventArgs e)
        {
            BulkPropLayer(lastCanBulkPropResult);
        }






       /// <summary>
       /// �h�����}�[�N�̕`��
       /// </summary>
       /// <param name="_pt"></param>
       /// <param name="dia"></param>
       /// <param name="dp"></param>
       /// <param name="col"></param>
       /// <param name="w"></param>
        public static void DrawDrillMark(Point _pt, int dia, DrawParam dp,Color col,int w)
        {

            Point pt = MbeObj.ToDrawDim(_pt, dp.scale);
            Pen pen;
            
            if (toolMarkLib != null) {
                MbeObjComponent objMark = null;
                string name = String.Format("D{0:##0.0###}", (double)dia/10000);
                foreach (MbeObjComponent obj in toolMarkLib.componentArray) {
                    if (obj.SigName == name) {
                        objMark = obj;
                        break;
                    }
                }
                if (objMark != null) {
                    pen = new Pen(col, w);
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    foreach (MbeObj obj in objMark.ContentsObj) {
                        if (obj.Id() == MbeObjID.MbeLine) {
                            Point pt0 = obj.GetPos(0);
                            Point pt1 = obj.GetPos(1);
                            int x0 = pt.X + (int)(pt0.X / dp.scale);
                            int y0 = pt.Y - (int)(pt0.Y / dp.scale);
                            int x1 = pt.X + (int)(pt1.X / dp.scale);
                            int y1 = pt.Y - (int)(pt1.Y / dp.scale);
                            dp.g.DrawLine(pen, x0, y0, x1, y1);
                        } else if (obj.Id() == MbeObjID.MbeArc) {
                            int drawWidth = ((MbeObjArc)obj).Radius * 2;
                            drawWidth = MbeObj.ToDrawDim(drawWidth, dp.scale) | 1;
                            Rectangle rc0= new Rectangle(pt.X - drawWidth / 2, pt.Y - drawWidth / 2, drawWidth, drawWidth);
                            int sweepAngle;
                            if (((MbeObjArc)obj).LimitStartEnd()) {
                                sweepAngle = 3600;
                            } else if (((MbeObjArc)obj).StartAngle > ((MbeObjArc)obj).EndAngle) {
                                sweepAngle = 3600 - ((MbeObjArc)obj).StartAngle + ((MbeObjArc)obj).EndAngle;
                            } else {
                                sweepAngle = ((MbeObjArc)obj).EndAngle - ((MbeObjArc)obj).StartAngle;
                            }
                            if (drawWidth > 0) {
                                dp.g.DrawArc(pen, rc0, ((float)-((MbeObjArc)obj).StartAngle) / 10, ((float)-sweepAngle) / 10);
                            }




                        }
                    }
                    pen.Dispose();

                    return;
                }
            }

            pen = new Pen(col, w);
            int drawDia = (int)(dia / dp.scale) | 1;
            Rectangle rc = new Rectangle(pt.X - drawDia / 2, pt.Y - drawDia / 2, drawDia, drawDia);
			dp.g.DrawEllipse(pen, rc);
            pen.Dispose();

            string label = String.Format("{0:##0.0###}", (double)dia / 10000);
            int h = 12000;
            int drawTextWidth = MbeBoardFont.DrawWidth(h, label);
            int x = pt.X - (int)(drawTextWidth / 2 / dp.scale);
            int y = pt.Y + (int)(h / 2 / dp.scale);
            h = (int)(h / dp.scale);

            boardFont.DrawString(dp.g, x, y, false, label, h, w, Color.Black);




            //refNum.Draw(dp);
            //if (contentsObj != null) {
            //    foreach (MbeObj obj in contentsObj) {
            //        if (obj.Id() == MbeObjID.MbePTH ||
            //           obj.Id() == MbeObjID.MbePinSMD ||
            //           obj.Id() == MbeObjID.MbeHole) {
            //            obj.Draw(dp);
            //        } else {
            //            drawSnapMark = false;
            //            obj.Draw(dp);
            //            drawSnapMark = true;
            //        }
            //    }
            //}
            //if (dp.mode != MbeDrawMode.Mono && dp.layer == layer) {
            //    Point pt0 = this.GetPos(0);
            //    pt0 = ToDrawDim(pt0, dp.scale);
            //    int marksize = (int)(10000 / dp.scale);
            //    if (marksize < 5) marksize = 5;
            //    DrawSnapPointMark(dp.g, pt0, marksize, selectFlag[0]);
            //}
        }

  	}

    //class Win32WindowProperty
    //{
    //    public const int WS_EX_CLIENTEDGE = unchecked((int)0x00000200);
    //    public const int WS_BORDER = unchecked((int)0x00800000);
    //}
}
