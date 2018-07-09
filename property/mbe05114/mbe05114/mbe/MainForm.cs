using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CE3IO;
namespace mbe
{
    public partial class MainForm : Form
    {
        int m_initialWindowState;
        Size m_initialWindowSize;


        public MainForm()
        {
            InitializeComponent();
#if MONO
			this.MenuFilePageSetup.Enabled = false;
			this.MenuFilePrint.Enabled = false;
			this.MenuFilePrintPreview.Enabled = false;
#endif


            mbeView.PrintLeftMargin = Properties.Settings.Default.PrintLeftMargin;
            mbeView.PrintBottomMargin = Properties.Settings.Default.PrintBottomMargin;

            mbeView.Print2xMode = Properties.Settings.Default.Print2xMode;
            mbeView.PrintMirrorMode = Properties.Settings.Default.PrintMirrorMode;
            mbeView.CenterPunchMode = Properties.Settings.Default.CenterPunchMode;
            mbeView.PrintToolMarkMode = Properties.Settings.Default.ToolMarkPrint;
            mbeView.PrintColorMode = (MbeView.PrintColorModeValue)Properties.Settings.Default.PrintColorMode;
            mbeView.ExportImageColorMode = Properties.Settings.Default.ImageColorMode;
            mbeView.PrintHeader = Properties.Settings.Default.PrintHeader;
            mbeView.HeaderText = Properties.Settings.Default.HeaderText;

            mbeView.PasteAtCurrsor = (Properties.Settings.Default.PasteObjectAtCursor!=0);
            Properties.Settings.Default.PasteObjectAtCursor =(uint)(mbeView.PasteAtCurrsor ? 1: 0);

            mbeView.EnableOneKeyToolSelection = (Properties.Settings.Default.OneKeyToolSelection!=0);
            
            Properties.Settings.Default.OneKeyToolSelection = (mbeView.EnableOneKeyToolSelection ? 1:0);

            m_initialWindowState = Properties.Settings.Default.MainWindowState;
            m_initialWindowSize = new Size(Properties.Settings.Default.MainWindowWidth, Properties.Settings.Default.MainWindowHeight);

            //if (Properties.Settings.Default.MainWindowState == 1) {
            //    WindowState = FormWindowState.Maximized;
            //} else {
            //    int cx = Properties.Settings.Default.MainWindowWidth;
            //    int cy = Properties.Settings.Default.MainWindowHeight;
            //    Size = new Size(cx, cy);
            //}

            SCKey_Load();
            SCKey_Save();

            Properties.Settings.Default.Save();

            //componentLibrary = new MbeLibs();
            Application.Idle += new EventHandler(Application_Idle);
			//lastCanBulkPropResult = new CanBulkPropResult();
        }

        private void SCKey_SetToMenu(System.Windows.Forms.ToolStripMenuItem menuItem,string strSCKey)
        {
            StringBuilder keyName = new StringBuilder();
            bool rdModifier = true;
            bool shiftKey = false;
            bool ctrlKey = false;
            for (int i = 0; i < strSCKey.Length; i++) {
                if (rdModifier) {
                    switch (Char.ToUpper(strSCKey[i])) {
                        case '+':
                            rdModifier = false;
                            break;
                        case 'S':
                            shiftKey = true;
                            break;
                        case 'C':
                            ctrlKey = true;
                            break;
                        default:
                            break;
                    }
                } else {
                    keyName.Append(Char.ToUpper(strSCKey[i]));
                }
            }
            if (keyName.Length == 0) {
                return;
            }else if (keyName.Length == 1) {
                if (keyName[0] < 'A' || keyName[0] > 'Z') {
                    return;
                }
                if(!ctrlKey){   //Short cut key A to Z needs a modifier CTRL.
                    return;
                }
            } else if (keyName.Length >= 2) {
                if (keyName[0] == 'F') {
                    string suffix = keyName.ToString().Substring(1);
                    int functionKeyValue;
                    try { functionKeyValue = Convert.ToInt32(suffix); }
                    catch (Exception) { functionKeyValue = 0; }
                    if (functionKeyValue < 1 || functionKeyValue > 24) {
                        return;
                    }
                } else {
                    return;
                }
            } else {
                return;
            }
            System.Windows.Forms.Keys keyValue = 0;
            if(shiftKey){
                keyValue |= System.Windows.Forms.Keys.Shift;
            }
            if(ctrlKey){
                keyValue |= System.Windows.Forms.Keys.Control;
            }

            KeysConverter kc = new KeysConverter();
            keyValue |= (System.Windows.Forms.Keys)kc.ConvertFromString(keyName.ToString());

            menuItem.ShortcutKeys = keyValue;

        }


        private void SCKey_Load()
        {
            SCKey_SetToMenu(this.MenuEditFlip, Properties.Settings.Default.SCKeyEditFlip);
            SCKey_SetToMenu(this.MenuEditComponenting, Properties.Settings.Default.SCKeyEditComponenting);
            SCKey_SetToMenu(this.MenuEditUncomponenting, Properties.Settings.Default.SCKeyEditUncomponenting);
            SCKey_SetToMenu(this.MenuEditProperty, Properties.Settings.Default.SCKeyEditProperty);
            SCKey_SetToMenu(this.MenuToolArc, Properties.Settings.Default.SCKeyToolArc);
            SCKey_SetToMenu(this.MenuToolComponent, Properties.Settings.Default.SCKeyToolComponent);
            SCKey_SetToMenu(this.MenuToolConnectionCheck, Properties.Settings.Default.SCKeyToolConnectionCheck);
            SCKey_SetToMenu(this.MenuToolHole, Properties.Settings.Default.SCKeyToolHole);
            SCKey_SetToMenu(this.MenuToolLine, Properties.Settings.Default.SCKeyToolLine);
            SCKey_SetToMenu(this.MenuToolRuler, Properties.Settings.Default.SCKeyToolMeasure);
            SCKey_SetToMenu(this.MenuToolPad, Properties.Settings.Default.SCKeyToolPad);
            SCKey_SetToMenu(this.MenuToolPolygon, Properties.Settings.Default.SCKeyToolPolygon);
            SCKey_SetToMenu(this.MenuToolPTH, Properties.Settings.Default.SCKeyToolPTH);
            SCKey_SetToMenu(this.MenuToolSelector, Properties.Settings.Default.SCKeyToolSelector);
            SCKey_SetToMenu(this.MenuToolText, Properties.Settings.Default.SCKeyToolText);
            SCKey_SetToMenu(this.MenuEditUpdateFillPolygon, Properties.Settings.Default.SCKeyUpdateFillPolygon);
            SCKey_SetToMenu(this.MenuViewPolygonFrameMode, Properties.Settings.Default.SCKeyViewPolygonFrameMode);
            SCKey_SetToMenu(this.MenuSetResetGrid, Properties.Settings.Default.SCKeyResetGridOrigin);
            SCKey_SetToMenu(this.MenuSetGridOrigin, Properties.Settings.Default.SCKeySetGridOrigin);
            SCKey_SetToMenu(this.MenuBulkPropHole, Properties.Settings.Default.SCKeyEditBulkHole);
            SCKey_SetToMenu(this.MenuBulkPropPTH, Properties.Settings.Default.SCKeyEditBulkPTH);
            SCKey_SetToMenu(this.MenuBulkPropPad, Properties.Settings.Default.SCKeyEditBulkPad);
            SCKey_SetToMenu(this.MenuBulkPropLine, Properties.Settings.Default.SCKeyEditBulkLine);
            SCKey_SetToMenu(this.MenuBulkPropPolygon, Properties.Settings.Default.SCKeyEditBulkPolygon);
            SCKey_SetToMenu(this.MenuBulkPropText, Properties.Settings.Default.SCKeyEditBulkText);
            SCKey_SetToMenu(this.MenuBulkPropLayer, Properties.Settings.Default.SCKeyEditBulkLayer);
        }

