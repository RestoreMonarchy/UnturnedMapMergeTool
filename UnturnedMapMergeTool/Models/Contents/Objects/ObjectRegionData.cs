using System.Collections.Generic;

namespace UnturnedMapMergeTool.Models.Contents.Objects
{
    public class ObjectRegionData
    {
        public ushort Count { get; set; }
        public byte RegionX { get; set; }
        public byte RegionY { get; set; }
        public List<ObjectData> Objects { get; set; }
    }
}
