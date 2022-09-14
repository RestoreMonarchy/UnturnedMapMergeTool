using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnturnedMapMergeTool.Models.Enums;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Models;

namespace UnturnedMapMergeTool
{
    public class Config
    {
        public List<CopyMapConfig> Maps { get; set; } = new()
        {
            new CopyMapConfig()
            {
                Name = "PEI",
                Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Unturned\\Maps\\PEI",
                Size = EMapSize.Medium,
                WithBorders = false,
                StartCoordinate = new Coordinate()
                {
                    X = 1,
                    Y = 1
                },
                ShiftX = -2000,
                ShiftY = -2000
            },
            new CopyMapConfig()
            {
                Name = "Washington",
                Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Unturned\\Maps\\Washington",
                Size = EMapSize.Medium,
                WithBorders = true,
                StartCoordinate = new Coordinate()
                {
                    X = 5,
                    Y = 3
                },
                ShiftX = 2000,
                ShiftY = 0
            }
        };

        public OutputMapConfig OutputMap { get; set; } = new()
        {
            Path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Unturned\\Maps\\MergedMaps"
        };
    }
}
