using System;
using System.Collections.Generic;
using System.Text;

namespace mbe
{
	class LengthString
	{
		public enum UNIT
		{
			mm,
			cm,
			mil,
			inch
		}

		public static double ToDouble(string _str,double errorValue)
		{
			double d;
			try {
				string str = _str.ToUpper();
				int length = str.Length;
				if (length > 4 && str.Substring(length - 4).Equals("INCH")) {
					d = Convert.ToDouble(str.Substring(0, length - 4));
					d *= 25.4;
				} else if (length > 3 && str.Substring(length - 3).Equals("MIL")) {
					d = Convert.ToDouble(str.Substring(0, length - 3));
					d *= 0.0254;
				} else if (length > 2 && str.Substring(length - 2).Equals("CM")) {
					d = Convert.ToDouble(str.Substring(0, length - 2));
					d *= 10.0;
				} else if (length > 2 && str.Substring(length - 2).Equals("MM")) {
					d = Convert.ToDouble(str.Substring(0, length - 2));
				} else {
					d = Convert.ToDouble(str);
					return d;
				}
			}
			catch {
				return errorValue;
			}
			switch (unit) {
				case UNIT.inch:
					return d / 25.4;
				case UNIT.mil:
					return d / 0.0254;
				case UNIT.cm:
					return d / 10;
				default:
					return d;
			}
		}
		

		private static UNIT unit = UNIT.mm;

		public static UNIT Unit
		{
			get { return unit; }
			set { unit = value; }
		}
	}
}
