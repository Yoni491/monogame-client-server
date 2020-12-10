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
    public class Simple_Enemy
    {
        private float _speed = 1f;

        private Vector2 _velocity;

        private AnimationManager _animationManager;

        private Dictionary<string, Animation> _animations;

        private Vector2 _position;

        private List<Player> _players;

        private HealthManager _health;

        public bool _destroy = false;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X + 15, (int)_position.Y + 15, _animationManager.Animation.Texture.Width / 4 - 30, _animationManager.Animation.Texture.Height - 15);
            }
        }

        public Simple_Enemy(Dictionary<string, Animation> i_animations, Vector2 position, List<Player> players, int health)
        {
            _animations = i_animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            _position = position;
            _animationManager.Position = _position;
            _players = players;
            _health = new HealthManager(health, position + new Vector2(8, 10));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch);
            _health.Draw(spriteBatch);
        }

        public void Move()
        {
            float closest_object_distance = float.MaxValue;
            if(_players!=null)
                foreach (var player in _players)
                {
                    if (Vector2.Distance(_position, player.Position) < closest_object_distance)
                    {
                        closest_object_distance = Vector2.Distance(_position, player.Position);
                        _velocity = player.Position - _position;
                    }
                }
            if (closest_object_distance > 35)
                _velocity = Vector2.Normalize(_velocity);
            else
                _velocity = new Vector2(0, 0);
        }
        protected void SetAnimations()
        {
            if (_velocity.X > Math.Abs(_velocity.Y))
            {
                _animationManager.Play(_animations["WalkRight"]);
            }
            else if (-_velocity.X > Math.Abs(_velocity.Y))
            {
                _animationManager.Play(_animations["WalkLeft"]);
            }
            else if (_velocity.Y > 0)
            {
                _animationManager.Play(_animations["WalkDown"]);
            }
            else if (_velocity.Y < 0)
            {
                _animationManager.Play(_animations["WalkUp"]);
            }
            else _animationManager.Stop();
        }



        public void Update(GameTime gameTime)
        {
            Move();

            SetAnimations();

            _velocity = _velocity * _speed;

            _position += _velocity;

            _animationManager.Position = _position;

            _animationManager.Update(gameTime);

            _velocity = Vector2.Zero;

            _health._position = _position + new Vector2(8, 10);
        }
        public bool isCollision(Bullet other)
        {
            if (other.Rectangle.X > Rectangle.X && other.Rectangle.X < Rectangle.X + Rectangle.Width)
                if (other.Rectangle.Y > Rectangle.Y && other.Rectangle.Y < Rectangle.Y + Rectangle.Height)
                {
                    _health._health_left -= 1;
                    if (_health._health_left <= 0)
                        _destroy = true;
                    return true;
                }


            return false;
        }

    }
}
