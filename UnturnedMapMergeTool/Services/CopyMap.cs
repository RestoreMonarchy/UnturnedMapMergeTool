using System;
using System.Collections.Generic;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Models.Contents.Flags;
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

        public void ApplyPositionShift(Vector3 position)
        {
            position.x += Config.ShiftX;
            position.z += Config.ShiftY;
        }

        public byte MaterialShift { get; set; } = 0;

        // This doesn't work because there cannot be more than 10 materials (in the Roads.unity3d file) on one map
        public byte GetShiftedMaterialId(byte material)
        {
            return (byte)(material + MaterialShift);
        }

        public byte ZombieTypeShift { get; set; } = 0;

        public byte GetShiftedZombieType(byte zombieType)
        {
            return (byte)(zombieType + ZombieTypeShift);
        }

        // Used and necessary for NavigationsDataMergeTool
        public List<FlagData> Flags { get; set; }
        public int FlagsStartIndex { get; set; }

    }
}
