using System;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Trees
{
    public class TreeData
    {
        public ushort AssetId { get; set; }
        public Guid Guid { get; set; }
        public Vector3 Position { get; set; }
        public bool IsGenerated { get; set; }
    }
}
