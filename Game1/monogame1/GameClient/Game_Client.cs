using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace GameClient
{
    public class Game_Client : Game
    {
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _UIbatch;
        private SpriteBatch _settingsBatch;
        private List<NetworkPlayer> _networkPlayers;
        private GameOverScreen _gameOverScreen;
        private Player _player;
        private List<Simple_Enemy> _enemies;
        private EnemyManager _enemyManager;
        private TileManager _tileManager;
        public PlayerManager _playerManager;
        public CollectionManager _collectionManager;
        public NetworkManagerClient _networkManager;
        private ItemManager _itemManager;
        private CollisionManager _collisionManager;
        public LevelManager _levelManager;
        private MapManager _mapManager;
        private PathFindingManager _pathFindingManager;
        private AudioManager _audioManager;
        static private InventoryManager _inventoryManager;
        static private MainMenuManager _menuManager;
        static private UIManager _UIManager;
        private ProgressManager _progressManager;
        private BulletReachManager _bulletReachManager;
        static public bool _inMenu = true;
        static public bool _IsMultiplayer = false;
        static public bool _isServer = true;

        #region Important Functions
        public Game_Client()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //Window.AllowUserResizing = true;
            //Scene.SetDefaultDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            PlayerIndex.One.GetType();
            _isServer = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            //graphics
            new GraphicManager(GraphicsDevice, Content,_graphics);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _UIbatch = new SpriteBatch(GraphicsDevice);
            _settingsBatch = new SpriteBatch(GraphicsDevice);
            //menu and ui
            _menuManager = new MainMenuManager(this, GraphicsDevice);
            _UIManager = new UIManager();
            _inventoryManager = new InventoryManager(GraphicsDevice);
            _gameOverScreen = new GameOverScreen();
            //game content
            _audioManager = new AudioManager(Content);
            _mapManager = new MapManager();
            _tileManager = new TileManager(GraphicsDevice, Content, _mapManager);
            _levelManager = new LevelManager(_tileManager);
            _collectionManager = new CollectionManager();
            _itemManager = new ItemManager(_collectionManager);
            _progressManager = new ProgressManager();
            //players and enemies
            _networkPlayers = new List<NetworkPlayer>();
            _enemies = new List<Simple_Enemy>();
            _playerManager = new PlayerManager(_networkPlayers, _collectionManager);
            _enemyManager = new EnemyManager(GraphicsDevice, _enemies, _collectionManager);
            //calculations
            _collisionManager = new CollisionManager();
            _pathFindingManager = new PathFindingManager();
            _bulletReachManager = new BulletReachManager();
            //network
            _networkManager = new NetworkManagerClient();
            //initializers
            _collectionManager.Initialize(_enemies, Content,_playerManager, _itemManager);
            _player = _playerManager.AddPlayer(_itemManager, _inventoryManager, GraphicsDevice, _UIManager);
            _bulletReachManager.Initialize(_player, _networkPlayers);
            _collisionManager.Initialize(_networkPlayers, _player, _enemies);
            _levelManager.Initialize(_player,_progressManager);
            _inventoryManager.Initialize(_player,_itemManager);
            _mapManager.Initialize(_player);
            _UIManager.Initialize(Content, _inventoryManager, GraphicsDevice);
            _networkManager.Initialize(_networkPlayers, _player, _playerManager, _enemies, _enemyManager,_inventoryManager, _levelManager);
            _progressManager.Initialize(_player,_inventoryManager,_playerManager,_levelManager);
            _gameOverScreen.Initialize(Content, GraphicsDevice, _progressManager);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _UIManager.Update(gameTime);
            if (_inMenu)
            {
                _menuManager.Update(gameTime);
            }
            else
            {
                if (GameOverScreen._showScreen)
                {
                    Player player = null;
                    _gameOverScreen.Update(gameTime);
                    _player = player;
                }
                else
                {
                    if (_tileManager._levelLoaded)
                    {
                        _enemyManager.Update(gameTime);
                        _playerManager.Update(gameTime);
                        _mapManager.Update();
                        _levelManager.Update();
                        _bulletReachManager.Update();
                        _pathFindingManager.Update();
                    }
                }
                if (_IsMultiplayer)
                    _networkManager.Update(gameTime);
            }
            base.Update(gameTime);

        }
        protected override void Draw(GameTime gameTime)
        {
            
            _UIbatch.Begin(SpriteSortMode.FrontToBack);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack,transformMatrix: GraphicManager.GetSpriteBatchMatrix());
            _settingsBatch.Begin(SpriteSortMode.FrontToBack);
            if (_inMenu)
            {
                _menuManager.Draw(_UIbatch);
                _UIManager.Draw(_settingsBatch);
            }
            else
            {
                if (GameOverScreen._showScreen)
                {
                    _gameOverScreen.Draw(_UIbatch);
                }
                _UIManager.Draw(_settingsBatch);
                if (_tileManager._levelLoaded)
                {
                    _tileManager.Draw(_spriteBatch);
                    _playerManager.Draw(_spriteBatch);
                    _enemyManager.Draw(_spriteBatch);
                    _itemManager.Draw(_spriteBatch);
                    _inventoryManager.Draw(_UIbatch);
                    _mapManager.Draw(_UIbatch);
                }
            }           
            _spriteBatch.End();
            _UIbatch.End();
            _settingsBatch.End();
            base.Draw(gameTime);
        }

        static public void ResetGraphics()
        {
            _inventoryManager.ResetGraphics();
            _UIManager.ResetGraphics();
            _menuManager.ResetGraphics();
        }
        #endregion
    }
}
