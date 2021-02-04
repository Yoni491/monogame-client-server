using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GameClient
{
    public class PathFindingManager
    {
        static Thread t;
        static private List<PathFinder> _pathFinderArray;
        static int pathFinderId;
        static public bool _isThreadBusy;
        static public PathFinder _currentPathFinder;
        public PathFindingManager()
        {
            t = new Thread(new ThreadStart(FindPaths));
            t.Start();
            _pathFinderArray = new List<PathFinder>();
        }
        static public PathFinder GetPathFinder()
        {
            PathFinder pathFinder = new PathFinder();
            _pathFinderArray.Add(pathFinder);
            return pathFinder;
        }
        static public void FindPaths()
        {
            while (true)
            {
                if (_isThreadBusy)
                {
                    if (_currentPathFinder!=null)
                        _currentPathFinder.FindPaths();
                    _currentPathFinder = null;
                    _isThreadBusy = false;
                }
                Thread.Sleep(1);
            }
        }
    }
}
