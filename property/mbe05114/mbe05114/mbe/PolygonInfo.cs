using System;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
    class PolygonInfo : MbeMyStd
    {
        public PolygonInfo()
        {
            traceWidth = MbeObjPolygon.DEFAULT_TRACE_WIDTH;
            patternGap = MbeObjPolygon.DEFAULT_PATTERN_GAP;
        }
        

        public int TraceWidth
        {
            get { return traceWidth; }
            set {
                if (value < MbeObjPolygon.MIN_TRACE_WIDTH) {
                    value = MbeObjPolygon.MIN_TRACE_WIDTH;
                } else if (value > MbeObjPolygon.MAX_TRACE_WIDTH) {
                    value = MbeObjPolygon.MAX_TRACE_WIDTH;
                }
                traceWidth = value; 
            }
        }

        public int PatternGap
        {
            get { return patternGap; }
            set
            {
                if (value < MbeObjPolygon.MIN_PATTERN_GAP) {
                    value = MbeObjPolygon.MIN_PATTERN_GAP;
                } else if (value > MbeObjPolygon.MAX_PATTERN_GAP) {
                    value = MbeObjPolygon.MAX_PATTERN_GAP;
                }
                patternGap = value;
            }
        }


        private int traceWidth;
        private int patternGap;


        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            PolygonInfo info = obj as PolygonInfo;
            if ((System.Object)info == null) {
                return false;
            }

            return (traceWidth == info.traceWidth &&
                    patternGap == info.patternGap);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("G:{0:##0.0###}mm, T:{1:##0.0###}mm", (double)patternGap / 10000, (double)traceWidth / 10000);
        }



        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+POLYGON_INFO");
            writeCE3.WriteRecordInt("G", patternGap);
            writeCE3.WriteRecordInt("T", traceWidth);
            writeCE3.WriteRecord("-POLYGON_INFO");
            //writeCE3.WriteNewLine();
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-POLYGON_INFO") {
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
                case "G":
                    try { PatternGap = Convert.ToInt32(str2); }
                    catch (Exception) { PatternGap = MbeObjPolygon.DEFAULT_PATTERN_GAP; }
                    return ReadCE3.RdStatus.NoError;
                case "T":
                    try { TraceWidth = Convert.ToInt32(str2); }
                    catch (Exception) { TraceWidth = MbeObjPolygon.DEFAULT_TRACE_WIDTH; }
                    return ReadCE3.RdStatus.NoError;
                default:
                    return ReadCE3.RdStatus.NoError;
            }
        }



    }
}
