using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedMapMergeTool.Models.Contents.Paths
{
    public class PathLineData
    {
        public ushort Count { get; set; }
        public byte Material { get; set; }
        public bool NewLoop { get; set; }

        public List<PathJointData> Joints { get; set; }
    }
}
