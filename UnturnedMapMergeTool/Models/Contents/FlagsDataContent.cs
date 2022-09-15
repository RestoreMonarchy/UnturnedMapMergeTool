using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Flags;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class FlagsDataContent
    {
        public FlagsDataContent()
        {

        }
        public FlagsDataContent(byte saveDataVersion, byte count)
        {
            SaveDataVersion = saveDataVersion;
            Count = count;

            Flags = new();
        }

        public byte SaveDataVersion { get; set; }
        public byte Count { get; set; }

        public List<FlagData> Flags { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new River(fileNamePath);

            river.writeByte(SaveDataVersion);
            river.writeByte(Count);

            foreach (FlagData flag in Flags)
            {
                river.writeSingleVector3(flag.Point);
                river.writeSingle(flag.Width);
                river.writeSingle(flag.Height);
            }

            river.closeRiver();
        }

        public static FlagsDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            FlagsDataContent content = new();
            content.Flags = new();

            content.SaveDataVersion = river.readByte();
            content.Count = river.readByte();

            for (byte i = 0; i < content.Count; i++)
            {
                FlagData flagData = new();
                content.Flags.Add(flagData);

                flagData.Point = river.readSingleVector3();
                flagData.Width = river.readSingle();
                flagData.Height = river.readSingle();
            }

            river.closeRiver();
            return content;
        }
    }
}
