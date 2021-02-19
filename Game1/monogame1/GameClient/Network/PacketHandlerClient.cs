using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace GameClient
{
    public class PacketHandlerClient
    {
        public bool handle = false;
        List<NetworkPlayer> _players;
        Player _player;
        PlayerManager _playerManager;
        private readonly EnemyManager _enemyManager;
        private  List<Simple_Enemy> _enemies;
        Packet _packet;
        ushort packetLength;
        ushort packetType;

        int usingResource = 0;
        public PacketHandlerClient(List<NetworkPlayer> players, Player player, PlayerManager playerManager, List<Simple_Enemy> enemies, EnemyManager enemyManager)
        {
            _players = players;
            _player = player;
            _playerManager = playerManager;
            _enemyManager = enemyManager;
            _enemies = enemies;
            _packet = new Packet();
        }
        public void Handle(byte[] buffer)
        {

            if (0 == Interlocked.Exchange(ref usingResource, 1))
            {
                _packet.UpdateBuffer(buffer);
                packetLength = _packet.ReadUShort();
                packetType = _packet.ReadUShort();
                //if (packetType != 0)
                //    Console.WriteLine("Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
                handle = true;
            }
        }
        public void Update()
        {
            if (handle)
            {
                //if (packetType != 0)
                //    Console.WriteLine("11Client: Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
                switch (packetType)
                {
                    case 0:
                        break;
                    case 1://short packet
                        int numOfPlayers = _packet.ReadInt();
                        int playerNum;
                        for (int i = 0; i < numOfPlayers; i++)
                        {
                            playerNum = _packet.ReadInt();
                            NetworkPlayer networkPlayer = _players.Find(x => x._playerNum == playerNum);
                            if (networkPlayer == null)
                            {
                                networkPlayer = _playerManager.AddnetworkPlayer(playerNum);
                            }
                            networkPlayer.ReadPacketShort(_packet);
                            if (packetLength <= _packet._offset)
                            {
                                break;
                            }
                        }
                        int numOfEnemies = _packet.ReadInt();
                        int enemyNum;
                        for (int i = 0; i < numOfEnemies; i++)
                        {
                            enemyNum = _packet.ReadInt();
                            Simple_Enemy simple_Enemy  = _enemies.Find(x => x._enemyNum == enemyNum);
                            int enemyId = _packet.ReadInt();
                            if (simple_Enemy == null && enemyId != -1)
                            {
                                simple_Enemy = _enemyManager.AddEnemyFromServer(enemyNum, enemyId);
                            }
                            if (enemyId == -1)
                                _enemies.Remove(simple_Enemy);
                            else
                                simple_Enemy.ReadPacketShort(_packet);
                        }
                        int numOfBoxes = _packet.ReadInt();
                        for (int i = 0; i < numOfBoxes; i++)
                        {
                            int boxNum = _packet.ReadInt();
                            Box box = MapManager._boxes[boxNum];
                            if (box != null)
                            {
                                box.Destroy();
                                MapManager._boxes.Remove(boxNum);
                            }
                        }
                        break;
                    case 2:
                        break;
                    case 3:
                        //long packet containing player number from server
                        _player._playerNum = _packet.ReadInt();
                        //Console.WriteLine("A");
                        break;
                    case 4:
                        //long packet from client to server
                        break;
                }
                _packet._offset = 0;
                handle = false;
                Interlocked.Exchange(ref usingResource, 0);
            }
        }
    }
}
