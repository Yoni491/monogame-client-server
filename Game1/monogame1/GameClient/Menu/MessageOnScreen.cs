using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient.Menu
{
    class MessageOnScreen
    {
        Texture2D _texture;
        SpriteFont _font;
        Rectangle _rectangle;
        Vector2 _position;
        string _text;
        Color _background;
        public void Text(string text) { _text = text; }
        public MessageOnScreen(Texture2D texture, SpriteFont font, Vector2 position, string text,Color background)
        {
            _texture = texture;
            _font = font;
            _position = position;
            _text = text;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            _background = background;

        }
        

        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Color _color = _background;

            spriteBatch.Draw(_texture, _rectangle, null, _color, 0, Vector2.Zero, SpriteEffects.None, 0.51f);

            if (!string.IsNullOrEmpty(_text))
            {
                var x = (_rectangle.X + (_rectangle.Width / 2)) - (_font.MeasureString(_text).X / 2);
                var y = (_rectangle.Y + (_rectangle.Height / 2)) - (_font.MeasureString(_text).Y / 2);

                spriteBatch.DrawString(_font, _text, new Vector2(x, y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.6f);
            }
        }

        public void ResetGraphics(Vector2 position)
        {
            _position = position;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }
    }
}
