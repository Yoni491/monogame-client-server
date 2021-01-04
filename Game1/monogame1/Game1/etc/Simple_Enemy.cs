using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GameClient
{
    public class Simple_Enemy
    {
        int _id;
        private float _speed;

        private Vector2 _velocity;

        private AnimationManager _animationManager;

        private Dictionary<string, Animation> _animations;

        public Vector2 _position;

        private PlayerManager _playerManager;

        private HealthManager _health;

        private int []_items_drop_list;

        public bool _destroy = false;

        private ItemManager _itemManager;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X + 15, (int)_position.Y + 15, _animationManager.Animation.Texture.Width / 4 - 30, _animationManager.Animation.Texture.Height - 15);
            }
        }

        public Simple_Enemy(Dictionary<string, Animation> i_animations, int id,Vector2 position,float speed, PlayerManager playerManager,ItemManager itemManager, int health, int []items_drop_list)
        {
            _id = 0;
            _animations = i_animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            _position = position;
            _animationManager.Position = _position;
            _playerManager = playerManager;
            _health = new HealthManager(health, position + new Vector2(8, 10));
            _items_drop_list = items_drop_list;
            _itemManager = itemManager;
            _speed = speed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            _health.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
        }

        public void Move()
        {
            Vector2 closest_player = _playerManager.getClosestPlayerToPosition(_position);
            if (Vector2.Distance(closest_player,_position) > 40)
                _velocity = Vector2.Normalize(closest_player - _position);
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
                    _health._health_left -= other._dmg;
                    if (_health._health_left <= 0)
                    {
                        _destroy = true;
                        _itemManager.DropItem(_items_drop_list, _position);
                    }
                    return true;
                }


            return false;
        }
        public Simple_Enemy Copy()
        {
            return new Simple_Enemy(_animations, _id, _position,_speed, _playerManager, _itemManager, _health._total_health, _items_drop_list);
        }
    }
}
