using System;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
    /// <summary>
    /// HoleのMyStandardに用いる設定情報クラス
    /// </summary>
    public class HoleInfo : MbeMyStd
    {
        public HoleInfo()
        {
            dia = MbeObjHole.DEFAULT_DIA;
        }
        

        public int Dia
        {
            get { return dia; }
            set {
                if (value < MbeObjHole.MIN_DIA) {
                    value = MbeObjHole.MIN_DIA;
                } else if (value > MbeObjHole.MAX_DIA) {
                    value = MbeObjHole.MAX_DIA;
                }
                dia = value; 
            }
        }

        private int dia;

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            HoleInfo info = obj as HoleInfo;
            if ((System.Object)info == null) {
                return false;
            }

            return (dia == info.dia);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0:##0.0###}mm", (double)dia / 10000);
        }



        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+HOLE_INFO");
            writeCE3.WriteRecordInt("D", dia);
            writeCE3.WriteRecord("-HOLE_INFO");
            //writeCE3.WriteNewLine();
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-HOLE_INFO") {
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
                case "D":
                    try { Dia = Convert.ToInt32(str2); }
                    catch (Exception) { Dia = MbeObjHole.DEFAULT_DIA; }
                    return ReadCE3.RdStatus.NoError;
                default:
                    return ReadCE3.RdStatus.NoError;
            }
        }


    }
}
