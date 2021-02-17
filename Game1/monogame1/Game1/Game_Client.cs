using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class Game_Client : Game
    {
        public static GraphicsDeviceManager _graphics;
        private MenuManager _menuManager;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _UIbatch;
        private SpriteBatch _settingsBatch;
        private List<NetworkPlayer> _network_players;
        private Player _player;
        private List<Simple_Enemy> _enemies;
        private EnemyManager _enemyManager;
        private TileManager _tileManager;
        public PlayerManager _playerManager;
        public CollectionManager _collectionManager;
        public NetworkManagerClient _networkManager;
        private InventoryManager _inventoryManager;
        private ItemManager _itemManager;
        private GraphicManager _graphicManager;
        private CollisionManager _collisionManager;
        private LevelManager _levelManager;
        private UIManager _UIManager;
        private MapManager _mapManager;
        private PathFindingManager _pathFindingManager;
        public bool _inMenu = true;
        public bool _IsMultiplayer = false;

        #region Important Functions
        public Game_Client()
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
            _graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _graphicManager = new GraphicManager(GraphicsDevice, Content,this,_graphics);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _UIbatch = new SpriteBatch(GraphicsDevice);
            _settingsBatch = new SpriteBatch(GraphicsDevice);
            _mapManager = new MapManager();
            _menuManager = new MenuManager(this, GraphicsDevice);
            _network_players = new List<NetworkPlayer>();
            _enemies = new List<Simple_Enemy>();
            _collisionManager = new CollisionManager();
            _collectionManager = new CollectionManager(_enemies, Content);
            _itemManager = new ItemManager(_collectionManager);
            _inventoryManager = new InventoryManager(GraphicsDevice, _itemManager);
            _UIManager = new UIManager();
            _playerManager = new PlayerManager(_network_players, _collectionManager);
            _enemyManager = new EnemyManager(GraphicsDevice, _enemies, _collectionManager);
            _pathFindingManager = new PathFindingManager();
            _tileManager = new TileManager(GraphicsDevice, Content, _mapManager);
            _networkManager = new NetworkManagerClient();
            _levelManager = new LevelManager(_tileManager);
            _collectionManager.Initialize(_playerManager, _itemManager);
            _player = _playerManager.AddPlayer(_itemManager, _inventoryManager, GraphicsDevice, _UIManager);
            _collisionManager.Initialize(_network_players, _player, _enemies);
            _levelManager.Initialize(_player);
            _inventoryManager.Initialize(_player);
            _mapManager.Initialize(_player);
            _UIManager.Initialize(Content, _inventoryManager, _graphics, _player,this);
            _networkManager.Initialize(_network_players, _player, _playerManager,this);


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (_inMenu)
            {
                _menuManager.Update(gameTime);
                _UIManager.Update(gameTime);
            }
            else
            {
                _enemyManager.Update(gameTime);
                _enemies.RemoveAll(enemy => enemy._destroy == true);
                if(_IsMultiplayer)
                    _networkManager.Update(gameTime);
                _UIManager.Update(gameTime);
                _playerManager.Update(gameTime, _enemies);
                _mapManager.Update();
                _levelManager.Update();
                _pathFindingManager.Update();
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
                _UIManager.Draw(_settingsBatch);
                _tileManager.Draw(_spriteBatch);
                _playerManager.Draw(_spriteBatch);
                _enemyManager.Draw(_spriteBatch);
                _itemManager.Draw(_spriteBatch);
                _inventoryManager.Draw(_UIbatch);
            }
            
            _spriteBatch.End();
            _UIbatch.End();
            _settingsBatch.End();
            base.Draw(gameTime);

        }

        public void ResetGraphics()
        {
            _inventoryManager.ResetGraphics();
            _UIManager.ResetGraphics();
            _menuManager.ResetGraphics();
        }
        #endregion
    }
}
