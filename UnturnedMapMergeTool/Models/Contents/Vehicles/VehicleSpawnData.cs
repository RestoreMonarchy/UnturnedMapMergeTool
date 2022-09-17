using System;
using System.Collections.Generic;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Vehicles
{
    public class VehicleSpawnData
    {
        public ushort AssetId { get; set; }
        public Guid Guid { get; set; }
        public uint InstanceId { get; set; }
        public ushort SkinId { get; set; }
        public ushort MythicId { get; set; }
        public float RoadPosition { get; set; }
        public Vector3 Point { get; set; }
        public EulerAngles Angles { get; set; }
        public ushort Fuel { get; set; }
        public ushort Health { get; set; }
        public ushort BatteryCharge { get; set; }
        public Guid BatteryItemGuid { get; set; }
        public byte TireAliveMask { get; set; }
        public CSteamID Owner { get; set; }
        public CSteamID Group { get; set; }
        public bool Locked { get; set; }
        public byte[][] Array { get; set; }
        public bool ItemJarFlag { get; set; }
        public byte ItemJarsCount { get; set; }
        public List<VehicleItemJarData> ItemJars { get; set; }

        public float DecayTimer { get; set; }
    }
}
