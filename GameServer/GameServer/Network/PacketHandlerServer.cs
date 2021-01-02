using System;
using System.Collections.Generic;
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
        private bool handle = false;
        private Player _player;
        public PacketHandlerServer(List<Player> players, PlayerManager playerManager, Player player)
        {
            _players = players;
            _playerManager = playerManager;
            _packetStructure = new PacketStructure();
            _player = player;
        }
        public void Handle(byte[] buffer)
        {
            if (!handle)
            {
                _packetStructure.updateBuffer(buffer);
                packetLength = _packetStructure.ReadUShort();
                packetType = _packetStructure.ReadUShort();
                handle = true;
            }

        }
        public void Update()
        {
            if (handle)
            {
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
                        _player._gun._id = _packetStructure.ReadInt();
                        break;

                }
                _packetStructure._offset = 0;
                handle = false;
            }
        }
    }
}
