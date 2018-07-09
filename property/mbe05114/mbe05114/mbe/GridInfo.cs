using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using CE3IO;


namespace mbe
{
    public class GridInfo : MbeMyStd
    {
        /// <summary>
        /// 初期グリッド間隔(0.0001mm単位)
        /// </summary>
        public const int INITIAL_GRID_VALUE = 12700;

        /// <summary>
        /// 最大グリッド間隔(0.0001mm単位)
        /// </summary>
        public const int MAX_GRID_VALUE = 100000;

        /// <summary>
        /// 最小グリッド間隔(0.0001mm単位)
        /// </summary>
        public const int MIN_GRID_VALUE = 10;

        /// <summary>
        /// 初期グリッド表示間隔(グリッドの本数)
        /// </summary>
        public const int INITIAL_GRID_DISPLAY_EVERY = 10;

        /// <summary>
        /// 最大グリッド表示間隔(グリッドの本数)
        /// </summary>
        public const int MAX_GRID_DISPLAY_EVERY = 1000;

        /// <summary>
        /// 最小グリッド表示間隔(グリッドの本数)
        /// </summary>
        public const int MIN_GRID_DISPLAY_EVERY = 1;



        public GridInfo()
        {
            horizontal = INITIAL_GRID_VALUE;
            vertical = INITIAL_GRID_VALUE;
            displayEvery = INITIAL_GRID_DISPLAY_EVERY;
        }
        
        public GridInfo(int horiz, int verti, int disp)
        {
            horizontal = horiz;
            vertical = verti;
            displayEvery = disp;
        }

        public GridInfo(GridInfo obj)
        {
            horizontal = obj.horizontal;
            vertical = obj.vertical;
            displayEvery = obj.displayEvery;
        }


        public int Horizontal
        {
            get { return horizontal; }
            set {
                if (value < MIN_GRID_VALUE) {
                    value = MIN_GRID_VALUE;
                } else if (value > MAX_GRID_VALUE) {
                    value = MAX_GRID_VALUE;
                }
                horizontal = value; 
            }
        }



        public int Vertical
        {
            get { return vertical; }
            set {
                if (value < MIN_GRID_VALUE) {
                    value = MIN_GRID_VALUE;
                } else if (value > MAX_GRID_VALUE) {
                    value = MAX_GRID_VALUE;
                } 
                vertical = value;
            }
        }



        public int DisplayEvery
        {
            get { return displayEvery; }
            set {
                if (value < MIN_GRID_DISPLAY_EVERY) {
                    value = MIN_GRID_DISPLAY_EVERY;
                } else if (value > MAX_GRID_DISPLAY_EVERY) {
                    value = MAX_GRID_DISPLAY_EVERY;
                } 
                displayEvery = value;
            }
        }

        private int horizontal;
        private int vertical;
        private int displayEvery;



        public override bool Equals(object obj)
        {
            if(obj == null) {
                return false;
            }

            GridInfo gridInfo = obj as GridInfo;
            if ((System.Object)gridInfo == null) {
                return false;
            }

            return (
                horizontal == gridInfo.horizontal &&
                vertical == gridInfo.vertical &&
                displayEvery == gridInfo.displayEvery);
        }

        /// <summary>
        /// グリッド間隔の範囲チェック(静的メソッド)
        /// </summary>
        /// <param name="value">チェックしたいグリッド間隔の参照</param>
        /// <returns>範囲外のときfalseを返す</returns>
        /// <remarks>範囲外のときは、valueを上限または下限に設定する</remarks>
        public static bool CheckRangeGridValue(ref int value)
        {
            if (value < MIN_GRID_VALUE) {
                value = MIN_GRID_VALUE;
                return false;
            } else if (value > MAX_GRID_VALUE) {
                value = MAX_GRID_VALUE;
                return false;
            }
            return true;
        }



        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("H:{0:##0.0###}mm, V:{1:##0.0###}mm, D:{2}", (double)horizontal / 10000, (double)vertical / 10000, displayEvery);
        }



        public override bool WriteMb3(WriteCE3 writeCE3)
        {
            writeCE3.WriteRecord("+GRID_INFO");
            writeCE3.WriteRecordInt("H", horizontal);
            writeCE3.WriteRecordInt("V", vertical);
            writeCE3.WriteRecordInt("HD", displayEvery);
            writeCE3.WriteRecord("-GRID_INFO");
            //writeCE3.WriteNewLine();
            return true;
        }

        public override ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
        {
            string str1;
            string str2;
            while (readCE3.GetRecord(out str1, out str2)) {
                if (str1[0] == '-') {
                    if (str1 != "-GRID_INFO") {
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
                    try { horizontal = Convert.ToInt32(str2); }
                    catch (Exception) { horizontal = GridInfo.INITIAL_GRID_VALUE; }
                    return ReadCE3.RdStatus.NoError;
                case "V":
                    try { vertical = Convert.ToInt32(str2); }
                    catch (Exception) { vertical = GridInfo.INITIAL_GRID_VALUE; }
                    return ReadCE3.RdStatus.NoError;
                case "HD":
                    try { displayEvery = Convert.ToInt32(str2); }
                    catch (Exception) { displayEvery = GridInfo.INITIAL_GRID_DISPLAY_EVERY; }
                    return ReadCE3.RdStatus.NoError;
                default:
                    return ReadCE3.RdStatus.NoError;
            }
        }

    }
}
