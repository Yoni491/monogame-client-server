using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace GameClient
{
    public class Packet
    {
        private byte[] _buffer = new byte[10000];
        public ushort _offset = 0;

        public Packet()
        {

        }

        public void UpdateBuffer(byte[] buffer)
        {
            buffer.CopyTo(_buffer, 0);
        }
        public void WriteUshort(ushort value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _offset, 2);
            _offset += 2;
        }
        public ushort ReadUShort()
        {
            _offset += 2;
            return BitConverter.ToUInt16(_buffer, _offset - 2);
        }
        public void WriteUInt(uint value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _offset, 4);
            _offset += 4;
        }
        public void WriteInt(int value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _offset, 4);
            _offset += 4;
        }
        public void WriteFloat(float value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _buffer, _offset, 4);
            _offset += 4;
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
            return new Vector2(x, y);
        }
        public float ReadFloat()
        {
            if (_offset >= _buffer.Length)
                return -1;
            _offset += 4;
            return BitConverter.ToSingle(_buffer, _offset - 4);
        }
        public int ReadInt()
        {
            if (_offset >= _buffer.Length)
                return -1;
            _offset += 4;
            return BitConverter.ToInt32(_buffer, _offset - 4);
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
        public void UpdatePacketLength()
        {
            Buffer.BlockCopy(BitConverter.GetBytes(_offset), 0, _buffer, 0, 2);
        }
        public void UpdateType(ushort type)//type 1 - short packet
        {
            Buffer.BlockCopy(BitConverter.GetBytes(type), 0, _buffer, 2, 2);
            _offset = 4;
        }
        public byte[] Data()
        {
            UpdatePacketLength();
            if (Game_Client._isServer)
            {
                Console.WriteLine("buffer array:");
                for (int i = 0; i < _offset; i++)
                {
                    if (i % 4 == 0)
                    {
                        Console.Write(i/4 + "-");
                    }
                    Console.Write(_buffer[i]);
                    if (i % 4 == 3)
                        Console.Write(" ");
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
            return _buffer;
        }
    }
}
