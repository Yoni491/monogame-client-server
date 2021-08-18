using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class TileSet
    {
        public Texture2D _texture;
        public int _tileWidth;
        public int _tileHeight;
        public int _tilesetTilesWide;
        public int _tilesetTilesHigh;
        public TileSet(Texture2D texture, int tileWidth, int tileHeight)
        {
            _texture = texture;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _tilesetTilesWide = texture.Width / tileWidth;
            _tilesetTilesHigh = texture.Height / tileHeight;
        }
    }
}
