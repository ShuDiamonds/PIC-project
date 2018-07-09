using System;
using System.Collections.Generic;
using System.Text;

namespace mbe
{
	public class MbeLayer
	{
		/// <summary>
		/// ���C���[�r�b�g enum���l
		/// </summary>
		/// <remarks>
		/// (LayerValue &amp; 0x0000000000000AAA)���[���ɂȂ�Ȃ��Ƃ��͔��c�ʑ�
		/// (LayerValue &amp; 0xFF00000000000000)���[���ɂȂ�Ȃ����C���[�͖ʈˑ����Ȃ�
		/// </remarks>
		public enum LayerValue : ulong
		{
			NUL =	0,
			PLC =	0x0000000000000001L,
			PLS =	0x0000000000000002L,
			STC =	0x0000000000000004L,
			STS =	0x0000000000000008L,
			CMP =	0x0000000000000010L,
			SOL =	0x0000000000000020L,
            L2  =   0x0000000000000100L,
            L3  =   0x0000000000000200L,
            MMC =   0x0000000000000400L,
            MMS =   0x0000000000000800L,
            DIM =   0x8000000000000000L,
			PTH =   0x4000000000000000L,
			DRL =	0x2000000000000000L,
			DOC =	0x1000000000000040L
		};


        public static ulong PatternFilmLayer
        {
            get
            {
                return ((ulong)LayerValue.CMP |
                            (ulong)LayerValue.SOL |
                            (ulong)LayerValue.STC |
                            (ulong)LayerValue.STS |
                            (ulong)LayerValue.MMC |
                            (ulong)LayerValue.MMS |
                            (ulong)LayerValue.L2 |
                            (ulong)LayerValue.L3 |
                            (ulong)LayerValue.PTH);

            }
        }

		public enum LayerIndex	//���̏����ŕ`�悳���
		{
			PTH=0,
            MMC,
			PLC,
			STC,
			CMP,
            L2,
            L3,
			SOL,
			STS,
			PLS,
            MMS,
			DIM,
			DRL,
			DOC
		};


		/// <summary>
		/// ���C���[�� ������e�[�u��
		/// </summary>
		public static readonly string[] nameTable = 
		{ 
			"PTH",
            "MMC",
			"PLC",
			"STC", 
			"CMP",
            "L2",
            "L3",
			"SOL", 
			"STS", 
			"PLS",
            "MMS",
			"DIM",
			"DRL",
			"DOC"
		};

		/// <summary>
		/// ���C���[ �r�b�g�l�e�[�u��
		/// </summary>
		public static readonly LayerValue[] valueTable =
		{
			LayerValue.PTH,
            LayerValue.MMC,
			LayerValue.PLC,
			LayerValue.STC,
			LayerValue.CMP,
            LayerValue.L2,
            LayerValue.L3,
			LayerValue.SOL,
			LayerValue.STS,
			LayerValue.PLS,
			LayerValue.MMS,
			LayerValue.DIM,
			LayerValue.DRL,
			LayerValue.DOC
		};


		/// <summary>
		/// LayerValue�l�ɑΉ����郌�C���[���𓾂�
		/// </summary>
		/// <param name="layer"></param>
		/// <returns></returns>
		public static string GetLayerName(LayerValue layer)
		{
			int count = valueTable.Length;
			for (int i = 0; i < count; i++) {
				if (valueTable[i] == layer) {
					return nameTable[i];
				}
			}
			return "DOC";
		}

		/// <summary>
		/// name �ɑΉ����� LayerValue��Ԃ��B
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static LayerValue GetLayerValue(string name)
		{
			int count = nameTable.Length;
			for (int i = 0; i < count; i++) {
				if (nameTable[i] == name) {
					return valueTable[i];
				}
			}
			return LayerValue.DOC;
		}

		/// <summary>
		/// ���i�ʁA�܂��͖ʈˑ����̂Ȃ����C���[���ǂ����𒲂ׂ�
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsComponentSide(LayerValue value)
		{
			return ((ulong)value & 0x0000000000000AAAL)==0;
		}

		/// <summary>
		/// ���C���[�l��ʔ��]����
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static LayerValue Flip(LayerValue value)
		{
			switch(value){
				case LayerValue.PLC:	return LayerValue.PLS;
				case LayerValue.PLS:	return LayerValue.PLC;
				case LayerValue.STC:	return LayerValue.STS;
				case LayerValue.STS:	return LayerValue.STC;
				case LayerValue.CMP:	return LayerValue.SOL;
				case LayerValue.SOL:	return LayerValue.CMP;
                case LayerValue.L2:     return LayerValue.L3;
                case LayerValue.L3:     return LayerValue.L2;
                case LayerValue.MMC:    return LayerValue.MMS;
                case LayerValue.MMS:    return LayerValue.MMC;

				default: return value; //DIM,DOC,PTH,DRL�̓��C���[�𔽓]���Ȃ�
			}
		}
	}
}
