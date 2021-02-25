using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GameClient
{
    public enum Direction { Up = 0, Down, Left, Right };
    public class Simple_Enemy
    {
        int _enemyId;
        static int _s_enemyNum=0;
        public int _enemyNum;
        private MeleeWeapon _meleeWeapon;
        public Gun _gun;
        private AnimationManager _animationManager;
        private PlayerManager _playerManager;
        public HealthManager _health;
        private ItemManager _itemManager;
        private Vector2 _velocity;
        public Vector2 _position;
        private Vector2 _shootingDirection = Vector2.Zero;
        private Vector2 _movingDirection;
        private PathFinder _pathFinder;
        private BulletReach _bulletReach;
        public bool _destroy = false;
        private bool _hide_weapon;
        private bool stopMoving;
        private bool _isStopingToAttack=false;
        private int[] _items_drop_list;
        private int _moving_direction;
        private int _width;
        private int _height;
        private float _speed;
        private float _scale;
        private float _sniperTimer = 0;
        private float _sniperStopTime = 1f;
        private float _movingBetweenShotsTime = 1f;
        private float _movingToPlayerMaxDistance = 1;
        public int _dmgDoneForServer=0;
        
        //public Vector2 Position_Feet { get => _position + new Vector2(_width / 2, _height * 2 / 3); }
        public Vector2 Position_Feet { get => new Vector2((int)(_position.X + (_width * _scale) * 0.3f), (int)(_position.Y + (_height * _scale) * 0.8f)); }

        public Rectangle Rectangle { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.35f), (int)(_position.Y + (_height * _scale) * 0.3f), (int)(_width * _scale * 0.3), (int)(_height * _scale * 0.6)); }

        public Rectangle RectangleMovement { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.5f), (int)(_position.Y + (_height * _scale) * 0.9f), (int)(_width * _scale * 0.1), (int)(_height * _scale * 0.1)); }
        public Simple_Enemy(AnimationManager animationManager, int enemyId, Vector2 position, float speed, PlayerManager playerManager, ItemManager itemManager, int health, int[] items_drop_list, MeleeWeapon meleeWeapon, Gun gun, PathFinder pathFinder, BulletReach bulletReach, int enemyNum = -1)
        {
            _enemyId = enemyId;
            _animationManager = animationManager;
            _position = position;
            _animationManager._position = _position;
            _playerManager = playerManager;
            _items_drop_list = items_drop_list;
            _itemManager = itemManager;
            _speed = speed;
            _meleeWeapon = meleeWeapon;
            _scale = animationManager._scale;
            if (meleeWeapon != null)
                _meleeWeapon._holderScale = _scale;
            _health = new HealthManager(health, position + new Vector2(8, 10), _scale);
            _width = _animationManager.Animation._frameWidth;
            _height = _animationManager.Animation._frameHeight;
            _gun = gun;
            if (gun != null)
            {
                _movingToPlayerMaxDistance = Math.Min(_gun._bullet._maxTravelDistance - 30, 1500);
                _gun._holderScale = _scale;
            }
            if(_meleeWeapon!=null)
            {
                _movingToPlayerMaxDistance = _meleeWeapon._maxAttackingDistance;
            }
            _pathFinder = pathFinder;
            _bulletReach = bulletReach;
            if (enemyNum == -1)
                _enemyNum = _s_enemyNum++;
            else
                _enemyNum = enemyNum;
        }
        public void Update(GameTime gameTime)
        {
            
            Move(gameTime);

            if (stopMoving)
            {
                _velocity = new Vector2(0, 0);
            }
            SetAnimations();
            if (!Game_Client._IsMultiplayer)
            {
                _velocity = _velocity * _speed;
            }

            _animationManager.Update(gameTime, _position);

            if (_meleeWeapon != null)
            {
                _meleeWeapon.Update(_moving_direction, gameTime, _position);
                if (!_meleeWeapon._swing_weapon)
                    _position += _velocity;
                if (stopMoving)
                    _meleeWeapon.SwingWeapon();
            }

            _health.Update(_position);
            
            if (_gun != null)
            {
                if (!Game_Client._IsMultiplayer)
                {
                    _sniperTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_gun._isSniper)
                    {
                        if (_isStopingToAttack)
                        {
                            _gun.Update(gameTime, _shootingDirection, 0, false, true, _position);
                            if (_sniperTimer >= _sniperStopTime)
                            {
                                _sniperTimer = 0;
                                _gun.Shot();
                                _isStopingToAttack = false;
                            }
                        }
                        else
                        {
                            _gun.Update(gameTime, _shootingDirection, 0, false, false, _position);
                            _position += _velocity;
                            if (_sniperTimer >= _movingBetweenShotsTime && _bulletReach._reachablePlayerPos!=Vector2.Zero)
                            {
                                _sniperTimer = 0;
                                _isStopingToAttack = true;
                            }
                        }
                    }
                    else
                    {
                        _position += _velocity;
                        _gun.Update(gameTime, _shootingDirection, 0, false, false, _position);
                        if (stopMoving)
                            _gun.Shot();
                    }
                }
                else
                {
                    _gun.Update(gameTime, _shootingDirection, 0, false, _isStopingToAttack, _position);
                }
            }
            
        }
        

        public void Move(GameTime gameTime)
        {

            Vector2 target_player;
            if (!Game_Client._IsMultiplayer)
            {
                _bulletReach.Update(Position_Feet);
                if (_gun != null && _gun._isSniper)
                {
                    if (_bulletReach._reachablePlayerPos != Vector2.Zero)
                    {
                        target_player = _bulletReach._reachablePlayerPos;
                        _shootingDirection = _gun._MaxPointBulletReach - _gun._tipOfTheGun;
                        _bulletReach._reachablePlayerPos = Vector2.Zero;
                    }
                    else
                    {
                        target_player = _playerManager.getClosestPlayerToPosition(Position_Feet);
                    }
                    
                    _pathFinder.Update(gameTime, Position_Feet, target_player);
                }                    
                else
                {
                    target_player = _playerManager.getClosestPlayerToPosition(Position_Feet);
                }
                
                Vector2 coordPosition = _pathFinder.GetNextCoordPosition();
                _velocity = Vector2.Normalize(coordPosition - Position_Feet);
                if (coordPosition == Vector2.Zero)
                    _velocity = Vector2.Zero;
                
                if (Vector2.Distance(target_player, Position_Feet) > _movingToPlayerMaxDistance)
                {
                    stopMoving = false;
                }
                else
                {
                    stopMoving = true;
                    if (_meleeWeapon != null)
                        _meleeWeapon.SwingWeapon();
                }
            }
            if (_velocity.X > Math.Abs(_velocity.Y))
            {
                _hide_weapon = false;
                _moving_direction = (int)Direction.Right;
            }
            else if (-_velocity.X > Math.Abs(_velocity.Y))
            {
                _hide_weapon = false;
                _moving_direction = (int)Direction.Left;
            }
            else if (_velocity.Y > 0)
            {
                _hide_weapon = false;
                _moving_direction = (int)Direction.Down;
            }
            else if (_velocity.Y < 0)
            {
                _hide_weapon = true;
                _moving_direction = (int)Direction.Up;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            _health.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y));
            if (_hide_weapon)
            {
                if (_meleeWeapon != null)
                    _meleeWeapon.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) - 0.01f);
                if (_gun != null)
                    _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) - 0.01f);
            }
            else
            {
                if (_meleeWeapon != null)
                    _meleeWeapon.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) + 0.01f);
                if (_gun != null)
                    _gun.Draw(spriteBatch, TileManager.GetLayerDepth(_position.Y) + 0.01f);
            }
        }
        public Simple_Enemy Copy()
        {
            int enemyNum = -1;
            if (!Game_Client._IsMultiplayer)
                enemyNum = _s_enemyNum++;
            Gun gun = null;
            if (_gun != null)
                gun = _gun.Copy(true, true, null);
            return new Simple_Enemy(_animationManager.Copy(_scale), _enemyId, _position, _speed,
                _playerManager, _itemManager, _health._total_health, _items_drop_list, _meleeWeapon, gun, PathFindingManager.GetPathFinder(), BulletReachManager.GetBulletReach(), enemyNum);
        }
        protected void SetAnimations()
        {
            if (stopMoving)
            {
                if (_meleeWeapon != null)
                {
                    if (!_meleeWeapon._swing_weapon)
                        _animationManager.Animation = _animationManager._animations[_moving_direction];
                }
                else
                {
                    _animationManager.Animation = _animationManager._animations[_moving_direction];
                }
                _animationManager.Stop();
            }
            if (!stopMoving)
            {
                _animationManager.Play(_moving_direction);
            }

        }
        public void PositionFeetAt(Vector2 position)
        {
            _position = position;
            Vector2 temp = _position - Position_Feet;
            _position += temp;
        }
        public void DealDamage(int dmg)
        {
            _dmgDoneForServer += dmg;
            if (!Game_Client._IsMultiplayer)
            {
                _health._health_left -= dmg;

                if (_health._health_left <= 0 && _destroy == false)
                {
                    _destroy = true;
                    if (!Game_Client._IsMultiplayer)
                    {
                        PathFindingManager.RemovePathFinder(_pathFinder);
                        ItemManager.DropItem(_items_drop_list, _position);
                    }
                }
            }
        }
        public void UpdatePacketDmg(Packet packet)
        {
            if(_dmgDoneForServer>0)
            {
                packet.WriteInt(_enemyNum);
                packet.WriteInt(_dmgDoneForServer);
                _dmgDoneForServer = 0;
            }
        }
        public void UpdatePacketShort(Packet packet)
        {
            packet.WriteInt(_enemyNum);
            packet.WriteInt(_enemyId);
            packet.WriteVector2(_position);
            packet.WriteInt(_health._health_left);
            packet.WriteInt(_health._total_health);
            packet.WriteVector2(_velocity);
            packet.WriteVector2(_shootingDirection);
            packet.WriteInt(_moving_direction);
            packet.WriteBool(_isStopingToAttack);
            if (_gun != null)
            {
                packet.WriteInt(0);//gun is 0
                packet.WriteInt(_gun._bullets.FindAll(x => x._bulletSent == false).Count());
                _gun.UpdatePacketShort(packet);
            }
            else if (_meleeWeapon != null)
            {
                packet.WriteInt(1);
            }
        }
        public void ReadPacketShort(Packet packet)
        {
            _position = packet.ReadVector2();
            _health._health_left = packet.ReadInt();
            _health._total_health = packet.ReadInt();
            _velocity = packet.ReadVector2();
            _shootingDirection = packet.ReadVector2();
            _moving_direction = packet.ReadInt();
            _isStopingToAttack = packet.ReadBool();
            int gunOrMeele = packet.ReadInt();//gun is 0
            if (gunOrMeele == 0)
            {
                _gun.ReadPacketShort(packet);
            }
        }
    }
}
