﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Molten.Graphics.Font
{
    abstract class FontTable
    {

    }

    abstract class FontTableParser
    {
        public abstract FontTable Parse(BinaryEndianAgnosticReader reader, TableHeader header, Logger log);

        /// <summary>Gets the table's expected tag string (e.g. cmap, CFF, head, name, OS/2).</summary>
        public abstract string TableTag { get; }
    }
}
