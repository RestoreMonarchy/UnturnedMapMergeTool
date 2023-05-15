using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Helpers;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Services
{
    public partial class CopyMap
    {
        public void CopyAllTiles()
        {
            CopyTilesFromDirectory("Landscape/Heightmaps", ETileType.Heightmap);
            CopyTilesFromDirectory("Landscape/Splatmaps", ETileType.Splatmap);
            CopyTilesFromDirectory("Landscape/Holes", ETileType.Hole);
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

                if (!config.WithBorders && TilesHelper.IsBorder(fileInfo.OriginalTileX, fileInfo.OriginalTileY, config.Size))
                {
                    LogTile(tileType, $"Skipping border tile {fileName}");
                    continue;
                }              

                Coordinate originalCoordinate = new()
                {
                    X = TilesHelper.TileToIndex(fileInfo.OriginalTileX, config.Size),
                    Y = TilesHelper.TileToIndex(fileInfo.OriginalTileY, config.Size)
                };

                if (config.BypassTiles != null && config.BypassTiles.Length > 0)
                {
                    if (config.BypassTiles.Contains(originalCoordinate))
                    {
                        LogTileWarning(tileType, $"The tile {originalCoordinate} was on the bypass list");
                        continue;
                    }
                }

                Coordinate targetCoordinate = new()
                {
                    X = originalCoordinate.X + StartCoordinate.X,
                    Y = originalCoordinate.Y + StartCoordinate.Y
                };

                int targetTileX = TilesHelper.IndexToTile(targetCoordinate.X, outputMap.Config.Size);
                int targetTileY = TilesHelper.IndexToTile(targetCoordinate.Y, outputMap.Config.Size);

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
    }
}
