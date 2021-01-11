using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GameClient
{
    public enum Direction { Up = 0, Down, Left, Right };
    public class Simple_Enemy
    {
        int _id;
        private float _speed;

        private Vector2 _velocity;

        private AnimationManager _animationManager;

        public Vector2 _position;

        private PlayerManager _playerManager;

        private HealthManager _health;

        private int []_items_drop_list;

        public bool _destroy = false;

        private ItemManager _itemManager;
        private MeleeWeapon _meleeWeapon;
        private bool _hide_weapon;
        private int _moving_direction;
        private float _scale;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X + 15, (int)_position.Y + 15, _animationManager.Animation._frameWidth / 4 - 30, _animationManager.Animation._frameHeight - 15);
            }
        }

        public Simple_Enemy(AnimationManager animationManager,int id,Vector2 position,float speed, PlayerManager playerManager,ItemManager itemManager, int health, int []items_drop_list, MeleeWeapon meleeWeapon)
        {
            _id = id;
            _animationManager = animationManager;
            _position = position;
            _animationManager._position = _position;
            _playerManager = playerManager;
            _items_drop_list = items_drop_list;
            _itemManager = itemManager;
            _speed = speed;
            _meleeWeapon = meleeWeapon;
            _scale = animationManager._scale;
            _meleeWeapon._holderScale = _scale;
            _health = new HealthManager(health, position + new Vector2(8, 10),_scale);
        }
        public void Update(GameTime gameTime)
        {
            Move();

            SetAnimations();

            _velocity = _velocity * _speed;

            _position += _velocity;

            _animationManager.Update(gameTime,_position);

            _meleeWeapon.Update(_moving_direction,gameTime,_position);

            _velocity = Vector2.Zero;

            _health.Update(_position);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            _health.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            if(_hide_weapon)
                _meleeWeapon.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) - 0.01f);
            else
                _meleeWeapon.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) + 0.01f);
        }
        public Simple_Enemy Copy(float scale)
        {

            return new Simple_Enemy(_animationManager.Copy(scale), _id, _position, _speed, _playerManager, _itemManager, _health._total_health, _items_drop_list, _meleeWeapon.Copy(0.7f));
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
            _moving_direction = -1;
            if (_velocity.X > Math.Abs(_velocity.Y))
            {
                _hide_weapon = false;
                _moving_direction = (int)Direction.Right;
            }
            else if (-_velocity.X > Math.Abs(_velocity.Y))
            {
                _hide_weapon = false;
                _moving_direction = (int)Direction.Left;
            }
            else if (_velocity.Y > 0)
            {
                _hide_weapon = false;
                _moving_direction = (int)Direction.Down;
            }
            else if (_velocity.Y < 0)
            {
                _hide_weapon = true;
                _moving_direction = (int)Direction.Up;
            }
            else _animationManager.Stop();
            if(_moving_direction != -1)
                _animationManager.Play(_moving_direction);
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

    }
}
