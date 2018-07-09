using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace mbe
{
	class MbeNetOut
	{
		public MbeNetOut()
		{
			workList = new LinkedList<MbeObj>();
            componentList = new List<MbeObjComponent>();
			conChk = new MbeConChk();

		}

		/// <summary>
		/// アレグロフォーマットで出力する。
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public bool DoExport(string filename, LinkedList<MbeObj> mbeList)
		{

			SetMbeObjData(mbeList);

			LinkedList<MbeObj> objList = new LinkedList<MbeObj>();

			int netNum = 0;
			bool result = true;

			StreamWriter streamWriter = null;
			try {
                if (Program.listFileUseLocalEncoding) {
                    streamWriter = new StreamWriter(filename, false, Encoding.Default);
                } else {
                    streamWriter = new StreamWriter(filename);
                }

                streamWriter.WriteLine("$PACKAGES");
                foreach(MbeObjComponent obj in componentList){
                    streamWriter.WriteLine(obj.PackageName+"! "+obj.SigName+"; "+obj.RefNumText);
                }


				streamWriter.WriteLine("$NETS");
				while (GetNet(objList, true)) {
					if (objList.Count < 2) continue;
					netNum++;
					string netName = String.Format("N{0:00000};  ", netNum);
					streamWriter.Write(netName);
					int nodeCount = 0;
					foreach(MbeObj obj in objList){
						if (nodeCount > 0 && nodeCount % 5 == 0) {
							streamWriter.WriteLine(" ,");
							streamWriter.Write("    ");
						}
						streamWriter.Write(" "+obj.TempPropString + "."+((MbeObjPin)obj).PinNum);
						nodeCount++;
					}
					streamWriter.WriteLine();
				}
				streamWriter.WriteLine("$END");
			}
			catch (Exception) {
				result = false;
			}
			finally {
				if (streamWriter != null) {
					streamWriter.Close();
				}
			}
			CleanUp();
			return result;
		}


		/// <summary>
		/// ネットリスト作成中に変更した図面要素のメンバーをクリアする
		/// </summary>
		public void CleanUp()
		{
			if (workList.Count > 0) {
				foreach (MbeObj obj in workList) {
					obj.ClearConnectCheck();
					obj.TempPropString = "";
					obj.ClearDeleteCount();
				}
				workList.Clear();
			}
            componentList.Clear();
		}

		/// <summary>
		/// ネットリストに必要な図面要素だけを抽出して workList に入れる
		/// </summary>
		/// <param name="mbeList"></param>
		/// <returns></returns>
		public bool SetMbeObjData(LinkedList<MbeObj> mbeList)
		{
			CleanUp();
			foreach (MbeObj obj in mbeList) {
				if (obj.DeleteCount >= 0) continue;

				if (obj.Id() == MbeObjID.MbeLine ||
					obj.Id() == MbeObjID.MbeArc ||
                    obj.Id() == MbeObjID.MbePTH ||
                    obj.Id() == MbeObjID.MbePinSMD) {
					if (obj.Layer == MbeLayer.LayerValue.CMP ||
                        obj.Layer == MbeLayer.LayerValue.L2 ||
                        obj.Layer == MbeLayer.LayerValue.L3 ||
						obj.Layer == MbeLayer.LayerValue.SOL ||
						obj.Layer == MbeLayer.LayerValue.PTH) {
						workList.AddLast(obj);
					}
					continue;
				}
				
				if (obj.Id() == MbeObjID.MbeComponent) {

                    componentList.Add((MbeObjComponent)obj);

					foreach (MbeObj objContent in ((MbeObjComponent)obj).ContentsObj) {
						if (objContent.Layer != MbeLayer.LayerValue.CMP &&
                            objContent.Layer != MbeLayer.LayerValue.L2 &&
                            objContent.Layer != MbeLayer.LayerValue.L3 &&
							objContent.Layer != MbeLayer.LayerValue.SOL &&
							objContent.Layer != MbeLayer.LayerValue.PTH) continue;
						if (objContent.Id() == MbeObjID.MbeLine ||
                            objContent.Id() == MbeObjID.MbeArc) {
							workList.AddLast(objContent);
						} else if (objContent.Id() == MbeObjID.MbePTH ||
								  objContent.Id() == MbeObjID.MbePinSMD) {
							objContent.TempPropString = ((MbeObjComponent)obj).RefNumText;
							workList.AddLast(objContent);
						}
					}
					continue;
				}
			}


            ComponentRefComparer crcmpr = new ComponentRefComparer();
            componentList.Sort(crcmpr);


			return workList.Count > 0;
		}


		/// <summary>
		/// workListからひとつのネット情報をobjListNetに取り出す
		/// </summary>
		/// <param name="objListNet"></param>
		/// <param name="pinOnly">ネット出力リストにピン情報だけを返すときtrue</param>
		/// <returns>ひとつもノードが見つからなかった場合はfalseを返す</returns>
		public bool GetNet(LinkedList<MbeObj> objListNet,bool pinOnly)
		{
			//ひとつのネットの起点になるピンを選び出す
			objListNet.Clear();
			MbeObj obj = null;
			foreach (MbeObj _obj in workList) {
				//すでにConnectionCheckActiveフラグがあるものは、ネットとして抽出済み
				if (_obj.ConnectionCheckActive) continue;

				if (_obj.Id() == MbeObjID.MbePTH ||
					_obj.Id() == MbeObjID.MbePinSMD) {
					//if (_obj.TempPropString.Length > 0) {
						obj = _obj;
						break;
					//}
				}
			}
			if(obj==null) return false;

			conChk.Clear();

			//objを起点にして、接続されているものの ConnectionCheckActive 属性を有効にする。
			conChk.ScanData(obj, workList);

			//ConnectionCheckActive 属性が有効で、_obj.DeleteCount が-2でないものが出力リストに未登録。
			foreach (MbeObj _obj in workList) {
				if (_obj.ConnectionCheckActive && _obj.DeleteCount != -2) {
					_obj.DeleteCount = -2;
					if (pinOnly) {
						if (_obj.Id() == MbeObjID.MbePTH ||
						   _obj.Id() == MbeObjID.MbePinSMD) {
							if (_obj.TempPropString.Length > 0) {
								objListNet.AddLast(_obj);
							}
						}
					} else {
						objListNet.AddLast(_obj);
					}
				}
			}
			return true;
		}

		public LinkedList<MbeObj> workList;
        public List<MbeObjComponent> componentList;
		protected MbeConChk conChk;
	}
}
