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
        private Gun _gun;
        private MeleeWeapon _meleeWeapon;
        private Input _input;
        private Vector2 _velocity;
        private AnimationManager _animationManager;
        public HealthManager _health;
        public Vector2 _position;
        private Vector2 _looking_direction;
        private bool _hide_weapon = false;
        public bool _isGamePad;
        public bool _clickedOnUi;
        private float _speed = 6f;
        private float _scale;
        public int _playerNum;
        private int _width;
        private int _height;
        private int _moving_direction;

        private PlayerManager _playerManager;
        private ItemManager _itemManager;
        private UIManager _uIManager;
        private InventoryManager _inventoryManager;

        public Vector2 Position_Feet { get => new Vector2((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f)); }
        public Rectangle Rectangle { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.35f), (int)(_position.Y + (_height * _scale) * 0.5f), (int)(_width * _scale * 0.3), (int)(_height * _scale * 0.4));}
        public Rectangle RectangleMovement { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f), (int)(_width * _scale * 0.1), (int)(_height * _scale * 0.1)); }
        public Player(AnimationManager animationManager, Vector2 position, Input input, int health, PlayerManager playerManager, ItemManager itemManager,InventoryManager inventoryManager,UIManager uIManager)
        {
            _animationManager = animationManager;
            _position = position;
            _input = input;
            _velocity = Vector2.Zero;
            _playerManager = playerManager;
            _itemManager = itemManager;
            _uIManager = uIManager;
            _inventoryManager = inventoryManager;
            _scale = _animationManager._scale;
            _health = new HealthManager(health, position,_scale);
            _width = _animationManager.Animation._frameWidth;
            _height = _animationManager.Animation._frameHeight;
        }
        public void Update(GameTime gameTime)
        {

            InputReader(gameTime);

            SetAnimations();

            if (CollisionManager.IsCollidingLeft(RectangleMovement, _velocity) && _velocity.X < 0)
                _velocity -= new Vector2(_velocity.X,0);
            if (CollisionManager.IsCollidingRight(RectangleMovement, _velocity) && _velocity.X > 0)
                _velocity -= new Vector2(_velocity.X, 0);
            if (CollisionManager.IsCollidingTop(RectangleMovement, _velocity) && _velocity.Y < 0)
                _velocity -= new Vector2(0,_velocity.Y);
            if (CollisionManager.IsCollidingBottom(RectangleMovement, _velocity) && _velocity.Y > 0)
                _velocity -= new Vector2(0, _velocity.Y);
            _position += _velocity;


            _animationManager.Update(gameTime, _position);

            if (_gun != null)
            {
                _gun.Update(gameTime, _looking_direction, _moving_direction, _isGamePad,_gun._isSniper,_position);
            }
            if (_meleeWeapon != null)
            {
                _meleeWeapon.Update(_moving_direction,gameTime,_position);
            }

            _health.Update(_position);

            _input.Update();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_hide_weapon && _gun != null)
            {
                //_meleeWeapon.Draw(spriteBatch, _position, TileManager.GetLayerDepth(_position.Y) - 0.01f);
                _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) - 0.01f);
            }
            _animationManager.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            if (_gun != null && !_hide_weapon)
            {
                //_meleeWeapon.Draw(spriteBatch, _position, TileManager.GetLayerDepth(_position.Y) + 0.01f);
                _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) + 0.01f);
            }
            _health.Draw(spriteBatch,TileManager.GetLayerDepth(_position.Y));
            
        }

        public void InputReader(GameTime gameTime)
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
                _looking_direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _gun.Position * GraphicManager.ScreenScale;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _isGamePad = false;
                if(!_clickedOnUi)
                { 
                    _gun.Shot();
                }

            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                _isGamePad = false;
                if (_gun != null)
                    _gun.SwingWeapon();
                else
                    _meleeWeapon.SwingWeapon();
            }
            if (_input._right_trigger > 0)
            {
                _gun.Shot();
            }
            if(Keyboard.GetState().IsKeyDown(_input._pick))
            {
                Item item = _itemManager.findClosestItem(_position + (_animationManager.getAnimationPickPosition()));
                if(item!=null)
                {
                    _inventoryManager.addItemToInventory(item);
                }
            }
            _clickedOnUi = false;

        }
        protected void SetAnimations()
        {

            if (_velocity == Vector2.Zero)
                _animationManager.Stop();
            else
            {
                _hide_weapon = false;
                _moving_direction = -1;
                if (_velocity.X >= Math.Abs(_velocity.Y))
                {
                    _moving_direction = (int)Direction.Right;

                }
                else if (-_velocity.X >= Math.Abs(_velocity.Y))
                {
                    _moving_direction = (int)Direction.Left;
                }
                else if (_velocity.Y > 0)
                {
                    _moving_direction = (int)Direction.Down;
                }
                else if (_velocity.Y < 0)
                {
                    _moving_direction = (int)Direction.Up;
                    _hide_weapon = true;
                }
                else _animationManager.Stop();
                if (_moving_direction != -1)
                    _animationManager.Play(_moving_direction);
            }
        }

        public void EquipGun(Gun gun)
        {
            _gun = gun;
            _gun._holderScale = _scale;
        }
        public void EquipMeleeWeapon(MeleeWeapon meleeWeapon)
        {
            _meleeWeapon = meleeWeapon;
        }
        public void PositionPlayerFeetAt(Vector2 position)
        {
            _position = position;
            Vector2 temp = _position - Position_Feet;
            _position += temp;
        }
        public void UpdatePacketShort(PacketShort_Client packet)
        {
            packet.WriteInt(_playerNum);
            packet.WriteVector2(_position);
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

