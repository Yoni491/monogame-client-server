using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class NetworkManagerServer
    {
        float _timer = 0;
        static List<Socket> _socket_list;
        private List<Player> _players;
        PacketHandlerServer _packetHandler;
        Socket _socket;
        byte[] _buffer = new byte[2000];

        public NetworkManagerServer(List<Socket> socket_list,List<Player> players, PlayerManager playerManager)
        {
            _socket_list = socket_list;
            _players = players;
            _packetHandler = new PacketHandlerServer(_players, playerManager);
        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 0.5f)
            {
                _timer = 0;
                PacketShort_Server packet1 = new PacketShort_Server(_players);
                foreach (var socket in _socket_list)
                {
                    if (socket.Connected)
                    {
                        socket.Send(packet1.Data());
                    }
                }
            }
            //_timer2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (_timer2 >= 1f)
            //{
            //    _timer2 = 0;
            //    PacketShort_Server packet1 = new PacketShort_Server(_players);
            //    packet1.UpdatePacket();
            //    foreach (var socket in _socket_list)
            //    {
            //        if (socket.Connected)
            //        {
            //            _socket.Send(packet1.Data());
            //        }
            //    }
            //}

        }
        public void Initialize_connection()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(0, 1994));
            _socket.Listen(0);
            Accept();

        }
        private void Accept()
        {
            _socket.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult result)
        {
            Socket client_socket = _socket.EndAccept(result);
            _socket_list.Add(client_socket);
            Player player = new Player(Vector2.Zero, 100);
            _players.Add(player);
            Accept();
            PacketStructure packet = new PacketStructure(3);
            packet.WriteInt(player.PlayerNum);
            client_socket.Send(packet.Data());
            Receive(client_socket, player);
        }
        private void Receive(Socket client_socket, Player player)
        {
            client_socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallBack, Tuple.Create(client_socket, player));
        }
        private void ReceivedCallBack(IAsyncResult result)
        {
            Tuple<Socket, Player> state = (Tuple<Socket, Player>)result.AsyncState;
            Socket client_socket = state.Item1;
            Player player = state.Item2;
            int buffer_size = client_socket.EndReceive(result);
            _packetHandler.Handle(_buffer, client_socket, player);
            Receive(client_socket, player);
        }
    }
}
