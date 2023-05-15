using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class NavigationsDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<List<NavigationsDataContent>>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            int count = 0;
            foreach (CopyMapData<List<NavigationsDataContent>> dataItem in Data)
            {
                for (int i = 0; i < dataItem.Content.Count; i++)
                {
                    NavigationsDataContent content = dataItem.Content[i];
                    if (dataItem.CopyMap.IsOriginalPositionBypassed(content.ForcedBoundsCenter))
                    {
                        continue;
                    }

                    dataItem.CopyMap.ApplyPositionShift(content.ForcedBoundsCenter);

                    int index = dataItem.CopyMap.FlagsStartIndex + i;
                    string savePath = outputMap.CombinePath($"Environment/Navigation_{index}.dat");
                    
                    content.SaveToFile(savePath);
                    count++;
                }
            }

            Log.Information($"Combined and saved {count} navigations");
        }

        public override void ReadData(CopyMap copyMap)
        {
            List<NavigationsDataContent> dataContents = new();

            for (int i = 0; i < copyMap.Flags.Count; i++)
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                string fileNamePath = Path.Combine(copyMap.Config.Path, $"Environment/Navigation_{i}.dat");
                NavigationsDataContent content = NavigationsDataContent.FromFile(fileNamePath);
                dataContents.Add(content);
                stopwatch.Stop();

                Log.Debug($"Read {content.NavmeshTiles.Length} navmesh tiles from {Path.GetFileName(fileNamePath)} in {stopwatch.Elapsed.TotalSeconds} seconds");
                // Write to JSON file for debug
                File.WriteAllText($"navigation_{i}_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));
            }

            CopyMapData<List<NavigationsDataContent>> dataItem = new()
            {
                CopyMap = copyMap,
                Content = dataContents
            };

            Data.Add(dataItem);

            Log.Information($"Read {copyMap.Flags.Count} navigations");
        }
    }
}
