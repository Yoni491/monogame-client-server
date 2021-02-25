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
        static private Player _player;
        static private List<NetworkPlayer> _networkPlayers;

        public BulletReachManager()
        {
            t = new Thread(new ThreadStart(FindPaths));
            t.Start();
            _bulletReaches = new List<BulletReach>();
            _bulletReachesToAdd = new List<BulletReach>();
            _indecesToRemove = new List<int>();

        }
        public void Initialize(Player player, List<NetworkPlayer> networkPlayers)
        {
            _player = player;
            _networkPlayers = networkPlayers;
        }
        public void Update()
        {
            if (!_continueSearch)
            {
                AddPaths();
                RemovePaths();
                _continueSearch = true;
            }
        }
        static public BulletReach GetBulletReach()
        {
            BulletReach bulletReach = new BulletReach(id++,_player,_networkPlayers);
            _bulletReachesToAdd.Add(bulletReach);
            return bulletReach;
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
