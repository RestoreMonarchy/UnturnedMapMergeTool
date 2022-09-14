using System;
using System.Text;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Unturned
{
    public class Block
    {
        // Token: 0x06002280 RID: 8832 RVA: 0x0008EA44 File Offset: 0x0008CC44
        private static object[] getObjects(int index)
        {
            object[] array = Block.objects[index];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = null;
            }
            return array;
        }

        // Token: 0x06002281 RID: 8833 RVA: 0x0008EA6C File Offset: 0x0008CC6C
        public string readString()
        {
            if (this.block != null && this.step < this.block.Length)
            {
                byte b = this.block[this.step];
                this.step++;
                string result;
                if (this.step + (int)b <= this.block.Length)
                {
                    result = Encoding.UTF8.GetString(this.block, this.step, (int)b);
                }
                else
                {
                    result = string.Empty;
                }
                this.step += (int)b;
                return result;
            }
            return string.Empty;
        }

        // Token: 0x06002282 RID: 8834 RVA: 0x0008EAF4 File Offset: 0x0008CCF4
        public string[] readStringArray()
        {
            if (this.block != null && this.step < this.block.Length)
            {
                string[] array = new string[(int)this.readByte()];
                byte b = 0;
                while ((int)b < array.Length)
                {
                    array[(int)b] = this.readString();
                    b += 1;
                }
                return array;
            }
            return new string[0];
        }

        // Token: 0x06002283 RID: 8835 RVA: 0x0008EB45 File Offset: 0x0008CD45
        public bool readBoolean()
        {
            if (this.block != null && this.step <= this.block.Length - 1)
            {
                bool result = BitConverter.ToBoolean(this.block, this.step);
                this.step++;
                return result;
            }
            return false;
        }

        // Token: 0x06002284 RID: 8836 RVA: 0x0008EB84 File Offset: 0x0008CD84
        public bool[] readBooleanArray()
        {
            if (this.block != null && this.step < this.block.Length)
            {
                bool[] array = new bool[(int)this.readUInt16()];
                ushort num = (ushort)Mathf.CeilToInt((float)array.Length / 8f);
                for (ushort num2 = 0; num2 < num; num2 += 1)
                {
                    byte b = 0;
                    while (b < 8 && (int)(num2 * 8 + (ushort)b) < array.Length)
                    {
                        array[(int)(num2 * 8 + (ushort)b)] = ((this.block[this.step + (int)num2] & Types.SHIFTS[(int)b]) == Types.SHIFTS[(int)b]);
                        b += 1;
                    }
                }
                this.step += (int)num;
                return array;
            }
            return new bool[0];
        }

        // Token: 0x06002285 RID: 8837 RVA: 0x0008EC2A File Offset: 0x0008CE2A
        public byte readByte()
        {
            if (this.block != null && this.step <= this.block.Length - 1)
            {
                byte result = this.block[this.step];
                this.step++;
                return result;
            }
            return 0;
        }

        // Token: 0x06002286 RID: 8838 RVA: 0x0008EC64 File Offset: 0x0008CE64
        public byte[] readByteArray()
        {
            if (this.block != null && this.step < this.block.Length)
            {
                byte[] array;
                if (this.longBinaryData)
                {
                    int num = this.readInt32();
                    if (num >= 30000)
                    {
                        return new byte[0];
                    }
                    array = new byte[num];
                }
                else
                {
                    array = new byte[(int)this.block[this.step]];
                    this.step++;
                }
                if (this.step + array.Length <= this.block.Length)
                {
                    try
                    {
                        Buffer.BlockCopy(this.block, this.step, array, 0, array.Length);
                    }
                    catch
                    {
                    }
                }
                this.step += array.Length;
                return array;
            }
            return new byte[0];
        }

        // Token: 0x06002287 RID: 8839 RVA: 0x0008ED30 File Offset: 0x0008CF30
        public short readInt16()
        {
            if (this.block != null && this.step <= this.block.Length - 2)
            {
                this.readBitConverterBytes(2);
                short result = BitConverter.ToInt16(this.block, this.step);
                this.step += 2;
                return result;
            }
            return 0;
        }

        // Token: 0x06002288 RID: 8840 RVA: 0x0008ED80 File Offset: 0x0008CF80
        public ushort readUInt16()
        {
            if (this.block != null && this.step <= this.block.Length - 2)
            {
                this.readBitConverterBytes(2);
                ushort result = BitConverter.ToUInt16(this.block, this.step);
                this.step += 2;
                return result;
            }
            return 0;
        }

        // Token: 0x06002289 RID: 8841 RVA: 0x0008EDD0 File Offset: 0x0008CFD0
        public int readInt32()
        {
            if (this.block != null && this.step <= this.block.Length - 4)
            {
                this.readBitConverterBytes(4);
                int result = BitConverter.ToInt32(this.block, this.step);
                this.step += 4;
                return result;
            }
            return 0;
        }

        // Token: 0x0600228A RID: 8842 RVA: 0x0008EE20 File Offset: 0x0008D020
        public int[] readInt32Array()
        {
            ushort num = this.readUInt16();
            int[] array = new int[(int)num];
            for (ushort num2 = 0; num2 < num; num2 += 1)
            {
                int num3 = this.readInt32();
                array[(int)num2] = num3;
            }
            return array;
        }

        // Token: 0x0600228B RID: 8843 RVA: 0x0008EE54 File Offset: 0x0008D054
        public uint readUInt32()
        {
            if (this.block != null && this.step <= this.block.Length - 4)
            {
                this.readBitConverterBytes(4);
                uint result = BitConverter.ToUInt32(this.block, this.step);
                this.step += 4;
                return result;
            }
            return 0U;
        }

        // Token: 0x0600228C RID: 8844 RVA: 0x0008EEA4 File Offset: 0x0008D0A4
        public float readSingle()
        {
            if (this.block != null && this.step <= this.block.Length - 4)
            {
                this.readBitConverterBytes(4);
                float result = BitConverter.ToSingle(this.block, this.step);
                this.step += 4;
                return result;
            }
            return 0f;
        }

        // Token: 0x0600228D RID: 8845 RVA: 0x0008EEF8 File Offset: 0x0008D0F8
        public long readInt64()
        {
            if (this.block != null && this.step <= this.block.Length - 8)
            {
                this.readBitConverterBytes(8);
                long result = BitConverter.ToInt64(this.block, this.step);
                this.step += 8;
                return result;
            }
            return 0L;
        }

        // Token: 0x0600228E RID: 8846 RVA: 0x0008EF48 File Offset: 0x0008D148
        public ulong readUInt64()
        {
            if (this.block != null && this.step <= this.block.Length - 8)
            {
                this.readBitConverterBytes(8);
                ulong result = BitConverter.ToUInt64(this.block, this.step);
                this.step += 8;
                return result;
            }
            return 0UL;
        }

        // Token: 0x0600228F RID: 8847 RVA: 0x0008EF98 File Offset: 0x0008D198
        public ulong[] readUInt64Array()
        {
            ushort num = this.readUInt16();
            ulong[] array = new ulong[(int)num];
            for (ushort num2 = 0; num2 < num; num2 += 1)
            {
                ulong num3 = this.readUInt64();
                array[(int)num2] = num3;
            }
            return array;
        }

        // Token: 0x06002290 RID: 8848 RVA: 0x0008EFCC File Offset: 0x0008D1CC
        public CSteamID readSteamID()
        {
            return new CSteamID(this.readUInt64());
        }

        // Token: 0x06002291 RID: 8849 RVA: 0x0008EFDC File Offset: 0x0008D1DC
        public Guid readGUID()
        {
            return GuidBuffer.FromBytes(this.readByteArray());
            //GuidBuffer guidBuffer = default(GuidBuffer);
            //guidBuffer.Read(this.readByteArray(), 0);
            //return guidBuffer.GUID;
        }

        // Token: 0x06002292 RID: 8850 RVA: 0x0008F008 File Offset: 0x0008D208
        public Vector3 readUInt16RVector3()
        {
            double num = (double)this.readByte();
            double num2 = (double)this.readUInt16() / 65535.0;
            double num3 = (double)this.readUInt16() / 65535.0;
            byte b = this.readByte();
            double num4 = (double)this.readUInt16() / 65535.0;
            num2 = num * (double)Regions.REGION_SIZE + num2 * (double)Regions.REGION_SIZE - 4096.0;
            num3 = num3 * 2048.0 - 1024.0;
            num4 = (double)(b * Regions.REGION_SIZE) + num4 * (double)Regions.REGION_SIZE - 4096.0;
            return new Vector3((float)num2, (float)num3, (float)num4);
        }

        // Token: 0x06002293 RID: 8851 RVA: 0x0008F0B0 File Offset: 0x0008D2B0
        public Vector3 readSingleVector3()
        {
            return new Vector3(this.readSingle(), this.readSingle(), this.readSingle());
        }

        // Token: 0x06002294 RID: 8852 RVA: 0x0008F0C9 File Offset: 0x0008D2C9
        public EulerAngles readSingleQuaternion()
        {
            return new(this.readSingle(), this.readSingle(), this.readSingle());
        }

        // Token: 0x06002295 RID: 8853 RVA: 0x0008F0E2 File Offset: 0x0008D2E2
        public Color readColor()
        {
            return new Color((float)this.readByte() / 255f, (float)this.readByte() / 255f, (float)this.readByte() / 255f);
        }

        // Token: 0x06002296 RID: 8854 RVA: 0x0008F110 File Offset: 0x0008D310
        public object read(Type type)
        {
            if (type == Types.STRING_TYPE)
            {
                return this.readString();
            }
            if (type == Types.STRING_ARRAY_TYPE)
            {
                return this.readStringArray();
            }
            if (type == Types.BOOLEAN_TYPE)
            {
                return this.readBoolean();
            }
            if (type == Types.BOOLEAN_ARRAY_TYPE)
            {
                return this.readBooleanArray();
            }
            if (type == Types.BYTE_TYPE)
            {
                return this.readByte();
            }
            if (type == Types.BYTE_ARRAY_TYPE)
            {
                return this.readByteArray();
            }
            if (type == Types.INT16_TYPE)
            {
                return this.readInt16();
            }
            if (type == Types.UINT16_TYPE)
            {
                return this.readUInt16();
            }
            if (type == Types.INT32_TYPE)
            {
                return this.readInt32();
            }
            if (type == Types.INT32_ARRAY_TYPE)
            {
                return this.readInt32Array();
            }
            if (type == Types.UINT32_TYPE)
            {
                return this.readUInt32();
            }
            if (type == Types.SINGLE_TYPE)
            {
                return this.readSingle();
            }
            if (type == Types.INT64_TYPE)
            {
                return this.readInt64();
            }
            if (type == Types.UINT64_TYPE)
            {
                return this.readUInt64();
            }
            if (type == Types.UINT64_ARRAY_TYPE)
            {
                return this.readUInt64Array();
            }
            if (type == Types.STEAM_ID_TYPE)
            {
                return this.readSteamID();
            }
            if (type == Types.GUID_TYPE)
            {
                return this.readGUID();
            }
            if (type == Types.VECTOR3_TYPE)
            {
                return this.readSingleVector3();
            }
            if (type == Types.COLOR_TYPE)
            {
                return this.readColor();
            }
            throw new NotSupportedException(string.Format("Cannot read type {0}", type));
        }

        // Token: 0x06002297 RID: 8855 RVA: 0x0008F2EC File Offset: 0x0008D4EC
        public object[] read(int offset, Type type_0)
        {
            object[] array = Block.getObjects(0);
            if (offset < 1)
            {
                array[0] = this.read(type_0);
            }
            return array;
        }

        // Token: 0x06002298 RID: 8856 RVA: 0x0008F310 File Offset: 0x0008D510
        public object[] read(int offset, Type type_0, Type type_1)
        {
            object[] array = Block.getObjects(1);
            if (offset < 1)
            {
                array[0] = this.read(type_0);
            }
            if (offset < 2)
            {
                array[1] = this.read(type_1);
            }
            return array;
        }

        // Token: 0x06002299 RID: 8857 RVA: 0x0008F341 File Offset: 0x0008D541
        public object[] read(Type type_0, Type type_1)
        {
            return this.read(0, type_0, type_1);
        }

        // Token: 0x0600229A RID: 8858 RVA: 0x0008F34C File Offset: 0x0008D54C
        public object[] read(int offset, Type type_0, Type type_1, Type type_2)
        {
            object[] array = Block.getObjects(2);
            if (offset < 1)
            {
                array[0] = this.read(type_0);
            }
            if (offset < 2)
            {
                array[1] = this.read(type_1);
            }
            if (offset < 3)
            {
                array[2] = this.read(type_2);
            }
            return array;
        }

        // Token: 0x0600229B RID: 8859 RVA: 0x0008F38C File Offset: 0x0008D58C
        public object[] read(Type type_0, Type type_1, Type type_2)
        {
            return this.read(0, type_0, type_1, type_2);
        }

        // Token: 0x0600229C RID: 8860 RVA: 0x0008F398 File Offset: 0x0008D598
        public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3)
        {
            object[] array = Block.getObjects(3);
            if (offset < 1)
            {
                array[0] = this.read(type_0);
            }
            if (offset < 2)
            {
                array[1] = this.read(type_1);
            }
            if (offset < 3)
            {
                array[2] = this.read(type_2);
            }
            if (offset < 4)
            {
                array[3] = this.read(type_3);
            }
            return array;
        }

        // Token: 0x0600229D RID: 8861 RVA: 0x0008F3E7 File Offset: 0x0008D5E7
        public object[] read(Type type_0, Type type_1, Type type_2, Type type_3)
        {
            return this.read(0, type_0, type_1, type_2, type_3);
        }

        // Token: 0x0600229E RID: 8862 RVA: 0x0008F3F8 File Offset: 0x0008D5F8
        public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
        {
            object[] array = Block.getObjects(4);
            if (offset < 1)
            {
                array[0] = this.read(type_0);
            }
            if (offset < 2)
            {
                array[1] = this.read(type_1);
            }
            if (offset < 3)
            {
                array[2] = this.read(type_2);
            }
            if (offset < 4)
            {
                array[3] = this.read(type_3);
            }
            if (offset < 5)
            {
                array[4] = this.read(type_4);
            }
            return array;
        }

        // Token: 0x0600229F RID: 8863 RVA: 0x0008F456 File Offset: 0x0008D656
        public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
        {
            return this.read(0, type_0, type_1, type_2, type_3, type_4);
        }

        // Token: 0x060022A0 RID: 8864 RVA: 0x0008F468 File Offset: 0x0008D668
        public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
        {
            object[] array = Block.getObjects(5);
            if (offset < 1)
            {
                array[0] = this.read(type_0);
            }
            if (offset < 2)
            {
                array[1] = this.read(type_1);
            }
            if (offset < 3)
            {
                array[2] = this.read(type_2);
            }
            if (offset < 4)
            {
                array[3] = this.read(type_3);
            }
            if (offset < 5)
            {
                array[4] = this.read(type_4);
            }
            if (offset < 6)
            {
                array[5] = this.read(type_5);
            }
            return array;
        }

        // Token: 0x060022A1 RID: 8865 RVA: 0x0008F4D5 File Offset: 0x0008D6D5
        public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
        {
            return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5);
        }

        // Token: 0x060022A2 RID: 8866 RVA: 0x0008F4E8 File Offset: 0x0008D6E8
        public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
        {
            object[] array = Block.getObjects(6);
            if (offset < 1)
            {
                array[0] = this.read(type_0);
            }
            if (offset < 2)
            {
                array[1] = this.read(type_1);
            }
            if (offset < 3)
            {
                array[2] = this.read(type_2);
            }
            if (offset < 4)
            {
                array[3] = this.read(type_3);
            }
            if (offset < 5)
            {
                array[4] = this.read(type_4);
            }
            if (offset < 6)
            {
                array[5] = this.read(type_5);
            }
            if (offset < 7)
            {
                array[6] = this.read(type_6);
            }
            return array;
        }

        // Token: 0x060022A3 RID: 8867 RVA: 0x0008F564 File Offset: 0x0008D764
        public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
        {
            return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5, type_6);
        }

        // Token: 0x060022A4 RID: 8868 RVA: 0x0008F584 File Offset: 0x0008D784
        public object[] read(int offset, params Type[] types)
        {
            object[] array = new object[types.Length];
            for (int i = offset; i < types.Length; i++)
            {
                array[i] = this.read(types[i]);
            }
            return array;
        }

        // Token: 0x060022A5 RID: 8869 RVA: 0x0008F5B5 File Offset: 0x0008D7B5
        public object[] read(params Type[] types)
        {
            return this.read(0, types);
        }

        // Token: 0x060022A6 RID: 8870 RVA: 0x0008F5BF File Offset: 0x0008D7BF
        protected void readBitConverterBytes(int length)
        {
        }

        // Token: 0x060022A7 RID: 8871 RVA: 0x0008F5C4 File Offset: 0x0008D7C4
        protected void writeBitConverterBytes(byte[] bytes)
        {
            int dstOffset = this.step;
            int count = bytes.Length;
            Buffer.BlockCopy(bytes, 0, Block.buffer, dstOffset, count);
        }

        // Token: 0x060022A8 RID: 8872 RVA: 0x0008F5EC File Offset: 0x0008D7EC
        public void writeString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            byte b = (byte)bytes.Length;
            Block.buffer[this.step] = b;
            this.step++;
            Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, (int)b);
            this.step += (int)b;
        }

        // Token: 0x060022A9 RID: 8873 RVA: 0x0008F644 File Offset: 0x0008D844
        public void writeStringArray(string[] values)
        {
            byte b = (byte)values.Length;
            this.writeByte(b);
            for (byte b2 = 0; b2 < b; b2 += 1)
            {
                this.writeString(values[(int)b2]);
            }
        }

        // Token: 0x060022AA RID: 8874 RVA: 0x0008F674 File Offset: 0x0008D874
        public void writeBoolean(bool value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Block.buffer[this.step] = bytes[0];
            this.step++;
        }

        // Token: 0x060022AB RID: 8875 RVA: 0x0008F6A8 File Offset: 0x0008D8A8
        public void writeBooleanArray(bool[] values)
        {
            this.writeUInt16((ushort)values.Length);
            ushort num = (ushort)Mathf.CeilToInt((float)values.Length / 8f);
            for (ushort num2 = 0; num2 < num; num2 += 1)
            {
                Block.buffer[this.step + (int)num2] = 0;
                byte b = 0;
                while (b < 8 && (int)(num2 * 8 + (ushort)b) < values.Length)
                {
                    if (values[(int)(num2 * 8 + (ushort)b)])
                    {
                        byte[] array = Block.buffer;
                        int num3 = this.step + (int)num2;
                        array[num3] |= Types.SHIFTS[(int)b];
                    }
                    b += 1;
                }
            }
            this.step += (int)num;
        }

        // Token: 0x060022AC RID: 8876 RVA: 0x0008F738 File Offset: 0x0008D938
        public void writeByte(byte value)
        {
            Block.buffer[this.step] = value;
            this.step++;
        }

        // Token: 0x060022AD RID: 8877 RVA: 0x0008F758 File Offset: 0x0008D958
        public void writeByteArray(byte[] values)
        {
            if (values.Length >= 30000)
            {
                return;
            }
            if (this.longBinaryData)
            {
                this.writeInt32(values.Length);
                Buffer.BlockCopy(values, 0, Block.buffer, this.step, values.Length);
                this.step += values.Length;
                return;
            }
            byte b = (byte)values.Length;
            Block.buffer[this.step] = b;
            this.step++;
            Buffer.BlockCopy(values, 0, Block.buffer, this.step, (int)b);
            this.step += (int)b;
        }

        // Token: 0x060022AE RID: 8878 RVA: 0x0008F7E8 File Offset: 0x0008D9E8
        public void writeInt16(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.writeBitConverterBytes(bytes);
            this.step += 2;
        }

        // Token: 0x060022AF RID: 8879 RVA: 0x0008F814 File Offset: 0x0008DA14
        public void writeUInt16(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.writeBitConverterBytes(bytes);
            this.step += 2;
        }

        // Token: 0x060022B0 RID: 8880 RVA: 0x0008F840 File Offset: 0x0008DA40
        public void writeInt32(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.writeBitConverterBytes(bytes);
            this.step += 4;
        }

        // Token: 0x060022B1 RID: 8881 RVA: 0x0008F86C File Offset: 0x0008DA6C
        public void writeInt32Array(int[] values)
        {
            this.writeUInt16((ushort)values.Length);
            ushort num = 0;
            while ((int)num < values.Length)
            {
                this.writeInt32(values[(int)num]);
                num += 1;
            }
        }

        // Token: 0x060022B2 RID: 8882 RVA: 0x0008F89C File Offset: 0x0008DA9C
        public void writeUInt32(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.writeBitConverterBytes(bytes);
            this.step += 4;
        }

        // Token: 0x060022B3 RID: 8883 RVA: 0x0008F8C8 File Offset: 0x0008DAC8
        public void writeSingle(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.writeBitConverterBytes(bytes);
            this.step += 4;
        }

        // Token: 0x060022B4 RID: 8884 RVA: 0x0008F8F4 File Offset: 0x0008DAF4
        public void writeInt64(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.writeBitConverterBytes(bytes);
            this.step += 8;
        }

        // Token: 0x060022B5 RID: 8885 RVA: 0x0008F920 File Offset: 0x0008DB20
        public void writeUInt64(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.writeBitConverterBytes(bytes);
            this.step += 8;
        }

        // Token: 0x060022B6 RID: 8886 RVA: 0x0008F94C File Offset: 0x0008DB4C
        public void writeUInt64Array(ulong[] values)
        {
            this.writeUInt16((ushort)values.Length);
            ushort num = 0;
            while ((int)num < values.Length)
            {
                this.writeUInt64(values[(int)num]);
                num += 1;
            }
        }

        // Token: 0x060022B7 RID: 8887 RVA: 0x0008F97B File Offset: 0x0008DB7B
        public void writeSteamID(CSteamID steamID)
        {
            this.writeUInt64(steamID.m_SteamID);
        }

        // Token: 0x060022B8 RID: 8888 RVA: 0x0008F98C File Offset: 0x0008DB8C
        public void writeGUID(Guid GUID)
        {
            this.writeByteArray(GuidBuffer.ToBytes(GUID));
            //GuidBuffer guidBuffer = new GuidBuffer(GUID);
            //guidBuffer.Write(GuidBuffer.GUID_BUFFER, 0);
            //this.writeByteArray(GuidBuffer.GUID_BUFFER);
        }

        // Token: 0x060022B9 RID: 8889 RVA: 0x0008F9BC File Offset: 0x0008DBBC
        public void writeUInt16RVector3(Vector3 value)
        {
            double num = (double)value.x + 4096.0;
            double num2 = (double)value.y + 1024.0;
            double num3 = (double)value.z + 4096.0;
            byte value2 = (byte)(num / (double)Regions.REGION_SIZE);
            byte value3 = (byte)(num3 / (double)Regions.REGION_SIZE);
            num %= (double)Regions.REGION_SIZE;
            num2 %= 2048.0;
            num3 %= (double)Regions.REGION_SIZE;
            num /= (double)Regions.REGION_SIZE;
            num2 /= 2048.0;
            num3 /= (double)Regions.REGION_SIZE;
            this.writeByte(value2);
            this.writeUInt16((ushort)(num * 65535.0));
            this.writeUInt16((ushort)(num2 * 65535.0));
            this.writeByte(value3);
            this.writeUInt16((ushort)(num3 * 65535.0));
        }

        // Token: 0x060022BA RID: 8890 RVA: 0x0008FA95 File Offset: 0x0008DC95
        public void writeSingleVector3(Vector3 value)
        {
            this.writeSingle(value.x);
            this.writeSingle(value.y);
            this.writeSingle(value.z);
        }

        // Token: 0x060022BB RID: 8891 RVA: 0x0008FABC File Offset: 0x0008DCBC
        public void writeSingleQuaternion(EulerAngles value)
        {
            this.writeSingle(value.x);
            this.writeSingle(value.y);
            this.writeSingle(value.z);
        }

        // Token: 0x060022BC RID: 8892 RVA: 0x0008FAF5 File Offset: 0x0008DCF5
        public void writeColor(Color value)
        {
            this.writeByte((byte)(value.r * 255f));
            this.writeByte((byte)(value.g * 255f));
            this.writeByte((byte)(value.b * 255f));
        }

        // Token: 0x060022BD RID: 8893 RVA: 0x0008FB30 File Offset: 0x0008DD30
        public void write(object objects)
        {
            Type type = objects.GetType();
            if (type == Types.STRING_TYPE)
            {
                this.writeString((string)objects);
                return;
            }
            if (type == Types.STRING_ARRAY_TYPE)
            {
                this.writeStringArray((string[])objects);
                return;
            }
            if (type == Types.BOOLEAN_TYPE)
            {
                this.writeBoolean((bool)objects);
                return;
            }
            if (type == Types.BOOLEAN_ARRAY_TYPE)
            {
                this.writeBooleanArray((bool[])objects);
                return;
            }
            if (type == Types.BYTE_TYPE)
            {
                this.writeByte((byte)objects);
                return;
            }
            if (type == Types.BYTE_ARRAY_TYPE)
            {
                this.writeByteArray((byte[])objects);
                return;
            }
            if (type == Types.INT16_TYPE)
            {
                this.writeInt16((short)objects);
                return;
            }
            if (type == Types.UINT16_TYPE)
            {
                this.writeUInt16((ushort)objects);
                return;
            }
            if (type == Types.INT32_TYPE)
            {
                this.writeInt32((int)objects);
                return;
            }
            if (type == Types.INT32_ARRAY_TYPE)
            {
                this.writeInt32Array((int[])objects);
                return;
            }
            if (type == Types.UINT32_TYPE)
            {
                this.writeUInt32((uint)objects);
                return;
            }
            if (type == Types.SINGLE_TYPE)
            {
                this.writeSingle((float)objects);
                return;
            }
            if (type == Types.INT64_TYPE)
            {
                this.writeInt64((long)objects);
                return;
            }
            if (type == Types.UINT64_TYPE)
            {
                this.writeUInt64((ulong)objects);
                return;
            }
            if (type == Types.UINT64_ARRAY_TYPE)
            {
                this.writeUInt64Array((ulong[])objects);
                return;
            }
            if (type == Types.STEAM_ID_TYPE)
            {
                this.writeSteamID((CSteamID)objects);
                return;
            }
            if (type == Types.GUID_TYPE)
            {
                this.writeGUID((Guid)objects);
                return;
            }
            if (type == Types.VECTOR3_TYPE)
            {
                this.writeSingleVector3((Vector3)objects);
                return;
            }
            if (type == Types.COLOR_TYPE)
            {
                this.writeColor((Color)objects);
                return;
            }
            throw new NotSupportedException(string.Format("Cannot write {0} of type {1}", objects, type));
        }

        // Token: 0x060022BE RID: 8894 RVA: 0x0008FD43 File Offset: 0x0008DF43
        public void write(object object_0, object object_1)
        {
            this.write(object_0);
            this.write(object_1);
        }

        // Token: 0x060022BF RID: 8895 RVA: 0x0008FD53 File Offset: 0x0008DF53
        public void write(object object_0, object object_1, object object_2)
        {
            this.write(object_0, object_1);
            this.write(object_2);
        }

        // Token: 0x060022C0 RID: 8896 RVA: 0x0008FD64 File Offset: 0x0008DF64
        public void write(object object_0, object object_1, object object_2, object object_3)
        {
            this.write(object_0, object_1, object_2);
            this.write(object_3);
        }

        // Token: 0x060022C1 RID: 8897 RVA: 0x0008FD77 File Offset: 0x0008DF77
        public void write(object object_0, object object_1, object object_2, object object_3, object object_4)
        {
            this.write(object_0, object_1, object_2, object_3);
            this.write(object_4);
        }

        // Token: 0x060022C2 RID: 8898 RVA: 0x0008FD8C File Offset: 0x0008DF8C
        public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5)
        {
            this.write(object_0, object_1, object_2, object_3, object_4);
            this.write(object_5);
        }

        // Token: 0x060022C3 RID: 8899 RVA: 0x0008FDA3 File Offset: 0x0008DFA3
        public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5, object object_6)
        {
            this.write(object_0, object_1, object_2, object_3, object_4, object_5);
            this.write(object_6);
        }

        // Token: 0x060022C4 RID: 8900 RVA: 0x0008FDBC File Offset: 0x0008DFBC
        public void write(params object[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                this.write(objects[i]);
            }
        }

        // Token: 0x060022C5 RID: 8901 RVA: 0x0008FDE0 File Offset: 0x0008DFE0
        public byte[] getBytes(out int size)
        {
            if (this.block == null)
            {
                size = this.step;
                return Block.buffer;
            }
            size = this.block.Length;
            return this.block;
        }

        // Token: 0x060022C6 RID: 8902 RVA: 0x0008FE08 File Offset: 0x0008E008
        public byte[] getHash()
        {
            if (this.block == null)
            {
                return Hash.SHA1(Block.buffer);
            }
            return Hash.SHA1(this.block);
        }

        // Token: 0x060022C7 RID: 8903 RVA: 0x0008FE28 File Offset: 0x0008E028
        public void reset(int prefix, byte[] contents)
        {
            this.step = prefix;
            this.block = contents;
        }

        // Token: 0x060022C8 RID: 8904 RVA: 0x0008FE38 File Offset: 0x0008E038
        public void reset(byte[] contents)
        {
            this.step = 0;
            this.block = contents;
        }

        // Token: 0x060022C9 RID: 8905 RVA: 0x0008FE48 File Offset: 0x0008E048
        public void reset(int prefix)
        {
            this.step = prefix;
            this.block = null;
        }

        // Token: 0x060022CA RID: 8906 RVA: 0x0008FE58 File Offset: 0x0008E058
        public void reset()
        {
            this.step = 0;
            this.block = null;
        }

        // Token: 0x060022CB RID: 8907 RVA: 0x0008FE68 File Offset: 0x0008E068
        public Block(int prefix, byte[] contents)
        {
            this.reset(prefix, contents);
        }

        // Token: 0x060022CC RID: 8908 RVA: 0x0008FE78 File Offset: 0x0008E078
        public Block(byte[] contents)
        {
            this.reset(contents);
        }

        // Token: 0x060022CD RID: 8909 RVA: 0x0008FE87 File Offset: 0x0008E087
        public Block(int prefix)
        {
            this.reset(prefix);
        }

        // Token: 0x060022CE RID: 8910 RVA: 0x0008FE96 File Offset: 0x0008E096
        public Block()
        {
            this.reset();
        }

        // Token: 0x0400109A RID: 4250
        public static readonly int BUFFER_SIZE = 65535;

        // Token: 0x0400109B RID: 4251
        public static byte[] buffer = new byte[Block.BUFFER_SIZE];

        // Token: 0x0400109C RID: 4252
        private static object[][] objects = new object[][]
        {
            new object[1],
            new object[2],
            new object[3],
            new object[4],
            new object[5],
            new object[6],
            new object[7]
        };

        // Token: 0x0400109D RID: 4253
        public bool longBinaryData;

        // Token: 0x0400109E RID: 4254
        public int step;

        // Token: 0x0400109F RID: 4255
        public byte[] block;
    }
}
