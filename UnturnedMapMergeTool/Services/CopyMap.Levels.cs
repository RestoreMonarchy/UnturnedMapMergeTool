using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnturnedMapMergeTool.Models.Contents;

namespace UnturnedMapMergeTool.Services
{
    public partial class CopyMap
    {
        public ObjectDataContent ObjectDataContent { get; private set; }
        public BuildableDataContent BuildableDataContent { get; private set; }

        public void ReadLevel()
        {
            Log("Reading level...");
            ReadLevelObjects();
            ReadLevelBuildables();
            Log("Done reading level");
        }

        private void ReadLevelObjects()
        {
            string fileNamePath = Path.Combine(config.Path, "Level/Objects.dat");
            ObjectDataContent = ObjectDataContent.FromFile(fileNamePath);
            File.WriteAllText($"objects_{config.Name}.json", JsonConvert.SerializeObject(ObjectDataContent, Formatting.Indented));
            int objectsCount = ObjectDataContent.ObjectRegions.Sum(x => x.Count);
            Log($"Read {objectsCount} objects");
        }

        private void ReadLevelBuildables()
        {
            string fileNamePath = Path.Combine(config.Path, "Level/Buildables.dat");
            BuildableDataContent = BuildableDataContent.FromFile(fileNamePath);
            File.WriteAllText($"buildables_{config.Name}.json", JsonConvert.SerializeObject(BuildableDataContent, Formatting.Indented));
            int buildablesCount = BuildableDataContent.BuildableRegions.Sum(x => x.Count);
            Log($"Read {buildablesCount} buildables");
        }
    }
}
