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
        static public Grid Astar_Grid,Bfs_Grid;
        private AStar _AStar;
        private BreadthFirst _BreadthFirst;
        private SearchDetails _searchDetails;
        private Vector2 _position;
        public List<Coord> _path;
        private Coord lastCoord;
        private Vector2 _start, _end,_first_position = Vector2.Zero;
        private bool _newSearchReady = true,_startNewSearch = true;
        public int _id;
        public bool _useAstar =true;
        public bool _waitForDestroyedWall;
        public bool _waitForEndPath;
        public PathFinder(int id,bool useAstar,bool waitForDestroyedWall)
        {
            _path = new List<Coord>();
            _id = id;
            _useAstar = useAstar;
            _waitForDestroyedWall = waitForDestroyedWall;
        }
        public static void UpdateGrid(Grid grid)
        {
            Astar_Grid = grid;
            Bfs_Grid = grid.GetGridCopy();
        }
        public void FindPaths()
        {
            if (_startNewSearch && _first_position!=Vector2.Zero)
            {
                if (_path.Count > 20)
                {
                    lastCoord = _path[20];
                    _start = TileManager.GetPositionFromCoord(_path[20]);
                    return;
                }
                else if (_path.Count > 0)
                {
                    lastCoord = _path[_path.Count - 1];
                    _start = TileManager.GetPositionFromCoord(_path[_path.Count - 1]);
                }
                else
                {
                    _start = _position;
                }
                _startNewSearch = false;
                _newSearchReady = false;
            }
            if (_newSearchReady || _start == Vector2.Zero || _end == Vector2.Zero)
                return;
            int tickAmount = 0;
            int pathNotPossible = 0;
            _AStar = new AStar();
            _AStar.Initialize(TileManager.GetCoordTile(_start), TileManager.GetCoordTile(_end), Astar_Grid);
            _BreadthFirst = new BreadthFirst();
            _BreadthFirst.Initialize(TileManager.GetCoordTile(_start), TileManager.GetCoordTile(_end), Bfs_Grid);
            while (true)
            {
                if (_useAstar && tickAmount <= 1000)
                {
                    if (tickAmount == 1000)
                    {
                        _useAstar = false;
                        _newSearchReady = true;
                        return;
                    }
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
                        _newSearchReady = true;
                        _waitForDestroyedWall = true;
                        return;
                    }
                }
                else
                {
                    var searchStatus = _BreadthFirst.GetPathTick();
                    if (searchStatus.PathFound)
                    {
                        _useAstar = true;
                        _searchDetails = searchStatus;
                        _newSearchReady = true;
                        return;
                    }
                    if (!searchStatus.PathPossible)
                    {
                        _useAstar = true;
                        _newSearchReady = true;
                        _waitForDestroyedWall = true;
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
                    _path.RemoveRange(index+1, _path.Count - index-1);
                }
                else
                {
                    _path.Clear();
                }
                
            }
            else
            {
                _path.Clear();
            }
            for (int i = 0; i < _searchDetails.Path.Length; i++)
            {
                _path.Add(_searchDetails.Path[i]);
            }
        }
        public void Update(GameTime gameTime,Vector2 start,Vector2 end)
        {
            if (_first_position == Vector2.Zero)
                _first_position = start;
            _position = start;
            _end = end;
            
        }
        public Vector2 GetNextCoordPosition()
        {
            if( _newSearchReady)
            {
                if(_searchDetails != null)
                    CopyPathArrToList();
                _searchDetails = null;
                _startNewSearch = true;
            }    
            if (_path.Count>0)
            {
                if (_path.Count < 10)
                    _useAstar = true;
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
