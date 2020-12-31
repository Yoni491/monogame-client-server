using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameServer
{
    public class Bullet
    {

        public int _bulletNumber;

        private float _speed = 5f;

        //public Texture2D _texture;

        Vector2 _position;

        private float _timer = 0;

        private Vector2 _direction;

        public bool _destroy = false;

        private Gun _gun;

        private List<Simple_Enemy> _enemies;

        //public Rectangle Rectangle
        //{
        //    get
        //    {
        //        return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        //    }
        //}

        public Bullet(int bulletNumber, Gun gun , Vector2 position, Vector2 direction)
        {
            _gun = gun;
            _position = position;
            _direction = Vector2.Normalize(direction);
            _bulletNumber = bulletNumber;
        }
        public void readPacketShort(PacketStructure packet)
        {
            _position = packet.ReadVector2();
            _direction = packet.ReadVector2();
        }
        public void UpdatePacketShort(PacketStructure packet)
        {
            packet.WriteInt(_bulletNumber);
            packet.WriteVector2(_position);
            packet.WriteVector2(_direction);
        }
    }

}
