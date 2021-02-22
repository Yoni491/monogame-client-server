using System;
using System.Collections.Concurrent;
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
        private readonly List<Simple_Enemy> _enemies;

        public PacketHandlerServer(List<NetworkPlayer> players, NetworkPlayer player, List<Simple_Enemy> enemies)
        {
            _players = players;
            _packet = new Packet();
            _player = player;
            this._enemies = enemies;
        }
        public void Handle(byte[] buffer)
        {
            //if (packetType != 0)
            //    Console.WriteLine("Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
            while(true)
                if (0 == Interlocked.Exchange(ref usingResource, 1))
                {
                    _packet.UpdateBuffer(buffer);
                    packetLength = _packet.ReadUShort();
                    packetType = _packet.ReadUShort();
                    handle = true;
                    return;
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
                        ReadPlayer();
                        ReadEnemies();
                        ReadBoxes();
                       
                        
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
        public void ReadPlayer()
        {
            int numOfPlayers = _packet.ReadInt();
            playerNum = _packet.ReadInt();
            _player.ReadPacketShort(_packet);
        }
        public void ReadEnemies()
        {
            int numOfEnemies = _packet.ReadInt();
            int enemyNum;
            for (int i = 0; i < numOfEnemies; i++)
            {
                enemyNum = _packet.ReadInt();
                Simple_Enemy simple_Enemy = _enemies.Find(x => x._enemyNum == enemyNum);
                int dmg = _packet.ReadInt();
                if (simple_Enemy != null)
                {
                    simple_Enemy.DealDamage(dmg);
                }
            }
        }
        public void ReadBoxes()
        {
            int numOfBoxes = _packet.ReadInt();
            for (int i = 0; i < numOfBoxes; i++)
            {
                int boxNum = _packet.ReadInt();
                if (MapManager._boxes.ContainsKey(boxNum))
                {
                    Box box = MapManager._boxes[boxNum];
                    box.Destroy();
                }
            }
        }
    }
}
