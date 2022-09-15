using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents.Trees;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class TreesDataContent
    {
        public byte SaveDataTreesVersion { get; set; }
        public List<TreeRegionData> TreeRegions { get; set; }

        public TreesDataContent()
        {

        }

        public TreesDataContent(byte saveDataTreesVersion)
        {
            SaveDataTreesVersion = saveDataTreesVersion;

            TreeRegions = new List<TreeRegionData>();

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    TreeRegionData objectRegionData = new()
                    {
                        RegionX = i,
                        RegionY = j,
                        Count = 0,
                        Trees = new List<TreeData>()
                    };

                    TreeRegions.Add(objectRegionData);
                }
            }
        }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);
            river.writeByte(SaveDataTreesVersion);

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    TreeRegionData regionData = TreeRegions.First(x => x.RegionX == i && x.RegionY == j);
                    river.writeUInt16(regionData.Count);

                    foreach (TreeData treeData in regionData.Trees)
                    {
                        river.writeUInt16(treeData.AssetId);
                        river.writeSingleVector3(treeData.Position);
                        river.writeBoolean(treeData.IsGenerated);                        
                    }
                }
            }

            river.closeRiver();
        }

        public static TreesDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            TreesDataContent content = new();

            content.SaveDataTreesVersion = river.readByte();

            content.TreeRegions = new List<TreeRegionData>();

            for (byte i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (byte j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    TreeRegionData region = new()
                    {
                        RegionX = i,
                        RegionY = j
                    };

                    region.Count = river.readUInt16();

                    region.Trees = new List<TreeData>();

                    for (ushort k = 0; k < region.Count; k++)
                    {
                        TreeData treeData = new();

                        treeData.AssetId = river.readUInt16();
                        treeData.Position = river.readSingleVector3();
                        treeData.IsGenerated = river.readBoolean();
                        
                        region.Trees.Add(treeData);
                    }

                    content.TreeRegions.Add(region);
                }
            }
            river.closeRiver();

            return content;
        }
    }
}
