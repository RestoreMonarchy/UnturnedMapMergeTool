using System;
using System.Collections.Generic;
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

        List<CopyMap> copyMaps = new();
        foreach (CopyMapConfig map in config.Maps)
        {
            CopyMap copyMap = new(map, outputMap);
            copyMaps.Add(copyMap);

            copyMap.CopyAllTiles();
            copyMap.ReadLevel();
        }

        LevelService levelService = new(outputMap, copyMaps);
        levelService.CombineAndSaveObjects();

        Console.ReadKey();
    }
}