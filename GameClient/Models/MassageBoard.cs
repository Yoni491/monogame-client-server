using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class MassageBoard
    {
        private readonly Vector2 _position;
        private readonly Rectangle _rectangle;
        private int _showMassageDistance = 50;
        public bool _destroy, _displayMassage, _displayMassageGamePad;
        ScreenMassage _screenMassage,_screenMassageGamePad;
        GraphicsDevice _graphicsDevice;

        public MassageBoard(GraphicsDevice graphicDevice, Rectangle rectangle, string text,string textGamePad = null, bool triggerByHitting = false)
        {
            _graphicsDevice = graphicDevice;
            _position = new Vector2(rectangle.X, rectangle.Y + TileManager._map.TileHeight);
            _rectangle = rectangle;
            _screenMassage = new ScreenMassage(_graphicsDevice,text);
            if(textGamePad!=null)
            {
                _screenMassageGamePad = new ScreenMassage(_graphicsDevice,textGamePad);
            }
        }
        public void Update(Rectangle player_position_rectangle)
        {
            Vector2 player_position = new Vector2(player_position_rectangle.X, player_position_rectangle.Y);
            if (Vector2.Distance(player_position, _position) <= _showMassageDistance)
            {
                _displayMassage = true;
                if(_screenMassageGamePad!= null)
                    _displayMassageGamePad = true;
            }
            else
            {
                _displayMassage = false;
                if (_screenMassageGamePad != null)
                    _displayMassageGamePad = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch, bool UsingGamePad)
        {

            if (UsingGamePad && _screenMassageGamePad != null)
            {
                if (_displayMassageGamePad)
                {
                    _screenMassageGamePad.Draw(spriteBatch);
                }
            }
            else
            {
                if (_displayMassage)
                {
                    _screenMassage.Draw(spriteBatch);
                }
            }
        }
        public void ResetGraphics()
        {
            _screenMassage.ResetGraphics();
        }
    }
}
