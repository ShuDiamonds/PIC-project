using System;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
    class TextInfo: MbeMyStd
    {
        public TextInfo()
        {
            TextHeight = MbeObjText.DEFAULT_TEXT_HEIGHT;
            LineWidth = MbeObjText.DEFAULT_LINE_WIDTH;
        }
        

        public int LineWidth
        {
            get { return lineWidth; }
            set {
                if (value < MbeObjText.MIN_LINE_WIDTH) {
                    value = MbeObjText.MIN_LINE_WIDTH;
                } else if (value > MbeObjText.MAX_LINE_WIDTH) {
                    value = MbeObjText.MAX_LINE_WIDTH;
                }
                lineWidth = value; 
            }
        }

        public int TextHeight
        {
            get { return textHeight; }
            set
            {
                if (value < MbeObjText.MIN_TEXT_HEIGHT) {
                    value = MbeObjText.MIN_TEXT_HEIGHT;
                } else if (value > MbeObjText.MAX_TEXT_HEIGHT) {
                    value = MbeObjText.MAX_TEXT_HEIGHT;
                }
                textHeight = value; 
            }
        }

        
        protected int lineWidth;
        protected int textHeight;

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            TextInfo info = obj as TextInfo;
            if ((System.Object)info == null) {
                return false;
            }

            return (lineWidth == info.lineWidth &&
                textHeight == info.textHeight);
        }
   

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("H:{0:##0.0###}mm, W:{1:##0.0###}mm", (double)textHeight / 10000, (double)lineWidth / 10000);
        }


        public virtual bool WriteMb3Member(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecordInt("H", textHeight);
            writeCE3.WriteRecordInt("W", lineWidth);
            return true;
        }

        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+TEXT_INFO");
            WriteMb3Member(writeCE3);
            writeCE3.WriteRecord("-TEXT_INFO");
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-TEXT_INFO") {
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
                case "H":
                    try { TextHeight = Convert.ToInt32(str2); }
                    catch (Exception) { TextHeight = MbeObjText.DEFAULT_TEXT_HEIGHT; }
                    return ReadCE3.RdStatus.NoError;
                case "W":
                    try { LineWidth = Convert.ToInt32(str2); }
                    catch (Exception) { LineWidth = MbeObjText.DEFAULT_LINE_WIDTH; }
                    return ReadCE3.RdStatus.NoError;
                default:
                    return ReadCE3.RdStatus.NoError;
            }
        }




    }
}
