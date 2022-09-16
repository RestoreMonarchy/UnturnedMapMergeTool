using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Bounds;
using UnturnedMapMergeTool.Models.Contents.Flags;
using UnturnedMapMergeTool.Models.Contents.Zombies;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools.Spawns
{
    public class ZombiesDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<ZombiesDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 9;
            byte count = (byte)Data.Sum(x => x.Content.Tables.Count);

            ZombiesDataContent content = new(saveDataVersion, count);

            foreach (CopyMapData<ZombiesDataContent> dataItem in Data)
            {
                foreach (ZombieTableData zombieTable in dataItem.Content.Tables)
                {
                    content.Tables.Add(zombieTable);
                }
            }

            string savePath = outputMap.CombinePath("Spawns/Zombies.dat");

            content.SaveToFile(savePath);

            // DEBUG
            File.WriteAllText($"zombies_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            Log.Information($"Combined and saved {content.Tables.Count} zombie tables");
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
