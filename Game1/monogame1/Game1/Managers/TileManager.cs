using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient
{
    public class TileManager
    {
        private Tile[,] _floorTiles;
        private Tile[,] _buildingTiles;
        private GraphicsDevice _graphicDevice;
        private ContentManager _contentManager;
        int tile_width_amount;
        static int tile_height_amount;
        static int tilesDistribution = 16;
        public TileManager(Tile[,] tiles, GraphicsDevice graphicDevice, ContentManager contentManager)
        {
            _floorTiles = tiles;
            _graphicDevice = graphicDevice;
            _contentManager = contentManager;
            tile_width_amount = _graphicDevice.Viewport.Bounds.Width / tilesDistribution;
            tile_height_amount = _graphicDevice.Viewport.Bounds.Height / tilesDistribution;
            InitFloor();
            _buildingTiles = new Tile[tile_width_amount, tile_height_amount];
           
        }
        public void InitFloor()
        {
            _floorTiles = new Tile[tile_width_amount, tile_height_amount];
            for (int i = 0; i < tile_width_amount; i++)
            {
                for (int j = 0; j < tile_height_amount; j++)
                {
                    _floorTiles[i, j] = new Tile(tilesDistribution, tilesDistribution, new Vector2(i * tilesDistribution, j * tilesDistribution),
                        SpriteManager.GetTextureSqaure(_contentManager.Load<Texture2D>("tiles/tf_A2_ashlands_1"), _graphicDevice, 3, 10, 0, 0));
                }

            }
        }
        public void AddTowerTile(Vector2 _position)
        {
            int x = (int)_position.X / tile_width_amount;
            int y = (int)_position.Y / tile_height_amount;
            _buildingTiles[x, y] = new Tile(tilesDistribution, tilesDistribution, new Vector2(x * tilesDistribution, y * tilesDistribution),_contentManager.Load<Texture2D>("Towers/AttackTower"));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tile_width_amount; i++)
            {
                for (int j = 0; j < tile_height_amount; j++)
                {
                    _floorTiles[i, j].Draw(spriteBatch,0);
                }
            }
            for (int i = 0; i < tile_width_amount; i++)
            {
                for (int j = 0; j < tile_height_amount; j++)
                {
                    if(_buildingTiles[i, j] != null)
                        _buildingTiles[i, j].Draw(spriteBatch, GetLayerDepth(tile_height_amount * i));
                }
            }
        }
        static public float GetLayerDepth(float y)
        {
            return (y / tile_height_amount) / tilesDistribution / 2 + 0.1f;
        }
    }
}
