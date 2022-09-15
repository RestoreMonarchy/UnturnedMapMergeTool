using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Roads;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class RoadsDataContent
    {
        public RoadsDataContent()
        {

        }
        public RoadsDataContent(byte saveDataRoadsVersion, byte count)
        {
            SaveDataRoadsVersion = saveDataRoadsVersion;
            Count = count;

            Materials = new List<RoadMaterialData>();
        }

        public byte SaveDataRoadsVersion { get; set; }
        public byte Count { get; set; }

        public List<RoadMaterialData> Materials { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            river.writeByte(SaveDataRoadsVersion);
            river.writeByte(Count);

            foreach (RoadMaterialData roadMaterial in Materials)
            {
                river.writeSingle(roadMaterial.Width);
                river.writeSingle(roadMaterial.Height);
                river.writeSingle(roadMaterial.Depth);
                river.writeSingle(roadMaterial.Offset);
                river.writeBoolean(roadMaterial.IsConcrete);
            }

            river.closeRiver();
        }

        public static RoadsDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            RoadsDataContent content = new();
            content.Materials = new List<RoadMaterialData>();

            content.SaveDataRoadsVersion = river.readByte();
            content.Count = river.readByte();

            for (int i = 0; i < content.Count; i++)
            {
                RoadMaterialData roadMaterial = new();
                content.Materials.Add(roadMaterial);

                roadMaterial.Width = river.readSingle();
                roadMaterial.Height = river.readSingle();
                roadMaterial.Depth = river.readSingle();
                if (content.SaveDataRoadsVersion > 1)
                {
                    roadMaterial.Offset = river.readSingle();
                }
                roadMaterial.IsConcrete = river.readBoolean();                
            }

            river.closeRiver();

            return content;
        }
    }
}
