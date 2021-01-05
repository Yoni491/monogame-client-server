using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace GameClient
{
    public class MeleeWeapon
    {
        public int _id;

        private Vector2 _position;

        protected AnimationManager _animationManager;

        protected Vector2 _velocity;

        protected float _scale = 0.5f;

        private int _direction;

        private List<Simple_Enemy> _enemies;

        private float _swing_range;
        private Texture2D _texture;


        public Vector2 Position { get => _position; set => _position = value; }


        public MeleeWeapon(int id, Texture2D texture, AnimationManager animationManager, Vector2 position, List<Simple_Enemy> enemies, float swing_range)
        {
            _id = id;
            _texture = texture;
            _animationManager = animationManager;
            _enemies = enemies;
            _position = position;
            _swing_range = swing_range;
        }
        public MeleeWeapon Copy()
        {
            return new MeleeWeapon(_id, _texture, _animationManager, _position, _enemies,_swing_range);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float layer)
        {
            _position = position + new Vector2(23, 44);
            if(_direction == (int)Direction.Up)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(4, 12), _scale, SpriteEffects.None, layer);
            }
            else if(_direction == (int)Direction.Down)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(-12, 12), _scale, SpriteEffects.FlipHorizontally, layer);
            }
            else if (_direction == (int)Direction.Right)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(4, 12), _scale, SpriteEffects.FlipHorizontally, layer);
            }
            else if (_direction == (int)Direction.Left)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(24, 12), _scale, SpriteEffects.None, layer);
            }
        }
        public void Update(int direction)
        {
            _direction = direction;
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
