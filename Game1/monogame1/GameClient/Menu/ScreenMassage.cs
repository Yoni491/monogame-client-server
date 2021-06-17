using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    class ScreenMassage
    {
        Texture2D _texture;
        SpriteFont _font;
        Rectangle _rectangle;
        Vector2 _position;
        string _text;
        Color _background;
        public bool _displayMassage = false;
        public void Text(string text) { _text = text; }
        public ScreenMassage(string text)
        {
            _texture = GraphicManager.getRectangleTexture(450, 100, Color.Black);
            _font = GraphicManager.GetBasicFont();
            _position = new Vector2(200,100);
            _text = text;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            _background = Color.Black;

        }
       
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_displayMassage)
            {
                spriteBatch.Draw(_texture, _rectangle, null, _background, 0, Vector2.Zero, SpriteEffects.None, 0.51f);

                if (!string.IsNullOrEmpty(_text))
                {
                    var x = (_rectangle.X + (_rectangle.Width / 2)) - (_font.MeasureString(_text).X / 2);
                    var y = (_rectangle.Y + (_rectangle.Height / 2)) - (_font.MeasureString(_text).Y / 2);

                    spriteBatch.DrawString(_font, _text, new Vector2(x, y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.6f);
                }
            }
        }

        public void ResetGraphics(Vector2 position)
        {
            _position = position;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }
    }
}
