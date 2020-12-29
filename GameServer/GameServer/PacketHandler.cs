using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class PacketHandler
    {
        public void Handle(byte[] packet, Socket socket)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);
            Console.WriteLine("Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
            switch(packetType)
            {
                case 1:
                    Console.WriteLine("hello");
                    break;
            }
        }
    }
}
