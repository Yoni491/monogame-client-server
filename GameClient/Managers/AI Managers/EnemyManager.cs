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
        static private List<SimpleEnemy> _enemies;

        public EnemyManager(GraphicsDevice graphicsDevice, List<SimpleEnemy> enemies, CollectionManager collectionManager)
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
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Update(gameTime);
            }
            if (!Game_Client._isMultiplayer && !Game_Client._isServer)
                _enemies.RemoveAll(enemy => enemy._destroy);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
            }

        }
        public static SimpleEnemy AddEnemyAtPosition(int EnemyID, Vector2 position, bool useAstar, bool waitForDestroyedWall)
        {
            SimpleEnemy enemy = _collectionManager.GetSimpleEnemyCopy(EnemyID, useAstar, waitForDestroyedWall);
            enemy.PositionFeetAt(position);
            _enemies.Add(enemy);
            return enemy;
        }
        public SimpleEnemy AddEnemyFromServer(int enemyNum, int enemyId)
        {
            SimpleEnemy simple_Enemy = _collectionManager.GetSimpleEnemyCopy(enemyId, true, false);
            simple_Enemy._enemyNum = enemyNum;
            _enemies.Add(simple_Enemy);
            return simple_Enemy;
        }
        //public void AddEnemiesRandomCircle()
        //{

        //    Vector2 center = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 2, _graphicsDevice.Viewport.Bounds.Height / 2);
        //    Random x = new Random();
        //    Vector2 random_direction = Vector2.Normalize(new Vector2(x.Next(-10, 10), x.Next(-10, 10))) * 400;
        //    AddEnemyAtPosition(center + random_direction);
        //}
        public static void Reset()
        {
            _enemies.Clear();
        }
    }
}
