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

        public HealthManager _health;

        private Vector2 _looking_direction;
        private int _animationNum = -1;
        private int _gunNum = -1;
        private float _scale = 1.5f;
        private int _width=0;
        private int _height=0;
        int _movingDirection = 0;
        public bool _serverUpdated;
        public NameDisplay _nameDisplay;

        public Vector2 Position_Feet { get => new Vector2((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f)); }
        public Vector2 Position_Head { get => new Vector2((int)(_position.X + (_width * _scale) * 0.35f), (int)(_position.Y + (_height * _scale) * 0.3f)); }
        public Rectangle Rectangle { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.35f), (int)(_position.Y + (_height * _scale) * 0.3f), (int)(_width * _scale * 0.3), (int)(_height * _scale * 0.6));}
        public Rectangle RectangleMovement { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f), 7, 7); }


        public NetworkPlayer(Vector2 position,AnimationManager animationManager, int health, int playerNum, Gun gun, NameDisplay nameDisplay)
        {
            _nameDisplay = nameDisplay;
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
                _nameDisplay.Update(Position_Feet + new Vector2(5, 20));

                _animationManager.Update(gameTime, _position);

                _animationManager.SetAnimationsFromServer(_velocity,ref _hide_gun,ref _movingDirection);

                _position += _velocity;

                if (_gun != null)
                {
                    _gun.Update(gameTime, _looking_direction, _movingDirection, false,false, _position);
                }

                _health.Update(_position);
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(_health._health_left<=0)
            {
                spriteBatch.Draw(GraphicManager._deadPlayerTexture, Position_Feet, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, TileManager.GetLayerDepth(_position.Y));
            }
            else
            {
                if (_hide_gun && _gun != null)
                    _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) - 0.01f);
                if (_animationManager != null)
                    _animationManager.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
                if (_gun != null && !_hide_gun)
                    _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) + 0.01f);
                _health.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            }
            _nameDisplay.Draw(spriteBatch);

        }
        public void UpdatePacketShort(Packet packet,bool writeName)
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
            _gun.UpdatePacketShort(packet);
            if(writeName)
            {
                packet.WriteString(_nameDisplay._text);
            }
        }

        public void ReadPacketShort(Packet packet,bool readName = false)
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
                try
                {
                    _animationManager = GraphicManager.GetAnimationManager_spriteMovement(animationNum, 1.5f);
                    _animationNum = animationNum;
                    _width = _animationManager.Animation._frameWidth;
                    _height = _animationManager.Animation._frameHeight;
                }
                catch
                {
                    Console.WriteLine("Animation Num not valid:" + _animationNum);
                }
                
            }
            int gunNum = packet.ReadInt();
            if (gunNum != _gunNum)
            {
                _gunNum = gunNum;
                _gun = CollectionManager.GetGunCopy(gunNum, false,false,null);
                _gun._holderScale = _scale;
            }
            _gun.ReadPacketShort(packet);
            if(readName)
            {
                _nameDisplay._text = packet.ReadString();
            }
        }

    }
}

