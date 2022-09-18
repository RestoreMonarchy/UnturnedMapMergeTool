using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents.Animals;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class AnimalsDataContent
    {
        public byte SaveDataVersion { get; set; }
        public List<ZombieRegionData> ZombieRegions { get; set; }

        public AnimalsDataContent()
        {

        }

        public AnimalsDataContent(byte saveDataVersion)
        {
            SaveDataVersion = saveDataVersion;

            ZombieRegions = new List<ZombieRegionData>();

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    ZombieRegionData zombieRegionData = new()
                    {
                        RegionX = i,
                        RegionY = j,
                        Count = 0,
                        ZombieSpawnpoints = new List<ZombieSpawnpointData>()
                    };

                    ZombieRegions.Add(zombieRegionData);
                }
            }
        }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);
            river.writeByte(SaveDataVersion);

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    ZombieRegionData regionData = ZombieRegions.First(x => x.RegionX == i && x.RegionY == j);
                    river.writeUInt16(regionData.Count);

                    foreach (ZombieSpawnpointData zombieSpawnpoint in regionData.ZombieSpawnpoints)
                    {
                        river.writeByte(zombieSpawnpoint.Type);
                        river.writeSingleVector3(zombieSpawnpoint.Point);
                    }
                }
            }

            river.closeRiver();
        }

        public static AnimalsDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            AnimalsDataContent content = new();

            content.SaveDataVersion = river.readByte();

            content.ZombieRegions = new List<ZombieRegionData>();

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    ZombieRegionData region = new()
                    {
                        RegionX = i,
                        RegionY = j
                    };
                    
                    region.Count = river.readUInt16();

                    region.ZombieSpawnpoints = new List<ZombieSpawnpointData>();

                    for (ushort k = 0; k < region.Count; k++)
                    {
                        ZombieSpawnpointData zombieSpawnpoint = new();

                        zombieSpawnpoint.Type = river.readByte();
                        zombieSpawnpoint.Point = river.readSingleVector3();

                        region.ZombieSpawnpoints.Add(zombieSpawnpoint);
                    }

                    content.ZombieRegions.Add(region);
                }
            }

            river.closeRiver();

            return content;
        }
    }
}
