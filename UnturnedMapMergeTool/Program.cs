using System;
using UnturnedMapMergeTool;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        Config config = new();
        OutputMap outputMap = new(config.OutputMap);

        // Delete existing directories and create empty
        outputMap.Preapare();

        foreach (CopyMapConfig map in config.Maps)
        {
            CopyMap mapCopier = new(map, outputMap);
            mapCopier.CopyAllTiles();
            mapCopier.CopyLevel();
        }

        Console.ReadKey();
    }
}