        private void SCKey_Save()
        {
            //System.Windows.Forms.Keys key;
            //KeysConverter kc = new KeysConverter();
            //key = (System.Windows.Forms.Keys)kc.ConvertFromString("numpad0");

            //System.Windows.Forms.Keys key2 = System.Windows.Forms.Keys.NumPad0;

            Properties.Settings.Default.SCKeyEditFlip = Properties.Settings.Default.SCKeyEditFlip; 
            Properties.Settings.Default.SCKeyEditComponenting = Properties.Settings.Default.SCKeyEditComponenting;
            Properties.Settings.Default.SCKeyEditUncomponenting = Properties.Settings.Default.SCKeyEditUncomponenting;
            Properties.Settings.Default.SCKeyEditProperty = Properties.Settings.Default.SCKeyEditProperty;
            Properties.Settings.Default.SCKeyToolArc = Properties.Settings.Default.SCKeyToolArc;
            Properties.Settings.Default.SCKeyToolComponent = Properties.Settings.Default.SCKeyToolComponent;
            Properties.Settings.Default.SCKeyToolConnectionCheck = Properties.Settings.Default.SCKeyToolConnectionCheck;
            Properties.Settings.Default.SCKeyToolHole = Properties.Settings.Default.SCKeyToolHole;
            Properties.Settings.Default.SCKeyToolLine = Properties.Settings.Default.SCKeyToolLine;
            Properties.Settings.Default.SCKeyToolMeasure = Properties.Settings.Default.SCKeyToolMeasure;
            Properties.Settings.Default.SCKeyToolPad = Properties.Settings.Default.SCKeyToolPad;
            Properties.Settings.Default.SCKeyToolPolygon = Properties.Settings.Default.SCKeyToolPolygon;
            Properties.Settings.Default.SCKeyToolPTH = Properties.Settings.Default.SCKeyToolPTH;
            Properties.Settings.Default.SCKeyToolSelector = Properties.Settings.Default.SCKeyToolSelector;
            Properties.Settings.Default.SCKeyToolText = Properties.Settings.Default.SCKeyToolText;
            Properties.Settings.Default.SCKeyUpdateFillPolygon =  Properties.Settings.Default.SCKeyUpdateFillPolygon;
            Properties.Settings.Default.SCKeyViewPolygonFrameMode = Properties.Settings.Default.SCKeyViewPolygonFrameMode;
            Properties.Settings.Default.SCKeyResetGridOrigin = Properties.Settings.Default.SCKeyResetGridOrigin;
            Properties.Settings.Default.SCKeySetGridOrigin = Properties.Settings.Default.SCKeySetGridOrigin;
            Properties.Settings.Default.SCKeyEditBulkHole = Properties.Settings.Default.SCKeyEditBulkHole;
            Properties.Settings.Default.SCKeyEditBulkPTH = Properties.Settings.Default.SCKeyEditBulkPTH;
            Properties.Settings.Default.SCKeyEditBulkPad = Properties.Settings.Default.SCKeyEditBulkPad;
            Properties.Settings.Default.SCKeyEditBulkLine = Properties.Settings.Default.SCKeyEditBulkLine;
            Properties.Settings.Default.SCKeyEditBulkPolygon = Properties.Settings.Default.SCKeyEditBulkPolygon;
            Properties.Settings.Default.SCKeyEditBulkText = Properties.Settings.Default.SCKeyEditBulkText;
            Properties.Settings.Default.SCKeyEditBulkLayer = Properties.Settings.Default.SCKeyEditBulkLayer;


        }

        void Application_Idle(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Application.Idle");
            if (WindowState != FormWindowState.Minimized) {
                SetMenuEditItemEnable();
                SetModeInfo();
            } else {
                System.Threading.Thread.Sleep(100);
            }
            //System.Diagnostics.Debug.WriteLine("end Application.Idle");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
            if (m_initialWindowState == 1) {
                WindowState = FormWindowState.Maximized;
            } else {
                Size = m_initialWindowSize;
            }

			MbeColors.InitColor();
			MbeColors.LoadSettings();
            mbeView.SetWorkAreaSize(MbeDoc.WORK_AREA_DEFAULT_WIDTH, MbeDoc.WORK_AREA_DEFAULT_HEIGHT);
			mbeView.EnablePaint = true;
			setLayerWindow.EnablePaint = true;
			mbeView.SetMode(MbeView.ModeMajor.SelectorMode);
            mbeView.findFormMovesCursor += new MbeView.FindFormMovesCursor(this.OnMouseMoveView);

            LoadLibraries();

            LoadToolMark();
            
			//tBarMajorはツールバーが表示しているモード
			//tBarMajorと実際にモードが異なるときにツールバーが更新される。
			//初期値はSelectorMode以外のどれかとする。
			tBarMajor = MbeView.ModeMajor.SetGridOriginMode;


			string[] cmdlineParam;
			cmdlineParam = System.Environment.GetCommandLineArgs();
            if (cmdlineParam.Length > 1) {
                string path = cmdlineParam[1];
                if (!DoFileOpen(path)) {
                    SetTitle();
                }
            } else {
                SetTitle();
            }

            DisplayGridValue();

            Program.listFileUseLocalEncoding = (Properties.Settings.Default.ListFileUseLocalEncoding>0 ? true: false);
            Properties.Settings.Default.ListFileUseLocalEncoding = (Program.listFileUseLocalEncoding ? 1 : 0);
            Program.inhibitHatchBrushPolygonframe = (Properties.Settings.Default.InhibitHatchBrushPolygonframe > 0 ? true : false);
            Properties.Settings.Default.InhibitHatchBrushPolygonframe = (Program.inhibitHatchBrushPolygonframe ? 1 : 0);
            //Program.drawTextSolidly = (Properties.Settings.Default.DrawTextSolidly > 0 ? true : false);
            Program.drawTextSolidly = true; //0.51.10 一時的措置
            Properties.Settings.Default.DrawTextSolidly = (Program.drawTextSolidly ? 1 : 0);

            
			//SetToolBarButton();
        }

  

