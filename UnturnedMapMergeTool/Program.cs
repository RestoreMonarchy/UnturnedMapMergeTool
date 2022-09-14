using System;
using UnturnedMapMergeTool;
using UnturnedMapMergeTool.Services;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Configs;
using SDG.Unturned;

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
        }

        Console.ReadKey();
    }
}