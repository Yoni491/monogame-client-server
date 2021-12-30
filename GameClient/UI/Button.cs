using GameClient.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GameClient
{
    public class Button
    {
        Texture2D _texture;
        public static SpriteFont _font;
        Rectangle _rectangle;
        Vector2 _refPosition, _position;
        ScreenPoint _refrencePoint;
        string _buttonText;
        bool _isHovering, _clickedButton, _currentlyPressed, _fixedPosition;
        MouseState _previousMouse, _currentMouse;
        Color _normalColor, _hoverColor;
        public void Text(string text) { _buttonText = text; }
        public Button(Texture2D texture, Vector2 refPosition, ScreenPoint refrencePoint, Color normalColor, Color hoverColor, string buttonText, bool fixedPosition = false)
        {
            _fixedPosition = fixedPosition;
            _refrencePoint = refrencePoint;
            _texture = texture;
            _refPosition = refPosition;
            _normalColor = normalColor;
            _hoverColor = hoverColor;
            _buttonText = buttonText;
            ResetPositionToRefrence();
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }
        public bool Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
            _isHovering = false;
            if (_currentMouse.LeftButton == ButtonState.Pressed && !_currentlyPressed)
            {
                _clickedButton = mouseRectangle.Intersects(_rectangle);
                _currentlyPressed = true;
            }
            else
            {
                _currentlyPressed = false;
            }
            if (mouseRectangle.Intersects(_rectangle))
            {
                Player._mouseIntersectsUI = true;
                _isHovering = true;
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    if (_clickedButton)
                        return true;
                }
            }
            return false;
        }
        public void ChangeText(string text)
        {
            _buttonText = text;
        }
        public void ChangeColor(Color normal)
        {
            _normalColor = normal;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Color _color = _normalColor;
            if (_isHovering)
                _color = _hoverColor;
            spriteBatch.Draw(_texture, _rectangle, null, _color, 0, Vector2.Zero, SpriteEffects.None, 0.52f);
            if (!string.IsNullOrEmpty(_buttonText))
            {
                int x = (int)((_rectangle.X + (_rectangle.Width / 2)) - (_font.MeasureString(_buttonText).X / 2));
                int y = (int)((_rectangle.Y + (_rectangle.Height / 2)) - (_font.MeasureString(_buttonText).Y / 2));
                spriteBatch.DrawString(_font, _buttonText, new Vector2(x, y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.6f);
            }
        }
        public void ResetGraphicsOld(Vector2 position)
        {
            _refPosition = position;
            _rectangle = new Rectangle((int)_refPosition.X, (int)_refPosition.Y, _texture.Width, _texture.Height);
        }
        public void ResetGraphics()
        {
            ResetPositionToRefrence();
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }
        public void ResetPositionToRefrence()
        {
            if (!_fixedPosition)
                _position = _refPosition + _refrencePoint.vector2;
            else
            {
                _position = _refPosition;
            }
        }
    }
}
