using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Objects;
using UnturnedMapMergeTool.Unturned;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class ObjectDataContent
    {
        public byte SaveDataVersion { get; set; }
        public CSteamID SteamID { get; set; }
        public uint AvailableInstanceId { get; set; }
        public byte[] Hash { get; set; }

        public List<ObjectRegionData> ObjectRegions { get; set; }

        public static ObjectDataContent FromFile(string fileNamePath)
        {
             River river = new River(fileNamePath);

            ObjectDataContent content = new();

            content.SaveDataVersion = river.readByte();
            

            if (content.SaveDataVersion <= 0)
            {
                return null;
            }

            if (content.SaveDataVersion > 1 && content.SaveDataVersion < 3)
            {
                content.SteamID = river.readSteamID();
            }

            if (content.SaveDataVersion > 8)
            {
                content.AvailableInstanceId = river.readUInt32();
            } else
            {
                content.AvailableInstanceId = 1U;
            }

            content.ObjectRegions = new List<ObjectRegionData>();

            for (int i = 0; i < Regions.WORLD_SIZE; i++)
            {
                for (int j = 0; j < Regions.WORLD_SIZE; j++)
                {
                    ObjectRegionData region = new ObjectRegionData();
                    region.Count = river.readUInt16();

                    region.Objects = new List<ObjectData>();

                    for (ushort k = 0; k < region.Count; k++)
                    {
                        ObjectData objectData = new();

                        objectData.Position = river.readSingleVector3();
                        objectData.Rotation = river.readSingleQuaternion();
                        if (content.SaveDataVersion > 3)
                        {
                            objectData.LocalScale = river.readSingleVector3();
                        } else
                        {
                            objectData.LocalScale = Vector3.one;
                        }

                        objectData.Id = river.readUInt16();
                        if (content.SaveDataVersion > 5 && content.SaveDataVersion < 10)
                        {
                            // What string is that?
                            river.readString();
                        }

                        if (content.SaveDataVersion > 7)
                        {
                            objectData.Guid = river.readGUID();
                        }

                        if (content.SaveDataVersion > 6)
                        {
                            objectData.PlacementOrigin = river.readByte();
                        }
                        
                        if (content.SaveDataVersion > 8)
                        {
                            objectData.InstanceId = river.readUInt32();
                        } else
                        {
                            objectData.InstanceId = content.AvailableInstanceId++;
                        }

                        region.Objects.Add(objectData);
                    }

                    content.ObjectRegions.Add(region);
                }                
            }

            content.Hash = river.getHash();
            river.closeRiver();

            return content;
        }
    }
}
