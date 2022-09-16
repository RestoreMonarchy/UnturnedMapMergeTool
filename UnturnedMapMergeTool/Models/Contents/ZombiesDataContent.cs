using System.Collections.Generic;
using System.IO;
using UnturnedMapMergeTool.Models.Contents.Zombies;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class ZombiesDataContent
    {
        public byte SaveDataVersion { get; set; }
        public byte TablesCount { get; set; }

        public List<ZombieTableData> Tables { get; set; }
         
        public static ZombiesDataContent FromFile(string fileNamePath)
        {
            byte[] bytes = File.ReadAllBytes(fileNamePath);

            Block block = new(0, bytes);

            ZombiesDataContent content = new();

            content.SaveDataVersion = block.readByte();
            if (content.SaveDataVersion > 3 && content.SaveDataVersion < 5)
            {
                block.readSteamID();
            }

            content.Tables = new();

            // TODO: Add support for SavedataVersion < 2

            content.TablesCount = block.readByte();
            for (byte i = 0; i < content.TablesCount; i++)
            {
                ZombieTableData zombieTable = new();
                content.Tables.Add(zombieTable);

                zombieTable.Color = block.readColor();
                zombieTable.Name = block.readString();
                zombieTable.IsMega = block.readBoolean();
                zombieTable.Health = block.readUInt16();
                zombieTable.Damage = block.readByte();
                zombieTable.LootIndex = block.readByte();
                if (content.SaveDataVersion > 6)
                {
                    zombieTable.LootId = block.readUInt16();
                }
                else
                {
                    zombieTable.LootId = 0;
                }

                if (content.SaveDataVersion > 7)
                {
                    zombieTable.XP = block.readUInt32();
                }
                else if (zombieTable.IsMega)
                {
                    zombieTable.XP = 40U;
                }
                else
                {
                    zombieTable.XP = 3U;
                }

                zombieTable.Regen = 10f;
                if (content.SaveDataVersion > 5)
                {
                    zombieTable.Regen = block.readSingle();
                }
                zombieTable.DifficultyGuid = string.Empty;
                if (content.SaveDataVersion > 8)
                {
                    zombieTable.DifficultyGuid = block.readString();
                }

                zombieTable.Slots = new ZombieSlotData[4];
                zombieTable.SlotsCount = block.readByte();

                for (byte j = 0; j < zombieTable.SlotsCount; j++)
                {
                    ZombieSlotData zombieSlot = new();
                    zombieTable.Slots[j] = zombieSlot;
                    zombieSlot.Clothes = new List<ZombieClothData>();

                    zombieSlot.Chance = block.readSingle();
                    zombieSlot.ClothesCount = block.readByte();

                    for (byte k = 0; k < zombieSlot.ClothesCount; k++)
                    {
                        ZombieClothData zombieCloth = new();
                        zombieCloth.Item = block.readUInt16();
                        zombieSlot.Clothes.Add(zombieCloth);
                    }
                }
            }

            return content;
        }
    }
}
