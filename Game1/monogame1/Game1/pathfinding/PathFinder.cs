using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;

namespace GameClient
{
    public class PathFinder
    {
        static public Grid s_grid;
        public AStar _AStar;
        public  BreadthFirst _BreadthFirst;
        public SearchDetails _searchDetails;
        public  Vector2 _position;
        float _timer=1;
        float _timerTillNextSearch = 0;
        public bool IsThreadBusy = false;
        public Coord _start, _end;

        public PathFinder()
        {
        }
        public static void UpdateGrid(Grid grid)
        {
            s_grid = grid;
        }
        private void Reset()
        {
            _timer = 0;
            _timerTillNextSearch = 5;
        }
        public void FindPaths()
        {
            Reset();
            int tickAmount = 0;
            while (true)
            {
                if (tickAmount < 2000)
                {
                    var searchStatus = _AStar.GetPathTick();
                    tickAmount++;
                    // If the path is found, draw the path, otherwise draw the updated search
                    if (searchStatus.PathFound)
                    {
                        _searchDetails = searchStatus;
                        _timerTillNextSearch = Math.Min(searchStatus.Path.Length, _timerTillNextSearch);
                        return;
                    }
                    if(!searchStatus.PathPossible)
                    {
                        return;
                    }

                }
                else
                {
                    var searchStatus = _BreadthFirst.GetPathTick();

                    // If the path is found, draw the path, otherwise draw the updated search
                    if (searchStatus.PathFound)
                    {
                        _searchDetails = searchStatus;
                        _timerTillNextSearch = Math.Min(searchStatus.Path.Length, _timerTillNextSearch);
                        return;
                    }
                    if (!searchStatus.PathPossible)
                    {
                        return;
                    }
                }
            }
        }
        public void Update(GameTime gameTime,Vector2 Start,Vector2 End)
        {
            _position = Start;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= _timerTillNextSearch)
            {
                if (!PathFindingManager._isThreadBusy)
                {
                    _timer = -1000;
                    _AStar = new AStar();
                    _AStar.Initialize(TileManager.GetCoordTile(Start), TileManager.GetCoordTile(End), s_grid);
                    _BreadthFirst = new BreadthFirst();
                    _BreadthFirst.Initialize(TileManager.GetCoordTile(Start), TileManager.GetCoordTile(End), s_grid);
                    PathFindingManager._currentPathFinder = this;
                    PathFindingManager._isThreadBusy = true;
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
