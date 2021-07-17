using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Net.Sockets;
using GameClient;
namespace GameServer
{
    public class Game_Server : Game
    {
        public static GraphicsDeviceManager _graphics;
        private List<NetworkPlayer> _networkPlayers;
        private List<SimpleEnemy> _enemies;
        private EnemyManager _enemyManager;
        private TileManager _tileManager;
        public PlayerManager _playerManager;
        public CollectionManager _collectionManager;
        public NetworkManagerServer _networkManager;
        private ItemManager _itemManager;
        private GraphicManager _graphicManager;
        private CollisionManager _collisionManager;
        private LevelManager _levelManager;
        private MapManager _mapManager;
        private PathFindingManager _pathFindingManager;
        static List<Socket> _socket_list = new List<Socket>();
        private BulletReachManager _bulletReachManager;
        private ProgressManager _progressManager;


        #region Important Functions
        public Game_Server()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //Window.AllowUserResizing = true;
            //Scene.SetDefaultDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            PlayerIndex.One.GetType();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _graphicManager = new GraphicManager(GraphicsDevice, Content,_graphics);

            _mapManager = new MapManager();
            _collectionManager = new CollectionManager();
            _itemManager = new ItemManager(_collectionManager);
            _tileManager = new TileManager(GraphicsDevice, Content, _mapManager);
            _levelManager = new LevelManager(_tileManager);
            
            _networkPlayers = new List<NetworkPlayer>();
            _enemies = new List<SimpleEnemy>();
            _playerManager = new PlayerManager(_networkPlayers, _collectionManager);
            _enemyManager = new EnemyManager(GraphicsDevice, _enemies, _collectionManager);
            _progressManager = new ProgressManager();


            _collisionManager = new CollisionManager();
            _pathFindingManager = new PathFindingManager();
            _bulletReachManager = new BulletReachManager();

            _networkManager = new NetworkManagerServer(_socket_list, _networkPlayers, _enemies,_levelManager);

            _bulletReachManager.Initialize(null, _networkPlayers);
            _collectionManager.Initialize(_enemies, Content, _playerManager, _itemManager);
            _collisionManager.Initialize(_networkPlayers, null, _enemies);
            _levelManager.Initialize(_networkPlayers,_progressManager);
            _mapManager.Initialize(_networkPlayers);
            _networkManager.Initialize_connection();
            _levelManager.LoadNewLevel(8);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _enemyManager.Update(gameTime);
            _networkManager.Update(gameTime);
            _playerManager.Update(gameTime);
            _mapManager.Update();
            _levelManager.Update();
            _bulletReachManager.Update();
            _pathFindingManager.Update();
            base.Update(gameTime);

        }
        #endregion
    }
}
