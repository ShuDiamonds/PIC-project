using System;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
    class ArcInfo : MbeMyStd
    {
        public ArcInfo()
        {
            startAngle = MbeObjArc.DEFAULT_START_ANGLE;
            endAngle = MbeObjArc.DEFAULT_END_ANGLE;
            radius = MbeObjArc.DEFAULT_RADIUS;
            width = MbeObjLine.DEFAULT_LINE_WIDTH;
        }
        

        public int Width
        {
            get { return width; }
            set {
                if (value < MbeObjLine.MIN_LINE_WIDTH) {
                    value = MbeObjLine.MIN_LINE_WIDTH;
                } else if (value > MbeObjLine.MAX_LINE_WIDTH) {
                    value = MbeObjLine.MAX_LINE_WIDTH;
                }
                width = value; 
            }
        }

        public int StartAngle
        {
            get { return startAngle; }
            set
            {
                startAngle = value % 3600;
                if (startAngle < 0) startAngle += 3600;
            }
        }

        public int EndAngle
        {
            get { return endAngle; }
            set
            {
                endAngle = value % 3600;
                if (endAngle < 0) endAngle += 3600;
            }
        }

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        
        protected int width;
        protected int startAngle;
        protected int endAngle;
        protected int radius;

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            ArcInfo info = obj as ArcInfo;
            if ((System.Object)info == null) {
                return false;
            }

            return (width == info.width && 
                startAngle == info.startAngle &&
                endAngle == info.endAngle &&
                radius == info.radius);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("R:{0:##0.0###}mm, S:{1:##0.0}, E:{2:##0.0}, W:{3:##0.0###}mm", (double)radius/10000, (double)startAngle / 10, (double)endAngle / 10, (double)width/10000);
        }


        public virtual bool WriteMb3Member(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecordInt("R",radius);
            writeCE3.WriteRecordInt("S", startAngle);
            writeCE3.WriteRecordInt("E", endAngle);
            writeCE3.WriteRecordInt("W", width);
            return true;
        }

        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+ARC_INFO");
            WriteMb3Member(writeCE3);
            writeCE3.WriteRecord("-ARC_INFO");
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-ARC_INFO") {
                        return ReadCE3.RdStatus.FormatError;
                    } else {
                        return ReadCE3.RdStatus.NoError;
                    }
                } else {
                    ReadCE3.RdStatus result = RdMb3Member(str1, str2, readCE3);
                    if (result != ReadCE3.RdStatus.NoError) {
                        return result;
                    }
                }
            }
            return ReadCE3.RdStatus.FileError;
        }

        public override ReadCE3.RdStatus RdMb3Member(string str1, string str2, ReadCE3 readCE3)
        {
            switch (str1) {
                case "R":
                    try { Radius = Convert.ToInt32(str2); }
                    catch (Exception) { Radius = MbeObjArc.DEFAULT_RADIUS; }
                    return ReadCE3.RdStatus.NoError;
                case "S":
                    try { StartAngle = Convert.ToInt32(str2); }
                    catch (Exception) { StartAngle = MbeObjArc.DEFAULT_START_ANGLE; }
                    return ReadCE3.RdStatus.NoError;
                case "E":
                    try { EndAngle = Convert.ToInt32(str2); }
                    catch (Exception) { EndAngle = MbeObjArc.DEFAULT_END_ANGLE; }
                    return ReadCE3.RdStatus.NoError;
                case "W":
                    try { Width = Convert.ToInt32(str2); }
                    catch (Exception) { Width = MbeObjLine.DEFAULT_LINE_WIDTH; }
                    return ReadCE3.RdStatus.NoError;
                default:
                    return ReadCE3.RdStatus.NoError;
            }
        }



    }
}
