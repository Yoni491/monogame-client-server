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
        static List<Socket> _socket_list;
        private List<NetworkPlayer> _players;
        private List<SimpleEnemy> _enemies;
        Socket _socketServer;
        private List<byte> _bufferList;
        List<PacketHandlerServer> _packetHandlers, _packetHandlersToRemove;
        int numOfPlayer = 0;
        static int _playerIDNumber=0;
        int addPlayers = 0, removePlayers = 0;
        List<Socket> _socketToAdd,_socketToRemove;
        Packet _packet;
        static bool _everyClientGotCurrentLevel;
        LevelManager _levelManager;
        Game_Server _gameServer;
        ServerScreen _serverScreen;
        bool justResetted;
        public static bool _sendNames;
        public static bool _Connected;
        public NetworkManagerServer(Game_Server gameServer, ServerScreen serverScreen, List<Socket> socket_list, List<NetworkPlayer> players, List<SimpleEnemy> enemies, LevelManager levelManager)
        {
            _gameServer = gameServer;
            _serverScreen = serverScreen;
            _levelManager = levelManager;
            _socket_list = socket_list;
            _players = players;
            _enemies = enemies;
            _packetHandlers = new List<PacketHandlerServer>();
            _packetHandlersToRemove = new List<PacketHandlerServer>();
            _socketToAdd = new List<Socket>();
            _socketToRemove = new List<Socket>();
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
        public void Reset()
        {
            _packetHandlers.Clear();
            _socket_list.Clear();
        }

        public void Update(GameTime gameTime)
        {
            RemovePlayerSocket();
            AddPlayerSocket();
            if(numOfPlayer==0 && !justResetted)
            {
                _gameServer.ResetGame(true);
                justResetted = true;
            }
            _timer_short += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer_short >= 0.1f)
            {
                _timer_short = 0;
                if(_sendNames)
                {
                    SendPacket(3, true);
                    _sendNames = false;
                }
                else
                {
                    SendPacket(1,false);
                }
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
        public void RemoveSocket(Socket client_socket, PacketHandlerServer packetHandlerServer)
        {
            _socketToRemove.Add(client_socket);
            _packetHandlersToRemove.Add(packetHandlerServer);
            removePlayers++;
        }
        public void RemovePlayerSocket()
        {
            int tempPlayers = removePlayers;
            for (int i = 0; i < tempPlayers; i++)
            {
                PacketHandlerServer packetHandlerServer = _packetHandlersToRemove[0];
                _players.Remove(packetHandlerServer._player);
                numOfPlayer--;
                _packetHandlers.Remove(packetHandlerServer);
                removePlayers--;
                Socket socket = _socketToRemove[0];
                _socket_list.Remove(socket);
                socket.Close();
                _socketToRemove.RemoveAt(0);
                _packetHandlersToRemove.RemoveAt(0);
            }
        }
        public void AddPlayerSocket()
        {
            int tempPlayers = addPlayers;
            for (int i = 0; i < tempPlayers; i++)
            {
                addPlayers--;
                Socket socket = _socketToAdd[0];
                _socket_list.Add(socket);
                _socketToAdd.RemoveAt(0);
                byte[] buffer = new byte[10000];
                NetworkPlayer player = new NetworkPlayer(Vector2.Zero,CollectionManager._playerAnimationManager[1],
                    100, _playerIDNumber++, null, new NameDisplay(null,""));
                _players.Add(player);
                PacketHandlerServer packetHandler = new PacketHandlerServer(_players, player, _enemies);
                _packetHandlers.Add(packetHandler);
                if (numOfPlayer == 0)
                {
                    if(string.IsNullOrEmpty( ServerScreen._startingLevelTextBox._text))
                        _levelManager.LoadNewLevel(LevelManager.startingLevelServer);
                    else
                        _levelManager.LoadNewLevel(Int32.Parse(ServerScreen._startingLevelTextBox._text));
                }
                SendPacket(2, true, player._playerNum,socket);
                WriteItems(true);
                socket.Send(_packet.Data());
                Receive(socket, packetHandler, buffer);
                numOfPlayer++;
                justResetted = false;
                _serverScreen.UpdateMessage("Connected!");
                _Connected = true;
            }
        }
        public void WritePlayers(bool writeNames = false)
        {
            _packet.WriteInt(_players.Count);
            foreach (var player in _players)
            {
                player.UpdatePacketShort(_packet,writeNames);
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
            if(sendAll)
            {
                _packet.WriteInt(MapManager._boxesDestroyed.Count);
                foreach (var box in MapManager._boxesDestroyed)
                {
                    _packet.WriteInt(box);
                }
            }
            else
            {
                _packet.WriteInt(MapManager._boxesToSend.Count);
                foreach (var box in MapManager._boxesToSend)
                {
                    if (MapManager._boxes.ContainsKey(box))
                        MapManager._boxes[box].UpdatePacket(_packet);
                    MapManager._boxes.Remove(box);
                }
            }
        }
        public void WriteDoors(bool sendAll)
        {
            if (sendAll)
            {
                _packet.WriteInt(MapManager._doorsDestroyed.Count);
                foreach (var door in MapManager._doorsDestroyed)
                {
                    _packet.WriteInt(door);
                }
            }
            else
            {
                _packet.WriteInt(MapManager._doorsToSend.Count);
                foreach (var door in MapManager._doorsToSend)
                {
                    if (MapManager._doors.ContainsKey(door))
                        MapManager._doors[door].UpdatePacket(_packet);
                    MapManager._doors.Remove(door);

                }
            }
        }
        public void WriteChests(bool sendAll)
        {
            if(sendAll)
            {
                _packet.WriteInt(MapManager._chestsDestroyed.Count);
                foreach (var chest in MapManager._chestsDestroyed)
                {
                    _packet.WriteInt(chest);
                }
            }
            else
            {
                _packet.WriteInt(MapManager._chestsToSend.Count);
                foreach (var chest in MapManager._chestsToSend)
                {
                    if (MapManager._chests.ContainsKey(chest))
                        MapManager._chests[chest].UpdatePacket(_packet);
                    MapManager._chests.Remove(chest);
                }
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
                _packet.WriteInt(ItemManager._itemsToSendPicked.Count);
                foreach (var item in ItemManager._itemsToSendPicked)
                {
                    if(ItemManager._itemsOnTheGround.ContainsKey(item))
                        ItemManager._itemsOnTheGround[item].UpdatePacket(_packet);
                }
            }
        }
        public void WriteItemsPickedUp()
        {
            _packet.WriteInt(ItemManager._itemsPickedUpToSend_Server.Count);
            foreach (var item in ItemManager._itemsPickedUpToSend_Server)
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
        public void SendPacket(int type, bool sendEverything, int playerNum = 0,Socket clientSocket = null)
        {
            _packet.UpdateType((ushort)type);
            if (type == 2)
                _packet.WriteInt(playerNum);
            WriteLevel();
            WritePlayers(type == 3 || type == 2);
            WriteEnemies();
            _enemies.RemoveAll(enemy => enemy._destroy);
            WriteBoxes(sendEverything);
            MapManager._boxesToSend.Clear();
            WriteDoors(sendEverything);
            MapManager._doorsToSend.Clear();
            WriteChests(sendEverything);
            MapManager._chestsToSend.Clear();
            WriteItems(sendEverything);
            ItemManager._itemsToSendPicked.Clear();
            WriteItemsPickedUp();
            ItemManager._itemsPickedUpToSend_Server.Clear();
            try
            {
                if (clientSocket != null)
                {
                    byte[] buffer = _packet.Data();
                    clientSocket.Send(buffer);
                }
                else
                {
                    foreach (var socket in _socket_list)
                    {
                        byte[] buffer = _packet.Data();
                        socket.Send(buffer);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error: packet not sent");
                return;
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
            int buffer_size;
            Tuple<Socket, PacketHandlerServer, byte[]> state = (Tuple<Socket, PacketHandlerServer, byte[]>)result.AsyncState;
            Socket client_socket = state.Item1;
            PacketHandlerServer packetHandlerServer = state.Item2;
            byte[] buffer = state.Item3;
            try
            {
                buffer_size = client_socket.EndReceive(result);
            }
            catch
            {
                RemoveSocket(client_socket, packetHandlerServer);
                return;
            }
            if(!client_socket.Connected)
            {
                RemoveSocket(client_socket, packetHandlerServer);
            }
            packetHandlerServer.Handle(buffer);
            try
            {
                Receive(client_socket, packetHandlerServer, buffer);

            }
            catch
            {
                Console.WriteLine("Error in receive");
            }
        }
        #endregion
    }
}
