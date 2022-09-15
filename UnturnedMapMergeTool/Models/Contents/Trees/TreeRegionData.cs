using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Objects;

namespace UnturnedMapMergeTool.Models.Contents.Trees
{
    public class TreeRegionData
    {
        public ushort Count { get; set; }
        public byte RegionX { get; set; }
        public byte RegionY { get; set; }
        public List<TreeData> Trees { get; set; }
    }
}
