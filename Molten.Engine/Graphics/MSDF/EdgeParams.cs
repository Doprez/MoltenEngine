﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Molten.Graphics.MSDF
{
    public struct EdgeParams
    {
        public SignedDistance minDistance;
        public EdgeSegment nearEdge;
        public double nearParam;
    }
}
