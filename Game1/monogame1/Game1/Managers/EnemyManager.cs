using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameClient
{
    public class EnemyManager
    {
        private List<OtherPlayer> _players;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;
        private List<Simple_Enemy> _enemies;
        private Texture2D _bullet_texture;
        private float _random_enemies_circle_timer = 0;
        public EnemyManager(List<OtherPlayer> players, ContentManager contentManager, GraphicsDevice graphicsDevice, List<Simple_Enemy> enemies)
        {
            _players = players;
            _enemies = enemies;
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;

        }
        public void AddEnemy(Vector2 position)
        {

            Texture2D texture = _contentManager.Load<Texture2D>("Patreon sprites 1/1");
            //AddEnemySprite(texture, position);
        }
        //public void AddEnemySprite(Texture2D i_texture, Vector2 i_position)
        //{
        //    _enemies.Add(new Simple_Enemy(new Dictionary<string, Animation>()
        //    {
        //    { "WalkDown", new Animation(SpriteManager.Resize4x4Sprite(i_texture,0,_graphicsDevice), 4) },
        //    { "WalkLeft", new Animation(SpriteManager.Resize4x4Sprite(i_texture,1,_graphicsDevice), 4) },
        //    { "WalkRight", new Animation(SpriteManager.Resize4x4Sprite(i_texture,2,_graphicsDevice), 4) },
        //    { "WalkUp", new Animation(SpriteManager.Resize4x4Sprite(i_texture,3,_graphicsDevice), 4) },
        //    },
        //    i_position,
        //    _players,
        //    100
        //    ));
        //}

        public void AddEnemiesRandomCircle()
        {

            Vector2 center = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2, _graphicsDevice.Viewport.Bounds.Height / 2);
            Random x = new Random();
            Vector2 random_direction = Vector2.Normalize(new Vector2(x.Next(-10, 10), x.Next(-10, 10))) * 400;
            AddEnemy(center + random_direction);
        }

        public void Update(GameTime gameTime)
        {
            if ((_random_enemies_circle_timer += (float)gameTime.ElapsedGameTime.TotalSeconds) >= 2f)
            {
                _random_enemies_circle_timer = 0;
                AddEnemiesRandomCircle();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var sprite in _enemies)
                sprite.Draw(spriteBatch);
        }
    }
}
