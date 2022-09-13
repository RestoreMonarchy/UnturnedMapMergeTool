using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Models.Enums;
using UnturnedMapMergeTool.Models.Configs;

namespace UnturnedMapMergeTool
{
    public class Config
    {
        public List<MapConfig> Maps { get; set; } = new()
        {
            new MapConfig()
            {
                Name = "PEI",
                Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Unturned\\Maps\\PEI",
                Size = EMapSize.Medium,
                WithBorders = false
            },
            new MapConfig()
            {
                Name = "Washington",
                Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Unturned\\Maps\\Washington",
                Size = EMapSize.Medium,
                WithBorders = true
            }
        };

        public OutputMapConfig OutputMap { get; set; } = new()
        {
            OutputDirectoryPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Unturned\\Maps\\MergedMaps"
        };
    }
}
