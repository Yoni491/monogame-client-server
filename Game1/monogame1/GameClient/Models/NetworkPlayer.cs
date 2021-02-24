using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace GameClient
{
    public class NetworkPlayer
    {
        public int _playerNum;

        public bool updateTexture = true;

        public Gun _gun;

        private Boolean _hide_gun = false;

        private Vector2 _velocity;

        private AnimationManager _animationManager;

        public Vector2 _position;

        private HealthManager _health;

        private Vector2 _looking_direction;
        private int _animationNum = -1;
        private int _gunNum = -1;
        private float _scale = 1.5f;
        private int _width=0;
        private int _height=0;
        int _movingDirection = 0;
        public Vector2 Position_Feet { get => new Vector2((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f)); }
        public Rectangle RectangleMovement { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f), (int)(_width * _scale * 0.1), (int)(_height * _scale * 0.1)); }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)(_width * _scale), (int)(_height * _scale));
            }
        }

        public NetworkPlayer(Vector2 position,AnimationManager animationManager, int health, int playerNum, Gun gun)
        {
            _animationManager = animationManager;
            _scale = _animationManager._scale;
            _position = position;
            _velocity = Vector2.Zero;
            _playerNum = playerNum;
            if (gun != null)
            {
                _gun = gun;
            }
            else
                _gun = CollectionManager._guns[0];
            _gun._holderScale = _scale;
            _health = new HealthManager(health, position + new Vector2(8, 10),_scale);
        }
        public void Update(GameTime gameTime)
        {
            if (!Game_Client._isServer)
            {
                SetAnimations();

                _animationManager.Update(gameTime, _position);

                _position += _velocity;


                if (_gun != null)
                {
                    _gun.Update(gameTime, _looking_direction,0, false,false, _position);
                }

                _health.Update(_position);
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_hide_gun && _gun != null)
                _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) - 0.01f);
            if (_animationManager != null)
                _animationManager.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            if (_gun != null && !_hide_gun)
                _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) + 0.01f);
            if (_hide_gun && _gun != null)
                _health.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
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
                else
                {
                    _animationManager.Play(_movingDirection);
                    _animationManager.Stop();
                }
            }
        }
        public void UpdatePacketShort(Packet packet)
        {
            packet.WriteInt(_playerNum);
            packet.WriteVector2(_position);
            packet.WriteInt(_movingDirection);
            packet.WriteInt(_health._health_left);
            packet.WriteInt(_health._total_health);
            packet.WriteVector2(_velocity);
            packet.WriteVector2(_looking_direction);
            packet.WriteInt(_animationNum);
            packet.WriteInt(_gunNum);
            packet.WriteInt(_gun._bullets.FindAll(x => x._bulletSent == false).Count());
            _gun.UpdatePacketShort(packet);
        }

        public void ReadPacketShort(Packet packet)
        {
            _position = packet.ReadVector2();
            _movingDirection = packet.ReadInt();
            _health._health_left = packet.ReadInt();
            _health._total_health = packet.ReadInt();
            _velocity = packet.ReadVector2();
            _looking_direction = packet.ReadVector2();
            int animationNum = packet.ReadInt();
            if (animationNum == 0)
            {
                _animationManager = GraphicManager.GetAnimationManager_spriteMovement(3, 1.5f);
                _width = _animationManager.Animation._frameWidth;
                _height = _animationManager.Animation._frameHeight;
            }
            else if (animationNum != _animationNum)
            {
                _animationNum = animationNum;
                _animationManager = GraphicManager.GetAnimationManager_spriteMovement(animationNum, 1.5f);
                _width = _animationManager.Animation._frameWidth;
                _height = _animationManager.Animation._frameHeight;
            }
            int gunNum = packet.ReadInt();
            if (gunNum != _gunNum)
            {
                _gunNum = gunNum;
                _gun = CollectionManager.GetGunCopy(gunNum, false,false);
                _gun._holderScale = _scale;
            }
            _gun.ReadPacketShort(packet);
        }

    }
}

