using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mbe
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

			//OperatingSystem os = Environment.OSVersion;
		    //PlatformID     pid = os.Platform;
			//if(pid == PlatformID.Unix){
			//    osIsUnix = true;
			//}else{
			//    osIsUnix = false;
			//}

            //MONO環境判定
            //MONO 2.6では、MONOオプションを付けなくてもほぼ動作した。
            //MONOオプションを付けない場合でも、
            //      スクロールでサムドラッグしたときの動作
            //      ファイルオープン時にもうひとつのウィンドウを起動するときのコマンドライン
            //以上については、MONO環境用に調整が必要となる。これらについては、monoRuntime
            //グローバル変数で実行時に分岐する。

            if (Type.GetType("Mono.Runtime")!=null) {
                monoRuntime = true;
            } else {
                monoRuntime = false;
            }

            if (monoRuntime) {
                System.Console.WriteLine("Running with Mono.Runtime");
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

		//public static bool osIsUnix;
        public static bool monoRuntime;
        public static bool listFileUseLocalEncoding;
        public static bool inhibitHatchBrushPolygonframe;
        public static bool drawTextSolidly;
    }
}