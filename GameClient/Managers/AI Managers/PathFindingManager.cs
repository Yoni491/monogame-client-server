using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GameClient
{
    public class PathFindingManager
    {
        static Thread AstarThread,BFSThread;
        static private List<PathFinder> _pathFinderList,_pathsToAdd;
        static public PathFinder _currentPathFinder;
        static List<int> _indecesToRemove;
        static private bool _continueSearch, _continueSearchBFS;
        static int id = 0;
        static public bool _continueSearchingBlockedPaths;
        static public bool _reset;
        public PathFindingManager()
        {
            AstarThread = new Thread(new ThreadStart(()=>FindPaths(true,ref _continueSearch)));
            AstarThread.Start();
            BFSThread = new Thread(new ThreadStart(() => FindPaths(false,ref _continueSearchBFS)));
            BFSThread.Start();
            _pathFinderList = new List<PathFinder>();
            _pathsToAdd = new List<PathFinder>();
            _indecesToRemove = new List<int>();
        }
        public void Update() //the order of the functions in this function is critical
        {
            if (!_continueSearchBFS)
            {
                if (!_continueSearch)
                {
                    if(_reset)
                    {
                        _pathFinderList.Clear();
                        _reset = false;
                    }
                    AddPaths();
                    RemovePaths();
                    _continueSearch = true;
                    _continueSearchBFS = true;
                }
            }
            else if (!_continueSearch)
            {
                _continueSearch = true;
            }
            
        }
        static public PathFinder GetPathFinder(bool useAstar,bool waitForDestroyedWall,float speed)
        {
            PathFinder pathFinder = new PathFinder(id++,useAstar,waitForDestroyedWall,speed);
            _pathsToAdd.Add(pathFinder);
            return pathFinder;
        }
        static public void Reset()
        {
            _reset = true;
            _pathsToAdd.Clear();
            _continueSearchingBlockedPaths = false;
        }
        static public void RemovePathFinder(PathFinder pathFinder)
        {
            _indecesToRemove.Add(pathFinder._id);
        }
        static private void RemovePaths()
        {
            for (int i = 0; i < _indecesToRemove.Count; i++)
            {

                _pathFinderList.RemoveAll(x=>x._id == _indecesToRemove[i]);
            }
            _indecesToRemove.Clear();
        }
        static private void AddPaths()
        {
            for (int i = 0; i < _pathsToAdd.Count; i++)
            {
                
                _pathFinderList.Add(_pathsToAdd[0]);
                _pathsToAdd.RemoveAt(0);
            }
        }
        static public void FindPaths(bool UseAstar, ref bool continueSearch)
        {
            int index = 0;
            while (true)
            {
                if (continueSearch)
                {
                    if (_pathFinderList.Count > 0)
                    {
                        if (index > _pathFinderList.Count - 1)
                            index = 0;
                        if (UseAstar && _pathFinderList[index]._useAstar)
                        {
                            if (!_pathFinderList[index]._waitForDestroyedWall || _continueSearchingBlockedPaths)
                            {
                                _pathFinderList[index].FindPaths();
                                continueSearch = false;
                            }
                        }
                        else if (!UseAstar && !_pathFinderList[index]._useAstar)
                        {
                            if (!_pathFinderList[index]._waitForDestroyedWall || _continueSearchingBlockedPaths)
                            {
                                _pathFinderList[index].FindPaths();
                                continueSearch = false;
                            }
                        }
                        index++;
                    }
                    continueSearch = false;
                }
                Thread.Sleep(1);
            }
        }
    }
}
