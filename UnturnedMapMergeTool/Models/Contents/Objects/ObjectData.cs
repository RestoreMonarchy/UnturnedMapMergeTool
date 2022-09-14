using System;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Objects
{
    public class ObjectData
    {
        public Vector3 Position { get; set; }
        public EulerAngles Rotation { get; set; }
        public Vector3 LocalScale { get; set; }
        public ushort AssetId { get; set; }
        public Guid Guid { get; set; }
        public byte PlacementOrigin { get; set; }
        public uint InstanceId { get; set; }
    }
}
