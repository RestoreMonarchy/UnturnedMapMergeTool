using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedMapMergeTool.Models.Contents.Vehicles
{
    public class VehicleItemJarData
    {
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Rotation { get; set; }
        public ushort AssetId { get; set; }
        public byte Amount { get; set; }
        public byte Quality { get; set; }
        public byte[] State { get; set; }
    }
}
