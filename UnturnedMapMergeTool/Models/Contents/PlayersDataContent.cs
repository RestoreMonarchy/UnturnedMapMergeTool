using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Players;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class PlayersDataContent
    {
        public PlayersDataContent(byte saveDataVersion, byte count)
        {
            SaveDataVersion = saveDataVersion;
            Count = count;

            Spawns = new();
        }

        public PlayersDataContent()
        {

        }

        public byte SaveDataVersion { get; set; }
        public byte Count { get; set; }
        public List<PlayerSpawnData> Spawns { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            river.writeByte(SaveDataVersion);
            river.writeByte(Count);

            foreach (PlayerSpawnData playerSpawn in Spawns)
            {
                river.writeSingleVector3(playerSpawn.Point);
                river.writeByte(playerSpawn.Angle);
                river.writeBoolean(playerSpawn.IsAlt);
            }

            river.closeRiver();
        }

        public static PlayersDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            PlayersDataContent content = new();

            content.SaveDataVersion = river.readByte();
            if (content.SaveDataVersion > 1 && content.SaveDataVersion < 3)
            {
                river.readSteamID();
            }

            content.Count = river.readByte();
            content.Spawns = new List<PlayerSpawnData>();

            for (int i = 0; i < content.Count; i++)
            {
                PlayerSpawnData playerSpawn = new();

                playerSpawn.Point = river.readSingleVector3();
                playerSpawn.Angle = river.readByte();
                playerSpawn.IsAlt = false;
                
                if (content.SaveDataVersion > 3)
                {
                    playerSpawn.IsAlt = river.readBoolean();
                }

                content.Spawns.Add(playerSpawn);
            }

            river.closeRiver();

            return content;
        }
    }
}
