using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Animals;

namespace UnturnedMapMergeTool.Models.Contents.Jars
{
    public class ItemRegionData
    {
        public byte RegionX { get; set; }
        public byte RegionY { get; set; }
        public ushort SpawnpointsCount { get; set; }
        public List<ItemSpawnpointData> Spawnpoints { get; set; }
    }
}
