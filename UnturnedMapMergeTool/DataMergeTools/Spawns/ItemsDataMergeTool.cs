using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Items;
using UnturnedMapMergeTool.Models.Contents.Vehicles;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools.Spawns
{
    public class ItemsDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<ItemsDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 4;
            byte tablesCount = (byte)Data.Sum(x => x.Content.TablesCount);

            ItemsDataContent content = new(saveDataVersion, tablesCount);

            byte tableId = 0;
            foreach (CopyMapData<ItemsDataContent> dataItem in Data)
            {
                dataItem.CopyMap.ItemTypeShift = tableId;

                foreach (ItemTableData vehicleTable in dataItem.Content.Tables)
                {
                    content.Tables.Add(vehicleTable);
                    tableId++;
                }
            }

            string savePath = outputMap.CombinePath("Spawns/Items.dat");

            content.SaveToFile(savePath);

            // DEBUG
            File.WriteAllText($"items_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            Log.Information($"Combined and saved {content.TablesCount} item tables");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Spawns/Items.dat");
            ItemsDataContent content = ItemsDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"items_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            CopyMapData<ItemsDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.TablesCount} item tables");
        }
    }
}
