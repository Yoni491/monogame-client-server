using Microsoft.Xna.Framework;
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
        private EnemyManager _enemyManager;
        private InventoryManager _inventoryManager;
        private LevelManager _levelManager;
        private  List<Simple_Enemy> _enemies;
        Packet _packet;
        ushort packetLength;
        ushort packetType;

        int usingResource = 0;
        public PacketHandlerClient(List<NetworkPlayer> players, Player player, PlayerManager playerManager, List<Simple_Enemy> enemies, EnemyManager enemyManager, InventoryManager inventoryManager, LevelManager levelManager)
        {
            _players = players;
            _player = player;
            _playerManager = playerManager;
            _enemyManager = enemyManager;
            _inventoryManager = inventoryManager;
            _levelManager = levelManager;
            _enemies = enemies;
            _packet = new Packet();
        }
        public void Handle(byte[] buffer)
        {
            while (true)
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
                //if (packetType != 0)
                //    Console.WriteLine("11Client: Recevied packet! Length: {0} | type: {1}", packetLength, packetType);
                switch (packetType)
                {
                    case 0:
                        break;
                    case 1://short packet
                        ReadNewLevel();
                        ReadPlayers();
                        ReadEnemies();
                        ReadBoxes();
                        ReadChests();
                        ReadItems();
                        ReadItemsPickedUp();
                        break;
                    case 3:
                        //long packet containing player number from server
                        _player._playerNum = _packet.ReadInt();
                        break;
                }
                _packet._offset = 0;
                handle = false;
                Interlocked.Exchange(ref usingResource, 0);
            }
        }
        public void ReadPlayers()
        {
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
            }
        }
        public void ReadEnemies()
        {
            int numOfEnemies = _packet.ReadInt();
            int enemyNum;
            for (int i = 0; i < numOfEnemies; i++)
            {
                enemyNum = _packet.ReadInt();
                Simple_Enemy simple_Enemy = _enemies.Find(x => x._enemyNum == enemyNum);
                int enemyId = _packet.ReadInt();
                if (simple_Enemy == null)
                {
                    simple_Enemy = _enemyManager.AddEnemyFromServer(enemyNum, enemyId);
                }
                simple_Enemy.ReadPacketShort(_packet);
                if (simple_Enemy._health._health_left <= 0)
                    _enemies.Remove(simple_Enemy);
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
                    MapManager._boxes.Remove(boxNum);
                    MapManager._boxesToSend.Remove(boxNum);
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
                    Chest chest = MapManager._chests[chestNum];
                    chest.Open();
                    MapManager._chests.Remove(chestNum);
                    MapManager._chestsToSend.Remove(chestNum);
                }
            }
        }
        public void ReadItems()
        {
            int numOfItems = _packet.ReadInt();
            for (int i = 0; i < numOfItems; i++)
            {
                int itemNum = _packet.ReadInt();
                int itemId = _packet.ReadInt();
                Vector2 position = _packet.ReadVector2(); 
                if (!ItemManager._itemsOnTheGround.ContainsKey(itemNum))
                {
                    ItemManager.DropItemFromServer(itemNum, itemId, position);
                }
            }
        }
        public void ReadItemsPickedUp()
        {
            int numOfItems = _packet.ReadInt();
            for (int i = 0; i < numOfItems; i++)
            {
                int playerNum = _packet.ReadInt();
                int itemNum = _packet.ReadInt();
                if (ItemManager._itemsOnTheGround.ContainsKey(itemNum))
                {
                    if(playerNum == _player._playerNum)
                    {
                        _inventoryManager.addItemToInventory(ItemManager._itemsOnTheGround[itemNum]);
                    }
                    else
                    {
                        ItemManager._itemsOnTheGround.Remove(itemNum);
                    }
                }
            }
        }
        public void ReadNewLevel()
        {
            if(_packet.ReadInt()==1)
            {
                _levelManager.LoadNewLevel(_packet.ReadInt());
                _player.PositionPlayerFeetAt(_packet.ReadVector2());
            }
        }
    }
}
