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

        public MapManager()
        {
            _graves = new List<Grave>();
            _chests = new List<Chest>();
            _boxes = new List<Box>();
        }
        public void Initialize(Player player)
        {
            _player = player;

        }
        public void Update()
        {
            foreach (var grave in _graves)
            {
                grave.Update(_player.RectangleMovement);
            }
            _graves.RemoveAll(item => item._destroy == true);
            _boxes.RemoveAll(item => item._destroy == true);
            _chests.RemoveAll(item => item._destroy == true);
        }
    }
}
