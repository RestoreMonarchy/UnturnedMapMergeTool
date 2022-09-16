using System.Collections.Generic;

namespace UnturnedMapMergeTool.Models.Contents.Zombies
{
    public class ZombieSlotData
    {
        public float Chance { get; set; }
        public byte ClothesCount { get; set; }

        public List<ZombieClothData> Clothes { get; set; }
    }
}
