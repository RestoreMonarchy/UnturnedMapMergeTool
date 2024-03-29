﻿using System.Collections.Generic;

namespace UnturnedMapMergeTool.Models.Contents.Vehicles
{
    public class VehicleTierData
    {
        public string Name { get; set; }
        public float Chance { get; set; }
        public byte SpawnsCount { get; set; }
        public List<VehicleSpawnData> Spawns { get; set; }
    }
}
