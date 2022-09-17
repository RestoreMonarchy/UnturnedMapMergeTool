using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Services;
using UnturnedMapMergeTool.Models.Contents.Zombies;
using UnturnedMapMergeTool.Models.Contents.Animals;
using UnturnedMapMergeTool.Models.Contents.Trees;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.DataMergeTools.Spawns
{
    public class AnimalsDataMergeTool : DataMergeToolBase
    {
        private List<CopyMapData<AnimalsDataContent>> Data { get; set; } = new();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            byte saveDataVersion = 1;

            AnimalsDataContent content = new(saveDataVersion);

            foreach (CopyMapData<AnimalsDataContent> dataItem in Data)
            {
                IEnumerable<ZombieSpawnpointData> zombieSpawnpoints = dataItem.Content.ZombieRegions.SelectMany(x => x.ZombieSpawnpoints);

                foreach (ZombieSpawnpointData zombieSpawnpoint in zombieSpawnpoints)
                {
                    ZombieSpawnpointData shiftedZombieSpawnpoint = new()
                    {
                        Point = zombieSpawnpoint.Point,
                        Type = zombieSpawnpoint.Type
                    };

                    dataItem.CopyMap.ApplyPositionShift(shiftedZombieSpawnpoint.Point);
                    shiftedZombieSpawnpoint.Type = dataItem.CopyMap.GetShiftedZombieType(shiftedZombieSpawnpoint.Type);

                    if (!Regions.tryGetCoordinate(shiftedZombieSpawnpoint.Point, out byte regionX, out byte regionY))
                    {
                        Log.Warning($"Zombie SpawnPoint: Failed to get coordinates for {shiftedZombieSpawnpoint.Point}");
                        continue;
                    }

                    ZombieRegionData zombieRegionData = content.ZombieRegions.First(x => x.RegionX == regionX && x.RegionY == regionY);
                    zombieRegionData.ZombieSpawnpoints.Add(shiftedZombieSpawnpoint);
                    zombieRegionData.Count++;
                }
            }

            string savePath = outputMap.CombinePath("Spawns/Animals.dat");

            content.SaveToFile(savePath);

            // DEBUG
            File.WriteAllText($"animals_output.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            Log.Information($"Combined and saved {content.ZombieRegions.Sum(x => x.Count)} zombie spawnpoints");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Spawns/Animals.dat");
            AnimalsDataContent content = AnimalsDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"animals_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));


            CopyMapData<AnimalsDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Data.Add(dataItem);

            Log.Information($"Read {content.ZombieRegions.Sum(x => x.Count)} zombie spawnpoints");
        }
    }
}
