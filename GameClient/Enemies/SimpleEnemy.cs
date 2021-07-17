using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GameClient
{
    public enum Direction { Up = 0, Down, Left, Right };
    public class SimpleEnemy
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
        private PathFinder _pathFinder;
        private BulletReach _bulletReach;
        public bool _destroy;
        private bool _hide_weapon;
        private bool _isStopingToShotOrMeleeAttack;
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
        private float _summonTimer = 0;
        public int _dmgDoneForServer=0;
        private int _summonEnemyID;
        private List<SimpleEnemy> _summonedEnemies;
        
        //public Vector2 Position_Feet { get => _position + new Vector2(_width / 2, _height * 2 / 3); }
        public Vector2 Position_Feet { get => new Vector2((int)(_position.X + (_width * _scale) * 0.3f), (int)(_position.Y + (_height * _scale) * 0.8f)); }
        public Vector2 Position_Head { get => new Vector2((int)(_position.X + (_width * _scale) * 0.35f), (int)(_position.Y + (_height * _scale) * 0.3f)); }
        public Rectangle Rectangle { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.35f), (int)(_position.Y + (_height * _scale) * 0.3f), (int)(_width * _scale * 0.3), (int)(_height * _scale * 0.6)); }
        public Rectangle RectangleMovement { get => new Rectangle((int)(_position.X + (_width * _scale) * 0.5f), (int)(_position.Y + (_height * _scale) * 0.9f), 7,7); }
        public SimpleEnemy(AnimationManager animationManager, int enemyId, Vector2 position, float speed, PlayerManager playerManager, ItemManager itemManager,
            int health, int[] items_drop_list, MeleeWeapon meleeWeapon, Gun gun = null, PathFinder pathFinder = null, BulletReach bulletReach = null, int enemyNum = -1, int summonEnemyID = -1)
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
            _summonEnemyID = summonEnemyID;
            _summonedEnemies = new List<SimpleEnemy>();
        }
        public void Update(GameTime gameTime)
        {
            Move(gameTime);

            if(!Game_Client._isServer && Game_Client._isMultiplayer)
                UpdateFromServer();

            _animationManager.Update(gameTime, _position);

            _health.Update(_position);

            MeleeCombatAlgorithm(gameTime);

            ShootingAlgorithm(gameTime);

            SummonEnemies(gameTime);
        }
        private void UpdateFromServer()
        {
            _animationManager.SetAnimationsFromServer(_moving_direction);
        }
        private void SummonEnemies(GameTime gameTime)
        {
            if(_summonEnemyID!=-1)
            {
                _summonedEnemies.RemoveAll(enemy => enemy._destroy == true);
                _summonTimer += (float)gameTime.ElapsedGameTime.Milliseconds;
                if (_summonTimer >= 2000 && _summonedEnemies.Count < 5)
                {
                    _summonTimer = 0;
                    _summonedEnemies.Add(EnemyManager.AddEnemyAtPosition(_summonEnemyID, Position_Feet, true, false));
                }
            }
        }
        private void MeleeCombatAlgorithm(GameTime gameTime)
        {
            if (_meleeWeapon != null)
            {
                _meleeWeapon.Update(_moving_direction, gameTime, _position);
                if (!_meleeWeapon._swing_weapon)
                    _position += _velocity;
                if (_isStopingToShotOrMeleeAttack)
                {
                    _meleeWeapon.SwingWeapon();
                }
            }
        }
        private void ShootingAlgorithm(GameTime gameTime)
        {
            if (_gun != null)
            {
                if (!Game_Client._isMultiplayer)
                {
                    _sniperTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_gun._isSniper)
                    {
                        if (_isStopingToShotOrMeleeAttack)
                        {
                            _gun.Update(gameTime, _shootingDirection, 0, false, true, _position);
                            if (_sniperTimer >= _sniperStopTime && _bulletReach._reachablePlayerPos != Vector2.Zero)
                            {
                                _sniperTimer = 0;
                                _gun.Shot();
                                _isStopingToShotOrMeleeAttack = false;
                            }
                        }
                        else
                        {
                            _gun.Update(gameTime, _shootingDirection, 0, false, false, _position);
                            _position += _velocity;
                            if (_sniperTimer >= _movingBetweenShotsTime)
                            {
                                _sniperTimer = 0;
                                _isStopingToShotOrMeleeAttack = true;
                            }
                        }
                    }
                    else
                    {
                        _position += _velocity;
                        _gun.Update(gameTime, _shootingDirection, 0, false, false, _position);
                        if (_velocity == Vector2.Zero && _shootingDirection != Vector2.Zero)
                            _gun.Shot();
                    }
                }
                else
                {
                    _gun.Update(gameTime, _shootingDirection, 0, false, _isStopingToShotOrMeleeAttack, _position);
                }
            }
        }

        private void Move(GameTime gameTime)
        {
            Vector2 target_player;
            if (!Game_Client._isMultiplayer)
            {
                if (_gun != null)
                {
                    if (_bulletReach._reachablePlayerPos != Vector2.Zero)
                    {
                        target_player = _bulletReach._reachablePlayerPos;
                        _shootingDirection = target_player - _gun.GetTipOfTheGun(target_player);
                    }
                    else
                    {
                        target_player = _playerManager.getClosestPlayerToPosition(Position_Feet);
                    }
                }                    
                else
                {
                    target_player = _playerManager.getClosestPlayerToPosition(Position_Feet);
                }
                _pathFinder.Update(gameTime, Position_Feet, target_player);

                Vector2 coordPosition = _pathFinder.GetNextCoordPosition();
                if (coordPosition == Vector2.Zero)
                {
                    _velocity = Vector2.Zero;
                    Vector2 direction = target_player - Position_Feet;
                    _animationManager.SetAnimations(direction, ref _hide_weapon, ref _moving_direction);
                }
                else
                {
                    _velocity = Vector2.Normalize(coordPosition - Position_Feet);
                    _animationManager.SetAnimations(_velocity, ref _hide_weapon, ref _moving_direction);
                }
                if (Vector2.Distance(target_player, Position_Feet) < _movingToPlayerMaxDistance)
                {
                    if(_meleeWeapon!=null)
                        _isStopingToShotOrMeleeAttack = true;
                    _velocity = Vector2.Zero;
                }
                else
                {
                    if (_meleeWeapon != null)
                        _isStopingToShotOrMeleeAttack = false;
                }
                _velocity = _velocity * _speed;
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
        
        public void PositionFeetAt(Vector2 position)
        {
            _position = position;
            Vector2 temp = _position - Position_Feet;
            _position += temp;
        }
        public void DealDamage(int dmg)
        {
            _dmgDoneForServer += dmg;
            if (!Game_Client._isMultiplayer)
            {
                _health._health_left -= dmg;

                if (_health._health_left <= 0 && _destroy == false)
                {
                    _destroy = true;
                    if (!Game_Client._isMultiplayer)
                    {
                        PathFindingManager.RemovePathFinder(_pathFinder);
                        ItemManager.DropItemFromList(_items_drop_list, Position_Feet);
                        foreach (var item in _summonedEnemies)
                        {
                            item.DealDamage(1000);
                        }
                    }
                }
            }
        }
        public SimpleEnemy Copy(bool useAstar, bool waitForDestroyedWall)
        {
            int enemyNum = -1;
            if (!Game_Client._isMultiplayer)
                enemyNum = _s_enemyNum++;
            Gun gun = null;
            if (_gun != null)
                gun = _gun.Copy(true, true, null);
            BulletReach bulletReach = null;
            if (gun != null)
                bulletReach = BulletReachManager.GetBulletReach(gun);
            MeleeWeapon meleeWeapon = null;
            if (_meleeWeapon != null)
                meleeWeapon = _meleeWeapon.Copy(true, true, null);
            return new SimpleEnemy(_animationManager.Copy(), _enemyId, _position, _speed,
                _playerManager, _itemManager, _health._total_health, _items_drop_list, meleeWeapon, gun, PathFindingManager.GetPathFinder(useAstar, waitForDestroyedWall),
                bulletReach, enemyNum,_summonEnemyID);
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
            packet.WriteBool(_isStopingToShotOrMeleeAttack);
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
            _isStopingToShotOrMeleeAttack = packet.ReadBool();
            int gunOrMeele = packet.ReadInt();//gun is 0
            if (gunOrMeele == 0)
            {
                _gun.ReadPacketShort(packet);
            }
        }
    }
}
