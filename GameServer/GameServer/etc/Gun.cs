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
    public class Gun
    {
        private Vector2 _position;

        //protected Texture2D _texture;

        protected Vector2 _velocity;

        protected float _scale = 0.5f;

        //protected Texture2D _bullet_texture;

        private List<Bullet> _bullets = new List<Bullet>();

        private Vector2 _direction;

        private List<Simple_Enemy> _enemies;

        public Vector2 Position { get => _position; set => _position = value; }

        public Gun(Vector2 position, List<Simple_Enemy> enemies)
        {
            _enemies = enemies;
        }
        public void Update(GameTime gameTime,List<Simple_Enemy> enemies,Vector2 direction)
        {
            _direction = direction;
            _enemies = enemies;
            foreach (var bullet in _bullets)
            {
                bullet.Update(gameTime,_enemies);
            }
            _bullets.RemoveAll(bullet => bullet._destroy==true);
        }
        public void Shot()
        {
            _bullets.Add(new Bullet(this, _position + Vector2.Normalize(_direction) * 20f, _direction, _enemies));
        }
    }
}
