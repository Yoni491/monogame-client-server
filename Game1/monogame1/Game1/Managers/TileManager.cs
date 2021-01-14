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
        private GraphicsDevice _graphicDevice;
        private ContentManager _contentManager;
        int tileWidth;
        int tileHeight;
        int tilesetTilesWide;
        int tilesetTilesHigh;
        TmxMap map;
        Texture2D tileset;

        public TileManager(GraphicsDevice graphicDevice, ContentManager contentManager)
        {
            _graphicDevice = graphicDevice;
            _contentManager = contentManager;

            map = new TmxMap(Directory.GetCurrentDirectory() + "/Content/maps/map1.tmx");
            tileset = _contentManager.Load<Texture2D>("maps/" + map.Tilesets[0].Name.ToString());

            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < map.TileLayers[0].Tiles.Count; i++)
            {
                int gid = map.TileLayers[0].Tiles[i].Gid;
                // Empty tile, do nothing
                if (gid == 0)
                {

                }
                else
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                    float x = (i % map.Width) * map.TileWidth;
                    float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                    spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White,0,Vector2.Zero,SpriteEffects.None,1);
                }
            }
        }


        static public float GetLayerDepth(float y)
        {
            return 0;
            //return (y / tile_height_amount) / tilesDistribution / 2 + 0.1f;
        }
    }
}
