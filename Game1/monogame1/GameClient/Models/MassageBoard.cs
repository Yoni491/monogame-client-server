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
        private int _showMassageDistance = 100;
        public bool _destroy;
        ScreenMassage _screenMassage;

        public MassageBoard(Rectangle rectangle,bool triggerByHitting = false)
        {
            _position = new Vector2(rectangle.X, rectangle.Y + TileManager._map.TileHeight);
            _rectangle = rectangle;
            _screenMassage = new ScreenMassage("HELLO WORLD");
        }
        public void Update(Rectangle player_position_rectangle)
        {
            Vector2 player_position = new Vector2(player_position_rectangle.X, player_position_rectangle.Y);
            if (Vector2.Distance(player_position, _position) <= _showMassageDistance)
            {
                _screenMassage._displayMassage = true;
            }
            else
            {
                _screenMassage._displayMassage = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            _screenMassage.Draw(spriteBatch);
        }
    }
}
