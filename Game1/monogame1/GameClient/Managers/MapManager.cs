using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class MapManager
    {
        public List<Grave> _graves;
        Player _player;
        static public List<Chest> _chests;
        //static public List<Box> _boxes;
        List<NetworkPlayer> _networkPlayers;
        static public Dictionary<int, Box> _boxes;
        static public List<int> _boxesToSend;
        public MapManager()
        {
            _graves = new List<Grave>();
            _chests = new List<Chest>();
            //_boxes = new List<Box>();
            _boxes = new Dictionary<int, Box>();
            _boxesToSend = new List<int>();
        }
        public void Initialize(Player player, List<NetworkPlayer> networkPlayers)
        {
            _networkPlayers = networkPlayers;
            _player = player;

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
            if (!Game_Client._IsMultiplayer || !Game_Client._isServer)
            {
                foreach (var item in _boxesToSend)
                {
                    _boxes.Remove(item);
                }
                _boxesToSend.Clear();
            }

            _chests.RemoveAll(item => item._destroy == true);
        }
    }
}
