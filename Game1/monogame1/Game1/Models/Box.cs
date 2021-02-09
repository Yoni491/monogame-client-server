using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class Box
    {
        private Rectangle _rectangle;
        private readonly Vector2 _position;
        private int _numberInTileset;
        public bool _destroy;

        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }

        public Box(Rectangle rectangle, int numberInTileset)
        {
            _numberInTileset = numberInTileset;
            Rectangle = rectangle;
            _position = new Vector2(rectangle.X, rectangle.Y + TileManager._map.TileHeight);

        }
        public void Update(Rectangle player_position_rectangle)
        {

        }
        public void Destroy()
        {
            ItemManager.DropGold(1, _position);
            _destroy = true;
            TileManager._map.TileLayers[2].Tiles[_numberInTileset].Gid = 0;
            TileManager._walls.RemoveAll(item=>item==Rectangle);
        }
    }
}
