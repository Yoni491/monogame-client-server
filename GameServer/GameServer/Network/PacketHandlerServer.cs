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
        PlayerManager _playerManager;
        PacketStructure _packetStructure;
        ushort packetLength;
        ushort packetType;
        int playerNum;
        public PacketHandlerServer(List<Player> players, PlayerManager playerManager)
        {
            _players = players;
            _playerManager = playerManager;
            _packetStructure = new PacketStructure();
        }
        public void Handle(byte[] buffer, Socket socket, Player player)
        {
            _packetStructure.updateBuffer(buffer);
            packetLength = _packetStructure.ReadUShort();
            packetType = _packetStructure.ReadUShort();
            if (packetType != 0)
                Console.WriteLine("Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
            switch (packetType)
            {
                case 0:
                    break;
                case 1:
                    //short packet from client to server
                    while (true)
                    {
                        playerNum = _packetStructure.ReadInt();
                        Player find_player = _players.Find(x => x.PlayerNum == playerNum);
                        if (find_player == null)
                            break;
                        find_player.ReadPacketShort(_packetStructure);
                        if (packetLength == _packetStructure._offset)
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
                    playerNum = _packetStructure.ReadInt();
                    player._gun._id = _packetStructure.ReadInt();
                    break;
                    
            }
            _packetStructure._offset = 0;
        }
    }
}
