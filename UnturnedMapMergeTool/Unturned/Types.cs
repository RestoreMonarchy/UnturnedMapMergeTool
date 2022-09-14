using System;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Unturned
{
    public class Types
    {
        // Token: 0x0400223B RID: 8763
        public static readonly Type STRING_TYPE = typeof(string);

        // Token: 0x0400223C RID: 8764
        public static readonly Type STRING_ARRAY_TYPE = typeof(string[]);

        // Token: 0x0400223D RID: 8765
        public static readonly Type BOOLEAN_TYPE = typeof(bool);

        // Token: 0x0400223E RID: 8766
        public static readonly Type BOOLEAN_ARRAY_TYPE = typeof(bool[]);

        // Token: 0x0400223F RID: 8767
        public static readonly Type BYTE_ARRAY_TYPE = typeof(byte[]);

        // Token: 0x04002240 RID: 8768
        public static readonly Type BYTE_TYPE = typeof(byte);

        // Token: 0x04002241 RID: 8769
        public static readonly Type INT16_TYPE = typeof(short);

        // Token: 0x04002242 RID: 8770
        public static readonly Type UINT16_TYPE = typeof(ushort);

        // Token: 0x04002243 RID: 8771
        public static readonly Type INT32_ARRAY_TYPE = typeof(int[]);

        // Token: 0x04002244 RID: 8772
        public static readonly Type INT32_TYPE = typeof(int);

        // Token: 0x04002245 RID: 8773
        public static readonly Type UINT32_TYPE = typeof(uint);

        // Token: 0x04002246 RID: 8774
        public static readonly Type SINGLE_TYPE = typeof(float);

        // Token: 0x04002247 RID: 8775
        public static readonly Type INT64_TYPE = typeof(long);

        // Token: 0x04002248 RID: 8776
        public static readonly Type UINT64_ARRAY_TYPE = typeof(ulong[]);

        // Token: 0x04002249 RID: 8777
        public static readonly Type UINT64_TYPE = typeof(ulong);

        // Token: 0x0400224A RID: 8778
        public static readonly Type STEAM_ID_TYPE = typeof(CSteamID);

        // Token: 0x0400224B RID: 8779
        public static readonly Type GUID_TYPE = typeof(Guid);

        // Token: 0x0400224C RID: 8780
        public static readonly Type VECTOR3_TYPE = typeof(Vector3);

        // Token: 0x0400224D RID: 8781
        public static readonly Type COLOR_TYPE = typeof(Color);

        // Token: 0x0400224E RID: 8782
        public static readonly Type QUATERNION_TYPE = typeof(Quaternion);

        // Token: 0x0400224F RID: 8783
        public static readonly byte[] SHIFTS = new byte[]
        {
            1,
            2,
            4,
            8,
            16,
            32,
            64,
            128
        };
    }
}
