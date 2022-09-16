﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedMapMergeTool.Models.Contents.Navigations
{
    public class NavmeshTileData
    {
        public ushort TrisCount { get; set; }
        public ushort[] Tris { get; set; }
        public ushort VertsCount { get; set; }
        public NavmeshTileVertData[] Verts { get; set; }
    }
}
