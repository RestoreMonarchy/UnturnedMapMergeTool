using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Unturned
{
    public class MathfEx
    {
        public static byte ClampToByte(int value)
        {
            return (byte)Mathf.Clamp(value, 0, 255);
        }

        public static ushort ClampToUShort(int value)
        {
            return (ushort)Mathf.Clamp(value, 0, 65535);
        }
    }
}
