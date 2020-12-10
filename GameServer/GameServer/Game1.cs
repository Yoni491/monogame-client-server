using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Game1
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private List<Player> _players;
        private List<Simple_Enemy> _enemies;
        private Texture2D _line_texture;
        private EnemyManager enemyManager;
        private Tile[,] _tiles;
        private TileManager tileManager;
        private PlayerManager playerManager;
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
            // TODO: Add your initialization logic here
            //graphics.PreferredBackBufferWidth = 1000;  // set this value to the desired width of your window
            //graphics.PreferredBackBufferHeight = 1000;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            
            _players = new List<Player>();
            _enemies = new List<Simple_Enemy>();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerManager = new PlayerManager(_players,Content,GraphicsDevice,_enemies);
            enemyManager = new EnemyManager(_players, Content, GraphicsDevice, _enemies);
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



            

            // If there a controller attached, handle it
            
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            tileManager.Draw(spriteBatch);
            playerManager.Draw(spriteBatch);
            enemyManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
        #region etcFunctions
        public void DrawLine(Vector2 start, Vector2 end)
        {
            _line_texture = Content.Load<Texture2D>("etc/lineSprite");
            spriteBatch.Draw(_line_texture, start, null, Color.White,
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