        private void FileExit_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.Application.Exit();
            Close();
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
			if (QueryCloseCurrentWork("MBE")) {
                
				SaveWindowState();
				Properties.Settings.Default.Save();
				return;
			} else {
				e.Cancel = true; //trueを設定するとクローズをキャンセルできる。
			}
        }

		private void PanelClientSizeChanged(object sender, EventArgs e)
		{
			Size clientSize = mbeView.ClientSize;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			if (!mbeView.BoardFontReady) {
				MessageBox.Show("Initialization of font data isn't complete.", "MBE");
			}
		}

		private void OnMouseMoveView(object sender, MouseEventArgs e)
		{
			Point point = mbeView.CursorPos;
			double x = (double)point.X / 10000;
			double y = (double)point.Y / 10000;

			toolStripDimText.Text = String.Format("Abs({0,8:F4}mm,{1,8:F4}mm)", x, y);

			if (!mbeView.CursorGridCoodPos.Equals(point)) {
				point = mbeView.CursorGridCoodPos;
				x = (double)point.X / 10000;
				y = (double)point.Y / 10000;
				this.toolStripDimGridOriginText.Text = String.Format("Rel({0,8:F4}mm,{1,8:F4}mm)", x, y);
			} else {
				this.toolStripDimGridOriginText.Text = "";
			}

            this.toolStripSnapPointProperty.Text = mbeView.ComponentPinProperty;

			statusStrip1.Refresh();//これを入れないとすぐに更新されない

		}

		//Setメニュー
		//private void OnMenuSetGrid(object sender, EventArgs e)
		//{
		//    System.Diagnostics.Debug.WriteLine("OnMenuSetGrid");
		//}

		//protected const double ZOOM_STEP = 1.5F; 

		private void OnMenuViewZoomIn(object sender, EventArgs e)
		{
			mbeView.ZoomInOut(true);
		}

		private void OnMenuViewZoomOut(object sender, EventArgs e)
		{
			mbeView.ZoomInOut(false);
		}

		///// <summary>
		///// Viewメニューが開かれたときの処理
		///// </summary>
		///// <param name="sender"></param>
		///// <param name="e"></param>
		///// <remarks>
		///// zoom項目の有効無効を設定
		///// </remarks>
		//private void OnOpeningMenuView(object sender, EventArgs e)
		//{
		//    double scale = mbeView.DisplayScale;
		//    //System.Diagnostics.Debug.WriteLine("OnOpeningMenuView() "+ scale);
		//    this.MenuViewZoomOut.Enabled = (scale * 1.5F <= MbeView.MAX_DISPLAY_SCALE);
		//    this.MenuViewZoomIn.Enabled = (scale / 1.5F >= MbeView.MIN_DISPLAY_SCALE);
		//}

		/// <summary>
		/// Viewメニューのアイテムのイネーブルの設定
		/// </summary>
		/// <remarks>タイマーイベントで呼ばれる</remarks>
		private void SetMenuViewItemEnable()
		{
			double scale = mbeView.DisplayScale;
			//System.Diagnostics.Debug.WriteLine("OnOpeningMenuView() "+ scale);
			this.MenuViewZoomOut.Enabled = (scale * 1.5F <= MbeView.MAX_DISPLAY_SCALE);
			this.MenuViewZoomIn.Enabled = (scale / 1.5F >= MbeView.MIN_DISPLAY_SCALE);
		}

		/// <summary>
		/// SetGridメニューが開かれたときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>
		/// モードに対応したチェックをつける
		/// </remarks>
		private void OnOpeningMenuSetGrid(object sender, EventArgs e)
		{
			MbeView.ModeMajor major;
			int minor;//dummy
			mbeView.GetMode(out major, out minor);
			this.MenuSetGridOrigin.Checked = (major == MbeView.ModeMajor.SetGridOriginMode); 
		}

