using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameClient
{
    public class NetworkPlayer
    {
        public int _playerNum;

        public bool updateTexture = true;

        private Gun _gun;

        private Boolean _hide_gun = false;

        private Vector2 _velocity;

        private AnimationManager _animationManager;

        private Vector2 _position;

        private HealthManager _health;

        private Vector2 _looking_direction;
        private int _animationNum = -1;
        private int _gunNum = -1;
        private float _scale = 1.5f;
        private int _width;
        private int _height;
        public Vector2 Position_Feet { get => _position + new Vector2(_width / 2, _height * 2 / 3); }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)(_width * _scale), (int)(_height * _scale));
            }
        }

        public NetworkPlayer(Vector2 position, int health, int playerNum, Gun gun)
        {
            _position = position;
            _velocity = Vector2.Zero;
            _playerNum = playerNum;
            _gun = gun;
            _health = new HealthManager(health, position + new Vector2(8, 10),_scale);
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
                    _gun.Update(gameTime, _looking_direction,0, false,false, _position);
                }
                //_velocity = Vector2.Zero;

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
        public void UpdateTexture(AnimationManager animationManager)
        {
            updateTexture = false;
            _animationManager = animationManager;
            _scale = _animationManager._scale;
            //maybe there will be a problem with the server when there is no gun scale
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

        public void ReadPacketShort(PacketStructure packet)
        {
            _position = packet.ReadVector2();
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
                _animationManager = GraphicManager.GetAnimationManager_spriteMovement(animationNum, 1.5f);
                _width = _animationManager.Animation._frameWidth;
                _height = _animationManager.Animation._frameHeight;
            }
            int gunNum = packet.ReadInt();
            if (gunNum != _gunNum)
            {
                _gunNum = gunNum;
                _gun = CollectionManager.GetGunCopy(gunNum, _scale, false);
                _gun._holderScale = _scale;
            }
            _gun.ReadPacketShort(packet);
        }

    }
}

