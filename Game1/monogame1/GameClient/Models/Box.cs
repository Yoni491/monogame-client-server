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
        public int _tilesetIndex;
        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }

        public Box(Rectangle rectangle, int numberInTileset,int tilesetIndex)
        {
            _tilesetIndex = tilesetIndex;
            _numberInTileset = numberInTileset;
            Rectangle = rectangle;
            _position = new Vector2(rectangle.X, rectangle.Y + TileManager._map.TileHeight);

        }
        public void Update(Rectangle player_position_rectangle)
        {

        }
        public void Destroy()
        {

            if (!Game_Client._IsMultiplayer)
            {
                _destroy = true;
                ItemManager.DropGold(1, _position);
                TileManager._map.TileLayers[_tilesetIndex].Tiles[_numberInTileset].Gid = 0;
                TileManager._walls.RemoveAll(item => item == Rectangle);
                PathFinder.s_grid.SetCell(_numberInTileset % TileManager._map.Width, _numberInTileset / TileManager._map.Width, Enums.CellType.Empty);
            }
            else
            {
                MapManager._boxesToSend.Add(this);
            }
        }
        public void UpdatePacket(Packet packet)
        {
            packet.WriteInt(_tilesetIndex);
            _destroy = true;
        }
    }
}
