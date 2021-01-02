using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class NetworkManagerServer
    {
        float _timer_short = 0;
        float _timer_long = 0;
        static List<Socket> _socket_list;
        private List<Player> _players;
        Socket _socketServer;
        private List<byte> _bufferList;
        //byte[] _buffer = new byte[10000];
        int packetType;
        //PacketShort_Server _packet;
        PlayerManager _playerManager;
        List<PacketHandlerServer> _packetHandlers;
        int numOfPlayer = 0;
        int addPlayers = 0;
        List<Socket> _socketToAdd;

        public NetworkManagerServer(List<Socket> socket_list, List<Player> players, PlayerManager playerManager)
        {
            _socket_list = socket_list;
            _players = players;
            //_packet = new PacketShort_Server(_players);
            //_packet_long = new PacketLong_Server();
            _playerManager = playerManager;
            _packetHandlers = new List<PacketHandlerServer>();
            _socketToAdd = new List<Socket>();
            _bufferList = new List<Byte>();
        }
        public void Update(GameTime gameTime)
        {
            int tempPlayers = addPlayers;
            for (int i = 0; i < tempPlayers; i++)
            {
                Socket socket= _socketToAdd[0];
                _socket_list.Add(socket);
                _socketToAdd.RemoveAt(0);
                byte[] buffer = new byte[10000];
                Player player = new Player(Vector2.Zero, 100,socket,_players);
                _players.Add(player);
                PacketHandlerServer packetHandler = new PacketHandlerServer(_players, _playerManager, player);
                _packetHandlers.Add(packetHandler);
                numOfPlayer++;
                PacketStructure packet = new PacketStructure();
                packet.UpdateType(3);
                packet.WriteInt(player.PlayerNum);
                socket.Send(packet.Data());
                Receive(socket, packetHandler,buffer);
            }
            addPlayers -= tempPlayers;
            _timer_short += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer_short >= 0.1f)
            {
                _timer_short = 0;
                foreach (var player in _players)
                {
                    player._shortPacket.UpdatePacket();
                    if (player._socket.Connected)
                    {
                        player._socket.Send(player._shortPacket.Data());
                        packetType = player._shortPacket.ReadUShort();
                        if (packetType != 0)
                            Console.WriteLine("server: packet left Length: {0} | type: {1}", packetType, player._shortPacket.ReadUShort());
                    }
                }
            }
            for (int i = 0; i < numOfPlayer; i++)
            {
                _packetHandlers[i].Update();
            }
            _timer_long += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (_timer_long >= 1f)
            //{
            //    _timer_long = 0;
            //    foreach (var player in _players)
            //    {
            //        if (player._socket.Connected)
            //        {
            //            player._longPacket.UpdatePacket();
            //            player._socket.Send(player._longPacket.Data());
            //            packetType = player._longPacket.ReadUShort();
            //            if (packetType != 0)
            //                Console.WriteLine("server: packet left Length: {0} | type: {1}", packetType, player._longPacket.ReadUShort());
            //        }
            //    }
            //}

        }
        public void Initialize_connection()
        {
            _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socketServer.Bind(new IPEndPoint(0, 1994));
            _socketServer.Listen(0);
            Accept();

        }
        private void Accept()
        {
            _socketServer.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult result)
        {
            Socket client_socket = _socketServer.EndAccept(result);
            addPlayers++;
            _socketToAdd.Add(client_socket);
            Accept();
            
        }
        private void Receive(Socket client_socket, PacketHandlerServer packetHandlerServer, byte[] buffer)
        {
            client_socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallBack, Tuple.Create(client_socket, packetHandlerServer, buffer));
        }
        private void ReceivedCallBack(IAsyncResult result)
        {
            Tuple<Socket,  PacketHandlerServer, byte[]> state = (Tuple<Socket, PacketHandlerServer, byte[]>)result.AsyncState;
            Socket client_socket = state.Item1;
            PacketHandlerServer packetHandlerServer = state.Item2;
            byte[] buffer = state.Item3;
            int buffer_size = client_socket.EndReceive(result);
            packetHandlerServer.Handle(buffer);
            Receive(client_socket, packetHandlerServer,buffer);
        }
    }
}
