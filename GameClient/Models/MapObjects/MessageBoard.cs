using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class MessageBoard
    {
        private readonly Vector2 _position;
        private readonly Rectangle _rectangle;
        private int _showMessageDistance = 50;
        public bool _destroy, _displayMessage, _displayMessageGamePad;
        ScreenMessage _screenMessage, _screenMessageGamePad;
        GraphicsDevice _graphicsDevice;

        public MessageBoard(GraphicsDevice graphicDevice, Rectangle rectangle, string text, string textGamePad = null, bool triggerByHitting = false)
        {
            _graphicsDevice = graphicDevice;
            _position = new Vector2(rectangle.X, rectangle.Y + TileManager._map.TileHeight);
            _rectangle = rectangle;
            _screenMessage = new ScreenMessage(_graphicsDevice, text);
            if (textGamePad != null)
            {
                _screenMessageGamePad = new ScreenMessage(_graphicsDevice, textGamePad);
            }
        }
        public void Update(Rectangle player_position_rectangle)
        {
            Vector2 player_position = new Vector2(player_position_rectangle.X, player_position_rectangle.Y);
            if (Vector2.Distance(player_position, _position) <= _showMessageDistance)
            {
                _displayMessage = true;
                if (_screenMessageGamePad != null)
                    _displayMessageGamePad = true;
            }
            else
            {
                _displayMessage = false;
                if (_screenMessageGamePad != null)
                    _displayMessageGamePad = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch, bool UsingGamePad)
        {

            if (UsingGamePad && _screenMessageGamePad != null)
            {
                if (_displayMessageGamePad)
                {
                    _screenMessageGamePad.Draw(spriteBatch);
                }
            }
            else
            {
                if (_displayMessage)
                {
                    _screenMessage.Draw(spriteBatch);
                }
            }
        }
        public void ResetGraphics()
        {
            _screenMessage.ResetGraphics();
        }
    }
}
