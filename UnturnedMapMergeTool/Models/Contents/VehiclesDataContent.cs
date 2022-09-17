using Serilog;
using System;
using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Vehicles;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class VehiclesDataContent
    {
        public byte SaveDataVersion { get; set; }
        
        public ushort VehicleSpawnsCount { get; set; }
        public List<VehicleSpawnData> VehicleSpawns { get; set; }

        public static VehiclesDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            VehiclesDataContent content = new();
            content.VehicleSpawns = new();

            content.SaveDataVersion = river.readByte();
            content.VehicleSpawnsCount = river.readUInt16();

            if (content.SaveDataVersion < 14)
            {
                Log.Warning($"Save data version {content.SaveDataVersion} cannot be combined, because it uses ids instead of guids!");
            }

            for (ushort i = 0; i < content.VehicleSpawnsCount; i++)
            {
                VehicleSpawnData vehicleSpawn = new();

                if (content.SaveDataVersion < 14)
                {
                    vehicleSpawn.AssetId = river.readUInt16();
                } else
                {
                    vehicleSpawn.Guid = river.readGUID();
                }

                if (content.SaveDataVersion >= 12)
                {
                    vehicleSpawn.InstanceId = river.readUInt32();
                }

                if (content.SaveDataVersion >= 8)
                {
                    vehicleSpawn.SkinId = river.readUInt16();
                }

                if (content.SaveDataVersion >= 9)
                {
                    vehicleSpawn.MythicId = river.readUInt16();
                }

                if (content.SaveDataVersion >= 10)
                {
                    vehicleSpawn.RoadPosition = river.readSingle();
                }

                vehicleSpawn.Point = river.readSingleVector3();
                vehicleSpawn.Angles = river.readSingleQuaternion();
                vehicleSpawn.Fuel = river.readUInt16();
                vehicleSpawn.Health = river.readUInt16();

                if (content.SaveDataVersion > 5)
                {
                    vehicleSpawn.BatteryCharge = river.readUInt16();
                }

                if (content.SaveDataVersion >= 15)
                {
                    vehicleSpawn.BatteryItemGuid = river.readGUID();
                } else
                {
                    vehicleSpawn.BatteryItemGuid = Guid.Empty;
                }

                if (content.SaveDataVersion > 6)
                {
                    vehicleSpawn.TireAliveMask = river.readByte();
                }

                if (content.SaveDataVersion > 4)
                {
                    vehicleSpawn.Owner = river.readSteamID();
                    vehicleSpawn.Group = river.readSteamID();
                    vehicleSpawn.Locked = river.readBoolean();
                }

                if (content.SaveDataVersion > 3)
                {
                    vehicleSpawn.Array = new byte[river.readByte()][];
                    for (byte j = 0; j < vehicleSpawn.Array.Length; j++)
                    {
                        vehicleSpawn.Array[j] = river.readBytes();
                    }
                }

                vehicleSpawn.ItemJarFlag = false;
                if (content.SaveDataVersion >= 11)
                {
                    vehicleSpawn.ItemJarFlag = river.readBoolean();
                }

                if (vehicleSpawn.ItemJarFlag)
                {
                    vehicleSpawn.ItemJarsCount = river.readByte();
                    vehicleSpawn.ItemJars = new();

                    for (byte k = 0; k < vehicleSpawn.ItemJarsCount; k++)
                    {
                        VehicleItemJarData itemJar = new();

                        itemJar.X = river.readByte();
                        itemJar.Y = river.readByte();
                        itemJar.Rotation = river.readByte();
                        itemJar.AssetId = river.readUInt16();
                        itemJar.Amount = river.readByte();
                        itemJar.Quality = river.readByte();
                        itemJar.State = river.readBytes();
                    }
                }

                if (content.SaveDataVersion >= 13)
                {
                    vehicleSpawn.DecayTimer = river.readSingle();
                }

                content.VehicleSpawns.Add(vehicleSpawn);
            }

            river.closeRiver();
            return content;
        }
    }
}
