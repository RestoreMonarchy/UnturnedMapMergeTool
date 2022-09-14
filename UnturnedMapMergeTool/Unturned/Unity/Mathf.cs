using System;

namespace UnturnedMapMergeTool.Unturned.Unity
{
    public class Mathf
    {
        public static int CeilToInt(float f)
        {
            return (int)Math.Ceiling(f);
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }
    }
}
