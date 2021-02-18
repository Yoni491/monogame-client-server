using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using GameClient;
namespace GameServer
{
    public class PacketHandlerServer
    {
        List<NetworkPlayer> _players;
        Packet _packet;
        ushort packetLength;
        ushort packetType;
        int playerNum;
        private bool handle = false;
        private int usingResource = 0;
        private NetworkPlayer _player;
        public PacketHandlerServer(List<NetworkPlayer> players, NetworkPlayer player)
        {
            _players = players;
            _packet = new Packet();
            _player = player;
        }
        public void Handle(byte[] buffer)
        {
            if (0 == Interlocked.Exchange(ref usingResource, 1))
            {
                _packet.UpdateBuffer(buffer);
                packetLength = _packet.ReadUShort();
                packetType = _packet.ReadUShort();
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
                            int numOfPlayers = _packet.ReadInt();
                            playerNum = _packet.ReadInt();
                            _player.ReadPacketShort(_packet);
                            if (packetLength == _packet._offset)
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
                        playerNum = _packet.ReadInt();
                        _player._gun._id = _packet.ReadInt();
                        break;

                }
                _packet._offset = 0;
                handle = false;
                Interlocked.Exchange(ref usingResource, 0);
            }
        }
    }
}
