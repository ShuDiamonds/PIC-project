using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using CE3IO;


namespace mbe
{
    /// <summary>
    /// LineのMyStandardに用いる設定情報クラス
    /// </summary>
    public class LineInfo : MbeMyStd
    {
 
        public LineInfo()
        {
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

        private int width;

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            LineInfo info = obj as LineInfo;
            if ((System.Object)info == null) {
                return false;
            }

            return (width == info.width);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0:##0.0###}mm", (double)width / 10000);
        }



        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+LINE_INFO");
            writeCE3.WriteRecordInt("W", width);
            writeCE3.WriteRecord("-LINE_INFO");
            //writeCE3.WriteNewLine();
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-LINE_INFO") {
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
