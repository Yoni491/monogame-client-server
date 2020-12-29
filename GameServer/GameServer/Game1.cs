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
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        //private SpriteBatch spriteBatch;
        private List<Player> _players;
        private List<Simple_Enemy> _enemies;
        //private Texture2D _line_texture;
        private EnemyManager enemyManager;
        private Tile[,] _tiles;
        private TileManager tileManager;
        private PlayerManager playerManager;
        PacketHandler packetHandler = new PacketHandler();
        Socket socket;
        byte[] buffer = new byte[2000];
        static List<Socket> socket_list = new List<Socket>();
        #region Important Functions
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            enemyManager = new EnemyManager(_players, _enemies);
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
            enemyManager.Update(gameTime);
            base.Update(gameTime);
            
        }
        private void Initialize_connection()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(0, 1994));
            socket.Listen(0);
            Accept();

        }
        private void ConnectCallBack(IAsyncResult result)
        {
            if (socket.Connected)
            {
                Receive(socket);
            }
        }
        private void Accept()
        {
            socket.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult result)
        {
            Socket client_socket = socket.EndAccept(result);
            socket_list.Add(client_socket);
            Accept();
            Receive(client_socket);
        }
        private void Receive(Socket client_socket)
        {
            client_socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallBack, client_socket);
        }
        private void ReceivedCallBack(IAsyncResult result)
        {
            Socket client_socket = result.AsyncState as Socket;
            int buffer_size = client_socket.EndReceive(result);
            packetHandler.Handle(buffer, client_socket);
            Receive(client_socket);
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
