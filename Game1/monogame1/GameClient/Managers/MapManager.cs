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
        static public List<Box> _boxes;
        List<NetworkPlayer> _networkPlayers;
        static public List<Box> _boxesToSend;
        public MapManager()
        {
            _graves = new List<Grave>();
            _chests = new List<Chest>();
            _boxes = new List<Box>();
            _boxesToSend = new List<Box>();
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
            if(!Game_Client._IsMultiplayer)
                _boxes.RemoveAll(item => item._destroy == true);
            else
                _boxes.RemoveAll(item => item._destroy == true && item._boxSent == true);
            _chests.RemoveAll(item => item._destroy == true);
        }
    }
}
