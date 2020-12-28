using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace GameServer
{
    public class TileManager
    {
        private Tile[,] _tiles;
        //private GraphicsDevice _graphicDevice;
        //private ContentManager _contentManager;
        int tile_width_amount;
        int tile_height_amount;
        public TileManager(Tile[,] tiles, GraphicsDevice graphicDevice,ContentManager contentManager)
        {
            _tiles = tiles;
            //_graphicDevice = graphicDevice;
            //_contentManager = contentManager;
            tile_width_amount = 1600 / 16;
            tile_height_amount = 1600 / 16;
            tileMaker();
        }
        public void tileMaker()
        {
            
            _tiles = new Tile[tile_width_amount, tile_height_amount];
            for (int i = 0; i < tile_width_amount; i++)
            {
                for (int j = 0; j < tile_height_amount; j++)
                {
                    _tiles[i, j] = new Tile(16, 16,new Vector2(i * 16, j * 16));
                }

            }
        }
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    for (int i = 0; i < tile_width_amount; i++)
        //    {
        //        for (int j = 0; j < tile_height_amount; j++)
        //        {
        //            _tiles[i, j].Draw(spriteBatch);
        //        }

        //    }
        //}
    }
}
