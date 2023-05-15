using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Flags;
using UnturnedMapMergeTool.Models.Contents.FlagsData;
using UnturnedMapMergeTool.Models.Contents.Nodes;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class FlagsDataMergeTool : DataMergeToolBase
    {
        public List<CopyMapData<FlagsDataContent>> Flags { get; set; } = new();
        private List<CopyMapData<FlagsDataDataContent>> FlagsData { get; set; } = new();

        private int flagsIndex = 0;

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte flagsSaveVersion = 4;
            byte flagsDataSaveVersion = 4;

            FlagsDataContent flagsContent = new(flagsSaveVersion, 0);
            FlagsDataDataContent flagsDataContent = new(flagsDataSaveVersion, 0);

            foreach (CopyMapData<FlagsDataContent> flagItem in Flags)
            {
                // If this throws an exception it means something is not cool
                CopyMapData<FlagsDataDataContent> flagDataItem = FlagsData.First(x => x.CopyMap == flagItem.CopyMap);

                if (flagDataItem.Content.FlagsData.Count != flagItem.Content.Flags.Count)
                {
                    throw new Exception("Flags data count and flags count should be the same");
                }

                for (int i = 0; i < flagItem.Content.Count; i++)
                {
                    FlagData flag = flagItem.Content.Flags[i];
                    FlagDataData flagData = flagDataItem.Content.FlagsData[i];

                    if (flagItem.CopyMap.IsOriginalPositionBypassed(flag.Point))
                    {
                        continue;
                    }

                    flagItem.CopyMap.ApplyPositionShift(flag.Point);

                    flagsContent.Flags.Add(flag);
                    flagsDataContent.FlagsData.Add(flagData);
                }
            }

            flagsContent.Count = (byte)flagsContent.Flags.Count;
            flagsDataContent.Count = (byte)flagsDataContent.FlagsData.Count;

            string flagsSavePath = outputMap.CombinePath("Environment/Flags.dat");
            flagsContent.SaveToFile(flagsSavePath);
            File.WriteAllText($"flags_output.json", JsonConvert.SerializeObject(flagsContent, Formatting.Indented));
            Log.Information($"Combined and saved {flagsContent.Flags.Count} flags");
            
            string FlagsDataSavePath = outputMap.CombinePath("Environment/Flags_Data.dat");
            flagsDataContent.SaveToFile(FlagsDataSavePath);
            File.WriteAllText($"flags_data_output.json", JsonConvert.SerializeObject(flagsDataContent, Formatting.Indented));
            Log.Information($"Combined and saved {flagsDataContent.FlagsData.Count} flags data");
        }

        public override void ReadData(CopyMap copyMap)
        {
            ReadFlags(copyMap);
            ReadFlagsData(copyMap); 
        }

        private void ReadFlags(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Environment/Flags.dat");
            FlagsDataContent content = FlagsDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"flags_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            // Add flag to the copy maps because it will be used by the navigation
            copyMap.Flags = new();
            copyMap.FlagsStartIndex = flagsIndex;
            foreach (FlagData flag in content.Flags)
            {
                copyMap.Flags.Add(flag);
                flagsIndex++;
            }

            CopyMapData<FlagsDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Flags.Add(dataItem);

            Log.Information($"Read {content.Flags.Count} flags");
        }

        public void ReadFlagsData(CopyMap copyMap)
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

            FlagsData.Add(dataItem);

            Log.Information($"Read {content.FlagsData.Count} flags data");
        }
    }
}
