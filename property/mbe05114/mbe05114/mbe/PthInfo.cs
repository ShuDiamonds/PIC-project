using System;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
    class PthInfo : PadInfo
    {
        public PthInfo()
        {
            dia = MbeObjPTH.DEFAULT_DIA;
        }
        

        public int Dia
        {
            get { return dia; }
            set {
                if (value < MbeObjPTH.MIN_DIA) {
                    value = MbeObjPTH.MIN_DIA;
                } else if (value > MbeObjPTH.MAX_DIA) {
                    value = MbeObjPTH.MAX_DIA;
                }
                dia = value; 
            }
        }

        protected int dia;

        public override bool Equals(object obj)
        {
            if (base.Equals(obj)) {
                PthInfo info = obj as PthInfo;
                if ((System.Object)info == null) {
                    return false;
                }

                return (dia == info.dia);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0}, W:{1:##0.0###}mm, H:{2:##0.0###}mm, D:{3:##0.0###}mm",MbeObjPin.GetPadShapeName(shape), (double)width / 10000, (double)height / 10000, (double)dia / 10000);
        }


        public override bool WriteMb3Member(WriteCE3 writeCE3)
        {
            base.WriteMb3Member(writeCE3);
            writeCE3.WriteRecordInt("D", dia);
            return true;
        }

        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+PTH_INFO");
            WriteMb3Member(writeCE3);
            writeCE3.WriteRecord("-PTH_INFO");
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-PTH_INFO") {
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
                    catch (Exception) { Dia = MbeObjPTH.DEFAULT_DIA; }
                    return ReadCE3.RdStatus.NoError;
                default:
                    return base.RdMb3Member(str1, str2, readCE3);
            }
        }




    }
}
