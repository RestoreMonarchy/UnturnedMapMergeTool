using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Faunas;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools.Spawns
{
    public class FaunaDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<FaunaDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 3;
            byte tablesCount = (byte)Data.Sum(x => x.Content.TablesCount);
            ushort spawnpointsCount = (ushort)Data.Sum(x => x.Content.SpawnPointsCount);

            FaunaDataContent content = new(saveDataVersion, tablesCount, spawnpointsCount);

            byte tableId = 0;
            foreach (CopyMapData<FaunaDataContent> dataItem in Data)
            {
                byte startTableId = tableId;

                foreach (AnimalTableData animalTable in dataItem.Content.Tables)
                {
                    content.Tables.Add(animalTable);
                    tableId++;
                }

                foreach (AnimalSpawnpointData animalSpawnPoint in dataItem.Content.SpawnPoints)
                {
                    animalSpawnPoint.Type = (byte)(animalSpawnPoint.Type + startTableId);
                    dataItem.CopyMap.ApplyPositionShift(animalSpawnPoint.Point);
                    content.SpawnPoints.Add(animalSpawnPoint);
                }
            }

            string savePath = outputMap.CombinePath("Spawns/Fauna.dat");

            content.SaveToFile(savePath);

            // DEBUG
            File.WriteAllText($"fauna_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            Log.Information($"Combined and saved {content.TablesCount} fauna tables and {content.SpawnPointsCount} spawnpoints");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Spawns/Fauna.dat");
            FaunaDataContent content = FaunaDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"fauna_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));


            CopyMapData<FaunaDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.TablesCount} fauna tables and {content.SpawnPointsCount} spawnpoints");
        }
    }
}
