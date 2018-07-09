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
		/// �R���X�g���N�^
		/// </summary>
		/// <remarks>���Ƃ���TextWriter��Attach����</remarks>
		public WriteCE3()
		{
			//
			// TODO: Add constructor logic here
			//
			newLine = true;
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public WriteCE3(TextWriter writer)
		{
			textWriter = writer;
			newLine = true;
		}

		/// <summary>
		/// �������ݑΏۂ�TextReader��ݒ�
		/// </summary>
		/// <param name="reader"></param>
		public void Attach(TextWriter writer)
		{
			textWriter = writer;
			newLine = true;
		}

		/// <summary>
		/// ���R�[�h�̏�������
		/// </summary>
		/// <param name="str"></param>
		/// <remarks>
		/// �s���ȊO�Ȃ�J���}��ł��Ă��烌�R�[�h����������
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
		/// ���s
		/// </summary>
		public void WriteNewLine()
		{
			textWriter.WriteLine();
			newLine = true;
		}

		/// <summary>
		/// �������R�[�h�̏�������
		/// </summary>
		/// <param name="name">�ϐ���</param>
		/// <param name="val">�����p�����[�^</param>
		public void WriteRecordInt(string name, int val)
		{
			WriteRecord(name + ":" + val);
		}


        /// <summary>
        /// ulong���R�[�h�̏�������
        /// </summary>
        /// <param name="name">�ϐ���</param>
        /// <param name="val">�p�����[�^</param>
        public void WriteRecordUlong(string name, ulong val)
        {
            WriteRecord(name + ":" + val);
        }

        
        /// <summary>
		/// �����񃌃R�[�h�̏�������
		/// </summary>
		/// <param name="name">�ϐ���</param>
		/// <param name="val">������p�����[�^</param>
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