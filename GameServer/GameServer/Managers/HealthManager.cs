using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class HealthManager
    {
        public int _total_health;

        public int _health_left;

        public Vector2 _position;

        //private Texture2D _healthbar;

        //private Texture2D _healthbar_background;

        //private int healthbar_width = 30;


        public HealthManager(int total_health,Vector2 position)
        {
            _total_health = total_health;
            _health_left = total_health;
        }
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    float life_precentage = ((float)_health_left / _total_health) * healthbar_width;
        //    Color[] data = new Color[healthbar_width];
        //    for (int i = 0; i < data.Length; ++i) data[i] = (i < life_precentage ? Color.Green : Color.Red);
        //    _healthbar.SetData(data);
        //    spriteBatch.Draw(_healthbar_background, _position+ new Vector2(-1,-1), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        //    spriteBatch.Draw(_healthbar, _position, null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
        //}
        public void Update(Vector2 position)
        {
            _position = position;
        }
    }
}
