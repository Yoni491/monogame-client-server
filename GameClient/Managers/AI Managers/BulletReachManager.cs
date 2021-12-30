using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace GameClient
{
    public class BulletReachManager
    {
        static Thread t;
        static private List<BulletReach> _bulletReaches, _bulletReachesToAdd;
        static public bool _isThreadBusy;
        static public PathFinder _currentPathFinder;
        static List<int> _indecesToRemove;
        static private bool _continueSearch = false;
        static int id = 0;
        static private List<Player> _players;
        static private List<NetworkPlayer> _networkPlayers;
        static bool _reset;
        public BulletReachManager()
        {
            t = new Thread(new ThreadStart(FindPaths));
            t.Start();
            _bulletReaches = new List<BulletReach>();
            _bulletReachesToAdd = new List<BulletReach>();
            _indecesToRemove = new List<int>();
        }
        public void Initialize(List<Player> players, List<NetworkPlayer> networkPlayers)
        {
            _players = players;
            _networkPlayers = networkPlayers;
        }
        public void Update()
        {
            if (!_continueSearch)
            {
                if (_reset)
                {
                    _bulletReaches.Clear();
                    _reset = false;
                }
                AddPaths();
                RemovePaths();
                _continueSearch = true;
            }
        }
        static public BulletReach GetBulletReach(Gun gun)
        {
            BulletReach bulletReach = new BulletReach(id++, _players, _networkPlayers, gun);
            _bulletReachesToAdd.Add(bulletReach);
            return bulletReach;
        }
        static public void Reset()
        {
            _reset = true;
        }
        static public void RemovePathFinder(PathFinder pathFinder)
        {
            _indecesToRemove.Add(pathFinder._id);
        }
        static private void RemovePaths()
        {
            for (int i = 0; i < _indecesToRemove.Count; i++)
            {
                _bulletReaches.RemoveAll(x => x._id == _indecesToRemove[i]);
            }
            _indecesToRemove.Clear();
        }
        static private void AddPaths()
        {
            for (int i = 0; i < _bulletReachesToAdd.Count; i++)
            {
                _bulletReaches.Add(_bulletReachesToAdd[0]);
                _bulletReachesToAdd.RemoveAt(0);
            }
        }
        static public void FindPaths()
        {
            int index = 0;
            while (true)
            {
                if (_continueSearch)
                {
                    if (index > _bulletReaches.Count - 1)
                        index = 0;
                    if (_bulletReaches.Count > 0)
                        _bulletReaches[index].FindReachablePlayer();
                    index++;
                    _continueSearch = false;
                }
                Thread.Sleep(1);
            }
        }
    }
}
