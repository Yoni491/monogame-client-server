using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GameClient
{
    public class Bullet
    {
        public Texture2D _texture;
        public bool _destroy = false;
        private bool _hitPlayers;
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
        public bool _bulletSent;
        public int _dmgShown;
        public float _spread;
        public bool _isSniper;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width / 2, _texture.Height / 2);
            }
        }
        public Vector2 Position_Center { get => new Vector2((int)(_position.X - _texture.Width / 2), (int)(_position.Y - _texture.Height / 2)); }

        public Bullet(int id, Texture2D texture, Vector2 position, Vector2 direction,bool isSniper, float speed,float spread, float shootingTimer, int dmg, int travelDistance, bool hitPlayers)
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
            _dmg = dmg;
            _spread = spread;
            _dmgShown = dmg;
            _isSniper = isSniper;
        }
        public Bullet(int id, Texture2D texture , bool isSniper,float speed, float shootingTimer, int dmg, int travelDistance,float spread)
        {
            _collection_id = id;
            _texture = texture;
            _speed = speed;
            _shootingTimer = shootingTimer;
            _dmg = dmg;
            _maxTravelDistance = travelDistance;
            _spread = spread;
            _isSniper = isSniper;
        }
        public void Update(GameTime gameTime)
        {
            if (_destroyIn3 < 0)
            {
                _velocity = _direction * _speed;
                if (_hitPlayers)
                {
                    if (CollisionManager.isColidedWithPlayers(Rectangle, _velocity, _dmg))
                    {
                        _destroy = true;
                        DmgMassageManager.CreateDmgMessage(_dmgShown, _position + _velocity, Color.Purple, _shootingTimer);
                    }
                }
                else
                {
                    if (CollisionManager.isColidedWithEnemies(Rectangle, _velocity, _dmg))
                    {
                        _destroy = true;
                        DmgMassageManager.CreateDmgMessage(_dmgShown, _position + _velocity, Color.Orange, _shootingTimer);
                    }
                    else if (CollisionManager.isCollidingBoxes(Rectangle, _velocity, _dmg))
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
                if (CollisionManager.isCollidingBoxes(Rectangle, _velocity, _dmg))
                {
                    _destroy = true;
                }
                if (--_destroyIn3 == -1)
                    _destroy = true;
            }
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Vector2.Distance(_startPosition, _position) >= _maxTravelDistance)
            {
                _destroy = true;
            }
            if (_timer >= 2f)
            {
                _destroy = true;
            }
            _position += _velocity;
        }
        public Bullet Copy(Vector2 direction, Vector2 position, bool hitPlayers)
        {
            return new Bullet(_collection_id, _texture, position, direction, _isSniper, _speed,_spread, _shootingTimer, _dmg, _maxTravelDistance, hitPlayers);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_destroyIn3 == -1)
                spriteBatch.Draw(_texture, Position_Center, null, Color.White, 1, Vector2.Zero, 1.7f, SpriteEffects.None, TileManager.GetLayerDepth(_position.Y - 60));
        }
        public void UpdatePacketShort(Packet packet)
        {
            packet.WriteVector2(_startPosition);
            packet.WriteVector2(_direction);
            _bulletSent = true;
        }
    }
}
