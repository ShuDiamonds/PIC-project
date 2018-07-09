using System;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
    public class PrintPageLayerInfo : MbeMyStd
    {
        public bool active;
        public bool mirror;
        public string name;
        public ulong checkvalue;

        public PrintPageLayerInfo()
        {
            active = true;
            mirror = false;
            name = "";
            checkvalue = 0;
        }

        public virtual bool WriteMb3Member(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecordInt("A", (active? 1:0));
            writeCE3.WriteRecordInt("M", (mirror ? 1 : 0));
            writeCE3.WriteRecordString("N", name);
            writeCE3.WriteRecordUlong("L", checkvalue);
            return true;
        }

        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+PPAGE_INFO");
            WriteMb3Member(writeCE3);
            writeCE3.WriteRecord("-PPAGE_INFO");
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-PPAGE_INFO") {
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
                case "A": {
                        int n = 0;
                        try { n = Convert.ToInt32(str2); }
                        catch (Exception) { n = 0; }
                        active = (n != 0);
                    }
                    return ReadCE3.RdStatus.NoError;
                case "M": {
                        int n = 0;
                        try { n = Convert.ToInt32(str2); }
                        catch (Exception) { n = 0; }
                        mirror = (n != 0);
                    }
                    return ReadCE3.RdStatus.NoError;
                case "N":
                    name = ReadCE3.DecodeCE3String(str2);
                    return ReadCE3.RdStatus.NoError;
                case "L": {
                        ulong u = 0;
                        try { u = Convert.ToUInt64(str2); }
                        catch (Exception) { u = 0; }
                        checkvalue = u;
                    }
                    return ReadCE3.RdStatus.NoError;
                default:
                    return ReadCE3.RdStatus.NoError;
            }
        }

        public static List<PrintPageLayerInfo>  LoadMyStandard()
        {
            //MyStandard値のロード
            string strMyStdInfo = Properties.Settings.Default.MyStandardPrintPageLayerString;
            MbeMyStd[] myStdInfoArray = MbeMyStd.LoadMyStdInfoArray(strMyStdInfo);

            List<PrintPageLayerInfo> infoList = new List<PrintPageLayerInfo>();

            foreach (MbeMyStd info in myStdInfoArray) {
                infoList.Add((PrintPageLayerInfo)info);
            }
            return infoList;
        }

        public static void SaveMyStandard(List<PrintPageLayerInfo> infoList)
        {
            int count = infoList.Count;

            PrintPageLayerInfo[] myStdInfoArray = new PrintPageLayerInfo[count];
            for (int i = 0; i < count; i++) {
                myStdInfoArray[i] = infoList[i];
            }
            Properties.Settings.Default.MyStandardPrintPageLayerString = MbeMyStd.SaveMyStdInfoArray(myStdInfoArray);
            Properties.Settings.Default.Save();
        }



    }
}
