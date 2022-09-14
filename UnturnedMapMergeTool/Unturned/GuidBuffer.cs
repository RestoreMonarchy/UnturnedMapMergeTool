using System;

namespace UnturnedMapMergeTool.Unturned
{
    public class GuidBuffer
    {
        public static Guid FromBytes(byte[] bytes)
        {
            return new Guid(bytes);
        }

        public static byte[] ToBytes(Guid guid)
        {
            return guid.ToByteArray();
        }
    }
}
