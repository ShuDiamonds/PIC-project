using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace mbe
{
	public class MbeGapChk
	{
		public const int MIN_GAP = 1000; 	//0.1mm
		public const int MAX_GAP = 10000; 	//1mm


		public MbeGapChk()
		{
			chkObjLList = new LinkedList<MbeGapChkObj>();
			chkObjList = new List<MbeGapChkObj>();
		}

		public void Add(MbeGapChkObj obj)
		{
			chkObjLList.AddLast(obj);
		}


		public void DoGapChk(LinkedList<MbeObj> dataList, int minGap, LinkedList<Point> ptLList, int limit)
		{
			if (SetData(dataList)) {
				GapChk(minGap, ptLList, limit);
			}
			chkObjLList.Clear();
			chkObjList.Clear();
		}


		protected void GapChk(int minGap, LinkedList<Point> ptLList, int limit)
		{
			int dataCount = chkObjList.Count;
			int currentNetStartIndex = 0;
			int nextNetStartIndex;
			int gapErrorCount = 0;

			int loopCount = 0;

			while (true) {
				loopCount++;
				System.Diagnostics.Debug.WriteLine("GapChk loopCount:" + loopCount);
				int netNum = chkObjList[currentNetStartIndex].netNum;
				int i;
				nextNetStartIndex = -1;
				for (i = currentNetStartIndex + 1; i < dataCount; i++) {
					if (chkObjList[i].netNum != netNum) {
						nextNetStartIndex = i;
						break;
					}
				}
				if (nextNetStartIndex == -1) break;

				for (i = currentNetStartIndex; i < nextNetStartIndex; i++) {
					for (int j = nextNetStartIndex; j < dataCount; j++) {
						if (chkObjList[i].layer != chkObjList[j].layer) continue; 
                        if(chkObjList[i].IsCloseTo(chkObjList[j],minGap)){
						    Point ptAt;
						    double dist = chkObjList[i].Distance(chkObjList[j], out ptAt);
						    //System.Diagnostics.Debug.WriteLine("distance = " + dist);
						    if (dist < minGap) {
							    System.Diagnostics.Debug.WriteLine("    GapError dist:" + dist + "  " + ptAt.X + "," + ptAt.Y);
							    ptLList.AddLast(ptAt);
							    gapErrorCount++;
							    if (gapErrorCount >= limit) return;
						    }
                        }
					}
				}
				currentNetStartIndex = nextNetStartIndex;
			}
		}

	
		protected bool SetData(LinkedList<MbeObj> dataList)
		{
			MbeNetOut netOut = new MbeNetOut();
			netOut.SetMbeObjData(dataList);		//ネット情報取得に必要な要素だけ収集
			int netNum = 0;
			LinkedList<MbeObj> netObjList = new LinkedList<MbeObj>();
			chkObjLList.Clear();
			chkObjList.Clear();

			//有効なネットに属しているオブジェクトからギャップチェック用データを作る
			while (netOut.GetNet(netObjList, false)) {			//ネットに属するオブジェクトを取得して
				netNum++;										//ネット番号を進めて
				foreach (MbeObj netMember in netObjList) {
					//netMember.GenerateGapChkData(this, netNum);	//ギャップチェック用データの生成
					netMember.GenerateGapChkData(chkObjLList, netNum);	//ギャップチェック用データの生成
				}
			}
			if(netNum==0){			//netNum==0は有効なネットがなかったとき
				netOut.CleanUp();
				return false;
			}

			netNum++;				//円弧や文字の要素など、ネットに含まれないメンバーはすべてこのnetNumに属する

			//ネットに含まれなかったメンバーの設定
			foreach (MbeObj netMember in netOut.workList) {
				if(!netMember.ConnectionCheckActive){
					//netMember.GenerateGapChkData(this, netNum);
					netMember.GenerateGapChkData(chkObjLList, netNum);
				}
			}
			netOut.CleanUp();		//ネット情報生成のために変更したフラグなどを掃除

			//テキストの設定
            //Version 0.44でArcをネットに含めるように変更
			foreach (MbeObj obj in dataList) {
				if (obj.DeleteCount >= 0) continue;

				if (obj.Id() == MbeObjID.MbeText){
					//||obj.Id() == MbeObjID.MbeArc) {
					if (obj.Layer == MbeLayer.LayerValue.CMP ||
                        obj.Layer == MbeLayer.LayerValue.L2  ||
                        obj.Layer == MbeLayer.LayerValue.L3  ||
						obj.Layer == MbeLayer.LayerValue.SOL) {
						//obj.GenerateGapChkData(this, netNum);
						obj.GenerateGapChkData(chkObjLList, netNum);
					}
				}else if (obj.Id() == MbeObjID.MbeComponent) {
					foreach (MbeObj objContent in ((MbeObjComponent)obj).ContentsObj) {
						if (obj.Id() == MbeObjID.MbeText){
							 //|| obj.Id() == MbeObjID.MbeArc) {

							if (objContent.Layer == MbeLayer.LayerValue.CMP ||
                                obj.Layer == MbeLayer.LayerValue.L2 ||
                                obj.Layer == MbeLayer.LayerValue.L3 ||
								objContent.Layer == MbeLayer.LayerValue.SOL) {
								//obj.GenerateGapChkData(this, netNum);
								obj.GenerateGapChkData(chkObjLList, netNum);
							}
						}
					}
				}
			}

			chkObjList.Capacity = chkObjLList.Count;

			foreach (MbeGapChkObj obj in chkObjLList) {
				chkObjList.Add(obj);
			}

			chkObjLList.Clear();

			return true;
		}

		protected LinkedList<MbeGapChkObj> chkObjLList;
		protected List<MbeGapChkObj> chkObjList;
	}


	public enum MbeGapChkShape
	{
		POINT,
		LINE,
		RECTANGLE
	}


	public abstract class MbeGapChkObj
	{
		public int netNum;			//ネットの識別番号
		public MbeObj mbeObj;		//元になったオブジェクト
		public MbeLayer.LayerValue layer;
		public int status;

		public abstract MbeGapChkShape Shape();
		public abstract double Distance(MbeGapChkObj gapObj,out Point chkPoint);
        public abstract bool IsCloseTo(MbeGapChkObj gapObj, int limit);

		protected MbeGapChkObj()
		{
			netNum = -1;
			mbeObj = null;
			status = 0;
		}
	}

	public class MbeGapChkObjLine : MbeGapChkObj
	{
		public Point p0;
		public Point p1;
		public int lineWidth;

		public override MbeGapChkShape Shape()
		{
			return MbeGapChkShape.LINE;
		}

		public MbeGapChkObjLine()
		{
			p0 = new Point(0, 0);
			p1 = new Point(0, 0);
			lineWidth = 0;
		}

		public void SetLineValue(Point _p0, Point _p1, int _lineWidth)
		{
			p0 = _p0;
			p1 = _p1;
			lineWidth = _lineWidth;
		}

		public void SetLineValue(Point ptCenter, int width, int height)
		{
			if (width > height) {
				lineWidth = height;
				int lineLength = width - height;
				p0 = new Point(ptCenter.X - lineLength / 2, ptCenter.Y);
				p1 = new Point(ptCenter.X + lineLength / 2, ptCenter.Y);
			} else {// height >= width
				lineWidth = width;
				int lineLength = height - width;
				p0 = new Point(ptCenter.X, ptCenter.Y - lineLength / 2);
				p1 = new Point(ptCenter.X, ptCenter.Y + lineLength / 2);
			}
		}

 
		public override double Distance(MbeGapChkObj gapObj,out Point chkPoint)
		{
			double dist = Int32.MaxValue;
			chkPoint = new Point(0,0);
			switch(gapObj.Shape()){
				case MbeGapChkShape.LINE:
					dist = Util.DistanceLineLine(p0,p1,
												((MbeGapChkObjLine)gapObj).p0,((MbeGapChkObjLine)gapObj).p1,
												out chkPoint);
					dist -= (lineWidth+((MbeGapChkObjLine)gapObj).lineWidth)/2;
					break;
				case MbeGapChkShape.POINT:
					dist = Util.DistancePointLine(((MbeGapChkObjPoint)gapObj).pt,
												p0,p1,
												out chkPoint);
					dist -= (lineWidth + ((MbeGapChkObjPoint)gapObj).dia)/2;
					break;
				case MbeGapChkShape.RECTANGLE:
					dist = Util.DistanceRectLine(((MbeGapChkObjRect)gapObj).rc,
												p0,p1,
												out chkPoint);
					dist -= lineWidth/2;
					break;
			}
			if(dist<0) dist = 0;
			return dist;
		}

		/// <summary>
		/// 線からの距離が設定値より近いかどうかの判定。
		/// </summary>
		/// <param name="gapObj"></param>
		/// <param name="chkPoint"></param>
		/// <returns></returns>
		/// <remarks>Polygonの更新時に使うために軽量化を目指す</remarks>
		public override bool IsCloseTo(MbeGapChkObj gapObj,int limit)
		{
			//Point chkPoint;
			//double dist = 3000000;
			int nDist;
			//chkPoint = new Point(0, 0);
			switch (gapObj.Shape()) {
				case MbeGapChkShape.LINE:
					nDist = limit + (lineWidth + ((MbeGapChkObjLine)gapObj).lineWidth) / 2;
					return Util.LineIsCloseToLine(p0, p1,
												((MbeGapChkObjLine)gapObj).p0, ((MbeGapChkObjLine)gapObj).p1,
												nDist);
					//dist = Util.DistanceLineLine(p0, p1,
					//                            ((MbeGapChkObjLine)gapObj).p0, ((MbeGapChkObjLine)gapObj).p1,
					//                            out chkPoint);
					//dist -= (lineWidth + ((MbeGapChkObjLine)gapObj).lineWidth) / 2;
					//break;
				case MbeGapChkShape.POINT:
					nDist = limit + (lineWidth + ((MbeGapChkObjPoint)gapObj).dia) / 2;
					return Util.PointIsCloseToLine(((MbeGapChkObjPoint)gapObj).pt,
												p0, p1,
												nDist);
					//dist = Util.DistancePointLine(((MbeGapChkObjPoint)gapObj).pt,
					//                            p0, p1,
					//                            out chkPoint);
					//dist -= (lineWidth + ((MbeGapChkObjPoint)gapObj).dia) / 2;
					//break;
				case MbeGapChkShape.RECTANGLE:
					nDist = limit+lineWidth / 2;

					return Util.LineIsCloseToRect(((MbeGapChkObjRect)gapObj).rc,
												p0, p1, nDist);

					//dist = Util.DistanceRectLine(((MbeGapChkObjRect)gapObj).rc,
					//                            p0, p1,
					//                            out chkPoint);
					//dist -= lineWidth / 2;
					//break;
			}
			return false;
		}






	}

	class MbeGapChkObjRect : MbeGapChkObj
	{
		public MbeRect rc;

		public override MbeGapChkShape Shape()
		{
			return MbeGapChkShape.RECTANGLE;
		}


		public MbeGapChkObjRect()
		{
			rc = new MbeRect(new Point(0, 0), new Point(0, 0));
		}

		public void SetRectValue(MbeRect _rc)
		{
			rc = _rc;
		}

		public void SetRectValue(Point ptCenter, int width, int height)
		{
			rc = new MbeRect(new Point(ptCenter.X - width / 2, ptCenter.Y + height / 2),	//Left, Top
							 new Point(ptCenter.X + width / 2, ptCenter.Y - height / 2));	//Right, Bottom
		}

		public override double Distance(MbeGapChkObj gapObj,out Point chkPoint)
		{
			double dist = Int32.MaxValue; //3000000;
			chkPoint = new Point(0,0);
			switch(gapObj.Shape()){
				case MbeGapChkShape.LINE:
					dist = Util.DistanceRectLine(rc,
											((MbeGapChkObjLine)gapObj).p0,((MbeGapChkObjLine)gapObj).p1,
											out chkPoint);
					dist -= ((MbeGapChkObjLine)gapObj).lineWidth/2;
					break;
				case MbeGapChkShape.POINT:
					dist = Util.DistanceRectPoint(rc,
												((MbeGapChkObjPoint)gapObj).pt,
												out chkPoint);
					dist -= ((MbeGapChkObjPoint)gapObj).dia/2;
					break;
				case MbeGapChkShape.RECTANGLE:
					dist = Util.DistanceRectRect(rc,
												((MbeGapChkObjRect)gapObj).rc,
												out chkPoint);
					break;
			}
			if(dist<0) dist = 0;
			return dist;
		}

        public override bool IsCloseTo(MbeGapChkObj gapObj,int limit)
		{
            int nDist;

            //double dist = Int32.MaxValue; //3000000;
            //chkPoint = new Point(0,0);
			switch(gapObj.Shape()){
				case MbeGapChkShape.LINE:
                    nDist = limit + ((MbeGapChkObjLine)gapObj).lineWidth/2;
                    return Util.LineIsCloseToRect(rc,((MbeGapChkObjLine)gapObj).p0,((MbeGapChkObjLine)gapObj).p1,nDist);
                    //dist = Util.DistanceRectLine(rc,
                    //                        ((MbeGapChkObjLine)gapObj).p0,((MbeGapChkObjLine)gapObj).p1,
                    //                        out chkPoint);
					//dist -= ((MbeGapChkObjLine)gapObj).lineWidth/2;
					//break;
				case MbeGapChkShape.POINT:
                    nDist = limit + ((MbeGapChkObjPoint)gapObj).dia/2;
                    return Util.PointIsCloseToRect(rc,((MbeGapChkObjPoint)gapObj).pt,nDist);
                    //dist = Util.DistanceRectPoint(rc,
                    //                            ((MbeGapChkObjPoint)gapObj).pt,
                    //                            out chkPoint);
                    //dist -= ((MbeGapChkObjPoint)gapObj).dia/2;
					//break;
				case MbeGapChkShape.RECTANGLE:
                    nDist = limit;
                    return Util.RectIsCloseToRect(rc,
												((MbeGapChkObjRect)gapObj).rc,
                                                nDist);
                    //dist = Util.DistanceRectRect(rc,
                    //                            ((MbeGapChkObjRect)gapObj).rc,
                    //                            out chkPoint);
					//break;
 			}
            return false;
			//if(dist<0) dist = 0;
			//return dist;
		}

	}



	class MbeGapChkObjPoint : MbeGapChkObj
	{
		public Point pt;
		public int dia;

		public override MbeGapChkShape Shape()
		{
			return MbeGapChkShape.POINT;
		}


		public MbeGapChkObjPoint()
		{
			pt = new Point(0, 0);
			dia = 0;
		}

		public void SetPointValue(Point _pt,int _dia)
		{
			pt = _pt;
			dia = _dia;
		}

		public override double Distance(MbeGapChkObj gapObj,out Point chkPoint)
		{
			double dist = Int32.MaxValue;
			chkPoint = new Point(0,0);
			switch(gapObj.Shape()){
				case MbeGapChkShape.LINE:
					dist = Util.DistancePointLine(pt,
												((MbeGapChkObjLine)gapObj).p0,((MbeGapChkObjLine)gapObj).p1,
												out chkPoint);
					dist -= (dia + ((MbeGapChkObjLine)gapObj).lineWidth)/2;
					break;
				case MbeGapChkShape.POINT:
					dist = Util.DistancePointPoint(pt,
												((MbeGapChkObjPoint)gapObj).pt,
												out chkPoint);
					dist -= (dia+((MbeGapChkObjPoint)gapObj).dia)/2;
					break;
				case MbeGapChkShape.RECTANGLE:
					dist = Util.DistanceRectPoint(((MbeGapChkObjRect)gapObj).rc,
													pt,
													out chkPoint);
					dist -= dia/2;
					break;
			}
			if (dist < 0) dist = 0;
			return dist;
		}

        public override bool IsCloseTo(MbeGapChkObj gapObj,int limit)
		{
            int nDist;
            //double dist = Int32.MaxValue;
            //chkPoint = new Point(0,0);
			switch(gapObj.Shape()){
				case MbeGapChkShape.LINE:
                    nDist = limit+(dia + ((MbeGapChkObjLine)gapObj).lineWidth)/2;
                    return Util.PointIsCloseToLine(pt,
												((MbeGapChkObjLine)gapObj).p0,((MbeGapChkObjLine)gapObj).p1,
                                                nDist);
                    //dist = Util.DistancePointLine(pt,
                    //                            ((MbeGapChkObjLine)gapObj).p0,((MbeGapChkObjLine)gapObj).p1,
                    //                            out chkPoint);
					//dist -= (dia + ((MbeGapChkObjLine)gapObj).lineWidth)/2;
					//break;
				case MbeGapChkShape.POINT:
                    nDist = limit+(dia+((MbeGapChkObjPoint)gapObj).dia)/2;
                    return Util.PointIsCloseToPoint(pt,
												((MbeGapChkObjPoint)gapObj).pt,
                                                nDist);
                    //dist = Util.DistancePointPoint(pt,
                    //                            ((MbeGapChkObjPoint)gapObj).pt,
                    //                            out chkPoint);
                    //dist -= (dia+((MbeGapChkObjPoint)gapObj).dia)/2;
					//break;
				case MbeGapChkShape.RECTANGLE:
                    nDist = limit+dia/2;
                    return Util.PointIsCloseToRect(((MbeGapChkObjRect)gapObj).rc,
													pt,
                                                    nDist);
                    //dist = Util.DistanceRectPoint(((MbeGapChkObjRect)gapObj).rc,
                    //                                pt,
                    //                                out chkPoint);
                    //dist -= dia/2;
					//break;
			}
			return false;
		}

	
	}


	class MbeDrc
	{
		public const int MIN_CHECK_LIMIT = 10;
		public const int MAX_CHECK_LIMIT = 200;
	}

	class MbeDrcParam
	{
		public int patternGap;
		public int checkLimit;

		public MbeDrcParam()
		{
			patternGap = MbeGapChk.MIN_GAP;
			checkLimit = MbeDrc.MAX_CHECK_LIMIT;
		}
	}

		


}
