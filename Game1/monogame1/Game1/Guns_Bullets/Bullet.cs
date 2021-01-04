using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace GameClient
{
    public class Bullet
    {
        static int s_bulletNumber = 0;

        public int _bulletNumber;

        public int _collection_id;

        public float _speed;

        public Texture2D _texture;

        Vector2 _position;

        private float _timer = 0;

        private Vector2 _direction;

        public bool _destroy = false;

        private Gun _gun;

        private List<Simple_Enemy> _enemies;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            }
        }

        public Bullet(int id, Gun gun, Texture2D texture, Vector2 position, Vector2 direction, List<Simple_Enemy> enemies, float speed, int bulletNumber)
        {
            _collection_id = id;
            _gun = gun;
            _texture = texture;
            _position = position;
            _direction = Vector2.Normalize(direction);
            _enemies = enemies;
            _speed = speed;
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
        }
        public Bullet(int id, Texture2D texture, List<Simple_Enemy> enemies, float speed)
        {
            _collection_id = id;
            _texture = texture;
            _speed = speed;
            _enemies = enemies;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 1, new Vector2(4, 12), 0.5f, SpriteEffects.FlipHorizontally, TileManager.GetLayerDepth(_position.Y - 60));
        }
        public void Update(GameTime gameTime, List<Simple_Enemy> enemies)
        {
            _enemies = enemies;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 2f)
            {
                _destroy = true;
            }
            _position += _direction * _speed;
            if (_enemies != null)
                foreach (var enemy in _enemies)
                {
                    if (enemy.isCollision(this))
                        _destroy = true;
                }
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
