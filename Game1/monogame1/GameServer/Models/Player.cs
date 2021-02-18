//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace GameServer
//{
//    public class Player
//    {
//        private Gun _gun;
//        private MeleeWeapon _meleeWeapon;
//        private Vector2 _velocity;
//        public HealthManager _health;
//        public Vector2 _position;
//        public AnimationManager _animationManager;
//        private Vector2 _looking_direction;
//        private bool _hide_weapon = false;
//        public bool _isGamePad;
//        public bool _clickedOnUi;
//        private float _speed = 6f;
//        private float _scale;
//        public int _playerNum;
//        private int _width;
//        private int _height;
//        private int _moving_direction;
//        public int _animationNum;

//        private PlayerManager _playerManager;
//        private ItemManager _itemManager;

//        public Vector2 Position_Feet { get => new Vector2((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f)); }
//        public Rectangle Rectangle { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.35f), (int)(_position.Y + (_height * _scale) * 0.5f), (int)(_width * _scale * 0.3), (int)(_height * _scale * 0.4));}
//        public Rectangle RectangleMovement { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f), (int)(_width * _scale * 0.1), (int)(_height * _scale * 0.1)); }
//        public Player(AnimationManager animationManager,int animationNum, Vector2 position, int health, PlayerManager playerManager, ItemManager itemManager)
//        {
//            _animationNum = animationNum;
//            _position = position;
//            _velocity = Vector2.Zero;
//            _playerManager = playerManager;
//            _itemManager = itemManager;
//            _scale = _animationManager._scale;
//            _health = new HealthManager(health);
//            _width = _animationManager.Animation._frameWidth;
//            _height = _animationManager.Animation._frameHeight;
//        }

//        public void EquipGun(Gun gun)
//        {
//            _gun = gun;
//            //_gun._holderScale = _scale;
//        }
//        public void EquipMeleeWeapon(MeleeWeapon meleeWeapon)
//        {
//            _meleeWeapon = meleeWeapon;
//        }
//        public void PositionPlayerFeetAt(Vector2 position)
//        {
//            _position = position;
//            Vector2 temp = _position - Position_Feet;
//            _position += temp;
//        }
//        //public void UpdatePacketShort(PacketShort_Client packet)
//        //{
//        //    packet.WriteInt(_playerNum);
//        //    packet.WriteVector2(_position);
//        //    packet.WriteInt(_health._health_left);
//        //    packet.WriteInt(_health._total_health);
//        //    packet.WriteVector2(_velocity);
//        //    packet.WriteVector2(_looking_direction);
//        //    packet.WriteInt(_animationNum);
//        //    packet.WriteInt(_gun._id);
//        //    packet.WriteInt(_gun._bullets.FindAll(x=>x._bulletSent==false).Count());
//        //    _gun.UpdatePacketShort(packet);
//        //}
//        //public void UpdatePacketLong(PacketLong_Client packet)
//        //{
//        //    packet.WriteInt(_playerNum);
//        //    packet.WriteInt(_gun._id);
//        //}

//    }
//}

