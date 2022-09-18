using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Jars;
using UnturnedMapMergeTool.Services;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.DataMergeTools.Spawns
{
    public class JarsDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<JarsDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 1;

            JarsDataContent content = new(saveDataVersion);

            foreach (CopyMapData<JarsDataContent> dataItem in Data)
            {
                IEnumerable<ItemSpawnpointData> itemSpawnpoints = dataItem.Content.ItemRegions.SelectMany(x => x.Spawnpoints);

                foreach (ItemSpawnpointData itemSpawnpoint in itemSpawnpoints)
                {
                    ItemSpawnpointData shiftedItemSpawnpoint = new()
                    {
                        Point = itemSpawnpoint.Point,
                        Type = itemSpawnpoint.Type
                    };

                    dataItem.CopyMap.ApplyPositionShift(shiftedItemSpawnpoint.Point);
                    shiftedItemSpawnpoint.Type = dataItem.CopyMap.GetShiftedItemType(shiftedItemSpawnpoint.Type);

                    if (!Regions.tryGetCoordinate(shiftedItemSpawnpoint.Point, out byte regionX, out byte regionY))
                    {
                        Log.Warning($"Item SpawnPoint: Failed to get coordinates for {shiftedItemSpawnpoint.Point}");
                        continue;
                    }

                    ItemRegionData itemRegionData = content.ItemRegions.First(x => x.RegionX == regionX && x.RegionY == regionY);
                    itemRegionData.Spawnpoints.Add(shiftedItemSpawnpoint);
                    itemRegionData.SpawnpointsCount++;
                }
            }

            string savePath = outputMap.CombinePath("Spawns/Jars.dat");

            content.SaveToFile(savePath);

            // DEBUG
            File.WriteAllText($"jars_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            Log.Information($"Combined and saved {content.ItemRegions.Sum(x => x.SpawnpointsCount)} item spawnpoints");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Spawns/Jars.dat");
            JarsDataContent content = JarsDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"jars_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));


            CopyMapData<JarsDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.ItemRegions.Sum(x => x.SpawnpointsCount)} item spawnpoints");
        }
    }
}
