using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Trees;
using UnturnedMapMergeTool.Services;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class TreesDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<TreesDataContent>> Data { get; set; } = new List<CopyMapData<TreesDataContent>>();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 5;
            TreesDataContent content = new(saveDataVersion);

            foreach (CopyMapData<TreesDataContent> dataItem in Data)
            {
                IEnumerable<TreeData> copyMapBuildables = dataItem.Content.TreeRegions.SelectMany(x => x.Trees);

                foreach (TreeData treeData in copyMapBuildables)
                {
                    if (!dataItem.CopyMap.ShouldIncludePosition(treeData.Position))
                    {
                        //Log.Warning($"TREE: Skipping tree outside of the border");
                        continue;
                    }

                    TreeData shiftedTreeData = new()
                    {
                        Position = treeData.Position,
                        Guid = treeData.Guid,
                        AssetId = treeData.AssetId,
                        IsGenerated = treeData.IsGenerated
                    };

                    dataItem.CopyMap.ApplyPositionShift(shiftedTreeData.Position);

                    if (!Regions.tryGetCoordinate(shiftedTreeData.Position, out byte regionX, out byte regionY))
                    {
                        //Log.Warning($"TREE: Failed to get coordinates for {shiftedTreeData.Position}");
                        continue;
                    }

                    TreeRegionData treeRegionData = content.TreeRegions.First(x => x.RegionX == regionX && x.RegionY == regionY);
                    treeRegionData.Trees.Add(shiftedTreeData);
                    treeRegionData.Count++;
                }
            }

            string treesSavePath = outputMap.CombinePath("Terrain/Trees.dat");

            content.SaveToFile(treesSavePath);

            Log.Information($"Combined and saved {content.TreeRegions.Sum(x => x.Count)} trees");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Terrain/Trees.dat");
            TreesDataContent content = TreesDataContent.FromFile(fileNamePath);

            // Write to json file for debug
            File.WriteAllText($"trees_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            CopyMapData<TreesDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            int treesCount = content.TreeRegions.Sum(x => x.Count);
            Log.Information($"Read {treesCount} trees");
        }
    }
}
