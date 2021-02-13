using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace GameServer
{
    public class Player
    {
        static int playerNum = 0;

        private int playerNum1;

        public Gun _gun;

        private float _speed = 2f;

        public Vector2 _velocity;

        private Vector2 _position;

        public HealthManager _health;

        private Vector2 _looking_direction;

        public Socket _socket;

        public PacketLong_Server _longPacket;

        public PacketShort_Server _shortPacket; 
        public Vector2 Position { get => _position; set => _position = value; }
        public int PlayerNum { get => playerNum1; set => playerNum1 = value; }

        private int _animationNum;
        private int _gunNum;

        public Player(Vector2 position, int health, Socket socket, List<Player> players)
        {
            _position = position;
            _health = new HealthManager(health, position + new Vector2(8, 10));
            _velocity = Vector2.Zero;
            PlayerNum = playerNum++;
            _gun = new Gun(position, 3);
            _socket = socket;
            _shortPacket = new PacketShort_Server(players);
            _longPacket = new PacketLong_Server(players, this);
        }
        public void EquipGun(Gun gun)
        {
            _gun = gun;
        }
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies)
        {
            _position += _velocity;

            if (_gun != null)
            {
                _gun.Update(gameTime, enemies, _looking_direction);
            }
            //_health._position = _position + new Vector2(8, 10);
        }
        public void UpdatePacketShort(PacketStructure packet)
        {
            packet.WriteInt(PlayerNum);
            packet.WriteVector2(Position);
            packet.WriteInt(_health._health_left);
            packet.WriteInt(_health._total_health);
            packet.WriteVector2(_velocity);
            packet.WriteVector2(_looking_direction);
            packet.WriteInt(_animationNum);
            packet.WriteInt(_gunNum);
            packet.WriteInt(_gun._bullets.Count());
            _gun.UpdatePacketShort(packet);
        }
        public void ReadPacketShort(PacketStructure packet)
        {
            Position = packet.ReadVector2();
            _health._health_left = packet.ReadInt();
            _health._total_health = packet.ReadInt();
            _velocity = packet.ReadVector2();
            _looking_direction = packet.ReadVector2();
            _animationNum = packet.ReadInt();
            _gunNum = packet.ReadInt();
            _gun.ReadPacketShort(packet);
        }
    }
}

