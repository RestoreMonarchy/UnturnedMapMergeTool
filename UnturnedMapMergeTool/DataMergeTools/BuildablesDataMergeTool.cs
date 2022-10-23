using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Buildables;
using UnturnedMapMergeTool.Services;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class BuildablesDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<BuildableDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 10;
            BuildableDataContent content = new(saveDataVersion);

            foreach (CopyMapData<BuildableDataContent> dataItem in Data)
            {
                IEnumerable<BuildableData> copyMapBuildables = dataItem.Content.BuildableRegions.SelectMany(x => x.Buildables);

                foreach (BuildableData buildableData in copyMapBuildables)
                {
                    BuildableData shiftedBuildableData = new()
                    {
                        Position = buildableData.Position,
                        AssetId = buildableData.AssetId,
                        Rotation = buildableData.Rotation
                    };

                    // Yeah Y is actually Z in unity Vector3
                    shiftedBuildableData.Position.x += dataItem.CopyMap.Config.ShiftX;
                    shiftedBuildableData.Position.z += dataItem.CopyMap.Config.ShiftY;

                    Regions.tryGetCoordinate(shiftedBuildableData.Position, out byte regionX, out byte regionY);

                    BuildableRegionData objectRegionData = content.BuildableRegions.FirstOrDefault(x => x.RegionX == regionX && x.RegionY == regionY);

                    if (objectRegionData == null)
                    {
                        Log.Warning("Object region not found for the shifted region X: {0} Y: {1}", regionX, regionY);
                        continue;
                    }

                    objectRegionData.Buildables.Add(shiftedBuildableData);
                    objectRegionData.Count++;
                }
            }

            string objectsSavePath = outputMap.CombinePath("Level/Buildables.dat");

            content.SaveToFile(objectsSavePath);

            Log.Information($"Combined and saved {content.BuildableRegions.Sum(x => x.Count)} buildables");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Level/Buildables.dat");
            BuildableDataContent content = BuildableDataContent.FromFile(fileNamePath);

            File.WriteAllText($"buildables_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            CopyMapData<BuildableDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };
            Data.Add(dataItem);

            int buildablesCount = content.BuildableRegions.Sum(x => x.Count);
            Log.Information($"Read {buildablesCount} buildables");
        }
    }
}
