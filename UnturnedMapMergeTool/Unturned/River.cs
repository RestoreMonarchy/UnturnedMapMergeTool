using System;
using System.IO;
using System.Text;
using UnturnedMapMergeTool.Unturned.Unity;

namespace UnturnedMapMergeTool.Unturned
{
    public class River
    {
        // Token: 0x0600238D RID: 9101 RVA: 0x0009271C File Offset: 0x0009091C
        public string readString()
        {
            if (this.block != null)
            {
                return this.block.readString();
            }
            int num = (int)this.readByte();
            if (num > 0)
            {
                this.stream.Read(River.buffer, 0, num);
                return Encoding.UTF8.GetString(River.buffer, 0, num);
            }
            return string.Empty;
        }

        // Token: 0x0600238E RID: 9102 RVA: 0x00092772 File Offset: 0x00090972
        public bool readBoolean()
        {
            if (this.block != null)
            {
                return this.block.readBoolean();
            }
            return this.readByte() > 0;
        }

        // Token: 0x0600238F RID: 9103 RVA: 0x00092791 File Offset: 0x00090991
        public byte readByte()
        {
            if (this.block != null)
            {
                return this.block.readByte();
            }
            return MathfEx.ClampToByte(this.stream.ReadByte());
        }

        // Token: 0x06002390 RID: 9104 RVA: 0x000927B8 File Offset: 0x000909B8
        public byte[] readBytes()
        {
            if (this.block != null)
            {
                return this.block.readByteArray();
            }
            byte[] array = new byte[(int)this.readUInt16()];
            this.stream.Read(array, 0, array.Length);
            return array;
        }

        // Token: 0x06002391 RID: 9105 RVA: 0x000927F7 File Offset: 0x000909F7
        public short readInt16()
        {
            if (this.block != null)
            {
                return this.block.readInt16();
            }
            this.stream.Read(River.buffer, 0, 2);
            return BitConverter.ToInt16(River.buffer, 0);
        }

        // Token: 0x06002392 RID: 9106 RVA: 0x0009282B File Offset: 0x00090A2B
        public ushort readUInt16()
        {
            if (this.block != null)
            {
                return this.block.readUInt16();
            }
            this.stream.Read(River.buffer, 0, 2);
            return BitConverter.ToUInt16(River.buffer, 0);
        }

        // Token: 0x06002393 RID: 9107 RVA: 0x0009285F File Offset: 0x00090A5F
        public int readInt32()
        {
            if (this.block != null)
            {
                return this.block.readInt32();
            }
            this.stream.Read(River.buffer, 0, 4);
            return BitConverter.ToInt32(River.buffer, 0);
        }

        // Token: 0x06002394 RID: 9108 RVA: 0x00092893 File Offset: 0x00090A93
        public uint readUInt32()
        {
            if (this.block != null)
            {
                return this.block.readUInt32();
            }
            this.stream.Read(River.buffer, 0, 4);
            return BitConverter.ToUInt32(River.buffer, 0);
        }

        // Token: 0x06002395 RID: 9109 RVA: 0x000928C7 File Offset: 0x00090AC7
        public float readSingle()
        {
            if (this.block != null)
            {
                return this.block.readSingle();
            }
            this.stream.Read(River.buffer, 0, 4);
            return BitConverter.ToSingle(River.buffer, 0);
        }

        // Token: 0x06002396 RID: 9110 RVA: 0x000928FB File Offset: 0x00090AFB
        public long readInt64()
        {
            if (this.block != null)
            {
                return this.block.readInt64();
            }
            this.stream.Read(River.buffer, 0, 8);
            return BitConverter.ToInt64(River.buffer, 0);
        }

        // Token: 0x06002397 RID: 9111 RVA: 0x0009292F File Offset: 0x00090B2F
        public ulong readUInt64()
        {
            if (this.block != null)
            {
                return this.block.readUInt64();
            }
            this.stream.Read(River.buffer, 0, 8);
            return BitConverter.ToUInt64(River.buffer, 0);
        }

