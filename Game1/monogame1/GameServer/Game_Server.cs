
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Net.Sockets;
using GameClient;
namespace GameServer
{
    public class Game_Server : Game
    {
        public static GraphicsDeviceManager _graphics;
        //private SpriteBatch spriteBatch;
        //private List<NetworkPlayer> _players;
        //private List<Simple_Enemy> _enemies;
        //private Texture2D _line_texture;
        //private EnemyManager _enemyManager;
        //private TileManager _tileManager;
        //private PlayerManager _playerManager;
        //private NetworkManagerServer _networkManager;
        //private CollectionManager _collectionManager;
        //private MapManager _mapManager;

        static List<Socket> _socket_list = new List<Socket>();
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
            base.Initialize();
        }
        protected override void LoadContent()
        {
            //_mapManager = new MapManager();
            //_collectionManager = new CollectionManager(_enemies, Content);
            //_players = new List<NetworkPlayer>();
            //_enemies = new List<Simple_Enemy>();
            ////spriteBatch = new SpriteBatch(GraphicsDevice);
            //_playerManager = new PlayerManager(_players,_collectionManager);
            //_enemyManager = new EnemyManager(_players, _enemies);
            //_tileManager = new TileManager(GraphicsDevice,Content,_mapManager);
            //_networkManager = new NetworkManagerServer(_socket_list, _players);
            //_networkManager.Initialize_connection();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //foreach (var player in _players)
            //    player.Update(gameTime, _enemies);
            //foreach (var enemy in _enemies)
            //{
            //    enemy.Update(gameTime);
            //}
            //_enemies.RemoveAll(enemy => enemy._destroy == true);
            //_enemyManager.Update(gameTime);
            base.Update(gameTime);
            //_networkManager.Update(gameTime);

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
