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
        float _timer_long = 0;
        bool _connect_again = false;
        PlayerManager _playerManager;
        Player _player;
        Packet _packet;
        public static bool _updatenetworkPlayerTexture = false;
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
                _timer_short += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer_short >= 0.05f)
                {
                    _timer_short = 0;
                    _packet.UpdateType(1);//packet type
                    _packet.WriteInt(1);//number of players to send.
                    _player.UpdatePacketShort(_packet);//player data
                    _packet.WriteInt(_enemies.FindAll(x => x._dmgDoneForServer != 0).Count);
                    foreach (var enemy in _enemies)
                    {
                        enemy.UpdatePacketDmg(_packet);
                    }
                    _packet.WriteInt(MapManager._boxesToSend.Count);
                    foreach (var box in MapManager._boxesToSend)
                    {
                        box.UpdatePacket(_packet);
                    }
                    MapManager._boxesToSend.Clear();
                    _socket.Send(_packet.Data());
                }
                //_timer_long += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //if (_timer_long >= 1.5f)
                //{
                //    _timer_long = 0;
                //    PacketLong_Client packet_long = new PacketLong_Client(_player);
                //    _socket.Send(packet_long.Data());
                //}
                _packetHandler.Update();
            }
            else if (_connect_again)
            {
                _connect_again = false;
                Initialize_connection();
            }
            if (NetworkManagerClient._updatenetworkPlayerTexture)
            {
                _playerManager.updatenetworkPlayerTexture();
            }
        }
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
    }
}
