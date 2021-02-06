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
        public void Update(GameTime gameTime, Vector2 direction, bool isGamePad,bool showLine)
        {
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float layer)
        {
            _position = position + new Vector2(23, 40) * _holderScale;
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
            if(_isSniper && _showLine)
            {
                Vector2 sniperStart = _tipOfTheGun;
                Vector2 sniperEnd;
                BulletReach();
                if (_isGamePad)
                {
                    sniperEnd = _direction * 300f + sniperStart;
                    GraphicManager.DrawLine(sniperStart, _MaxPointBulletReach,spriteBatch);
                }
                else
                {
                    sniperEnd = _direction * 300f + sniperStart;
                    //sniperEnd = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    if (Vector2.Distance(sniperEnd, sniperStart) > 30)
                        GraphicManager.DrawLine(sniperStart, _MaxPointBulletReach, spriteBatch);
                }
            }
        }
        public Gun Copy(float scale,bool hitPlayers)
        {
            return new Gun(_id, _texture, _position, _enemies, _bullet, _isSniper,_spread,hitPlayers);
        }
        public void Shot()
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

                Bullet bullet = _bullet.Copy(_directionSpread,_tipOfTheGun, _direction,_hitPlayers);
                _bullets.Add(bullet);
            }
        }
        public bool BulletReach()
        {
            //_MaxPointBulletReach = CollisionManager.GetClosestCollision(_position + _direction * 38f, _direction, _hitPlayers);
            Vector2 tempPos;
            tempPos = _tipOfTheGun;
            Rectangle tempRec;
            while (true)
            {
                if (tempPos.X < 2000 && tempPos.X > 0 && tempPos.Y < 2000 && tempPos.Y > 0)
                {
                    tempRec = new Rectangle((int)tempPos.X, (int)tempPos.Y, _bullet.Rectangle.Width, _bullet.Rectangle.Height);
                    if (_hitPlayers && CollisionManager.isColidedWithPlayer(tempRec, 0))
                    {
                        _MaxPointBulletReach = tempPos;
                        return true;
                    }
                    else if (!_hitPlayers && CollisionManager.isColidedWithEnemies(tempRec, 0))
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
