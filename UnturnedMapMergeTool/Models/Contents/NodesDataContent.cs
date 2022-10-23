using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents.Nodes;
using UnturnedMapMergeTool.Unturned;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class NodesDataContent
    {
        public NodesDataContent()
        {

        }

        public NodesDataContent(byte saveDataVersion, byte count)
        {
            SaveDataVersion = saveDataVersion;
            Count = count;

            Nodes = new List<NodeData>();
        }

        public byte SaveDataVersion { get; set; }
        public byte Count { get; set; }

        public List<NodeData> Nodes { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new River(fileNamePath);

            river.writeByte(SaveDataVersion);
            river.writeByte(Count);

            foreach (NodeData node in Nodes)
            {
                river.writeSingleVector3(node.Point);
                river.writeByte(node.Type);
                if (node.Type == 0)
                {
                    // Location
                    river.writeString(node.LocationName);
                }
                else if (node.Type == 1)
                {
                    // Safezone
                    river.writeSingle(node.Radius);
                    river.writeBoolean(node.IsHeight);
                    river.writeBoolean(node.NoWeapons);
                    river.writeBoolean(node.NoBuildables);
                }
                else if (node.Type == 2)
                {
                    // Purchase
                    river.writeSingle(node.Radius);
                    river.writeUInt16(node.AssetId);
                    river.writeUInt32(node.Cost);
                }
                else if (node.Type == 3)
                {
                    // Arena                    
                    river.writeSingle(node.Radius);
                }
                else if (node.Type == 4)
                {
                    // Deadzone
                    river.writeSingle(node.Radius);
                    river.writeByte(node.DeadzoneType);
                }
                else if (node.Type == 5)
                {
                    // Airdrop
                    river.writeUInt16(node.AssetId);
                }
                else if (node.Type == 6)
                {
                    // Effect
                    river.writeByte(node.Shape);
                    river.writeSingle(node.Radius);
                    river.writeSingleVector3(node.Bounds);
                    river.writeUInt16(node.AssetId);
                    river.writeBoolean(node.NoWater);
                    river.writeBoolean(node.NoLighting);
                }
            }

            river.closeRiver();
        }

        public static NodesDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            NodesDataContent content = new();
            content.Nodes = new List<NodeData>();

            content.SaveDataVersion = river.readByte();
            content.Count = river.readByte();

            for (byte i = 0; i < content.Count; i++)
            {
                NodeData node = new NodeData();
                content.Nodes.Add(node);

                node.Point = river.readSingleVector3();
                node.Type = river.readByte();

                
                if (node.Type == 0)
                {
                    // Location
                    node.LocationName = river.readString();
                }
                else if (node.Type == 1)
                {
                    // Safezone
                    node.Radius = river.readSingle();
                    if (content.SaveDataVersion > 1)
                    {
                        node.IsHeight = river.readBoolean();
                    }
                    if (content.SaveDataVersion > 4)
                    {
                        node.NoWeapons = river.readBoolean();
                    }
                    if (content.SaveDataVersion > 4)
                    {
                        node.NoBuildables = river.readBoolean();
                    }
                } else if (node.Type == 2)
                {
                    // Purchase
                    node.Radius = river.readSingle();
                    node.AssetId = river.readUInt16();
                    node.Cost = river.readUInt32();
                } else if (node.Type == 3)
                {
                    // Arena
                    node.Radius = river.readSingle();
                    if (content.SaveDataVersion < 6)
                    {
                        node.Radius *= 0.5f;
                    }
                } else if (node.Type == 4)
                {
                    // Deadzone
                    node.Radius = river.readSingle();
                    node.DeadzoneType = 0;
                    if (content.SaveDataVersion > 6)
                    {
                        node.DeadzoneType = river.readByte();
                    }
                } else if (node.Type == 5)
                {
                    // Airdrop
                    node.AssetId = river.readUInt16();
                } else if (node.Type == 6)
                {
                    // Effect
                    node.Shape = 0;
                    if (content.SaveDataVersion > 2)
                    {
                        node.Shape = river.readByte();
                    }
                    node.Radius = river.readSingle();
                    node.Bounds = Vector3.one;
                    if (content.SaveDataVersion > 2)
                    {
                        node.Bounds = river.readSingleVector3();
                    }
                    node.AssetId = river.readUInt16();
                    node.NoWater = river.readBoolean();
                    node.NoLighting = false;
                    if (content.SaveDataVersion > 3)
                    {
                        node.NoLighting = river.readBoolean();
                    }
                }
            }

            river.closeRiver();
            return content;
        }
    }
}
