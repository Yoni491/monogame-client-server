using GameClient.UI;
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
        Vector2 _position, _refPosition;
        string _text;
        Color _background;
        GraphicsDevice _graphicsDevice;
        ScreenPoint _refPoint;

        public ScreenMessage(GraphicsDevice graphicDevice, string text, ScreenPoint refPoint = null, Vector2? refPositon = null)
        {
            _refPoint = refPoint;
            _graphicsDevice = graphicDevice;
            _texture = GraphicManager.getRectangleTexture(450, 50, Color.Black);
            _font = GraphicManager.GetBasicFont("basic_22");
            if (refPositon != null)
            {
                _refPosition = (Vector2)refPositon;
                _position = _refPosition + refPoint.vector2;
            }
            else
            {
                _position = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 4, _graphicsDevice.Viewport.Bounds.Height / 4);
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
        public void ResetGraphicsOld(Vector2? position = null)
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
        public void ResetGraphics()
        {
            if (_refPoint != null)
            {
                _position = _refPosition + _refPoint.vector2;
            }
            else
            {
                _position = new Vector2(_graphicsDevice.Viewport.Bounds.Width / 4, _graphicsDevice.Viewport.Bounds.Height / 4); ;
            }
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, ((int)_font.MeasureString(_text).X + 20), ((int)_font.MeasureString(_text).Y + 20));
        }
    }
}
