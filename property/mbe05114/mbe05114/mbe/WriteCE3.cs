using System;
using System.IO;
using System.Text;

namespace CE3IO
{
	/// <summary>
	/// Summary description for WriteCE3
	/// </summary>
	public class WriteCE3
	{
		protected TextWriter textWriter;
		protected bool newLine;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <remarks>あとからTextWriterをAttachする</remarks>
		public WriteCE3()
		{
			//
			// TODO: Add constructor logic here
			//
			newLine = true;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public WriteCE3(TextWriter writer)
		{
			textWriter = writer;
			newLine = true;
		}

		/// <summary>
		/// 書き込み対象のTextReaderを設定
		/// </summary>
		/// <param name="reader"></param>
		public void Attach(TextWriter writer)
		{
			textWriter = writer;
			newLine = true;
		}

		/// <summary>
		/// レコードの書き込み
		/// </summary>
		/// <param name="str"></param>
		/// <remarks>
		/// 行頭以外ならカンマを打ってからレコードを書き込む
		/// </remarks>
		public void WriteRecord(string str)
		{
			if(!newLine){
				textWriter.Write(",");
			}else{
				newLine = false;
			}
			textWriter.Write(str);
		}

		/// <summary>
		/// 改行
		/// </summary>
		public void WriteNewLine()
		{
			textWriter.WriteLine();
			newLine = true;
		}

		/// <summary>
		/// 整数レコードの書き込み
		/// </summary>
		/// <param name="name">変数名</param>
		/// <param name="val">整数パラメータ</param>
		public void WriteRecordInt(string name, int val)
		{
			WriteRecord(name + ":" + val);
		}


        /// <summary>
        /// ulongレコードの書き込み
        /// </summary>
        /// <param name="name">変数名</param>
        /// <param name="val">パラメータ</param>
        public void WriteRecordUlong(string name, ulong val)
        {
            WriteRecord(name + ":" + val);
        }

        
        /// <summary>
		/// 文字列レコードの書き込み
		/// </summary>
		/// <param name="name">変数名</param>
		/// <param name="val">文字列パラメータ</param>
		public void WriteRecordString(string name, string val)
		{
			StringBuilder strDst = new StringBuilder();
			int i;
			for(i=0;i<val.Length;i++){
				char cc = val[i];
				if((cc >= '\x01')&&(cc <= ' ') || (cc == '%') || (cc == ',')){
					strDst.Append(string.Format("%{0:X2}",(int)cc));
				}else{
					strDst.Append(cc);
				}
			}
			WriteRecord(name + ":" + strDst);
		}

	}
}