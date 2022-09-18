using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Items
{
    public class ItemTableData
    {
        public Color Color { get; set; }
        public string Name { get; set; }
        public ushort TableId { get; set; }
        public byte TiersCount { get; set; }
        public List<ItemTierData> Tiers { get; set; }
    }
}
