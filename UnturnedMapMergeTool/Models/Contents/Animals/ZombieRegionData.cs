using System.Collections.Generic;

namespace UnturnedMapMergeTool.Models.Contents.Animals
{
    public class ZombieRegionData
    {
        public byte RegionX { get; set; }
        public byte RegionY { get; set; }
        public ushort Count { get; set; }
        public List<ZombieSpawnpointData> ZombieSpawnpoints { get; set; }
    }
}
