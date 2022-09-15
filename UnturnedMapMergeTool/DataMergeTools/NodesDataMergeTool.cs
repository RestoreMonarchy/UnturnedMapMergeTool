using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Nodes;
using UnturnedMapMergeTool.Models.Contents.Roads;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class NodesDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<NodesDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 2;
            int count = Data.Sum(x => x.Content.Nodes.Count);
            byte countByte = (byte)count;
            if (count > byte.MaxValue)
            {
                Log.Warning($"There is more nodes than byte max value! {count}");
                countByte = byte.MaxValue;
                
            }

            NodesDataContent content = new(saveDataVersion, countByte);

            foreach (CopyMapData<NodesDataContent> dataItem in Data)
            {
                foreach (NodeData node in dataItem.Content.Nodes)
                {
                    dataItem.CopyMap.ApplyPositionShift(node.Point);
                    content.Nodes.Add(node);
                }
            }

            content.Nodes = content.Nodes.OrderBy(x => x.Type).ToList();

            string nodesSavePath = outputMap.CombinePath("Environment/Nodes.dat");

            content.SaveToFile(nodesSavePath);

            Log.Information($"Combined and saved {content.Nodes.Count} nodes");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Environment/Nodes.dat");
            NodesDataContent content = NodesDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"nodes_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            CopyMapData<NodesDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.Nodes.Count} nodes");
        }
    }
}
