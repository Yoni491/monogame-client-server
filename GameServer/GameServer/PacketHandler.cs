using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class PacketHandler
    {
        public void Handle(byte[] packet, Socket socket,Player player)
        {
            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);
            PacketStructure packetStructure = new PacketStructure(packet);
            Console.WriteLine("Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
            switch(packetType)
            {
                case 1:
                    Console.WriteLine("1");
                    player.Position = new Vector2(packetStructure.ReadFloat(), packetStructure.ReadFloat());
                    break;
                case 2:
                    Console.WriteLine("2");
                    break;
            }
        }
    }
}
