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
        float _timer_long = 0;
        bool _connect_again = false;
        PlayerManager _playerManager;
        Player _player;
        Packet _packet;
        List<Simple_Enemy> _enemies;
        public NetworkManagerClient()
        {
            
        }
        public void Initialize(List<NetworkPlayer> _network_players, Player player, PlayerManager playerManager, List<Simple_Enemy> enemies, EnemyManager enemyManager)
        {
            _enemies = enemies;
            _playerManager = playerManager;
            _player = player;
            _packetHandler = new PacketHandlerClient(_network_players, player, playerManager, enemies, enemyManager);
            _packet = new Packet();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Update(GameTime gameTime)
        {
            if (_socket.Connected)
            {
                SendPacket(gameTime);
                _packetHandler.Update();
            }
            else if (_connect_again)
            {
                _connect_again = false;
                Initialize_connection();
            }
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
                MapManager._boxes[item].UpdatePacket(_packet);
                Console.WriteLine("box,");
                MapManager._boxes.Remove(item);
            }
        }
        public void SendPacket(GameTime gameTime)
        {
            _timer_short += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer_short >= 0.1f)//doesnt work if it is too fast
            {
                _timer_short = 0;
                _packet.UpdateType(1);//packet type
                WritePlayers();
                WriteEnemies();
                WriteBoxes();
                MapManager._boxesToSend.Clear();
                byte[] buffer = _packet.Data();
                _socket.Send(buffer);
            }
        }
        #region NetworkMethods
        public void Initialize_connection()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("77.126.31.222"), 1994);
            _socket.BeginConnect(endPoint, ConnectCallBack, _socket);

        }
        private void ConnectCallBack(IAsyncResult result)
        {
            if (_socket.Connected)
            {
                MenuManager._connected = true;
                Game_Client._IsMultiplayer = true;
                Receive();
            }
            else
            {
                _connect_again = true;
            }
        }
        private void Receive()
        {
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallBack, null);
        }
        private void ReceivedCallBack(IAsyncResult result)
        {
            int buffer_size = _socket.EndReceive(result);
            _packetHandler.Handle(_buffer);
            Receive();
        }
        public void CloseConnection()
        {
            if(_socket.Connected)
                _socket.Close();
        }
        #endregion
    }
}
