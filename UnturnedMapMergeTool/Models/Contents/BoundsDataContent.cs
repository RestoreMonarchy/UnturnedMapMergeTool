using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Models.Contents.Bounds;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class BoundsDataContent
    {
        public BoundsDataContent(byte saveDataVersion, byte count)
        {
            SaveDataVersion = saveDataVersion;
            Count = count;

            Bounds = new List<BoundData>();
        }

        public BoundsDataContent()
        {

        }

        public byte SaveDataVersion { get; set; }
        public byte Count { get; set; }

        public List<BoundData> Bounds { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            river.writeByte(SaveDataVersion);
            river.writeByte(Count);

            foreach (BoundData bound in Bounds)
            {
                river.writeSingleVector3(bound.Center);
                river.writeSingleVector3(bound.Size);
            }

            river.closeRiver();
        }

        public static BoundsDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            BoundsDataContent content = new();
            content.Bounds = new();

            content.SaveDataVersion = river.readByte();
            content.Count = river.readByte();

            for (int i = 0; i < content.Count; i++)
            {
                BoundData bound = new();
                content.Bounds.Add(bound);

                bound.Center = river.readSingleVector3();
                bound.Size = river.readSingleVector3();
            }

            river.closeRiver();
            return content;
        }

    }
}
