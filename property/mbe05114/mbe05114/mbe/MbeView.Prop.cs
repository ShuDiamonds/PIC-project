using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace mbe
{
	partial class MbeView
	{
		private bool EditPropertyText(MbeObjText obj)
		{
			SetTextForm dlg = new SetTextForm();
			dlg.LineWidth = obj.LineWidth;
			dlg.TextHeight = obj.TextHeight;
			dlg.NameString = obj.SigName;
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.LineWidth = dlg.LineWidth;
				obj.TextHeight = dlg.TextHeight;
				obj.SigName = dlg.NameString;
				return true;
			}
			return false;
		}


		private bool EditPropertyArc(MbeObjArc obj)
		{
			SetArcForm dlg = new SetArcForm();
			dlg.LineWidth = obj.LineWidth;
			dlg.Radius = obj.Radius;
			dlg.StartAngle = obj.StartAngle;
			dlg.EndAngle = obj.EndAngle;
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.LineWidth = dlg.LineWidth;
				obj.Radius = dlg.Radius;
				obj.StartAngle = dlg.StartAngle;
				obj.EndAngle =dlg.EndAngle;
				obj.SetupPosition();
				return true;
			}
			return false;
		}



		private bool EditPropertyComponent(MbeObjComponent obj)
		{
			SetComponentForm dlg = new SetComponentForm();
			dlg.LineWidth = obj.RefNumLineWidth;
			dlg.TextHeight = obj.RefNumTextHeight;
            dlg.PackageName = obj.PackageName;
			dlg.RefNumString = obj.RefNumText;
			dlg.ComponentName = obj.SigName;
			dlg.RemarksText = obj.RemarksText;
			dlg.DrawRefOnDoc = obj.DrawRefOnDoc;
            dlg.Anglex10 = obj.AngleX10;
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.RefNumLineWidth = dlg.LineWidth;
				obj.RefNumTextHeight = dlg.TextHeight;
                obj.PackageName = dlg.PackageName;
				obj.RefNumText = dlg.RefNumString;
				obj.SigName = dlg.ComponentName;
				obj.RemarksText = dlg.RemarksText;
				obj.DrawRefOnDoc = dlg.DrawRefOnDoc;
                obj.AngleX10 = dlg.Anglex10;
				return true;
			}
			return false;
		}



		private bool BulkPropertyText(MbeObjText obj)
		{
			BulkPropTextForm dlg = new BulkPropTextForm();
			dlg.LineWidth = obj.LineWidth;
			dlg.TextHeight = obj.TextHeight;
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.LineWidth = dlg.LineWidth;
				obj.TextHeight = dlg.TextHeight;
				return true;
			}
			return false;
		}
		

		/// <summary>
		/// Lineのプロパティ編集ダイアログの起動
		/// </summary>
		/// <param name="obj">編集対象MbeObjLine</param>
		/// <returns></returns>
		private bool EditPropertyLine(MbeObjLine obj)
		{
			SetLineForm dlg = new SetLineForm();
			dlg.LineWidth = obj.LineWidth;
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.LineWidth = dlg.LineWidth;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Lineのプロパティ編集ダイアログの起動
		/// </summary>
		/// <param name="obj">編集対象MbeObjLine</param>
		/// <returns></returns>
		private bool EditPropertyLineStyle(MbeObjLine obj)
		{
			SetLinePropForm dlg = new SetLinePropForm();
			dlg.LineWidth = obj.LineWidth;
			dlg.LineStyle = obj.LineStyle;
            dlg.P0 = obj.GetPos(0);
            dlg.P1 = obj.GetPos(1);
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.LineWidth = dlg.LineWidth;
				obj.LineStyle = dlg.LineStyle;
				return true;
			}
			return false;
		}



		/// <summary>
		/// ポリゴンのプロパティ編集ダイアログの起動
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private bool EditPropertyPolygon(MbeObjPolygon obj)
		{
			SetPolygonForm dlg = new SetPolygonForm();
			dlg.PatternGap = obj.PatternGap;
			dlg.TraceWidth = obj.TraceWidth;
			dlg.Priority = obj.FillingPriority;
			dlg.RemoveFloating = (obj.RemoveFloating ? CheckState.Checked : CheckState.Unchecked);
            dlg.RestrictMask = (obj.RestrictMask ? CheckState.Checked : CheckState.Unchecked);
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.PatternGap = dlg.PatternGap;
				obj.TraceWidth = dlg.TraceWidth;
				obj.FillingPriority = dlg.Priority;
				obj.RemoveFloating = (dlg.RemoveFloating == CheckState.Checked);
                obj.RestrictMask = (dlg.RestrictMask == CheckState.Checked);
				return true;
			}
			return false;
		}



		/// <summary>
		/// Holeのプロパティ編集ダイアログの起動
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private bool EditPropertyHole(MbeObjHole obj)
		{
			SetHoleForm dlg = new SetHoleForm();
			dlg.Drill = obj.Diameter;
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.Diameter = dlg.Drill;
				return true;
			}
			return false;
		}

		/// <summary>
		/// SMD Padのプロパティ編集ダイアログの起動
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="editPinNumberMode"></param>
		/// <returns></returns>
		private bool EditPropertySMDPad(MbeObjPinSMD obj, bool editPinNumberMode)
		{
            if (obj.Layer == MbeLayer.LayerValue.PLC || obj.Layer == MbeLayer.LayerValue.PLS) {
                editPinNumberMode = false;
            }

			SetSMDPadForm dlg = new SetSMDPadForm();
			dlg.LandHeight = obj.PadSize.Height;
			dlg.LandWidth = obj.PadSize.Width;
			dlg.Shape = obj.Shape;
            dlg.No_metalMask = obj.No_MM;
            dlg.No_resistMask = obj.No_ResistMask;
            if (editPinNumberMode) {
				dlg.PinNumber = obj.PinNum;
				dlg.EditPinNumberMode = true;
                dlg.ThermalReliefSetting = obj.ThermalRelief;
            }
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.PadSize = new Size(dlg.LandWidth,dlg.LandHeight);
				obj.Shape = dlg.Shape;
                obj.ThermalRelief = dlg.ThermalReliefSetting;
                obj.No_MM = dlg.No_metalMask;
                obj.No_ResistMask = dlg.No_resistMask;
                if (editPinNumberMode) {
					obj.PinNum = dlg.PinNumber;
				}
				return true;
			} else {
				return false;
			}
		}


		/// <summary>
		/// PinTHのプロパティ編集ダイアログの起動
		/// </summary>
		/// <param name="obj">編集対象PinTHオブジェクト</param>
		/// <param name="editPinNumberMode">ピン番号文字列も編集するときtrue</param>
		/// <returns></returns>
		private bool EditPropertyPTH(MbeObjPTH obj, bool editPinNumberMode)
		{
			SetPTHForm dlg = new SetPTHForm();
			dlg.LandHeight = obj.PadSize.Height;
			dlg.LandWidth = obj.PadSize.Width;
			dlg.Drill = obj.Diameter;
			dlg.Shape = obj.Shape;
            dlg.No_ResistMask = obj.No_ResistMask;
            dlg.ThermalReliefSetting = obj.ThermalRelief;
            dlg.EditPinNumberMode = true;
            if (editPinNumberMode) {
				dlg.PinNumber = obj.PinNum;
			}
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				obj.PadSize = new Size(dlg.LandWidth,dlg.LandHeight);
				obj.Diameter = dlg.Drill;
				obj.Shape = dlg.Shape;
                obj.ThermalRelief = dlg.ThermalReliefSetting;
                obj.No_ResistMask = dlg.No_ResistMask;
                if (editPinNumberMode) {
					obj.PinNum = dlg.PinNumber;
				}
				return true;
			} else {
				return false;
			}
		}

        private bool BulkLayerMove(ref MbeLayer.LayerValue layerValue, ulong moveSelectableLayer)
		{
			BulkMoveLayerForm dlg = new BulkMoveLayerForm();
			dlg.layer = layerValue;
            dlg.selectableLayer = moveSelectableLayer;
			DialogResult retv = dlg.ShowDialog();
			if (retv == DialogResult.OK) {
				layerValue = dlg.layer;
				return true;
			}
			return false;
		}


	}
}
