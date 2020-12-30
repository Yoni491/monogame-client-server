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
    public class Game_Server : Game
    {
        public static GraphicsDeviceManager _graphics;
        //private SpriteBatch spriteBatch;
        private List<Player> _players;
        private List<Simple_Enemy> _enemies;
        //private Texture2D _line_texture;
        private EnemyManager _enemyManager;
        private Tile[,] _tiles;
        private TileManager tileManager;
        private PlayerManager playerManager;
        private int playerAmount = 0;
        PacketHandler _packetHandler = new PacketHandler();
        Socket _socket;
        byte[] _buffer = new byte[2000];
        static List<Socket> _socket_list = new List<Socket>();
        float _timer = 0;
        float _timer2 = 0;
        #region Important Functions
        public Game_Server()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            PlayerIndex.One.GetType();
        }
        protected override void Initialize()
        {
            _players = new List<Player>();
            _enemies = new List<Simple_Enemy>();
            Initialize_connection();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            playerManager = new PlayerManager(_players,Content,GraphicsDevice,_enemies);
            _enemyManager = new EnemyManager(_players, _enemies);
            tileManager = new TileManager(_tiles, GraphicsDevice,Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            foreach (var player in _players)
                player.Update(gameTime,_enemies);
            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime);
            }
            _enemies.RemoveAll(enemy => enemy._destroy == true);
            _enemyManager.Update(gameTime);
            base.Update(gameTime);
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 1f)
            {
                _timer = 0;
                PacketShort_Server packet1 = new PacketShort_Server(_players);
                packet1.UpdatePacket();
                foreach (var socket in _socket_list)
                {
                    if (socket.Connected)
                    {
                        _socket.Send(packet1.Data);
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
            //            _socket.Send(packet1.Data);
            //        }
            //    }
            //}


        }
        private void Initialize_connection()
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
            Player player = new Player(Vector2.Zero,100);
            playerAmount++;
            Accept();
            PacketStructure packet = new PacketStructure(10,3);
            packet.WriteInt(player._playerNum);
            _socket.Send(packet.Data);
            Receive(client_socket,player);
        }
        private void Receive(Socket client_socket,Player player)
        {
            client_socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None,ReceivedCallBack,Tuple.Create(client_socket,player));
        }
        private void ReceivedCallBack(IAsyncResult result)
        {
            Tuple<Socket, Player> state = (Tuple<Socket, Player>)result.AsyncState;
            Socket client_socket = state.Item1;
            Player player = state.Item2;
            int buffer_size = client_socket.EndReceive(result);
            _packetHandler.Handle(_buffer, client_socket, player);
            Receive(client_socket,player);
        }
        #endregion
        #region etcFunctions
        //public void DrawLine(Vector2 start, Vector2 end)
        //{
        //    _line_texture = Content.Load<Texture2D>("etc/lineSprite");
        //    spriteBatch.Draw(_line_texture, start, null, Color.White,
        //                     (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
        //                     new Vector2(0f, (float)_line_texture.Height / 2),
        //                     new Vector2(Vector2.Distance(start, end), 0.005f),
        //                     SpriteEffects.None, 0f);

        //}
        #endregion
        #region tiles

        #endregion

    }
}
