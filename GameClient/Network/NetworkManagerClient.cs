using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
namespace GameClient
{
    public class NetworkManagerClient
    {
        Socket _socket;
        byte[] _buffer = new byte[10000];
        PacketHandlerClient _packetHandler;
        float _timer_short = 0;
        PlayerManager _playerManager;
        List<Player> _players;
        List<NetworkPlayer> _network_players;
        Packet _packet;
        List<SimpleEnemy> _enemies;
        int _sendPacketNames = 0;
        ConnectionScreen _multiplayerMenu;
        public NetworkManagerClient()
        {
        }
        public void Initialize(List<NetworkPlayer> network_players, List<Player> players,
            PlayerManager playerManager, List<SimpleEnemy> enemies, EnemyManager enemyManager,
            InventoryManager inventoryManager, LevelManager levelManager, ConnectionScreen multiplayerMenu)
        {
            _multiplayerMenu = multiplayerMenu;
            _network_players = network_players;
            _enemies = enemies;
            _playerManager = playerManager;
            _players = players;
            _packetHandler = new PacketHandlerClient(_network_players, players, playerManager, enemies, enemyManager, inventoryManager, levelManager);
            _packet = new Packet();
        }
        public void Update(GameTime gameTime)
        {
            if (_socket.Connected)
            {
                _sendPacketNames++;
                _timer_short += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer_short >= 0.1f)//doesnt work if it is too fastss
                {
                    if (_sendPacketNames >= 100)
                    {
                        SendPacket(gameTime, 2);
                        _sendPacketNames = 0;
                    }
                    else
                    {
                        SendPacket(gameTime);
                    }
                    _timer_short = 0;
                }
                _packetHandler.Update();
            }
        }
        public void WriteLevel()
        {
            _packet.WriteInt(LevelManager._currentLevel);
        }
        public void WritePlayers()
        {
            _packet.WriteInt(_players.Count);//number of players to send.
            _players.ForEach(player => player.UpdatePacketShort(_packet));//player data
        }
        public void WriteEnemies()
        {
            _packet.WriteInt(_enemies.FindAll(x => x._dmgDoneForServer != 0).Count);
            foreach (var enemy in _enemies)
            {
                enemy.UpdatePacketDmg(_packet);
            }
        }
        public void WriteBoxes()
        {
            _packet.WriteInt(MapManager._boxesToSend.Count);
            foreach (var item in MapManager._boxesToSend)
            {
                if (MapManager._boxes.ContainsKey(item))
                    MapManager._boxes[item].UpdatePacket(_packet);
                MapManager._boxes.Remove(item);
            }
        }
        public void WriteDoors()
        {
            _packet.WriteInt(MapManager._doorsToSend.Count);
            foreach (var item in MapManager._doorsToSend)
            {
                if (MapManager._doors.ContainsKey(item))
                    MapManager._doors[item].UpdatePacket(_packet);
                MapManager._doors.Remove(item);
            }
        }
        public void WriteChests()
        {
            _packet.WriteInt(MapManager._chestsToSend.Count);
            foreach (var item in MapManager._chestsToSend)
            {
                if (MapManager._chests.ContainsKey(item))
                    MapManager._chests[item].UpdatePacket(_packet);
                MapManager._chests.Remove(item);
            }
        }
        public void WriteItemsPickedFromGround()
        {
            _packet.WriteInt(ItemManager._itemsToSendPicked.Count);
            foreach (var item in ItemManager._itemsToSendPicked)
            {
                if (ItemManager._itemsOnTheGround.ContainsKey(item))
                {
                    ItemManager._itemsOnTheGround[item].UpdatePacketNum(_packet);
                    ItemManager._itemsOnTheGround[item]._aboutToBeSent = false;
                }
            }
        }
        public void WriteItemDroppedToGround()
        {
            _packet.WriteInt(ItemManager._itemsToSendDropped.Count);
            foreach (var item in ItemManager._itemsToSendDropped)
            {
                _packet.WriteInt(item.Item1);
                _packet.WriteVector2(item.Item2);
            }
        }
        public void SendPacket(GameTime gameTime, int packetType = 1)
        {
            //if (packetType == 1)
            //{
            //    _packet.UpdateType(1);//packet type
            //    WriteLevel();
            //    WritePlayers();
            //    WriteEnemies();
            //    WriteBoxes();
            //    MapManager._boxesToSend.Clear();
            //    WriteDoors();
            //    MapManager._doorsToSend.Clear();
            //    WriteChests();
            //    MapManager._chestsToSend.Clear();
            //    WriteItemsPickedFromGround();
            //    ItemManager._itemsToSendPicked.Clear();
            //    WriteItemDroppedToGround();
            //    ItemManager._itemsToSendDropped.Clear();
            //}
            //else if (packetType == 2)
            //{
            //    _packet.UpdateType(2);//packet type
            //    _packet.WriteString(_player._nameDisplay._text);
            //}
            //byte[] buffer = _packet.Data();
            //_socket.Send(buffer);
        }
        #region NetworkMethods
        public void CheckIfIpValid(string ip, out IPAddress iPAddress)
        {
            if (string.IsNullOrEmpty(ip))
            {
                iPAddress = null;
                return;
            }
            IPAddress.TryParse(ip, out iPAddress);
        }
        public void Initialize_connection(string ip)
        {
            IPAddress iPAddress;
            IPEndPoint endPoint;
            CheckIfIpValid(ip, out iPAddress);
            if (iPAddress != null)
            {
                endPoint = new IPEndPoint(iPAddress, 1994);
            }
            else
            {
                endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1994);
            }
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.BeginConnect(endPoint, ConnectCallBack, _socket);
        }
        private void ConnectCallBack(IAsyncResult result)
        {
            if (_socket.Connected)
            {
                MainMenuScreen._connected = true;
                Game_Client._isMultiplayer = true;
                Receive();
            }
            else
            {
                _multiplayerMenu._connecting = false;
            }
        }
        private void Receive()
        {
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallBack, null);
        }
        private void ReceivedCallBack(IAsyncResult result)
        {
            try
            {
                int buffer_size = _socket.EndReceive(result);
            }
            catch
            {
                return;
            }
            _packetHandler.Handle(_buffer);
            Receive();
        }
        public void CloseConnection()
        {
            if (_socket != null && _socket.Connected)
            {
                _socket.Close();
            }
            MainMenuScreen._connected = false;
            Game_Client._isMultiplayer = false;
            _network_players.Clear();
        }
        #endregion
    }
}
