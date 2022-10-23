using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Models.Configs
{
    public class CopyMapConfig
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public EMapSize Size { get; set; }
        public bool WithBorders { get; set; }
        public Coordinate StartCoordinate { get; set; }
        public int ShiftX { get; set; }
        public int ShiftY { get; set; }
        public bool IgnoreAirdropNodes { get; set; }
        public bool IgnoreArenaNodes { get; set; }
    }
}
