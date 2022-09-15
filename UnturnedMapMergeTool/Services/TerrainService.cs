using System;
using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Trees;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Services
{
    public class TerrainService
    {
        private readonly IEnumerable<CopyMap> copyMaps;
        private readonly OutputMap outputMap;

        public TerrainService(IEnumerable<CopyMap> copyMaps, OutputMap outputMap)
        {
            this.copyMaps = copyMaps;
            this.outputMap = outputMap;
        }

        public void CombineAndSaveTrees()
        {
            byte saveDataVersion = 5;
            TreesDataContent content = new(saveDataVersion);

            foreach (CopyMap copyMap in copyMaps)
            {
                IEnumerable<TreeData> copyMapBuildables = copyMap.TreesDataContent.TreeRegions.SelectMany(x => x.Trees);

                foreach (TreeData treeData in copyMapBuildables)
                {
                    TreeData shiftedTreeData = new()
                    {
                        Position = treeData.Position,
                        AssetId = treeData.AssetId,
                        IsGenerated = treeData.IsGenerated
                    };

                    // Yeah Y is actually Z in unity Vector3
                    shiftedTreeData.Position.x += copyMap.Config.ShiftX;
                    shiftedTreeData.Position.z += copyMap.Config.ShiftY;

                    Regions.tryGetCoordinate(shiftedTreeData.Position, out byte regionX, out byte regionY);

                    TreeRegionData treeRegionData = content.TreeRegions.FirstOrDefault(x => x.RegionX == regionX && x.RegionY == regionY);
                    treeRegionData.Trees.Add(shiftedTreeData);
                    treeRegionData.Count++;
                }
            }

            string treesSavePath = outputMap.CombinePath("Terrain/Trees.dat");

            content.SaveToFile(treesSavePath);

            Log($"Combined and saved {content.TreeRegions.Sum(x => x.Count)} trees");
        }

        private void Log(string message)
        {
            Console.WriteLine($"[TerrainService]: {message}");
        }
    }
}
