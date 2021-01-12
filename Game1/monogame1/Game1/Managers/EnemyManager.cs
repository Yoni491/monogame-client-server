using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameClient
{
    public class EnemyManager
    {
        private GraphicsDevice _graphicsDevice;
        private CollectionManager _collectionManager;
        private List<Simple_Enemy> _enemies;
        private float _random_enemies_circle_timer = 0;
        
        public EnemyManager( GraphicsDevice graphicsDevice, List<Simple_Enemy> enemies, CollectionManager collectionManager)
        {
            _enemies = enemies;
            _graphicsDevice = graphicsDevice;
            _collectionManager = collectionManager;

        }
        public void Update(GameTime gameTime)
        {
            if ((_random_enemies_circle_timer += (float)gameTime.ElapsedGameTime.TotalSeconds) >= 0.5f)
            {
                _random_enemies_circle_timer = 0;
                AddEnemiesRandomCircle();
            }
            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in _enemies)
                enemy.Draw(spriteBatch);

        }
        public void AddEnemy(Vector2 position)
        {
            Simple_Enemy enemy = _collectionManager.GetSimpleEnemyCopy(0,1f);
            enemy._position = position;
            _enemies.Add(enemy);
        }

        public void AddEnemiesRandomCircle()
        {

            Vector2 center = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2, _graphicsDevice.Viewport.Bounds.Height / 2);
            Random x = new Random();
            Vector2 random_direction = Vector2.Normalize(new Vector2(x.Next(-10, 10), x.Next(-10, 10))) * 400;
            AddEnemy(center + random_direction);
        }


    }
}
