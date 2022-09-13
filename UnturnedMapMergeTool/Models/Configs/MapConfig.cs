﻿using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Models.Configs
{
    public class MapConfig
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public EMapSize Size { get; set; }
        public bool WithBorders { get; set; }
    }
}
