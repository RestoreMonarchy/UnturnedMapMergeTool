using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Abstractions;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Contents.Objects;
using UnturnedMapMergeTool.Models.Contents.Trees;
using UnturnedMapMergeTool.Services;
using UnturnedMapMergeTool.Unturned;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.DataMergeTools
{
    public class ObjectsDataMergeTool : DataMergeToolBase
    {
        public List<CopyMapData<ObjectDataContent>> Items { get; set; } = new List<CopyMapData<ObjectDataContent>>();

        public override void CombineAndSaveData(OutputMap outputMap)
        {
            uint availableInstanceId = 0;
            byte saveDataVersion = 12;
            CSteamID steamID = null;
            ObjectDataContent content = new(saveDataVersion, steamID, availableInstanceId);

            foreach (CopyMapData<ObjectDataContent> dataItem in Items)
            {
                List<ObjectData> copyMapObjects = dataItem.Content.ObjectRegions.SelectMany(x => x.Objects).ToList();

                foreach (ObjectData objectData in copyMapObjects)
                {
                    if (!dataItem.CopyMap.ShouldIncludePosition(objectData.Position))
                    {
                        Log.Warning($"OBJECT: Skipping object outside of the border");
                        continue;
                    }

                    if (dataItem.CopyMap.IsOriginalPositionBypassed(objectData.Position))
                    {
                        continue;
                    }

                    ObjectData shiftedObjectData = new()
                    {
                        Position = objectData.Position,
                        LocalScale = objectData.LocalScale,
                        AssetId = objectData.AssetId,
                        Guid = objectData.Guid,
                        PlacementOrigin = objectData.PlacementOrigin,
                        Rotation = objectData.Rotation,
                        InstanceId = 0
                    };                    

                    dataItem.CopyMap.ApplyPositionShift(shiftedObjectData.Position);

                    if (!Regions.tryGetCoordinate(shiftedObjectData.Position, out byte regionX, out byte regionY))
                    {
                        //Log.Warning($"OBJECT: Failed to get coordinates for {shiftedObjectData.Position}");
                        continue;
                    }

                    shiftedObjectData.InstanceId = availableInstanceId++;

                    ObjectRegionData objectRegionData = content.ObjectRegions.First(x => x.RegionX == regionX && x.RegionY == regionY);

                    objectRegionData.Objects.Add(shiftedObjectData);
                    objectRegionData.Count++;
                }
            }

            string objectsSavePath = outputMap.CombinePath("Level/Objects.dat");

            content.SaveToFile(objectsSavePath);

            Log.Information($"Combined and saved {content.ObjectRegions.Sum(x => x.Count)} objects");
        }

        public override void ReadData(CopyMap copyMap)
        {
            string fileNamePath = Path.Combine(copyMap.Config.Path, "Level/Objects.dat");
            ObjectDataContent content = ObjectDataContent.FromFile(fileNamePath);

            // Write to JSON file for debug
            File.WriteAllText($"objects_{copyMap.Config.Name}.json", JsonConvert.SerializeObject(content, Formatting.Indented));

            int objectsCount = content.ObjectRegions.Sum(x => x.Count);

            CopyMapData<ObjectDataContent> dataItem = new()
            {
                CopyMap = copyMap,
                Content = content
            };

            Items.Add(dataItem);
            
            Log.Information($"Read {objectsCount} objects");
        }
    }
}
