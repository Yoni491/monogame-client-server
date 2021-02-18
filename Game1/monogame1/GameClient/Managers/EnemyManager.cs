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
        static private CollectionManager _collectionManager;
        static private List<Simple_Enemy> _enemies;
        
        public EnemyManager( GraphicsDevice graphicsDevice, List<Simple_Enemy> enemies, CollectionManager collectionManager)
        {
            _enemies = enemies;
            _graphicsDevice = graphicsDevice;
            _collectionManager = collectionManager;

        }
        public void Update(GameTime gameTime)
        {
            //if ((_random_enemies_circle_timer += (float)gameTime.ElapsedGameTime.TotalSeconds) >= 2f)
            //{
            //    _random_enemies_circle_timer = -200;
            //    //AddEnemiesRandomCircle();
            //}
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
        public static void AddEnemyAtPosition(Vector2 position)
        {
            Simple_Enemy enemy = _collectionManager.GetSimpleEnemyCopy(1, 1f);
            enemy.PositionFeetAt(position);
            _enemies.Add(enemy);
        }
        public Simple_Enemy AddEnemyFromServer(int enemyNum,int enemyId)
        {
            Simple_Enemy simple_Enemy = _collectionManager.GetSimpleEnemyCopy(enemyId,1);
            simple_Enemy._enemyNum = enemyNum;
            _enemies.Add(simple_Enemy);
            return simple_Enemy;
        }
        public void AddEnemiesRandomCircle()
        {

            Vector2 center = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2, _graphicsDevice.Viewport.Bounds.Height / 2);
            Random x = new Random();
            Vector2 random_direction = Vector2.Normalize(new Vector2(x.Next(-10, 10), x.Next(-10, 10))) * 400;
            AddEnemyAtPosition(center + random_direction);
        }
        public static void Reset()
        {
            foreach (var enemy in _enemies)
            {
                enemy._destroy = true;
            }
        }
    }
}
