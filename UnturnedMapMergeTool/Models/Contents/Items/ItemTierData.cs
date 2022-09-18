using System.Collections.Generic;

namespace UnturnedMapMergeTool.Models.Contents.Items
{
    public class ItemTierData
    {
        public string Name { get; set; }
        public float Chance { get; set; }
        public byte SpawnsCount { get; set; }
        public List<ItemSpawnData> Spawns { get; set; }
    }
}
