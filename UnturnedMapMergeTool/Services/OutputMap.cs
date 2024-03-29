﻿using System.Collections.Generic;
using System.IO;
using UnturnedMapMergeTool.Models.Configs;

namespace UnturnedMapMergeTool.Services
{
    public partial class OutputMap
    {
        private readonly OutputMapConfig config;

        public OutputMap(OutputMapConfig config)
        {
            this.config = config;
        }

        public OutputMapConfig Config => config;

        public void Preapare()
        {
            PrepareLandscapes();
            PrepareSpawns();
            PrepareLevel();
        }

        private void PrepareLevel()
        {
            string spawnsDirectory = CombinePath("Level");
            if (Directory.Exists(spawnsDirectory))
            {
                Directory.Delete(spawnsDirectory, true);
            }

            Directory.CreateDirectory(spawnsDirectory);
        }

        private void PrepareSpawns()
        {
            string spawnsDirectory = CombinePath("Spawns");
            if (Directory.Exists(spawnsDirectory))
            {
                Directory.Delete(spawnsDirectory, true);
            }

            Directory.CreateDirectory(spawnsDirectory);
        }

        private void PrepareLandscapes()
        {
            string landscapesPath = Path.Combine(config.Path, "Landscape");
            List<string> otherDirectories = new List<string>()
            {
                Path.Combine(landscapesPath, "Heightmaps"),
                Path.Combine(landscapesPath, "Splatmaps"),
                Path.Combine(landscapesPath, "Holes")
            };

            if (Directory.Exists(landscapesPath))
            {
                Directory.Delete(landscapesPath, true);
            }

            Directory.CreateDirectory(landscapesPath);

            foreach (string directoryPath in otherDirectories)
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
        }
    }
}
