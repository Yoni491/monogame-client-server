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
            _direction = direction;
            foreach (var bullet in _bullets)
            {
                bullet.Update(gameTime);
            }
            _bullets.RemoveAll(bullet => bullet._destroy == true);
            _isGamePad = isGamePad;
            _shooting_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _showLine = showLine;
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
                Vector2 sniperStart = _position + Vector2.Normalize(_direction) * 38f;
                Vector2 sniperEnd;
                if (_isGamePad)
                {
                    sniperEnd = Vector2.Normalize(_direction) * 300f + sniperStart;
                    GraphicManager.DrawLine(sniperStart, sniperEnd,spriteBatch);
                }
                else
                {
                    sniperEnd = Vector2.Normalize(_direction) * 300f + sniperStart;
                    //sniperEnd = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    if (Vector2.Distance(sniperEnd, sniperStart) > 30)
                        GraphicManager.DrawLine(sniperStart, sniperEnd, spriteBatch);
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
                    _directionSpread = Vector2.Normalize(_direction) + new Vector2(((float)x.NextDouble() - 0.5f) * _spread, ((float)x.NextDouble() - 0.5f) * _spread);
                }
                else
                {
                    _directionSpread = _direction;
                }

                Bullet bullet = _bullet.Copy(_directionSpread,_position,_direction,_hitPlayers);
                _bullets.Add(bullet);
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
