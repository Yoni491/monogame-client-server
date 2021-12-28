using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class Grave
    {
        private readonly Vector2 _position;
        private readonly Rectangle _rectangle;
        private readonly bool _spawnAtTheStart;
        private int spawnDistance = 150;
        public bool _destroy;

        public Grave(Rectangle rectangle, bool spawnAtTheStart)
        {
            _position = new Vector2(rectangle.X, rectangle.Y + TileManager._map.TileHeight);
            _rectangle = rectangle;
            _spawnAtTheStart = spawnAtTheStart;
        }
        public void Update(Rectangle player_position_rectangle)
        {
            Vector2 player_position = new Vector2(player_position_rectangle.X, player_position_rectangle.Y);
            if (Vector2.Distance(player_position, _position) <= spawnDistance || _spawnAtTheStart)
            {
                if (!CollisionManager.isCollidingBoxes(_rectangle, Vector2.Zero, 0))
                {
                    EnemyManager.AddEnemyAtPosition(1, _position, true, false);
                    _destroy = true;
                }
            }
        }
    }
}
