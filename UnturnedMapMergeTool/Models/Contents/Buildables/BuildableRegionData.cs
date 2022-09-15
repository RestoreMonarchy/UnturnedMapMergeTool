using System.Collections.Generic;

namespace UnturnedMapMergeTool.Models.Contents.Buildables
{
    public class BuildableRegionData
    {
        public ushort Count { get; set; }
        public byte RegionX { get; set; }
        public byte RegionY { get; set; }
        public List<BuildableData> Buildables { get; set; }
    }
}
