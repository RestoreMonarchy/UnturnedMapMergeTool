using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Helpers;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Models.Contents;
using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Services
{
    public class CopyMap
    {
        private readonly CopyMapConfig config;
        private readonly OutputMap outputMap;

        public Guid Identifier { get; private set; }

        public CopyMap(CopyMapConfig config, OutputMap outputMap)
        {
            Identifier = Guid.NewGuid();
            this.config = config;
            this.outputMap = outputMap;
        }

        public CopyMapConfig Config => config;

        private Coordinate StartCoordinate => config.StartCoordinate;

        public ObjectDataContent ObjectDataContent { get; private set; }

        public void CopyAllTiles()
        {
            CopyTilesFromDirectory("Landscape/Heightmaps", ETileType.Heightmap);
            CopyTilesFromDirectory("Landscape/Splatmaps", ETileType.Splatmap);
            CopyTilesFromDirectory("Landscape/Holes", ETileType.Hole);
        }

        public void ReadLevel() 
        {
            Log("Reading level...");
            ReadLevelObjects();
            Log("Done reading level");
        }

        private void ReadLevelObjects()
        {
            string fileNamePath = Path.Combine(config.Path, "Level/Objects.dat");
            ObjectDataContent = ObjectDataContent.FromFile(fileNamePath);
            File.WriteAllText($"objects_{config.Name}.json", JsonConvert.SerializeObject(ObjectDataContent, Formatting.Indented));
            int objectsCount = ObjectDataContent.ObjectRegions.Sum(x => x.Count);
            Log($"Read {objectsCount} objects");
        }

        private void CopyTilesFromDirectory(string directory, ETileType tileType)
        {
            string directoryPath = Path.Combine(config.Path, directory);

            if (!Directory.Exists(directoryPath))
            {
                LogTile(tileType, $"No {tileType} tiles on this map");
                return;
            }

            string outputDirectoryPath = outputMap.CombinePath(directory);

            foreach (string fileNamePath in Directory.EnumerateFiles(directoryPath))
            {
                string fileName = Path.GetFileName(fileNamePath);
                TileFileInfo fileInfo = TileFileInfo.FromFileName(fileName, tileType);
                Coordinate originalCoordinate = new()
                {
                    X = TilesHelper.TileToIndex(fileInfo.OriginalTileX, config.Size),
                    Y = TilesHelper.TileToIndex(fileInfo.OriginalTileY, config.Size)
                };

                if (!config.WithBorders && TilesHelper.IsBorder(originalCoordinate, config.Size))
                {
                    LogTile(tileType, $"Skipping border {originalCoordinate} tile {fileName}");
                    continue;
                }

                Coordinate targetCoordinate = new()
                {
                    X = originalCoordinate.X + StartCoordinate.X,
                    Y = originalCoordinate.Y + StartCoordinate.Y
                };

                int targetTileX = TilesHelper.IndexToTile(targetCoordinate.X, EMapSize.Insane);
                int targetTileY = TilesHelper.IndexToTile(targetCoordinate.Y, EMapSize.Insane);

                string targetFileName = string.Format(fileInfo.FileNameFormat, targetTileX, targetTileY);
                string targetFileNamePath = Path.Combine(outputDirectoryPath, targetFileName);

                bool flag = File.Exists(targetFileNamePath);
                File.Copy(fileNamePath, targetFileNamePath, true);
                if (flag)
                {
                    LogTile(tileType, $"Overrided {targetFileName} with {fileName}");
                }
                else
                {
                    LogTile(tileType, $"Copied {fileName} to {targetFileName}");
                }
            }
        }

        private void LogTile(ETileType tileType, string message)
        {
            Log($"[{tileType}] {message}");
        }

        private void Log(string message)
        {
            Console.WriteLine($"[{config.Name}]: {message}");
        }
    }
}
