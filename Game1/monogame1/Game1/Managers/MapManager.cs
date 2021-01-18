using System;
using System.Collections.Generic;
using System.Text;

namespace GameClient
{
    public class MapManager
    {
        public List<Grave> _graves;
        Player _player;
        public MapManager()
        {
            _graves = new List<Grave>();
        }
        public void Initialize(Player player)
        {
            _player = player;

        }
        public void Update()
        {
            Console.WriteLine();
            foreach (var grave in _graves)
            {
                grave.Update(_player.Position_Feet);
            }
            _graves.RemoveAll(grave => grave._destroy == true);
        }
    }
}
