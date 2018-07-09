using System;
using System.Collections.Generic;
using System.Text;

namespace mbe
{
    public struct GenOutlineParam
    {
        public MbeRect rc;
        public MbeLayer.LayerValue layer;
        public int traceWidth;
        public int gap;
        public int option;

        public const int P0_NO_LINECAP = 1;
        public const int P1_NO_LINECAP = 2;
    }
}
