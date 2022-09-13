using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Helpers;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Copiers
{
    public class CopyMapService
    {
        private readonly MapConfig map;
        private readonly Coordinate startCoordinate;
        private readonly string outputMapDirectory;

        public CopyMapService(MapConfig map, Coordinate startCoordinate, string outputMapDirectory)
        {
            this.map = map;
            this.startCoordinate = startCoordinate;
            this.outputMapDirectory = outputMapDirectory;
        }

        public void CopyTiles()
        {
            CopyTilesFromDirectory("Landscape/Heightmaps");
            CopyTilesFromDirectory("Landscape/Splatmaps");
        }

        private void CopyTilesFromDirectory(string directory)
        {
            string path = Path.Combine(map.Path, directory);
            string outputPath = Path.Combine(outputMapDirectory, directory);

            foreach (string fileNamePath in Directory.EnumerateFiles(path))
            {
                string fileName = Path.GetFileName(fileNamePath);
                TileFileInfo fileInfo = TileFileInfo.FromFileName(fileName);
                Coordinate originalCoordinate = new()
                {
                    X = TilesHelper.TileToIndex(fileInfo.OriginalTileX, map.Size),
                    Y = TilesHelper.TileToIndex(fileInfo.OriginalTileY, map.Size)
                };

                Coordinate targetCoordinate = new()
                {
                    X = originalCoordinate.X + startCoordinate.X,
                    Y = originalCoordinate.Y + startCoordinate.Y
                };

                int targetTileX = TilesHelper.IndexToTile(targetCoordinate.X, EMapSize.Insane);
                int targetTileY = TilesHelper.IndexToTile(targetCoordinate.Y, EMapSize.Insane);

                string targetFileName = string.Format(fileInfo.FileNameFormat, targetTileX, targetTileY);
                string targetFileNamePath = Path.Combine(outputPath, targetFileName);

                bool flag = File.Exists(targetFileNamePath);
                File.Copy(fileNamePath, targetFileNamePath, true);
                if (flag)
                {
                    Log($"Overrided {targetFileName} with {fileName}");
                }
                else
                {
                    Log($"Copied {fileName} to {targetFileName}");
                }
            }
        }

        private void Log(string message)
        {
            Console.WriteLine($"[{map.Name}]: {message}");
        }
    }
}
