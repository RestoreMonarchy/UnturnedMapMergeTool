using System.Collections.Generic;
using UnturnedMapMergeTool.Models.Contents.Items;
using UnturnedMapMergeTool.Models.Contents.Vehicles;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class ItemsDataContent
    {
        public ItemsDataContent(byte saveDataVersion, byte tablesCount)
        {
            SaveDataVersion = saveDataVersion;
            TablesCount = tablesCount;

            Tables = new();
        }

        public ItemsDataContent()
        {

        }

        public byte SaveDataVersion { get; set; }
        public byte TablesCount { get; set; }
        public List<ItemTableData> Tables { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            river.writeByte(SaveDataVersion);
            river.writeByte(TablesCount);

            foreach (ItemTableData itemTable in Tables)
            {
                river.writeColor(itemTable.Color);
                river.writeString(itemTable.Name);
                river.writeUInt16(itemTable.TableId);
                river.writeByte(itemTable.TiersCount);

                foreach (ItemTierData itemTier in itemTable.Tiers)
                {
                    river.writeString(itemTier.Name);
                    river.writeSingle(itemTier.Chance);
                    river.writeByte(itemTier.SpawnsCount);

                    foreach (ItemSpawnData itemSpawn in itemTier.Spawns)
                    {
                        river.writeUInt16(itemSpawn.ItemId);
                    }
                }
            }

            river.closeRiver();
        }

        public static ItemsDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            ItemsDataContent content = new();

            content.SaveDataVersion = river.readByte();

            if (content.SaveDataVersion > 1 && content.SaveDataVersion < 3)
            {
                river.readSteamID();
            }

            content.TablesCount = river.readByte();
            content.Tables = new();

            for (byte i = 0; i < content.TablesCount; i++)
            {
                ItemTableData itemTable = new();
                itemTable.Color = river.readColor();
                itemTable.Name = river.readString();

                if (content.SaveDataVersion > 3)
                {
                    itemTable.TableId = river.readUInt16();
                }
                else
                {
                    itemTable.TableId = 0;
                }

                itemTable.TiersCount = river.readByte();
                itemTable.Tiers = new();

                for (byte j = 0; j < itemTable.TiersCount; j++)
                {
                    ItemTierData itemTier = new();
                    itemTier.Name = river.readString();
                    itemTier.Chance = river.readSingle();
                    itemTier.SpawnsCount = river.readByte();
                    itemTier.Spawns = new();

                    for (byte k = 0; k < itemTier.SpawnsCount; k++)
                    {
                        ItemSpawnData itemSpawn = new();
                        itemSpawn.ItemId = river.readUInt16();
                        itemTier.Spawns.Add(itemSpawn);
                    }

                    itemTable.Tiers.Add(itemTier);
                }

                content.Tables.Add(itemTable);
            }

            river.closeRiver();
            return content;
        }
    }
}
