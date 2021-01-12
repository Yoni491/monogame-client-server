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

        private bool _isSniper;

        private bool _isGamePad;

        public float _holderScale = 0;
        public Gun(int id, Texture2D texture, Vector2 position, List<Simple_Enemy> enemies, Bullet bullet, bool isSniper)
        {
            _id = id;
            _texture = texture;
            _enemies = enemies;
            _bullet = bullet;
            _position = position;
            _isSniper = isSniper;
        }
        public void Update(GameTime gameTime, Vector2 direction, bool isGamePad)
        {
            _direction = direction;
            foreach (var bullet in _bullets)
            {
                bullet.Update(gameTime);
            }
            _bullets.RemoveAll(bullet => bullet._destroy == true);
            _isGamePad = isGamePad;
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
            if(_isSniper)
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
                    sniperEnd = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    if (Vector2.Distance(sniperEnd, sniperStart) > 30)
                        GraphicManager.DrawLine(sniperStart, sniperEnd, spriteBatch);
                }
            }
        }
        public Gun Copy(float scale)
        {
            return new Gun(_id, _texture, _position, _enemies, _bullet, _isSniper);
        }
        public void Shot()
        {
            Bullet bullet = new Bullet(_bullet._collection_id, this, _bullet._texture, _position + Vector2.Normalize(_direction) * 20f, _direction, _enemies, _bullet._speed, -1, _bullet._shootingTimer, _bullet._dmg);
            _bullets.Add(bullet);
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
                    _bullets.Add(new Bullet(_bullet._collection_id, this, _bullet._texture, packet.ReadVector2(), packet.ReadVector2(), _enemies, _bullet._speed, bullet_num,_bullet._shootingTimer,_bullet._dmg));
                }
            }
        }

    }
}
