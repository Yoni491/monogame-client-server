using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GameClient
{
    public class PathFindingManager
    {
        static Thread t;
        static private List<PathFinder> _pathFinderList,_pathsToAdd;
        static public bool _isThreadBusy;
        static public PathFinder _currentPathFinder;
        static public bool UseAStar = true;
        static List<int> _indecesToRemove;
        static private bool _continueSearch=false;

        public PathFindingManager()
        {
            t = new Thread(new ThreadStart(FindPaths));
            t.Start();
            _pathFinderList = new List<PathFinder>();
            _pathsToAdd = new List<PathFinder>();
            _indecesToRemove = new List<int>();
        }
        public void Update()
        {
            if (!_continueSearch)
            {
                RemovePaths();
                AddPaths();
                _continueSearch = true;
            }
        }
        static public PathFinder GetPathFinder()
        {
            PathFinder pathFinder = new PathFinder();
            _pathsToAdd.Add(pathFinder);
            return pathFinder;
        }
        static public void RemovePathFinder(PathFinder pathFinder)
        {
            _indecesToRemove.Add(_pathFinderList.FindIndex(0,x=> x == pathFinder));
        }
        static private void RemovePaths()
        {
            for (int i = 0; i < _indecesToRemove.Count; i++)
            {
                _pathFinderList.RemoveAt(_indecesToRemove[0]);
                _indecesToRemove.RemoveAt(0);
            }
        }
        static private void AddPaths()
        {
            for (int i = 0; i < _pathsToAdd.Count; i++)
            {
                _pathFinderList.Add(_pathsToAdd[0]);
                _pathsToAdd.RemoveAt(0);
            }
        }
        static public void FindPaths()
        {
            int index = 0;
            while (true)
            {
                if (_continueSearch)
                {
                    if (index > _pathFinderList.Count - 1)
                        index = 0;
                    if (_pathFinderList.Count > 0)
                        _pathFinderList[index].FindPaths();
                    index++;
                    _continueSearch = false;
                }
            }
        }
    }
}