        // Token: 0x06002398 RID: 9112 RVA: 0x00092963 File Offset: 0x00090B63
        public CSteamID readSteamID()
        {
            return new CSteamID(this.readUInt64());
        }

        // Token: 0x06002399 RID: 9113 RVA: 0x00092970 File Offset: 0x00090B70
        public Guid readGUID()
        {
            if (this.block != null)
            {
                return this.block.readGUID();
            }

            byte[] bytes = this.readBytes();
            return new Guid(bytes);

            //GuidBuffer guidBuffer = default(GuidBuffer);
            //guidBuffer.Read(this.readBytes(), 0);
            //return guidBuffer.GUID;
        }

        // Token: 0x0600239A RID: 9114 RVA: 0x000929AD File Offset: 0x00090BAD
        public Vector3 readSingleVector3()
        {
            return new Vector3(this.readSingle(), this.readSingle(), this.readSingle());
        }

        // Token: 0x0600239B RID: 9115 RVA: 0x000929C6 File Offset: 0x00090BC6
        public Quaternion readSingleQuaternion()
        {
            return Quaternion.Euler(this.readSingle(), this.readSingle(), this.readSingle());
        }

        // Token: 0x0600239C RID: 9116 RVA: 0x000929DF File Offset: 0x00090BDF
        public Color readColor()
        {
            return new Color((float)this.readByte() / 255f, (float)this.readByte() / 255f, (float)this.readByte() / 255f);
        }

