using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.FlagsData;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class FlagsDataDataContent
    {
        public FlagsDataDataContent(byte saveDataVersion, byte count)
        {
            SaveDataVersion = saveDataVersion;
            Count = count;

            FlagsData = new();
        }

        public FlagsDataDataContent()
        {

        }

        public byte SaveDataVersion { get; set; }
        public byte Count { get; set; }
        public List<FlagDataData> FlagsData { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            river.writeByte(SaveDataVersion);
            river.writeByte(Count);

            foreach (FlagDataData flagData in FlagsData)
            {
                river.writeString(flagData.DifficultyGuid);
                river.writeByte(flagData.MaxZombies);
                river.writeBoolean(flagData.SpawnZombies);
                river.writeBoolean(flagData.HyperAgro);
            }

            river.closeRiver();
        }

        public static FlagsDataDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            FlagsDataDataContent content = new();

            content.SaveDataVersion = river.readByte();
            content.Count = river.readByte();
            content.FlagsData = new();

            for (int i = 0; i < content.Count; i++)
            {
                FlagDataData flagData = new();
                content.FlagsData.Add(flagData);

                flagData.DifficultyGuid = river.readString();
                flagData.MaxZombies = river.readByte();
                flagData.SpawnZombies = river.readBoolean();
                flagData.HyperAgro = river.readBoolean();
            }

            river.closeRiver();
            return content;
        }
    }
}
