using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Nodes
{
    public class NodeData
    {
        public Vector3 Point { get; set; }
        public byte Type { get; set; }
        public string LocationName { get; set; }
        public float Radius { get; set; }
        public bool IsHeight { get; set; }
        public bool NoWeapons { get; set; }
        public bool NoBuildables { get; set; }
        public ushort AssetId { get; set; }
        public uint Cost { get; set; }
        public byte DeadzoneType { get; set; }
        public byte Shape { get; set; }
        public Vector3 Bounds { get; set; }
        public bool NoWater { get; set; }
        public bool NoLighting { get; set; }
    }
}
