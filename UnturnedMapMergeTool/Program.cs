using System;
using UnturnedMapMergeTool;
using UnturnedMapMergeTool.Copiers;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Configs;

internal class Program
{
    private static void Main(string[] args)
    {
        Config config = new();
        OutputMapService outputMap = new(config.OutputMap);

        // Delete existing directories and create empty
        outputMap.Preapare();

        foreach (MapConfig map in config.Maps)
        {
            Console.Write($"[{map.Name}] Start coordinate X: ");
            int xCoordinate = int.Parse(Console.ReadLine());
            Console.Write($"[{map.Name}] Start coordinate Y: ");
            int yCoordinate = int.Parse(Console.ReadLine());

            Coordinate startCooprdinate = new()
            {
                X = xCoordinate,
                Y = yCoordinate
            };

            CopyMapService mapCopier = new(map, startCooprdinate, config.OutputMap.OutputDirectoryPath);
            mapCopier.CopyTiles();
        }

        Console.ReadKey();
    }
}