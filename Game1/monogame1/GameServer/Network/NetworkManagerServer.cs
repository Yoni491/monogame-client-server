using GameClient;
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
        private List<NetworkPlayer> _players;
        private List<Simple_Enemy> _enemies;
        Socket _socketServer;
        private List<byte> _bufferList;
        int packetType;
        List<PacketHandlerServer> _packetHandlers;
        int numOfPlayer = 0;
        int addPlayers = 0;
        List<Socket> _socketToAdd;
        Packet _packet;
        public NetworkManagerServer(List<Socket> socket_list, List<NetworkPlayer> players, List<Simple_Enemy> enemies)
        {
            _socket_list = socket_list;
            _players = players;
            _enemies = enemies;
            _packetHandlers = new List<PacketHandlerServer>();
            _socketToAdd = new List<Socket>();
            _bufferList = new List<Byte>();
            _packet = new Packet();
            
        }
        public void Initialize_connection()
        {
            _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socketServer.Bind(new IPEndPoint(0, 1994));
            _socketServer.Listen(0);
            Accept();
        }
        public void Update(GameTime gameTime)
        {
            int tempPlayers = addPlayers;
            for (int i = 0; i < tempPlayers; i++)
            {
                Socket socket = _socketToAdd[0];
                _socket_list.Add(socket);
                _socketToAdd.RemoveAt(0);
                byte[] buffer = new byte[10000];
                NetworkPlayer player = new NetworkPlayer(Vector2.Zero, 100, numOfPlayer,null);
                _players.Add(player);
                PacketHandlerServer packetHandler = new PacketHandlerServer(_players, player,_enemies);
                _packetHandlers.Add(packetHandler);
                numOfPlayer++;
                Packet packet = new Packet();
                packet.UpdateType(3);
                packet.WriteInt(player._playerNum);
                socket.Send(packet.Data());
                Receive(socket, packetHandler, buffer);
            }
            addPlayers -= tempPlayers;
            _timer_short += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer_short >= 0.01f)
            {
                _timer_short = 0;
                _packet.UpdateType(1);
                _packet.WriteInt(_players.Count);
                foreach (var player in _players)
                {
                    player.UpdatePacketShort(_packet);
                    if (player._gun != null)
                        player._gun._bullets.Clear();
                }
                _packet.WriteInt(_enemies.Count);
                foreach (var enemy in _enemies)
                {
                    enemy.UpdatePacketShort(_packet);
                    if (enemy._gun != null)
                        enemy._gun._bullets.Clear();
                }
                _enemies.RemoveAll(enemy => enemy._destroy == true);
                _packet.WriteInt(MapManager._boxesToSend.Count);
                foreach (var box in MapManager._boxesToSend)
                {
                    MapManager._boxes[box].UpdatePacket(_packet);
                    MapManager._boxes.Remove(box);
                }
                MapManager._boxesToSend.Clear();
                foreach (var socket in _socket_list)
                {
                    if (socket.Connected)
                        socket.Send(_packet.Data());
                }
            }
            //_timer_long += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (_timer_long >= 1.5f)
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
            for (int i = 0; i < numOfPlayer; i++)
            {
                _packetHandlers[i].Update();
            }

        }

        private void Accept()
        {
            _socketServer.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult result)
        {
            Socket client_socket = _socketServer.EndAccept(result);
            _socketToAdd.Add(client_socket);
            addPlayers++;
            Accept();

        }
        private void Receive(Socket client_socket, PacketHandlerServer packetHandlerServer, byte[] buffer)
        {
            client_socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallBack, Tuple.Create(client_socket, packetHandlerServer, buffer));
        }
        private void ReceivedCallBack(IAsyncResult result)
        {
            Tuple<Socket, PacketHandlerServer, byte[]> state = (Tuple<Socket, PacketHandlerServer, byte[]>)result.AsyncState;
            Socket client_socket = state.Item1;
            PacketHandlerServer packetHandlerServer = state.Item2;
            byte[] buffer = state.Item3;
            int buffer_size = client_socket.EndReceive(result);
            packetHandlerServer.Handle(buffer);
            Receive(client_socket, packetHandlerServer, buffer);
        }
    }
}
