using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedMapMergeTool.Models.Contents.Nodes
{
    public class SafezoneNodeData
    {
        public float Radius { get; set; }
        public bool IsHeight { get; set; }
        public bool NoWeapons { get; set; }
        public bool NoBuildables { get; set; }
    }
}
