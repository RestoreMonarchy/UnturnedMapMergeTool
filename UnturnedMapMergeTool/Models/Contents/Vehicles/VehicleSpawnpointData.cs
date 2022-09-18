using System;
using System.Collections.Generic;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents.Vehicles
{
    public class VehicleSpawnpointData
    {
        public byte Type { get; set; }
        public Vector3 Point { get; set; }
        public byte Angle { get; set; }
    }
}
