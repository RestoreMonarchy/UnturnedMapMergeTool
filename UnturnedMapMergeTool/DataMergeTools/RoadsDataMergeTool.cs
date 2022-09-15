using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Roads;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class RoadsDataMergeTool : DataMergeToolBase
    {
        public List<CopyMapData<RoadsDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 2;
            byte count = (byte)Data.Sum(x => x.Content.Materials.Count);
            RoadsDataContent content = new(saveDataVersion, count);

            byte materialId = 0;
            foreach (CopyMapData<RoadsDataContent> dataItem in Data)
            {
                dataItem.CopyMap.MaterialShift = materialId;
                foreach (RoadMaterialData roadMaterial in dataItem.Content.Materials)
                {
                    content.Materials.Add(roadMaterial);
                    materialId++;
                }                
            }

            string roadsSavePath = outputMap.CombinePath("Environment/Roads.dat");

            content.SaveToFile(roadsSavePath);

            Log.Information($"Combined and saved {content.Materials.Count} road materials");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Environment/Roads.dat");
            RoadsDataContent content = RoadsDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"roads_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            CopyMapData<RoadsDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.Count} road materials");
        }
    }
}
