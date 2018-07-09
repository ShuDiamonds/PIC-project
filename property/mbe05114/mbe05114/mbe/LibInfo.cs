using System;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
    class LibInfo : MbeMyStd
    {
        private string libPath;

        public string LibPath
        {
            get { return libPath; }
            set { libPath = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            LibInfo info = obj as LibInfo;
            if ((System.Object)info == null) {
                return false;
            }

            return (libPath.Equals(info.libPath));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

   
        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+LIB_INFO");
            writeCE3.WriteRecordString("P", libPath);
            writeCE3.WriteRecord("-LIB_INFO");
            //writeCE3.WriteNewLine();
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-LIB_INFO") {
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
                case "P":
                    libPath = ReadCE3.DecodeCE3String(str2);
                    return ReadCE3.RdStatus.NoError;
                default:
                    return ReadCE3.RdStatus.NoError;
            }
        }





    }
}
