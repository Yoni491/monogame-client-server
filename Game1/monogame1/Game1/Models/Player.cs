using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameClient
{
    public class Player
    {
        public int _playerNum;

        private Gun _gun;

        private Boolean _hide_gun = false;

        private Input _input;

        private float _speed = 2f;

        private Vector2 _velocity;

        private AnimationManager _animationManager;

        private Dictionary<string, Animation> _animations;

        private Vector2 _position;

        private HealthManager _health;

        private Vector2 _looking_direction;

        float _timer = 0;

        PlayerManager _playerManager;

        ItemManager _itemManager;

        InventoryManager _inventoryManager;

        public bool _isGamePad;
        public Vector2 Position { get => _position; set => _position = value; }

        public Player(Dictionary<string, Animation> i_animations, Vector2 position, Input input, int health, PlayerManager playerManager, ItemManager itemManager,InventoryManager inventoryManager)
        {
            _animations = i_animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            _position = position;
            _input = input;
            _health = new HealthManager(health, position + new Vector2(8, 10));
            _velocity = Vector2.Zero;
            _playerManager = playerManager;
            _itemManager = itemManager;
            _inventoryManager = inventoryManager;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_hide_gun && _gun != null)
                _gun.Draw(spriteBatch, _position, TileManager.GetLayerDepth(_position.Y) - 0.01f);
            _animationManager.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            if (_gun != null && !_hide_gun)
                _gun.Draw(spriteBatch, _position, TileManager.GetLayerDepth(_position.Y) + 0.01f);
            _health.Draw(spriteBatch,TileManager.GetLayerDepth(Position.Y));
        }

        public void InputReader()
        {
            _velocity = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(_input._up))
            {
                _isGamePad = false;
                _velocity.Y = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(_input._down))
            {
                _isGamePad = false;
                _velocity.Y = 1;
            }
            if (Keyboard.GetState().IsKeyDown(_input._left))
            {
                _isGamePad = false;
                _velocity.X = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(_input._right))
            {
                _isGamePad = false;
                _velocity.X = 1;
            }
            if (_input._left_joystick_direction != Vector2.Zero)
            {
                _isGamePad = true;
                _velocity = _input._left_joystick_direction;
            }

            if (_velocity != Vector2.Zero)
            {
                _velocity = Vector2.Normalize(_velocity) * _speed;
            }
            if (_input._right_joystick_direction != Vector2.Zero)
            {
                _isGamePad = true;
                _looking_direction = _input._right_joystick_direction;
            }
            if(!_isGamePad)
                _looking_direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _gun.Position;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _isGamePad = false;
                if (_timer >= _gun._bullet._shootingTimer)
                {
                    _gun.Shot();
                    _timer = 0;
                }
            }

            if (_input._right_trigger > 0)
            {
                if (_timer >= _gun._bullet._shootingTimer)
                {
                    _gun.Shot();
                    _timer = 0;
                }
            }
            if(Keyboard.GetState().IsKeyDown(_input._pick))
            {
                Item item = _itemManager.findClosestItem(_position + (_animationManager.getAnimationPickPosition()));
                if(item!=null)
                {
                    _inventoryManager.addItemToInventory(item);
                }
            }

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
                    _animationManager.Play(_animations["WalkRight"]);

                }
                else if (-_velocity.X >= Math.Abs(_velocity.Y))
                {
                    _animationManager.Play(_animations["WalkLeft"]);
                }
                else if (_velocity.Y > 0)
                {
                    _animationManager.Play(_animations["WalkDown"]);
                }
                else if (_velocity.Y < 0)
                {
                    _animationManager.Play(_animations["WalkUp"]);
                    _hide_gun = true;
                }
                else _animationManager.Stop();
            }
        }


        public void EquipGun(Gun gun)
        {
            _gun = gun;
        }
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            InputReader();

            SetAnimations();

            _position += _velocity;

            _animationManager.Position = _position;

            _animationManager.Update(gameTime);

            if (_gun != null)
            {
                _gun.Update(gameTime, enemies, _looking_direction, _isGamePad);
            }

            _health._position = _position + new Vector2(8, 10);

            _input.Update();
        }
        public void UpdatePacketShort(PacketShort_Client packet)
        {
            packet.WriteInt(_playerNum);
            packet.WriteVector2(Position);
            packet.WriteInt(_health._health_left);
            packet.WriteInt(_health._total_health);
            packet.WriteVector2(_velocity);
            packet.WriteVector2(_looking_direction);
            packet.WriteInt(_gun._bullets.Count());
            _gun.UpdatePacketShort(packet);
        }
        public void UpdatePacketLong(PacketLong_Client packet)
        {
            packet.WriteInt(_playerNum);
            packet.WriteInt(_gun._id);
        }

    }
}

