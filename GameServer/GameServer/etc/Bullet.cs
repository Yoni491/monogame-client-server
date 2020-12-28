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
    public class Bullet
    {
        private float _speed = 5f;

        //public Texture2D _texture;

        Vector2 _position;

        private float _timer = 0;

        private Vector2 _direction;

        public bool _destroy = false;

        private Gun _gun;

        private List<Simple_Enemy> _enemies;

        //public Rectangle Rectangle
        //{
        //    get
        //    {
        //        return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        //    }
        //}

        public Bullet(Gun gun , Vector2 position, Vector2 direction ,List<Simple_Enemy> enemies)
        {
            _gun = gun;
            //_texture = texture;
            _position = position;
            _direction = Vector2.Normalize(direction);
            _enemies = enemies;
        }
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(_texture, _position, null, Color.White, 1, new Vector2(4, 12), 0.5f, SpriteEffects.FlipHorizontally, 1);             
        //}
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies)
        {
            _enemies = enemies;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 2f)
            {
                _destroy = true;
            }
            _position += _direction * _speed;
            //if(_enemies != null)
                //foreach (var enemy in _enemies)
                //{
                //    if(enemy.isCollision(this))
                //        _destroy = true;
                //}
        }
    }

}