        // Token: 0x0600239D RID: 9117 RVA: 0x00092A10 File Offset: 0x00090C10
        public void writeString(string value)
        {
            if (this.block != null)
            {
                this.block.writeString(value);
                return;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            byte b = MathfEx.ClampToByte(bytes.Length);
            this.stream.WriteByte(b);
            this.stream.Write(bytes, 0, (int)b);
            this.water += (int)(1 + b);
        }

        // Token: 0x0600239E RID: 9118 RVA: 0x00092A71 File Offset: 0x00090C71
        public void writeBoolean(bool value)
        {
            if (this.block != null)
            {
                this.block.writeBoolean(value);
                return;
            }
            this.stream.WriteByte(value ? (byte)1 : (byte)0);
            this.water++;
        }

        // Token: 0x0600239F RID: 9119 RVA: 0x00092AA8 File Offset: 0x00090CA8
        public void writeByte(byte value)
        {
            if (this.block != null)
            {
                this.block.writeByte(value);
                return;
            }
            this.stream.WriteByte(value);
            this.water++;
        }

        // Token: 0x060023A0 RID: 9120 RVA: 0x00092ADC File Offset: 0x00090CDC
        public void writeBytes(byte[] values)
        {
            if (this.block != null)
            {
                this.block.writeByteArray(values);
                return;
            }
            ushort num = MathfEx.ClampToUShort(values.Length);
            this.writeUInt16(num);
            this.stream.Write(values, 0, (int)num);
            this.water += (int)num;
        }

        // Token: 0x060023A1 RID: 9121 RVA: 0x00092B2C File Offset: 0x00090D2C
        public void writeInt16(short value)
        {
            if (this.block != null)
            {
                this.block.writeInt16(value);
                return;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            this.stream.Write(bytes, 0, 2);
            this.water += 2;
        }

        // Token: 0x060023A2 RID: 9122 RVA: 0x00092B74 File Offset: 0x00090D74
        public void writeUInt16(ushort value)
        {
            if (this.block != null)
            {
                this.block.writeUInt16(value);
                return;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            this.stream.Write(bytes, 0, 2);
            this.water += 2;
        }

        // Token: 0x060023A3 RID: 9123 RVA: 0x00092BBC File Offset: 0x00090DBC
        public void writeInt32(int value)
        {
            if (this.block != null)
            {
                this.block.writeInt32(value);
                return;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            this.stream.Write(bytes, 0, 4);
            this.water += 4;
        }

        // Token: 0x060023A4 RID: 9124 RVA: 0x00092C04 File Offset: 0x00090E04
        public void writeUInt32(uint value)
        {
            if (this.block != null)
            {
                this.block.writeUInt32(value);
                return;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            this.stream.Write(bytes, 0, 4);
            this.water += 4;
        }

        // Token: 0x060023A5 RID: 9125 RVA: 0x00092C4C File Offset: 0x00090E4C
        public void writeSingle(float value)
        {
            if (this.block != null)
            {
                this.block.writeSingle(value);
                return;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            this.stream.Write(bytes, 0, 4);
            this.water += 4;
        }

        // Token: 0x060023A6 RID: 9126 RVA: 0x00092C94 File Offset: 0x00090E94
        public void writeInt64(long value)
        {
            if (this.block != null)
            {
                this.block.writeInt64(value);
                return;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            this.stream.Write(bytes, 0, 8);
            this.water += 8;
        }

        // Token: 0x060023A7 RID: 9127 RVA: 0x00092CDC File Offset: 0x00090EDC
        public void writeUInt64(ulong value)
        {
            if (this.block != null)
            {
                this.block.writeUInt64(value);
                return;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            this.stream.Write(bytes, 0, 8);
            this.water += 8;
        }

        // Token: 0x060023A8 RID: 9128 RVA: 0x00092D21 File Offset: 0x00090F21
        public void writeSteamID(CSteamID steamID)
        {
            this.writeUInt64(steamID.m_SteamID);
        }

        // Token: 0x060023A9 RID: 9129 RVA: 0x00092D30 File Offset: 0x00090F30
        public void writeGUID(Guid guid)
        {
            //GuidBuffer guidBuffer = new GuidBuffer(guid);
            //guidBuffer.Write(GuidBuffer.GUID_BUFFER, 0);
            this.writeBytes(guid.ToByteArray());
        }

        // Token: 0x060023AA RID: 9130 RVA: 0x00092D5D File Offset: 0x00090F5D
        public void writeSingleVector3(Vector3 value)
        {
            this.writeSingle(value.x);
            this.writeSingle(value.y);
            this.writeSingle(value.z);
        }

        // Token: 0x060023AB RID: 9131 RVA: 0x00092D84 File Offset: 0x00090F84
        public void writeSingleQuaternion(Quaternion value)
        {
            Vector3 eulerAngles = value.eulerAngles;
            this.writeSingle(eulerAngles.x);
            this.writeSingle(eulerAngles.y);
            this.writeSingle(eulerAngles.z);
        }

        // Token: 0x060023AC RID: 9132 RVA: 0x00092DBD File Offset: 0x00090FBD
        public void writeColor(Color value)
        {
            this.writeByte((byte)(value.r * 255f));
            this.writeByte((byte)(value.g * 255f));
            this.writeByte((byte)(value.b * 255f));
        }

        // Token: 0x060023AD RID: 9133 RVA: 0x00092DF8 File Offset: 0x00090FF8
        public byte[] getHash()
        {
            this.stream.Position = 0L;
            return Hash.SHA1(this.stream);
        }

        // Token: 0x060023AE RID: 9134 RVA: 0x00092E14 File Offset: 0x00091014
        public void closeRiver()
        {
            if (this.block != null)
            {
                byte[] bytes = block.getBytes(out int size);
                File.WriteAllBytes(path, bytes);
                return;
            }
            if (this.water > 0)
            {
                this.stream.SetLength((long)this.water);
            }
            this.stream.Flush();
            this.stream.Close();
            this.stream.Dispose();
        }

        // Token: 0x060023AF RID: 9135 RVA: 0x00092E78 File Offset: 0x00091078
        public River(string newPath)
        {
            this.path = newPath;
            if (!Directory.Exists(Path.GetDirectoryName(this.path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(this.path));
            }
            this.stream = new FileStream(this.path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            this.water = 0;
        }

        // Token: 0x040010B8 RID: 4280
        private static byte[] buffer = new byte[Block.BUFFER_SIZE];

        // Token: 0x040010B9 RID: 4281
        private int water;

        // Token: 0x040010BA RID: 4282
        private string path;

        // Token: 0x040010BB RID: 4283
        private Stream stream;

        // Token: 0x040010BC RID: 4284
        private Block block;
    }
}
