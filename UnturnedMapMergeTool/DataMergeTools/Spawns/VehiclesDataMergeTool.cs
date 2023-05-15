using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Faunas;
using UnturnedMapMergeTool.Models.Contents.Vehicles;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools.Spawns
{
    public class VehiclesDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<VehiclesDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 4;
            byte tablesCount = (byte)Data.Sum(x => x.Content.TablesCount);
            ushort spawnpointsCount = (ushort)Data.Sum(x => x.Content.SpawnpointsCount);

            VehiclesDataContent content = new(saveDataVersion, tablesCount, spawnpointsCount);

            byte tableId = 0;
            foreach (CopyMapData<VehiclesDataContent> dataItem in Data)
            {
                byte startTableId = tableId;

                foreach (VehicleTableData vehicleTable in dataItem.Content.Tables)
                {
                    content.Tables.Add(vehicleTable);
                    tableId++;
                }

                foreach (VehicleSpawnpointData vehicleSpawnpoint in dataItem.Content.Spawnpoints)
                {
                    if (dataItem.CopyMap.IsOriginalPositionBypassed(vehicleSpawnpoint.Point))
                    {
                        continue;
                    }

                    vehicleSpawnpoint.Type = (byte)(vehicleSpawnpoint.Type + startTableId);
                    dataItem.CopyMap.ApplyPositionShift(vehicleSpawnpoint.Point);
                    content.Spawnpoints.Add(vehicleSpawnpoint);
                }
            }

            string savePath = outputMap.CombinePath("Spawns/Vehicles.dat");

            content.SaveToFile(savePath);

            // DEBUG
            File.WriteAllText($"vehicles_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            Log.Information($"Combined and saved {content.TablesCount} vehicle tables and {content.SpawnpointsCount} spawnpoints");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Spawns/Vehicles.dat");
            VehiclesDataContent content = VehiclesDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"vehicles_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            CopyMapData<VehiclesDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.TablesCount} vehicle tables and {content.SpawnpointsCount} spawnpoints");
        }
    }
}
