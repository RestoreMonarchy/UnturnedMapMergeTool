using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents.Objects;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Unturned.Unity;
using UnturnedMapMergeTool.Unturned;
using System;

namespace UnturnedMapMergeTool.Services
{
    public class LevelService
    {
        private readonly OutputMap outputMap;
        private readonly IEnumerable<CopyMap> copyMaps;

        public LevelService(OutputMap outputMap, IEnumerable<CopyMap> copyMaps)
        {
            this.outputMap = outputMap;
            this.copyMaps = copyMaps;
        }

        public void CombineAndSaveObjects()
        {
            IEnumerable<ObjectData> objects = copyMaps.SelectMany(x => x.ObjectDataContent.ObjectRegions.SelectMany(y => y.Objects));

            Log($"Combining {objects.Count()} objects...");

            // Get objects from all maps in one list
            List<MapObjectData> mapObjectsData = new();
            foreach (CopyMap copyMap in copyMaps)
            {
                IEnumerable<ObjectData> copyMapObjects = copyMap.ObjectDataContent.ObjectRegions.SelectMany(x => x.Objects);

                foreach (ObjectData objectData in copyMapObjects)
                {
                    MapObjectData mapObjectData = new()
                    {
                        Map = copyMap,
                        ObjectData = objectData
                    };

                    mapObjectsData.Add(mapObjectData);
                }
            }

            uint instanceId = 0;
            // Shift positions
            foreach (MapObjectData mapObjectData in mapObjectsData)
            {
                instanceId++;
                mapObjectData.ObjectData.Position.x += mapObjectData.Map.Config.ShiftX;
                // Yeaah Y is actually Z in Vector3, shrug
                mapObjectData.ObjectData.Position.z += mapObjectData.Map.Config.ShiftY;
                mapObjectData.ObjectData.InstanceId = instanceId;
            }

            string objectsSavePath = outputMap.CombinePath("Level/Objects.dat");

            byte saveDataVersion = 10;
            CSteamID steamID = null;
            uint availableInstanceId = instanceId + 1;
            ObjectDataContent content = new(saveDataVersion, steamID, availableInstanceId);

            foreach (MapObjectData mapObjectData in mapObjectsData)
            {
                Regions.tryGetCoordinate(mapObjectData.ObjectData.Position, out byte regionX, out byte regionY);

                ObjectRegionData objectRegionData = content.ObjectRegions.FirstOrDefault(x => x.RegionX == regionX && x.RegionY == regionY);
                objectRegionData.Objects.Add(mapObjectData.ObjectData);
                objectRegionData.Count++;
            }

            content.SaveToFile(objectsSavePath);

            Log($"Finished combining and saved {objects.Count()} objects in output map ");
        }

        private void Log(string message)
        {
            Console.WriteLine($"[LevelService]: {message}");
        }
    }
}
