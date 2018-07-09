using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace mbe
{
	class MbeConChk
	{

		public MbeConChk()
		{
			chkPointList = new LinkedList<MbeConChkElm>();
		}

		public void Clear()
		{
			chkPointList.Clear();
		}


		public bool ScanData(MbeObj startObj, LinkedList<MbeObj> objList)
		{
			startObj.SetConnectCheck();
			AddObjPoint(startObj);

			LinkedListNode<MbeConChkElm> node = chkPointList.First;
			while (node != null) {
				scanObj(node.Value, objList);
				node = node.Next;
			}
			return true;
		}

		public bool ScanData(Point startPoint, ulong _layerMask, LinkedList<MbeObj> objList)
		{
			Point ptLT = new Point(startPoint.X - 1, startPoint.Y + 1);
			Point ptRB = new Point(startPoint.X + 1, startPoint.Y - 1);
			MbeRect rc = new MbeRect(ptLT, ptRB);

			MbeObj startObj = null;

            ulong layerMask = _layerMask & (    (ulong)MbeLayer.LayerValue.PTH |
                                                (ulong)MbeLayer.LayerValue.CMP |
                                                (ulong)MbeLayer.LayerValue.L2 |
                                                (ulong)MbeLayer.LayerValue.L3 |
                                                (ulong)MbeLayer.LayerValue.SOL);



			foreach (MbeObj obj in objList) {
				if (obj.DeleteCount >= 0) continue;
				startObj = obj.ConChkSeed(rc, layerMask);
				if (startObj != null) break;
			}
			if (startObj == null) return false;

			return ScanData(startObj, objList);
		}


		public bool ScanDataConnectPoint(Point connectPoint, ulong layerMask, LinkedList<MbeObj> objList)
		{
			//Point ptLT = new Point(connectPoint.X - 1, connectPoint.Y + 1);
			//Point ptRB = new Point(connectPoint.X + 1, connectPoint.Y - 1);
			//MbeRect rc = new MbeRect(ptLT, ptRB);

			//MbeObj startObj = null;

			//foreach (MbeObj obj in objList) {
			//    if (obj.DeleteCount >= 0) continue;
			//    startObj = obj.ConChkSeed(rc, layerMask);
			//    if (startObj != null) break;
			//}
			//if (startObj == null) return false;

			//return ScanData(startObj, objList);

			//startObj.SetConnectCheck();
			//AddObjPoint(startObj);

			AddPoint(connectPoint, layerMask);

			LinkedListNode<MbeConChkElm> node = chkPointList.First;
            bool retval = false;
			while (node != null) {
                if (scanObj(node.Value, objList)) {
                    retval = true;
                }
				node = node.Next;
			}

			return retval;
		}



		/// <summary>
		/// objがchkPointと接続されているかの判定を行う
		/// </summary>
		/// <param name="chkPoint"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected bool testObj(MbeConChkElm chkPoint,MbeObj obj)
		{
			if (obj.DeleteCount >= 0) return false;
			if(obj.ConnectionCheckActive) return false;
			
			if(obj.Id() == MbeObjID.MbePTH){
				if(obj.GetPos(0).Equals(chkPoint.pt)){
					obj.SetConnectCheck();
					AddObjPoint(obj);	//反対面にchkPointの追加
					return true;
				}
			}else if(obj.Id() == MbeObjID.MbePinSMD){
				if((chkPoint.layer == (ulong)obj.Layer)&&obj.GetPos(0).Equals(chkPoint.pt)){
					obj.SetConnectCheck();
					return true;
				}
			}else if(obj.Id() == MbeObjID.MbePolygon){
				if((chkPoint.layer == (ulong)obj.Layer)&&obj.GetPos(0).Equals(chkPoint.pt)){
					obj.SetConnectCheck();
					return true;
				}
            } else if (obj.Id() == MbeObjID.MbeArc) {
                if (chkPoint.layer != (ulong)obj.Layer) return false;
                if (obj.GetPos(1).Equals(chkPoint.pt)) {
                    obj.SetConnectCheck();
                    AddPoint(obj.GetPos(2), (ulong)obj.Layer);
                }
                if (obj.GetPos(2).Equals(chkPoint.pt)) {
                    obj.SetConnectCheck();
                    AddPoint(obj.GetPos(1), (ulong)obj.Layer);
                }
                return obj.ConnectionCheckActive;
            } else if (obj.Id() == MbeObjID.MbeLine) {
				if(chkPoint.layer != (ulong)obj.Layer) return false;
				if(obj.GetPos(0).Equals(chkPoint.pt)){
					obj.SetConnectCheck();
					AddPoint(obj.GetPos(1), (ulong)obj.Layer);
				}
				if(obj.GetPos(1).Equals(chkPoint.pt)){
					obj.SetConnectCheck();
					AddPoint(obj.GetPos(0), (ulong)obj.Layer);
				}
				return obj.ConnectionCheckActive;
			}else if(obj.Id() == MbeObjID.MbeComponent){
				bool result = false;
				foreach(MbeObj objContent in ((MbeObjComponent)obj).ContentsObj){
					result |= testObj(chkPoint, objContent);	//再帰呼び出し
				}
				return result;
			}
			return false;
		}

		protected bool scanObj(MbeConChkElm chkPoint, LinkedList<MbeObj> objList)
		{
			bool result = false;
			foreach (MbeObj obj in objList) {
				if (obj.DeleteCount >= 0) continue;
				result |= testObj(chkPoint, obj);
			}
			chkPoint.scanEnd = true;
			return result;
		}

		/// <summary>
		/// オブジェクトの点を追加する
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected bool AddObjPoint(MbeObj obj)
		{
			if (obj.DeleteCount >= 0) return false;
			if (((ulong)obj.Layer & ((ulong)MbeLayer.LayerValue.CMP |
						  (ulong)MbeLayer.LayerValue.L2  |
                          (ulong)MbeLayer.LayerValue.L3  |
                          (ulong)MbeLayer.LayerValue.SOL |
                          (ulong)MbeLayer.LayerValue.PTH)) == 0) return false;

			if (obj.Id() == MbeObjID.MbePTH || 
				obj.Id() == MbeObjID.MbePinSMD ||
				obj.Id() == MbeObjID.MbePolygon) {
				return AddPoint(obj.GetPos(0), (ulong)obj.Layer);
			}
			if (obj.Id() == MbeObjID.MbeLine) {
				bool result = AddPoint(obj.GetPos(0), (ulong)obj.Layer);
				result |= AddPoint(obj.GetPos(1), (ulong)obj.Layer);
				return result;
			}
            if (obj.Id() == MbeObjID.MbeArc) {
                bool result = AddPoint(obj.GetPos(1), (ulong)obj.Layer);
                result |= AddPoint(obj.GetPos(2), (ulong)obj.Layer);
                return result;
            }

			return false;
		}

		/// <summary>
		/// 点の追加
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		protected bool AddPoint(Point pt,ulong layer)
		{
			if (layer == (ulong)MbeLayer.LayerValue.PTH) {
				layer = (ulong)MbeLayer.LayerValue.CMP | 
                        (ulong)MbeLayer.LayerValue.L2 | 
                        (ulong)MbeLayer.LayerValue.L3 | 
                        (ulong)MbeLayer.LayerValue.SOL;
			}

			bool appendCMP = ((layer & (ulong)MbeLayer.LayerValue.CMP)!=0);
			bool appendL2  = ((layer & (ulong)MbeLayer.LayerValue.L2) !=0);
            bool appendL3  = ((layer & (ulong)MbeLayer.LayerValue.L3) != 0);
            bool appendSOL = ((layer & (ulong)MbeLayer.LayerValue.SOL)!= 0);

			foreach(MbeConChkElm elm in chkPointList){
				if(elm.pt.Equals(pt)){//位置が同じ点が登録済み
                    if (elm.layer == (ulong)MbeLayer.LayerValue.CMP) appendCMP = false;
                    if (elm.layer == (ulong)MbeLayer.LayerValue.L2)  appendL2 = false;
                    if (elm.layer == (ulong)MbeLayer.LayerValue.L3)  appendL3 = false;
					if (elm.layer == (ulong)MbeLayer.LayerValue.SOL) appendSOL = false;
				}
                if (!appendCMP && !appendL2 && !appendL3 && !appendSOL) return false;
			}
			if (appendCMP) {
				MbeConChkElm newelm = new MbeConChkElm();
				newelm.layer = (ulong)MbeLayer.LayerValue.CMP;
				newelm.pt = pt;
				chkPointList.AddLast(newelm);
			}
            if (appendL2) {
                MbeConChkElm newelm = new MbeConChkElm();
                newelm.layer = (ulong)MbeLayer.LayerValue.L2;
                newelm.pt = pt;
                chkPointList.AddLast(newelm);
            }
            if (appendL3) {
                MbeConChkElm newelm = new MbeConChkElm();
                newelm.layer = (ulong)MbeLayer.LayerValue.L3;
                newelm.pt = pt;
                chkPointList.AddLast(newelm);
            }
            if (appendSOL) {
				MbeConChkElm newelm = new MbeConChkElm();
				newelm.layer = (ulong)MbeLayer.LayerValue.SOL;
				newelm.pt = pt;
				chkPointList.AddLast(newelm);
			}
			return true;
		}

		protected LinkedList<MbeConChkElm> chkPointList;
	}

	class MbeConChkElm
	{
		public bool scanEnd;
		public Point pt;
		public ulong layer;

		public MbeConChkElm()
		{
			scanEnd = false;
			pt = new Point(0, 0);
			layer = 0;
		}
	}
}
