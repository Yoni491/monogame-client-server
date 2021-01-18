using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using TiledSharp;
namespace GameClient
{
    public class TileManager
    {
        static private GraphicsDevice _graphicDevice;
        private ContentManager _contentManager;
        private MapManager _mapManager;
        List<TileSet> _tileSets;
        TmxMap _map;
        //Rectangle[] _floor;
        public static List<Rectangle> _walls;

        public TileManager(GraphicsDevice graphicDevice, ContentManager contentManager, MapManager mapManager)
        {
            _graphicDevice = graphicDevice;
            _contentManager = contentManager;
            this._mapManager = mapManager;
            _tileSets = new List<TileSet>();
        }
        public void LoadMap(int mapNum)
        {
            _walls = new List<Rectangle>();

            string mapName = Directory.GetCurrentDirectory() + "/Content/maps/" + "map" + mapNum.ToString() + ".tmx";
            _map = new TmxMap(mapName);
            int tilesetIndex = 0;

            for (int i = 0; i < _map.Tilesets.Count; i++)
            {
                _tileSets.Add(new TileSet(_contentManager.Load<Texture2D>("maps/" + _map.Tilesets[i].Name.ToString()),
                _map.Tilesets[i].TileWidth, _map.Tilesets[i].TileHeight));
            }
            
            foreach (var tilset in _tileSets)
            {
                for (int i = 0; i < _map.TileLayers[1].Tiles.Count; i++)
                {
                    int gid = _map.TileLayers[1].Tiles[i].Gid;
                    if (gid != 0)
                    {
                        float x = (i % _map.Width) * _map.TileWidth;
                        float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;
                        Rectangle rectangle = new Rectangle((int)x, (int)y, _tileSets[0]._tileWidth, _tileSets[0]._tileHeight);
                        _walls.Add(rectangle);
                        if (_map.Tilesets[tilesetIndex].FirstGid > 0)
                        {
                            gid = gid - (_map.Tilesets[tilesetIndex].FirstGid - 1);
                        }
                        if (gid == 197)
                        {
                            _mapManager._graves.Add(new Grave(rectangle));
                        }
                    }
                }
                tilesetIndex++;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            int tilesetIndex = 0;
            foreach (var tileset in _tileSets)
            {
                for (int i = 0; i < _map.TileLayers[0].Tiles.Count; i++)
                {
                    int gid = _map.TileLayers[0].Tiles[i].Gid;
                    if (gid != 0)
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % tileset._tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tileset._tilesetTilesWide);

                        float x = (i % _map.Width) * _map.TileWidth;
                        float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;

                        Rectangle tilesetRec = new Rectangle(tileset._tileWidth * column, tileset._tileHeight * row, tileset._tileWidth, tileset._tileHeight);
                        spriteBatch.Draw(tileset._texture, new Rectangle((int)x, (int)y, tileset._tileWidth, tileset._tileHeight), tilesetRec, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    }
                }
                for (int i = 0; i < _map.TileLayers[1].Tiles.Count; i++)
                {
                    int gid = _map.TileLayers[1].Tiles[i].Gid;
                    if (gid != 0 && gid >= _map.Tilesets[tilesetIndex].FirstGid)
                    {
                        if (_map.Tilesets[tilesetIndex].FirstGid > 0)
                        {
                            gid = gid - (_map.Tilesets[tilesetIndex].FirstGid - 1);
                        }
                        int tileFrame = gid - 1;
                        int column = tileFrame % tileset._tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tileset._tilesetTilesWide);

                        float x = (i % _map.Width) * _map.TileWidth;
                        float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;

                        Rectangle tilesetRec = new Rectangle(tileset._tileWidth * column, tileset._tileHeight * row, tileset._tileWidth, tileset._tileHeight);
                        
                        spriteBatch.Draw(tileset._texture, new Rectangle((int)x, (int)y, tileset._tileWidth, tileset._tileHeight), tilesetRec, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
                    }
                }
                tilesetIndex++;

            }
        }


        static public float GetLayerDepth(float y)
        {
            return (y / _graphicDevice.Viewport.Height * GraphicManager.ScreenScale.Y) + 0.1f;
        }
    }
}
