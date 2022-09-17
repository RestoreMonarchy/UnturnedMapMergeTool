using System.Collections.Generic;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Faunas
{
    public class AnimalTableData
    {
        public Color Color { get; set; }
        public string Name { get; set; }
        public ushort TableId { get; set; }
        public byte TiersCount { get; set; }
        public List<AnimalTierData> Tiers { get; set; }
    }
}
