using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mbe
{
	public partial class SetColorForm : Form
	{
		private const int TEXT_HEIGHT = 10;
		private const int COLOR_SAMPLE_WIDTH = 16;

		/// <summary>
		/// 色設定が変わったときの通知デリゲート
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//public delegate void ColorChangeHandler(object sender, EventArgs e);
        public delegate void ColorChangeHandler();

		/// <summary>
		/// 選択レイヤー情報が変わったときの通知ハンドラ
		/// </summary>
		public event ColorChangeHandler ColorChange;

		private static readonly string[] nameTable = 
		{ 
			"OriginMark",
			"GridOrigin",
			"Grid", 
			"SnapMark", 
			"ActivePointMark",//(ActiveSnapMark) 
			"PinNum",
			"Cursor",
			"BackGround",
			"BackGroundOutside",
			"InputError"
		};


		public SetColorForm()
		{
			ColorChange = null;
			InitializeComponent();
		}

		/// <summary>
		/// フォームがロードされたときのハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>リストボックスに文字列を登録</remarks>
		private void FormLoad(object sender, EventArgs e)
		{
			int layerCount = MbeLayer.valueTable.Length;
			for (int i = 0; i < layerCount; i++) {
				string str = "layer:" + MbeLayer.nameTable[i];
				listBoxColorDisplay.Items.Add(str);
			}

            for (int i = 0; i < layerCount; i++) {
                string str = "layer:" + MbeLayer.nameTable[i];
                listBoxColorPrint.Items.Add(str);
            }

            
            for(int i=0;i<nameTable.Length;i++){
				listBoxColorDisplay.Items.Add(nameTable[i]);
			}
		}

		/// <summary>
		/// オーナードロウリストボックスの描画
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index <= -1) return;

			int index = e.Index;

			//System.Diagnostics.Debug.WriteLine("OnDrawItem state=" + e.State);

			Brush brushText = new SolidBrush(e.ForeColor);
			e.DrawBackground();
			string str = (string)listBoxColorDisplay.Items[index];
			//Font font = new Font(FontFamily.GenericSansSerif, TEXT_HEIGHT, GraphicsUnit.Pixel);
			e.Graphics.DrawString(str, e.Font, brushText, e.Bounds.X + COLOR_SAMPLE_WIDTH, e.Bounds.Y + 1);
			//font.Dispose();
			e.DrawFocusRectangle();
			brushText.Dispose();

			uint nCol = 0;
			if (index < MbeLayer.valueTable.Length) {
				nCol = MbeColors.GetLayerColor(index);
			} else {
				int tabelIndex = index - MbeLayer.valueTable.Length;
				if (tabelIndex < nameTable.Length) {
					switch(tabelIndex){
						case 0: nCol = MbeColors.OriginMark; break;
						case 1: nCol = MbeColors.GridOriginMark; break;
						case 2: nCol = MbeColors.Grid; break;
						case 3: nCol = MbeColors.SnapPoint; break;
                        case 4: nCol = MbeColors.ActiveSnapPoint; break;
						case 5: nCol = MbeColors.PinNum; break;
						case 6: nCol = MbeColors.CrossCursor; break;
                        case 7: nCol = MbeColors.Background; break;
                        case 8: nCol = MbeColors.BgNotWorkArea; break;
						default: nCol = MbeColors.InputErr; break;

					}
				}
			}
			Color col = Color.FromArgb(unchecked((int)nCol));
			SolidBrush brushSample = new SolidBrush(col);
			Rectangle rc = new Rectangle(e.Bounds.X, e.Bounds.Y + 2, COLOR_SAMPLE_WIDTH, 12);
			e.Graphics.FillRectangle(brushSample, rc);
			brushSample.Dispose();
			Pen framePen = new Pen(Color.Black, 1);
			e.Graphics.DrawRectangle(framePen, rc);
			framePen.Dispose();

		}

        /// <summary>
        /// オーナードロウリストボックスの描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDrawItemPrintColor(object sender, DrawItemEventArgs e)
        {
            if (e.Index <= -1) return;

            int index = e.Index;

            //System.Diagnostics.Debug.WriteLine("OnDrawItem state=" + e.State);

            Brush brushText = new SolidBrush(e.ForeColor);
            e.DrawBackground();
            string str = (string)listBoxColorDisplay.Items[index];
            //Font font = new Font(FontFamily.GenericSansSerif, TEXT_HEIGHT, GraphicsUnit.Pixel);
            e.Graphics.DrawString(str, e.Font, brushText, e.Bounds.X + COLOR_SAMPLE_WIDTH, e.Bounds.Y + 1);
            //font.Dispose();
            e.DrawFocusRectangle();
            brushText.Dispose();

            uint nCol = 0;
            if (index < MbeLayer.valueTable.Length) {
                nCol = MbeColors.GetLayerPrintColor(index);
            } else {
                return;
            }
            Color col = Color.FromArgb(unchecked((int)nCol));
            SolidBrush brushSample = new SolidBrush(col);
            Rectangle rc = new Rectangle(e.Bounds.X, e.Bounds.Y + 2, COLOR_SAMPLE_WIDTH, 12);
            e.Graphics.FillRectangle(brushSample, rc);
            brushSample.Dispose();
            Pen framePen = new Pen(Color.Black, 1);
            e.Graphics.DrawRectangle(framePen, rc);
            framePen.Dispose();

        }




		/// <summary>
		/// リストボックスがクリックされたときのハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClickListBoxColor(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnClickListBoxColor index= "+listBoxColorDisplay.SelectedIndex);

			if(listBoxColorDisplay.SelectedIndex<=-1)return;

			int index = listBoxColorDisplay.SelectedIndex;	//レイヤーのインデックス
			int index2 = index - MbeLayer.valueTable.Length;	//レイヤー以外のインデックス

			uint nCol = 0;
			if (index < MbeLayer.valueTable.Length) {
				nCol = MbeColors.GetLayerColor(index);
			}else if(index2 < nameTable.Length) {
				switch(index2){
					case 0: nCol = MbeColors.OriginMark; break;
					case 1: nCol = MbeColors.GridOriginMark; break;
					case 2: nCol = MbeColors.Grid; break;
					case 3: nCol = MbeColors.SnapPoint; break;
                    case 4: nCol = MbeColors.ActiveSnapPoint; break;
                    case 5: nCol = MbeColors.PinNum; break;
					case 6: nCol = MbeColors.CrossCursor; break;
					case 7: nCol = MbeColors.Background; break;
                    case 8: nCol = MbeColors.BgNotWorkArea; break;
                    default: nCol = MbeColors.InputErr; break;
				}
			}else{
				return;
			}


			Color col = Color.FromArgb(unchecked((int)nCol));
			ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
			colorDialog.Color = col;
			if (colorDialog.ShowDialog() == DialogResult.OK) {
				nCol = unchecked((uint)(colorDialog.Color.ToArgb()));
			}else{
				return;
			}

			if (index < MbeLayer.valueTable.Length) {
				MbeColors.SetLayerColor(index, nCol);
			}else{
				switch(index2){
					case 0: MbeColors.OriginMark = nCol; break;
					case 1: MbeColors.GridOriginMark = nCol; break;
					case 2: MbeColors.Grid = nCol; break;
					case 3: MbeColors.SnapPoint = nCol; break;
                    case 4: MbeColors.ActiveSnapPoint = nCol; break;
                    case 5: MbeColors.PinNum = nCol; break;
					case 6: MbeColors.CrossCursor = nCol; break;
					case 7: MbeColors.Background = nCol; break;
                    case 8: MbeColors.BgNotWorkArea = nCol; break;
                    default: MbeColors.InputErr = nCol; break;
				}
			}
			if (ColorChange != null) {
			    //ColorChange(this, null);
                ColorChange();
            }
			MbeColors.StoreSettings();
			listBoxColorDisplay.Invalidate();
		}

		private void OnButtonClose(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void ObButtonDefault(object sender, EventArgs e)
		{
			DialogResult dr = MessageBox.Show("Set all display colors default.","MBE",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
			if (dr == DialogResult.OK) {
				MbeColors.SetDefaultColor();
				if (ColorChange != null) {
                    //ColorChange(this, null);
                    ColorChange();
				}
				MbeColors.StoreSettings();
				listBoxColorDisplay.Invalidate();
			}
		}

        /// <summary>
        /// 印刷用色リストボックスがクリックされたときのハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxColorPrint_Click(object sender, EventArgs e)
        {
            if (listBoxColorPrint.SelectedIndex <= -1) return;

            int index = listBoxColorPrint.SelectedIndex;	//レイヤーのインデックス

            uint nCol = 0;
            if (index < MbeLayer.valueTable.Length) {
                nCol = MbeColors.GetLayerPrintColor(index);
            } else {
                return;
            }


            Color col = Color.FromArgb(unchecked((int)nCol));
            ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.Color = col;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                nCol = unchecked((uint)(colorDialog.Color.ToArgb()));
            } else {
                return;
            }

            MbeColors.SetLayerPrintColor(index, nCol);

            MbeColors.StoreSettings();
            listBoxColorPrint.Invalidate();

        }

        private void buttonDefaultPrint_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Set all printing colors default.", "MBE",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.OK) {
                MbeColors.SetDefaultPrintColor();
                MbeColors.StoreSettings();
                listBoxColorPrint.Invalidate();
            }
        }
	}
}