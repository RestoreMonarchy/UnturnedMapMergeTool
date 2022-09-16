using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models.Contents.Flags;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Services;
using UnturnedMapMergeTool.Models.Contents.FlagsData;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class FlagsDataDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<FlagsDataDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 4;
            byte count = (byte)Data.Sum(x => x.Content.FlagsData.Count);

            FlagsDataDataContent content = new(saveDataVersion, count);

            foreach (CopyMapData<FlagsDataDataContent> dataItem in Data)
            {
                foreach (FlagDataData flagData in dataItem.Content.FlagsData)
                {
                    content.FlagsData.Add(flagData);
                }
            }

            string savePath = outputMap.CombinePath("Environment/Flags_Data.dat");

            content.SaveToFile(savePath);

            // DEBUG
            File.WriteAllText($"flags_data_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            Log.Information($"Combined and saved {content.FlagsData.Count} flags data");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Environment/Flags_Data.dat");
            FlagsDataDataContent content = FlagsDataDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"flags_data_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            CopyMapData<FlagsDataDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.FlagsData.Count} flags data");
        }
    }
}
