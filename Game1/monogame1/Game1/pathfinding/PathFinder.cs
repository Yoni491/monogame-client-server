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
        private AStar _AStar;
        private BreadthFirst _BreadthFirst;
        private SearchDetails _searchDetails, _searchDetailsNew;
        private Vector2 _position;
        private List<Coord> _path;
        private Coord lastCoord;
        private Vector2 _start, _end;
        private bool _newSearchReady = true,_startNewSearch = true;
        public PathFinder()
        {
            _path = new List<Coord>();
        }
        public static void UpdateGrid(Grid grid)
        {
            s_grid = grid;
        }
        public void FindPaths()
        {
            if (_newSearchReady || _start == Vector2.Zero || _end == Vector2.Zero)
                return;
            int tickAmount = 0;
            int pathNotPossible = 0;
            _AStar = new AStar();
            _AStar.Initialize(TileManager.GetCoordTile(_start), TileManager.GetCoordTile(_end), s_grid);
            _BreadthFirst = new BreadthFirst();
            _BreadthFirst.Initialize(TileManager.GetCoordTile(_start), TileManager.GetCoordTile(_end), s_grid);
            while (true)
            {
                if (tickAmount < 1000 && PathFindingManager.UseAStar)
                {
                    var searchStatus = _AStar.GetPathTick();
                    tickAmount++;
                    if (searchStatus.PathFound)
                    {
                        _searchDetails = searchStatus;
                        _newSearchReady=true;
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
                        _newSearchReady = true;
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
                    Console.WriteLine(index);
                    _path.RemoveRange(index+1, _path.Count - index-1);
                }
                else
                {
                    for (int i = 0; i < _path.Count; i++)
                    {
                        if(Vector2.Distance(TileManager.GetPositionFromCoord(_path[i]), TileManager.GetPositionFromCoord(_searchDetails.Path[0]))<16)
                        {
                            index = i;
                            break;
                        }
                    }
                    _path.Clear();
                }
                
            }
            for (int i = 0; i < _searchDetails.Path.Length; i++)
            {
                _path.Add(_searchDetails.Path[i]);
            }
        }
        public void Update(GameTime gameTime,Vector2 start,Vector2 end)
        {
            _position = start;
            _end = end;
            if (_startNewSearch)
            {
                if (_path.Count > 10)
                {
                    lastCoord = _path[10];
                    _start = TileManager.GetPositionFromCoord(_path[10]);
                }
                else if (_path.Count > 0)
                {
                    lastCoord = _path[_path.Count - 1];
                    _start = TileManager.GetPositionFromCoord(_path[_path.Count - 1]);
                }
                else
                {
                    _start = start;
                }
                _startNewSearch = false;
                _newSearchReady = false;
            }
        }
        public Vector2 GetNextCoordPosition()
        {
            if(_searchDetails!= null && _newSearchReady)
            {
                CopyPathArrToList();
                _searchDetails = null;
                _startNewSearch = true;
            }    
            if (_path.Count>0)
            {
                
                while (_path.Count > 0 && Vector2.Distance(_position, TileManager.GetPositionFromCoord(_path[0])) <16f)
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
