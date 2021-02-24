using GameClient;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
        static bool _everyClientGotCurrentLevel;
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
            AddPlayerSocket();
            _timer_short += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer_short >= 0.1f)
            {
                _timer_short = 0;
                SendPacket(1,false);
            }
            _everyClientGotCurrentLevel = true;
            for (int i = 0; i < numOfPlayer; i++)
            {
                if (_packetHandlers[i]._playerCurrentLevel != LevelManager._currentLevel)
                    _everyClientGotCurrentLevel = false;
                _packetHandlers[i].Update();
            }
            if (_everyClientGotCurrentLevel)
                LevelManager._sendNewLevel = false;
        }
        public void AddPlayerSocket()
        {
            int tempPlayers = addPlayers;
            for (int i = 0; i < tempPlayers; i++)
            {
                addPlayers -= tempPlayers;
                Socket socket = _socketToAdd[0];
                _socket_list.Add(socket);
                _socketToAdd.RemoveAt(0);
                byte[] buffer = new byte[10000];
                NetworkPlayer player = new NetworkPlayer(Vector2.Zero,CollectionManager._playerAnimationManager[1], 100, numOfPlayer, null);
                _players.Add(player);
                PacketHandlerServer packetHandler = new PacketHandlerServer(_players, player, _enemies);
                _packetHandlers.Add(packetHandler);
                numOfPlayer++;
                SendPacket(3, true, player._playerNum);
                
                WriteItems(true);
                socket.Send(_packet.Data());
                Receive(socket, packetHandler, buffer);
            }
        }
        public void WritePlayers()
        {
            _packet.WriteInt(_players.Count);
            foreach (var player in _players)
            {
                player.UpdatePacketShort(_packet);
                if (player._gun != null)
                    player._gun._bullets.Clear();
            }
        }
        public void WriteEnemies()
        {
            _packet.WriteInt(_enemies.Count);
            foreach (var enemy in _enemies)
            {
                enemy.UpdatePacketShort(_packet);
                if (enemy._gun != null)
                    enemy._gun._bullets.Clear();
            }
        }
        public void WriteBoxes(bool sendAll)
        {
            _packet.WriteInt(MapManager._boxesToSend.Count);
            foreach (var box in MapManager._boxesToSend)
            {
                MapManager._boxes[box].UpdatePacket(_packet);
                MapManager._boxes.Remove(box);
                
            }
        }
        public void WriteDoors(bool sendAll)
        {
            _packet.WriteInt(MapManager._doorsToSend.Count);
            foreach (var door in MapManager._doorsToSend)
            {
                MapManager._doors[door].UpdatePacket(_packet);
                MapManager._doors.Remove(door);

            }
        }
        public void WriteChests(bool sendAll)
        {
            _packet.WriteInt(MapManager._chestsToSend.Count);
            foreach (var chest in MapManager._chestsToSend)
            {
                MapManager._chests[chest].UpdatePacket(_packet);
                MapManager._chests.Remove(chest);

            }
        }
        public void WriteItems(bool sendAll)
        {
            if (sendAll)
            {
                _packet.WriteInt(ItemManager._itemsOnTheGround.Count);
                foreach (var item in ItemManager._itemsOnTheGround)
                {
                    item.Value.UpdatePacket(_packet);
                }
            }
            else
            {
                _packet.WriteInt(ItemManager._itemsToSend.Count);
                foreach (var item in ItemManager._itemsToSend)
                {
                    ItemManager._itemsOnTheGround[item].UpdatePacket(_packet);
                }
            }
        }
        public void WriteItemsPickedUp()
        {
            _packet.WriteInt(ItemManager._itemsPickedUpToSend.Count);
            foreach (var item in ItemManager._itemsPickedUpToSend)
            {
                _packet.WriteInt(item.Item1);//player num
                _packet.WriteInt(item.Item2);//item num
                ItemManager._itemsOnTheGround.Remove(item.Item2);
            }
        }
        public void WriteLevel()
        {
            _packet.WriteInt(LevelManager._currentLevel);
            _packet.WriteVector2(LevelManager._spawnPoint);
        }
        public void SendPacket(int type,bool sendEverything ,int playerNum = 0)
        {
            _packet.UpdateType((ushort)type);
            if(type == 3)
                _packet.WriteInt(playerNum);
            WriteLevel();
            WritePlayers();
            WriteEnemies();
            _enemies.RemoveAll(enemy => enemy._destroy == true);
            WriteBoxes(sendEverything);
            MapManager._boxesToSend.Clear();
            WriteDoors(sendEverything);
            MapManager._doorsToSend.Clear();
            WriteChests(sendEverything);
            MapManager._chestsToSend.Clear();
            WriteItems(sendEverything);
            ItemManager._itemsToSend.Clear();
            WriteItemsPickedUp();
            ItemManager._itemsPickedUpToSend.Clear();
            _packet.PrintData();
            foreach (var socket in _socket_list)
            {
                byte[] buffer = _packet.Data();
                socket.Send(buffer);
            }
        }

        #region socketMethods
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
        #endregion
    }
}