		private void OnMenuSetGridOrigin(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.SetGridOriginMode);
			//SetToolBarButton();
		}

		private void OnMenuSetResetGridOrigin(object sender, EventArgs e)
		{
			Point pt = new Point(0, 0);
			mbeView.SetMode(MbeView.ModeMajor.SelectorMode);
			mbeView.SetGridOrigin(pt);

		}

		private void OnMenuHelpAbout(object sender, EventArgs e)
		{
			//MessageBox.Show("MBE","About");
			AboutBoxForm dlg = new AboutBoxForm();
			dlg.ShowDialog();
		}

		private void OnMenuSetGridValue(object sender, EventArgs e)
		{
			SetGridForm dlg = new SetGridForm();

            dlg.CurrentGridInfo = mbeView.CurrentGridInfo;
            //dlg.MyStandardGrid = mbeView.MyStandardGrid;
            DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
                mbeView.SetCurrentGridInfo(dlg.CurrentGridInfo);

                DisplayGridValue();
			}

           
            //System.Diagnostics.Debug.WriteLine("OnMenuSetGridValue "+retv);
		}

        private void DisplayGridValue()
        {
            double x = (double)(mbeView.CurrentGridInfo.Horizontal) / 10000;
            double y = (double)(mbeView.CurrentGridInfo.Vertical) / 10000;
            toolStripGridVal.Text = String.Format("Grid[{0:#0.0###}mm,{1:#0.0###}mm]", x, y);

            statusStrip1.Refresh();//これを入れないとすぐに更新されない
        }


		/// <summary>
		/// mbeViewが選択レイヤー情報を変更したことをsetLayerWindowにセットする
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>
		/// MainFormか、MbeView自身がモードを変更すると、選択レイヤー
		/// 表示レイヤーが変わる。これをレイヤー設定ウィンドウに通知する。
		/// </remarks>
		private void OnLayerSelectChange(object sender, LayerInfoEventArgs e)
		{
			setLayerWindow.OnLayerSelectInfoChange(sender, e);
		}

		/// <summary>
		/// mbeView自身がモードを変更したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//private void OnViewChangeMode(object sender, EventArgs e)
		//{
		//    SetToolBarButton();
		//}

		private MbeView.ModeMajor tBarMajor;

        //private MbeLibs componentLibrary;

        //public static MbeLib toolMarkLib;


		/// <summary>
		/// mbeViewからモード情報を得て、ツールバーのボタンの状態を設定する。
		/// </summary>
		private void SetModeInfo()
		{
			MbeView.ModeMajor major;
			int minor;
			mbeView.GetMode(out major, out minor);
			//if (major == MbeView.ModeMajor.SelectorMode) {
			//    this.ContextMenuStrip = this.contextMenu;
			//} else {
			//    this.ContextMenuStrip = null;
			//}

			if (tBarMajor != major) {
				TBarSelector.Checked = (major == MbeView.ModeMajor.SelectorMode);
				TBarConChk.Checked = (major == MbeView.ModeMajor.ConChkMode);
                TBarRuler.Checked = (major == MbeView.ModeMajor.Measure);
				TBarHole.Checked = (major == MbeView.ModeMajor.PlaceHole);
				TBarPTH.Checked = (major == MbeView.ModeMajor.PlacePTH);
				TBarSMD.Checked = (major == MbeView.ModeMajor.PlacePad);
				TBarLine.Checked = (major == MbeView.ModeMajor.PlaceLine);
				TBarARC.Checked = (major == MbeView.ModeMajor.PlaceArc);
				TBarText.Checked = (major == MbeView.ModeMajor.PlaceText);
				TBarPolygon.Checked = (major == MbeView.ModeMajor.PlacePolygon);
                TBarComponent.Checked = (major == MbeView.ModeMajor.PlaceComponent);
				tBarMajor = major;
			}
		}


		private void OnOpenMenuTool(object sender, EventArgs e)
		{
			MbeView.ModeMajor major;
			int minor;
			mbeView.GetMode(out major, out minor);
			this.MenuToolSelector.Checked = (major == MbeView.ModeMajor.SelectorMode);
			this.MenuToolConnectionCheck.Checked = (major == MbeView.ModeMajor.ConChkMode);
            this.MenuToolRuler.Checked = (major == MbeView.ModeMajor.Measure);
			this.MenuToolHole.Checked = (major == MbeView.ModeMajor.PlaceHole);
			this.MenuToolPTH.Checked = (major == MbeView.ModeMajor.PlacePTH);
			this.MenuToolPad.Checked = (major == MbeView.ModeMajor.PlacePad);
			this.MenuToolLine.Checked = (major == MbeView.ModeMajor.PlaceLine);
			this.MenuToolPolygon.Checked = (major == MbeView.ModeMajor.PlacePolygon);
			this.MenuToolArc.Checked = (major == MbeView.ModeMajor.PlaceArc);
			this.MenuToolText.Checked = (major == MbeView.ModeMajor.PlaceText);
            this.MenuToolComponent.Checked = (major == MbeView.ModeMajor.PlaceComponent);
		}

		/// <summary>
		/// Toolメニューのセレクタを選択したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMenuToolSelector(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.SelectorMode);
			//SetToolBarButton();
		}

		/// <summary>
		/// ツールバーのセレクタをクリックしたときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTBarSelector(object sender, EventArgs e)
		{
			OnMenuToolSelector(sender, e);
		}

		/// <summary>
		/// ToolメニューのConChkを選択したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMenuToolConChk(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.ConChkMode);
		}

		/// <summary>
		/// ツールバーのConChkクリックしたときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTBarConChk(object sender, EventArgs e)
		{
			OnMenuToolConChk(sender, e);
		}


		/// <summary>
		/// ToolメニューのHoleを選択したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMenuToolHole(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.PlaceHole);
		}

		/// <summary>
		/// ツールバーのHoleをクリックしたときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTBarHole(object sender, EventArgs e)
		{
			OnMenuToolHole(sender, e);
		}


		/// <summary>
		/// ToolメニューのPadTHを選択したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMenuToolPTH(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.PlacePTH);
		}

		/// <summary>
		/// ツールバーのPinTHをクリックしたときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTBarPTH(object sender, EventArgs e)
		{
			OnMenuToolPTH(sender, e);
		}

		/// <summary>
		/// ToolメニューのPADを選択したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMenuToolPad(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.PlacePad);
		}

		/// <summary>
		/// ツールバーのPADをクリックしたときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTBarSMD(object sender, EventArgs e)
		{
			OnMenuToolPad(sender, e);
		}

		/// <summary>
		/// ToolメニューのLineを選択したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMenuToolLine(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.PlaceLine);
		}

		/// <summary>
		/// ツールバーのLineをクリックしたときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTBarLine(object sender, EventArgs e)
		{
			OnMenuToolLine(sender, e);
		}

		private void OnMenuToolArc(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.PlaceArc);
		}

		private void OnTBarArc(object sender, EventArgs e)
		{
			OnMenuToolArc(sender, e);
		}



		private void OnMenuToolText(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.PlaceText);
		}

		private void OnTBarText(object sender, EventArgs e)
		{
			OnMenuToolText(sender, e);
		}

		private void OnMenuSetColor(object sender, EventArgs e)
		{
			SetColorForm dlg = new SetColorForm();
			dlg.ColorChange += new SetColorForm.ColorChangeHandler(this.OnColorChange);
			dlg.ShowDialog();
		}

		private void OnMenuToolPolygon(object sender, EventArgs e)
		{
			mbeView.SetMode(MbeView.ModeMajor.PlacePolygon);
		}

		private void OnTBarPolygon(object sender, EventArgs e)
		{
			OnMenuToolPolygon(sender, e);
		}

        private void OnMenuToolComponent(object sender, EventArgs e)
        {
            //mbeView.mbeLibs = componentLibrary;
            mbeView.SetMode(MbeView.ModeMajor.PlaceComponent);
        }

        private void OnTBarComponent(object sender, EventArgs e)
        {
            OnMenuToolComponent(sender, e);
        }

		/// <summary>
		/// SetLayerWindowで選択レイヤーを変更したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSelectLayerChange(object sender, LayerInfoEventArgs e)
		{
			mbeView.SelectLayer = e.selectLayer;
			mbeView.SetVisibleLayer(e.visibleLayer);
		}

		/// <summary>
		/// SetLayerWindowで可視レイヤーを変更したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnVisibleLayerChange(object sender, LayerInfoEventArgs e)
		{
			mbeView.SetVisibleLayer(e.visibleLayer);
		}

		/// <summary>
		/// SetLayerWindowでレイヤー表示色を変更したときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnLayerColorChange(object sender, EventArgs e)
		{
			mbeView.RedrawAll();
		}

        //private void OnColorChange(object sender, EventArgs e)
		private void OnColorChange()
		{
			System.Diagnostics.Debug.WriteLine("OnColorChange");
			mbeView.RedrawAll();
			setLayerWindow.Invalidate();
		}



		/// <summary>
		/// タイトルバー文字列の設定
		/// </summary>
		protected void SetTitle()
		{
            string TitleText = Properties.Settings.Default.MainTitleText;
            if (TitleText.Length == 0) {
                TitleText = "mbe";
                Properties.Settings.Default.MainTitleText = TitleText;
            }

			if (mbeView.Document.DocumentName != null && mbeView.Document.DocumentName.Length > 0) {
                this.Text = mbeView.Document.DocumentName + " - " + TitleText;  // "mbe";
			} else {
                this.Text = TitleText;  // "mbe";
			}
		}



        private bool DocumentIsVoid()
        {
            mbeView.Document.ReleaseTemp();
            mbeView.RedrawAll();
            return (mbeView.Document.MainList.Count == 0);
        }


		/// <summary>
		/// ファイルオープンかFILE-NEWにおいて現在のワークのクリアを続行するか尋ねる
		/// </summary>
		/// <returns>現在のワークをクリアできるときはtrueを返す</returns>
		private bool QueryCloseCurrentWork(string msgTitle)
		{
			mbeView.Document.ReleaseTemp();
			mbeView.RedrawAll();
			if (!mbeView.Document.DocModified){
				return true;	// Modifiedでなければtrueを返す
			} else {
				string fname = mbeView.Document.DocumentName;
				if (fname.Length == 0) fname = "Untitled";
				//「保存しますか?」yes no cancel
				DialogResult dr = MessageBox.Show("Save changes to " + fname + "?", msgTitle ,
					MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
				if (dr == DialogResult.Cancel) {
					return false;	//実行しない
				} else if (dr == DialogResult.Yes) {
					bool saveResult;
					if (mbeView.Document.DocumentPath.Length == 0) {
						saveResult = DoFileSaveAs();
					} else {
						saveResult = DoFileSave();
					}
					if (!saveResult) return false;
					else return true;
				} else {
					return true;	//Noを選択。保存せずに続行
				}
			}

		}

		private void OnMenuFileNew(object sender, EventArgs e)
		{
            if (!DocumentIsVoid()){
                System.Diagnostics.Process.Start(Application.ExecutablePath);
            }else{
                mbeView.Document.ClearAll();
                SetTitle();
                mbeView.RedrawAll();
            }



            //if(QueryCloseCurrentWork("MBE Clear current document")){
            //    mbeView.Document.ClearAll();
            //    SetTitle();
            //    mbeView.RedrawAll();
            //}
		}

		/// <summary>
		/// ファイルオープンの実際
		/// </summary>
		private bool DoFileOpen(string path)
		{
            //0.48.04 拡張子がmb3でないときは何もせずに戻る
            string ext = Path.GetExtension(path);
            if (string.Compare(ext, ".mb3", true) != 0) return false;

            //0.48.04 現在のドキュメントが空でないときは、別ウィンドウで開くロジックをOnMenuFileOpen()から移動してきた
            if (!DocumentIsVoid()) {
                string argpath = "\"" + path + "\"";
                //MONO環境では、実行ファイルのパスの前に"mono "を付加する。
                string appPath;
                if (Program.monoRuntime) {
                    appPath = "mono " + Application.ExecutablePath;
                }else{
                    appPath = Application.ExecutablePath;
                }
                System.Diagnostics.Process.Start(appPath, argpath);
                return true;
            }

			if (mbeView.Document.FileOpen(path) != ReadCE3.RdStatus.NoError) {
				mbeView.Document.ClearAll();
			}
			SetTitle();
			//if (mbeView.Document.MainList.Count > 0) {
			mbeView.SetViewPolygonFrameOnly(true);
				mbeView.RedrawAll();
			//}
                return true;
		}

		/// <summary>
		/// ファイルオープン
		/// </summary>
		private void OnMenuFileOpen(object sender, EventArgs e)
		{
			//if (!QueryCloseCurrentWork("MBE Open document")) {
			//    return;
			//}

			OpenFileDialog openFileDialog = new OpenFileDialog();	
			openFileDialog.FileName = "*.mb3";
			openFileDialog.Filter = "MBE file (*.mb3)|*.mb3";
			openFileDialog.DefaultExt = "mb3";
			DialogResult dr= openFileDialog.ShowDialog();
			if (dr == DialogResult.OK) {
                //ドキュメントが空かどうかのチェックは、DoFileOpen()に移動 0.48.04
                //if (!DocumentIsVoid()) {
                //    string path = "\""+openFileDialog.FileName+"\"";
                //    System.Diagnostics.Process.Start(Application.ExecutablePath, path);
                //} else {
                //    string path = openFileDialog.FileName;
                //    DoFileOpen(path);
                //}
                string path = openFileDialog.FileName;
                DoFileOpen(path);



                //if (!QueryCloseCurrentWork("MBE Open document")) {
                //    return;
                //}
                //string path = openFileDialog.FileName;
                //DoFileOpen(path);
			}
		}

		/// <summary>
		/// ファイル保存に失敗したときのメッセージの表示
		/// </summary>
		/// <param name="fname"></param>
		private void ShowMsgFailToSave(string fname)
		{
			MessageBox.Show("Failed to save " + fname, "MBE",
				MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		/// <summary>
		/// 上書き保存の実際
		/// </summary>
		private bool DoFileSave()
		{
			//string fname = mbeView.Document.DocumentName;
			string path = mbeView.Document.DocumentPath;
			bool result;
			if (path.Length == 0) {
				result =  DoFileSaveAs();
			} else {
				if (!mbeView.Document.FileSave()) {
					string fname = Path.GetFileName(path);
					ShowMsgFailToSave(fname);
					result = false;
				}else{
					result = true;
					mbeView.Document.ClearAll();
					mbeView.Document.FileOpen(path);
					SetTitle();
				}
			}
			mbeView.SetViewPolygonFrameOnly(true);
			mbeView.RedrawAll();
			return result;
		}

		/// <summary>
		/// 上書き保存のハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMenuFileSave(object sender, EventArgs e)
		{
			DoFileSave();
		}

		/// <summary>
		/// 名前を付けて保存の実際
		/// </summary>
		/// <remarks>
		/// ファイルの保存をしたときはtrueを返す
		/// </remarks>
		private bool DoFileSaveAs()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			
			saveFileDialog.FileName = "*.mb3";
			saveFileDialog.Filter = "MBE file (*.mb3)|*.mb3";
			saveFileDialog.DefaultExt = "mb3";
			DialogResult dr= saveFileDialog.ShowDialog();
			if (dr == DialogResult.OK) {
				string path = saveFileDialog.FileName;
				if (!mbeView.Document.FileSaveAs(path)) {
					string fname;
					try {
						fname = Path.GetFileName(path);
					}
					catch (Exception) {
						fname = "Untitled";
					}
					ShowMsgFailToSave(fname);
					mbeView.RedrawAll();
					return false;
				}
				mbeView.Document.ClearAll();
				mbeView.Document.FileOpen(path);
				SetTitle();
				mbeView.SetViewPolygonFrameOnly(true);
				mbeView.RedrawAll(); 
				return true;
			}else{
				return false;
			}
		}

		private void OnMenuFileSaveAs(object sender, EventArgs e)
		{
			DoFileSaveAs();
		}


		/// <summary>
		/// Editメニューのアイテムのイネーブルの設定
		/// </summary>
		/// <remarks>
        /// Idleイベントで呼ばれる
        /// 
        /// 2010/06/27追記
        /// タイマーイベントで呼ばれるようにしたのはショートカットの有効無効が設定されないから
        /// 各メニュー項目のハンドラにガードがかかっているなら、メニューが閉じたときにはすべてイネーブルにすればいい?
        /// 
        /// </remarks>
		private void SetMenuEditItemEnable()
		{
			bool bCanCopy = mbeView.Document.CanCopy();
			this.MenuEditCopy.Enabled = bCanCopy;                                   //SC
			this.MenuEditCut.Enabled = bCanCopy;                                    //SC
			this.MenuEditDelete.Enabled = bCanCopy;                                 //SC
			this.MenuEditPaste.Enabled = mbeView.Document.CanPaste();               //SC
			this.MenuEditUndo.Enabled = mbeView.Document.CanUndo();                 //SC
			this.MenuEditRedo.Enabled = mbeView.Document.CanRedo();                 //SC
			//this.MenuEditProperty.Enabled = mbeView.Document.CanEditProperty();
            this.MenuEditProperty.Enabled = mbeView.CanEditProperty();
            this.MenuEditFlip.Enabled = bCanCopy;
			this.MenuEditRotate.Enabled = bCanCopy;                                 //SC
			this.MenuEditComponenting.Enabled = mbeView.Document.CanComponenting();
			this.MenuEditUncomponenting.Enabled = mbeView.Document.CanUncomponenting();
			this.MenuEditUpdateFillPolygon.Enabled = !MbeView.ViewPolygonFrame;
            this.MenuEditMove.Enabled = bCanCopy;                                   //SC

		}


		//private void OnOpeningMenuEdit(object sender, EventArgs e)
		//{
		//    SetMenuEditItemEnable();
		//    //bool bCanCopy = mbeView.Document.CanCopy();
		//    //this.MenuEditCopy.Enabled = bCanCopy;
		//    //this.MenuEditCut.Enabled = bCanCopy;
		//    //this.MenuEditDelete.Enabled = bCanCopy;
		//    //this.MenuEditPaste.Enabled = mbeView.Document.CanPaste();
		//    //this.MenuEditUndo.Enabled = mbeView.Document.CanUndo();
		//}

		private void OnMenuEditUndo(object sender, EventArgs e)
		{
            mbeView.Undo();
            //mbeView.Document.Undo();
            //mbeView.RedrawAll();
		}

		private void OnMenuEditCut(object sender, EventArgs e)
		{
			//if (mbeView.Document.Copy()) {
			//    mbeView.Document.Delete();
			//    mbeView.RedrawAll();
			//}
			mbeView.Cut();
		}

		private void OnMenuEditCopy(object sender, EventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("OnMenuEditCopy");
			//mbeView.Document.Copy();
            Point dummyPt;
			mbeView.Copy(out dummyPt);
		}

		private void OnMenuEditPaste(object sender, EventArgs e)
		{
			mbeView.Paste();
		}

		private void OnMenuEditDelete(object sender, EventArgs e)
		{
			mbeView.Document.Delete();
			mbeView.RedrawAll();
		}

		private void OnMenuEditRotate(object sender, EventArgs e)
		{
			mbeView.Document.Rotate(true);
			mbeView.Invalidate();
		}

		private void OnMenuEditFlip(object sender, EventArgs e)
		{
			mbeView.Flip();
		}

		private void OnMenuEditComponenting(object sender, EventArgs e)
		{
			mbeView.Componenting();
		}

		private void OnMenuEditUncomponenting(object sender, EventArgs e)
		{
			mbeView.UnComponenting();
		}

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    //System.Diagnostics.Debug.WriteLine("timer1_Tick");
        //    //SetMenuEditItemEnable();
        //    //SetModeInfo();
        //}

		private void OnMenuEditProperty(object sender, EventArgs e)
		{
			mbeView.EditProperty();	
		}

		//private void OnOpeningContextMenu(object sender, CancelEventArgs e)
		//{
		//    System.Diagnostics.Debug.WriteLine("OnOpeningContextMenu");
		//    propertyToolStripMenuItem.Enabled = mbeView.Document.CanEditProperty();
		//    addNodeToolStripMenuItem.Enabled = mbeView.CanDivideLine();
		//}

		//private void OnContextMenuProperty(object sender, EventArgs e)
		//{
		//    mbeView.EditProperty();	
		//}

		//private void OnContextMenuAddNode(object sender, EventArgs e)
		//{
		//    //System.Diagnostics.Debug.WriteLine("OnContextMenuAddNode");
		//    mbeView.AddNode();
		//}

		private void OnMenuSetBoardFont(object sender, EventArgs e)
		{
			SetBoardFontForm dlg = new SetBoardFontForm();
			dlg.useEmbeddedFont = Properties.Settings.Default.UseEmbeddedFont;
			dlg.fontPath = Properties.Settings.Default.PathBoardFont;
			DialogResult dr = dlg.ShowDialog();
			if (dr == DialogResult.OK) {
				Properties.Settings.Default.UseEmbeddedFont = dlg.useEmbeddedFont;
				Properties.Settings.Default.PathBoardFont = dlg.fontPath;
				if (!dlg.useEmbeddedFont) {
					if (MbeView.boardFont.ReadFontDataFile(dlg.fontPath)) {
						mbeView.RedrawAll();
						return;
					}
				}
				MbeView.boardFont.SetupEmbeddedFont();
				mbeView.RedrawAll();
			}
		}



		//private CanBulkPropResult lastCanBulkPropResult;

		private void OnMenuEditBulkPropOpen(object sender, EventArgs e)
		{
			CanBulkPropResult result = mbeView.Document.CanBulkProperty();
			this.MenuBulkPropHole.Enabled = result.CanPropHole;
			this.MenuBulkPropPTH.Enabled = result.CanPropPTH;
			this.MenuBulkPropPad.Enabled = result.CanPropPad;
			this.MenuBulkPropLine.Enabled = result.CanPropLine;
			this.MenuBulkPropText.Enabled = result.CanPropText;
			this.MenuBulkPropLayer.Enabled = result.CanMoveLayer;
            this.MenuBulkPropPolygon.Enabled = result.CanPropPolygon;
            mbeView.lastCanBulkPropResult = result;
		}

	

		private void OnBulkPropHole(object sender, EventArgs e)
		{
			mbeView.BulkPropHole();
		}

		private void OnBulkPropPTH(object sender, EventArgs e)
		{
			mbeView.BulkPropPTH();
		}

		private void OnBulkPropPad(object sender, EventArgs e)
		{
			mbeView.BulkPropPad();
		}

		private void OnBulkPropLine(object sender, EventArgs e)
		{
			mbeView.BulkPropLine();
		}

		private void OnBulkPropText(object sender, EventArgs e)
		{
			mbeView.BulkPropText();
		}

        private void OnBulkPropPolygon(object sender, EventArgs e)
        {
            mbeView.BulkPropPolygon();
        }

		private void OnBulkPropLayer(object sender, EventArgs e)
		{
			//MbeObjID id = (lastCanBulkPropResult.CanPropLine ? MbeObjID.MbeLine: MbeObjID.MbeText);
            mbeView.BulkPropLayer(mbeView.lastCanBulkPropResult);
            //mbeView.BulkPropLayer();
        }

		private void OnFilePrint(object sender, EventArgs e)
		{
			mbeView.Print();
		}

		private void OnFilePrintPreview(object sender, EventArgs e)
		{
			mbeView.PrintPreview();
		}

		private void OnMenuSetConvertFont(object sender, EventArgs e)
		{
			if (!mbeView.Document.CanConvertBoardFont()) return;

			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.FileName = Properties.Settings.Default.PathBoardFont;
			saveFileDialog.Filter = "MBE board font data (*.dat)|*.dat";
			saveFileDialog.DefaultExt = "dat";
			DialogResult dr = saveFileDialog.ShowDialog();
			if (dr == DialogResult.OK) {
				string path = saveFileDialog.FileName;
				mbeView.Document.ConvertBoardFont(path);
			}
		}

		private void OnOpenMenuSet(object sender, EventArgs e)
		{
			MenuSetConvertFont.Enabled = mbeView.Document.CanConvertBoardFont();
#if MONO
            MenuSetTextformatClipboard.Enabled = false;
#endif
            MenuSetTextformatClipboard.Checked = mbeView.Document.UseTextClipboard;
            //MenuSetPasteObjectAtCursorToolStripMenuItem.Checked = mbeView.PasteAtCurrsor; 
		}

		private void OnMenuFileGerber(object sender, EventArgs e)
		{
			if (DoFileSave()) {

                ExpGerberForm dlg = new ExpGerberForm();
                dlg.LayerMask = Properties.Settings.Default.GerberExportLayer;
                dlg.SelectedDir = Path.GetDirectoryName(mbeView.Document.DocumentPath);
                if (dlg.ShowDialog() != DialogResult.OK) {
                    return;
                }
                Properties.Settings.Default.GerberExportLayer = dlg.LayerMask;
                Properties.Settings.Default.Save();

                ulong exportLayerMask = dlg.LayerMask;
                if (exportLayerMask == 0) {
                    MessageBox.Show("No layers are selected",
                                "Export Gerber",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                    return;
                }


#if !MONO
                Cursor.Current = Cursors.WaitCursor;
#endif
				CamOutResult result = mbeView.Document.ExportGerber(dlg.SelectedDir,exportLayerMask);
				switch (result.code) {
					case CamOutResult.ResultCode.FILEERROR:
						{
							string msg = "File write error : " + result.filename;
							MessageBox.Show(msg,
										"Export Gerber", 
										MessageBoxButtons.OK,
										MessageBoxIcon.Stop);
						}
						break;
					case CamOutResult.ResultCode.TCODEOVER:
						MessageBox.Show("T-code exceeded 99.",
									"Export Gerber",
									MessageBoxButtons.OK,
									MessageBoxIcon.Stop);
						break;
					case CamOutResult.ResultCode.DCODEOVER:
						MessageBox.Show("D-code exceeded 999.",
									"Export Gerber",
									MessageBoxButtons.OK,
									MessageBoxIcon.Stop);
						break;
					default:
						MessageBox.Show("Finished Export Gerber.",
									"Export Gerber",
									MessageBoxButtons.OK,
									MessageBoxIcon.Information);
						break;

				}
				mbeView.RedrawAll();
			}
		}

		private void OnMenuFileNetlist(object sender, EventArgs e)
		{
			if (DoFileSave()) {
                ExportFilePathForm dlg = new  ExportFilePathForm();
                dlg.Extension = "net";
                dlg.PathName = Path.ChangeExtension(mbeView.Document.DocumentPath,"net");
                dlg.DlgTitle = "Export Netlist";
                dlg.FileType = "Netlist";
                dlg.FileDlgFilter = "Netlist (*.net)|*.net";

                if (dlg.ShowDialog() != DialogResult.OK) {
                    return;
                }
                
                
                if (mbeView.Document.ExportNetlist(dlg.PathName)) {
                    MessageBox.Show("Finished Export Netlist.",
                                        "Export Netlist",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                } else {
                    string msg = "File write error";// +result.filename;
                    MessageBox.Show(msg,
                                "Export Netlist",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                }
			}
		}

        private void OnMenuFileComponentPosition(object sender, EventArgs e)
        {
            if (DoFileSave()) {
                ExportFilePathForm dlg = new ExportFilePathForm();
                dlg.Extension = "csv";
                dlg.PathName = Path.ChangeExtension(mbeView.Document.DocumentPath, "csv");
                dlg.DlgTitle = "Export component position";
                dlg.FileType = "CSV file";
                dlg.FileDlgFilter = "Netlist (*.csv)|*.csv";

                if (dlg.ShowDialog() != DialogResult.OK) {
                    return;
                }



                if (mbeView.Document.ExportComponentPosition(dlg.PathName)) {
                    MessageBox.Show("Finished Export Component position list.",
                                    "Component position list",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                } else {
                    string msg = "File write error";// +result.filename;
                    MessageBox.Show(msg,
                                "Export Component position list",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                }
            }
        }

		private void OnMenuFileDrc(object sender, EventArgs e)
		{
			mbeView.Drc();
		}

		private void OnMenuViewDRC(object sender, EventArgs e)
		{
			mbeView.DisplayDrcMark = !mbeView.DisplayDrcMark;
			mbeView.RedrawAll();
		}

		private void OnMenuViewPolygonFrameMode(object sender, EventArgs e)
		{
#if !MONO
            Cursor.Current = Cursors.WaitCursor;
#endif
            mbeView.SetViewPolygonFrameOnly(!MbeView.ViewPolygonFrame);
			//mbeView.RedrawAll();
		}

		private void OnOpenMenuView(object sender, EventArgs e)
		{
			MenuViewDRC.Checked = mbeView.DisplayDrcMark;
			MenuViewPolygonFrameMode.Checked = MbeView.ViewPolygonFrame;
		}

		private void OnMenuEditUpdateFillPolygon(object sender, EventArgs e)
		{
#if !MONO
            Cursor.Current = Cursors.WaitCursor;
#endif
            mbeView.FillPolygon();
		}

		private void OnMenuEditRedo(object sender, EventArgs e)
		{
			mbeView.Document.Redo();
			mbeView.RedrawAll();
		}


		private void SaveWindowState()
		{
			FormWindowState ws = this.WindowState;
            if (ws == FormWindowState.Minimized) return;//最小化のときはウィンドウステートを保存しない
			Properties.Settings.Default.MainWindowState = (ws == FormWindowState.Maximized ? 1 : 0);
			Properties.Settings.Default.MainWindowWidth = Size.Width;
			Properties.Settings.Default.MainWindowHeight = Size.Height;

            System.Console.WriteLine("Saved Window size");
            System.Console.WriteLine("   Window state :" + Properties.Settings.Default.MainWindowState);
            System.Console.WriteLine("   Window width :" + Size.Width);
            System.Console.WriteLine("   Window height:" + Size.Height);
        }

		private void OnSizeChanged(object sender, EventArgs e)
		{
			//SaveWindowState();
		}

		private void OnFilePageSetup(object sender, EventArgs e)
		{
			SetupPageForm dlg = new SetupPageForm();
			dlg.PrintLeftMargin = mbeView.PrintLeftMargin;
			dlg.PrintBottomMargin = mbeView.PrintBottomMargin;
            dlg.Print2xMode = mbeView.Print2xMode;
            dlg.PrintCurrentView = Properties.Settings.Default.PrintCurrentView;
            dlg.CenterPunchMode = mbeView.CenterPunchMode;
            dlg.PrintToolMarkMode = mbeView.PrintToolMarkMode;
            dlg.PrintToolMarkModeEnable = (MbeView.toolMarkLib != null);
            dlg.PrintColorMode = mbeView.PrintColorMode;
            dlg.PrintHeader = mbeView.PrintHeader;
            dlg.HeaderText = mbeView.HeaderText;
            dlg.PrintMirror = mbeView.PrintMirrorMode;
            
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				mbeView.PrintLeftMargin = dlg.PrintLeftMargin;
				mbeView.PrintBottomMargin = dlg.PrintBottomMargin;
                mbeView.Print2xMode = dlg.Print2xMode;
                mbeView.PrintMirrorMode = dlg.PrintMirror;
                mbeView.CenterPunchMode = dlg.CenterPunchMode;
                mbeView.PrintToolMarkMode = dlg.PrintToolMarkMode;
                mbeView.PrintColorMode = dlg.PrintColorMode;
                mbeView.PrintHeader = dlg.PrintHeader;
                mbeView.HeaderText = dlg.HeaderText;

				Properties.Settings.Default.PrintLeftMargin = dlg.PrintLeftMargin;
				Properties.Settings.Default.PrintBottomMargin = dlg.PrintBottomMargin;
                Properties.Settings.Default.Print2xMode = dlg.Print2xMode;
                Properties.Settings.Default.PrintMirrorMode = dlg.PrintMirror;
                Properties.Settings.Default.CenterPunchMode = dlg.CenterPunchMode;
                Properties.Settings.Default.ToolMarkPrint = dlg.PrintToolMarkMode;
                Properties.Settings.Default.PrintColorMode = (uint)dlg.PrintColorMode;
                Properties.Settings.Default.PrintCurrentView = dlg.PrintCurrentView;
                Properties.Settings.Default.PrintHeader = dlg.PrintHeader;
                Properties.Settings.Default.HeaderText = dlg.HeaderText;
			}
		}

		private void OnMenuFileBitmap(object sender, EventArgs e)
		{
			mbeView.ExportImage();
		}

        private void OnMenuSetLibrary(object sender, EventArgs e)
        {
            SetLibForm dlg = new SetLibForm();
            if (dlg.ShowDialog() == DialogResult.OK) {
                //componentLibrary.LoadLibraries(dlg.LibPathNameArray);
                LoadLibraries();
            }

        }

        private void LoadLibraries()
        {
            string strMyStdInfo = Properties.Settings.Default.MyStandardLibString;
            MbeMyStd[] myStdInfoArray = MbeMyStd.LoadMyStdInfoArray(strMyStdInfo);
            int count = myStdInfoArray.Length;
            string[] pathNameArray = new string[count];
            for (int i = 0; i < count; i++) {
                pathNameArray[i] = ((LibInfo)myStdInfoArray[i]).LibPath;
            }


            mbeView.mbeLibs.LoadLibraries(pathNameArray);
            TBarComponent.Enabled = (mbeView.mbeLibs.LibCount > 0);

            //componentLibrary.LoadLibraries(pathNameArray);
            //TBarComponent.Enabled = (componentLibrary.LibCount > 0);

        }

        private void OnMenuSetToolMarkFile(object sender, EventArgs e)
        {
            string path = Properties.Settings.Default.ToolMarkFile;
            if ((path.Length > 0) && (!Path.IsPathRooted(path))) {
                string appPath = Application.ExecutablePath;
                appPath = Path.GetDirectoryName(appPath);
                path = Path.Combine(appPath, path);
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Path.GetDirectoryName(path);
            openFileDialog.FileName = Path.GetFileName(path);
            openFileDialog.Filter = "MBE data file (*.mb3)|*.mb3";
            openFileDialog.DefaultExt = "mb3";
            DialogResult dr = openFileDialog.ShowDialog();
            if (dr == DialogResult.OK) {
                Properties.Settings.Default.ToolMarkFile = openFileDialog.FileName;
                Properties.Settings.Default.Save();
                LoadToolMark();
            }
        }

        private bool LoadToolMark()
        {
            MbeView.toolMarkLib = null;
      
            string path = Properties.Settings.Default.ToolMarkFile;
            if ((path.Length > 0) && (!Path.IsPathRooted(path))) {
                string appPath = Application.ExecutablePath;
                appPath = Path.GetDirectoryName(appPath);
                path = Path.Combine(appPath, path);
            }

            if (path.Length == 0) return false;
            MbeView.toolMarkLib = new MbeLib();
            if (!MbeView.toolMarkLib.LoadLibrary(path)) {
                MbeView.toolMarkLib = null;
                return false;
            }
            return true;
        }

        //0.48.04 ドラッグ＆ドロップ
        //メインフォームのプロパティでAllowDropをtrueにすること
        //ドラッグしてきたら
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.All;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }
        //ドロップしたら
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            for (i = 0; i < s.Length; i++) {
                DoFileOpen(s[i]);
            }
        }

        private void OnMenuEditFind(object sender, EventArgs e)
        {
            mbeView.Find(this);
        }

        private void OnMenuEditMove(object sender, EventArgs e)
        {
            mbeView.MoveByValue();
        }

        private void MenuSetTextformatClipboard_Click(object sender, EventArgs e)
        {
#if MONO
            mbeView.Document.UseTextClipboard = true;
#else
            mbeView.Document.UseTextClipboard = !mbeView.Document.UseTextClipboard;
#endif
        }

        //private void MenuSetPasteObjectAtCursorToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    mbeView.PasteAtCurrsor = !mbeView.PasteAtCurrsor;

        //    Properties.Settings.Default.PasteObjectAtCursor = mbeView.PasteAtCurrsor;
        //    Properties.Settings.Default.Save();
        //}

        private void OnMenuToolRuler(object sender, EventArgs e)
        {
            mbeView.SetMode(MbeView.ModeMajor.Measure);
        }

        private void OnTBarRuler(object sender, EventArgs e)
        {
            OnMenuToolRuler(sender, e);
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ResizeEnd");
            SaveWindowState();
            Properties.Settings.Default.Save();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("MainForm_KeyDown " + e.KeyData);
        }

        
    }


}