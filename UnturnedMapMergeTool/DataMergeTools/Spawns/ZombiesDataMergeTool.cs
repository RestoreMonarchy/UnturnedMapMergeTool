using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Flags;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools.Spawns
{
    public class ZombiesDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<ZombiesDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            throw new NotImplementedException();
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Spawns/Zombies.dat");
            ZombiesDataContent content = ZombiesDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"zombies_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));


            CopyMapData<ZombiesDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.Tables.Count} zombie tables");
        }
    }
}
