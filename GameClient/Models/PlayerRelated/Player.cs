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
        public Gun _gun;
        private MeleeWeapon _meleeWeapon;
        public Input _input;
        private Vector2 _velocity;
        public AnimationManager _animationManager;
        public HealthManager _health;
        public Vector2 _position;
        private Vector2 _looking_direction;
        private bool _hide_weapon = false;
        static public bool _mouseIntersectsUI;
        private float _speed = 6f;
        private float _scale;
        public int _playerNum;
        private int _width;
        private int _height;
        private int _moving_direction = 1;
        public int _animationNum;
        public bool _dead;
        public NameDisplay _nameDisplay;

        private PlayerManager _playerManager;
        private ItemManager _itemManager;
        private SettingsScreen _uIManager;
        public InventoryManager _inventoryManager;

        public Vector2 Position_Feet { get => new Vector2((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f)); }
        public Vector2 Position_Head { get => new Vector2((int)(_position.X + (_width * _scale) * 0.35f), (int)(_position.Y + (_height * _scale) * 0.3f)); }
        public Rectangle Rectangle { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.35f), (int)(_position.Y + (_height * _scale) * 0.3f), (int)(_width * _scale * 0.3), (int)(_height * _scale * 0.6));}
        public Rectangle RectangleMovement { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.4f), (int)(_position.Y + (_height * _scale) * 0.8f), 7, 7); }
        public Player(AnimationManager animationManager,int animationNum, Vector2 position, Input input, int health, PlayerManager playerManager, ItemManager itemManager,InventoryManager inventoryManager,SettingsScreen uIManager,NameDisplay nameDisplay)
        {
            _animationManager = animationManager;
            _animationNum = animationNum;
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
            _nameDisplay = nameDisplay;
        }
        public void Update(GameTime gameTime)
        {
            InputReader(gameTime);

            _nameDisplay.Update(Position_Feet + new Vector2(5,20));

            _animationManager.Update(gameTime, _position);

            _animationManager.SetAnimations(_velocity,ref _hide_weapon,ref _moving_direction);
            if (CollisionManager.IsCollidingLeftWalls(RectangleMovement, _velocity) && _velocity.X < 0)
                _velocity -= new Vector2(_velocity.X,0);
            if (CollisionManager.IsCollidingRightWalls(RectangleMovement, _velocity) && _velocity.X > 0)
                _velocity -= new Vector2(_velocity.X, 0);
            if (CollisionManager.IsCollidingTopWalls(RectangleMovement, _velocity) && _velocity.Y < 0)
                _velocity -= new Vector2(0,_velocity.Y);
            if (CollisionManager.IsCollidingBottomWalls(RectangleMovement, _velocity) && _velocity.Y > 0)
                _velocity -= new Vector2(0, _velocity.Y);
            _position += _velocity;



            if (_gun != null)
            {
                _gun.Update(gameTime, _looking_direction, _moving_direction, _input._isGamePad,_gun._isSniper,_position);
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
            if (_dead)
            {
                spriteBatch.Draw(GraphicManager._deadPlayerTexture, Position_Feet, null, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, TileManager.GetLayerDepth(_position.Y));
            }
            else
            {
                if (_hide_weapon)
                {
                    if (_gun != null)
                        _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) - 0.01f);
                    if (_meleeWeapon != null)
                        _meleeWeapon.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) - 0.01f);
                }
                _animationManager.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
                if (!_hide_weapon)
                {
                    if (_gun != null)
                        _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) + 0.01f);
                    if (_meleeWeapon != null)
                        _meleeWeapon.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) + 0.01f);
                }
                _health.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            }
            _nameDisplay.Draw(spriteBatch);
            
            
        }

        public void InputReader(GameTime gameTime)
        {
            _velocity = Vector2.Zero;
            _input.GetVelocity(ref _velocity, _speed);
            _input.GetLookingDirection(ref _looking_direction, _gun, _meleeWeapon);
            if(_input.Shot())
            {
                if (!_mouseIntersectsUI)
                {
                    if (_gun != null)
                        _gun.Shot();
                }
            }
            if(_input.MeleeAttack())
            {
                if (!_mouseIntersectsUI && !_input._isGamePad)
                {
                    if (_gun != null)
                        _gun.SwingWeapon();
                    else
                        _meleeWeapon.SwingWeapon();
                }
                else if(_input._isGamePad)
                {
                    if (_gun != null)
                        _gun.SwingWeapon();
                    else
                        _meleeWeapon.SwingWeapon();
                }
            }
            if (_input.Pick())
            {
                Item item = _itemManager.findClosestItem(_position + (_animationManager.getAnimationPickPosition()));
                if (item != null)
                {
                    if (!Game_Client._isMultiplayer)
                    {
                        _inventoryManager.AddItemToInventory(item);
                    }
                    else
                    {
                        if (_inventoryManager.HasSpaceForItem(item) && ItemManager._waitingForServerApprovalForItemPicked)
                        {
                            item._aboutToBeSent = true;
                            ItemManager._waitingForServerApprovalForItemPicked = true;
                            ItemManager._itemNumWaitingForServerApproval = item._itemNum;
                            ItemManager._itemsToSendPicked.Add(item._itemNum);
                        }
                    }
                }
            }
            if(_input.MoveInventoryPointerRight())
            {
                _inventoryManager.MoveInventoryPointerRight();
            }
            if (_input.MoveInventoryPointerLeft())
            {
                _inventoryManager.MoveInventoryPointerLeft();
            }
            if (_input.ClickInventoryItemGamePad())
            {
                _inventoryManager.ClickInventoryItemGamePad();
            }
            if (_input.DropInventoryItemGamePad())
            {
                _inventoryManager.DropInventoryItemGamePad();
            }
            _mouseIntersectsUI = false;

        }
        

        public void EquipGun(Gun gun)
        {
            _gun = gun;
            _gun._holderScale = _scale;
            _gun._inventoryManager = _inventoryManager;
        }
        public void EquipMeleeWeapon(MeleeWeapon meleeWeapon)
        {
            _meleeWeapon = meleeWeapon;
            _meleeWeapon._holderScale = _scale;
        }
        public void PositionPlayerFeetAt(Vector2 position)
        {
            _position = position;
            Vector2 temp = _position - Position_Feet;
            _position += temp;
        }
        public void UpdatePacketShort(Packet packet)
        {
            packet.WriteInt(_playerNum);
            packet.WriteVector2(_position);
            packet.WriteInt(_moving_direction);
            packet.WriteInt(_health._health_left);
            packet.WriteInt(_health._total_health);
            packet.WriteVector2(_velocity);
            packet.WriteVector2(_looking_direction);
            packet.WriteInt(_animationNum);
            packet.WriteInt(_gun._id);
            
            _gun.UpdatePacketShort(packet);
        }

    }
}

