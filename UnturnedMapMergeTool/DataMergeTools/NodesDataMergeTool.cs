using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Nodes;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class NodesDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<NodesDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 8;
            int count = Data.Sum(x => x.Content.Nodes.Count);
            byte countByte = (byte)count;

            NodesDataContent content = new(saveDataVersion, countByte);

            foreach (CopyMapData<NodesDataContent> dataItem in Data)
            {
                foreach (NodeData node in dataItem.Content.Nodes)
                {
                    if (dataItem.CopyMap.Config.IgnoreAirdropNodes && node.Type == 5)
                    {
                        continue;
                    }

                    if (dataItem.CopyMap.Config.IgnoreArenaNodes && node.Type == 3)
                    {
                        continue;
                    }

                    if (dataItem.CopyMap.IsOriginalPositionBypassed(node.Point))
                    {
                        continue;
                    }

                    dataItem.CopyMap.ApplyPositionShift(node.Point);
                    content.Nodes.Add(node);
                }
            }

            content.Count = (byte)content.Nodes.Count;
            content.Nodes = content.Nodes.ToList();

            // DEBUG
            File.WriteAllText($"nodes_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

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
