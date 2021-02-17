using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace GameServer
{
    public class PacketHandlerServer
    {
        List<NetworkPlayer> _players;
        PlayerManager _playerManager;
        PacketStructure _packetStructure;
        ushort packetLength;
        ushort packetType;
        int playerNum;
        private bool handle = false;
        private int usingResource = 0;
        private NetworkPlayer _player;
        public PacketHandlerServer(List<NetworkPlayer> players, PlayerManager playerManager, NetworkPlayer player)
        {
            _players = players;
            _playerManager = playerManager;
            _packetStructure = new PacketStructure();
            _player = player;
        }
        public void Handle(byte[] buffer)
        {
            if (0 == Interlocked.Exchange(ref usingResource, 1))
            {
                _packetStructure.updateBuffer(buffer);
                packetLength = _packetStructure.ReadUShort();
                packetType = _packetStructure.ReadUShort();
                if (packetType != 0)
                    Console.WriteLine("Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
                handle = true;
            }

        }
        public void Update()
        {
            if (handle)
            {
                switch (packetType)
                {
                    case 0:
                        break;
                    case 1:
                        //short packet from client to server
                        while (true)
                        {
                            playerNum = _packetStructure.ReadInt();
                            if (playerNum == 1)
                                Console.WriteLine("1");
                            NetworkPlayer find_player = _players.Find(x => x.PlayerNum == playerNum);
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
                        //long packet containing player number from server
                        break;
                    case 4:
                        //long packet from client to server
                        playerNum = _packetStructure.ReadInt();
                        _player._gun._id = _packetStructure.ReadInt();
                        break;

                }
                _packetStructure._offset = 0;
                handle = false;
                Interlocked.Exchange(ref usingResource, 0);
            }
        }
    }
}
