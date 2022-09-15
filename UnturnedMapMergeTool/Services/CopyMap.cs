using System;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Services
{
    public partial class CopyMap
    {
        private readonly CopyMapConfig config;
        private readonly OutputMap outputMap;

        public Guid Identifier { get; private set; }

        public CopyMap(CopyMapConfig config, OutputMap outputMap)
        {
            Identifier = Guid.NewGuid();
            this.config = config;
            this.outputMap = outputMap;
        }

        public CopyMapConfig Config => config;

        private Coordinate StartCoordinate => config.StartCoordinate;

        public void ApplyShift(Vector3 position)
        {
            position.x += Config.ShiftX;
            position.z += Config.ShiftY;
        }

        
    }
}
