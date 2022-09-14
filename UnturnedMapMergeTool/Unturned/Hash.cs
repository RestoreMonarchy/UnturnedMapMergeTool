using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Unturned
{
    public class Hash
    {
        // Token: 0x06004651 RID: 18001 RVA: 0x001A5B91 File Offset: 0x001A3D91
        public static byte[] SHA1(byte[] bytes)
        {

            return Hash.service.ComputeHash(bytes);
        }

        // Token: 0x06004652 RID: 18002 RVA: 0x001A5B9E File Offset: 0x001A3D9E
        public static byte[] SHA1(Stream stream)
        {
            return Hash.service.ComputeHash(stream);
        }

        // Token: 0x06004653 RID: 18003 RVA: 0x001A5BAB File Offset: 0x001A3DAB
        public static byte[] SHA1(string text)
        {
            return Hash.SHA1(Encoding.UTF8.GetBytes(text));
        }

        // Token: 0x06004654 RID: 18004 RVA: 0x001A5BBD File Offset: 0x001A3DBD
        public static byte[] SHA1(CSteamID steamID)
        {
            return Hash.SHA1(BitConverter.GetBytes(steamID.m_SteamID));
        }

        // Token: 0x06004655 RID: 18005 RVA: 0x001A5BD0 File Offset: 0x001A3DD0
        public static bool verifyHash(byte[] hash_0, byte[] hash_1)
        {
            if (hash_0.Length != 20 || hash_1.Length != 20)
            {
                return false;
            }
            for (int i = 0; i < hash_0.Length; i++)
            {
                if (hash_0[i] != hash_1[i])
                {
                    return false;
                }
            }
            return true;
        }

        // Token: 0x06004656 RID: 18006 RVA: 0x001A5C06 File Offset: 0x001A3E06
        public static byte[] combineSHA1Hashes(byte[] a, byte[] b)
        {
            if (a.Length != 20 || b.Length != 20)
            {
                throw new Exception("both lengths should be 20");
            }
            a.CopyTo(Hash._40bytes, 0);
            b.CopyTo(Hash._40bytes, 20);
            return Hash.SHA1(Hash._40bytes);
        }

        // Token: 0x06004657 RID: 18007 RVA: 0x001A5C44 File Offset: 0x001A3E44
        public static byte[] combine(params byte[][] hashes)
        {
            byte[] array = new byte[hashes.Length * 20];
            for (int i = 0; i < hashes.Length; i++)
            {
                hashes[i].CopyTo(array, i * 20);
            }
            return Hash.SHA1(array);
        }

        // Token: 0x06004658 RID: 18008 RVA: 0x001A5C80 File Offset: 0x001A3E80
        public static byte[] combine(List<byte[]> hashes)
        {
            byte[] array = new byte[hashes.Count * 20];
            for (int i = 0; i < hashes.Count; i++)
            {
                hashes[i].CopyTo(array, i * 20);
            }
            return Hash.SHA1(array);
        }

        // Token: 0x06004659 RID: 18009 RVA: 0x001A5CC4 File Offset: 0x001A3EC4
        public static string toString(byte[] hash)
        {
            if (hash == null)
            {
                return "null";
            }
            string text = "";
            for (int i = 0; i < hash.Length; i++)
            {
                text += hash[i].ToString("X2");
            }
            return text;
        }

        // Token: 0x0600465A RID: 18010 RVA: 0x001A5D08 File Offset: 0x001A3F08
        internal static string ToCodeString(byte[] hash)
        {
            StringBuilder stringBuilder = new StringBuilder(hash.Length * 6);
            stringBuilder.Append("0x");
            stringBuilder.Append(hash[0].ToString("X2"));
            for (int i = 1; i < hash.Length; i++)
            {
                stringBuilder.Append(", 0x");
                stringBuilder.Append(hash[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        // Token: 0x0600465B RID: 18011 RVA: 0x001A5D7C File Offset: 0x001A3F7C
        public static void log(byte[] hash)
        {
            if (hash == null || hash.Length != 20)
            {
                return;
            }
        }

        // Token: 0x04002DF7 RID: 11767
        private static SHA1 service = System.Security.Cryptography.SHA1.Create();

        // Token: 0x04002DF8 RID: 11768
        private static byte[] _40bytes = new byte[40];
    }
}
