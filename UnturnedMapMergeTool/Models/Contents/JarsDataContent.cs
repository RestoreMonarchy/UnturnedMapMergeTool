using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents.Jars;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class JarsDataContent
    {
        public JarsDataContent()
        {

        }

        public JarsDataContent(byte saveDataVersion)
        {
            SaveDataVersion = saveDataVersion;

            ItemRegions = new List<ItemRegionData>();

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    ItemRegionData itemRegion = new()
                    {
                        RegionX = i,
                        RegionY = j,
                        SpawnpointsCount = 0,
                        Spawnpoints = new List<ItemSpawnpointData>()
                    };

                    ItemRegions.Add(itemRegion);
                }
            }
        }

        public byte SaveDataVersion { get; set; }
        public List<ItemRegionData> ItemRegions { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);
            river.writeByte(SaveDataVersion);

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    ItemRegionData regionData = ItemRegions.First(x => x.RegionX == i && x.RegionY == j);
                    river.writeUInt16(regionData.SpawnpointsCount);

                    foreach (ItemSpawnpointData itemSpawnpoint in regionData.Spawnpoints)
                    {
                        river.writeByte(itemSpawnpoint.Type);
                        river.writeSingleVector3(itemSpawnpoint.Point);
                    }
                }
            }

            river.closeRiver();
        }

        public static JarsDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            JarsDataContent content = new();

            content.SaveDataVersion = river.readByte();

            content.ItemRegions = new List<ItemRegionData>();

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    ItemRegionData region = new()
                    {
                        RegionX = i,
                        RegionY = j
                    };

                    region.SpawnpointsCount = river.readUInt16();

                    region.Spawnpoints = new List<ItemSpawnpointData>();

                    for (ushort k = 0; k < region.SpawnpointsCount; k++)
                    {
                        ItemSpawnpointData itemSpawnpoint = new();

                        itemSpawnpoint.Type = river.readByte();
                        itemSpawnpoint.Point = river.readSingleVector3();

                        region.Spawnpoints.Add(itemSpawnpoint);
                    }

                    content.ItemRegions.Add(region);
                }
            }

            river.closeRiver();

            return content;
        }
    }
}
