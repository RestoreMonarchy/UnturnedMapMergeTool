using Serilog;
using System;
using System.Collections.Generic;
using UnturnedMapMergeTool;
using UnturnedMapMergeTool.DataMergeTools;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Config config = new();
        OutputMap outputMap = new(config.OutputMap);

        // Delete existing directories and create empty
        outputMap.Preapare();

        List<CopyMap> copyMaps = new();

        ObjectsDataMergeTool objectsDataMergeTool = new();
        BuildablesDataMergeTool buildablesDataMergeTool = new();
        TreesDataMergeTool treesDataMergeTool = new();
        PathsDataMergeTool pathsDataMergeTool = new();
        RoadsDataMergeTool roadsDataMergeTool = new();
        NodesDataMergeTool nodesDataMergeTool = new();



        foreach (CopyMapConfig mapConfig in config.Maps)
        {
            CopyMap copyMap = new(mapConfig, outputMap);

            objectsDataMergeTool.ReadData(copyMap);
            buildablesDataMergeTool.ReadData(copyMap);
            treesDataMergeTool.ReadData(copyMap);
            pathsDataMergeTool.ReadData(copyMap);
            roadsDataMergeTool.ReadData(copyMap);
            nodesDataMergeTool.ReadData(copyMap);

            copyMaps.Add(copyMap);

            copyMap.CopyAllTiles();
        }

        objectsDataMergeTool.CombineAndSaveData(outputMap);
        buildablesDataMergeTool.CombineAndSaveData(outputMap);
        treesDataMergeTool.CombineAndSaveData(outputMap);
        pathsDataMergeTool.CombineAndSaveData(outputMap);
        roadsDataMergeTool.CombineAndSaveData(outputMap);
        nodesDataMergeTool.CombineAndSaveData(outputMap);
        

        Console.ReadKey();
    }
}