using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents.Objects;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Unturned.Unity;
using UnturnedMapMergeTool.Unturned;
using System;
using UnturnedMapMergeTool.Models.Contents.Buildables;

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
            uint availableInstanceId = 0;
            byte saveDataVersion = 10;
            CSteamID steamID = null;
            ObjectDataContent content = new(saveDataVersion, steamID, availableInstanceId);

            foreach (CopyMap copyMap in copyMaps)
            {
                IEnumerable<ObjectData> copyMapObjects = copyMap.ObjectDataContent.ObjectRegions.SelectMany(x => x.Objects);

                foreach (ObjectData objectData in copyMapObjects)
                {
                    ObjectData shiftedObjectData = new()
                    {
                        Position = objectData.Position,
                        LocalScale = objectData.LocalScale,
                        AssetId = objectData.AssetId,
                        Guid = objectData.Guid,
                        PlacementOrigin = objectData.PlacementOrigin,
                        Rotation = objectData.Rotation,
                        InstanceId = availableInstanceId++
                    };

                    // Yeah Y is actually Z in unity Vector3
                    shiftedObjectData.Position.x += copyMap.Config.ShiftX;
                    shiftedObjectData.Position.z += copyMap.Config.ShiftY;

                    Regions.tryGetCoordinate(shiftedObjectData.Position, out byte regionX, out byte regionY);

                    ObjectRegionData objectRegionData = content.ObjectRegions.FirstOrDefault(x => x.RegionX == regionX && x.RegionY == regionY);
                    objectRegionData.Objects.Add(shiftedObjectData);
                    objectRegionData.Count++;
                }
            }

            string objectsSavePath = outputMap.CombinePath("Level/Objects.dat");

            content.SaveToFile(objectsSavePath);

            Log($"Combined and saved {content.ObjectRegions.Sum(x => x.Count)} objects");
        }

        public void CombineAndSaveBuildables()
        {
            byte saveDataVersion = 10;
            BuildableDataContent content = new(saveDataVersion);

            foreach (CopyMap copyMap in copyMaps)
            {
                IEnumerable<BuildableData> copyMapBuildables = copyMap.BuildableDataContent.BuildableRegions.SelectMany(x => x.Buildables);

                foreach (BuildableData buildableData in copyMapBuildables)
                {
                    BuildableData shiftedBuildableData = new()
                    {
                        Position = buildableData.Position,
                        AssetId = buildableData.AssetId,
                        Rotation = buildableData.Rotation
                    };

                    // Yeah Y is actually Z in unity Vector3
                    shiftedBuildableData.Position.x += copyMap.Config.ShiftX;
                    shiftedBuildableData.Position.z += copyMap.Config.ShiftY;

                    Regions.tryGetCoordinate(shiftedBuildableData.Position, out byte regionX, out byte regionY);

                    BuildableRegionData objectRegionData = content.BuildableRegions.FirstOrDefault(x => x.RegionX == regionX && x.RegionY == regionY);
                    objectRegionData.Buildables.Add(shiftedBuildableData);
                    objectRegionData.Count++;
                }
            }

            string objectsSavePath = outputMap.CombinePath("Level/Buildables.dat");

            content.SaveToFile(objectsSavePath);

            Log($"Combined and saved {content.BuildableRegions.Sum(x => x.Count)} buildables");
        }

        private void Log(string message)
        {
            Console.WriteLine($"[LevelService]: {message}");
        }
    }
}
