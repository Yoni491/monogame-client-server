using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using GameClient;
using Microsoft.Xna.Framework;

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
        public int _playerCurrentLevel;
        private Vector2 _playerPreviousPosition;

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
                        ReadLevel();
                        ReadPlayer();
                        ReadEnemies();
                        ReadBoxes();
                        ReadChests();
                        ReadItems();
                        
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
        public void ReadLevel()
        {
            _playerCurrentLevel = _packet.ReadInt();
        }
        public void ReadPlayer()
        {
            int numOfPlayers = _packet.ReadInt();
            playerNum = _packet.ReadInt();
            //_playerPreviousPosition = _player._position;
            _player.ReadPacketShort(_packet);
            //if (_playerCurrentLevel != LevelManager._currentLevel)
            //    _player._position = _playerPreviousPosition;
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
        public void ReadChests()
        {
            int numOfChests = _packet.ReadInt();
            for (int i = 0; i < numOfChests; i++)
            {
                int chestNum = _packet.ReadInt();
                if (MapManager._chests.ContainsKey(chestNum))
                {
                    MapManager._chests[chestNum].Open();
                }
            }
        }
        public void ReadItems()
        {
            int numOfItems = _packet.ReadInt();
            for (int i = 0; i < numOfItems; i++)
            {
                int itemNum = _packet.ReadInt();
                if (ItemManager._itemsOnTheGround.ContainsKey(itemNum))
                {
                    ItemManager._itemsPickedUpToSend.Add((_player._playerNum,itemNum));
                    ItemManager._itemsOnTheGround.Remove(itemNum);
                }
            }
        }
    }
}
