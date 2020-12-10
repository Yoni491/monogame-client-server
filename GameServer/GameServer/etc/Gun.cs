using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Gun
    {
        private Vector2 _position;

        protected Texture2D _texture;

        protected Vector2 _velocity;

        protected float _scale = 0.5f;

        protected Texture2D _bullet_texture;

        private List<Bullet> _bullets = new List<Bullet>();

        private Vector2 _direction;

        private List<Simple_Enemy> _enemies;

        public Vector2 Position { get => _position; set => _position = value; }

        public Gun(Texture2D texture, Vector2 position, Texture2D bullet_texture, List<Simple_Enemy> enemies)
        {
            _texture = texture;
            _bullet_texture = bullet_texture;
            _enemies = enemies;
        }
        public void Draw(SpriteBatch spriteBatch,Vector2 position)
        {
            _position = position + new Vector2(23,40);
            Program.game.DrawLine(_position +new Vector2(0.1f,0.1f), _position);
            
            float rotation = (float)Math.Atan2(_direction.Y, _direction.X);
            if (rotation > -Math.PI / 2 && rotation < Math.PI / 2)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, rotation, new Vector2(4, 12), _scale, SpriteEffects.FlipHorizontally, 1);
            }
            else
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, rotation, new Vector2(4, 20), _scale, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, 1);
            }
            foreach (var bullet in _bullets)
            {
                bullet.Draw(spriteBatch);
            }
            //TODO:for sniper:
            //Program.game.DrawLine(_position + Vector2.Normalize(_direction) * 38f, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
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
            _bullets.Add(new Bullet(this, _bullet_texture, _position + Vector2.Normalize(_direction) * 20f, _direction, _enemies));
        }
    }
}
