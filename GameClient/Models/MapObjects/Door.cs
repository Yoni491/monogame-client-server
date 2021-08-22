using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class Door
    {
        private Rectangle _rectangle;
        private readonly Vector2 _position;
        private int _numberInTileset;
        public int _tilesetIndex;
        public bool _sendDoor;
        public bool _destroy;

        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }

        public Door(Rectangle rectangle, int numberInTileset, int tilesetIndex)
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
                TileManager._map.TileLayers[_tilesetIndex].Tiles[_numberInTileset].Gid = 0;
                if (TileManager._walls.ContainsKey(_numberInTileset))
                {
                    PathFinder.Astar_Grid.SetCell(_numberInTileset % TileManager._map.Width, _numberInTileset / TileManager._map.Width, Enums.CellType.Empty);
                    PathFinder.Bfs_Grid.SetCell(_numberInTileset % TileManager._map.Width, _numberInTileset / TileManager._map.Width, Enums.CellType.Empty);
                    TileManager._walls.Remove(_numberInTileset);
                    TileManager.RemoveWallsAroundTile(_numberInTileset);
                }
                MapManager._doorsToSend.Add(_numberInTileset);
                PathFindingManager._continueSearchingBlockedPaths = true;
                _destroy = true;
            }

        }
        public void UpdatePacket(Packet packet)
        {
            packet.WriteInt(_numberInTileset);
            _sendDoor = true;
        }
    }
}
