using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Players;
using UnturnedMapMergeTool.Models.Contents.Zombies;
using UnturnedMapMergeTool.Services;

namespace UnturnedMapMergeTool.DataMergeTools.Spawns
{
    public class PlayersDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<PlayersDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 4;
            byte count = (byte)Data.Sum(x => x.Content.Spawns.Count);

            PlayersDataContent content = new(saveDataVersion, count);

            foreach (CopyMapData<PlayersDataContent> dataItem in Data)
            {
                foreach (PlayerSpawnData playerSpawn in dataItem.Content.Spawns)
                {
                    if (dataItem.CopyMap.IsOriginalPositionBypassed(playerSpawn.Point))
                    {
                        continue;
                    }

                    dataItem.CopyMap.ApplyPositionShift(playerSpawn.Point);

                    if (dataItem.CopyMap.IsOutputMapBorder(playerSpawn.Point))
                    {
                        continue;
                    }

                    content.Spawns.Add(playerSpawn);
                }
            }

            string savePath = outputMap.CombinePath("Spawns/Players.dat");

            content.SaveToFile(savePath);

            // DEBUG
            File.WriteAllText($"players_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            Log.Information($"Combined and saved {content.Count} players spawns");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Spawns/Players.dat");
            PlayersDataContent content = PlayersDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"players_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));


            CopyMapData<PlayersDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.Spawns.Count} player spawns");
        }
    }
}
