using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace GameServer
{
    public class Bullet
    {

        private float _speed = 5f;

        //public Texture2D _texture;

        Vector2 _position;

        private float _timer = 0;

        private Vector2 _direction;

        public bool _destroy = false;

        private Gun _gun;

        private List<Simple_Enemy> _enemies;

        public bool _sent = false;

        public Bullet(Gun gun, Vector2 position, Vector2 direction)
        {
            _gun = gun;
            _position = position;
            _direction = Vector2.Normalize(direction);
        }
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies)
        {
            _enemies = enemies;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 2f)
            {
                _destroy = true;
            }
            _position += _direction * _speed;
            //if (_enemies != null)
            //    foreach (var enemy in _enemies)
            //    {
            //        if (enemy.isCollision(this))
            //            _destroy = true;
            //    }
        }
        public void readPacketShort(PacketStructure packet)
        {
            _position = packet.ReadVector2();
            _direction = packet.ReadVector2();
        }
        public void UpdatePacketShort(PacketStructure packet)
        {
            packet.WriteVector2(_position);
            packet.WriteVector2(_direction);
        }
    }

}
