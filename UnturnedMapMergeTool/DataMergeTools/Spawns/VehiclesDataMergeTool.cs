using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools.Spawns
{
    public class VehiclesDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<VehiclesDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            throw new NotImplementedException();
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

            Log.Information($"Read {content.VehicleSpawns.Count} vehicle spawns");
        }
    }
}
