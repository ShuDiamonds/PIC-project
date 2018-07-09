using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using CE3IO;

namespace mbe
{	/// <summary>
	/// �t�@�C�����o�͂̃w�b�_�[�B�h�L�������g�ŗL�̏��������B
	/// </summary>
	class MbeDocInfo
	{
		/// <summary>
		/// ���[�U�[���ݒ肷�郏�[�N�G���A�T�C�Y�̐ݒ�Ǝ擾
		/// </summary>
		public Size SizeWorkArea
		{
			get { return sizeWorkArea; }
			set {
                SetWorkAreaWidth(value.Width);
                SetWorkAreaHeight(value.Height);
            }
		}

        public void SetWorkAreaWidth(int width)
        {
            if (width > MbeDoc.WORK_AREA_MAX_WIDTH) {
                width = MbeDoc.WORK_AREA_MAX_WIDTH;
            } else if (width < MbeDoc.WORK_AREA_MIN_WIDTH) {
                width = MbeDoc.WORK_AREA_MIN_WIDTH;
            }
            sizeWorkArea.Width = width;
        }

        public void SetWorkAreaHeight(int height)
        {
            if (height > MbeDoc.WORK_AREA_MAX_HEIGHT) {
                height = MbeDoc.WORK_AREA_MAX_HEIGHT;
            } else if (height < MbeDoc.WORK_AREA_MIN_HEIGHT) {
                height = MbeDoc.WORK_AREA_MIN_HEIGHT;
            }
            sizeWorkArea.Height = height;
        }


        public const int DATA_VERSION = 51;

        public int  FileDataVersion
        {
            get { return fileDataVersion; }
        }

		/// <summary>
		/// �����C���[�̐ݒ�Ǝ擾
		/// </summary>
		//public ulong VisibleLayer
		//{
		//    get { return visibleLayer; }
		//    set { visibleLayer = value; }
		//}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public MbeDocInfo()
		{
			//visibleLayer = (ulong)MbeLayer.LayerValue.PTH |
			//                    (ulong)MbeLayer.LayerValue.CMP |
			//                    (ulong)MbeLayer.LayerValue.SOL |
			//                    (ulong)MbeLayer.LayerValue.PLC |
			//                    (ulong)MbeLayer.LayerValue.PLS |
			//                    (ulong)MbeLayer.LayerValue.DOC |
			//                    (ulong)MbeLayer.LayerValue.DIM;
			//sizeWorkArea = new Size(MbeView.WORK_AREA_DEFAULT_WIDTH, MbeView.WORK_AREA_DEFAULT_HEIGHT);
            sizeWorkArea = new Size(MbeDoc.WORK_AREA_DEFAULT_WIDTH, MbeDoc.WORK_AREA_DEFAULT_HEIGHT);
            fileDataVersion = DATA_VERSION;
			//sizeWorkArea = new Size(3000000, 3000000);
		}


		/// <summary>
		/// Mb3�t�@�C���̓ǂݍ��ݎ��̃����o�[�̉��߂��s��
		/// </summary>
		/// <param name="str1">�ϐ����܂���"+"�Ŏn�܂�u���b�N�^�O</param>
		/// <param name="str2">�ϐ��l</param>
		/// <param name="readCE3">�u���b�N�ǂݍ��ݎ��Ɏg��ReadCE3�N���X</param>
		/// <returns>
		/// ����I������ReadCE3.RdStatus.NoError��Ԃ��B
		/// �����������_�ł͖{���\�b�h�͏��ReadCE3.RdStatus.NoError��Ԃ��B
		/// </returns>
		protected ReadCE3.RdStatus RdMb3Member(string str1, string str2, ReadCE3 readCE3)
		{
            int n;
			switch (str1) {
				case "HEIGHT":
					try { n = Convert.ToInt32(str2); }
					catch (Exception) { n = MbeDoc.WORK_AREA_DEFAULT_HEIGHT; }
                    SetWorkAreaHeight(n);
					return ReadCE3.RdStatus.NoError;
				case "WIDTH":
					try { n = Convert.ToInt32(str2); }
                    catch (Exception) { n = MbeDoc.WORK_AREA_DEFAULT_WIDTH; }
                    SetWorkAreaWidth(n);
					return ReadCE3.RdStatus.NoError;
                case "DATAVERSION":
                    try { fileDataVersion = Convert.ToInt32(str2); }
                    catch (Exception) { fileDataVersion = DATA_VERSION; }
                    return ReadCE3.RdStatus.NoError;

				//case "VISIBLE":
				//    VisibleLayer |= (ulong)MbeLayer.GetLayerValue(str2);
				//    return ReadCE3.RdStatus.NoError;
				default:
					return ReadCE3.RdStatus.NoError;
			}
		}

		/// <summary>
		/// ���̃N���X��Mb3�t�@�C���̓ǂݍ���
		/// </summary>
		/// <param name="readCE3">�ǂݍ��ݑΏۂ�ReadCE3�N���X</param>
		/// <returns>����I������ReadCE3.RdStatus.NoError ��Ԃ�</returns>
		public ReadCE3.RdStatus RdMb3(ReadCE3 readCE3)
		{
			//VisibleLayer = 0L;
			string str1;
			string str2;
			ReadCE3.RdStatus result = ReadCE3.RdStatus.NoError;
			while (readCE3.GetRecord(out str1, out str2)) {
				if (str1[0] == '-') {
					if (str1 != "-MBE_DOCINFO") {
						result = ReadCE3.RdStatus.FormatError;
					}
					break;
				} else {
					result = RdMb3Member(str1, str2, readCE3);
					if (result != ReadCE3.RdStatus.NoError) {
						break;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// ���̃N���X��Mb3�t�@�C���ւ̏����o��
		/// </summary>
		/// <param name="writeCE3"></param>
		/// <returns></returns>
		public virtual bool WrMb3(WriteCE3 writeCE3)
		{
			writeCE3.WriteRecord("+MBE_DOCINFO");
			writeCE3.WriteRecordInt("HEIGHT", sizeWorkArea.Height);
			writeCE3.WriteRecordInt("WIDTH", sizeWorkArea.Width);
            writeCE3.WriteRecordInt("DATAVERSION", DATA_VERSION);
			//writeCE3.WriteNewLine();
			//int count = MbeLayer.valueTable.Length;
			//for (int i = 0; i < count; i++) {
			//    if ((VisibleLayer & (ulong)MbeLayer.valueTable[i] )!=0) {
			//        writeCE3.WriteRecordString("VISIBLE", MbeLayer.nameTable[i]);
			//        writeCE3.WriteNewLine();
			//    }
			//}
			writeCE3.WriteRecord("-MBE_DOCINFO");
			writeCE3.WriteNewLine();
			return true;
		}

		//private ulong visibleLayer;
		private Size sizeWorkArea;
        private int fileDataVersion;

	}
}
