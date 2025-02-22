﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GameClient
{
    public class Gun
    {
        public int _id;
        public Vector2 _position;
        private Texture2D _texture;
        public List<Bullet> _bullets = new List<Bullet>();
        private Vector2 _direction;
        private List<SimpleEnemy> _enemies;
        public Bullet _bullet;
        public Vector2 Position { get => _position; set => _position = value; }
        public bool _isSniper;
        private bool _isGamePad;
        private bool _showLine;
        private float _spread;
        public float _holderScale = 0;
        private float _shooting_timer = 0;
        private bool _hitPlayers;
        public Vector2 _MaxPointBulletReach;
        public Vector2 _tipOfTheGun;
        private bool _dealDmg;
        public Inventory _inventoryManager;
        #region meleeAttackVariables
        private int _moving_direction_int;
        private int _swingWeaponDmg = 5;
        private float _between_attacks_timer;
        private float _swing_timer;
        private float _between_attacks_timer_window = 0.2f;
        private float _swing_frame_window = 0.01f;
        private float _swing_frame_timer = 0;
        private float swingSpeed = 14;
        private bool _isColided = false, _isColidedBox;
        public bool _swing_weapon;
        private bool _swingWeapon_send;
        private Chest _colidedChest;
        private bool _currentlyShooting;
        AnimationManager _animationManager;
        private float _animationStopAfterBulletFiredTime = 0.1f;
        private float _animationStopAfterBulletFiredTimer = 0;

        #endregion
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y - 4, (int)(_texture.Width * _holderScale * 0.4f), (int)(_texture.Height * _holderScale * 0.4f));
            }
        }
        public Gun(int id, Texture2D texture, AnimationManager animationManager, Vector2 position, List<SimpleEnemy> enemies, Bullet bullet, bool hitPlayers, bool dealDmg)
        {
            _animationManager = animationManager;
            _id = id;
            _texture = texture;
            _enemies = enemies;
            _bullet = bullet;
            _position = position;
            _isSniper = bullet._isSniper;
            _spread = bullet._spread;
            _hitPlayers = hitPlayers;
            _dealDmg = dealDmg;
        }
        public void Update(GameTime gameTime, Vector2 direction, int moving_direction, bool isGamePad, bool showLine, Vector2 position)
        {
            if (!_swing_weapon)
                _moving_direction_int = moving_direction;
            MelleAttackUpdate(gameTime, position);
            _direction = Vector2.Normalize(direction);
            foreach (var bullet in _bullets)
            {
                bullet.Update(gameTime);
            }
            if (Game_Client._isServer)
            {
                _bullets.RemoveAll(bullet => bullet._destroy && bullet._bulletSent);
            }
            else
            {
                _bullets.RemoveAll(bullet => bullet._destroy);
            }
            _isGamePad = isGamePad;
            _shooting_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _showLine = showLine;
            _tipOfTheGun = _position + Vector2.Normalize(_direction) * _texture.Width / 2 + new Vector2(0, 5);
            _animationStopAfterBulletFiredTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _animationManager.Update(gameTime, _position);
        }
        private void MelleAttackCollision(GameTime gameTime, Vector2 position)
        {
            if (_swing_weapon)
            {
                Rectangle swingRectangle;
                if (_moving_direction_int == (int)Direction.Right || _moving_direction_int == (int)Direction.Left)
                    swingRectangle = new Rectangle((int)_position.X, (int)_position.Y - 16, 16, 48);
                else
                    swingRectangle = new Rectangle((int)_position.X - 16, (int)_position.Y + 16, 48, 16);
                if (_hitPlayers)
                {
                    if (!_isColided && CollisionManager.isColidedWithPlayers(swingRectangle, Vector2.Zero, _swingWeaponDmg))
                    {
                        _isColided = true;
                        DmgMassageManager.CreateDmgMessage(_swingWeaponDmg, _position, Color.Purple, _shooting_timer);
                    }
                }
                else
                {
                    if (!_isColided && CollisionManager.isColidedWithEnemies(swingRectangle, Vector2.Zero, _swingWeaponDmg))
                    {
                        DmgMassageManager.CreateDmgMessage(_swingWeaponDmg, _position, Color.Orange, _shooting_timer);
                        _isColided = true;
                    }
                    else if (!_isColided && CollisionManager.isCollidingBoxes(swingRectangle, Vector2.Zero, _swingWeaponDmg))
                    {
                        _isColidedBox = true;
                        while (CollisionManager.isCollidingBoxes(swingRectangle, Vector2.Zero, _swingWeaponDmg))
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
                            if (_inventoryManager.RemoveItemFromInventory(6))
                            {
                                _colidedDoor.Destroy();
                            }
                        }
                    }
                }
            }
        }
        private void MelleAttackUpdate(GameTime gameTime, Vector2 position)
        {
            if (_swing_weapon)
            {
                SwingPositionUpdate(gameTime);
                if (_dealDmg)
                    MelleAttackCollision(gameTime, position);
                _swing_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            else
            {
                _position = position + new Vector2(23, 40) * _holderScale;
                _between_attacks_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        public void DrawSwing(SpriteBatch spriteBatch, float layer)
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
        public void Draw(SpriteBatch spriteBatch, float layer)
        {
            if (layer > 1)
                layer = 1;
            //GraphicManager.DrawRectangle(spriteBatch, Rectangle, layer);
            if (_swing_weapon)
                DrawSwing(spriteBatch, layer);
            else
            {
                DrawGunSprite(spriteBatch, layer, _texture);
            }
            _animationManager.DrawGun(spriteBatch, layer);
        }
        public void DrawGunSprite(SpriteBatch spriteBatch, float layer, Texture2D texture)
        {
            float rotation = (float)Math.Atan2(_direction.Y, _direction.X);
            if (rotation > -Math.PI / 2 && rotation < Math.PI / 2)
            {
                _animationManager.SetAnimationsGun(_currentlyShooting, rotation, new Vector2(4, 12), _holderScale * 0.5f, SpriteEffects.FlipHorizontally);
            }
            else
            {
                _animationManager.SetAnimationsGun(_currentlyShooting, rotation, new Vector2(4, 20), _holderScale * 0.5f, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
            }
            _currentlyShooting = false;
            foreach (var bullet in _bullets)
            {
                bullet.Draw(spriteBatch);
            }
            if (_isSniper && _showLine)
            {
                if (!_hitPlayers)
                    BulletReach();
                if (_isGamePad)
                {
                    GraphicManager.DrawLine(_tipOfTheGun, _MaxPointBulletReach, spriteBatch);
                }
                else
                {
                    if (Vector2.Distance(_tipOfTheGun, _MaxPointBulletReach) > 30)
                        GraphicManager.DrawLine(_tipOfTheGun, _MaxPointBulletReach, spriteBatch);
                }
            }
        }
        public Gun Copy(bool hitPlayers, bool dealDmg, Inventory inventoryManager)
        {
            Gun gun = new Gun(_id, _texture, _animationManager.Copy(), _position, _enemies, _bullet, hitPlayers, dealDmg);
            return gun;
        }
        public void Shot()
        {
            if (!_swing_weapon)
            {
                if (_shooting_timer >= _bullet._shootingTimer)
                {
                    _animationStopAfterBulletFiredTimer = 0;
                    _shooting_timer = 0;
                    _currentlyShooting = true;
                    Vector2 _directionSpread;
                    if (_spread != 0)
                    {
                        Random x = new Random();
                        _directionSpread = _direction + new Vector2(((float)x.NextDouble() - 0.5f) * _spread, ((float)x.NextDouble() - 0.5f) * _spread);
                    }
                    else
                    {
                        _directionSpread = _direction;
                    }
                    Bullet bullet = _bullet.Copy(_directionSpread, _tipOfTheGun, _hitPlayers);
                    if (!_dealDmg)
                    {
                        bullet._dmg = 0;
                    }
                    if (_bullet._shootingTimer < 0.1f)
                        AudioManager.PlaySound("Rifle", 0.2f);
                    else
                        AudioManager.PlaySound("Rifle");
                    _bullets.Add(bullet);
                }
            }
        }
        private void SwingPositionUpdate(GameTime gameTime)
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
                    _position += new Vector2(-swingSpeed, 0);
                }
            }
        }
        public void SwingWeapon()
        {
            if (_between_attacks_timer > _between_attacks_timer_window && !_swing_weapon)
            {
                AudioManager.PlaySound("SwingWeapon");
                _swing_weapon = true;
                _swingWeapon_send = true;
            }
        }
        public bool BulletReach()
        {
            Vector2 tempPos;
            tempPos = _tipOfTheGun;
            Rectangle tempRec;
            while (true)
            {
                if (tempPos.X < 2000 && tempPos.X > 0 && tempPos.Y < 2000 && tempPos.Y > 0)
                {
                    tempRec = new Rectangle((int)tempPos.X, (int)tempPos.Y, _bullet.Rectangle.Width, _bullet.Rectangle.Height);
                    if (_hitPlayers && CollisionManager.isColidedWithPlayers(tempRec, Vector2.Zero, 0))
                    {
                        _MaxPointBulletReach = tempPos;
                        return true;
                    }
                    else if (_hitPlayers && CollisionManager.isColidedWithNetworkPlayers(tempRec, Vector2.Zero, 0))
                    {
                        _MaxPointBulletReach = tempPos;
                        return true;
                    }
                    else if (!_hitPlayers && CollisionManager.isColidedWithEnemies(tempRec, Vector2.Zero, 0))
                    {
                        _MaxPointBulletReach = tempPos;
                        return true;
                    }
                    if (CollisionManager.isCollidingWalls(tempRec, _direction * _bullet._speed))
                    {
                        _MaxPointBulletReach = tempPos;
                        return false;
                    }
                    tempPos += _direction * _bullet._speed;
                }
                else
                {
                    _MaxPointBulletReach = tempPos;
                    return false;
                }
            }
        }
        public Vector2 GetTipOfTheGun(Vector2 target)
        {
            Vector2 direction = target - _position;
            return _position + Vector2.Normalize(direction) * _texture.Width / 2 + new Vector2(0, 5);
        }
        public void UpdatePacketShort(Packet packet)
        {
            packet.WriteBool(_swingWeapon_send);
            if (_swingWeapon_send)
                _swingWeapon_send = false;
            packet.WriteInt(_bullets.FindAll(x => x._bulletSent == false).Count());
            foreach (var bullet in _bullets)
            {
                if (!bullet._bulletSent)
                    bullet.UpdatePacketShort(packet);
            }
        }
        public void ReadPacketShort(Packet packet, bool playSound)
        {
            _swing_weapon = packet.ReadBool();
            _swingWeapon_send = _swing_weapon;
            if (!Game_Client._isServer && _swing_weapon)
            {
                if (playSound)
                    AudioManager.PlaySound("SwingWeapon");
            }
            int bulletAmount = packet.ReadInt();
            for (int i = 0; i < bulletAmount; i++)
            {
                Vector2 position = packet.ReadVector2();
                Vector2 direction = packet.ReadVector2();
                Bullet bullet = _bullet.Copy(direction, position, _hitPlayers);
                if (!_dealDmg)
                    bullet._dmg = 0;
                if (!Game_Client._isServer)
                    bullet._bulletSent = true;
                _bullets.Add(bullet);
                if (playSound)
                {
                    if (_bullet._shootingTimer < 0.1f)
                        AudioManager.PlaySound("Rifle", 0.2f);
                    else
                        AudioManager.PlaySound("Rifle");
                }
            }
        }
    }
}
