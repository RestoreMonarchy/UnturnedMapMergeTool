﻿using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
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

        Config config;
        if (File.Exists("config.json"))
        {
            string configJson = File.ReadAllText("config.json");
            config = JsonConvert.DeserializeObject<Config>(configJson);
        } else
        {
            config = new();
            string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText("config.json", configJson);
        }

        
        OutputMap outputMap = new(config.OutputMap);

        // Delete existing directories and create empty
        outputMap.Preapare();

        List<CopyMap> copyMaps = new();

        ObjectsDataMergeTool objectsDataMergeTool = new();
        BuildablesDataMergeTool buildablesDataMergeTool = new();
        TreesDataMergeTool treesDataMergeTool = new();
        RoadsDataMergeTool roadsDataMergeTool = new();
        PathsDataMergeTool pathsDataMergeTool = new();
        NodesDataMergeTool nodesDataMergeTool = new();

        foreach (CopyMapConfig mapConfig in config.Maps)
        {
            CopyMap copyMap = new(mapConfig, outputMap);

            objectsDataMergeTool.ReadData(copyMap);
            buildablesDataMergeTool.ReadData(copyMap);
            treesDataMergeTool.ReadData(copyMap);
            
            roadsDataMergeTool.ReadData(copyMap);
            pathsDataMergeTool.ReadData(copyMap);
            nodesDataMergeTool.ReadData(copyMap);

            copyMaps.Add(copyMap);

            copyMap.CopyAllTiles();
        }

        objectsDataMergeTool.CombineAndSaveData(outputMap);
        buildablesDataMergeTool.CombineAndSaveData(outputMap);
        treesDataMergeTool.CombineAndSaveData(outputMap);
        roadsDataMergeTool.CombineAndSaveData(outputMap);
        pathsDataMergeTool.CombineAndSaveData(outputMap);        
        nodesDataMergeTool.CombineAndSaveData(outputMap);

        Console.ReadKey();
    }
}