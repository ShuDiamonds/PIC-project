using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CE3IO;

namespace mbe
{
    public abstract  class MbeMyStd
    {

        public abstract bool WriteMb3(WriteCE3 writeCE3);
        public abstract ReadCE3.RdStatus RdMb3(ReadCE3 readCE3);
        public abstract ReadCE3.RdStatus RdMb3Member(string str1, string str2, ReadCE3 readCE3);


        /// <summary>
        /// MbeMyStdクラスの配列を文字列化
        /// </summary>
        /// <param name="infoArray"></param>
        /// <returns></returns>
        public static string SaveMyStdInfoArray(MbeMyStd[] infoArray)
        {
            StringBuilder strBuilder = new StringBuilder();
            StringWriter stringWriter = null;
            try {
                stringWriter = new StringWriter(strBuilder);
                WriteCE3 writeMb3 = new WriteCE3(stringWriter);
                foreach (MbeMyStd info in infoArray) {
                    info.WriteMb3(writeMb3);
                }
                stringWriter.Flush();
                string strBuff = strBuilder.ToString();
                return strBuff;
            }
            catch (Exception) {
                return "";
            }
        }

        public static MbeMyStd[] LoadMyStdInfoArray(string str)
        {
            LinkedList<MbeMyStd> infoLList = new LinkedList<MbeMyStd>();
            if (str != null) {
                StringReader stringReader = new StringReader(str);
                ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;
                try {
                    ReadCE3 readMb3 = new ReadCE3(stringReader);
                    string str1;
                    string str2;
                    while (readMb3.GetRecord(out str1, out str2)) {
                        if (str1[0] == '-') {
                            break;
                        } else {
                            MbeMyStd info = null;
                            if (str1 == "") continue;
                            else if (str1 == "+PAD_INFO") {
                                info = new PadInfo();
                            } else if (str1 == "+PTH_INFO") {
                                info = new PthInfo();
                            } else if (str1 == "+LINE_INFO") {
                                info = new LineInfo();
                            } else if (str1 == "+ARC_INFO") {
                                info = new ArcInfo();
                            } else if (str1 == "+HOLE_INFO") {
                                info = new HoleInfo();
                            } else if (str1 == "+GRID_INFO") {
                                info = new GridInfo();
                            } else if (str1 == "+TEXT_INFO") {
                                info = new TextInfo();
                            } else if (str1 == "+POLYGON_INFO") {
                                info = new PolygonInfo();
                            } else if (str1 == "+LIB_INFO") {
                                info = new LibInfo();
                            } else if (str1 == "+PPAGE_INFO") {
                                info = new PrintPageLayerInfo();
                            }
                            if (info != null) {
                                result = info.RdMb3(readMb3);
                                if (result == ReadCE3.RdStatus.NoError) {
                                    infoLList.AddLast(info);
                                } else {
                                    break;
                                }
                            }
                        }
                    }
                }
                catch {
                }
                finally {
                    stringReader.Dispose();
                }
            }
            MbeMyStd[] infoArray = new MbeMyStd[infoLList.Count];
            int index = 0;
            foreach (MbeMyStd info in infoLList) {
                infoArray[index] = info;
                index++;
            }

            return infoArray;
        }



    }
}
