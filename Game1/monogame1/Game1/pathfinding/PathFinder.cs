using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace GameClient
{
    public class PathFinder
    {
        public static Grid s_grid;
        private Grid _grid;
        public static AStar _AStar;
        SearchDetails _searchDetails;
        Vector2 _position;
        float _timer=3;

        public PathFinder()
        {
            
        }
        public static void UpdateGrid(Grid grid)
        {
            s_grid = grid;
        }
        public void Update(GameTime gameTime,Vector2 _Start,Vector2 _End)
        {
            _position = _Start;
            _timer +=(float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 3)
            {
                _AStar = new AStar();
                _AStar.Initialize(TileManager.GetCoordTile(_Start), TileManager.GetCoordTile(_End), s_grid.GetGridCopy());
                _timer = 0;
                while(true)
                {
                    var searchStatus = _AStar.GetPathTick();
                    // If the path is found, draw the path, otherwise draw the updated search
                    if (searchStatus.PathFound)
                    {
                        _searchDetails = searchStatus;
                        return;
                    }
                }
            }
        }
        public Vector2 GetNextCoordPosition()
        {
            if (_searchDetails != null)
            {
                
                while (_searchDetails.Path.Length > 0 && Vector2.Distance(_position, TileManager.GetPositionFromCoord(_searchDetails.Path[0])) <1f)
                {
                    _searchDetails.Path = _searchDetails.Path.Skip(1).ToArray();
                }
                if (_searchDetails.Path.Length > 0)
                    return TileManager.GetPositionFromCoord(_searchDetails.Path[0]);
                else
                    return Vector2.Zero;
            }
            else
                return Vector2.Zero;
        }
    }
}
