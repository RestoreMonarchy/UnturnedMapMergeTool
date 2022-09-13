using System;
using UnturnedMapMergeTool.Models;
using UnturnedMapMergeTool.Models.Enums;

namespace UnturnedMapMergeTool.Helpers
{
    public class TilesHelper
    {
        public static int TileToIndex(int tile, EMapSize mapSize)
        {
            return mapSize switch
            {
                EMapSize.Medium => tile + 2,
                EMapSize.Insane => tile + 5,
                _ => throw new NotSupportedException($"{mapSize} map size is not supported")
            };
        }

        public static int IndexToTile(int index, EMapSize mapSize)
        {
            return mapSize switch
            {
                EMapSize.Medium => index - 2,
                EMapSize.Insane => index - 5,
                _ => throw new NotSupportedException($"{mapSize} map size not supported")
            };
        }

        public static bool IsBorder(Coordinate coordinate, EMapSize mapSize)
        {
            return mapSize switch
            {
                EMapSize.Medium => coordinate.X == 0 || coordinate.Y == 0 || coordinate.X == 3 || coordinate.Y == 3,
                EMapSize.Insane => coordinate.X == 0 || coordinate.Y == 0 || coordinate.X == 9 || coordinate.Y == 9,
                _ => throw new NotSupportedException($"{mapSize} map size not supported")
            };
        }
    }
}
