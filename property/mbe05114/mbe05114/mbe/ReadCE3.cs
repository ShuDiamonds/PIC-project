using System;
using System.IO;
using System.Text;

namespace CE3IO
{
    /// <summary>
    /// Summary description for ReadCE3
    /// </summary>
    public class ReadCE3
    {
		public enum RdStatus
		{
			NoError = 0,
			FormatError,
			FileError
		}


        protected TextReader textReader;

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <remarks> ���Ƃ���TextReader��Attach���� </remarks>
	    public ReadCE3()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
	    }

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="reader"></param>
        public ReadCE3(TextReader reader)
        {
            textReader = reader;
        }

        /// <summary>
        /// �ǂݍ��ݑΏۂ�TextReader��ݒ�
        /// </summary>
        /// <param name="reader"></param>
		public void Attach(TextReader reader)
        {
            textReader = reader;
        }

		/// <summary>
		/// ���R�[�h�̎擾
		/// </summary>
		/// <param name="rStr1"></param>
		/// <param name="rStr2"></param>
		/// <returns>���R�[�h�̎擾�O��TextReader���I�[�ɓ��B������false</returns>
        public bool GetRecord(out string rStr1, out string rStr2)
        {
			rStr1 = "";
			rStr2 = "";
			if (textReader == null) return false;
			int rdChar;
			StringBuilder str1 = new StringBuilder();
			StringBuilder str2 = new StringBuilder();

			//try {
				do {
					rdChar = textReader.Read();
				} while ((rdChar != -1) && (Char.IsWhiteSpace((char)rdChar)));

				bool appendFlag = true;
				bool detect1stColon = false;
				while (rdChar != -1 && rdChar != ',' && rdChar != '\r' && rdChar != '\n') {
					if (appendFlag) {
						if (rdChar == ' ' || rdChar == '\t') {
						//if (Char.IsWhiteSpace((char)rdChar)) {
							appendFlag = false;
						} else {
							if (!detect1stColon && rdChar == ':') {
								detect1stColon = true;
							} else {
								if (!detect1stColon) {
									str1.Append((char)rdChar);
								} else {
									str2.Append((char)rdChar);
								}
							}
						}
					}
					rdChar = textReader.Read();
				}
			//}
			//catch (Exception) {
			//}
            rStr1 = str1.ToString();
            rStr2 = str2.ToString();
			if (rStr1.Length == 0) return false;
            return true;
        }

		/// <summary>
		/// �w�肵��������܂ł̓ǂݔ�΂�
		/// </summary>
		/// <param name="strSkipTo"></param>
		/// <returns></returns>
		/// <remarks>
		/// ���m�̃u���b�N +blockname �����ꂽ�Ƃ��ɁA-blockname�܂œǂݔ�΂�
		/// �̂Ɏg���B
		/// </remarks>
        public bool SkipTo(string strSkipTo)
        {
            string str1, str2;
            while (GetRecord(out str1, out str2)){
                if (str1.Equals(strSkipTo)) return true;
            }
            return false;
        }

		/// <summary>
		/// CE3������(%xx ���܂�)�̃f�R�[�h
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		static public string DecodeCE3String(string str)
		{
			StringBuilder strDst = new StringBuilder();
			int i;
			int decodeState = 0;
			char[] decodeStr = new char[2];
			for (i = 0; i < str.Length; i++) {
				if (decodeState == 0) {
					if (str[i] == '%') {
						decodeState = 1;
					} else {
						strDst.Append(str[i]);
					}
				}else if(decodeState==1){
					decodeStr[0]= str[i];
					decodeState = 2;
				}else{
					decodeStr[1]= str[i];
					string strHexDecimal = new string(decodeStr);
					char c;
					try { c = (char)Convert.ToInt16(strHexDecimal, 16); }
					catch (Exception) { c = ' '; }
					//strDst.Append((char)Convert.ToInt16(strHexDecimal, 16));
					strDst.Append(c);
					decodeState = 0;
				}
			}
			return strDst.ToString();
		}
	}
}
