using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameClient
{
    public class ScreenMessage
    {
        Texture2D _texture;
        SpriteFont _font;
        Rectangle _rectangle;
        Vector2 _position;
        string _text;
        Color _background;
        GraphicsDevice _graphicsDevice;

        public ScreenMessage(GraphicsDevice graphicDevice, string text, Vector2? positon = null, int positionOffsetY = 0)
        {
            _graphicsDevice = graphicDevice;
            _texture = GraphicManager.getRectangleTexture(450, 50, Color.Black);
            _font = GraphicManager.GetBasicFont("basic_22");
            if (positon != null)
            {
                _position = (Vector2)positon;
            }
            else
            {
                _position = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 4, _graphicsDevice.Viewport.Bounds.Height / 4 + positionOffsetY);
            }
            _text = text;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, ((int)_font.MeasureString(_text).X + 20), ((int)_font.MeasureString(_text).Y + 20));
            _background = new Color(Color.Black, 0.1f);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, null, _background, 0, Vector2.Zero, SpriteEffects.None, 0.51f);

            if (!string.IsNullOrEmpty(_text))
            {
                int x = (int)((_rectangle.X + (_rectangle.Width / 2)) - (_font.MeasureString(_text).X) / 2);
                int y = (int)((_rectangle.Y + (_rectangle.Height / 2)) - (_font.MeasureString(_text).Y) / 2);

                spriteBatch.DrawString(_font, _text, new Vector2(x, y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.6f);
            }
        }
        public void Text(string text)
        {
            _text = text;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, ((int)_font.MeasureString(_text).X + 20), ((int)_font.MeasureString(_text).Y + 20));
        }
        public void ResetGraphics(Vector2? position = null)
        {
            if (position != null)
            {
                _position = (Vector2)position;
            }
            else
            {
                _position = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 4, _graphicsDevice.Viewport.Bounds.Height / 4); ;
            }
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, ((int)_font.MeasureString(_text).X + 20), ((int)_font.MeasureString(_text).Y + 20));
        }
    }
}
