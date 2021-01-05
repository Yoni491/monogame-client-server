using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameClient
{
    public class OtherPlayer
    {
        public int _playerNum;

        public bool updateTexture = true;

        private Gun _gun;

        private Boolean _hide_gun = false;

        //private Input _input;

        //private float _speed = 2f;

        private Vector2 _velocity;

        private AnimationManager _animationManager;

        private Vector2 _position;

        private HealthManager _health;

        private Vector2 _looking_direction;

        public Vector2 Position { get => _position; set => _position = value; }

        public OtherPlayer(Vector2 position, int health, int playerNum, Gun gun)
        {
            _position = position;
            _health = new HealthManager(health, position + new Vector2(8, 10));
            _velocity = Vector2.Zero;
            _playerNum = playerNum;
            _gun = gun;
        }
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies)
        {
            if (!updateTexture)
            {
                SetAnimations();

                _position += _velocity;

                _animationManager.Update(gameTime, _position);



                if (_gun != null)
                {
                    _gun.Update(gameTime, _looking_direction, false);
                }
                //_velocity = Vector2.Zero;

                _health._position = _position + new Vector2(8, 10);
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_hide_gun && _gun != null)
                _gun.Draw(spriteBatch, _position, TileManager.GetLayerDepth(_position.Y) - 0.01f);
            if (_animationManager != null)
                _animationManager.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            if (_gun != null && !_hide_gun)
                _gun.Draw(spriteBatch, _position, TileManager.GetLayerDepth(_position.Y) + 0.01f);
            if (_hide_gun && _gun != null)
                _health.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
        }
        public void UpdateTexture(AnimationManager animationManager)
        {
            updateTexture = false;
            _animationManager = animationManager;
        }
        protected void SetAnimations()
        {

            if (_velocity == Vector2.Zero)
                _animationManager.Stop();
            else
            {
                _hide_gun = false;
                if (_velocity.X >= Math.Abs(_velocity.Y))
                {
                    _animationManager.Play((int)Direction.Right);

                }
                else if (-_velocity.X >= Math.Abs(_velocity.Y))
                {
                    _animationManager.Play((int)Direction.Left);
                }
                else if (_velocity.Y > 0)
                {
                    _animationManager.Play((int)Direction.Down);
                }
                else if (_velocity.Y < 0)
                {
                    _animationManager.Play((int)Direction.Up);
                    _hide_gun = true;
                }
                else _animationManager.Stop();
            }
        }

        //public void EquipGun(Gun gun)
        //{
        //    _gun = gun;
        //}

        public void ReadPacketShort(PacketStructure packet)
        {
            Position = packet.ReadVector2();
            _health._health_left = packet.ReadInt();
            _health._total_health = packet.ReadInt();
            _velocity = packet.ReadVector2();
            _looking_direction = packet.ReadVector2();
            _gun.ReadPacketShort(packet);
        }
    }
}

