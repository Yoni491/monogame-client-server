using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class PacketStructure
    {
        private byte[] _buffer;

        public PacketStructure(ushort length,ushort type)
        {
            _buffer = new byte[length];
            WriteUshort(length, 0);
            WriteUshort(type, 2);
        }
        public PacketStructure(byte []buffer)
        {
            _buffer = buffer;
        }
        public void WriteUshort(ushort value, int offset)
        {
            byte[] tempBuf = new byte[2];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, 2);
        }
        public ushort ReadUShort(int offset)
        {
            return BitConverter.ToUInt16(_buffer, offset);
        }
        public void WriteUInt(uint value,int offset)
        {
            byte[] tempBuf = new byte[4];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, 4);
        }
        public void WriteFloat(float value,int offset)
        {
            byte[] tempBuf = new byte[4];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, offset, 4);
        }
        public float ReadFloat(int offset)
        {
            return BitConverter.ToSingle(_buffer, offset);
        }
        public string ReadString(int offset,int count)
        {
            return Encoding.UTF8.GetString(_buffer, offset, count);
        }
        public void RewriteHeader(ushort length, ushort type)
        {
            WriteUshort(length, 0);
            WriteUshort(type, 2);
        }
        public byte[] Data { get { return _buffer; } }
    }
}
