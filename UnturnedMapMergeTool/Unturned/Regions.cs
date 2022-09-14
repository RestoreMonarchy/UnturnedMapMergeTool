using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Unturned
{
    public class Regions
    {
        public static readonly byte WORLD_SIZE = 64;
        public static readonly byte REGION_SIZE = 128;

        public static bool tryGetCoordinate(Vector3 point, out byte x, out byte y)
        {
            x = byte.MaxValue;
            y = byte.MaxValue;
            if (Regions.checkSafe(point))
            {
                x = (byte)((point.x + 4096f) / (float)Regions.REGION_SIZE);
                y = (byte)((point.z + 4096f) / (float)Regions.REGION_SIZE);
                return true;
            }
            return false;
        }

        public static bool checkSafe(Vector3 point)
        {
            return point.x >= -4096f && point.z >= -4096f && point.x < 4096f && point.z < 4096f;
        }
    }
}
