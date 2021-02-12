using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameClient
{
    public class Gun
    {
        public int _id;
        private Vector2 _position;
        private Texture2D _texture;
        public List<Bullet> _bullets = new List<Bullet>();
        private Vector2 _direction;
        private List<Simple_Enemy> _enemies;
        public Bullet _bullet;
        public Vector2 Position { get => _position; set => _position = value; }
        public bool _isSniper;
        private bool _isGamePad;
        private bool _showLine;
        private float _spread;
        public float _holderScale = 0;
        private float _shooting_timer = 0;
        private bool _hitPlayers;
        private Vector2 _MaxPointBulletReach;
        private Vector2 _tipOfTheGun;

        #region meleeAttackVariables
        private int _moving_direction_int;
        private float _between_attacks_timer;
        private float _between_attacks_timer_window = 0.2f;
        private float _swing_timer;
        private float _swing_frame_window = 0.01f;
        private float _swing_frame_timer = 0;
        private float swingSpeed = 14;
        private bool _isColided = false;
        private float _swing_range = 32;
        public bool _swing_weapon;
        #endregion


        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y-4, (int)(_texture.Width * _holderScale * 0.4f), (int)(_texture.Height * _holderScale * 0.4f));
            }
        }

        public Gun(int id, Texture2D texture, Vector2 position, List<Simple_Enemy> enemies, Bullet bullet, bool isSniper, float spread,bool hitPlayers)
        {
            _id = id;
            _texture = texture;
            _enemies = enemies;
            _bullet = bullet;
            _position = position;
            _isSniper = isSniper;
            _spread = spread;
            _hitPlayers = hitPlayers;
        }
        public void Update(GameTime gameTime, Vector2 direction,int moving_direction, bool isGamePad,bool showLine, Vector2 position)
        {
            MelleAttackUpdate(gameTime, position);
            _moving_direction_int = moving_direction;
            _direction = Vector2.Normalize(direction);
            foreach (var bullet in _bullets)
            {
                bullet.Update(gameTime);
            }
            _bullets.RemoveAll(bullet => bullet._destroy == true);
            _isGamePad = isGamePad;
            _shooting_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _showLine = showLine;
            _tipOfTheGun =_position + Vector2.Normalize(_direction) * _texture.Width / 2 +new Vector2(0, 5);
        }
        public void MelleAttackUpdate(GameTime gameTime, Vector2 position)
        {
            if (_swing_weapon)
            {
                SwingUpdate(gameTime);
                _swing_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_hitPlayers)
                {
                    if (!_isColided && CollisionManager.isColidedWithPlayer(Rectangle,Vector2.Zero, 5))
                    {
                        _isColided = true;
                    }
                }
                else
                {
                    if (!_isColided && CollisionManager.isColidedWithEnemies(Rectangle, Vector2.Zero, 5))
                    {
                        _isColided = true;
                    }
                    else if(!_isColided && CollisionManager.isCollidingChests(Rectangle, Vector2.Zero))
                    {
                        _isColided = true;
                    }
                    else if (!_isColided && CollisionManager.isCollidingBoxes(Rectangle, Vector2.Zero))
                    {

                    }
                }
            }
            else
            {
                _position = position + new Vector2(23, 40) * _holderScale;
                _between_attacks_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (_swing_timer >= 0.1f)
            {
                _swing_timer = 0;
                _swing_weapon = false;
                _position = position + new Vector2(23, 44) * _holderScale;
                _isColided = false;
                _between_attacks_timer = 0;
            }
        }
        public void DrawSwing(SpriteBatch spriteBatch, float layer)
        {
            if (_moving_direction_int == (int)Direction.Up)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(4, 12), _holderScale * 0.5f, SpriteEffects.None, layer);
            }
            else if (_moving_direction_int == (int)Direction.Down)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(-12, 12), _holderScale * 0.5f, SpriteEffects.FlipHorizontally, layer);
            }
            else if (_moving_direction_int == (int)Direction.Right)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(4, 12), _holderScale * 0.5f, SpriteEffects.FlipHorizontally, layer);
            }
            else if (_moving_direction_int == (int)Direction.Left)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(24, 12), _holderScale * 0.5f, SpriteEffects.None, layer);
            }
        }
        public void Draw(SpriteBatch spriteBatch,  float layer)
        {
            //GraphicManager.DrawRectangle(spriteBatch, Rectangle, layer);
            if (_swing_weapon)
                DrawSwing(spriteBatch, layer);
            else
            {
                float rotation = (float)Math.Atan2(_direction.Y, _direction.X);
                if (rotation > -Math.PI / 2 && rotation < Math.PI / 2)
                {
                    spriteBatch.Draw(_texture, _position, null, Color.White, rotation, new Vector2(4, 12), _holderScale * 0.5f, SpriteEffects.FlipHorizontally, layer);
                }
                else
                {
                    spriteBatch.Draw(_texture, _position, null, Color.White, rotation, new Vector2(4, 20), _holderScale * 0.5f, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, layer);
                }
                foreach (var bullet in _bullets)
                {
                    bullet.Draw(spriteBatch);
                }
                if (_isSniper && _showLine)
                {
                    BulletReach();
                    if (_isGamePad)
                    {
                        GraphicManager.DrawLine(_tipOfTheGun, _MaxPointBulletReach, spriteBatch);
                    }
                    else
                    {
                        if (Vector2.Distance(_tipOfTheGun, _MaxPointBulletReach) > 30)
                            GraphicManager.DrawLine(_tipOfTheGun, _MaxPointBulletReach, spriteBatch);
                    }
                }
            }
        }
        public Gun Copy(float scale,bool hitPlayers)
        {
            return new Gun(_id, _texture, _position, _enemies, _bullet, _isSniper,_spread,hitPlayers);
        }
        public void Shot()
        {
            if (!_swing_weapon)
            {
                if (_shooting_timer >= _bullet._shootingTimer)
                {
                    _shooting_timer = 0;
                    Vector2 _directionSpread;
                    if (_spread != 0)
                    {
                        Random x = new Random();
                        _directionSpread = _direction + new Vector2(((float)x.NextDouble() - 0.5f) * _spread, ((float)x.NextDouble() - 0.5f) * _spread);
                    }
                    else
                    {
                        _directionSpread = _direction;
                    }

                    Bullet bullet = _bullet.Copy(_directionSpread, _tipOfTheGun, _direction, _hitPlayers);
                    _bullets.Add(bullet);
                }
            }
        }

        private void SwingUpdate(GameTime gameTime)
        {
            _swing_frame_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_swing_frame_timer > _swing_frame_window)
            {
                _swing_frame_timer = 0;
                if (_moving_direction_int == (int)Direction.Up)
                {
                    _position += new Vector2(0, -swingSpeed);
                }
                else if (_moving_direction_int == (int)Direction.Down)
                {
                    _position += new Vector2(0, swingSpeed);
                }
                else if (_moving_direction_int == (int)Direction.Right)
                {
                    _position += new Vector2(swingSpeed, 0);
                }
                else if (_moving_direction_int == (int)Direction.Left)
                {
                    _position += new Vector2(-swingSpeed, 0);
                }
            }
        }
        public void SwingWeapon()
        {
            if (_between_attacks_timer > _between_attacks_timer_window)
            {
                _swing_weapon = true;
            }
        }
        public bool BulletReach()
        {
            Vector2 tempPos;
            tempPos = _tipOfTheGun;
            Rectangle tempRec;
            while (true)
            {
                if (tempPos.X < 2000 && tempPos.X > 0 && tempPos.Y < 2000 && tempPos.Y > 0)
                {
                    tempRec = new Rectangle((int)tempPos.X, (int)tempPos.Y, _bullet.Rectangle.Width, _bullet.Rectangle.Height);
                    if (_hitPlayers && CollisionManager.isColidedWithPlayer(tempRec, Vector2.Zero, 0))
                    {
                        _MaxPointBulletReach = tempPos;
                        return true;
                    }
                    else if (!_hitPlayers && CollisionManager.isColidedWithEnemies(tempRec, Vector2.Zero, 0))
                    {
                        _MaxPointBulletReach = tempPos;
                        return true;
                    }
                    if (CollisionManager.isCollidingWalls(tempRec, _direction * _bullet._speed))
                    {

                        _MaxPointBulletReach = tempPos;
                        return false;
                    }
                    tempPos += _direction * _bullet._speed;
                }
                else
                {
                    _MaxPointBulletReach = tempPos;
                    return false;
                }

            }
        }
        public void UpdatePacketShort(PacketStructure packet)
        {
            foreach (var bullet in _bullets)
            {
                bullet.UpdatePacketShort(packet);
            }

        }
        public void ReadPacketShort(PacketStructure packet)
        {
            int bulletAmount = packet.ReadInt();
            for (int i = 0; i < bulletAmount; i++)
            {
                int bullet_num = packet.ReadInt();
                Bullet bullet = _bullets.Find(bullet => bullet._bulletNumber == bullet_num);
                if (bullet != null)
                {
                    bullet.readPacketShort(packet);
                }
                else
                {
                    //_bullets.Add(new Bullet(_bullet._collection_id, this, _bullet._texture, packet.ReadVector2(), packet.ReadVector2(), _enemies, _bullet._speed, bullet_num,_bullet._shootingTimer,_bullet._dmg));
                }
            }
        }

    }
}
