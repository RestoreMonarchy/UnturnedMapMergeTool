using System;
using System.Collections.Generic;
using UnturnedMapMergeTool.Helpers;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Configs;
using UnturnedMapMergeTool.Models.Contents.Flags;
using UnturnedMapMergeTool.Models.Enums;
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

        public bool IsOriginalPositionBypassed(Vector3 position)
        {
            if (!ShouldIncludePosition(position))
            {
                return true;
            }

            if (config.BypassTiles == null)
            {
                return false;
            }

            foreach (Coordinate coordinate in config.BypassTiles)
            {
                int shiftX = TilesHelper.TileToShift(coordinate.X, config.Size);
                int shiftY = TilesHelper.TileToShift(coordinate.Y, config.Size);
                int xMin = shiftX * 1024;
                int xMax = (shiftX + 1) * 1024;
                int yMin = shiftY * 1024;
                int yMax = (shiftY + 1) * 1024;

                if ((xMin <= position.x && xMax >= position.x) && (yMin <= position.z && yMax >= position.z))
                {
                    return true;
                }
            }

            return false;
        }

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

        public byte ItemTypeShift { get; set; } = 0;

        public byte GetShiftedItemType(byte itemType)
        {
            return (byte)(itemType + ItemTypeShift);
        }

        // Used and necessary for NavigationsDataMergeTool
        public List<FlagData> Flags { get; set; }
        public int FlagsStartIndex { get; set; }

        public bool ShouldIncludePosition(Vector3 position) 
        {
            return config.WithBorders || !TilesHelper.IsBorder(position, config.Size);
        }
    }
}
