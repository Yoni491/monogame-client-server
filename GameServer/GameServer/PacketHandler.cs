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
        List<Player> _players;
        Player _player;
        PlayerManager _playerManager;
        public PacketHandler(List<Player> players, PlayerManager playerManager)
        {
            _players = players;
            _playerManager = playerManager;
        }
        public void Handle(byte[] packet, Socket socket,Player player)
        {
            PacketStructure packetStructure = new PacketStructure(packet);
            ushort packetLength = packetStructure.ReadUShort();
            ushort packetType = packetStructure.ReadUShort();
            Console.WriteLine("Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
            switch (packetType)
            {
                case 1:
                    Console.WriteLine("1");
                    player.Position = new Vector2(packetStructure.ReadFloat(), packetStructure.ReadFloat());
                    break;
                case 3:
                    //first message
                    _player._playerNum = packetStructure.ReadInt();
                    break;
            }
        }
    }
}
