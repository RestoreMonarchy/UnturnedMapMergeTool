using System.Collections.Generic;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Vehicles
{
    public class VehicleTableData
    {
        public Color Color { get; set; }
        public string Name { get; set; }
        public ushort TableId { get; set; }
        public byte TiersCount { get; set; }
        public List<VehicleTierData> Tiers { get; set; }
    }
}
