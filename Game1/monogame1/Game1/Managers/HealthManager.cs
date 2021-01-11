using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient
{
    public class HealthManager
    {
        public int _total_health;

        public int _health_left;

        private Vector2 _position;

        private Texture2D _healthbar;

        private Texture2D _healthbar_background;

        private int healthbar_width = 30;


        private float _playerScale;


        public HealthManager(int total_health, Vector2 position, float scale)
        {
            _total_health = total_health;
            _health_left = total_health;
            _position = position + new Vector2(20, 10) * scale;
            _healthbar = new Texture2D(Client.game.GraphicsDevice, healthbar_width, 1);
            Color[] data = new Color[healthbar_width];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Green;
            _healthbar.SetData(data);
            _healthbar_background = new Texture2D(Client.game.GraphicsDevice, healthbar_width + 2, 3);
            Color[] data2 = new Color[(healthbar_width + 2) * 3];
            for (int i = 0; i < data2.Length; ++i) data2[i] = Color.Black;
            _healthbar_background.SetData(data2);
            _playerScale = scale;
        }
        public void Update(Vector2 position)
        {
            _position = position + new Vector2(8,12)*_playerScale;
        }
        public void Draw(SpriteBatch spriteBatch, float layer)
        {
            float life_precentage = ((float)_health_left / _total_health) * healthbar_width;
            Color[] data = new Color[healthbar_width];
            for (int i = 0; i < data.Length; ++i) data[i] = (i < life_precentage ? Color.Green : Color.Red);
            _healthbar.SetData(data);
            spriteBatch.Draw(_healthbar_background, _position + new Vector2(-1, -1), null, Color.White, 0, new Vector2(0, 0), _playerScale, SpriteEffects.None, layer - 0.01f);
            spriteBatch.Draw(_healthbar, _position, null, Color.White, 0, new Vector2(0, 0), _playerScale, SpriteEffects.None, layer);
        }

    }
}
