using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Models.Contents.Faunas;
using UnturnedMapMergeTool.Unturned;

namespace UnturnedMapMergeTool.Models.Contents
{
    public class FaunaDataContent
    {
        public FaunaDataContent(byte saveDataVersion, byte tablesCount, ushort spawnpointsCount)
        {
            SaveDataVersion = saveDataVersion;
            TablesCount = tablesCount;
            SpawnPointsCount = spawnpointsCount;

            Tables = new();
            SpawnPoints = new();
            
        }

        public FaunaDataContent()
        {

        }

        public byte SaveDataVersion { get; set; }
        
        public byte TablesCount { get; set; }
        public List<AnimalTableData> Tables { get; set; }

        public ushort SpawnPointsCount { get; set; }
        public List<AnimalSpawnpointData> SpawnPoints { get; set; }

        public void SaveToFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            river.writeByte(SaveDataVersion);
            river.writeByte(TablesCount);

            foreach (AnimalTableData animalTable in Tables)
            {
                river.writeColor(animalTable.Color);
                river.writeString(animalTable.Name);
                river.writeUInt16(animalTable.TableId);
                river.writeByte(animalTable.TiersCount);

                foreach (AnimalTierData animalTier in animalTable.Tiers)
                {
                    river.writeString(animalTier.Name);
                    river.writeSingle(animalTier.Chance);
                    river.writeByte(animalTier.SpawnsCount);

                    foreach (AnimalSpawnData animalSpawn in animalTier.Spawns)
                    {
                        river.writeUInt16(animalSpawn.Animal);
                    }
                }
            }

            river.writeUInt16(SpawnPointsCount);
            foreach (AnimalSpawnpointData animalSpawnPoint in SpawnPoints)
            {
                river.writeByte(animalSpawnPoint.Type);
                river.writeSingleVector3(animalSpawnPoint.Point);
            }

            river.closeRiver();
        }

        public static FaunaDataContent FromFile(string fileNamePath)
        {
            River river = new(fileNamePath);

            FaunaDataContent content = new();
            content.Tables = new();
            content.SpawnPoints = new();

            content.SaveDataVersion = river.readByte();
            
            content.TablesCount = river.readByte();

            for (byte i = 0; i < content.TablesCount; i++)
            {
                AnimalTableData animalTable = new();
                animalTable.Tiers = new();
                
                animalTable.Color = river.readColor();
                animalTable.Name = river.readString();
                if (content.SaveDataVersion > 2)
                {
                    animalTable.TableId = river.readUInt16();
                } else
                {
                    animalTable.TableId = 0;
                }                

                animalTable.TiersCount = river.readByte();

                for (byte j = 0; j < animalTable.TiersCount; j++)
                {
                    AnimalTierData animalTier = new();
                    animalTier.Spawns = new();

                    animalTier.Name = river.readString();
                    animalTier.Chance = river.readSingle();
                    animalTier.SpawnsCount = river.readByte();

                    for (byte k = 0; k < animalTier.SpawnsCount; k++)
                    {
                        AnimalSpawnData animalSpawn = new();
                        animalSpawn.Animal = river.readUInt16();

                        animalTier.Spawns.Add(animalSpawn);
                    }

                    animalTable.Tiers.Add(animalTier);
                }

                content.Tables.Add(animalTable);
            }

            content.SpawnPointsCount = river.readUInt16();
            for (byte l = 0; l < content.SpawnPointsCount; l++)
            {
                AnimalSpawnpointData animalSpawnpoint = new();
                animalSpawnpoint.Type = river.readByte();
                animalSpawnpoint.Point = river.readSingleVector3();

                content.SpawnPoints.Add(animalSpawnpoint);
            }


            river.closeRiver();
            return content;
        }

    }
}
