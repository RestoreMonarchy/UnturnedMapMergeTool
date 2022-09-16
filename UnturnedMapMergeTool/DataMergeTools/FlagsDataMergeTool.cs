using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Flags;
using UnturnedMapMergeTool.Models.Contents.Nodes;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class FlagsDataMergeTool : DataMergeToolBase
    {
        public List<CopyMapData<FlagsDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 4;
            byte count = (byte)Data.Sum(x => x.Content.Flags.Count);

            FlagsDataContent content = new(saveDataVersion, count);

            foreach (CopyMapData<FlagsDataContent> dataItem in Data)
            {
                foreach (FlagData flag in dataItem.Content.Flags)
                {
                    dataItem.CopyMap.ApplyPositionShift(flag.Point);
                    content.Flags.Add(flag);
                }
            }

            string nodesSavePath = outputMap.CombinePath("Environment/Flags.dat");

            content.SaveToFile(nodesSavePath);

            Log.Information($"Combined and saved {content.Flags.Count} flags");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Environment/Flags.dat");
            FlagsDataContent content = FlagsDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"flags_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            // Add flag to the copy maps because it will be used by the navigation
            copyMap.Flags = new();
            foreach (FlagData flag in content.Flags)
            {                
                copyMap.Flags.Add(flag);
            }
            
            CopyMapData<FlagsDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.Flags.Count} flags");
        }
    }
}
