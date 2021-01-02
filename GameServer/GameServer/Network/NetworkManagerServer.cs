using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class NetworkManagerServer
    {
        float _timer = 0;
        static List<Socket> _socket_list;
        private List<Player> _players;
        Socket _socket;
        byte[] _buffer = new byte[10000];
        int packetType;
        PacketShort_Server _packet;
        PlayerManager _playerManager;
        List<PacketHandlerServer> _packetHandlers;
        int numOfPlayer = 0;

        public NetworkManagerServer(List<Socket> socket_list, List<Player> players, PlayerManager playerManager)
        {
            _socket_list = socket_list;
            _players = players;
            _packet = new PacketShort_Server(_players);
            _playerManager = playerManager;
            _packetHandlers = new List<PacketHandlerServer>();
        }
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 0.1f)
            {
                _timer = 0;
                foreach (var socket in _socket_list)
                {
                    _packet.updatePacket();
                    if (socket.Connected)
                    {
                        socket.Send(_packet.Data());
                        packetType = _packet.ReadUShort();
                        if (packetType != 0)
                            Console.WriteLine("server: packet left Length: {0} | type: {1}", packetType, _packet.ReadUShort());
                    }
                }
            }
            for (int i = 0; i < numOfPlayer; i++)
            {
                _packetHandlers[i].Update();
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
            PacketHandlerServer packetHandler = new PacketHandlerServer(_players, _playerManager, player);
            Accept();
            _packetHandlers.Add(packetHandler);
            numOfPlayer++;
            PacketStructure packet = new PacketStructure();
            packet.UpdateType(3);
            packet.WriteInt(player.PlayerNum);
            client_socket.Send(packet.Data());
            Receive(client_socket, packetHandler);
        }
        private void Receive(Socket client_socket, PacketHandlerServer packetHandlerServer)
        {
            client_socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallBack, Tuple.Create(client_socket, packetHandlerServer));
        }
        private void ReceivedCallBack(IAsyncResult result)
        {
            Tuple<Socket,  PacketHandlerServer> state = (Tuple<Socket, PacketHandlerServer>)result.AsyncState;
            Socket client_socket = state.Item1;
            PacketHandlerServer packetHandlerServer = state.Item2;
            int buffer_size = client_socket.EndReceive(result);
            packetHandlerServer.Handle(_buffer);
            Receive(client_socket, packetHandlerServer);
        }
    }
}
