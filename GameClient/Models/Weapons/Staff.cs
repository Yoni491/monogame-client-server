using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace GameClient
{
    public class Staff
    {
        public int _id;
        private Vector2 _position;
        private int _direction;
        private float _swing_range;
        private Texture2D _texture;
        public bool _swing_weapon;
        public float _holderScale;
        private float _swing_timer;
        private float _swing_frame_window = 0.02f;
        private float _swing_frame_timer = 0;
        private float _between_attacks_timer;
        private float _between_attacks_timer_window = 0.2f;
        private float swingSpeed = 7;
        private int _dmg;
        public int _maxAttackingDistance = 30;
        private bool _isColided = false;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)(_texture.Width * _holderScale), (int)(_texture.Height * _holderScale));
            }
        }
        public Vector2 Position { get => _position; set => _position = value; }
        public Staff(int id, Texture2D texture, Vector2 position, float swing_range, int dmg)
        {
            _id = id;
            _texture = texture;
            _position = position;
            _swing_range = swing_range;
            _dmg = dmg;
        }
        public void Update(int direction, GameTime gameTime, Vector2 position)
        {
            if (_swing_weapon)
            {
                SwingUpdate(gameTime);
                _swing_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!_isColided && CollisionManager.isColidedWithPlayers(Rectangle, Vector2.Zero, _dmg))
                {
                    _isColided = true;
                }
            }
            else
            {
                _position = position + new Vector2(23, 44) * _holderScale;
                _between_attacks_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (direction != -1)
                _direction = direction;
            if (_swing_timer >= 0.3f)
            {
                _swing_timer = 0;
                _swing_weapon = false;
                _position = position + new Vector2(23, 44) * _holderScale;
                _isColided = false;
                _between_attacks_timer = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch, float layer)
        {
            if (_direction == (int)Direction.Up)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(4, 12), _holderScale * 0.5f, SpriteEffects.None, layer);
            }
            else if (_direction == (int)Direction.Down)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(-12, 12), _holderScale * 0.5f, SpriteEffects.FlipHorizontally, layer);
            }
            else if (_direction == (int)Direction.Right)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(4, 12), _holderScale * 0.5f, SpriteEffects.FlipHorizontally, layer);
            }
            else if (_direction == (int)Direction.Left)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(24, 12), _holderScale * 0.5f, SpriteEffects.None, layer);
            }
        }
        public Staff Copy(float scale)
        {
            return new Staff(_id, _texture, _position, _swing_range, _dmg);
        }
        public void SwingWeapon()
        {
            if (_between_attacks_timer > _between_attacks_timer_window)
            {
                _swing_weapon = true;
            }
        }
        private void SwingUpdate(GameTime gameTime)
        {
            _swing_frame_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_swing_frame_timer > _swing_frame_window)
            {
                _swing_frame_timer = 0;
                if (_direction == (int)Direction.Up)
                {
                    _position += new Vector2(0, -swingSpeed);
                }
                else if (_direction == (int)Direction.Down)
                {
                    _position += new Vector2(0, swingSpeed);
                }
                else if (_direction == (int)Direction.Right)
                {
                    _position += new Vector2(swingSpeed, 0);
                }
                else if (_direction == (int)Direction.Left)
                {
                    _position += new Vector2(-swingSpeed, 0);
                }
            }
        }
        //public void UpdatePacketShort(PacketStructure packet)
        //{
        //    foreach (var bullet in _bullets)
        //    {
        //        bullet.UpdatePacketShort(packet);
        //    }
        //}
        //public void ReadPacketShort(PacketStructure packet)
        //{
        //    int bulletAmount = packet.ReadInt();
        //    for (int i = 0; i < bulletAmount; i++)
        //    {
        //        int bullet_num = packet.ReadInt();
        //        Bullet bullet = _bullets.Find(bullet => bullet._bulletNumber == bullet_num);
        //        //if (bullet != null)
        //        //{
        //        //    bullet.readPacketShort(packet);
        //        //}
        //        //else
        //        //{
        //        //    //_bullets.Add(new Bullet(_bullet._collection_id, this, _bullet._texture, packet.ReadVector2(), packet.ReadVector2(), _enemies, _bullet._speed, bullet_num, _bullet._shootingTimer, _bullet._dmg));
        //        //}
        //    }
        //}
        //public void Shot()
        //{
        //    Bullet bullet = new Bullet(_bullet._collection_id, this, _bullet._texture, _position + Vector2.Normalize(_direction) * 20f, _direction, _enemies, _bullet._speed, -1, _bullet._shootingTimer, _bullet._dmg);
        //    _bullets.Add(bullet);
        //}
    }
}
