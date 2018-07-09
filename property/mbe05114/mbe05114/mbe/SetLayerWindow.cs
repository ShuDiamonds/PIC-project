using System;
//using System.Collections.Generic;
//using System.IO;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace mbe
{
	/// <summary>
	/// レイヤー設定パネルクラス
	/// </summary>
	class SetLayerWindow : Panel
	{
		private const int MEMBER_HEIGHT= 16;
		private const int TEXT_HEIGHT  = 10;
		private const int TEXT_WIDTH = 35;	
		private const int COLOR_SAMPLE_WIDTH = 16;
		private const int ICON_COLUMN_WIDTH = 16;

		//private int iconColumnWidth = 20;
		//private int selectColumnWidth = 20;
		//private int leftPos = 0;

		private bool enablePaint;

		private int textX;
		private int colSampleX;
		private int visibleColumnX;
		private int selectColumnX;

		private ulong visibleLayer;
		private ulong selectableLayer;
		private MbeLayer.LayerValue selectLayer;

		protected Icon visibleIcon;
		protected Icon placeIcon;

		private ColorDialog colorDialog;

		/// <summary>
		/// 選択可能なレイヤーの設定と取得
		/// </summary>
		public ulong SelectableLayer
		{
			get{ return selectableLayer;}
			set{ selectableLayer = value;}
		}

		/// <summary>
		/// 可視レイヤーの設定と取得
		/// </summary>
		public ulong VisibleLayer
		{
			get{ return visibleLayer;}
			set{ visibleLayer = value;}
		}

		/// <summary>
		/// 選択レイヤーの設定と取得
		/// </summary>
		public MbeLayer.LayerValue SelectLayer
		{
			get{ return selectLayer;}
			set{ selectLayer = value;}
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SetLayerWindow()
		{
			InitializeComponent();
			visibleIcon = null;
			placeIcon = null;
			EnablePaint = false;

			//ダブルバッファリング設定
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

		}

		/// <summary>
		/// ペイントの許可
		/// </summary>
		public bool EnablePaint
		{
			set
			{
				enablePaint = value;
				Invalidate();
			}
			get	{return enablePaint;}
		}

		/// <summary>
		/// 表示レイヤー情報が変わったときの通知デリゲート
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void VisibleLayerChangeHandler(object sender, LayerInfoEventArgs e);

		/// <summary>
		/// 表示レイヤー情報が変わったときの通知ハンドラ
		/// </summary>
		public event VisibleLayerChangeHandler VisibleLayerChange;

		/// <summary>
		/// 選択レイヤー情報が変わったときの通知デリゲート
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void SelectLayerChangeHandler(object sender, LayerInfoEventArgs e);

		/// <summary>
		/// 選択レイヤー情報が変わったときの通知ハンドラ
		/// </summary>
		public event SelectLayerChangeHandler SelectLayerChange;

		/// <summary>
		/// 色設定が変わったときの通知デリゲート
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void LayerColorChangeHandler(object sender, EventArgs e);

		/// <summary>
		/// 選択レイヤー情報が変わったときの通知ハンドラ
		/// </summary>
		public event LayerColorChangeHandler LayerColorChange;


		/// <summary>
		/// レイヤー情報が変わったときに本クラスに通知するハンドラ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>
		/// mbeViewのモード(ツール)が変わったときのイベントハンドラとして準備したが
		/// イベントはいったんフレームウィンドウで受けるようにしているため、
		/// 普通のメソッドとして使っている。
		/// </remarks>
		public void OnLayerSelectInfoChange(object sender, LayerInfoEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("SetLayerWindow.OnLayerSelectInfoChange");
			SelectLayer = e.selectLayer;
			SelectableLayer = e.selectableLayer;
			VisibleLayer = e.visibleLayer;
			Invalidate();
		}

		/// <summary>
		/// クライアントサイズ変更ハンドラ
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>
		/// 今は各項目の横位置を設定するのに使っているだけ。
		/// 将来レイヤーが増えて、panelにスクロールバーが付くようになると、
		/// いろいろ調整が必要になるだろう。
		/// </remarks>
		protected override void OnClientSizeChanged(EventArgs e)
		{
			base.OnClientSizeChanged(e);
			int width = this.ClientSize.Width;
			textX = (width - (TEXT_WIDTH + COLOR_SAMPLE_WIDTH + ICON_COLUMN_WIDTH*2))/2;
			colSampleX = textX + TEXT_WIDTH;
			visibleColumnX = colSampleX + COLOR_SAMPLE_WIDTH;
			selectColumnX = visibleColumnX + ICON_COLUMN_WIDTH;
			Invalidate();
		}

		/// <summary>
		/// カラーサンプルがクリックされたときの処理
		/// </summary>
		/// <param name="index"></param>
		/// <remarks>
		/// カラーダイアログを呼び出し、変更があればイベントを発行する。
		/// </remarks>
		protected void ColorSampleClick(int index)
		{
			uint nCol = MbeColors.GetLayerColor(index);
			Color col = Color.FromArgb(unchecked((int)nCol));
			colorDialog.Color = col;
			if (colorDialog.ShowDialog() == DialogResult.OK) {
				nCol = unchecked((uint)(colorDialog.Color.ToArgb()));
				MbeColors.SetLayerColor(index, nCol);
				if (LayerColorChange != null) {
					LayerColorChange(this, null);
				}
				MbeColors.StoreSettings();
				Invalidate();
			}
		}

		/// <summary>
		/// 可視アイコンがクリックされたときの処理
		/// </summary>
		/// <param name="index"></param>
		/// <remarks>
		/// 変更が発生したらイベントを発行する。
		/// </remarks>
		protected void VisibleIconClick(int index)
		{
			MbeLayer.LayerValue layer = MbeLayer.valueTable[index];
			if ((visibleLayer & (ulong)layer) != 0) {
				//可視だったものがクリックされた場合
				if ((selectLayer == layer) && 
					((selectableLayer & (ulong)layer) != 0) )return;
				visibleLayer &= ~((ulong)layer);
			} else {
				//不可視だったものがクリックされた場合
				visibleLayer |= ((ulong)layer);
			}

			if (VisibleLayerChange != null) {
				LayerInfoEventArgs e = new LayerInfoEventArgs();
				e.visibleLayer = visibleLayer;
				VisibleLayerChange(this, e);
			}

			Invalidate();
		}

		/// <summary>
		/// 選択レイヤーがクリックされたときの処理
		/// </summary>
		/// <param name="index"></param>
		/// <remarks>
		/// 変更が発生したらイベントを発行する。
		/// </remarks>
		protected void SelectIconClick(int index)
		{
			MbeLayer.LayerValue layer = MbeLayer.valueTable[index];
			if (selectLayer == layer) return;
			if ((selectableLayer & (ulong)layer) == 0) return;
			selectLayer = layer;
			visibleLayer |= ((ulong)layer);

			if (SelectLayerChange != null) {
				LayerInfoEventArgs e = new LayerInfoEventArgs();
				e.selectLayer = selectLayer;
				e.visibleLayer = visibleLayer;
				SelectLayerChange(this, e);
			}

			Invalidate();
		}

		/// <summary>
		/// マウスクリック
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			int index = e.Y / MEMBER_HEIGHT;

			if (e.Button == MouseButtons.Left) {

				if (index >= MbeLayer.valueTable.Length) {
					return;
				}

				if ((e.X > colSampleX) && (e.X < colSampleX + COLOR_SAMPLE_WIDTH)) {
					ColorSampleClick(index);
					return;
				}


				if ((e.X > visibleColumnX) && (e.X < visibleColumnX + ICON_COLUMN_WIDTH)) {
					VisibleIconClick(index);
					return;
				}

				if ((e.X > selectColumnX) && (e.X < selectColumnX + ICON_COLUMN_WIDTH)) {
					SelectIconClick(index);
					return;
				}
			}
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (!enablePaint) return;

			Graphics g = e.Graphics;

			Font font = new Font(FontFamily.GenericSansSerif, TEXT_HEIGHT, 
								GraphicsUnit.Pixel);
			Brush brushDisable = new SolidBrush(Color.Gray);
			Brush brushText = new SolidBrush(Color.Black);
			Pen framePen = new Pen(Color.Black, 1);
			Rectangle rc;

			int layerCount = MbeLayer.valueTable.Length;

			for (int i = 0; i < layerCount; i++) {
				int y = MEMBER_HEIGHT * i;
				//レイヤー名の描画
				g.DrawString(MbeLayer.nameTable[i], font, brushText, new PointF(textX, y));

				//レイヤー色の描画
				uint nCol = MbeColors.GetLayerColor(i);
                if (MbeLayer.nameTable[i] == "L2/4") {
                    System.Diagnostics.Debug.WriteLine("L2/4 color " + nCol);
                }
				Color col = Color.FromArgb(unchecked((int)nCol));
				SolidBrush brushSample = new SolidBrush(col);
				rc = new Rectangle(colSampleX, y, COLOR_SAMPLE_WIDTH, MEMBER_HEIGHT);
				g.FillRectangle(brushSample, rc);
				brushSample.Dispose();
				g.DrawRectangle(framePen, rc);
				
				

				//可視アイコンの描画
				if((visibleLayer & (ulong)MbeLayer.valueTable[i])!=0){
					if(visibleIcon == null){
						visibleIcon = new Icon(Properties.Resources.visiblelayer,16,16);
					}
					g.DrawIcon(visibleIcon, visibleColumnX, y);
				}

				//選択アイコン背景(選択可能属性)の描画
				if ((selectableLayer & (ulong)MbeLayer.valueTable[i]) == 0) {
					rc = new Rectangle(selectColumnX, y, ICON_COLUMN_WIDTH, MEMBER_HEIGHT);
					g.FillRectangle(brushDisable, rc);
				}

				//選択アイコンの描画
				if((selectLayer == MbeLayer.valueTable[i])&&
				   (((ulong)selectLayer & selectableLayer) != 0)){
					if(placeIcon == null){
						placeIcon = new Icon(Properties.Resources.placelayer,16,16);
					}
					g.DrawIcon(placeIcon, selectColumnX, y);
				}
			}
			framePen.Dispose();
			brushText.Dispose();
			brushDisable.Dispose();
			font.Dispose();
		}

		private void InitializeComponent()
		{
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.SuspendLayout();
			this.ResumeLayout(false);

		}
	}
}
