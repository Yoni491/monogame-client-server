using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace GameClient
{
    public class PathFinder
    {
        public static Grid s_grid;
        private Grid _grid;
        public static AStar _AStar;
        SearchDetails _searchDetails;
        Vector2 _position;
        float _timer=0;

        public PathFinder()
        {
            _grid = s_grid.GetGridCopy();
            _AStar = new AStar(s_grid);
        }
        public static void UpdateGrid(Grid grid)
        {
            s_grid = grid;
        }
        public void Update(GameTime gameTime,Vector2 _Start,Vector2 _End)
        {
            _position = _Start;
            _timer +=(float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= 1)
            {
                _timer = 0;
                s_grid.SetStartAndEnd(TileManager.GetCoordTile(_Start), TileManager.GetCoordTile(_End));
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
            float closestCoordDistance = float.MaxValue;
            Vector2 closestCoord = Vector2.Zero;
            int index = 0;
            foreach (var coord in _searchDetails.Path)
            {
                if ((index++ != 0) &&  Vector2.Distance(_position, TileManager.GetPositionFromCoord(coord))< closestCoordDistance)
                {
                    closestCoord = TileManager.GetPositionFromCoord(coord);
                }
            }
            return closestCoord;
        }
    }
}
