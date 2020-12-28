using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace GameServer
{
    public class EnemyManager
    {
        private List<Player> _players;
        //private ContentManager _contentManager;
        //private GraphicsDevice _graphicsDevice;
        private List<Simple_Enemy> _enemies;
        //private Texture2D _bullet_texture;
        private float _random_enemies_circle_timer = 0;
        public EnemyManager(List<Player> players, List<Simple_Enemy> enemies)
        {
            _players = players;
            _enemies = enemies;
            //_contentManager = contentManager;
            //_graphicsDevice = graphicsDevice;
            
        }
        public void AddEnemy(Vector2 position)
        {

            //Texture2D texture = _contentManager.Load<Texture2D>("Patreon sprites 1/1");
            AddEnemySprite(position);
        }
        public void AddEnemySprite(Vector2 i_position)
        {
            _enemies.Add(new Simple_Enemy(
            i_position,
            _players,
            100
            ));
        }

        public void AddEnemiesRandomCircle()
        {

            Vector2 center = new Vector2(250, 250);
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
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    foreach (var sprite in _enemies)
        //        sprite.Draw(spriteBatch);
        //}
    }
}
