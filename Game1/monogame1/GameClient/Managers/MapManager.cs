using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class MapManager
    {
        static public List<Grave> _graves;
        Player _player;
        static public Dictionary<int, Chest> _chests;
        List<NetworkPlayer> _networkPlayers;
        static public Dictionary<int, Box> _boxes;
        static public List<int> _boxesToSend;
        static public List<int> _chestsToSend;
        public MapManager()
        {
            _graves = new List<Grave>();
            _chests = new Dictionary<int, Chest>();
            _boxes = new Dictionary<int, Box>();
            _boxesToSend = new List<int>();
            _chestsToSend = new List<int>();
        }
        public void Initialize(Player player)
        {
            _player = player;
        }
        public void Initialize(List<NetworkPlayer> networkPlayers)
        {
            _networkPlayers = networkPlayers;
        }
        public void Update()
        {
            foreach (var grave in _graves)
            {
                if(_player!=null)
                    grave.Update(_player.RectangleMovement);
                else if(_networkPlayers!=null)
                {
                    foreach (var player in _networkPlayers)
                    {
                        grave.Update(player.RectangleMovement);
                    }
                }
            }
            _graves.RemoveAll(item => item._destroy == true);
            if (!Game_Client._IsMultiplayer && !Game_Client._isServer)
            {
                foreach (var item in _boxesToSend)
                {
                    _boxes.Remove(item);
                }
                _boxesToSend.Clear();
                foreach (var item in _chestsToSend)
                {
                    _chests.Remove(item);
                }
                _chestsToSend.Clear();
            }
        }
        public static void ResetMap()
        {
            _graves.Clear();
            _chests.Clear();
            _boxes.Clear();
            _boxesToSend.Clear();
            _chestsToSend.Clear();
        }
    }
}
