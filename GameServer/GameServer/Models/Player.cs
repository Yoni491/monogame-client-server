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
    public class Player
    {
        static int playerNum = 0;

        public int _playerNum;

        private Gun _gun;

        private float _speed = 2f;

        public Vector2 _velocity;

        private Vector2 _position;

        public HealthManager _health;

        private Vector2 _looking_direction;

        public Vector2 Position { get => _position; set => _position = value; }

        public Player(Vector2 position , int health)
        {
            _position = position;
            _health = new HealthManager(health,position +new Vector2(8,10));
            _velocity = Vector2.Zero;
            _playerNum = playerNum++;
        }

        public void Update(GameTime gameTime,List<Simple_Enemy> enemies)
        {
            _position += _velocity;

            if (_gun != null)
            {
                _gun.Update(gameTime, enemies,_looking_direction);
            }
            _velocity = Vector2.Zero;

            _health._position = _position + new Vector2(8, 10);

        }
        public void UpdatePacketShort(PacketShort_Server packet)
        {
            packet.WriteInt(_playerNum);
            packet.WriteVector2(Position);
            packet.WriteInt(_health._health_left);
            packet.WriteInt(_health._total_health);
            packet.WriteVector2(_velocity);
            packet.WriteVector2(_looking_direction);  
        }
        public void ReadPacketShort(PacketShort_Server packet)
        {
            _playerNum = packet.ReadInt();
            Position = packet.ReadVector2();
            _health._health_left = packet.ReadInt();
            _health._total_health = packet.ReadInt();
            _velocity = packet.ReadVector2();
            _looking_direction = packet.ReadVector2();
        }
    }
}

