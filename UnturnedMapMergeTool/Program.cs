using System;
using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Objects;
using UnturnedMapMergeTool.Services;
using UnturnedMapMergeTool.Unturned;
using UnturnedMapMergeTool.Unturned.Unity;

internal class Program
{
    private static void Main(string[] args)
    {
        Config config = new();
        OutputMap outputMap = new(config.OutputMap);

        // Delete existing directories and create empty
        outputMap.Preapare();

        List<CopyMap> copyMaps = new();
        foreach (CopyMapConfig map in config.Maps)
        {
            CopyMap copyMap = new(map, outputMap);
            copyMap.CopyAllTiles();
            copyMap.ReadLevel();
            copyMaps.Add(copyMap);
        }

        IEnumerable<ObjectData> objects = copyMaps.SelectMany(x => x.ObjectDataContent.ObjectRegions.SelectMany(y => y.Objects));

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
            mapObjectData.ObjectData.Guid = Guid.NewGuid();
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

        Console.ReadKey();
    }
}