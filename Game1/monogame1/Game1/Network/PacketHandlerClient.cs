using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace GameClient
{
    public class PacketHandlerClient
    {
        public bool handle = false;
        List<OtherPlayer> _players;
        Player _player;
        PlayerManager _playerManager;
        PacketStructure _packetStructure;
        ushort packetLength;
        ushort packetType;
        int playerNum;
        int usingResource = 0;
        public PacketHandlerClient(List<OtherPlayer> players, Player player, PlayerManager playerManager)
        {
            _players = players;
            _player = player;
            _playerManager = playerManager;
            _packetStructure = new PacketStructure();
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
                if (packetType != 0)
                    Console.WriteLine("11Client: Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
                switch (packetType)
                {
                    case 0:
                        break;
                    case 1:
                        //short packet from client to server
                        break;
                    case 2:
                        //short packet from server to client

                        while (true)
                        {
                            playerNum = _packetStructure.ReadInt();
                            OtherPlayer otherPlayer = _players.Find(x => x._playerNum == playerNum);
                            if (otherPlayer == null)
                            {
                                otherPlayer = _playerManager.AddOtherPlayer(playerNum);
                            }
                            otherPlayer.ReadPacketShort(_packetStructure);
                            if (packetLength <= _packetStructure._offset)
                            {
                                break;
                            }
                        }
                        break;
                    case 3:
                        //long packet containing player number from server
                        _player._playerNum = _packetStructure.ReadInt();
                        Console.WriteLine("A");
                        break;
                    case 4:
                        //long packet from client to server
                        break;
                }
                _packetStructure._offset = 0;
                handle = false;
                Interlocked.Exchange(ref usingResource, 0);
            }
        }
    }
}
