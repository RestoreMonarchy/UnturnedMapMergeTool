using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Paths
{
    public class PathJointData
    {
        public Vector3 Vertex { get; set; }
        public Vector3[] Tangents { get; set; }
        public byte RoadMode { get; set; }
        public float Offset { get; set; }
        public bool IgnoreTerrain { get; set; }
    }
}
