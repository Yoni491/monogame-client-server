using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Player
    {
        private Gun _gun;

        private float _speed = 2f;

        private Vector2 _velocity;

        private Vector2 _position;

        private HealthManager _health;

        private Vector2 _looking_direction;

        public Vector2 Position { get => _position; set => _position = value; }

        public Player(Vector2 position , int health)
        {
            _position = position;
            _health = new HealthManager(health,position +new Vector2(8,10));
            _velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime,List<Simple_Enemy> enemies)
        {
            _position += _velocity;

            if (_gun != null)
            {
                _gun.Update(gameTime, enemies,_looking_direction);
            }
            _velocity = Vector2.Zero;

            _health._position = _position + new Vector2(8, 10);

        }

    }
}

