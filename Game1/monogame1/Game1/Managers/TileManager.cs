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
        static int _tileWidth;
        static int _tileHeight;
        static int _tilesetTilesWide;
        static int _tilesetTilesHigh;
        TmxMap _map;
        Texture2D _tileset;
        //Rectangle[] _floor;
        public static List<Rectangle> _walls;

        public TileManager(GraphicsDevice graphicDevice, ContentManager contentManager)
        {
            _graphicDevice = graphicDevice;
            _contentManager = contentManager;
        }
        public void LoadMap(int mapNum)
        {
            string mapName = Directory.GetCurrentDirectory() + "/Content/maps/" + "map" + mapNum.ToString() + ".tmx";
            _map = new TmxMap(mapName);
            _tileset = _contentManager.Load<Texture2D>("maps/" + _map.Tilesets[0].Name.ToString());

            _tileWidth = _map.Tilesets[0].TileWidth;
            _tileHeight = _map.Tilesets[0].TileHeight;

            _tilesetTilesWide = _tileset.Width / _tileWidth;
            _tilesetTilesHigh = _tileset.Height / _tileHeight;

            //for (var i = 0; i < _map.TileLayers[0].Tiles.Count; i++)
            //{
            //    int gid = _map.TileLayers[0].Tiles[i].Gid;
            //    if (gid != 0)
            //    {
            //        float x = (i % _map.Width) * _map.TileWidth;
            //        float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;
            //        _floor. new Rectangle((int)x, (int)y, _tileWidth, _tileHeight);
            //    }
            //}
            _walls = new List<Rectangle>();
            for (var i = 0; i < _map.TileLayers[1].Tiles.Count; i++)
            {
                int gid = _map.TileLayers[1].Tiles[i].Gid;
                if (gid != 0)
                {
                    float x = (i % _map.Width) * _map.TileWidth;
                    float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;
                    _walls.Add(new Rectangle((int)x, (int)y, _tileWidth, _tileHeight));
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _map.TileLayers[0].Tiles.Count; i++)
            {
                int gid = _map.TileLayers[0].Tiles[i].Gid;
                if (gid != 0)
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % _tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)_tilesetTilesWide);

                    float x = (i % _map.Width) * _map.TileWidth;
                    float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(_tileWidth * column, _tileHeight * row, _tileWidth, _tileHeight);

                    spriteBatch.Draw(_tileset, new Rectangle((int)x, (int)y, _tileWidth, _tileHeight), tilesetRec, Color.White,0,Vector2.Zero,SpriteEffects.None,0);
                }
            }
            for (var i = 0; i < _map.TileLayers[1].Tiles.Count; i++)
            {
                int gid = _map.TileLayers[1].Tiles[i].Gid;
                if (gid != 0)
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % _tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)_tilesetTilesWide);

                    float x = (i % _map.Width) * _map.TileWidth;
                    float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(_tileWidth * column, _tileHeight * row, _tileWidth, _tileHeight);

                    spriteBatch.Draw(_tileset, new Rectangle((int)x, (int)y, _tileWidth, _tileHeight), tilesetRec, Color.White, 0, Vector2.Zero, SpriteEffects.None, GetLayerDepth(y)- 0.07f);
                }
            }
        }


        static public float GetLayerDepth(float y)
        {
            return (y / _graphicDevice.Viewport.Height * GraphicManager.ScreenScale.Y) + 0.1f;
        }
    }
}
