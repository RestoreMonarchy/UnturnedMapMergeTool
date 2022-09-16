using Newtonsoft.Json;
using Serilog;
using System.IO;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Services;
using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Bounds;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents.FlagsData;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class BoundsDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<BoundsDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 1;
            byte count = (byte)Data.Sum(x => x.Content.Bounds.Count);

            BoundsDataContent content = new(saveDataVersion, count);

            foreach (CopyMapData<BoundsDataContent> dataItem in Data)
            {
                foreach (BoundData bound in dataItem.Content.Bounds)
                {
                    dataItem.CopyMap.ApplyPositionShift(bound.Center);
                    content.Bounds.Add(bound);
                }
            }

            string savePath = outputMap.CombinePath("Environment/Bounds.dat");

            content.SaveToFile(savePath);
            
            // DEBUG
            File.WriteAllText($"bounds_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            Log.Information($"Combined and saved {content.Bounds.Count} bounds");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Environment/Bounds.dat");
            BoundsDataContent content = BoundsDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"bounds_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            CopyMapData<BoundsDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.Bounds.Count} bounds");
        }
    }
}
