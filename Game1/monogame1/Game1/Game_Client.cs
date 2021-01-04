using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameClient
{
    public class Game_Client : Game
    {
        public static bool _updateOtherPlayerTexture = false;
        public static GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<OtherPlayer> _other_players;
        private Player _player;
        private List<Simple_Enemy> _enemies;
        private Texture2D _line_texture;
        private EnemyManager _enemyManager;
        private Tile[,] _tiles;
        private TileManager _tileManager;
        private PlayerManager _playerManager;
        public CollectionManager _collectionManager;
        private NetworkManagerClient _networkManager;
        private InventoryManager _inventoryManager;
        float _timer = 0;
        #region Important Functions
        public Game_Client()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            PlayerIndex.One.GetType();
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //graphics.PreferredBackBufferWidth = 1000;  // set this value to the desired width of your window
            //graphics.PreferredBackBufferHeight = 1000;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _other_players = new List<OtherPlayer>();
            _enemies = new List<Simple_Enemy>();
            _collectionManager = new CollectionManager(_enemies, Content);
            _playerManager = new PlayerManager(_other_players, Content, GraphicsDevice, _enemies, _collectionManager);
            _enemyManager = new EnemyManager(_other_players, Content, GraphicsDevice, _enemies);
            _tileManager = new TileManager(_tiles, GraphicsDevice, Content);
            _player = _playerManager.AddPlayer();
            _networkManager = new NetworkManagerClient(_other_players, _player, _playerManager);
            _networkManager.Initialize_connection();
            _tileManager.AddTowerTile(new Vector2(200,200));
            _inventoryManager = new InventoryManager(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var player in _other_players)
                player.Update(gameTime, _enemies);
            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime);
            }
            _enemies.RemoveAll(enemy => enemy._destroy == true);
            _enemyManager.Update(gameTime);
            _playerManager.Update(gameTime, _enemies);
            _networkManager.Update(gameTime);
            base.Update(gameTime);

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            _tileManager.Draw(_spriteBatch);
            _playerManager.Draw(_spriteBatch);
            _enemyManager.Draw(_spriteBatch);
            _inventoryManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
        #region etcFunctions
        public void DrawLine(Vector2 start, Vector2 end)
        {
            _line_texture = Content.Load<Texture2D>("etc/lineSprite");
            _spriteBatch.Draw(_line_texture, start, null, Color.White,
                             (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float)_line_texture.Height / 2),
                             new Vector2(Vector2.Distance(start, end), 0.005f),
                             SpriteEffects.None, 0f);

        }
        #endregion
        #region tiles

        #endregion

    }
}
