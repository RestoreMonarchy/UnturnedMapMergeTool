using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnturnedMapMergeTool.Models.Contents.Zombies;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class ZombiesDataContent
    {
        public ZombiesDataContent(byte saveDataVersion, byte tablesCount)
        {
            SaveDataVersion = saveDataVersion;
            TablesCount = tablesCount;

            Tables = new();
        }

        public ZombiesDataContent()
        {

        }

        public byte SaveDataVersion { get; set; }
        public byte TablesCount { get; set; }

        public List<ZombieTableData> Tables { get; set; }
         
        public void SaveToFile(string fileNamePath)
        {
            Block block = new();
            block.writeByte(SaveDataVersion);
            block.writeByte(TablesCount);

            for (byte i = 0; i < TablesCount; i++)
            {
                ZombieTableData zombieTable = Tables[i];
                block.writeColor(zombieTable.Color);
                block.writeString(zombieTable.Name);
                block.writeBoolean(zombieTable.IsMega);
                block.writeUInt16(zombieTable.Health);
                block.writeByte(zombieTable.Damage);
                block.writeByte(zombieTable.LootIndex);
                block.writeUInt16(zombieTable.LootId);
                block.writeUInt32(zombieTable.XP);
                block.writeSingle(zombieTable.Regen);
                block.writeString(zombieTable.DifficultyGuid);

                block.writeByte(zombieTable.SlotsCount);

                for (byte j = 0; j < zombieTable.SlotsCount; j++)
                {
                    ZombieSlotData zombieSlot = zombieTable.Slots[j];
                    block.writeSingle(zombieSlot.Chance);

                    block.writeByte(zombieSlot.ClothesCount);

                    for (byte k = 0; k < zombieSlot.ClothesCount; k++)
                    {
                        ZombieClothData zombieCloth = zombieSlot.Clothes[k];
                        block.writeUInt16(zombieCloth.Item);
                    }
                }
            }

            byte[] bytes = block.getBytes(out _);
            File.WriteAllBytes(fileNamePath, bytes);
        }

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

            if (content.SaveDataVersion >= 10)
            {
                block.readInt32();
            }

            content.Tables = new();

            // TODO: Add support for SavedataVersion < 2

            content.TablesCount = block.readByte();
            for (byte i = 0; i < content.TablesCount; i++)
            {
                ZombieTableData zombieTable = new();
                content.Tables.Add(zombieTable);

                if (content.SaveDataVersion >= 10)
                {
                    block.readInt32();
                }
                
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

                zombieTable.SlotsCount = block.readByte();
                zombieTable.Slots = new ZombieSlotData[zombieTable.SlotsCount];
                
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
