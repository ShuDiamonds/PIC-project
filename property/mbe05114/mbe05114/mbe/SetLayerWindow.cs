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
	/// ���C���[�ݒ�p�l���N���X
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
		/// �I���\�ȃ��C���[�̐ݒ�Ǝ擾
		/// </summary>
		public ulong SelectableLayer
		{
			get{ return selectableLayer;}
			set{ selectableLayer = value;}
		}

		/// <summary>
		/// �����C���[�̐ݒ�Ǝ擾
		/// </summary>
		public ulong VisibleLayer
		{
			get{ return visibleLayer;}
			set{ visibleLayer = value;}
		}

		/// <summary>
		/// �I�����C���[�̐ݒ�Ǝ擾
		/// </summary>
		public MbeLayer.LayerValue SelectLayer
		{
			get{ return selectLayer;}
			set{ selectLayer = value;}
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public SetLayerWindow()
		{
			InitializeComponent();
			visibleIcon = null;
			placeIcon = null;
			EnablePaint = false;

			//�_�u���o�b�t�@�����O�ݒ�
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

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
			get	{return enablePaint;}
		}

		/// <summary>
		/// �\�����C���[��񂪕ς�����Ƃ��̒ʒm�f���Q�[�g
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void VisibleLayerChangeHandler(object sender, LayerInfoEventArgs e);

		/// <summary>
		/// �\�����C���[��񂪕ς�����Ƃ��̒ʒm�n���h��
		/// </summary>
		public event VisibleLayerChangeHandler VisibleLayerChange;

		/// <summary>
		/// �I�����C���[��񂪕ς�����Ƃ��̒ʒm�f���Q�[�g
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void SelectLayerChangeHandler(object sender, LayerInfoEventArgs e);

		/// <summary>
		/// �I�����C���[��񂪕ς�����Ƃ��̒ʒm�n���h��
		/// </summary>
		public event SelectLayerChangeHandler SelectLayerChange;

		/// <summary>
		/// �F�ݒ肪�ς�����Ƃ��̒ʒm�f���Q�[�g
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void LayerColorChangeHandler(object sender, EventArgs e);

		/// <summary>
		/// �I�����C���[��񂪕ς�����Ƃ��̒ʒm�n���h��
		/// </summary>
		public event LayerColorChangeHandler LayerColorChange;


		/// <summary>
		/// ���C���[��񂪕ς�����Ƃ��ɖ{�N���X�ɒʒm����n���h��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>
		/// mbeView�̃��[�h(�c�[��)���ς�����Ƃ��̃C�x���g�n���h���Ƃ��ď���������
		/// �C�x���g�͂�������t���[���E�B���h�E�Ŏ󂯂�悤�ɂ��Ă��邽�߁A
		/// ���ʂ̃��\�b�h�Ƃ��Ďg���Ă���B
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
		/// �N���C�A���g�T�C�Y�ύX�n���h��
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>
		/// ���͊e���ڂ̉��ʒu��ݒ肷��̂Ɏg���Ă��邾���B
		/// �������C���[�������āApanel�ɃX�N���[���o�[���t���悤�ɂȂ�ƁA
		/// ���낢�뒲�����K�v�ɂȂ邾�낤�B
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
		/// �J���[�T���v�����N���b�N���ꂽ�Ƃ��̏���
		/// </summary>
		/// <param name="index"></param>
		/// <remarks>
		/// �J���[�_�C�A���O���Ăяo���A�ύX������΃C�x���g�𔭍s����B
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
		/// ���A�C�R�����N���b�N���ꂽ�Ƃ��̏���
		/// </summary>
		/// <param name="index"></param>
		/// <remarks>
		/// �ύX������������C�x���g�𔭍s����B
		/// </remarks>
		protected void VisibleIconClick(int index)
		{
			MbeLayer.LayerValue layer = MbeLayer.valueTable[index];
			if ((visibleLayer & (ulong)layer) != 0) {
				//�����������̂��N���b�N���ꂽ�ꍇ
				if ((selectLayer == layer) && 
					((selectableLayer & (ulong)layer) != 0) )return;
				visibleLayer &= ~((ulong)layer);
			} else {
				//�s�����������̂��N���b�N���ꂽ�ꍇ
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
		/// �I�����C���[���N���b�N���ꂽ�Ƃ��̏���
		/// </summary>
		/// <param name="index"></param>
		/// <remarks>
		/// �ύX������������C�x���g�𔭍s����B
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
		/// �}�E�X�N���b�N
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
				//���C���[���̕`��
				g.DrawString(MbeLayer.nameTable[i], font, brushText, new PointF(textX, y));

				//���C���[�F�̕`��
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
				
				

				//���A�C�R���̕`��
				if((visibleLayer & (ulong)MbeLayer.valueTable[i])!=0){
					if(visibleIcon == null){
						visibleIcon = new Icon(Properties.Resources.visiblelayer,16,16);
					}
					g.DrawIcon(visibleIcon, visibleColumnX, y);
				}

				//�I���A�C�R���w�i(�I���\����)�̕`��
				if ((selectableLayer & (ulong)MbeLayer.valueTable[i]) == 0) {
					rc = new Rectangle(selectColumnX, y, ICON_COLUMN_WIDTH, MEMBER_HEIGHT);
					g.FillRectangle(brushDisable, rc);
				}

				//�I���A�C�R���̕`��
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
