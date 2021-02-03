using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class Grave
    {
        private readonly Vector2 _position;
        private int spawnDistance = 150;
        public bool _destroy;

        public Grave(Rectangle rectangle)
        {
            _position = new Vector2(rectangle.X, rectangle.Y);
        }
        public void Update(Rectangle player_position_rectangle)
        {
            Vector2 player_position = new Vector2(player_position_rectangle.X, player_position_rectangle.Y);
            if (Vector2.Distance(player_position, _position) <= 150)
            {
                EnemyManager.AddEnemyAtPosition(_position);
                _destroy = true;
            }
        }
    }
}
