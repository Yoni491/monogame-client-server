using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;



namespace GameClient
{
    public class Button
    {
        Texture2D _texture;
        SpriteFont _font;
        bool _isHovering;
        Rectangle _rectangle;
        Vector2 _position;
        MouseState _previousMouse, _currentMouse;
        Color _normal, _hover;
        string _text;

        public void Text(string text) { _text = text; }

        public Button(Texture2D texture, SpriteFont font,Vector2 position,Color normal,Color hover,string text)
        {
            _texture = texture;
            _font = font;
            _position = position;
            _normal = normal;
            _hover = hover;
            _text = text;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }
        public bool Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(_rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    return true;
                }
            }
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Color _color = _normal;

            if (_isHovering)
                _color = _hover;

            spriteBatch.Draw(_texture, _rectangle,null, _color,0,Vector2.Zero,SpriteEffects.None,0.51f);

            if (!string.IsNullOrEmpty(_text))
            {
                var x = (_rectangle.X + (_rectangle.Width / 2)) - (_font.MeasureString(_text).X / 2);
                var y = (_rectangle.Y + (_rectangle.Height / 2)) - (_font.MeasureString(_text).Y / 2);

                spriteBatch.DrawString(_font, _text, new Vector2(x, y), Color.White,0,Vector2.Zero,1,SpriteEffects.None,0.6f);
            }
        }

        public void ResetGraphics(Vector2 position)
        {
            _position = position;
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }

    }
}
