using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GameClient
{
    public class NetworkManagerClient
    {
        Socket _socket;
        byte[] _buffer = new byte[10000];
        PacketHandlerClient _packetHandler;
        float _timer_short = 0;
        PlayerManager _playerManager;
        Player _player;
        List<NetworkPlayer> _network_players;
        Packet _packet;
        List<SimpleEnemy> _enemies;
        MultiplayerMenu _multiplayerMenu;
        public NetworkManagerClient()
        {
            
        }
        public void Initialize(List<NetworkPlayer> network_players, Player player,
            PlayerManager playerManager, List<SimpleEnemy> enemies, EnemyManager enemyManager,
            InventoryManager inventoryManager,LevelManager levelManager,MultiplayerMenu multiplayerMenu)
        {
            _multiplayerMenu = multiplayerMenu;
            _network_players = network_players;
            _enemies = enemies;
            _playerManager = playerManager;
            _player = player;
            _packetHandler = new PacketHandlerClient(_network_players, player, playerManager, enemies, enemyManager,inventoryManager, levelManager);
            _packet = new Packet();
            
        }
        public void Update(GameTime gameTime)
        {
            if (_socket.Connected)
            {
                _timer_short += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer_short >= 0.1f)//doesnt work if it is too fastss
                {
                    _timer_short = 0;
                    SendPacket(gameTime);
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
            _packet.WriteInt(1);//number of players to send.
            _player.UpdatePacketShort(_packet);//player data
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
                if(MapManager._chests.ContainsKey(item))
                    MapManager._chests[item].UpdatePacket(_packet);
                MapManager._chests.Remove(item);
            }
        }
        public void WriteItems()
        {
            _packet.WriteInt(ItemManager._itemsToSend.Count);
            foreach (var item in ItemManager._itemsToSend)
            {
                if (ItemManager._itemsOnTheGround.ContainsKey(item))
                    ItemManager._itemsOnTheGround[item].UpdatePacketNum(_packet);
            }
        }
        public void SendPacket(GameTime gameTime, int packetType = 1)
        {
            if (packetType == 1)
            {
                _packet.UpdateType(1);//packet type
                WriteLevel();
                WritePlayers();
                WriteEnemies();
                WriteBoxes();
                MapManager._boxesToSend.Clear();
                WriteDoors();
                MapManager._doorsToSend.Clear();
                WriteChests();
                MapManager._chestsToSend.Clear();
                WriteItems();
                ItemManager._itemsToSend.Clear();
            }
            else if( packetType == 2)
            {
                _packet.UpdateType(2);//packet type
                _packet.WriteString(_player._nameDisplay._text);
            }
            byte[] buffer = _packet.Data();
            _socket.Send(buffer);
        }
        #region NetworkMethods
        public void CheckIfIpValid(string ip, out IPAddress iPAddress)
        {
            if (string.IsNullOrEmpty(ip))
            {
                iPAddress = null;
                return;
            }
            IPAddress.TryParse(ip,out iPAddress);
        }
        public void Initialize_connection(string ip)
        {
            IPAddress iPAddress;
            IPEndPoint endPoint;
            CheckIfIpValid(ip, out iPAddress);
            if (iPAddress!=null)
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
                MainMenuManager._connected = true;
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
            if (_socket!=null && _socket.Connected)
            {
                _socket.Close();
            }
            MainMenuManager._connected = false;
            Game_Client._isMultiplayer = false;
            _network_players.Clear();
        }
        #endregion
    }
}
