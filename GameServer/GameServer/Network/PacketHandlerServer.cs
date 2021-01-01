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
    public class PacketHandlerServer
    {
        List<Player> _players;
        Player _player;
        PlayerManager _playerManager;
        public PacketHandlerServer(List<Player> players, PlayerManager playerManager)
        {
            _players = players;
            _playerManager = playerManager;
        }
        public void Handle(byte[] packet, Socket socket,Player player)
        {
            PacketStructure packetStructure = new PacketStructure(packet);
            ushort packetLength = packetStructure.ReadUShort();
            ushort packetType = packetStructure.ReadUShort();
            if (packetType != 0)
                Console.WriteLine("Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
            int playerNum;
            switch (packetType)
            {
                case 0:
                    break;
                case 1:
                    //short packet from client to server
                    while (true)
                    {
                        playerNum = packetStructure.ReadInt();
                        Player find_player = _players.Find(x => x.PlayerNum == playerNum);
                        if (find_player == null)
                            break;
                        find_player.ReadPacketShort(packetStructure);
                        if (packetLength == packetStructure._offset)
                        {
                            break;
                        }
                    }
                    break;
                case 2:
                    //short packet from client to server
                    break;
                case 3:
                    //first message containing player number from server
                    break;
                case 4:
                    //long packet from client to server
                    playerNum = packetStructure.ReadInt();
                    player._gun._id = packetStructure.ReadInt();
                    break;
                    
            }
        }
    }
}
