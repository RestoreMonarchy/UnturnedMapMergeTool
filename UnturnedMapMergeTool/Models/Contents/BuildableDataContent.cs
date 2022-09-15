using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents.Buildables;
using UnturnedMapMergeTool.Models.Contents.Objects;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class BuildableDataContent
    {
        public byte SaveDataVersion { get; set; }
        public List<BuildableRegionData> BuildableRegions { get; set; }

        public BuildableDataContent()
        {

        }

        public BuildableDataContent(byte saveDataVersion)
        {
            SaveDataVersion = saveDataVersion;

            BuildableRegions = new List<BuildableRegionData>();

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    BuildableRegionData buildableRegionData = new()
                    {
                        RegionX = i,
                        RegionY = j,
                        Count = 0,
                        Buildables = new List<BuildableData>()
                    };

                    BuildableRegions.Add(buildableRegionData);
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
                    BuildableRegionData regionData = BuildableRegions.First(x => x.RegionX == i && x.RegionY == j);
                    river.writeUInt16(regionData.Count);

                    foreach (BuildableData buildableData in regionData.Buildables)
                    {
                        river.writeSingleVector3(buildableData.Position);
                        river.writeSingleQuaternion(buildableData.Rotation);
                        river.writeUInt16(buildableData.AssetId);
                    }
                }
            }

            river.closeRiver();
        }

        public static BuildableDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            BuildableDataContent content = new();

            content.SaveDataVersion = river.readByte();

            content.BuildableRegions = new List<BuildableRegionData>();

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    BuildableRegionData region = new()
                    {
                        RegionX = i,
                        RegionY = j
                    };

                    region.Count = river.readUInt16();

                    region.Buildables = new List<BuildableData>();

                    for (ushort k = 0; k < region.Count; k++)
                    {
                        BuildableData buildableData = new();

                        buildableData.Position = river.readSingleVector3();
                        buildableData.Rotation = river.readSingleQuaternion();
                        buildableData.AssetId = river.readUInt16();

                        region.Buildables.Add(buildableData);
                    }

                    content.BuildableRegions.Add(region);
                }
            }
            river.closeRiver();

            return content;
        }
    }
}
