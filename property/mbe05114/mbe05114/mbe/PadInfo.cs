using System;
using System.Collections.Generic;
using System.Text;
using CE3IO;

namespace mbe
{
    public class PadInfo : MbeMyStd
    {
        public PadInfo()
        {
            width = MbeObjPin.DEFAULT_WIDTH;
            height = MbeObjPin.DEFAULT_WIDTH;
        }
        

        public int Width
        {
            get { return width; }
            set {
                if (value < MbeObjPin.MIN_PAD_SIZE) {
                    value = MbeObjPin.MIN_PAD_SIZE;
                } else if (value > MbeObjPin.MAX_PAD_SIZE) {
                    value = MbeObjPin.MAX_PAD_SIZE;
                }
                width = value; 
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                if (value < MbeObjPin.MIN_PAD_SIZE) {
                    value = MbeObjPin.MIN_PAD_SIZE;
                } else if (value > MbeObjPin.MAX_PAD_SIZE) {
                    value = MbeObjPin.MAX_PAD_SIZE;
                }
                height = value;
            }
        }

        public MbeObjPin.PadShape Shape 
        {
            get { return shape; }
            set
            {
                if (value == MbeObjPin.PadShape.Obround || value == MbeObjPin.PadShape.Rect) {
                    shape = value;
                }
            }
        }


        
        protected int width;
        protected int height;
        protected MbeObjPin.PadShape shape;

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            PadInfo info = obj as PadInfo;
            if ((System.Object)info == null) {
                return false;
            }

            return (shape == info.shape && width == info.width && height == info.height);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {

            return String.Format("{0}, W:{1:##0.0###}mm, H:{2:##0.0###}mm", MbeObjPin.GetPadShapeName(shape), (double)width / 10000, (double)height / 10000);
        }


        public virtual bool WriteMb3Member(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecordString("S",MbeObjPin.GetPadShapeName(shape));
            writeCE3.WriteRecordInt("W", width);
            writeCE3.WriteRecordInt("H", height);
            return true;
        }

        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+PAD_INFO");
            WriteMb3Member(writeCE3);
            writeCE3.WriteRecord("-PAD_INFO");
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-PAD_INFO") {
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
                case "S":
                    shape = MbeObjPin.GetPadShapeValue(str2);
                    return ReadCE3.RdStatus.NoError;
                case "W":
                    try { Width = Convert.ToInt32(str2); }
                    catch (Exception) { Width = MbeObjLine.DEFAULT_LINE_WIDTH; }
                    return ReadCE3.RdStatus.NoError;
                case "H":
                    try { Height = Convert.ToInt32(str2); }
                    catch (Exception) { Height = MbeObjLine.DEFAULT_LINE_WIDTH; }
                    return ReadCE3.RdStatus.NoError;
                default:
                    return ReadCE3.RdStatus.NoError;
            }
        }


    }
}
