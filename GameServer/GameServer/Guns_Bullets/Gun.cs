using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameServer
{
    public class Gun
    {
        public int _id;

        private Vector2 _position;

        protected Vector2 _velocity;

        public List<Bullet> _bullets = new List<Bullet>();

        private Vector2 _direction;

        public Vector2 Position { get => _position; set => _position = value; }

        public Gun(Vector2 position, int id)
        {
            _position = position;
            _id = id;
        }
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies, Vector2 direction)
        {
            //_direction = direction;
            //_enemies = enemies;
            foreach (var bullet in _bullets)
            {
                bullet.Update(gameTime, enemies);
            }
            _bullets.RemoveAll(bullet => bullet._destroy == true);
        }
        public void ReadPacketShort(PacketStructure packet)
        {
            int num = packet.ReadInt();
            for (int i = 0; i < num; i++)
            {
                _bullets.Add(new Bullet(this, packet.ReadVector2(), packet.ReadVector2()));
            }
        }
        public void UpdatePacketShort(PacketStructure packet)
        {
            foreach (var bullet in _bullets)
            {
                bullet.UpdatePacketShort(packet);
                bullet._sent = true;
            }
        }
    }
}
