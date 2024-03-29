﻿using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using UnturnedMapMergeTool;
using UnturnedMapMergeTool.DataMergeTools;
using UnturnedMapMergeTool.DataMergeTools.Spawns;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Services;

internal class Program
{

    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt")
            .CreateLogger();

        Log.Information($"UnturnedMapMergeTool v{typeof(Program).Assembly.GetName().Version} has been loaded!");

        Config config;
        if (File.Exists("config.json"))
        {
            string configJson = File.ReadAllText("config.json");
            config = JsonConvert.DeserializeObject<Config>(configJson);
        }
        else
        {
            config = new();
            config.LoadDefaultValues();

            string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText("config.json", configJson);
            Log.Information("Generated a config.json file with. Modify it before running this program again!");
            return;
        }

        try
        {
            Start(config);
        } catch (Exception e)
        {
            Log.Error(e, "");
        }
        
        Log.Information("Press any key to exit the program...");
        Console.ReadKey();
    }

    private static void Start(Config config)
    {
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
        BoundsDataMergeTool boundsDataMergeTool = new();
        FlagsDataMergeTool flagsDataMergeTool = new();
        //NavigationsDataMergeTool navigationsDataMergeTool = new();

        ZombiesDataMergeTool zombiesDataMergeTool = new();
        AnimalsDataMergeTool animalsDataMergeTool = new();
        PlayersDataMergeTool playersDataMergeTool = new();
        FaunaDataMergeTool faunaDataMergeTool = new();
        VehiclesDataMergeTool vehiclesDataMergeTool = new();
        ItemsDataMergeTool itemsDataMergeTool = new();
        JarsDataMergeTool jarsDataMergeTool = new();

        Log.Information($"Start combining {config.Maps.Count} maps");

        foreach (CopyMapConfig mapConfig in config.Maps)
        {
            CopyMap copyMap = new(mapConfig, outputMap);

            objectsDataMergeTool.ReadData(copyMap);
            buildablesDataMergeTool.ReadData(copyMap);
            treesDataMergeTool.ReadData(copyMap);

            roadsDataMergeTool.ReadData(copyMap);
            pathsDataMergeTool.ReadData(copyMap);
            nodesDataMergeTool.ReadData(copyMap);
            boundsDataMergeTool.ReadData(copyMap);
            flagsDataMergeTool.ReadData(copyMap);
            //navigationsDataMergeTool.ReadData(copyMap);

            zombiesDataMergeTool.ReadData(copyMap);
            animalsDataMergeTool.ReadData(copyMap);
            playersDataMergeTool.ReadData(copyMap);
            faunaDataMergeTool.ReadData(copyMap);
            vehiclesDataMergeTool.ReadData(copyMap);
            itemsDataMergeTool.ReadData(copyMap);
            jarsDataMergeTool.ReadData(copyMap);

            copyMaps.Add(copyMap);

            copyMap.CopyAllTiles();
        }

        objectsDataMergeTool.CombineAndSaveData(outputMap);
        buildablesDataMergeTool.CombineAndSaveData(outputMap);
        treesDataMergeTool.CombineAndSaveData(outputMap);
        roadsDataMergeTool.CombineAndSaveData(outputMap);
        pathsDataMergeTool.CombineAndSaveData(outputMap);
        nodesDataMergeTool.CombineAndSaveData(outputMap);
        boundsDataMergeTool.CombineAndSaveData(outputMap);
        flagsDataMergeTool.CombineAndSaveData(outputMap);
        //navigationsDataMergeTool.CombineAndSaveData(outputMap);

        zombiesDataMergeTool.CombineAndSaveData(outputMap);
        animalsDataMergeTool.CombineAndSaveData(outputMap);
        playersDataMergeTool.CombineAndSaveData(outputMap);
        faunaDataMergeTool.CombineAndSaveData(outputMap);
        vehiclesDataMergeTool.CombineAndSaveData(outputMap);
        itemsDataMergeTool.CombineAndSaveData(outputMap);
        jarsDataMergeTool.CombineAndSaveData(outputMap);

        Log.Information($"Finished migrating {config.Maps.Count} maps!");
    }
}