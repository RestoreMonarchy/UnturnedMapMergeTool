using Serilog;
using System;
using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Vehicles;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class VehiclesDataContent
    {
        public VehiclesDataContent(byte saveDataVersion, byte tablesCount, ushort spawnpointsCount)
        {
            SaveDataVersion = saveDataVersion;
            TablesCount = tablesCount;
            SpawnpointsCount = spawnpointsCount;

            Tables = new();
            Spawnpoints = new();
        }

        public VehiclesDataContent()
        {

        }

        public byte SaveDataVersion { get; set; }

        public byte TablesCount { get; set; }
        public List<VehicleTableData> Tables { get; set; }

        public ushort SpawnpointsCount { get; set; }
        public List<VehicleSpawnpointData> Spawnpoints { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            river.writeByte(SaveDataVersion);
            river.writeByte(TablesCount);

            foreach (VehicleTableData vehicleTable in Tables)
            {
                river.writeColor(vehicleTable.Color);
                river.writeString(vehicleTable.Name);
                river.writeUInt16(vehicleTable.TableId);
                river.writeByte(vehicleTable.TiersCount);

                foreach (VehicleTierData vehicleTier in vehicleTable.Tiers)
                {
                    river.writeString(vehicleTier.Name);
                    river.writeSingle(vehicleTier.Chance);
                    river.writeByte(vehicleTier.SpawnsCount);

                    foreach (VehicleSpawnData vehicleSpawn in vehicleTier.Spawns)
                    {
                        river.writeUInt16(vehicleSpawn.VehicleId);
                    }
                }
            }

            river.writeUInt16(SpawnpointsCount);
            
            foreach (VehicleSpawnpointData vehicleSpawnpoint in Spawnpoints)
            {
                river.writeByte(vehicleSpawnpoint.Type);
                river.writeSingleVector3(vehicleSpawnpoint.Point);
                river.writeByte(vehicleSpawnpoint.Angle);
            }

            river.closeRiver();
        }

        public static VehiclesDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            VehiclesDataContent content = new();
            
            content.SaveDataVersion = river.readByte();
            
            if (content.SaveDataVersion > 1 && content.SaveDataVersion < 3)
            {
                river.readSteamID();
            }
            
            content.TablesCount = river.readByte();
            content.Tables = new();

            for (byte i = 0; i < content.TablesCount; i++)
            {
                VehicleTableData vehicleTable = new();
                vehicleTable.Color = river.readColor();
                vehicleTable.Name = river.readString();
                
                if (content.SaveDataVersion > 3)
                {
                    vehicleTable.TableId = river.readUInt16();
                } else
                {
                    vehicleTable.TableId = 0;
                }

                vehicleTable.TiersCount = river.readByte();
                vehicleTable.Tiers = new();

                for (byte j = 0; j < vehicleTable.TiersCount; j++)
                {
                    VehicleTierData vehicleTier = new();
                    vehicleTier.Name = river.readString();
                    vehicleTier.Chance = river.readSingle();
                    vehicleTier.SpawnsCount = river.readByte();
                    vehicleTier.Spawns = new();

                    for (byte k = 0; k < vehicleTier.SpawnsCount; k++)
                    {
                        VehicleSpawnData vehicleSpawn = new();
                        vehicleSpawn.VehicleId = river.readUInt16();
                        vehicleTier.Spawns.Add(vehicleSpawn);
                    }

                    vehicleTable.Tiers.Add(vehicleTier);
                }

                content.Tables.Add(vehicleTable);
            }

            content.SpawnpointsCount = river.readUInt16();
            content.Spawnpoints = new();

            for (ushort l = 0; l < content.SpawnpointsCount; l++)
            {
                VehicleSpawnpointData vehicleSpawnpoint = new();
                vehicleSpawnpoint.Type = river.readByte();
                vehicleSpawnpoint.Point = river.readSingleVector3();
                vehicleSpawnpoint.Angle = river.readByte();

                content.Spawnpoints.Add(vehicleSpawnpoint);
            }

            river.closeRiver();
            return content;
        }
    }
}
