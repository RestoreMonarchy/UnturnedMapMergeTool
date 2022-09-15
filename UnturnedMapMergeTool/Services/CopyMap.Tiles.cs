using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Helpers;
using UnturnedMapMergeTool.Models.Enums;
using UnturnedMapMergeTool.Models;

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
    }
}
