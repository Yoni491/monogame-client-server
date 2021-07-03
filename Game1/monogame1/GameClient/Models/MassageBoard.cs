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
        public bool _destroy;
        ScreenMassage _screenMassage,_screenMassageGamePad;

        public MassageBoard(Rectangle rectangle, string text,string textGamePad = null, bool triggerByHitting = false)
        {
            _position = new Vector2(rectangle.X, rectangle.Y + TileManager._map.TileHeight);
            _rectangle = rectangle;
            _screenMassage = new ScreenMassage(text);
            if(textGamePad!=null)
            {
                _screenMassageGamePad = new ScreenMassage(textGamePad);
            }
        }
        public void Update(Rectangle player_position_rectangle)
        {
            Vector2 player_position = new Vector2(player_position_rectangle.X, player_position_rectangle.Y);
            if (Vector2.Distance(player_position, _position) <= _showMassageDistance)
            {
                _screenMassage._displayMassage = true;
                if(_screenMassageGamePad!= null)
                    _screenMassageGamePad._displayMassage = true;
            }
            else
            {
                _screenMassage._displayMassage = false;
                if (_screenMassageGamePad != null)
                    _screenMassageGamePad._displayMassage = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch,bool UsingGamePad)
        {
            if(UsingGamePad && _screenMassageGamePad != null)
                _screenMassageGamePad.Draw(spriteBatch);
            else
                _screenMassage.Draw(spriteBatch);
        }
    }
}
