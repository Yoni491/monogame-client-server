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
        public SearchDetails _searchDetails, _searchDetailsNew;
        public  Vector2 _position;
        float _timer=1;
        float _timerTillNextSearch = 0;
        public bool IsThreadBusy = false;
        //public Coord _start, _end;
        public List<Coord> _path;
        private Coord lastCoord;
        public PathFinder()
        {
            _path = new List<Coord>();
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
            int pathNotPossible = 0;
            while (true)
            {
                if (tickAmount < 1000 && PathFindingManager.UseAStar)
                {
                    var searchStatus = _AStar.GetPathTick();
                    tickAmount++;
                    if (searchStatus.PathFound)
                    {
                        _searchDetails = searchStatus;
                        _timerTillNextSearch = Math.Min(searchStatus.Path.Length, _timerTillNextSearch);
                        return;
                    }
                    if(!searchStatus.PathPossible)
                    {
                        pathNotPossible++;
                    }
                    else
                    {
                        pathNotPossible = 0;
                    }
                    if(pathNotPossible>1)
                    {
                        return;
                    }
                }
                else
                {
                    PathFindingManager.UseAStar = false;
                    var searchStatus = _BreadthFirst.GetPathTick();
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
        private void CopyPathArrToList()
        {
            if(lastCoord != null)
            {
                int index = _path.FindIndex(x => x == lastCoord);
                if (index > -1)
                {
                    _path.RemoveRange(index, _path.Count - index);
                }
                else
                {
                    _path.Clear();
                }
                
            }
            for (int i = 0; i < _searchDetails.Path.Length; i++)
            {
                _path.Add(_searchDetails.Path[i]);
            }
        }
        public void Update(GameTime gameTime,Vector2 Start,Vector2 End)
        {
            _position = Start;
            Vector2 startPosition = Start;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= _timerTillNextSearch)
            {
                if (!PathFindingManager._isThreadBusy)
                {
                    if (_path.Count > 10)
                    {
                        lastCoord = _path[10];
                        startPosition = TileManager.GetPositionFromCoord(_path[10]);
                    }
                    else if(_path.Count>0)
                    {
                        lastCoord = _path[_path.Count-1];
                        startPosition = TileManager.GetPositionFromCoord(_path[_path.Count-1]);
                    }
                    _timer = -1000;
                    _AStar = new AStar();
                    _AStar.Initialize(TileManager.GetCoordTile(startPosition), TileManager.GetCoordTile(End), s_grid);
                    _BreadthFirst = new BreadthFirst();
                    _BreadthFirst.Initialize(TileManager.GetCoordTile(startPosition), TileManager.GetCoordTile(End), s_grid);
                    PathFindingManager._currentPathFinder = this;
                    PathFindingManager._isThreadBusy = true;
                }
            }
        }
        public Vector2 GetNextCoordPosition()
        {
            if(_searchDetails!= null)
            {
                CopyPathArrToList();
                _searchDetails = null;
            }    
            if (_path.Count>0)
            {
                
                while (_path.Count > 0 && Vector2.Distance(_position, TileManager.GetPositionFromCoord(_path[0])) <1f)
                {
                    _path.RemoveAt(0);
                }
                if (_path.Count > 0)
                    return TileManager.GetPositionFromCoord(_path[0]);
                else
                    return Vector2.Zero;
            }
            else
                return Vector2.Zero;
        }
    }
}
