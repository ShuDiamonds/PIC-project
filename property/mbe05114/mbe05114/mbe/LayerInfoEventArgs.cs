using System;
using System.Collections.Generic;
using System.Text;

namespace mbe
{
	/// <summary>
	/// ���C���[����`����C�x���g�N���X
	/// </summary>
	public class LayerInfoEventArgs : EventArgs
	{
		public ulong selectableLayer;
		public MbeLayer.LayerValue selectLayer;
		public ulong relateiveLayer;
		public ulong visibleLayer;
		public LayerInfoEventArgs()
		{
			selectableLayer = 0;
			selectLayer = MbeLayer.LayerValue.DOC;
			relateiveLayer = 0;
			visibleLayer = 0;
		}
	}
}
