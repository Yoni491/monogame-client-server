using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class PacketStructure
    {
        private byte[] _buffer;
        private int _offset = 0;

        public PacketStructure(ushort length,ushort type)
        {
            _buffer = new byte[length];
            WriteUshort(length);
            WriteUshort(type);
        }
        public PacketStructure(byte []buffer)
        {
            _buffer = buffer;
        }
        public void WriteUshort(ushort value)
        {
            byte[] tempBuf = new byte[2];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, _offset, 2);
        }
        public ushort ReadUShort(int offset)
        {
            _offset += 2;
            return BitConverter.ToUInt16(_buffer, offset);
        }
        public void WriteUInt(uint value)
        {
            _offset += 4;
            byte[] tempBuf = new byte[4];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, _offset, 4);
        }
        public void WriteInt(int value)
        {
            _offset += 4;
            byte[] tempBuf = new byte[4];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, _offset, 4);
        }
        public void WriteFloat(float value)
        {
            _offset += 4;
            byte[] tempBuf = new byte[4];
            tempBuf = BitConverter.GetBytes(value);
            Buffer.BlockCopy(tempBuf, 0, _buffer, _offset, 4);
        }
        public void WriteVector2(Vector2 value)
        {
            WriteFloat(value.X);
            WriteFloat(value.Y);
        }
        public Vector2 ReadVector2()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            return new Vector2(x,y);
        }
        public float ReadFloat()
        {
            _offset += 4;
            return BitConverter.ToSingle(_buffer, _offset);
        }
        public int ReadInt()
        {
            _offset += 4;
            return BitConverter.ToInt32(_buffer, _offset);
        }
        public string ReadString(int count)
        {
            return Encoding.UTF8.GetString(_buffer, _offset, count);
        }
        public void RewriteHeader(ushort length, ushort type)
        {
            _offset = 0;
            WriteUshort(length);
            WriteUshort(type);
        }
        public byte[] Data { get { return _buffer; } }
    }
}
