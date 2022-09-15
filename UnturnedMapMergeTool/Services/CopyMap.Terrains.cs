using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Models.Contents;

namespace UnturnedMapMergeTool.Services
{
    public partial class CopyMap
    {
        public TreesDataContent TreesDataContent { get; private set; }

        public void ReadTerrain()
        {
            Log("Reading terrain...");
            ReadTerrainTrees();
            Log("Done reading terrain");
        }

        private void ReadTerrainTrees()
        {
            string fileNamePath = Path.Combine(config.Path, "Terrain/Trees.dat");
            TreesDataContent = TreesDataContent.FromFile(fileNamePath);
            File.WriteAllText($"trees_{config.Name}.json", JsonConvert.SerializeObject(TreesDataContent, Formatting.Indented));
            int treesCount = TreesDataContent.TreeRegions.Sum(x => x.Count);
            Log($"Read {treesCount} trees");
        }
    }
}
