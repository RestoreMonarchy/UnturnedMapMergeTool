using System;
using System.Collections.Generic;
using System.Linq;
using UnturnedMapMergeTool.Models.Enums;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Helpers
{
    public class TilesHelper
    {
        private static Dictionary<EMapSize, int[]> MapTiles = new()
        {
            { EMapSize.Tiny, new int[] { -1, 0 } },
            { EMapSize.Small, new int[] { -1, 0 } },
            { EMapSize.Medium, new int[] { -1, 0 } },
            { EMapSize.Large, new int[] { -2, -1, 0, 1 } },
            { EMapSize.Insane, new int[] { -4, -3, -2, -1, 0, 1, 2, 3 } }
        };

        public static int TileToIndex(int tile, EMapSize mapSize)
        {
            return mapSize switch
            {
                EMapSize.Tiny => tile + 1,
                EMapSize.Small => tile + 1,
                EMapSize.Medium => tile + 1,
                EMapSize.Large => tile + 2,
                EMapSize.Insane => tile + 4,                
                _ => throw new NotSupportedException($"{mapSize} map size is not supported")
            };
        }

        public static int IndexToTile(int index, EMapSize mapSize)
        {
            return mapSize switch
            {
                EMapSize.Tiny => index - 1,
                EMapSize.Small => index - 1,
                EMapSize.Medium => index - 1,
                EMapSize.Large => index - 2,
                EMapSize.Insane => index - 4,
                _ => throw new NotSupportedException($"{mapSize} map size not supported")
            };
        }

        public static int TileToShift(int tile, EMapSize mapSize)
        {
            return mapSize switch
            {
                EMapSize.Medium => tile - 1,
                _ => throw new NotSupportedException($"{mapSize} map size not supported")
            };
        }

        public static bool IsBorder(int tileX, int tileY, EMapSize mapSize)
        {
            if (!MapTiles[mapSize].Contains(tileX))
            {
                return true;
            }

            if (!MapTiles[mapSize].Contains(tileY))
            {
                return true;
            }

            return false;
        }

        public static bool IsBorder(Vector3 position, EMapSize mapSize)
        {
            int min = MapTiles[mapSize].Min() * 1024;
            int max = (MapTiles[mapSize].Max() + 1) * 1024;

            if (position.z > max || position.z < min)
                return true;
            if (position.x > max || position.x < min)
                return true;

            return false;
        }
    }
}
