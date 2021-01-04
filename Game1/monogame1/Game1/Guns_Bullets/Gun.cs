using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameClient
{
    public class Gun
    {
        public int _id;

        private Vector2 _position;

        protected Texture2D _texture;

        protected Vector2 _velocity;

        protected float _scale = 0.5f;

        protected Texture2D _bullet_texture;

        public List<Bullet> _bullets = new List<Bullet>();

        private Vector2 _direction;

        private List<Simple_Enemy> _enemies;

        public Bullet _bullet;
        public Vector2 Position { get => _position; set => _position = value; }

        public Gun(int id, Texture2D texture, Vector2 position, List<Simple_Enemy> enemies, Bullet bullet)
        {
            _id = id;
            _texture = texture;
            _enemies = enemies;
            _bullet = bullet;
            _position = position;
        }
        public Gun Copy()
        {
            return new Gun(_id, _texture, _position, _enemies, _bullet);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float layer)
        {
            _position = position + new Vector2(23, 40);
            Client.game.DrawLine(_position + new Vector2(0.1f, 0.1f), _position);

            float rotation = (float)Math.Atan2(_direction.Y, _direction.X);
            if (rotation > -Math.PI / 2 && rotation < Math.PI / 2)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, rotation, new Vector2(4, 12), _scale, SpriteEffects.FlipHorizontally, layer);
            }
            else
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, rotation, new Vector2(4, 20), _scale, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, layer);
            }
            foreach (var bullet in _bullets)
            {
                bullet.Draw(spriteBatch);
            }
            //TODO:for sniper:
            //Program.game.DrawLine(_position + Vector2.Normalize(_direction) * 38f, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
        }
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies, Vector2 direction)
        {
            _direction = direction;
            _enemies = enemies;
            foreach (var bullet in _bullets)
            {
                bullet.Update(gameTime, _enemies);
            }
            _bullets.RemoveAll(bullet => bullet._destroy == true);
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
                    _bullets.Add(new Bullet(_bullet._collection_id, this, _bullet._texture, packet.ReadVector2(), packet.ReadVector2(), _enemies, _bullet._speed, bullet_num,_bullet._shootingTimer));
                }
            }
        }
        public void Shot()
        {
            Bullet bullet = new Bullet(_bullet._collection_id, this, _bullet._texture, _position + Vector2.Normalize(_direction) * 20f, _direction, _enemies, _bullet._speed, -1, _bullet._shootingTimer);
            _bullets.Add(bullet);
        }
    }
}
