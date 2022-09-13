using System.Collections.Generic;
using System.IO;
using UnturnedMapMergeTool.Models.Configs;

namespace UnturnedMapMergeTool.Copiers
{
    public class OutputMapService
    {
        private readonly OutputMapConfig config;

        public OutputMapService(OutputMapConfig config)
        {
            this.config = config;
        }

        public void Preapare()
        {
            string landscapesPath = Path.Combine(config.OutputDirectoryPath, "Landscape");
            List<string> otherDirectories = new List<string>()
            {
                Path.Combine(landscapesPath, "Heightmaps"),
                Path.Combine(landscapesPath, "Splatmaps")
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
