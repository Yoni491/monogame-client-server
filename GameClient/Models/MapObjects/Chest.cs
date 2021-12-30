using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
namespace GameClient
{
    public class Chest
    {
        private Rectangle _rectangle;
        private readonly Vector2 _position;
        private int _numberInTileset;
        private readonly int tilesetIndex;
        public bool _destroy;
        public bool _sendChest;
        private int _itemToDrop;
        private bool _smallChest;
        public Rectangle Rectangle { get => _rectangle; set => _rectangle = value; }
        public Chest(Rectangle rectangle, int numberInTileset, int tilesetIndex, int item = -1, bool smallChest = false)
        {
            _numberInTileset = numberInTileset;
            this.tilesetIndex = tilesetIndex;
            Rectangle = rectangle;
            _position = new Vector2(rectangle.X, rectangle.Y + TileManager._map.TileHeight);
            _itemToDrop = item;
            _smallChest = smallChest;
        }
        public void Update(Rectangle player_position_rectangle)
        {
        }
        public void Open()
        {
            if (!_destroy)
            {
                if (!Game_Client._isMultiplayer)
                {
                    if (_itemToDrop != -1)
                    {
                        ItemManager.DropItem(_itemToDrop, _position, true);
                    }
                    else if (_smallChest)
                    {
                        ItemManager.DropItemSmallChest(_position);
                    }
                    else
                    {
                        ItemManager.DropItemNormalChest(_position);
                    }
                }
                MapManager._chestsToSend.Add(_numberInTileset);
                _destroy = true;
                TileManager._map.TileLayers[tilesetIndex].Tiles[_numberInTileset].Gid = 0;
            }
        }
        public void UpdatePacket(Packet packet)
        {
            packet.WriteInt(_numberInTileset);
            _sendChest = true;
        }
    }
}
