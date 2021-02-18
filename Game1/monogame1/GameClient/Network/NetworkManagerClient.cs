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
        ushort packetType;
        PacketShort_Client _packet_short;
        public static bool _updatenetworkPlayerTexture = false;
        public NetworkManagerClient()
        {
            
        }
        public void Initialize(List<NetworkPlayer> _network_players, Player player, PlayerManager playerManager)
        {
            _playerManager = playerManager;
            _player = player;
            _packetHandler = new PacketHandlerClient(_network_players, player, playerManager);
            _packet_short = new PacketShort_Client();
            _packet_short.Initialize(player);
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
                    _packet_short.UpdatePacket();
                    _socket.Send(_packet_short.Data());
                    packetType = _packet_short.ReadUShort();
                    //if (packetType != 0)
                    //    Console.WriteLine("client: packet left Length: {0} | type: {1}", packetType, _packet_short.ReadUShort());
                }
                _timer_long += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer_long >= 1.5f)
                {
                    _timer_long = 0;
                    PacketLong_Client packet_long = new PacketLong_Client(_player);
                    _socket.Send(packet_long.Data());
                }
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
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("77.124.38.9"), 1994);
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
