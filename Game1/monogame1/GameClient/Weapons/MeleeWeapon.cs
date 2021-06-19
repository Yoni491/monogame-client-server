using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace GameClient
{
    public class MeleeWeapon
    {
        public int _id;
        private Vector2 _position;
        private int _moving_direction_int;
        private float _swing_range;
        private Texture2D _texture;
        public bool _swing_weapon;
        public float _holderScale;
        private float _swing_timer;
        private float _swing_frame_window = 0.01f;
        private float _swing_frame_timer = 0;
        private float _between_attacks_timer;
        private float _between_attacks_timer_window = 0.2f;
        private float swingSpeed = 14;
        private int _dmg;
        public int _maxAttackingDistance = 60;
        private bool _isColided = false;
        public bool _hitPlayers;
        private bool _dealDmg;
        private InventoryManager _inventoryManager;
        private bool _isColidedBox;
        private Chest _colidedChest;


        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)(_texture.Width * _holderScale), (int)(_texture.Height * _holderScale));
            }
        }
        public Vector2 Position { get => _position; set => _position = value; }
        public MeleeWeapon(int id, Texture2D texture, Vector2 position, float swing_range, int dmg, bool hitPlayers, bool dealDmg, InventoryManager inventoryManager)
        {
            _id = id;
            _texture = texture;
            _position = position;
            _swing_range = swing_range;
            _dmg = dmg;
            _hitPlayers = hitPlayers;
            _dealDmg = dealDmg;
            _inventoryManager = inventoryManager;
        }
        public void Update(int direction,GameTime gameTime,Vector2 position)
        {
            _moving_direction_int = direction;
            MelleAttackUpdate(gameTime, position);
        }
        public void MelleAttackUpdate(GameTime gameTime, Vector2 position)
        {
            if (_swing_weapon)
            {
                SwingUpdate(gameTime);
                Rectangle swingRectangle;
                if (_moving_direction_int == (int)Direction.Right || _moving_direction_int == (int)Direction.Left)
                    swingRectangle = new Rectangle((int)_position.X, (int)_position.Y - 16, 16, 48);
                else
                    swingRectangle = new Rectangle((int)_position.X - 16, (int)_position.Y + 16, 48, 16);
                _swing_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_hitPlayers)
                {
                    if (!_isColided && CollisionManager.isColidedWithPlayer(swingRectangle, Vector2.Zero, 5))
                    {
                        _isColided = true;
                    }
                }
                else
                {
                    if (!_isColided && CollisionManager.isColidedWithEnemies(swingRectangle, Vector2.Zero, 5))
                    {
                        _isColided = true;
                    }
                    else if (!_isColided && CollisionManager.isCollidingBoxes(swingRectangle, Vector2.Zero, 5))
                    {
                        _isColidedBox = true;
                        while (CollisionManager.isCollidingBoxes(swingRectangle, Vector2.Zero, 5))
                        {

                        }
                    }
                    else if (!_isColidedBox && !_isColided)
                    {
                        _colidedChest = CollisionManager.isCollidingChests(swingRectangle, Vector2.Zero);
                        if (_colidedChest != null)
                        {
                            _isColided = true;
                        }
                        Door _colidedDoor = CollisionManager.IsCollidingDoors(swingRectangle, Vector2.Zero);
                        if (_colidedDoor != null)
                        {
                            if (_inventoryManager.RemoveItemFromInventory(11))
                            {
                                _colidedDoor.Destroy();
                            }
                        }
                    }
                }
            }
            else
            {
                _position = position + new Vector2(23, 40) * _holderScale;
                _between_attacks_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (_swing_timer >= 0.1f)
            {
                if (_colidedChest != null && !_isColidedBox)
                    _colidedChest.Open();
                _swing_timer = 0;
                _swing_weapon = false;
                _position = position + new Vector2(23, 44) * _holderScale;
                _isColided = false;
                _isColidedBox = false;
                _between_attacks_timer = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch, float layer)
        {
            if (_moving_direction_int == (int)Direction.Up)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(4, 12), _holderScale * 0.5f, SpriteEffects.None, layer);
            }
            else if (_moving_direction_int == (int)Direction.Down)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(-12, 12), _holderScale * 0.5f, SpriteEffects.FlipHorizontally, layer);
            }
            else if (_moving_direction_int == (int)Direction.Right)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(4, 12), _holderScale * 0.5f, SpriteEffects.FlipHorizontally, layer);
            }
            else if (_moving_direction_int == (int)Direction.Left)
            {
                spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(24, 12), _holderScale * 0.5f, SpriteEffects.None, layer);
            }
        }
        public MeleeWeapon Copy(bool hitPlayers, bool dealDmg, InventoryManager inventoryManager)
        {
            return new MeleeWeapon(_id, _texture, _position, _swing_range, _dmg,hitPlayers,dealDmg,inventoryManager);
        }
        public void SwingWeapon()
        {
            if (_between_attacks_timer > _between_attacks_timer_window && !_swing_weapon)
            {
                AudioManager.PlaySound("SwingWeapon");
                _swing_weapon = true;
            }
        }
        private void SwingUpdate(GameTime gameTime)
        {
            _swing_frame_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_swing_frame_timer > _swing_frame_window)
            {
                _swing_frame_timer = 0;
                if (_moving_direction_int == (int)Direction.Up)
                {
                    _position += new Vector2(0, -swingSpeed);
                }
                else if (_moving_direction_int == (int)Direction.Down)
                {
                    _position += new Vector2(0, swingSpeed);
                }
                else if (_moving_direction_int == (int)Direction.Right)
                {
                    _position += new Vector2(swingSpeed, 0);
                }
                else if (_moving_direction_int == (int)Direction.Left)
                {
                    _position +=  new Vector2(-swingSpeed, 0);
                }
            }
        }

    }
}
