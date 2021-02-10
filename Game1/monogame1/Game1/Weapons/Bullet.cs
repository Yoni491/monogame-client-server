using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace GameClient
{
    public class Bullet
    {
        public Texture2D _texture;
        public bool _destroy = false;
        private bool _hitPlayers;
        static int s_bulletNumber = 0;
        public int _bulletNumber;
        public int _collection_id;
        public int _maxTravelDistance;
        public int _dmg;
        public float _speed;
        private float _timer = 0;
        public float _shootingTimer;
        private Vector2 _velocity;
        private Vector2 _position;
        private Vector2 _direction;
        private Vector2 _startPosition;
        public int _destroyIn3 = -1;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width / 2, _texture.Height / 2);
            }
        }

        public Bullet(int id, Texture2D texture, Vector2 position, Vector2 direction, float speed, int bulletNumber, float shootingTimer, int dmg, int travelDistance,bool hitPlayers)
        {
            _collection_id = id;
            _texture = texture;
            _startPosition = position;
            _position = position;
            _direction = Vector2.Normalize(direction);
            _speed = speed;
            _shootingTimer = shootingTimer;
            _maxTravelDistance = travelDistance;
            _hitPlayers = hitPlayers;
            if (bulletNumber < 0)
            {
                _bulletNumber = s_bulletNumber++;
                if (bulletNumber > 2000)
                    s_bulletNumber = 0;
            }
            else
            {
                _bulletNumber = bulletNumber;
            }
            _dmg = dmg;
        }
        public Bullet(int id, Texture2D texture, float speed, float shootingTimer,int dmg, int travelDistance)
        {
            _collection_id = id;
            _texture = texture;
            _speed = speed;
            _shootingTimer = shootingTimer;
            _dmg = dmg;
            _maxTravelDistance = travelDistance;
        }

        public void Update(GameTime gameTime)
        {
            if (_destroyIn3 < 0)
            {
                _velocity = _direction * _speed;
                if (_hitPlayers)
                {
                    if (CollisionManager.isColidedWithPlayer(Rectangle, _velocity, _dmg))
                        _destroy = true;
                }
                else
                {
                    if (CollisionManager.isColidedWithEnemies(Rectangle, _velocity, _dmg))
                    {
                        _destroy = true;
                    }
                    else if (CollisionManager.isCollidingBoxes(Rectangle, _velocity))
                    {
                        _destroy = true;
                    }
                }
                if (CollisionManager.isCollidingWalls(Rectangle, _velocity))
                {
                    _destroyIn3 = 3;
                }
            }
            else
            {
                if (CollisionManager.isCollidingBoxes(Rectangle, _velocity))
                {
                    _destroy = true;
                }
                if (_destroyIn3-- == -1)
                    _destroy = true;
            }

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(Vector2.Distance(_startPosition,_position)>= _maxTravelDistance)
            {
                _destroy = true;
            }
            if (_timer >= 2f)
            {
                _destroy = true;
            }
            _position += _velocity;
        }
        public Bullet Copy(Vector2 directionSpread,Vector2 position, Vector2 direction,bool hitEnemies)
        {
            return new Bullet(_collection_id,  _texture, position, directionSpread, _speed, -1, _shootingTimer, _dmg, _maxTravelDistance, hitEnemies);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 1,Vector2.Zero , 1f, SpriteEffects.None, TileManager.GetLayerDepth(_position.Y - 60));//new Vector2(4, 12)
        }
        public void readPacketShort(PacketStructure packet)
        {
            _position = packet.ReadVector2();
            _direction = packet.ReadVector2();
        }
        public void UpdatePacketShort(PacketStructure packet)
        {
            packet.WriteInt(_bulletNumber);
            packet.WriteVector2(_position);
            packet.WriteVector2(_direction);
        }
    }
}
