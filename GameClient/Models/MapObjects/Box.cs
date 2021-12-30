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
        public int _tilesetIndex;
        public bool _sendBox;
        public bool _destroy;
        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }
        public Box(Rectangle rectangle, int numberInTileset, int tilesetIndex)
        {
            _tilesetIndex = tilesetIndex;
            _numberInTileset = numberInTileset;
            Rectangle = rectangle;
            _position = new Vector2(rectangle.X, rectangle.Y + TileManager._map.TileHeight);
        }
        public void Update()
        {
        }
        public void Destroy()
        {
            if (!_destroy)
            {
                if (!Game_Client._isMultiplayer)
                {
                    ItemManager.DropGold(1, _position);
                    ItemManager.DropItemFromList(CollectionManager.allConsumables, _position);
                }
                TileManager._map.TileLayers[_tilesetIndex].Tiles[_numberInTileset].Gid = 0;
                TileManager._destroyableWalls.Remove(_numberInTileset);
                if (!TileManager._walls.ContainsKey(_numberInTileset))
                {
                    PathFinder.Astar_Grid.SetCell(_numberInTileset % TileManager._map.Width, _numberInTileset / TileManager._map.Width, Enums.CellType.Empty);
                    PathFinder.Bfs_Grid.SetCell(_numberInTileset % TileManager._map.Width, _numberInTileset / TileManager._map.Width, Enums.CellType.Empty);
                }
                MapManager._boxesToSend.Add(_numberInTileset);
                _destroy = true;
                PathFindingManager._continueSearchingBlockedPaths = true;
            }
        }
        public void UpdatePacket(Packet packet)
        {
            packet.WriteInt(_numberInTileset);
            _sendBox = true;
        }
    }
}
